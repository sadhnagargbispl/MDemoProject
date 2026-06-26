using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

public partial class ComplainSolution : System.Web.UI.Page
{
    DAL Obj;
    DataTable dt;
    string str = "";
    clsGeneral objGen = new clsGeneral();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           

            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                Obj = new DAL();
                lblother.Text = "Complaint Detail";
                lbludaan.Visible = false;
                lblother.Visible = true;
                str = Obj.IsoStart + "Select M.IDNo, M.CID, Cast(M.CID as varchar) as VCId, M.MemName," +
                      " ISNULL(Replace(CONVERT(varchar, M.RecTimeStamp, 106), ' ', '-'), '') as CDate," +
                      " M.CType, M.Complaint, ISNULL(S.Solution, '') as Solution," +
                      " ISNULL(Replace(CONVERT(varchar, S.RecTimeStamp, 106), ' ', '-'), '') as SDate " +
                      " FROM (Select b.MemFirstName + ' ' + b.MemLastName as MemName, a.* " +
                      "       FROM " + Obj.dBName + "..M_ComplaintMaster a, " + Obj.dBName + "..M_MemberMaster b" +
                      "       WHERE a.IDNo = b.IDNo AND a.IDNo = '" + Session["IDNo"] + "') M" +
                      " LEFT JOIN " + Obj.dBName + "..M_SolutionMaster S ON M.CID = S.CID " +
                      " WHERE 1 = 1 ORDER BY M.RecTimeStamp DESC" + Obj.IsoEnd;

                dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];

                RptDirects.DataSource = dt;
                RptDirects.DataBind();
                Session["DirectData1"] = dt;
            }
            else
            {
                Response.Redirect("logout.aspx");
            }

            
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);

            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }
    }
    int PageSize = 10;

    public int PageIndex
    {
        get { return ViewState["PageIndex"] == null ? 0 : Convert.ToInt32(ViewState["PageIndex"]); }
        set { ViewState["PageIndex"] = value; }
    }
    private void ApplyPagination()
    {
        DataTable dt = (DataTable)Session["DirectData1"]; // your main data
        int total = dt.Rows.Count;

        // Disable PREV on first page
        lnkPrev.Enabled = PageIndex > 0;

        // Disable NEXT on last page
        int maxPage = (int)Math.Ceiling((double)total / PageSize) - 1;
        lnkNext.Enabled = PageIndex < maxPage;

        // Paginate data
        DataTable pagedData = dt.Clone();
        int start = PageIndex * PageSize;
        int end = Math.Min(start + PageSize, total);

        for (int i = start; i < end; i++)
            pagedData.ImportRow(dt.Rows[i]);

        // Bind
        RptDirects.DataSource = pagedData;
        RptDirects.DataBind();
    }
    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        if (PageIndex > 0)
            PageIndex--;

        ApplyPagination();
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["DirectData1"];
        int total = dt.Rows.Count;

        int maxPage = (int)Math.Ceiling((double)total / PageSize) - 1;

        if (PageIndex < maxPage)
            PageIndex++;

        ApplyPagination();
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }
    }
}
