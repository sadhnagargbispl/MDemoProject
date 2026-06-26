using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Numerics;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;


public partial class NewTree : System.Web.UI.Page
{
    string strQuery;
    int minDeptLevel;
    SqlConnection conn = new SqlConnection();
    SqlCommand Comm = new SqlCommand();
    SqlDataAdapter Adp1;
    DataSet dsGetQry = new DataSet();
    string strDrawKit;
    DAL obj;
    DataSet Ds;
    clsGeneral objGen = new clsGeneral();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                obj = new DAL();

                conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
                conn.Open();

                if (!Page.IsPostBack)
                {
                    int compId = Convert.ToInt32(Session["CompId"]);

                    if (compId == 1109)
                    {
                        BtnExtremeLeft.Text = "Extreme Group A";
                        BtnExtremeRight.Text = "Extreme Group B";
                        Button2.Text = "Extreme Group C";
                        Button2.Visible = true;
                    }
                    else
                    {
                        BtnExtremeLeft.Text = "Extreme Left";
                        BtnExtremeRight.Text = "Extreme Right";
                        Button2.Visible = false;
                    }

                    // ========== Create Proc Sp_CheckDownLineMemTree ==========
                    try
                    {
                        string str = @"
CREATE OR ALTER PROCEDURE Sp_CheckDownLineMemTree
(
    @FormnoDwn VARCHAR(50),
    @FormNo INT
)
AS
BEGIN
    DECLARE @Isformno NUMERIC(18,0);

    SELECT @Isformno = FormNo
    FROM M_MemberMaster
    WHERE IdNo = @FormnoDwn OR convert(varchar,FormNo) = @FormnoDwn;

    SELECT FormnoDwn
    FROM M_MemTreeRelation
    WHERE FormNoDwn = @Isformno
      AND FormNo = @FormNo;
END
";

                        int i = SqlHelper.ExecuteNonQuery(
                            HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(),
                            CommandType.Text,
                            str
                        );
                        //string str = " Create Proc Sp_CheckDownLineMemTree( @FormnoDwn varchar(50), @FormNo int ) " +
                        //             " As Begin declare @Isformno numeric(18,0) ;select @Isformno = formno from m_membermaster where idno = @FormnoDwn ; Select FormnoDwn FROM M_MemTreeRelation " +
                        //             " WHERE FormNoDwn = @Isformno AND FormNo = @FormNo End ";
                        //str += " Go ";

                        //int i = SqlHelper.ExecuteNonQuery(
                        //            HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(),
                        //            CommandType.Text,
                        //            str
                        //        );
                    }
                    catch (Exception)
                    {
                        // ignore error if proc already exists
                    }

                    // ========== Create Proc SP_MemberMaster ==========
                    try
                    {
                        string str1 = " Create Proc SP_MemberMaster ( @SelectCoulumn nvarchar(max), @WhereCondtion nvarchar(max) ) " +
                                      " As Begin DECLARE @sqlText nvarchar(max) " +
                                      " SET @sqlText = N'Select ' + @SelectCoulumn + ' from M_MemberMAster Where '  + @WhereCondtion " +
                                      " Exec (@sqlText) End ";
                        str1 += " Go ";

                        int y = SqlHelper.ExecuteNonQuery(
                                    HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(),
                                    CommandType.Text,
                                    str1
                                );
                    }
                    catch (Exception)
                    {
                        // ignore error if proc already exists
                    }



                }
                ValidateTree();
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

            obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void ValidateTree()
    {
        try
        {
            string strSelectedFormNo;

            // Set minDeptLevel
            if (Session["CompId"] != null &&
               (Session["CompId"].ToString() == "1033" || Session["CompId"].ToString() == "1074"))
            {
                minDeptLevel = 3;
            }
            else if (Session["CompId"] != null && Session["CompId"].ToString() == "1101")
            {
                minDeptLevel = 2;
            }
            else
            {
                minDeptLevel = 5;
            }
            string sessionFormNo = Convert.ToString(Session["FormNO"]);
            string reqFormNo = Convert.ToString(Request["DownLineFormNo"]);
            // 1️⃣ First Condition
            if ((sessionFormNo == "" || string.IsNullOrEmpty(reqFormNo)) || (Convert.ToString(Session["FormNo"]) == reqFormNo))
            {
                strSelectedFormNo = Convert.ToString(Session["FORMNO"]);
                BtnStepAbove.Enabled = false;
            }
            // 2️⃣ Second Condition
            else if (Convert.ToString(Session["MemUpliner"]) != Convert.ToString(Session["Upliner"]))
            {
                if (!CheckDownLineMemberTree())
                {
                    Response.Write("Please Check DownLine Member ID");
                    Response.End();
                    return;
                }
                else
                {
                    strSelectedFormNo = reqFormNo;
                    BtnStepAbove.Enabled = true;
                }
            }
            // 3️⃣ Third Condition
            //else if (Session["Formno"].ToString() == Session["Upliner"].ToString() || Session["MemUpliner"].ToString() == Session["Upliner"].ToString())
            //{
            //    BtnStepAbove.Enabled = false;
            //    Response.Write("Sorry!! You can't see your upliner tree.");
            //    Response.End();
            //    return;
            //}
            // 4️⃣ Default else
            else
            {
                if (!CheckDownLineMemberTree())
                {
                    Response.Write("Please Check DownLine Member ID");
                    Response.End();
                    return;
                }

                strSelectedFormNo = reqFormNo;
            }

            // Build Query
            strQuery = getQuery(strSelectedFormNo, minDeptLevel);

            // Generate Tree
            GenerateTree(strQuery);
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }

    protected void cmdBack_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(Convert.ToString(Session["IndexPage"]));
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff") + Environment.NewLine;
            obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private bool CheckDownLineMemberTree()
    {
        try
        {
            bool CheckDownLineMemberTree = false;

            // Build SQL
            string strQuery = "";
            strQuery = "exec Sp_CheckDownLineMemTree '" + Request["DownLineFormNo"] + "','" + Session["FORMNO"] + "' ";
            DataTable dt = new DataTable();
            dt = obj.GetData(strQuery);

            if (dt.Rows.Count <= 0)
                CheckDownLineMemberTree = false;
            else
                CheckDownLineMemberTree = true;

            return CheckDownLineMemberTree;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            return false;
        }
    }

    private string ToolTipTable()
    {
        string strToolTip = "";
        return strToolTip;
    }
    private string getQuery(string strSelectedFormNo, int minDeptLevel)
    {
        try
        {
            return $"exec sp_ShowTree {strSelectedFormNo},{minDeptLevel}";
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff") + Environment.NewLine;
            obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            return "";
        }
    }
    protected void BtnStepAbove_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["Upliner"] != null && Session["Upliner"].ToString() != "0" && Session["MemUpliner"].ToString() != Session["Upliner"].ToString())
            {
                string uplnformno = Session["Upliner"].ToString();
                Response.Redirect("NewTree.aspx?DownLineFormNo=" + uplnformno);
            }
            else if (Session["MemUpliner"].ToString() == Session["Upliner"].ToString())
            {
                Response.Write("Sorry!! You can't see your upliner tree.");
                Response.End();
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            string scrname = "";
            string DownFormNo = get_FormNo(DownLineFormNo.Value);
            //string DownFormNo = DownLineFormNo.Value;
            if (!string.IsNullOrEmpty(DownFormNo))
            {
                Response.Redirect("NewTree.aspx?DownLineFormNo=" + DownFormNo);
            }
            else
            {
                scrname = "<script language='javascript'>alert('Invalid distributor id');</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }

    private string get_FormNo(string IDNo)
    {
        try
        {
            string FormNo = "";
            SqlDataReader dr;

            string Str = "SP_MemberMaster 'FormNo','IDNo = ''" + IDNo + "''' ";
            Comm = new SqlCommand(Str, conn);
            dr = Comm.ExecuteReader();

            if (dr.Read())
            {
                FormNo = dr["FormNo"].ToString();
            }

            dr.Close();
            Comm.Cancel();

            return FormNo;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;
            obj.WriteToFile(text + ex.Message);

            Response.Write("Try later.");
            return "";
        }
    }

    private string get_PerentID(string PerentID)
    {
        try
        {
            string FormNo = "";
            SqlDataReader dr;

            string Str = "SP_MemberMaster 'IDno','Formno = ''" + PerentID + "''' ";
            Comm = new SqlCommand(Str, conn);
            dr = Comm.ExecuteReader();

            if (dr.Read())
            {
                FormNo = dr["Idno"].ToString();
            }

            dr.Close();
            Comm.Cancel();

            return FormNo;
        }
        catch
        {
            return "";
        }
    }

    protected void btnhomeTree_Click(object sender, EventArgs e)
    {
        Response.Redirect("Newtree.aspx", false);
    }
    private void GenerateTree(string strQuery)
    {
        try
        {
            Comm = new SqlCommand(strQuery, conn);
            Comm.CommandTimeout = 100000000;
            Adp1 = new SqlDataAdapter(Comm);
            Adp1.Fill(dsGetQry);
            double ParentId = -1;
            string ParentIdNo = "";
            double FormNo = 0;
            string MemberName = "";
            string LegNo = "";
            string Todayleftunit = "";
            string Todayrightunit = "";
            string Doj = "";
            string Category = "";
            double LeftBV = 0, RightBV = 0;
            double LeftPV = 0, RightPV = 0;
            double LeftJoining = 0, RightJoining = 0;
            string UpLiner = "", Sponsor = "";
            int level = 0;
            string NodeName = "", IdNo = "";
            string myRunTimeString = "";
            string ExpandYesNo = "false";
            string strImageFile = "";
            string strUrlPath = "";
            string UpDt = "";
            int ActiveDirect = 0, ActiveIndirect = 0, TodayLeftactive = 0, TodayRightActive = 0, TodayLeftJoin = 0, TodayRightJoin = 0;
            double LeftCarryBv = 0, RightCarryBv = 0;
            double groupcRightJoining = 0, groupcLeftCarryForwardBv = 0;
            double groupcLeftActive = 0, groupcRightBv = 0;
            string tooltipstrig = "";
            string Block = "";
            string BlockStatus = "";
            string Iscompany = "";
            string Iscompanystatus = "";
            string Target_ = "_self";
            double CurrentMonLeftRBV = 0, CurrentMonRightRbv = 0;
            double PerviousMonLeftRBV = 0, PerviousMonRightRbv = 0;
            double AmmLeftRBV = 0, AmRightRbv = 0;
            double SelftPv = 0;
            double LeftDirect = 0, RightDirect = 0;
            string ColName1 = Convert.ToString(Session["ColName1"] ?? "");
            string yesshow = "Y";
            // Build JS
            var sb = new System.Text.StringBuilder();
            sb.Append("<script language=\"javascript\">" + Environment.NewLine);

            tooltipstrig = ToolTipTable(); // assumes this returns string as in VB

            //--- Define Parent Setting ----------------
            ParentId = -1;

            if (!string.IsNullOrEmpty(Request.QueryString["DownLineFormNo"]))
            {
                FormNo = Convert.ToDouble(Request.QueryString["DownLineFormNo"]);
            }
            else
            {
                // Session("FormNo").ToString in VB
                FormNo = Session["FormNo"] != null ? Convert.ToDouble(Session["FormNo"]) : 0;
            }

            strImageFile = "img/base.jpg";
            int i = 0;
            int LoopValue = 0;
            string FolderFile = "img/Deactivate.jpg";
            int k = 0;

            // iterate rows
            if (dsGetQry != null && dsGetQry.Tables.Count > 0)
            {
                foreach (DataRow dr in dsGetQry.Tables[0].Rows)
                {
                    strImageFile = Convert.ToString(dr["JoinColor"] ?? "");

                    if (i == 0)
                    {
                        if (!string.IsNullOrEmpty(Request.QueryString["DownLineFormNo"]) && i == 0)
                        {
                            Session["Upliner"] = Convert.ToString(dr["uplinerformno"] ?? "");
                        }
                        else
                        {
                            Session["Upliner"] = null;
                        }

                        sb.Append("mytree = new dTree('mytree','" + strImageFile + "');" + Environment.NewLine);
                        i++;
                    }

                    k++;
                    ParentId = dr["UPLNFORMNO"] != DBNull.Value ? Convert.ToDouble(dr["UPLNFORMNO"]) : -1;
                    ParentIdNo = get_PerentID(Convert.ToString(ParentId));
                    FormNo = dr["FormNoDwn"] != DBNull.Value ? Convert.ToDouble(dr["FormNoDwn"]) : 0;
                    LegNo = Convert.ToString(dr["legno"] ?? "");

                    if (Convert.ToString(Session["CompID"]) == "1007")
                    {
                        UpLiner = Convert.ToString(dr["Rank"] ?? "");
                    }
                    else
                    {
                        UpLiner = Convert.ToString(dr["UpLiner"] ?? "");
                    }

                    string dbFormNo = Convert.ToString(dr["Formno"]).Replace("DV", "");

                    //if (Convert.ToString(FormNo) == dbFormNo)
                    //{
                    //    yesshow = "N";
                    //}
                    //else
                    //{
                    //    yesshow = "Y";
                    //}

                    //FormNo = dr["FormNoDwn"] != DBNull.Value ? Convert.ToDouble(dr["FormNoDwn"]) : 0;
                    Sponsor = Convert.ToString(dr["Sponsor"] ?? "");
                    Doj = Convert.ToString(dr["doj"] ?? "");
                    Category = Convert.ToString(dr["Category"] ?? "");

                    LeftBV = dr["LeftBV"] != DBNull.Value ? Convert.ToDouble(dr["LeftBV"]) : 0;
                    RightBV = dr["rightBV"] != DBNull.Value ? Convert.ToDouble(dr["rightBV"]) : 0;

                    if (Session["compid"].ToString() == "1109")
                    {
                        groupcRightBv = dr["groupcRightBv"] != DBNull.Value ? Convert.ToDouble(dr["groupcRightBv"]) : 0;
                        groupcRightJoining = dr["groupcRightJoining"] != DBNull.Value ? Convert.ToDouble(dr["groupcRightJoining"]) : 0;
                        groupcLeftActive = dr["groupcLeftActive"] != DBNull.Value ? Convert.ToInt32(dr["groupcLeftActive"]) : 0;
                        groupcLeftCarryForwardBv = dr["groupcLeftCarryForwardBv"] != DBNull.Value ? Convert.ToDouble(dr["groupcLeftCarryForwardBv"]) : 0;
                    }
                    LeftJoining = dr["Leftjoining"] != DBNull.Value ? Convert.ToDouble(dr["Leftjoining"]) : 0;
                    RightJoining = dr["Rightjoining"] != DBNull.Value ? Convert.ToDouble(dr["Rightjoining"]) : 0;
                    

                    ActiveDirect = dr["LeftActive"] != DBNull.Value ? Convert.ToInt32(dr["LeftActive"]) : 0;
                    ActiveIndirect = dr["RightActive"] != DBNull.Value ? Convert.ToInt32(dr["RightActive"]) : 0;
                   

                    IdNo = Convert.ToString(dr["Formno"] ?? "") + "<br/>(" + Convert.ToString(dr["memName"] ?? "") + ")";

                    LeftCarryBv = dr["LeftCarryForwardBv"] != DBNull.Value ? Convert.ToDouble(dr["LeftCarryForwardBv"]) : 0;
                    RightCarryBv = dr["RightCarryForwardBv"] != DBNull.Value ? Convert.ToDouble(dr["RightCarryForwardBv"]) : 0;
                   

                    Block = Convert.ToString(dr["IsBlock"] ?? "");
                    BlockStatus = Convert.ToString(dr["BlockedStatus"] ?? "");
                    level = dr["level"] != DBNull.Value ? Convert.ToInt32(dr["level"]) : 0;
                    strUrlPath = "NewJoining.aspx?DownLineFormNo=" + FormNo.ToString();
                    Todayleftunit = Convert.ToString(dr["Todayleftunit"] ?? "");
                    Todayrightunit = Convert.ToString(dr["Todayrightunit"] ?? "");
                    TodayLeftactive = dr["TodayLeftActive"] != DBNull.Value ? Convert.ToInt32(dr["TodayLeftActive"]) : 0;
                    TodayRightActive = dr["TodayRightActive"] != DBNull.Value ? Convert.ToInt32(dr["TodayRightActive"]) : 0;
                    TodayLeftJoin = dr["TodayLeftJoin"] != DBNull.Value ? Convert.ToInt32(dr["TodayLeftJoin"]) : 0;
                    TodayRightJoin = dr["TodayRightJoin"] != DBNull.Value ? Convert.ToInt32(dr["TodayRightJoin"]) : 0;
                    // ColName1 = Session["ColName1"].ToString();
                    UpDt = Convert.ToString(dr["UpDt"] ?? "");
                    MemberName = Convert.ToString(dr["Formno"] ?? "");
                    LoopValue = dr["mlevel"] != DBNull.Value ? Convert.ToInt32(dr["mlevel"]) : 0;

                    if (LoopValue < 6 && LoopValue > 0)
                    {
                        ExpandYesNo = "true";
                    }
                    else
                    {
                        ExpandYesNo = "false";
                    }
                    if (Session["compid"].ToString() == "1108" || Session["compid"].ToString() == "1110")
                    {
                        int currentFormNo = dr["FormNoDwn"] != DBNull.Value ? Convert.ToInt32(dr["FormNoDwn"]) : 0;
                        Sponsor = dr["Sponsor"] != DBNull.Value ? dr["Sponsor"].ToString() : "";
                        // 🔥 Force override
                        if (currentFormNo == 337229 || currentFormNo == 611713)
                        {
                            Sponsor = "(DV123456)DV9 WELLNESS";
                        }
                    }
                    if (ParentId == -1)
                    {
                        ExpandYesNo = "true";
                    }
                    if (UpDt == "01 Jan 00")
                    {
                        UpDt = "";
                    }

                    if (FormNo <= 0)
                    {
                        Target_ = "_blank";
                        MemberName = "Direct";
                        strUrlPath = "";

                        if (Convert.ToString(Session["compid"]) == "1101")
                        {
                            MemberName = "Join Now";
                            // Crypto.Encrypt usage preserved; make sure Crypto class exists in C#.
                            strUrlPath = Session["JoinPage"] + "?ref=" + Crypto.Encrypt(ParentIdNo + "/" + LegNo + "/" + MemberName) + "&side=" + MemberName;
                        }
                        else if (Convert.ToString(Session["compid"]) == "1109")
                        {
                            MemberName = (LegNo == "1") ? "Group A" : (LegNo == "2") ? "Group B" : "Group C";
                            // Crypto.Encrypt usage preserved; make sure Crypto class exists in C#.
                            strUrlPath = Session["JoinPage"] + "?ref=" + Crypto.Encrypt(ParentIdNo + "/" + LegNo + "/" + MemberName) + "&side=" + MemberName;
                        }
                        else
                        {
                            strUrlPath = Session["JoinPage"] + "?ref=" + Crypto.Encrypt(ParentIdNo + "/" + LegNo) + "&side=" + MemberName;
                            MemberName = (LegNo == "1") ? "Left" : "Right";
                        }
                    }
                    else
                    {
                        string activeStatus = Convert.ToString(dr["ActiveStatus"] ?? "");
                        int kitid = dr["Kitid"] != DBNull.Value ? Convert.ToInt32(dr["Kitid"]) : 0;

                        if (activeStatus == "N")
                        {
                            strImageFile = "img/deact.jpg";
                        }
                        else if (kitid == 1)
                        {
                            strImageFile = "img/Red.png";
                        }
                        else if (kitid == 2)
                        {
                            strImageFile = "img/Blue.png";
                        }
                        else if (kitid == 3)
                        {
                            strImageFile = "img/Green.png";
                        }
                        else if (kitid == 4)
                        {
                            strImageFile = "img/Yellow.jpg";
                        }
                        else if (kitid == 5)
                        {
                            strImageFile = "img/Orange.jpg";
                        }
                        else if (kitid == 6)
                        {
                            strImageFile = "img/purpel.jpg";
                        }
                        else
                        {
                            strImageFile = "img/empty.png";
                        }

                        Target_ = "";
                        strUrlPath = "newtree.aspx?DownLineFormNo=" + FormNo.ToString();
                    }

                    // override with JoinColor again (matching VB line)
                    strImageFile = Convert.ToString(dr["JoinColor"] ?? "");

                    // Special CompID logic
                    if (Convert.ToString(Session["CompID"]) == "1010" || Convert.ToString(Session["CompID"]) == "1033")
                    {
                        DataSet Ds = new DataSet();
                        SqlParameter[] prms = new SqlParameter[1];
                        prms[0] = new SqlParameter("@FormNo", Convert.ToInt32(FormNo));
                        Ds = SqlHelper.ExecuteDataset(Convert.ToString(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]]), "Proc_MyBVBusiness", prms);

                        if (Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
                        {
                            CurrentMonLeftRBV = Ds.Tables[0].Rows[0]["CWBVL"] != DBNull.Value ? Convert.ToDouble(Ds.Tables[0].Rows[0]["CWBVL"]) : 0;
                            CurrentMonRightRbv = Ds.Tables[0].Rows[0]["CWBVR"] != DBNull.Value ? Convert.ToDouble(Ds.Tables[0].Rows[0]["CWBVR"]) : 0;
                        }
                        else
                        {
                            CurrentMonLeftRBV = 0;
                            CurrentMonRightRbv = 0;
                        }

                        if (Ds.Tables.Count > 1 && Ds.Tables[1].Rows.Count > 0)
                        {
                            PerviousMonLeftRBV = Ds.Tables[1].Rows[0]["PWBVL"] != DBNull.Value ? Convert.ToDouble(Ds.Tables[1].Rows[0]["PWBVL"]) : 0;
                            PerviousMonRightRbv = Ds.Tables[1].Rows[0]["PWBVR"] != DBNull.Value ? Convert.ToDouble(Ds.Tables[1].Rows[0]["PWBVR"]) : 0;
                        }
                        else
                        {
                            PerviousMonLeftRBV = 0;
                            PerviousMonRightRbv = 0;
                        }

                        AmmLeftRBV = Convert.ToDouble(CurrentMonLeftRBV) + Convert.ToDouble(PerviousMonLeftRBV);
                        AmRightRbv = Convert.ToDouble(CurrentMonRightRbv) + Convert.ToDouble(PerviousMonRightRbv);
                    }

                    // Build the mytree.add(...) JavaScript call.
                    // Note: use single quotes inside JS for strings to avoid escaping.

                    if (Convert.ToString(Session["CompID"]) == "1109")
                    {
                        sb.Append(" mytree.add(");
                        sb.Append(FormNo.ToString() + ",");
                        sb.Append(ParentId.ToString() + ",");
                        sb.Append("'" + Category.Replace("'", "\\'") + "',");
                        sb.Append("'" + Doj.Replace("'", "\\'") + "',");
                        sb.Append("'" + MemberName.Replace("'", "\\'") + "',");
                        sb.Append("'" + NodeName.Replace("'", "\\'") + "',");
                        sb.Append("'" + UpLiner.Replace("'", "\\'") + "',");
                        sb.Append("'" + Sponsor.Replace("'", "\\'") + "',");
                        sb.Append(LeftBV.ToString() + ",");
                        sb.Append(RightBV.ToString() + ",");
                        sb.Append("'" + Todayleftunit.Replace("'", "\\'") + "',");
                        sb.Append("'" + Todayrightunit.Replace("'", "\\'") + "',");
                        sb.Append("'" + ActiveDirect + "',");
                        sb.Append("'" + ActiveIndirect + "',");
                        sb.Append("'" + strUrlPath.Replace("'", "\\'") + "',");
                        sb.Append("'" + MemberName.Replace("'", "\\'") + "',");
                        sb.Append("'" + Target_.Replace("'", "\\'") + "',");
                        sb.Append("'" + strImageFile.Replace("'", "\\'") + "',");
                        sb.Append("'" + strImageFile.Replace("'", "\\'") + "',");
                        sb.Append(ExpandYesNo + ",");
                        sb.Append("'" + LeftJoining + "',");
                        sb.Append("'" + RightJoining + "',");
                        sb.Append("'" + level + "',");
                        sb.Append("'" + UpDt.Replace("'", "\\'") + "',");
                        sb.Append("'" + IdNo.Replace("'", "\\'") + "',");
                        sb.Append("'" + LeftCarryBv + "',");
                        sb.Append("'" + RightCarryBv + "',");
                        sb.Append("'" + Block.Replace("'", "\\'") + "',");
                        sb.Append("'" + BlockStatus.Replace("'", "\\'") + "',");
                        sb.Append("'" + TodayLeftactive + "',");
                        sb.Append("'" + TodayRightActive + "',");
                        sb.Append("'" + TodayLeftJoin + "',");
                        sb.Append("'" + TodayRightJoin + "',");
                        sb.Append("'" + CurrentMonLeftRBV + "',");
                        sb.Append("'" + CurrentMonRightRbv + "',");
                        sb.Append("'" + PerviousMonLeftRBV + "',");
                        sb.Append("'" + PerviousMonRightRbv + "',");
                        sb.Append("'" + AmmLeftRBV + "',");
                        sb.Append("'" + AmRightRbv + "',");
                        sb.Append("'" + groupcRightJoining + "',");
                        sb.Append("'" + groupcRightBv + "',");
                        sb.Append("'" + groupcLeftActive + "',");
                        sb.Append("'" + groupcLeftCarryForwardBv + "',");
                        sb.Append("'" + SelftPv + "',");
                        sb.Append("'" + ColName1.Replace("'", "\\'") + "',");
                        sb.Append("'" + LeftPV + "',");
                        sb.Append("'" + RightPV + "',");
                        sb.Append("'" + yesshow + "');" + Environment.NewLine);

                    }
                    else
                    {
                        sb.Append(" mytree.add(");
                        sb.Append(FormNo.ToString() + ",");
                        sb.Append(ParentId.ToString() + ",");
                        sb.Append("'" + Category.Replace("'", "\\'") + "',");
                        sb.Append("'" + Doj.Replace("'", "\\'") + "',");
                        sb.Append("'" + MemberName.Replace("'", "\\'") + "',");
                        sb.Append("'" + NodeName.Replace("'", "\\'") + "',");
                        sb.Append("'" + UpLiner.Replace("'", "\\'") + "',");
                        sb.Append("'" + Sponsor.Replace("'", "\\'") + "',");
                        sb.Append(LeftBV.ToString() + ",");
                        sb.Append(RightBV.ToString() + ",");
                        sb.Append("'" + Todayleftunit.Replace("'", "\\'") + "',");
                        sb.Append("'" + Todayrightunit.Replace("'", "\\'") + "',");
                        sb.Append("'" + ActiveDirect + "',");
                        sb.Append("'" + ActiveIndirect + "',");
                        sb.Append("'" + strUrlPath.Replace("'", "\\'") + "',");
                        sb.Append("'" + MemberName.Replace("'", "\\'") + "',");
                        sb.Append("'" + Target_.Replace("'", "\\'") + "',");
                        sb.Append("'" + strImageFile.Replace("'", "\\'") + "',");
                        sb.Append("'" + strImageFile.Replace("'", "\\'") + "',");
                        sb.Append(ExpandYesNo + ",");
                        sb.Append("'" + LeftJoining + "',");
                        sb.Append("'" + RightJoining + "',");
                        sb.Append("'" + level + "',");
                        sb.Append("'" + UpDt.Replace("'", "\\'") + "',");
                        sb.Append("'" + IdNo.Replace("'", "\\'") + "',");
                        sb.Append("'" + LeftCarryBv + "',");
                        sb.Append("'" + RightCarryBv + "',");
                        sb.Append("'" + Block.Replace("'", "\\'") + "',");
                        sb.Append("'" + BlockStatus.Replace("'", "\\'") + "',");
                        sb.Append("'" + TodayLeftactive + "',");
                        sb.Append("'" + TodayRightActive + "',");
                        sb.Append("'" + TodayLeftJoin + "',");
                        sb.Append("'" + TodayRightJoin + "',");
                        sb.Append("'" + CurrentMonLeftRBV + "',");
                        sb.Append("'" + CurrentMonRightRbv + "',");
                        sb.Append("'" + PerviousMonLeftRBV + "',");
                        sb.Append("'" + PerviousMonRightRbv + "',");
                        sb.Append("'" + AmmLeftRBV + "',");
                        sb.Append("'" + AmRightRbv + "',");
                        sb.Append("'" + SelftPv + "',");
                        sb.Append("'" + ColName1.Replace("'", "\\'") + "',");
                        sb.Append("'" + LeftPV + "',");
                        sb.Append("'" + RightPV + "',");
                        sb.Append("'" + yesshow + "');" + Environment.NewLine);

                    }
                } // foreach
            } // if ds tables

            sb.Append(Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine);
            sb.Append(" document.write(mytree);" + Environment.NewLine);
            sb.Append(Environment.NewLine + "</script> " + "<br /> <br /> <br /> <br /> ");

            // Register client script block (use ClientScript to register properly)
            ClientScript.RegisterClientScriptBlock(this.GetType(), "clientScript", sb.ToString());

        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    public bool CreateOrAlter_GetExtreme_SP()
    {
        try
        {
            string strSql = "CREATE OR ALTER PROCEDURE dbo.GetExtreme_SP " + "( " + "   @FormNo NUMERIC(18,0), " + "   @LegNo  NUMERIC(18,0) " + ") " + "AS " + "BEGIN " + "   SET NOCOUNT ON; " + "   BEGIN TRY " + "  " +
                "     IF OBJECT_ID('tempdb..#toIns') IS NOT NULL DROP TABLE #toIns; " + "       CREATE TABLE #toIns ( " + "           FormNo NUMERIC(18,0), " + "      " +
                "     FormNoDwn NUMERIC(18,0), " + "           LegNo NUMERIC(18,0), " + "           MLevel INT " + "       ); " + "    " +
                "   CREATE CLUSTERED INDEX IX_toIns_1 ON #toIns (FormNoDwn, LegNo, MLevel); " + "      " +
                " INSERT INTO #toIns (FormNo, FormNoDwn, LegNo, MLevel) " + "      " +
                " SELECT a.FormNo, a.FormNoDwn, a.LegNo, a.MLevel " + "       " +
                "FROM " + obj.dBName + "..M_MemTreeRelation a WITH (NOLOCK) " + "      " +
                " WHERE a.FormNo = @FormNo; " + "       DECLARE @GetId NUMERIC(18,0); " + "      " +
                " SELECT TOP (1) @GetId = A.FormNoDwn " + "       FROM ( " + "         " +
                "  SELECT mt.FormNoDwn, mt.LegNo " + "           FROM " + obj.dBName + "..M_MemTreeRelation mt WITH (NOLOCK) " + "        " +
                "   INNER JOIN (SELECT FormNoDwn FROM #toIns WHERE LegNo = @LegNo) D1 " + "              " +
                " ON mt.FormNoDwn = D1.FormNoDwn " + "           INNER JOIN (SELECT FormNoDwn FROM #toIns) D2 " + "           " +
                "    ON mt.FormNo = D2.FormNoDwn " + "           GROUP BY mt.FormNoDwn, mt.LegNo " + "       ) A " + "    " +
                "   INNER JOIN #toIns B ON A.FormNoDwn = B.FormNoDwn " + "       GROUP BY A.FormNoDwn, B.MLevel " + "  " +
                "     HAVING MAX(A.LegNo) = @LegNo AND MIN(A.LegNo) = @LegNo " + "       ORDER BY B.MLevel DESC; " + "  " +
                "     IF @GetId IS NULL " + "       BEGIN " + "           SELECT @GetId = FormNo " + "         " +
                "  FROM " + obj.dBName + "..M_MemberMaster WITH (NOLOCK) " + "          " +
                " WHERE UpLnFormNo = @FormNo AND LegNo = @LegNo; " + "       END " + "      " +
                " IF @GetId IS NULL SET @GetId = @FormNo; " + "       " +
                "SELECT @GetId AS GetId; " + "   END TRY " + "   BEGIN CATCH THROW; END CATCH " + "END";
            int isOk;
            isOk = Convert.ToInt32(SqlHelper.ExecuteNonQuery(Convert.ToString(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]]), CommandType.Text, strSql));
            if (isOk > 0)
                return true;
            else
                return false;
        }

        catch (Exception ex)
        {
            throw new Exception("CreateOrAlter_GetExtreme_SP Error : " + ex.Message);
        }
    }
    protected void BtnExtremeLeft_Click(object sender, EventArgs e)
    {
        CreateOrAlter_GetExtreme_SP();
        string query = obj.IsoStart + "EXEC dbo.GetExtreme_SP '" + Session["formno"] + "',1;" + obj.IsoEnd;
        DataTable dt;
        dt = new DataTable();
        dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, query).Tables[0];
        if (dt.Rows.Count > 0)
            Response.Redirect("newtree.aspx?DownLineFormNo=" + dt.Rows[0]["GetId"]);
    }
    protected void BtnExtremeRight_Click(object sender, EventArgs e)
    {
        CreateOrAlter_GetExtreme_SP();
        string query = obj.IsoStart + "EXEC dbo.GetExtreme_SP '" + Session["formno"] + "',2;" + obj.IsoEnd;
        DataTable dt;
        dt = new DataTable();
        dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, query).Tables[0];
        if (dt.Rows.Count > 0)
            Response.Redirect("newtree.aspx?DownLineFormNo=" + dt.Rows[0]["GetId"]);
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        CreateOrAlter_GetExtreme_SP();
        string query = obj.IsoStart + "EXEC dbo.GetExtreme_SP '" + Session["formno"] + "',3;" + obj.IsoEnd;
        DataTable dt;
        dt = new DataTable();
        dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, query).Tables[0];
        if (dt.Rows.Count > 0)
            Response.Redirect("newtree.aspx?DownLineFormNo=" + dt.Rows[0]["GetId"]);
    }
}
