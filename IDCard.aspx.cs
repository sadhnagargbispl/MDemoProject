using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;
using System.Text;

public partial class IDCard : System.Web.UI.Page
{

    string Sql;

    protected void Page_Load(object sender, EventArgs e)
    {


        if (Session["Status"] != null && Session["Status"].ToString() == "OK")
        {
            if (!IsPostBack)
            {
                BtnBack.Attributes.Add("onclick", DisableTheButton(this.Page, BtnBack));
                GetCompID();
                LoadCard();
            }
        }
        else
        {
            Response.Redirect("logout.aspx");
        }


    }
    private string DisableTheButton(Control pge, Control btn)
    {
        var sb = new StringBuilder();
        sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
        sb.Append("this.value = 'Please wait...';");
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
                HttpContext.Current.Session["CompID"] = dRead["ID"].ToString();
                HttpContext.Current.Session["Logo"] = dRead["Logo"].ToString();
                HttpContext.Current.Session["WRPartyCode"] = dRead["PartyCode"].ToString();
                HttpContext.Current.Session["CompName"] = dRead["Name"].ToString();
                HttpContext.Current.Session["Title"] = "Welcome To " + dRead["Name"].ToString();
                HttpContext.Current.Session["gvPortalCompID"] = dRead["gvPortalCompID"].ToString();
                HttpContext.Current.Session["UtiLityPortalID"] = dRead["UtiLityPortalID"].ToString();
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


    void LoadCard()
    {

        SqlConnection con = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());

        SqlCommand cmd = new SqlCommand();

        cmd.Connection = con;

        cmd.CommandType = CommandType.Text;

        cmd.CommandText = @"

DECLARE @TotalPV NUMERIC(18,2)=0

SELECT @TotalPV=ISNULL(SUM(PVValue),0)
FROM hemalika..trnorder a
LEFT JOIN hemalika..Repurchincome b
ON a.formno=b.formno
AND a.billno=b.RID
WHERE a.formno=@formno


DECLARE @Rank VARCHAR(100)


IF EXISTS
(
SELECT 1
FROM hemalika..Mstrankachievers
WHERE formno=@formno
)

SELECT @Rank=
(
SELECT Rank
FROM hemalika..mstranks
WHERE rankid=
(
SELECT ISNULL(MAX(rankid),0)
FROM hemalika..Mstrankachievers
WHERE formno=@formno
)
)

ELSE

SELECT TOP 1 @Rank=RankName
FROM hemalika..WholeIncomeMaster
WHERE @TotalPV BETWEEN StartRange AND EndRange


SELECT

IdNo,

(MemFirstName+' '+MemLastName) AS MemName,

Mobl AS MobileNo,

REPLACE(CONVERT(VARCHAR,doj,106),' ','-') AS MemDOB,

CASE
WHEN ISNULL(ProfilePic,'')=''
THEN '" + Session["CompWeb"] + @"Images/No_Photo.Jpg'
ELSE ProfilePic
END AS ProfilePic,

ISNULL(Address1,'')+', '+ISNULL(Address2,'') AS Address,

REPLACE(CONVERT(VARCHAR,UpgradeDate,106),' ','-') AS UpgradeDate,

@Rank AS Rank

FROM m_MemberMaster

WHERE formno=@formno
AND ActiveStatus='Y'

";

        cmd.Parameters.AddWithValue("@formno", Session["Formno"]);

        SqlDataAdapter da = new SqlDataAdapter(cmd);

        DataTable dt = new DataTable();

        da.Fill(dt);

        RptIdCard.DataSource = dt;

        RptIdCard.DataBind();

    }

    protected void BtnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("IndexT.aspx");
    }
}