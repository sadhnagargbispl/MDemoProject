using AjaxControlToolkit;
using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public partial class fasaidetail : System.Web.UI.Page
{
    DAL Obj;
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
                lblverstatus.Text = dt.Rows[0]["FSSAIVerf"].ToString();
                Lblverdate.Text = dt.Rows[0]["FSSAIVerifyDate"] == DBNull.Value ? "" : dt.Rows[0]["FSSAIVerifyDate"].ToString();
                LblRemark.Text = dt.Rows[0]["RejectRemark"].ToString();
                LbLrejectRemark.Text = dt.Rows[0]["RejectReason"].ToString();
                status = dt.Rows[0]["FSSAIVerf"].ToString();
                txtpan.Text = dt.Rows[0]["FSSAINo"].ToString();
                if (string.IsNullOrEmpty(txtpan.Text))
                    txtpan.Enabled = true; // Enable if empty
                else
                    txtpan.Enabled = false;// Disable if not empty
                if (dt.Rows[0]["IsFSSAI"].ToString() != "R" && dt.Rows[0]["IsFSSAI"].ToString() != "P")
                {
                    Fuidentity.Enabled = false;
                    Fuidentity.Attributes.Add("class", "form-control");
                }
                else
                {
                    Fuidentity.Enabled = true;
                    c++;
                }
                BindImage(dt.Rows[0]["FSSAIimage"].ToString(), ShowIdentity, FrontAddress, lblimage, Fuidentity);
            }
            if (dt.Rows[0]["IsFSSAI"].ToString() != "R" && c == 0)
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Key", "alert('FSSAI Number must be exactly 14 characters.');location.replace('fasaidetail.aspx');", true);
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
            str = Obj.IsoStart + "Exec sp_FillKyc '" + Session["Formno"] + "'" + Obj.IsoEnd;
            Dt1 = SqlHelper.ExecuteDataset((string)HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]], CommandType.Text, str).Tables[0];
            if (Dt1.Rows.Count > 0)
            {
                DataRow r = Dt1.Rows[0];
                if (ClearInject(r["FSSAINo"].ToString()) != ClearInject(txtpan.Text))
                    Remark += " FSSAINo,";

                if ((r["FSSAIimage"] == DBNull.Value ? "" : r["FSSAIimage"].ToString()) != AdrsProof)
                    Remark += " FSSAI Certificate,";
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
                "Insert Into TempMemberMaster Select *,'Update FSSAI Certificate - " +
                Request.UserHostAddress + "',GetDate(),'U' From M_MemberMaster Where FormNo='" + Session["FormNo"] + "';" +
                "Insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId) Values(0,'" + Session["MemName"] + "','FSSAI Certificate','FSSAI Certificate Update','" +
                Remark + "',GetDate(),'" + Session["FormNo"] + "');";
                string sql = Qry + " Update KycVerify set FSSAINo = '" + txtpan.Text.ToUpper() + "',FSSAIimage = '" + AdrsProof + "', FSSAIImageDate = GetDate(), IsFSSAIVerified = 'N',IsFSSAI = 'N' where Formno = '" + Session["Formno"] + "'";
                int j = Obj.SaveData(sql);
                if (j != 0)
                {
                    string url = "fasaidetail.aspx";
                    string script = "window.onload = function(){ alert('";
                    script += "FSSAI Certificate Upload successfully.!";
                    script += "');";
                    script += "window.location = '";
                    script += url;
                    script += "'; }";
                    ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                    return;
                }
                else
                {
                    string url = "fasaidetail.aspx";
                    string script = "window.onload = function(){ alert('";
                    script += "FSSAI Certificate Upload unsuccessfully.!";
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
                string url = "fasaidetail.aspx";
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