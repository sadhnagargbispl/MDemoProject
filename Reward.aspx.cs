using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;


public partial class Reward : System.Web.UI.Page
{
    DataTable dt;

    // DAL object
    DAL Obj;

    // General utility class
    clsGeneral objGen = new clsGeneral();
    SqlConnection Conn;
    SqlCommand Comm;
    DataTable Dt;
    SqlDataAdapter Ad;

    string str = "";

  
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

                    str = "Exec Sp_GetReward '" + Session["Formno"] + "'";

                    dt = new DataTable();
                    dt = Obj.GetData(str);
                    gv.DataSource = dt;
                    gv.DataBind();
                    Session["Reward"] = dt;
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

   
}