using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Xml;
using System.Net;
using System.Text;
using DocumentFormat.OpenXml.Presentation;
using System.IdentityModel.Protocols.WSTrust;
using System.Security.Cryptography;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System.Net.NetworkInformation;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Office;
using System.Net.Mail;

public partial class ProcessAPIWithK : System.Web.UI.Page
{
    public string _ReqType;
    public GetMsg2 ErrObj = new GetMsg2();

    SqlConnection Conn;
    SqlConnection Connselect;
    SqlCommand Comm;
    SqlDataAdapter Adp;
    DataSet Ds;
    SqlDataReader Dr;

    string _NewID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    Random Rnd = new Random();
    bool Bool = true;

    string HostIp = HttpContext.Current.Request.UserHostAddress;
    string _Company = "";
    string _Logo = "";
    string strQry = "";
    string _MailID = "";
    string _MailPass = "";
    string _MailHost = "";
    string _SMSSender = "APPSMS";
    string _SMSUser = "";
    string _SMSPass = "";
    string _RefFormNo = "";
    string _UpLnFormNo = "";
    string _Token = "GW739IESP1956rerir";
    string _Tokenlogin = "JaiGW739IESPrerirDarbar";
    string membername = "";
    string constr = "";
    clsGeneral objGen = new clsGeneral();
    string KeyE = "6b04d38748f94490a636cf1be3d82841";
    string IV = "f8adbf3c94b7463d";
    string sResult = "";
    DAL Obj;
    DAL Objd;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //GetCompID();
            HttpContext.Current.Session["CompID"] = "1108";
            objGen.GetConnectionByComp();
            objGen.GetInvDataBaseByComp();
            Obj = new DAL();
            Objd = new DAL();
            getData();

            _Company = Session["CompName"]?.ToString();
            _Logo = Session["Logo"]?.ToString();
            _MailID = Session["CompMail"]?.ToString();
            _MailPass = Session["MailPass"]?.ToString();
            _MailHost = Session["MailHost"]?.ToString();
            _SMSSender = Session["ClientId"]?.ToString();
            _SMSUser = Session["SmsId"]?.ToString();
            _SMSPass = Session["SmsPass"]?.ToString();

            Session["InvDB"] = "";
            Session["WR"] = "sr123456";
            Session["Website"] = "http://srecwholesale.com/";
           // GetSmsTemplate();


            string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            int random_number = new Random().Next(0, 999);
            string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');
            sResult = formatted_datetime;

            string URL = "https://" + HttpContext.Current.Request.Url.Host + "/ProcessAPIWithK.aspx";

