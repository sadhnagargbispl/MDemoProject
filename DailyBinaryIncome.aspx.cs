using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;

public partial class DailyBinaryIncome : System.Web.UI.Page
{
    // DataTable
    DataTable dt;

    // DAL object
    DAL Obj;

    // General utility class
    clsGeneral objGen = new clsGeneral();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {

              
                Obj = new DAL();
                if (!Page.IsPostBack)
                {
                    FillData();
                }
            }
            else
            {
                Response.Redirect("logout.aspx");
                return;
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);

            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }
    }
    public void FillData()
    {
        string str = "";
        str = Obj.IsoStart + "Exec sp_GetDailyPayoutDetailDis '" + Session["Formno"] + "'" + Obj.IsoEnd;
        dt = new DataTable();
        dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];
        if (dt.Rows.Count > 0)
        {
            Session["PairincomeData1"] = dt;
            if (Session["CompID"].ToString() == "1107")
            {
                divev.Visible = true;
                DivMakeAndGro.Visible = false;
                DivMavitri.Visible = false;
                DivHemalikha.Visible = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

            }
            else if (Session["CompID"].ToString() == "1106")
            {
                divev.Visible = false;
                DivHemalikha.Visible = true;
                DivMakeAndGro.Visible = false;
                DivMavitri.Visible = false;
                GridView2.DataSource = dt;
                GridView2.DataBind();

            }
            else if (Session["CompID"].ToString() == "1108")
            {
                Dv9.Visible = true;
                divev.Visible = false;
                DivHemalikha.Visible = false;
                DivMakeAndGro.Visible = false;
                DivMavitri.Visible = false;
                GridView3.DataSource = dt;
                GridView3.DataBind();
            }
            else if (Session["CompID"].ToString() == "1110")
            {
                DivMakeAndGro.Visible = false;
                DivMavitri.Visible = true;
                Dv9.Visible = false;
                divev.Visible = false;
                DivHemalikha.Visible = false;
                GridView5.DataSource = dt;
                GridView5.DataBind();
            }
            else if (Session["CompID"].ToString() == "1102")
            {
                DivMakeAndGro.Visible = true;
                DivMavitri.Visible = false;
                Dv9.Visible = false;
                divev.Visible = false;
                DivHemalikha.Visible = false;
                GridView4.DataSource = dt;
                GridView4.DataBind();
            }
            else
            {
                divev.Visible = false;
                DivMakeAndGro.Visible = false;
                DivMavitri.Visible = false;
                DivHemalikha.Visible = false;
                Dv9.Visible = false;
                gv.DataSource = dt;
                gv.DataBind();

            }
        }
        else
        {
            DataRow row = dt.NewRow();
            row["Sno"] = 0;
            dt.Rows.Add(row);
            if (Session["CompID"].ToString() == "1107")
            {
                divev.Visible = true;
                Dv9.Visible = false;
                DivHemalikha.Visible = false;
                DivMakeAndGro.Visible = false;
                DivMavitri.Visible = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();
                //GridView1.Columns[6].Visible = false;
            }
            else if (Session["CompID"].ToString() == "1106")
            {
                divev.Visible = false;
                Dv9.Visible = false;
                DivHemalikha.Visible = true;
                DivMakeAndGro.Visible = false;
                DivMavitri.Visible = false;
                GridView2.DataSource = dt;
                GridView2.DataBind();
                //GridView1.Columns[6].Visible = false;
            }
            else if (Session["CompID"].ToString() == "1108")
            {
                Dv9.Visible = true;
                divev.Visible = false;
                DivMakeAndGro.Visible = false;
                DivMavitri.Visible = false;
                DivHemalikha.Visible = false;
                GridView3.DataSource = dt;
                GridView3.DataBind();
            }
            else if (Session["CompID"].ToString() == "1110")
            {
                DivMakeAndGro.Visible = false;
                DivMavitri.Visible = true;
                Dv9.Visible = false;
                divev.Visible = false;
                DivHemalikha.Visible = false;
                GridView5.DataSource = dt;
                GridView5.DataBind();
            }
            else if (Session["CompID"].ToString() == "1102")
            {
                DivMakeAndGro.Visible = true;
                DivMavitri.Visible = false;
                Dv9.Visible = false;
                divev.Visible = false;
                DivHemalikha.Visible = false;
                GridView4.DataSource = dt;
                GridView4.DataBind();
            }
            else
            {
                divev.Visible = false;
                DivHemalikha.Visible = false;
                Dv9.Visible = false;
                DivMakeAndGro.Visible = false;
                DivMavitri.Visible = false;
                gv.DataSource = dt;
                gv.DataBind();

            }

        }
    }
    protected void gv_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            gv.PageIndex = e.NewPageIndex;
            FillData();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
        }
    }

    protected void GridView1_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            GridView1.PageIndex = e.NewPageIndex;
            FillData();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
        }
    }
    protected void GridView2_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            GridView2.PageIndex = e.NewPageIndex;
            FillData();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
        }
    }
    protected void GridView3_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            GridView3.PageIndex = e.NewPageIndex;
            FillData();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
        }
    }
    protected void GridView4_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            GridView4.PageIndex = e.NewPageIndex;
            FillData();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
        }
    }
    protected void GridView5_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            GridView5.PageIndex = e.NewPageIndex;
            FillData();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
        }
    }
   
}
