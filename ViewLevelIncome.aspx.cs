using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web;

public partial class ViewLevelIncome : System.Web.UI.Page
{
    DataTable dtData = new DataTable();
    DAL objDAL;
    ModuleFunction objModuleFun;
    string ReqNo;
    DataSet Ds;
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["Status"] != null && Session["Status"].ToString() == "OK")
        {
            // valid session
        }
        else
        {
            Response.Redirect("Logout.aspx");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string scrname;
        if (!Page.IsPostBack)
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                objDAL = new DAL();
                objModuleFun = new ModuleFunction(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
                if (!string.IsNullOrEmpty(Request["Sessid"]))
                {
                    BindData();
                }
            }
            else
            {
                scrname = "<SCRIPT language='javascript'> window.top.location.reload();</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(
                    this.Page,
                    this.GetType(),
                    "Close",
                    scrname,
                    false
                );
            }
        }
    }

    public void BindData(string SrchCond = "")
    {
        try
        {
            string cond = "";
            string formno = "";
            string sql1 = "";
        
            if (Session["CompID"].ToString() == "1110")
           
            {
                 sql1 = objDAL.IsoStart + " Exec SP_LevelIncomeReportmavitr '" + Session["formno"] + "', '" + Request["Sessid"] + "' " + objDAL.IsoEnd;
            }
            else
            {
                 sql1 = objDAL.IsoStart + " Exec SP_LevelIncomeReport '" + Session["formno"] + "', '" + Request["Sessid"] + "' " + objDAL.IsoEnd;
            }
            //string sql1 = objDAL.IsoStart + " Exec SP_LevelIncomeReport '" + Session["formno"] + "', '" + Request["Sessid"] + "' " + objDAL.IsoEnd;
            dtData = new DataTable();
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql1);
            GvData.DataSource = Ds.Tables[0];
            GvData.DataBind();
            Session["GData"] = Ds.Tables[0];
            lbltotal.Text = " : " + Ds.Tables[1].Rows[0]["Income"].ToString();
        }
        catch (Exception ex)
        {
            // optional: log error
        }
    }

    protected void GvData_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        GvData.PageIndex = e.NewPageIndex;
        GvData.DataSource = Session["GData"];
        GvData.DataBind();
    }
}
