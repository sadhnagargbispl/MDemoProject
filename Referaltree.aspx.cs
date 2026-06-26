using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;

public partial class Referaltree : System.Web.UI.Page
{
    string strQuery;
    int minDeptLevel;
    SqlConnection Cnn = new SqlConnection();
    SqlCommand Comm = new SqlCommand();
    SqlDataAdapter Adp1;
    DataSet dsGetQry = new DataSet();
    DAL Obj;
    string strToolTip;
    string strDrawKit;
    string IsoStart;
    string IsoEnd;
    // string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                Cnn = new SqlConnection((string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]]);
                Obj = new DAL();
                if (!IsPostBack)
                {
                    ValidateTree();
                }
            }
            else
            {
                Response.Redirect("Logout.aspx");
            }
        }
        catch (Exception ex)
        {
            if (Cnn.State == ConnectionState.Open)
            {
                Cnn.Close();
            }

            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void ValidateTree()
    {
        try
        {
            string strSelectedFormNo;
            int minDeptLevel;

            // If User Not Set MinDept level, then set 1 as the default
            if (string.IsNullOrEmpty(Request["DeptLevel"]))
            {
                minDeptLevel = 1;
            }
            else
            {
                minDeptLevel = 1; // Explicitly setting to 1 in both cases
            }

            if (string.IsNullOrEmpty(Session["FormNo"].ToString()) || string.IsNullOrEmpty(Request["DownLineFormNo"]) || Request["DownLineFormNo"] == "123456")
            {
                strSelectedFormNo = Session["FormNo"].ToString();
                BtnStepAbove.Enabled = false;
            }
            else
            {
                if (!CheckDownLineMemberTree())
                {
                    BtnStepAbove.Enabled = false;
                    Response.Write("Please Check DownLine Member ID");
                    Response.End();
                }
                strSelectedFormNo = Request["DownLineFormNo"];
                BtnStepAbove.Enabled = true;
            }

            string strQuery = getQuery(strSelectedFormNo, minDeptLevel);
            GenerateTree(strQuery);
        }
        catch (Exception ex)
        {
            BtnStepAbove.Enabled = false;
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private bool CheckDownLineMemberTree()
    {
        try
        {
            bool checkDownLineMemberTree = false;
            DataSet ds1 = new DataSet();
            SqlParameter[] prms = new SqlParameter[2];
            prms[0] = new SqlParameter("@FormDwn", Request["DownLineFormNo"]);
            prms[1] = new SqlParameter("@FormNo", Session["FormNo"]);
            ds1 = SqlHelper.ExecuteDataset((string)HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]], CommandType.Text, "Exec sp_CheckDownLineMemberTree '" + prms[0].Value + "','" + prms[1].Value + "'");
            if (ds1.Tables[0].Rows.Count <= 0)
            {
                checkDownLineMemberTree = false;
            }
            else
            {
                checkDownLineMemberTree = true;
            }
            ds1.Dispose();
            return checkDownLineMemberTree;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void GenerateTree(string strQuery)
    {
        try
        {
            DataSet ds1 = new DataSet();
            ds1 = SqlHelper.ExecuteDataset((string)HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]], CommandType.Text, IsoStart + strQuery + IsoEnd);
            string ParentId;
            double FormNo;
            string MemberName = "";
            string LegNo;
            string Doj = "";
            string UpgradeDate = "";
            string DirctLeftActive = "";
            string DirectRightActive = "";
            string DirectBV = "0";
            string InDirectBV = "0";
            string Category = "";
            double LeftBV = 0;
            double RightBV = 0;
            double investment = 0;
            string UpLiner = "";
            string Sponsor = "";
            string NodeName = "";
            string myRunTimeString = "";
            string ExpandYesNo;
            string strImageFile = "";
            string strUrlPath = "";
            string tooltipstrig;
            string Mobile = "0";
            string idStatus = "";
            string Isblock = "";
            string slab = "";

            myRunTimeString = myRunTimeString + "<Script Language=Javascript>" + Environment.NewLine;
            myRunTimeString = myRunTimeString + "mytree = new dTree('mytree');" + Environment.NewLine;
            tooltipstrig = ToolTipTable();
            ParentId = "-1";
            if (Request["DownLineFormNo"] != null)
                FormNo = Convert.ToInt32(Request["DownLineFormNo"]);
            else
                FormNo = Convert.ToInt32(Session["FormNo"]);


            DataSet tmpDS = new DataSet();
            DataSet tmpDS1 = new DataSet();

            tmpDS = SqlHelper.ExecuteDataset((string)HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]], CommandType.Text, IsoStart + " Exec sp_TreeDetail '" + FormNo + "' " + IsoEnd);
            //tmpDS1 = SqlHelper.ExecuteDataset(constr1, CommandType.Text, IsoStart + " select top(1)slab from " + Obj.dBName + "..MstLevelIncome where formno= '" + FormNo + "' order by sessid desc " + IsoEnd);
            if (tmpDS.Tables[0].Rows.Count > 0)
            {

                DataRow row = tmpDS.Tables[0].Rows[0];
                idStatus = row["idstatus"].ToString();
                MemberName = row["IdNo"].ToString();
                NodeName = row["Idno"].ToString() + "(" + row["MemFirstName"].ToString() + ")";
                strImageFile = "img/" + row["Joincolor"].ToString();
                Doj = row["Doj"].ToString();
                LeftBV = Convert.ToDouble(row["Direct"]);
                RightBV = Convert.ToDouble(row["Indirect"]);
                Category = row["BV"].ToString();
                UpgradeDate = row["UpgradeDate1"].ToString();
                DirctLeftActive = row["DirectActive"].ToString();
                DirectRightActive = row["InDirectActive"].ToString();
                Sponsor = row["Sponsorid"].ToString() + "(" + row["SponsorName"].ToString() + ")";
                Mobile = "0";
                Isblock = row["Isblock"].ToString();
                //if (tmpDS1.Tables[0].Rows.Count > 0)
                //{
                //    slab = tmpDS1.Tables[0].Rows[0]["slab"].ToString();
                //}
                //else
                //{
                //    slab = "0";
                //}
            }
            myRunTimeString = myRunTimeString + "mytree = new dTree('mytree','" + strImageFile + "');" + Environment.NewLine;
            myRunTimeString = myRunTimeString + " mytree.add(" + FormNo + "," + ParentId + "," + "'" + Category + "'" + "," + "'" + Doj + "'," + "'" + UpgradeDate + "','" + MemberName + "','" + NodeName + "'," +
                "'" + UpLiner + "'" + ",'" + Sponsor + "'," + LeftBV + "," + RightBV + "," + DirctLeftActive + "," + DirectRightActive + "," + "'" + strUrlPath + "'" + ",'" + MemberName + "'," + "'" + "" + "'" + "," +
                "" + "'" + strImageFile + "'" + "," + "'" + strImageFile + "'" + ",'true','" + idStatus + "','" + Mobile + "','" + Isblock + "');" + Environment.NewLine;

            //myRunTimeString += "mytree = new dTree('mytree','" + strImageFile + "');\n";
            //        //myRunTimeString += " mytree.add({FormNo},{ParentId},'{Category}','{Doj}','{UpgradeDate}','{MemberName}','{NodeName}','{UpLiner}','{Sponsor}',{LeftBV},{RightBV},'{DirctLeftActive}','{DirectRightActive}','{strUrlPath}','{MemberName}',''," +
            //        //                   "'{strImageFile}','{strImageFile}','true','{idStatus}','{Mobile}','{Isblock}');\n";


            int LoopValue = 0;
            string FolderFile = "";


            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                //ParentId = Convert.ToInt32(dr["RefFormno"]);
                //FormNo = Convert.ToDouble(dr["FormNoDwn"]);
                ParentId = dr["RefFormno"].ToString();
                FormNo = Convert.ToDouble(dr["FormNoDwn"].ToString());
                LegNo = dr["Reflegno"].ToString();
                UpLiner = "0";
                Sponsor = dr["Sponsorid"].ToString() + "(" + dr["SponsorName"].ToString() + ")";
                Doj = dr["doj"].ToString();
                Category = "0";
                LeftBV = Convert.ToDouble(dr["Direct"]);
                RightBV = Convert.ToDouble(dr["Indirect"]);
                idStatus = dr["Idstatus"].ToString();

                strUrlPath = "Referaltree.aspx?DownLineFormNo=" + FormNo + "";

                strImageFile = dr["JoinColor"].ToString();
                MemberName = dr["IdNO"].ToString();
                NodeName = dr["Idno"].ToString() + "(" + dr["MemFirstName"].ToString() + ")";
                UpgradeDate = dr["UpgradeDate"].ToString();
                DirctLeftActive = dr["ActiveDirect"].ToString();
                DirectRightActive = dr["ActiveInDirect"].ToString();
                // Mobile = dr["mobl"].ToString();
                //Isblock = dr["Isblock"].ToString();

                LoopValue++;
                ExpandYesNo = LoopValue <= 3 ? "true" : "false";

                if (FormNo <= 0)
                {
                    strImageFile = "img/empty.jpg";
                    MemberName = "Direct";
                    strUrlPath = "";
                }
                else
                {
                    strImageFile = "img/" + dr["JoinColor"];

                }

                myRunTimeString += " mytree.add(" + FormNo + "," + ParentId + ",'" + Category + "','" + Doj + "','" + UpgradeDate + "','" + MemberName + "','" + NodeName + "','" + UpLiner + "','" + Sponsor + "',";
                myRunTimeString += "" + LeftBV + "," + RightBV + ",'" + DirctLeftActive + "','" + DirectRightActive + "','" + strUrlPath + "','" + MemberName + "','',";
                myRunTimeString += "'" + strImageFile + "','" + strImageFile + "'," + ExpandYesNo + ",'" + idStatus + "','" + Mobile + "','" + Isblock + "');\n";
            }
            myRunTimeString = myRunTimeString + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + " document.write(mytree);" + Environment.NewLine;
            myRunTimeString = myRunTimeString + "</script> " + "<br> <br> <br> <br> ";
            RegisterStartupScript("clientScript", myRunTimeString);
        }
        catch (Exception ex)
        {
            Response.Write("Try later.");
        }
    }
    private string ToolTipTable()
    {

        // strToolTip = "onMouseOver=""ddrivetip('<table width=100% border=0 cellpadding=5 cellspacing=1 bgcolor=#CCCCCC class=containtd>...</table>')"" onMouseOut=""hideddrivetip()"""
        return strToolTip;
    }
    private string getQuery(string strSelectedFormNo, int minDeptLevel)
    {
        try
        {
            return "exec sp_ShowRefTree " + strSelectedFormNo + " , " + minDeptLevel;
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
            if (Cnn.State == ConnectionState.Open)
            {
                Cnn.Close();
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
            if (Cnn.State == ConnectionState.Open)
            {
                Cnn.Close();
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
        }
    }
    protected void cmdBack_Click(object sender, EventArgs e)
    {
        try
        {
            HttpContext.Current.Response.Redirect(HttpContext.Current.Session["IndexPage"]?.ToString(), false);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    protected void BtnStepAbove_Click(object sender, EventArgs e)
    {

    }
}
