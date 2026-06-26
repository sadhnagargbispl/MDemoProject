using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;

public partial class Invoice : Page
{

    // ================= MODEL =================

    public class InvoiceItem
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal GstAmount { get; set; }
        public decimal GstRate { get; set; }   // ✅ ADDED: GST % (e.g. 5)
        public decimal Discount { get; set; }
    }

    public class TaxLine
    {
        public string ProductName { get; set; }
        public string Description { get; set; }

        public int Qty { get; set; }
        public decimal Rate { get; set; }

        public decimal Discount { get; set; }
        public decimal Taxable { get; set; }

        public bool IsIntraState { get; set; }

        public decimal CgstPct { get; set; }
        public decimal CgstAmt { get; set; }

        public decimal SgstPct { get; set; }
        public decimal SgstAmt { get; set; }

        public decimal IgstPct { get; set; }
        public decimal IgstAmt { get; set; }

        public decimal LineTotal { get; set; }
    }


    // ================= GLOBAL STATE =================

    private bool _isIntraState;

    private decimal _totalTaxable;
    private decimal _totalCgst;
    private decimal _totalSgst;
    private decimal _totalIgst;
    private decimal _grandTotal;


    // ================= GET COMP ID =================

    private string GetCompID()
    {
        string url = string.Empty;
        SqlConnection conn = null;

        try
        {
            url = HttpContext.Current.Request.Url.Host
                .ToUpper()
                .Replace("HTTP://", "")
                .Replace("HTTPS://", "")
                .Replace("WWW.", "")
                .Replace("BASICMLM.", "")
                .Replace("CPANEL.", "")
                .Replace("LOGIN.", "")
                .Replace("CONSULTANT.", "")
                .Replace("NETWORK.", "");

            string str = string.Empty;

            if (url == "LOCALHOST")
            {
                str = "Select ID,Logo,PartyCode,Name,URL,gvPortalCompID,UtiLityPortalID from M_CompanyMasterNew " +
                      "Where IsActive = 1 And ID='" + ConfigurationManager.AppSettings["CompanyID"] + "'";
            }
            else
            {
                str = "Select ID,Logo,PartyCode,Name,URL,gvPortalCompID,UtiLityPortalID from M_CompanyMasterNew " +
                      "Where IsActive = 1 And (Upper(URL) = '" + url.ToUpper().Trim() + "')";
            }

            SqlDataReader dRead;
            SqlCommand cmd;
            conn = new SqlConnection(Application["sConnect"].ToString());
            conn.Open();
            cmd = new SqlCommand(str, conn);
            dRead = cmd.ExecuteReader();

            if (dRead.Read())
            {
                HttpContext.Current.Session["CompID"] = dRead["ID"].ToString();
                HttpContext.Current.Session["Logo"] = dRead["Logo"].ToString();
                imgLogo.Src = dRead["Logo"].ToString();
                HttpContext.Current.Session["WRPartyCode"] = dRead["PartyCode"].ToString();
                HttpContext.Current.Session["CompName"] = dRead["Name"].ToString();
                HttpContext.Current.Session["Title"] = "Welcome To " + dRead["Name"].ToString();
                HttpContext.Current.Session["gvPortalCompID"] = dRead["gvPortalCompID"].ToString();
                HttpContext.Current.Session["UtiLityPortalID"] = dRead["UtiLityPortalID"].ToString();
            }

            dRead.Close();
            conn.Close();
        }
        catch (Exception)
        {
            if (conn != null && conn.State == ConnectionState.Open)
                conn.Close();
        }

        return url;
    }


    // ================= PAGE LOAD =================

    protected void Page_Load(object sender, EventArgs e)
    {
        BtnBack.Attributes.Add("onclick", DisableTheButton(this.Page, BtnBack));

        if (Session["Status"] != null && Session["Status"].ToString() == "OK")
        {
            GetCompID();
            if (!IsPostBack)
                LoadInvoice();
            if (Session["compid"].ToString() == "1105")
            {
                GSTIN.Text = "08AAQCP5426J1ZW";
            }
        }
        else
        {
            Response.Redirect("logout.aspx");
            return;
        }
    }

    private string DisableTheButton(Control pge, Control btn)
    {
        var sb = new StringBuilder();
        sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
        sb.Append("this.value = 'Please wait...';");
        sb.Append("this.disabled = true;");
        sb.Append(pge.Page.GetPostBackEventReference(btn));
        sb.Append(";");
        return sb.ToString();
    }


    // ================= LOAD INVOICE =================

    private void LoadInvoice()
    {
        string encodedOrderNo = Request.QueryString["OrderNo"] ?? "";
        string billNo = "";
        try
        {
            byte[] data = Convert.FromBase64String(encodedOrderNo);
            billNo = System.Text.Encoding.UTF8.GetString(data);
        }
        catch
        {
            billNo = "0"; // fallback
        }
        string compState = Session["CompState"] != null ? Session["CompState"].ToString().Trim() : "";

        using (var con = new SqlConnection((string)Session["MlmDatabase" + Session["CompID"]]))
        {
            con.Open();

            LoadHeader(con, billNo, compState);

            var items = LoadItems(con, billNo);
            var taxLines = ComputeTaxLines(items);

            RenderGstBadge();
            RenderTableHead();
            RenderTableBody(taxLines);
            RenderMobileCards(taxLines);
            RenderTotals();
        }
    }


    // ================= HEADER =================

    private void LoadHeader(SqlConnection con, string billNo, string compState)
    {
        //        string sql = @"
        //SELECT
        //    b.rid as Id,
        //    b.BillNo,
        //    b.BillDate,
        //    mobl                AS Mobile,
        //    m.idno              AS MemberID,
        //    m.memfirstname      AS MemberName,
        //    m.Statecode         AS CustStateCode,
        //    c.Statename         AS CustStateName,
        //    CONCAT_WS(', ',
        //        NULLIF(m.Address1, ''),
        //        NULLIF(m.City,     ''),
        //        NULLIF(m.District, ''),
        //        NULLIF(c.Statename,''),
        //        NULLIF(m.PinCode,  '')
        //    ) AS CustFullAddress
        //FROM repurchincome b
        //JOIN m_membermaster  m ON m.formno   = b.formno
        //JOIN M_StateDivMaster c ON c.statecode = m.Statecode
        //WHERE b.BillNo = @BillNo";
        string sql = @"
SELECT 
    b.rid AS Id,
    b.BillNo,
    b.BillDate,
    m.mobl AS Mobile,
    c.id AS ReqID,
    m.idno AS MemberID,
    m.memfirstname AS MemberName,
    Statecodes AS CustStateCode,
    CASE 
        WHEN statename = '--Choose State Name--' THEN '' 
        ELSE statename 
    END AS CustStateName,
    
    CONCAT_WS(', ',
        NULLIF(UserAddress,''),
        NULLIF(c.City,''),
        NULLIF(c.District,''),
        NULLIF(
            CASE 
                WHEN statename = '--Choose State Name--' THEN '' 
                ELSE statename 
            END,''
        ),
        NULLIF(c.PinCode,'')
    ) AS CustFullAddress

FROM
(
    SELECT *,
           ROW_NUMBER() OVER(PARTITION BY formno, kitid ORDER BY billdate DESC) rn
    FROM repurchincome
) b

JOIN
(
    SELECT *,
           ROW_NUMBER() OVER(PARTITION BY formno, kitid ORDER BY reqdate DESC) rn
    FROM purchaseReq
) c
    ON b.formno = c.formno
    AND b.kitid = c.kitid
    AND b.rn = c.rn

JOIN m_membermaster m
    ON m.formno = b.formno

WHERE b.BillNo = @BillNo";
        using (var cmd = new SqlCommand(sql, con))
        {
            cmd.Parameters.AddWithValue("@BillNo", billNo);

            using (var dr = cmd.ExecuteReader())
            {
                if (!dr.Read()) return;

                string custStateCode = dr["CustStateCode"].ToString().Trim();
                _isIntraState = compState.Equals(custStateCode, StringComparison.OrdinalIgnoreCase);

                LblBillno.Text      = dr["BillNo"].ToString();
                LblBillnumber.Text  = dr["ReqID"].ToString();
                LblBillDate.Text    = Convert.ToDateTime(dr["BillDate"]).ToString("dd MMM yyyy");
                LblMemberID.Text    = dr["MemberID"].ToString();
                LblMemberName.Text  = dr["MemberName"].ToString();
                LblName.Text        = dr["MemberName"].ToString();
                LblAddress.Text     = dr["CustFullAddress"].ToString();

                string mob = dr["Mobile"].ToString().Trim();
                LblMobile.Text = mob;
                LblMobile.Parent.Visible = !string.IsNullOrEmpty(mob);
            }
        }
    }


    // ================= LOAD ITEMS =================

    private List<InvoiceItem> LoadItems(SqlConnection con, string billNo)
    {
        var list = new List<InvoiceItem>();

        if (Session["compid"].ToString() == "1105")
        {
            // ✅ Hardcoded items with GstRate set
            list.Add(new InvoiceItem
            {
                ProductName = "Stem Life",
                Description = "",
                Qty         = 1,
                Rate        = 3333.33m,
                GstAmount   = 166.66m,
                GstRate     = 5,        // ✅ ADDED
                Discount    = 0
            });

            list.Add(new InvoiceItem
            {
                ProductName = "Cordy Berry Gold",
                Description = "",
                Qty         = 1,
                Rate        = 3333.33m,
                GstAmount   = 166.66m,
                GstRate     = 5,        // ✅ ADDED
                Discount    = 0
            });
        }
        else
        {
            // ✅ SQL query now includes GstRate column
            string sql = @"
        SELECT
            kitname                             AS ProductName,
            ''                                  AS Description,
            1                                   AS Qty,
            ROUND(kitamount / 1.05, 2)          AS Rate,
            ROUND((kitamount / 1.05) * 0.05, 2) AS GstAmount,
            5                                   AS GstRate,
            0                                   AS Discount
        FROM repurchincome A
        JOIN M_kitmaster B ON A.kitid = B.kitid
        WHERE BillNo = @BillNo";

            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@BillNo", billNo);

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new InvoiceItem
                        {
                            ProductName = dr["ProductName"].ToString(),
                            Description = dr["Description"].ToString(),
                            Qty         = Convert.ToInt32(dr["Qty"]),
                            Rate        = Convert.ToDecimal(dr["Rate"]),
                            GstAmount   = Convert.ToDecimal(dr["GstAmount"]),
                            GstRate     = Convert.ToDecimal(dr["GstRate"]),   // ✅ ADDED
                            Discount    = Convert.ToDecimal(dr["Discount"])
                        });
                    }
                }
            }
        }

        return list;
    }


    // ================= GST CALCULATION =================

    private List<TaxLine> ComputeTaxLines(List<InvoiceItem> items)
    {
        var lines = new List<TaxLine>();

        foreach (var item in items)
        {
            decimal taxable = item.Qty * item.Rate;

            var line = new TaxLine
            {
                ProductName  = item.ProductName,
                Description  = item.Description,
                Qty          = item.Qty,
                Rate         = item.Rate,
                Discount     = item.Discount,
                Taxable      = taxable,
                IsIntraState = _isIntraState
            };

            if (_isIntraState)
            {
                decimal halfRate = item.GstRate / 2;                     // ✅ e.g. 5/2 = 2.5
                decimal halfAmt  = Math.Round(item.GstAmount / 2, 2);

                line.CgstPct = halfRate;    // ✅ 2.5%
                line.CgstAmt = halfAmt;

                line.SgstPct = halfRate;    // ✅ 2.5%
                line.SgstAmt = halfAmt;

                line.LineTotal = taxable + item.GstAmount;

                _totalCgst += halfAmt;
                _totalSgst += halfAmt;
            }
            else
            {
                line.IgstPct = item.GstRate;    // ✅ 5%
                line.IgstAmt = item.GstAmount;

                line.LineTotal = taxable + item.GstAmount;

                _totalIgst += item.GstAmount;
            }

            _totalTaxable += taxable;

            lines.Add(line);
        }

        _grandTotal = Math.Round(
            _totalTaxable + _totalCgst + _totalSgst + _totalIgst,
            0,
            MidpointRounding.AwayFromZero
        );

        return lines;
    }


    // ================= GST BADGE =================

    private void RenderGstBadge()
    {
        LitGstBadge.Text = _isIntraState
            ? "<span class='gst-type-badge intra'>CGST + SGST</span>"
            : "<span class='gst-type-badge inter'>IGST</span>";
    }


    // ================= TABLE HEAD =================

    private void RenderTableHead()
    {
        var sb = new StringBuilder();
        sb.Append("<tr>");
        sb.Append("<th>Package</th>");
        sb.Append("<th>Qty</th>");
        sb.Append("<th>Rate</th>");
        sb.Append("<th>Taxable</th>");

        if (_isIntraState)
        {
            sb.Append("<th class='cgst-col'>CGST %</th>");
            sb.Append("<th class='cgst-col'>CGST</th>");
            sb.Append("<th class='sgst-col'>SGST %</th>");
            sb.Append("<th class='sgst-col'>SGST</th>");
        }
        else
        {
            sb.Append("<th class='igst-col'>IGST %</th>");
            sb.Append("<th class='igst-col'>IGST</th>");
        }

        sb.Append("<th>Total</th>");
        sb.Append("</tr>");

        LitTableHead.Text = sb.ToString();
    }


    // ================= TABLE BODY =================

    private void RenderTableBody(List<TaxLine> lines)
    {
        var sb = new StringBuilder();

        foreach (var l in lines)
        {
            sb.Append("<tr>");
            sb.Append($"<td>{l.ProductName}</td>");
            sb.Append($"<td>{l.Qty}</td>");
            sb.Append($"<td class='num'>{l.Rate:N2}</td>");
            sb.Append($"<td class='num'>{l.Taxable:N2}</td>");

            if (_isIntraState)
            {
                sb.Append($"<td class='cgst-col'>{l.CgstPct}%</td>");       // ✅ Shows 2.5%
                sb.Append($"<td class='num cgst-col'>{l.CgstAmt:N2}</td>");
                sb.Append($"<td class='sgst-col'>{l.SgstPct}%</td>");       // ✅ Shows 2.5%
                sb.Append($"<td class='num sgst-col'>{l.SgstAmt:N2}</td>");
            }
            else
            {
                sb.Append($"<td class='igst-col'>{l.IgstPct}%</td>");       // ✅ Shows 5%
                sb.Append($"<td class='num igst-col'>{l.IgstAmt:N2}</td>");
            }

            sb.Append($"<td>{Math.Round(l.LineTotal, 0, MidpointRounding.AwayFromZero):N0}</td>");
            sb.Append("</tr>");
        }

        LitTableBody.Text = sb.ToString();
    }


    // ================= MOBILE VIEW =================

    private void RenderMobileCards(List<TaxLine> lines)
    {
        var sb = new StringBuilder();

        foreach (var l in lines)
        {
            sb.Append("<div class='m-item'>");

            sb.Append("<div class='m-item-header'>");
            sb.Append($"<div class='m-item-name'>{l.ProductName}</div>");
            sb.Append($"<div class='m-item-total'>&#8377;{l.LineTotal:N2}</div>");
            sb.Append("</div>");

            sb.Append($"<div class='m-item-desc'>Qty: {l.Qty} &middot; Rate: &#8377;{l.Rate:N2}</div>");

            sb.Append("<div class='m-grid'>");
            sb.Append($"<div class='gc'><label>Taxable</label><span>{l.Taxable:N2}</span></div>");

            if (l.IsIntraState)
            {
                sb.Append($"<div class='gc'><label>GST Rate</label><span>{l.CgstPct + l.SgstPct}%</span></div>");
                sb.Append($"<div class='gc cgst'><label>CGST {l.CgstPct}%</label><span>{l.CgstAmt:N2}</span></div>");
                sb.Append($"<div class='gc sgst'><label>SGST {l.SgstPct}%</label><span>{l.SgstAmt:N2}</span></div>");
            }
            else
            {
                sb.Append($"<div class='gc'><label>GST Type</label><span>IGST</span></div>");
                sb.Append($"<div class='gc igst'><label>IGST {l.IgstPct}%</label><span>{l.IgstAmt:N2}</span></div>");
            }

            sb.Append("</div>"); // .m-grid
            sb.Append("</div>"); // .m-item
        }

        LitMobileCards.Text = sb.ToString();
    }


    // ================= TOTALS =================
    private void RenderTotals()
    {
        var sb = new StringBuilder();

        sb.Append(Row("Taxable Amount", _totalTaxable));

        if (_isIntraState)
        {
            sb.Append(Row("CGST", _totalCgst));
            sb.Append(Row("SGST", _totalSgst));
        }
        else
        {
            sb.Append(Row("IGST", _totalIgst));
        }

        // ✅ Actual sum before rounding
        decimal actualTotal = _totalTaxable + _totalCgst + _totalSgst + _totalIgst;
        decimal roundOff = _grandTotal - actualTotal;

        sb.Append(Row("Grand Total", actualTotal));

        // ✅ Round off row (only show if non-zero)
        if (roundOff != 0)
        {
            string sign = roundOff > 0 ? "+" : "";
            sb.Append($@"
        <div class='totals-row'>
            <span class='lbl'>Round Off</span>
            <span class='val'>{sign}{roundOff:N2}</span>
        </div>");
        }

        // ✅ Final TOTAL
        sb.Append($@"
    <div class='totals-row grand'>
        <span class='lbl'>TOTAL</span>
        <span class='val'>&#8377;{_grandTotal:N0}</span>
    </div>");

        LitTotals.Text = sb.ToString();
    }
    //private void RenderTotals()
    //{
    //    var sb = new StringBuilder();

    //    sb.Append(Row("Taxable Amount", _totalTaxable));

    //    if (_isIntraState)
    //    {
    //        sb.Append(Row("CGST", _totalCgst));
    //        sb.Append(Row("SGST", _totalSgst));
    //    }
    //    else
    //    {
    //        sb.Append(Row("IGST", _totalIgst));
    //    }

    //    sb.Append($@"
    //    <div class='totals-row grand'>
    //        <span class='lbl'>TOTAL</span>
    //        <span class='val'>&#8377;{_grandTotal:N0}</span>
    //    </div>");

    //    LitTotals.Text = sb.ToString();
    //}


    // ================= HELPER =================

    private string Row(string label, decimal value)
    {
        return $@"
    <div class='totals-row'>
        <span class='lbl'>{label}</span>
        <span class='val'>{value:N2}</span>
    </div>";
    }


    // ================= BACK BUTTON =================

    protected void BtnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProductRequestDetail.aspx");
    }
}
