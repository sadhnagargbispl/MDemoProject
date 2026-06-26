using AjaxControlToolkit;
using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public partial class gstdetail : System.Web.UI.Page
{
    DAL Obj;
    // PDF thumbnail inline icon (koi physical file ki zaroorat nahi)
    private const string PdfIconDataUri = "data:image/svg+xml;base64,PHN2ZyB4bWxucz0naHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmcnIHZpZXdCb3g9JzAgMCA2NCA2NCc+PHJlY3QgeD0nMTInIHk9JzQnIHdpZHRoPSczNicgaGVpZ2h0PSc1Nicgcng9JzQnIGZpbGw9JyNmZmZmZmYnIHN0cm9rZT0nI2U1MzkzNScgc3Ryb2tlLXdpZHRoPSczJy8+PHBhdGggZD0nTTQwIDQgbDggOCBoLTggeicgZmlsbD0nI2U1MzkzNScvPjxyZWN0IHg9JzgnIHk9JzM0JyB3aWR0aD0nNDAnIGhlaWdodD0nMTYnIHJ4PScyJyBmaWxsPScjZTUzOTM1Jy8+PHRleHQgeD0nMjgnIHk9JzQ2JyBmb250LWZhbWlseT0nQXJpYWwsSGVsdmV0aWNhLHNhbnMtc2VyaWYnIGZvbnQtc2l6ZT0nMTEnIGZvbnQtd2VpZ2h0PSdib2xkJyBmaWxsPScjZmZmZmZmJyB0ZXh0LWFuY2hvcj0nbWlkZGxlJz5QREY8L3RleHQ+PC9zdmc+";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                if (!Page.IsPostBack)
                {
                    HdnCheckTrnns.Value = GenerateRandomStringJoining(6);
                    Obj = new DAL();
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

    // Saved attachment ka preview: PDF -> inline icon + View PDF link. Empty -> kuch nahi dikhega.
    private void BindAttachment(string dbValue)
    {
        if (string.IsNullOrEmpty(dbValue))
        {
            divAttach.Visible = false;   // attachment hi nahi -> Attachment block hide
            return;
        }
        lblimage.Text = dbValue;                                  // save ke liye purana url
        ShowIdentity.ImageUrl = dbValue.ToLower().EndsWith(".pdf") ? PdfIconDataUri : dbValue;
        FrontAddress.HRef = dbValue;                              // click -> actual file (nayi tab)
        aViewGst.HRef = dbValue;
        divAttach.Visible = true;
    }

    private void loadImages()
    {
        try
        {
            int c = 0;
            string status = "";
            string str = "";
            DataTable dt = new DataTable();
            str = Obj.IsoStart + "Exec sp_FillGSTKyc " + Convert.ToInt32(Session["Formno"]) + Obj.IsoEnd;
            dt = SqlHelper.ExecuteDataset((string)HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]], CommandType.Text, str).Tables[0];
            if ((dt.Rows.Count > 0))
            {
                lblverstatus.Text = dt.Rows[0]["GSTVerf"].ToString();
                Lblverdate.Text = dt.Rows[0]["GstVerifyDate"] == DBNull.Value ? "" : dt.Rows[0]["GstVerifyDate"].ToString();
                LblRemark.Text = dt.Rows[0]["RejectRemark"].ToString();
                LbLrejectRemark.Text = dt.Rows[0]["RejectReason"].ToString();
                status = dt.Rows[0]["GSTVerf"].ToString();
                txtpan.Text = dt.Rows[0]["GSTNo"].ToString();
                if (string.IsNullOrEmpty(txtpan.Text))
                    txtpan.Enabled = true; // Enable if empty
                else
                    txtpan.Enabled = false;// Disable if not empty
                if (dt.Rows[0]["IsGSTVerified"].ToString() != "R" && dt.Rows[0]["IsGSTVerified"].ToString() != "P")
                {
                    Fuidentity.Enabled = false;
                    Fuidentity.Attributes.Add("class", "form-control");
                }
                else
                {
                    Fuidentity.Enabled = true;
                    c++;
                }
                // Attachment preview (PDF icon + View PDF) - attachment na ho to kuch nahi dikhega
                BindAttachment(dt.Rows[0]["PDFLink1"].ToString());
            }
            if (dt.Rows[0]["IsGSTVerified"].ToString() != "R" && c == 0)
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
                txtpan.Enabled = true;
            }
            else if (status == "Rejected")
            {
                VerifyDate.Visible = true;
                Lblverdate.Visible = true;
                LblVerfReason.Visible = true;
                LblVerfRemark.Visible = true;
                VerifyDate.Text = "Reject Date:";
                DivVerify.Attributes.Add("style", "color:red");
                txtpan.Enabled = true;
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
    protected void BtnIdentity_Click(object sender, EventArgs e)
    {
        try
        {
            string FlNm = "";
            string scrname = "";
            string AdrsProof = "";
            string strextension = "";
            string Remark = "";
            DataTable Dt1 = new DataTable();
            string str = "";
            if (!Fuidentity.HasFile)
            {
                lblimage.Text = "Please upload PDF file.";
                return;
            }
            string ext = Path.GetExtension(Fuidentity.FileName).ToLower();
            if (ext != ".pdf")
            {
                lblimage.Text = "Only PDF files are allowed.";
                return;
            }
            // 2 MB = 2097152 bytes
            decimal sizeKb = Math.Round((decimal)Fuidentity.PostedFile.ContentLength / 1024, 1);

            if (sizeKb > 2048) // 2 MB = 2048 KB
            {
                scrname = "<script>alert('Please upload PDF/JPG/PNG file upto 2 MB size only.!');</script>";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtpan.Text) || txtpan.Text.Trim().Length != 14)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('GST Number must be exactly 14 characters.');location.replace('GSTDetail.aspx');", true);
                return;
            }
            string compId = Session["CompID"] != null ? Session["CompID"].ToString() : "0";
            string uploadRoot = Server.MapPath("~/images/UploadImage/" + compId + "/Uploads/");
            if (!Directory.Exists(uploadRoot))
            {
                Directory.CreateDirectory(uploadRoot);
            }
            // File upload handling
            if (Fuidentity.HasFile)
            {
                strextension = Path.GetExtension(Fuidentity.FileName).ToUpper();
                if (strextension == ".PDF")
                {
                    FlNm = DateTime.Now.ToString("yyMMddHHmmssfff") + Session["CompID"] + Path.GetExtension(Fuidentity.FileName);
                    string fullPath = Path.Combine(uploadRoot, FlNm);
                    Fuidentity.PostedFile.SaveAs(fullPath);
                    Fuidentity.PostedFile.SaveAs(Server.MapPath("~/images/UploadImage/") + FlNm);
                    AdrsProof = "https://" + HttpContext.Current.Request.Url.Host + "/images/UploadImage/" + compId + "/Uploads/" + FlNm;
                }
                else
                {
                    scrname = "<script>alert('You can upload only .pdf files!!');</script>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                    return;
                }
            }
            else
            {
                AdrsProof = lblimage.Text;
            }
            Obj = new DAL();
            str = Obj.IsoStart + "Exec sp_FillGSTKyc '" + Session["Formno"] + "'" + Obj.IsoEnd;
            Dt1 = SqlHelper.ExecuteDataset((string)HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]], CommandType.Text, str).Tables[0];
            if (Dt1.Rows.Count > 0)
            {
                DataRow r = Dt1.Rows[0];
                if (ClearInject(r["GSTNo"].ToString()) != ClearInject(txtpan.Text))
                    Remark += " GSTNo,";

                if ((r["GStImage"] == DBNull.Value ? "" : r["GStImage"].ToString()) != AdrsProof)
                    Remark += " GST Certificate,";
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
                string Qry =
                "Insert Into TempMemberMaster Select *,'Update GST Certificate - " +
                Request.UserHostAddress + "',GetDate(),'U' From M_MemberMaster Where FormNo='" + Session["FormNo"] + "';" +
                "Insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId) Values(0,'" + Session["MemName"] + "','GST Certificate','GST Certificate Update','" +
                Remark + "',GetDate(),'" + Session["FormNo"] + "');";
                string sql = Qry + " Update M_MemberMaster set AadharNo3 = '" + txtpan.Text.ToUpper() + "' where Formno = '" + Session["Formno"] + "';" +
                    " Update KycVerify set GStImage = '" + AdrsProof + "', GStImageDate = GetDate(), IsGSTVerified = 'N',IsGST = 'N' where Formno = '" + Session["Formno"] + "'";
                int j = Obj.SaveData(sql);

                if (j != 0)
                {
                    scrname = "<script>alert('GST Certificate Upload successfully.!');location.replace('gstdetail.aspx');</script>";
                    Session["ScrName"] = scrname;
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                    return;
                }
                else
                {
                    string url = "gstdetail.aspx";
                    string script = "window.onload = function(){ alert('";
                    script += "GST Certificate Upload unsuccessfully.!";
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
                string url = "gstdetail.aspx";
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
        }
    }
    private string ClearInject(string StrObj)
    {
        StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "");
        return StrObj;
    }
}