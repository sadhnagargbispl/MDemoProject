using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Directlogin : System.Web.UI.Page
{
    string uid;
    string Pwd;
    string type;
    string scrname;
    SqlConnection conn = new SqlConnection();
    SqlCommand Cmm = new SqlCommand();
    int i;
    SqlDataReader dr;
    ModuleFunction objModuleFun;
    clsGeneral objGen = new clsGeneral();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // CreateReq();

            GetCompID();
            getToken();
            objGen.GetConnectionByComp();
            objGen.GetInvDataBaseByComp();
            ColumnName();
            Pages();

            conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
            conn.Open();

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (!Page.IsPostBack)
            {
                getData();

                type = Base64Helpers.Base64Decode(Request["ref"]);
                if (type.ToUpper() == "LOGIN")
                {
                    string info = Base64Helpers.Base64Decode(Request["info"]);
                    string[] s = info.Split(';');

                    //string nowString =
                    //    DateTime.Now.Year.ToString() +
                    //    DateTime.Now.Month.ToString().PadLeft(2, '0') +
                    //    DateTime.Now.Day.ToString().PadLeft(2, '0') +
                    //    DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                    //    DateTime.Now.Minute.ToString().PadLeft(2, '0');

                    //string nowMinusOne =
                    //    (Convert.ToInt64(nowString) - 1).ToString();
                    uid = s[0];
                    Pwd = s[1];

                    if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(Pwd))
                    {
                        if (type == "F")
                            enterFranchisePg();
                        else
                            enterHomePg();
                    }
                    else
                    {
                        Response.Redirect("logout.aspx", false);
                    }
                    //if (nowMinusOne == s[2] || nowString == s[2])
                    //{

                    //}
                    //else
                    //{
                    //    Response.Redirect("logout.aspx", false);
                    //}
                }
                else
                {
                    Response.Redirect("logout.aspx", false);
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void CreateReq()
    {
        try
        {
            string refStr = "Login";
            refStr = Base64Helpers.Base64Encode(refStr);

            string uid = "S223344";
            // uid = Base64Encode(uid);

            string Pwd = "Swastik@1312";
            Pwd = Base64Helpers.Base64Encode(Pwd);

            string info = "S223344;Swastik@1312;" +
                          DateTime.Now.Year.ToString() +
                          DateTime.Today.Month.ToString().PadLeft(2, '0') +
                          DateTime.Now.Day.ToString().PadLeft(2, '0') +
                          DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                          DateTime.Now.Minute.ToString().PadLeft(2, '0');

            info = Base64Helpers.Base64Encode(info);

            string url = "http://localhost:62665/BasicMlm/Directlogin.aspx?ref=" +
                         refStr + "&info=" + info;

            Response.Write(url);
            Response.End();
        }
        catch (Exception ex)
        {
        }
    }
    private void Pages()
    {
        try
        {
            DataTable dtMenu = new DataTable();
            DataSet ds = new DataSet();

            string str = "SELECT a.MenuId as MenuId, a.MenuName, a.ParentId, a.OnSelect " +
                         "FROM M_CompWiseWebMenuMasterDis a " +
                         "WHERE MenuId IN (1,3) AND a.ActiveStatus='Y' AND a.RowStatus='Y' " +
                         "AND a.CompanyID='" + HttpContext.Current.Session["CompID"] + "' " +
                         "ORDER BY CONVERT(decimal, RTRIM(LTRIM(a.Hierar))), a.MenuId";

            ds = SqlHelper.ExecuteDataset(Application["sConnect"].ToString(), CommandType.Text, str);
            dtMenu = ds.Tables[0];

            Session["IndexPage"] = dtMenu.Rows[0]["OnSelect"];
            Session["JoinPage"] = dtMenu.Rows[1]["OnSelect"];
        }
        catch (Exception ex)
        {
        }
    }
    private void ColumnName()
    {
        try
        {
            DataTable dtMenu = new DataTable();
            DataSet ds = new DataSet();

            string str = "SELECT ColName1, ColName2, ColName3, IDNo, Name, ActiveDate " +
                         "FROM M_CompanyColSetting a " +
                         "WHERE a.CompanyID='" + HttpContext.Current.Session["CompID"] + "'";

            ds = SqlHelper.ExecuteDataset(Application["sConnect"].ToString(), CommandType.Text, str);
            dtMenu = ds.Tables[0];

            Session["ColName1"] = dtMenu.Rows[0]["ColName1"];
            Session["ColName2"] = dtMenu.Rows[0]["ColName2"];
            Session["ColName3"] = dtMenu.Rows[0]["ColName3"];

            Session["MPIDNoC"] = dtMenu.Rows[0]["IDNo"];
            Session["MPNameC"] = dtMenu.Rows[0]["Name"];
            Session["MPActiveDateC"] = dtMenu.Rows[0]["ActiveDate"];
        }
        catch (Exception ex)
        {
        }
    }
    public string GetCompID()
    {
        string url = string.Empty;
        SqlConnection conn;

        try
        {
            url = HttpContext.Current.Request.Url.Host
                .ToUpper()
                .Replace("HTTP://", "")
                .Replace("HTTPS://", "")
                .Replace("WWW.", "")
                .Replace("BASICMLM.", "")
                .Replace("CPANEL.", "")
                .Replace("NETWORK.", "");

            string str = string.Empty;

            if (url == "LOCALHOST")
            {
                str = "SELECT ID, Logo, PartyCode, Name, URL, gvPortalCompID, UtiLityPortalID " +
                      "FROM M_CompanyMasterNew WHERE IsActive=1 AND ID='" +
                      ConfigurationManager.AppSettings["CompanyID"] + "'";
            }
            else
            {
                str = "SELECT ID, Logo, PartyCode, Name, URL, gvPortalCompID, UtiLityPortalID " +
                      "FROM M_CompanyMasterNew WHERE IsActive=1 AND UPPER(URL)='" + url + "'";
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
                Response.Redirect("UnderCons.aspx", false);
            }

            dRead.Close();
            conn.Close();
        }
        catch (Exception ex)
        {
        }

        return url;
    }
    private void getToken()
    {
        try
        {
            Session["RLoginUtility"] = "";
            Session["RLoginCashBack"] = "";
            Session["RShopMod"] = "";
            Session["RLoginShopping"] = "";
            Session["UtilityURL"] = "";

            DataSet ds = new DataSet();
            string str = "Exec Sp_GetToken '" + Session["CompID"] + "'";

            ds = SqlHelper.ExecuteDataset(Application["sConnect"].ToString(), CommandType.Text, str);

            // Login Utility Portal  
            if (ds.Tables[0].Rows.Count > 0)
            {
                Session["RLoginUtility"] = ds.Tables[0].Rows[0]["TokenKey"];
                Session["UtilityURL"] = ds.Tables[0].Rows[0]["URL"];
            }

            // Login Cashback Portal  
            if (ds.Tables[1].Rows.Count > 0)
            {
                Session["RLoginCashBack"] = ds.Tables[1].Rows[0]["TokenKey"];
            }

            // Login Shopping Portal  
            if (ds.Tables[2].Rows.Count > 0)
            {
                Session["RShopMod"] = ds.Tables[2].Rows[0]["Mode"];
                Session["RLoginShopping"] = ds.Tables[2].Rows[0]["TokenKey"];
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void getData()
    {
        try
        {
            cls_DataAccess dbConnect = new cls_DataAccess(
                HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString()
            );

            dbConnect.OpenConnection();

            SqlDataReader dRead;
            SqlCommand cmd;

            // ===========================
            // 1. Read Company Master Data
            // ===========================
            cmd = new SqlCommand("SELECT * FROM M_CompanyMaster", dbConnect.cnnObject);
            dRead = cmd.ExecuteReader();

            if (dRead.Read())
            {
                Session["CompAdd"] = dRead["CompAdd"];
                Session["CompWeb"] = (dRead["WebSite"].ToString() == "" ? "index.asp" : dRead["WebSite"]);
                Session["CompMail"] = dRead["CompMail"];
                Session["CompMobile"] = dRead["MobileNo"];
                Session["ClientId"] = dRead["smsSenderId"];
                Session["SmsId"] = dRead["smsUserNm"];
                Session["SmsPass"] = dRead["smPass"];
                Session["MailPass"] = dRead["mailPass"];
                Session["MailHost"] = dRead["mailHost"];
                Session["AdminWeb"] = dRead["AdminWeb"];
                Session["CompCST"] = dRead["CompCSTNo"];
                Session["CompState"] = dRead["CompState"];
                Session["CompDate"] = Convert.ToDateTime(dRead["RecTimeStamp"]).ToString("dd-MMM-yyyy");
                Session["Spons"] = "PR10000001";
                Session["CompWeb1"] = dRead["WebSite"];
                Session["CompMovieWeb"] = "";
                Session["SmsAPI"] = "";

                Session["CompShortUrl"] =
                    "https://" + HttpContext.Current.Request.Url.Host + "/" + Session["JoinPage"];

                Session["CompType"] = dRead["PlanType"];
            }
            else
            {
                Session["CompName"] = "";
                Session["CompAdd"] = "";
                Session["CompWeb"] = "";
                Session["Title"] = "Welcome";
            }
            dRead.Close();

            // ===========================
            // 2. Read Config Master
            // ===========================
            cmd = new SqlCommand("SELECT * FROM M_ConfigMaster", dbConnect.cnnObject);
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
                Session["Logout"] = dRead["LogoutPg"];
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
                Session["Logout"] = "Default.aspx";
            }
            dRead.Close();

            // ===========================
            // 3. Max Session ID (Monthly Pay Detail)
            // ===========================
            cmd = new SqlCommand("SELECT Max(SEssid) AS SessID FROM D_Monthlypaydetail", dbConnect.cnnObject);
            dRead = cmd.ExecuteReader();

            if (dRead.Read())
                Session["MaxSessn"] = dRead["SessID"];
            else
                Session["MaxSessn"] = "";

            dRead.Close();

            // ===========================
            // 4. Current Session (Master)
            // ===========================
            cmd = new SqlCommand("SELECT Max(SEssid) AS SessID FROM m_SessnMaster", dbConnect.cnnObject);
            dRead = cmd.ExecuteReader();

            if (dRead.Read())
                Session["CurrentSessn"] = dRead["SessID"];
            else
                Session["CurrentSessn"] = "";

            dRead.Close();
        }
        catch
        {
            Session["CompName"] = "";
            Session["CompAdd"] = "";
            Session["CompWeb"] = "";
        }
    }
    private void enterHomePg()
    {
        try
        {
            // assume uid and Pwd are class-level fields (string uid, Pwd)
            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(Pwd))
            {
                string scrname = string.Empty;

                var compIdObj = HttpContext.Current.Session["CompId"];
                string compIdStr = compIdObj != null ? compIdObj.ToString() : string.Empty;

                SqlParameter[] prms = new SqlParameter[2];
                prms[0] = new SqlParameter("@UserID", uid);
                prms[1] = new SqlParameter("@Password", Pwd);

                // execute stored proc - first arg is connection string/key like in your other conversions
                string connKey = HttpContext.Current.Session["MlmDatabase" + HttpContext.Current.Session["CompID"]]?.ToString();
                string connKeyMlmSelectDatabase = HttpContext.Current.Session["MlmSelectDatabase" + HttpContext.Current.Session["CompID"]]?.ToString();
                using (SqlDataReader dr = SqlHelper.ExecuteReader(connKeyMlmSelectDatabase, "sp_Login", prms))
                {
                    if (!dr.Read())
                    {
                        // invalid login
                        scrname = "<script language='javascript'>alert('Please Enter valid UserName or Password.');</script>";
                        var page = HttpContext.Current.Handler as System.Web.UI.Page;
                        if (page != null)
                            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "Login Error", scrname, false);
                        return;
                    }
                    else if (dr["IsBlock"] != DBNull.Value && dr["IsBlock"].ToString() == "Y")
                    {
                        scrname = "<script language='javascript'>alert('Your ID is Block due to " + dr["BlockRemark"].ToString() + "');</script>";
                        var page = HttpContext.Current.Handler as System.Web.UI.Page;
                        if (page != null)
                            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "Login Error", scrname, false);
                        dr.Close();
                        return;
                    }
                    else
                    {
                        // successful login: set sessions
                        HttpContext.Current.Session["Run"] = 0;
                        HttpContext.Current.Session["Status"] = "OK";
                        HttpContext.Current.Session["IDNo"] = dr["IDNo"].ToString();
                        HttpContext.Current.Session["EMail"] = dr["EMail"] != DBNull.Value ? dr["EMail"].ToString() : "";
                        HttpContext.Current.Session["FormNo"] = dr["Formno"] != DBNull.Value ? dr["Formno"].ToString() : "";
                        HttpContext.Current.Session["MemName"] = dr["MemFirstName"] != DBNull.Value ? dr["MemFirstName"].ToString() : "";
                        HttpContext.Current.Session["MobileNo"] = dr["Mobl"] != DBNull.Value ? dr["Mobl"].ToString() : "";
                        HttpContext.Current.Session["MemKit"] = dr["KitID"] != DBNull.Value ? dr["KitID"].ToString() : "";
                        HttpContext.Current.Session["Package"] = dr["KitName"] != DBNull.Value ? dr["KitName"].ToString() : "";
                        HttpContext.Current.Session["Position"] = dr["fld3"] != DBNull.Value ? dr["fld3"].ToString() : "";
                        // dates with formatting
                        if (dr["Doj"] != DBNull.Value)
                            HttpContext.Current.Session["Doj"] = Convert.ToDateTime(dr["Doj"]).ToString("dd-MMM-yyyy");
                        else
                            HttpContext.Current.Session["Doj"] = "";

                        if (dr["Upgradedate"] != DBNull.Value)
                            HttpContext.Current.Session["DOA"] = Convert.ToDateTime(dr["Upgradedate"]).ToString("dd-MMM-yyyy");
                        else
                            HttpContext.Current.Session["DOA"] = "";
                        HttpContext.Current.Session["Address"] = dr["Address1"] != DBNull.Value ? dr["Address1"].ToString() : "";
                        HttpContext.Current.Session["IsFranchise"] = dr["Fld5"] != DBNull.Value ? dr["Fld5"].ToString() : "";
                        HttpContext.Current.Session["ActiveStatus"] = dr["ActiveStatus"] != DBNull.Value ? dr["ActiveStatus"].ToString() : "";
                        HttpContext.Current.Session["MemPassw"] = dr["Passw"] != DBNull.Value ? dr["Passw"].ToString() : "";
                        HttpContext.Current.Session["MFormno"] = dr["MFormNo"] != DBNull.Value ? dr["MFormNo"].ToString() : "";
                        HttpContext.Current.Session["MemUpliner"] = dr["UplnFormno"] != DBNull.Value ? dr["UplnFormno"].ToString() : "";
                        HttpContext.Current.Session["customertype"] = dr["Aadharno3"] != DBNull.Value ? dr["Aadharno3"].ToString() : "";
                        HttpContext.Current.Session["Sessncheck"] = "OK" + dr["IDNo"].ToString();
                        // Session("UserName") = dr("UserNm")
                        HttpContext.Current.Session["LoginKey"] = dr["IDNo"].ToString() + "|" + dr["Passw"].ToString() + "|" + HttpContext.Current.Session["gvPortalCompID"];
                        HttpContext.Current.Session["CashBack"] = dr["IDNo"].ToString() + ";" + dr["Passw"].ToString() + ";" + HttpContext.Current.Session["MemName"] + ";" + (dr["Email"] != DBNull.Value ? dr["Email"].ToString() : "") + ";" + (dr["Mobl"] != DBNull.Value ? dr["Mobl"].ToString() : "") + ";" + (dr["Doj"] != DBNull.Value ? Convert.ToDateTime(dr["Doj"]).ToString("dd-MMM-yyyy") : "");
                        HttpContext.Current.Session["LoginUtility"] = "UserName=" + dr["IDNo"].ToString() + "&Password=" + dr["Passw"].ToString() + "&Action=Login";
                        HttpContext.Current.Session["Shopping"] = "uid=" + dr["IDNo"].ToString() + "&pwd=" + dr["Passw"].ToString();
                        HttpContext.Current.Session["Mart"] = "uid=" + dr["IDNo"].ToString() + "&pwd=" + dr["Passw"].ToString();
                        HttpContext.Current.Session["MoviePortal"] = dr["IDNo"].ToString() + ";" + dr["Passw"].ToString();
                        string compId = HttpContext.Current.Session["CompID"] != null ? HttpContext.Current.Session["CompID"].ToString() : "";
                        if (compId == "1108")
                        {
                            HttpContext.Current.Session["RegNo"] = dr["RegNo"] != DBNull.Value ? dr["RegNo"].ToString() : "";
                            HttpContext.Current.Session["RegType"] = dr["RegType"] != DBNull.Value ? dr["RegType"].ToString() : "";
                            HttpContext.Current.Session["AuthorizedPName"] = dr["AuthorizedPName"] != DBNull.Value ? dr["AuthorizedPName"].ToString() : "";
                        }

                    }
                }
                HttpContext.Current.Response.Redirect(HttpContext.Current.Session["IndexPage"]?.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message);
        }
        //try
        //{
        //    if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(Pwd))
        //    {
        //        SqlParameter[] prms = new SqlParameter[2];
        //        prms[0] = new SqlParameter("@UserID", uid);
        //        prms[1] = new SqlParameter("@Password", Pwd);

        //        dr = SqlHelper.ExecuteReader(
        //                HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(),
        //                "sp_Login",
        //                prms
        //             );

        //        if (!dr.Read())
        //        {
        //            dr.Close();
        //            Response.Redirect("logout.aspx", false);
        //            return;
        //        }

        //        Session["Run"] = 0;
        //        Session["Status"] = "OK";

        //        Session["IDNo"] = dr["IDNo"];
        //        Session["FormNo"] = dr["Formno"];
        //        Session["MemName"] = dr["MemFirstName"] + " " + dr["MemLastName"];
        //        Session["MobileNo"] = dr["Mobl"];
        //        Session["MemKit"] = dr["KitID"];
        //        Session["Package"] = dr["KitName"];
        //        Session["Position"] = dr["fld3"];
        //        Session["Doj"] = Convert.ToDateTime(dr["Doj"]).ToString("dd-MMM-yyyy");
        //        Session["DOA"] = Convert.ToDateTime(dr["Upgradedate"]).ToString("dd-MMM-yyyy");
        //        Session["Address"] = dr["Address1"];
        //        Session["IsFranchise"] = dr["Fld5"];
        //        Session["ActiveStatus"] = dr["ActiveStatus"];
        //        Session["MemPassw"] = dr["Passw"];
        //        Session["MFormno"] = dr["MFormNo"];
        //        Session["MemUpliner"] = dr["UplnFormno"];
        //        Session["customertype"] = dr["Aadharno3"];

        //        Session["Sessncheck"] = "OK" + dr["IDNo"];

        //        Session["LoginKey"] = dr["IDNo"] + "|" + dr["Passw"] + "|" + Session["gvPortalCompID"];
        //        Session["CashBack"] = dr["IDNo"] + ";" + dr["Passw"] + ";" + Session["MemName"] + ";" +
        //                              dr["Email"] + ";" + dr["Mobl"] + ";" + Convert.ToDateTime(dr["Doj"]).ToString("dd-MMM-yyyy");

        //        Session["LoginUtility"] = "UserName=" + dr["IDNo"] + "&Password=" +
        //                                  dr["Passw"] + "&Action=Login";

        //        Session["Shopping"] = "uid=" + dr["IDNo"] + "&pwd=" + dr["Passw"];
        //        Session["MoviePortal"] = dr["IDNo"] + ";" + dr["Passw"];

        //        dr.Close();

        //        Response.Redirect(Session["IndexPage"].ToString(), false);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Response.Write(ex.Message);
        //}
    }
    private void enterFranchisePg()
    {
        try
        {
            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(Pwd))
            {
                Cmm = new SqlCommand(
                    "SELECT * FROM M_FranchiseMaster WHERE userid='" + uid + "' AND passw='" + Pwd + "'",
                    conn
                );

                dr = Cmm.ExecuteReader();

                if (!dr.Read())
                {
                    dr.Close();
                    Response.Redirect("Default.aspx?Error=Y", false);
                }
                else
                {
                    Session["Franchise"] = "OK";
                    Session["IDNo"] = dr["UserID"];
                    Session["UserID"] = dr["FormNo"];
                    Session["MemName"] = dr["FranchiseName"];
                    Session["Doj"] = Convert.ToDateTime(dr["Doj"]).ToString("dd-MMM-yyyy");

                    dr.Close();

                    Response.Redirect("Franchise/findex.aspx", false);
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

}