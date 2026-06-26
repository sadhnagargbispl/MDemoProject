using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.VariantTypes;

public partial class RefIndex : System.Web.UI.Page
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
                Obj = new DAL();
                string compId = Convert.ToString(Session["CompID"]);
                if (!Page.IsPostBack)
                {
                    bool showRegRows = (compId != "1078" && compId != "1093");
                    bool showActivationRows = (compId != "1093");
                    LoadTeam();
                    Fill_Balance();
                    AchieveReward();
                    Filldirect();


                    if (compId == "1068" || compId == "1077" || compId == "1082" ||
                        compId == "1089" || compId == "1092" || compId == "1101")
                    {
                        trReferalHead.Visible = false;
                        aRfLink.Visible = true;

                        if (compId == "1089")
                        {
                            // VB used a hard-coded URL for 1089
                            // note: Crypto.Encrypt should return a string
                            lblLink.Text = "https://megamart.ai/Registartion.aspx?ref=" + Crypto.Encrypt(Convert.ToString(Session["IdNo"]) + "/0");
                            aRfLink.HRef = lblLink.Text;
                        }
                        else
                        {
                            lblLink.Text = Convert.ToString(Session["CompShortUrl"]) + "?ref=" + Crypto.Encrypt(Convert.ToString(Session["IdNo"]) + "/0/0");
                            aRfLink.HRef = lblLink.Text;
                        }
                    }
                    else if (compId == "1093")
                    {
                        // Original VB hid Div13 for 1093
                        Div13.Visible = false;
                    }
                    else
                    {
                        trReferalHead.Visible = true;
                        lblLink.Text = Convert.ToString(Session["CompShortUrl"]) + "?ref=" + Crypto.Encrypt(Convert.ToString(Session["IdNo"]) + "/1") + "&node=1";
                        aRfLink.HRef = lblLink.Text;

                        lbllink1.Text = Convert.ToString(Session["CompShortUrl"]) + "?ref=" + Crypto.Encrypt(Convert.ToString(Session["IdNo"]) + "/2") + "&node=2";
                        aRfLink1.HRef = lbllink1.Text;
                    }
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            // If Obj has WriteToFile like VB; otherwise replace with your logger
            // Obj.WriteToFile(text + ex.Message);
            HttpContext.Current.Response.Write("Try later.");
        }
    }
    private void Fill_Balance()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = SqlHelper.ExecuteDataset(Convert.ToString(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]]),
                                         CommandType.Text,
                                         " Exec Sp_GetBalanceDashBord_New " + Convert.ToString(Session["FormNo"]));

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                gvBalance.DataSource = ds.Tables[0];
                gvBalance.DataBind();
                // uddanbalance.Visible = false;
                gvBalance.Visible = true;
            }
            else
            {
                // keep same behavior as VB: bind empty table to both controls
                if (ds != null && ds.Tables.Count > 0)
                {
                    gvBalance.DataSource = ds.Tables[0];
                    gvBalance.DataBind();
                    //uddanbalance.DataSource = ds.Tables[0];
                    //uddanbalance.DataBind();
                }
                else
                {
                    gvBalance.DataSource = null;
                    gvBalance.DataBind();
                    //uddanbalance.DataSource = null;
                    // uddanbalance.DataBind();
                }

                // uddanbalance.Visible = false;
                gvBalance.Visible = true;
            }
        }
        catch (Exception)
        {
            // original VB swallowed exceptions. Keep that behavior,
            // but consider logging here if desired.
        }
    }
    private void GetNews()
    {
        try
        {
            string strQuery;
            Obj = new DAL();

            DataTable tmptable = new DataTable();
            strQuery = Obj.IsoStart + "SELECT NewsHdr,newsdtl FROM " + Obj.dBName + "..M_NewsSeminarMaster WHERE ActiveStatus='Y' " +
                       "AND RowStatus='Y' AND convert(datetime,dbo.formatdate(TODATE,'dd-MMM-yyyy'),1) >= dbo.formatdate(GETDATE(),'dd-MMM-yyyy')" + Obj.IsoEnd;

            tmptable = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, strQuery).Tables[0];

            // original VB did not do anything with tmptable after fetch;
            // if you need to bind it to a repeater/grid, do it here.
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            // Obj may be null if construction failed; guard it
            try
            {
                if (Obj != null)
                    Obj.WriteToFile(text + ex.Message);
            }
            catch { /* swallow secondary logging errors as VB did */ }

            HttpContext.Current.Response.Write("Try later.");
        }
    }
    private void AchieveReward()
    {
        try
        {
            string strQ;
            Obj = new DAL();

            DataTable tmptable = new DataTable();

            if (Convert.ToString(Session["CompID"]) == "1077")
            {
                // Val(Session("Formno")) in VB — convert safely to integer/string as needed
                strQ = " exec sp_RichNetRankBusiness '" + Convert.ToString(Session["Formno"]) + "'";
            }
            else
            {
                strQ = Obj.IsoStart + "select a.*,CONVERT(varchar,b.frmDate,106) as AchieveDate from " + Obj.dBName + "..V#rewardDetail as a," +
                       " " + Obj.dBName + "..D_Sessnmaster as b where a.SEssid=b.SEssid and IDNO='" + Convert.ToString(Session["IDNO"]) + "' order by Rewardid Desc" + Obj.IsoEnd;
            }

            tmptable = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, strQ).Tables[0];

            if (tmptable != null && tmptable.Rows.Count > 0)
            {
                if (Convert.ToString(Session["CompId"]) == "1077")
                {
                    //LblRichnetreward.Text = Convert.ToString(tmptable.Rows[0]["Upcoming Reward Name"]);
                    //LblRichnetreward.Visible = true;

                    //RichnetReward.DataSource = tmptable;
                    //RichnetReward.DataBind();
                    /// RichnetReward.Visible = true;

                    DataGrid1.Visible = false;
                }
                else
                {
                    // LblRichnetreward.Visible = false;

                    DataGrid1.DataSource = tmptable;
                    DataGrid1.DataBind();
                    DataGrid1.Visible = true;

                    // RichnetReward.Visible = false;
                }
            }
            else
            {
                DataGrid1.Visible = false;
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            try
            {
                if (Obj != null) Obj.WriteToFile(text + ex.Message);
            }
            catch { }
            HttpContext.Current.Response.Write("Try later.");
        }
    }
    public void LoadTeam()
    {
        try
        {
            DataSet Ds = new DataSet();
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@FormNo", HttpContext.Current.Session["FormNo"]);
            string connKey = HttpContext.Current.Session["MlmSelectDatabase" + HttpContext.Current.Session["CompID"]].ToString();
            Ds = SqlHelper.ExecuteDataset(connKey, "sp_LoadTeam1", prms);
            decimal ValDecimal(object o)
            {
                decimal d;
                return decimal.TryParse(Convert.ToString(o), out d) ? d : 0m;
            }

            string SafeString(object o) => Convert.ToString(o);

            if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
            {
                DataRow row0 = Ds.Tables[0].Rows[0];
                TodayDirectRegister.InnerText = SafeString(row0["DirectTodayRegister"]);
                TodayIndirectRegister.InnerText = SafeString(row0["IndirectTodayRegister"]);
                TotalTodayRegister.InnerText = (ValDecimal(row0["DirectTodayRegister"]) + ValDecimal(row0["InDirectTodayRegister"])).ToString();

                TodayDirectActive.InnerText = SafeString(row0["DirectTodayActive"]);
                TodayIndirectActive.InnerText = SafeString(row0["InDirectTodayActive"]);
                TodayTotalActive.InnerText = (ValDecimal(row0["DirectTodayActive"]) + ValDecimal(row0["InDirectTodayActive"])).ToString();

                TotalDirectJoin.InnerText = SafeString(row0["DirectRegister"]);
                TotalIndirectJoin.InnerText = SafeString(row0["InDirectRegister"]);
                TotalJoin.InnerText = (ValDecimal(row0["DirectRegister"]) + ValDecimal(row0["InDirectRegister"])).ToString();

                TotalDirectActivation.InnerText = SafeString(row0["Directactive"]);
                TotalIndirectActivation.InnerText = SafeString(row0["InDirectActive"]);
                TotalActivation.InnerText = (ValDecimal(row0["Directactive"]) + ValDecimal(row0["InDirectActive"])).ToString();

                //prvmnthabv.InnerText = SafeString(row0["PreviousmonthBV"]);
                //crntmnthteambv.InnerText = SafeString(row0["CurrentMonthBV"]);
                //selfcurrntbv.InnerText = SafeString(row0["SelfCurrentmonthBV"]);

                //tBv.InnerHtml = (ValDecimal(row0["SelfCurrentmonthBV"]) + ValDecimal(row0["CurrentMonthBV"])).ToString("0.00");

                //accumteambv.InnerText = SafeString(row0["Totalteambv"]);
                //accumpernlbv.InnerText = SafeString(row0["TotalSelfBV"]);
                //totalbv.InnerText = (ValDecimal(row0["Totalteambv"]) + ValDecimal(row0["TotalSelfBV"])).ToString("0.00");
            }

            if (Ds != null && Ds.Tables.Count > 1 && Ds.Tables[1].Rows.Count > 0)
            {
                DataRow row1 = Ds.Tables[1].Rows[0];
                Image2.ImageUrl = SafeString(row1["ProfilePic"]);
                LblMemId.Text = SafeString(row1["Idno"]);
                LblMemName.Text = SafeString(row1["Name"]);
            }

            if (Ds != null && Ds.Tables.Count > 4 && Ds.Tables[4].Rows.Count > 0)
            {
                string compId = Convert.ToString(HttpContext.Current.Session["compid"]);
                if (compId == "1077")
                {
                    // original VB uses Tables(7) when CompId = 1077
                    if (Ds.Tables.Count > 7)
                    {
                        repMyIncome.DataSource = Ds.Tables[7];
                        repMyIncome.DataBind();
                    }
                    // udaanincome.Visible = false;
                    repMyIncome.Visible = true;
                }
                else
                {
                    repMyIncome.DataSource = Ds.Tables[4];
                    repMyIncome.DataBind();
                    //udaanincome.Visible = false;
                    repMyIncome.Visible = true;
                }
            }

            if (Ds != null && Ds.Tables.Count > 3 && Ds.Tables[3].Rows.Count > 0)
            {
                RptNews.DataSource = Ds.Tables[3];
                RptNews.DataBind();
            }

            if (Ds != null && Ds.Tables.Count > 5 && Ds.Tables[5].Rows.Count > 0)
            {
                string compId = Convert.ToString(HttpContext.Current.Session["CompID"]);
                if (compId == "1007" || compId == "1003")
                {
                    // commented out in original for those comp ids
                    lblRankTitle.Text = SafeString(Ds.Tables[5].Rows[0]["Rank"]);
                }
                else
                {
                    
                    if (SafeString(Ds.Tables[5].Rows[0]["Rank"]) == "")
                    {
                        lblRankTitle.Text = "Not Achieved";
                    }
                    else
                    {
                        lblRankTitle.Text = SafeString(Ds.Tables[5].Rows[0]["Rank"]);
                    }
                }
            }
            // special-case: if CompID is 1007 or 1003 then get rank from table 6 (VB did this)
            if ((Convert.ToString(HttpContext.Current.Session["CompID"]) == "1007" ||
                 Convert.ToString(HttpContext.Current.Session["CompID"]) == "1003") &&
                 Ds != null && Ds.Tables.Count > 6 && Ds.Tables[6].Rows.Count > 0)
            {
                lblRankTitle.Text = SafeString(Ds.Tables[6].Rows[0]["Rank"]);
            }

            if (Convert.ToString(HttpContext.Current.Session["CompID"]) == "1093")
            {
                dvVPRequest.Visible = false;
            }

            //divCurrentMonthLamcy.Visible = false;
            // DivBvDetail.Visible = false;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            // Assuming Obj.WriteToFile exists (as in VB). If not, replace with your logger.
            Obj.WriteToFile(text + ex.Message);
            HttpContext.Current.Response.Write("Try later.");
        }
    }
    private void Filldirect()
    {
        try
        {
            DataSet Ds = new DataSet();
            SqlParameter[] prms = new SqlParameter[7];
            prms[0] = new SqlParameter("@MLevel", "1");
            prms[1] = new SqlParameter("@Legno", "0");
            prms[2] = new SqlParameter("@ActiveStatus", "Y");
            prms[3] = new SqlParameter("@FormNo", Session["FormNo"]);
            prms[4] = new SqlParameter("@PageIndex", "1");
            prms[5] = new SqlParameter("@PageSize", int.Parse("150000000")); // same large page size as VB
            prms[6] = new SqlParameter("@RecordCount", ParameterDirection.Output);
            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + HttpContext.Current.Session["CompID"]].ToString(), "sp_GetLevelDetail", prms);
            if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
            {
                var top10 = Ds.Tables[0].AsEnumerable().Take(10).CopyToDataTable();
                RptDirects.DataSource = top10;
                RptDirects.DataBind();
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            try
            {
                if (Obj != null) Obj.WriteToFile(text + ex.Message);
            }
            catch { }
            HttpContext.Current.Response.Write("Try later.");
        }
    }
}