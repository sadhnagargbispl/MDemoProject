using System.Data.SqlClient;
using System.IO;
using System.Net;
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Web;
using System.Web.UI;
using System.Runtime.Remoting;
using System.Web.UI.HtmlControls;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

public partial class Default : System.Web.UI.Page
{
    private string uid = null;
    private string Pwd = null;
    private string Memberid = null;
    private string type = null;
    private string scrname = null;
    private SqlConnection conn = new SqlConnection();
    private SqlCommand Cmm = new SqlCommand();
    private int i = 0;
    private SqlDataReader dr = null;
    private ModuleFunction objModuleFun = null;
    private clsGeneral objGen = new clsGeneral();
    DAL obj;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // Session("TransID") = Format(DateTime.Now(), "yyyyMMddhhmmssfff")
            Session["TransID"] = DateTime.Now.ToString("yyyyMMddhhmmssfff");
            HttpContext.Current.Session["CompID"] = "1108";
            // GetCompID();
            // getToken();
            objGen.GetConnectionByComp();
            objGen.GetInvDataBaseByComp();
           // ColumnName();
            //Pages();
            obj = new DAL();
            getData();
            string decryptedStr = null;
            imgLogo.Src = Session["LogoUrl"].ToString();
            Session["Logo"] = Session["LogoUrl"].ToString();
            // Response.Cache settings
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

