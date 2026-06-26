using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Rptwithdrawls : System.Web.UI.Page
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
                try
                {
                    if (!Page.IsPostBack)
                    {
                        FillDownline();
                    }
                }
                catch (Exception)
                {
                    // inner catch intentionally left empty (same as VB code)
                }
            }
            else
            {
                Response.Redirect("Logout.aspx");
                Response.End();
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);

            Response.Write("Try later.");
            Response.Write(ex.Message);

            Response.End();
        }
    }
    private void FillDownline()
    {
        try
        {
            string str = string.Empty;
            str += Obj.IsoStart + " Select ReqID,ReqAmount,Convert(Varchar,ReqDate,106) As ReqDate,";
            str += " IsNULL(Convert(Varchar,IssueDate,106),'') As IssueDate,";
            str += " Case When Status='P' Then 'PENDING' ";
            str += " Else Case When Status='A' Then 'APPROVED' Else 'REJECTED' End End As Status,";
            str += " TdsAmount,AdminCharge,NetAmount,A.PayeeName,B.BankName,A.AcNo,A.IFSCode,A.PanNo,A.BranchName";
            str += " From " + Obj.dBName + "..Fundwithdrawls As A," + Obj.dBName + "..M_BankMaster As B ";
            str += " Where A.BankID=B.BankCode ";
            str += " And FormNo=" + Convert.ToInt32(Session["FormNo"]);
            str += " Order By Sno Desc" + Obj.IsoEnd;

            DataTable dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];

            Session["WithdrawData"] = dt;

            if (dt.Rows.Count > 0)
            {
                GrdDirects.DataSource = dt;
                GrdDirects.DataBind();
                lbltotal.Text = dt.Rows.Count.ToString();
            }
        }
        catch (Exception ex)
        {

        }
    }
    //private void PaymentDetails()
    //{
    //    try
    //    {
    //        string strquery = "";
    //        DataTable dt = new DataTable();

    //        if (Session["compid"] != null && Session["compid"].ToString() == "1007")
    //        {
    //            strquery = Obj.IsoStart + "exec Sp_GetWalletDetail '" + Session["Formno"] + "'" + Obj.IsoEnd;
    //        }
    //        else
    //        {
    //            strquery = Obj.IsoStart + " select ReqNo, Replace(Convert(Varchar, a.RecTimeStamp, 106), ' ', '-') + ' ' + ";
    //            strquery += " CONVERT(varchar(15), CAST(a.RecTimeStamp AS TIME), 100) as ReqDate, ";
    //            strquery += " PayMode, Chqno, Replace(Convert(Varchar, ChqDate, 106), ' ', '-') as ChequeDate, ";
    //            strquery += " b.BankName, a.Branchname, ";
    //            strquery += " Case when IsApprove='N' then 'Pending' when IsApprove='Y' then 'Approved' else 'Rejected' end as status, ";
    //            strquery += " Amount, a.Remarks, ";
    //            strquery += " Case when ScannedFile='' then '' when ScannedFile like 'http%' then ScannedFile ";
    //            strquery += " else 'images/UploadImage/'+'" + Session["compid"] + "/' + ScannedFile end as ScannedFile, ";
    //            strquery += " Case when ScannedFile='' then 'False' else 'True' end as ScannedFileStatus, ";
    //            strquery += " ApproveRemark ";
    //            strquery += " from " + Obj.dBName + "..WalletReq as a, " + Obj.dBName + "..M_BankMaster as b ";
    //            strquery += " where a.BankId = b.BankCode and b.RowStatus='Y' ";
    //            strquery += " and a.Formno='" + Session["Formno"] + "' ";
    //            strquery += " order by ReqNo desc " + Obj.IsoEnd;
    //        }

    //        dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, strquery).Tables[0];

    //        if (dt.Rows.Count > 0)
    //        {
    //            Session["ReceivedPin"] = dt;
    //            RptDirects.DataSource = dt;
    //            RptDirects.DataBind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string path = HttpContext.Current.Request.Url.AbsoluteUri;
    //        string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;

    //        Obj.WriteToFile(text + ex.Message);
    //        Response.Write("Try later.");
    //    }
    //}
    private void BindPagedData()
    {
        try
        {
            DataTable fullDt = (DataTable)Session["WithdrawData"];

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

            GrdDirects.DataSource = NewDT;
            GrdDirects.DataBind();
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
    private void LogError(Exception ex)
    {
        string path = HttpContext.Current.Request.Url.AbsoluteUri;
        string text = path + ": " +
                      DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
        Environment.NewLine;

        objGen.WriteToFile(text + ex.Message);
    }
}