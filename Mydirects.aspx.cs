using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;

public partial class Mydirects : System.Web.UI.Page
{
    DataSet Ds;
    DataTable dt;
    SqlConnection conn = new SqlConnection();
    SqlCommand Comm = new SqlCommand();
    SqlDataAdapter Adp;
    DAL Obj;
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
                    }
                    else
                    {
                        DivSearchBy.Visible = true;
                        DivSearchByGreen.Visible = false;
                    }



                    if (compId == "1105")
                    {
                        trDirectBV.Visible = false;
                    }
                    else
                    {
                        trDirectBV.Visible = true;

                    }
                    // FillLevel();
                    // DdlLevel.SelectedValue = "0";
                    LevelDetail(1);
                    FillData();
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
    protected void PageSize_Changed(object sender, EventArgs e)
    {
        try
        {
            this.LevelDetail(1);
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
            Obj = new DAL();

            string legno = "";
            string level = "";

            string compId = Session["CompID"]?.ToString();

            if (compId == "1109")
            {
                if (rbtnsearchr.SelectedValue == "L")
                {
                    legno = "0";
                    level = "1";
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
                    level = "1";
                }
                else
                {
                    legno = rbtnsearch.SelectedValue;
                    level = "1";
                }
            }





            SqlParameter[] prms = new SqlParameter[7];
            prms[0] = new SqlParameter("@MLevel", level);
            prms[1] = new SqlParameter("@Legno", legno);
            prms[2] = new SqlParameter("@ActiveStatus", DDlSearchby.SelectedValue);
            prms[3] = new SqlParameter("@FormNo", Session["FormNo"]);
            prms[4] = new SqlParameter("@PageIndex", pageIndex);
            prms[5] = new SqlParameter("@PageSize", int.Parse("20"));
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
            string qry = Obj.IsoStart + "Select * from " + Obj.dBName + "..V#ReferalDownlineinfo where Formno=" + Session["FormNo"] + Obj.IsoEnd;
            DataTable dt1 = new DataTable();

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
        try
        {
            LevelDetail(1);
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

}
