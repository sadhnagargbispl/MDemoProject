using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text.pdf.security;
using iTextSharp.tool.xml.parser.state;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public partial class KYCUpdate : System.Web.UI.Page
{
    double dblBank;
    DataTable tmpTable = new DataTable();
    DAL Obj;
    clsGeneral objGen = new clsGeneral();
    // PDF thumbnail inline icon (koi physical file ki zaroorat nahi)
    private const string PdfIconDataUri = "data:image/svg+xml;base64,PHN2ZyB4bWxucz0naHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmcnIHZpZXdCb3g9JzAgMCA2NCA2NCc+PHJlY3QgeD0nMTInIHk9JzQnIHdpZHRoPSczNicgaGVpZ2h0PSc1Nicgcng9JzQnIGZpbGw9JyNmZmZmZmYnIHN0cm9rZT0nI2U1MzkzNScgc3Ryb2tlLXdpZHRoPSczJy8+PHBhdGggZD0nTTQwIDQgbDggOCBoLTggeicgZmlsbD0nI2U1MzkzNScvPjxyZWN0IHg9JzgnIHk9JzM0JyB3aWR0aD0nNDAnIGhlaWdodD0nMTYnIHJ4PScyJyBmaWxsPScjZTUzOTM1Jy8+PHRleHQgeD0nMjgnIHk9JzQ2JyBmb250LWZhbWlseT0nQXJpYWwsSGVsdmV0aWNhLHNhbnMtc2VyaWYnIGZvbnQtc2l6ZT0nMTEnIGZvbnQtd2VpZ2h0PSdib2xkJyBmaWxsPScjZmZmZmZmJyB0ZXh0LWFuY2hvcj0nbWlkZGxlJz5QREY8L3RleHQ+PC9zdmc+";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string sql = @"CREATE TABLE [dbo].[Trnjoining](
[ID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
[Transid] [numeric](18, 0) NOT NULL,
[Rectimestamp] [datetime] DEFAULT (getdate()) NOT NULL)";
            int i;
            try
            {
                i = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(),
                                              CommandType.Text, sql);
            }
            catch { }
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                if (!Page.IsPostBack)
                {
                    HdnCheckTrnns.Value = GenerateRandomStringJoining(6);
                    if (Session["CompID"].ToString() == "1007")
                    {
                        if (!GetKycPerStatus())
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key",
                                "alert('Kyc option is temporarily off. Please try after some time.!!');location.replace('" + Session["IndexPage"] + "');", true);
                            return;
                        }
                    }
                    if (Session["CompID"].ToString() == "1101")
                    {
                        if (!GetKycPerStatusCheck())
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Key",
                                "alert('We are unable to process your KYC because your ID is not active. Please activate your ID and you can complete your KYC smoothly.');location.replace('" + Session["IndexPage"] + "');", true);
                            return;
                        }
                    }
                    if (
                        Session["CompID"].ToString() == "1082" || Session["CompID"].ToString() == "1074" ||
                        Session["CompID"].ToString() == "1084" || Session["CompID"].ToString() == "1089" ||
                        Session["CompID"].ToString() == "1091" || Session["CompID"].ToString() == "1092" ||
                        Session["CompID"].ToString() == "1093" || Session["CompID"].ToString() == "1095" ||
                        Session["CompID"].ToString() == "1097" || Session["CompID"].ToString() == "1100" ||
                        Session["CompID"].ToString() == "1102" || Session["CompID"].ToString() == "1101" ||
                        Session["CompID"].ToString() == "1106" || Session["CompID"].ToString() == "1107" ||
                        Session["CompID"].ToString() == "1108" || Session["CompID"].ToString() == "1110" ||
                        Session["CompID"].ToString() == "1105"
                    )
                    { Accountype.Visible = true; }
                    else { Accountype.Visible = false; }

                    Fill_State();
                    FillIdtypeMaster();
                    FillBankMaster();
                    loadImages();

                    // ===== COMPANY (1108 + RegType=C): labels + dropdown + per-type proof fields =====
                    if (IsCompanyKyc())
                    {
                        ApplyCompanyLabels();
                        BuildCompanyDropdown();
                        ShowCompProofFields();
                        // Company flag - Aadhaar 12-digit validation submit (SaveButton) par hogi. AutoPostBack markup me false (no reload).
                        hdnIsCompany.Value = "1";
                    }
                }
            }
            else { Response.Redirect("logout.aspx"); }
        }
        catch (Exception ex) { Response.Write("Try later."); }
    }

    // ===================== COMPANY KYC HELPERS =====================
    private bool IsCompanyKyc()
    {
        string compId = HttpContext.Current.Session["CompID"] != null
            ? HttpContext.Current.Session["CompID"].ToString().Trim() : "";
        if (compId != "1108")
            return false;
        string regType = HttpContext.Current.Session["RegType"] != null
            ? HttpContext.Current.Session["RegType"].ToString().Trim().ToUpper() : "";
        return regType == "C";   // >>> ADJUST: Company ki exact value (C / COMPANY)
    }

    // Company member ke liye labels (sirf text change). Aadhaar ki jagah "Authorized".
    private void ApplyCompanyLabels()
    {
        try
        {
            LblKycHead.Text = "Company KYC Detail";
            LblAddressHead.Text = "Company Address Proof";
            LblBankHead.Text = "Company Bank Detail";
            LblPanHead.Text = "Company PAN Detail";
            LblAddresProof.Text = "Authorized Person Aadhaar No.";
            LblFrontUpload.Text = "Authorized Aadhaar Front Upload:";
            LblBackUpload.Text = "Authorized Aadhaar Back Upload:";
            // Uploaded Images captions bhi Authorized
            try { LblImgFront.Text = "Authorized Aadhaar Front"; } catch { }
            try { LblImgBack.Text = "Authorized Aadhaar Back"; } catch { }
        }
        catch { }
    }

    // Sirf selected proof type ke fields dikhao (baaki hide)
    private void ShowCompProofFields()
    {
        try
        {
            divBillNo.Visible = divBillUpload.Visible = true;
            divGstNo.Visible = divGstUpload.Visible = true;
            divOtherNo.Visible = divOtherUpload.Visible = true;

            string s = DDLAddressProof.SelectedItem != null ? DDLAddressProof.SelectedItem.Text.Trim().ToUpper() : "";
            bool bill = (s == "LIGHT BILL");
            bool gst = (s == "GSTIN");
            bool oth = (!bill && !gst);

            SetDisp(divBillNo, bill); SetDisp(divBillUpload, bill);
            SetDisp(divGstNo, gst); SetDisp(divGstUpload, gst);
            SetDisp(divOtherNo, oth); SetDisp(divOtherUpload, oth);
        }
        catch { }
    }
    private void SetDisp(System.Web.UI.HtmlControls.HtmlControl ctl, bool show)
    {
        try { ctl.Style["display"] = show ? "" : "none"; } catch { }
    }

    // Company proof file upload helper (jpg/jpeg/png/pdf, 5MB; pdf compress nahi)
    private bool UploadCompFile(FileUpload fu, string prefix, string compId, string uploadRoot, out string url)
    {
        url = "";
        string ext = System.IO.Path.GetExtension(fu.FileName);
        if (!(ext.ToUpper() == ".JPG" || ext.ToUpper() == ".JPEG" || ext.ToUpper() == ".PNG" || ext.ToUpper() == ".PDF"))
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close",
                "<SCRIPT language='javascript'>alert('Only jpg/jpeg/png/pdf allowed!! ');</SCRIPT>", false);
            return false;
        }
        decimal size = Math.Round((decimal)(fu.PostedFile.ContentLength) / 1024, 1);
        if (size > 5120)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close",
                "<SCRIPT language='javascript'>alert('File up to 5 MB only!! ');</SCRIPT>", false);
            return false;
        }
        string name = prefix + DateTime.Now.ToString("yyMMddhhmmssfff") + Session["formno"].ToString() + ext;
        string path = Path.Combine(uploadRoot, name);
        fu.PostedFile.SaveAs(path);
        if (ext.ToUpper() != ".PDF")
            CompressAndSaveImage(fu.PostedFile.InputStream, path, ext, 50L);
        url = "https://" + HttpContext.Current.Request.Url.Host + "/images/UploadImage/" + compId + "/" + name;
        return true;
    }

    // Company ke time DDLAddressProof me sirf 3 options: Light Bill, GSTIN, Other
    // >>> ADJUST: ye Value (Id) M_IdTypeMaster ke actual Id se match karo (KycVerify.Idtype me jaata hai)
    private void BuildCompanyDropdown()
    {
        try
        {
            string prev = DDLAddressProof.SelectedValue;
            DDLAddressProof.Items.Clear();
            DDLAddressProof.Items.Add(new System.Web.UI.WebControls.ListItem("Light Bill", "4")); // >>> ADJUST Id
            DDLAddressProof.Items.Add(new System.Web.UI.WebControls.ListItem("GSTIN", "5"));      // >>> ADJUST Id
            DDLAddressProof.Items.Add(new System.Web.UI.WebControls.ListItem("Other", "6"));      // >>> ADJUST Id
            System.Web.UI.WebControls.ListItem sel = DDLAddressProof.Items.FindByValue(prev);
            if (sel != null)
                DDLAddressProof.SelectedValue = prev;
            DDLAddressProof.Attributes.Add("onchange", "onCompProofChange(this)");
        }
        catch { }
    }
    // ===============================================================

    public string GenerateRandomStringJoining(int length)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        string sResult = "";
        for (int i = 0; i < length; i++) { sResult += allowChrs[rdm.Next(allowChrs.Length)]; }
        return sResult;
    }
    private void loadImages()
    {
        try
        {
            int c = 0;
            string status = "";
            string str = "";
            DataTable dt = new DataTable();
            str = Obj.IsoStart + "Exec sp_FillKyc " + Convert.ToInt32(Session["Formno"]) + Obj.IsoEnd;
            dt = SqlHelper.ExecuteDataset((string)HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]], CommandType.Text, str).Tables[0];
            if ((dt.Rows.Count > 0))
            {
                lblid.Text = dt.Rows[0]["idno"].ToString();
                hdnSessn.Value = Crypto.Encrypt(dt.Rows[0]["idno"].ToString());
                txtaddrs.Text = dt.Rows[0]["Address1"].ToString();
                Txtpincode.Text = dt.Rows[0]["Pincode"].ToString();
                Txtcity.Text = dt.Rows[0]["City"].ToString();
                Txtdistrict.Text = dt.Rows[0]["District"].ToString();
                StateCode.Value = dt.Rows[0]["Statecode"].ToString();
                HDistrictCode.Value = dt.Rows[0]["Districtcode"].ToString();
                HCityCode.Value = dt.Rows[0]["Citycode"].ToString();
                ddlState.SelectedItem.Text = dt.Rows[0]["StateName"].ToString();
                lblverstatus.Text = dt.Rows[0]["idVerf"].ToString();
                DDLAddressProof.SelectedValue = dt.Rows[0]["Id"].ToString();
                DDlVillage.SelectedValue = dt.Rows[0]["areacode"].ToString();
                Lblverdate.Text = dt.Rows[0]["AddrProofDate"] == DBNull.Value ? "" : dt.Rows[0]["AddrProofDate"].ToString();
                LblRemark.Text = dt.Rows[0]["AdressRejectRemark"].ToString();
                LbLrejectRemark.Text = dt.Rows[0]["AddressRejectReason"].ToString();
                status = dt.Rows[0]["Idverf"].ToString();
                TxtIdProofNo.Text = dt.Rows[0]["IdProofNo"].ToString();
                Txtacno.Text = dt.Rows[0]["Acno"].ToString();
                lblverstatus.Text = dt.Rows[0]["BankVerf"].ToString();
                Txtcode.Text = dt.Rows[0]["IFscode"].ToString();
                Txtbranch.Text = dt.Rows[0]["Branchname"].ToString();
                cmbbank.SelectedValue = dt.Rows[0]["BAnkid"].ToString();
                DDLAccountType.SelectedValue = dt.Rows[0]["Fax"].ToString();
                Lblverdate.Text = dt.Rows[0]["BankProofDate"] == DBNull.Value ? "" : dt.Rows[0]["BankProofDate"].ToString();
                txtpan.Text = dt.Rows[0]["Panno"].ToString();
                // Company: per-type proof fetch (Bill/GST/Other - alag columns)
                if (IsCompanyKyc())
                {
                    string billNo = dt.Columns.Contains("CompBillNo") ? dt.Rows[0]["CompBillNo"].ToString() : "";
                    string billImg = dt.Columns.Contains("CompBillImg") ? dt.Rows[0]["CompBillImg"].ToString() : "";
                    string gstNo = dt.Columns.Contains("CompGstNo") ? dt.Rows[0]["CompGstNo"].ToString() : "";
                    string gstImg = dt.Columns.Contains("CompGstImg") ? dt.Rows[0]["CompGstImg"].ToString() : "";
                    string otherNo = dt.Columns.Contains("CompOtherNo") ? dt.Rows[0]["CompOtherNo"].ToString() : "";
                    string otherImg = dt.Columns.Contains("CompOtherImg") ? dt.Rows[0]["CompOtherImg"].ToString() : "";

                    txtBillNo.Text = billNo;
                    txtGstNo.Text = gstNo;
                    txtOtherNo.Text = otherNo;

                    BindCompanyProof(billImg, imgBill, BillAnchor, LblBillImg, FuBill);
                    BindCompanyProof(gstImg, imgGst, GstAnchor, LblGstImg, FuGst);
                    BindCompanyProof(otherImg, imgOther, OtherAnchor, LblOtherImg, FuOther);

                    // Uploaded Images me sirf SELECTED type ka proof dikhe (baaki hide)
                    string selProof = DDLAddressProof.SelectedItem != null ? DDLAddressProof.SelectedItem.Text.Trim().ToUpper() : "";
                    bool selBill = (selProof == "LIGHT BILL");
                    bool selGst = (selProof == "GSTIN");
                    bool selOther = (!selBill && !selGst);
                    divBillImg.Visible = selBill && !string.IsNullOrEmpty(billImg);
                    divGstImg.Visible = selGst && !string.IsNullOrEmpty(gstImg);
                    divOtherImg.Visible = selOther && !string.IsNullOrEmpty(otherImg);

                    // bhara hua to disable (Rejected/empty pe neeche wapas enable)
                    if (!string.IsNullOrEmpty(billNo)) txtBillNo.Enabled = false;
                    if (!string.IsNullOrEmpty(billImg)) FuBill.Enabled = false;
                    if (!string.IsNullOrEmpty(gstNo)) txtGstNo.Enabled = false;
                    if (!string.IsNullOrEmpty(gstImg)) FuGst.Enabled = false;
                    if (!string.IsNullOrEmpty(otherNo)) txtOtherNo.Enabled = false;
                    if (!string.IsNullOrEmpty(otherImg)) FuOther.Enabled = false;
                }
                if (dt.Rows[0]["Fax"].ToString().ToUpper() != "CHOOSE ACCOUNT TYPE") { DDLAccountType.Enabled = false; }
                else { DDLAccountType.Enabled = true; c++; }

                if (string.IsNullOrEmpty(TxtIdProofNo.Text)) TxtIdProofNo.Enabled = true; else TxtIdProofNo.Enabled = false;
                if (string.IsNullOrEmpty(txtaddrs.Text)) txtaddrs.Enabled = true; else txtaddrs.Enabled = false;
                if (string.IsNullOrEmpty(txtpan.Text)) txtpan.Enabled = true; else txtpan.Enabled = false;
                if (string.IsNullOrEmpty(Txtpincode.Text)) Txtpincode.Enabled = true; else Txtpincode.Enabled = false;
                if (string.IsNullOrEmpty(ddlState.SelectedItem.Text) || ddlState.SelectedItem.Text.ToString() == "--Choose State Name--") ddlState.Enabled = true; else ddlState.Enabled = false;
                if (string.IsNullOrEmpty(Txtdistrict.Text) || Txtdistrict.Text == "0") Txtdistrict.Enabled = true; else Txtdistrict.Enabled = false;
                if (string.IsNullOrEmpty(Txtcity.Text) || Txtcity.Text == "0") Txtcity.Enabled = true; else Txtcity.Enabled = false;
                if (string.IsNullOrEmpty(DDLAddressProof.SelectedItem.Text) || DDLAddressProof.SelectedItem.Text.ToString() == "--Choose Id Proof--") DDLAddressProof.Enabled = true; else DDLAddressProof.Enabled = false;
                if (string.IsNullOrEmpty(cmbbank.SelectedItem.Text) || cmbbank.SelectedItem.Text.ToString() == "--Choose Bank Name--") cmbbank.Enabled = true; else cmbbank.Enabled = false;
                if (string.IsNullOrEmpty(Txtbranch.Text)) Txtbranch.Enabled = true; else Txtbranch.Enabled = false;
                if (string.IsNullOrEmpty(Txtcode.Text)) Txtcode.Enabled = true; else Txtcode.Enabled = false;
                BindImage(dt.Rows[0]["AddrProof"].ToString(), ShowIdentity, FrontAddress, lblimage, Fuidentity);
                BindImage(dt.Rows[0]["BackAddressProof"].ToString(), showBackImage, BackAddress, LblBackImage, FileUpload1);
                BindImage(dt.Rows[0]["BankProof"].ToString(), bANKiMAGE, BankProof, LblBankImage, BankKYCFileUpload3);
                BindImage(dt.Rows[0]["PanImg"].ToString(), pANiMAGE, PanCard, LblPanImage, Pankyc);
                if (!string.IsNullOrEmpty(dt.Rows[0]["AddrProof"].ToString()) && dt.Rows[0]["IsAddrssVerified"].ToString() != "R")
                { Fuidentity.Enabled = false; Fuidentity.Attributes.Add("class", "form-control"); }
                else { Fuidentity.Enabled = true; c++; }
                if (!string.IsNullOrEmpty(dt.Rows[0]["BackAddressProof"].ToString()) && dt.Rows[0]["IsAddrssVerified"].ToString() != "R")
                { FileUpload1.Enabled = false; FileUpload1.Attributes.Add("class", "form-control"); }
                else { FileUpload1.Enabled = true; c++; }
                if (!string.IsNullOrEmpty(LblBankImage.Text) && dt.Rows[0]["IsBankVerified"].ToString() != "R")
                { BankKYCFileUpload3.Enabled = false; BankKYCFileUpload3.Attributes.Add("class", "form-control"); }
                else { BankKYCFileUpload3.Enabled = true; c++; }
                if (!string.IsNullOrEmpty(LblPanImage.Text) && dt.Rows[0]["IsPanVerified"].ToString() != "R")
                { Pankyc.Enabled = false; Pankyc.Attributes.Add("class", "form-control"); }
                else { Pankyc.Enabled = true; c++; }
                if (dt.Rows[0]["IsBankVerified"].ToString() != "R" && (!string.IsNullOrEmpty(dt.Rows[0]["BankProof"].ToString()) || Txtacno.Text.Length > 10))
                { Txtacno.Enabled = false; }
                else { Txtacno.Enabled = true; c++; }
            }
            if (dt.Rows[0]["IsAddrssVerified"].ToString() != "R" && c == 0)
            { BtnIdentity.Visible = false; LbLrejectRemark.Text = ""; }
            else BtnIdentity.Visible = true;
            if (status == "Verification Due")
            {
                VerifyDate.Visible = false; Lblverdate.Visible = false; LblVerfReason.Visible = false; LblVerfRemark.Visible = false;
                LbLrejectRemark.Text = ""; VerifyDate.Text = ""; DivVerify.Attributes.Add("style", "color:black");
            }
            else if (status == "")
            {
                VerifyDate.Visible = true; Lblverdate.Visible = true; LblVerfReason.Visible = false; LblVerfRemark.Visible = false;
                VerifyDate.Text = ""; DivVerify.Attributes.Add("style", "color:black");
                TxtIdProofNo.Enabled = true; txtaddrs.Enabled = true; txtpan.Enabled = true; Txtpincode.Enabled = true;
                ddlState.Enabled = true; Txtdistrict.Enabled = true; Txtcity.Enabled = true; DDLAddressProof.Enabled = true;
                DDLAccountType.Enabled = true; cmbbank.Enabled = true; Txtbranch.Enabled = true; Txtcode.Enabled = true;
                Fuidentity.Enabled = true; FileUpload1.Enabled = true; BankKYCFileUpload3.Enabled = true; Pankyc.Enabled = true;
                Txtacno.Enabled = true; BtnIdentity.Visible = true;
                if (IsCompanyKyc()) { txtBillNo.Enabled = true; FuBill.Enabled = true; txtGstNo.Enabled = true; FuGst.Enabled = true; txtOtherNo.Enabled = true; FuOther.Enabled = true; }
            }
            else if (status == "Rejected")
            {
                VerifyDate.Visible = true; Lblverdate.Visible = true; LblVerfReason.Visible = true; LblVerfRemark.Visible = true;
                VerifyDate.Text = "Reject Date:"; DivVerify.Attributes.Add("style", "color:red");
                TxtIdProofNo.Enabled = true; txtaddrs.Enabled = true; txtpan.Enabled = true; Txtpincode.Enabled = true;
                ddlState.Enabled = true; Txtdistrict.Enabled = true; Txtcity.Enabled = true; DDLAddressProof.Enabled = true;
                DDLAccountType.Enabled = true; cmbbank.Enabled = true; Txtbranch.Enabled = true; Txtcode.Enabled = true;
                Txtacno.Enabled = true; BtnIdentity.Visible = true;
                if (IsCompanyKyc()) { txtBillNo.Enabled = true; FuBill.Enabled = true; txtGstNo.Enabled = true; FuGst.Enabled = true; txtOtherNo.Enabled = true; FuOther.Enabled = true; }
            }
            else
            {
                VerifyDate.Visible = true; Lblverdate.Visible = true; LblVerfReason.Visible = false; LblVerfRemark.Visible = false;
                LbLrejectRemark.Text = ""; VerifyDate.Text = "Verify Date:"; DivVerify.Attributes.Add("style", "color:Green");
            }
            LblVerification.Visible = true;
        }
        catch (Exception ex) { }
    }
    private bool GetKycPerStatusCheck()
    {
        try
        {
            bool result = false;
            DataSet Ds12 = new DataSet(); DataTable dt12 = new DataTable();
            string str12 = Obj.IsoStart + "select Activestatus from " + Obj.dBName + "..m_membermaster where formno = '" + Session["formno"] + "'" + Obj.IsoEnd;
            Ds12 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str12);
            dt12 = Ds12.Tables[0];
            if (dt12.Rows[0]["Activestatus"].ToString().ToUpper() == "Y") result = true; else result = false;
            return result;
        }
        catch { return false; }
    }
    private void FillBankMaster()
    {
        try
        {
            Obj = new DAL();
            string Strquery = Obj.IsoStart + "SELECT BankCode as Bid, BANKNAME as Bank FROM " + Obj.dBName + "..M_BankMaster WHERE ACTIVESTATUS='Y' and Rowstatus='Y' ORDER BY BANKCode" + Obj.IsoEnd;
            tmpTable = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Strquery).Tables[0];
            cmbbank.DataSource = tmpTable; cmbbank.DataValueField = "Bid"; cmbbank.DataTextField = "Bank"; cmbbank.DataBind(); cmbbank.SelectedIndex = 0;
        }
        catch (Exception ex) { Obj.WriteToFile(ex.Message); Response.Write("Try later."); }
    }
    protected void cmbbank_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbbank.SelectedItem.Text.ToUpper() == "OTHERS") { divBank.Visible = true; Txtbank.Focus(); Txtbank.Text = ""; }
        else { divBank.Visible = false; Txtbank.Text = ""; Txtbranch.Focus(); }
    }
    private void FillIdtypeMaster()
    {
        try
        {
            Obj = new DAL();
            string strQuery = Obj.IsoStart + "SELECT Id, IdType FROM " + Obj.dBName + "..M_IdTypeMaster WHERE ACTIVESTATUS='Y'" + Obj.IsoEnd;
            tmpTable = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, strQuery).Tables[0];
            DDLAddressProof.DataSource = tmpTable; DDLAddressProof.DataValueField = "Id"; DDLAddressProof.DataTextField = "IdType"; DDLAddressProof.DataBind(); DDLAddressProof.SelectedIndex = 0;
            LblIdproofText.Text = "";
            for (int s = 0; s < tmpTable.Rows.Count; s++)
            { if (tmpTable.Rows[s]["Id"].ToString() != "0") { LblIdproofText.Text += tmpTable.Rows[s]["IdType"].ToString() + ","; } }
            if (!string.IsNullOrEmpty(LblIdproofText.Text)) { LblIdproofText.Text = LblIdproofText.Text.Remove(LblIdproofText.Text.Length - 1, 1); }
        }
        catch (Exception e) { Obj.WriteToFile(e.Message); Response.Write("Try later."); }
    }
    private void Fill_State()
    {
        try
        {
            Obj = new DAL(); DataTable dt = new DataTable();
            string str = Obj.IsoStart + "Select StateCode, StateName from " + Obj.dBName + "..M_STateDivMaster Where ActiveStatus='Y' and RowStatus='Y' Order by StateCode" + Obj.IsoEnd;
            dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];
            if (dt.Rows.Count > 0) { ddlState.DataSource = dt; ddlState.DataValueField = "StateCode"; ddlState.DataTextField = "StateName"; ddlState.DataBind(); }
        }
        catch { }
    }
    protected void Txtpincode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string sql = "";
            if (Convert.ToInt32(Txtpincode.Text) != 0)
            {
                sql = Obj.IsoStart + "select a.Statename, b.DistrictName, c.CityName, d.VillageName, d.Pincode, a.StateCode, b.DistrictCode, c.CityCode, d.VillageCode " +
                      "from " + Obj.dBName + "..M_STateDivMaster a with(NoLock) " +
                      "inner join " + Obj.dBName + "..M_DistrictMaster b with(NoLock) on a.StateCode=b.StateCode and a.ActivEstatus='Y' and b.ActiveStatus='Y' " +
                      "inner join " + Obj.dBName + "..M_CityStatemaster c with(NoLock) on b.DistrictCode=c.DistrictCode and c.ActivEstatus='Y' " +
                      "inner join " + Obj.dBName + "..M_VillageMaster d with(NoLock) on c.CityCode=d.CityCode and d.ActiveStatus='Y' " +
                      "where d.Pincode='" + Convert.ToInt32(Txtpincode.Text) + "' Union all select '' as StateName, '' as DistrictName, '' as CityName, 'Others', '', 0, 0, 0, 381264" + Obj.IsoEnd;
                DataTable Dt = new DataTable(); Obj = new DAL();
                Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];
                if (Dt.Rows.Count > 0)
                {
                    ddlState.SelectedItem.Text = Dt.Rows[0]["StateName"].ToString(); StateCode.Value = Dt.Rows[0]["StateCode"].ToString();
                    Txtdistrict.Text = Dt.Rows[0]["DistrictName"].ToString(); HDistrictCode.Value = Dt.Rows[0]["DistrictCode"].ToString();
                    Txtcity.Text = Dt.Rows[0]["CityName"].ToString(); HCityCode.Value = Dt.Rows[0]["CityCode"].ToString();
                    DDlVillage.DataSource = Dt; DDlVillage.DataValueField = "VillageCode"; DDlVillage.DataTextField = "VillageName"; DDlVillage.DataBind(); DDlVillage.SelectedIndex = 0; DDlVillage.Focus();
                }
                else
                {
                    Txtpincode.Focus(); ddlState.Items.Clear(); StateCode.Value = "0"; Txtcity.Text = ""; HCityCode.Value = "0"; Txtdistrict.Text = ""; HDistrictCode.Value = "0"; DDlVillage.Items.Clear();
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Permanent Pincode Not exist.');", true);
                }
            }
        }
        catch (Exception ex) { Obj.WriteToFile(ex.Message); Response.Write("Try later."); }
    }
    private bool GetKycPerStatus()
    {
        try
        {
            bool result = false; DataSet Ds12 = new DataSet(); DataTable dt12 = new DataTable();
            string str12 = Obj.IsoStart + "Select IsWithDra From " + Obj.dBName + "..m_KycPer" + Obj.IsoEnd;
            Ds12 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str12);
            dt12 = Ds12.Tables[0];
            if (dt12.Rows[0]["IsWithDra"].ToString().ToUpper() == "Y") result = true;
            return result;
        }
        catch { return false; }
    }
    private string ClearInject(string StrObj) { return StrObj.Replace(";", "").Replace("'", "").Replace("=", ""); }
    protected void txtpan_TextChanged(object sender, EventArgs e)
    {
        if (Session["CompId"].ToString() == "1077" || Session["CompId"].ToString() == "1091")
        {
            if (panverify()) { BtnIdentity.Enabled = true; }
            else { BtnIdentity.Enabled = false; ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Pan card already register another id.!!');", true); txtpan.Text = ""; return; }
        }
        if (Session["CompId"].ToString() == "1074")
        {
            if (panverifycashless() == false) { BtnIdentity.Enabled = true; }
            else { BtnIdentity.Enabled = false; ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Pan card already register another id.!!');", true); txtpan.Text = ""; return; }
        }
    }
    protected void TxtIdProofNo_TextChanged(object sender, EventArgs e)
    {
        if (Session["CompId"].ToString() == "1091")
        {
            if (aadharverify()) { BtnIdentity.Enabled = true; }
            else { BtnIdentity.Enabled = false; ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Aadhar no already register another id.!!');", true); TxtIdProofNo.Text = ""; return; }
        }
    }
    private bool panverify()
    {
        try
        {
            bool result = false; DataSet Ds12 = new DataSet(); DataTable dt12 = new DataTable();
            string str12 = Obj.IsoStart + "select count(panno) as cnt from " + Obj.dBName + "..KycVerify as a, " + Obj.dBName + "..m_membermaster as b where a.formno=b.formno AND Panno<>'' and Ispanverified in ('Y','N') and panno='" + txtpan.Text + "'" + Obj.IsoEnd;
            Ds12 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str12);
            dt12 = Ds12.Tables[0];
            if (Convert.ToInt32(dt12.Rows[0]["cnt"]) > 1) result = false; else if (Convert.ToInt32(dt12.Rows[0]["cnt"]) == 0) result = true; else result = false;
            return result;
        }
        catch { return false; }
    }
    private bool panverifycashless()
    {
        try
        {
            bool result = false; DataSet Ds12 = new DataSet(); DataTable dt12 = new DataTable();
            string str12 = Obj.IsoStart + "select count(panno) as cnt from " + Obj.dBName + "..KycVerify as a, " + Obj.dBName + "..m_membermaster as b where a.formno=b.formno AND Panno<>'' and Ispanverified in ('Y','N') and panno='" + txtpan.Text + "'" + Obj.IsoEnd;
            Ds12 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str12);
            dt12 = Ds12.Tables[0];
            if (Convert.ToInt32(dt12.Rows[0]["cnt"]) >= 3) result = true; else if (Convert.ToInt32(dt12.Rows[0]["cnt"]) == 0) result = false; else result = false;
            return result;
        }
        catch { return false; }
    }
    private bool aadharverify()
    {
        try
        {
            bool result = false; DataSet Ds12 = new DataSet(); DataTable dt12 = new DataTable();
            string str12 = Obj.IsoStart + "select count(AadharNo) as cnt from " + Obj.dBName + "..KycVerify as a, " + Obj.dBName + "..m_membermaster as b where a.formno=b.formno AND AadharNo<>'' and IsAddrssVerified in ('N','Y') and AadharNo='" + TxtIdProofNo.Text + "'" + Obj.IsoEnd;
            Ds12 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str12);
            dt12 = Ds12.Tables[0];
            if (Convert.ToInt32(dt12.Rows[0]["cnt"]) > 1) result = false; else if (Convert.ToInt32(dt12.Rows[0]["cnt"]) == 0) result = true; else result = false;
            return result;
        }
        catch { return false; }
    }

    // ===== AJAX (onblur) duplicate check - KYC.aspx jaisa, par KYCUpdate ke tenant connection se =====
    // return true  = OK (duplicate nahi) ; return false = already registered (block)
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static bool VerifyAadhaar(string idProofNo)
    {
        try
        {
            idProofNo = (idProofNo ?? "").Replace("'", "").Replace(";", "").Replace("=", "").Trim();
            if (idProofNo == "") return true;
            var ses = HttpContext.Current.Session;
            DAL d = new DAL();
            string cs = ses["MlmSelectDatabase" + ses["CompID"]].ToString();
            string sql = d.IsoStart + "select count(AadharNo) as cnt from " + d.dBName + "..KycVerify as a, " + d.dBName + "..m_membermaster as b where a.formno=b.formno AND AadharNo<>'' and IsAddrssVerified in ('N','Y') and AadharNo='" + idProofNo + "'" + d.IsoEnd;
            DataTable dt = SqlHelper.ExecuteDataset(cs, CommandType.Text, sql).Tables[0];
            int cnt = Convert.ToInt32(dt.Rows[0]["cnt"]);
            return (cnt == 0);   // 0 = available
        }
        catch { return false; }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static bool panverifyC(string txtpan)
    {
        try
        {
            txtpan = (txtpan ?? "").Replace("'", "").Replace(";", "").Replace("=", "").Trim();
            if (txtpan == "") return true;
            var ses = HttpContext.Current.Session;
            DAL d = new DAL();
            string cs = ses["MlmSelectDatabase" + ses["CompID"]].ToString();
            string sql = d.IsoStart + "select count(panno) as cnt from " + d.dBName + "..KycVerify as a, " + d.dBName + "..m_membermaster as b where a.formno=b.formno AND Panno<>'' and Ispanverified in ('Y','N') and panno='" + txtpan + "'" + d.IsoEnd;
            DataTable dt = SqlHelper.ExecuteDataset(cs, CommandType.Text, sql).Tables[0];
            int cnt = Convert.ToInt32(dt.Rows[0]["cnt"]);
            return (cnt == 0);   // 0 = available
        }
        catch { return false; }
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlState.SelectedValue == "0") { BtnIdentity.Enabled = false; ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('please select State name.!!');", true); txtpan.Text = ""; return; }
        else { BtnIdentity.Enabled = true; }
    }
    private void BindImage(string dbValue, Image img, HtmlAnchor anchor, Label lbl, WebControl uploadControl)
    {
        if (string.IsNullOrEmpty(dbValue)) { img.ImageUrl = "~/images/no_photo.jpg"; anchor.HRef = "~/images/no_photo.jpg"; }
        else
        {
            img.ImageUrl = dbValue; anchor.HRef = dbValue;
            if (lbl != null) lbl.Text = dbValue;
            if (uploadControl != null) uploadControl.Attributes.Add("class", "form-control");
        }
    }
    // Company proof: PDF ho to thumbnail me pdf icon, click pe actual file (openPopup pdf ko nayi tab me kholega)
    private void BindCompanyProof(string dbValue, Image img, HtmlAnchor anchor, Label lbl, WebControl uploadControl)
    {
        if (string.IsNullOrEmpty(dbValue))
        {
            img.ImageUrl = "~/images/no_photo.jpg";
            anchor.HRef = "~/images/no_photo.jpg";
            return;
        }
        if (lbl != null) lbl.Text = dbValue;            // save ke liye purana url
        anchor.HRef = dbValue;                          // actual file (image/pdf)
        if (dbValue.ToLower().EndsWith(".pdf"))
            img.ImageUrl = PdfIconDataUri;              // pdf -> inline icon (koi file nahi chahiye)
        else
            img.ImageUrl = dbValue;                     // image -> wahi
        if (uploadControl != null) uploadControl.Attributes.Add("class", "form-control");
    }
    private void CompressAndSaveImage(Stream inputStream, string savePath, string extension, long quality = 50L)
    {
        using (System.Drawing.Image img = System.Drawing.Image.FromStream(inputStream))
        {
            EncoderParameters encoderParams = new EncoderParameters(1);
            ImageCodecInfo codec = null;
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.MimeType == "image/jpeg");
                    encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality); break;
                case ".png":
                    codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.MimeType == "image/png"); encoderParams = null; break;
                case ".gif":
                    codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.MimeType == "image/gif"); encoderParams = null; break;
                default: throw new Exception("Unsupported file type.");
            }
            if (codec != null) { if (encoderParams != null) img.Save(savePath, codec, encoderParams); else img.Save(savePath, codec, null); }
        }
    }
    protected void BtnIdentity_Click(object sender, EventArgs e)
    {
        string CompID = Session["CompID"].ToString();
        try
        {
            Obj = new DAL();
            string scrname = string.Empty;
            int i = 0;
            if (Session["CompID"] != null && Session["CompID"].ToString() == "1007")
            {
                if (!GetKycPerStatus()) { ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Kyc option is temporarily off. Please try after some time.!!');location.replace('" + Session["IndexPage"] + "');", true); return; }
            }
            string condition = "";
            DataTable dt1 = new DataTable();
            if (Session["CompID"] != null && Session["CompID"].ToString() == "1091")
            {
                if (DDLAddressProof.SelectedValue == "1")
                {
                    if (!string.IsNullOrWhiteSpace(TxtIdProofNo.Text))
                    {
                        string s1 = Obj.IsoStart + "select Count(IdProofNo) as AadharNo from " + Obj.dBName + "..KycVerify where IdProofNo='" + TxtIdProofNo.Text.Trim() + "' AND FORMNO<>'" + Convert.ToInt32(Session["FormNo"]) + "'" + Obj.IsoEnd;
                        dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s1).Tables[0];
                        if (dt1.Rows.Count > 0 && Convert.ToInt32(dt1.Rows[0]["AadharNo"]) >= 1)
                        { scrname = "<SCRIPT language='javascript'>alert('Already Registered by this Aadhar No.');</SCRIPT>"; ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Login Error", scrname, false); return; }
                        else { condition = ",AadharNo='" + TxtIdProofNo.Text.Trim() + "'"; }
                    }
                }
            }
            if (Session["CompID"] != null && Session["CompID"].ToString() == "1074")
            {
                if (!panverifycashless()) { BtnIdentity.Enabled = true; }
                else { BtnIdentity.Enabled = false; ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('Pan card already register another id.!!');", true); txtpan.Text = ""; return; }
            }
            string AdrsProof = "", BackAdrsProof = "", BaNKProof = "", panproof = "";
            string FlAddrs = "", FlBackAddrs = "", FlBank = "", FlPan = "";
            string strextension = "";
            if (Fuidentity.Enabled && !Fuidentity.HasFile)
            { scrname = "<SCRIPT language='javascript'>alert('Please front address upload jpg/jpeg/png/ image of upto 20 mb size only!! ');</SCRIPT>"; ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Close", scrname, false); return; }
            string compId = Session["CompID"] != null ? Session["CompID"].ToString() : "0";
            string uploadRoot = Server.MapPath("~/images/UploadImage/" + compId + "/");
            if (!Directory.Exists(uploadRoot)) { Directory.CreateDirectory(uploadRoot); }
            if (Session["CompID"] != null)
            {
                if (txtpan.Text.Trim().Length < 10) { scrname = "<SCRIPT language='javascript'>alert('Invalid PAN No!!');</SCRIPT>"; ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Close", scrname, false); return; }
            }
            if (Fuidentity.HasFile)
            {
                strextension = System.IO.Path.GetExtension(Fuidentity.FileName);
                if (strextension.ToUpper() == ".JPG" || strextension.ToUpper() == ".JPEG" || strextension.ToUpper() == ".PNG")
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(Fuidentity.PostedFile.InputStream);
                    decimal size = Math.Round((decimal)(Fuidentity.PostedFile.ContentLength) / 1024, 1);
                    if (size > 5120) { scrname = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>"; ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false); return; }
                    else
                    {
                        FlAddrs = "FA" + DateTime.Now.ToString("yyMMddhhmmssfff") + Session["formno"].ToString() + System.IO.Path.GetExtension(Fuidentity.PostedFile.FileName);
                        string fullPath = Path.Combine(uploadRoot, FlAddrs); Fuidentity.PostedFile.SaveAs(fullPath);
                        CompressAndSaveImage(Fuidentity.PostedFile.InputStream, fullPath, strextension, 50L);
                        AdrsProof = "https://" + HttpContext.Current.Request.Url.Host + "/images/UploadImage/" + compId + "/" + FlAddrs;
                    }
                }
                else { scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg, .jpeg, and .png extension files!! ');</SCRIPT>"; ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false); return; }
            }
            else { AdrsProof = lblimage.Text; }
            if (FileUpload1.Enabled) { if (!FileUpload1.HasFile) { ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Close", "<SCRIPT language='javascript'>alert('Please upload a jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>", false); return; } }
            if (FileUpload1.HasFile)
            {
                strextension = System.IO.Path.GetExtension(FileUpload1.FileName);
                if (strextension.ToUpper() == ".JPG" || strextension.ToUpper() == ".JPEG" || strextension.ToUpper() == ".PNG")
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream);
                    decimal size = Math.Round((decimal)(FileUpload1.PostedFile.ContentLength) / 1024, 1);
                    if (size > 5120) { scrname = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>"; ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false); return; }
                    else
                    {
                        FlBackAddrs = "BA" + DateTime.Now.ToString("yyMMddhhmmssfff") + Session["formno"].ToString() + System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
                        string fullPath = Path.Combine(uploadRoot, FlBackAddrs); FileUpload1.PostedFile.SaveAs(fullPath);
                        CompressAndSaveImage(FileUpload1.PostedFile.InputStream, fullPath, strextension, 50L);
                        BackAdrsProof = "https://" + HttpContext.Current.Request.Url.Host + "/images/UploadImage/" + compId + "/" + FlBackAddrs;
                    }
                }
                else { scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg, .jpeg, and .png extension files!! ');</SCRIPT>"; ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false); return; }
            }
            else { BackAdrsProof = LblBackImage.Text; }
            if (Pankyc.Enabled) { if (!Pankyc.HasFile) { ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Close", "<SCRIPT language='javascript'>alert('Please upload a jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>", false); return; } }
            if (Pankyc.HasFile)
            {
                strextension = System.IO.Path.GetExtension(Pankyc.FileName);
                if (strextension.ToUpper() == ".JPG" || strextension.ToUpper() == ".JPEG" || strextension.ToUpper() == ".PNG")
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(Pankyc.PostedFile.InputStream);
                    decimal size = Math.Round((decimal)(Pankyc.PostedFile.ContentLength) / 1024, 1);
                    if (size > 5120) { scrname = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>"; ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false); return; }
                    else
                    {
                        FlPan = "PAN" + DateTime.Now.ToString("yyMMddhhmmssfff") + Session["formno"].ToString() + System.IO.Path.GetExtension(Pankyc.PostedFile.FileName);
                        string fullPath = Path.Combine(uploadRoot, FlPan); Pankyc.PostedFile.SaveAs(fullPath);
                        CompressAndSaveImage(Pankyc.PostedFile.InputStream, fullPath, strextension, 50L);
                        panproof = "https://" + HttpContext.Current.Request.Url.Host + "/images/UploadImage/" + compId + "/" + FlPan;
                    }
                }
                else { scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg, .jpeg, and .png extension files!! ');</SCRIPT>"; ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false); return; }
            }
            else { panproof = LblPanImage.Text; }
            if (BankKYCFileUpload3.Enabled) { if (!BankKYCFileUpload3.HasFile) { ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Close", "<SCRIPT language='javascript'>alert('Please upload a jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>", false); return; } }
            if (BankKYCFileUpload3.HasFile)
            {
                strextension = System.IO.Path.GetExtension(BankKYCFileUpload3.FileName);
                if (strextension.ToUpper() == ".JPG" || strextension.ToUpper() == ".JPEG" || strextension.ToUpper() == ".PNG")
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(BankKYCFileUpload3.PostedFile.InputStream);
                    decimal size = Math.Round((decimal)(BankKYCFileUpload3.PostedFile.ContentLength) / 1024, 1);
                    if (size > 5120) { scrname = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>"; ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false); return; }
                    else
                    {
                        FlBank = "Bank" + DateTime.Now.ToString("yyMMddhhmmssfff") + Session["formno"].ToString() + System.IO.Path.GetExtension(BankKYCFileUpload3.PostedFile.FileName);
                        string fullPath = Path.Combine(uploadRoot, FlBank); BankKYCFileUpload3.PostedFile.SaveAs(fullPath);
                        CompressAndSaveImage(BankKYCFileUpload3.PostedFile.InputStream, fullPath, strextension, 50L);
                        BaNKProof = "https://" + HttpContext.Current.Request.Url.Host + "/images/UploadImage/" + compId + "/" + FlBank;
                    }
                }
                else { scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg, .jpeg, and .png extension files!! ');</SCRIPT>"; ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false); return; }
            }
            else { BaNKProof = LblBankImage.Text; }

            // ===== COMPANY: KycType='C' + sirf selected type ke columns. Individual me blank. =====
            string companyFlag = "";
            if (IsCompanyKyc())
            {
                string sel = DDLAddressProof.SelectedItem != null ? DDLAddressProof.SelectedItem.Text.Trim().ToUpper() : "";
                if (sel == "LIGHT BILL")
                {
                    string img = LblBillImg.Text;
                    if (FuBill.HasFile) { if (!UploadCompFile(FuBill, "BILL", compId, uploadRoot, out img)) return; }
                    // sirf Bill rakho, GST/Other blank
                    companyFlag = ",KycType='C',CompBillNo='" + txtBillNo.Text.Trim() + "',CompBillImg='" + img + "'" +
                                  ",CompGstNo='',CompGstImg='',CompOtherNo='',CompOtherImg=''";
                }
                else if (sel == "GSTIN")
                {
                    string img = LblGstImg.Text;
                    if (FuGst.HasFile) { if (!UploadCompFile(FuGst, "GST", compId, uploadRoot, out img)) return; }
                    // sirf GST rakho, Bill/Other blank
                    companyFlag = ",KycType='C',CompGstNo='" + txtGstNo.Text.Trim() + "',CompGstImg='" + img + "'" +
                                  ",CompBillNo='',CompBillImg='',CompOtherNo='',CompOtherImg=''";
                }
                else
                {
                    string img = LblOtherImg.Text;
                    if (FuOther.HasFile) { if (!UploadCompFile(FuOther, "OTH", compId, uploadRoot, out img)) return; }
                    // sirf Other rakho, Bill/GST blank
                    companyFlag = ",KycType='C',CompOtherNo='" + txtOtherNo.Text.Trim() + "',CompOtherImg='" + img + "'" +
                                  ",CompBillNo='',CompBillImg='',CompGstNo='',CompGstImg=''";
                }
            }

            double dblBank = 0;
            DataTable Dt = new DataTable();
            if (cmbbank.SelectedItem != null && cmbbank.SelectedItem.Text.ToUpper() == "OTHERS")
            {
                if (!string.IsNullOrWhiteSpace(Txtbank.Text))
                {
                    string Q1 = Obj.IsoStart + "Select * from " + Obj.dBName + "..M_BankMaster where BankName='" + Txtbank.Text.Trim() + "' and Activestatus='Y'and RowStatus='Y' " + Obj.IsoEnd;
                    Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Q1).Tables[0];
                    if (Dt.Rows.Count == 0)
                    {
                        Q1 = "insert into M_BankMaster (BankCode,BankName,AcNo,IFSCode,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus) Select Case When Max(BankCode) Is Null Then '1' Else Max(BankCode)+1 END as BankCode,'" + Txtbank.Text + "','0','0','', 'Y','Add by " + Session["IdNo"] + " at " + DateTime.Now.ToString() + "','" + Session["MemName"] + "','" + Convert.ToInt32(Session["FormNo"]) + "','','Y' From M_BankMaster ";
                        i = Obj.SaveData(Q1);
                        if (i > 0)
                        {
                            Q1 = Obj.IsoStart + " select Max(BankCode) as BankCode from " + Obj.dBName + "..M_BankMaster where ActiveStatus='Y' and RowStatus='Y'" + Obj.IsoEnd;
                            dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Q1).Tables[0];
                            if (dt1.Rows.Count > 0) dblBank = Convert.ToDouble(dt1.Rows[0]["BankCode"]);
                        }
                    }
                    else { dblBank = Convert.ToDouble(Dt.Rows[0]["BankCode"]); }
                }
            }
            else { if (!string.IsNullOrEmpty(cmbbank.SelectedValue)) double.TryParse(cmbbank.SelectedValue, out dblBank); }
            if (!string.IsNullOrEmpty(Txtbank.Text) || !string.IsNullOrWhiteSpace(Txtcode.Text))
            {
                if (cmbbank.SelectedValue == "0" || string.IsNullOrEmpty(cmbbank.SelectedValue)) { scrname = "<SCRIPT language='javascript'>alert('Choose Bank Name');</SCRIPT>"; RegisterStartupScript("MyAlert", scrname); return; }
                if (string.IsNullOrEmpty(Txtbranch.Text)) { scrname = "<SCRIPT language='javascript'>alert('Enter Branch Name.');</SCRIPT>"; RegisterStartupScript("MyAlert", scrname); return; }
                if (string.IsNullOrEmpty(Txtcode.Text)) { scrname = "<SCRIPT language='javascript'>alert('Enter IFSC Code.');</SCRIPT>"; RegisterStartupScript("MyAlert", scrname); return; }
            }

            string Remark = "";
            string str = Obj.IsoStart + " Exec sp_FillKyc '" + Convert.ToInt32(Session["Formno"]) + "'" + Obj.IsoEnd;
            DataTable dtCompare = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];
            if (dtCompare.Rows.Count > 0)
            {
                if (ClearInject(dtCompare.Rows[0]["Address1"].ToString()) != ClearInject(txtaddrs.Text)) Remark += "Address ,";
                if (ClearInject(dtCompare.Rows[0]["City"].ToString()) != ClearInject(Txtcity.Text)) Remark += " City ,";
                if (ClearInject(dtCompare.Rows[0]["District"].ToString()) != ClearInject(Txtdistrict.Text)) Remark += " District,";
                if (Convert.ToInt32(dtCompare.Rows[0]["StateCode"]) != Convert.ToInt32(ddlState.SelectedValue)) Remark += " State ,";
                if (ddlState.SelectedValue == "0") ddlState.SelectedValue = dtCompare.Rows[0]["StateCode"].ToString();
                if (ClearInject(dtCompare.Rows[0]["PinCode"].ToString()) != ClearInject(Txtpincode.Text)) Remark += " PinCode,";
                if (ClearInject(dtCompare.Rows[0]["AddrProof"].ToString()) != ClearInject(AdrsProof)) Remark += " AddressProof,";
                if (ClearInject(dtCompare.Rows[0]["BackAddressProof"].ToString()) != ClearInject(BackAdrsProof)) Remark += " BackAddressProof,";
                if (ClearInject(dtCompare.Rows[0]["id"].ToString()) != ClearInject(DDLAddressProof.SelectedValue)) Remark += " AddressProofType,";
                if (ClearInject(dtCompare.Rows[0]["IdProofNo"].ToString()) != ClearInject(TxtIdProofNo.Text.Trim())) Remark += "AddressProofNo,";
                if (Convert.ToString(dtCompare.Rows[0]["BankId"]) != cmbbank.SelectedValue) Remark += " Bank,";
                if (ClearInject(dtCompare.Rows[0]["BranchName"].ToString()) != ClearInject(Txtbranch.Text)) Remark += " BranchName,";
                if (ClearInject(dtCompare.Rows[0]["AcNo"].ToString()) != ClearInject(Txtacno.Text)) Remark += " AccountNo,";
                if (ClearInject(dtCompare.Rows[0]["IFSCode"].ToString()) != ClearInject(Txtcode.Text)) Remark += " IFSCCode,";
                if (ClearInject(dtCompare.Rows[0]["BankProof"].ToString()) != ClearInject(BaNKProof)) Remark += " BankProof,";
                if (Session["CompID"] != null && (CompID == "1102" || CompID == "1100" || CompID == "1106" || CompID == "1107" || CompID == "1108" || CompID == "1105" || CompID == "1109" || CompID == "1110"))
                { if (dtCompare.Rows[0]["Fax"].ToString() != DDLAccountType.SelectedItem.Text) Remark += " Account Type,"; }
                if (ClearInject(dtCompare.Rows[0]["Panno"].ToString()) != ClearInject(txtpan.Text)) Remark += " PANNo,";
                if (ClearInject(dtCompare.Rows[0]["PanImg"].ToString()) != ClearInject(panproof)) Remark += " PanCardImage,";
            }

            if (DDLAddressProof.SelectedValue == "0") { scrname = "<SCRIPT language='javascript'>alert('Choose ID Proof Type.');</SCRIPT>"; RegisterStartupScript("MyAlert", scrname); return; }
            if (ddlState.SelectedValue == "0" || string.IsNullOrEmpty(ddlState.SelectedValue)) { scrname = "<SCRIPT language='javascript'>alert('Select State Name.');</SCRIPT>"; RegisterStartupScript("MyAlert", scrname); return; }

            int AreaCode = 0;
            if (!(Session["CompID"] != null && (CompID == "1100" || CompID == "1102" || CompID == "1101" || CompID == "1106" || CompID == "1105" || CompID == "1108" || CompID == "1107" || CompID == "1109" || CompID == "1110")))
            {
                if (DDlVillage.SelectedItem != null && DDlVillage.SelectedItem.Text.ToUpper() == "OTHERS")
                {
                    if (!string.IsNullOrWhiteSpace(TxtVillage.Text))
                    {
                        string q = Obj.IsoStart + "Select * from " + Obj.dBName + "..M_VillageMaster where VillageName='" + TxtVillage.Text.Trim() + "' and Activestatus='Y' and Pincode='" + Convert.ToInt32(Txtpincode.Text) + "' " + Obj.IsoEnd;
                        Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, q).Tables[0];
                        if (Dt.Rows.Count == 0)
                        {
                            q = "insert into M_VillageMaster (VillageName,CityCode,PinCode) Values('" + TxtVillage.Text.ToUpper() + "','" + HCityCode.Value + "','" + Txtpincode.Text + "')";
                            i = Obj.SaveData(q);
                            if (i > 0)
                            {
                                q = Obj.IsoStart + " select Max(VillageCode) as VillageCode from " + Obj.dBName + "..M_VillageMaster where ActiveStatus='Y'" + Obj.IsoEnd;
                                Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, q).Tables[0];
                                if (Dt.Rows.Count > 0) AreaCode = Convert.ToInt32(Dt.Rows[0]["VillageCode"]);
                            }
                        }
                        else { AreaCode = Convert.ToInt32(Dt.Rows[0]["VillageCode"]); }
                    }
                }
                else { if (!string.IsNullOrEmpty(DDlVillage.SelectedValue)) AreaCode = Convert.ToInt32(DDlVillage.SelectedValue); }
            }

            string Strqueryquer = "Insert into Trnjoining(Transid)values(" + HdnCheckTrnns.Value + ")";
            int isOk1 = 0;
            try { isOk1 = Convert.ToInt32(SqlHelper.ExecuteNonQuery(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(), CommandType.Text, Strqueryquer)); }
            catch (Exception ex) { isOk1 = 0; }
            if (isOk1 > 0)
            {
                string Qry = "Insert Into TempMemberMaster Select *,'Update KYC - " + Context.Request.UserHostAddress.ToString() + "',GetDate(),'U' From M_MemberMaster Where FormNo='" + Convert.ToInt32(Session["FormNo"]) + "'";
                Qry += " ; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values(0,'" + Session["MemName"] + "','KYC Detail','KYC Detail Update','" + Remark + "',Getdate(),'" + Session["FormNo"] + "')";
                string sqlU = Qry + ";Update m_MemberMaster set Address1='" + txtaddrs.Text.ToUpper() + "', Tehsil='" + Txtcity.Text.ToUpper() + "',City='" + Txtcity.Text.ToUpper() + "',District='" + Txtdistrict.Text.ToUpper() + "',StateCode='" + ddlState.SelectedValue + "', Pincode='" + Txtpincode.Text + "' ,AreaCode='" + AreaCode + "',Fax='" + DDLAccountType.SelectedItem.Text + "', CityCode='" + HCityCode.Value + "',DistrictCode='" + HDistrictCode.Value + "',Panno='" + txtpan.Text.ToUpper() + "',Acno='" + Txtacno.Text + "',Bankid='" + dblBank + "',IFscode='" + Txtcode.Text.ToUpper() + "', Branchname='" + Txtbranch.Text.ToUpper() + "'" + condition + " where Formno= '" + Session["Formno"] + "'";
                if (Session["CompID"] != null && CompID == "1108")
                {
                    sqlU = sqlU + ";Update KycVerify Set Idtype='" + DDLAddressProof.SelectedValue + "',IdProofNo='" + TxtIdProofNo.Text.Trim().ToUpper() + "', AddrProof='" + AdrsProof + "',BackAddressProof='" + BackAdrsProof + "',BackAddressDate=Getdate(),IsaddrssVerified='N',IsAddress = 'N', PanImg='" + panproof + "',PANImgDate=Getdate(),IsPanVerified='N',IsPan = 'N',BankProof='" + BaNKProof + "',BankProofDate=Getdate(),IsBankVerified='N',IsBank = 'N'" + companyFlag + " where Formno= '" + Session["Formno"] + "'";
                }
                else if ((Session["CompID"] != null && (CompID == "1102" || CompID == "1106" || CompID == "1105" || CompID == "1107" || CompID == "1109" || CompID == "1110")))
                {
                    sqlU = sqlU + ";Update KycVerify Set Idtype='" + DDLAddressProof.SelectedValue + "',IdProofNo='" + TxtIdProofNo.Text.Trim().ToUpper() + "', AddrProof='" + AdrsProof + "',BackAddressProof='" + BackAdrsProof + "',BackAddressDate=Getdate(),IsaddrssVerified='N',IsAddress = 'N', PanImg='" + panproof + "',PANImgDate=Getdate(),IsPanVerified='N',IsPan = 'N',BankProof='" + BaNKProof + "',BankProofDate=Getdate(),IsBankVerified='N',IsBank = 'N' where Formno= '" + Session["Formno"] + "'";
                }
                else
                {
                    sqlU = sqlU + ";Update KycVerify Set Idtype='" + DDLAddressProof.SelectedValue + "',IdProofNo='" + TxtIdProofNo.Text.Trim().ToUpper() + "', AddrProof='" + AdrsProof + "',BackAddressProof='" + BackAdrsProof + "',BackAddressDate=Getdate(),IsaddrssVerified='N', PanImg='" + panproof + "',PANImgDate=Getdate(),IsPanVerified='N',BankProof='" + BaNKProof + "',BankProofDate=Getdate(),IsBankVerified='N' where Formno= '" + Session["Formno"] + "'";
                }
                int j = Obj.SaveData(sqlU);
                if (j != 0)
                {
                    scrname = "<script>alert('KYC Upload successfully');location.replace('KYCUpdate.aspx');</script>";
                    Session["ScrName"] = scrname;
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                    return;
                }
                else
                {
                    string script = "window.onload = function(){ alert('KYC Upload unsuccessfully');window.location = 'KYCUpdate.aspx'; }";
                    ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                    return;
                }
            }
            else
            {
                string script = "window.onload = function(){ alert('Try After Some Time.!');window.location = 'KYCUpdate.aspx'; }";
                ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                return;
            }
        }
        catch (Exception ex)
        {
            try { if (Obj != null) Obj.WriteToFile(HttpContext.Current.Request.Url.AbsoluteUri + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + ex.Message); } catch { }
            Response.Write("Try later.");
        }
    }
}