            string folderPath = Server.MapPath("~/assets/" + HttpContext.Current.Session["MlmDatabaseName" + Session["CompId"]].ToString() ?? "" + "/");
            string cssFilePath = Path.Combine(folderPath, "admin-color.css");
            // 1. Create folder if not exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            if (!File.Exists(cssFilePath))
            {
                string cssContent = @"/* ------------------------------------------
   PREMIUM DASHBOARD THEME � BY CHATGPT
------------------------------------------- */

/* Google Font */
@import url('https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap');

:root {
    --primary: #4f46e5; /* Indigo */
    --primary-dark: #3730a3;
    --success: #10b981; /* Emerald */
    --warning: #f59e0b; /* Amber */
    --danger: #ef4444; /* Red */
    --info: #0ea5e9; /* Sky Blue */
    --dark: #1e293b; /* Slate */
    --text-gray: #475569;
}

/* ---------- GENERAL PAGE ---------- */


body {
    background: linear-gradient(135deg, #eef2ff 0%, #e0e7ff 100%);
    min-height: 100vh;
    font-family: 'Poppins', sans-serif;
    color: var(--dark);
}

h1, h2, h3, h4, h5, h6 {
    font-weight: 600;
}

p, td, th, span, small {
    font-weight: 400;
    font-size: 14px;
}

/* ---------- CARD DESIGN ---------- */

.card {
    border: none;
    border-radius: 18px;
    box-shadow: 0 8px 30px rgba(0,0,0,0.07);
    overflow: hidden;
    transition: all 0.3s ease;
    background: #ffffff;
}

    .card:hover {
        /*transform: translateY(-6px);*/
        box-shadow: 0 15px 40px rgba(0,0,0,0.12);
    }

/* ---------- CARD HEADER ---------- */

.card-header {
    background: linear-gradient(135deg, var(--primary), var(--primary-dark));
    color: white;
    font-size: 16px;
    font-weight: 600;
    padding: 14px 20px;
    border-bottom: none !important;
}

/* ---------- PROFILE IMAGE ---------- */

.profile-img {
    width: 125px;
    height: 125px;
    object-fit: cover;
    border: 4px solid white;
    box-shadow: 0 10px 25px rgba(0,0,0,0.25);
    border-radius: 50%;
}

/* ---------- STAT CARDS ---------- */

.stat-card {
    background: white;
    border-radius: 18px;
    padding: 1.7rem;
    text-align: center;
    box-shadow: 0 8px 20px rgba(0,0,0,0.08);
    transition: all 0.3s ease;
}

    .stat-card:hover {
        transform: translateY(-5px);
        box-shadow: 0 12px 30px rgba(0,0,0,0.15);
    }

.stat-icon {
    width: 75px;
    height: 75px;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    margin: 0 auto 14px;
    font-size: 28px;
    color: white;
}

/* ICON GRADIENTS */
.bg-grad1 {
    background: linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%);
}

.bg-grad2 {
    background: linear-gradient(135deg, #fb7185 0%, #f43f5e 100%);
}

.bg-grad3 {
    background: linear-gradient(135deg, #3b82f6 0%, #06b6d4 100%);
}

.bg-grad4 {
    background: linear-gradient(135deg, #22c55e 0%, #10b981 100%);
}

/* ---------- COPY BUTTON ---------- */

.copy-btn {
    background: var(--primary);
    border: none;
    color: white;
    padding: 10px 22px;
    border-radius: 10px;
    font-size: 14px;
    font-weight: 500;
    transition: 0.3s ease;
}

    .copy-btn:hover {
        background: var(--primary-dark);
        transform: translateY(-2px);
    }

/* ---------- NEWS SCROLLER ---------- */

.news-marquee {
    height: 300px;
    overflow-y: scroll;
    padding-right: 5px;
}

/* ---------- TABLES ---------- */

.table th {
    background: #f1f5f9;
    font-weight: 600;
    font-size: 14px;
}

.table td {
    font-size: 14px;
}

.table:not(.table-sm) thead th {
    border-bottom: none;
    background-color: #ececec;
    color: #18173c;
    padding-top: 15px;
    padding-bottom: 15px;
}
/* Zebra rows */
.table tbody tr:nth-child(odd) {
    background-color: #f9fafb;
}

.table tbody tr:hover {
    background-color: #eef2ff;
    transition: 0.2s;
}
/* ---------------------------
   GLOBAL CARD TITLE PREMIUM THEME
---------------------------- */

/* Font Family */
body, h1, h2, h3, h4, h5, h6, td, th, p, span, strong {
    font-family: ""Poppins"", sans-serif !important;
}

/* All card titles (stat cards + normal cards) */
.card-header h5,
.stat-card h5,
.card h5,
.card-title {
    font-size: 15px !important;
    font-weight: 600 !important;
    /*color: #e1e4e9 !important; */
    /* Slate Gray � Professional */
    letter-spacing: 0.3px;
    margin-bottom: 4px;
}


.theme-white .text-primary {
    color: #4f4949 !important;
}
/* Amounts / Numbers on cards */
.stat-card h3 {
    font-size: 21px !important;
    font-weight: 700 !important;
    color: #111827 !important; /* Darker premium */
}

/* card-header background (premium blue-purple) */
/*.card-header {
    background: linear-gradient(135deg, #2e815b, #2e815b) !important;
    color: #ffffff !important;
    font-weight: 600;
    font-size: 16px;
    border: none;
    padding: 14px 20px;
}*/
.card-header {
    background: linear-gradient(135deg, #ffffff, #ffffff) !important;
    color: #181717 !important;
    font-weight: 600;
    font-size: 16px;
    border: none;
    padding: 14px 20px;
}
/* stat-card design */
.stat-card {
    background: #ffffff;
    border-radius: 16px;
    padding: 1.2rem 1rem;
    text-align: center;
    border: 1px solid #f1f5f9;
    box-shadow: 0 4px 18px rgba(0,0,0,0.06);
    transition: 0.25s;
}

    .stat-card:hover {
        transform: translateY(-4px);
        box-shadow: 0 12px 32px rgba(0,0,0,0.12);
    }

/* Icon smaller and premium */
.stat-icon {
    width: 55px;
    height: 55px;
    border-radius: 14px;
    font-size: 22px;
    display: flex;
    align-items: center;
    justify-content: center;
    margin: 0 auto 12px;
    color: white;
}
/* Member card - professional modern style */
.member-card {
    border: none;
    border-radius: 14px;
    background: #ffffff;
    box-shadow: 0 10px 30px rgba(18, 24, 37, 0.06);
    padding: 18px;
    overflow: hidden;
}

    /* container inside card that aligns image + text */
    .member-card .card-body {
        display: flex;
        gap: 22px;
        align-items: center;
        flex-wrap: nowrap;
        padding: 12px;
    }

/* profile circle */
.profile-wrap {
    width: 110px;
    height: 110px;
    min-width: 110px;
    border-radius: 50%;
    overflow: hidden;
    display: flex;
    align-items: center;
    justify-content: center;
    background: linear-gradient(180deg,#f3f6fb,#ffffff);
    box-shadow: 0 12px 30px rgba(37,50,88,0.08);
    border: 3px solid rgba(74,92,255,0.06);
}

/* actual image */
.profile-img {
    width: 100%;
    height: 100%;
    object-fit: cover;
    display: block;
}

/* Text area */
.member-text {
    flex: 1 1 auto;
    text-align: left;
    padding-right: 8px;
}

/* Member name row */
.member-title {
    font-family: 'Inter', system-ui, -apple-system, 'Segoe UI', Roboto, 'Helvetica Neue', Arial;
    font-size: 1.02rem;
    font-weight: 600;
    color: #111827;
    margin: 0 0 6px 0;
    display: block;
}

    /* label inside title */
    .member-title .label {
        font-weight: 600;
        color: #374151;
        margin-right: 6px;
        font-size: 0.95rem;
    }

/* highlighted value */
.value {
    color: #2e815b; /* premium blue */
    font-weight: 700;
    letter-spacing: 0.25px;
    display: inline-block;
}

/* sub details row */
.member-sub {
    font-size: 0.93rem;
    color: #4b5563;
    margin: 0;
    display: flex;
    align-items: center;
    gap: 10px;
    flex-wrap: wrap;
}

/* small dot separator */
.dot {
    color: #bfc9d9;
    margin: 0 6px;
    font-weight: 700;
}

/* rank small badge */
.rank-badge {
    background: linear-gradient(90deg,#fff,#f3f6ff);
    border: 1px solid rgba(65,89,255,0.08);
    color: #0f172a;
    padding: 6px 10px;
    border-radius: 16px;
    font-weight: 600;
    font-size: 0.9rem;
    display: inline-block;
}

/* responsive for small screens */
@media (max-width: 576px) {
    .member-card .card-body {
        flex-direction: row;
        gap: 12px;
    }

    .member-text {
        padding-right: 0;
    }

    .profile-wrap {
        width: 88px;
        height: 88px;
        min-width: 88px;
    }
}
/* Quick Links (small icon buttons next to profile) */
.quick-links {
    display: flex;
    gap: 12px;
    align-items: center;
    margin-left: 8px;
    flex-wrap: wrap;
}

/* Each quick link item */
.ql-item {
    width: 64px;
    text-align: center;
    font-size: 12px;
    color: var(--text-gray, #475569);
    text-decoration: none;
    display: inline-flex;
    flex-direction: column;
    align-items: center;
    gap: 6px;
    transform: translateY(0);
    transition: transform .18s ease, box-shadow .18s ease;
}

/* icon circle */
.ql-icon {
    width: 46px;
    height: 46px;
    border-radius: 12px;
    display: inline-flex;
    align-items: center;
    justify-content: center;
    background: linear-gradient(180deg,#ffffff, #f3f6ff);
    border: 1px solid rgba(74,92,255,0.06);
    box-shadow: 0 6px 16px rgba(37,50,88,0.06);
    font-size: 20px;
    color: var(--primary, #4f46e5);
}

/* label below icon */
.ql-label {
    font-size: 12px;
    max-width: 70px;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    color: #263238;
}

/* hover effect */
.ql-item:hover .ql-icon {
    transform: translateY(-4px);
    box-shadow: 0 10px 28px rgba(37,50,88,0.12);
}

.ql-item:hover .ql-label {
    color: var(--primary, #4f46e5);
}

/* compact variant for small cards */
.quick-links.compact .ql-item {
    width: 54px;
}

.quick-links.compact .ql-icon {
    width: 40px;
    height: 40px;
    border-radius: 10px;
    font-size: 18px;
}

/* responsive (stack under text on very small screens) */
@media (max-width:420px) {
    .card-body {
        gap: 12px;
    }

    .quick-links {
        margin-left: 0;
        margin-top: 10px;
    }
}
/* Referral card responsive styling */
.ref-card {
    border-radius: 12px;
    overflow: hidden;
    background: #fff;
    box-shadow: 0 8px 24px rgba(32,40,80,0.06);
    border: 1px solid #eef2ff;
}

    .ref-card .card-header {
        background: linear-gradient(135deg,#6d28d9,#4f46e5);
        color: #fff;
        padding: 12px 16px;
        font-weight: 600;
        border-bottom: 1px solid rgba(255,255,255,0.06);
    }

    .ref-card .card-body {
        padding: 14px;
    }

/* Grid row: label | link | button */
.ref-row {
    display: grid;
    grid-template-columns: 180px 1fr 120px;
    gap: 12px;
    align-items: center;
    width: 100%;
}

    /* label red (same as your screenshot) */
    .ref-row .ref-label {
        color: #d32f2f;
        font-weight: 700;
        padding: 8px 6px;
    }

    /* link cell */
    .ref-row .ref-link {
        padding: 8px 6px;
        font-weight: 500;
        color: #1b4ed6;
        word-break: break-word; /* wrap very long URL */
        hyphens: auto;
        max-width: 100%;
        overflow-wrap: anywhere;
    }

    /* copy button cell center */
    .ref-row .ref-action {
        text-align: center;
    }

/* small screen: stack */
@media (max-width: 768px) {
    .ref-row {
        grid-template-columns: 1fr; /* single column stack */
        gap: 8px;
    }

        .ref-row .ref-action {
            text-align: left;
        }
}

/* make table fallback responsive (if you keep table) */
.table-responsive-custom {
    width: 100%;
    overflow-x: auto;
    -webkit-overflow-scrolling: touch;
}

/* subtle link style */
.ref-link a {
    color: #1b4ed6;
    text-decoration: none;
}

    .ref-link a:hover {
        text-decoration: underline;
    }

/* small helpers */
.copy-btn {
    padding: 8px 14px;
    border-radius: 8px;
    border: none;
    background: #2e815b;
    color: #fff;
    cursor: pointer;
    font-weight: 600;
}

    .copy-btn:active {
        transform: translateY(1px);
    }

.theme-white .btn-primary {
    background-color: #2e815b;
    border-color: transparent !important;
    color: #fff;
}

    .theme-white .btn-primary:hover {
        background-color: #2e815b !important;
        color: #fff;
    }

    .theme-white .btn-primary:focus:active {
        background-color: #2e815b !important;
    }
";
                File.WriteAllText(cssFilePath, cssContent);
            }
            if (!Page.IsPostBack)
            {
                // MakeConnection() commented out in original

                if (Request["lgnT"] != null)
                {
                    objModuleFun = new ModuleFunction((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]]);
                    decryptedStr = Crypto.Decrypt(Request["lgnT"].Replace(" ", "+"));

                    DateTime currentDate = DateTime.Now;
                    string result = currentDate.Day.ToString()
                                    + currentDate.Hour.ToString()
                                    + currentDate.Year.ToString()
                                    + (currentDate.Month - 1).ToString();

                    if (decryptedStr.Contains("uid="))
                    {
                        int UIdIndx = decryptedStr.IndexOf("&pwd");
                        if (UIdIndx > 4)
                        {
                            uid = decryptedStr.Substring(4, UIdIndx - 4);
                            Pwd = decryptedStr.Substring(UIdIndx + 5, decryptedStr.Length - UIdIndx - 5);
                        }
                    }
                    //if (result == Request["ID"])
                    //{

                    //}
                    //else
                    //{
                    //    Response.Redirect("logout.aspx", false);
                    //}
                }
                else if (Request["uid"] != null)
                {
                    uid = Request["uid"];
                    Pwd = Request["pwd"];
                    type = Request["ref"];

                    uid = uid.Trim().Replace("'", "").Replace("=", "").Replace(";", "");
                    Pwd = Pwd.Trim().Replace("'", "").Replace("=", "").Replace(";", "");
                }

                if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(Pwd))
                {
                    if (type == "F")
                        enterFranchisePg();
                    else
                        enterHomePg();
                }
                // else original had commented redirect to Default.aspx?Error=Y
            }

            // Add CSS based on CompId
            object compIdObj = Session["CompId"];

            if (compIdObj != null)
            {
                int compId;
                if (int.TryParse(compIdObj.ToString(), out compId))
                {
                    string cssPath = null;
                    //switch (compId)
                    //{
                    //    case 1083: cssPath = "assets/fis/admin-color.css"; break;
                    //    case 1088: cssPath = "assets/viv/admin-color.css"; break;
                    //    case 1089: cssPath = "assets/mega/admin-color.css"; break;
                    //    case 1090: cssPath = "assets/genesis/admin-color.css"; break;
                    //    case 1091: cssPath = "assets/solfit/admin-color.css"; break;
                    //    case 1092: cssPath = "assets/A7kart/admin-color.css"; break;
                    //    case 1093: cssPath = "assets/Swastik/admin-color.css"; break;
                    //    case 1095: cssPath = "assets/Aarogya/admin-color.css"; break;
                    //    case 1097: cssPath = "assets/JoshMart/admin-color.css"; break;
                    //    case 1100: cssPath = "assets/SOLBRIGHT/admin-color.css"; break;
                    //    case 1101: cssPath = "assets/Runecha/admin-color.css"; break;
                    //}

                    if (!string.IsNullOrEmpty(cssPath))
                    {
                        var link = new HtmlLink();
                        link.Href = cssPath;
                        link.Attributes["rel"] = "stylesheet";
                        link.Attributes["type"] = "text/css";
                        Page.Header.Controls.Add(link);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // original VB just did conn.Close() on exception
            if (conn != null)
            {
                try { conn.Close(); } catch { /* ignore close errors */ }
            }

            // optionally log ex if you have a logger
            // Logger.LogError(ex);
        }

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
                HttpContext.Current.Session["CompID"] = dRead["ID"].ToString();
                HttpContext.Current.Session["Logo"] = dRead["Logo"].ToString();

                if (HttpContext.Current.Session["CompID"].ToString() == "1069")
                {
                    imgLogoZienva.Src = dRead["Logo"].ToString();
                }
                else
                {
                    imgLogo.Src = dRead["Logo"].ToString();
                }

                HttpContext.Current.Session["WRPartyCode"] = dRead["PartyCode"].ToString();
                HttpContext.Current.Session["CompName"] = dRead["Name"].ToString();
                HttpContext.Current.Session["Title"] = "Welcome To " + dRead["Name"].ToString();

                // set home HRef depending on comp id
                string compIdStr = HttpContext.Current.Session["CompID"].ToString();
                if (compIdStr == "1069")
                {
                    ahome.HRef = "http://" + dRead["URL"].ToString();
                }
                else if (compIdStr == "1101")
                {
                    ahome.HRef = "https://runecha.com/";
                }
                else
                {
                    ahome.HRef = "https://" + dRead["URL"].ToString();
                }

                HttpContext.Current.Session["gvPortalCompID"] = dRead["gvPortalCompID"].ToString();
                HttpContext.Current.Session["UtiLityPortalID"] = dRead["UtiLityPortalID"].ToString();
            }
            else
            {
                // Response.Redirect("UnderCons.aspx", false); // keep commented as in original
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
    private void getToken()
    {
        try
        {
            HttpContext.Current.Session["RLoginUtility"] = "";
            HttpContext.Current.Session["RLoginCashBack"] = "";
            HttpContext.Current.Session["RShopMod"] = "";
            HttpContext.Current.Session["RLoginShopping"] = "";
            HttpContext.Current.Session["UtilityURL"] = "";

            DataSet ds = new DataSet();
            string str = " Exec Sp_GetToken '" + HttpContext.Current.Session["CompID"] + "' ";
            ds = SqlHelper.ExecuteDataset(Application["sConnect"].ToString(), CommandType.Text, str);

            // For Login Utility Portal
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                HttpContext.Current.Session["RLoginUtility"] = ds.Tables[0].Rows[0]["TokenKey"].ToString();
                HttpContext.Current.Session["UtilityURL"] = ds.Tables[0].Rows[0]["URL"].ToString();
            }
            else
            {
                HttpContext.Current.Session["RLoginUtility"] = "";
                HttpContext.Current.Session["UtilityURL"] = "";
            }

            // For Login Cashback Portal
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                HttpContext.Current.Session["RLoginCashBack"] = ds.Tables[1].Rows[0]["TokenKey"].ToString();
            }
            else
            {
                HttpContext.Current.Session["RLoginCashBack"] = "";
            }

            // For Login Shopping Portal
            if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
            {
                HttpContext.Current.Session["RShopMod"] = ds.Tables[2].Rows[0]["Mode"].ToString();
                HttpContext.Current.Session["RLoginShopping"] = ds.Tables[2].Rows[0]["TokenKey"].ToString();
            }
            else
            {
                HttpContext.Current.Session["RShopMod"] = "";
                HttpContext.Current.Session["RLoginShopping"] = "";
            }
        }
        catch (Exception ex)
        {
            // Consider logging ex
        }
    }
    private void Pages()
    {
        try
        {
            DataTable dtMenu = new DataTable();
            DataSet ds = new DataSet();

            string str = " Select a.MenuId as MenuId, a.MenuName as MenuName, a.ParentId as ParentId, a.OnSelect as OnSelect from ";
            str += " M_CompWiseWebMenuMasterDis a Where MenuId in (1,3) And  a.ActiveStatus = 'Y' And a.RowStatus ='Y' And a.CompanyID = '"
                   + HttpContext.Current.Session["CompID"] + "' order by Convert(decimal,RTRIM(LTRIM(a.Hierar))),a.MenuId";

            ds = SqlHelper.ExecuteDataset(Application["sConnect"].ToString(), CommandType.Text, str);
            dtMenu = ds.Tables[0];

            Session["IndexPage"] = dtMenu.Rows[0]["OnSelect"].ToString();
            if (Session["CompID"].ToString() == "1108" || Session["CompID"].ToString() == "1107" || Session["CompID"].ToString() == "1109" || Session["CompID"].ToString() == "1110")
            {
                Regdiv.Visible = true;
            }
            if (Session["CompID"].ToString() == "1093")
            {
                Regdiv.Visible = false;
            }
            else if (Session["CompID"].ToString() == "1102")
            {
                Regdiv.Visible = false;
            }
            else
            {
                Session["JoinPage"] = dtMenu.Rows[1]["OnSelect"].ToString();
                aJoining.HRef = Session["JoinPage"].ToString();
                Regdiv.Visible = true;
            }
        }
        catch (Exception ex)
        {
            // Handle error if needed
        }
    }
    private void ColumnName()
    {
        try
        {
            DataTable dtMenu = new DataTable();
            DataSet ds = new DataSet();

            string str = " Select ColName1,ColName2,ColName3,IDNo,Name,ActiveDate ";
            str += " from M_CompanyColSetting a  Where  a.CompanyID = '"
                   + HttpContext.Current.Session["CompID"] + "'";

            ds = SqlHelper.ExecuteDataset(Application["sConnect"].ToString(), CommandType.Text, str);
            dtMenu = ds.Tables[0];

            Session["ColName1"] = dtMenu.Rows[0]["ColName1"].ToString();
            Session["ColName2"] = dtMenu.Rows[0]["ColName2"].ToString();
            Session["ColName3"] = dtMenu.Rows[0]["ColName3"].ToString();

            Session["MPIDNoC"] = dtMenu.Rows[0]["IDNo"].ToString();
            Session["MPNameC"] = dtMenu.Rows[0]["Name"].ToString();
            Session["MPActiveDateC"] = dtMenu.Rows[0]["ActiveDate"].ToString();
        }
        catch (Exception ex)
        {
            // Handle error if needed
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
                    HttpContext.Current.Session["LogoUrl"] = dRead["LogoUrl"]?.ToString() ?? "";
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
    private string ClearInject(string StrObj)
    {
        try
        {
            StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "");
            return StrObj.Trim();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
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
                HttpContext.Current.Session["IndexPage"] = "indextb.aspx";
                Session["Logo"] = "";
                HttpContext.Current.Response.Redirect(HttpContext.Current.Session["IndexPage"]?.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message);
        }
    }
    private void enterFranchisePg()
    {
        try
        {
            if (uid.Length > 0 && Pwd.Length > 0)
            {
                string Str = obj.IsoStart + " Select * from " + obj.dBName + "..M_FranchiseMaster where userid='" + uid + "' and passw='" + Pwd + "' " + obj.IsoEnd;
                DataTable Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + HttpContext.Current.Session["CompID"]]?.ToString(), CommandType.Text, Str).Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    Response.Redirect("Login.aspx?Error=Y", false);
                }
                else
                {
                    Session["Franchise"] = "OK";
                    Session["IDNo"] = dr["UserID"];
                    Session["UserID"] = dr["FormNo"];
                    Session["MemName"] = dr["FranchiseName"];
                    Session["Doj"] = ((DateTime)Dt.Rows[0]["Doj"]).ToString("dd-MMM-yyyy");
                    Response.Redirect("Franchise/findex.aspx", false);
                    dr.Close();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        try
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void Page_Unload(object sender, EventArgs e)
    {
        try
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request["uid"] != null)
            {
                uid = Request["uid"];
                Pwd = Request["pwd"];
            }
            else
            {
                uid = Txtuid.Value;
                Pwd = Txtpwd.Value;
            }
            type = Request["ref"];
            uid = uid.Replace("'", "").Replace("=", "").Replace(";", "");
            Pwd = Pwd.Replace("'", "").Replace("=", "").Replace(";", "");
            if (string.IsNullOrEmpty(uid))
            {
                scrname = "<script language='javascript'>alert('Please Enter User ID.');</script>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                return;
            }
            if (string.IsNullOrEmpty(Pwd))
            {
                scrname = "<script language='javascript'>alert('Please Enter Password.');</script>";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Login Error", scrname, false);
                return;

            }
            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(Pwd))
            {
                if (type == "F")
                {
                    enterFranchisePg();
                }
                else
                {
                    enterHomePg();
                }
            }
            else
            {
                Response.Redirect("logout.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
}
