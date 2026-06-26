using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System.Configuration;
using System.Web.Services;

public partial class walletrequest : System.Web.UI.Page
{
    DAL ObjDAL;
    SqlDataReader ds;
    SqlDataReader ds1;
    SqlConnection Conn;
    SqlCommand Comm;
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dRead;
    private cls_DataAccess dbConnect;
    int TransferId;
    DataTable tmpTable = new DataTable();
    DataTable dt1;
    DataTable dt2;
    string strQuery = "";
    SqlDataAdapter Ad;
    string scrname;
    DAL Obj;
    clsGeneral objGen = new clsGeneral();
    protected void Page_Load(object sender, EventArgs e)
    {
        // MasterPage Mst = new MasterPage();
        try
        {

            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                Obj = new DAL();
                try
                {
                    string str = "exec('Create table Trnwalletreqcpanel ([ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,[Transid] [numeric](18, 0) NOT NULL,[Rectimestamp] [datetime] NOT NULL,PRIMARY KEY CLUSTERED ([Transid] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF," +
                                 "ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) ON [PRIMARY] ALTER TABLE [dbo].[Trnwalletreqcpanel] ADD  DEFAULT (getdate()) FOR [Rectimestamp] ')";

                    int i = SqlHelper.ExecuteNonQuery(
                                (string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]],
                                CommandType.Text,
                                str
                            );
                }
                catch (Exception)
                {
                }
                this.cmdSave1.Attributes.Add("onclick", DisableTheButton(this.Page, this.cmdSave1));
                if (!Page.IsPostBack)
                {
                    

                    HdnCheckTrnns.Value = GenerateRandomStringactive(6);

                    if (Session["CompId"] != null && Session["CompId"].ToString() == "1049")
                    {
                        CheckDDnod();
                    }

                    FillPaymode();
                    CheckVisible();

                    hdnSessn.Value = Crypto.Encrypt(Session["IDNo"].ToString());

                    // Company ID 1070
                    if (Session["CompId"] != null && Session["CompId"].ToString() == "1070")
                    {
                        if (!GetReqStatus())
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                "Key", "alert('Request already Pending.!!');location.replace('LifeHome.aspx');", true);
                            return;
                        }
                    }

