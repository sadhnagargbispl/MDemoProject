using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class kyc : System.Web.UI.Page
{
    double dblBank;
    // Data table
    DataTable tmpTable = new DataTable();
    // DAL object (initialize later using Session)
    DAL Obj;
    clsGeneral objGen = new clsGeneral();
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
                            ScriptManager.RegisterStartupScript(
                                this, this.GetType(), "Key",
                                "alert('Kyc option is temporarily off. Please try after some time.!!');" +
                                "location.replace('" + Session["IndexPage"] + "');",
                                true
                            );
                            return;
                        }
                    }

                    if (Session["CompID"].ToString() == "1101")
                    {
                        if (!GetKycPerStatusCheck())
                        {
                            ScriptManager.RegisterStartupScript(
                                this, this.GetType(), "Key",
                                "alert('We’re unable to process your KYC because your ID is not active. Please activate your ID and you can complete your KYC smoothly.');" +
                                "location.replace('" + Session["IndexPage"] + "');",
                                true
                            );
                            return;
                        }
                    }

                    if (
                        Session["CompID"].ToString() == "1082" ||
                        Session["CompID"].ToString() == "1074" ||
                        Session["CompID"].ToString() == "1084" ||
                        Session["CompID"].ToString() == "1089" ||
                        Session["CompID"].ToString() == "1091" ||
                        Session["CompID"].ToString() == "1092" ||
                        Session["CompID"].ToString() == "1093" ||
                        Session["CompID"].ToString() == "1095" ||
                        Session["CompID"].ToString() == "1097" ||
                        Session["CompID"].ToString() == "1100" ||
                        Session["CompID"].ToString() == "1102" ||
                        Session["CompID"].ToString() == "1101" ||
                        Session["CompID"].ToString() == "1106" ||
                        Session["CompID"].ToString() == "1107" ||
                        Session["CompID"].ToString() == "1108" ||
                        Session["CompID"].ToString() == "1110" ||
                        Session["CompID"].ToString() == "1105"
                    )
                    {
                        Accountype.Visible = true;
                    }
                    else
                    {
                        Accountype.Visible = false;
                    }

                    // Call your methods
                    Fill_State();
                    FillIdtypeMaster();
                    FillBankMaster();
                    loadImages();
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
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            Response.Write("Try later.");
        }
    }
    public string GenerateRandomStringJoining(int length)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        string sResult = "";

        for (int i = 0; i < length; i++)
        {
            sResult += allowChrs[rdm.Next(allowChrs.Length)];
        }

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
                if (dt.Rows[0]["Fax"].ToString().ToUpper() != "CHOOSE ACCOUNT TYPE")
                {
                    DDLAccountType.Enabled = false;
                }
                else
                {
                    DDLAccountType.Enabled = true;
                    c++;
                }

                // FillCityPinDetail()
                if (string.IsNullOrEmpty(TxtIdProofNo.Text))
                    TxtIdProofNo.Enabled = true; // Enable if empty
                else
                    TxtIdProofNo.Enabled = false;// Disable if not empty

                if (string.IsNullOrEmpty(txtaddrs.Text))
                    txtaddrs.Enabled = true; // Enable if empty
                else
                    txtaddrs.Enabled = false;// Disable if not empty

                if (string.IsNullOrEmpty(txtpan.Text))
                    txtpan.Enabled = true; // Enable if empty
                else
                    txtpan.Enabled = false;// Disable if not empty

                if (string.IsNullOrEmpty(Txtpincode.Text))
                    Txtpincode.Enabled = true; // Enable if empty
                else
                    Txtpincode.Enabled = false;// Disable if not empty

                if (string.IsNullOrEmpty(ddlState.SelectedItem.Text) || ddlState.SelectedItem.Text.ToString() == "--Choose State Name--")
                    ddlState.Enabled = true; // Enable if empty
                else
                    ddlState.Enabled = false;// Disable if not empty

                if (string.IsNullOrEmpty(Txtdistrict.Text) || Txtdistrict.Text == "0")
                    Txtdistrict.Enabled = true; // Enable if empty
                else
                    Txtdistrict.Enabled = false;// Disable if not empty

                if (string.IsNullOrEmpty(Txtcity.Text) || Txtcity.Text == "0")
                    Txtcity.Enabled = true; // Enable if empty
                else
                    Txtcity.Enabled = false;// Disable if not empty
                if (string.IsNullOrEmpty(DDLAddressProof.SelectedItem.Text) || DDLAddressProof.SelectedItem.Text.ToString() == "--Choose Id Proof--")
                    DDLAddressProof.Enabled = true; // Enable if empty
                else
                    // DDLAddressProof.Enabled = False ' Disable if not empty
                    DDLAddressProof.Enabled = false;

                if (string.IsNullOrEmpty(cmbbank.SelectedItem.Text) || cmbbank.SelectedItem.Text.ToString() == "--Choose Bank Name--")
                    cmbbank.Enabled = true; // Enable if empty
                else
                    cmbbank.Enabled = false;// Disable if not empty

                if (string.IsNullOrEmpty(Txtbranch.Text))
                    Txtbranch.Enabled = true; // Enable if empty
                else
                    Txtbranch.Enabled = false;// Disable if not empty

                if (string.IsNullOrEmpty(Txtcode.Text))
                    Txtcode.Enabled = true; // Enable if empty
                else
                    Txtcode.Enabled = false;// Disable if not empty
                BindImage(dt.Rows[0]["AddrProof"].ToString(), ShowIdentity, FrontAddress, lblimage, Fuidentity);
                BindImage(dt.Rows[0]["BackAddressProof"].ToString(), showBackImage, BackAddress, LblBackImage, FileUpload1);
                BindImage(dt.Rows[0]["BankProof"].ToString(), bANKiMAGE, BankProof, LblBankImage, BankKYCFileUpload3);
                BindImage(dt.Rows[0]["PanImg"].ToString(), pANiMAGE, PanCard, LblPanImage, Pankyc);
                // Front Address Proof
                if (!string.IsNullOrEmpty(dt.Rows[0]["AddrProof"].ToString()) &&
                    dt.Rows[0]["IsAddrssVerified"].ToString() != "R")
                {
                    Fuidentity.Enabled = false;
                    Fuidentity.Attributes.Add("class", "form-control");
                }
                else
                {
                    Fuidentity.Enabled = true;
                    c++;
                }

                // Back Address Proof
                if (!string.IsNullOrEmpty(dt.Rows[0]["BackAddressProof"].ToString()) &&
                    dt.Rows[0]["IsAddrssVerified"].ToString() != "R")
                {
                    FileUpload1.Enabled = false;
                    FileUpload1.Attributes.Add("class", "form-control");
                }
                else
                {
                    FileUpload1.Enabled = true;
                    c++;
                }

                // Bank Proof
                if (!string.IsNullOrEmpty(LblBankImage.Text) &&
                    dt.Rows[0]["IsBankVerified"].ToString() != "R")
                {
                    BankKYCFileUpload3.Enabled = false;
                    BankKYCFileUpload3.Attributes.Add("class", "form-control");
                }
                else
                {
                    BankKYCFileUpload3.Enabled = true;
                    c++;
                }

                // PAN Proof
                if (!string.IsNullOrEmpty(LblPanImage.Text) &&
                    dt.Rows[0]["IsPanVerified"].ToString() != "R")
                {
                    Pankyc.Enabled = false;
                    Pankyc.Attributes.Add("class", "form-control");
                }
                else
                {
                    Pankyc.Enabled = true;
                    c++;
                }
                if (
    dt.Rows[0]["IsBankVerified"].ToString() != "R" &&
    (
        !string.IsNullOrEmpty(dt.Rows[0]["BankProof"].ToString()) ||
        Txtacno.Text.Length > 10
    )
)
                {
                    Txtacno.Enabled = false;
                }
                else
                {
                    Txtacno.Enabled = true;
                    c++;
                }
                //if (!string.IsNullOrEmpty(Txtacno.Text))
                //{
                //    Pankyc.Enabled = false;
                //    // Pankyc.Attributes.Add("class", "form-control");
                //}
                //else
                //{
                //    Pankyc.Enabled = true;
                //}

            }
            if (dt.Rows[0]["IsAddrssVerified"].ToString() != "R" && c == 0)
            {
                BtnIdentity.Visible = false;
                LbLrejectRemark.Text = "";
            }
            else
                BtnIdentity.Visible = true;
            if (status == "Verification Due")
            {
                VerifyDate.Visible = false;
                Lblverdate.Visible = false;
                LblVerfReason.Visible = false;
                LblVerfRemark.Visible = false;
                LbLrejectRemark.Text = "";
                VerifyDate.Text = "";
                DivVerify.Attributes.Add("style", "color:black");
            }
            else if (status == "")
            {
                VerifyDate.Visible = true;
                Lblverdate.Visible = true;
                LblVerfReason.Visible = false;
                LblVerfRemark.Visible = false;
                VerifyDate.Text = "";
                DivVerify.Attributes.Add("style", "color:black");
                TxtIdProofNo.Enabled = true;
                txtaddrs.Enabled = true;
                txtpan.Enabled = true;
                Txtpincode.Enabled = true;
                ddlState.Enabled = true;
                Txtdistrict.Enabled = true;
                Txtcity.Enabled = true;
                DDLAddressProof.Enabled = true;
                DDLAccountType.Enabled = true;
                cmbbank.Enabled = true;
                Txtbranch.Enabled = true;
                Txtcode.Enabled = true;
                Fuidentity.Enabled = true;
                FileUpload1.Enabled = true;
                BankKYCFileUpload3.Enabled = true;
                Pankyc.Enabled = true;
                Txtacno.Enabled = true;
                BtnIdentity.Visible = true;
            }
            else if (status == "Rejected")
            {
                VerifyDate.Visible = true;
                Lblverdate.Visible = true;
                LblVerfReason.Visible = true;
                LblVerfRemark.Visible = true;
                VerifyDate.Text = "Reject Date:";
                DivVerify.Attributes.Add("style", "color:red");
                TxtIdProofNo.Enabled = true;
                txtaddrs.Enabled = true;
                txtpan.Enabled = true;
                Txtpincode.Enabled = true;
                ddlState.Enabled = true;
                Txtdistrict.Enabled = true;
                Txtcity.Enabled = true;
                DDLAddressProof.Enabled = true;
                DDLAccountType.Enabled = true;
                cmbbank.Enabled = true;
                Txtbranch.Enabled = true;
                Txtcode.Enabled = true;
                Txtacno.Enabled = true;
                BtnIdentity.Visible = true;
            }
            else
            {
                VerifyDate.Visible = true;
                Lblverdate.Visible = true;
                LblVerfReason.Visible = false;
                LblVerfRemark.Visible = false;
                LbLrejectRemark.Text = "";
                VerifyDate.Text = "Verify Date:";
                DivVerify.Attributes.Add("style", "color:Green");
            }
            LblVerification.Visible = true;
        }
        catch (Exception ex)
        {

        }
    }
    private bool GetKycPerStatusCheck()
    {
        try
        {
            bool result = false;
            DataSet Ds12 = new DataSet();
            DataTable dt12 = new DataTable();

            string str12 = Obj.IsoStart + "select Activestatus from " + Obj.dBName + "..m_membermaster where formno = '" + Session["formno"] + "'" + Obj.IsoEnd;
            Ds12 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str12);

            dt12 = Ds12.Tables[0];

            if (dt12.Rows[0]["Activestatus"].ToString().ToUpper() == "Y")
                result = true;
            else
                result = false;

            return result;
        }
        catch
        {
            return false;
        }
    }
    private void FillBankMaster()
    {
        try
        {
            Obj = new DAL();

            string Strquery = Obj.IsoStart + "SELECT BankCode as Bid, BANKNAME as Bank " +
                              "FROM " + Obj.dBName + "..M_BankMaster WHERE ACTIVESTATUS='Y' and Rowstatus='Y' " +
            "ORDER BY BANKCode" + Obj.IsoEnd;

            tmpTable = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Strquery).Tables[0];

            cmbbank.DataSource = tmpTable;
            cmbbank.DataValueField = "Bid";
            cmbbank.DataTextField = "Bank";
            cmbbank.DataBind();
            cmbbank.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }
    }
    protected void cmbbank_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbbank.SelectedItem.Text.ToUpper() == "OTHERS")
        {
            divBank.Visible = true;
            Txtbank.Focus();
            Txtbank.Text = "";
        }
        else
        {
            divBank.Visible = false;
            Txtbank.Text = "";
            Txtbranch.Focus();
        }
    }
    private void FillIdtypeMaster()
    {
        try
        {
            string strQuery = "";
            Obj = new DAL();

            strQuery = Obj.IsoStart + "SELECT Id, IdType FROM " + Obj.dBName + "..M_IdTypeMaster WHERE ACTIVESTATUS='Y'" + Obj.IsoEnd;
            tmpTable = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, strQuery).Tables[0];

            // Bind dropdown
            DDLAddressProof.DataSource = tmpTable;
            DDLAddressProof.DataValueField = "Id";
            DDLAddressProof.DataTextField = "IdType";
            DDLAddressProof.DataBind();
            DDLAddressProof.SelectedIndex = 0;

            // Build comma-separated label text
            LblIdproofText.Text = "";

            for (int s = 0; s < tmpTable.Rows.Count; s++)
            {
                if (tmpTable.Rows[s]["Id"].ToString() != "0")
                {
                    LblIdproofText.Text += tmpTable.Rows[s]["IdType"].ToString() + ",";
                }
            }

            // Remove last comma
            if (!string.IsNullOrEmpty(LblIdproofText.Text))
            {
                LblIdproofText.Text =
                    LblIdproofText.Text.Remove(LblIdproofText.Text.Length - 1, 1);
            }
        }
        catch (Exception e)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                          Environment.NewLine;

            Obj.WriteToFile(text + e.Message);
            Response.Write("Try later.");
        }
    }
    private void Fill_State()
    {
        try
        {
            Obj = new DAL();
            DataTable dt = new DataTable();

            string str = Obj.IsoStart + "Select StateCode, StateName from " + Obj.dBName + "..M_STateDivMaster " +
                         "Where ActiveStatus='Y' and RowStatus='Y' Order by StateCode" + Obj.IsoEnd;

            dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];

            if (dt.Rows.Count > 0)
            {
                ddlState.DataSource = dt;
                ddlState.DataValueField = "StateCode";
                ddlState.DataTextField = "StateName";
                ddlState.DataBind();
            }
        }
        catch
        {
        }
    }
    private void FillCityPinDetail()
    {
        string sql = "";

        if (Convert.ToInt32(Txtpincode.Text) != 0)
        {
            sql = Obj.IsoStart + "select a.Statename, b.DistrictName, c.CityName, d.VillageName, d.Pincode, " +
                  "a.StateCode, b.DistrictCode, c.CityCode, d.VillageCode " +
                  "from " + Obj.dBName + "..M_STateDivMaster a with(NoLock) " +
                  "inner join " + Obj.dBName + "..M_DistrictMaster b with(NoLock) on a.StateCode=b.StateCode and a.ActivEstatus='Y' and b.ActiveStatus='Y' " +
                  "inner join " + Obj.dBName + "..M_CityStatemaster c with(NoLock) on b.DistrictCode=c.DistrictCode and c.ActivEstatus='Y' " +
                  "inner join " + Obj.dBName + "..M_VillageMaster d with(NoLock) on c.CityCode=d.CityCode and d.ActiveStatus='Y' " +
                  "where d.Pincode='" + Convert.ToInt32(Txtpincode.Text) + "' " +
                  "Union all " +
                  "select '' as StateName, '' as DistrictName, '' as CityName, 'Others', '', 0, 0, 0, 381264" + Obj.IsoEnd;

            DataTable Dt = new DataTable();

            Obj = new DAL();
            Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];

            if (Dt.Rows.Count > 0)
            {
                Txtcity.Text = Dt.Rows[0]["CityName"].ToString();
                Txtdistrict.Text = Dt.Rows[0]["DistrictName"].ToString();

                StateCode.Value = Dt.Rows[0]["StateCode"].ToString();
                HDistrictCode.Value = Dt.Rows[0]["DistrictCode"].ToString();
                HCityCode.Value = Dt.Rows[0]["CityCode"].ToString();

                ddlState.SelectedItem.Text = Dt.Rows[0]["StateName"].ToString();

                DDlVillage.DataSource = Dt;
                DDlVillage.DataValueField = "VillageCode";
                DDlVillage.DataTextField = "VillageName";
                DDlVillage.DataBind();
                DDlVillage.SelectedIndex = 0;
            }
        }
    }
    protected void Txtpincode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string sql = "";
            string scrname = "";

            if (Convert.ToInt32(Txtpincode.Text) != 0)
            {
                sql = Obj.IsoStart + "select a.Statename, b.DistrictName, c.CityName, d.VillageName, d.Pincode, " +
                      "a.StateCode, b.DistrictCode, c.CityCode, d.VillageCode " +
                      "from " + Obj.dBName + "..M_STateDivMaster a with(NoLock) " +
                      "inner join " + Obj.dBName + "..M_DistrictMaster b with(NoLock) on a.StateCode=b.StateCode and a.ActivEstatus='Y' and b.ActiveStatus='Y' " +
                      "inner join " + Obj.dBName + "..M_CityStatemaster c with(NoLock) on b.DistrictCode=c.DistrictCode and c.ActivEstatus='Y' " +
                      "inner join " + Obj.dBName + "..M_VillageMaster d with(NoLock) on c.CityCode=d.CityCode and d.ActiveStatus='Y' " +
                      "where d.Pincode='" + Convert.ToInt32(Txtpincode.Text) + "' " +
                      "Union all " +
                      "select '' as StateName, '' as DistrictName, '' as CityName, 'Others', '', 0, 0, 0, 381264" + Obj.IsoEnd;
                DataTable Dt = new DataTable();
                Obj = new DAL();
                Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];

                if (Dt.Rows.Count > 0)
                {
                    ddlState.SelectedItem.Text = Dt.Rows[0]["StateName"].ToString();
                    StateCode.Value = Dt.Rows[0]["StateCode"].ToString();

                    Txtdistrict.Text = Dt.Rows[0]["DistrictName"].ToString();
                    HDistrictCode.Value = Dt.Rows[0]["DistrictCode"].ToString();

                    Txtcity.Text = Dt.Rows[0]["CityName"].ToString();
                    HCityCode.Value = Dt.Rows[0]["CityCode"].ToString();

                    DDlVillage.DataSource = Dt;
                    DDlVillage.DataValueField = "VillageCode";
                    DDlVillage.DataTextField = "VillageName";
                    DDlVillage.DataBind();
                    DDlVillage.SelectedIndex = 0;

                    DDlVillage.Focus();
                }
                else
                {
                    Txtpincode.Focus();

                    ddlState.Items.Clear();
                    StateCode.Value = "0";

                    Txtcity.Text = "";
                    HCityCode.Value = "0";

                    Txtdistrict.Text = "";
                    HDistrictCode.Value = "0";

                    DDlVillage.Items.Clear();

                    ScriptManager.RegisterClientScriptBlock(
                        this.Page,
                        this.Page.GetType(),
                        "alert",
                        "alert('Permanent Pincode Not exist.');",
                        true
                    );
                }
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
    private bool GetKycPerStatus()
    {
        try
        {
            bool result = false;
            DataSet Ds12 = new DataSet();
            DataTable dt12 = new DataTable();

            string str12 = Obj.IsoStart + "Select IsWithDra From " + Obj.dBName + "..m_KycPer" + Obj.IsoEnd;

            Ds12 = SqlHelper.ExecuteDataset(
                       HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                       CommandType.Text,
                       str12
                   );

            dt12 = Ds12.Tables[0];

            if (dt12.Rows[0]["IsWithDra"].ToString().ToUpper() == "Y")
                result = true;

            return result;
        }
        catch
        {
            return false;
        }
    }
    private string ClearInject(string StrObj)
    {
        StrObj = StrObj.Replace(";", "")
                       .Replace("'", "")
                       .Replace("=", "");

        return StrObj;
    }
    protected void txtpan_TextChanged(object sender, EventArgs e)
    {
        if (Session["CompId"].ToString() == "1077" || Session["CompId"].ToString() == "1091")
        {
            if (panverify())
            {
                BtnIdentity.Enabled = true;
            }
            else
            {
                BtnIdentity.Enabled = false;
                ScriptManager.RegisterStartupScript(
                    this, this.GetType(), "Key",
                    "alert('Pan card already register another id.!!');", true);

                txtpan.Text = "";
                return;
            }
        }

        if (Session["CompId"].ToString() == "1074")
        {
            if (panverifycashless() == false)
            {
                BtnIdentity.Enabled = true;
            }
            else
            {
                BtnIdentity.Enabled = false;
                ScriptManager.RegisterStartupScript(
                    this, this.GetType(), "Key",
                    "alert('Pan card already register another id.!!');", true);

                txtpan.Text = "";
                return;
            }
        }
    }
    protected void TxtIdProofNo_TextChanged(object sender, EventArgs e)
    {
        if (Session["CompId"].ToString() == "1091")
        {
            if (aadharverify())
            {
                BtnIdentity.Enabled = true;
            }
            else
            {
                BtnIdentity.Enabled = false;
                ScriptManager.RegisterStartupScript(
                    this, this.GetType(), "Key",
                    "alert('Aadhar no already register another id.!!');", true);

                TxtIdProofNo.Text = "";
                return;
            }
        }
    }
    private bool panverify()
    {
        try
        {
            bool result = false;
            DataSet Ds12 = new DataSet();
            DataTable dt12 = new DataTable();

            string str12 = Obj.IsoStart + "select count(panno) as cnt from " + Obj.dBName + "..KycVerify as a, " + Obj.dBName + "..m_membermaster as b " +
                           "where a.formno=b.formno AND Panno<>'' and Ispanverified in ('Y','N') " +
                           "and panno='" + txtpan.Text + "'" + Obj.IsoEnd;

            Ds12 = SqlHelper.ExecuteDataset(
                        HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                        CommandType.Text,
                        str12);

            dt12 = Ds12.Tables[0];

            if (Convert.ToInt32(dt12.Rows[0]["cnt"]) > 1)
            {
                result = false;
            }
            else if (Convert.ToInt32(dt12.Rows[0]["cnt"]) == 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }
        catch
        {
            return false;
        }
    }
    private bool panverifycashless()
    {
        try
        {
            bool result = false;
            DataSet Ds12 = new DataSet();
            DataTable dt12 = new DataTable();

            string str12 = Obj.IsoStart + "select count(panno) as cnt from " + Obj.dBName + "..KycVerify as a, " + Obj.dBName + "..m_membermaster as b " +
                           "where a.formno=b.formno AND Panno<>'' " +
                           "and Ispanverified in ('Y','N') and panno='" + txtpan.Text + "'" + Obj.IsoEnd;

            Ds12 = SqlHelper.ExecuteDataset(
                        HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                        CommandType.Text,
                        str12);

            dt12 = Ds12.Tables[0];

            if (Convert.ToInt32(dt12.Rows[0]["cnt"]) >= 3)
                result = true;
            else if (Convert.ToInt32(dt12.Rows[0]["cnt"]) == 0)
                result = false;
            else
                result = false;

            return result;
        }
        catch
        {
            return false;
        }
    }
    private bool aadharverify()
    {
        try
        {
            bool result = false;
            DataSet Ds12 = new DataSet();
            DataTable dt12 = new DataTable();

            string str12 = Obj.IsoStart + "select count(AadharNo) as cnt from " + Obj.dBName + "..KycVerify as a, " + Obj.dBName + "..m_membermaster as b " +
                           "where a.formno=b.formno AND AadharNo<>'' " +
                           "and IsAddrssVerified in ('N','Y') and AadharNo='" + TxtIdProofNo.Text + "'" + Obj.IsoEnd;

            Ds12 = SqlHelper.ExecuteDataset(
                        HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                        CommandType.Text,
                        str12);

            dt12 = Ds12.Tables[0];

            if (Convert.ToInt32(dt12.Rows[0]["cnt"]) > 1)
                result = false;
            else if (Convert.ToInt32(dt12.Rows[0]["cnt"]) == 0)
                result = true;
            else
                result = false;

            return result;
        }
        catch
        {
            return false;
        }
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlState.SelectedValue == "0")
        {
            BtnIdentity.Enabled = false;

            ScriptManager.RegisterStartupScript(
                this, this.GetType(), "Key",
                "alert('please select State name.!!');",
                true
            );

            txtpan.Text = "";
            return;
        }
        else
        {
            BtnIdentity.Enabled = true;
        }
    }
    private void BindImage(string dbValue, Image img, HtmlAnchor anchor, Label lbl, WebControl uploadControl)
    {
        if (string.IsNullOrEmpty(dbValue))
        {
            img.ImageUrl = "~/images/no_photo.jpg";
            anchor.HRef = "~/images/no_photo.jpg";
        }
        else
        {
            img.ImageUrl = dbValue;
            anchor.HRef = dbValue;

            if (lbl != null)
                lbl.Text = dbValue;

            if (uploadControl != null)
                uploadControl.Attributes.Add("class", "form-control");
        }
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
                    encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                    break;

                case ".png":
                    codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.MimeType == "image/png");
                    encoderParams = null; // PNG doesn't support quality settings
                    break;

                case ".gif":
                    codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.MimeType == "image/gif");
                    encoderParams = null;
                    break;

                default:
                    throw new Exception("Unsupported file type.");
            }

            if (codec != null)
            {
                if (encoderParams != null)
                {
                    img.Save(savePath, codec, encoderParams);
                }
                else
                {
                    img.Save(savePath, codec, null);
                }
            }
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

            // KYC switch for specific company
            if (Session["CompID"] != null && Session["CompID"].ToString() == "1007")
            {
                if (!GetKycPerStatus())
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key",
                        "alert('Kyc option is temporarily off. Please try after some time.!!');location.replace('" +
                        Session["IndexPage"] + "');", true);
                    return;
                }
            }

            string condition = "";
            DataTable dt1 = new DataTable();
            if (Session["CompID"] != null && Session["CompID"].ToString() == "1091")
            {
                if (DDLAddressProof.SelectedValue == "1")
                {
                    if (!string.IsNullOrWhiteSpace(TxtIdProofNo.Text))
                    {
                        string s1 = Obj.IsoStart + "select Count(IdProofNo) as AadharNo from " + Obj.dBName + "..KycVerify where IdProofNo='" +
                                    TxtIdProofNo.Text.Trim() + "' AND FORMNO<>'" + Convert.ToInt32(Session["FormNo"]) + "'" + Obj.IsoEnd;
                        dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, s1).Tables[0];

                        if (dt1.Rows.Count > 0 && Convert.ToInt32(dt1.Rows[0]["AadharNo"]) >= 1)
                        {
                            scrname = "<SCRIPT language='javascript'>alert('Already Registered by this Aadhar No.');</SCRIPT>";
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Login Error", scrname, false);
                            return;
                        }
                        else
                        {
                            condition = ",AadharNo='" + TxtIdProofNo.Text.Trim() + "'";
                        }
                    }
                }
            }

            // panverifycashless check for comp 1074
            if (Session["CompID"] != null && Session["CompID"].ToString() == "1074")
            {
                if (!panverifycashless())
                {
                    BtnIdentity.Enabled = true;
                }
                else
                {
                    BtnIdentity.Enabled = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key",
                        "alert('Pan card already register another id.!!');", true);
                    txtpan.Text = "";
                    return;
                }
            }

            // File variables
            string AdrsProof = "", BackAdrsProof = "", BaNKProof = "", panproof = "";
            string FlAddrs = "", FlBackAddrs = "", FlBank = "", FlPan = "";
            string strextension = "";

            // Pre-check: if Fuidentity.Enabled == true then file must be present
            if (Fuidentity.Enabled && !Fuidentity.HasFile)
            {
                scrname = "<SCRIPT language='javascript'>alert('Please front address upload jpg/jpeg/png/ image of upto 20 mb size only!! ');</SCRIPT>";
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Close", scrname, false);
                return;
            }

            // CompID folder root
            string compId = Session["CompID"] != null ? Session["CompID"].ToString() : "0";
            string uploadRoot = Server.MapPath("~/images/UploadImage/" + compId + "/");
            if (!Directory.Exists(uploadRoot))
            {
                Directory.CreateDirectory(uploadRoot);
            }

            // PAN length check for comp 1074
            //if (Session["CompID"] != null && Session["CompID"].ToString() == "1074" )
            //{
            //    if (txtpan.Text.Trim().Length < 10)
            //    {
            //        scrname = "<SCRIPT language='javascript'>alert('Invalid Pan no!! ');</SCRIPT>";
            //        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Close", scrname, false);
            //        return;
            //    }
            //}
            if (Session["CompID"] != null)
            {
                if (txtpan.Text.Trim().Length < 10)
                {
                    scrname = "<SCRIPT language='javascript'>alert('Invalid PAN No!!');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Close", scrname, false);
                    return;
                }
            }
            if (Fuidentity.HasFile)
            {
                strextension = System.IO.Path.GetExtension(Fuidentity.FileName);
                if (strextension.ToUpper() == ".JPG" || strextension.ToUpper() == ".JPEG" || strextension.ToUpper() == ".PNG")
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(Fuidentity.PostedFile.InputStream);
                    int height = img.Height;
                    int width = img.Width;
                    decimal size = Math.Round((decimal)(Fuidentity.PostedFile.ContentLength) / 1024, 1);
                    if (size > 5120)
                    {
                        scrname = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                        return;
                    }
                    else
                    {
                        FlAddrs = "FA" + DateTime.Now.ToString("yyMMddhhmmssfff") + Session["formno"].ToString() + System.IO.Path.GetExtension(Fuidentity.PostedFile.FileName);

                        string fullPath = Path.Combine(uploadRoot, FlAddrs);
                        Fuidentity.PostedFile.SaveAs(fullPath);
                        CompressAndSaveImage(Fuidentity.PostedFile.InputStream, fullPath, strextension, 50L); // Quality 50%
                        AdrsProof = "https://" + HttpContext.Current.Request.Url.Host + "/images/UploadImage/" + compId + "/" + FlAddrs;
                    }
                }
                else
                {
                    scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg, .jpeg, and .png extension files!! ');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                    return;
                }
            }
            else
            {
                AdrsProof = lblimage.Text;
            }

            if (FileUpload1.Enabled)
            {
                if (!FileUpload1.HasFile)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Close", "<SCRIPT language='javascript'>alert('Please upload a jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>", false);
                    return;
                }
            }
            if (FileUpload1.HasFile)
            {
                strextension = System.IO.Path.GetExtension(FileUpload1.FileName);
                if (strextension.ToUpper() == ".JPG" || strextension.ToUpper() == ".JPEG" || strextension.ToUpper() == ".PNG")
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream);
                    int height = img.Height;
                    int width = img.Width;
                    decimal size = Math.Round((decimal)(FileUpload1.PostedFile.ContentLength) / 1024, 1);

                    if (size > 5120)
                    {
                        scrname = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                        return;
                    }
                    else
                    {
                        FlBackAddrs = "BA" + DateTime.Now.ToString("yyMMddhhmmssfff") + Session["formno"].ToString() + System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);

                        string fullPath = Path.Combine(uploadRoot, FlBackAddrs);
                        FileUpload1.PostedFile.SaveAs(fullPath);
                        CompressAndSaveImage(FileUpload1.PostedFile.InputStream, fullPath, strextension, 50L); // Quality 50%
                        BackAdrsProof = "https://" + HttpContext.Current.Request.Url.Host + "/images/UploadImage/" + compId + "/" + FlBackAddrs;
                    }
                }
                else
                {
                    scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg, .jpeg, and .png extension files!! ');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                    return;
                }
            }
            else
            {
                BackAdrsProof = LblBackImage.Text;
            }

            if (Pankyc.Enabled)
            {
                if (!Pankyc.HasFile)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Close", "<SCRIPT language='javascript'>alert('Please upload a jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>", false);
                    return;
                }
            }

            if (Pankyc.HasFile)
            {
                strextension = System.IO.Path.GetExtension(Pankyc.FileName);
                if (strextension.ToUpper() == ".JPG" || strextension.ToUpper() == ".JPEG" || strextension.ToUpper() == ".PNG")
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(Pankyc.PostedFile.InputStream);
                    int height = img.Height;
                    int width = img.Width;
                    decimal size = Math.Round((decimal)(Pankyc.PostedFile.ContentLength) / 1024, 1);

                    if (size > 5120)
                    {
                        scrname = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                        return;
                    }
                    else
                    {
                        FlPan = "PAN" + DateTime.Now.ToString("yyMMddhhmmssfff") + Session["formno"].ToString() + System.IO.Path.GetExtension(Pankyc.PostedFile.FileName);

                        string fullPath = Path.Combine(uploadRoot, FlPan);

                        Pankyc.PostedFile.SaveAs(fullPath);
                        CompressAndSaveImage(Pankyc.PostedFile.InputStream, fullPath, strextension, 50L); // Quality 50%
                        panproof = "https://" + HttpContext.Current.Request.Url.Host + "/images/UploadImage/" + compId + "/" + FlPan;
                    }
                }
                else
                {
                    scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg, .jpeg, and .png extension files!! ');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                    return;
                }
            }
            else
            {
                panproof = LblPanImage.Text;
            }

            if (BankKYCFileUpload3.Enabled)
            {
                if (!BankKYCFileUpload3.HasFile)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Close", "<SCRIPT language='javascript'>alert('Please upload a jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>", false);
                    return;
                }
            }
            if (BankKYCFileUpload3.HasFile)
            {
                strextension = System.IO.Path.GetExtension(BankKYCFileUpload3.FileName);
                if (strextension.ToUpper() == ".JPG" || strextension.ToUpper() == ".JPEG" || strextension.ToUpper() == ".PNG")
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(BankKYCFileUpload3.PostedFile.InputStream);
                    int height = img.Height;
                    int width = img.Width;
                    decimal size = Math.Round((decimal)(BankKYCFileUpload3.PostedFile.ContentLength) / 1024, 1);

                    if (size > 5120)
                    {
                        scrname = "<SCRIPT language='javascript'>alert('Please upload jpg/jpeg/png image of up to 5 MB size only!! ');</SCRIPT>";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                        return;
                    }
                    else
                    {
                        FlBank = "Bank" + DateTime.Now.ToString("yyMMddhhmmssfff") + Session["formno"].ToString() + System.IO.Path.GetExtension(BankKYCFileUpload3.PostedFile.FileName);

                        string fullPath = Path.Combine(uploadRoot, FlBank);
                        BankKYCFileUpload3.PostedFile.SaveAs(fullPath);
                        CompressAndSaveImage(BankKYCFileUpload3.PostedFile.InputStream, fullPath, strextension, 50L); // Quality 50%
                        BaNKProof = "https://" + HttpContext.Current.Request.Url.Host + "/images/UploadImage/" + compId + "/" + FlBank;
                    }
                }
                else
                {
                    scrname = "<SCRIPT language='javascript'>alert('You can upload only .jpg, .jpeg, and .png extension files!! ');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                    return;
                }
            }
            else
            {
                BaNKProof = LblBankImage.Text;
            }



            // -------------------- BANK SELECT or Insert logic --------------------
            double dblBank = 0;
            DataTable Dt = new DataTable();
            if (cmbbank.SelectedItem != null && cmbbank.SelectedItem.Text.ToUpper() == "OTHERS")
            {
                if (!string.IsNullOrWhiteSpace(Txtbank.Text))
                {
                    string Q1 = Obj.IsoStart + "Select * from " + Obj.dBName + "..M_BankMaster where BankName='" + Txtbank.Text.Trim() +
                                "' and Activestatus='Y'and RowStatus='Y' " + Obj.IsoEnd;
                    Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Q1).Tables[0];
                    if (Dt.Rows.Count == 0)
                    {
                        Q1 = "insert into M_BankMaster (BankCode,BankName,AcNo,IFSCode,Remarks,ActiveStatus,LastModified,UserCode,UserId,IPAdrs,RowStatus) " +
                             " Select Case When Max(BankCode) Is Null Then '1' Else Max(BankCode)+1 END as BankCode,'" + Txtbank.Text +
                             "','0','0','', 'Y','Add by " + Session["IdNo"] + " at " + DateTime.Now.ToString() +
                             "','" + Session["MemName"] + "','" + Convert.ToInt32(Session["FormNo"]) + "','','Y' From M_BankMaster ";
                        i = Obj.SaveData(Q1);
                        if (i > 0)
                        {
                            Q1 = Obj.IsoStart + " select Max(BankCode) as BankCode from " + Obj.dBName + "..M_BankMaster where ActiveStatus='Y' and RowStatus='Y'" + Obj.IsoEnd;
                            dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Q1).Tables[0];
                            if (dt1.Rows.Count > 0) dblBank = Convert.ToDouble(dt1.Rows[0]["BankCode"]);
                        }
                    }
                    else
                    {
                        dblBank = Convert.ToDouble(Dt.Rows[0]["BankCode"]);
                    }
                }
            }
            else
            {
                // if cmbbank.SelectedValue is not numeric this will throw - but following VB logic kept.
                if (!string.IsNullOrEmpty(cmbbank.SelectedValue))
                    double.TryParse(cmbbank.SelectedValue, out dblBank);
            }
            if (!string.IsNullOrEmpty(Txtbank.Text) || !string.IsNullOrWhiteSpace(Txtcode.Text))
            {
                if (cmbbank.SelectedValue == "0" || string.IsNullOrEmpty(cmbbank.SelectedValue))
                {
                    scrname = "<SCRIPT language='javascript'>alert('Choose Bank Name');</SCRIPT>";
                    RegisterStartupScript("MyAlert", scrname);
                    return;
                }
                if (string.IsNullOrEmpty(Txtbranch.Text))
                {
                    scrname = "<SCRIPT language='javascript'>alert('Enter Branch Name.');</SCRIPT>";
                    RegisterStartupScript("MyAlert", scrname);
                    return;
                }
                if (string.IsNullOrEmpty(Txtcode.Text))
                {
                    scrname = "<SCRIPT language='javascript'>alert('Enter IFSC Code.');</SCRIPT>";
                    RegisterStartupScript("MyAlert", scrname);
                    return;
                }
            }
            // BANK / IFSC / Branch validations if user entered bank or IFSC provided
            if (!string.IsNullOrEmpty(Txtbank.Text) || !string.IsNullOrWhiteSpace(Txtcode.Text))
            {
                if (cmbbank.SelectedValue == "0" || string.IsNullOrEmpty(cmbbank.SelectedValue))
                {
                    scrname = "<SCRIPT language='javascript'>alert('Choose Bank Name');</SCRIPT>";
                    RegisterStartupScript("MyAlert", scrname);
                    return;
                }
                if (string.IsNullOrEmpty(Txtbranch.Text))
                {
                    scrname = "<SCRIPT language='javascript'>alert('Enter Branch Name.');</SCRIPT>";
                    RegisterStartupScript("MyAlert", scrname);
                    return;
                }
                if (string.IsNullOrEmpty(Txtcode.Text))
                {
                    scrname = "<SCRIPT language='javascript'>alert('Enter IFSC Code.');</SCRIPT>";
                    RegisterStartupScript("MyAlert", scrname);
                    return;
                }
            }

            // -------------------- Build Remark by comparing previous data --------------------
            string Remark = "";
            string str = Obj.IsoStart + " Exec sp_FillKyc '" + Convert.ToInt32(Session["Formno"]) + "'" + Obj.IsoEnd;
            DataTable dtCompare = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];
            if (dtCompare.Rows.Count > 0)
            {
                if (ClearInject(dtCompare.Rows[0]["Address1"].ToString()) != ClearInject(txtaddrs.Text))
                    Remark += "Address ,";
                if (ClearInject(dtCompare.Rows[0]["City"].ToString()) != ClearInject(Txtcity.Text))
                    Remark += " City ,";
                if (ClearInject(dtCompare.Rows[0]["District"].ToString()) != ClearInject(Txtdistrict.Text))
                    Remark += " District,";
                if (Convert.ToInt32(dtCompare.Rows[0]["StateCode"]) != Convert.ToInt32(ddlState.SelectedValue))
                    Remark += " State ,";
                if (ddlState.SelectedValue == "0")
                    ddlState.SelectedValue = dtCompare.Rows[0]["StateCode"].ToString();
                if (ClearInject(dtCompare.Rows[0]["PinCode"].ToString()) != ClearInject(Txtpincode.Text))
                    Remark += " PinCode,";
                if (ClearInject(dtCompare.Rows[0]["AddrProof"].ToString()) != ClearInject(AdrsProof))
                    Remark += " AddressProof,";
                if (ClearInject(dtCompare.Rows[0]["BackAddressProof"].ToString()) != ClearInject(BackAdrsProof))
                    Remark += " BackAddressProof,";
                if (ClearInject(dtCompare.Rows[0]["id"].ToString()) != ClearInject(DDLAddressProof.SelectedValue))
                    Remark += " AddressProofType,";
                if (ClearInject(dtCompare.Rows[0]["IdProofNo"].ToString()) != ClearInject(TxtIdProofNo.Text.Trim()))
                    Remark += "AddressProofNo,";
                if (Convert.ToString(dtCompare.Rows[0]["BankId"]) != cmbbank.SelectedValue)
                    Remark += " Bank,";
                if (ClearInject(dtCompare.Rows[0]["BranchName"].ToString()) != ClearInject(Txtbranch.Text))
                    Remark += " BranchName,";
                if (ClearInject(dtCompare.Rows[0]["AcNo"].ToString()) != ClearInject(Txtacno.Text))
                    Remark += " AccountNo,";
                if (ClearInject(dtCompare.Rows[0]["IFSCode"].ToString()) != ClearInject(Txtcode.Text))
                    Remark += " IFSCCode,";
                if (ClearInject(dtCompare.Rows[0]["BankProof"].ToString()) != ClearInject(BaNKProof))
                    Remark += " BankProof,";
                if (Session["CompID"] != null && (CompID == "1102" ||
                        CompID == "1100" || CompID == "1106" || CompID == "1107" || CompID == "1108" || CompID == "1105" || CompID == "1109" || CompID == "1110"))
                {
                    if (dtCompare.Rows[0]["Fax"].ToString() != DDLAccountType.SelectedItem.Text)
                        Remark += " Account Type,";
                }
                if (ClearInject(dtCompare.Rows[0]["Panno"].ToString()) != ClearInject(txtpan.Text))
                    Remark += " PANNo,";
                if (ClearInject(dtCompare.Rows[0]["PanImg"].ToString()) != ClearInject(panproof))
                    Remark += " PanCardImage,";
            }

            // -------------------- Required dropdown validations --------------------
            if (DDLAddressProof.SelectedValue == "0")
            {
                scrname = "<SCRIPT language='javascript'>alert('Choose ID Proof Type.');</SCRIPT>";
                RegisterStartupScript("MyAlert", scrname);
                return;
            }

            if (ddlState.SelectedValue == "0" || string.IsNullOrEmpty(ddlState.SelectedValue))
            {
                scrname = "<SCRIPT language='javascript'>alert('Select State Name.');</SCRIPT>";
                RegisterStartupScript("MyAlert", scrname);
                return;
            }

            // -------------------- AreaCode handling (Village) --------------------
            int AreaCode = 0;
            if (!(Session["CompID"] != null &&
                 (CompID == "1100" || CompID == "1102" ||
                 CompID == "1101" || CompID == "1106" ||
                 CompID == "1105" || CompID == "1108" || CompID == "1107" || CompID == "1109" || CompID == "1110")))
            {
                if (DDlVillage.SelectedItem != null && DDlVillage.SelectedItem.Text.ToUpper() == "OTHERS")
                {
                    if (!string.IsNullOrWhiteSpace(TxtVillage.Text))
                    {
                        string q = Obj.IsoStart + "Select * from " + Obj.dBName + "..M_VillageMaster where VillageName='" + TxtVillage.Text.Trim() +
                                   "' and Activestatus='Y' and Pincode='" + Convert.ToInt32(Txtpincode.Text) + "' " + Obj.IsoEnd;
                        Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, q).Tables[0];
                        if (Dt.Rows.Count == 0)
                        {
                            q = "insert into M_VillageMaster (VillageName,CityCode,PinCode) " +
                                " Values('" + TxtVillage.Text.ToUpper() + "','" + HCityCode.Value + "','" + Txtpincode.Text + "')";
                            i = Obj.SaveData(q);
                            if (i > 0)
                            {
                                q = Obj.IsoStart + " select Max(VillageCode) as VillageCode from " + Obj.dBName + "..M_VillageMaster where ActiveStatus='Y'" + Obj.IsoEnd;
                                Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, q).Tables[0];
                                if (Dt.Rows.Count > 0) AreaCode = Convert.ToInt32(Dt.Rows[0]["VillageCode"]);
                            }
                        }
                        else
                        {
                            AreaCode = Convert.ToInt32(Dt.Rows[0]["VillageCode"]);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(DDlVillage.SelectedValue))
                        AreaCode = Convert.ToInt32(DDlVillage.SelectedValue);
                }
            }

            string Strqueryquer = "";
            Strqueryquer = "Insert into Trnjoining(Transid)values(" + HdnCheckTrnns.Value + ")";
            int isOk1 = 0;
            try
            {
                isOk1 = Convert.ToInt32(SqlHelper.ExecuteNonQuery(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(), CommandType.Text, Strqueryquer));
            }
            catch (Exception ex)
            {
                isOk1 = 0;
            }
            if (isOk1 > 0)
            {
                // -------------------- Build final SQL & save --------------------
                string Qry = "Insert Into TempMemberMaster Select *,'Update KYC - " + Context.Request.UserHostAddress.ToString() +
                         "',GetDate(),'U' From M_MemberMaster Where FormNo='" + Convert.ToInt32(Session["FormNo"]) + "'";
                Qry += " ; insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values" +
                       "(0,'" + Session["MemName"] + "','KYC Detail','KYC Detail Update','" + Remark + "',Getdate(),'" + Session["FormNo"] + "')";

                string sql = Qry + ";Update m_MemberMaster set Address1='" + txtaddrs.Text.ToUpper() +
                             "', Tehsil='" + Txtcity.Text.ToUpper() + "',City='" + Txtcity.Text.ToUpper() +
                             "',District='" + Txtdistrict.Text.ToUpper() + "',StateCode='" + ddlState.SelectedValue +
                             "', Pincode='" + Txtpincode.Text + "' ,AreaCode='" + AreaCode + "',Fax='" + DDLAccountType.SelectedItem.Text +
                             "', CityCode='" + HCityCode.Value + "',DistrictCode='" + HDistrictCode.Value + "',Panno='" + txtpan.Text.ToUpper() +
                             "',Acno='" + Txtacno.Text + "',Bankid='" + dblBank + "',IFscode='" + Txtcode.Text.ToUpper() +
                             "', Branchname='" + Txtbranch.Text.ToUpper() + "'" + condition + " where Formno= '" + Session["Formno"] + "'";

                if ((Session["CompID"] != null && (CompID == "1102" || CompID == "1106" || CompID == "1108" || CompID == "1105" || CompID == "1107" || CompID == "1109" || CompID == "1110")))
                {
                    sql = sql + ";Update KycVerify Set Idtype='" + DDLAddressProof.SelectedValue +
                      "',IdProofNo='" + TxtIdProofNo.Text.Trim().ToUpper() + "'," +
                      " AddrProof='" + AdrsProof + "',BackAddressProof='" + BackAdrsProof + "',BackAddressDate=Getdate(),IsaddrssVerified='N',IsAddress = 'N'," +
                      " PanImg='" + panproof + "',PANImgDate=Getdate(),IsPanVerified='N',IsPan = 'N',BankProof='" + BaNKProof + "',BankProofDate=Getdate(),IsBankVerified='N',IsBank = 'N' where Formno= '" + Session["Formno"] + "'";
                }
                else
                {
                    sql = sql + ";Update KycVerify Set Idtype='" + DDLAddressProof.SelectedValue +
                      "',IdProofNo='" + TxtIdProofNo.Text.Trim().ToUpper() + "'," +
                      " AddrProof='" + AdrsProof + "',BackAddressProof='" + BackAdrsProof + "',BackAddressDate=Getdate(),IsaddrssVerified='N'," +
                      " PanImg='" + panproof + "',PANImgDate=Getdate(),IsPanVerified='N',BankProof='" + BaNKProof + "',BankProofDate=Getdate(),IsBankVerified='N' where Formno= '" + Session["Formno"] + "'";
                }
                int j = Obj.SaveData(sql);
                if (j != 0)
                {

                    scrname = "<script>alert('KYC Upload successfully');location.replace('kyc.aspx');</script>";
                    Session["ScrName"] = scrname;
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                    return;
                }
                else
                {
                    string url = "kyc.aspx";
                    string script = "window.onload = function(){ alert('";
                    script += "KYC Upload unsuccessfully";
                    script += "');";
                    script += "window.location = '";
                    script += url;
                    script += "'; }";
                    ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                    return;
                }
            }
            else
            {
                string url = "kyc.aspx";
                string script = "window.onload = function(){ alert('";
                script += "Try After Some Time.!";
                script += "');";
                script += "window.location = '";
                script += url;
                script += "'; }";
                ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                return;
            }
        }
        catch (Exception ex)
        {
            try
            {
                string path = HttpContext.Current.Request.Url.AbsoluteUri;
                string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff ") + Environment.NewLine;
                if (Obj != null) Obj.WriteToFile(text + ex.Message);
            }
            catch { }
            Response.Write("Try later.");
        }
    }
}