            string sql = @"CREATE TABLE [dbo].[Tbl_ApiRequest_ResponseQrCode](
                           [Id] [int] IDENTITY(1,1) NOT NULL,
                           [ReqID] [numeric](24, 0) NULL,
                           [Request] [nvarchar](max) NULL,
                           [postdata] [nvarchar](max) NULL,
                           [Response] [nvarchar](max) NULL,
                           [RecTimeStamp] [datetime] DEFAULT (getdate()) NOT NULL,
                           CompID Numeric(18,0)
                       )";

            int i;

            try
            {
                i = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(),
                                              CommandType.Text, sql);
            }
            catch { }
            Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
            Conn.Open();
            Connselect = new SqlConnection(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString());
            Connselect.Open();
            constr = HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString();

            // Case: Request.Form with MyJson
            if (Request.Form.HasKeys() && Request.Form["MyJson"] != null)
            {
                string sRequestData = ClearInject(Request.Form["MyJson"]);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, string> dict = jss.Deserialize<Dictionary<string, string>>(sRequestData);

                try
                {
                    SqlCommand Comm = new SqlCommand("INSERT INTO ReqType(reqtype) VALUES('" +
                        sRequestData.Replace("//n", "\n") + "')", Conn);
                    Comm.ExecuteNonQuery();

                    string sql_req = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID, Request, postdata, CompID) " +
                                     "VALUES ('" + sResult.Trim() + "','" + URL.Trim() + "', '" +
                                     sRequestData.Replace("//n", "\n") + "','" + Session["CompID"] + "')";

                    SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req);
                }
                catch { }

                if (dict.ContainsKey("reqtype"))
                {
                    _ReqType = dict["reqtype"];
                    if (!string.IsNullOrEmpty(_ReqType))
                    {
                        //OneProcessFile(_ReqType, dict);
                    }
                    else
                    {
                        string Result_Json = "{\"response\":\"Failed\",\"msg\":\"File Api Request Not Found.!\"}";
                        string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json +
                                         "' WHERE ReqID = '" + sResult.Trim() + "'";
                        SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res);

                        Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                        Response.Clear();
                        Response.ContentType = "application/json";
                        Response.Write(Result_Json);
                    }
                }
            }
            // Case: File Upload
            else if (Request.Form["File"] != null)
            {
                Response.Write(Base64ToImage(Request.Form["File"]));
            }
            else
            {
                // Raw JSON request
                Request.InputStream.Position = 0;
                StreamReader reader = new StreamReader(Request.InputStream);
                string json = reader.ReadToEnd();

                try
                {
                    string sql_req = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID, Request, postdata, CompID) " +
                                     "VALUES ('" + sResult.Trim() + "','" + URL.Trim() + "', '" +
                                     json.Replace("//n", "\n") + "','" + Session["CompID"] + "')";

                    SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_req);

                    SqlCommand Comm = new SqlCommand("INSERT INTO ReqType(reqtype) VALUES('" +
                        json.Replace("//n", "\n") + "')", Conn);
                    Comm.ExecuteNonQuery();
                }
                catch { }

                if (!string.IsNullOrEmpty(json))
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    var data = jss.Deserialize<Dictionary<string, object>>(json);

                    if (data.ContainsKey("reqtype"))
                    {
                        string reqType = data["reqtype"].ToString();

                        if (reqType == "shopping")
                        {
                            //Response.Write(RepurchaseProductRequest(data));
                        }
                        else
                        {
                            Dictionary<string, string> dict = jss.Deserialize<Dictionary<string, string>>(json);

                            if (dict.ContainsKey("reqtype"))
                            {
                                _ReqType = dict["reqtype"];
                                if (!string.IsNullOrEmpty(_ReqType))
                                {
                                    Process(_ReqType, dict);
                                }
                                else
                                {
                                    string Result_Json = "{\"response\":\"FAILED\",\"msg\":\"Api Request Type Not Found.!\"}";
                                    string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json +
                                                     "' WHERE ReqID = '" + sResult.Trim() + "'";
                                    SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res);

                                    Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                                    Response.Clear();
                                    Response.ContentType = "application/json";
                                    Response.Write(Result_Json);
                                }
                            }
                        }
                    }
                }
                else
                {
                    string Result_Json = "{\"response\":\"FAILED\",\"msg\":\"Api Json Request Not Found.!\"}";
                    string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json +
                                     "' WHERE ReqID = '" + sResult.Trim() + "'";
                    SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res);

                    Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                    Response.Clear();
                    Response.ContentType = "application/json";
                    Response.Write(Result_Json);
                }
            }
        }
        catch (Exception)
        {
            string Result_Json = "{\"response\":\"FAILED\",\"msg\":\"Exception Case MyJson Not Valid.!\"}";
            string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json +
                             "' WHERE ReqID = '" + sResult.Trim() + "'";
            SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql_res);

            Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
            Response.Clear();
            Response.ContentType = "application/json";
            Response.Write(Result_Json);
        }

        Response.End();
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
    private void getData()
    {
        try
        {
            var dbConnect = new cls_DataAccess((string)HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]]);
            dbConnect.OpenConnection();

            // -----------------------
            // M_CompanyMaster
            // -----------------------
            using (var cmd = new SqlCommand(Obj.IsoStart + "select * from " + Obj.dBName + "..M_CompanyMaster" + Obj.IsoEnd, dbConnect.cnnObject))
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
            using (var cmd = new SqlCommand(Obj.IsoStart + "select * from " + Obj.dBName + "..M_ConfigMaster" + Obj.IsoEnd, dbConnect.cnnObject))
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
            using (var cmd = new SqlCommand(Obj.IsoStart + "select Max(SEssid) as SessID from " + Obj.dBName + "..D_Monthlypaydetail" + Obj.IsoEnd, dbConnect.cnnObject))
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
            using (var cmd = new SqlCommand(Obj.IsoStart + "select Max(SEssid) as SessID from " + Obj.dBName + "..m_SessnMaster" + Obj.IsoEnd, dbConnect.cnnObject))
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
    public void Process(string _Reqtype, Dictionary<string, string> dict)
    {
        try
        {

            if (_Reqtype == "joining")
            {
                string Result_Json = Register(dict);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            //if (_Reqtype == "joiningmakeandg")
            //{
            //    string Result_Json = RegisterMakeAnd(dict);
            //    string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
            //    int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
            //    Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
            //    Response.Clear();
            //    Response.ContentType = "application/json";
            //    Response.Write(Result_Json);
            //}
            else if (_ReqType == "banklist")
            {
                string _ReqUser = "";
                try
                {
                    _ReqUser = ClearInject(dict["userid"]);
                }
                catch (Exception)
                {
                }

                string _ReqPassw = "";
                try
                {
                    _ReqPassw = ClearInject(dict["passwd"]);
                }
                catch (Exception)
                {
                }

                string Result_Json = BankList(_ReqUser, _ReqPassw);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_ReqType == "statelist")
            {
                string _ReqUser = "";
                try
                {
                    _ReqUser = ClearInject(dict["userid"]);
                }
                catch (Exception)
                {
                }

                string _ReqPassw = "";
                try
                {
                    _ReqPassw = ClearInject(dict["passwd"]);
                }
                catch (Exception)
                {
                }

                string _CountryCode = ClearInject(dict["countrycode"]);

                string Result_Json = statelist(_ReqUser, _ReqPassw, _CountryCode);

                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_ReqType == "accounttype")
            {
                string _ReqUser = "";
                try
                {
                    _ReqUser = ClearInject(dict["userid"]);
                }
                catch (Exception) { }

                string _ReqPassw = "";
                try
                {
                    _ReqPassw = ClearInject(dict["passwd"]);
                }
                catch (Exception) { }

                string Result_Json = AccountType(_ReqUser, _ReqPassw);

                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_ReqType == "idtype")
            {
                string _ReqUser = "";
                try
                {
                    _ReqUser = ClearInject(dict["userid"]);
                }
                catch (Exception) { }

                string _ReqPassw = "";
                try
                {
                    _ReqPassw = ClearInject(dict["passwd"]);
                }
                catch (Exception) { }

                string Result_Json = IdTypelist();
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_ReqType == "checkpancard")
            {
                string _ReqMobileNo = "";
                try
                {
                    _ReqMobileNo = ClearInject(dict["userid"]);
                }
                catch (Exception) { }

                string _ReqOtpCode = "";
                try
                {
                    _ReqOtpCode = ClearInject(dict["passwd"]);
                }
                catch (Exception) { }

                string _ReqSponsor = ClearInject(dict["panno"]);

                string Result_Json = CheckPanno(_ReqMobileNo, _ReqOtpCode, _ReqSponsor);

                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_ReqType == "checksponsor")
            {
                string _ReqMobileNo = "";
                try
                {
                    _ReqMobileNo = ClearInject(dict["userid"]);
                }
                catch (Exception) { }

                string _ReqOtpCode = "";
                try
                {
                    _ReqOtpCode = ClearInject(dict["passwd"]);
                }
                catch (Exception) { }

                string _ReqSponsor = ClearInject(dict["sponsorid"]);

                string Result_Json = CheckSponsor(_ReqMobileNo, _ReqOtpCode, _ReqSponsor);

                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_ReqType == "kycpancarddetail")
            {
                string _ReqMobileNo = "";
                try
                {
                    _ReqMobileNo = ClearInject(dict["userid"]);
                }
                catch (Exception) { }

                string _ReqOtpCode = "";
                try
                {
                    _ReqOtpCode = ClearInject(dict["passwd"]);
                }
                catch (Exception) { }

                string Result_Json = KYCPancardDetail(_ReqMobileNo, _ReqOtpCode);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_ReqType == "kycbankdetail")
            {
                string _ReqMobileNo = "";
                try
                {
                    _ReqMobileNo = ClearInject(dict["userid"]);
                }
                catch (Exception) { }

                string _ReqOtpCode = "";
                try
                {
                    _ReqOtpCode = ClearInject(dict["passwd"]);
                }
                catch (Exception) { }

                string Result_Json = KYCBankDetail(_ReqMobileNo, _ReqOtpCode);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "kycbanksave")
            {
                string _ReqUser = ClearInject(dict["userid"]);
                string _ReqPassw = ClearInject(dict["passwd"]);

                string accounttype = ClearInject(dict["accounttype"]);
                string accountno = ClearInject(dict["accountno"]);
                string bankcode = ClearInject(dict["bankcode"]);
                string bankname = ClearInject(dict["bankname"]);

                string otherbankname = "";
                try
                {
                    if (ClearInject(dict["otherbankname"]) == null)
                    {
                        otherbankname = ClearInject(dict["otherbankname"]);
                    }
                }
                catch (Exception)
                {
                    otherbankname = "";
                }

                string branchname = ClearInject(dict["branchname"]);
                string ifsccode = ClearInject(dict["ifsccode"]);
                string Bankimage;
                try
                {
                    Bankimage = ClearInject(dict["bankimage"]);
                }
                catch (Exception)
                {
                    Bankimage = "";
                }
                string Result_Json = BANKDETAILSAVE(_ReqUser, _ReqPassw, accounttype, accountno, bankcode, bankname, otherbankname, branchname, ifsccode, Bankimage);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "kycpancardsave")
            {
                string _ReqUser = ClearInject(dict["userid"].ToString());
                string _ReqPassw = ClearInject(dict["passwd"].ToString());
                string panno = ClearInject(dict["panno"].ToString());

                string panimage;
                try
                {
                    panimage = ClearInject(dict["panimage"].ToString());
                }
                catch (Exception)
                {
                    panimage = "";
                }
                string Result_Json = PanCardSave(_ReqUser, _ReqPassw, panno, panimage);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "kycdetail")
            {
                string _ReqUser = ClearInject(dict["userid"]);
                string _ReqPassw = ClearInject(dict["passwd"]);
                string Result_Json = KYCDetail(_ReqUser, _ReqPassw);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "kycaddress")
            {
                string _ReqUser = ClearInject(dict["userid"]);
                string _ReqPassw = ClearInject(dict["passwd"]);

                string address = ClearInject(dict["address"]);
                string cityname = ClearInject(dict["cityname"]);
                string citycode = ClearInject(dict["citycode"]);
                string pincode = ClearInject(dict["pincode"]);
                string statecode = ClearInject(dict["statecode"]);
                string districtcode = ClearInject(dict["districtcode"]);
                string district = ClearInject(dict["district"]);
                string areaname = ClearInject(dict["areaname"]);
                string areacode = ClearInject(dict["areacode"]);

                string otherareaname = "";
                try
                {
                    if (ClearInject(dict["otherareaname"]) == null)
                        otherareaname = "";
                    else
                        otherareaname = ClearInject(dict["otherareaname"]);
                }
                catch (Exception)
                {
                    otherareaname = "";
                }

                string idproofid = ClearInject(dict["idproofid"]);
                string idproofno = ClearInject(dict["idproofno"]);
                string frontaddressproof = ClearInject(dict["frontaddressproof"]);
                string backaddressproof = ClearInject(dict["backaddressproof"]);
                string Result_Json = KYCADDRESSSAVE(_ReqUser, _ReqPassw, address, pincode, cityname, citycode, statecode, district, districtcode, areacode, areaname, otherareaname, idproofid, idproofno, frontaddressproof, backaddressproof);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "formupload")
            {
                string _ReqUser = ClearInject(dict["userid"]);
                string _ReqPassw = ClearInject(dict["passwd"]);
                string Front = ClearInject(dict["formupload"]);
                string Result_Json = FormUpload(_ReqUser, _ReqPassw, Front);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_ReqType == "getkyc")
            {
                string _ReqMobileNo = "";
                try
                {
                    _ReqMobileNo = ClearInject(dict["userid"]);
                }
                catch (Exception) { }

                string _ReqOtpCode = "";
                try
                {
                    _ReqOtpCode = ClearInject(dict["passwd"]);
                }
                catch (Exception) { }

                string Result_Json = GetKyc(_ReqMobileNo, _ReqOtpCode);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_ReqType == "saveallkyc")
            {
                string _ReqUser = ClearInject(dict["userid"]);
                string _ReqPassw = ClearInject(dict["passwd"]);

                string address = ClearInject(dict["address"]);
                string citycode = ClearInject(dict["citycode"]);
                string cityname = ClearInject(dict["cityname"]);
                string pincode = ClearInject(dict["pincode"]);
                string statecode = ClearInject(dict["statecode"]);
                string districtcode = ClearInject(dict["districtcode"]);
                string district = ClearInject(dict["district"]);
                string areaname = ClearInject(dict["areaname"]);
                string areacode = ClearInject(dict["areacode"]);

                string otherareaname = "";
                try
                {
                    if (dict["otherareaname"] == null)
                        otherareaname = "";
                    else
                        otherareaname = ClearInject(dict["otherareaname"]);
                }
                catch (Exception)
                {
                    otherareaname = "";
                }

                string idproofid = ClearInject(dict["idproofid"]);
                string idproofno = ClearInject(dict["idproofno"]);
                string frontaddressproof = ClearInject(dict["frontaddressproof"]);
                string backaddressproof = ClearInject(dict["backaddressproof"]);

                string accounttype = ClearInject(dict["accounttype"]);
                string accountno = ClearInject(dict["accountno"]);
                string bankcode = ClearInject(dict["bankcode"]);
                string bankname = ClearInject(dict["bankname"]);

                string otherbankname = "";
                try
                {
                    if (dict["otherbankname"] == null)
                        otherbankname = "";
                    else
                        otherbankname = ClearInject(dict["otherbankname"]);
                }
                catch (Exception)
                {
                    otherbankname = "";
                }

                string branchname = ClearInject(dict["branchname"]);
                string ifsccode = ClearInject(dict["ifsccode"]);
                string Bankimage = ClearInject(dict["bankimage"]);
                string panno = ClearInject(dict["panno"]);
                string panimage = ClearInject(dict["panimage"]);
                string Front = ClearInject(dict["formupload"].ToString());
                string Result_Json = Allkyc(
                    _ReqUser, _ReqPassw, address, pincode, cityname, citycode, statecode,
                    district, districtcode, areacode, areaname, otherareaname,
                    idproofid, idproofno, frontaddressproof, backaddressproof,
                    accounttype, accountno, bankcode, bankname, otherbankname,
                    branchname, ifsccode, Bankimage, panno, panimage, Front
                );
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "cpassword") // Change Password
            {
                string _ReqUserID = ClearInject(dict["userid"].ToString());
                string _ReqPwd = ClearInject(dict["passwd"].ToString());
                string _ReqNPwd = ClearInject(dict["npasswd"].ToString());

                string Result_Json = string.Empty;

                Result_Json = ChangePassword(_ReqUserID, _ReqPwd, _ReqNPwd);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "complainttype") // Change Password
            {
                string _ReqUserID = ClearInject(dict["userid"].ToString());
                string _ReqPwd = ClearInject(dict["passwd"].ToString());
                string Result_Json = string.Empty;
                Result_Json = complaintlist(_ReqUserID, _ReqPwd);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "complainttypewithoutlogin") // Change Password
            {
                string _ReqUserID = ClearInject(dict["userid"].ToString());
                string _ReqPwd = ClearInject(dict["passwd"].ToString());
                string Result_Json = string.Empty;
                Result_Json = complaintlistwithoutlogin(_ReqUserID, _ReqPwd);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "referrallink") // Change Password
            {
                string _ReqUserID = ClearInject(dict["userid"].ToString());
                string _ReqPwd = ClearInject(dict["passwd"].ToString());
                string Result_Json = string.Empty;
                Result_Json = Getreferrallink1(_ReqUserID, _ReqPwd);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "complaintdetail") // for complaint detail
            {
                string _ReqUser = ClearInject(dict["userid"].ToString());
                string _ReqPassw = ClearInject(dict["passwd"].ToString());
                int _ReqFromNo = Convert.ToInt32(ClearInject(dict["from"].ToString()));
                int _ReqToNo = Convert.ToInt32(ClearInject(dict["to"].ToString()));
                string Result_Json = "";
                Result_Json = ComplaintDetail(_ReqUser, _ReqPassw, _ReqFromNo, _ReqToNo);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "savecomplaint") // save complaint
            {
                string _ReqUser = ClearInject(dict["userid"].ToString());
                string _ReqPassw = ClearInject(dict["passwd"].ToString());
                string _ReqIdno = ClearInject(dict["idno"].ToString());
                string _Reqname = ClearInject(dict["name"].ToString());
                string _ReqMobileno = ClearInject(dict["mobileno"].ToString());
                string _ReqEmail = ClearInject(dict["email"].ToString());
                string Complaintid = ClearInject(dict["complaintid"].ToString());
                string _ReqSubject = ClearInject(dict["subject"].ToString());
                string _ReqDescription = ClearInject(dict["description"].ToString());
                string Result_Json = "";
                Result_Json = SaveComplaint(_ReqUser, _ReqPassw, _ReqIdno, _Reqname, _ReqMobileno, _ReqEmail, Complaintid, _ReqSubject, _ReqDescription);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "savecomplaintwithout") // save complaint
            {
                string _ReqUser = ClearInject(dict["userid"].ToString());
                string _ReqPassw = ClearInject(dict["passwd"].ToString());
                string _ReqIdno = ClearInject(dict["idno"].ToString());
                string _Reqname = ClearInject(dict["name"].ToString());
                string _ReqMobileno = ClearInject(dict["mobileno"].ToString());
                string _ReqEmail = ClearInject(dict["email"].ToString());
                string Complaintid = ClearInject(dict["complaintid"].ToString());
                string _ReqSubject = ClearInject(dict["subject"].ToString());
                string _ReqDescription = ClearInject(dict["description"].ToString());
                string Result_Json = "";
                Result_Json = SaveComplaintWithout(_ReqUser, _ReqPassw, _ReqIdno, _Reqname, _ReqMobileno, _ReqEmail, Complaintid, _ReqSubject, _ReqDescription);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "complaintreply") // for View complaint Reply detail
            {
                string _ReqUser = ClearInject(dict["userid"].ToString());
                string _ReqPassw = ClearInject(dict["passwd"].ToString());

                int _ReqcomplaintId = Convert.ToInt32(
                                          ClearInject(dict["complaintid"].ToString())
                                      );

                string Result_Json = "";

                Result_Json = ComplaintReplyDetail(_ReqUser, _ReqPassw, _ReqcomplaintId);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "allwallet") // for View complaint Reply detail
            {
                string _ReqUser = ClearInject(dict["userid"].ToString());
                string _ReqPassw = ClearInject(dict["passwd"].ToString());
                string Result_Json = "";
                Result_Json = AllWalletList(_ReqUser, _ReqPassw);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "allwalletlist")
            {
                string _ReqUser = "";
                try
                {
                    _ReqUser = ClearInject(dict["userid"].ToString());
                }
                catch
                {
                    // ignore (same as VB)
                }

                string _ReqPassw = "";
                try
                {
                    _ReqPassw = ClearInject(dict["passwd"].ToString());
                }
                catch
                {
                    // ignore (same as VB)
                }

                string Actype = ClearInject(dict["wallettype"].ToString());

                int fromno = 0;
                try
                {
                    fromno = Convert.ToInt32(ClearInject(dict["fromno"].ToString()));
                }
                catch
                {
                    fromno = 0;
                }

                int tono = 0;
                try
                {
                    tono = Convert.ToInt32(ClearInject(dict["tono"].ToString()));
                }
                catch
                {
                    tono = 0;
                }

                string Result_Json = "";

                Result_Json = AllwalletReport(_ReqUser, _ReqPassw, Actype, fromno, tono);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "getprofile")
            {
                string _ReqUser = ClearInject(dict["userid"].ToString());
                string _ReqPassw = ClearInject(dict["passwd"].ToString());
                string _ReqMemberID = ClearInject(dict["memberid"].ToString());

                string Result_Json = "";

                Result_Json = GetProfile(_ReqUser, _ReqPassw, _ReqMemberID);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "getmemberdata")
            {
                string _ReqMemberID = ClearInject(dict["memberid"].ToString());
                string Result_Json = "";
                Result_Json = GetmemberData(_ReqMemberID);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "setprofile")
            {
                string _ReqUser = ClearInject(dict["userid"].ToString());
                string _ReqPassw = ClearInject(dict["passwd"].ToString());
                string _ReqMemberID = ClearInject(dict["memberid"].ToString());
                string _Reqphoneno = ClearInject(dict["phno"].ToString());
                string _Gaurdian = ClearInject(dict["fname"].ToString());
                string _Dob = ClearInject(dict["dob"].ToString());
                string _Mobile = ClearInject(dict["mobile"].ToString());
                string _Email = ClearInject(dict["email"].ToString());
                string _Nominee = ClearInject(dict["nominee"].ToString());

                string _Nomrelation = "";
                try
                {
                    _Nomrelation = ClearInject(dict["nomprefix"].ToString());
                }
                catch
                {
                    // same as VB: ignore if not present
                }

                string _Relation = "";
                try
                {
                    _Relation = ClearInject(dict["nomineerelation"].ToString());
                }
                catch
                {
                    // same as VB: ignore if not present
                }

                string Result_Json = "";

                Result_Json = SetProfile(_ReqUser, _ReqPassw, _ReqMemberID, _Gaurdian, _Dob, _Email, _Nominee, _Relation, _Reqphoneno, _Mobile, _Nomrelation);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "ctpassword") // 29Apr16 NJ
            {
                string _ReqUserID = ClearInject(dict["userid"].ToString());
                string _ReqPassw = ClearInject(dict["passwd"].ToString());
                string _ReqPwd = ClearInject(dict["tpasswd"].ToString());
                string _ReqNPwd = ClearInject(dict["ntpasswd"].ToString());
                string Result_Json = "";
                Result_Json = ChangeTransactionPassword(_ReqUserID, _ReqPassw, _ReqPwd, _ReqNPwd);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "forgot") // 29Apr16 NJ
            {
                string _ReqUserID = ClearInject(dict["userid"].ToString());
                string Result_Json = "";

                Result_Json = forgot(_ReqUserID);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "complaintdetailwithno") // for complaint detail
            {
                string _ReqUser = ClearInject(dict["userid"].ToString());
                string _Reqcomplaintid = ClearInject(dict["passwd"].ToString());
                int _ReqFromNo = Convert.ToInt32(ClearInject(dict["from"].ToString()));
                int _ReqToNo = Convert.ToInt32(ClearInject(dict["to"].ToString()));
                string Result_Json = "";
                Result_Json = ComplaintDetailwithno(_ReqUser, _Reqcomplaintid, _ReqFromNo, _ReqToNo);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else if (_Reqtype == "complaintreplywith") // for View complaint Reply detail
            {
                string _ReqUser = ClearInject(dict["userid"].ToString());
                string _ReqPassw = ClearInject(dict["passwd"].ToString());
                int _ReqcomplaintId = Convert.ToInt32(ClearInject(dict["complaintid"].ToString()));
                string Result_Json = "";
                Result_Json = ComplaintReplyDetailwith(_ReqcomplaintId);
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
            else
            {
                string Result_Json = "";
                Result_Json = "{\"response\":\"FAILED\",\"msg\":\"Api Process Request Not Found.!\" }";
                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
                int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
                Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(Result_Json);
            }
        }
        catch (Exception)
        {
            string Result_Json = "";
            Result_Json = "{\"response\":\"FAILED\",\"msg\":\"Exception Case Process Not Valid.!\" }";
            string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" + Result_Json.Trim() + "' WHERE ReqID = '" + sResult.Trim() + "'";
            int x_res = Convert.ToInt32(SqlHelper.ExecuteNonQuery((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]], CommandType.Text, sql_res));
            Result_Json = Result_Json.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "");
            Response.Clear();
            Response.ContentType = "application/json";
            Response.Write(Result_Json);
        }

    }
    private string PanCardSave(string UserName, string Password, string Panno, string Panimage)
    {
        string _Output = "";
        string Remark = "";
        string formno = "0";

        try
        {
            Bool = UserExists(UserName, Password);
            formno = GetFormNo(UserName).ToString();

            if (Bool == true)
            {
                DataTable Dt1;
                int j = 0;
                DAL obj;
                string FlNm = "";
                string scrname = "";
                int i;

                obj = new DAL();

                string s1 = "";
                string Condition = "";

                DataTable Dt;
                string str;

                str = Obj.IsoStart + " Exec sp_FillKyc '" + Convert.ToInt32(formno) + "'" + Obj.IsoEnd;
                obj = new DAL();

                Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];

                if (Dt1.Rows.Count > 0)
                {
                    if (ClearInject(Dt1.Rows[0]["Panno"].ToString()) != ClearInject(Panno))
                    {
                        Remark += " PANNo,";
                    }

                    if (ClearInject(Dt1.Rows[0]["PanImg"].ToString()) != ClearInject(Panimage))
                    {
                        Remark += " PanCardImage,";
                    }
                }

                string q = "";

                string Qry = "Insert Into TempMemberMaster Select *,'Update KYC - "
                    + Context.Request.UserHostAddress.ToString() + "',GetDate(),'U' From M_MemberMaster Where FormNo='"
                    + Convert.ToInt32(formno) + "'";

                Qry += ";Insert Into TempKycVerify Select *,GetDate(),'" + formno + "' From KycVerify Where FormNo='"
                    + Convert.ToInt32(formno) + "'";

                Qry += ";insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values"
                    + "(0,'" + Dt1.Rows[0]["MemName"].ToString()
                    + "','KYC Detail','KYC Detail Update','" + Remark + "',Getdate(),'" + Convert.ToInt32(formno) + "')";

                string sql = Qry + ";Update m_MemberMaster set Panno='" + Panno.ToUpper() + "' "
                    + Condition + " where Formno='" + Convert.ToInt32(formno) + "'";

                sql += ";Update KycVerify Set PanImg='" + Panimage
                    + "',PANImgDate=Getdate(),IsPanVerified='Y',PanVerifyDate = GETDATE(),IsPan ='Y' where Formno='"
                    + Convert.ToInt32(formno) + "'";

                obj = new DAL();

                j = obj.SaveData(sql);

                if (j != 0)
                {
                    _Output = "{\"response\":\"OK\",\"msg\":\"save successfully\"}";
                }
                else
                {
                    _Output = "{\"response\":\"FAILED\"}";
                }
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        Response.Write(_Output);
        return _Output;
    }
    private string KYCADDRESSSAVE(string UserName, string Password, string Address, string pincode, string cityname, string citycode, string StateCode, string District, string DistrictCode,
        string AreaCode, string areaName, string otherareaname, string idProofid, string idproofNo, string frontAddressproof, string BackAddressproof)
    {
        string _Output = "";
        string Remark = "";
        string formno = "0";

        try
        {
            Bool = UserExists(UserName, Password);
            formno = GetFormNo(UserName).ToString();

            if (Bool == true)
            {
                DataTable Dt1;
                int j = 0;
                DAL obj;
                string FlNm = "";
                string scrname = "";
                int i;

                obj = new DAL();
                string s1 = "";
                string Condition = "";

                DataTable Dt;

                string str =
                    Obj.IsoStart + " Select a.IDNo,a.MemFirstName As MemName,a.Address1,a.City,a.Tehsil,a.District, " +
                    " a.Statecode,a.Pincode,b.IdproofNo,b.AddrProof,b.BackAddressProof,Case when b.IsAddrssverified<>'N' then " +
                    " Replace(CONVERT(varchar,b.AddrssVerifyDate,106),' ','-') Else '' End as AddrProofDate, " +
                    " b.IsAddrssverified,CASE WHEN b.IsAddrssverified='Y' THEN 'Verified' when b.IsAddrssverified='R'  " +
                    " then 'Rejected' Else 'Not Verified' END AS idVerf,Case when b.IsAddrssverified='R'  " +
                    " then b.AddrssRemark else'' end as RejectRemark,BackAddressDate,c.IdType,c.id ,a.Aadharno  " +
                    " From " + Obj.dBName + "..M_MemberMaster  AS A Inner Join " + Obj.dBName + "..KycVerify as b On a.Formno=b.formno  Inner Join  " +
                    " " + Obj.dBName + "..M_IdTypeMaster as c  On b.IdTYpe=c.Id and C.ActiveStatus='Y'  where a.Formno='" +
                    Convert.ToInt32(formno) + "'" + Obj.IsoEnd;

                obj = new DAL();
                Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];

                if (Dt1.Rows.Count > 0)
                {
                    if (ClearInject(Dt1.Rows[0]["Address1"].ToString()) != ClearInject(Address))
                        Remark += "Address ,";

                    if (ClearInject(Dt1.Rows[0]["City"].ToString()) != ClearInject(cityname))
                        Remark += " City ,";

                    if (ClearInject(Dt1.Rows[0]["District"].ToString()) != ClearInject(District))
                        Remark += " District,";

                    if (ClearInject(Dt1.Rows[0]["PinCode"].ToString()) != ClearInject(pincode))
                        Remark += " PinCode,";

                    if (ClearInject(Dt1.Rows[0]["AddrProof"].ToString()) != ClearInject(frontAddressproof))
                        Remark += " AddressProof,";

                    if (ClearInject(Dt1.Rows[0]["BackAddressProof"].ToString()) != ClearInject(BackAddressproof))
                        Remark += " BackAddressProof,";

                    if (ClearInject(Dt1.Rows[0]["id"].ToString()) != ClearInject(idProofid))
                        Remark += " AddressProofType,";

                    if (ClearInject(Dt1.Rows[0]["IdProofNo"].ToString()) != ClearInject(idproofNo))
                        Remark += " AddressProofNo,";
                }

                if (idProofid == "0")
                    return "{\"response\":\"FAILED\",\"msg\":\"choose Id Proof.\"}";

                string q = "";
                if (areaName == "OTHERS")
                {
                    if (otherareaname != "")
                    {
                        q = Obj.IsoStart + "Select * from " + Obj.dBName + "..M_VillageMaster where VillageName='" + otherareaname + "' and Activestatus='Y' and Pincode='" + Convert.ToInt32(pincode) + "'" + Obj.IsoEnd;

                        obj = new DAL();
                        Dt = new DataTable();
                        Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, q).Tables[0];
                        if (Dt.Rows.Count == 0)
                        {
                            q = "insert into M_VillageMaster (VillageName,CityCode,PinCode) Values('" +
                                otherareaname + "','" + citycode + "','" + pincode + "')";

                            i = obj.SaveData(q);

                            if (i > 0)
                            {
                                q = Obj.IsoStart + " select Max(VillageCode)as VillageCode from " + Obj.dBName + "..M_VillageMaster where ActiveStatus='Y'" + Obj.IsoEnd;
                                Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, q).Tables[0];
                                if (Dt.Rows.Count > 0)
                                    AreaCode = Dt.Rows[0]["VillageCode"].ToString();
                            }
                        }
                        else
                        {
                            AreaCode = Dt.Rows[0]["VillageCode"].ToString();
                        }
                    }
                }

                string Qry =
                    "Insert Into TempMemberMaster Select *,'Update KYC - " +
                    Context.Request.UserHostAddress +
                    "',GetDate(),'U' From M_MemberMaster Where FormNo='" + Convert.ToInt32(formno) + "'";

                Qry += ";Insert Into TempKycVerify Select *,GetDate(),'" + formno +
                       "' From KycVerify Where FormNo='" + Convert.ToInt32(formno) + "'";

                Qry += ";insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" +
                       "(0,'" + Dt1.Rows[0]["MemName"] +
                       "','KYC Detail','KYC Detail Update','" + Remark +
                       "',Getdate(),'" + Convert.ToInt32(formno) + "')";

                string sql = Qry +
                    ";Update m_MemberMaster set Address1='" + Address.ToUpper() +
                    "', Tehsil='" + cityname +
                    "',City='" + cityname.ToUpper() +
                    "',District='" + District.ToUpper() +
                    "',StateCode='" + StateCode +
                    "', Pincode='" + pincode +
                    "',AreaCode='" + AreaCode +
                    "', CityCode='" + citycode +
                    "',DistrictCode='" + DistrictCode + "'" +
                    Condition +
                    " where Formno= '" + Convert.ToInt32(formno) + "'";
                if (Session["compid"].ToString() == "1102")
                {
                    sql += ";Update KycVerify Set Idtype = '" + idProofid + "',IdProofNo = '" + idproofNo + "', AddrProof = '',BackAddressProof = ''," +
                        "BackAddressDate = Getdate(),IsaddrssVerified = 'Y',IsAddress = 'Y',AddrssVerifyDate = Getdate() where Formno= '" + Convert.ToInt32(formno) + "'";
                    
                }
                else
                {
                    sql += ";Update KycVerify Set Idtype = '" + idProofid + "',IdProofNo = '" + idproofNo + "', AddrProof = '" + frontAddressproof + "',BackAddressProof = '" + BackAddressproof +
                     "',BackAddressDate = Getdate(),IsaddrssVerified='N',IsAddress = 'N' where Formno= '" + Convert.ToInt32(formno) + "'";
                }
              
                obj = new DAL();
                j = obj.SaveData(sql);

                if (j != 0)
                    _Output = "{\"response\":\"OK\",\"msg\":\"save successfully\"}";
                else
                    _Output = "{\"response\":\"FAILED\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }
        return _Output;
    }
    private string KYCDetail(string userid, string passwd)
    {
        string _Output = "";

        try
        {
            bool Bool = UserExists(userid, passwd);

            if (Bool == true)
            {
                DataTable DtProfile = new DataTable();

                string Str =
                    Obj.IsoStart + " Select a.IDNo,a.MemFirstName As MemName,a.Address1,a.City,a.Tehsil,a.District,AreaCode," +
                    " CityCode,DistrictCode, " +
                    " a.Statecode,a.Pincode,b.IdproofNo,b.IdProof,b.AddrProof,Case when b.Isidverified<>'N' then " +
                    " Replace(CONVERT(varchar,b.idVerifyDate,106),' ','-') Else '' End as idProofDate,Case when b.IsAddrssverified<>'N' then " +
                    " Replace(CONVERT(varchar,b.AddrssVerifyDate,106),' ','-') Else '' End as AddrProofDate, " +
                    " b.IsAddrssverified,b.Isidverified,CASE WHEN b.IsAddrssverified='Y' THEN 'Verified' when b.IsAddrssverified='R'  " +
                    " then 'Rejected' Else 'Verification Due' END AS idVerf,Case when b.IsAddrssverified='R'  " +
                    " then b.AddrssRemark else'' end as RejectRemark,b.BackAddressProof,BackAddressDate,c.IdType,c.id," +
                    " Isnull(f.Reason,'')As RejectReason,D.Statename   " +
                    " From " + Obj.dBName + "..M_MemberMaster  AS A Inner Join " + Obj.dBName + "..KycVerify as b On a.Formno=b.formno " +
                    " Left Join " + Obj.dBName + "..M_KycReject as f On f.Kid=b.AddressRejectId " +
                    " Inner Join " + Obj.dBName + "..M_IdTypeMaster as c  On b.IdTYpe=c.Id and C.ActiveStatus='Y'  " +
                    " Inner Join " + Obj.dBName + "..M_StateDivMaster as d On a.Statecode=d.Statecode and d.RowStatus='Y'" +
                    " where a.idno='" + userid + "'" + Obj.IsoEnd;

                Adp = new SqlDataAdapter(Str, Connselect);
                Adp.Fill(DtProfile);

                if (DtProfile.Rows.Count > 0)
                {
                    _Output = "{"
                        + "\"idno\":\"" + DtProfile.Rows[0]["IdNo"] + "\","
                        + "\"idproof\":\"" + DtProfile.Rows[0]["IdProof"] + "\","
                        + "\"address\":\"" + DtProfile.Rows[0]["Address1"] + "\","
                        + "\"pincode\":\"" + DtProfile.Rows[0]["Pincode"] + "\","
                        + "\"city\":\"" + DtProfile.Rows[0]["City"] + "\","
                        + "\"district\":\"" + DtProfile.Rows[0]["District"] + "\","
                        + "\"statecode\":\"" + DtProfile.Rows[0]["Statecode"] + "\","
                        + "\"idproofdate\":\"" + DtProfile.Rows[0]["IdProofDate"] + "\","
                        + "\"idverf\":\"" + DtProfile.Rows[0]["idVerf"] + "\","
                        + "\"addrproof\":\"" + DtProfile.Rows[0]["AddrProof"] + "\","
                        + "\"addrproofdate\":\"" + DtProfile.Rows[0]["AddrProofDate"] + "\","
                        + "\"IdproofNo\":\"" + DtProfile.Rows[0]["IdproofNo"] + "\","
                        + "\"addrsverf\":\"" + DtProfile.Rows[0]["IsAddrssverified"] + "\","
                        + "\"backaddressproof\":\"" + DtProfile.Rows[0]["BackAddressProof"] + "\","
                        + "\"backaddressdate\":\"" + DtProfile.Rows[0]["BackAddressDate"] + "\","
                        + "\"idtype\":\"" + DtProfile.Rows[0]["IdType"] + "\","
                        + "\"rejectremark\":\"" + DtProfile.Rows[0]["RejectRemark"] + "\","
                        + "\"rejectreason\":\"" + DtProfile.Rows[0]["RejectReason"] + "\","
                        + "\"areacode\":\"" + DtProfile.Rows[0]["AreaCode"] + "\","
                        + "\"statename\":\"" + DtProfile.Rows[0]["StateName"] + "\","
                        + "\"response\":\"OK\""
                        + "}";
                }
                else
                {
                    _Output = "{\"response\":\"FAILED\"}";
                }
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    private int CheckFormno(string formno)
    {
        using (SqlCommand cmd = new SqlCommand(Obj.IsoStart + "SELECT COUNT(*) FROM " + Obj.dBName + "..M_FormUpload WHERE Formno = @Formno" + Obj.IsoEnd, Connselect))
        {
            cmd.Parameters.AddWithValue("@Formno", formno);

            if (Conn.State == ConnectionState.Closed)
                Conn.Open();

            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count > 0 ? 1 : 0;
        }
    }
    private string FormUpload(string userid, string passwd, string Front)
    {
        string _output = "";
        string Sql = "";

        try
        {
            bool Bool = UserExists(userid, passwd);
            int FormNo = GetFormNo(ClearInject(userid));

            if (Bool == true)
            {
                if (Session["compid"].ToString() == "1102")
                {
                    if (CheckFormno(FormNo.ToString()) == 0)
                    {
                        Sql = "INSERT INTO M_FormUpload(Formno, FrontSideForm, ActiveStatus, Remark, AdminRemark,IsForm)VALUES ('" + FormNo + "','" + Front + "','N','','','N')";
                    }
                    if (CheckFormno(FormNo.ToString()) == 1)
                    {
                        Sql = "UPDATE M_FormUpload SET FrontSideForm = '" + Front + "',ActiveStatus = 'N',IsForm ='N' WHERE Formno = '" + FormNo + "'";
                    }
                }
                else
                {
                    if (CheckFormno(FormNo.ToString()) == 0)
                    {
                        Sql = "INSERT INTO M_FormUpload(Formno, FrontSideForm, ActiveStatus, Remark, AdminRemark)VALUES ('" + FormNo + "','" + Front + "','N','','')";
                    }
                    if (CheckFormno(FormNo.ToString()) == 1)
                    {
                        Sql = "UPDATE M_FormUpload SET FrontSideForm = '" + Front + "',ActiveStatus = 'N' WHERE Formno = '" + FormNo + "'";
                    }
                }
                using (SqlCommand Cmd = new SqlCommand(Sql, Conn))
                {
                    int UpdtEffect = Cmd.ExecuteNonQuery();

                    if (UpdtEffect == 0)
                    {
                        _output = "{\"response\":\"FAILED\",\"msg\":\"Form Upload unsuccessfuly.\"}";
                    }
                    else
                    {
                        _output = "{\"response\":\"OK\",\"msg\":\"Form Upload successfuly.\"}";
                    }
                }
            }
            else
            {
                _output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            _output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _output;
    }
    public string forgot(string userID)
    {
        try
        {
            string MemberPass = "";
            string MemberTransPassw = "";

            Comm = new SqlCommand(
               Obj.IsoStart + "SELECT * FROM " + Obj.dBName + "..M_MemberMaster " +
                "WHERE (Email = @UserID OR CAST(Mobl AS VARCHAR) = @UserID OR IdNo = @UserID)" + Obj.IsoEnd,
                Connselect);

            Comm.Parameters.AddWithValue("@UserID", userID);

            if (Conn.State == ConnectionState.Closed)
                Conn.Open();

            Dr = Comm.ExecuteReader();
            bool Bool = Dr.Read();

            if (Bool == true)
            {
                string userEmail = Dr["Email"].ToString();
                string userMobl = Dr["Mobl"].ToString();

                MemberPass = Dr["Passw"].ToString();
                MemberTransPassw = Dr["EPassw"].ToString();

                // ===== URL encode passwords (same replacements as VB) =====
                MemberPass = HttpUtility.UrlEncode(MemberPass);
                MemberTransPassw = HttpUtility.UrlEncode(MemberTransPassw);

                // ===== Special CompID 1093 logic =====
                if (Session["CompID"].ToString() == "1093")
                {
                    string mobl = "", Password1 = "", url1 = "", Username = "";

                    string str = Obj.IsoStart + "EXEC Sp_MemberForgotPassw @UserID" + Obj.IsoEnd;

                    SqlParameter[] param =
                    {
                    new SqlParameter("@UserID", userID)
                };

                    DataTable Dt = SqlHelper.ExecuteDataset(
                                        HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                                        CommandType.Text,
                                        str,
                                        param
                                    ).Tables[0];

                    if (Dt.Rows.Count > 0)
                    {
                        Username = Dt.Rows[0]["Idno"].ToString();
                        Password1 = Dt.Rows[0]["Passw"].ToString();
                        mobl = Dt.Rows[0]["mobl"].ToString();
                        url1 = Dt.Rows[0]["website"].ToString();
                    }



                    return "{\"response\":\"OK\",\"msg\":\"Success\",\"isuser\":\"Y\"}";
                }
                else
                {
                    // ===== SMS & EMAIL logic =====
                    string baseurl = "";
                    string wMsg =
                        "<table style=\"margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%\">" +
                        "<tr><td>" +
                        "<span style=\"color:#0099CC;font-weight:bold;\"><h2>Dear " + Dr["MemFirstName"] + ",</h2></span><br/>" +
                        "Welcome Back to your Account.<br/>" +
                        "<strong>Your Login password is : " + Dr["Passw"] +
                        " and Transaction password is " + Dr["EPassw"] +
                        " for Customer ID " + Dr["Idno"] + "</strong><br/>" +
                        "Login On: <a href=\"" + Session["CompWeb"] +
                        "\" target=\"_blank\">" + Session["CompWeb"] + "</a><br/>" +
                        "</td></tr></table>";

                    Dr.Close();

                    string isSmsSent = "N";
                    string isMailSent = "N";
                    string _MailHead = "Forgot Password";

                    // ===== Mail sending =====
                    if (Session["CompID"].ToString() == "1057" ||
                        Session["CompID"].ToString() == "1077" ||
                        Session["CompID"].ToString() == "1074" ||
                        Session["CompID"].ToString() == "1096")
                    {
                        if (!string.IsNullOrEmpty(userEmail))
                        {
                            if (SendMail(userEmail, wMsg, _MailHead, "FORGOT"))
                                isMailSent = "Y";
                        }
                    }

                    // ===== SMS sending =====
                    if (long.TryParse(userMobl, out _) && userMobl.Length >= 10)
                    {

                    }

                    return "{\"response\":\"OK\",\"msg\":\"Success\",\"isuser\":\"Y\"," +
                           "\"ismailsent\":\"" + isMailSent + "\"," +
                           "\"issmssent\":\"" + isSmsSent + "\"}";
                }
            }
            else
            {
                return "{\"response\":\"FAILED\",\"isuser\":\"N\",\"msg\":\"Not Sent.\"}";
            }
        }
        catch (Exception ex)
        {
            return "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }
    }
    public bool SendMail(string MailID, string Msg, string MsgHead, string MsgType)
    {
        try
        {
            if (!string.IsNullOrEmpty(_MailHost) &&
                !string.IsNullOrEmpty(_MailID) &&
                !string.IsNullOrEmpty(_MailPass))
            {
                MailAddress SendFrom = new MailAddress(_MailID);
                MailAddress SendTo = new MailAddress(MailID);

                MailMessage MyMessage = new MailMessage(SendFrom, SendTo);
                MyMessage.Subject = MsgHead;
                MyMessage.Body = Msg;
                MyMessage.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient(_MailHost);
                smtp.Credentials = new NetworkCredential(_MailID, _MailPass);

                // smtp.Port = 25;
                smtp.Port = 587; // added on 02 May 2022

                if (Session["compid"].ToString() != "1074")
                {
                    smtp.EnableSsl = true;
                }

                smtp.Send(MyMessage);
                return true;
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }

        return false;
    }
    public string ChangeTransactionPassword(string UserID, string Passwd, string OldPasswd, string NewPasswd)
    {
        string _Output = "";

        try
        {
            if (UserExists(UserID, Passwd))
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                int IsMemExist = 0;

                // ===== Check old transaction password =====
                Comm = new SqlCommand(
                   Obj.IsoStart + "SELECT 1 FROM " + Obj.dBName + "..M_MemberMaster WHERE IDNo = @UserID AND EPassw = @OldPasswd" + Obj.IsoEnd,
                    Connselect);

                Comm.Parameters.AddWithValue("@UserID", UserID);
                Comm.Parameters.AddWithValue("@OldPasswd", OldPasswd);

                Dr = Comm.ExecuteReader();

                if (Dr.Read())
                {
                    IsMemExist = 1;
                }
                else
                {
                    _Output = "{\"response\":\"Incorrect Transaction Password\"}";
                }

                Dr.Close();

                // ===== Update transaction password =====
                if (IsMemExist == 1)
                {
                    string strQry =
                        "INSERT INTO TempMemberMaster " +
                        "SELECT *,'Transaction Password updated through App',GETDATE(),'C' " +
                        "FROM M_MemberMaster WHERE IDNo = @UserID; " +
                        "UPDATE M_MemberMaster SET EPassw = @NewPasswd WHERE IDNO = @UserID;";

                    if (Conn.State == ConnectionState.Closed)
                        Conn.Open();

                    SqlCommand Comm_ = new SqlCommand(strQry, Conn);
                    Comm_.Parameters.AddWithValue("@UserID", UserID);
                    Comm_.Parameters.AddWithValue("@NewPasswd", NewPasswd);

                    if (Comm_.ExecuteNonQuery() != 0)
                    {
                        _Output = "{\"response\":\"OK\",\"msg\":\"Success\"}";
                    }
                    else
                    {
                        _Output = "{\"response\":\"FAILED\",\"msg\":\"Password Not Changed.\"}";
                    }
                }

                if (Comm != null)
                    Comm.Cancel();
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            if (Conn != null && Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }

            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    private string SetProfile(string userid, string passwd, string memberid, string Gaurdian, string Dob, string Email, string Nominee, string Relation, string Phoneno, string Mobile = "0", string nomprefix = "")
    {
        string _Output = "";

        try
        {
            bool Bool = UserExists(userid, passwd);

            if (Bool == true)
            {
                if (string.IsNullOrEmpty(Relation))
                {
                    return "{\"response\":\"FAILED\",\"msg\":\"Please enter Relation.\"}";
                }

                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                string sql =
                    "UPDATE M_MemberMaster SET " +
                    "MemFName=@Gaurdian, " +
                    "MemDob=@Dob, " +
                    "Email=@Email, " +
                    "NomineeName=@Nominee, " +
                    "PhN1=@Phoneno, " +
                    "MemRelation = @NomPrefix, " +
                    "Relation=@Relation";

                if (Mobile != "0")
                {
                    sql += ", Mobl=@Mobile";
                }

                sql += " WHERE IDNo=@MemberID";

                Comm = new SqlCommand(sql, Conn);
                Comm.Parameters.AddWithValue("@Gaurdian", Gaurdian);
                Comm.Parameters.AddWithValue("@Dob", Dob);
                Comm.Parameters.AddWithValue("@Email", Email);
                Comm.Parameters.AddWithValue("@Nominee", Nominee);
                Comm.Parameters.AddWithValue("@Phoneno", Phoneno);
                Comm.Parameters.AddWithValue("@NomPrefix", nomprefix);
                Comm.Parameters.AddWithValue("@Relation", Relation);
                Comm.Parameters.AddWithValue("@MemberID", memberid);

                if (Mobile != "0")
                    Comm.Parameters.AddWithValue("@Mobile", Mobile);

                int i = Comm.ExecuteNonQuery();

                if (i != 0)
                {
                    _Output = "{\"response\":\"OK\"}";
                }
                else
                {
                    _Output = "{\"response\":\"FAILED\"}";
                }

                if (Comm != null)
                    Comm.Cancel();
            }
            else
            {
                // kept exactly as VB (duplicate msg key)
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\",\"msg\":\"Success\"}";
            }

            if (Comm != null)
                Comm.Cancel();
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    private string GetmemberData(string memberid)
    {
        string _Output = "";

        try
        {
            bool Bool = true;

            if (Bool == true)
            {
                DataTable DtProfile = new DataTable();

                string sql =
                   Obj.IsoStart + "SELECT a.FormNo, a.IdNo, " +
                    "a.MemFirstName + ' ' + a.MemLastName AS MemName, " +
                    "a.MemFName, " +
                    "REPLACE(CONVERT(VARCHAR, a.MemDob, 106), ' ', '-') AS MemDob, " +
                    "REPLACE(CONVERT(VARCHAR, a.Doj, 106), ' ', '-') AS Doj, " +
                    "REPLACE(CONVERT(VARCHAR, a.Upgradedate, 106), ' ', '-') AS Upgradedate, " +
                    "a.Mobl, a.Email, a.NomineeName, a.Relation, a.Fld5, " +
                    "CASE WHEN a.Legno = 1 THEN 'Left' ELSE 'Right' END AS position, " +
                    "a.MemRelation, a.PhN1, ISNULL(b.Idno, '') AS sponsorid " +
                    "FROM " + Obj.dBName + "..M_MemberMaster a " +
                    "LEFT JOIN " + Obj.dBName + "..M_MemberMaster b ON a.refformno = b.formno " +
                    "WHERE a.IDNo = @MemberID" + Obj.IsoEnd;

                Adp = new SqlDataAdapter(sql, Connselect);
                Adp.SelectCommand.Parameters.AddWithValue("@MemberID", memberid);
                Adp.Fill(DtProfile);

                if (DtProfile.Rows.Count > 0)
                {
                    DataRow r = DtProfile.Rows[0];

                    _Output =
                        "{"
                        + "\"idno\":\"" + r["IdNo"].ToString() + "\","
                        + "\"name\":\"" + r["MemName"].ToString() + "\","
                        + "\"position\":\"" + r["position"].ToString() + "\","
                        + "\"relation\":\"" + r["MemRelation"].ToString() + "\","
                        + "\"fname\":\"" + r["MemFName"].ToString() + "\","
                        + "\"dob\":\"" + r["MemDob"].ToString() + "\","
                        + "\"mobile\":\"" + r["Mobl"].ToString() + "\","
                        + "\"phoneno\":\"" + r["PhN1"].ToString() + "\","
                        + "\"email\":\"" + r["Email"].ToString() + "\","
                        + "\"nominee\":\"" + r["NomineeName"].ToString() + "\","
                        + "\"nomineerelation\":\"" + r["Relation"].ToString() + "\","
                        + "\"dateofjoining\":\"" + r["Doj"].ToString() + "\","
                        + "\"dateofactivation\":\"" + r["Upgradedate"].ToString() + "\","
                        + "\"sponsorid\":\"" + r["sponsorid"].ToString() + "\","
                        + "\"nomprefix\":\"" + r["Fld5"].ToString() + "\","
                        + "\"response\":\"OK\""
                        + "}";
                }
                else
                {
                    _Output = "{\"response\":\"FAILED\"}";
                }

                if (Comm != null)
                    Comm.Cancel();
            }
            else
            {
                // preserved exactly as VB (note duplicate msg key)
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\",\"msg\":\"Success\"}";
            }

            if (Comm != null)
                Comm.Cancel();
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    private string GetProfile(string userid, string passwd, string memberid)
    {
        string _Output = "";

        try
        {
            bool Bool = UserExists(userid, passwd);

            if (Bool == true)
            {
                DataTable DtProfile = new DataTable();

                string sql =
                   Obj.IsoStart + "SELECT a.FormNo, a.IdNo, " +
                    "a.MemFirstName + ' ' + a.MemLastName AS MemName, " +
                    "a.MemFName, " +
                    "REPLACE(CONVERT(VARCHAR, a.MemDob, 106), ' ', '-') AS MemDob, " +
                    "REPLACE(CONVERT(VARCHAR, a.Doj, 106), ' ', '-') AS Doj, " +
                    "REPLACE(CONVERT(VARCHAR, a.Upgradedate, 106), ' ', '-') AS Upgradedate, " +
                    "a.Mobl, a.Email, a.NomineeName, a.Relation, a.Fld5, " +
                    "CASE WHEN a.Legno = 1 THEN 'Left' ELSE 'Right' END AS position, " +
                    "a.MemRelation, a.PhN1, ISNULL(b.Idno, '') AS sponsorid " +
                    "FROM " + Obj.dBName + "..M_MemberMaster a " +
                    "LEFT JOIN " + Obj.dBName + "..M_MemberMaster b ON a.refformno = b.formno " +
                    "WHERE a.IDNo = @MemberID" + Obj.IsoEnd;

                Adp = new SqlDataAdapter(sql, Connselect);
                Adp.SelectCommand.Parameters.AddWithValue("@MemberID", memberid);
                Adp.Fill(DtProfile);

                if (DtProfile.Rows.Count > 0)
                {
                    DataRow r = DtProfile.Rows[0];

                    _Output =
                        "{"
                        + "\"idno\":\"" + r["IdNo"].ToString() + "\","
                        + "\"name\":\"" + r["MemName"].ToString() + "\","
                        + "\"position\":\"" + r["position"].ToString() + "\","
                        + "\"relation\":\"" + r["MemRelation"].ToString() + "\","
                        + "\"fname\":\"" + r["MemFName"].ToString() + "\","
                        + "\"dob\":\"" + r["MemDob"].ToString() + "\","
                        + "\"mobile\":\"" + r["Mobl"].ToString() + "\","
                        + "\"phoneno\":\"" + r["PhN1"].ToString() + "\","
                        + "\"email\":\"" + r["Email"].ToString() + "\","
                        + "\"nominee\":\"" + r["NomineeName"].ToString() + "\","
                        + "\"nomineerelation\":\"" + r["Relation"].ToString() + "\","
                        + "\"dateofjoining\":\"" + r["Doj"].ToString() + "\","
                        + "\"dateofactivation\":\"" + r["Upgradedate"].ToString() + "\","
                        + "\"sponsorid\":\"" + r["sponsorid"].ToString() + "\","
                        + "\"nomprefix\":\"" + r["Fld5"].ToString() + "\","
                        + "\"response\":\"OK\""
                        + "}";
                }
                else
                {
                    _Output = "{\"response\":\"FAILED\"}";
                }

                if (Comm != null)
                    Comm.Cancel();
            }
            else
            {
                // preserved exactly as VB (note duplicate msg key)
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\",\"msg\":\"Success\"}";
            }

            if (Comm != null)
                Comm.Cancel();
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    public string AllWalletList(string userid, string passwd)
    {
        string _Output = "";

        try
        {
            DataTable DtCountry = new DataTable();
            int FormNo = GetFormNo(ClearInject(userid));

            _Output = "{\"wallet\":[";

            string str = "";

            if (Session["compid"].ToString() == "1096")
            {
                str = Obj.IsoStart + "EXEC sp_WalletTypeUpdate @FormNo" + Obj.IsoEnd;
            }
            else
            {
                str = Obj.IsoStart + "exec sp_WalletType" + Obj.IsoEnd;
            }

            Adp = new SqlDataAdapter(str, Connselect);

            if (str.Contains("@FormNo"))
                Adp.SelectCommand.Parameters.AddWithValue("@FormNo", FormNo);

            Adp.Fill(DtCountry);

            if (DtCountry.Rows.Count > 0)
            {
                foreach (DataRow Dr in DtCountry.Rows)
                {
                    _Output += "{"
                            + "\"actype\":\"" + Dr["actype"].ToString() + "\","
                            + "\"walletname\":\"" + Dr["walletname"].ToString() + "\""
                            + "},";
                }

                _Output = _Output.Remove(_Output.Length - 1, 1);
            }

            _Output += "],\"response\":\"OK\",\"msg\":\"Success\"}";

            if (Comm != null)
                Comm.Cancel();
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    private string AllwalletReport(string userid, string passwd, string Actype, int FromNo, int ToNo)
    {
        string _Output = "";

        try
        {
            bool Bool = UserExists(userid, passwd);

            if (Bool == true)
            {
                int FormNo = GetFormNo(ClearInject(userid));
                DataTable DtCountry = new DataTable();
                int RecordCount = 0;
                string col;

                _Output = "{\"status\":[";

                strQry = Obj.IsoStart + "EXEC Sp_WalletrReportApp @Actype,@FormNo,@FromNo,@ToNo" + Obj.IsoEnd;

                SqlParameter[] param =
                {
                new SqlParameter("@Actype", Actype),
                new SqlParameter("@FormNo", FormNo),
                new SqlParameter("@FromNo", FromNo),
                new SqlParameter("@ToNo", ToNo)
            };

                DataSet ds1 = SqlHelper.ExecuteDataset(
                                    HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                                    CommandType.Text,
                                    strQry,
                                    param
                                );

                DataTable dt = new DataTable();

                if (ds1.Tables[0].Rows.Count > 0)
                    dt = ds1.Tables[0];

                if (ds1.Tables[1].Rows.Count > 0)
                    DtCountry = ds1.Tables[1];

                // ================= STATUS DATA =================
                foreach (DataRow Dr in dt.Rows)
                {
                    col = "{";

                    foreach (DataColumn column in dt.Columns)
                    {
                        string value = Dr[column] == DBNull.Value ? "0" : Dr[column].ToString();
                        col += "\"" + column.ColumnName + "\":\"" + value + "\",";
                    }

                    col = col.Remove(col.Length - 1, 1) + "},";
                    _Output += col;
                }

                _Output = _Output.Remove(_Output.Length - 1, 1);
                _Output += "],\"wallet\":[";

                // ================= WALLET DATA =================
                foreach (DataRow Dr in DtCountry.Rows)
                {
                    col = "{";

                    foreach (DataColumn column in DtCountry.Columns)
                    {
                        string value = Dr[column] == DBNull.Value ? "0" : Dr[column].ToString();
                        col += "\"" + column.ColumnName + "\":\"" + value + "\",";
                    }

                    col = col.Remove(col.Length - 1, 1) + "},";
                    _Output += col;
                }

                if (DtCountry.Rows.Count > 0)
                    _Output = _Output.Remove(_Output.Length - 1, 1);

                RecordCount = Convert.ToInt32(ds1.Tables[2].Rows[0]["recordCount"]);

                _Output += "],\"recordcount\":\"" + RecordCount +
                           "\",\"response\":\"OK\",\"msg\":\"Success\"}";

                if (Comm != null)
                    Comm.Cancel();
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }

            if (Comm != null)
                Comm.Cancel();
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    private string ComplaintReplyDetailwith(int complaintid)
    {
        string _Output = "";

        try
        {
            bool Bool = true;

            if (Bool == true)
            {
                DataTable DtCountry = new DataTable();

                _Output = "{\"complaintreplydetail\":[";

                strQry =
                   Obj.IsoStart + "SELECT M.IDNo, M.MemName, " +
                    "ISNULL(REPLACE(CONVERT(VARCHAR, M.RecTimeStamp, 106), ' ', '-'), '') AS CDate, " +
                    "M.CType, M.Complaint, " +
                    "ISNULL(S.Solution, '') AS Solution, " +
                    "ISNULL(REPLACE(CONVERT(VARCHAR, S.RecTimeStamp, 106), ' ', '-'), '') AS SDate " +
                    "FROM ( " +
                    "   SELECT b.MemFirstName + ' ' + b.MemLastName AS MemName, a.* " +
                    "   FROM " + Obj.dBName + "..M_ComplaintMaster a " +
                    "   INNER JOIN " + Obj.dBName + "..M_MemberMaster b ON a.IDNo = b.IDNo " +
                    "   WHERE a.CID = @CID " +
                    ") M " +
                    "LEFT JOIN " + Obj.dBName + "..M_SolutionMaster S ON M.CID = S.CID" + Obj.IsoEnd;

                Adp = new SqlDataAdapter(strQry, Connselect);
                Adp.SelectCommand.Parameters.AddWithValue("@CID", complaintid);
                Adp.Fill(DtCountry);

                // ===== Reply list =====
                if (DtCountry.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtCountry.Rows)
                    {
                        _Output += "{"
                            + "\"replydate\":\"" + Dr["SDate"].ToString() + "\","
                            + "\"reply\":\"" + Dr["Solution"].ToString() + "\""
                            + "},";
                    }

                    // remove last comma
                    _Output = _Output.Remove(_Output.Length - 1, 1);
                }

                // ===== Complaint header (same logic as VB) =====
                if (DtCountry.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtCountry.Rows)
                    {
                        _Output += "],"
                            + "\"complainttype\":\"" + Dr["CType"].ToString() + "\","
                            + "\"complaint\":\"" + Dr["Complaint"].ToString() + "\"}";

                        if (Comm != null)
                            Comm.Cancel();
                    }

                    // VB had this remove; preserved exactly
                    _Output = _Output.Remove(_Output.Length - 1, 1);
                }

                _Output += ",\"response\":\"OK\",\"msg\":\"Success\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }

            if (Comm != null)
                Comm.Cancel();
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    private string ComplaintReplyDetail(string userid, string passwd, int complaintid)
    {
        string _Output = "";

        try
        {
            bool Bool = UserExists(userid, passwd);

            if (Bool == true)
            {
                DataTable DtCountry = new DataTable();

                _Output = "{\"complaintreplydetail\":[";

                strQry =
                   Obj.IsoStart + "SELECT M.IDNo, M.MemName, " +
                    "ISNULL(REPLACE(CONVERT(VARCHAR, M.RecTimeStamp, 106), ' ', '-'), '') AS CDate, " +
                    "M.CType, M.Complaint, " +
                    "ISNULL(S.Solution, '') AS Solution, " +
                    "ISNULL(REPLACE(CONVERT(VARCHAR, S.RecTimeStamp, 106), ' ', '-'), '') AS SDate " +
                    "FROM ( " +
                    "   SELECT b.MemFirstName + ' ' + b.MemLastName AS MemName, a.* " +
                    "   FROM " + Obj.dBName + "..M_ComplaintMaster a " +
                    "   INNER JOIN " + Obj.dBName + "..M_MemberMaster b ON a.IDNo = b.IDNo " +
                    "   WHERE a.CID = @CID " +
                    ") M " +
                    "LEFT JOIN " + Obj.dBName + "..M_SolutionMaster S ON M.CID = S.CID" + Obj.IsoEnd;

                Adp = new SqlDataAdapter(strQry, Connselect);
                Adp.SelectCommand.Parameters.AddWithValue("@CID", complaintid);
                Adp.Fill(DtCountry);

                // ===== Reply list =====
                if (DtCountry.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtCountry.Rows)
                    {
                        _Output += "{"
                            + "\"replydate\":\"" + Dr["SDate"].ToString() + "\","
                            + "\"reply\":\"" + Dr["Solution"].ToString() + "\""
                            + "},";
                    }

                    // remove last comma
                    _Output = _Output.Remove(_Output.Length - 1, 1);
                }

                // ===== Complaint header (same logic as VB) =====
                if (DtCountry.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtCountry.Rows)
                    {
                        _Output += "],"
                            + "\"complainttype\":\"" + Dr["CType"].ToString() + "\","
                            + "\"complaint\":\"" + Dr["Complaint"].ToString() + "\"}";

                        if (Comm != null)
                            Comm.Cancel();
                    }

                    // VB had this remove; preserved exactly
                    _Output = _Output.Remove(_Output.Length - 1, 1);
                }

                _Output += ",\"response\":\"OK\",\"msg\":\"Success\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }

            if (Comm != null)
                Comm.Cancel();
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    private string SaveComplaintWithout(string userid, string passwd, string Idno, string name, string mobileno, string email, string compaintid, string subject, string description)
    {
        string _output = "";

        try
        {
            bool Bool = true;

            if (Bool == true)
            {
                // ================= VALIDATIONS =================
                if (string.IsNullOrWhiteSpace(Idno))
                    return "{\"response\":\"FAILED\",\"msg\":\"Please enter Complaint ID.\"}";
                if (string.IsNullOrWhiteSpace(name))
                    return "{\"response\":\"FAILED\",\"msg\":\"Please enter name.\"}";

                if (string.IsNullOrWhiteSpace(mobileno))
                    return "{\"response\":\"FAILED\",\"msg\":\"Please enter mobileno.\"}";

                if (string.IsNullOrWhiteSpace(email))
                    return "{\"response\":\"FAILED\",\"msg\":\"Please enter email id.\"}";

                if (compaintid == "0")
                    return "{\"response\":\"FAILED\",\"msg\":\"Please select nature of grievance.\"}";

                if (string.IsNullOrWhiteSpace(subject))
                    return "{\"response\":\"FAILED\",\"msg\":\"Please enter subject.\"}";

                if (string.IsNullOrWhiteSpace(description))
                    return "{\"response\":\"FAILED\",\"msg\":\"Please enter description.\"}";

                DataTable Dt = new DataTable();
                string Sql = Obj.IsoStart + "Select * from " + Obj.dBName + "..M_membermaster where Idno = '" + Idno + "' " + Obj.IsoEnd;
                Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Sql).Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    // ================= INSERT COMPLAINT =================
                    string sql =
                        "INSERT INTO M_ComplaintMaster " +
                        "(IDNO, CTypeID, CType, Complaint, Subject, MemberName, Mobileno, Email) " +
                        "SELECT @IDNO, CtypeId, CType, @Complaint, @Subject, @MemberName, @Mobileno, @Email " +
                        "FROM M_ComplaintTypeMaster " +
                        "WHERE RowStatus='Y' AND ActiveStatus='Y' AND CTypeId=@CTypeId";

                    if (Conn.State == ConnectionState.Closed)
                        Conn.Open();

                    SqlCommand Cmd = new SqlCommand(sql, Conn);
                    Cmd.Parameters.AddWithValue("@IDNO", Idno);
                    Cmd.Parameters.AddWithValue("@CTypeId", compaintid);
                    Cmd.Parameters.AddWithValue("@Complaint", description.Trim());
                    Cmd.Parameters.AddWithValue("@Subject", subject.Trim());
                    Cmd.Parameters.AddWithValue("@MemberName", name.Trim());
                    Cmd.Parameters.AddWithValue("@Mobileno", mobileno.Trim());
                    Cmd.Parameters.AddWithValue("@Email", email.Trim());

                    int UpdtEffect = Cmd.ExecuteNonQuery();

                    string ComplaintNo = "";   // same as VB (not assigned here)
                    string ComplaintName = ""; // same as VB

                    if (UpdtEffect == 0)
                    {
                        _output = "{\"response\":\"FAILED\",\"msg\":\"Complaint not sent .\"}";
                    }
                    else
                    {
                        // ================= NOTIFICATIONS =================
                        //SendAdminMail(compaintid, name, subject, description, ComplaintNo, ComplaintName);

                        if (mobileno.Length >= 10)
                        {
                            string sms =
                                "Hi " + name +
                                ", Your complaint has been received and complaint no. is " +
                                ComplaintNo +
                                ". Regards " + Session["CompName"];
                        }
                        string sql1 = Obj.IsoStart + "select Top 1 Cid from " + Obj.dBName + "..M_ComplaintMaster where Idno = '" + Idno + "' and CTypeId=" + compaintid + " Order By Cid Desc " + Obj.IsoEnd;
                        DataTable dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql1).Tables[0];
                        if (dt.Rows.Count > 0)
                            ComplaintNo = dt.Rows[0]["CId"].ToString();
                        _output =
                            "{\"response\":\"OK\",\"msg\":\"Your complaint has been successfully submitted on " +
                            DateTime.Now.ToString(" dd MMMM yyyy,hh:mm dddd") +
                            ". Your Complaint No. is " + ComplaintNo +
                            ". Our customer service representative will get in touch with you shortly. Keep patience.\"}";
                    }
                }
                else
                {
                    _output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Complaint ID.\"}";
                }
            }
            else
            {
                _output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }

            if (Comm != null)
                Comm.Cancel();
        }
        catch (Exception ex)
        {
            _output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _output;
    }
    private string SaveComplaint(string userid, string passwd, string Idno, string name, string mobileno, string email, string compaintid, string subject, string description)
    {
        string _output = "";

        try
        {
            bool Bool = UserExists(userid, passwd);

            if (Bool == true)
            {
                // ================= VALIDATIONS =================
                if (string.IsNullOrWhiteSpace(name))
                    return "{\"response\":\"FAILED\",\"msg\":\"Please enter name.\"}";

                if (string.IsNullOrWhiteSpace(mobileno))
                    return "{\"response\":\"FAILED\",\"msg\":\"Please enter mobileno.\"}";

                if (string.IsNullOrWhiteSpace(email))
                    return "{\"response\":\"FAILED\",\"msg\":\"Please enter email id.\"}";

                if (compaintid == "0")
                    return "{\"response\":\"FAILED\",\"msg\":\"Please select nature of grievance.\"}";

                if (string.IsNullOrWhiteSpace(subject))
                    return "{\"response\":\"FAILED\",\"msg\":\"Please enter subject.\"}";

                if (string.IsNullOrWhiteSpace(description))
                    return "{\"response\":\"FAILED\",\"msg\":\"Please enter description.\"}";

                // ================= INSERT COMPLAINT =================
                string sql =
                    "INSERT INTO M_ComplaintMaster " +
                    "(IDNO, CTypeID, CType, Complaint, Subject, MemberName, Mobileno, Email) " +
                    "SELECT @IDNO, CtypeId, CType, @Complaint, @Subject, @MemberName, @Mobileno, @Email " +
                    "FROM M_ComplaintTypeMaster " +
                    "WHERE RowStatus='Y' AND ActiveStatus='Y' AND CTypeId=@CTypeId";

                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                SqlCommand Cmd = new SqlCommand(sql, Conn);
                Cmd.Parameters.AddWithValue("@IDNO", Idno);
                Cmd.Parameters.AddWithValue("@CTypeId", compaintid);
                Cmd.Parameters.AddWithValue("@Complaint", description.Trim());
                Cmd.Parameters.AddWithValue("@Subject", subject.Trim());
                Cmd.Parameters.AddWithValue("@MemberName", name.Trim());
                Cmd.Parameters.AddWithValue("@Mobileno", mobileno.Trim());
                Cmd.Parameters.AddWithValue("@Email", email.Trim());

                int UpdtEffect = Cmd.ExecuteNonQuery();

                string ComplaintNo = "";   // same as VB (not assigned here)
                string ComplaintName = ""; // same as VB

                if (UpdtEffect == 0)
                {
                    _output = "{\"response\":\"FAILED\",\"msg\":\"Complaint not sent .\"}";
                }
                else
                {
                    // ================= NOTIFICATIONS =================
                    //SendAdminMail(compaintid, name, subject, description, ComplaintNo, ComplaintName);

                    if (mobileno.Length >= 10)
                    {
                        string sms =
                            "Hi " + name +
                            ", Your complaint has been received and complaint no. is " +
                            ComplaintNo +
                            ". Regards " + Session["CompName"];
                    }
                    string sql1 = Obj.IsoStart + "select Top 1 Cid from " + Obj.dBName + "..M_ComplaintMaster where Idno = '" + Idno + "' and CTypeId=" + compaintid + " Order By Cid Desc " + Obj.IsoEnd;
                    DataTable dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql1).Tables[0];
                    if (dt.Rows.Count > 0)
                        ComplaintNo = dt.Rows[0]["CId"].ToString();
                    _output =
                        "{\"response\":\"OK\",\"msg\":\"Your complaint has been successfully submitted on " +
                        DateTime.Now.ToString(" dd MMMM yyyy,hh:mm dddd") +
                        ". Your Complaint No. is " + ComplaintNo +
                        ". Our customer service representative will get in touch with you shortly. Keep patience.\"}";
                }
            }
            else
            {
                _output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }

            if (Comm != null)
                Comm.Cancel();
        }
        catch (Exception ex)
        {
            _output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _output;
    }
    private string ComplaintDetailwithno(string userid, string complaintid, int FromNo, int ToNo)
    {
        string _Output = "";
        try
        {
            bool Bool = true;

            if (Bool == true)
            {
                DataTable DtCountry = new DataTable();
                string Recordcount = "0";

                // ================= RECORD COUNT =================
                strQry =
                    Obj.IsoStart + "SELECT COUNT(*) AS TotalCount FROM " +
                    "( " +
                    "   SELECT b.MemFirstName + ' ' + b.MemLastName AS MemName, a.* " +
                    "   FROM " + Obj.dBName + "..M_ComplaintMaster a " +
                    "   INNER JOIN " + Obj.dBName + "..M_MemberMaster b ON a.IDNo = b.IDNo " +
                    "   WHERE a.IDNo = @UserID " +
                    ") AS M " +
                    "LEFT JOIN " + Obj.dBName + "..M_SolutionMaster S ON M.CID = S.CID" + Obj.IsoEnd;

                Comm = new SqlCommand(strQry, Connselect);
                Comm.Parameters.AddWithValue("@UserID", userid);

                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                Recordcount = Comm.ExecuteScalar().ToString();

                _Output = "{\"complaintdetail\":[";

                // ================= MAIN DATA =================
                strQry =
                    "SELECT M.IDNo,M.CID,CAST(M.CID AS VARCHAR) AS VCId,M.MemName," +
                    "ISNULL(REPLACE(CONVERT(VARCHAR,M.RecTimeStamp,106),' ','-'),'') AS CDate," +
                    "M.CType,M.Complaint," +
                    "ISNULL(S.Solution,'') AS Solution," +
                    "ISNULL(REPLACE(CONVERT(VARCHAR,S.RecTimeStamp,106),' ','-'),'') AS SDate " +
                    "FROM " +
                    "( " +
                    "   SELECT b.MemFirstName + ' ' + b.MemLastName AS MemName, a.* " +
                    "   FROM " + Obj.dBName + "..M_ComplaintMaster a " +
                    "   INNER JOIN " + Obj.dBName + "..M_MemberMaster b ON a.IDNo = b.IDNo " +
                    "   WHERE a.IDNo = @UserID " +
                    ") AS M " +
                    "LEFT JOIN " + Obj.dBName + "..M_SolutionMaster S ON M.CID = S.CID";

                string FinalQry =
                   Obj.IsoStart + "SELECT * FROM (" +
                    "   SELECT *, ROW_NUMBER() OVER (ORDER BY CID DESC) AS SNO " +
                    "   FROM (" + strQry + ") a" +
                    ") b WHERE SNO >= @FromNo AND SNO <= @ToNo AND CID = @CID ORDER BY SNO" + Obj.IsoEnd;

                Adp = new SqlDataAdapter(FinalQry, Connselect);
                Adp.SelectCommand.Parameters.AddWithValue("@UserID", userid);
                Adp.SelectCommand.Parameters.AddWithValue("@FromNo", FromNo);
                Adp.SelectCommand.Parameters.AddWithValue("@ToNo", ToNo);
                Adp.SelectCommand.Parameters.AddWithValue("@CID", complaintid);
                Adp.Fill(DtCountry);

                if (DtCountry.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtCountry.Rows)
                    {
                        _Output += "{"
                            + "\"complaintid\":\"" + Dr["CID"].ToString() + "\","
                            + "\"complaintdate\":\"" + Dr["CDate"].ToString() + "\","
                            + "\"complaint\":\"" + Dr["Complaint"].ToString() + "\","
                            + "\"replydate\":\"" + Dr["SDate"].ToString() + "\","
                            + "\"reply\":\"" + Dr["Solution"].ToString() + "\""
                            + "},";
                    }

                    // remove last comma
                    _Output = _Output.Remove(_Output.Length - 1, 1);
                }

                _Output += "],\"recordcount\":\"" + Recordcount +
                           "\",\"response\":\"OK\",\"msg\":\"Success\"}";

                if (Comm != null)
                    Comm.Cancel();
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }

            if (Comm != null)
                Comm.Cancel();
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    private string ComplaintDetail(string userid, string passwd, int FromNo, int ToNo)
    {
        string _Output = "";

        try
        {
            bool Bool = UserExists(userid, passwd);

            if (Bool == true)
            {
                DataTable DtCountry = new DataTable();
                string Recordcount = "0";

                // ================= RECORD COUNT =================
                strQry =
                    Obj.IsoStart + "SELECT COUNT(*) AS TotalCount FROM " +
                    "( " +
                    "   SELECT b.MemFirstName + ' ' + b.MemLastName AS MemName, a.* " +
                    "   FROM " + Obj.dBName + "..M_ComplaintMaster a " +
                    "   INNER JOIN " + Obj.dBName + "..M_MemberMaster b ON a.IDNo = b.IDNo " +
                    "   WHERE a.IDNo = @UserID " +
                    ") AS M " +
                    "LEFT JOIN " + Obj.dBName + "..M_SolutionMaster S ON M.CID = S.CID" + Obj.IsoEnd;

                Comm = new SqlCommand(strQry, Connselect);
                Comm.Parameters.AddWithValue("@UserID", userid);

                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                Recordcount = Comm.ExecuteScalar().ToString();

                _Output = "{\"complaintdetail\":[";

                // ================= MAIN DATA =================
                strQry =
                    "SELECT M.IDNo,M.CID,CAST(M.CID AS VARCHAR) AS VCId,M.MemName," +
                    "ISNULL(REPLACE(CONVERT(VARCHAR,M.RecTimeStamp,106),' ','-'),'') AS CDate," +
                    "M.CType,M.Complaint," +
                    "ISNULL(S.Solution,'') AS Solution," +
                    "ISNULL(REPLACE(CONVERT(VARCHAR,S.RecTimeStamp,106),' ','-'),'') AS SDate " +
                    "FROM " +
                    "( " +
                    "   SELECT b.MemFirstName + ' ' + b.MemLastName AS MemName, a.* " +
                    "   FROM " + Obj.dBName + "..M_ComplaintMaster a " +
                    "   INNER JOIN " + Obj.dBName + "..M_MemberMaster b ON a.IDNo = b.IDNo " +
                    "   WHERE a.IDNo = @UserID " +
                    ") AS M " +
                    "LEFT JOIN " + Obj.dBName + "..M_SolutionMaster S ON M.CID = S.CID";

                string FinalQry =
                   Obj.IsoStart + "SELECT * FROM (" +
                    "   SELECT *, ROW_NUMBER() OVER (ORDER BY CID DESC) AS SNO " +
                    "   FROM (" + strQry + ") a" +
                    ") b WHERE SNO >= @FromNo AND SNO <= @ToNo ORDER BY SNO" + Obj.IsoEnd;

                Adp = new SqlDataAdapter(FinalQry, Connselect);
                Adp.SelectCommand.Parameters.AddWithValue("@UserID", userid);
                Adp.SelectCommand.Parameters.AddWithValue("@FromNo", FromNo);
                Adp.SelectCommand.Parameters.AddWithValue("@ToNo", ToNo);

                Adp.Fill(DtCountry);

                if (DtCountry.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtCountry.Rows)
                    {
                        _Output += "{"
                            + "\"complaintid\":\"" + Dr["CID"].ToString() + "\","
                            + "\"complaintdate\":\"" + Dr["CDate"].ToString() + "\","
                            + "\"complaint\":\"" + Dr["Complaint"].ToString() + "\","
                            + "\"replydate\":\"" + Dr["SDate"].ToString() + "\","
                            + "\"reply\":\"" + Dr["Solution"].ToString() + "\""
                            + "},";
                    }

                    // remove last comma
                    _Output = _Output.Remove(_Output.Length - 1, 1);
                }

                _Output += "],\"recordcount\":\"" + Recordcount +
                           "\",\"response\":\"OK\",\"msg\":\"Success\"}";

                if (Comm != null)
                    Comm.Cancel();
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }

            if (Comm != null)
                Comm.Cancel();
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    public string Getreferrallink1(string userid, string passwd)
    {
        string _Output = "";

        try
        {
            bool Bool = false;

            if (string.IsNullOrEmpty(userid) && string.IsNullOrEmpty(passwd))
            {
                Bool = true;
            }
            else
            {
                Bool = UserExists(userid, passwd);
            }

            if (Bool == true)
            {
                string WeburlLeft = "";
                string WeburlRight = "";
                DataTable DtCountry = new DataTable();

                int FormNo = GetFormNo(ClearInject(userid));

                string Str = Obj.IsoStart + "EXEC sp_getrerallink @FormNo" + Obj.IsoEnd;

                SqlParameter[] param =
                {
                new SqlParameter("@FormNo", FormNo)
            };

                DtCountry = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Str, param).Tables[0];
                if (DtCountry.Rows.Count > 0)
                {
                    if (Session["compid"].ToString() == "1102")
                    {
                        WeburlLeft = "https://makeandgrowth.com/Account/SignUp?refid=" + Crypto.Encrypt(DtCountry.Rows[0]["idno"].ToString() + "/1");
                        WeburlRight = "https://makeandgrowth.com/Account/SignUp?refid=" + Crypto.Encrypt(DtCountry.Rows[0]["idno"].ToString() + "/2");
                    }
                    else if (Session["CompID"].ToString() == "1108" || Session["compid"].ToString() == "1107")
                    {
                        WeburlLeft = "https://alphaaevplug.in/Account/SignUp?refid=" + Crypto.Encrypt(DtCountry.Rows[0]["idno"].ToString() + "/1");
                        WeburlRight = "https://alphaaevplug.in/Account/SignUp?refid=" + Crypto.Encrypt(DtCountry.Rows[0]["idno"].ToString() + "/2");
                    }
                    else
                    {
                        WeburlLeft = "https://myhemalika.com/Account/SignUp?refid=" + Crypto.Encrypt(DtCountry.Rows[0]["idno"].ToString() + "/1");
                        WeburlRight = "";
                    }

                }

                _Output = "{\"urlLeft\":\"" + WeburlLeft + "\",\"urlright\":\"" + WeburlRight + "\",\"response\":\"OK\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    public string complaintlistwithoutlogin(string userid, string passwd)
    {
        string _Output = "";

        try
        {
            bool Bool = true;

            if (Bool == true)
            {
                DataTable DtState = new DataTable();

                _Output = "{\"complainttype\":[";

                Adp = new SqlDataAdapter(
                   Obj.IsoStart + "SELECT DISTINCT ctypeid AS ctypeid, ctype " +
                    "FROM " + Obj.dBName + "..M_ComplaintTypeMaster " +
                    "WHERE RowStatus='Y' AND ActiveStatus='Y'" + Obj.IsoEnd,
                    Connselect);

                Adp.Fill(DtState);

                if (DtState.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtState.Rows)
                    {
                        _Output += "{"
                                + "\"complaintid\":\"" + Dr["ctypeid"].ToString() + "\","
                                + "\"complaintname\":\"" + Dr["ctype"].ToString() + "\""
                                + "},";
                    }

                    // Remove last comma
                    _Output = _Output.Remove(_Output.Length - 1, 1);
                }

                _Output += "],\"response\":\"OK\",\"msg\":\"Success\"}";

                if (Comm != null)
                    Comm.Cancel();
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }

            if (Comm != null)
                Comm.Cancel();
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    public string complaintlist(string userid, string passwd)
    {
        string _Output = "";

        try
        {
            bool Bool = UserExists(userid, passwd);

            if (Bool == true)
            {
                DataTable DtState = new DataTable();

                _Output = "{\"complainttype\":[";

                Adp = new SqlDataAdapter(
                   Obj.IsoStart + "SELECT DISTINCT ctypeid AS ctypeid, ctype " +
                    "FROM " + Obj.dBName + "..M_ComplaintTypeMaster " +
                    "WHERE RowStatus='Y' AND ActiveStatus='Y'" + Obj.IsoEnd,
                    Connselect);

                Adp.Fill(DtState);

                if (DtState.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtState.Rows)
                    {
                        _Output += "{"
                                + "\"complaintid\":\"" + Dr["ctypeid"].ToString() + "\","
                                + "\"complaintname\":\"" + Dr["ctype"].ToString() + "\""
                                + "},";
                    }

                    // Remove last comma
                    _Output = _Output.Remove(_Output.Length - 1, 1);
                }

                _Output += "],\"response\":\"OK\",\"msg\":\"Success\"}";

                if (Comm != null)
                    Comm.Cancel();
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }

            if (Comm != null)
                Comm.Cancel();
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    public string ChangePassword(string UserID, string OldPasswd, string NewPasswd)
    {
        string _Output = "";

        try
        {
            if (UserExists(UserID, OldPasswd))
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                int IsMemExist = 0;
                string mobileno = "";

                Comm = new SqlCommand(
                   Obj.IsoStart + "SELECT * FROM " + Obj.dBName + "..M_MemberMaster WHERE IDNo = @UserID AND Passw = @OldPasswd" + Obj.IsoEnd,
                    Connselect);

                Comm.Parameters.AddWithValue("@UserID", UserID);
                Comm.Parameters.AddWithValue("@OldPasswd", OldPasswd);

                Dr = Comm.ExecuteReader();

                if (Dr.Read())
                {
                    IsMemExist = 1;
                    mobileno = Dr["Mobl"].ToString().Trim();
                }
                else
                {
                    _Output = "{\"response\":\"Incorrect Password\"}";
                }

                Dr.Close();

                if (IsMemExist == 1)
                {
                    strQry = "INSERT INTO TempMemberMaster " +
                                "SELECT *,'Password updated through App',GETDATE(),'C' " +
                                "FROM M_MemberMaster WHERE IDNo = @UserID; " +

                                "UPDATE M_MemberMaster SET Passw = @NewPasswd " +
                                "WHERE IDNO = @UserID; " +

                                "UPDATE M_AppUser SET OTP = @NewPasswd WHERE UserID = @UserID;";
                    if (Conn.State == ConnectionState.Closed)
                        Conn.Open();

                    SqlCommand Comm_ = new SqlCommand(strQry, Conn);
                    Comm_.Parameters.AddWithValue("@UserID", UserID);
                    Comm_.Parameters.AddWithValue("@NewPasswd", NewPasswd);

                    if (Comm_.ExecuteNonQuery() != 0)
                    {

                        _Output = "{\"response\":\"OK\"}";
                    }
                    else
                    {
                        _Output = "{\"response\":\"FAILED\"}";
                    }
                }

                Comm.Cancel();
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Incorrect Password\"}";
            }
        }
        catch (Exception ex)
        {
            if (Conn != null && Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }

            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    private string Allkyc(
    string UserName, string Password, string Address, string pincode,
    string cityname, string citycode, string StateCode, string District,
    string DistrictCode, string AreaCode, string areaName, string otherareaname,
    string idProofid, string idproofNo, string frontAddressproof,
    string BackAddressproof, string accounttype, string Accountno, string BankCode,
    string BankName, string Otherbankname, string BranchName, string IfscCode,
    string BankImage, string Panno, string Panimage, string formupload)
    {
        string _Output = "";
        string Remark = "";
        string formno = "0";

        try
        {
            Bool = UserExists(UserName, Password);
            formno = GetFormNo(UserName).ToString();

            if (Bool == true)
            {
                DataTable Dt1;
                int j = 0;
                DAL obj;
                DataTable Dt;
                int i;

                obj = new DAL();
                string Condition = "";

                // ================================
                //  BANK NAME == OTHERS LOGIC
                // ================================
                if (BankName == "OTHERS")
                {
                    if (Otherbankname != "")
                    {
                        string Q1 = Obj.IsoStart + "Select * from " + Obj.dBName + "..M_BankMaster where BankName='" + Otherbankname +
                                    "' and Activestatus='Y'and RowStatus='Y' " + Obj.IsoEnd;

                        obj = new DAL();
                        Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Q1).Tables[0];

                        if (Dt.Rows.Count == 0)
                        {
                            Q1 = "insert into M_BankMaster (BankCode,BankName,AcNo,IFSCode,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus) " +
                                 " Select Case When Max(BankCode) Is Null Then '1' Else Max(BankCode)+1 END as BankCode,'" + Otherbankname +
                                 "','0','0',''," +
                                 "'Y','Add by " + UserName + " at " + DateTime.Now.ToString() + "','" + UserName + "','" + Convert.ToInt32(formno) + "','','Y' From M_BankMaster ";

                            i = obj.SaveData(Q1);
                            if (i > 0)
                            {
                                Q1 = Obj.IsoStart + " select Max(BankCode)as BankCode from " + Obj.dBName + "..M_BankMaster where ActiveStatus='Y' and RowStatus='Y'" + Obj.IsoEnd;
                                Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Q1).Tables[0];
                                if (Dt1.Rows.Count > 0)
                                    BankCode = Dt1.Rows[0]["BankCode"].ToString();
                            }
                        }
                        else
                        {
                            BankCode = Dt.Rows[0]["BankCode"].ToString();
                        }
                    }
                }

                // ================================
                // BANK VALIDATION
                // ================================
                if (BankCode != "0" || IfscCode.Trim() != "")
                {
                    if (Convert.ToInt32(BankCode) == 0)
                    {
                        return "{\"response\":\"FAILED\",\"msg\":\"choose bank.\"}";
                    }

                    if (BranchName == "")
                    {
                        return "{\"response\":\"FAILED\",\"msg\":\"enter branch name\"}";
                    }

                    if (IfscCode == "")
                    {
                        return "{\"response\":\"FAILED\",\"msg\":\"enter IFSC Code.\"}";
                    }
                }

                // ================================
                // FILL KYC FOR COMPARISON
                // ================================
                string str = Obj.IsoStart + " Exec sp_FillKyc '" + Convert.ToInt32(formno) + "'" + Obj.IsoEnd;

                obj = new DAL();
                Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];

                if (Dt1.Rows.Count > 0)
                {
                    DataRow R = Dt1.Rows[0];

                    if (ClearInject(R["Address1"].ToString()) != ClearInject(Address)) Remark += "Address ,";
                    if (ClearInject(R["City"].ToString()) != ClearInject(cityname)) Remark += " City ,";
                    if (ClearInject(R["District"].ToString()) != ClearInject(District)) Remark += " District,";
                    if (ClearInject(R["PinCode"].ToString()) != ClearInject(pincode)) Remark += " PinCode,";
                    if (ClearInject(R["AddrProof"].ToString()) != ClearInject(frontAddressproof)) Remark += " AddressProof,";
                    if (ClearInject(R["BackAddressProof"].ToString()) != ClearInject(BackAddressproof)) Remark += " BackAddressProof,";
                    if (ClearInject(R["idtype"].ToString()) != ClearInject(idProofid)) Remark += " AddressProofType,";
                    if (ClearInject(R["IdProofNo"].ToString()) != ClearInject(idproofNo)) Remark += "AddressProofNo,";
                    if (Convert.ToInt32(R["BankId"]) != Convert.ToInt32(BankCode)) Remark += " Bank,";
                    if (ClearInject(R["BranchName"].ToString()) != ClearInject(BranchName)) Remark += " BranchName,";
                    if (ClearInject(R["AcNo"].ToString()) != ClearInject(Accountno)) Remark += " AccountNo,";
                    if (ClearInject(R["IFSCode"].ToString()) != ClearInject(IfscCode)) Remark += " IFSCCode,";
                    if (ClearInject(R["BankProof"].ToString()) != ClearInject(BankImage)) Remark += " BankProof,";
                    if (R["Fax"].ToString() != accounttype) Remark += " Account Type,";
                    if (ClearInject(R["Panno"].ToString()) != ClearInject(Panno)) Remark += " PANNo,";
                    if (ClearInject(R["panimg"].ToString()) != ClearInject(Panimage)) Remark += " PanCardImage,";
                }

                // ================================
                // ID PROOF SELECTION CHECK
                // ================================
                if (idProofid == "0")
                    return "{\"response\":\"FAILED\",\"msg\":\"choose Id Proof.\"}";

                // ================================
                // AREA NAME = OTHERS LOGIC
                // ================================
                if (areaName == "OTHERS")
                {
                    if (otherareaname != "")
                    {
                        string q = Obj.IsoStart + "Select * from " + Obj.dBName + "..M_VillageMaster where VillageName='" + otherareaname +
                                   "' and Activestatus='Y' and Pincode='" + Convert.ToInt32(pincode) + "' " + Obj.IsoEnd;

                        obj = new DAL();
                        Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, q).Tables[0];

                        if (Dt.Rows.Count == 0)
                        {
                            q = "insert into M_VillageMaster (VillageName,CityCode,PinCode) " +
                                " Values('" + otherareaname + "','" + citycode + "','" + pincode + "')";

                            i = obj.SaveData(q);
                            if (i > 0)
                            {
                                q = Obj.IsoStart + " select Max(VillageCode)as VillageCode from " + Obj.dBName + "..M_VillageMaster where ActiveStatus='Y'" + Obj.IsoEnd;
                                Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, q).Tables[0];
                                if (Dt.Rows.Count > 0)
                                    AreaCode = Dt.Rows[0]["VillageCode"].ToString();
                            }
                        }
                        else
                        {
                            AreaCode = Dt.Rows[0]["VillageCode"].ToString();
                        }
                    }
                }

                // ================================
                // BUILD MAIN SQL UPDATE BLOCK
                // ================================
                string Qry =
                    "Insert Into TempMemberMaster Select *,'Update KYC - " + Context.Request.UserHostAddress.ToString() +
                    "',GetDate(),'U' From M_MemberMaster Where FormNo='" + Convert.ToInt32(formno) + "'";

                //Qry += ";Insert Into TempKycVerify Select *,GetDate(),'" + formno +
                //       "' From KycVerify Where FormNo='" + Convert.ToInt32(formno) + "'";

                Qry += ";insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" +
                       "(0,'" + Dt1.Rows[0]["MemName"] + "','KYC Detail','KYC Detail Update','" + Remark +
                       "',Getdate(),'" + Convert.ToInt32(formno) + "')";

                string sql = Qry +
                    ";Update m_MemberMaster set Address1='" + Address.ToUpper() +
                    "', Tehsil='" + cityname + "',City='" + cityname.ToUpper() +
                    "',District='" + District.ToUpper() + "',StateCode='" + StateCode +
                    "', Pincode='" + pincode + "' ,AreaCode='" + AreaCode +
                    "', CityCode='" + citycode + "',DistrictCode='" + DistrictCode +
                    "',Panno='" + Panno.ToUpper() + "',Acno='" + Accountno +
                    "',Bankid='" + BankCode + "',IFscode='" + IfscCode.ToUpper() +
                    "', Branchname='" + BranchName.ToUpper() + "',Fax='" + accounttype +
                    "'" + Condition + " where Formno= '" + Convert.ToInt32(formno) + "'";
                if (Session["compid"].ToString() == "1102")
                {
                    sql += ";Update KycVerify Set Idtype='" + idProofid +
                       "',IdProofNo='" + idproofNo +
                       "', AddrProof='" + frontAddressproof +
                       "',BackAddressProof='" + BackAddressproof +
                       "',BackAddressDate=Getdate(),IsaddrssVerified='N',IsAddress = 'N'," +
                       " PanImg='" + Panimage +
                       "',PANImgDate=Getdate(),IsPanVerified='N',IsPan = 'N'," +
                       "BankProof='" + BankImage + "',BankProofDate=Getdate(),IsBankVerified='N',IsBank = 'N' where Formno= '" + Convert.ToInt32(formno) + "';";
                    if (CheckFormno(formno.ToString()) == 0)
                    {
                        sql += "INSERT INTO M_FormUpload(Formno, FrontSideForm, ActiveStatus, Remark, AdminRemark,IsForm)VALUES ('" + formno + "','" + formupload + "','N','','','N')";
                    }
                    if (CheckFormno(formno.ToString()) == 1)
                    {
                        sql += "UPDATE M_FormUpload SET FrontSideForm = '" + formupload + "',ActiveStatus = 'N',IsForm = 'N' WHERE Formno = '" + formno + "'";
                    }
                }
                else if (Session["compid"].ToString() == "1106")
                {
                    sql += ";Update KycVerify Set Idtype='" + idProofid +
                       "',IdProofNo='" + idproofNo +
                       "', AddrProof='" + frontAddressproof +
                       "',BackAddressProof='" + BackAddressproof +
                       "',BackAddressDate=Getdate(),IsaddrssVerified='N',IsAddress = 'N'," +
                       " PanImg='" + Panimage +
                       "',PANImgDate=Getdate(),IsPanVerified='N',IsPan = 'N'," +
                       "BankProof='" + BankImage + "',BankProofDate=Getdate(),IsBankVerified='N',IsBank = 'N' where Formno= '" + Convert.ToInt32(formno) + "';";
                    if (CheckFormno(formno.ToString()) == 0)
                    {
                        sql += "INSERT INTO M_FormUpload(Formno, FrontSideForm, ActiveStatus, Remark, AdminRemark,IsForm)VALUES ('" + formno + "','" + formupload + "','N','','','N')";
                    }
                    if (CheckFormno(formno.ToString()) == 1)
                    {
                        sql += "UPDATE M_FormUpload SET FrontSideForm = '" + formupload + "',ActiveStatus = 'N',IsForm = 'N' WHERE Formno = '" + formno + "'";
                    }
                }
                else
                {
                    sql += ";Update KycVerify Set Idtype='" + idProofid +
                       "',IdProofNo='" + idproofNo +
                       "', AddrProof='" + frontAddressproof +
                       "',BackAddressProof='" + BackAddressproof +
                       "',BackAddressDate=Getdate(),IsaddrssVerified='N'," +
                       " PanImg='" + Panimage +
                       "',PANImgDate=Getdate(),IsPanVerified='N'," +
                       "BankProof='" + BankImage +
                       "',BankProofDate=Getdate(),IsBankVerified='N' where Formno= '" + Convert.ToInt32(formno) + "';";
                    if (CheckFormno(formno.ToString()) == 0)
                    {
                        sql += "INSERT INTO M_FormUpload(Formno, FrontSideForm, ActiveStatus, Remark, AdminRemark)VALUES ('" + formno + "','" + formupload + "','N','','')";
                    }
                    if (CheckFormno(formno.ToString()) == 1)
                    {
                        sql += "UPDATE M_FormUpload SET FrontSideForm = '" + formupload + "',ActiveStatus = 'N' WHERE Formno = '" + formno + "'";
                    }
                }
                obj = new DAL();
                j = obj.SaveData(sql);

                if (j != 0)
                {
                    _Output = "{\"response\":\"OK\",\"msg\":\"save successfully\"}";
                }
                else
                {
                    _Output = "{\"response\":\"FAILED\"}";
                }
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    private string BANKDETAILSAVE(string UserName, string Password, string accounttype, string Accountno, string BankCode, string BankName, string Otherbankname, string BranchName, string IfscCode, string BankImage)
    {
        string _Output = "";
        string Remark = "";
        string formno = "0";

        try
        {
            Bool = UserExists(UserName, Password);
            formno = GetFormNo(UserName).ToString();
            if (Bool == true)
            {
                DataTable Dt1;
                int j = 0;
                DAL obj;
                string FlNm = "";
                string scrname = "";
                int i;

                obj = new DAL();
                string s1 = "";
                string Condition = "";

                DataTable Dt;

                if (BankName == "OTHERS")
                {
                    if (Otherbankname != "")
                    {
                        string Q1 =
                            Obj.IsoStart + "Select * from " + Obj.dBName + "..M_BankMaster where BankName='" + Otherbankname +
                            "' and Activestatus='Y'and RowStatus='Y' " + Obj.IsoEnd;

                        obj = new DAL();
                        Dt = new DataTable();
                        Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Q1).Tables[0];

                        if (Dt.Rows.Count == 0)
                        {
                            Q1 =
                                "insert into M_BankMaster (BankCode,BankName,AcNo,IFSCode,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus) " +
                                " Select Case When Max(BankCode) Is Null Then '1' Else Max(BankCode)+1 END as BankCode,'" +
                                Otherbankname + "','0','0','','Y','Add by " + UserName + " at " +
                                DateTime.Now + "','" + UserName + "','" + Convert.ToInt32(formno) +
                                "','','Y' From M_BankMaster";

                            i = obj.SaveData(Q1);

                            if (i > 0)
                            {
                                Q1 = Obj.IsoStart + " select Max(BankCode)as BankCode from " + Obj.dBName + "..M_BankMaster where ActiveStatus='Y' and RowStatus='Y'" + Obj.IsoEnd;
                                Dt1 = new DataTable();
                                Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Q1).Tables[0];

                                if (Dt1.Rows.Count > 0)
                                    BankCode = Dt1.Rows[0]["BankCode"].ToString();
                            }
                        }
                        else
                        {
                            BankCode = Dt.Rows[0]["BankCode"].ToString();
                        }
                    }
                }

                if (BankCode != "0" || IfscCode.Trim() != "")
                {
                    if (Convert.ToInt32(BankCode) == 0)
                        return "{\"response\":\"FAILED\",\"msg\":\"choose bank.\"}";

                    if (BranchName == "")
                        return "{\"response\":\"FAILED\",\"msg\":\"enter branch name\"}";

                    if (IfscCode == "")
                        return "{\"response\":\"FAILED\",\"msg\":\"enter IFSC Code.\"}";
                }

                string str =
                   Obj.IsoStart + "Select a.IDNo,a.MemFirstName As MemName,a.Panno,a.Acno,a.BAnkid,a.IFscode,a.Fax,a.Branchname,b.BankProof," +
                    " Case when b.ISbankverified<>'N' then Replace(CONVERT(varchar,b.BankVerifyDate,106),' ','-')" +
                    " Else '' End as BankProofDate,b.isBankverified,CASE WHEN b.IsBankVerified='Y' THEN 'Verified' " +
                    " when b.IsBankVerified='R' then 'Rejected' Else 'Verification Due' END AS BankVerf," +
                    " Case when b.IsBankVerified='R' then b.BankProofRemark else '' end as RejectRemark,Isnull(f.Reason,' ')As RejectReason" +
                    " From " + Obj.dBName + "..M_MemberMaster as a inner join " + Obj.dBName + "..KycVerify as b On a.Formno=b.Formno " +
                    " Left Join " + Obj.dBName + "..M_KycReject as f On b.BankRejectId=f.Kid " +
                    " where a.Formno='" + Convert.ToInt32(formno) + "'" + Obj.IsoEnd;

                obj = new DAL();
                Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];

                if (Dt1.Rows.Count > 0)
                {
                    if (Convert.ToInt32(Dt1.Rows[0]["BankId"]) != Convert.ToInt32(BankCode))
                        Remark += " Bank,";

                    if (ClearInject(Dt1.Rows[0]["BranchName"].ToString()) != ClearInject(BranchName))
                        Remark += " BranchName,";

                    if (ClearInject(Dt1.Rows[0]["AcNo"].ToString()) != ClearInject(Accountno))
                        Remark += " AccountNo,";

                    if (ClearInject(Dt1.Rows[0]["IFSCode"].ToString()) != ClearInject(IfscCode))
                        Remark += " IFSCCode,";

                    if (ClearInject(Dt1.Rows[0]["BankProof"].ToString()) != ClearInject(BankImage))
                        Remark += " BankProof,";

                    if (Dt1.Rows[0]["Fax"].ToString() != accounttype)
                        Remark += " Account Type,";
                }

                string Qry =
                    "Insert Into TempMemberMaster Select *,'Update KYC - " +
                    Context.Request.UserHostAddress +
                    "',GetDate(),'U' From M_MemberMaster Where FormNo='" + Convert.ToInt32(formno) + "'";

                Qry += ";Insert Into TempKycVerify Select *,GetDate(),'" + formno +
                       "' From KycVerify Where FormNo='" + Convert.ToInt32(formno) + "'";

                Qry += ";insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" +
                       "(0,'" + Dt1.Rows[0]["MemName"] +
                       "','KYC Detail','KYC Detail Update','" + Remark +
                       "',Getdate(),'" + Convert.ToInt32(formno) + "')";

                string sql = Qry +
                    ";Update m_MemberMaster set Acno='" + Accountno +
                    "',Bankid='" + BankCode +
                    "',IFscode='" + IfscCode.ToUpper() +
                    "', Branchname='" + BranchName.ToUpper() +
                    "',Fax='" + accounttype + "'" + Condition +
                    " where Formno= '" + Convert.ToInt32(formno) + "'";

                sql += ";Update KycVerify Set BankProof='" + BankImage + "',BankProofDate=Getdate(),IsBankVerified='Y',BankverifyDate = getdate(),IsBank = 'Y' where Formno= '" + Convert.ToInt32(formno) + "'";

                obj = new DAL();
                j = obj.SaveData(sql);

                if (j != 0)
                    _Output = "{\"response\":\"OK\",\"msg\":\"save successfully\"}";
                else
                    _Output = "{\"response\":\"FAILED\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }
        return _Output;
    }
    private string KYCBankDetail(string userid, string passwd)
    {
        string _Output = "";

        try
        {
            bool Bool = UserExists(userid, passwd);

            if (Bool == true)
            {
                DataTable DtProfile = new DataTable();

                string Str = Obj.IsoStart + "Select a.IDNo,a.MemFirstName As MemName,a.Panno,a.Acno,a.BAnkid,a.IFscode,a.Fax,a.Branchname,b.BankProof," +
                             " Case when b.ISbankverified<>'N' then Replace(CONVERT(varchar,b.BankVerifyDate,106),' ','-')" +
                             " Else '' End as BankProofDate,IsBank as isBankverified," +
                             " CASE WHEN b.IsBankVerified='Y' THEN 'Verified' " +
                             " when b.IsBankVerified='R' then 'Rejected' Else 'Verification Due' END AS BankVerf," +
                             " Case when b.IsBankVerified='R' then b.BankProofRemark else '' end as RejectRemark," +
                             " Isnull(f.Reason,' ') As RejectReason" +
                             " From " + Obj.dBName + "..M_MemberMaster as a inner join " + Obj.dBName + "..KycVerify as b On a.Formno=b.Formno " +
                             " Left Join " + Obj.dBName + "..M_KycReject as f On b.BankRejectId=f.Kid " +
                             " where a.idno='" + userid + "'" + Obj.IsoEnd;

                Adp = new SqlDataAdapter(Str, Connselect);
                Adp.Fill(DtProfile);

                if (DtProfile.Rows.Count > 0)
                {
                    _Output = "{"
                        + "\"idno\":\"" + DtProfile.Rows[0]["IdNo"] + "\","
                        + "\"bankid\":\"" + DtProfile.Rows[0]["Bankid"] + "\","
                        + "\"acno\":\"" + DtProfile.Rows[0]["acno"] + "\","
                        + "\"ifscode\":\"" + DtProfile.Rows[0]["IFscode"] + "\","
                        + "\"accounttype\":\"" + DtProfile.Rows[0]["Fax"] + "\","
                        + "\"branchname\":\"" + DtProfile.Rows[0]["Branchname"] + "\","
                        + "\"bankproof\":\"" + DtProfile.Rows[0]["BankProof"] + "\","
                        + "\"bankproofdate\":\"" + DtProfile.Rows[0]["BankProofDate"] + "\","
                        + "\"bankverf\":\"" + DtProfile.Rows[0]["BankVerf"] + "\","
                        + "\"isbankverified\":\"" + DtProfile.Rows[0]["isBankverified"] + "\","
                        + "\"rejectremark\":\"" + DtProfile.Rows[0]["RejectRemark"] + "\","
                        + "\"rejectreason\":\"" + DtProfile.Rows[0]["RejectReason"] + "\","
                        + "\"response\":\"OK\""
                        + "}";
                }
                else
                {
                    _Output = "{\"response\":\"FAILED\"}";
                }
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    private string KYCPancardDetail(string userid, string passwd)
    {
        string _Output = "";

        try
        {
            bool Bool = UserExists(userid, passwd);

            if (Bool == true)
            {
                DataTable DtProfile = new DataTable();

                string Str = Obj.IsoStart + "Select a.IDNo,a.MemFirstName As MemName,a.Panno, b.PanImg, " +
                             "Replace(Convert(Varchar,b.PANImgDate,106),' ','-') as PanProofdate, " +
                             " IsPan as IsPanVerified,Case when b.IsPanVerified<>'N' then " +
                             "Replace(CONVERT(varchar,b.PanVerifyDate ,106),' ','-') " +
                             "Else '' End as PanVerifyDate, " +
                             "CASE WHEN b.IsPanVerified='Y' THEN 'Verified' " +
                             "when b.IsPanVerified='R' then 'Rejected' Else 'Verification Due' END AS PanVerf, " +
                             "case when b.IsPanVerified='R' then b.PanRemarks else '' end as RejectRemark, " +
                             "Isnull(f.Reason,'') As RejectReason " +
                             "From " + Obj.dBName + "..M_MemberMaster as a Inner Join " + Obj.dBName + "..KycVerify as b On a.Formno=b.Formno " +
                             "Left Join " + Obj.dBName + "..M_KycReject as f On b.PanRejectId=f.Kid " +
                             "where a.idno='" + userid + "'" + Obj.IsoEnd;

                Adp = new SqlDataAdapter(Str, Connselect);
                Adp.Fill(DtProfile);

                if (DtProfile.Rows.Count > 0)
                {
                    _Output = "{"
                        + "\"idno\":\"" + DtProfile.Rows[0]["IdNo"] + "\","
                        + "\"panno\":\"" + DtProfile.Rows[0]["Panno"] + "\","
                        + "\"panimage\":\"" + DtProfile.Rows[0]["Panimg"] + "\","
                        + "\"panproofdate\":\"" + DtProfile.Rows[0]["PanProofdate"] + "\","
                        + "\"ispanverified\":\"" + DtProfile.Rows[0]["IsPanVerified"] + "\","
                        + "\"panverifydate\":\"" + DtProfile.Rows[0]["PanVerifyDate"] + "\","
                        + "\"panverf\":\"" + DtProfile.Rows[0]["PanVerf"] + "\","
                        + "\"rejectremark\":\"" + DtProfile.Rows[0]["RejectRemark"] + "\","
                        + "\"rejectreason\":\"" + DtProfile.Rows[0]["RejectReason"] + "\","
                        + "\"response\":\"OK\""
                        + "}";
                }
                else
                {
                    _Output = "{\"response\":\"FAILED\"}";
                }
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    private string GetKyc(string userid, string passwd)
    {
        string _Output = "";

        try
        {
            bool Bool = UserExists(userid, passwd);

            if (Bool == true)
            {
                int FormNo = GetFormNo(ClearInject(userid));

                DataTable DtProfile = new DataTable();
                string Str = Obj.IsoStart + "Exec sp_FillKyc '" + Convert.ToInt32(FormNo) + "'" + Obj.IsoEnd;

                SqlDataAdapter Adp = new SqlDataAdapter(Str, Connselect);
                Adp.Fill(DtProfile);

                if (DtProfile.Rows.Count > 0)
                {
                    DataRow row = DtProfile.Rows[0];

                    _Output =
                        "{"
                        + "\"idno\":\"" + row["IdNo"] + "\","
                        + "\"addrsverf\":\"" + row["IsAddrssverified"] + "\","
                        + "\"idverf\":\"" + row["idVerf"] + "\","
                        + "\"rejectreason\":\"" + row["AddressRejectReason"] + "\","
                        + "\"rejectremark\":\"" + row["AdressRejectRemark"] + "\","
                        + "\"vaerifydate\":\"" + row["AddrProofDate"] + "\","
                        + "\"isBankverified\":\"" + row["isBankverified"] + "\","
                        + "\"BankVerf\":\"" + row["BankVerf"] + "\","
                        + "\"BankRejectReason\":\"" + row["BankRejectReason"] + "\","
                        + "\"BankRejectRemark\":\"" + row["BankRejectRemark"] + "\","
                        + "\"BankProofDate\":\"" + row["BankProofDate"] + "\","
                        + "\"IsPanVerified\":\"" + row["IsPanVerified"] + "\","
                        + "\"PanVerf\":\"" + row["PanVerf"] + "\","
                        + "\"PanRejectReason\":\"" + row["PanRejectReason"] + "\","
                        + "\"PanRejectRemark\":\"" + row["PanRejectRemark"] + "\","
                        + "\"PanVerifyDate\":\"" + row["PanVerifyDate"] + "\","
                        + "\"Isformupload\":\"" + row["Isformupload"] + "\","
                        + "\"formuploadVerf\":\"" + row["formuploadVerf"] + "\","
                        + "\"formuploadVerifyDate\":\"" + row["formuploadVerifyDate"] + "\","

                        + "\"addressdetail\":{"
                            + "\"idproof\":\"" + row["IdProofNo"] + "\","
                            + "\"address\":\"" + row["Address1"] + "\","
                            + "\"pincode\":\"" + row["Pincode"] + "\","
                            + "\"city\":\"" + row["City"] + "\","
                            + "\"district\":\"" + row["District"] + "\","
                            + "\"statecode\":\"" + row["Statecode"] + "\","
                            + "\"statename\":\"" + row["StateName"] + "\","
                            + "\"addrproof\":\"" + row["AddrProof"] + "\","
                            + "\"IdproofNo\":\"" + row["IdproofNo"] + "\","
                            + "\"backaddressproof\":\"" + row["BackAddressProof"] + "\","
                            + "\"backaddressdate\":\"" + row["BackAddressDate"] + "\","
                            + "\"idtype\":\"" + row["IdType"] + "\","
                            + "\"areacode\":\"" + row["areacode"] + "\""
                        + "},"

                        + "\"bankdetail\":{"
                            + "\"bankid\":\"" + row["Bankid"] + "\","
                            + "\"acno\":\"" + row["acno"] + "\","
                            + "\"ifscode\":\"" + row["IFscode"] + "\","
                            + "\"accounttype\":\"" + row["Fax"] + "\","
                            + "\"branchname\":\"" + row["Branchname"] + "\","
                            + "\"bankproof\":\"" + row["BankProof"] + "\""
                        + "},"

                        + "\"pandetail\":{"
                            + "\"panno\":\"" + row["Panno"] + "\","
                            + "\"panimage\":\"" + row["Panimg"] + "\""
                        + "},"
                         + "\"formuploaddetail\":{"
                            + "\"formupload\":\"" + row["FrontSideForm"] + "\""
                        + "},"

                        + "\"response\":\"OK\","
                        + "\"msg\":\"Success\""
                    + "}";
                }
                else
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"Record Not Found.\"}";
                }

                if (Comm != null)
                    Comm.Cancel();
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }

            if (Comm != null)
                Comm.Cancel();
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    private string CheckPanno(string userid, string passwd, string Panno)
    {
        bool isValid = false;

        if (userid == "" && passwd == "")
        {
            isValid = true;
        }
        else
        {
            isValid = UserExists(userid, passwd);
        }

        string _output = "";

        if (isValid == true)
        {
            string Sqlstr = Obj.IsoStart + "Select panno from " + Obj.dBName + "..M_MemberMaster where panno = '" + Panno.Trim() + "' " + Obj.IsoEnd;
            DataTable Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Sqlstr).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                _output = "{\"response\":\"FAILED\",\"panno\":\"" + Dt.Rows[0]["panno"].ToString() + "\",\"msg\":\"already exist\"}";
            }
            else
            {
                _output = "{\"response\":\"OK\",\"panno\":\"" + Panno.ToString() + "\",\"msg\":\"success\"}";
            }
        }
        else
        {
            _output = "{\"response\":\"FAILED\",\"panno\":\"\",\"msg\":\"Invalid Login Details.\"}";
        }

        return _output;
    }
    private string CheckSponsor(string userid, string passwd, string sponsorid)
    {
        bool isValid = false;

        if (userid == "" && passwd == "")
        {
            isValid = true;
        }
        else
        {
            isValid = UserExists(userid, passwd);
        }

        string _output = "";

        if (isValid == true)
        {
            string sponsorname = "";
            string formno = GetFormNo(sponsorid).ToString();
            string UplnFormno = "";
            string Sqlstr = "";
            if (Session["compid"].ToString() == "1102")
            {
                Sqlstr = Obj.IsoStart + "Select idno,(MemfirstName+' '+memlastName) as MemberName,formno from " + Obj.dBName + "..M_MemberMaster where Idno = '" + sponsorid.Trim() + "' AND fld5 <> 'P' " + Obj.IsoEnd;
            }
            else
            {
                Sqlstr = Obj.IsoStart + "Select idno,(MemfirstName+' '+memlastName) as MemberName,formno from " + Obj.dBName + "..M_MemberMaster where Idno = '" + sponsorid.Trim() + "'" + Obj.IsoEnd;
            }

            DataTable Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Sqlstr).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                sponsorname = Dt.Rows[0]["MemberName"].ToString();
                UplnFormno = Dt.Rows[0]["formno"].ToString();
                if (ShowUpline(UplnFormno.ToString()) == "True")
                {
                    _output = "{\"response\":\"OK\",\"sponsorname\":\"" + sponsorname + "\",\"showupline\":\"true\",\"msg\":\"success\"}";
                }
                else
                {
                    _output = "{\"response\":\"OK\",\"sponsorname\":\"" + sponsorname + "\",\"showupline\":\"false\",\"msg\":\"success\"}";
                }
            }
            else
            {
                _output = "{\"response\":\"FAILED\",\"sponsorname\":\"\",\"showupline\":\"false\",\"msg\":\"This ID Not Valid For Sponsor.!\"}";
            }
        }
        else
        {
            _output = "{\"response\":\"FAILED\",\"sponsorname\":\"\",\"showupline\":\"false\",\"msg\":\"Invalid Login Details.\"}";
        }

        return _output;
    }
    private string ShowUpline(string RefFormno)
    {
        string RtrVal = "False";
        DataTable tmpTable = new DataTable();
        string str = Obj.IsoStart + "select Count(1) As Cnt From " + Obj.dBName + "..M_MemberMaster Where RefFormno = '" + RefFormno + "'" + Obj.IsoEnd;
        DataSet dsCh = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str);
        tmpTable = dsCh.Tables[0];
        if (Convert.ToInt32(tmpTable.Rows[0]["Cnt"]) >= 2)
        {
            RtrVal = "True";
        }
        else
        {
            RtrVal = "False";
        }

        return RtrVal;
    }
    private int GetFormNo(string IDNO, string AStatus = "", string Mobl = "", string Address = "")
    {
        int FrmNo = 0;

        string Str = Obj.IsoStart + "Select FormNo, ActiveStatus, Mobl, Address1 " +
                     "From " + Obj.dBName + "..M_MemberMaster WHERE IDNO='" + IDNO + "'" + Obj.IsoEnd;

        if (Conn.State == ConnectionState.Closed)
            Conn.Open();

        SqlCommand Comd = new SqlCommand(Str, Connselect);
        SqlDataReader Dr = Comd.ExecuteReader();

        if (Dr.Read())
        {
            FrmNo = Convert.ToInt32(Dr["FormNo"]);
            AStatus = Dr["ActiveStatus"].ToString();
            Mobl = Dr["Mobl"].ToString();
            Address = Dr["Address1"].ToString();
        }

        Dr.Close();
        return FrmNo;
    }
    public bool UserExists(string userid, string passwd)
    {
        try
        {
            string compId = Session["CompId"]?.ToString();
            string Sql = "";
            DataTable Dt = new DataTable();

            Sql = Obj.IsoStart + "Select * from " + Obj.dBName + "..M_membermaster where Idno = '" + userid + "' And Passw = '" + passwd + "'" + Obj.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(
                     HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                     CommandType.Text,
                     Sql
                 ).Tables[0];

            if (Dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public string IdTypelist()
    {
        string _Output = "";

        try
        {
            DataTable DtState = new DataTable();
            _Output = "{\"idtype\":[";

            string str = Obj.IsoStart + "SELECT Id, IdType FROM " + Obj.dBName + "..M_IdTypeMaster WHERE ACTIVESTATUS='Y' Order by id" + Obj.IsoEnd;

            DtState = SqlHelper.ExecuteDataset(
                            HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                            CommandType.Text,
                            str
                      ).Tables[0];

            if (DtState.Rows.Count > 0)
            {
                foreach (DataRow Dr in DtState.Rows)
                {
                    _Output += "{\"id\":\"" + Dr["Id"] +
                               "\",\"Idtype\":\"" + Dr["IdType"] + "\"},";
                }

                // remove last comma
                _Output = _Output.Substring(0, _Output.Length - 1);
            }

            _Output += "],\"response\":\"OK\",\"msg\":\"Success\"}";
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    public string AccountType(string userid, string passwd)
    {
        string _Output = "";

        try
        {
            bool Bool = false;


            Bool = true;


            if (Bool == true)
            {
                DataTable DtState = new DataTable();
                _Output = "{\"accounttype\":[";

                string str = Obj.IsoStart + "Select * From " + Obj.dBName + "..AccountType Where ActiveStatus='Y' Order by accid" + Obj.IsoEnd;

                DtState = SqlHelper.ExecuteDataset(
                            HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                            CommandType.Text,
                            str
                         ).Tables[0];

                if (DtState.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtState.Rows)
                    {
                        _Output += "{\"accid\":\"" + Dr["accid"] +
                                   "\",\"accountype\":\"" + Dr["AccounType"] + "\"},";
                    }

                    // remove trailing comma
                    _Output = _Output.Substring(0, _Output.Length - 1);
                }

                _Output += "],\"response\":\"OK\",\"msg\":\"Success\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    public string statelist(string userid, string passwd, string CountryCode)
    {
        string _Output = "";

        try
        {
            bool Bool = false;

            Bool = true;
            if (Bool == true)
            {
                DataTable DtState = new DataTable();
                _Output = "{\"states\": [";

                SqlDataAdapter Adp = new SqlDataAdapter(
                    Obj.IsoStart + "Select * From " + Obj.dBName + "..M_StateDivMaster Where ActiveStatus='Y' And CountryCode=" +
                    CountryCode + " And RowStatus='Y' Order by StateName" + Obj.IsoEnd, Connselect);

                Adp.Fill(DtState);

                if (DtState.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtState.Rows)
                    {
                        _Output += "{\"statecode\":\"" + Dr["StateCode"] +
                                   "\",\"statename\":\"" + Dr["StateName"] + "\"},";
                    }

                    // Remove last comma
                    _Output = _Output.Substring(0, _Output.Length - 1);
                }

                _Output += "],\"response\":\"OK\",\"msg\":\"Success\"}";

                if (Comm != null)
                    Comm.Cancel();
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }

            if (Comm != null)
                Comm.Cancel();
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    public string BankList(string userid, string passwd)
    {
        string _Output = "";

        try
        {
            bool Bool = false;

            Bool = true;
            if (Bool == true)
            {
                DataTable DtCountry = new DataTable();
                _Output = "{\"bankers\": [";

                string Str = Obj.IsoStart + "Select * From " + Obj.dBName + "..M_BankMaster Where ActiveStatus='Y' And RowStatus='Y' Order by BankName" + Obj.IsoEnd;

                DtCountry = SqlHelper.ExecuteDataset(
                               HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                               CommandType.Text,
                               Str
                            ).Tables[0];

                if (DtCountry.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtCountry.Rows)
                    {
                        _Output += "{\"bankcode\":\"" + Dr["BankCode"] +
                                   "\",\"bankname\":\"" + Dr["BankName"] + "\"},";
                    }

                    // Remove last comma
                    _Output = _Output.Substring(0, _Output.Length - 1);
                }

                _Output += "],\"response\":\"OK\",\"msg\":\"Success\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"Invalid Login Details.\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        return _Output;
    }
    public string Validate_(string Referral, string Sponsor, string Side, string Type)
    {
        SqlDataReader Dread;

        // Checking Referral ID
        if (Session["CompId"].ToString() == "1088")
        {
            Comm = new SqlCommand(
               Obj.IsoStart + "Select FormNo, MemFirstName + ' ' + MemLastName as MemName from " + Obj.dBName + "..M_MemberMaster " +
                "where Idno='" + Referral + "'" + Obj.IsoEnd,
                Connselect);
        }
        else if (Session["CompId"].ToString() == "1096")
        {
            Comm = new SqlCommand(
               Obj.IsoStart + "Select FormNo, MemFirstName + ' ' + MemLastName as MemName from " + Obj.dBName + "..M_MemberMaster " +
                "where Idno='" + Referral + "'" + Obj.IsoEnd,
                Connselect);
        }
        else
        {
            Comm = new SqlCommand(
               Obj.IsoStart + "Select FormNo, MemFirstName + ' ' + MemLastName as MemName from " + Obj.dBName + "..M_MemberMaster " +
                "where Idno='" + Referral + "' AND( Formno in (Select distinct FormnoDwn FROM " + Obj.dBName + "..M_MemTreeRelation) " +
                "Or Formno in(select distinct formno from " + Obj.dBName + "..M_memtreeRelation))" + Obj.IsoEnd,
                Connselect);
        }

        Dread = Comm.ExecuteReader();

        if (!Dread.Read())
        {
            Dread.Close();
            return "Invalid Referral ID.";
        }

        _RefFormNo = Dread["FormNo"].ToString();
        Dread.Close();
        Comm.Cancel();

        // If TYPE is empty, do full validation
        if (Type == "")
        {
            string _IsGetExtreme = "N";

            // Fetch config
            Comm = new SqlCommand(Obj.IsoStart + "select * from " + Obj.dBName + "..M_ConfigMaster" + Obj.IsoEnd, Connselect);
            Dread = Comm.ExecuteReader();

            if (Dread.Read())
            {
                _IsGetExtreme = Dread["IsGetExtreme"].ToString();
            }

            Dread.Close();
            Comm.Cancel();

            if (_IsGetExtreme == "N")
            {
                // Validate Sponsor ID
                Comm = new SqlCommand(
                    Obj.IsoStart + "Select FormNo, MemFirstName + ' ' + MemLastName as MemName from " + Obj.dBName + "..M_MemberMaster where Idno='" + Sponsor + "'" + Obj.IsoEnd,
                    Connselect);

                Dread = Comm.ExecuteReader();

                if (!Dread.Read())
                {
                    Dread.Close();
                    return "Invalid Sponsor ID.";
                }

                _UpLnFormNo = Dread["FormNo"].ToString();
                Dread.Close();
                Comm.Cancel();

                // Validate Side availability
                Comm = new SqlCommand(
                    Obj.IsoStart + "SELECT COUNT(*) AS CNT From " + Obj.dBName + "..M_MemberMaster WHERE UpLnFormNo in " +
                    "(Select FormNo From " + Obj.dBName + "..M_MemberMaster Where IDNo='" + Sponsor + "') And Legno = " + Side + Obj.IsoEnd,
                    Connselect);

                Dread = Comm.ExecuteReader();

                if (!Dread.Read())
                {
                    Dread.Close();
                    return "Selected Side Not Available.";
                }
                else
                {
                    if (Convert.ToInt32(Dread["CNT"]) >= 1)
                    {
                        Dread.Close();
                        return "Selected Side Not Available.";
                    }
                }

                Dread.Close();
                Comm.Cancel();

                // Check if Sponsor is in Referral Downline
                if (_RefFormNo != _UpLnFormNo)
                {
                    Comm = new SqlCommand(
                        Obj.IsoStart + "Select * from " + Obj.dBName + "..M_MemTreeRelation where FormNo=" + _RefFormNo +
                        " And FormNoDwn=" + _UpLnFormNo + Obj.IsoEnd,
                        Connselect);

                    Dread = Comm.ExecuteReader();

                    if (!Dread.Read())
                    {
                        Dread.Close();
                        return "Sponsor does not exist in referral downline.";
                    }

                    Dread.Close();
                    Comm.Cancel();
                }
            }
        }

        return "OK";
    }
    private string checkaadhar(string Panno)
    {
        string sql = "";
        string _Output = "";
        string errType = "";

        try
        {
            if (!string.IsNullOrWhiteSpace(Panno))
            {
                sql = Obj.IsoStart + "Select Count(*) Cnt FROM " + Obj.dBName + "..M_Membermaster WHERE AadharNo = '" + Panno + "'" + Obj.IsoEnd;
                Comm = new SqlCommand(sql, Connselect);
                Dr = Comm.ExecuteReader();

                if (Dr.Read())
                {
                    if (Convert.ToInt32(Dr["Cnt"]) > 1)
                    {
                        errType = "Aadhar No"; // already registered on another Ids.
                        _Output = "Faild";
                    }
                    else
                    {
                        _Output = "Ok";
                    }
                }

                Dr.Close();
            }
        }
        catch (Exception ex)
        {
            // handle exception if needed
        }

        return _Output;
    }
    private string checkPanno(string Panno)
    {
        string sql = "";
        string _Output = "";
        string errType = "";

        try
        {
            if (!string.IsNullOrWhiteSpace(Panno))
            {
                sql = Obj.IsoStart + "Select Count(*) Cnt FROM " + Obj.dBName + "..M_Membermaster WHERE Panno='" + Panno + "'" + Obj.IsoEnd;
                Comm = new SqlCommand(sql, Connselect);
                Dr = Comm.ExecuteReader();

                if (Dr.Read())
                {
                    if (Convert.ToInt32(Dr["Cnt"]) > 1)
                    {
                        errType = "Pan No"; // already registered on another Ids.
                        _Output = "Faild";
                    }
                    else
                    {
                        _Output = "Ok";
                    }
                }

                Dr.Close();
            }
        }
        catch (Exception ex)
        {
            // handle exception if needed
        }

        return _Output;
    }
    private string checkMobileNo(string MobileNo)
    {
        string sql = "";
        string _Output = "";
        string errType = "";

        try
        {
            if (!string.IsNullOrWhiteSpace(MobileNo))
            {
                sql = Obj.IsoStart + "SELECT COUNT(*) Cnt FROM " + Obj.dBName + "..M_Membermaster WHERE Mobl='" + MobileNo + "'" + Obj.IsoEnd;

                DataTable Dt = new DataTable();
                Dt = SqlHelper.ExecuteDataset(Connselect, CommandType.Text, sql).Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(Dt.Rows[0]["Cnt"]) >= 1)
                    {
                        errType = "Mobile Number";
                        _Output = "Faild";
                    }
                    else
                    {
                        _Output = "Ok";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // log exception if needed
        }

        return _Output;
    }
    private string checkEmailID(string EmailID)
    {
        string sql = "";
        string _Output = "";
        string errType = "";

        try
        {
            if (EmailID == null)
            {
                _Output = "Ok";
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(EmailID))
                {
                    sql = Obj.IsoStart + "SELECT COUNT(*) Cnt FROM " + Obj.dBName + "..M_Membermaster WHERE Email='" + EmailID + "'" + Obj.IsoEnd; ;
                    Comm = new SqlCommand(sql, Connselect);
                    Dr = Comm.ExecuteReader();

                    if (Dr.Read())
                    {
                        if (Convert.ToInt32(Dr["Cnt"]) >= 1)
                        {
                            errType = "Email ID";
                            _Output = "Faild";
                        }
                        else
                        {
                            _Output = "Ok";
                        }
                    }

                    Dr.Close();
                }
            }
        }
        catch (Exception ex)
        {
            // log error if required
        }

        return _Output;
    }
    public string Register(Dictionary<string, string> dict)
    {
        string _Output = "";
        try
        {
            string strQry = "";
            string dblDistrict = "", dblTehsil = "", IfSC = "", dblPlan = "", JoinStatus = "", Category = "", Formno = "0";
            int SessID = 0, dblState = 0, dblBank = 0, InVoiceNo = 0, KitID = 0;
            double Bv = 0, Rp = 0, Kitamount = 0, legno = 0;
            char cGender = 'M', cMarried = 'N';
            string HostIp = HttpContext.Current.Request.UserHostAddress.ToString();
            string _Response = "";
            SqlDataReader DRead;

            try
            {
                if (Session["compid"].ToString() == "1102")
                {
                    if (string.IsNullOrWhiteSpace(dict["fortype"]))
                    {
                        _Output = "{\"response\":\"FAILED\",\"msg\":\"Please Select For Type.\"}";
                        return _Output;
                    }
                    if (dict["fortype"].ToString() == "D")
                    {
                        if (string.IsNullOrWhiteSpace(dict["referralid"]))
                        {
                            _Output = "{\"response\":\"FAILED\",\"msg\":\"Please Enter Sponsor ID.\"}";
                            return _Output;
                        }
                        if (dict["side"] == "0")
                        {
                            _Output = "{\"response\":\"FAILED\",\"msg\":\"Please Select Leg.\"}";
                            return _Output;
                        }
                        if (string.IsNullOrWhiteSpace(dict["panno"]))
                        {
                            _Output = "{\"response\":\"FAILED\",\"msg\":\"Please Enter Panno.\"}";
                            return _Output;
                        }
                        if (checkPanno(dict["panno"]) != "Ok")
                        {
                            _Output = "{\"response\":\"FAILED\",\"msg\":\"Your Pan No. already registered on another Ids.\"}";
                            return _Output;
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(dict["referralid"]))
                    {
                        _Output = "{\"response\":\"FAILED\",\"msg\":\"Please Enter Sponsor ID.\"}";
                        return _Output;
                    }
                }

                if (string.IsNullOrWhiteSpace(dict["name"]))
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"Please Enter Full Name.\"}";
                    return _Output;
                }
                if (dict["mobl"] == "0")
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"Please Enter Mobile No.\"}";
                    return _Output;
                }
                if (dict["email"] == "")
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"Please Enter Email ID.\"}";
                    return _Output;
                }
                if (checkEmailID(dict["email"]) != "Ok")
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"Your Email ID already registered on another Ids.\"}";
                    return _Output;
                }

                if (checkMobileNo(dict["mobl"]) != "Ok")
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"Your Mobile No. already registered on another Ids.\"}";
                    return _Output;
                }
                if (Session["compid"].ToString() == "1102")
                {
                    if (dict["fortype"].ToString() == "D")
                    {
                        legno = Convert.ToInt32(dict["side"]);
                        if (!string.IsNullOrWhiteSpace(dict["referralid"]) && Convert.ToInt32(dict["side"]) > 0)
                        {
                            _Response = Validate_(dict["referralid"], dict["referralid"], dict["side"], "");
                        }
                        else
                        {
                            _Response = "OK";
                        }
                    }
                    else
                    {
                        legno = 0;
                        _RefFormNo = "0";
                        _Response = "OK";
                    }
                }
                else
                {
                    legno = Convert.ToInt32(dict["side"]);
                    if (!string.IsNullOrWhiteSpace(dict["referralid"]) && Convert.ToInt32(dict["side"]) > 0)
                    {
                        _Response = Validate_(dict["referralid"], dict["referralid"], dict["side"], "");
                    }
                    else
                    {
                        _Response = "OK";
                    }
                }


                if (_Response == "OK")
                {
                    Bv = 0; Rp = 0; Category = "Registration"; KitID = 1; JoinStatus = "N";
                    if (_Response == "OK")
                    {
                        dblDistrict = "0";
                        dblTehsil = "0";
                        dblState = 0;
                        dblBank = 0;
                        IfSC = "";
                        dblPlan = "0";
                        InVoiceNo = 0;
                        if (Conn.State == ConnectionState.Closed) Conn.Open();
                        Comm = new SqlCommand(Obj.IsoStart + "Select top 1 SessId as SessId from " + Obj.dBName + "..M_SessnMaster order by SessID desc" + Obj.IsoEnd, Connselect);
                        DRead = Comm.ExecuteReader();
                        SessID = DRead.Read() ? Convert.ToInt32(DRead["SessID"]) : 0;
                        DRead.Close();
                        Comm.Cancel();
                        DateTime Dtp, Dtp1, MariedDate;
                        try { Dtp = Convert.ToDateTime(dict["dob"]); } catch { Dtp = DateTime.Parse("1940-01-01"); }
                        try { Dtp1 = Convert.ToDateTime(dict["transdate"]); } catch { Dtp1 = DateTime.Now; }
                        try { MariedDate = Convert.ToDateTime(dict["marriagedate"]); } catch { MariedDate = DateTime.Now; }
                        Random rand = new Random();
                        string RandomNumber = rand.Next(10000, 99999).ToString();
                        DataTable dt = new DataTable();
                        DAL obj = new DAL();
                        _UpLnFormNo = "0";
                        if (Session["compid"].ToString() == "1102")
                        {
                            strQry = "INSERT INTO m_memberMaster (" +
"SessId,IdNo,CardNo,FormNo,KitId," +
"UpLnFormNo,RefId,LegNo,RefLegNo,RefFormNo," +
"MemFirstName,MemLastName,MemRelation,MemFName,MemDOB,MemGender,MemOccupation," +
"NomineeName,Address1,Address2,Post," +
"Tehsil,City,CityCode,District,DistrictCode,StateCode,CountryId,AreaCode," +
"PinCode,PhN1,Fax,Mobl,MarrgDate," +
"Passw,Doj,Relation,PanNo," +
"BankID,MICRCode,BranchName,EMail,BV," +
"UpGrdSessId,E_MainPassw,EPassw,ActiveStatus,billNo,RP,HostIp," +
"PID,Paymode,ChDDNo,ChDDBankID,ChDDBank,ChddDate,ChDDBranch,IsPanCard,AadharNo,AadharNo2,AAdharNo3,fld6,fld5) " +
"VALUES (" + SessID + ",0,0,0," + KitID + "," + _UpLnFormNo + ",0," + legno + ",0," + _RefFormNo + "," +
"'" + ClearInject(dict["name"].ToUpper()) + "',''," +
"'S/O','','" + Dtp.ToString("dd-MMM-yyyy") + "','" + cGender + "',''," +
"'','','',''," +
dblTehsil + "," + dblTehsil + ",0," +
dblDistrict + ",0," + dblState + ",1,0," +
"'','0','CHOOSE ACCOUNT TYPE','" + dict["mobl"] + "','" + MariedDate.ToString("dd-MMM-yyyy") + "'," +
"'" + RandomNumber + "',GETDATE(),'','" + ClearInject(dict["panno"]) + "'," +
dblBank + ",'','','" + ClearInject(dict["email"]) + "'," +
Bv + ",0,'" + RandomNumber + "','" + RandomNumber + "'," +
"'" + JoinStatus + "'," +
"'" + InVoiceNo + "','" + Rp + "','" + HostIp + "',0,'',''," +
"'0','','" + Dtp1.ToString("dd-MMM-yyyy") + "','','N','','0','0','APP'" +
",'" + ClearInject(dict["fortype"]) + "')";
                        }
                        else
                        {
                            strQry = "INSERT INTO m_memberMaster (" +
"SessId,IdNo,CardNo,FormNo,KitId," +
"UpLnFormNo,RefId,LegNo,RefLegNo,RefFormNo," +
"MemFirstName,MemLastName,MemRelation,MemFName,MemDOB,MemGender,MemOccupation," +
"NomineeName,Address1,Address2,Post," +
"Tehsil,City,CityCode,District,DistrictCode,StateCode,CountryId,AreaCode," +
"PinCode,PhN1,Fax,Mobl,MarrgDate," +
"Passw,Doj,Relation,PanNo," +
"BankID,MICRCode,BranchName,EMail,BV," +
"UpGrdSessId,E_MainPassw,EPassw,ActiveStatus,billNo,RP,HostIp," +
"PID,Paymode,ChDDNo,ChDDBankID,ChDDBank,ChddDate,ChDDBranch,IsPanCard,AadharNo,AadharNo2,AAdharNo3,fld6,fld5) " +
"VALUES (" + SessID + ",0,0,0," + KitID + "," + _UpLnFormNo + ",0," + legno + ",0," + _RefFormNo + "," +
"'" + ClearInject(dict["name"].ToUpper()) + "',''," +
"'S/O','','" + Dtp.ToString("dd-MMM-yyyy") + "','" + cGender + "',''," +
"'','','',''," +
dblTehsil + "," + dblTehsil + ",0," +
dblDistrict + ",0," + dblState + ",1,0," +
"'','0','CHOOSE ACCOUNT TYPE','" + dict["mobl"] + "','" + MariedDate.ToString("dd-MMM-yyyy") + "'," +
"'" + RandomNumber + "',GETDATE(),'','" + ClearInject(dict["panno"]) + "'," +
dblBank + ",'','','" + ClearInject(dict["email"]) + "'," +
Bv + ",0,'" + RandomNumber + "','" + RandomNumber + "'," +
"'" + JoinStatus + "'," +
"'" + InVoiceNo + "','" + Rp + "','" + HostIp + "',0,'',''," +
"'0','','" + Dtp1.ToString("dd-MMM-yyyy") + "','','N','','0','0','APP'" +
",'')";
                        }

                        string Ks = " BEGIN TRY BEGIN TRANSACTION " + strQry + " COMMIT TRANSACTION END TRY BEGIN CATCH ROLLBACK TRANSACTION END CATCH ";
                        int i = 0;
                        if (Conn.State == ConnectionState.Closed) Conn.Open();
                        Comm = new SqlCommand(Ks, Conn);
                        i = Comm.ExecuteNonQuery();
                        Comm.Cancel();
                        string membername = "", Mobl = "", LastInsertid = "", Password = "", lastformno = "", sponsorName = "", sponsorMobl = "";
                        if (i != 0)
                        {
                            Comm = new SqlCommand(Obj.IsoStart + "SELECT TOP 1 a.Mid,a.DSessid, a.IDNO,a.formno,b.IsBill,a.Passw,a.MemFirstname,a.MemlastName,a.Email,a.Mobl," +
                                                  " '' as SponsorName,0 as SponsorMobl FROM " + Obj.dBName + "..m_MemberMaster as a," + Obj.dBName + "..m_KitMaster as b where a.kitid=b.kitid ORDER BY a.mid DESC" + Obj.IsoEnd, Connselect);
                            DRead = Comm.ExecuteReader();
                            if (DRead.Read())
                            {
                                _Output = "{\"response\":\"OK\",\"msg\":\"Registered Successfully!!\",\"idno\":\"" + DRead["IDNo"] + "\",\"password\":\"" + DRead["Passw"] + "\"," +
                                          "\"formno\":\"" + DRead["Formno"] + "\"}";

                                membername = DRead["MemfirstName"] + " " + DRead["MemLastName"];
                                LastInsertid = DRead["idno"].ToString();
                                lastformno = DRead["formno"].ToString();
                                Password = DRead["Passw"].ToString();
                                Mobl = DRead["Mobl"].ToString();
                                HttpContext.Current.Session["Kit"] = DRead["IsBill"];
                                if (Session["compid"].ToString() == "1102")
                                {
                                    if (dict["fortype"].ToString() == "D")
                                    {
                                        string sql = ";Update KycVerify Set PanImg = '',PANImgDate = GETDATE(),IsPanVerified = 'Y',PanVerifyDate = GETDATE(),IsPan = 'Y' where Formno = '" + lastformno + "'";
                                        int j = 0;
                                        try
                                        {
                                            j = Objd.SaveData(sql);
                                        }
                                        catch (Exception e)
                                        {
                                            _Output = "{\"response\":\"OK\",\"msg\":\"Registered Successfully!!\",\"idno\":\"" + DRead["IDNo"] + "\",\"password\":\"" + DRead["Passw"] + "\"," +
                                            "\"formno\":\"" + DRead["Formno"] + "\"}";
                                        }
                                    }
                                }
                            }
                            DRead.Close();
                        }

                    }
                    else
                    {
                        _Output = "{\"response\":\"FAILED\",\"msg\":\"" + _Response + "\"}";
                    }
                }
                else
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"" + _Response + "\"}";
                }
            }
            catch (Exception e)
            {
                _Output = "{\"response\":\"FAILED\",\"msg\":\"" + e.Message + "\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"msg\":\"FAILED\"}";
        }
        return _Output;
    }
    // public string RegisterMakeAnd(Dictionary<string, string> dict)
    // {
    //     string _Output = "";
    //     try
    //     {
    //         string strQry = "";
    //         string dblDistrict = "", dblTehsil = "", IfSC = "", dblPlan = "", JoinStatus = "", Category = "", Formno = "0";
    //         int SessID = 0, dblState = 0, dblBank = 0, InVoiceNo = 0, KitID = 0;
    //         double Bv = 0, Rp = 0, Kitamount = 0, legno = 0;
    //         char cGender = 'M', cMarried = 'N';
    //         string HostIp = HttpContext.Current.Request.UserHostAddress.ToString();
    //         string _Response = "";
    //         SqlDataReader DRead;

    //         try
    //         {
    //             if (string.IsNullOrWhiteSpace(dict["fortype"]))
    //             {
    //                 _Output = "{\"response\":\"FAILED\",\"msg\":\"Please Select For Type.\"}";
    //                 return _Output;
    //             }
    //             if (dict["fortype"].ToString() == "D")
    //             {
    //                 if (string.IsNullOrWhiteSpace(dict["referralid"]))
    //                 {
    //                     _Output = "{\"response\":\"FAILED\",\"msg\":\"Please Enter Sponsor ID.\"}";
    //                     return _Output;
    //                 }
    //                 if (dict["side"] == "0")
    //                 {
    //                     _Output = "{\"response\":\"FAILED\",\"msg\":\"Please Select Leg.\"}";
    //                     return _Output;
    //                 }
    //                 if (string.IsNullOrWhiteSpace(dict["panno"]))
    //                 {
    //                     _Output = "{\"response\":\"FAILED\",\"msg\":\"Please Enter Panno.\"}";
    //                     return _Output;
    //                 }
    //                 if (string.IsNullOrWhiteSpace(dict["aadhar"]))
    //                 {
    //                     _Output = "{\"response\":\"FAILED\",\"msg\":\"Please Enter Aadhar No..\"}";
    //                     return _Output;
    //                 }
    //                 if (checkaadhar(dict["aadhar"]) != "Ok")
    //                 {
    //                     _Output = "{\"response\":\"FAILED\",\"msg\":\"Your Aadhar No. already registered on another Ids.\"}";
    //                     return _Output;
    //                 }
    //                 if (checkPanno(dict["panno"]) != "Ok")
    //                 {
    //                     _Output = "{\"response\":\"FAILED\",\"msg\":\"Your Pan No. already registered on another Ids.\"}";
    //                     return _Output;
    //                 }
    //             }
    //             if (string.IsNullOrWhiteSpace(dict["name"]))
    //             {
    //                 _Output = "{\"response\":\"FAILED\",\"msg\":\"Please Enter Full Name.\"}";
    //                 return _Output;
    //             }
    //             if (dict["mobl"] == "0")
    //             {
    //                 _Output = "{\"response\":\"FAILED\",\"msg\":\"Please Enter Mobile No.\"}";
    //                 return _Output;
    //             }
    //             if (dict["email"] == "")
    //             {
    //                 _Output = "{\"response\":\"FAILED\",\"msg\":\"Please Enter Email ID.\"}";
    //                 return _Output;
    //             }
    //             if (checkEmailID(dict["email"]) != "Ok")
    //             {
    //                 _Output = "{\"response\":\"FAILED\",\"msg\":\"Your Email ID already registered on another Ids.\"}";
    //                 return _Output;
    //             }
    //             if (checkMobileNo(dict["mobl"]) != "Ok")
    //             {
    //                 _Output = "{\"response\":\"FAILED\",\"msg\":\"Your Mobile No. already registered on another Ids.\"}";
    //                 return _Output;
    //             }
    //             if (dict["fortype"].ToString() == "D")
    //             {
    //                 legno = Convert.ToInt32(dict["side"]);
    //                 if (!string.IsNullOrWhiteSpace(dict["referralid"]) && Convert.ToInt32(dict["side"]) > 0)
    //                 {
    //                     _Response = Validate_(dict["referralid"], dict["referralid"], dict["side"], "");
    //                 }
    //                 else
    //                 {
    //                     _Response = "OK";
    //                 }
    //             }
    //             else
    //             {
    //                 legno = 0;
    //                 _RefFormNo = "0";
    //                 _Response = "OK";
    //             }
    //             if (_Response == "OK")
    //             {
    //                 Bv = 0; Rp = 0; Category = "Registration"; KitID = 1; JoinStatus = "N";
    //                 if (_Response == "OK")
    //                 {
    //                     dblDistrict = "0";
    //                     dblTehsil = "0";
    //                     dblState = 0;
    //                     dblBank = 0;
    //                     IfSC = "";
    //                     dblPlan = "0";
    //                     InVoiceNo = 0;
    //                     if (Conn.State == ConnectionState.Closed) Conn.Open();
    //                     Comm = new SqlCommand(Obj.IsoStart + "Select top 1 SessId as SessId from " + Obj.dBName + "..M_SessnMaster order by SessID desc" + Obj.IsoEnd, Connselect);
    //                     DRead = Comm.ExecuteReader();
    //                     SessID = DRead.Read() ? Convert.ToInt32(DRead["SessID"]) : 0;
    //                     DRead.Close();
    //                     Comm.Cancel();
    //                     DateTime Dtp, Dtp1, MariedDate;
    //                     try { Dtp = Convert.ToDateTime(dict["dob"]); } catch { Dtp = DateTime.Parse("1940-01-01"); }
    //                     try { Dtp1 = Convert.ToDateTime(dict["transdate"]); } catch { Dtp1 = DateTime.Now; }
    //                     try { MariedDate = Convert.ToDateTime(dict["marriagedate"]); } catch { MariedDate = DateTime.Now; }
    //                     Random rand = new Random();
    //                     string RandomNumber = rand.Next(10000, 99999).ToString();
    //                     DataTable dt = new DataTable();
    //                     DAL obj = new DAL();
    //                     string statecode = "0";
    //                     if (Conn.State == ConnectionState.Closed) Conn.Open();
    //                     Comm = new SqlCommand(Obj.IsoStart + "Select isnull(Max(statecode),0) as statecode from " + Obj.dBName + "..M_StateDivMaster where activestatus = 'Y' AND rowstatus = 'Y' AND statename = '" + ClearInject(dict["statename"]) + "'" + Obj.IsoEnd, Connselect);
    //                     DRead = Comm.ExecuteReader();
    //                     statecode = DRead.Read() ? Convert.ToString(DRead["statecode"]) : "0";
    //                     DRead.Close();
    //                     Comm.Cancel();
    //                     _UpLnFormNo = "0";
    //                     strQry = "INSERT INTO m_memberMaster (" +
    //"SessId,IdNo,CardNo,FormNo,KitId," +
    //"UpLnFormNo,RefId,LegNo,RefLegNo,RefFormNo," +
    //"MemFirstName,MemLastName,MemRelation,MemFName,MemDOB,MemGender,MemOccupation," +
    //"NomineeName,Address1,Address2,Post," +
    //"Tehsil,City,CityCode,District,DistrictCode,StateCode,CountryId,AreaCode," +
    //"PinCode,PhN1,Fax,Mobl,MarrgDate," +
    //"Passw,Doj,Relation,PanNo," +
    //"BankID,MICRCode,BranchName,EMail,BV," +
    //"UpGrdSessId,E_MainPassw,EPassw,ActiveStatus,billNo,RP,HostIp," +
    //"PID,Paymode,ChDDNo,ChDDBankID,ChDDBank,ChddDate,ChDDBranch,IsPanCard,AadharNo,AadharNo2,AAdharNo3,fld6,fld5) " +
    //"VALUES (" + SessID + ",0,0,0," + KitID + "," + _UpLnFormNo + ",0," + legno + ",0," + _RefFormNo + "," +
    //"'" + ClearInject(dict["name"].ToUpper()) + "',''," +
    //"'S/O','','" + Dtp.ToString("dd-MMM-yyyy") + "','" + cGender + "',''," +
    //"'','" + ClearInject(dict["aadress"]) + "','',''," +
    //dblTehsil + ",'" + ClearInject(dict["city"]) + "',0,'" +
    //ClearInject(dict["district"]) + "',0," + statecode + ",1,0," +
    //"'" + ClearInject(dict["pincode"]) + "','0','CHOOSE ACCOUNT TYPE','" + dict["mobl"] + "','" + MariedDate.ToString("dd-MMM-yyyy") + "'," +
    //"'" + RandomNumber + "',GETDATE(),'','" + ClearInject(dict["panno"]) + "'," +
    //dblBank + ",'','','" + ClearInject(dict["email"]) + "'," +
    //Bv + ",0,'" + RandomNumber + "','" + RandomNumber + "'," +
    //"'" + JoinStatus + "'," +
    //"'" + InVoiceNo + "','" + Rp + "','" + HostIp + "',0,'',''," +
    //"'0','','" + Dtp1.ToString("dd-MMM-yyyy") + "','','N','" + ClearInject(dict["aadhar"]) + "','0','0','APP'" +
    //",'" + ClearInject(dict["fortype"]) + "')";
    //                     string Ks = " BEGIN TRY BEGIN TRANSACTION " + strQry + " COMMIT TRANSACTION END TRY BEGIN CATCH ROLLBACK TRANSACTION END CATCH ";
    //                     int i = 0;
    //                     if (Conn.State == ConnectionState.Closed) Conn.Open();
    //                     Comm = new SqlCommand(Ks, Conn);
    //                     i = Comm.ExecuteNonQuery();
    //                     Comm.Cancel();
    //                     string membername = "", Mobl = "", LastInsertid = "", Password = "", lastformno = "", sponsorName = "", sponsorMobl = "";
    //                     if (i != 0)
    //                     {
    //                         Comm = new SqlCommand(Obj.IsoStart + "SELECT TOP 1 a.Mid,a.DSessid, a.IDNO,a.formno,b.IsBill,a.Passw,a.MemFirstname,a.MemlastName,a.Email,a.Mobl," +
    //                                               " '' as SponsorName,0 as SponsorMobl FROM " + Obj.dBName + "..m_MemberMaster as a," + Obj.dBName + "..m_KitMaster as b where a.kitid=b.kitid ORDER BY a.mid DESC" + Obj.IsoEnd, Connselect);
    //                         DRead = Comm.ExecuteReader();
    //                         if (DRead.Read())
    //                         {
    //                             _Output = "{\"response\":\"OK\",\"msg\":\"Registered Successfully!!\",\"idno\":\"" + DRead["IDNo"] + "\",\"password\":\"" + DRead["Passw"] + "\"," +
    //                                       "\"formno\":\"" + DRead["Formno"] + "\"}";

    //                             membername = DRead["MemfirstName"] + " " + DRead["MemLastName"];
    //                             LastInsertid = DRead["idno"].ToString();
    //                             lastformno = DRead["formno"].ToString();
    //                             Password = DRead["Passw"].ToString();
    //                             Mobl = DRead["Mobl"].ToString();
    //                             HttpContext.Current.Session["Kit"] = DRead["IsBill"];
    //                             if (dict["fortype"].ToString() == "D")
    //                             {
    //                                 string sql = ";Update KycVerify Set PanImg = '',PANImgDate = GETDATE(),IsPanVerified = 'Y',PanVerifyDate = GETDATE(),IsPan = 'Y' where Formno = '" + lastformno + "'";
    //                                 sql = sql + ";Update KycVerify Set Idtype='1',IdProofNo = '" + ClearInject(dict["aadhar"]) + "'," +
    //                              " AddrProof='',BackAddressProof='',BackAddressDate = GETDATE(),IsaddrssVerified='Y',IsAddress = 'Y',AddrssVerifyDate = GETDATE() where Formno= '" + lastformno + "'";

    //                                 int j = 0;
    //                                 try
    //                                 {
    //                                     j = Objd.SaveData(sql);
    //                                 }
    //                                 catch (Exception e)
    //                                 {
    //                                     _Output = "{\"response\":\"OK\",\"msg\":\"Registered Successfully!!\",\"idno\":\"" + DRead["IDNo"] + "\",\"password\":\"" + DRead["Passw"] + "\"," +
    //                                     "\"formno\":\"" + DRead["Formno"] + "\"}";
    //                                 }
    //                             }
    //                         }
    //                         DRead.Close();
    //                     }

    //                 }
    //                 else
    //                 {
    //                     _Output = "{\"response\":\"FAILED\",\"msg\":\"" + _Response + "\"}";
    //                 }
    //             }
    //             else
    //             {
    //                 _Output = "{\"response\":\"FAILED\",\"msg\":\"" + _Response + "\"}";
    //             }
    //         }
    //         catch (Exception e)
    //         {
    //             _Output = "{\"response\":\"FAILED\",\"msg\":\"" + e.Message + "\"}";
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         _Output = "{\"response\":\"FAILED\",\"msg\":\"FAILED\"}";
    //     }
    //     return _Output;
    // }
    public void writeJson(object _object)
    {
        try
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string jsondata = javaScriptSerializer.Serialize(_object);
            writeRaw(jsondata);
        }
        catch (Exception)
        {
            if (Conn != null && Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }
        }
    }
    public void writeRaw(string text)
    {
        try
        {
            Response.Write(text);
        }
        catch (Exception)
        {
            if (Conn != null && Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }
        }
    }
    public string Base64ToImage(string base64string)
    {
        string json = "";
        try
        {
            System.Drawing.Image img;
            System.IO.MemoryStream MS;

            string b64 = base64string.Replace(" ", "+");
            byte[] b = Convert.FromBase64String(b64);
            MS = new System.IO.MemoryStream(b);

            // Create image from memory stream
            img = System.Drawing.Image.FromStream(MS);

            // Resize the image (optional, as in your VB code)
            using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(img, 500, 500))
            {
                using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                {
                    string FlNm = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                    string FilePath = HttpContext.Current.Server.MapPath("images/UploadImage/" + FlNm + ".png");

                    // Save the image to the server path
                    bitmap.Save(FilePath, System.Drawing.Imaging.ImageFormat.Png);

                    // Build public URL for the image
                    string FileName = "https://" + HttpContext.Current.Request.Url.Host + "/images/UploadImage/" + FlNm + ".png";

                    json = "{\"response\":\"OK\",\"type\":\"pancard\",\"image\":\"" + FileName + "\" }";
                }
            }
        }
        catch (Exception ex)
        {
            json = "{\"response\":\"Failed\",\"type\":\"pancard\",\"image\":\"" + ex.Message.ToString() + "\" }";
        }

        return json;
    }
    public string FillSponsor(string userid, string passwd, string Sponsor)
    {
        string _Output = "";
        try
        {
            DataTable DtSponsor = new DataTable();

            string strQry =
               Obj.IsoStart + "Select FormNo, MemFirstName + ' ' + MemLastName as MemName " +
                "from " + Obj.dBName + "..M_MemberMaster " +
                "where IDNo='" + Sponsor + "'" + Obj.IsoEnd;

            using (SqlDataAdapter Adp = new SqlDataAdapter(strQry, (string)HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]]))
            {
                Adp.Fill(DtSponsor);
            }

            if (DtSponsor.Rows.Count > 0)
            {
                _Output = "{\"sponsorno\":\"" + DtSponsor.Rows[0]["FormNo"] + "\"," +
                          "\"sponsornm\":\"" + DtSponsor.Rows[0]["MemName"] + "\"," +
                          "\"response\":\"OK\"}";
            }
            else
            {
                _Output = "{\"response\":\"FAILED\"}";
            }
        }
        catch (Exception)
        {
            _Output = "{\"response\":\"FAILED\"}";
        }

        return _Output;
    }
    public string JsonEncode(string str)
    {
        str = str.Replace("\\", "\\\\");
        str = str.Replace("\"", "\\\"");
        str = str.Replace("//n", " \\n ");
        str = str.Replace("\\n", "\n");
        str = str.Replace("\n", " \\n ");
        if (!string.IsNullOrEmpty(str))
        {
            str = str.Replace(Environment.NewLine, " \\n ");
        }
        str = str.Replace("\r\n", " \\n ");
        str = str.Replace("\t", "\\t");
        return str;
    }
    private string ClearInject(string str)
    {
        string strReturn = str.Replace("'", "''").Replace("\t", " ");
        strReturn = strReturn.Replace("\\\\", "\\");
        if (!string.IsNullOrEmpty(strReturn))
        {
            strReturn = strReturn.Replace(Environment.NewLine, " \\n ");
        }
        return strReturn;
    }
    public void WriteJson(object _object)
    {
        try
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string jsonData = javaScriptSerializer.Serialize(_object);
            WriteRaw(jsonData);
        }
        catch (Exception)
        {
            if (Conn != null)
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
    }
    public void WriteRaw(string text)
    {
        try
        {
            Response.Write(text);
        }
        catch (Exception)
        {
            if (Conn != null)
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
    }
    public DataSet ConvertJsonStringToDataSet(string jsonString)
    {
        XmlDocument xd = new XmlDocument();
        jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
        xd = JsonConvert.DeserializeXmlNode(jsonString);
        DataSet ds = new DataSet();
        ds.ReadXml(new XmlNodeReader(xd));
        return ds;
    }
}
public class GetMsg2
{
    private string _Error;

    public string Response
    {
        get { return _Error; }
        set { _Error = value; }
    }
}