                    // Company ID 1082
                    if (Session["CompId"] != null && Session["CompId"].ToString() == "1082")
                    {
                        if (!GetReqStatus())
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                "Key", "alert('Request already Pending.!!');location.replace('RefIndex.aspx');", true);
                            return;
                        }
                    }

                    // Company ID 1092
                    if (Session["CompId"] != null && Session["CompId"].ToString() == "1092")
                    {
                        if (!GetReqStatus())
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                "Key", "alert('Request already Pending.!!');location.replace('RefIndex.aspx');", true);
                            return;
                        }
                    }

                    // Company ID 1075 (KYC Check)
                    if (Session["CompID"] != null && Session["CompID"].ToString() == "1075")
                    {
                        if (GetKycPerStatus())
                        {
                            cmdSave1.Enabled = true;
                        }
                        else
                        {
                            cmdSave1.Enabled = false;
                            ScriptManager.RegisterStartupScript(this, this.GetType(),
                                "Key", "alert('KYC Not Verified or Please Upload KYC.!!');location.replace('" + Session["IndexPage"] + "');", true);
                            return;
                        }
                    }
                }
            }
            else
            {
                Response.Redirect("Logout.aspx");
                Response.End();
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);

            Response.Write("Try later.");
        }

    }
    private void LoadCompanyPaymentInfo()
    {
        if (Session["CompID"] == null) return;

        int compId;
        if (!int.TryParse(Session["CompID"].ToString(), out compId)) return;

        string cs = (string)HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]];

        using (SqlConnection con = new SqlConnection(cs))
        using (SqlCommand cmd = new SqlCommand(
            Obj.IsoStart + "SELECT TOP 1 qr_image, show_qr, ac_holder, bank_name, ac_no, ifsc, branch, show_bank " +
            "FROM " + Obj.dBName + "..CompanyPaymentInfo WHERE comp_id = @cid AND is_active = 1" + Obj.IsoEnd, con))
        {
            cmd.Parameters.AddWithValue("@cid", compId);
            con.Open();

            using (SqlDataReader rd = cmd.ExecuteReader())
            {
                if (!rd.Read()) return;   // entry nahi -> Div2/Div3 dono hidden

                // ---- QR card (Div2) ----
                bool showQr = rd["show_qr"] != DBNull.Value && Convert.ToBoolean(rd["show_qr"]);
                string qrPath = rd["qr_image"] == DBNull.Value ? "" : rd["qr_image"].ToString();
                if (showQr && !string.IsNullOrEmpty(qrPath))
                {
                    imgQr.Src = qrPath;
                    //Div2.Visible = true;
                }

                // ---- Bank details (Div3) ----
                bool showBank = rd["show_bank"] != DBNull.Value && Convert.ToBoolean(rd["show_bank"]);
                if (showBank)
                {
                    Label26.Text = rd["ac_holder"].ToString();
                    Label38.Text = rd["bank_name"].ToString();
                    Label40.Text = rd["ac_no"].ToString();
                    Label42.Text = rd["ifsc"].ToString();
                    Label44.Text = rd["branch"].ToString();
                   // Div3.Visible = true;
                }
            }
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
    private bool GetReqStatus()
    {
        try
        {
            bool result = false;

            string strSql = Obj.IsoStart + "exec SP_GetReqidStatus '" + Session["FormNo"] + "'" + Obj.IsoEnd;

            DataSet Ds = SqlHelper.ExecuteDataset(
                            (string)HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]],
                            CommandType.Text, strSql);

            DataTable dt = Ds.Tables[0];

            if (Convert.ToInt32(dt.Rows[0]["Cnt"]) == 0)
                result = true;
            else
                result = false;

            return result;
        }
        catch (Exception)
        {
            return false;
        }
    }
    private bool GetKycPerStatus()
    {
        try
        {
            bool result = false;

            string Str = Obj.IsoStart + "select * from " + Obj.dBName + "..KycVerify as a, " + Obj.dBName + "..m_membermaster as b " +
                         "where a.formno=b.formno and Isaddrssverified in ('N','Y') " +
                         "and a.AddrProof<>'' and a.BackAddressProof<>'' " +
                         "AND a.formno='" + Convert.ToInt32(Session["formno"]) + "'" + Obj.IsoEnd;

            DataSet Ds = SqlHelper.ExecuteDataset(
                            (string)HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]],
                            CommandType.Text, Str);

            DataTable dt = Ds.Tables[0];

            result = dt.Rows.Count > 0;

            return result;
        }
        catch (Exception)
        {
            return false;
        }
    }
    private void FillPaymode()
    {
        try
        {
            strQuery = Obj.IsoStart + "SELECT * FROM " + Obj.dBName + "..M_PayModeMaster WHERE ActiveStatus='Y' ORDER BY Pid" + Obj.IsoEnd;

            tmpTable = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, strQuery).Tables[0];

            DdlPaymode.DataSource = tmpTable;
            DdlPaymode.DataValueField = "PID";
            DdlPaymode.DataTextField = "Paymode";
            DdlPaymode.DataBind();

            Session["PaymodeDetail"] = tmpTable;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    private void FillBankMaster(string condition)
    {
        try
        {
            strQuery = Obj.IsoStart + "SELECT BankCode, BankName FROM " + Obj.dBName + "..M_BankMaster " +
                       "WHERE ActiveStatus='Y' AND RowStatus='Y' " + condition +
                       " ORDER BY BankCode" + Obj.IsoEnd;

            tmpTable = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, strQuery).Tables[0];

            DDlBank.DataSource = tmpTable;
            DDlBank.DataValueField = "BankCode";
            DDlBank.DataTextField = "BankName";
            DDlBank.DataBind();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected void DdlPaymode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckVisible();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    public string ClearInject(string StrObj)
    {
        StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "");
        return StrObj;
    }
    protected void CheckVisible()
    {
        DataTable dt;
        string condition = "";

        dt = (DataTable)Session["PaymodeDetail"];
        DataRow[] Dr = dt.Select("PID='" + DdlPaymode.SelectedValue + "'");

        if (Dr.Length > 0)
        {
            // 1. Bank Detail
            DivBank.Visible = (Dr[0]["IsBankDtl"].ToString() == "Y");

            // 2. Branch Detail
            DivBranch.Visible = (Dr[0]["IsBranchDtl"].ToString() == "Y");
            if (!DivBranch.Visible) TxtIssueBranch.Text = "";

            // 3. Transaction Number
            divDDno.Visible = (Dr[0]["IsTransNo"].ToString() == "Y");
            if (!divDDno.Visible) TxtDDNo.Text = "";

            // 4. Display upload-copy logic (Comp 1091)
            if (Session["CompId"].ToString() == "1110" || Session["CompId"].ToString() == "1108")
            {
                if (DdlPaymode.SelectedValue == "1")
                {
                    Div2.Visible = true;
                    Div3.Visible = false;
                    LoadCompanyPaymentInfo();
                }
                else if (DdlPaymode.SelectedValue == "4")
                {
                    Div2.Visible = false;
                    Div3.Visible = true;
                    LoadCompanyPaymentInfo();
                }
                else
                {
                    divupcopy.Visible = true;
                    Div2.Visible = false;
                    Div3.Visible = false;
                }

            }
            else
            {
                divupcopy.Visible = true;
                Div2.Visible = false;
                Div3.Visible = false;
            }

            // 5. AllBank logic
            if (Dr[0]["AllBank"].ToString() == " ")
                condition = "";
            else if (Dr[0]["AllBank"].ToString() == "N")
                condition = "and MacAdrs='C' and BranChName<>'N'";
            else
                condition = "and MacAdrs='C'";

            // 6. Special logic for company 1074
            if (Session["CompId"].ToString() == "1074")
            {
                if (DdlPaymode.SelectedValue == "6")
                {
                    divupcopy.Visible = false;
                    divupcopyREm.Visible = false;
                    divDDno.Visible = false;
                    divDDDate.Visible = false;
                    divthirdpoint.Visible = true;
                    divnotes.Visible = false;
                }
                else if (DdlPaymode.SelectedValue == "0")
                {
                    divupcopy.Visible = false;
                    divupcopyREm.Visible = false;
                    divDDno.Visible = false;
                    divDDDate.Visible = false;
                    divthirdpoint.Visible = false;
                    divnotes.Visible = true;
                }
                else
                {
                    divupcopy.Visible = true;
                    divupcopyREm.Visible = true;
                    divthirdpoint.Visible = false;
                    divnotes.Visible = true;
                }
            }
            else
            {
                divupcopyREm.Visible = true;
            }

            // Fill bank dropdown
            FillBankMaster(condition);

            // Set labels
            LblDDNo.Text = Dr[0]["TransNoLbl"].ToString();
            LblDDDate.Text = Dr[0]["TransDateLbl"].ToString();
        }
    }
    protected void TxtDDNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string s = "";
            DataTable dt = new DataTable();
            Obj = new DAL();

            if (Convert.ToInt32(DdlPaymode.SelectedValue) != 10)
            {
                // COMP 1091 special logic
                if (Session["CompID"].ToString() == "1091")
                {
                    if (divDDno.Visible)
                    {
                        s = Obj.IsoStart + "select * from " + Obj.dBName + "..WalletReq where ChqNo='" + TxtDDNo.Text.Trim() + "' and IsApprove<>'R'" + Obj.IsoEnd;
                        dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s).Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            scrname = "<script>alert('" + LblDDNo.Text + " already exist');</script>";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);

                            cmdSave1.Enabled = false;
                            TxtDDNo.Text = "";
                            return;
                        }
                        else { cmdSave1.Enabled = true; }
                    }

                    if (divDDno.Visible)
                    {
                        s = Obj.IsoStart + "select * from " + Obj.dBName + "..TrnProductorderDetail where txnid='" + TxtDDNo.Text.Trim() + "' and IsApprove<>'R'" + Obj.IsoEnd;
                        dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            scrname = "<script>alert('" + LblDDNo.Text + " already exist');</script>";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);

                            cmdSave1.Enabled = false;
                            TxtDDNo.Text = "";
                            return;
                        }
                        else { cmdSave1.Enabled = true; }
                    }
                }
                else
                {
                    if (divDDno.Visible)
                    {
                        s = Obj.IsoStart + "select * from " + Obj.dBName + "..WalletReq where ChqNo='" + TxtDDNo.Text.Trim() + "' and IsApprove<>'R'" + Obj.IsoEnd;
                        dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            scrname = "<script>alert('" + LblDDNo.Text + " already exist');</script>";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                            cmdSave1.Enabled = false;
                            TxtDDNo.Text = "";
                        }
                        else
                        {
                            cmdSave1.Enabled = true;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected bool CheckDDnod()
    {
        string s = "";
        DataTable dt = new DataTable();
        Obj = new DAL();

        s = Obj.IsoStart + "select case when fld5='Y' then 8000 else 1 end as minimumreqamount " +
            "from " + Obj.dBName + "..M_MemberMaster where formno='" + Convert.ToInt32(Session["formno"]) + "'" + Obj.IsoEnd;

        dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s).Tables[0];

        if (dt.Rows.Count > 0)
        {
            Session["MinReqAmount"] = dt.Rows[0]["minimumreqamount"];
            return true;
        }

        return true;
    }
    protected bool CheckDDno()
    {
        string s = "";
        DataTable dt = new DataTable();
        Obj = new DAL();

        if (Convert.ToInt32(DdlPaymode.SelectedValue) != 10)
        {
            if (divDDno.Visible && TxtDDNo.Text != "")
            {
                if (Session["CompID"].ToString() == "1091")
                {
                    s = Obj.IsoStart + "select * from " + Obj.dBName + "..WalletReq where ChqNo='" + TxtDDNo.Text.Trim() + "' and IsApprove<>'R'" + Obj.IsoEnd;
                    dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s).Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        scrname = "<script>alert('" + LblDDNo.Text + " already exist');</script>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);

                        cmdSave1.Enabled = false;
                        TxtDDNo.Text = "";
                        return false;
                    }
                    else
                    {
                        cmdSave1.Enabled = true;
                    }

                    // Check product order table also
                    s = Obj.IsoStart + "select * from " + Obj.dBName + "..TrnProductorderDetail where txnid='" + TxtDDNo.Text.Trim() + "' and IsApprove<>'R'" + Obj.IsoEnd;
                    dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s).Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        scrname = "<script>alert('" + LblDDNo.Text + " already exist');</script>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);

                        cmdSave1.Enabled = false;
                        TxtDDNo.Text = "";
                        return false;
                    }
                    else
                    {
                        cmdSave1.Enabled = true;
                    }
                }
                else
                {
                    s = Obj.IsoStart + "select * from " + Obj.dBName + "..WalletReq where ChqNo='" + TxtDDNo.Text.Trim() + "' and IsApprove<>'R'" + Obj.IsoEnd;
                    dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s).Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        scrname = "<script>alert('" + LblDDNo.Text + " already exist');</script>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);

                        TxtDDNo.Text = "";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
        }

        return true;
    }
    protected void ValidateFileSize(object sender, ServerValidateEventArgs e)
    {
        try
        {
            if (FlDoc.HasFile)
            {
                string strextension = System.IO.Path.GetExtension(FlDoc.FileName).ToUpper();

                if (strextension == ".JPG" || strextension == ".GIF" || strextension == ".JPEG" ||
                    strextension == ".BMP" || strextension == ".PNG")
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(FlDoc.PostedFile.InputStream);

                    decimal size = Math.Round((Convert.ToDecimal(FlDoc.PostedFile.ContentLength) / 1024), 2);

                    if (size > 10240)   // 10 MB
                    {
                        CustomValidator1.ErrorMessage = "File size must not exceed 10 MB.";
                        e.IsValid = false;
                        LblImage.Text = "False";
                    }
                    else
                    {
                        LblImage.Text = "True";
                    }
                }
                else
                {
                    CustomValidator1.ErrorMessage = "You can upload only .jpg,.gif,.jpeg,.bmp,.png extension file!!";
                    e.IsValid = false;
                    LblImage.Text = "False";
                }
            }
            else
            {
                LblImage.Text = "True";
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            Response.End();
        }
    }
    protected void cmdSave1_Click(object sender, EventArgs e)
    {
        string FlNm = "", flnm1 = "";
        string scrname = "";
        Session["CkyPinRequest"] = null;

        if (Convert.ToDecimal(TxtAmount.Text == "" ? "0" : TxtAmount.Text) <= 0)
        {
            scrname = "<script language='javascript'>alert('Please Enter Amount.');</script>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
            return;
        }

        if (DdlPaymode.SelectedValue == "0")
        {
            scrname = "<script language='javascript'>alert('Choose Paymode');</script>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
            return;
        }

        // File required checks depending on company and paymode
        if (Session["CompId"] != null && Session["CompId"].ToString() == "1091")
        {
            if (DdlPaymode.SelectedValue != "3")
            {
                if (FlDoc.Enabled)
                {
                    if (!FlDoc.HasFile)
                    {
                        scrname = "<script language='javascript'>alert('Please upload wallet receipt jpg/jpeg/png/ image of upto 5 mb size only!!');</script>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                        return;
                    }
                }
            }
        }
        else if (Session["CompId"] != null && Session["CompId"].ToString() == "1074")
        {
            if (DdlPaymode.SelectedValue != "6")
            {
                if (FlDoc.Enabled)
                {
                    if (!FlDoc.HasFile)
                    {
                        scrname = "<script language='javascript'>alert('Please upload wallet receipt jpg/jpeg/png/ image of upto 5 mb size only!!');</script>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                        return;
                    }
                }
            }
        }
        else
        {
            if (FlDoc.Enabled)
            {
                if (!FlDoc.HasFile)
                {
                    scrname = "<script language='javascript'>alert('Please upload wallet receipt jpg/jpeg/png/ image of upto 5 mb size only!!');</script>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                    return;
                }
            }
        }

        // Remarks mandatory
        if (Session["CompId"] != null && Session["CompId"].ToString() == "1074")
        {
            if (DdlPaymode.SelectedValue != "6")
            {
                if (string.IsNullOrWhiteSpace(TxtRemarks.Text))
                {
                    scrname = "<script language='javascript'>alert('Please enter remarks');</script>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                    return;
                }
            }
        }
        else
        {
            if (string.IsNullOrWhiteSpace(TxtRemarks.Text))
            {
                scrname = "<script language='javascript'>alert('Please enter remarks');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                return;
            }
        }

        // Check request status for CompId 1070
        if (Session["CompId"] != null && Session["CompId"].ToString() == "1070")
        {
            if (!GetReqStatus())
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Request already Pending.!!');location.replace('LifeHome.aspx');", true);
                return;
            }
        }

        // Minimum amount check for CompId 1049
        if (Session["CompId"] != null && Session["CompId"].ToString() == "1049")
        {
            CheckDDnod();

            decimal minReq = 0;
            if (Session["MinReqAmount"] != null)
                decimal.TryParse(Session["MinReqAmount"].ToString(), out minReq);

            decimal enteredAmt = 0;
            decimal.TryParse(TxtAmount.Text == "" ? "0" : TxtAmount.Text, out enteredAmt);

            if (minReq > enteredAmt)
            {
                scrname = "<script language='javascript'>alert('Minimum Request Amount " + minReq.ToString() + "/- ');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                return;
            }
        }

        // Bank selection validation
        if (DivBank.Visible)
        {
            if (DDlBank.SelectedValue == "0" || string.IsNullOrEmpty(DDlBank.SelectedValue))
            {
                scrname = "<script language='javascript'>alert('Choose Bank Name');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                return;
            }
        }

        bool flag = false;
        int paymodeVal = 0;
        int.TryParse(DdlPaymode.SelectedValue, out paymodeVal);

        if (paymodeVal == 1)
        {
            flag = true;
        }
        else
        {
            if (divDDno.Visible && string.IsNullOrWhiteSpace(TxtDDNo.Text))
            {
                scrname = "<script language='javascript'>alert('" + LblDDNo.Text + " can not be blank');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                return;
            }
            else
            {
                if (CheckDDno())
                    flag = true;
                else
                    flag = false;

                if (DivBranch.Visible)
                {
                    if (string.IsNullOrWhiteSpace(TxtIssueBranch.Text))
                    {
                        scrname = "<script language='javascript'>alert('Branch name can not be blank');</script>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                        return;
                    }
                }
            }
        }

        string strextension = "";
        string compId = Session["CompID"] != null ? Session["CompID"].ToString() : "0";
        string uploadRoot = Server.MapPath("~/images/UploadImage/" + compId + "/");
        if (!Directory.Exists(uploadRoot))
        {
            Directory.CreateDirectory(uploadRoot);
        }
        try
        {
            if (flag)
            {
                if (FlDoc.HasFile)
                {
                    strextension = Path.GetExtension(FlDoc.FileName);
                    string extUpper = strextension.ToUpperInvariant();

                    if (extUpper == ".JPG" || extUpper == ".GIF" || extUpper == ".JPEG" || extUpper == ".BMP" || extUpper == ".PNG")
                    {
                        if (LblImage.Text == "True")
                        {
                            flnm1 = DateTime.Now.ToString("yyMMddhhmmssfff") + Path.GetExtension(FlDoc.PostedFile.FileName);
                            string saveDir = Server.MapPath("~/images/UploadImage/");
                            if (!Directory.Exists(saveDir))
                            {
                                Directory.CreateDirectory(saveDir);
                            }

                            //string savePath = Path.Combine(saveDir, flnm1);
                            //FlDoc.PostedFile.SaveAs(savePath);

                            //// CompressAndSaveImage(stream, path, extension, quality) - assumed present in codebase
                            //CompressAndSaveImage(FlDoc.PostedFile.InputStream, savePath, strextension, 50L);

                            //FlNm = flnm1;
                            flnm1 = DateTime.Now.ToString("yyMMddhhmmssfff") + Path.GetExtension(FlDoc.PostedFile.FileName);
                            //Fuidentity.PostedFile.SaveAs(Server.MapPath("images/UploadImage/") + flAddrs);
                            //string savePath = Server.MapPath("images/UploadImage/") + flAddrs;
                            string fullPath = Path.Combine(uploadRoot, flnm1);
                            FlDoc.PostedFile.SaveAs(fullPath);
                            CompressAndSaveImage(FlDoc.PostedFile.InputStream, fullPath, strextension, 50L); // Quality 50%
                            FlNm = flnm1;
                            Session["WalletImage"] = FlNm;
                        }
                        else
                        {
                            scrname = "<script language='javascript'>alert('File size must not exceed 10 mb');</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), "MyAlert", scrname);
                            return;
                        }
                    }
                    else
                    {
                        scrname = "<script language='javascript'>alert('You can upload only .jpg,.gif,.jpeg,.bmp,.png extension file!! ');</script>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                        return;
                    }
                }
                else
                {
                    FlNm = "";
                }
            }

            if (Session["CkyPinRequest"] != null)
            {

            }
            else
            {
                try
                {

                    string TransPassw = TxtPassword.Text.Trim();

                    DataTable Dt1 = new DataTable();
                    DAL objDal = new DAL();

                    string str = Obj.IsoStart + "select * from " + Obj.dBName + "..M_MemberMaster where Epassw='" + TransPassw + "' and Formno=" + Session["Formno"] + Obj.IsoEnd;
                    Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];

                    if (Dt1.Rows.Count > 0)
                    {
                        Session["CkyPinTransfer1"] = Dt1.Rows[0]["EPassw"].ToString();
                    }
                    else
                    {
                        Session["CkyPinTransfer1"] = null;

                        scrname = "<script language='javascript'>alert('Please Enter valid Transaction Password.');</script>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Login Error", scrname, false);
                    }

                    if (Session["CkyPinTransfer1"] != null)
                    {
                        if (Session["CkyPinTransfer1"].ToString() != "")
                        {
                            if (Session["CompId"] != null && Session["CompId"].ToString() == "1049")
                            {
                                CheckDDnod();
                            }

                            if (Session["CompId"] != null && Session["CompId"].ToString() == "1091")
                            {
                                CheckDDnod();
                            }

                            if (Session["CompId"] != null && Session["CompId"].ToString() == "1074")
                            {
                                if (DdlPaymode.SelectedValue == "6")
                                {
                                    string OrderId = DateTime.Now.ToString("yyyyMMddhhmmssfff");

                                    string sql = "Insert into OnlineTransaction(Response,Orderid,Orderdate,Amount,status,paymentToken,name,email,mobl,address1," +
                                                 "pincode,statecode,city,epinno,scratchno,kitid,Refformno) " +
                                                 "Values('','" + OrderId + "',Getdate(),'" + TxtAmount.Text + "','','" + TxtDDNo.Text + "','" + Session["MemName"] + "','" +
                                                 Session["EMail"] + "','" + Session["MobileNo"] + "','" + Session["Address"] + "'," +
                                                 "'0','0','','','','0','" + Session["FormNo"] + "')";

                                    int i = SqlHelper.ExecuteNonQuery(
                                               (string)HttpContext.Current.Session["MlmDatabase" + Session["CompID"]],
                                                CommandType.Text,
                                                sql);

                                    if (i > 0)
                                    {
                                        // GenerateQrCode(OrderId, TxtAmount.Text);
                                    }
                                }
                                else
                                {
                                    SaveRequest();
                                }
                            }
                            else
                            {
                                SaveRequest();
                            }

                            // Reset pin session
                            Session["CkyPinTransfer1"] = null;
                        }
                    }
                    else
                    {
                        //feedbackpop1.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    string path = HttpContext.Current.Request.Url.AbsoluteUri;
                    string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;

                    Obj.WriteToFile(text + ex.Message);
                    Response.Write("Try later.");
                }
                // feedbackpop1.Visible = true;
            }
        }
        catch (Exception ex)
        {
            try
            {
                string path = HttpContext.Current.Request.Url.AbsoluteUri;
                string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
                Obj.WriteToFile(text + ex.Message);
            }
            catch { }
            Response.Write("Try later.");
        }
    }
    private void CompressAndSaveImage(Stream inputStream, string savePath, string extension, long quality = 50L)
    {
        using (System.Drawing.Image img = System.Drawing.Image.FromStream(inputStream))
        {
            ImageCodecInfo codec = null;
            EncoderParameters encoderParams = new EncoderParameters(1);

            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    codec = ImageCodecInfo.GetImageEncoders()
                            .FirstOrDefault(c => c.MimeType == "image/jpeg");

                    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                    break;

                case ".png":
                    codec = ImageCodecInfo.GetImageEncoders()
                            .FirstOrDefault(c => c.MimeType == "image/png");

                    // PNG does not support quality compression
                    encoderParams = null;
                    break;

                case ".gif":
                    codec = ImageCodecInfo.GetImageEncoders()
                            .FirstOrDefault(c => c.MimeType == "image/gif");

                    encoderParams = null;
                    break;

                default:
                    throw new Exception("Unsupported file type.");
            }

            if (codec != null)
            {
                if (encoderParams != null)
                    img.Save(savePath, codec, encoderParams);
                else
                    img.Save(savePath, codec, null);
            }
        }
    }
    protected void SaveRequest()
    {
        try
        {
            ObjDAL = new DAL();

            int updateeffect = 0;
            string StrSql2 = "Insert into Trnwalletreqcpanel (Transid,Rectimestamp) values(" +
                             HdnCheckTrnns.Value + ", getdate())";

            updateeffect = ObjDAL.SaveData(StrSql2);

            if (updateeffect > 0)
            {
                // ---------------------- Session Validation ----------------------
                if (Session["Sessncheck"].ToString().ToUpper() != ("OK" + Crypto.Decrypt(hdnSessn.Value).ToUpper()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                        "Key", "alert('Session expire, Please Re-Login.!!');location.replace('logout.aspx');", true);
                    return;
                }

                // --------------------- Company based rules -----------------------
                if (Session["CompId"] != null && Session["CompId"].ToString() == "1049")
                {
                    if (Convert.ToDecimal(Session["MinReqAmount"]) > Convert.ToDecimal(TxtAmount.Text))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(),
                            "Close", "<script>alert('Minimum Request Amount " +
                            Session["MinReqAmount"] + " ');</script>", false);
                        return;
                    }
                }

                if (Session["CompId"] != null && Session["CompId"].ToString() == "1070")
                {
                    if (!GetReqStatus())
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(),
                            "Key", "alert('Request already Pending.!!');location.replace('LifeHome.aspx');", true);
                        return;
                    }
                }

                if (Session["CompId"] != null && Session["CompId"].ToString() == "1082")
                {
                    if (!GetReqStatus())
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(),
                            "Key", "alert('Request already Pending.!!');location.replace('RefIndex.aspx');", true);
                        return;
                    }
                }

                if (Session["CompId"] != null && Session["CompId"].ToString() == "1092")
                {
                    if (!GetReqStatus())
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(),
                            "Key", "alert('Request already Pending.!!');location.replace('RefIndex.aspx');", true);
                        return;
                    }
                }

                // ------------------- Data Cleaning -----------------------
                TxtDDNo.Text = ClearInject(TxtDDNo.Text.Trim());
                TxtDDDate.Text = ClearInject(TxtDDDate.Text.Trim());
                TxtIssueBranch.Text = ClearInject(TxtIssueBranch.Text.Trim());
                TxtRemarks.Text = ClearInject(TxtRemarks.Text.Trim());

                string FlNm = "";
                string flnm1 = "";

                DateTime ChqDate;

                try
                {
                    ChqDate = Convert.ToDateTime(TxtDDDate.Text);
                }
                catch
                {
                    ChqDate = DateTime.Now;
                }

                // -------------------- Validate DD fields ------------------------
                bool flag;

                if (Convert.ToInt32(DdlPaymode.SelectedValue) == 1)
                {
                    flag = true;
                }
                else
                {
                    if (divDDno.Visible && TxtDDNo.Text == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(),
                            "Close", "<script>alert('" + LblDDNo.Text + " can not be blank');</script>", false);
                        return;
                    }
                    else
                    {
                        flag = CheckDDno();

                        if (DivBranch.Visible)
                        {
                            if (TxtIssueBranch.Text == "")
                            {
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(),
                                    "Close", "<script>alert('Branch name can not be blank');</script>", false);
                                return;
                            }
                        }
                    }
                }

                // ----------------------- File Upload ------------------------
                string strextension = "";
                // CompID folder root
                string compId = Session["CompID"] != null ? Session["CompID"].ToString() : "0";
                string uploadRoot = Server.MapPath("~/images/UploadImage/" + compId + "/");
                if (!Directory.Exists(uploadRoot))
                {
                    Directory.CreateDirectory(uploadRoot);
                }
                try
                {
                    if (flag)
                    {
                        if (FlDoc.HasFile)
                        {
                            strextension = Path.GetExtension(FlDoc.FileName);

                            string ext = strextension.ToUpper();

                            if (ext == ".JPG" || ext == ".GIF" || ext == ".JPEG" ||
                                ext == ".BMP" || ext == ".PNG")
                            {
                                if (LblImage.Text == "True")
                                {
                                    //flnm1 = DateTime.Now.ToString("yyMMddhhmmssfff") + Path.GetExtension(FlDoc.PostedFile.FileName);

                                    //string fullpath = Server.MapPath("images/UploadImage/") + flnm1;

                                    //FlDoc.PostedFile.SaveAs(fullpath);

                                    //CompressAndSaveImage(FlDoc.PostedFile.InputStream, fullpath, strextension, 50L);

                                    //FlNm = flnm1;
                                    flnm1 = DateTime.Now.ToString("yyMMddhhmmssfff") + Path.GetExtension(FlDoc.PostedFile.FileName);
                                    //Fuidentity.PostedFile.SaveAs(Server.MapPath("images/UploadImage/") + flAddrs);
                                    //string savePath = Server.MapPath("images/UploadImage/") + flAddrs;
                                    string fullPath = Path.Combine(uploadRoot, flnm1);
                                    FlDoc.PostedFile.SaveAs(fullPath);
                                    CompressAndSaveImage(FlDoc.PostedFile.InputStream, fullPath, strextension, 50L); // Quality 50%
                                    FlNm = flnm1;
                                    // FlNm = "https://" + HttpContext.Current.Request.Url.Host + "/images/UploadImage/" + compId + "/" + flnm1;
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(),
                                        "Close", "<script>alert('File size must not exceed 10 mb');</script>", false);
                                    return;
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(),
                                    "Close", "<script>alert('You can upload only .jpg,.gif,.jpeg,.bmp,.png extension file!!');</script>", false);
                                return;
                            }
                        }
                        else
                        {
                            FlNm = "";
                        }

                        // Use previously uploaded image if exists
                        FlNm = Convert.ToString(Session["WalletImage"]);

                        // ---------------------- Insert Request -----------------------
                        string str = "INSERT INTO WalletReq(ReqNo,ReqDate,Formno,PID,Paymode,Amount,ChqNo,ChqDate," +
                                     "BankName,BranchName,ScannedFile,Remarks,BankId,Transno) " +
                                     "Select ISNULL(Max(ReqNo)+1,'1001'),'" +
                                     DateTime.Now.ToString("dd-MMM-yyyy") + "','" +
                                     Convert.ToInt32(Session["Formno"]) + "','" +
                                     Convert.ToInt32(DdlPaymode.SelectedValue) + "','" +
                                     DdlPaymode.SelectedItem.Text.Trim() + "','" +
                                     Convert.ToDecimal(TxtAmount.Text) + "','" +
                                     TxtDDNo.Text.Trim() + "','" +
                                     ChqDate.ToString("dd-MMM-yyyy") + "','" +
                                     DDlBank.SelectedItem.Text.Trim() + "','" +
                                     TxtIssueBranch.Text.Trim() + "','" +
                                     FlNm + "','" +
                                     TxtRemarks.Text.Trim() + "','" +
                                     Convert.ToInt32(DDlBank.SelectedValue) + "','0' FROM WalletReq " +

                                     ";INSERT INTO UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)" +
                                     "VALUES('" + Session["FormNo"] + "','" + Session["MemName"] +
                                     "','Payment Request','Payment Request','Amount: " +
                                     Convert.ToDecimal(TxtAmount.Text) + "',Getdate()," +
                                     Session["FormNo"] + ")";

                        int i = ObjDAL.SaveData(str);

                        // --------------------- Fetch ReqNo ------------------------
                        dt1 = new DataTable();
                        string q2 = Obj.IsoStart + " Select Max(ReqNo) as ReqNo FROM " + Obj.dBName + "..WalletReq WHERE Formno='" +
                                    Convert.ToInt32(Session["Formno"]) +
                                    "' AND Amount='" + Convert.ToDecimal(TxtAmount.Text) + "'" + Obj.IsoEnd;

                        dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, q2).Tables[0];

                        string ReqNo = "";
                        if (dt1.Rows.Count > 0)
                            ReqNo = dt1.Rows[0]["ReqNo"].ToString();

                        // ------ Success Message ------
                        scrname = "<script>alert('Payment Request Sent Successfully.\\nYour Request no. is " +
                                  ReqNo + "');location.replace('WalletRequest.aspx');</script>";

                        Session["ScrName"] = scrname;

                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);

                        // Reset UI
                        TxtAmount.Text = "";
                        TxtDDDate.Text = "";
                        TxtDDNo.Text = "";
                        TxtIssueBranch.Text = "";
                        TxtRemarks.Text = "";

                        // Send SMS/Email to member except listed companies
                        //if (!(Session["CompId"] is "1066" or "1068" or "1069" or "1070" or "1072" or "1073" or "1074" or "1075" or "1082" or "1090" or "1091" or "1092" or "1093" or "1095" or "1097" or "1100" or "1101" or "1103"))
                        //{
                        //    SendToMemberMail(ReqNo);
                        //}
                        string comp = Session["CompId"]?.ToString();

                        if (!new[]
                        {
    "1066","1068","1069","1070","1072","1073","1074","1075",
    "1082","1090","1091","1092","1093","1095","1097",
    "1100","1101","1103","1108","1109","1110"
}.Contains(comp))
                        {
                            SendToMemberMail(ReqNo);
                        }
                        FillPaymode();
                        CheckVisible();
                    }
                }
                catch (Exception ex2)
                {
                    string path = HttpContext.Current.Request.Url.AbsoluteUri;
                    string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
                    Obj.WriteToFile(text + ex2.Message);
                    Response.Write("Try later.");
                }
            }
            else
            {
                Response.Redirect("WalletRequest.aspx");
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    [WebMethod(EnableSession = true)]
    public static string CheckDuplicateTxn(string ddno, string paymode)
    {
        try
        {
            ddno = (ddno ?? "").Trim().Replace("'", "");   // basic guard
            if (ddno == "") return "0";

            // paymode 10 par DD/Txn check nahi hota
            int pm;
            if (int.TryParse(paymode, out pm) && pm == 10) return "0";

            var ses = HttpContext.Current.Session;
            if (ses["CompID"] == null) return "0";

            string compId = ses["CompID"].ToString();
            string dbSession = ses["MlmSelectDatabase" + compId] != null
                                ? ses["MlmSelectDatabase" + compId].ToString() : "";

            DAL Obj = new DAL();
            string s;
            DataTable dt;

            // --- 1) WalletReq (sab company ke liye) ---
            s = Obj.IsoStart + "select * from " + Obj.dBName +
                "..WalletReq where ChqNo='" + ddno + "' and IsApprove<>'R'" + Obj.IsoEnd;
            dt = SqlHelper.ExecuteDataset(dbSession, CommandType.Text, s).Tables[0];
            if (dt.Rows.Count > 0) return "1";

            // --- 2) COMP 1091 => TrnProductorderDetail bhi check ---
            if (compId == "1091")
            {
                s = Obj.IsoStart + "select * from " + Obj.dBName +
                    "..TrnProductorderDetail where txnid='" + ddno + "' and IsApprove<>'R'" + Obj.IsoEnd;
                dt = SqlHelper.ExecuteDataset(dbSession, CommandType.Text, s).Tables[0];
                if (dt.Rows.Count > 0) return "1";
            }

            return "0";   // koi duplicate nahi
        }
        catch
        {
            return "0";   // error pe block mat karo (jaise aapke purane catch{} me tha)
        }
    }
    public bool SendToMemberMail(string RequestNo)
    {
        try
        {
            string StrMsg = "";

            MailAddress SendFrom = new MailAddress(Session["CompMail"].ToString());
            MailAddress SendTo = new MailAddress(Session["EMail"].ToString());

            MailMessage MyMessage = new MailMessage(SendFrom, SendTo);

            StrMsg =
                "<table style='margin:0; padding:10px; font-size:12px; font-family:Verdana, Arial, Helvetica, sans-serif; line-height:23px; text-align:justify;width:100%'>" +
                "<tr>" +
                "<td>" +
                "<span style='color: #0099CC; font-weight: bold;'><h2>Dear " + Session["MemName"] + ",</h2></span><br />" +
                "<h4>Payment Request Sent Successfully. Your Request no. is " + RequestNo + "</h4><br />" +
                "<br/> For login go to our site : <a href='" + Session["CompWeb"] + "' target='_blank' style='color:#0000FF; text-decoration:underline;'>" + Session["CompWeb"] + "</a><br/>" +
                "Thank you!<br> Regards : <br/><a href='" + Session["CompWeb"] + "' target='_blank' style='color:#0000FF; text-decoration:underline;'>" + Session["CompName"] + "</a><br />" +
                "<br /><br />" +
                "</td>" +
                "</tr>" +
                "</table>";

            MyMessage.Subject = "Wallet Request";
            MyMessage.Body = StrMsg;
            MyMessage.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            // SmtpClient smtp = new SmtpClient(Session["MailHost"].ToString());

            smtp.UseDefaultCredentials = false;
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtp.Credentials = new NetworkCredential(
                Session["CompMail"].ToString(),
                Session["MailPass"].ToString()
            );

            smtp.Send(MyMessage);

            return true;
        }
        catch (Exception)
        {
            Response.Write("Try later.");
        }

        return false;
    }
}