using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reftree : System.Web.UI.Page
{
    SqlCommand Comm;
    SqlConnection Cnn;
    DAL obj = new DAL();
    string IsoStart;
    string IsoEnd;

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                Cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["constr1"].ConnectionString);
                string scrname = "";
                string DownFormNo = Get_FormNo(DownLineFormNo.Value);

                if (!string.IsNullOrEmpty(DownFormNo))
                {
                    TreeFrame.Attributes["src"] = "ReferalTree.aspx?DownLineFormNo=" + DownFormNo;
                }
                else
                {
                    scrname = "<SCRIPT language='javascript'>alert('Invalid distributor id');</SCRIPT>";
                    ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);
                }
            }
            else
            {
                Response.Redirect("logout.aspx");
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"].ToString() != "OK")
            {
                Response.Redirect("logout.aspx");
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }

    protected void cmdBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Dashboard.aspx");
    }

    private string Get_FormNo(string IDNo)
    {
        try
        {
            string str = "";
            string FormNo = "";
            DataTable dt = new DataTable();

            str = IsoStart + "Select FormNo From " + obj.dBName + "..M_MemberMaster Where IDNo='" + IDNo + "'" + IsoEnd;
            dt = SqlHelper.ExecuteDataset(Cnn, CommandType.Text, str).Tables[0];

            if (dt.Rows.Count > 0)
            {
                FormNo = dt.Rows[0]["FormNo"].ToString();
            }

            return FormNo;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            return null;
        }
    }

}