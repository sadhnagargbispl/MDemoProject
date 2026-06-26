using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GroupbusinessDate : System.Web.UI.Page
{
    string strquery;
    DataTable dt;
    clsGeneral objGen = new clsGeneral();
    DAL Obj;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Status"] != null && Session["Status"].ToString() == "OK")
        {
            if (!IsPostBack)
            {
                Filltotal(Session["Formno"].ToString(), Session["Formno"].ToString());
                LoadDownline(Session["FormNo"].ToString());
            }
        }
        else
        {
            Response.Redirect("Logout.aspx");
            Response.End();
        }
    }
    private void Filltotal(string Formno, string dwnFormno)
    {
        try
        {
            DataTable Dt = new DataTable();
            string strSql = "Exec Sp_MydirectReportDate '" + Formno + "','" + dwnFormno + "','" + TxtFromDate.Text + "','" + TxtToDate.Text + "'";
            Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),CommandType.Text, strSql).Tables[0];
            Session["DirectDownline"] = Dt;
            Grdtotal.DataSource = Dt;
            Grdtotal.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private void LoadDownline(string FormNo)
    {
        try
        {
            DataTable Dt1 = new DataTable();
            string strSql = "Exec Sp_MydirectReportNewDate '" + FormNo + "','" + TxtFromDate.Text + "','" + TxtToDate.Text + "'";
            Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),CommandType.Text, strSql).Tables[0];
            Session["DirectDownline"] = Dt1;
            DLDirects.DataSource = Dt1;
            DLDirects.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage",
                "alert('" + ex.Message + "')", true);
        }
    }

    protected void PerformData(object sender, EventArgs e)
    {
        try
        {
            RepeaterItem Di = (RepeaterItem)((ImageButton)sender).NamingContainer;
            string FormNo = ((Label)Di.FindControl("lblID")).Text;

            Filltotal(Session["formno"].ToString(), FormNo);
            LoadDownline(FormNo);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage",
                "alert('" + ex.Message + "')", true);
        }
    }

    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            Session["Data"] = Session["Formno"];
            Session["Data"] = Session["dwnFormno"]; // Add on 30Oct2018

            Filltotal(Session["Formno"].ToString(), Session["Formno"].ToString());
            //Filltotal(Session["Formno"].ToString(), Session["dwnFormno"].ToString());
            LoadDownline(Session["FormNo"].ToString());
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage",
                "alert('" + ex.Message + "')", true);
        }
    }
}