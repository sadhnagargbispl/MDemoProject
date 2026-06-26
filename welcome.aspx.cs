using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;

public partial class welcome : System.Web.UI.Page
{
    DAL Obj;
    string Url;
    clsGeneral objGen = new clsGeneral();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            GetCompID();
            objGen.GetConnectionByComp();
            objGen.GetInvDataBaseByComp();
            Pages();
            Obj = new DAL();
            getData();
            if (!Page.IsPostBack)
            {
                string strcondition = "";
                string str = "";
                string k = "";
                DataTable dt = new DataTable();

                if (Request["id"] != null)
                {
                    k = Request["id"].Replace(" ", "+");
                    string s = Crypto.Decrypt(k);
                    string[] sbstr = s.Split('/');

                    strcondition = " and mMst.FormNo=''" + sbstr[0] + "''";
                }
                else
                {
                    if (Session["JOIN"] != null && Session["JOIN"].ToString() == "YES")
                    {
                        strcondition = " and mMst.IDNo=''" + Session["LASTID"] + "''";
                        Session["JOIN"] = "FINISH";
                        BtnHome.Visible = false;
                    }
                    else if (Session["Status"] != null && Session["Status"].ToString() == "OK")
                    {
                        strcondition = " and mMst.FormNo=''" + Convert.ToInt32(Session["Formno"]) + "''";
                    }
                    else
                    {
                        Response.Redirect("Default.aspx");
                        Response.End();
                    }
                }

                str = Obj.IsoStart + "exec sp_MemDtl '" + strcondition + "'" + Obj.IsoEnd;
                dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    LblYear.Text = dt.Rows[0]["CYear"].ToString();
                    LblId.Text = dt.Rows[0]["Idno"].ToString();
                    LblIdno.Text = dt.Rows[0]["Idno"].ToString();

                    LblName.Text = dt.Rows[0]["Memname"].ToString();
                    Session["Membername"] = dt.Rows[0]["Memname"].ToString();

                    //LblAddress.Text = dt.Rows[0]["Address1"].ToString();
                    //LblCity.Text = dt.Rows[0]["cityName"].ToString();
                    //LblDistrict.Text = dt.Rows[0]["District"].ToString();
                    //LblState.Text = dt.Rows[0]["statename"].ToString();
                    LblMobl.Text = dt.Rows[0]["Mobl"].ToString();

                    LblEmail.Text = dt.Rows[0]["Email"].ToString();
                    //LblPanno.Text = dt.Rows[0]["Panno"].ToString();

                    //lblDoj.Text = Convert.ToDateTime(dt.Rows[0]["Doj"]).ToString("dd-MMM-yyyy");

                    //LblPlacementid.Text = dt.Rows[0]["RefIdno"].ToString();
                    //LblPlacementName.Text = dt.Rows[0]["RefName"].ToString();

                    //if (Convert.ToInt32(Session["CompId"]) != 1067)
                    //{
                    //    LblKitName.Text = dt.Rows[0]["Category"].ToString();
                    //    LblKitAmount.Text = dt.Rows[0]["Kitamount"].ToString();
                    //}

                    LblPassw.Text = dt.Rows[0]["Password"].ToString();

                    if (Convert.ToInt32(Session["CompID"]) == 1055)
                    {
                        LblEPassw.Visible = false;
                        lblEpasswl.Visible = false;
                    }
                    else
                    {
                        lblEpasswl.Visible = true;
                        LblEPassw.Visible = true;
                        LblEPassw.Text = dt.Rows[0]["Epassw"].ToString();
                    }

                    
                    hdnFormNo.Value = dt.Rows[0]["FormNo"].ToString();
                }
            }

            if (Convert.ToInt32(Session["CompID"]) == 1006)
            {
                btnActive.Visible = true;
                btnPackage.Visible = true;
            }
            else
            {
                btnActive.Visible = false;
                btnPackage.Visible = false;
            }

            if (Convert.ToInt32(Session["CompID"]) == 1030)
            {
                btnShopping.Visible = true;
            }
            else
            {
                btnShopping.Visible = false;
            }

            //if (Request["IDNo"] != null)
            //{
            //    if (Convert.ToInt32(Session["CompID"]) == 1025)
            //    {
            //        if (!string.IsNullOrEmpty(LblEmail.Text))
            //        {
            //            SendToMemberMail(LblId.Text, LblEmail.Text, LblName.Text, LblPassw.Text,
            //                "2020", LblEPassw.Text, LblDistrict.Text, LblDistrict.Text,
            //                LblKitName.Text, LblKitAmount.Text, LblAddress.Text,
            //                LblMobl.Text, LblState.Text, lblDoj.Text, LblPanno.Text);
            //        }
            //    }
            //}

            if (Convert.ToInt32(Session["CompID"]) == 1038)
            {
                signUcm.Visible = true;
                signAll.Visible = false;
            }
            else
            {
                signAll.Visible = true;
                signUcm.Visible = false;
            }

            if (Convert.ToInt32(Session["CompID"]) == 1066)
            {
                signUcm.Visible = true;
                signAll.Visible = false;
            }
            else
            {
                signAll.Visible = true;
                signUcm.Visible = false;
            }

            if (Convert.ToInt32(Session["CompID"]) == 1039)
            {
                divWelcomeAction.Visible = true;
                divWelcomeAll.Visible = false;
            }
            else
            {
                divWelcomeAction.Visible = false;
                divWelcomeAll.Visible = true;
            }

            if (Convert.ToInt32(Session["CompID"]) == 1078 || Convert.ToInt32(Session["CompID"]) == 1093)
            {
                signUcm.Visible = false;
                signAll.Visible = false;
                signmanjar.Visible = true;
            }
            else
            {
                signAll.Visible = false;
                signUcm.Visible = false;
                signmanjar.Visible = false;
            }

            //if (Session["CompID"].ToString() == "1007" || Session["CompID"].ToString() == "1075")
            //{
            //    tdKitDetail.Visible = false;
            //}
            //else
            //{
            //    tdKitDetail.Visible = true;
            //    tdkit.Visible = true;
            //}

            //if (Session["CompID"].ToString() == "1066"
            //    || Session["CompID"].ToString() == "1078"
            //    || Session["CompID"].ToString() == "1093")
            //{
            //    tdKitDetail.Visible = false;
            //    tdkit.Visible = false;
            //}
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    public string GetCompID()
    {
        string url = string.Empty;
        SqlConnection conn = null;

        try
        {
            url = HttpContext.Current.Request.Url.Host.ToUpper()
                .Replace("HTTP://", "")
                .Replace("HTTPS://", "")
                .Replace("WWW.", "")
                .Replace("BASICMLM.", "")
                .Replace("CPANEL.", "")
                .Replace("NETWORK.", "");

            string str = string.Empty;

            if (url == "LOCALHOST")
            {
                str = "Select ID,Logo,PartyCode,Name,URL,gvPortalCompID,UtiLityPortalID " +
                      "from M_CompanyMasterNew Where IsActive=1 And ID='" +
                       ConfigurationManager.AppSettings["CompanyID"] + "'";
            }
            else
            {
                str = "Select ID,Logo,PartyCode,Name,URL,gvPortalCompID,UtiLityPortalID " +
                      "from M_CompanyMasterNew Where IsActive=1 And Upper(URL)='" + url + "'";
            }

            conn = new SqlConnection(HttpContext.Current.Application["sConnect"].ToString());
            conn.Open();

            SqlCommand cmd = new SqlCommand(str, conn);
            SqlDataReader dRead = cmd.ExecuteReader();

            if (dRead.Read())
            {
                HttpContext.Current.Session["CompID"] = dRead["ID"];
                HttpContext.Current.Session["Logo"] = dRead["Logo"];
                HttpContext.Current.Session["WRPartyCode"] = dRead["PartyCode"];
                HttpContext.Current.Session["CompName"] = dRead["Name"];
                HttpContext.Current.Session["Title"] = "Welcome To " + dRead["Name"];
                HttpContext.Current.Session["gvPortalCompID"] = dRead["gvPortalCompID"];
                HttpContext.Current.Session["UtiLityPortalID"] = dRead["UtiLityPortalID"];
            }

            dRead.Close();
            conn.Close();
        }
        catch
        {
            if (conn != null && conn.State == ConnectionState.Open)
                conn.Close();
        }

        return url;
    }
    private void Pages()
    {
        try
        {
            string str = "Select a.MenuId, a.MenuName, a.ParentId, a.OnSelect " +
                         "From M_CompWiseWebMenuMasterDis a " +
                         "Where MenuId in (1,3) And a.ActiveStatus='Y' And a.RowStatus='Y' " +
                         "And a.CompanyID='" + HttpContext.Current.Session["CompID"] + "' " +
                         "Order By Convert(decimal, RTRIM(LTRIM(a.Hierar))), a.MenuId";

            DataSet ds = SqlHelper.ExecuteDataset(HttpContext.Current.Application["sConnect"].ToString(),
                                                  CommandType.Text, str);

            DataTable dtMenu = ds.Tables[0];

            HttpContext.Current.Session["IndexPage"] = dtMenu.Rows[0]["OnSelect"];
            HttpContext.Current.Session["JoinPage"] = dtMenu.Rows[1]["OnSelect"];
        }
        catch
        {
        }
    }
    private void getData()
    {
        try
        {
            cls_DataAccess dbConnect =
                new cls_DataAccess(HttpContext.Current.Session["MlmDatabase" + HttpContext.Current.Session["CompID"]].ToString());

            dbConnect.OpenConnection();

            SqlCommand cmd;
            SqlDataReader dRead;

            // Company Master
            cmd = new SqlCommand("select * from M_CompanyMaster", dbConnect.cnnObject);
            dRead = cmd.ExecuteReader();

            if (dRead.Read())
            {
                HttpContext.Current.Session["CompAdd"] = dRead["CompAdd"];
                HttpContext.Current.Session["CompWeb"] = string.IsNullOrEmpty(dRead["WebSite"].ToString())
                    ? "index.asp"
                    : dRead["WebSite"];

                HttpContext.Current.Session["CompMail"] = dRead["CompMail"];
                HttpContext.Current.Session["CompMobile"] = dRead["MobileNo"];
                HttpContext.Current.Session["ClientId"] = dRead["smsSenderId"];
                HttpContext.Current.Session["SmsId"] = dRead["smsUserNm"];
                HttpContext.Current.Session["SmsPass"] = dRead["smPass"];
                HttpContext.Current.Session["MailPass"] = dRead["mailPass"];
                HttpContext.Current.Session["MailHost"] = dRead["mailHost"];
                HttpContext.Current.Session["AdminWeb"] = dRead["AdminWeb"];
                HttpContext.Current.Session["CompCST"] = dRead["CompCSTNo"];
                HttpContext.Current.Session["CompState"] = dRead["CompState"];
                HttpContext.Current.Session["CompDate"] =
                    Convert.ToDateTime(dRead["RecTimeStamp"]).ToString("dd-MMM-yyyy");
                HttpContext.Current.Session["Spons"] = "PR10000001";
                HttpContext.Current.Session["CompWeb1"] = dRead["WebSite"];
                HttpContext.Current.Session["CompMovieWeb"] = "";
                HttpContext.Current.Session["SmsAPI"] = "";
                HttpContext.Current.Session["CompShortUrl"] =
                    (HttpContext.Current.Request.Url.Host.ToUpper().StartsWith("HTTPS") ? "https://" : "http://") +
                    HttpContext.Current.Request.Url.Host + "/" +
                    HttpContext.Current.Session["JoinPage"];
            }
            else
            {
                HttpContext.Current.Session["CompName"] = "";
                HttpContext.Current.Session["CompAdd"] = "";
                HttpContext.Current.Session["CompWeb"] = "";
                HttpContext.Current.Session["Title"] = "Welcome";
            }

            dRead.Close();

            // Config Master
            cmd = new SqlCommand("select * from M_ConfigMaster", dbConnect.cnnObject);
            dRead = cmd.ExecuteReader();

            if (dRead.Read())
            {
                HttpContext.Current.Session["IsGetExtreme"] = dRead["IsGetExtreme"];
                HttpContext.Current.Session["IsTopUp"] = dRead["IsTopUp"];
                HttpContext.Current.Session["IsSendSMS"] = dRead["IsSendSMS"];
                HttpContext.Current.Session["IdNoPrefix"] = dRead["IdNoPrefix"];
                HttpContext.Current.Session["IsFreeJoin"] = dRead["IsFreeJoin"];
                HttpContext.Current.Session["IsStartJoin"] = dRead["IsStartJoin"];
                HttpContext.Current.Session["JoinStartFrm"] = dRead["JoinStartFrm"];
                HttpContext.Current.Session["IsSubPlan"] = dRead["IsSubPlan"];
                HttpContext.Current.Session["Logout"] = dRead["LogoutPg"];
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

            dRead.Close();

            // Monthly Pay Detail
            cmd = new SqlCommand("select Max(Sessid) as SessID from D_Monthlypaydetail", dbConnect.cnnObject);
            dRead = cmd.ExecuteReader();

            HttpContext.Current.Session["MaxSessn"] = dRead.Read() ? dRead["SessID"] : "";

            dRead.Close();

            // Session Master
            cmd = new SqlCommand("select Max(Sessid) as SessID from m_SessnMaster", dbConnect.cnnObject);
            dRead = cmd.ExecuteReader();

            HttpContext.Current.Session["CurrentSessn"] = dRead.Read() ? dRead["SessID"] : "";

            dRead.Close();
        }
        catch
        {
            HttpContext.Current.Session["CompName"] = "";
            HttpContext.Current.Session["CompAdd"] = "";
            HttpContext.Current.Session["CompWeb"] = "";
        }
    }
    protected void btnActive_ServerClick(object sender, EventArgs e)
    {
        if (Convert.ToInt32(Session["CompID"]) == 1006)
        {
            Response.Redirect("RepurchaseProductRequestTB.aspx?Frm=" + hdnFormNo.Value, false);
        }
        else
        {
            Response.Redirect("RepurchaseProductRequest.aspx?Frm=" + hdnFormNo.Value, false);
        }
    }
    protected void btnPackage_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("epinDetail.aspx");
    }
    protected void btnShopping_ServerClick(object sender, EventArgs e)
    {
        try
        {
            string refCode = "Login";
            refCode = Base64Helpers.Base64Encode(refCode);

            string info = LblIdno.Text + ";" + LblPassw.Text + ";" +
                          DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            info = Base64Helpers.Base64Encode(info);

            string kShopUrl = "http://shopping.life4ever.co.in/Account/Directlogin?refs="
                              + refCode + "&info=" + info;

            Response.Redirect(kShopUrl);
        }
        catch (Exception)
        {
            // You can add logging if needed
        }
    }
    public bool SendToMemberMail(
    string IdNo, string Email, string MemberName, string Password,
    string year, string TransPassw, string City, string District,
    string KitName, string KitAmount, string address, string Mobile,
    string State, string Doj, string Panno)
    {
        try
        {
            string path = Server.MapPath("~/images/Logo1025.png");
            LinkedResource img1 = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
            img1.ContentId = "Image1";

            MailAddress SendFrom = new MailAddress(Session["CompMail"].ToString());
            MailAddress SendTo = new MailAddress(Email);

            MailMessage MyMessage = new MailMessage(SendFrom, SendTo);

            string StrMsg = @"
        <table>
            <tr>
                <td width='50%'>
                    <div class='logobox' align='left' style='margin-left: 20px'>
                        <img src='cid:Image1'>
                    </div>
                </td>
                <td width='50%'>
                    <h4>&nbsp;&nbsp; Welcome Letter</h4>
                </td>
            </tr>
        </table>

        <br />
        <div class='col-sm-12'>
            <h5 style='display:none;'>Letter(No - EMC / " + IdNo + @")/" + year + @"</h5>

            <p>&nbsp;</p>

            <h6 class='text-danger'>
                <strong><em>Dear Clients / Participants and Families,</em></strong>
            </h6>

            <p>
                We are " + Session["CompName"] + @", are pleased to welcome you as our new client
                and take this opportunity to extend our warm greetings to you.
            </p>

            <p>
                We assure you that you will find it enjoyable and professionally beneficial...
            </p>

            <p>
                Please find below your enrollment details with " + Session["CompName"] + @" :
            </p>

            <table class='table table-bordered' cellspacing='0' 
               style='border:1px solid black;'>

                <tr>
                    <td style='font-weight:bold;border:1px solid black;'>ID NO</td>
                    <td style='border:1px solid black;'>" + IdNo + @"</td>

                    <td style='font-weight:bold;border:1px solid black;'>Name</td>
                    <td style='border:1px solid black;'>" + MemberName + @"</td>
                </tr>

                <tr>
                    <td style='font-weight:bold;border:1px solid black;'>Address</td>
                    <td style='border:1px solid black;'>" + address + @"</td>

                    <td style='font-weight:bold;border:1px solid black;'>City</td>
                    <td style='border:1px solid black;'>" + City + @"</td>
                </tr>

                <tr>
                    <td style='font-weight:bold;border:1px solid black;'>District</td>
                    <td style='border:1px solid black;'>" + District + @"</td>

                    <td style='font-weight:bold;border:1px solid black;'>State</td>
                    <td style='border:1px solid black;'>" + State + @"</td>
                </tr>

                <tr>
                    <td style='font-weight:bold;border:1px solid black;'>Mobile</td>
                    <td style='border:1px solid black;'>" + Mobile + @"</td>

                    <td style='font-weight:bold;border:1px solid black;'>Joining Date</td>
                    <td style='border:1px solid black;'>" + Doj + @"</td>
                </tr>

                <tr>
                    <td style='font-weight:bold;border:1px solid black;'>Joining Kit</td>
                    <td style='border:1px solid black;'>" + KitName + @"</td>

                    <td style='font-weight:bold;border:1px solid black;'>Kit Amount</td>
                    <td style='border:1px solid black;'>" + KitAmount + @"</td>
                </tr>

                <tr>
                    <td style='font-weight:bold;border:1px solid black;'>Email ID</td>
                    <td style='border:1px solid black;'>" + Email + @"</td>

                    <td style='font-weight:bold;border:1px solid black;'>PAN No</td>
                    <td style='border:1px solid black;'>" + Panno + @"</td>
                </tr>

                <tr>
                    <td style='font-weight:bold;border:1px solid black;'>Password</td>
                    <td style='border:1px solid black;'>" + Password + @"</td>

                    <td style='font-weight:bold;border:1px solid black;'>T.Password</td>
                    <td style='border:1px solid black;'>" + TransPassw + @"</td>
                </tr>

            </table>

            <p><br> See you at the Top</p>

            <p class='text-success'>
                Support Team <br/>
                <strong>" + Session["CompName"] + @"</strong>
            </p>
        </div>
        ";

            AlternateView av1 = AlternateView.CreateAlternateViewFromString(StrMsg, null, MediaTypeNames.Text.Html);
            av1.LinkedResources.Add(img1);

            MyMessage.AlternateViews.Add(av1);
            MyMessage.Subject = "Welcome and Congratulations!";
            MyMessage.Body = StrMsg;
            MyMessage.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(Session["MailHost"].ToString());
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(Session["CompMail"].ToString(), Session["MailPass"].ToString());

            smtp.Send(MyMessage);

            return true;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            HttpContext.Current.Response.Write("Try later.");
            return false;
        }
    }
    protected void BtnHome_Click(object sender, EventArgs e)
    {
        Response.Redirect(Session["IndexPage"].ToString(), false);
    }
    protected void BtnNewJoin_Click(object sender, EventArgs e)
    {
        Response.Redirect(Session["JoinPage"].ToString(), false);
    }
}