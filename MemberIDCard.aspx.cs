using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
public partial class MemberIDCard : System.Web.UI.Page
{
    string strquery;
    DAL objDal;

    DAL Obj;
    DataTable dt;
    string str = "";
    clsGeneral objGen = new clsGeneral();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            

            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                Obj = new DAL();
                if (!IsPostBack)
                {
                    GetMemberIDCrad();
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
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);

            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }
    }
    private void GetMemberIDCrad()
    {
        try
        {
            string FormNo = Session["FormNo"]?.ToString();
            string idNo = Session["IDNo"]?.ToString();
            if (string.IsNullOrEmpty(idNo))
            {
                Response.Write("ID not found in session.");
                return;
            }


            string strquery = Obj.IsoStart + "EXEC SP_GetMemberCardData '" + FormNo + "'" + Obj.IsoEnd;
            DataTable dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, strquery).Tables[0];

            if (dt.Rows.Count > 0)
            {
                var member = dt.Rows[0];

                lblName.Text = member["Name"]?.ToString() ?? "";
                lblIDNo.Text = member["MemberID"]?.ToString() ?? "";
                lblMobile.Text = member["MobileNo"]?.ToString() ?? "";
                lblValid.Text = member["DOJ"] != DBNull.Value ? Convert.ToDateTime(member["DOJ"]).ToString("dd MMM yyyy") : "";
                lblCity.Text = member["City"]?.ToString() ?? "";
                lblState.Text = member["StateName"]?.ToString() ?? "";
                lblPan.Text = member["panno"]?.ToString() ?? "";
                lblRank.Text = member["Rank"]?.ToString() ?? "";
                Image2.ImageUrl = member["ProfilePic"]?.ToString() ?? "";
            }
            else
            {
                Response.Write("Member not found.");
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }



}