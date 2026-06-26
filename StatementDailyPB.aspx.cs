using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class StatementDailyPB : System.Web.UI.Page
{
    clsGeneral objGen = new clsGeneral();
    DAL ObjDAL;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                GetCompID();
                getData();
                objGen.GetConnectionByComp();
                objGen.GetInvDataBaseByComp();

                imglogo.Src = Session["Logo"] + "";

                string strQuery = "";
                ObjDAL = new DAL();

                if (Request.QueryString["Formno"] != null)
                {
                    strQuery = ObjDAL.IsoStart + " Exec Sp_StateMentDailyPB " + Convert.ToInt32(Request.QueryString["Formno"]) +
                               ",'" + Convert.ToInt32(Request["PayoutNo"]) + "'" + ObjDAL.IsoEnd;
                }
                else if (Session["Formno"] != null)
                {
                    strQuery = ObjDAL.IsoStart + " Exec Sp_StateMentDailyPB " + Convert.ToInt32(Session["Formno"]) +
                               ",'" + Convert.ToInt32(Request["PayoutNo"]) + "'" + ObjDAL.IsoEnd;
                }
                else
                {
                    Response.Redirect("logout.aspx");
                }

                DataSet ds = SqlHelper.ExecuteDataset(
                    HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                    CommandType.Text,
                    strQuery);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    MemName.InnerText = ds.Tables[0].Rows[0]["Name"].ToString();
                    IDNO.InnerText = ds.Tables[0].Rows[0]["Idno"].ToString();
                    Add.InnerText = ds.Tables[0].Rows[0]["Address1"].ToString();
                    City.InnerText = ds.Tables[0].Rows[0]["City"].ToString();
                    District.InnerText = ds.Tables[0].Rows[0]["District"].ToString();
                    Mobile.InnerText = ds.Tables[0].Rows[0]["Mobl"].ToString();
                    PinCode.InnerText = ds.Tables[0].Rows[0]["PinCode"].ToString();
                    State.InnerText = ds.Tables[0].Rows[0]["StateName"].ToString();

                    PayoutTime.InnerText = ds.Tables[1].Rows[0]["SessID"].ToString();
                    Period.InnerText = ds.Tables[1].Rows[0]["PayoutDate"].ToString();

                    // Hide totals for specific companies
                    if (Session["compid"].ToString() == "1090" ||
                        Session["compid"].ToString() == "1091" ||
                        Session["compid"].ToString() == "1095" ||
                        Session["CompID"].ToString() == "1097" ||
                        Session["compid"].ToString() == "1100")
                    {
                        DivTotaldis.Visible = false;
                        DivTotalEar.Visible = false;
                    }
                    else
                    {
                        DivTotaldis.Visible = true;
                        DivTotalEar.Visible = true;
                    }

                    // Earnings Block
                    string comp = Session["compid"].ToString();

                    if (comp == "1090")
                    {
                        DivGeMartIncomeElse.Visible = false;
                        DivGeMartIncome.Visible = true;

                        LblMatchingIncome.Text = ds.Tables[2].Rows[0]["Amount"].ToString();
                        LblDirectSponsorIncome.Text = ds.Tables[2].Rows[1]["Amount"].ToString();
                        LblSponsorMatchingIncome.Text = ds.Tables[2].Rows[2]["Amount"].ToString();
                        LblRewardIncome.Text = ds.Tables[2].Rows[5]["Amount"].ToString();
                        LblRoyaltyIncome.Text = ds.Tables[2].Rows[6]["Amount"].ToString();
                        LblLeadershipDevBonus.Text = ds.Tables[2].Rows[7]["Amount"].ToString();
                        LblTotalEarnings.Text = ds.Tables[2].Rows[3]["Amount"].ToString();

                        if (comp == "1090" || comp == "1091")
                            NetPayble.InnerText = ds.Tables[2].Rows[4]["Amount"].ToString();
                    }
                    else if (comp == "1091")
                    {
                        DivGeMartIncomeElse.Visible = false;
                        DivGeMartIncome.Visible = true;

                        LblMatchingIncome.Text = ds.Tables[2].Rows[0]["Amount"].ToString();
                        LblDirectSponsorIncome.Text = ds.Tables[2].Rows[1]["Amount"].ToString();
                        LblSponsorMatchingIncome.Visible = false;
                        LblRewardIncome.Text = ds.Tables[2].Rows[5]["Amount"].ToString();
                        LblRoyaltyIncome.Text = ds.Tables[2].Rows[9]["Amount"].ToString();
                        LblTotalEarnings.Text = ds.Tables[2].Rows[3]["Amount"].ToString();

                        if (comp == "1090" || comp == "1091")
                            NetPayble.InnerText = ds.Tables[2].Rows[4]["Amount"].ToString();
                    }
                    else if (comp == "1095" ||
                             Session["CompID"].ToString() == "1097" ||
                             comp == "1100")
                    {
                        DivGeMartIncomeElse.Visible = false;
                        DivGeMartIncome.Visible = true;

                        LblMatchingIncome.Text = ds.Tables[2].Rows[0]["Amount"].ToString();
                        LblDirectSponsorIncome.Text = ds.Tables[2].Rows[1]["Amount"].ToString();
                        LblSponsorMatchingIncome.Text = ds.Tables[2].Rows[2]["Amount"].ToString();
                        lblselfincome.Text = ds.Tables[2].Rows[3]["Amount"].ToString();
                        LblRoyaltyIncome.Text = ds.Tables[2].Rows[4]["Amount"].ToString();
                        LblTotalEarnings.Text = ds.Tables[2].Rows[5]["Amount"].ToString();
                    }
                    else if (comp == "1074")
                    {
                        DivGeMartIncomeElse.Visible = false;
                        DivGeMartIncome.Visible = true;

                        LblDirectSponsorIncome.Text = ds.Tables[2].Rows[1]["Amount"].ToString();
                        LblSponsorMatchingIncome.Text = ds.Tables[2].Rows[2]["Amount"].ToString();
                        LblMatchingIncome.Text = ds.Tables[2].Rows[0]["Amount"].ToString();
                    }
                    else
                    {
                        DivGeMartIncomeElse.Visible = true;
                        DivGeMartIncome.Visible = false;

                        gvincome.DataSource = ds.Tables[2];
                        gvincome.DataBind();
                    }

                    // Deduction Section
                    if (comp == "1090" ||
                        comp == "1091" ||
                        comp == "1095" ||
                        Session["CompID"].ToString() == "1097" ||
                        comp == "1100")
                    {
                        DivGeMartElse.Visible = false;
                        DivGeMart.Visible = true;

                        LblTDSAmount.Text = ds.Tables[3].Rows[0]["Amount"].ToString();
                        LblAdminCharge.Text = ds.Tables[3].Rows[1]["Amount"].ToString();
                        LblRepurchaseDeduction.Text = ds.Tables[3].Rows[3]["Amount"].ToString();

                        decimal totalDed =
                            Convert.ToDecimal(ds.Tables[3].Rows[0]["Amount"]) +
                            Convert.ToDecimal(ds.Tables[3].Rows[1]["Amount"]) +
                            Convert.ToDecimal(ds.Tables[3].Rows[3]["Amount"]);

                        LblTotalDeducation.Text = totalDed.ToString("0.00");
                    }
                    else
                    {
                        DivGeMartElse.Visible = true;
                        DivGeMart.Visible = false;

                        gvDeduction.DataSource = ds.Tables[3];
                        gvDeduction.DataBind();
                    }

                    // Totals
                    if (Session["CompID"].ToString() == "1074")
                    {
                        TotalEarnings.InnerText = ds.Tables[2].Rows[3]["Amount"].ToString();
                        TotalDeductions.InnerText = ds.Tables[4].Rows[0]["TotalDeduction"].ToString();
                    }
                    else
                    {
                        TotalEarnings.InnerText = ds.Tables[4].Rows[0]["TotalEarning"].ToString();
                        TotalDeductions.InnerText = ds.Tables[4].Rows[0]["TotalDeduction"].ToString();
                    }

                    // Net Payable
                    if (comp == "1090" || comp == "1091")
                    {
                        // Already handled above
                    }
                    else if (comp == "1074")
                    {
                        NetPayble.InnerText = ds.Tables[2].Rows[4]["Amount"].ToString();
                    }
                    else if (comp == "1095" ||
                             Session["CompID"].ToString() == "1097" ||
                             comp == "1100")
                    {
                        decimal totalCredit =
                            Convert.ToDecimal(ds.Tables[2].Rows[0]["Amount"]) +
                            Convert.ToDecimal(ds.Tables[2].Rows[1]["Amount"]) +
                            Convert.ToDecimal(ds.Tables[2].Rows[2]["Amount"]) +
                            Convert.ToDecimal(ds.Tables[2].Rows[3]["Amount"]) +
                            Convert.ToDecimal(ds.Tables[2].Rows[4]["Amount"]);

                        decimal totalDebit =
                            Convert.ToDecimal(ds.Tables[3].Rows[0]["Amount"]) +
                            Convert.ToDecimal(ds.Tables[3].Rows[1]["Amount"]) +
                            Convert.ToDecimal(ds.Tables[3].Rows[3]["Amount"]);

                        decimal netPay = totalCredit - totalDebit;

                        NetPayble.InnerText = netPay.ToString("0.00");
                    }
                    else
                    {
                        NetPayble.InnerText = ds.Tables[4].Rows[0]["NetPay"].ToString();
                    }

                    // Business Details
                    if (ds.Tables[5].Rows.Count > 0)
                    {
                        BfXBV.InnerText = ds.Tables[5].Rows[0]["LegXBvBF"].ToString();
                        BfYBV.InnerText = ds.Tables[5].Rows[0]["LegYBvBF"].ToString();

                        NewXBV.InnerText = ds.Tables[5].Rows[0]["LegXBv"].ToString();
                        NewYBV.InnerText = ds.Tables[5].Rows[0]["LegYBv"].ToString();

                        MatchedXBV.InnerText = ds.Tables[5].Rows[0]["wkrlegbv"].ToString();
                        MatchedYBV.InnerText = ds.Tables[5].Rows[0]["wkrlegbv"].ToString();

                        CfXBV.InnerText = ds.Tables[5].Rows[0]["LegXBvCF"].ToString();
                        CfYBV.InnerText = ds.Tables[5].Rows[0]["LegYBvCF"].ToString();
                    }
                    else
                    {
                        BfXBV.InnerText = "0";
                        BfYBV.InnerText = "0";
                        NewXBV.InnerText = "0";
                        NewYBV.InnerText = "0";
                        MatchedXBV.InnerText = "0";
                        MatchedYBV.InnerText = "0";
                        CfXBV.InnerText = "0";
                        CfYBV.InnerText = "0";
                    }

                    // Company-specific sections
                    if (Session["CompID"].ToString() == "1009" ||
                        Session["CompID"].ToString() == "1023")
                    {
                        divlevelIncomeGoldwings.Visible = true;
                        RepLevelGoldwings.DataSource = ds.Tables[6];
                        RepLevelGoldwings.DataBind();
                    }

                    if (comp == "1090" ||
                        comp == "1091" ||
                        comp == "1095" ||
                        Session["CompID"].ToString() == "1097" ||
                        comp == "1100")
                    {
                        divDownlineIncome.Visible = true;
                        repdownlineincome.DataSource = ds.Tables[6];
                        repdownlineincome.DataBind();
                    }

                    if (Session["CompID"].ToString() == "1090")
                    {
                        divlevelIncomeGoldwings.Visible = true;
                        RepLevelGoldwings.DataSource = ds.Tables[8];
                        RepLevelGoldwings.DataBind();
                    }

                    if (Session["CompID"].ToString() == "1023")
                    {
                        divDownlineIncome.Visible = true;
                        repdownlineincome.DataSource = ds.Tables[7];
                        repdownlineincome.DataBind();
                    }

                    if (Session["CompID"].ToString() == "1030")
                    {
                        divMentorshipLife.Visible = true;
                        repMentorship.DataSource = ds.Tables[6];
                        repMentorship.DataBind();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log ex if needed
        }
    }
    private void getData()
    {
        try
        {
            cls_DataAccess dbConnect = new cls_DataAccess(
                HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString()
            );

            dbConnect.OpenConnection();

            SqlCommand cmd = new SqlCommand(ObjDAL.IsoStart + "SELECT * FROM " + ObjDAL.dBName + "..M_CompanyMaster" + ObjDAL.IsoEnd, dbConnect.cnnObject);
            SqlDataReader dRead = cmd.ExecuteReader();

            if (dRead.Read())
            {
                Session["CompAdd"] = dRead["CompAdd"];
                Session["CompWeb"] = (dRead["WebSite"].ToString() == "")
                    ? "index.asp"
                    : dRead["WebSite"].ToString();

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

                string protocol = HttpContext.Current.Request.Url.Host.ToUpper().StartsWith("HTTPS")
                    ? "https://"
                    : "https://";

                Session["CompShortUrl"] = protocol + HttpContext.Current.Request.Url.Host + "/" + Session["JoinPage"];
            }
            else
            {
                Session["CompName"] = "";
                Session["CompAdd"] = "";
                Session["CompWeb"] = "";
                Session["Title"] = "Welcome";
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
                .Replace("LOGIN.", "");

            string str = "";

            if (url == "LOCALHOST")
            {
                str = "SELECT ID,Logo,PartyCode,Name,URL,gvPortalCompID,UtiLityPortalID " +
                      "FROM M_CompanyMasterNew WHERE IsActive = 1 AND ID='" +
                      ConfigurationManager.AppSettings["CompanyID"] + "'";
            }
            else
            {
                str = "SELECT ID,Logo,PartyCode,Name,URL,gvPortalCompID,UtiLityPortalID " +
                      "FROM M_CompanyMasterNew WHERE IsActive = 1 AND UPPER(URL)='" + url.Trim() + "'";
            }

            conn = new SqlConnection(Application["sConnect"].ToString());
            conn.Open();

            SqlCommand cmd = new SqlCommand(str, conn);
            SqlDataReader dRead = cmd.ExecuteReader();

            if (dRead.Read())
            {
                Session["CompID"] = dRead["ID"];
                Session["CompName"] = dRead["Name"];
                Session["Logo"] = dRead["Logo"];
            }
            else
            {
                Response.Redirect("UnderCons.aspx", false);
            }

            dRead.Close();
            conn.Close();
        }
        catch (Exception)
        {
            if (conn != null && conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        return url;
    }

}