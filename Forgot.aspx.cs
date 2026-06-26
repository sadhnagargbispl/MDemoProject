using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Remoting;

public partial class Forgot : System.Web.UI.Page
{
    DAL obj;
    private clsGeneral objGen = new clsGeneral();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.Submit.Attributes.Add("onclick", DisableTheButton(this.Page, this.Submit));
            if (!Page.IsPostBack)
            {
                // CheckLiveRateCoin();
                // Main();

                GetCompID();
                objGen.GetConnectionByComp();
                objGen.GetInvDataBaseByComp();
                GetSmsTemplate();
                obj = new DAL();
                getData();
                try
                {
                    string str = @"
                CREATE PROC Sp_MemberForgotPassw
                (
                    @IDNo NVARCHAR(50)
                )
                AS
                BEGIN
                    SELECT 
                        (a.MemFirstName + ' ' + a.MemLastname) AS MemName,
                        a.Idno,
                        a.Passw,
                        a.EPassw,
                        a.mobl,
                        b.smsUsernm,
                        b.smsSenderID,
                        b.SmPass
                    FROM m_membermaster AS a,
                         m_companymaster AS b
                    WHERE a.IDNo = @IDNo
                END";

                    int i = 0;
                    i = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, str));
                }
                catch (Exception ex)
                {
                    // optional: log exception
                }
            }
        }
        catch (Exception ex)
        {
            // optional: log exception
        }
    }
    private void getData()
    {
        try
        {
            var dbConnect = new cls_DataAccess((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]]);
            dbConnect.OpenConnection();
            var dbConnectselect = new cls_DataAccess((string)HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]]);
            dbConnectselect.OpenConnection();
            using (var cmd = new SqlCommand(obj.IsoStart + " select * from " + obj.dBName + "..M_CompanyMaster " + obj.IsoEnd, dbConnectselect.cnnObject))
            using (var dRead = cmd.ExecuteReader())
            {
                if (dRead.Read())
                {
                    HttpContext.Current.Session["CompAdd"] = dRead["CompAdd"]?.ToString() ?? "";
                    HttpContext.Current.Session["CompWeb"] = string.IsNullOrEmpty(dRead["WebSite"]?.ToString()) ? "index.asp" : dRead["WebSite"].ToString();
                    HttpContext.Current.Session["CompMail"] = dRead["CompMail"]?.ToString() ?? "";
                    HttpContext.Current.Session["CompMobile"] = dRead["MobileNo"]?.ToString() ?? "";
                    HttpContext.Current.Session["ClientId"] = dRead["smsSenderId"]?.ToString() ?? "";
                    HttpContext.Current.Session["SmsId"] = dRead["smsUserNm"]?.ToString() ?? "";
                    HttpContext.Current.Session["SmsPass"] = dRead["smPass"]?.ToString() ?? "";
                    HttpContext.Current.Session["MailPass"] = dRead["mailPass"]?.ToString() ?? "";
                    HttpContext.Current.Session["MailHost"] = dRead["mailHost"]?.ToString() ?? "";
                    HttpContext.Current.Session["AdminWeb"] = dRead["AdminWeb"]?.ToString() ?? "";
                    HttpContext.Current.Session["CompCST"] = dRead["CompCSTNo"]?.ToString() ?? "";
                    HttpContext.Current.Session["CompState"] = dRead["CompState"]?.ToString() ?? "";

                    // Format date as "dd-MMM-yyyy" if present
                    if (dRead["RecTimeStamp"] != DBNull.Value)
                    {
                        var dt = Convert.ToDateTime(dRead["RecTimeStamp"]);
                        HttpContext.Current.Session["CompDate"] = dt.ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        HttpContext.Current.Session["CompDate"] = "";
                    }

                    HttpContext.Current.Session["Spons"] = "PR10000001";
                    HttpContext.Current.Session["CompWeb1"] = dRead["WebSite"]?.ToString() ?? "";
                    HttpContext.Current.Session["CompMovieWeb"] = "";
                    HttpContext.Current.Session["SmsAPI"] = "";
                    HttpContext.Current.Session["CompType"] = dRead["PlanType"]?.ToString() ?? "";

                    // Determine scheme and build short url (keeps original intent)
                    string scheme = HttpContext.Current.Request.Url.Scheme; // "http" or "https"
                    string host = HttpContext.Current.Request.Url.Host;
                    string joinPage = HttpContext.Current.Session["JoinPage"]?.ToString() ?? "";
                    HttpContext.Current.Session["CompShortUrl"] = string.Format("{0}://{1}/{2}", scheme, host, joinPage);
                }
                else
                {
                    HttpContext.Current.Session["CompName"] = "";
                    HttpContext.Current.Session["CompAdd"] = "";
                    HttpContext.Current.Session["CompWeb"] = "";
                    HttpContext.Current.Session["Title"] = "Welcome";
                }
            }

            // -----------------------
            // M_ConfigMaster
            // -----------------------
            using (var cmd = new SqlCommand(obj.IsoStart + " select * from " + obj.dBName + "..M_ConfigMaster " + obj.IsoEnd, dbConnectselect.cnnObject))
            using (var dRead = cmd.ExecuteReader())
            {
                if (dRead.Read())
                {
                    HttpContext.Current.Session["IsGetExtreme"] = dRead["IsGetExtreme"]?.ToString() ?? "N";
                    HttpContext.Current.Session["IsTopUp"] = dRead["IsTopUp"]?.ToString() ?? "N";
                    HttpContext.Current.Session["IsSendSMS"] = dRead["IsSendSMS"]?.ToString() ?? "N";
                    HttpContext.Current.Session["IdNoPrefix"] = dRead["IdNoPrefix"]?.ToString() ?? "";
                    HttpContext.Current.Session["IsFreeJoin"] = dRead["IsFreeJoin"]?.ToString() ?? "N";
                    HttpContext.Current.Session["IsStartJoin"] = dRead["IsStartJoin"]?.ToString() ?? "N";
                    HttpContext.Current.Session["JoinStartFrm"] = dRead["JoinStartFrm"]?.ToString() ?? "01-Sep-2011";
                    HttpContext.Current.Session["IsSubPlan"] = dRead["IsSubPlan"]?.ToString() ?? "N";
                    HttpContext.Current.Session["Logout"] = dRead["LogoutPg"]?.ToString() ?? "Default.aspx";
                }
                else
                {
                    HttpContext.Current.Session["IsGetExtreme"] = "N";
                    HttpContext.Current.Session["IsTopUp"] = "N";
                    HttpContext.Current.Session["IsSendSMS"] = "N";
                    HttpContext.Current.Session["IdNoPrefix"] = "";
                    HttpContext.Current.Session["IsFreeJoin"] = "N";
                    HttpContext.Current.Session["IsStartJoin"] = "N";
                    HttpContext.Current.Session["JoinStartFrm"] = "01-Sep-2011";
                    HttpContext.Current.Session["IsSubPlan"] = "N";
                    HttpContext.Current.Session["Logout"] = "Default.aspx";
                }
            }

            // -----------------------
            // Max(SEssid) from D_Monthlypaydetail
            // -----------------------
            using (var cmd = new SqlCommand(obj.IsoStart + " select Max(SEssid) as SessID from " + obj.dBName + "..D_Monthlypaydetail " + obj.IsoEnd, dbConnectselect.cnnObject))
            using (var dRead = cmd.ExecuteReader())
            {
                if (dRead.Read())
                {
                    HttpContext.Current.Session["MaxSessn"] = dRead["SessID"] != DBNull.Value ? dRead["SessID"].ToString() : "";
                }
                else
                {
                    HttpContext.Current.Session["MaxSessn"] = "";
                }
            }

            // -----------------------
            // Max(SEssid) from m_SessnMaster
            // -----------------------
            using (var cmd = new SqlCommand(obj.IsoStart + " select Max(SEssid) as SessID from " + obj.dBName + "..m_SessnMaster " + obj.IsoEnd, dbConnectselect.cnnObject))
            using (var dRead = cmd.ExecuteReader())
            {
                if (dRead.Read())
                {
                    HttpContext.Current.Session["CurrentSessn"] = dRead["SessID"] != DBNull.Value ? dRead["SessID"].ToString() : "";
                }
                else
                {
                    HttpContext.Current.Session["CurrentSessn"] = "";
                }
            }
        }
        catch
        {
            HttpContext.Current.Session["CompName"] = "";
            HttpContext.Current.Session["CompAdd"] = "";
            HttpContext.Current.Session["CompWeb"] = "";
        }
    }
    private string DisableTheButton(Control pge, Control btn)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
        sb.Append("this.value = 'Please Wait...';");
        sb.Append("this.disabled = true;");
        sb.Append(pge.Page.GetPostBackEventReference(btn));
        sb.Append(";");

        return sb.ToString();
    }
    private string GetCompID()
    {
        string url = string.Empty;
        SqlConnection conn = null;
        try
        {
            url = HttpContext.Current.Request.Url.Host
                .ToUpper()
                .Replace("HTTP://", "")
                .Replace("HTTPS://", "")
                .Replace("WWW.", "")
                .Replace("BASICMLM.", "")
                .Replace("CPANEL.", "")
                .Replace("LOGIN.", "")
                .Replace("CONSULTANT.", "")
                .Replace("NETWORK.", "");

            string str = string.Empty;

            if (url == "LOCALHOST")
            {
                str = "Select ID,Logo,PartyCode,Name,URL,gvPortalCompID,UtiLityPortalID from M_CompanyMasterNew " +
                      "Where IsActive = 1 And ID='" + ConfigurationManager.AppSettings["CompanyID"] + "'";
            }
            else
            {
                str = "Select ID,Logo,PartyCode,Name,URL,gvPortalCompID,UtiLityPortalID from M_CompanyMasterNew " +
                      "Where IsActive = 1 And (Upper(URL) = '" + url.ToUpper().Trim() + "')";
            }

            SqlDataReader dRead;
            SqlCommand cmd;
            conn = new SqlConnection(Application["sConnect"].ToString());
            conn.Open();
            cmd = new SqlCommand(str, conn);
            dRead = cmd.ExecuteReader();

            if (dRead.Read())
            {
                Session["CompID"] = dRead["ID"];
                Session["Logo"] = dRead["Logo"];
                Session["WRPartyCode"] = dRead["PartyCode"];
                Session["CompName"] = dRead["Name"];
                Session["Title"] = "Welcome To " + dRead["Name"];
                Session["gvPortalCompID"] = dRead["gvPortalCompID"];
                Session["UtiLityPortalID"] = dRead["UtiLityPortalID"];
            }
            else
            {
                Response.Redirect("UnderCons.aspx", false); // keep commented as in original
            }

            dRead.Close();
            conn.Close();
        }
        catch (Exception ex)
        {
            // Consider logging ex
            if (conn != null)
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        return url;
    }
    private void GetSmsTemplate()
    {
        try
        {
            DataTable dtMenu = new DataTable();
            DataSet ds = new DataSet();

            string str = "EXEC Sp_GetSmsTemplate '"
                         + HttpContext.Current.Session["CompID"] + "'";

            ds = SqlHelper.ExecuteDataset((string)Application["sConnect"], CommandType.Text, str);

            dtMenu = ds.Tables[1];

            if (dtMenu.Rows.Count > 0)
            {
                Session["ForgotPasswordsms"] = dtMenu.Rows[0]["ForgotPassword"];
            }
            else
            {
                Session["ForgotPasswordsms"] = "";
            }
        }
        catch (Exception ex)
        {
            // optional: log exception
        }
    }
    protected void Submit_Click(object sender, EventArgs e)
    {
        SqlCommand Comm;
        DataTable Dt;
        SqlDataAdapter Ad;
        string scrname;

        // lblerror.Text = "";

        if (txtIDNo.Text == "")
        {
            scrname = "<SCRIPT language='javascript'>alert('ID No. can not be left blank');</SCRIPT>";
            this.RegisterStartupScript("MyAlert", scrname);
            return;
        }

        if (TxtMobileNo.Text == "")
        {
            scrname = "<SCRIPT language='javascript'>alert('Email id. can not be left blank');</SCRIPT>";
            this.RegisterStartupScript("MyAlert", scrname);
            return;
        }

        if (txtIDNo.Text == "" || TxtMobileNo.Text == "")
        {
            scrname = "<SCRIPT language='javascript'>alert('Please Fill Detail');</SCRIPT>";
            this.RegisterStartupScript("MyAlert", scrname);
            return;
        }
        string IDNo = txtIDNo.Text.Trim()
            .Replace("'", "")
            .Replace(";", "")
            .Replace("=", "")
            .Replace("-", "");

        if (IDNo != "")
        {
            string MemberPass = "";
            string MemberTransPassw = "";

            string str = "EXEC Sp_MemberForgotPassw '" + IDNo + "'";

            Dt = SqlHelper.ExecuteDataset(
                    (string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]],
                    CommandType.Text,
                    str
                 ).Tables[0];

            if (Dt.Rows.Count > 0)
            {
                string Username = Dt.Rows[0]["Idno"].ToString();
                string Password = Dt.Rows[0]["Passw"].ToString();
                string TranPassw = Dt.Rows[0]["EPassw"].ToString();
                string Email = Dt.Rows[0]["EMail"].ToString();
                string mobl = Dt.Rows[0]["mobl"].ToString();
                string MemfristName = Dt.Rows[0]["MemName"].ToString();

                Session["SmsId"] = Dt.Rows[0]["smsUsernm"];
                Session["SmsPass"] = Dt.Rows[0]["SmPass"];
                Session["ClientId"] = Dt.Rows[0]["smsSenderID"];

                if (TxtMobileNo.Text == Dt.Rows[0]["Email"].ToString())
                {
                    MemberPass = Password;
                    MemberTransPassw = TranPassw;

                    // URL encoding (same logic as VB)
                    MemberPass = MemberPass.Replace("%", "%25")
                                           .Replace("&", "%26")
                                           .Replace("#", "%23")
                                           .Replace("'", "%22")
                                           .Replace(",", "%2C")
                                           .Replace("(", "%28")
                                           .Replace(")", "%29")
                                           .Replace("*", "%2A")
                                           .Replace("!", "%21")
                                           .Replace("/", "%2F")
                                           .Replace("@", "%40");

                    MemberTransPassw = MemberTransPassw.Replace("%", "%25")
                                                       .Replace("&", "%26")
                                                       .Replace("#", "%23")
                                                       .Replace("'", "%22")
                                                       .Replace(",", "%2C")
                                                       .Replace("(", "%28")
                                                       .Replace(")", "%29")
                                                       .Replace("*", "%2A")
                                                       .Replace("!", "%21")
                                                       .Replace("/", "%2F")
                                                       .Replace("@", "%40");

                    string sms = "";

                    //sendSMS("", MemberPass, MemberTransPassw, txtIDNo.Text, Session["CompWeb"], mobl);
                    SendToMemberMail(txtIDNo.Text, Email, MemfristName, Password, TranPassw);

                    scrname = "<SCRIPT language='javascript'>alert('Your Password has been sent on your E mail Id !');location.replace('forgot.aspx');</SCRIPT>";
                    this.RegisterStartupScript("MyAlert", scrname);

                    txtIDNo.Text = "";
                    TxtMobileNo.Text = "";

                    scrname = "<SCRIPT language='javascript'>window.top.location.reload();</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                    return;
                }
                else
                {
                    scrname = "<SCRIPT language='javascript'>alert('Invalid Email id.');</SCRIPT>";
                    this.RegisterStartupScript("MyAlert", scrname);
                    return;
                }
            }
            else
            {
                scrname = "<SCRIPT language='javascript'>alert('Invalid ID No.');</SCRIPT>";
                this.RegisterStartupScript("MyAlert", scrname);
                return;
            }
        }
    }
    public bool SendToMemberMail(string IdNo, string Email, string MemberName, string Password, string EPassword)
    {
        try
        {
            DataTable dt;
            string sql = "";
            string userEmail = "";
            string StrMsg = "";
            MailAddress SendFrom = new MailAddress(Session["CompMail"].ToString());
            MailAddress SendTo = new MailAddress(Email);
            MailMessage MyMessage = new MailMessage(SendFrom, SendTo);
            StrMsg =
                "<table style=\"margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%\">" +
                "<tr>" +
                "<td>" +
                "<span style=\"color: #0099CC; font-weight: bold;\"><h2>Dear " + MemberName + ",</h2></span><br />" +
                "Your Forgot Login password is <strong>" + Password + "</strong> and Transaction password is <strong>" + EPassword + "</strong> of IDNO <strong>" + IdNo + "</strong>.<br/>" +
                "For login go to our site : <a href=\"" + Session["CompWeb"] + "\" target=\"_blank\" style=\"color:#0000FF; text-decoration:underline;\">" +
                Session["CompWeb"] + "</a><br/>Thank you!<br> Regards : <br/>" +
                "<a href=\"" + Session["CompWeb"] + "\" target=\"_blank\" style=\"color:#0000FF; text-decoration:underline;\">" +
                Session["CompName"] + "</a><br /><br /><br />" +
                "</td>" +
                "</tr>" +
                "</table>";
            MyMessage.Subject = "Forgot Password";
            MyMessage.Body = StrMsg;
            MyMessage.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient(Session["MailHost"].ToString());
            smtp.UseDefaultCredentials = false;
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(Session["CompMail"].ToString(), Session["MailPass"].ToString());
            smtp.Send(MyMessage);
            return true;
        }
        catch (Exception ex)
        {
            // Response.Write(ex.Message);
            Response.Write("Try later.");
            return false;
        }
    }
}