using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Globalization;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Web.UI;
using System.Web;
using System.Text;

public partial class Idactivation : System.Web.UI.Page
{
    string scrName;
    DAL ObjDAL;
    DAL obj;
    clsGeneral objGen = new clsGeneral();
    DAL Obj;
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                ObjDAL = new DAL();
                string str = "exec('Create table Trnactivecpanel " +
                             "([ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL," +
                             "[Transid] [numeric](18, 0) NOT NULL," +
                             "[Rectimestamp] [datetime] NOT NULL," +
                             "PRIMARY KEY CLUSTERED ([Transid] ASC) " +
                             "WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, " +
                             "ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] " +
                             "ALTER TABLE [dbo].[Trnactivecpanel] " +
                             "ADD DEFAULT (getdate()) FOR [Rectimestamp] ')";

                int i = 0;
                try
                {
                    i = SqlHelper.ExecuteNonQuery(
                    HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(),
                    CommandType.Text,
                    str
                );
                }
                catch (Exception ex)
                { }
                Obj = new DAL();
                if (!Page.IsPostBack)
                {
                    HDnTopupSeq.Value = "0";
                    HdnCheckTrnns.Value = GenerateRandomStringactive(6);
                    GetBalance();
                    fillkit();
                    Session["CkyPinTransfer"] = null;
                }
                cmdSave1.Attributes.Add("onclick", DisableTheButton(this.Page, cmdSave1));

            }
            else
            {
                Response.Redirect("logout.aspx");
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff") +
                          Environment.NewLine;

            ObjDAL.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private string GetName()
    {
        try
        {
            string str = "";
            DataTable dt = new DataTable();

            str = Obj.IsoStart + "Select a.Formno,a.Idno,a.MemFirstName + ' ' + a.MemLastName as MemName," +
                  "IsNull(c.Idno,'') as SponsorId,a.Mobl,a.email," +
                  "isnull((c.MemFirstName+' '+c.MemLastname),' ') as SponsorName," +
                  "a.IsTopup ,a.KitId,b.MACAdrs,b.TopUpSeq,a.LegNo,B.KitName,a.BV,b.bv as KBv,A.Isblock " +
                  ",Case when a.ActiveStatus='Y' then Replace(Convert(Varchar,a.UpgradeDate,106),' ','-') else '' end as UpgradeDate," +
                  "a.ActiveStatus,a.FLD1,a.Planid " +
                  "from " + Obj.dBName + "..M_KitMaster as b, " + Obj.dBName + "..M_MemberMaster as a " +
                  "Left Join " + Obj.dBName + "..M_MemberMaster as c on a.RefFormno=c.Formno " +
                  "where a.KitId=b.KitId and (b.activestatus='Y') " +
                  "and a.IDNo='" + txtMemberId.Text + "' and a.IsBlock='N'" + Obj.IsoEnd;

            dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];

            if (dt.Rows.Count == 0)
            {
                string scrName = "<SCRIPT>alert('Invalid ID Does Not Exists');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(
                    this.Page, this.GetType(), "Login Error", scrName, false);

                TxtMemberName.Text = "";
                return "";
            }
            else
            {
                string compId = Session["CompID"].ToString();

                if (compId == "1088" || compId == "1089" || compId == "1092" ||
                    compId == "1097" || compId == "1100" || compId == "1103")
                {
                    lblFormno.Text = dt.Rows[0]["Formno"].ToString();
                    TxtMemberName.Text = dt.Rows[0]["MemName"].ToString();
                    LblMobile.Text = dt.Rows[0]["Mobl"].ToString();
                    kitid.Text = dt.Rows[0]["KitId"].ToString();
                    HDnTopupSeq.Value = dt.Rows[0]["TopUpSeq"].ToString();
                    return "OK";
                }
                else
                {
                    if (compId == "1108" || compId == "1110")
                    {
                        lblFormno.Text = dt.Rows[0]["Formno"].ToString();
                        TxtMemberName.Text = dt.Rows[0]["MemName"].ToString();
                        LblMobile.Text = dt.Rows[0]["Mobl"].ToString();
                        lblemail.Text = dt.Rows[0]["email"].ToString();
                        return "OK";
                    }
                    else
                    {
                        if (dt.Rows[0]["ActiveStatus"].ToString() == "N")
                        {
                            lblFormno.Text = dt.Rows[0]["Formno"].ToString();
                            TxtMemberName.Text = dt.Rows[0]["MemName"].ToString();
                            LblMobile.Text = dt.Rows[0]["Mobl"].ToString();
                            lblemail.Text = dt.Rows[0]["email"].ToString();
                            return "OK";
                        }
                        else
                        {
                            //string scrName =
                            //    "<SCRIPT>setTimeout(function(){ alert('Id Already Activate'); },10000);</SCRIPT>";

                            //ScriptManager.RegisterClientScriptBlock(
                            //    this.Page, this.GetType(), "Login Error", scrName, false);
                            string scrName = "<SCRIPT>alert('Id Already Activate.!');</SCRIPT>";

                            ScriptManager.RegisterClientScriptBlock(
                                this.Page, this.GetType(), "Upgraded", scrName, false);
                            TxtMemberName.Text = "";
                            LblMobile.Text = "";
                            return "";
                        }
                    }
                    
                }
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff") +
                          Environment.NewLine;

            ObjDAL.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            return "";
        }
    }
    protected void fillkit(string condition = "")
    {
        try
        {
            string query = "";
            DataTable Dt_Kit = new DataTable();
            string compId = Session["CompID"].ToString();

            if (compId == "1068" || compId == "1077" || compId == "1093" || compId == "1082" || compId == "1089")
            {
                query = Obj.IsoStart + "Select KitId,KitName,KitAmount From " + Obj.dBName + "..M_KitMaster " +
                        "where ActiveStatus='Y' and Rowstatus='Y' " +
                        "and KitAmount <= (select Balance from dbo.ufnGetBalance('" +
                        Session["Formno"] + "','S')) " +
                        "and KitAmount > 0 Order By KitName desc" + Obj.IsoEnd;
            }
            else if (compId == "1092")
            {
                query = Obj.IsoStart + "Select KitId,KitName,KitAmount From " + Obj.dBName + "..M_KitMaster " +
                        "where ActiveStatus='Y' and Rowstatus='Y' " +
                        "and KitAmount <= (select Balance from dbo.ufnGetBalance('" +
                        Session["Formno"] + "','S')) " +
                        "and KitAmount > 0 And TopupSeq > 1 Order By KitName desc" + Obj.IsoEnd;
            }
            else if (compId == "1097" || compId == "1100" || compId == "1103" || compId == "1108" || compId == "1109" || compId == "1110")
            {
                query = Obj.IsoStart + "Select KitId,KitName,KitAmount From " + Obj.dBName + "..M_KitMaster " +
                        "where ActiveStatus='Y' and Rowstatus='Y' " +
                        "and KitAmount <= (select Balance from dbo.ufnGetBalance('" +
                        Session["Formno"] + "','R')) " +
                        "and KitAmount > 0 " + condition + " Order By KitId" + Obj.IsoEnd;
            }
            else
            {
                query = Obj.IsoStart + "Select KitId,KitName,KitAmount From " + Obj.dBName + "..M_KitMaster " +
                        "where ActiveStatus='Y' and Rowstatus='Y' " +
                        "and KitAmount <= (select Balance from dbo.ufnGetBalance('" +
                        Session["Formno"] + "','R')) " +
                        "and KitAmount > 0 Order By KitName desc" + Obj.IsoEnd;
            }

            Dt_Kit = SqlHelper.ExecuteDataset(
                        HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                        CommandType.Text,
                        query
                    ).Tables[0];

            Session["KitTable"] = Dt_Kit;

            CmbKit.DataSource = Dt_Kit;
            CmbKit.DataTextField = "KitName";
            CmbKit.DataValueField = "KitId";
            CmbKit.DataBind();

            Session["MKit"] = Dt_Kit;

            if (Dt_Kit.Rows.Count > 0)
            {
                txtAmount.Text = Dt_Kit.Rows[0]["KitAmount"].ToString();
                Session["KitId"] = Dt_Kit.Rows[0]["KitId"].ToString();
            }
            else
            {
                txtAmount.Text = "0";
                Session["KitId"] = "1";
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff") +
                          Environment.NewLine;

            ObjDAL.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    public string GenerateRandomStringactive(int iLength)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        string sResult = "";

        for (int i = 0; i < iLength; i++)
        {
            sResult += allowChrs[rdm.Next(0, allowChrs.Length)];
        }

        return sResult;
    }
    private string DisableTheButton(Control pge, Control btn)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
        sb.Append("this.value = 'Please wait...';");
        sb.Append("this.disabled = true;");
        sb.Append(pge.Page.GetPostBackEventReference(btn));
        sb.Append(";");

        return sb.ToString();
    }
    protected void GetBalance()
    {
        try
        {
            DataTable Dt = new DataTable();
            string str = "";
            string compId = Session["CompID"].ToString();

            if (compId == "1068" || compId == "1077" || compId == "1093" ||
                compId == "1082" || compId == "1092" || compId == "1089")
            {
                str = Obj.IsoStart + "Select * From dbo.ufnGetBalance('" +
                      Convert.ToInt64(Session["Formno"]) + "','S')" + Obj.IsoEnd;
            }
            else
            {
                str = Obj.IsoStart + "Select * From dbo.ufnGetBalance('" +
                      Convert.ToInt64(Session["Formno"]) + "','R')" + Obj.IsoEnd;
            }

            Dt = SqlHelper.ExecuteDataset(
                    HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                    CommandType.Text,
                    str
                 ).Tables[0];

            if (Dt.Rows.Count > 0)
            {
                AvailableBal.InnerText = Convert.ToDecimal(Dt.Rows[0]["Balance"]).ToString();
            }
            else
            {
                AvailableBal.InnerText = "0";
            }

            Session["ServiceWallet"] = AvailableBal.InnerText;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff") +
                          Environment.NewLine;

            ObjDAL.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected void IdActivation()
    {
        string query = "";

        try
        {
            ObjDAL = new DAL();
            int updateeffect = 0;

            string StrSql2 =
                "Insert into Trnactivecpanel (Transid,Rectimestamp) values(" +
                HdnCheckTrnns.Value + ",getdate())";

            try
            {
                updateeffect = SqlHelper.ExecuteNonQuery(
                HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(),
                CommandType.Text,
                StrSql2
            );
            }
            catch
            {

            }
            if (updateeffect > 0)
            {
                ObjDAL = new DAL();

                if (GetName() == "OK")
                {
                    CheckAmount();

                    if (Convert.ToDecimal(Session["ServiceWallet"]) >= Convert.ToDecimal(txtAmount.Text))
                    {
                        string sql = "";

                        if (Session["CompID"].ToString() == "1088")
                        {
                            string Bill_No = GenerateRandomStringactive(6);
                            sql = "exec Sp_ActivateMember '" + txtMemberId.Text.Trim() + "','" +
                                  Convert.ToInt32(CmbKit.SelectedValue) + "','" +
                                  Convert.ToInt32(Session["Formno"]) + "','" + Bill_No + "'";
                        }
                        else if (Session["CompID"].ToString() == "1089")
                        {
                            string Bill_No = GenerateRandomStringactive(6);
                            sql = "exec Sp_ActivateMemberUpdate '" + txtMemberId.Text.Trim() + "','" +
                                  Convert.ToInt32(CmbKit.SelectedValue) + "','" +
                                  Convert.ToInt32(Session["Formno"]) + "','" + Bill_No + "'";
                        }
                        else if (Session["CompID"].ToString() == "1092" ||
                                 Session["CompID"].ToString() == "1097" ||
                                 Session["CompID"].ToString() == "1100" ||
                                 Session["CompID"].ToString() == "1104" ||
                                 Session["CompID"].ToString() == "1108" ||
                                 Session["CompID"].ToString() == "1109" ||
                                 Session["CompID"].ToString() == "1110" ||
                                 Session["CompID"].ToString() == "1107")
                        {
                            string Bill_No = GenerateRandomStringactive(6);
                            sql = "exec Sp_ActivateMember '" + txtMemberId.Text.Trim() + "','" +
                                  Convert.ToInt32(CmbKit.SelectedValue) + "','" +
                                  Convert.ToInt32(Session["Formno"]) + "','" + Bill_No + "'";
                        }
                        else if (Session["CompID"].ToString() == "1103")
                        {
                            DataTable Dt_Kit = new DataTable();
                            string s = "Exec GetBillTypeByRepurchase '" + lblFormno.Text + "','" +
                                       Convert.ToInt32(CmbKit.SelectedValue) + "'";

                            Dt_Kit = SqlHelper.ExecuteDataset(
                                        HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(),
                                        CommandType.Text,
                                        s
                                    ).Tables[0];

                            if (Dt_Kit.Rows.Count > 0)
                            {
                                string Bill_No = GenerateRandomStringactive(6);
                                sql = "exec Sp_ActivateMember '" + txtMemberId.Text.Trim() + "','" +
                                      Convert.ToInt32(CmbKit.SelectedValue) + "','" +
                                      Convert.ToInt32(Session["Formno"]) + "','" +
                                      Bill_No + "','" + Dt_Kit.Rows[0]["BillType"] + "'";
                            }
                        }
                        else if (Session["CompID"].ToString() == "1105")
                        {
                            string Qry = "Insert into purchaseReq(FormNo,FromFormNo,Kitid,UserAddress,Pincode,StateName,City,District,ReqDate,Statecodes)";
                            Qry += " select '" + lblFormno.Text + "','" + Convert.ToInt32(Session["Formno"]) + "','" + Convert.ToInt32(CmbKit.SelectedValue) + "'," +
                                   " address1,pincode,statename,city,district,getdate(),SMst.StateCode from M_MemberMaster as mMst " +
                                   " left join m_StateDivMaster SMst on SMst.StateCode = mMst.StateCode And sMst.RowStatus = 'Y' " +
                                   " where formno = '" + lblFormno.Text + "' ";
                            int j = Convert.ToInt32(
                                SqlHelper.ExecuteNonQuery(
                                    HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(),
                                    CommandType.Text,
                                    Qry
                                )
                            );
                            string Bill_No = GenerateRandomStringactive(6);
                            sql = "exec Sp_ActivateMember '" + txtMemberId.Text.Trim() + "','" + Convert.ToInt32(CmbKit.SelectedValue) + "','" + Convert.ToInt32(Session["Formno"]) + "','" + Bill_No + "'";
                        }
                        else
                        {
                            sql = "exec Sp_ActivateMember '" + txtMemberId.Text.Trim() + "','" +
                                  Convert.ToInt32(CmbKit.SelectedValue) + "','" +
                                  Convert.ToInt32(Session["Formno"]) + "'";
                        }

            
                        query =
                            " Begin Try Begin Transaction " + sql +
                            " Commit Transaction End Try " +
                            " BEGIN CATCH ROLLBACK Transaction END CATCH";

                        int i = Convert.ToInt32(
                            SqlHelper.ExecuteNonQuery(
                                HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(),
                                CommandType.Text,
                                query
                            )
                        );

                        if (i > 0)
                        {
                            string sms =
                                "Dear " + TxtMemberName.Text.Trim() +
                                ", Your id " + txtMemberId.Text.Trim().ToUpper() +
                                " is successfully topup by " +
                                CmbKit.SelectedItem.Text +
                                ". Best of luck, Regards: " +
                                Session["CompName"];

                            sendSMS(sms);

                            GetBalance();

                            string scrName =
                                "<SCRIPT>alert('ID : " + txtMemberId.Text.Trim().ToUpper() +
                                ". Name : " + TxtMemberName.Text +
                                ". Package Name : " + CmbKit.SelectedItem.Text.Trim() +
                                ". Activated successfully!!');" +
                                "location.replace('IdActivation.aspx');</SCRIPT>";

                            ScriptManager.RegisterClientScriptBlock(
                                this.Page, this.GetType(), "Upgraded", scrName, false);

                            Clear();
                            fillkit();
                        }
                    }
                    else
                    {
                        Clear();
                        string scrName =
                            "<SCRIPT>alert('Insufficient Balance!!');</SCRIPT>";

                        ScriptManager.RegisterClientScriptBlock(
                            this.Page, this.GetType(), "Upgraded", scrName, false);

                        LblError.Text = "Insuffiecient Balance!!";
                    }
                }
            }
            else
            {
                // Response.Redirect("IdActivation.aspx");
                string scrName = "<SCRIPT>alert('Try Again.!');" + "location.replace('IdActivation.aspx');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff") +
                          Environment.NewLine;

            ObjDAL.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private string CreateRandomAlphanumericString(int size)
    {
        try
        {
            char[] allowedChars = "123456789".ToCharArray();
            byte[] bytes = new byte[size];

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(bytes);
            }

            StringBuilder retVal = new StringBuilder(size);
            foreach (byte b in bytes)
            {
                retVal.Append(allowedChars[b % allowedChars.Length]);
            }

            return retVal.ToString();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff") +
                          Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
            return string.Empty;
        }
    }
    protected void txtAmount_TextChanged(object sender, EventArgs e)
    {
        try
        {
            CheckAmount();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff") +
                          Environment.NewLine;

            ObjDAL.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected bool CheckAmount()
    {
        try
        {
            DataTable Dt = new DataTable();
            string str = "";
            string compId = Session["CompID"].ToString();

            if (compId == "1068" || compId == "1077" || compId == "1093" ||
                compId == "1082" || compId == "1092" || compId == "1089")
            {
                str = Obj.IsoStart + "Select * From dbo.ufnGetBalance('" +
                      Convert.ToInt64(Session["Formno"]) + "','S')" + Obj.IsoEnd;
            }
            else
            {
                str = Obj.IsoStart + "Select * From dbo.ufnGetBalance('" +
                      Convert.ToInt64(Session["Formno"]) + "','R')" + Obj.IsoEnd;
            }

            Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];

            if (Dt.Rows.Count > 0)
            {
                decimal balance = Convert.ToDecimal(Dt.Rows[0]["Balance"]);
                Session["ServiceWallet"] = balance;
                LblAmount.Text = balance.ToString();

                if (balance < Convert.ToDecimal(txtAmount.Text))
                {
                    LblAmount.Text = "Insufficient Balance";
                    LblAmount.ForeColor = System.Drawing.Color.Red;
                    LblAmount.Visible = true;
                    cmdSave1.Enabled = false;
                    return false;
                }
                else
                {
                    cmdSave1.Enabled = true;
                    LblAmount.Visible = false;
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff") +
                          Environment.NewLine;

            ObjDAL.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }

        return false;
    }
    private bool IsValidID(string MemberID, string PinNo, ref string Msg)
    {
        DataTable dt;
        bool BoolResult = false;
        int NewKitTopupseq;
        string NewKitMacAdrs = "";

        MemberID = MemberID.Trim()
                           .Replace(";", "")
                           .Replace("'", "")
                           .Replace("=", "");

        string q =
            Obj.IsoStart + "Select a.KitName,a.Allowtopup,a.MACAdrs,a.TopUpSeq,a.KitAmount,a.KitId,a.RP " +
            "FROM " + Obj.dBName + "..M_KitMaster as a WHERE cast(a.kitAmount as Numeric)='" + Convert.ToDecimal(PinNo) + "' " +
            "AND a.Allowtopup='Y' and a.RowStatus='Y' and a.activeStatus='Y' order by a.kitid desc" + Obj.IsoEnd;

        dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, q).Tables[0];

        if (dt.Rows.Count > 0)
        {
            NewKitTopupseq = Convert.ToInt32(dt.Rows[0]["TopUpSeq"]);
            NewKitMacAdrs = dt.Rows[0]["MACAdrs"].ToString();
        }
        else
        {
            Msg = "Package not found.";
            return false;
        }

        string qr1 =
           Obj.IsoStart + "Select a.Formno,a.MemFirstName + ' ' + a.MemLastName as MemName," +
            "isnull(c.Idno,' ') as SponsorId," +
            "Isnull((c.MemFirstName+' '+c.MemLastname),' ') as SponsorName," +
            "a.IsTopup,a.KitId,b.KitName,b.MACAdrs,b.TopUpSeq,a.LegNo,'' as Is_FranKit " +
            "from " + Obj.dBName + "..M_KitMaster as b," + Obj.dBName + "..M_MemberMaster as a " +
            "Left Join " + Obj.dBName + "..M_MemberMaster as c on a.RefFormno=c.Formno " +
            "where a.KitId=b.KitId and b.RowStatus='Y' and a.formno='" + MemberID + "'" + Obj.IsoEnd;

        dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, qr1).Tables[0];

        if (dt.Rows.Count > 0)
        {
            BoolResult = true;

            if (dt.Rows[0]["Is_FranKit"].ToString() == "Y")
            {
                Msg = "OK";
            }
            else
            {
                if (NewKitTopupseq >= Convert.ToInt32(dt.Rows[0]["TopUpSeq"]))
                {
                    Msg = "OK";
                }
                else
                {
                    Msg = " Member Could Not Be Upgraded By This Package.";
                }
            }
        }
        else
        {
            Msg = " Member already activated. Please enter another Member ID.";
            return false;
        }

        return Msg == "OK";
    }
    protected void cmdSave1_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["CkyPinTransfer"] != null)
            {
                if (Session["CkyPinTransfer"].ToString() != "")
                {
                    feedbackpop1.Visible = false;
                }
            }
            else
            {
                if (txtMemberId.Text.Trim() == "")
                {
                    scrName = "<SCRIPT>alert('Enter Id No');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
                }
                else if (Convert.ToDecimal(txtAmount.Text) == 0)
                {
                    scrName = "<SCRIPT>alert('Invalid Amount');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Upgraded", scrName, false);
                }
                else if (GetName() == "OK")
                {
                    if (CheckAmount())
                    {
                        string scrname = "";
                        DataTable Dt1 = new DataTable();

                        ObjDAL = new DAL();

                        string str =
                            Obj.IsoStart + "select * from " + Obj.dBName + "..M_MemberMaster where Epassw='" + TxtPassword.Text.Trim() + "' and Formno=" + Session["Formno"] + Obj.IsoEnd;
                        Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];

                        if (Dt1.Rows.Count > 0)
                        {
                            Session["CkyPinTransfer1"] = Dt1.Rows[0]["EPassw"];
                            IdActivation();
                        }
                        else
                        {
                            Session["CkyPinTransfer1"] = null;
                            scrname =
                                "<SCRIPT>alert('Please Enter valid Transaction Password.');</SCRIPT>";

                            ScriptManager.RegisterClientScriptBlock(
                                this.Page, this.GetType(), "Login Error", scrname, false);
                        }
                    }
                    else
                    {
                        scrName = "<SCRIPT>alert('Insufficient Balance');</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(
                            this.Page, this.GetType(), "Upgraded", scrName, false);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff") +
                          Environment.NewLine;

            ObjDAL.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected void Clear()
    {
        try
        {
            txtMemberId.Text = "";
            TxtMemberName.Text = "";
            txtAmount.Text = "";
            LblAmount.Text = "";
            LblAmount.Visible = false;
            LblError.Visible = false;
            GetBalance();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff") +
                          Environment.NewLine;

            ObjDAL.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private bool Check_IdNo()
    {
        try
        {
            string billtype = "";
            DataTable dt1 = Session["MKit"] as DataTable;
            DataRow[] Dr = dt1.Select("KitID='" + CmbKit.SelectedValue + "'");
            string Sql = "";

            Sql = Obj.IsoStart + "Select a.Formno,a.Idno,a.MemFirstName + ' ' + a.MemLastName as MemName,IsNull(c.Idno,'') as SponsorId," +
                  " isnull((c.MemFirstName+' '+c.MemLastname),' ') as SponsorName,a.IsTopup ,a.KitId,b.MACAdrs,b.TopUpSeq,a.LegNo," +
                  " B.KitName,a.BV,'' as Is_FranKit,b.bv as KBv " +
                  ",Case when a.ActiveStatus='Y' then Replace(Convert(Varchar,a.UpgradeDate,106),' ','-') else '' end as UpgradeDate," +
                  " a.ActiveStatus,a.FLD1,a.Planid,a.isblock,a.Fld4 " +
                  " from " + Obj.dBName + "..M_KitMaster as b," + Obj.dBName + "..M_MemberMaster as a " +
                  " Left Join " + Obj.dBName + "..M_MemberMaster as c on a.RefFormno=c.Formno " +
                  " where a.KitId=b.KitId and (b.RowStatus='Y') " +
                  " and a.IDNo='" + txtMemberId.Text + "' and a.IsBlock='N'" + Obj.IsoEnd;

            DataTable Dt_ = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Sql).Tables[0];

            if (Dt_.Rows.Count > 0)
            {
                kitid.Text = Dt_.Rows[0]["KitId"].ToString();
                LblError.Text = "";
                TxtMemberName.Text = Dt_.Rows[0]["MemName"].ToString();
                lblFormno.Text = Dt_.Rows[0]["Formno"].ToString();

                if (Session["CompID"].ToString() == "1103")
                {
                    DataTable Dt_Kit = new DataTable();
                    string s = Obj.IsoStart + "Exec GetBillTypeByRepurchase '" + lblFormno.Text + "','" +
                               Convert.ToInt32(kitid.Text) + "'" + Obj.IsoEnd;

                    Dt_Kit = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                                CommandType.Text,
                                s
                            ).Tables[0];

                    if (Dt_Kit.Rows.Count > 0)
                    {
                        billtype = Dt_Kit.Rows[0]["BillType"].ToString();
                        if (billtype.ToUpper() == "R")
                        {
                            LblCondition.Text = "and TopupSeq = '" + Dt_.Rows[0]["TopupSeq"] + "'";
                        }
                        else
                        {
                            LblCondition.Text = "and TopupSeq > '" + Dt_.Rows[0]["TopupSeq"] + "'";
                        }
                    }
                    else
                    {
                        LblCondition.Text = "and TopupSeq > '" + Dt_.Rows[0]["TopupSeq"] + "'";
                    }
                }
                else
                {
                    LblCondition.Text = "and TopupSeq > '" + Dt_.Rows[0]["TopupSeq"] + "'";
                }

                return true;
            }
            else
            {
                kitid.Text = "";
                LblError.Text = "";
                TxtMemberName.Text = "";
                lblFormno.Text = "";
                LblCondition.Text = "";
                return false;
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff") +
                          Environment.NewLine;

            Response.Write("Try later.");
            return false;
        }
    }
    protected void txtMemberId_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string compId = Session["CompID"].ToString();

            if (compId == "1097" || compId == "1100" || compId == "1103" || compId == "1108" || compId == "1110")
            {
                if (Check_IdNo())
                {
                    fillkit(LblCondition.Text.Trim());

                    if (Convert.ToInt32(CmbKit.SelectedValue) == 0)
                    {
                        txtAmount.Text = "0";
                    }
                    else
                    {
                        cmdSave1.Enabled = true;
                    }
                }
                else
                {
                    cmdSave1.Enabled = false;
                }
            }
            else
            {
                GetName();
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff") +
                          Environment.NewLine;

            ObjDAL.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected void CmbKit_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt = Session["KitTable"] as DataTable;
        DataRow[] Dr = Dt.Select("KitID='" + CmbKit.SelectedValue + "'");

        if (Dr.Length > 0)
        {
            txtAmount.Text = Dr[0]["KitAmount"].ToString();
            divEP.Visible = false;
        }
    }
    public bool SendToMemberMail(string IdNo, string Email, string MemberName, string Packagename)
    {
        try
        {
            string StrMsg =
                "<table style='margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%'>" +
                "<tr><td>" +
                "<span style='color:#0099CC; font-weight:bold;'><h2>Dear " + MemberName + ", Congratulations,</h2></span><br/>" +
                "Your id <strong>" + IdNo + "</strong> is successfully Activated by <strong>" +
                Packagename + "</strong>.<br/>" +
                "Good Luck for your bright Future.<br/>" +
                "Regards: <strong>Discount Mart</strong><br/><br/>" +
                "</td></tr></table>";

            MailAddress sendFrom = new MailAddress(Session["CompMail"].ToString());
            MailAddress sendTo = new MailAddress(Email);

            MailMessage message = new MailMessage(sendFrom, sendTo);
            message.Subject = "Forgot Password";
            message.Body = StrMsg;
            message.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.UseDefaultCredentials = false;
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Credentials =
                new NetworkCredential(Session["CompMail"].ToString(),
                                      Session["MailPass"].ToString());

            smtp.Send(message);
            return true;
        }
        catch
        {
            Response.Write("Try later.");
            return false;
        }
    }
    private void sendSMS(string sms)
    {
        // dbConnect.OpenConnection();

        if (LblMobile.Text.Length >= 10 && long.TryParse(LblMobile.Text, out _))
        {
            WebClient client = new WebClient();
            string baseurl = "";
            Stream data = null;

            try
            {
                baseurl = " http://www.apiconnecto.com/API/SMSHttp.aspx?UserId=" + Session["SmsId"] + "&pwd=" + Session["SmsPass"] + "&Message=" + sms + "&Contacts=" + Session["UPGRDMobileNo"] + "&SenderId=" + Session["ClientId"] + "";
                data = client.OpenRead(baseurl);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();

                reader.Close();
                data.Close();
            }
            catch (Exception)
            {
                // silently ignored (same as VB code)
            }
        }
    }

}

