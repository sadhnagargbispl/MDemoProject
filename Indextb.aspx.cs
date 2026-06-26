using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.VariantTypes;
using System.Configuration;
using System.IO;
using System.Net;
using Newtonsoft.Json;

public partial class Indextb : System.Web.UI.Page
{
    DAL Obj;
    DataSet Ds;
    clsGeneral objGen = new clsGeneral();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                Session["TransID"] = DateTime.Now.ToString("yyyyMMddhhmmssfff");

                if (!Page.IsPostBack)
                {
                  
                    if (Session["CompID"].ToString() == "1106")
                    {
                        Referalreq referalreq = new Referalreq();
                        referalreq.islogin = "N";
                        referalreq.reqtype = "referrallink";
                        referalreq.userid = Convert.ToString(Session["IDNo"]);
                        referalreq.passwd = Convert.ToString(Session["MemPassw"]);
                        // Request ko JSON me convert karna
                        string detail = JsonConvert.SerializeObject(referalreq);
                        // API Call
                        string response = CallPostFunction(detail, "https://cpanel.myhemalika.com/ProcessAPIWithK.aspx");
                        // Response ko Object me convert karna
                        Referalres referalres = JsonConvert.DeserializeObject<Referalres>(response);

                        // Check Response
                        if (referalres != null && referalres.response == "OK")
                        {
                            lblLinkupdate.Text = referalres.urlLeft;
                            aRfLinkupdate.HRef = lblLinkupdate.Text;
                        }
                    }

                    // Compid = 1097
                    if (Session["CompID"].ToString() == "1097" || Session["CompID"].ToString() == "1102" || Session["CompID"].ToString() == "1106" )
                    {
                        Div13.Visible = false;
                        Div4.Visible = false;
                    }
                    else
                    {
                        if (Session["CompID"].ToString() == "1109")
                        {
                            Div13.Visible = false;
                            Div4.Visible = true;
                        }
                        else
                        {
                            Div13.Visible = true;
                            Div4.Visible = false;
                        }


                        
                    }
                    string companyId = Session["CompID"].ToString(); // ya jahan se CompID aata ho
                    
                    if (companyId == "1108")
                    {
                        divwithdrawal.Visible = true;
                        BindWithdrawalStatus();
                    }
                    else
                    {
                        divwithdrawal.Visible = false;
                    }
                    if (companyId == "1106")
                    {
                        divProfile.Attributes["class"] = "col-lg-6";
                        Div1.Visible = true;
                        DivIncomeSummary.Visible = false;
                        DivWalletSummary.Visible = false;
                    }
                    else
                    {
                        DivIncomeSummary.Visible = true;
                        DivWalletSummary.Visible = true;
                        divProfile.Attributes["class"] = "col-lg-12";
                        Div1.Visible = false;
                    }
                    if (companyId == "1105")
                    {
                        DivNews.Attributes["class"] = "col-lg-4";
                        divTotalBusiness.Visible = false;
                    }
                    else if (companyId == "1107" || companyId == "1108" || companyId == "1109" || companyId == "1110")
                    {
                        DivNews.Attributes["class"] = "col-lg-4";
                        divTotalBusiness.Visible = true;
                    }
                    else if (companyId == "1106")
                    {
                        DivNews.Attributes["class"] = "col-lg-6";
                        DivMyKYCStatus.Visible = true;
                        divTotalBusiness.Visible = false;
                        DivPurchaseVolume.Visible = true;
                    }
                    else
                    {
                        DivNews.Attributes["class"] = "col-lg-12";
                        divTotalBusiness.Visible = true;
                        DivMyKYCStatus.Visible = false;
                        DivPurchaseVolume.Visible = false;
                    }
                    // divVedicindia.Visible = false;
                    // Compid = 1007
                    if (companyId != null && companyId == "1010")
                        Fill_WellValueBonanza();
                    else
                        Div20.Visible = false;
                    // Compid = 1091
                    if (companyId != null && companyId == "1091")
                        Fill_solfitBonanza();
                    else
                        Div20.Visible = false;
                    if (companyId != null && companyId == "1107" && companyId == "1108" && companyId == "1109" && companyId == "1110")
                    {
                        FilldirectDirectBusinessReward();
                        DivEV.Visible = true;
                    }
                    else
                    {
                        DivEV.Visible = false;
                    }
                    // Compid = 1074
                    if (companyId != null && companyId == "1074")
                        Fill_CashlessPopup();
                    GetDashboardIncome();
                    GetTotalDirects();
                    LoadTeam();
                    Obj = new DAL();
                    LoadProfileImage();
                    Filldirect();
                    Load_Headings();
                    Fill_Balance();

                    if (companyId == "1106")
                    {
                        FillKYCStatus();
                        FillPPVStatus();
                    }
                    if (companyId != null && companyId == "1106")
                    {
                        lblLink.Text = Session["CompShortUrl"] + "?ref=" + Crypto.Encrypt(Session["IdNo"] + "/1") + "";
                        aRfLink.HRef = lblLink.Text;
                        trReferalHead.Visible = false;
                    }
                    else
                    {
                        lblLinkgreen.Text = Session["CompShortUrl"] + "?ref=" + Crypto.Encrypt(Session["IdNo"] + "/0") + "";
                        aRfLinkgreen.HRef = lblLinkgreen.Text;

                        lblLink.Text = Session["CompShortUrl"] + "?ref=" +
                                   Crypto.Encrypt(Session["IdNo"] + "/1") + "&side=Left";
                        aRfLink.HRef = lblLink.Text;

                        lbllink1.Text = Session["CompShortUrl"] + "?ref=" +
                                        Crypto.Encrypt(Session["IdNo"] + "/2") + "&side=Right";
                        aRfLink1.HRef = lbllink1.Text;
                        trReferalHead.Visible = true;
                    }
                    DivDirectIncome.Visible = false;
                    divAppAmount.Visible = false;
                    divPairCut.Visible = false;
                }
            }
            else
            {
                Response.Redirect("logout.aspx");
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void BindWithdrawalStatus()
    {
        DataTable dt = GetWithdrawalCounts();

        foreach (DataRow row in dt.Rows)
        {
            row["StatusHtml"] = GetStatusHtml(
                row["Status"].ToString(), row["TotalAmount"].ToString());
        }

        Repeater1.DataSource = dt;
        Repeater1.DataBind();
    }

    private DataTable GetWithdrawalCounts()
    {
        // Saare 3 status hamesha dikhenge, chahe count 0 ho.
        // JOIN trim/case-safe hai taaki DB me extra space ya case diff ho to bhi match ho.
        string sql = @"
SELECT 
    s.Status,
    ISNULL(SUM(w.Netamount), 0) AS TotalAmount
FROM (VALUES ('A'), ('P'), ('R')) AS s(Status)
LEFT JOIN (
    SELECT 
        LTRIM(RTRIM(Status)) AS Status,
        SUM(Netamount) AS Netamount
    FROM fundwithdrawls
    WHERE FormNo = @FormNo
    GROUP BY LTRIM(RTRIM(Status))
) w ON LOWER(w.Status) = LOWER(s.Status)
GROUP BY s.Status
ORDER BY s.Status";

        DataTable dt = new DataTable();

        string cs = HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString();

        using (SqlConnection con = new SqlConnection(cs))
        using (SqlCommand cmd = new SqlCommand(sql, con))
        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            cmd.Parameters.AddWithValue("@FormNo", Session["FormNo"].ToString());

            da.Fill(dt);
        }

        if (!dt.Columns.Contains("StatusHtml"))
            dt.Columns.Add("StatusHtml", typeof(string));

   

        return dt;
    }
    public static string GetStatusHtml(string status, string count)
    {
        string cssClass, icon, label;

        switch ((status ?? "").Trim().ToUpperInvariant())
        {
            case "A":
                cssClass = "sc-approved"; icon = "fa-check-circle"; label = "APPROVED";
                break;
            case "R":
                cssClass = "sc-rejected"; icon = "fa-times-circle"; label = "REJECTED";
                break;
            case "P":
            default:
                cssClass = "sc-pending"; icon = "fa-clock-o"; label = "PENDING";
                break;
        }

        return string.Format(
            "<div class=\"status-count-box {0}\">" +
            "<div class=\"status-count-label\"><i class=\"fa {1}\"></i>{2}</div>" +
            "<p class=\"status-count-num\">{3}</p></div>",
            cssClass, icon, label, count);
    }
    public string CallPostFunction(string detail, string url)
    {
        try
        {
            // Create a request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(detail);
                streamWriter.Flush();
            }
            // Get the response
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    return result; // Optionally deserialize this JSON string to an object
                }
            }
        }
        catch (Exception ex)
        {
            var message = ex.Message;
        }
        return "";
    }
    private void Load_Headings()
    {
        try
        {
            string Str = Obj.IsoStart + "SELECT TOP 2 *, '' AS divstart FROM " + Obj.dBName + "..M_PopupMaster " +
                         "WHERE ActiveStatus='Y' AND showOn IN ('D','B') " +
                         "ORDER BY Pid DESC" + Obj.IsoEnd;

            Obj = new DAL();

            DataTable Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Str).Tables[0];

            if (Dt.Rows.Count > 0)
            {
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        Dt.Rows[i]["divstart"] = "<div><a class=\"fbox\" id=\"First\" href=\"" +
                                                 Dt.Rows[i]["ImgPath"] + "\"></a></div>";
                    }
                    if (i == 1)
                    {
                        Dt.Rows[i]["divstart"] = "<div><a class=\"fbox\" id=\"Second\" href=\"" +
                                                 Dt.Rows[i]["ImgPath"] + "\"></a></div>";
                    }
                }
            }

            Session["PopupImg"] = Dt;

            RptPopup.DataSource = Dt;
            RptPopup.DataBind();
        }
        catch (Exception)
        {
        }
    }
    private void CreateBlankTable()
    {
        Session["ProductList"] = null;

        DataTable BlankDt = new DataTable();
        BlankDt.Columns.Add("Offerid");
        BlankDt.Columns.Add("MatchingBV");
        BlankDt.Columns.Add("DirectBV");
        BlankDt.Columns.Add("Reward");
        BlankDt.Columns.Add("Status");

        Session["ProductList"] = BlankDt;

        DataTable Dt = new DataTable();
        Dt.Columns.Add("Offerid");
        Dt.Columns.Add("MatchingBV");
        Dt.Columns.Add("DirectBV");
        Dt.Columns.Add("Reward");
        Dt.Columns.Add("Status");

        DataRow Dr = Dt.NewRow();
        Dr["Offerid"] = "0";
        Dr["MatchingBV"] = "0";
        Dr["DirectBV"] = "";
        Dr["Reward"] = "0";
        Dr["Status"] = "";
        Dt.Rows.Add(Dr);

        gvDirectBonanza.DataSource = Dt;
        gvDirectBonanza.DataBind();
    }
    private void Fill_WellValueBonanza()
    {
        try
        {
            Div20.Visible = true;

            DataSet ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Obj.IsoStart + "Exec sp_Bonanza " + Session["FormNo"] + Obj.IsoEnd);

            // Table 0 Binding
            gvbonanza.DataSource = ds.Tables[0];
            gvbonanza.DataBind();

            if (ds.Tables[0].Rows.Count > 0)
            {
                Bonzaname.Text = ds.Tables[0].Rows[0]["BONANZA"].ToString();
            }

            DataTable dt = new DataTable();

            // Table 1 Processing
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                CreateBlankTable();
                dt = (DataTable)Session["ProductList"];

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();

                    dr["Offerid"] = ds.Tables[1].Rows[i]["offerid"].ToString();
                    dr["MatchingBV"] = ds.Tables[1].Rows[i]["MatchingBV"].ToString();
                    dr["DirectBV"] = ds.Tables[1].Rows[i]["DirectBV"].ToString();
                    dr["Reward"] = ds.Tables[1].Rows[i]["Reward"].ToString();
                    dr["Status"] = ds.Tables[1].Rows[i]["Status"].ToString();

                    // Status override logic
                    if (i > 0)
                    {
                        if (ds.Tables[1].Rows[i - 1]["Status"].ToString() == "Achieve" &&
                            ds.Tables[1].Rows[i]["Status"].ToString() == "Achieve")
                        {
                            dt.Rows[i - 1]["Status"] = "--";
                        }
                    }

                    dt.Rows.Add(dr);
                }

                gvDirectBonanza.DataSource = dt;
                gvDirectBonanza.DataBind();

                LblBonanzaDirect.Text = ds.Tables[1].Rows[0]["BONANZA"].ToString();
            }
        }
        catch (Exception)
        {
        }
    }
    private void Fill_solfitBonanza()
    {
        try
        {
            Div20.Visible = true;

            DataSet ds = SqlHelper.ExecuteDataset(
                HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                CommandType.Text,
               Obj.IsoStart + "Exec sp_Bonanza " + Session["FormNo"] + Obj.IsoEnd
            );

            // Bind table 0
            gvbonanza.DataSource = ds.Tables[0];
            gvbonanza.DataBind();

            if (ds.Tables[0].Rows.Count > 0)
            {
                Bonzaname.Text = ds.Tables[0].Rows[0]["BONANZA"].ToString();
            }

            // NOTE: Table 1 logic was commented in VB, so kept same
        }
        catch (Exception)
        {
        }
    }
    public string CalculateTimeDifference(DateTime startDate, DateTime endDate)
    {
        int days = 0;
        int hours = 0;
        int mins = 0;
        int secs = 0;
        string final = string.Empty;

        if (endDate > startDate)
        {
            TimeSpan diff = endDate - startDate;

            days = diff.Days;
            hours = diff.Hours;
            mins = diff.Minutes;
            secs = diff.Seconds;

            final = string.Format("{0} days {1} hours {2} mins {3} secs", days, hours, mins, secs);
        }

        return final;
    }
    private void GetDashboardIncome()
    {
        string compDb = HttpContext.Current.Session["MlmDatabase" +
                          HttpContext.Current.Session["CompID"]].ToString();

        using (SqlConnection con = new SqlConnection(compDb))
        {
            con.Open();



            string checkSP = "";
            if (Session["compid"].ToString() == "1106")
            {
                checkSP = @"
IF NOT EXISTS (SELECT * FROM sys.objects 
               WHERE type = 'P' AND name = 'Sp_DashboardIncome_Total')
BEGIN
    EXEC('
CREATE PROC Sp_DashboardIncome_Total
(
    @formNO VARCHAR(20)
)
AS
BEGIN

DECLARE @sql NVARCHAR(MAX) = ''''
DECLARE @tableName VARCHAR(200)
DECLARE @columnName VARCHAR(200)

DECLARE cur CURSOR FOR
SELECT TableName, ColumnName
FROM M_PayOutColumnSetting
WHERE columnname = ''chqAmt''
ORDER BY Seq

OPEN cur
FETCH NEXT FROM cur INTO @tableName, @columnName

WHILE @@FETCH_STATUS = 0
BEGIN

    SET @sql = @sql + ''

    SELECT SUM(ISNULL('' + QUOTENAME(@columnName) + '',0)) amt
    FROM '' + QUOTENAME(@tableName) + '' a
    INNER JOIN M_MonthSessnMaster b ON a.sessid = b.sessid
    WHERE b.onwebsite = ''''Y'''' AND a.FormNo = '''''' + @formNO + ''''''

    UNION ALL ''

    FETCH NEXT FROM cur INTO @tableName, @columnName

END

CLOSE cur
DEALLOCATE cur

IF RIGHT(@sql,10) = ''UNION ALL ''
    SET @sql = LEFT(@sql, LEN(@sql) - 10)

SET @sql = ''

SELECT ISNULL(SUM(amt),0) TotalIncome
FROM
(
    '' + @sql + ''
) x''

EXEC(@sql)

END
')
END
";
            }
            else
            {
                checkSP = @"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Sp_DashboardIncome_Total')
            BEGIN
                EXEC('
                    CREATE PROC Sp_DashboardIncome_Total
                    (
                        @formNO VARCHAR(20)
                    )
                    AS
                    BEGIN
                        DECLARE @sql NVARCHAR(MAX) = N'''';
                        DECLARE @tableName VARCHAR(200);
                        DECLARE @columnName VARCHAR(200);

                        DECLARE cur CURSOR FOR
                            SELECT TableName, ColumnName
                            FROM M_PayOutColumnSetting 
                            WHERE IsDash = ''1''
                            ORDER BY Seq;

                        OPEN cur;
                        FETCH NEXT FROM cur INTO @tableName, @columnName;

                        WHILE @@FETCH_STATUS = 0
                        BEGIN
                            SET @sql = @sql + ''
                                SELECT SUM(ISNULL('' + QUOTENAME(@columnName) + '',0)) AS amt
                                FROM '' + QUOTENAME(@tableName) + ''
                                WHERE FormNo = '''''' + @formNO + ''''''
                                UNION ALL
                            '';

                            FETCH NEXT FROM cur INTO @tableName, @columnName;
                        END;

                        CLOSE cur;
                        DEALLOCATE cur;

                        -- REMOVE LAST UNION ALL
                        SET @sql = REVERSE(STUFF(
                            REVERSE(@sql),
                            1,
                            PATINDEX(''%LLA NOINU%'', REVERSE(@sql)) + 8,
                            ''''
                        ));

                        SET @sql = ''
                            SELECT ISNULL(SUM(amt),0) AS TotalIncome
                            FROM ('' + @sql + '') AS x
                        '';

                        EXEC(@sql);
                    END
                ')
            END";
            }

            SqlCommand cmdCheck = new SqlCommand(checkSP, con);
            cmdCheck.ExecuteNonQuery();

            // 2) EXECUTE THE PROCEDURE
            SqlCommand cmd = new SqlCommand("Sp_DashboardIncome_Total", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@formNO", Session["FormNo"]);

            object result = cmd.ExecuteScalar();

            con.Close();

            decimal income = 0;

            if (result != null && result != DBNull.Value)
                income = Convert.ToDecimal(result);
            LblMyIncome.Text = income.ToString("N2");
        }
    }
    public int GetTotalDirects()
    {
        int totalDirects = 0;
        int teamDirect = 0;

        string compDb = HttpContext.Current.Session["MlmDatabase" +
                         HttpContext.Current.Session["CompID"]].ToString();

        using (SqlConnection con = new SqlConnection(compDb))
        {
            con.Open();



            string createOrAlterProc = @"
            IF OBJECT_ID('Sp_TotalDirects', 'P') IS NULL
            BEGIN
                EXEC('
                    CREATE PROCEDURE Sp_TotalDirects
                    (
                        @FormNo INT
                    )
                    AS
                    BEGIN
                        SELECT 
                            SUM(MyDirects) AS MyDirects,
                            SUM(TeamDirect) AS TeamDirect
                        FROM (
                            SELECT COUNT(*) AS MyDirects, 0 AS TeamDirect
                            FROM R_Memtreerelation a WITH(NOLOCK)
                            INNER JOIN M_MemberMaster b WITH(NOLOCK) ON a.FormnoDwn = b.Formno
                            INNER JOIN m_KitMaster c WITH(NOLOCK) ON b.kitid = c.KitId
                            WHERE c.RowStatus = ''Y''
                              AND a.FormNo = @FormNo
                              AND a.Mlevel = 1

                            UNION ALL

                            SELECT 0 AS MyDirects, COUNT(*) AS TeamDirect
                            FROM R_Memtreerelation a WITH(NOLOCK)
                            INNER JOIN M_MemberMaster b WITH(NOLOCK) ON a.FormnoDwn = b.Formno
                            INNER JOIN m_KitMaster c WITH(NOLOCK) ON b.kitid = c.KitId
                            WHERE c.RowStatus = ''Y''
                              AND a.FormNo = @FormNo
                        ) T
                    END
                ')
            END
            ELSE
            BEGIN
                EXEC('
                    ALTER PROCEDURE Sp_TotalDirects
                    (
                        @FormNo INT
                    )
                    AS
                    BEGIN
                        SELECT 
                            SUM(MyDirects) AS MyDirects,
                            SUM(TeamDirect) AS TeamDirect
                        FROM (
                            SELECT COUNT(*) AS MyDirects, 0 AS TeamDirect
                            FROM R_Memtreerelation a WITH(NOLOCK)
                            INNER JOIN M_MemberMaster b WITH(NOLOCK) ON a.FormnoDwn = b.Formno
                            INNER JOIN m_KitMaster c WITH(NOLOCK) ON b.kitid = c.KitId
                            WHERE c.RowStatus = ''Y''
                              AND a.FormNo = @FormNo
                              AND a.Mlevel = 1

                            UNION ALL

                            SELECT 0 AS MyDirects, COUNT(*) AS TeamDirect
                            FROM R_Memtreerelation a WITH(NOLOCK)
                            INNER JOIN M_MemberMaster b WITH(NOLOCK) ON a.FormnoDwn = b.Formno
                            INNER JOIN m_KitMaster c WITH(NOLOCK) ON b.kitid = c.KitId
                            WHERE c.RowStatus = ''Y''
                              AND a.FormNo = @FormNo
                        ) T
                    END
                ')
            END";

            using (SqlCommand cmdProc = new SqlCommand(createOrAlterProc, con))
            {
                cmdProc.ExecuteNonQuery();
            }

            // 2️⃣ CALL PROCEDURE
            using (SqlCommand cmd = new SqlCommand("Sp_TotalDirects", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FormNo", Session["FormNo"]);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        totalDirects = dr["MyDirects"] != DBNull.Value
                            ? Convert.ToInt32(dr["MyDirects"])
                            : 0;

                        teamDirect = dr["TeamDirect"] != DBNull.Value
                            ? Convert.ToInt32(dr["TeamDirect"])
                            : 0;
                    }
                }
            }
        }

        LblMyDirects.Text = totalDirects.ToString();
        LblMyTeam.Text = teamDirect.ToString();

        return totalDirects;
    }
    private void LoadTeam()
    {
        try
        {
            DataSet Ds = new DataSet();

            trtotalplanbactive.Visible = false;
            trplanBCurrentactive.Visible = false;
            trdirectactive.Visible = true;
            trToalBv.Visible = true;
            trTotaldirect.Visible = true;
            trcWeek.Visible = true;

            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@FormNo", Session["FormNo"]);

            Ds = SqlHelper.ExecuteDataset(
                    HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                    "sp_LoadTeam",
                    prms);

            // Table 0
            if (Ds.Tables[0].Rows.Count > 0)
            {
                TodayLeftRegister.InnerText = Ds.Tables[0].Rows[0]["LeftTodayRegister"].ToString();
                TodayRightRegister.InnerText = Ds.Tables[0].Rows[0]["RightTodayRegister"].ToString();
                TotalTodayRegister.InnerText = (Convert.ToDecimal(Ds.Tables[0].Rows[0]["LeftTodayRegister"]) + Convert.ToDecimal(Ds.Tables[0].Rows[0]["RightTodayRegister"])).ToString();

                TodayLeftActive.InnerText = Ds.Tables[0].Rows[0]["LeftTodayActive"].ToString();
                TodayRightActive.InnerText = Ds.Tables[0].Rows[0]["RightTodayActive"].ToString();
                TodayTotalActive.InnerText =
                    (Convert.ToDecimal(Ds.Tables[0].Rows[0]["LeftTodayActive"]) +
                     Convert.ToDecimal(Ds.Tables[0].Rows[0]["RightTodayActive"])).ToString();

                TdTodayLeftUnit.InnerText = Ds.Tables[0].Rows[0]["TodayLeftUnit"].ToString();
                TdTodayRightUnit.InnerText = Ds.Tables[0].Rows[0]["TodayRightUnit"].ToString();
                TdTodayTotalUnit.InnerText =
                    (Convert.ToDecimal(Ds.Tables[0].Rows[0]["TodayLeftUnit"]) +
                     Convert.ToDecimal(Ds.Tables[0].Rows[0]["TodayRightUnit"])).ToString();

                TotalLeftJoin.InnerText = Ds.Tables[0].Rows[0]["LeftRegister"].ToString();
                TotalRightJoin.InnerText = Ds.Tables[0].Rows[0]["RightRegister"].ToString();
                TotalJoin.InnerText =
                    (Convert.ToDecimal(Ds.Tables[0].Rows[0]["LeftRegister"]) +
                     Convert.ToDecimal(Ds.Tables[0].Rows[0]["RightRegister"])).ToString();

                TotalLeftActivation.InnerText = Ds.Tables[0].Rows[0]["Leftactive"].ToString();
                TotalRightActivation.InnerText = Ds.Tables[0].Rows[0]["RightActive"].ToString();
                TotalActivation.InnerText =
                    (Convert.ToDecimal(Ds.Tables[0].Rows[0]["Leftactive"]) +
                     Convert.ToDecimal(Ds.Tables[0].Rows[0]["RightActive"])).ToString();

                DirectActive.InnerText = Ds.Tables[0].Rows[0]["Directactive"].ToString();
                IndirectActive.InnerText = Ds.Tables[0].Rows[0]["InDirectactive"].ToString();
                TotalDirectActive.InnerText =
                    (Convert.ToDecimal(Ds.Tables[0].Rows[0]["Directactive"]) +
                     Convert.ToDecimal(Ds.Tables[0].Rows[0]["InDirectactive"])).ToString();

                TotalLeftUnit.InnerText = Ds.Tables[0].Rows[0]["DirectUnit"].ToString();
                TotalRightUnit.InnerText = Ds.Tables[0].Rows[0]["InDirectUnit"].ToString();
                TotalUnit.InnerText =
                    (Convert.ToDecimal(Ds.Tables[0].Rows[0]["DirectUnit"]) +
                     Convert.ToDecimal(Ds.Tables[0].Rows[0]["InDirectUnit"])).ToString();

                LblTotalBusiness.Text = (Convert.ToDecimal(Ds.Tables[0].Rows[0]["DirectUnit"]) +
                     Convert.ToDecimal(Ds.Tables[0].Rows[0]["InDirectUnit"])).ToString();

                Directunit.InnerText = Ds.Tables[0].Rows[0]["TotalDirectUnit"].ToString();
                indirectunit.InnerText = Ds.Tables[0].Rows[0]["TotalInDirectUnit"].ToString();
                totalDirectunit.InnerText =
                    (Convert.ToDecimal(Ds.Tables[0].Rows[0]["TotalDirectUnit"]) +
                     Convert.ToDecimal(Ds.Tables[0].Rows[0]["TotalInDirectUnit"])).ToString();

                TdLeftCMonth.InnerText = Ds.Tables[0].Rows[0]["CLeftCount"].ToString();
                TdRightCMonth.InnerText = Ds.Tables[0].Rows[0]["CRightCount"].ToString();
                TdTotalCMonth.InnerText =
                    (Convert.ToDecimal(Ds.Tables[0].Rows[0]["CLeftCount"]) +
                     Convert.ToDecimal(Ds.Tables[0].Rows[0]["CRightCount"])).ToString();

                // Company-wise additional fields
                if (Session["Compid"].ToString() == "1074")
                {
                    LeftRoyaltyBV.InnerText = Ds.Tables[0].Rows[0]["RLeft"].ToString();
                    RightRoyaltyBV.InnerText = Ds.Tables[0].Rows[0]["RRight"].ToString();
                    TotalRoyaltyBV.InnerText =
                        (Convert.ToDecimal(Ds.Tables[0].Rows[0]["RLeft"]) +
                         Convert.ToDecimal(Ds.Tables[0].Rows[0]["RRight"])).ToString();

                    LeftRepurchinBV.InnerText = Ds.Tables[0].Rows[0]["TotalBVDirect"].ToString();
                    RightRepurchinBV.InnerText = Ds.Tables[0].Rows[0]["TotalBVInDirect"].ToString();
                    TotalRepurchinBV.InnerText =
                        (Convert.ToDecimal(Ds.Tables[0].Rows[0]["TotalBVDirect"]) +
                         Convert.ToDecimal(Ds.Tables[0].Rows[0]["TotalBVInDirect"])).ToString();
                }

                if (Session["Compid"].ToString() == "1010")
                {
                    LeftRoyaltyBV.InnerText = Ds.Tables[0].Rows[0]["LeftRoyaltyBV"].ToString();
                    RightRoyaltyBV.InnerText = Ds.Tables[0].Rows[0]["RightRoyaltyBV"].ToString();
                    TotalRoyaltyBV.InnerText =
                        (Convert.ToDecimal(Ds.Tables[0].Rows[0]["LeftRoyaltyBV"]) +
                         Convert.ToDecimal(Ds.Tables[0].Rows[0]["RightRoyaltyBV"])).ToString();

                    LeftRepurchinBV.InnerText = Ds.Tables[0].Rows[0]["LeftRepurBV"].ToString();
                    RightRepurchinBV.InnerText = Ds.Tables[0].Rows[0]["RightRepurBV"].ToString();
                    TotalRepurchinBV.InnerText =
                        (Convert.ToDecimal(Ds.Tables[0].Rows[0]["LeftRepurBV"]) +
                         Convert.ToDecimal(Ds.Tables[0].Rows[0]["RightRepurBV"])).ToString();
                }

                if (Session["CompID"].ToString() == "1007")
                {
                    trRepurcurrSessPVBV.Visible = true;
                    TdTodayLeftUnitR.InnerText = Ds.Tables[0].Rows[0]["CLeftCoin"].ToString();
                    TdTodayRightUnitR.InnerText = Ds.Tables[0].Rows[0]["CRightCoin"].ToString();
                    TdTodayTotalUnitR.InnerText =
                        (Convert.ToDecimal(Ds.Tables[0].Rows[0]["CLeftCoin"]) +
                         Convert.ToDecimal(Ds.Tables[0].Rows[0]["CRightCoin"])).ToString();
                }
                else
                {
                    trRepurcurrSessPVBV.Visible = false;
                }

                if (Session["CompID"].ToString() == "1010" ||
                    Session["CompID"].ToString() == "1007" ||
                    Session["CompID"].ToString() == "1074")
                {
                    trcurrSessPVBV.Visible = true;
                    trCFBVPV.Visible = true;

                    CLeftCoin.InnerText = Ds.Tables[0].Rows[0]["CLeftCoin"].ToString();
                    CRightCoin.InnerText = Ds.Tables[0].Rows[0]["CRightCoin"].ToString();
                    CTotalCoin.InnerText =
                        (Convert.ToDecimal(Ds.Tables[0].Rows[0]["CLeftCoin"]) +
                         Convert.ToDecimal(Ds.Tables[0].Rows[0]["CRightCoin"])).ToString();

                    LegXBvCF.InnerText = Ds.Tables[0].Rows[0]["LegXBvCF"].ToString();
                    LegYBvCF.InnerText = Ds.Tables[0].Rows[0]["LegYBvCF"].ToString();
                    TotalBVCF.InnerText =
                        (Convert.ToDecimal(Ds.Tables[0].Rows[0]["LegXBvCF"]) +
                         Convert.ToDecimal(Ds.Tables[0].Rows[0]["LegYBvCF"])).ToString();

                    if (Session["CompID"].ToString() == "1007" ||
                        Session["CompID"].ToString() == "1074")
                    {
                        trcurrSessPVBV.Visible = false;
                    }
                }
                else
                {
                    trcurrSessPVBV.Visible = false;
                    trCFBVPV.Visible = false;
                }

                if (Session["CompID"].ToString() == "1033")
                {
                    trtodaysb.Visible = true;
                    trtotalsb.Visible = true;
                    trdirectsb.Visible = true;

                    Tdtotaldirectsb.InnerText =
                        (Convert.ToDecimal(Ds.Tables[0].Rows[0]["Totaldirectsb"]) +
                         Convert.ToDecimal(Ds.Tables[0].Rows[0]["Totalindirectsb"])).ToString();

                    TdLeftdirectsb.InnerText = Ds.Tables[0].Rows[0]["Totaldirectsb"].ToString();
                    Tdrightdirectsb.InnerText = Ds.Tables[0].Rows[0]["Totalindirectsb"].ToString();

                    todayleftsb.InnerText = Ds.Tables[0].Rows[0]["todayleftsb"].ToString();
                    todayrightsb.InnerText = Ds.Tables[0].Rows[0]["todayrightsb"].ToString();
                    todaytotalsb.InnerText =
                        (Convert.ToDecimal(Ds.Tables[0].Rows[0]["todayleftsb"]) +
                         Convert.ToDecimal(Ds.Tables[0].Rows[0]["todayrightsb"])).ToString();

                    Totalleftsb.InnerText = Ds.Tables[0].Rows[0]["totalleftsb"].ToString();
                    totalrightsb.InnerText = Ds.Tables[0].Rows[0]["totalrightsb"].ToString();
                    totalsb.InnerText =
                        (Convert.ToDecimal(Ds.Tables[0].Rows[0]["totalleftsb"]) +
                         Convert.ToDecimal(Ds.Tables[0].Rows[0]["totalrightsb"])).ToString();
                }
                else
                {
                    trtodaysb.Visible = false;
                    trtotalsb.Visible = false;
                    trdirectsb.Visible = false;
                }
            }

            // Table 1 (Profile)
            if (Ds.Tables[1].Rows.Count > 0)
            {
                Image2.ImageUrl = Ds.Tables[1].Rows[0]["ProfilePic"].ToString();
                LblMemId.Text = Ds.Tables[1].Rows[0]["Idno"].ToString();
                LblMemName.Text = Ds.Tables[1].Rows[0]["Name"].ToString();
            }

            // Company 1010 special popup
            if (Session["CompID"].ToString() == "1010")
            {
                if (Ds.Tables[6].Rows.Count > 0)
                {
                    for (int i = 0; i < Ds.Tables[6].Rows.Count; i++)
                    {
                        if (i == 0)
                            Ds.Tables[6].Rows[i]["divstart"] =
                                "<div><a class=\"fbox\" id=\"First\" href=\"" +
                                Ds.Tables[6].Rows[i]["ImgPath"] + "\"></a></div>";

                        if (i == 1)
                            Ds.Tables[6].Rows[i]["divstart"] =
                                "<div><a class=\"fbox\" id=\"Second\" href=\"" +
                                Ds.Tables[6].Rows[i]["ImgPath"] + "\"></a></div>";
                    }

                    Session["PopupImg"] = Ds.Tables[6];
                }

                if (Ds.Tables[2].Rows.Count > 0)
                {
                    gvBalance.DataSource = Ds.Tables[2];
                    gvBalance.DataBind();
                }
            }

            // Company based binding of news & income
            if (Session["CompID"].ToString() == "1007" ||
                Session["CompID"].ToString() == "1074" ||
                Session["CompID"].ToString() == "1091" ||
                Session["CompID"].ToString() == "1095" ||
                Session["CompID"].ToString() == "1102" ||
                Session["CompID"].ToString() == "1097" ||
                Session["CompID"].ToString() == "1105" ||
                Session["CompID"].ToString() == "1106" ||
                Session["CompID"].ToString() == "1107" ||
                Session["CompID"].ToString() == "1108" || 
                Session["CompID"].ToString() == "1109" ||
                Session["CompID"].ToString() == "1110" ||
                Session["CompID"].ToString() == "1100")
            {
                if (Ds.Tables[6].Rows.Count > 0)
                {
                    lblRankTitle.Text = Ds.Tables[6].Rows[0]["Rank"].ToString();
                }

                if (Ds.Tables[4].Rows.Count > 0)
                {
                    RptNews.DataSource = Ds.Tables[4];
                    RptNews.DataBind();
                }

                if (Ds.Tables[5].Rows.Count > 0)
                {
                    repMyIncome.DataSource = Ds.Tables[5];
                    repMyIncome.DataBind();
                }
            }
            else
            {
                if (Ds.Tables[4].Rows.Count > 0)
                {
                    repMyIncome.DataSource = Ds.Tables[4];
                    repMyIncome.DataBind();
                }

                if (Ds.Tables[5].Rows.Count > 0)
                {
                    lblRankTitle.Text = Ds.Tables[5].Rows[0]["Rank"].ToString();
                }

                if (Ds.Tables[3].Rows.Count > 0)
                {
                    RptNews.DataSource = Ds.Tables[3];
                    RptNews.DataBind();
                }
            }

            // KYC status for compid 1091
            if (Session["CompID"].ToString() == "1091")
            {
                dvVPRequest.Visible = false;
                DivWalletSummary.Visible = false;
                Div15.Visible = false;

                SqlParameter[] prmss = new SqlParameter[1];
                prmss[0] = new SqlParameter("@FormNo", Session["FormNo"]);

                Ds = SqlHelper.ExecuteDataset(
                        HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(),
                        "Sp_GetKYCApproveStatus",
                        prmss);

                if (Ds.Tables[0].Rows.Count > 0)
                {
                    kycstatus.Visible = true;
                    status.InnerText = Ds.Tables[0].Rows[0]["Status"].ToString();
                }
            }
            else
            {
                
                if (Session["CompID"].ToString() == "1106")
                {
                    DivWalletSummary.Visible = false;
                    dvVPRequest.Visible = false;
                    Div15.Visible = false;
                }
                else
                {
                    dvVPRequest.Visible = true;
                    DivWalletSummary.Visible = true;
                    Div15.Visible = true;
                }
                
            }

            Div3.Visible = false;

            if (Session["CompID"].ToString() == "1033")
            {
                Fill_RBV();
            }

            Divsingle.Visible = false;

            if (Session["CompID"].ToString() == "1033")
            {
                if (Ds.Tables[8].Rows.Count > 0)
                {
                    divsurkshafund.Visible = true;
                    lblstatus.Text = Ds.Tables[8].Rows[0]["Status"].ToString();
                }
                else
                {
                    divsurkshafund.Visible = false;
                }
            }
            else
            {
                divsurkshafund.Visible = false;
            }

            if (Session["CompID"].ToString() == "1033")
            {
                if (Ds.Tables[9].Rows.Count > 0)
                {
                    divreentry.Visible = true;
                    lblreentry.Text = "<a href=\"GlobalRentry.aspx\">" +
                                       Ds.Tables[9].Rows[0]["Mformno"] + "</a>";
                }
                else
                {
                    divreentry.Visible = false;
                }
            }
            else
            {
                divreentry.Visible = false;
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void Fill_Balance()
    {
        try
        {
            DataSet ds = SqlHelper.ExecuteDataset(
                HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                CommandType.Text,
                "Exec Sp_GetBalanceDashBord_New " + Session["FormNo"]
            );

            gvBalance.DataSource = ds.Tables[0];
            gvBalance.DataBind();
        }
        catch (Exception)
        {
        }
    }
    private void Fill_CashlessPopup()
    {
        //try
        //{
        //   // divVedicindia.Visible = false;

        //    DataSet ds = SqlHelper.ExecuteDataset(
        //        HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(),
        //        CommandType.Text,
        //        "Exec Sp_Showpop " + Session["FormNo"]
        //    );

        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        if (ds.Tables[0].Rows[0]["Msg"].ToString() != "")
        //        {
        //            LblBlockStatus.Text = ds.Tables[0].Rows[0]["Msg"].ToString();
        //            lblHeading.Text = ds.Tables[0].Rows[0]["Heading"].ToString();
        //            divVedicindia.Visible = true;
        //        }
        //        else
        //        {
        //            divVedicindia.Visible = false;
        //        }
        //    }
        //    else
        //    {
        //        divVedicindia.Visible = false;
        //    }
        //}
        //catch (Exception)
        //{
        //}
    }
    private void Fill_RBV()
    {
        try
        {
            DataSet Ds = new DataSet();

            double CurrentMonLeftRBV, CurrentMonRightRbv;
            double PerviousMonLeftRBV, PerviousMonRightRbv;
            double AmmLeftRBV, AmRightRbv;

            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@FormNo", Session["FormNo"]);

            Ds = SqlHelper.ExecuteDataset(
                    HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(),
                    "Proc_MyBVBusiness",
                    prms);

            // -------- Current Month RBV --------
            if (Ds.Tables[0].Rows.Count > 0)
            {
                CurrentMonLeftRBV = Convert.ToDouble(Ds.Tables[0].Rows[0]["CWBVL"]);
                CurrentMonRightRbv = Convert.ToDouble(Ds.Tables[0].Rows[0]["CWBVR"]);
            }
            else
            {
                CurrentMonLeftRBV = 0;
                CurrentMonRightRbv = 0;
            }

            // -------- Previous Month RBV --------
            if (Ds.Tables[1].Rows.Count > 0)
            {
                PerviousMonLeftRBV = Convert.ToDouble(Ds.Tables[1].Rows[0]["PWBVL"]);
                PerviousMonRightRbv = Convert.ToDouble(Ds.Tables[1].Rows[0]["PWBVR"]);
            }
            else
            {
                PerviousMonLeftRBV = 0;
                PerviousMonRightRbv = 0;
            }

            // Totals
            AmmLeftRBV = CurrentMonLeftRBV + PerviousMonLeftRBV;
            AmRightRbv = CurrentMonRightRbv + PerviousMonRightRbv;

            // -------- Assign Values to Labels --------
            CurrentMonthLeftRBV.InnerText = CurrentMonLeftRBV.ToString();
            CurrentMonthRightRbv.InnerText = CurrentMonRightRbv.ToString();
            TotalCurrentMonthRbv.InnerText =
                (CurrentMonLeftRBV + CurrentMonRightRbv).ToString();

            PerviousMonthLeftRBV.InnerText = PerviousMonLeftRBV.ToString();
            PerviousMonthRightRbv.InnerText = PerviousMonRightRbv.ToString();
            TotalPerviousMonthRbv.InnerText =
                (PerviousMonLeftRBV + PerviousMonRightRbv).ToString();

            TAmmLeftRBV.InnerText = AmmLeftRBV.ToString();
            TAmRightRbv.InnerText = AmRightRbv.ToString();
            TotalAmmRbv.InnerText =
                (AmmLeftRBV + AmRightRbv).ToString();

            divRbv.Visible = true;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void FillPPVStatus()
    {
        try
        {
            DataSet Ds = new DataSet();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@FormNo", Session["FormNo"]);
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), "Sp_GetCombinedWholesalePV", prms);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                LblCurrentMonthPPV.Text = Ds.Tables[0].Rows[0]["CurrentMonthPV"].ToString();
                LblLastMonthPPV.Text = Ds.Tables[0].Rows[0]["LastMonthPV"].ToString();
                LblTotalMonthPPV.Text = Ds.Tables[0].Rows[0]["TotalPV"].ToString();
            }
            //-------------------------
            if (Ds.Tables[1].Rows.Count > 0)
            {
                LblCurrentMonthWBV.Text = Ds.Tables[1].Rows[0]["TotalDownlineCurrentMonthPV"].ToString();
                LblLastMonthWBV.Text = Ds.Tables[1].Rows[0]["TotalDownlineLastMonthPV"].ToString();
                LblTotalMonthWBV.Text = Ds.Tables[1].Rows[0]["TotalDownlinePV"].ToString();
            }
            if (Ds.Tables[2].Rows.Count > 0)
            {
                //-------------------------------------------------------------
                LblCurrentMonthRBV.Text = Ds.Tables[2].Rows[0]["TotalCurrentMonthPV"].ToString();
                LblLastMonthRBV.Text = Ds.Tables[2].Rows[0]["TotalLastMonthPV"].ToString();
                LblTotalMonthRBV.Text = Ds.Tables[2].Rows[0]["GrandTotalPV"].ToString();
            }
            if (Ds.Tables[3].Rows.Count > 0)
            {

                //----------------------------------------------------------------------------------
                LblCurrentMonthPBV.Text = Ds.Tables[3].Rows[0]["TotalCurrentMonthPV"].ToString();
                LblLastMonthPBV.Text = Ds.Tables[3].Rows[0]["TotalLastMonthPV"].ToString();
                LblTotalMonthPBV.Text = Ds.Tables[3].Rows[0]["GrandTotalPV"].ToString();
            }
            if (Ds.Tables[4].Rows.Count > 0)
            {
                //----------------------------------------------------------------------------------
                LblCuunerentPPVWBV.Text = Ds.Tables[4].Rows[0]["TotalCurrentMonthPV"].ToString();
                LblLastPPVWBV.Text = Ds.Tables[4].Rows[0]["TotalLastMonthPV"].ToString();
                LblTotalPPVWBV.Text = Ds.Tables[4].Rows[0]["GrandTotalPV"].ToString();
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void FillKYCStatus()
    {
        try
        {
            DataSet Ds = new DataSet();

            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@FormNo", Session["FormNo"]);
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), "Sp_GetKYCStatus", prms);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                var top10 = Ds.Tables[0].AsEnumerable().Take(10).CopyToDataTable();
                RptKYCStatus.DataSource = top10;
                RptKYCStatus.DataBind();
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void FilldirectDirectBusinessReward()
    {
        try
        {
            DataSet Ds = new DataSet();

            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@FormNo", Session["FormNo"]);
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), "Sp_MyDirectBusinessReward", prms);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                var top10 = Ds.Tables[0].AsEnumerable().Take(10).CopyToDataTable();
                RptDirectBusinessReward.DataSource = top10;
                RptDirectBusinessReward.DataBind();
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void Filldirect()
    {
        try
        {
            DataSet Ds = new DataSet();

            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@FormNo", Session["FormNo"]);
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), "Sp_MyDirect", prms);
            if (Ds.Tables[0].Rows.Count > 0)
            {
                var top10 = Ds.Tables[0].AsEnumerable().Take(10).CopyToDataTable();
                RptDirects.DataSource = top10;
                RptDirects.DataBind();
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            string compid = Session["CompID"].ToString();
            string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);

            // Folder path
            string folderPath = Server.MapPath("~/images/UploadImage/" + compid + "/ProfilePic/");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string savePath = folderPath + fileName;

            // Save file in folder
            FileUpload1.SaveAs(savePath);

            // Path to store in DB
            string dbPath = "/images/UploadImage/" + compid + "/ProfilePic/" + fileName;

            using (SqlConnection con = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + compid].ToString()))
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE m_membermaster SET ProfilePic=@img WHERE formno=@uid", con);

                cmd.Parameters.AddWithValue("@img", dbPath);
                cmd.Parameters.AddWithValue("@uid", Session["formno"]);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            // Update previews
            Image2.ImageUrl = dbPath;
            photoPreview.Src = dbPath;
        }
    }
    private void LoadProfileImage()
    {
        string compid = Session["CompID"].ToString();

        using (SqlConnection con = new SqlConnection(HttpContext.Current.Session["MlmSelectDatabase" + compid].ToString()))
        {
            SqlCommand cmd = new SqlCommand(Obj.IsoStart + "SELECT case when ProfilePic = '' then 'https://www.iconpacks.net/icons/2/free-user-icon-3296-thumb.png' else mlmurl + ProfilePic end as ProfilePic " +
                "FROM " + Obj.dBName + "..m_companymaster as a," + Obj.dBName + "..m_membermaster WHERE formno= '" + Session["formno"] + "'" + Obj.IsoEnd, con);
            con.Open();
            object result = cmd.ExecuteScalar();
            con.Close();

            string imgPath = result != null ? result.ToString() : "";

            if (!string.IsNullOrEmpty(imgPath))
            {
                Image2.ImageUrl = imgPath;
                photoPreview.Src = imgPath;
            }
            else
            {
                string defaultImg = "https://www.iconpacks.net/icons/2/free-user-icon-3296-thumb.png";
                Image2.ImageUrl = defaultImg;
                photoPreview.Src = defaultImg;
            }
        }
    }
}
public class Referalres
{
    public string response { get; set; }
    public string urlLeft { get; set; }
    public string urlright { get; set; }
}
public class Referalreq
{
    public string islogin { get; set; }
    public string reqtype { get; set; }
    public string userid { get; set; }
    public string passwd { get; set; }
}