using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

public partial class DailyBinaryIncomeWithView : System.Web.UI.Page
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
                    string str = "";
                    str = Obj.IsoStart + "Exec sp_GetDailyPayoutDetailDis '" + Session["Formno"] + "'" + Obj.IsoEnd;
                    dt = new DataTable();
                    dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        gv.DataSource = dt;
                        gv.DataBind();
                        Session["PairincomeData1"] = dt;
                    }
                    else
                    {
                        DataRow row = dt.NewRow();
                        row["Sno"] = 0;
                        dt.Rows.Add(row);

                        gv.DataSource = dt;
                        gv.DataBind();
                    }
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

    protected void gv_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {
        try
        {
            gv.PageIndex = e.NewPageIndex;
            gv.DataSource = Session["PairincomeData1"];
            gv.DataBind();
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
