using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections;
public partial class ProductRequestDetail : System.Web.UI.Page
{
    string strquery;
    DataTable dt;
    clsGeneral objGen = new clsGeneral();

    DAL Obj;
    int PageSize = 10;

    public int PageIndex
    {
        get { return ViewState["PageIndex"] != null ? Convert.ToInt32(ViewState["PageIndex"]) : 0; }
        set { ViewState["PageIndex"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["Status"] != null && Session["Status"].ToString() == "OK")
        {
            Obj = new DAL();
            if (!Page.IsPostBack)
            {
                // Fillkit();
                PaymentDetails();
            }
        }
        else
        {
            Response.Redirect("Logout.aspx");
            Response.End();
        }
    }
    private void PaymentDetails()
    {
        try
        {
            DataTable dt = new DataTable();
            string col = "";
            string col2 = "";
            string col9 = "";

            DAL obj = new DAL();

            string Condition = "";
            strquery = Obj.IsoStart + "Exec Sp_MyPurchaseDetail '" + Convert.ToInt32(Session["Formno"]) + "'" + Obj.IsoEnd;
            dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, strquery).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                lbltotal.Visible = true;
                lbltotal.Text = dt.Rows.Count.ToString();
            }
            else
            {
                lbltotal.Text = "";
                lbltotal.Visible = false;
            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //col2 = "<a href=\"#\" " + "style=\"color:blue\">" + dr["OrderNo"] + "</a>";
                    //dr["Orderno"] = col2;
                }
            }

            RptDirects.DataSource = dt;
            RptDirects.DataBind();
            RptDirects.Visible = true;
            customer1.Visible = true;
            Session["ReceivedPin"] = dt;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + "SideB");
        }
    }

    private static string Base64Encode(string plainText)
    {
        byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    private static string Base64Decode(string base64EncodedData)
    {
        byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
    private void BindPagedData()
    {
        try
        {
            DataTable fullDt = (DataTable)Session["ReceivedPin"];

            if (fullDt == null || fullDt.Rows.Count == 0)
                return;

            DataTable NewDT = fullDt.Clone();

            int start = PageIndex * PageSize;
            int end = start + PageSize;

            if (end > fullDt.Rows.Count)
                end = fullDt.Rows.Count;

            for (int i = start; i < end; i++)
            {
                NewDT.ImportRow(fullDt.Rows[i]);
            }

            RptDirects.DataSource = NewDT;
            RptDirects.DataBind();
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    }

    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        if (PageIndex > 0)
        {
            PageIndex--;
            BindPagedData();
        }
    }


    protected void lnkNext_Click(object sender, EventArgs e)
    {
        int total = Convert.ToInt32(lbltotal.Text);
        int maxPage = (total - 1) / PageSize;

        if (PageIndex < maxPage)
        {
            PageIndex++;
            BindPagedData();
        }
    }
    private void LogError(Exception ex)
    {
        string path = HttpContext.Current.Request.Url.AbsoluteUri;
        string text = path + ": " +
                      DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
        Environment.NewLine;

        objGen.WriteToFile(text + ex.Message);
    }
}