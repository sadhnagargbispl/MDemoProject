<%@ Application Language="C#" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>

<script RunAt="server">


    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        string path = Request.AppRelativeCurrentExecutionFilePath.ToLower();

        // Redirect to .aspx if no extension is provided
        if (!path.EndsWith(".aspx") && !path.Contains("."))
        {
            Context.RewritePath(path + ".aspx");
        }
    }

    protected void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        // HttpContext.Current.Session["InvDatabase" + Session["CompID"]] = "PhpRoyalInv"; // session not available here normally

        Application["sConnect"] = "Data Source=103.193.74.91,1533;Initial Catalog=MLMMenuMaster;Integrated Security=false;UID=Mlmmenu;PWD=menu@$#123;Pooling=False;Connect Timeout=900000000";
        ScriptManager.ScriptResourceMapping.AddDefinition(
"jquery",
new ScriptResourceDefinition
{
    Path = "~/Scripts/jquery-3.7.1.min.js", // अपनी jQuery file path डालें
    DebugPath = "~/Scripts/jquery-3.7.1.js"
}
);
        // Application["CompID"] = "1003";
    }

    protected void Application_End(object sender, EventArgs e)
    {
        // Code that runs on application shutdown
    }

    protected void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
    }

    protected void Session_Start(object sender, EventArgs e)
    {
       
        getData(); // if you want to call it here, ensure required session keys are present
    }

    protected void Session_End(object sender, EventArgs e)
    {
        // Note: Session_End is raised only when session state mode is InProc
    }
    private void getData()
    {
        try
        {

            var dbConnect = new cls_DataAccess((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]]);
            dbConnect.OpenConnection();
            var dbConnectselect = new cls_DataAccess((string)HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]]);
            dbConnectselect.OpenConnection();
            var session = HttpContext.Current.Session;
            var dbName = Convert.ToString(session["MlmDatabase" + session["CompID"]]);
            //var dbConnect = new cls_DataAccess(dbName);
            //dbConnect.OpenConnection();

            SqlDataReader dRead = null;
            SqlCommand cmd = null;

            // 1) M_CompanyMaster
            cmd = new SqlCommand("select * from M_CompanyMaster", dbConnect.cnnObject);
            dRead = cmd.ExecuteReader();
            if (dRead.Read())
            {
                session["CompName"] = dRead["CompName"] != DBNull.Value ? dRead["CompName"].ToString() : "";
                session["CompAdd"] = dRead["CompAdd"] != DBNull.Value ? dRead["CompAdd"].ToString() : "";
                session["CompWeb"] = (dRead["WebSite"] == DBNull.Value || string.IsNullOrEmpty(dRead["WebSite"].ToString())) ? "index.asp" : dRead["WebSite"].ToString();
                session["Title"] = dRead["CompTitle"] != DBNull.Value ? dRead["CompTitle"].ToString() : "";
                session["CompMail"] = dRead["CompMail"] != DBNull.Value ? dRead["CompMail"].ToString() : "";
                session["CompMobile"] = dRead["MobileNo"] != DBNull.Value ? dRead["MobileNo"].ToString() : "";
                session["ClientId"] = dRead["smsSenderId"] != DBNull.Value ? dRead["smsSenderId"].ToString() : "";
                session["SmsId"] = dRead["smsUserNm"] != DBNull.Value ? dRead["smsUserNm"].ToString() : "";
                session["SmsPass"] = dRead["smPass"] != DBNull.Value ? dRead["smPass"].ToString() : "";
                session["MailPass"] = dRead["mailPass"] != DBNull.Value ? dRead["mailPass"].ToString() : "";
                session["MailHost"] = dRead["mailHost"] != DBNull.Value ? dRead["mailHost"].ToString() : "";
                session["AdminWeb"] = dRead["AdminWeb"] != DBNull.Value ? dRead["AdminWeb"].ToString() : "";
                session["CompCST"] = dRead["CompCSTNo"] != DBNull.Value ? dRead["CompCSTNo"].ToString() : "";
                session["CompState"] = dRead["CompState"] != DBNull.Value ? dRead["CompState"].ToString() : "";
                session["CompDate"] = dRead["RecTimeStamp"] != DBNull.Value ? Convert.ToDateTime(dRead["RecTimeStamp"]).ToString("dd-MMM-yyyy") : "";
                session["Spons"] = "PR10000001";
                session["CompWeb1"] = "www.phproyalvision.com";
                session["CompMovieWeb"] = "";
                session["SmsAPI"] = "";
                session["CompShortUrl"] = dRead["UrlShort"] != DBNull.Value ? dRead["UrlShort"].ToString() : "";
            }
            else
            {
                session["CompName"] = "";
                session["CompAdd"] = "";
                session["CompWeb"] = "";
                session["Title"] = "Welcome";
            }
            dRead.Close();

            // 2) M_ConfigMaster
            cmd = new SqlCommand("select * from M_ConfigMaster", dbConnect.cnnObject);
            dRead = cmd.ExecuteReader();
            if (dRead.Read())
            {
                session["IsGetExtreme"] = dRead["IsGetExtreme"] != DBNull.Value ? dRead["IsGetExtreme"].ToString() : "N";
                session["IsTopUp"] = dRead["IsTopUp"] != DBNull.Value ? dRead["IsTopUp"].ToString() : "N";
                session["IsSendSMS"] = dRead["IsSendSMS"] != DBNull.Value ? dRead["IsSendSMS"].ToString() : "N";
                session["IdNoPrefix"] = dRead["IdNoPrefix"] != DBNull.Value ? dRead["IdNoPrefix"].ToString() : "";
                session["IsFreeJoin"] = dRead["IsFreeJoin"] != DBNull.Value ? dRead["IsFreeJoin"].ToString() : "N";
                session["IsStartJoin"] = dRead["IsStartJoin"] != DBNull.Value ? dRead["IsStartJoin"].ToString() : "N";
                session["JoinStartFrm"] = dRead["JoinStartFrm"] != DBNull.Value ? dRead["JoinStartFrm"].ToString() : "01-Sep-2011";
                session["IsSubPlan"] = dRead["IsSubPlan"] != DBNull.Value ? dRead["IsSubPlan"].ToString() : "N";
                session["Logout"] = dRead["LogoutPg"] != DBNull.Value ? dRead["LogoutPg"].ToString() : "Default.aspx";
            }
            else
            {
                session["IsGetExtreme"] = "N";
                session["IsTopUp"] = "N";
                session["IsSendSMS"] = "N";
                session["IdNoPrefix"] = "";
                session["IsFreeJoin"] = "N";
                session["IsStartJoin"] = "N";
                session["JoinStartFrm"] = "01-Sep-2011";
                session["IsSubPlan"] = "N";
                session["Logout"] = "Default.aspx";
            }
            dRead.Close();

            // 3) Max(SEssid) from D_Monthlypaydetail
            cmd = new SqlCommand("select Max(SEssid) as SessID from D_Monthlypaydetail", dbConnect.cnnObject);
            dRead = cmd.ExecuteReader();
            if (dRead.Read())
            {
                session["MaxSessn"] = dRead["SessID"] != DBNull.Value ? dRead["SessID"].ToString() : "";
            }
            else
            {
                session["MaxSessn"] = "";
            }
            dRead.Close();

            // 4) Max(SEssid) from m_SessnMaster
            cmd = new SqlCommand("select Max(SEssid) as SessID from m_SessnMaster", dbConnect.cnnObject);
            dRead = cmd.ExecuteReader();
            if (dRead.Read())
            {
                session["CurrentSessn"] = dRead["SessID"] != DBNull.Value ? dRead["SessID"].ToString() : "";
            }
            else
            {
                session["CurrentSessn"] = "";
            }
            dRead.Close();

            // Close connection if your cls_DataAccess provides a close method
            dbConnect.CloseConnection();
        }
        catch
        {
            // On any error set defaults
            var session = HttpContext.Current.Session;
            session["CompName"] = "";
            session["CompAdd"] = "";
            session["CompWeb"] = "";
            session["Title"] = "Welcome";
        }
    }
</script>
