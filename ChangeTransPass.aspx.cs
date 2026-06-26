using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ChangeTransPass : System.Web.UI.Page
{
    private cls_DataAccess dbConnect;
    private clsGeneral dbGeneral = new clsGeneral();
    string scrname;
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dRead;
    // AccClass.MyAccClass.NewClass QryCls = new AccClass.MyAccClass.NewClass();
    // DAL object (same as your commented VB line)
    DAL Obj;
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
                    hdnSessn.Value = Crypto.Encrypt(Session["IDNo"].ToString());
                }

                // BtnUpdate.Attributes.Add("OnClick", "return ValidForm();");
            }
            else
            {
                Response.Redirect("logout.aspx");
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            return;
        }
    }
    private void UpdatePass()
    {
        try
        {
            if (Session["Sessncheck"].ToString().ToUpper() !=
                ("OK" + Crypto.Decrypt(hdnSessn.Value).ToString().ToUpper()))
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "Key",
                    "alert('Session expire, Please Re-Login.!!');location.replace('logout.aspx');",
                    true
                );
                return;
            }

            string strQry;
            DataTable tmptbl = new DataTable();

            try
            {
                // -------------------------------
                // Blank Field Validations
                // -------------------------------
                if (oldpass.Text == "")
                {
                    scrname = "<SCRIPT>alert('Old password can not be left blank');</SCRIPT>";
                    this.RegisterStartupScript("MyAlert", scrname);
                    oldpass.Focus();
                    return;
                }
                else if (pass1.Text == "")
                {
                    scrname = "<SCRIPT>alert('New Password can not be left blank');</SCRIPT>";
                    this.RegisterStartupScript("MyAlert", scrname);
                    pass1.Focus();
                    return;
                }
                else if (pass2.Text == "")
                {
                    scrname = "<SCRIPT>alert('Confirm Password can not be left blank');</SCRIPT>";
                    this.RegisterStartupScript("MyAlert", scrname);
                    pass2.Focus();
                    return;
                }
                else if (oldpass.Text == "" || pass1.Text == "" || pass2.Text == "")
                {
                    scrname = "<SCRIPT>alert('Field can not be left blank');</SCRIPT>";
                    this.RegisterStartupScript("MyAlert", scrname);
                    oldpass.Focus();
                    return;
                }

                // --------------------------------
                // Match Check
                // --------------------------------
                if (pass1.Text != pass2.Text)
                {
                    scrname = "<SCRIPT>alert('Password and Confirm password not match');</SCRIPT>";
                    this.RegisterStartupScript("MyAlert", scrname);

                    pass1.Text = "";
                    pass2.Text = "";
                    oldpass.Focus();
                    return;
                }

                // --------------------------------
                // Verify Old Password
                // --------------------------------
                string str =
                   Obj.IsoStart + "Select IdNo from " + Obj.dBName + "..M_MemberMaster " +
                    " Where FormNo='" + Session["Formno"] + "' and " +
                    " epassw ='" + oldpass.Text.Trim() + "' " + Obj.IsoEnd;

                tmptbl = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];

                if (tmptbl.Rows.Count == 1)
                {
                    // --------------------------------
                    // Update New Password
                    // --------------------------------
                    strQry =
                        "Update M_MemberMaster Set EPassw='" + pass1.Text.Trim() +  "' Where FormNo=" + Session["FormNo"] + ";";

                    int i = Obj.SaveData(strQry);

                    if (i != 0)
                    {
                        scrname =
                            "<SCRIPT>alert('Password Changed Successfully, Login Again!!');</SCRIPT>";
                        this.RegisterStartupScript("MyAlert", scrname);
                    }
                }
                else
                {
                    scrname = "<SCRIPT>alert('Check Old Password');</SCRIPT>";
                    this.RegisterStartupScript("MyAlert", scrname);
                    oldpass.Text = "";
                    oldpass.Focus();
                    return;
                }
            }
            catch (Exception e)
            {
                string path = HttpContext.Current.Request.Url.AbsoluteUri;
                string text =
                    path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                    Environment.NewLine;

                Obj.WriteToFile(text + e.Message);
                Response.Write("Try later.");

                scrname = "<SCRIPT>alert('" + e.Message + "');</SCRIPT>";
                this.RegisterStartupScript("MyAlert", scrname);

                return;
            }
        }
        catch (Exception e)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text =
                path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                Environment.NewLine;

            Obj.WriteToFile(text + e.Message);
            Response.Write("Try later.");
            return;
        }
    }
    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(2000);
        UpdatePass();
    }
}