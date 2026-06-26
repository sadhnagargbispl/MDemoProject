using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Groupdirects : System.Web.UI.Page
{
    DataSet Ds;
    DataTable dt;
    SqlConnection conn = new SqlConnection();
    SqlCommand Comm = new SqlCommand();
    SqlDataAdapter Adp;
    // DAL object
    // Obj = new DAL(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
    DAL Obj;
    DataTable dtData = new DataTable();
    // string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    clsGeneral objGen = new clsGeneral();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                if (!Page.IsPostBack)
                {
                    string compId = Session["CompID"]?.ToString();

                    if (compId == "1109")
                    {
                        DivSearchBy.Visible = false;
                        DivSearchByGreen.Visible = true;
                        divgreen.Visible = true;
                        divall.Visible = false;
                    }
                    else
                    {
                        DivSearchBy.Visible = true;
                        DivSearchByGreen.Visible = false;
                        divgreen.Visible = false;
                        divall.Visible = true;
                    }

                    FillLevel();
                    DdlLevel.SelectedValue = "0";

                    LevelDetail(1);
                    FillData();
                    //Filldate();

                    if (Session["CompID"] != null && Session["CompID"].ToString() == "1101")
                    {
                        DivH.Visible = false;
                    }
                    else
                    {
                        DivH.Visible = true;
                    }

                    if (Session["CompID"] != null &&
                       (Session["CompID"].ToString() == "1084" || Session["CompID"].ToString() == "1074"))
                    {
                        datewisepanel.Visible = true;
                    }
                    else
                    {
                        datewisepanel.Visible = false;
                    }
                    //string compId = Session["CompID"]?.ToString();

                    if (compId == "1105")
                    {
                        trDirect.Visible = false;

                    }
                    else
                    {
                        trDirect.Visible = true;

                    }
                    //Label1.Visible = false;
                    //lbltotal.Visible = false;
                }
            }
            else
            {
                Response.Redirect("logout.aspx");
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
    public int PageIndex
    {
        get
        {
            return ViewState["PageIndex"] != null ? Convert.ToInt32(ViewState["PageIndex"]) : 1;
        }
        set
        {
            ViewState["PageIndex"] = value;
        }
    }
    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        if (PageIndex > 1)
        {
            PageIndex--;
            LevelDetail(PageIndex);
        }
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        int total = Convert.ToInt32(lbltotal.Text);
        int maxPage = (int)Math.Ceiling((double)total / 10);

        if (PageIndex < maxPage)
        {
            PageIndex++;
            LevelDetail(PageIndex);
        }
    }
    protected void FillLevel()
    {
        try
        {
            Obj = new DAL();

            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@FormNo", Session["FormNo"]);
            prms[1] = new SqlParameter("@type", "N");

            Ds = SqlHelper.ExecuteDataset(
                    HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                    "sp_GetLevel",
                    prms
                );

            DdlLevel.DataSource = Ds.Tables[0];
            DdlLevel.DataTextField = "LevelName";
            DdlLevel.DataValueField = "MLevel";
            DdlLevel.DataBind();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    public void LevelDetail(int pageIndex)
    {
        try
        {
            string legno = "";
            string level = "";
            DateTime startDate;
            DateTime endDate;

            lbltotal.Text = "";
            string compId = Session["CompID"]?.ToString();

            if (compId == "1109")
            {
                if (rbtnsearchr.SelectedValue == "L")
                {
                    legno = "0";
                    level = DdlLevel.SelectedValue;
                }
                else
                {
                    legno = rbtnsearchr.SelectedValue;
                    level = "1";
                }
            }
            else
            {
                if (rbtnsearch.SelectedValue == "L")
                {
                    legno = "0";
                    level = DdlLevel.SelectedValue;
                }
                else
                {
                    legno = rbtnsearch.SelectedValue;
                    level = "1";
                }
            }





            // =======================
            // CASE 1 → COMPID = 1084
            // =======================
            if (Session["CompID"] != null && Session["CompID"].ToString() == "1084")
            {
                startDate = string.IsNullOrEmpty(txtStartDate.Text)
                            ? Convert.ToDateTime(Session["CompDate"])
                            : Convert.ToDateTime(txtStartDate.Text);

                endDate = string.IsNullOrEmpty(txtEndDate.Text)
                          ? Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy"))
                          : Convert.ToDateTime(txtEndDate.Text);

                SqlParameter[] prms = new SqlParameter[10];
                prms[0] = new SqlParameter("@MLevel", level);
                prms[1] = new SqlParameter("@Legno", legno);
                prms[2] = new SqlParameter("@ActiveStatus", DDlSearchby.SelectedValue);
                prms[3] = new SqlParameter("@FormNo", Session["FormNo"]);
                prms[4] = new SqlParameter("@PageIndex", pageIndex);
                prms[5] = new SqlParameter("@PageSize", 20);
                prms[6] = new SqlParameter("@RecordCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
                prms[7] = new SqlParameter("@Startdate", startDate);
                prms[8] = new SqlParameter("@Enddate", endDate);
                prms[9] = new SqlParameter("@SearchType", DDlDate.SelectedValue);

                Ds = SqlHelper.ExecuteDataset(
                        HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                        "sp_GetLevelDetailZaracpanelLR",
                        prms
                    );

                RptDirects.DataSource = Ds.Tables[0];
                RptDirects.DataBind();

                if (Ds.Tables[1].Rows.Count > 0)
                {
                    int recordCount = Convert.ToInt32(Ds.Tables[1].Rows[0]["RecordCount"]);
                    lbltotal.Text = recordCount.ToString();
                    Label1.Visible = true;
                }
            }

            // =======================
            // CASE 2 → COMPID = 1074
            // =======================
            else if (Session["CompID"] != null && Session["CompID"].ToString() == "1074")
            {
                startDate = string.IsNullOrEmpty(txtStartDate.Text)
                            ? Convert.ToDateTime(Session["CompDate"])
                            : Convert.ToDateTime(txtStartDate.Text);

                endDate = string.IsNullOrEmpty(txtEndDate.Text)
                          ? Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy"))
                          : Convert.ToDateTime(txtEndDate.Text);

                SqlParameter[] prms = new SqlParameter[10];
                prms[0] = new SqlParameter("@MLevel", level);
                prms[1] = new SqlParameter("@Legno", legno);
                prms[2] = new SqlParameter("@ActiveStatus", DDlSearchby.SelectedValue);
                prms[3] = new SqlParameter("@FormNo", Session["FormNo"]);
                prms[4] = new SqlParameter("@PageIndex", pageIndex);
                prms[5] = new SqlParameter("@PageSize", 20);
                prms[6] = new SqlParameter("@RecordCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
                prms[7] = new SqlParameter("@Startdate", startDate);
                prms[8] = new SqlParameter("@Enddate", endDate);
                prms[9] = new SqlParameter("@SearchType", DDlDate.SelectedValue);

                Ds = SqlHelper.ExecuteDataset(
                        HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                        "sp_GetLevelDetailZaracpanel",
                        prms
                    );

                RptDirects.DataSource = Ds.Tables[0];
                RptDirects.DataBind();

                if (Ds.Tables[1].Rows.Count > 0)
                {
                    int recordCount = Convert.ToInt32(Ds.Tables[1].Rows[0]["RecordCount"]);
                    lbltotal.Text = recordCount.ToString();
                    Label1.Visible = true;
                }
            }

            // =======================
            // DEFAULT CONDITION
            // =======================
            else
            {
                SqlParameter[] prms = new SqlParameter[7];
                prms[0] = new SqlParameter("@MLevel", level);
                prms[1] = new SqlParameter("@Legno", legno);
                prms[2] = new SqlParameter("@ActiveStatus", DDlSearchby.SelectedValue);
                prms[3] = new SqlParameter("@FormNo", Session["FormNo"]);
                prms[4] = new SqlParameter("@PageIndex", pageIndex);
                prms[5] = new SqlParameter("@PageSize", 20);
                prms[6] = new SqlParameter("@RecordCount", SqlDbType.Int) { Direction = ParameterDirection.Output };

                Ds = SqlHelper.ExecuteDataset(
                        HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                        "sp_GetLevelDetail",
                        prms
                    );

                RptDirects.DataSource = Ds.Tables[0];
                RptDirects.DataBind();

                int recordCount = Convert.ToInt32(Ds.Tables[1].Rows[0]["RecordCount"]);
                lbltotal.Text = recordCount.ToString();
                Label1.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + "SideB");

            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void FillData()
    {
        try
        {
            string qry = "";
            qry = Obj.IsoStart + "Select * from " + Obj.dBName + "..V#ReferalDownlineinfonew where Formno=" + Session["FormNo"] + Obj.IsoEnd;
            DataTable Dt1 = new DataTable();
            dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, qry).Tables[0];
            if (dt.Rows.Count > 0)
            {
                tdDirectleft.InnerText = dt.Rows[0]["RegisterLeft"].ToString();
                tdDirectright.InnerText = dt.Rows[0]["RegisterRight"].ToString();
                TotalDirect.InnerText = (Convert.ToInt32(dt.Rows[0]["RegisterLeft"]) + Convert.ToInt32(dt.Rows[0]["RegisterRight"])).ToString();
                
                tddirectActive.InnerText = dt.Rows[0]["ConfirmLeft"].ToString();
                tdindirectActive.InnerText = dt.Rows[0]["ConfirmRight"].ToString();
                TotalActive.InnerText = (Convert.ToInt32(dt.Rows[0]["ConfirmLeft"]) + Convert.ToInt32(dt.Rows[0]["ConfirmRight"])).ToString();
                
                Directunit.InnerText = dt.Rows[0]["LeftBv"].ToString();
                indirectunit.InnerText = dt.Rows[0]["RightBv"].ToString();
                totalunit.InnerText = (Convert.ToInt32(dt.Rows[0]["LeftBv"]) + Convert.ToInt32(dt.Rows[0]["RightBv"])).ToString();
                string compId = Session["CompID"]?.ToString();
                if (compId == "1109")
                {
                    TotalDirect1.InnerText = (Convert.ToInt32(dt.Rows[0]["RegisterLeft"]) + Convert.ToInt32(dt.Rows[0]["RegisterRight"])).ToString();
                    TotalActive1.InnerText = (Convert.ToInt32(dt.Rows[0]["ConfirmLeft"]) + Convert.ToInt32(dt.Rows[0]["ConfirmRight"])).ToString();
                    TotalTeam.InnerText = (Convert.ToInt32(dt.Rows[0]["totalleftteam"]) + Convert.ToInt32(dt.Rows[0]["totalrightteam"])).ToString();
                    TotalActiveTeam.InnerText = (Convert.ToInt32(dt.Rows[0]["totalleftactiveteam"]) + Convert.ToInt32(dt.Rows[0]["totalleftactiveteam"])).ToString();
                }
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

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        LevelDetail(1);
    }
    protected void rbtnsearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rbtnsearch.SelectedValue == "L")
            {
                lbllevel.Visible = true;
                // lbltotal.Visible = false;
            }
            else
            {
                lbllevel.Visible = false;
                // lbltotal.Visible = false;
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
    protected void DDlDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDlDate.SelectedValue == "A")
        {
            // lbltotal.Visible = false;
        }
        else
        {
            // lbltotal.Visible = false;
        }
    }

}