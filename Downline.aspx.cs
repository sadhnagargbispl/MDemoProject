using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;

public partial class Downline : System.Web.UI.Page
{
    DataTable dt;
    SqlConnection conn = new SqlConnection();
    SqlCommand Comm = new SqlCommand();
    SqlDataAdapter Adp;
    DataSet ds = new DataSet();

    private clsGeneral dbGeneral = new clsGeneral();
    private cls_DataAccess dbConnect;

    // public string formNo;
    string strquery;
    string FrmCondition = "";
    int ACnt = 0;
    int BCnt = 0;

    // DAL objDal = new DAL(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
    DAL objDal;

    // ModuleFunction objModuleFun = new ModuleFunction();
    clsGeneral objGen = new clsGeneral();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                // UserStatus.InnerHtml = "<p>Welcome " + Session["MemName"] + "(" + Session["FormNo"] + ") To " + Session["CompName"] + " </p>";

                objDal = new DAL();

                if (!Page.IsPostBack)
                {
                    FillDownlineSumm();
                    FillDownline(null, true);
                    FillDownline();
                    if (Session["CompId"].ToString() == "1074")
                    {
                        joiningtype.Visible = true;
                        startdate.Visible = true;
                        enddate.Visible = true;
                        idbtnsearch.Visible = true;
                    }
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

            objDal.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }

    }
    int PageSize = 20;

    int PageIndexA
    {
        get { return ViewState["PageIndexA"] == null ? 0 : (int)ViewState["PageIndexA"]; }
        set { ViewState["PageIndexA"] = value; }
    }

    int PageIndexB
    {
        get { return ViewState["PageIndexB"] == null ? 0 : (int)ViewState["PageIndexB"]; }
        set { ViewState["PageIndexB"] = value; }
    }
    private DataTable PaginateTable(DataTable dt, int pageIndex)
    {
        DataTable newDt = dt.Clone();

        int start = pageIndex * PageSize;

        for (int i = start; i < start + PageSize && i < dt.Rows.Count; i++)
            newDt.ImportRow(dt.Rows[i]);

        return newDt;
    }
    protected void lnkPrevA_Click(object sender, EventArgs e)
    {
        if (PageIndexA > 0)
            PageIndexA--;

        BindLeft();
    }

    protected void lnkNextA_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["DirectData1"];

        int maxPage = (dt.Rows.Count - 1) / PageSize;

        if (PageIndexA < maxPage)
            PageIndexA++;

        BindLeft();
    }
    private void BindLeft()
    {
        DataTable dt = (DataTable)Session["DirectData1"];
        if (dt != null)
        {
            DataTable paged = PaginateTable(dt, PageIndexA);
            RptDirects.DataSource = paged;
            RptDirects.DataBind();
        }
    }
    protected void lnkPrevB_Click(object sender, EventArgs e)
    {
        if (PageIndexB > 0)
            PageIndexB--;

        BindRight();
    }

    protected void lnkNextB_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["DirectData2"];

        int maxPage = (dt.Rows.Count - 1) / PageSize;

        if (PageIndexB < maxPage)
            PageIndexB++;

        BindRight();
    }
    private void BindRight()
    {
        DataTable dt = (DataTable)Session["DirectData2"];
        if (dt != null)
        {
            DataTable paged = PaginateTable(dt, PageIndexB);
            Repeater3.DataSource = paged;
            Repeater3.DataBind();
        }
    }


    private void FillDownline( string Condition = "", bool IsSideA = false)
    {
        try
        {
            Condition = "";
            DataTable Dt = new DataTable();

            if (Session["CompId"].ToString() == "1074")
            {
                if (DDltype.SelectedValue == "J")
                {
                    if (!string.IsNullOrEmpty(txtStartDate.Text) &&
                        !string.IsNullOrEmpty(txtEndDate.Text))
                    {
                    }
                }

                if (DDltype.SelectedValue == "A")
                {
                    if (!string.IsNullOrEmpty(txtStartDate.Text) &&
                        !string.IsNullOrEmpty(txtEndDate.Text))
                    {
                    }
                }

                if (string.IsNullOrEmpty(txtStartDate.Text) &&
                    string.IsNullOrEmpty(txtEndDate.Text))
                {
                    strquery =
                       objDal.IsoStart + "exec sp_ShowDownline " +
                        Session["Formno"] + "," +
                        (IsSideA ? 1 : 2) +
                        (Condition == "" ? "" : "," + Condition) + objDal.IsoEnd;
                }
                else
                {
                    string sDate = Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd");
                    string eDate = Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd");

                    strquery =
                        objDal.IsoStart + "exec sp_ShowDownLinedatewaise " +
                        Session["Formno"] + "," +
                        (IsSideA ? 1 : 2) +
                        ",'" + sDate + "','" + eDate + "','" +
                        DDltype.SelectedValue + "'" + objDal.IsoEnd;
                }
            }
            else
            {
                strquery =
                    "exec sp_ShowDownline " +
                    Session["Formno"] + "," +
                    (IsSideA ? 1 : 2) +
                    (Condition == "" ? "" : "," + Condition);
            }

            // Fetch data
            Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, strquery).Tables[0];

            if (IsSideA)
            {
               Session["DirectData1"] = Dt;
                DataTable pagedLeft = PaginateTable(Dt, PageIndexA);
                RptDirects.DataSource = pagedLeft;
                RptDirects.DataBind();
                //RptDirects.DataSource = Dt;
                //RptDirects.DataBind();
                DivSideA.Style["display"] = "block";

            }
            else
            {
                //Session["DirectData2"] = Dt;
                //Repeater3.DataSource = Dt;
                Session["DirectData2"] = Dt;
                DataTable pagedRight = PaginateTable(Dt, PageIndexB);
                Repeater3.DataSource = pagedRight;
                Repeater3.DataBind();
                DivSideB.Style["display"] = "block";
            }

            ds.Dispose();
            RadioButton();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + (IsSideA ? "SideA" : "SideB"));

            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text =
                path + ": " +
                DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                Environment.NewLine;

            objDal.WriteToFile(text + ex.Message);

            Response.Write("Try later.");
        }
    }

    private void FillDownlineSumm()
    {
        try
        {
            strquery = "SELECT * FROM V#DownlineInfo WHERE FormNo=" + Session["Formno"];

            DataTable dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, strquery).Tables[0];

            if (dt.Rows.Count > 0)
            {
                LblMemLJ.InnerText = dt.Rows[0]["RegisterLeft"].ToString();
                LblMemRJ.InnerText = dt.Rows[0]["RegisterRight"].ToString();
                LblMemLT.InnerText = dt.Rows[0]["ConfirmLeft"].ToString();
                LblMemRT.InnerText = dt.Rows[0]["ConfirmRight"].ToString();
                LblLeftBv.InnerText = dt.Rows[0]["Leftbv"].ToString();
                LblRightBv.InnerText = dt.Rows[0]["Rightbv"].ToString();
            }

            RadioButton();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            objDal.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void ExportDownline(string Condition = "", bool IsSideA = false)
    {
        try
        {
            DataTable dtTemp = new DataTable();
            Condition = "";
            strquery = "exec sp_ShowDownline " + Convert.ToInt32(Session["Formno"]) + "," + (IsSideA ? 1 : 2);

            dt = new DataTable();
            dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, strquery).Tables[0];

            dtTemp.Columns.Add("<b>Id No</b>");
            dtTemp.Columns.Add("<b>Member Name</b>");
            dtTemp.Columns.Add("<b>Sponsor ID</b>");
            dtTemp.Columns.Add("<b>Date Of Joining</b>");
            dtTemp.Columns.Add("<b>Package Name</b>");
            dtTemp.Columns.Add("<b>Activation Date</b>");
            dtTemp.Columns.Add("<b>Package</b>");
            dtTemp.Columns.Add("<b>SV</b>");

            for (int rCnt = 0; rCnt < dt.Rows.Count; rCnt++)
            {
                DataRow drAddItem = dtTemp.NewRow();
                for (int cCnt = 0; cCnt <= 7; cCnt++)
                {
                    drAddItem[cCnt] = dt.Rows[rCnt][cCnt].ToString();
                }
                dtTemp.Rows.Add(drAddItem);
            }

            DataGrid dg = new DataGrid
            {
                DataSource = dtTemp
            };
            dg.DataBind();

            ExportToExcel(IsSideA ? "SideADownline.xls" : "SideBDownline.xls", dg);
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message + (IsSideA ? "Error In Exporting Side A" : "Error In Exporting Side B"));

            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            objDal.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void ExportToExcel(string strFileName, DataGrid dg)
    {
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw;

        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/vnd.xls";
        Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
        Response.Charset = "";

        dg.EnableViewState = false;
        htw = new HtmlTextWriter(sw);
        dg.RenderControl(htw);

        Response.Write(sw.ToString());
        Response.End();
    }
    private void ExportToSpreadsheet(DataTable table, string name)
    {
        HttpContext context = HttpContext.Current;
        context.Response.Clear();

        foreach (DataColumn column in table.Columns)
        {
            context.Response.Write(column.ColumnName + ";");
        }

        context.Response.Write(Environment.NewLine);

        foreach (DataRow row in table.Rows)
        {
            for (int i = 0; i < table.Columns.Count; i++)
            {
                context.Response.Write(row[i].ToString().Replace(";", "") + ";");
            }

            context.Response.Write(Environment.NewLine);
        }

        context.Response.ContentType = "text/csv";
        context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + name + ".csv");
        context.Response.End();
    }
    private void RadioButton()
    {
        try
        {
            if (rbleg.SelectedIndex == 1)
            {
                DivSideA.Style["display"] = "block";
                DivSideB.Style["display"] = "none";
            }
            else if (rbleg.SelectedIndex == 2)
            {
                DivSideA.Style["display"] = "none";
                DivSideB.Style["display"] = "block";
            }
            else
            {
                DivSideA.Style["display"] = "block";
                DivSideB.Style["display"] = "block";
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            objDal.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected void BtnExportB_Click(object sender, EventArgs e)
    {
        try
        {
            ExportDownline("", false);
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            objDal.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            FillDownlineSumm();
            FillDownline("", true);
            FillDownline();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            objDal.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected void BtnExportA_Click(object sender, EventArgs e)
    {
        try
        {
            ExportDownline("", true);
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            objDal.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected void rbleg_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButton();
    }
}