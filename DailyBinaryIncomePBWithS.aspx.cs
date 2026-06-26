using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DailyBinaryIncomePBWithS : System.Web.UI.Page
{
    DataTable dt;
    DAL Obj;
    clsGeneral objGen = new clsGeneral();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {


            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                Obj = new DAL();
                if (!Page.IsPostBack)
                {
                    Filldata();
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
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);

            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }

    }
    protected void gv_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[2].Visible = false;
    }

    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        Filldata();
    }

    private void Filldata()
    {
        try
        {
            string str = "";
            DateTime startDate;
            DateTime endDate;

            if (string.IsNullOrEmpty(TxtFromDate.Text))
            {
                startDate = Convert.ToDateTime(Session["CompDate"]);
            }
            else
            {
                startDate = Convert.ToDateTime(TxtFromDate.Text);
            }

            if (string.IsNullOrEmpty(TxtToDate.Text))
            {
                endDate = DateTime.Now;
            }
            else
            {
                endDate = Convert.ToDateTime(TxtToDate.Text);
            }

            str = Obj.IsoStart + "Exec sp_GetDailyPayoutDetailDisPBWithS '" +
                  Session["Formno"] + "','" +
                  TxtFromDate.Text + "','" +
                  TxtToDate.Text + "'" + Obj.IsoEnd;

            dt = new DataTable();
            dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];

            if (dt.Rows.Count > 0)
            {
                Session["DirectData1"] = dt;
                gv.DataSource = dt;
                gv.DataBind();
            }
            else
            {
                DataRow row = dt.NewRow();
                gv.DataSource = dt;
                gv.DataBind();
            }
        }
        catch (Exception ex)
        {
            // optional logging here
        }
    }
    protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gv.PageIndex = e.NewPageIndex;

        // Session se data bind karein
        DataTable dt = (DataTable)Session["DirectData1"];

        gv.DataSource = dt;
        gv.DataBind();
    }

}