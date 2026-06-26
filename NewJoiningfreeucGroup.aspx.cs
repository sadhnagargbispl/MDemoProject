using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

public partial class NewJoiningfreeucGroup : System.Web.UI.Page
{
    double _dblAvailLeg = 0;
    private clsGeneral dbGeneral = new clsGeneral();
    private cls_DataAccess dbConnect;
    private cls_DataAccess dbConnectselect;
    DAL ObjDAL;
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dRead;
    public string DsnName, UserName, Passw;
    private string strQuery, strCaptcha;
    DataTable tmpTable = new DataTable();
    //AccClass.MyAccClass.NewClass QryCls = new AccClass.MyAccClass.NewClass();
    int minSpnsrNoLen, minScrtchLen;

    string Upln = "0", dblSpons = "0", dblState = "0", dblBank = "0", dblIdNo = "0";
    string dblDistrict, dblTehsil, IfSC;
    string dblPlan;
    DateTime CurrDt;
    string scrname;
    string LastInsertID = "";
    string InVoiceNo;
    int SupplierId;
    string BillNo;
    string TaxType;
    string BillDate;
    int SBillNo;
    string SoldBy = "WR";
    string FType;
    clsGeneral objGen = new clsGeneral();
    private string ClearInject(string StrObj)
    {
        StrObj = StrObj.Replace(";", "")
                       .Replace("'", "")
                       .Replace("=", "");
        return StrObj.Trim();
    }
    private string DisableTheButton(Control pge, Control btn)
    {
        try
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
            sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
            sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
            sb.Append("this.value = 'Please Wait...';");
            sb.Append("this.disabled = true;");
            sb.Append(pge.Page.GetPostBackEventReference(btn));
            sb.Append(";");
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // SendToMemberMail("DV431287", "sadhnagarg.bispl@gmail.com", "SADHNA GARG", "Bright@Discount#11");
            GetCompID();
            objGen.GetConnectionByComp();
            objGen.GetInvDataBaseByComp();
            ColumnName();
            Pages();
            GetSmsTemplate();

            dbConnect = new cls_DataAccess(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
            dbConnect.OpenConnection();
            dbConnectselect = new cls_DataAccess(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString());
            dbConnectselect.OpenConnection();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            CmdSave.Attributes.Add("onclick", DisableTheButton(Page, CmdSave));
            // Put user code to initialize the page here
            txtUplinerId.Text = ClearInject(txtUplinerId.Text.Trim());
            ObjDAL = new DAL();
            // Below 23 Jan 2023
            //FillIdtypeMaster();

            getData();

            // dbConnect = new cls_DataAccess(Application["Connect"].ToString());

            string sr = "";
            string[] sbstr;
            string Key;
            string K = "";
            if (!Page.IsPostBack)
            {
                HdnCheckTrnns.Value = GenerateRandomStringJoining(6);
                GetSmsTemplate();
                Fill_State();
                if (Session["CompID"].ToString() == "1108" || Session["CompID"].ToString() == "1110")
                {
                    divlogin.Visible = true;
                }
                else
                {
                    divlogin.Visible = false;
                }

                if (Session["CompID"].ToString() == "1106")
                {
                    DivLeg1.Visible = false;
                }
                else
                {
                    DivLeg1.Visible = true;
                }
                if (!string.IsNullOrEmpty(txtPanNo.Text))
                    txtPanNo.Attributes.Add("CssClass", "form-control validate[custom[panno]]");

                if (Session["CompID"].ToString() == "1088")
                    DivVVRanta.Visible = true;
                else
                    DivVVRanta.Visible = false;

                divAdhar.Visible = false;

                // COMPANY-SPECIFIC UI SETTINGS
                if (Session["compid"].ToString() == "1108")
                {
                    Divdob.Visible = false;
                    divAddress.Visible = false;
                    divTermAll.Visible = false;
                    divTermVisionAllright.Visible = false;
                    txtAddLn1.Attributes.Add("Class", "form-control");
                    txtEMailId.Visible = true;
                    divPan.Visible = true;
                    ltrTerms.Text = "<a href='DIRECT_SELLER_CONTRACT.html' target='_blank'>Terms And Condition</a>";
                }
                else
                {
                    Divdob.Visible = false;
                    divAddress.Visible = false;
                    divTermAll.Visible = true;
                    divTermVisionAllright.Visible = false;
                    txtAddLn1.Attributes.Add("Class", "form-control");
                    ltrTerms.Text = "Terms And Condition";
                    txtEMailId.Visible = true;

                    divPan.Visible = true;
                }

                // rfvValidator.Enabled = false;

                if (Session["CompID"].ToString() == "1107")
                {
                    DivVVRanta.Visible = true;
                }
                else
                {
                    DivVVRanta.Visible = false;
                }

                ClrCtrl();

                RbtnLegNo.Items.Add("Group A");
                RbtnLegNo.Items.Add("Group B");
                RbtnLegNo.Items.Add("Group C");
                RbtnLegNo.Items[0].Selected = true;
                //===========================
                //    QueryString: "s"
                //===========================
                if (Request.QueryString["s"] != null && Request.QueryString["s"].Length > 0)
                {
                    K = Request["s"].Replace(" ", "+");
                    sr = Crypto.Decrypt(K);

                    sbstr = sr.Split('/');
                    string UplinerFormno = sbstr[1];

                    string sql = ObjDAL.IsoStart + "select * from " + ObjDAL.dBName + "..M_MemberMaster where Formno='" + UplinerFormno + "'" + ObjDAL.IsoEnd;
                    DataTable dt = new DataTable();

                    dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];

                    if (dt.Rows.Count > 0)
                        txtUplinerId.Text = dt.Rows[0]["Idno"].ToString();

                    string LegNo = sbstr[3];

                    txtUplinerId.ReadOnly = true;
                    txtRefralId.Text = Session["Idno"].ToString();
                    txtMobileNo.Text = Session["PhN1"].ToString();
                    txtPhNo.Text = Session["Mobl"].ToString();
                    txtEMailId.Text = Session["EMail"].ToString();

                    RbtnLegNo.SelectedIndex = (LegNo == "1") ? 0 : 1;
                    RbtnLegNo.Enabled = false;

                    Session["iLeg"] = LegNo;
                }

                //===========================
                // QueryString: "ref"
                //===========================
                if (Request.QueryString["ref"] != null)
                {
                    string req = Request.QueryString["ref"].Replace(" ", "+");
                    string dec = Crypto.Decrypt(req);
                    string[] rfAr = dec.Split('/');
                    txtRefralId.Text = rfAr[0];
                    //RbtnLegNo.SelectedIndex = 0;
                    //RbtnLegNo.Enabled = false;
                    goto RefLink;
                    //if (rfAr.Length >= 2)
                    //{
                    //    if (!string.IsNullOrEmpty(rfAr[0]) && rfAr[1] == "1")
                    //    {
                    //        txtRefralId.Text = rfAr[0];
                    //        RbtnLegNo.SelectedIndex = 0;
                    //        RbtnLegNo.Enabled = false;
                    //        goto RefLink;
                    //    }
                    //    else if (!string.IsNullOrEmpty(rfAr[0]) && rfAr[1] == "2")
                    //    {
                    //        txtRefralId.Text = rfAr[0];
                    //        RbtnLegNo.SelectedIndex = 1;
                    //        RbtnLegNo.Enabled = false;

                    //        goto RefLink;
                    //    }
                    //    else if (!string.IsNullOrEmpty(rfAr[0]) && rfAr[1] == "3")
                    //    {
                    //        txtRefralId.Text = rfAr[0];
                    //        RbtnLegNo.SelectedIndex = 2;
                    //        RbtnLegNo.Enabled = false;

                    //        goto RefLink;
                    //    }
                    //}

                    if (!string.IsNullOrEmpty(Request.QueryString["RefFormNo"]))
                        txtRefralId.Text = Get_IDNo(Request.QueryString["RefFormNo"].ToString());

                RefLink:
                    if (!string.IsNullOrWhiteSpace(txtRefralId.Text))
                        FillReferral();

                    txtRefralId.ReadOnly = true;
                }

                FillPaymode();
                dbGeneral.Fill_Date_box(ddlDOBdt, ddlDOBmnth, ddlDOBYr, 1940, DateTime.Now.AddYears(-18).Year);
                dbGeneral.Fill_Date_box(DDlMDay, DDLMMonth, DDLMYear, 1940, DateTime.Now.Year);

                FillBankMaster();
                FindSession();
                GetConfigDtl();

                vsblCtrl(false, true);
            }
            try
            {
                SqlDataReader dRead;
                SqlCommand cmd;

                cmd = new SqlCommand(
                    ObjDAL.IsoStart + "Select top 1 SessId as SessId from " + ObjDAL.dBName + "..M_MonthSessnMaster order by SessID desc" + ObjDAL.IsoEnd,
                    dbConnectselect.cnnObject
                );

                dRead = cmd.ExecuteReader();

                if (dRead.Read())
                    Session["Dsessid"] = dRead["SessID"];
                else
                    Session["Dsessid"] = 0;

                dRead.Close();
                cmd.Cancel();
            }
            catch
            {
                // Optional: logging
            }

            if (Session["IsGetExtreme"] != null && Session["IsGetExtreme"].ToString() == "N")
                rwSpnsr.Visible = true;
            else
                rwSpnsr.Visible = false;

        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            objGen.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    public string GenerateRandomStringJoining(int length)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        string sResult = "";

        for (int i = 0; i < length; i++)
        {
            sResult += allowChrs[rdm.Next(allowChrs.Length)];
        }

        return sResult;
    }
    private void GetSmsTemplate()
    {
        try
        {
            DataTable dtMenu = new DataTable();
            DataSet ds = new DataSet();

            string str = "exec Sp_GetSmsTemplate '" + HttpContext.Current.Session["CompID"] + "'";

            ds = SqlHelper.ExecuteDataset(Application["sConnect"].ToString(), CommandType.Text, str);
            dtMenu = ds.Tables[0];

            if (dtMenu.Rows.Count > 0)
            {
                Session["JoiningSms"] = dtMenu.Rows[0]["JoiningSms"];
            }
            else
            {
                Session["JoiningSms"] = "";
            }
        }
        catch (Exception)
        {
            // Optional: logging
        }
    }
    public string GetCompID()
    {
        string url = string.Empty;
        SqlConnection conn = null;

        try
        {
            // Get domain name
            url = HttpContext.Current.Request.Url.Host
                .ToUpper()
                .Replace("HTTP://", "")
                .Replace("HTTPS://", "")
                .Replace("WWW.", "")
                .Replace("BASICMLM.", "")
                .Replace("CPANEL.", "")
                .Replace("LOGIN.", "");

            string str = string.Empty;

            if (url == "LOCALHOST")
            {
                str = "SELECT ID, Logo, PartyCode FROM M_CompanyMasterNew WHERE IsActive = 1 AND ID='"
                      + ConfigurationManager.AppSettings["CompanyID"] + "'";
            }
            else
            {
                str = "SELECT ID, Logo, PartyCode FROM M_CompanyMasterNew WHERE IsActive = 1 " +
                      "AND (UPPER(URL) = '" + url.Trim().ToUpper() + "')";
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
            }

            dRead.Close();
            conn.Close();
        }
        catch (Exception)
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
            DataTable dtMenu = new DataTable();
            DataSet ds = new DataSet();

            string str = "SELECT a.MenuId, a.MenuName, a.ParentId, a.OnSelect, ActiveStatus " +
                         "FROM M_CompWiseWebMenuMasterDis a " +
                         "WHERE MenuId IN (1,3) AND a.CompanyID = '" + HttpContext.Current.Session["CompID"] + "' " +
                         "ORDER BY CONVERT(decimal, RTRIM(LTRIM(a.Hierar))), a.MenuId";

            ds = SqlHelper.ExecuteDataset(HttpContext.Current.Application["sConnect"].ToString(), CommandType.Text, str);
            dtMenu = ds.Tables[0];

            HttpContext.Current.Session["IndexPage"] = dtMenu.Rows[0]["OnSelect"].ToString();
            HttpContext.Current.Session["ActiveStatusJoin"] = dtMenu.Rows[0]["ActiveStatus"].ToString();
            HttpContext.Current.Session["JoinPage"] = dtMenu.Rows[1]["OnSelect"].ToString();
        }
        catch (Exception)
        {
            // swallow exception like VB version
        }
    }
    private void ColumnName()
    {
        try
        {
            DataTable dtMenu = new DataTable();
            DataSet ds = new DataSet();

            string str = "SELECT ColName1, ColName2, ColName3 " +
                         "FROM M_CompanyColSetting a " +
                         "WHERE a.CompanyID = '" + HttpContext.Current.Session["CompID"] + "'";

            ds = SqlHelper.ExecuteDataset(HttpContext.Current.Application["sConnect"].ToString(), CommandType.Text, str);
            dtMenu = ds.Tables[0];

            HttpContext.Current.Session["ColName1"] = dtMenu.Rows[0]["ColName1"].ToString();
            HttpContext.Current.Session["ColName2"] = dtMenu.Rows[0]["ColName2"].ToString();
            HttpContext.Current.Session["ColName3"] = dtMenu.Rows[0]["ColName3"].ToString();
        }
        catch (Exception)
        {
        }
    }

    private void getData()
    {
        try
        {
            var dbConnect = new cls_DataAccess((string)HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]]);
            dbConnect.OpenConnection();

            // -----------------------
            // M_CompanyMaster
            // -----------------------
            using (var cmd = new SqlCommand(ObjDAL.IsoStart + " select * from " + ObjDAL.dBName + "..M_CompanyMaster" + ObjDAL.IsoEnd, dbConnect.cnnObject))
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
            using (var cmd = new SqlCommand(ObjDAL.IsoStart + "select * from " + ObjDAL.dBName + "..M_ConfigMaster" + ObjDAL.IsoEnd, dbConnect.cnnObject))
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
            using (var cmd = new SqlCommand(ObjDAL.IsoStart + "select Max(SEssid) as SessID from " + ObjDAL.dBName + "..D_Monthlypaydetail" + ObjDAL.IsoEnd, dbConnect.cnnObject))
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
            using (var cmd = new SqlCommand(ObjDAL.IsoStart + "select Max(SEssid) as SessID from " + ObjDAL.dBName + "..m_SessnMaster" + ObjDAL.IsoEnd, dbConnect.cnnObject))
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
    private void FillPaymode()
    {
        try
        {
            strQuery = ObjDAL.IsoStart + "SELECT * FROM " + ObjDAL.dBName + "..M_PayModeMaster WHERE ActiveStatus='Y'" + ObjDAL.IsoEnd;
            ObjDAL = new DAL();
            tmpTable = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, strQuery).Tables[0];
            DdlPaymode.DataSource = tmpTable;
            DdlPaymode.DataValueField = "PID";
            DdlPaymode.DataTextField = "Paymode";
            DdlPaymode.DataBind();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void GetConfigDtl()
    {
        try
        {
            SqlDataReader dRead;
            SqlCommand cmd;

            cmd = new SqlCommand(ObjDAL.IsoStart + "SELECT * FROM " + ObjDAL.dBName + "..M_ConfigMaster" + ObjDAL.IsoEnd, dbConnectselect.cnnObject);
            dRead = cmd.ExecuteReader();

            if (dRead.Read())
            {
                Session["IsGetExtreme"] = dRead["IsGetExtreme"];
                Session["IsTopUp"] = dRead["IsTopUp"];
                Session["IsSendSMS"] = dRead["IsSendSMS"];
                Session["IdNoPrefix"] = dRead["IdNoPrefix"];
                Session["IsFreeJoin"] = dRead["IsFreeJoin"];
                Session["IsStartJoin"] = dRead["IsStartJoin"];
                Session["JoinStartFrm"] = dRead["JoinStartFrm"];
                Session["IsSubPlan"] = dRead["IsSubPlan"];
            }
            else
            {
                Session["IsGetExtreme"] = "N";
                Session["IsTopUp"] = "N";
                Session["IsSendSMS"] = "N";
                Session["IdNoPrefix"] = "";
                Session["IsFreeJoin"] = "N";
                Session["IsStartJoin"] = "N";
                Session["JoinStartFrm"] = "01-Sep-2011";
                Session["IsSubPlan"] = "N";
            }

            dRead.Close();
        }
        catch
        {
            Session["CompName"] = "";
            Session["CompAdd"] = "";
            Session["CompWeb"] = "";
        }
    }
    private string Get_IDNo(string MyFormNo)
    {
        try
        {
            string IdNo = "";

            SqlDataReader dRead;
            SqlCommand cmd;

            cmd = new SqlCommand(ObjDAL.IsoStart + "SELECT IdNo FROM " + ObjDAL.dBName + "..M_MemberMaster WHERE FormNo = '" + MyFormNo + "'" + ObjDAL.IsoEnd, dbConnectselect.cnnObject);

            dRead = cmd.ExecuteReader();

            if (dRead.Read())
            {
                IdNo = dRead["IdNo"].ToString();
            }

            dRead.Close();
            return IdNo;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
            Response.Write("Try later.");

            return "";
        }
    }
    protected void vsblCtrl(bool IsVsbl, bool IsOnlyDv)
    {
        try
        {
            if (!IsOnlyDv)
            {
                txtUplinerId.Enabled = !IsVsbl;
                txtRefralId.Enabled = !IsVsbl;
                // txtPIN.Enabled = !IsVsbl;
                // txtScratch.Enabled = !IsVsbl;
            }

            // dv_Main.Visible = IsVsbl;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    public string GenerateRandomString(int iLength)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        string sResult = "";

        for (int i = 0; i < iLength; i++)
        {
            sResult += allowChrs[rdm.Next(0, allowChrs.Length)];
        }

        return sResult;
    }
    public DataSet ConvertJsonStringToDataSet(string jsonString)
    {
        XmlDocument xd = new XmlDocument();

        jsonString = "{ \"rootNode\": " + jsonString.Trim().TrimStart('{').TrimEnd('}') + " }";

        xd = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonString);

        DataSet ds = new DataSet();
        ds.ReadXml(new XmlNodeReader(xd));

        return ds;
    }
    private void FillBankMaster()
    {
        try
        {
            strQuery = ObjDAL.IsoStart + "SELECT BankCode as Bid, BANKNAME as Bank " +
                       "FROM " + ObjDAL.dBName + "..M_BankMaster WHERE ACTIVESTATUS='Y' AND Rowstatus='Y' " +
                       "ORDER BY BankName" + ObjDAL.IsoEnd;
            ObjDAL = new DAL();
            tmpTable = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, strQuery).Tables[0];
            CmbBank.DataSource = tmpTable;
            CmbBank.DataValueField = "Bid";
            CmbBank.DataTextField = "Bank";
            CmbBank.DataBind();
            CmbBank.SelectedIndex = 0;

            TxtBank.Text = CmbBank.SelectedItem.Text;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    public string Validt_SpnsrDtl(string chkby)
    {
        try
        {
            string result = "";

            if (Session["IsGetExtreme"].ToString() == "N")
            {
                if (string.IsNullOrWhiteSpace(txtUplinerId.Text))
                {
                    scrname = "<SCRIPT language='javascript'>alert('Check Placement Id');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Login Error", scrname, false);
                    txtUplinerId.Focus();
                    return "";
                }
            }

            txtRefralId.Text = txtRefralId.Text.Trim().Replace("'", "").Replace("=", "").Replace(";", "");
            txtUplinerId.Text = txtUplinerId.Text.Trim().Replace("'", "").Replace("=", "").Replace(";", "");

            // ---------- VALIDATE SPONSOR ----------
            if (!string.IsNullOrWhiteSpace(txtRefralId.Text))
            {
                try
                {
                    cmd = new SqlCommand(
                        ObjDAL.IsoStart + "SELECT FormNo, MemFirstName + ' ' + MemLastName AS MemName, ActiveStatus " +
                        "FROM " + ObjDAL.dBName + "..M_MemberMaster WHERE Idno='" + txtRefralId.Text + "'" + ObjDAL.IsoEnd,
                        dbConnectselect.cnnObject);

                    dRead = cmd.ExecuteReader();

                    if (!dRead.Read())
                    {
                        scrname = "<SCRIPT language='javascript'>alert('Sponsor ID Not Exist.');</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Login Error", scrname, false);

                        dRead.Close();
                        vsblCtrl(false, true);

                        return "";
                    }
                    else
                    {
                        Session["Kitid"] = 1;
                        Session["Bv"] = 0;
                        Session["JoinStatus"] = "N";
                        Session["RP"] = 0;
                        result = "OK";
                    }

                    Session["Refral"] = dRead["FormNo"].ToString();
                    lblRefralNm.Text = dRead["MemName"].ToString();

                    dRead.Close();
                    cmd.Cancel();
                }
                catch
                {
                    Response.Write("Please check sponsor ID.");
                }
            }
            else
            {
                scrname = "<SCRIPT language='javascript'>alert('Check Sponsor ID.');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Login Error", scrname, false);
                txtRefralId.Focus();
                return "";
            }

            // ---------- VALIDATE UPLINER ----------
            if (Session["IsGetExtreme"].ToString() == "N")
            {
                if (!string.IsNullOrWhiteSpace(txtUplinerId.Text))
                {
                    try
                    {
                        cmd = new SqlCommand(
                            ObjDAL.IsoStart + "SELECT FormNo, MemFirstName + ' ' + MemLastName AS MemName " +
                            "FROM " + ObjDAL.dBName + "..M_MemberMaster WHERE Idno='" + txtUplinerId.Text + "'" + ObjDAL.IsoEnd,
                            dbConnectselect.cnnObject);

                        dRead = cmd.ExecuteReader();

                        if (!dRead.Read())
                        {
                            scrname = "<SCRIPT language='javascript'>alert('Sponsor ID Not Exist');</SCRIPT>";
                            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Login Error", scrname, false);
                            dRead.Close();
                            vsblCtrl(false, true);
                            return "";
                        }

                        Session["Uplnr"] = dRead["FormNo"].ToString();
                        result = "OK";

                        lblUplnrNm.Text = dRead["MemName"].ToString();

                        dRead.Close();
                        cmd.Cancel();
                    }
                    catch
                    {
                        Response.Write("Incorrect Place under ID");
                    }
                }
                else
                {
                    scrname = "<SCRIPT language='javascript'>alert('Check Sponsor ID');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Login Error", scrname, false);

                    return "";
                }
            }
            else
            {
                txtUplinerId.Text = "0";
                lblUplnrNm.Text = "";
                Session["Uplnr"] = "0";
            }

            // ---------- CHECK UPLINER UNDER SPONSOR ----------
            if (Session["IsGetExtreme"].ToString() == "N")
            {
                if (Convert.ToInt32(Session["Refral"]) != Convert.ToInt32(Session["Uplnr"]))
                {
                    cmd = new SqlCommand(
                        ObjDAL.IsoStart + "SELECT * FROM " + ObjDAL.dBName + "..M_MemTreeRelation WHERE FormNo=" + Session["Refral"] + " AND FormNoDwn=" + Session["Uplnr"] + ObjDAL.IsoEnd,
                        dbConnectselect.cnnObject);

                    dRead = cmd.ExecuteReader();

                    if (!dRead.Read())
                    {
                        scrname = "<SCRIPT language='javascript'>alert('Place Under Does Not Exist In Sponsor Downline!!');</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Login Error", scrname, false);
                        dRead.Close();
                        vsblCtrl(false, true);
                        return "";
                    }

                    dRead.Close();
                    cmd.Cancel();
                }
            }

            // ---------- CHECK AVAILABLE LEG ----------
            if (Session["IsGetExtreme"].ToString() == "N")
            {
                if (!checkAvailLeg())
                {
                    vsblCtrl(false, true);
                    return "";
                }
            }
            RbtnLegNo.Enabled = false;
            txtUplinerId.Enabled = false;
            txtRefralId.Enabled = false;
            return result;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
            Response.Write("Try later.");

            return "";
        }
    }
    protected void CmdSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!chkterms.Checked)
            {
                ShowMsg("Please select Terms and Conditions");
                return;
            }

            if (txtRefralId.Text.Trim() == "")
            {
                ShowMsg("Please Enter Sponsor ID.");
                return;
            }

            if (string.IsNullOrEmpty(RbtnLegNo.SelectedValue))
            {
                ShowMsg("Please Select Leg.");
                return;
            }

            if (txtFrstNm.Text.Trim() == "")
            {
                ShowMsg("Please Enter Name.");
                return;
            }

            if (txtFNm.Text.Trim() == "")
            {
                ShowMsg("Please Enter Father Name.");
                return;
            }

            if (txtPinCode.Text.Trim() == "")
            {
                ShowMsg("Please Enter Pincode.");
                return;
            }

            if (txtMobileNo.Text.Trim() == "")
            {
                ShowMsg("Please Enter Mobile No.");
                return;
            }

            if (txtEMailId.Text.Trim() == "")
            {
                ShowMsg("Please Enter E-Mail ID.");
                return;
            }

            if (Session["compid"].ToString() == "1108" || Session["compid"].ToString() == "1110")
            {
                if (TxtPasswd.Text.Trim() == "")
                {
                    ShowMsg("Please Enter Password.");
                    return;
                }

                if (TxtConfirmPasswd.Text.Trim() == "")
                {
                    ShowMsg("Please Enter Confirm Password.");
                    return;
                }

                if (TxtPasswd.Text != TxtConfirmPasswd.Text)
                {
                    ShowMsg("Password and Confirm Password must match.");
                    return;
                }
            }
            if (txtPanNo.Text.Trim() != "")
            {
                if (!IsValidPAN(txtPanNo.Text))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                        "alert('Invalid PAN Number. Format: ABCDE1234F');", true);
                    txtPanNo.Focus();
                    return;
                }
            }

            SaveIntoDB();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }

    }
    private bool IsValidPAN(string panNo)
    {
        return Regex.IsMatch(
            panNo.Trim().ToUpper(),
            @"^[A-Z]{5}[0-9]{4}[A-Z]$"
        );
    }
    private void ShowMsg(string message)
    {
        ScriptManager.RegisterClientScriptBlock(
            Page,
            GetType(),
            "msg",
            "alert('" + message + "');",
            true
        );
    }
    private void FindSession()
    {
        try
        {
            cmd = new SqlCommand(
               ObjDAL.IsoStart + "SELECT TOP 1 SessId AS SessId FROM " + ObjDAL.dBName + "..M_SessnMaster ORDER BY SessID DESC" + ObjDAL.IsoEnd,
                dbConnectselect.cnnObject
            );

            dRead = cmd.ExecuteReader();

            if (dRead.Read())
            {
                Session["SessID"] = dRead["SessID"];
            }
            else
            {
                errMsg.Text = "Session Not Exist. Please Enter New Session.";
                dRead.Close();
                return;
            }

            dRead.Close();
            cmd.Cancel();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private bool checkAvailLeg()
    {
        try
        {
            int iLegNo, iformNo;

            // Determine LEFT / RIGHT leg
            if (RbtnLegNo.SelectedIndex == 0)
                iLegNo = 1;
            else if (RbtnLegNo.SelectedIndex == 1)
                iLegNo = 2;
            else if (RbtnLegNo.SelectedIndex == 2)
                iLegNo = 3;
            else
            {
                scrname = "<SCRIPT language='javascript'>alert('Choose Position.');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Login Error", scrname, false);
                return false;
            }

            // --- GET Upliner Info ---
            cmd = new SqlCommand(
                ObjDAL.IsoStart + "SELECT * FROM " + ObjDAL.dBName + "..M_MemberMaster WHERE IdNo='" + txtUplinerId.Text + "'" + ObjDAL.IsoEnd,
                dbConnectselect.cnnObject
            );

            dRead = cmd.ExecuteReader();

            if (dRead.Read())
            {
                iformNo = Convert.ToInt32(dRead["FormNo"]);
            }
            else
            {
                errMsg.Text = "Check Placeunder Id.";
                scrname = "<SCRIPT language='javascript'>alert('" + errMsg.Text + "');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Login Error", scrname, false);

                dRead.Close();
                return false;
            }

            dRead.Close();

            // --- CHECK IF LEG AVAILABLE ---
            cmd = new SqlCommand(
               ObjDAL.IsoStart + "SELECT COUNT(*) AS CNT FROM " + ObjDAL.dBName + "..M_MemberMaster " +
                "WHERE UpLnFormNo=" + iformNo + " AND LegNo=" + iLegNo + ObjDAL.IsoEnd,
                dbConnectselect.cnnObject
            );

            dRead = cmd.ExecuteReader();

            if (dRead.Read())
            {
                if (Convert.ToInt32(dRead["CNT"]) > 0)
                {
                    errMsg.Text = (iLegNo == 1 ? "Group A" : (iLegNo == 2 ? "Group B" : "Group C"))
               + " Position already used, please select correct Position.!";
                    scrname = "<SCRIPT language='javascript'>alert('" + errMsg.Text + "');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Login Error", scrname, false);
                    dRead.Close();
                    return false;
                }
                else
                {
                    _dblAvailLeg = iformNo;
                    dRead.Close();
                    return true;
                }
            }
            else
            {
                errMsg.Text = "Error In Position Selection.";
                scrname = "<SCRIPT language='javascript'>alert('" + errMsg.Text + "');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Login Error", scrname, false);

                dRead.Close();
                return false;
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
            Response.Write("Try later.");

            return false;
        }
    }
    private void sendSMS()
    {
        try
        {
            string MemberPass = "";
            string MemberTransPassw = "";

            // Read member details
            cmd = new SqlCommand(
               ObjDAL.IsoStart + "SELECT *, DATEADD(day, 15, Doj) AS Maxdate " +
                "FROM " + ObjDAL.dBName + "..M_MemberMaster WHERE IDNo = '" + LastInsertID + "'" + ObjDAL.IsoEnd,
                dbConnectselect.cnnObject
            );

            dRead = cmd.ExecuteReader();

            if (dRead.Read())
            {
                Session["SMSIDNo"] = dRead["IDNo"].ToString();
                Session["SMSIDPass"] = dRead["Passw"].ToString();
                Session["Name"] = dRead["MemFirstName"].ToString();
                Session["MaxDate"] = dRead["MaxDate"];
                Session["SMSTransPassw"] = dRead["EPassw"].ToString();
            }

            dRead.Close();

            // Load passwords
            MemberPass = Session["SMSIDPass"].ToString();
            MemberTransPassw = Session["SMSTransPassw"].ToString();

            // Escape special chars
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

            // Send SMS only if mobile valid
            if (txtMobileNo.Text.Length >= 10 && txtMobileNo.Text.All(char.IsDigit))
            {
                WebClient client = new WebClient();

                // Default message
                string sms = " Congrats! " + Session["Name"] +
                             ", Your Joining has been done Successfully. Username:" +
                             Session["SMSIDNo"] + ", Pwd:" + Session["SMSIDPass"] +
                             ".Login Team MFLMGO";

                // Call internal SendSMS()
                sendsmSs(sms, txtMobileNo.Text, "MFLMGO");

                // Prepare template SMS
                string baseurl = Session["JoiningSms"]
                                    .ToString()
                                    .Replace("{#var1#}", Session["Name"].ToString())
                                    .Replace("{#var2#}", Session["SMSIDNo"].ToString())
                                    .Replace("{#var3#}", Session["SMSIDPass"].ToString())
                                    .Replace("{#var4#}", Session["CompWeb1"].ToString())
                                    .Replace("$MOB$", txtMobileNo.Text);

                try
                {
                    Stream data = client.OpenRead(baseurl);
                    StreamReader reader = new StreamReader(data);
                    string s = reader.ReadToEnd();

                    data.Close();
                    reader.Close();
                }
                catch
                {
                    // Ignore SMS API failure
                }
            }

            ClrCtrl();
            if (dRead != null && !dRead.IsClosed)
                dRead.Close();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    public bool sendsmSs(string sms, string Mobl, string senderid)
    {
        WebClient client = new WebClient();
        string baseurl = "https://www.fast2sms.com/dev/bulkV2?authorization=" + senderid + "&message=" + sms + "&language=english&route=v3&numbers=" + Mobl + "";
        Stream data;

        try
        {
            // baseurl must be constructed as per your API
            data = client.OpenRead(baseurl);

            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();

            data.Close();
            reader.Close();

            return true;
        }
        catch
        {
            // Silent fail, same as VB
            return false;
        }
    }
    private void MaxInvoiceNo()
    {
        try
        {
            strQuery = ObjDAL.IsoStart + "SELECT Max(BillNo) + 1 AS BillNo FROM " + ObjDAL.dBName + "..M_MemberMAster" + ObjDAL.IsoEnd;
            tmpTable = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, strQuery).Tables[0];

            if (tmpTable.Rows.Count > 0)
            {
                DataRow DR = tmpTable.Rows[0];
                InVoiceNo = DR["BillNo"].ToString();
            }

            tmpTable.Clear();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            //MsgBox equivalent is ignored (not available in C# WebForms)
        }
    }
    private void FindPV()
    {
        try
        {
            cmd = new SqlCommand(
               ObjDAL.IsoStart + "SELECT TOP 1 PV FROM " + ObjDAL.dBName + "..M_KitMaster WHERE Kitid = '" + Session["KitId"] + "'" + ObjDAL.IsoEnd,
                dbConnectselect.cnnObject
            );

            dRead = cmd.ExecuteReader();

            if (dRead.Read())
            {
                Session["BV"] = dRead["PV"];
            }
            else
            {
                dRead.Close();
                return;
            }

            dRead.Close();
            cmd.Cancel();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected void txtUplinerId_TextChanged(object sender, EventArgs e)

    {
        FillSponsor();
    }
    private void FillSponsor()
    {
        try
        {
            errMsg.Text = "";
            lblErrEpin.Text = "";
            int i = 0;

            txtUplinerId.Text = txtUplinerId.Text.Trim()
                                                .Replace(";", "")
                                                .Replace("'", "")
                                                .Replace("=", "");

            strQuery = ObjDAL.IsoStart + "SELECT FormNo, MemFirstName + ' ' + MemLastName AS MemName " +
                       "FROM " + ObjDAL.dBName + "..M_MemberMaster WHERE IDNo='" + txtUplinerId.Text + "'" + ObjDAL.IsoEnd;

            cmd = new SqlCommand(strQuery, dbConnectselect.cnnObject);
            dRead = cmd.ExecuteReader();

            if (dRead.Read())
            {
                lblUplnrNm.Text = dRead["MemName"].ToString();
                Session["Uplnr"] = dRead["FormNo"].ToString();
                i++;
            }
            else
            {
                errMsg.Text = "Invalid PlaceUnder ID!!";
                lblErrEpin.Text = "Invalid PlaceUnder ID!!";

                scrname = "<SCRIPT language='javascript'>alert('" + errMsg.Text + "');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Login Error", scrname, false);
            }

            dRead.Close();
            cmd.Cancel();

            if (i == 1)
            {
                checkAvailLeg();
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void FillReferral()
    {
        try
        {
            lblErrEpin.Text = "";
            errMsg.Text = "";

            ObjDAL = new DAL();

            txtRefralId.Text = txtRefralId.Text.Trim()
                                              .Replace(";", "")
                                              .Replace("'", "")
                                              .Replace("=", "");
            strQuery = ObjDAL.IsoStart + "SELECT FormNo, MemFirstName + ' ' + MemLastName AS MemName,activestatus " +
                "FROM " + ObjDAL.dBName + "..M_MemberMaster WHERE IDNo='" + txtRefralId.Text + "' AND IsBlock='N' " + ObjDAL.IsoEnd;
            cmd = new SqlCommand(strQuery, dbConnectselect.cnnObject);
            dRead = cmd.ExecuteReader();
            if (dRead.Read())
            {
                if (Session["compid"].ToString() == "1107")
                {
                    if (dRead["activestatus"].ToString() == "N")
                    {
                        scrname = "<SCRIPT language='javascript'>alert('Unauthorized access. You are not allowed to sponsor.');</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Login Error", scrname, false);
                        txtRefralId.Text = "";
                        lblRefralNm.Text = "";
                        dRead.Close();
                        return;
                    }
                    else
                    {
                        lblRefralNm.Text = dRead["MemName"].ToString();
                    }
                }
                else
                {
                    lblRefralNm.Text = dRead["MemName"].ToString();
                }

            }
            else
            {
                scrname = "<SCRIPT language='javascript'>alert('Invalid Sponsor Id.');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Login Error", scrname, false);

                txtRefralId.Text = "";
                dRead.Close();
                return;
            }

            dRead.Close();
            cmd.Cancel();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected void CmdCancel_Click(object sender, EventArgs e)
    {
        ClrCtrl();
    }
    protected void txtRefralId_TextChanged(object sender, EventArgs e)
    {
        FillReferral();
    }
    protected void TxtPostPincode_TextChanged(object sender, EventArgs e)
    {
        string sql = "";

        if (Convert.ToInt32(TxtPostPincode.Text) != 0)
        {
            sql = ObjDAL.IsoStart + "SELECT a.Statename, b.DistrictName, c.CityName, d.VillageName, d.Pincode, " +
                  "a.StateCode, b.DistrictCode, c.CityCode, d.VillageCode " +
                  "FROM " + ObjDAL.dBName + "..M_StateDivMaster a WITH (NoLock) " +
                  "INNER JOIN " + ObjDAL.dBName + "..M_DistrictMaster b WITH (NoLock) ON a.StateCode = b.StateCode " +
                  "AND a.ActiveStatus = 'Y' AND b.ActiveStatus = 'Y' " +
                  "INNER JOIN " + ObjDAL.dBName + "..M_CityStateMaster c WITH (NoLock) ON b.DistrictCode = c.DistrictCode " +
                  "AND c.ActiveStatus = 'Y' " +
                  "INNER JOIN " + ObjDAL.dBName + "..M_VillageMaster d WITH (NoLock) ON c.CityCode = d.CityCode " +
                  "AND d.ActiveStatus = 'Y' " +
                  "WHERE d.Pincode = '" + Convert.ToInt32(TxtPostPincode.Text) + "' " +
                  "UNION ALL " +
                  "SELECT '' AS StateName, '' AS DistrictName, '' AS CityName, 'Others', '', 0, 0, 0, 381264" + ObjDAL.IsoEnd;

            DataTable Dt = new DataTable();
            ObjDAL = new DAL();
            Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];

            if (Dt.Rows.Count > 0)
            {
                TxtpostState.Text = Dt.Rows[0]["StateName"].ToString();
                HPostStateCode.Value = Dt.Rows[0]["StateCode"].ToString();

                TxtPostDistrict.Text = Dt.Rows[0]["DistrictName"].ToString();
                HPostDistrict.Value = Dt.Rows[0]["DistrictCode"].ToString();

                TxtPostCity.Text = Dt.Rows[0]["CityName"].ToString();
                HPostCity.Value = Dt.Rows[0]["CityCode"].ToString();

                DDlPostVillage.DataSource = Dt;
                DDlPostVillage.DataValueField = "VillageCode";
                DDlPostVillage.DataTextField = "VillageName";
                DDlPostVillage.DataBind();
                DDlPostVillage.SelectedIndex = 0;

                DDlPostVillage.Focus();
            }
            else
            {
                TxtpostState.Text = "";
                HPostStateCode.Value = "0";

                TxtPostCity.Text = "";
                HPostCity.Value = "0";

                TxtPostDistrict.Text = "";
                HPostDistrict.Value = "0";

                DDlPostVillage.Items.Clear();

                TxtPostPincode.Text = "";
                TxtPostPincode.Focus();

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "alert",
                    "alert('Post Pincode Not exist.');", true);
            }
        }
    }
    private void Fill_State()
    {
        try
        {
            ObjDAL = new DAL();
            DataTable dtMaster = new DataTable();
            string str = ObjDAL.IsoStart + "SELECT StateCode, StateName FROM " + ObjDAL.dBName + "..M_StateDivMaster WHERE ActiveStatus = 'Y' AND RowStatus = 'Y' ORDER BY StateName" + ObjDAL.IsoEnd;
            dtMaster = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];
            if (dtMaster.Rows.Count > 0)
            {
                ddlState.DataSource = dtMaster;
                ddlState.DataValueField = "StateCode";
                ddlState.DataTextField = "StateName";
                ddlState.DataBind();
            }
        }
        catch (Exception)
        {
            // optional logging
        }
    }
    protected void txtMobileNo_TextChanged(object sender, EventArgs e)
    {
        // If you want FillMobileNo() here, call it
        // FillMobileNo();
    }
    private void FillMobileNo()
    {
        try
        {
            lblErrEpin.Text = "";
            errMsg.Text = "";

            ObjDAL = new DAL();

            txtMobileNo.Text = txtMobileNo.Text.Trim()
                                               .Replace(";", "")
                                               .Replace("'", "")
                                               .Replace("=", "");

            string sql = ObjDAL.IsoStart + "SELECT Count(phn1) AS phn1 FROM " + ObjDAL.dBName + "..M_Membermaster WHERE phn1='" +
                         txtMobileNo.Text.Trim() + "'" + ObjDAL.IsoEnd;

            DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];

            if (Convert.ToInt32(Dt1.Rows[0]["phn1"]) >= 7)
            {
                CmdSave.Enabled = true;
                chkterms.Checked = false;
                lblMobileNo.Text = "Already Registered by this Mobile.";
                txtMobileNo.Text = "";
                return;
            }
            else
            {
                lblMobileNo.Text = "";
                lblMobileNo.Visible = false;
                return;
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void FillMobileNoProholi()
    {
        try
        {
            lblErrEpin.Text = "";
            errMsg.Text = "";

            ObjDAL = new DAL();

            txtMobileNo.Text = txtMobileNo.Text.Trim()
                                               .Replace(";", "")
                                               .Replace("'", "")
                                               .Replace("=", "");

            string sql = ObjDAL.IsoStart + "SELECT Count(Mobl) AS Mobl FROM " + ObjDAL.dBName + "..M_Membermaster WHERE Mobl='" +
                         txtMobileNo.Text.Trim() + "'" + ObjDAL.IsoEnd;

            DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];

            if (Convert.ToInt32(Dt1.Rows[0]["Mobl"]) >= 3)
            {
                CmdSave.Enabled = true;
                chkterms.Checked = false;
                lblMobileNo.Text = "Already Registered by this Mobile.";
                txtMobileNo.Text = "";
                return;
            }
            else
            {
                lblMobileNo.Text = "";
                lblMobileNo.Visible = false;
                return;
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected void txtEMailId_TextChanged(object sender, EventArgs e)
    {
        // Call if needed:
        // Fillemail();
    }
    private void Fillemail()
    {
        try
        {
            lblErrEpin.Text = "";
            errMsg.Text = "";

            ObjDAL = new DAL();

            txtEMailId.Text = txtEMailId.Text.Trim()
                                             .Replace(";", "")
                                             .Replace("'", "")
                                             .Replace("=", "");

            string sql = ObjDAL.IsoStart + "SELECT Count(email) AS email FROM " + ObjDAL.dBName + "..M_Membermaster WHERE email='" +
                         txtEMailId.Text.Trim() + "'" + ObjDAL.IsoEnd;

            DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];

            if (Convert.ToInt32(Dt1.Rows[0]["email"]) >= 7)
            {
                CmdSave.Enabled = true;
                chkterms.Checked = false;
                lblemail.Text = "Already Registered by this email id.";
                txtEMailId.Text = "";
                return;
            }
            else
            {
                lblemail.Text = "";
                lblemail.Visible = false;
                return;
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void FillemailProholi()
    {
        try
        {
            lblErrEpin.Text = "";
            errMsg.Text = "";

            ObjDAL = new DAL();

            txtEMailId.Text = txtEMailId.Text.Trim()
                                             .Replace(";", "")
                                             .Replace("'", "")
                                             .Replace("=", "");

            string sql = ObjDAL.IsoStart + "SELECT Count(email) AS email FROM " + ObjDAL.dBName + "..M_Membermaster WHERE email='" + txtEMailId.Text.Trim() + "'" + ObjDAL.IsoEnd;
            DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];
            if (Convert.ToInt32(Dt1.Rows[0]["email"]) >= 3)
            {
                CmdSave.Enabled = true;
                chkterms.Checked = false;
                lblemail.Text = "Already Registered by this email id.";
                txtEMailId.Text = "";
                return;
            }
            else
            {
                lblemail.Text = "";
                lblemail.Visible = false;
                return;
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected void txtPinCode_TextChanged(object sender, EventArgs e)
    {
        if (!Regex.IsMatch(txtPinCode.Text, "^[1-9][0-9]{5}$"))
        {
            scrname = "<SCRIPT language='javascript'>alert('Invalid Pin Code.');</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Login Error", scrname, false);
            return;
        }
    }
    private string Checkuserid(string userid)
    {
        string sql = ObjDAL.IsoStart + "SELECT idno, (MemFirstName + ' ' + MemLastName) AS MemberName, formno " +
                     "FROM " + ObjDAL.dBName + "..M_MemberMaster WHERE Idno = '" + userid.Trim() + "'" + ObjDAL.IsoEnd;

        DataTable Dt = SqlHelper.ExecuteDataset((string)HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]], CommandType.Text,
                           sql
                       ).Tables[0];

        if (Dt.Rows.Count > 0)
        {
            return "Faild";  // Keeping original typo
        }
        else
        {
            return "Ok";
        }
    }
    protected void TxtUserName_TextChanged(object sender, EventArgs e)
    {

        if (Session["compid"] != null && Session["compid"].ToString() == "1088" && Session["CompID"].ToString() == "1108" || Session["compid"].ToString() == "1107" || Session["compid"].ToString() == "1109" || Session["compid"].ToString() == "1110")
        {
            if (string.IsNullOrWhiteSpace(TxtUserName.Text))
            {
                chkterms.Enabled = true;
                CmdSave.Enabled = true;

                LblUsername.Visible = true;
                LblUsername.Text = "Enter User Name.";
                return;
            }
            else
            {
                LblUsername.Visible = false;
            }

            if (TxtUserName.Text == "0")
            {
                chkterms.Enabled = true;
                CmdSave.Enabled = true;

                LblUsername.Visible = true;
                LblUsername.Text = "Enter Valid User Name.";
                return;
            }
            else
            {
                LblUsername.Visible = false;
            }
        }

        if (!string.IsNullOrWhiteSpace(TxtUserName.Text))
        {
            if (Checkuserid(TxtUserName.Text) != "Ok")
            {
                LblUsername.Text = "Your ID. already registered.!";
                LblUsername.Visible = true;
                TxtUserName.Text = "";
            }
            else
            {
                LblUsername.Visible = false;
            }
        }
    }
    protected void txtFrstNm_TextChanged(object sender, EventArgs e)
    {
        string s1 = "";

        if (Session["CompId"] != null && Session["CompId"].ToString() == "1074")
        {
            if (!string.IsNullOrWhiteSpace(txtFrstNm.Text))
            {
                s1 = ObjDAL.IsoStart + "SELECT Count(MemFirstName) AS MemFirstName " +
                     "FROM " + ObjDAL.dBName + "..M_Membermaster " +
                     "WHERE MemFirstName='" + txtFrstNm.Text.Trim() + "'" + ObjDAL.IsoEnd;

                ObjDAL = new DAL();
                DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s1).Tables[0];

                if (Convert.ToInt32(Dt1.Rows[0]["MemFirstName"]) >= 3)
                {
                    CmdSave.Enabled = true;
                    chkterms.Checked = false;

                    lblemail.Visible = true;
                    lblmemmsg.Text = "Already Registered by this Name other id.";

                    txtFrstNm.Text = "";
                    return;
                }
                else
                {
                    lblmemmsg.Visible = false;
                }
            }
        }
    }
    public bool SendToMemberMail(string IdNo, string Email, string MemberName, string Password)
    {
        try
        {
            string StrMsg = "";
            var SendFrom = new MailAddress(Session["CompMail"].ToString());
            var SendTo = new MailAddress(Email);
            var MyMessage = new MailMessage(SendFrom, SendTo);

            StrMsg = @"
<!DOCTYPE html>
<html>
<head>
  <meta charset='UTF-8' />
  <meta name='viewport' content='width=device-width, initial-scale=1.0' />
</head>
<body style='margin:0; padding:0; background-color:#f0f2f8; font-family:Segoe UI, Arial, sans-serif;'>

  <table width='100%' cellpadding='0' cellspacing='0' style='background-color:#f0f2f8; padding:24px 0;'>
    <tr>
      <td align='center'>
        <table width='600' cellpadding='0' cellspacing='0'
               style='background:#fff; border:1px solid #e0e0e0; border-radius:16px;
                      overflow:hidden; box-shadow:0 4px 24px rgba(0,0,0,0.09); max-width:600px;'>

          <!-- HEADER -->
          <tr>
            <td style='background:linear-gradient(135deg,#7B5A1E 0%,#b8860b 55%,#d4a843 100%);
                       padding:22px 28px;'>
              <table width='100%' cellpadding='0' cellspacing='0'>
                <tr>
                  <td style='vertical-align:middle;'>
                    <p style='margin:0; color:#fff; font-size:22px; font-weight:700;'>Welcome and Congratulations!</p>
                    <p style='margin:4px 0 0; color:rgba(255,255,255,0.78); font-size:13px;'>Enrollment Confirmation</p>
                    <p style='margin:6px 0 0; color:rgba(255,255,255,0.6); font-size:11px;'>
                      Letter No: EMC / " + IdNo + @" / " + DateTime.Now.Year + @"
                    </p>
                  </td>
                </tr>
              </table>
            </td>
          </tr>

          <!-- BODY -->
          <tr>
            <td style='padding:26px 30px;'>

              <p style='margin:0 0 14px; font-size:14px; font-weight:700; color:#b71c1c;'>
                Dear " + MemberName + @",
              </p>

              <p style='margin:0 0 11px; font-size:14px; color:#444; line-height:1.8; text-align:justify;'>
                We are <span style='font-weight:700; color:#b75555;'>" + Session["CompName"] + @"</span>,
                pleased to welcome you as our new client and take this opportunity to extend our warm greetings to you.
              </p>
              <p style='margin:0 0 11px; font-size:14px; color:#444; line-height:1.8; text-align:justify;'>
                Congratulations! You have successfully registered with us.
                We assure you that you will find it enjoyable and professionally beneficial to avail our services.
              </p>
              <p style='margin:0 0 11px; font-size:14px; color:#444; line-height:1.8; text-align:justify;'>
                We, at <span style='font-weight:700; color:#b75555;'>" + Session["CompName"] + @"</span>,
                respect the concern of our clients and observe the highest degree of corporate ethics.
                Our representatives are always available 24×7 with dedication to address all your concerns.
              </p>

              <!-- DIVIDER -->
              <hr style='height:1px; background:linear-gradient(90deg,transparent,#b8860b,transparent);
                         border:none; margin:20px 0 16px; opacity:0.5;' />
              <p style='margin:0 0 13px; font-size:11px; font-weight:700; letter-spacing:1.4px;
                        color:#b8860b; text-transform:uppercase;'>Enrollment Details</p>

              <!-- DETAILS TABLE -->
              <table width='100%' cellpadding='0' cellspacing='0'
                     style='border-collapse:collapse;'>
                <tr style='border-bottom:1px solid #f2f2f2;'>
                  <td style='padding:11px 8px; font-size:13px; color:#888; width:44%;'>
                    <span style='display:inline-block; width:7px; height:7px; border-radius:50%;
                                 background:#b8860b; margin-right:8px; vertical-align:middle;'></span>
                    ID No.
                  </td>
                  <td style='padding:11px 8px; font-size:14px; font-weight:600; color:#b71c1c;'>
                    " + IdNo + @"
                  </td>
                </tr>
                <tr style='border-bottom:1px solid #f2f2f2;'>
                  <td style='padding:11px 8px; font-size:13px; color:#888;'>
                    <span style='display:inline-block; width:7px; height:7px; border-radius:50%;
                                 background:#b8860b; margin-right:8px; vertical-align:middle;'></span>
                    Password
                  </td>
                  <td style='padding:11px 8px; font-size:14px; font-weight:600; color:#b71c1c;'>
                    " + Password + @"
                  </td>
                </tr>
                <tr>
                  <td style='padding:11px 8px; font-size:13px; color:#888;'>
                    <span style='display:inline-block; width:7px; height:7px; border-radius:50%;
                                 background:#b8860b; margin-right:8px; vertical-align:middle;'></span>
                    Login URL
                  </td>
                  <td style='padding:11px 8px; font-size:14px; font-weight:600;'>
                    <a href='" + Session["CompWeb"] + @"'
                       style='color:#b8860b; text-decoration:underline;'>
                      " + Session["CompWeb"] + @"
                    </a>
                  </td>
                </tr>
              </table>

            </td>
          </tr>

          <!-- SIGNATURE -->
          <tr>
            <td style='padding:6px 30px 22px;'>
              <span style='display:block; font-size:12px; color:#aaa; margin-bottom:4px;'>Warm Regards,</span>
              <span style='display:block; font-size:14px; font-weight:600; color:#333;'>CMD</span>
              <span style='display:block; font-size:15px; font-weight:700; color:#b8860b;'>
                " + Session["CompName"] + @"
              </span>
            </td>
          </tr>

          <!-- FOOTER -->
          <tr>
            <td style='background:#fafaf7; padding:13px 30px; border-top:1px solid #eeece0;'>
              <table width='100%' cellpadding='0' cellspacing='0'>
                <tr>
                  <td style='font-size:12px; color:#aaa;'>
                    &copy; <strong>" + Session["CompName"] + @"</strong> &nbsp;|&nbsp; Available 24×7
                  </td>
                  <td align='right'>
                    <span style='font-size:11px; font-weight:700; background:#fef3c7; color:#92400e;
                                 border-radius:20px; padding:4px 14px; border:1px solid #d97706;
                                 white-space:nowrap;'>
                      Active Member
                    </span>
                  </td>
                </tr>
              </table>
            </td>
          </tr>

        </table>
      </td>
    </tr>
  </table>

</body>
</html>";

            MyMessage.Subject = "Welcome and Congratulations!";
            MyMessage.Body = StrMsg;
            MyMessage.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(Session["MailHost"].ToString())
            {
                UseDefaultCredentials = false,
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(Session["CompMail"].ToString(), Session["MailPass"].ToString())
            };
            smtp.Send(MyMessage);
            return true;
        }
        catch
        {
            return false;
        }

    }
    public bool SendToMemberMailcashless(string IdNo, string Email, string MemberName, string Password)
    {
        try
        {
            string StrMsg = "";

            var SendFrom = new MailAddress(Session["CompMail"].ToString());
            var SendTo = new MailAddress(Email);
            var MyMessage = new MailMessage(SendFrom, SendTo);

            StrMsg =
                "<table style=\"margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; " +
                "line-height:23px; text-align:justify; width:100%; color:black;\">" +
                "<tr><td>" +
                "<span style=\"font-weight: bold;\"><h2>Dear " + MemberName + ",</h2></span>" +
                "Congratulations! You have successfully registered with " + Session["CompName"] + ".<br />" +
                "Your username and password are given below:<br />" +
                "<strong>User ID: " + IdNo + "</strong><br />" +
                "<strong>Password: " + Password + "</strong><br />" +
                "You may login at: <a href=\"" + Session["CompWeb"] + "\" target=\"_blank\" " +
                "style=\"color:#0000FF; text-decoration:underline;\">" + Session["CompWeb"] + "</a><br />" +
                "<span style=\"color: #0099FF; font-weight: bold;\">Regards,</span><br />" +
                "<a href=\"" + Session["CompWeb"] + "\" target=\"_blank\" style=\"color:#0000FF; text-decoration:underline;\">" +
                Session["CompName"] + "</a><br /><br /><br /></td></tr></table>";

            MyMessage.Subject = "Welcome and Congratulations!";
            MyMessage.Body = StrMsg;
            MyMessage.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(Session["MailHost"].ToString())
            {
                UseDefaultCredentials = false,
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(Session["CompMail"].ToString(), Session["MailPass"].ToString())
            };

            smtp.Send(MyMessage);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public bool SendToMemberMailzaradobit(string IdNo, string Email, string MemberName, string Password)
    {
        try
        {
            string StrMsg = "";

            var SendFrom = new MailAddress(Session["CompMail"].ToString());
            var SendTo = new MailAddress(Email);
            var MyMessage = new MailMessage(SendFrom, SendTo);

            StrMsg =
                "<table style=\"margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; " +
                "line-height:23px; text-align:justify; width:100%; color:black;\">" +
                "<tr><td>" +
                "<span style=\"font-weight: bold;\"><h2>Dear " + MemberName + ",</h2></span>" +
                "Congratulations! You have successfully registered with " + Session["CompName"] + ".<br />" +
                "Your username and password are given below:<br />" +
                "<strong>User ID: " + IdNo + "</strong><br />" +
                "<strong>Password: " + Password + "</strong><br />" +
                "You may login here: <a href='https://www.cpanel.zaradobits.in/' target=\"_blank\" " +
                "style=\"color:#0000FF; text-decoration:underline;\">https://www.cpanel.zaradobits.in/</a><br />" +
                "<span style=\"color: #0099FF; font-weight: bold;\">Regards,</span><br />" +
                "<a href=\"" + Session["CompWeb"] + "\" target=\"_blank\" style=\"color:#0000FF; text-decoration:underline;\">" +
                Session["CompName"] + "</a><br /><br /><br /></td></tr></table>";

            MyMessage.Subject = "Welcome and Congratulations!";
            MyMessage.Body = StrMsg;
            MyMessage.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(Session["MailHost"].ToString())
            {
                UseDefaultCredentials = false,
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(Session["CompMail"].ToString(), Session["MailPass"].ToString())
            };

            smtp.Send(MyMessage);
            return true;
        }
        catch
        {
            return false;
        }
    }
    private void ClrCtrl()
    {
        //txtAddLn2.Text = "";
        txtAddLn1.Text = "";
        txtEMailId.Text = "";
        txtFNm.Text = "";
        txtFrstNm.Text = "";
        txtMobileNo.Text = "";
        txtNominee.Text = "";
        txtPanNo.Text = "";
        txtPhNo.Text = "";
        txtPinCode.Text = "";
        txtRelation.Text = "";
        txtUplinerId.Text = "";
        lblUplnrNm.Text = "";
        ddlDistrict.Text = "";
        ddlTehsil.Text = "";
        TxtBranchName.Text = "";
        TxtAccountNo.Text = "";
        txtIfsCode.Text = "";
        //txtPIN.Text = "";
        //txtScratch.Text = "";
        txtRefralId.Text = "";
        lblRefralNm.Text = "";
        //dv_Main.Visible = false;

        txtUplinerId.Enabled = true;
        txtRefralId.Enabled = true;
        //txtPIN.Enabled = true;
        //txtScratch.Enabled = true;
        //cmdNext.Visible = true;

        RbtnLegNo.Enabled = true;
    }
    public bool checkpanno(string panno)
    {
        string sql = "";
        DAL obj = new DAL();
        DataTable dt = new DataTable();
        sql = ObjDAL.IsoStart + "SELECT COUNT(Panno) AS Panno FROM " + ObjDAL.dBName + "..M_Membermaster WHERE Panno='" + panno.Trim() + "'" + ObjDAL.IsoEnd;
        dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];
        if (dt.Rows.Count > 0)
        {
            if (Convert.ToInt32(dt.Rows[0]["Panno"]) > 3)
                return false;
            else
                return true;
        }

        return true; // default if no row returned
    }
    private void FUND_LOGIN_CHECK(string userid, string passwd, string formno)
    {
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');

        sResult = formatted_datetime;

        string postData = "";
        string URL = "";
        string Str = "";

        try
        {
            string referCode = userid;
            string password = passwd;

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;   // TLS 1.2

            URL = "http://holidayapi1.bisplindia.in/api/login";

            WebRequest tRequest = WebRequest.Create(URL);
            tRequest.Method = "POST";
            tRequest.ContentType = "application/json";
            tRequest.ContentLength = 0;

            postData = "{\"userName\":\"" + referCode + "\",\"password\":\"" + password + "\",\"companyId\":\"2989\"}";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            string sql_req = "INSERT INTO Tbl_ApiRequest_Response(ReqID, Formno, Request, postdata, ForType) " +
                             "VALUES ('" + sResult + "', '" + Convert.ToInt32(formno) + "', '" + URL + "', '" + postData + "', 'HOLIDAYAPILOGIN')";

            int x_Req = Convert.ToInt32(
                SqlHelper.ExecuteNonQuery(
                    (string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]],
                    CommandType.Text,
                    sql_req
                )
            );

            tRequest.ContentLength = byteArray.Length;

            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();

            dataStream = tResponse.GetResponseStream();
            StreamReader tReader = new StreamReader(dataStream);

            Str = tReader.ReadToEnd();

            string sql_res = "UPDATE Tbl_ApiRequest_Response SET Response = '" + Str.Trim() + "' " +
                             "WHERE ReqID = '" + sResult.Trim() + "' AND ForType = 'HOLIDAYAPILOGIN'";

            int x_res = Convert.ToInt32(
                SqlHelper.ExecuteNonQuery(
                    (string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]],
                    CommandType.Text,
                    sql_res
                )
            );
        }
        catch (Exception ex)
        {
            string sql_res = "UPDATE Tbl_ApiRequest_Response SET Response = '" + ex.Message.Replace("'", "''") + "' " +
                             "WHERE ReqID = '" + sResult.Trim() + "' AND ForType = 'HOLIDAYAPILOGIN'";

            int x_res = Convert.ToInt32(
                SqlHelper.ExecuteNonQuery(
                    (string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]],
                    CommandType.Text,
                    sql_res
                )
            );
        }
    }
    public void SaveIntoDB()
    {
        try
        {
            // Local vars
            char IsPanCard = 'N';
            string strQry = "";
            string strDOB = "", strDOM = "", strDOJ = "", s = "";
            int iLeg = 0;
            char cGender = 'M';
            char cMarried = 'N';
            string HostIp = Context.Request.UserHostAddress.ToString();
            string DistrictCode = "0", CityCode = "0", VillageCode = "0";

            // Early checks
            if (Session["ActiveStatusJoin"] != null && Session["ActiveStatusJoin"].ToString() == "N")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key",
                    "alert('Please contact to admin.!');location.replace('Logout.aspx');", true);
                return;
            }

            if (!Regex.IsMatch(txtPinCode.Text, "^[1-9][0-9]{5}$"))
            {
                string scrname = "<SCRIPT language='javascript'>alert('Invalid Pin Code.');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }

            // Validate sponsor/upline and inputs
            if (Validt_SpnsrDtl("") == "OK")
            {
                if (RbtnLegNo.SelectedIndex == 0)
                    iLeg = 1;
                else if (RbtnLegNo.SelectedIndex == 1)
                    iLeg = 2;
                else if (RbtnLegNo.SelectedIndex == 2)
                    iLeg = 3;
                else
                {
                    chkterms.Checked = false;
                    CmdSave.Enabled = true;
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert",
                        "alert('Choose Position.');", true);
                    RbtnLegNo.Enabled = true;
                    return;
                }

                // PAN selection
                if (RbtPan.SelectedValue == "Y")
                {
                    IsPanCard = 'Y';
                    if (string.IsNullOrWhiteSpace(txtPanNo.Text))
                    {
                        chkterms.Enabled = true;
                        CmdSave.Enabled = true;
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert",
                            "alert('Enter PAN No.');", true);
                        return;
                    }
                }
                else if (RbtPan.SelectedValue == "N")
                {
                    IsPanCard = 'N';
                }
                else
                {
                    chkterms.Enabled = true;
                    CmdSave.Enabled = true;
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert",
                        "alert('Choose PAN No Available Or Not.');", true);
                    return;
                }
                if (Session["compid"] != null && Session["compid"].ToString() == "1107")
                {
                    if (string.IsNullOrWhiteSpace(TxtUserName.Text))
                    {
                        chkterms.Enabled = true;
                        CmdSave.Enabled = true;
                        LblUsername.Visible = true;
                        LblUsername.Text = "Enter User Name.";
                        return;
                    }
                    else
                    {
                        LblUsername.Visible = false;
                    }

                    if (TxtUserName.Text == "0")
                    {
                        chkterms.Enabled = true;
                        CmdSave.Enabled = true;
                        LblUsername.Visible = true;
                        LblUsername.Text = "Enter Valid User Name.";
                        return;
                    }
                    else
                    {
                        LblUsername.Visible = false;
                    }
                }
                // Generate password
                if (Session["CompID"].ToString() != "1108" || Session["compid"].ToString() == "1110")
                {
                    TxtPasswd.Text = GenerateRandomString(6);
                }

                string s1 = "";

                // CompID = 1057 extra checks
                if (Session["CompID"] != null && Session["CompID"].ToString() == "1057")
                {
                    if (string.IsNullOrWhiteSpace(txtEMailId.Text))
                    {
                        chkterms.Enabled = true;
                        CmdSave.Enabled = true;
                        lblemail.Visible = true;
                        lblemail.Text = "Enter E mail id.";
                        return;
                    }
                    else
                    {
                        lblemail.Visible = false;
                    }

                    if (string.IsNullOrWhiteSpace(txtPanNo.Text))
                    {
                        chkterms.Enabled = true;
                        CmdSave.Enabled = true;
                        lblpan.Visible = true;
                        lblpan.Text = "Enter Pan no.";
                        return;
                    }
                    else
                    {
                        lblpan.Visible = false;
                    }
                }

                // CompID = 1088 mobile & email uniqueness checks
                if (Session["CompID"] != null && Session["CompID"].ToString() == "1088")
                {
                    if (!string.IsNullOrWhiteSpace(txtMobileNo.Text))
                    {
                        string SqlMobileCheck = ObjDAL.IsoStart + "select Count(mobl) as mobileno from " + ObjDAL.dBName + "..M_Membermaster where Mobl='" + txtMobileNo.Text.Trim() + "'" + ObjDAL.IsoEnd;
                        ObjDAL = new DAL();
                        DataTable Dt_MobileCheck = SqlHelper.ExecuteDataset(
                            (string)HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]],
                            CommandType.Text,
                            SqlMobileCheck).Tables[0];

                        if (Convert.ToInt32(Dt_MobileCheck.Rows[0]["mobileno"]) >= 1)
                        {
                            CmdSave.Enabled = true;
                            chkterms.Checked = false;
                            lblMobileNo.Visible = true;
                            lblMobileNo.Text = "Already Registerd by this Mobile Number.";
                            return;
                        }
                        else
                        {
                            lblMobileNo.Visible = false;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(txtEMailId.Text))
                    {
                        string SqlEmailCheck = ObjDAL.IsoStart + "select Count(email) as email from " + ObjDAL.dBName + "..M_Membermaster where email = '" + txtEMailId.Text.Trim() + "'" + ObjDAL.IsoEnd;
                        ObjDAL = new DAL();
                        DataTable Dt_EmailCheck = SqlHelper.ExecuteDataset(
                            (string)HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]],
                            CommandType.Text,
                            SqlEmailCheck).Tables[0];

                        if (Convert.ToInt32(Dt_EmailCheck.Rows[0]["email"]) >= 1)
                        {
                            CmdSave.Enabled = true;
                            chkterms.Checked = false;
                            lblemail.Visible = true;
                            lblemail.Text = "Already Registerd by this Email ID.";
                            return;
                        }
                        else
                        {
                            lblemail.Visible = false;
                        }
                    }
                }

                // Email must not be empty
                if (string.IsNullOrWhiteSpace(txtEMailId.Text))
                {
                    CmdSave.Enabled = true;
                    chkterms.Checked = false;
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error",
                        "<SCRIPT language='javascript'>alert('Please Enter Email ID  ');</SCRIPT>", false);
                    return;
                }

                // PAN counts checks
                if (!string.IsNullOrWhiteSpace(txtPanNo.Text))
                {
                    s1 = ObjDAL.IsoStart + "select Count(Panno) as PanNo from " + ObjDAL.dBName + "..M_Membermaster where Panno='" + txtPanNo.Text.Trim() + "'" + ObjDAL.IsoEnd;
                    ObjDAL = new DAL();
                    DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s1).Tables[0];
                    if (Convert.ToInt32(Dt1.Rows[0]["Panno"]) >= 11)
                    {
                        CmdSave.Enabled = true;
                        chkterms.Checked = false;
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error",
                            "<SCRIPT language='javascript'>alert('Already Registerd by this panno.');</SCRIPT>", false);
                        return;
                    }
                }

                // CompID specific PAN checks
                if (Session["CompID"] != null && Session["CompID"].ToString() == "1057")
                {
                    if (!string.IsNullOrWhiteSpace(txtPanNo.Text))
                    {
                        s1 = ObjDAL.IsoStart + "select Count(Panno) as PanNo from " + ObjDAL.dBName + "..M_Membermaster where Panno='" + txtPanNo.Text.Trim() + "'" + ObjDAL.IsoEnd;
                        ObjDAL = new DAL();
                        DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s1).Tables[0];
                        if (Convert.ToInt32(Dt1.Rows[0]["Panno"]) >= 3)
                        {
                            CmdSave.Enabled = true;
                            chkterms.Checked = false;
                            lblpan.Visible = true;
                            lblpan.Text = "Panno already exist in 3 another ids.";
                            return;
                        }
                        else
                        {
                            lblpan.Visible = false;
                        }
                    }
                }

                if (Session["CompID"] != null && Session["CompID"].ToString() == "1074")
                {
                    if (!string.IsNullOrWhiteSpace(txtPanNo.Text))
                    {
                        s1 = ObjDAL.IsoStart + "select Count(Panno) as PanNo from " + ObjDAL.dBName + "..M_Membermaster where Panno='" + txtPanNo.Text.Trim() + "'" + ObjDAL.IsoEnd;
                        ObjDAL = new DAL();
                        DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s1).Tables[0];
                        if (Convert.ToInt32(Dt1.Rows[0]["Panno"]) >= 3)
                        {
                            CmdSave.Enabled = true;
                            chkterms.Checked = false;
                            lblpan.Visible = true;
                            lblpan.Text = "Panno already exist in 3 another ids.";
                            return;
                        }
                        else
                        {
                            lblpan.Visible = false;
                        }
                    }
                }

                if (Session["CompID"] != null &&
                    (Session["CompID"].ToString() == "1095" ||
                     Session["CompID"].ToString() == "1097" ||
                     Session["CompID"].ToString() == "1100" || Session["CompID"].ToString() == "1102" || Session["CompID"].ToString() == "1106" || Session["CompID"].ToString() == "1105" || Session["CompID"].ToString() == "1108" || Session["CompID"].ToString() == "1107" || Session["compid"].ToString() == "1109" || Session["compid"].ToString() == "1110"))
                {
                    if (!string.IsNullOrWhiteSpace(txtPanNo.Text))
                    {
                        s1 = ObjDAL.IsoStart + "select Count(Panno) as PanNo from " + ObjDAL.dBName + "..M_Membermaster where Panno='" + txtPanNo.Text.Trim() + "'" + ObjDAL.IsoEnd;
                        ObjDAL = new DAL();
                        DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s1).Tables[0];
                        if (Convert.ToInt32(Dt1.Rows[0]["Panno"]) >= 1)
                        {
                            CmdSave.Enabled = true;
                            chkterms.Checked = false;
                            lblpan.Visible = true;
                            lblpan.Text = "Already Registerd by this PanNo Number.";
                            return;
                        }
                        else
                        {
                            lblpan.Visible = false;
                        }
                    }
                }

                if (Session["CompID"] != null && Session["CompID"].ToString() == "1091")
                {
                    if (!string.IsNullOrWhiteSpace(txtPanNo.Text))
                    {
                        s1 = ObjDAL.IsoStart + "select Count(Panno) as PanNo from " + ObjDAL.dBName + "..M_Membermaster where Panno='" + txtPanNo.Text.Trim() + "'" + ObjDAL.IsoEnd;
                        ObjDAL = new DAL();
                        DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s1).Tables[0];
                        if (Convert.ToInt32(Dt1.Rows[0]["Panno"]) >= 1)
                        {
                            CmdSave.Enabled = true;
                            chkterms.Checked = false;
                            lblpan.Visible = true;
                            lblpan.Text = "Already Registerd by this PanNo Number.";
                            return;
                        }
                        else
                        {
                            lblpan.Visible = false;
                        }
                    }
                }
                if (Session["CompID"] != null && Session["CompID"].ToString() == "1108" && Session["CompID"].ToString() == "1110")
                {
                    if (!string.IsNullOrWhiteSpace(txtMobileNo.Text))
                    {
                        s1 = ObjDAL.IsoStart + "select Count(mobl) as mobileno from " + ObjDAL.dBName + "..M_Membermaster where Mobl='" + txtMobileNo.Text.Trim() + "'" + ObjDAL.IsoEnd;
                        ObjDAL = new DAL();
                        DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s1).Tables[0];
                        if (Convert.ToInt32(Dt1.Rows[0]["mobileno"]) >= 100000)
                        {
                            CmdSave.Enabled = true;
                            chkterms.Checked = false;
                            lblMobileNo.Visible = true;
                            lblMobileNo.Text = "Already Registerd by this Mobile Number.";
                            return;
                        }
                        else
                        {
                            lblMobileNo.Visible = false;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(txtEMailId.Text))
                    {
                        s1 = ObjDAL.IsoStart + "select Count(Email) as Email from " + ObjDAL.dBName + "..M_Membermaster where Email='" + txtEMailId.Text.Trim() + "'" + ObjDAL.IsoEnd;
                        ObjDAL = new DAL();
                        DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s1).Tables[0];
                        if (Convert.ToInt32(Dt1.Rows[0]["Email"]) >= 1000000)
                        {
                            CmdSave.Enabled = true;
                            chkterms.Checked = false;
                            lblemail.Visible = true;
                            lblemail.Text = "Already Registerd by this E mail id.";
                            return;
                        }
                        else
                        {
                            lblemail.Visible = true;
                        }
                    }
                }
                if (Session["CompID"] != null && Session["CompID"].ToString() == "1107" || Session["compid"].ToString() == "1109")
                {
                    if (!string.IsNullOrWhiteSpace(txtMobileNo.Text))
                    {
                        s1 = ObjDAL.IsoStart + "select Count(mobl) as mobileno from " + ObjDAL.dBName + "..M_Membermaster where Mobl='" + txtMobileNo.Text.Trim() + "'" + ObjDAL.IsoEnd;
                        ObjDAL = new DAL();
                        DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s1).Tables[0];
                        if (Convert.ToInt32(Dt1.Rows[0]["mobileno"]) >= 1)
                        {
                            CmdSave.Enabled = true;
                            chkterms.Checked = false;
                            lblMobileNo.Visible = true;
                            lblMobileNo.Text = "Already Registerd by this Mobile Number.";
                            return;
                        }
                        else
                        {
                            lblMobileNo.Visible = false;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(txtEMailId.Text))
                    {
                        s1 = ObjDAL.IsoStart + "select Count(Email) as Email from " + ObjDAL.dBName + "..M_Membermaster where Email='" + txtEMailId.Text.Trim() + "'" + ObjDAL.IsoEnd;
                        ObjDAL = new DAL();
                        DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s1).Tables[0];
                        if (Convert.ToInt32(Dt1.Rows[0]["Email"]) >= 1)
                        {
                            CmdSave.Enabled = true;
                            chkterms.Checked = false;
                            lblemail.Visible = true;
                            lblemail.Text = "Already Registerd by this E mail id.";
                            return;
                        }
                        else
                        {
                            lblemail.Visible = true;
                        }
                    }
                }

                if (Session["CompID"] != null && Session["CompID"].ToString() == "1057")
                {
                    if (!string.IsNullOrWhiteSpace(txtMobileNo.Text))
                    {
                        s1 = ObjDAL.IsoStart + "select Count(mobl) as mobileno from " + ObjDAL.dBName + "..M_Membermaster where Mobl='" + txtMobileNo.Text.Trim() + "'" + ObjDAL.IsoEnd;
                        ObjDAL = new DAL();
                        DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s1).Tables[0];
                        if (Convert.ToInt32(Dt1.Rows[0]["mobileno"]) >= 1000)
                        {
                            CmdSave.Enabled = true;
                            chkterms.Checked = false;
                            lblMobileNo.Visible = true;
                            lblMobileNo.Text = "Already Registerd by this Mobile Number.";
                            return;
                        }
                        else
                        {
                            lblMobileNo.Visible = false;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(txtEMailId.Text))
                    {
                        s1 = ObjDAL.IsoStart + "select Count(Email) as Email from " + ObjDAL.dBName + "..M_Membermaster where Email='" + txtEMailId.Text.Trim() + "'" + ObjDAL.IsoEnd;
                        ObjDAL = new DAL();
                        DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s1).Tables[0];
                        if (Convert.ToInt32(Dt1.Rows[0]["Email"]) >= 1000)
                        {
                            CmdSave.Enabled = true;
                            chkterms.Checked = false;
                            lblemail.Visible = true;
                            lblemail.Text = "Already Registerd by this E mail id.";
                            return;
                        }
                        else
                        {
                            lblemail.Visible = true;
                        }
                    }
                }

                // CompID 1060 checks
                if (Session["CompID"] != null && Session["CompID"].ToString() == "1060")
                {
                    if (!string.IsNullOrWhiteSpace(txtMobileNo.Text))
                    {
                        s1 = ObjDAL.IsoStart + "select Count(mobl) as mobileno from " + ObjDAL.dBName + "..M_Membermaster where Mobl='" + txtMobileNo.Text.Trim() + "'" + ObjDAL.IsoEnd;
                        ObjDAL = new DAL();
                        DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s1).Tables[0];
                        if (Convert.ToInt32(Dt1.Rows[0]["mobileno"]) >= 10)
                        {
                            CmdSave.Enabled = true;
                            chkterms.Checked = false;
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error",
                                "<SCRIPT language='javascript'>alert('Already Registerd by this Mobile Number.');</SCRIPT>", false);
                            return;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(txtEMailId.Text))
                    {
                        s1 = ObjDAL.IsoStart + "select Count(Email) as Email from " + ObjDAL.dBName + "..M_Membermaster where Email='" + txtEMailId.Text.Trim() + "'" + ObjDAL.IsoEnd;
                        ObjDAL = new DAL();
                        DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s1).Tables[0];
                        if (Convert.ToInt32(Dt1.Rows[0]["Email"]) >= 10)
                        {
                            CmdSave.Enabled = true;
                            chkterms.Checked = false;
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error",
                                "<SCRIPT language='javascript'>alert('Already Registerd by this Emailid.');</SCRIPT>", false);
                            return;
                        }
                    }
                }

                // CompId = 1074 name duplication check
                if (Session["CompId"] != null && Session["CompId"].ToString() == "1074")
                {
                    if (!string.IsNullOrWhiteSpace(txtFrstNm.Text))
                    {
                        s1 = ObjDAL.IsoStart + "select Count(MemFirstName) as MemFirstName from " + ObjDAL.dBName + "..M_Membermaster where Email='" + txtFrstNm.Text.Trim() + "'" + ObjDAL.IsoEnd;
                        ObjDAL = new DAL();
                        DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s1).Tables[0];
                        if (Convert.ToInt32(Dt1.Rows[0]["MemFirstName"]) > 3)
                        {
                            CmdSave.Enabled = true;
                            chkterms.Checked = false;
                            lblemail.Visible = true;
                            lblmemmsg.Text = "Already Registerd by this Name other id.";
                            txtFrstNm.Text = "";
                            return;
                        }
                        else
                        {
                            lblmemmsg.Visible = false;
                        }
                    }
                }

                // If not comp 1095/1097/1100, require PAN
                if (!(Session["CompId"] != null &&
                      (Session["CompId"].ToString() == "1095" || Session["CompId"].ToString() == "1097" || Session["CompID"].ToString() == "1100" || Session["CompID"].ToString() == "1102" || Session["CompID"].ToString() == "1106" || Session["CompID"].ToString() == "1105" || Session["CompID"].ToString() == "1108" || Session["CompID"].ToString() == "1107" || Session["compid"].ToString() == "1109" || Session["compid"].ToString() == "1110")))
                {
                    if (string.IsNullOrWhiteSpace(txtPanNo.Text))
                    {
                        chkterms.Enabled = true;
                        CmdSave.Enabled = true;
                        lblpan.Visible = true;
                        lblpan.Text = "Please Enter Pan no.";
                        return;
                    }
                    else
                    {
                        lblpan.Visible = false;
                    }
                }

                if (!string.IsNullOrWhiteSpace(txtPanNo.Text))
                {
                    s1 = ObjDAL.IsoStart + "select Count(Panno) as PanNo from " + ObjDAL.dBName + "..M_Membermaster where Panno='" + txtPanNo.Text.Trim() + "'" + ObjDAL.IsoEnd;
                    ObjDAL = new DAL();
                    DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s1).Tables[0];
                    if (Convert.ToInt32(Dt1.Rows[0]["Panno"]) >= 1)
                    {
                        CmdSave.Enabled = true;
                        chkterms.Checked = false;
                        lblpan.Text = "Already Registerd by this panno.";
                        lblpan.Visible = true;
                        return;
                    }
                    else
                    {
                        lblpan.Visible = false;
                    }
                }
                // If mobile empty default to "0"
                if (string.IsNullOrWhiteSpace(txtMobileNo.Text))
                    txtMobileNo.Text = "0";

                string q = "";
                int i = 0;
                DataTable Dt;
                int BankCode = 0;

                // Bank handling: if Others selected
                if (CmbBank.SelectedItem != null && CmbBank.SelectedItem.Text.ToUpper() == "OTHERS")
                {
                    if (!string.IsNullOrWhiteSpace(TxtBank.Text))
                    {
                        q = ObjDAL.IsoStart + "Select * from " + ObjDAL.dBName + "..M_BankMaster where BankName='" + TxtBank.Text.Trim() + "' and Activestatus='Y'and RowStatus='Y' " + ObjDAL.IsoEnd;
                        ObjDAL = new DAL();
                        Dt = new DataTable();
                        Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, q).Tables[0];
                        if (Dt.Rows.Count == 0)
                        {
                            q = "";
                            q = "insert into M_BankMaster (BankCode,BankName,AcNo,IFSCode,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus) ";
                            q += " Select Case When Max(BankCode) Is Null Then '1' Else Max(BankCode)+1 END as BankCode,'" + TxtBank.Text.ToUpper() + "','0','0', ";
                            q += " '','Y','Add by " + Session["IdNo"] + " at " + DateTime.Now.ToString() + "','" + Session["MemName"] + "','" + Convert.ToInt32(Session["FormNo"]) + "',";
                            q += "'','Y' From M_BankMaster ";

                            SqlCommand cmd = new SqlCommand(q, dbConnect.cnnObject);
                            i = cmd.ExecuteNonQuery();
                            if (i > 0)
                            {
                                q = ObjDAL.IsoStart + " select Max(BankCode)as BankCode from " + ObjDAL.dBName + "..M_BankMaster where ActiveStatus='Y' and RowStatus='Y'" + ObjDAL.IsoEnd;
                                cmd = new SqlCommand(q, dbConnectselect.cnnObject);
                                SqlDataReader dRead = cmd.ExecuteReader();
                                if (dRead.Read())
                                {
                                    dblBank = dRead["BankCode"].ToString();
                                }
                                dRead.Close();
                            }
                        }
                        else
                        {
                            dblBank = Dt.Rows[0]["BankCode"].ToString();
                        }
                    }
                }
                else
                {
                    dblBank = CmbBank.SelectedValue;
                }

                int AreaCode = 0;
                AreaCode = 0;
                string RegestType = "";
                if (RbCategory.SelectedValue == "IN")
                    RegestType = "IN";
                else
                    RegestType = CbSubCategory.SelectedValue;

                int PostalAreaCode = 0;
                strDOB = ddlDOBdt.Text + "-" + ddlDOBmnth.Text + "-" + ddlDOBYr.Text;
                strDOM = DDlMDay.Text + "-" + DDLMMonth.Text + "-" + DDLMYear.Text;
                strDOJ = string.Format("{0:dd-MMM-yyyy}", dbConnect.Get_ServerDate());

                dblDistrict = ClearInject(ddlDistrict.Text.ToUpper());
                dblTehsil = ClearInject(ddlTehsil.Text.ToUpper());

                if (dvpin.Visible == true)
                {
                    if (dblDistrict == null)
                        dblDistrict = "";
                    if (ddlState.SelectedValue == "0" || ddlState.SelectedValue == null)
                        dblState = "0";
                    else
                        dblState = ddlState.SelectedValue;
                    DistrictCode = "0";
                    CityCode = "0";
                    VillageCode = "0";
                }
                else
                {
                    dblDistrict = "";
                    dblState = "0";
                    DistrictCode = "0";
                    CityCode = "0";
                    VillageCode = "0";
                }

                IfSC = ClearInject(txtIfsCode.Text.ToUpper());
                dblPlan = "0";
                InVoiceNo = "0";

                if (Session["SessID"] == null || Convert.ToInt32(Session["SessID"]) == 0)
                    FindSession();

                string Name = "";
                string fathername = "";
                if (RbCategory.SelectedValue == "IN")
                {
                    Name = ClearInject(txtFrstNm.Text.ToUpper());
                    fathername = ClearInject(txtFNm.Text.ToUpper());
                }
                else
                {
                    fathername = ClearInject(txtFrstNm.Text.ToUpper());
                    Name = ClearInject(TxtCompanyName.Text.ToUpper());
                }

                // Bank/account validations
                if (!string.IsNullOrWhiteSpace(TxtAccountNo.Text) || !string.IsNullOrWhiteSpace(txtIfsCode.Text))
                {
                    if (string.IsNullOrWhiteSpace(TxtAccountNo.Text))
                    {
                        chkterms.Checked = false;
                        CmdSave.Enabled = true;
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert",
                            "<SCRIPT language='javascript'>alert('Enter Account No.');</SCRIPT>", true);
                        return;
                    }
                    if (CmbBank.SelectedValue == "0" || CmbBank.SelectedValue == null)
                    {
                        chkterms.Checked = false;
                        CmdSave.Enabled = true;
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert",
                            "<SCRIPT language='javascript'>alert('Choose Bank Name.');</SCRIPT>", true);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(TxtBranchName.Text))
                    {
                        chkterms.Checked = false;
                        CmdSave.Enabled = true;
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert",
                            "<SCRIPT language='javascript'>alert('Enter Branch Name.');</SCRIPT>", true);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(DDLAccountType.SelectedValue))
                    {
                        chkterms.Checked = false;
                        CmdSave.Enabled = true;
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert",
                            "<SCRIPT language='javascript'>alert('Enter Account Name.');</SCRIPT>", true);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(txtIfsCode.Text))
                    {
                        chkterms.Checked = false;
                        CmdSave.Enabled = true;
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert",
                            "<SCRIPT language='javascript'>alert('Enter IFSC Code.');</SCRIPT>", true);
                        return;
                    }
                }

                if (Session["CompID"] != null && Convert.ToInt32(Session["CompID"]) == 1007)
                {
                    if (Convert.ToInt32(Session["Uplnr"]) == 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert",
                            "<SCRIPT language='javascript'>alert('Invalid Upline Id Please try Again.!!');</SCRIPT>", true);
                        return;
                    }
                }

                string Strqueryquer = "";

                Strqueryquer = "Insert into Trnfundwithdrawcpanel (Transid) values(" + HdnCheckTrnns.Value + ")";
                int isOk1 = 0;
                try
                {
                    isOk1 = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, Strqueryquer));
                }
                catch (Exception e)
                {
                    isOk1 = 0;
                }
                if (isOk1 > 0)
                {
                    if (Session["compid"] != null && Session["compid"].ToString() == "1088")
                    {
                        strQry = " insert into m_memberMaster(SessId,IdNo,CardNo,FormNo,KitId,UpLnFormNo,RefId,LegNo,RefLegNo,RefFormNo,"
                               + "MemFirstName,MemLastName,MemRelation,MemFName,MemDOB,MemGender,MemOccupation,NomineeName,Address1,Address2,Post,"
                               + "Tehsil,City,District,StateCode,CountryId,PinCode,PhN1,Fax,Mobl,MarrgDate,Passw,Doj,Relation,PanNo,BankID,MICRCode,BranchName,EMail,BV,"
                               + "UpGrdSessId,E_MainPassw,EPassw,ActiveStatus,billNo,RP,HostIp,PID,Paymode,ChDDNo,ChDDBankID,ChDDBank,ChddDate,ChDDBranch,IsPanCard,"
                               + "AadharNo,AadharNo2,AAdharNo3,AreaName,AreaCode)";

                        strQry += "Values(" + Session["SessID"] + ",'" + TxtUserName.Text.Trim() + "',0,0," + Session["Kitid"] + "," + Session["Uplnr"] + ",0," + iLeg + ",0,"
                               + Session["Refral"] + ",'" + ClearInject(txtFrstNm.Text.ToUpper()) + "','','" + CmbType.SelectedValue + "',"
                               + "'" + ClearInject(txtFNm.Text.ToUpper()) + "','" + strDOB + "','" + cGender + "','','" + ClearInject(txtNominee.Text.ToUpper()) + "',"
                               + "'" + ClearInject(txtAddLn1.Text.ToUpper()) + "','','','" + dblTehsil + "','" + dblTehsil + "','" + dblDistrict + "'," + dblState + ",1,"
                               + "'" + txtPinCode.Text + "','" + txtPhNo.Text + "','CHOOSE ACCOUNT TYPE','" + Regex.Replace(txtMobileNo.Text, @"\D", "") + "','" + strDOM + "',"
                               + "'" + ClearInject(TxtPasswd.Text) + "',Getdate(),'" + ClearInject(txtRelation.Text.ToUpper()) + "','" + ClearInject(txtPanNo.Text.ToUpper()) + "',"
                               + "'" + dblBank + "','" + ClearInject(TxtMICR.Text.ToUpper()) + "','" + TxtBranchName.Text.ToUpper() + "','" + ClearInject(txtEMailId.Text) + "',"
                               + Session["Bv"] + ",0,'" + ClearInject(TxtPasswd.Text) + "','" + ClearInject(TxtPasswd.Text) + "','" + Session["JoinStatus"] + "',"
                               + "'" + InVoiceNo + "','" + Session["RP"] + "','" + HostIp + "'," + DdlPaymode.SelectedValue + ",'" + DdlPaymode.SelectedItem.Text.ToUpper() + "',"
                               + "'" + ClearInject(TxtDDNo.Text) + "','0','" + ClearInject(TxtIssueBank.Text.ToUpper()) + "','" + TxtDDDate.Text + "',"
                               + "'" + ClearInject(TxtIssueBranch.Text) + "','N','" + ClearInject(TxtAAdhar1.Text) + "','" + ClearInject(TxtAadhar2.Text) + "',"
                               + "'" + ClearInject(TxtAadhar3.Text) + "','" + TxtVillage.Text + "','" + VillageCode + "')";
                    }
                    else if (Session["compid"] != null && (Session["compid"].ToString() == "1097" || Session["compid"].ToString() == "1102" ||
                        Session["compid"].ToString() == "1100" || Session["CompID"].ToString() == "1106" || Session["CompID"].ToString() == "1108" || Session["compid"].ToString() == "1105" || Session["compid"].ToString() == "1109" || Session["compid"].ToString() == "1110"))
                    {
                        strQry = " insert into m_memberMaster(SessId,IdNo,CardNo,FormNo,KitId,UpLnFormNo,RefId,LegNo,RefLegNo,RefFormNo,"
                               + "MemFirstName,MemLastName,MemRelation,MemFName,MemDOB,MemGender,MemOccupation,NomineeName,Address1,Address2,Post,"
                               + "Tehsil,City,District,StateCode,CountryId,PinCode,PhN1,Fax,Mobl,MarrgDate,Passw,Doj,Relation,PanNo,BankID,MICRCode,BranchName,EMail,BV,"
                               + "UpGrdSessId,E_MainPassw,EPassw,ActiveStatus,billNo,RP,HostIp,PID,Paymode,ChDDNo,ChDDBankID,ChDDBank,ChddDate,ChDDBranch,IsPanCard,"
                               + "AadharNo,AadharNo2,AAdharNo3,AreaName,AreaCode,PostalPin,PostalStateCode,PostalAreaCode,PostalDistrict,PostalCity,PostalState,fld5,fld6)";
                        strQry += "Values(" + Session["SessID"] + ",0,0,0," + Session["Kitid"] + "," + Session["Uplnr"] + ",0," + iLeg + ",0,"
                               + Session["Refral"] + ",'" + ClearInject(txtFrstNm.Text.ToUpper()) + "','','" + CmbType.SelectedValue + "',"
                               + "'" + ClearInject(txtFNm.Text.ToUpper()) + "','" + strDOB + "','" + cGender + "','','" + ClearInject(txtNominee.Text.ToUpper()) + "',"
                               + "'" + ClearInject(txtAddLn1.Text.ToUpper()) + "','','','" + dblTehsil + "','" + dblTehsil + "','" + dblDistrict + "'," + dblState + ",1,"
                               + "'" + txtPinCode.Text + "','" + txtPhNo.Text + "','CHOOSE ACCOUNT TYPE','" + Regex.Replace(txtMobileNo.Text, @"\D", "") + "','" + strDOM + "',"
                               + "'" + ClearInject(TxtPasswd.Text) + "',Getdate(),'" + ClearInject(txtRelation.Text.ToUpper()) + "','" + ClearInject(txtPanNo.Text.ToUpper()) + "',"
                               + "'" + dblBank + "','" + ClearInject(TxtMICR.Text.ToUpper()) + "','" + TxtBranchName.Text.ToUpper() + "','" + ClearInject(txtEMailId.Text) + "',"
                               + Session["Bv"] + ",0,'" + ClearInject(TxtPasswd.Text) + "','" + ClearInject(TxtPasswd.Text) + "','" + Session["JoinStatus"] + "',"
                               + "'" + InVoiceNo + "','" + Session["RP"] + "','" + HostIp + "'," + DdlPaymode.SelectedValue + ",'" + DdlPaymode.SelectedItem.Text.ToUpper() + "',"
                               + "'" + ClearInject(TxtDDNo.Text) + "','0','" + ClearInject(TxtIssueBank.Text.ToUpper()) + "','" + TxtDDDate.Text + "',"
                               + "'" + ClearInject(TxtIssueBranch.Text) + "','N','" + ClearInject(TxtAAdhar1.Text) + "','" + ClearInject(TxtAadhar2.Text) + "',"
                               + "'" + ClearInject(TxtAadhar3.Text) + "','" + TxtVillage.Text + "','" + VillageCode + "','" + txtPinCode.Text + "'," + dblState + ",'" + VillageCode + "',"
                               + "'" + dblDistrict + "','" + dblTehsil + "','" + ddlState.SelectedItem.Text + "','D','WEB')";
                    }
                    else if (Session["compid"] != null && (Session["compid"].ToString() == "1107"))
                    {

                        strQry = " insert into m_memberMaster(SessId,IdNo,CardNo,FormNo,KitId,UpLnFormNo,RefId,LegNo,RefLegNo,RefFormNo,"
                               + "MemFirstName,MemLastName,MemRelation,MemFName,MemDOB,MemGender,MemOccupation,NomineeName,Address1,Address2,Post,"
                               + "Tehsil,City,District,StateCode,CountryId,PinCode,PhN1,Fax,Mobl,MarrgDate,Passw,Doj,Relation,PanNo,BankID,MICRCode,BranchName,EMail,BV,"
                               + "UpGrdSessId,E_MainPassw,EPassw,ActiveStatus,billNo,RP,HostIp,PID,Paymode,ChDDNo,ChDDBankID,ChDDBank,ChddDate,ChDDBranch,IsPanCard,"
                               + "AadharNo,AadharNo2,AAdharNo3,AreaName,AreaCode,PostalPin,PostalStateCode,PostalAreaCode,PostalDistrict,PostalCity,PostalState,fld5,fld6)";
                        strQry += "Values(" + Session["SessID"] + ",'" + TxtUserName.Text + "',0,0," + Session["Kitid"] + "," + Session["Uplnr"] + ",0," + iLeg + ",0,"
                               + Session["Refral"] + ",'" + ClearInject(txtFrstNm.Text.ToUpper()) + "','','" + CmbType.SelectedValue + "',"
                               + "'" + ClearInject(txtFNm.Text.ToUpper()) + "','" + strDOB + "','" + cGender + "','','" + ClearInject(txtNominee.Text.ToUpper()) + "',"
                               + "'" + ClearInject(txtAddLn1.Text.ToUpper()) + "','','','" + dblTehsil + "','" + dblTehsil + "','" + dblDistrict + "'," + dblState + ",1,"
                               + "'" + txtPinCode.Text + "','" + txtPhNo.Text + "','CHOOSE ACCOUNT TYPE','" + Regex.Replace(txtMobileNo.Text, @"\D", "") + "','" + strDOM + "',"
                               + "'" + ClearInject(TxtPasswd.Text) + "',Getdate(),'" + ClearInject(txtRelation.Text.ToUpper()) + "','" + ClearInject(txtPanNo.Text.ToUpper()) + "',"
                               + "'" + dblBank + "','" + ClearInject(TxtMICR.Text.ToUpper()) + "','" + TxtBranchName.Text.ToUpper() + "','" + ClearInject(txtEMailId.Text) + "',"
                               + Session["Bv"] + ",0,'" + ClearInject(TxtPasswd.Text) + "','" + ClearInject(TxtPasswd.Text) + "','" + Session["JoinStatus"] + "',"
                               + "'" + InVoiceNo + "','" + Session["RP"] + "','" + HostIp + "'," + DdlPaymode.SelectedValue + ",'" + DdlPaymode.SelectedItem.Text.ToUpper() + "',"
                               + "'" + ClearInject(TxtDDNo.Text) + "','0','" + ClearInject(TxtIssueBank.Text.ToUpper()) + "','" + TxtDDDate.Text + "',"
                               + "'" + ClearInject(TxtIssueBranch.Text) + "','N','" + ClearInject(TxtAAdhar1.Text) + "','" + ClearInject(TxtAadhar2.Text) + "',"
                               + "'" + ClearInject(TxtAadhar3.Text) + "','" + TxtVillage.Text + "','" + VillageCode + "','" + txtPinCode.Text + "'," + dblState + ",'" + VillageCode + "',"
                               + "'" + dblDistrict + "','" + dblTehsil + "','" + ddlState.SelectedItem.Text + "','D','WEB')";
                    }

                    else
                    {
                        strQry = " insert into m_memberMaster(SessId,IdNo,CardNo,FormNo,KitId,UpLnFormNo,RefId,LegNo,RefLegNo,RefFormNo,";
                        strQry += "MemFirstName,MemLastName,MemRelation,MemFName,MemDOB,MemGender,MemOccupation,NomineeName,Address1,Address2,Post,";
                        strQry += "Tehsil,City,District,StateCode,CountryId,PinCode,PhN1,Fax,Mobl,MarrgDate,Passw,Doj,Relation,PanNo,BankID,MICRCode,BranchName,EMail,BV,";
                        strQry += "UpGrdSessId,E_MainPassw,EPassw,ActiveStatus,billNo,RP,HostIp,PID,Paymode,ChDDNo,ChDDBankID,ChDDBank,ChddDate,ChDDBranch,IsPanCard,";
                        strQry += "AadharNo,AadharNo2,AAdharNo3,AreaName,AreaCode)";
                        strQry += "Values(" + Session["SessID"] + ",0,0,0," + Session["Kitid"] + "," + Session["Uplnr"] + ",0," + iLeg + ",0,";
                        strQry += "" + Session["Refral"] + ",'" + ClearInject(txtFrstNm.Text.ToUpper()) + "','','" + CmbType.SelectedValue + "',";
                        strQry += "'" + ClearInject(txtFNm.Text.ToUpper()) + "','" + strDOB + "','" + cGender + "','','" + ClearInject(txtNominee.Text.ToUpper()) + "',";
                        strQry += "'" + ClearInject(txtAddLn1.Text.ToUpper()) + "','','','" + dblTehsil + "','" + dblTehsil + "','" + dblDistrict + "'," + dblState + ",1,";
                        strQry += "'" + txtPinCode.Text + "','" + txtPhNo.Text + "','CHOOSE ACCOUNT TYPE','" + Regex.Replace(txtMobileNo.Text, @"\D", "") + "','" + strDOM + "',";
                        strQry += "'" + ClearInject(TxtPasswd.Text) + "',Getdate(),'" + ClearInject(txtRelation.Text.ToUpper()) + "','" + ClearInject(txtPanNo.Text.ToUpper()) + "',";
                        strQry += "'" + dblBank + "','" + ClearInject(TxtMICR.Text.ToUpper()) + "','" + TxtBranchName.Text.ToUpper() + "','" + ClearInject(txtEMailId.Text) + "',";
                        strQry += "" + Session["Bv"] + ",0,'" + ClearInject(TxtPasswd.Text) + "','" + ClearInject(TxtPasswd.Text) + "','" + Session["JoinStatus"] + "',";
                        strQry += "'" + InVoiceNo + "','" + Session["RP"] + "','" + HostIp + "'," + DdlPaymode.SelectedValue + ",'" + DdlPaymode.SelectedItem.Text.ToUpper() + "',";
                        strQry += "'" + ClearInject(TxtDDNo.Text) + "','0','" + ClearInject(TxtIssueBank.Text.ToUpper()) + "','" + TxtDDDate.Text + "',";
                        strQry += "'" + ClearInject(TxtIssueBranch.Text) + "','N','" + ClearInject(TxtAAdhar1.Text) + "','" + ClearInject(TxtAadhar2.Text) + "',";
                        strQry += "'" + ClearInject(TxtAadhar3.Text) + "','" + TxtVillage.Text + "','" + VillageCode + "')";
                    }

                    int isOk = Convert.ToInt32(SqlHelper.ExecuteNonQuery(
                        (string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]],
                        CommandType.Text,
                        strQry));

                    LastInsertID = "0";
                    if (isOk > 0)
                    {
                        string membername = "";
                        string Email = "";
                        string mobile = "";
                        string Password = "";
                        string SqlStr = "";
                        SqlStr = ObjDAL.IsoStart + "SELECT TOP 1 a.IDNO,a.formno,b.IsBill,a.Passw,a.MemFirstname,a.MemlastName,a.Email,a.mobl FROM " + ObjDAL.dBName + "..m_MemberMaster as a," + ObjDAL.dBName + "..m_KitMaster as b "
                               + "where a.kitid=b.kitid ORDER BY mid DESC" + ObjDAL.IsoEnd;
                        DataTable DtSql = SqlHelper.ExecuteDataset((string)HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]], CommandType.Text, SqlStr).Tables[0];
                        if (DtSql.Rows.Count > 0)
                        {
                            membername = DtSql.Rows[0]["MemfirstName"].ToString() + " " + DtSql.Rows[0]["MemLastName"].ToString();
                            Email = DtSql.Rows[0]["Email"].ToString();
                            LastInsertID = DtSql.Rows[0]["IDNO"].ToString();
                            mobile = DtSql.Rows[0]["mobl"].ToString();
                            Password = DtSql.Rows[0]["Passw"].ToString();
                            Session["Kit"] = DtSql.Rows[0]["IsBill"];
                        }
                        else
                        {
                            LastInsertID = "10001";
                        }

                        if (Session["IsSendSMS"] != null && Session["IsSendSMS"].ToString() == "Y")
                        {
                            sendSMS();
                        }
                        CmdSave.Enabled = true;
                        if (Session["CompId"] != null && Session["CompId"].ToString() == "1105" || Session["CompId"].ToString() == "1108" || Session["CompId"].ToString() == "1110")
                        {
                            if (!string.IsNullOrWhiteSpace(Email)) SendToMemberMail(LastInsertID, Email, membername, Password);
                        }
                        Session["LASTID"] = LastInsertID;
                        Session["Join"] = "YES";
                        Response.Redirect("Welcome.Aspx?IDNo=" + LastInsertID, false);
                        string scrname2 = "<SCRIPT language='javascript'>alert('Congratulations! Your have successfully registered And Login details has been sent on your mobile and registered Email Id! ');</SCRIPT>";
                        ClrCtrl();
                    }
                    else
                    {
                        CmdSave.Enabled = true;
                        chkterms.Checked = false;
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert",
                            "<SCRIPT language='javascript'>alert('Try Again Later.');</SCRIPT>", true);
                    }
                }
                else
                {
                    Response.Redirect("NewJoiningfreeuc.Aspx");
                }
            } // end Validt_SpnsrDtl == OK


        }
        catch (Exception e)
        {
            CmdSave.Enabled = true;
            chkterms.Checked = false;
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert",
                "<SCRIPT language='javascript'>alert('" + e.Message.Replace("'", "\\'") + "');</SCRIPT>", true);

            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            // Assuming ObjDAL has WriteToFile method (your VB used ObjDAL.WriteToFile)
            try
            {
                ObjDAL.WriteToFile(text + e.Message);
            }
            catch { }

            Response.Write("Try later.");
            return;
        }
    }
}