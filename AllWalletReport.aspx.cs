using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

public partial class AllWalletReport : System.Web.UI.Page
{
    DataTable Dt;
    DAL Obj;
    clsGeneral objGen = new clsGeneral();

    string query = "";

    // PAGE SIZE
    int PageSize = 10;

    // PAGE INDEX
    public int PageIndex
    {
        get { return ViewState["PageIndex"] != null ? Convert.ToInt32(ViewState["PageIndex"]) : 0; }
        set { ViewState["PageIndex"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {


            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                Obj = new DAL();
                if (!IsPostBack)
                {
                    PageIndex = 0;
                    FillWallettype();
                    FillBalance();
                    FillDetail();
                }
            }
            else
            {
                Response.Redirect("logout.aspx");
            }
           
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    }

    // ------------------ WALLET TYPE -------------------------
    private void FillWallettype()
    {
        try
        {
            DataTable dt = new DataTable();

            if (Session["compid"].ToString() == "1077")
            {
                query = Obj.IsoStart + "exec FillWallettype" + Obj.IsoEnd;
            }
            else
            {
                query = Obj.IsoStart + "Select Actype,WalletName from " + Obj.dBName + "..VoucherType Where ActiveStatus='Y' Order by AcID" + Obj.IsoEnd;
            }

            dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, query).Tables[0];

            if (dt.Rows.Count > 0)
            {
                ddlVoucherType.DataSource = dt;
                ddlVoucherType.DataTextField = "WalletName";
                ddlVoucherType.DataValueField = "Actype";
                ddlVoucherType.DataBind();
            }
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    }

    // ------------------ BALANCE -------------------------
    private void FillBalance()
    {
        try
        {
            lblHeading.Text = ddlVoucherType.SelectedItem.Text + " Status ";

            query = Obj.IsoStart + "Select * From dbo.ufnGetBalance('" +
                     Session["FormNo"] + "','" +
                     ddlVoucherType.SelectedValue + "')" + Obj.IsoEnd;

            Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, query).Tables[0];

            if (Dt.Rows.Count > 0)
            {
                MCredit.InnerText = Dt.Rows[0]["Credit"].ToString();
                MDebit.InnerText = Dt.Rows[0]["Debit"].ToString();
                MBal.InnerText = Dt.Rows[0]["Balance"].ToString();
            }
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    }

    // ------------------ MAIN DETAIL (FULL DATA) -------------------------
    private void FillDetail()
    {
        try
        {
            query = Obj.IsoStart + " Exec Sp_WalletrReport '" +
                    ddlVoucherType.SelectedValue + "','" +
                    Session["FormNo"] + "',1,100000" + Obj.IsoEnd;

            Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, query).Tables[0];

            Session["MainFund"] = Dt;

            lbltotal.Text = Dt.Rows.Count.ToString();

            BindPagedData();
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    }

    // ------------------ PAGINATION ONLY C# -------------------------
    private void BindPagedData()
    {
        try
        {
            DataTable fullDt = (DataTable)Session["MainFund"];

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

    // ------------------ PREV BUTTON -------------------------
    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        if (PageIndex > 0)
        {
            PageIndex--;
            BindPagedData();
        }
    }

    // ------------------ NEXT BUTTON -------------------------
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

    // ------------------ SEARCH BUTTON -------------------------
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            PageIndex = 0;
            FillBalance();
            FillDetail();
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    }

    // ------------------ ERROR LOG -------------------------
    private void LogError(Exception ex)
    {
        string path = HttpContext.Current.Request.Url.AbsoluteUri;
        string text = path + ": " +
                      DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                      Environment.NewLine;

        objGen.WriteToFile(text + ex.Message);
    }
}
