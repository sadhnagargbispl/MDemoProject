using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
public partial class Fundwithdraw : System.Web.UI.Page
{
    SqlConnection Conn;
    SqlCommand Comm;
    SqlDataAdapter Adp;
    SqlDataReader Dr;
    string Query;
    int BankID = 0;
    string BranchName = "";
    string PayName = "";
    string IFSCode = "";
    string Acno = "";
    string PanNo = "";

    clsGeneral objGen = new clsGeneral();
    DAL ObjDAL;
    DAL Obj;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // ---------- Create table if not exists (wrapped in TRY-CATCH) ----------

            // ---------- Session validation ----------
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                try
                {
                    string str = @"
                EXEC('
                CREATE TABLE Trnfundwithdrawcpanel 
                (
                    [ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
                    [Transid] [numeric](18, 0) NOT NULL,
                    [Rectimestamp] [datetime] NOT NULL,
                    PRIMARY KEY CLUSTERED ([Transid] ASC)
                    WITH (
                        PAD_INDEX = OFF,
                        STATISTICS_NORECOMPUTE = OFF,
                        IGNORE_DUP_KEY = OFF,
                        ALLOW_ROW_LOCKS = ON,
                        ALLOW_PAGE_LOCKS = ON
                    )
                ) ON [PRIMARY];

                ALTER TABLE [dbo].[Trnfundwithdrawcpanel]
                ADD DEFAULT (GETDATE()) FOR [Rectimestamp];
                ')";

                    SqlHelper.ExecuteNonQuery(
                        (string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]],
                        CommandType.Text,
                        str
                    );
                }
                catch
                {
                    // Ignore if table already exists
                }

                // ---------- Disable buttons on click ----------
                BtnSubmit.Attributes.Add("onclick", DisableTheButton(this.Page, BtnSubmit));
                BtnPassword.Attributes.Add("onclick", DisableTheButton(this.Page, BtnPassword));
                Obj = new DAL();
                if (!Page.IsPostBack)
                {
                    HdnCheckTrnns.Value = GenerateRandomStringactive(6);
                    Session["CkyPinTransfer1"] = null;
                    hdnSessn.Value = Crypto.Encrypt(Convert.ToString(Session["IDNo"]));

                    string today = DateTime.Now.DayOfWeek.ToString();

                    // ---------- CompID = 1060 ----------
                    if (Convert.ToString(Session["CompID"]) == "1060")
                    {
                        if (!GetReqStatus())
                        {
                            ScriptManager.RegisterStartupScript(
                                this,
                                this.GetType(),
                                "Key",
                                "alert('Request already Sent today please try Again tomorrow.!!');location.replace('LifeHome.aspx');",
                                true
                            );
                            return;
                        }

                        if (!GetReqAproveStatuss())
                        {
                            ScriptManager.RegisterStartupScript(
                                this,
                                this.GetType(),
                                "Key",
                                "alert('You have reached today maximum withdrawal limit. Please try again tomorrow.!!');location.replace('LifeHome.aspx');",
                                true
                            );
                            return;
                        }
                    }

                    // ---------- CompID = 1095 / 1097 / 1100 / 1103 ----------
                    string compId = Convert.ToString(Session["CompID"]);
                    if (compId == "1105")
                    {
                        divadmin.Visible = true;
                    }
                    else
                    {
                        divadmin.Visible = false;
                    }
                    if (compId == "1095" || compId == "1097" || compId == "1100" || compId == "1103")
                    {
                        if (!GetKYCStatus())
                        {
                            ScriptManager.RegisterStartupScript(
                                this,
                                this.GetType(),
                                "Key",
                                "alert('Action denied! KYC not approved. Please approve KYC before continuing.!');location.replace('"
                                + Session["IndexPage"] + "');",
                                true
                            );
                            return;
                        }

                        if (!GetReqStatus())
                        {
                            ScriptManager.RegisterStartupScript(
                                this,
                                this.GetType(),
                                "Key",
                                "alert('Request already Sent today please try Again tomorrow.!!');location.replace('"
                                + Session["IndexPage"] + "');",
                                true
                            );
                            return;
                        }
                    }

                    // ---------- Load data ----------
                    FillDetail();
                    TxtCredit.Text = Amount().ToString();
                }
            }
            else
            {
                Response.Redirect("Logout.aspx");
                return;
            }
            // ---------- First page load ----------
          
        }
        catch
        {
            // Optional: log exception
        }
    }
    private void FillDetail()
    {
        DataTable tmpTable = new DataTable();

        string connStr = Convert.ToString(
            HttpContext.Current.Session["MlmDatabase" + Session["CompID"]]
        );

        using (Conn = new SqlConnection(connStr))
        {
            using (Comm = new SqlCommand())
            {
                Comm.Connection = Conn;
                Comm.CommandText = @"
                SELECT 
                    1 AS Sno,
                    a.BankId,
                    b.BANKNAME,
                    a.AcNo,
                    a.IFSCode,
                    a.MemFirstName AS PayeeName,
                    a.BranchName,
                    a.PanNo
                FROM M_MemberMaster a
                INNER JOIN M_BankMaster b 
                    ON a.BankId = b.BankCode
                WHERE a.FormNo = @FormNo";

                Comm.Parameters.AddWithValue("@FormNo", Session["FormNo"]);

                Adp = new SqlDataAdapter(Comm);
                Adp.Fill(tmpTable);
            }
        }

        GrdBankDetail.DataSource = tmpTable;
        GrdBankDetail.DataBind();
    }
    private string CheckAcountDetail()
    {
        string RtrVal = "False";
        DataTable tmpTable = new DataTable();

        string connStr = Convert.ToString(
            HttpContext.Current.Session["MlmDatabase" + Session["CompID"]]
        );

        using (SqlConnection Conn = new SqlConnection(connStr))
        {
            using (SqlCommand Comm = new SqlCommand())
            {
                Comm.Connection = Conn;
                Comm.CommandText = @"
                SELECT 
                    1 AS Sno,
                    a.BankId,
                    b.BANKNAME,
                    a.AcNo,
                    a.IFSCode,
                    a.MemFirstName AS PayeeName,
                    a.BranchName,
                    a.PanNo
                FROM M_MemberMaster a
                INNER JOIN M_BankMaster b 
                    ON a.BankId = b.BankCode
                WHERE a.FormNo = @FormNo";

                Comm.Parameters.AddWithValue("@FormNo", Session["FormNo"]);

                SqlDataAdapter Adp = new SqlDataAdapter(Comm);
                Adp.Fill(tmpTable);
            }
        }

        if (tmpTable.Rows.Count > 0)
        {
            DataRow row = tmpTable.Rows[0];

            if (
                row["BankId"].ToString() == "0" ||
                row["BANKNAME"].ToString() == "--Choose Bank Name--" ||
                row["AcNo"].ToString() == "0" ||
                row["IFSCode"].ToString() == ""
            )
            {
                RtrVal = "False";
            }
            else
            {
                RtrVal = "True";
            }
        }

        return RtrVal;
    }
    private bool getWithdrawlLimit()
    {
        try
        {
            string connStr = Convert.ToString(
                HttpContext.Current.Session["MlmDatabase" + Session["CompID"]]
            );

            string sql = @"
            SELECT TOP (1) *
            FROM M_withdrawlLimit
            WHERE FormNo = @FormNo 
              AND ActiveStatus = 'Y'
            ORDER BY RID DESC";

            DataTable dtLimit = SqlHelper.ExecuteDataset(
                connStr,
                CommandType.Text,
                sql,
                new SqlParameter("@FormNo", Session["Formno"])
            ).Tables[0];

            if (dtLimit.Rows.Count > 0)
            {
                decimal amountLimit = Convert.ToDecimal(dtLimit.Rows[0]["AMountLimit"]);
                decimal reqAmount = Convert.ToDecimal(TxtReqAmt.Text);

                if (amountLimit >= reqAmount)
                {
                    sql = @"
                    SELECT ISNULL(SUM(ReqAmount), 0) AS ReqAmount
                    FROM Fundwithdrawls
                    WHERE FormNo = @FormNo
                      AND Status <> 'R'
                    GROUP BY FormNo
                    HAVING ISNULL(SUM(ReqAmount), 0) >= @Limit";

                    DataTable dt = SqlHelper.ExecuteDataset(
                        connStr,
                        CommandType.Text,
                        sql,
                        new SqlParameter("@FormNo", Session["Formno"]),
                        new SqlParameter("@Limit", amountLimit)
                    ).Tables[0];

                    if (dt.Rows.Count == 0)
                    {
                        return true;
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(
                            this.Page,
                            this.GetType(),
                            "LimitError",
                            "alert('Withdrawal limit exceeded. You cannot withdraw more than this limit.');",
                            true
                        );
                        return false;
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(
                        this.Page,
                        this.GetType(),
                        "LimitError",
                        $"alert('Withdrawal limit is {amountLimit}. You cannot withdraw more than this limit.');",
                        true
                    );
                    return false;
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
    private bool GetKycPerStatus()
    {
        try
        {
            string connStr = Convert.ToString(
                HttpContext.Current.Session["MlmDatabase" + Session["CompID"]]
            );

            string sql = "EXEC sp_CheckKycStatus @FormNo";

            DataSet ds = SqlHelper.ExecuteDataset(
                connStr,
                CommandType.Text,
                sql,
                new SqlParameter("@FormNo", Convert.ToInt32(Session["Formno"]))
            );

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["Status"]
                         .ToString()
                         .ToUpper() == "TRUE";
            }

            return false;
        }
        catch
        {
            return false;
        }
    }
    private double Amount()
    {
        try
        {
            double RtrVal = 0;

            string connStr = Convert.ToString(
                HttpContext.Current.Session["MlmDatabase" + Session["CompID"]]
            );

            using (SqlConnection Conn = new SqlConnection(connStr))
            {
                using (SqlCommand Comm = new SqlCommand())
                {
                    Comm.Connection = Conn;
                    Comm.CommandText =
                        "SELECT balance FROM dbo.ufnGetBalance(@FormNo, 'M')";

                    Comm.Parameters.AddWithValue("@FormNo", Session["FormNo"]);

                    Conn.Open();

                    using (SqlDataReader Dr = Comm.ExecuteReader())
                    {
                        if (Dr.Read())
                        {
                            RtrVal = Convert.ToDouble(Dr["Balance"]);
                        }
                    }
                }
            }

            return RtrVal;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            return 0;
        }
    }
    private bool GetDateFormatCondition()
    {
        try
        {
            string connStr = Convert.ToString(
                HttpContext.Current.Session["MlmDatabase" + Session["CompID"]]
            );

            string sql = "EXEC Sp_GetDateFormatConditionUpdate";

            DataSet ds = SqlHelper.ExecuteDataset(
                connStr,
                CommandType.Text,
                sql
            );

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(ds.Tables[0].Rows[0]["Status"]) != 0;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }
    public string GenerateRandomStringactive(int iLength)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        StringBuilder result = new StringBuilder();

        for (int i = 0; i < iLength; i++)
        {
            result.Append(allowChrs[rdm.Next(0, allowChrs.Length)]);
        }

        return result.ToString();
    }
    private string DisableTheButton(Control pge, Control btn)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
        sb.Append("this.value = 'Please wait...';");
        sb.Append("this.disabled = true;");
        sb.Append(pge.Page.GetPostBackEventReference(btn));
        sb.Append(";");

        return sb.ToString();
    }
    private bool GetKYCStatus()
    {
        try
        {
            string sql = @"
            SELECT *
            FROM KycVerify a
            INNER JOIN M_MemberMaster b ON a.FormNo = b.FormNo
            WHERE b.AcNo <> ''
              AND b.PanNo <> ''
              AND a.IspanVerified = 'Y'
              AND a.IsBankVerified = 'Y'
              AND a.FormNo = @FormNo";

            DataSet ds = SqlHelper.ExecuteDataset(
                (string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]],
                CommandType.Text,
                sql,
                new SqlParameter("@FormNo", Convert.ToInt32(Session["Formno"]))
            );

            return ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;
        }
        catch
        {
            return false;
        }
    }
    private bool GetidStatus()
    {
        try
        {
            string sql = @"
            SELECT *
            FROM KycVerify a
            INNER JOIN M_MemberMaster b ON a.FormNo = b.FormNo
            WHERE b.ActiveStatus = 'Y'
              AND a.FormNo = @FormNo";

            DataSet ds = SqlHelper.ExecuteDataset(
                (string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]],
                CommandType.Text,
                sql,
                new SqlParameter("@FormNo", Convert.ToInt32(Session["Formno"]))
            );

            return ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;
        }
        catch
        {
            return false;
        }
    }
    protected void BtnPassword_Click(object sender, EventArgs e)
    {
        try
        {
            string scrname = string.Empty;

            string transPassw = TxtPassword.Text.Trim();
            DataTable dt1 = new DataTable();

            DAL objDal = new DAL();

            string sql =
               Obj.IsoStart + "select * from " + Obj.dBName + "..M_MemberMaster " +
                "where Epassw='" + transPassw + "' " +
                "and Formno=" + Session["Formno"] + Obj.IsoEnd;

            dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];

            if (dt1.Rows.Count > 0)
            {
                Session["CkyPinTransfer1"] = dt1.Rows[0]["EPassw"];
                feedbackpop1.Visible = false;
            }
            else
            {
                Session["CkyPinTransfer1"] = null;
                scrname = "<script language='javascript'>alert('Please Enter valid Transaction Password.');</script>";
                ScriptManager.RegisterClientScriptBlock(
                    this.Page,
                    this.GetType(),
                    "Login Error",
                    scrname,
                    false
                );
            }

            if (Session["CkyPinTransfer1"] != null)
            {
                if (!string.IsNullOrEmpty(Session["CkyPinTransfer1"].ToString()))
                {
                    feedbackpop1.Visible = false;
                    FundTransfer();
                    Session["CkyPinTransfer1"] = null;
                    BtnSubmit.Visible = true;
                }
            }
            else
            {
                feedbackpop1.Visible = true;
            }
        }
        catch (Exception)
        {
            // Optional: log exception
        }
    }
    private bool GetReqStatus()
    {
        try
        {
            string sql = "EXEC SP_GetReqStatus @FormNo";

            DataSet ds = SqlHelper.ExecuteDataset(
                (string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]],
                CommandType.Text,
                sql,
                new SqlParameter("@FormNo", Session["FormNo"])
            );

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(ds.Tables[0].Rows[0]["Cnt"]) == 0;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }
    private bool GetReqAproveStatuss()
    {
        try
        {
            string sql = "EXEC SP_GetReqAproveStatus @FormNo";

            DataSet ds = SqlHelper.ExecuteDataset(
                (string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]],
                CommandType.Text,
                sql,
                new SqlParameter("@FormNo", Session["FormNo"])
            );

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(ds.Tables[0].Rows[0]["Cnt"]) == 0;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string compId = Convert.ToString(Session["CompID"]);

            // ---------- CompID = 1060 ----------
            if (compId == "1060")
            {
                if (!GetReqStatus())
                {
                    ScriptManager.RegisterStartupScript(
                        this,
                        this.GetType(),
                        "Key",
                        "alert('Request already Sent today please try Again tomorrow.!!');location.replace('LifeHome.aspx');",
                        true
                    );
                    return;
                }

                if (!GetReqAproveStatuss())
                {
                    ScriptManager.RegisterStartupScript(
                        this,
                        this.GetType(),
                        "Key",
                        "alert('You have reached today maximum withdrawal limit. Please try again tomorrow.!!');location.replace('LifeHome.aspx');",
                        true
                    );
                    return;
                }
            }

            // ---------- KYC mandatory companies ----------
            if (compId == "1095" || compId == "1097" || compId == "1100" || compId == "1103")
            {
                if (!GetKYCStatus())
                {
                    ScriptManager.RegisterStartupScript(
                        this,
                        this.GetType(),
                        "Key",
                        "alert('Action denied! KYC not approved. Please approve KYC before continuing.!');location.replace('" +
                        Session["IndexPage"] + "');",
                        true
                    );
                    return;
                }

                if (!GetReqStatus())
                {
                    ScriptManager.RegisterStartupScript(
                        this,
                        this.GetType(),
                        "Key",
                        "alert('Request already Sent today please try Again tomorrow.!!');location.replace('" +
                        Session["IndexPage"] + "');",
                        true
                    );
                    return;
                }
            }

            Label1.Text = string.Empty;

            // ---------- Extra check for CompID = 1060 ----------
            if (compId == "1060")
            {
                DataTable dt1 = new DataTable();
                DAL objDal = new DAL();

                string sql =
                    "Select Count(*) as cnt, status " +
                    "From Fundwithdrawls " +
                    "Where Formno = '" + Session["FormNo"] + "' " +
                    "And Cast(Getdate() As Date)= Cast(ReqDate As Date) " +
                    "Group by status";

                dt1 = objDal.GetData(sql);

                if (dt1.Rows.Count > 0)
                {
                    if (dt1.Rows[0]["status"].ToString() == "R")
                    {
                        // Same as VB: no action (commented logic kept)
                    }
                }
            }

            // ---------- Account / KYC check ----------
            if (CheckAcountDetail() == "False")
            {
                Label1.Text = "Please update KYC first!!";
                return;
            }

            // ---------- Minimum request amount ----------
            if (compId == "1060")
            {
                if (Convert.ToDecimal(TxtReqAmt.Text) < 300)
                {
                    ScriptManager.RegisterClientScriptBlock(
                        this.Page,
                        this.GetType(),
                        "LoginError",
                        "alert('Invalid Request Amount!! Minimum Request Amount 300/.');",
                        true
                    );
                    return;
                }
            }
            else
            {
                if (Convert.ToDecimal(TxtReqAmt.Text) < 500)
                {
                    Label1.Text = "Invalid Request Amount!!";
                    return;
                }
            }

            // ---------- Balance check ----------
            if (Convert.ToDecimal(TxtReqAmt.Text) > Convert.ToDecimal(Amount()))
            {
                Label1.Text = "Insufficient Balance!!";
                return;
            }

            // ---------- Transaction password validation ----------
            if (Session["CkyPinTransfer1"] != null &&
                !string.IsNullOrEmpty(Session["CkyPinTransfer1"].ToString()))
            {
                feedbackpop1.Visible = false;
            }
            else
            {
                feedbackpop1.Visible = true;
                BtnSubmit.Visible = false;
                TxtReqAmt.Enabled = false;
            }
        }
        catch (Exception)
        {
            // Optional: log exception
        }
    }
    private void FundTransfer()
    {
        try
        {
            ObjDAL = new DAL();

            int updateeffect = 0;

            string strSql2 =
                "Insert into Trnfundwithdrawcpanel (Transid,Rectimestamp) values(" +
                HdnCheckTrnns.Value + ",getdate())";

            updateeffect = ObjDAL.SaveData(strSql2);

            if (updateeffect > 0)
            {
                // ---------- Session validation ----------
                if (Session["Sessncheck"].ToString().ToUpper() !=
                    ("OK" + Crypto.Decrypt(hdnSessn.Value).ToString().ToUpper()))
                {
                    ScriptManager.RegisterStartupScript(
                        this,
                        this.GetType(),
                        "Key",
                        "alert('Session expire, Please Re-Login.!!');location.replace('logout.aspx');",
                        true
                    );
                    return;
                }

                string Sessid = "";

                Conn = new SqlConnection(
                    HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString()
                );
                Conn.Open();

                // ---------- Read bank details ----------
                PayName = ((Label)GrdBankDetail.Items[0].FindControl("lblName")).Text;
                Acno = ((Label)GrdBankDetail.Items[0].FindControl("lblAcNo")).Text;
                BankID = Convert.ToInt32(((Label)GrdBankDetail.Items[0].FindControl("lblBankId")).Text);
                BranchName = ((Label)GrdBankDetail.Items[0].FindControl("lblBranch")).Text;
                IFSCode = ((Label)GrdBankDetail.Items[0].FindControl("lblIFSC")).Text;
                PanNo = ((Label)GrdBankDetail.Items[0].FindControl("lblPanNo")).Text;

                // ---------- 10 minute withdrawal restriction ----------
                string strCheck = "";
                DataTable dtCheck = new DataTable();

                strCheck = " SELECT DATEADD(mi,10,Rectimestamp) as Min,Rectimestamp,Getdate() as CurrentTime " +
                           " FROM Fundwithdrawls " +
                           " where formno = '" + Session["FormNo"] + "' " +
                           " and ReqID=(select Top 1 ReqID from Fundwithdrawls where Formno='" +
                           Session["FormNo"] + "' Order by RectimeStamp Desc) " +
                           " Order by ReqID Desc";

                ObjDAL = new DAL();
                dtCheck = ObjDAL.GetData(strCheck);

                if (dtCheck.Rows.Count > 0)
                {
                    if (Convert.ToDateTime(dtCheck.Rows[0]["Min"]) >
                        Convert.ToDateTime(dtCheck.Rows[0]["CurrentTime"]))
                    {
                        ScriptManager.RegisterStartupScript(
                            this,
                            this.GetType(),
                            "Key",
                            "alert('You can withdraw after 10 min. !');location.replace('RefIndex.aspx');",
                            true
                        );
                        return;
                    }
                }

                // ---------- Get Session ID ----------
                Comm = new SqlCommand(
                    "Select Max(SESSID) AS Sessid from M_SessnMaster order by SessID desc",
                    Conn
                );
                Dr = Comm.ExecuteReader();
                if (Dr.Read())
                {
                    Sessid = Dr["Sessid"].ToString();
                }
                Dr.Close();

                // ---------- Generate ReqID ----------
                DataTable Dt = new DataTable();
                string sql = "Select Isnull(Max(ReqID),5000)+1 as ReqId from Fundwithdrawls";

                DAL Obj = new DAL();
                Dt = Obj.GetData(sql);

                string ReqNo = "";
                if (Dt.Rows.Count > 0)
                {
                    ReqNo = Dt.Rows[0]["ReqId"].ToString();
                }

                string Remark =
                    "Fund Debited Against Bank Withdrawal with req. no." + ReqNo;

                if (Convert.ToDecimal(TxtReqAmt.Text) > Convert.ToDecimal(Amount()))
                {
                    ScriptManager.RegisterClientScriptBlock(
                        this.Page,
                        this.GetType(),
                        "Login Error",
                        "alert('Invalid Request Amount');",
                        true
                    );
                    return;
                }

                // ---------- Insert withdrawal ----------
                if (Session["CompID"].ToString() == "1060")
                {
                    Query =
                        "Insert Into Fundwithdrawls(FormNo,ReqID,ReqAmount,ReqDate,PayeeName,AcNo,BankID,BranchName,IfsCode,PanNo,WSessid)" +
                        " values(" + Session["FormNo"] + ",'" + ReqNo + "'," + TxtReqAmt.Text +
                        ",Replace(Convert(Varchar,GetDate(),106),' ','-'),'" + PayName + "','" +
                        Acno + "'," + BankID + ",'" + BranchName + "','" + IFSCode + "','" +
                        PanNo + "','" + Sessid + "') ";

                    Query +=
                        "INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,VTYpe,SessID,WSessID) " +
                        "SELECT ISNULL(Max(VoucherNo)+1,1001),'" + DateTime.Now.ToString("dd-MMM-yyyy") +
                        "','" + Session["FormNo"] + "','0'," + TxtReqAmt.Text + ",'" + Remark +
                        "','Req/" + ReqNo + "','M','D',convert(varchar,getdate(),112),'" +
                        Session["CurrentSessn"] + "' FROM TrnVoucher;";
                }
                else
                {
                    Query =
                        "Insert Into Fundwithdrawls(FormNo,ReqID,ReqAmount,ReqDate,PayeeName,AcNo,BankID,BranchName,IfsCode,PanNo,WSessid)" +
                        " values(" + Session["FormNo"] + ",'" + ReqNo + "'," + TxtReqAmt.Text +
                        ",Replace(Convert(Varchar,GetDate(),106),' ','-'),'" + PayName + "','" +
                        Acno + "'," + BankID + ",'" + BranchName + "','" + IFSCode + "','" +
                        PanNo + "','" + Sessid + "') ";

                    Query +=
                        "INSERT INTO TrnVoucher(VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,AcType,VTYpe,SessID,WSessID) " +
                        "SELECT ISNULL(Max(VoucherNo)+1,1001),'" + DateTime.Now.ToString("dd-MMM-yyyy") +
                        "','" + Session["FormNo"] + "','0'," + TxtReqAmt.Text + ",'" + Remark +
                        "','Req/" + ReqNo + "','M','D',convert(varchar,getdate(),112),'" +
                        Session["CurrentSessn"] + "' FROM TrnVoucher;";
                }

                Query +=
                    " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,Memberid)Values" +
                    "('" + Session["FormNo"] + "','" + Session["MemName"] +
                    "','Bank Withdrawl','Bank Withdrawl Request'," +
                    "' Bank WithDrawls Request For Req No " + ReqNo +
                    " ',Getdate()," + Session["FormNo"] + ")";

                Conn.Open();
                Comm = new SqlCommand(Query, Conn);
                Comm.ExecuteNonQuery();
                Conn.Close();
                Conn = null;

                Label1.Text = "Request Send Successfully..We will revert you soon.Thank You";
                TxtReqAmt.Text = "0";
                TxtCredit.Text = Amount().ToString();

                if (Session["CompID"].ToString() == "1060")
                {
                    TxtReqAmt.Enabled = true;
                }
            }
            else
            {
                Response.Redirect("FundWithdraw.aspx");
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
    protected void TxtReqAmt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            // No logic in original VB code
        }
        catch (Exception)
        {
            // Optional: log exception
        }
    }

}