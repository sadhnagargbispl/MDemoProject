using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Complain : System.Web.UI.Page
{
    string scrname;
    // DAL obj = new DAL(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
    DAL Obj;
    clsGeneral objGen = new clsGeneral();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           

            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                try
                {
                    string str =
                        "exec('Create table TrncomplainUniqe ([ID] numeric(18,0) IDENTITY(1,1) NOT NULL,[Transid] numeric(18,0) NOT NULL,[Rectimestamp] datetime NOT NULL,PRIMARY KEY CLUSTERED ([Transid] ASC))')";

                    int i = SqlHelper.ExecuteNonQuery(
                                HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(),
                                CommandType.Text, str);
                }
                catch (Exception)
                { }

                BtnSubMit.Attributes.Add("onclick", DisableTheButton(this.Page, BtnSubMit));
                Obj = new DAL();
                if (!Page.IsPostBack)
                {
                    HdnCheckTrnns.Value = GenerateRandomString(6);

                    if (Session["CompID"].ToString() == "1055")
                    {
                        Bind_ComplaintType();
                        Filldetail();
                        hdnSessn.Value = Crypto.Encrypt(Session["IDNo"].ToString());

                        Iddiscountmart.Visible = true;
                        lblother.Text = "Complaint";
                        lbludaan.Visible = false;
                        lblother.Visible = true;
                    }
                    else if (Session["CompID"].ToString() == "1056")
                    {
                        lbludaan.Visible = true;
                        lblother.Visible = false;

                        lbludaan.Text = "Grievance";

                        Bind_ComplaintType();
                        Filldetail();

                        hdnSessn.Value = Crypto.Encrypt(Session["IDNo"].ToString());
                        Iddiscountmart.Visible = false;
                    }
                    else
                    {
                        lbludaan.Visible = false;
                        lblother.Visible = true;

                        Bind_ComplaintType();
                        Filldetail();

                        hdnSessn.Value = Crypto.Encrypt(Session["IDNo"].ToString());
                        Iddiscountmart.Visible = false;
                    }
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
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }
    }
    private void Bind_ComplaintType()
    {
        try
        {
            string Sql = Obj.IsoStart + "Select * FROM " + Obj.dBName + "..M_ComplaintTypeMaster WHERE RowStatus='Y' AND ActiveStatus='Y'" + Obj.IsoEnd;
            DataTable Dt = new DataTable();

            Obj = new DAL();
            Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Sql).Tables[0];

            CmbCmplntType.DataSource = Dt;
            CmbCmplntType.DataTextField = "CType";
            CmbCmplntType.DataValueField = "CTypeID";
            CmbCmplntType.DataBind();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }
    }
    protected void TxtDirectSeller_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string scrname = "";

            if (TxtDirectSeller.Text != "")
            {
                string str = Obj.IsoStart + "select * from " + Obj.dBName + "..M_MemberMaster where Idno='" + TxtDirectSeller.Text.Trim() + "'" + Obj.IsoEnd;
                Obj = new DAL();
                DataTable Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];

                if (Dt.Rows.Count > 0)
                {
                    TxtName.Text = Dt.Rows[0]["MemFirstName"].ToString() + " " + Dt.Rows[0]["MemLastName"].ToString();
                    TxtMobl.Text = Dt.Rows[0]["Mobl"].ToString();
                    TxtEmail.Text = Dt.Rows[0]["Email"].ToString();

                    TxtName.Enabled = false;
                    TxtEmail.Enabled = false;
                    TxtMobl.Enabled = false;
                }
                else
                {
                    TxtName.Text = "";
                    TxtMobl.Text = "";
                    TxtEmail.Text = "";

                    TxtName.Enabled = true;
                    TxtEmail.Enabled = true;
                    TxtMobl.Enabled = true;

                    scrname = "<SCRIPT>alert('Invalid Member Id.');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Error", scrname, false);
                    return;
                }
            }
            else
            {
                TxtName.Text = "";
                TxtMobl.Text = "";
                TxtEmail.Text = "";

                TxtName.Enabled = true;
                TxtEmail.Enabled = true;
                TxtMobl.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }
    }
    private void Filldetail()
    {
        try
        {
            string Str = Obj.IsoStart + "Select * from " + Obj.dBName + "..M_MemberMaster where Formno='" + Session["Formno"] + "'" + Obj.IsoEnd;
            DataTable Dt = new DataTable();

            Obj = new DAL();
            Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Str).Tables[0];

            if (Dt.Rows.Count > 0)
            {
                TxtDirectSeller.Text = Dt.Rows[0]["idno"].ToString();
                TxtName.Text = Dt.Rows[0]["Memfirstname"].ToString();
                TxtEmail.Text = Dt.Rows[0]["Email"].ToString();
                TxtMobl.Text = Dt.Rows[0]["Mobl"].ToString();
            }

            if (TxtEmail.Text == "")
                TxtEmail.Enabled = true;
            else
                TxtEmail.Enabled = false;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }
    }
    public bool SendMail()
    {
        try
        {
            string sql = Obj.IsoStart + "select top 1 * from " + Obj.dBName + "..M_ComplaintTypeMaster a, " + Obj.dBName + "..M_ComplaintMaster b, " +
                         "" + Obj.dBName + "..M_UserMaster c, " + Obj.dBName + "..M_UserGroupMaster d " +
                         " where a.CtypeId=b.CtypeId and c.UserId=a.UserId and c.GroupId=d.GroupId " +
                         " and d.ActiveStatus='Y' and d.RowStatus='Y' and c.ActiveStatus='Y' and c.RowStatus='Y'" +
                         " and a.RowStatus='Y' and a.ActiveStatus='Y' and a.CtypeId='" +
                         Convert.ToInt32(CmbCmplntType.SelectedValue) + "' order by CId Desc" + Obj.IsoEnd;

            DataTable dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];

            string userEmail = "";
            if (dt.Rows.Count > 0)
            {
                userEmail = dt.Rows[0]["ToUserEmail"].ToString();
                LblCompalin.Text = dt.Rows[0]["CId"].ToString();
                Lblgroup.Text = dt.Rows[0]["GroupName"].ToString();
            }

            if (userEmail != "")
            {
                string StrMsg = "<table><tr><td>" +
                    "<b>Dear Sir/Madam,</b><br><br>" +
                    "<strong>Name:</strong> " + TxtName.Text + "<br><br>" +
                    "I am sending a complaint.<br><br>" +
                    "<strong>Complaint Type:</strong> " + CmbCmplntType.SelectedItem.Text + "<br>" +
                    "<strong>Subject:</strong> " + TxtSubject.Text + "<br>" +
                    "<strong>Description:</strong> " + TxtDesc.Text + "<br><br>" +
                    "Admin Panel: " + Session["AdminWeb"] + "<br><br>" +
                    "</td></tr></table>";

                var message = new System.Net.Mail.MailMessage(
                    new System.Net.Mail.MailAddress(Session["CompMail"].ToString()),
                    new System.Net.Mail.MailAddress(userEmail));

                message.Subject = "Member Complaint";
                message.Body = StrMsg;
                message.IsBodyHtml = true;

                var smtp = new System.Net.Mail.SmtpClient(Session["MailHost"].ToString());
                smtp.Credentials = new System.Net.NetworkCredential(Session["CompMail"].ToString(), Session["MailPass"].ToString());
                smtp.Send(message);

                return true;
            }
        }
        catch (Exception ex)
        {
            Obj.WriteToFile(ex.Message);
            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }

        return false;
    }
    protected void BtnSubMit_Click(object sender, EventArgs e)
    {
        try
        {
            string scrname = "";

            string StrSql1 = "Insert into TrncomplainUniqe (Transid,Rectimestamp) values(" + HdnCheckTrnns.Value + ",getdate())";
            int updateeffect = Obj.SaveData(StrSql1);

            if (updateeffect > 0)
            {
                if ((Session["Sessncheck"].ToString().ToUpper()) != ("OK" + Crypto.Decrypt(hdnSessn.Value).ToString().ToUpper()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Key",
                        "alert('Session expire, Please Re-Login.!!');location.replace('logout.aspx');", true);
                    return;
                }

                // VALIDATIONS
                if (TxtName.Text.Trim() == "")
                {
                    scrname = "<SCRIPT>alert('Please Enter Name.');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Error", scrname, false);
                    TxtDesc.Focus();
                    return;
                }
                else if (TxtMobl.Text.Trim() == "")
                {
                    scrname = "<SCRIPT>alert('Please Enter MobileNo.');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Error", scrname, false);
                    TxtDesc.Focus();
                    return;
                }
                else if (TxtEmail.Text.Trim() == "")
                {
                    scrname = "<SCRIPT>alert('Please Enter Email.');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Error", scrname, false);
                    TxtDesc.Focus();
                    return;
                }
                else if (Convert.ToInt32(CmbCmplntType.SelectedValue) == 0)
                {
                    scrname = "<SCRIPT>alert('Please Select Nature of Grievance.');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Error", scrname, false);
                    CmbCmplntType.Focus();
                    return;
                }
                else if (TxtSubject.Text.Trim() == "")
                {
                    scrname = "<SCRIPT>alert('Please Enter Subject.');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Error", scrname, false);
                    TxtSubject.Focus();
                    return;
                }
                else if (TxtDesc.Text.Trim() == "")
                {
                    scrname = "<SCRIPT>alert('Please Enter Description.');</SCRIPT>";
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Error", scrname, false);
                    TxtDesc.Focus();
                    return;
                }

                string Sql = "";
                string ComplainProof = "";

                // FILE UPLOAD FOR COMPANY 1055
                if (Session["CompID"].ToString() == "1055")
                {
                    if (Fuidentity.Enabled == true)
                    {
                        if (!Fuidentity.HasFile)
                        {
                            scrname = "<SCRIPT>alert('Please upload jpg/jpeg/png image upto 5 MB only.');</SCRIPT>";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                            return;
                        }
                    }

                    if (Fuidentity.HasFile)
                    {
                        string ext = Path.GetExtension(Fuidentity.FileName).ToUpper();

                        if (ext == ".JPG" || ext == ".JPEG" || ext == ".PNG")
                        {
                            decimal size = Math.Round((decimal)Fuidentity.PostedFile.ContentLength / 1024, 1);

                            if (size > 1024)
                            {
                                scrname = "<SCRIPT>alert('Please upload image upto 5 MB size only!');</SCRIPT>";
                                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                                return;
                            }

                            string FileName = "FA" + DateTime.Now.ToString("yyMMddhhmmssfff") +
                                              Session["CompID"] + ext;

                            Fuidentity.PostedFile.SaveAs(Server.MapPath("images/UploadImage/") + FileName);

                            ComplainProof = "https://" + HttpContext.Current.Request.Url.Host +
                                            "/images/UploadImage/" + FileName;
                        }
                        else
                        {
                            scrname = "<SCRIPT>alert('Only JPG, JPEG, PNG files allowed!');</SCRIPT>";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
                            return;
                        }
                    }
                    else
                    {
                        ComplainProof = lblimage.Text;
                    }

                    // INSERT QUERY WITH COMPLAIN PROOF
                    Sql = "INSERT INTO M_ComplaintMaster(IDNO,CTypeID,CType,Complaint,Subject,MemberName," +
                          "Mobileno,Email,complainProof,formno) VALUES " +
                          "('" + TxtDirectSeller.Text.Trim() + "'," +
                          Convert.ToInt32(CmbCmplntType.SelectedValue) + ",'" +
                          CmbCmplntType.SelectedItem.Text + "',N'" +
                          TxtDesc.Text.Trim() + "',N'" +
                          TxtSubject.Text.Trim() + "','" +
                          TxtName.Text.Trim() + "','" +
                          TxtMobl.Text.Trim() + "','" +
                          TxtEmail.Text.Trim() + "','" +
                          ComplainProof + "','" +
                          Session["Formno"] + "')";
                }
                else
                {
                    // SIMPLE INSERT (NO FILE)
                    Sql = "INSERT INTO M_ComplaintMaster(IDNO,CTypeID,CType,Complaint,Subject,MemberName," +
                          "Mobileno,Email) VALUES " +
                          "('" + TxtDirectSeller.Text.Trim() + "'," +
                          Convert.ToInt32(CmbCmplntType.SelectedValue) + ",'" +
                          CmbCmplntType.SelectedItem.Text + "',N'" +
                          TxtDesc.Text.Trim() + "',N'" +
                          TxtSubject.Text.Trim() + "','" +
                          TxtName.Text.Trim() + "','" +
                          TxtMobl.Text.Trim() + "','" +
                          TxtEmail.Text.Trim() + "')";
                }

                int UpdtEffect = Obj.SaveData(Sql);

                if (UpdtEffect == 0)
                {
                    scrname = "<SCRIPT>alert('Complaint not sent.');</SCRIPT>";
                }
                else
                {
                    // SMS (if required)
                    //if (TxtMobl.Text.Length >= 10)
                    //{
                    //    if (Session["CompID"].ToString() != "1007")
                    //        sendSMS();
                    //}

                    // FETCH LAST COMPLAINT ID
                    string sql1 = Obj.IsoStart + "select Top 1 Cid from " + Obj.dBName + "..M_ComplaintMaster where Idno='" +
                                   TxtDirectSeller.Text.Trim() + "' and CTypeId=" +
                                   Convert.ToInt32(CmbCmplntType.SelectedValue) +
                                   " Order By Cid Desc " + Obj.IsoEnd;

                    DataTable dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql1).Tables[0];
                    if (dt.Rows.Count > 0)
                        LblCompalin.Text = dt.Rows[0]["CId"].ToString();

                    // SUCCESS MESSAGE
                    spanError.InnerText =
                        "Your complaint has been successfully submitted on " +
                        DateTime.Now.ToString("dd MMMM yyyy,hh:mm dddd") +
                        ". Your Complaint No. is " + LblCompalin.Text +
                        ". Our customer service representative will contact you shortly.";

                    DivError.Visible = true;
                    spanError.Visible = true;
                    divSuccess.Visible = false;

                    // CLEAR FIELDS
                    TxtDesc.Text = "";
                    TxtDirectSeller.Text = "";
                    TxtName.Text = "";
                    TxtSubject.Text = "";
                    TxtMobl.Text = "";
                    TxtEmail.Text = "";
                }
            }
            else
            {
                Response.Redirect("Complain.aspx");
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }
    }
    private void sendSMS()
    {
        WebClient client = new WebClient();

        Stream data;
        DateTime datet = DateTime.Now;

        string Sms = "Hi " + TxtName.Text + ", Your complaint has been received and complaint no. is "
                     + LblCompalin.Text + ". Regards " + Session["CompName"];
        string baseurl = " http://www.apiconnecto.com/API/SMSHttp.aspx?UserId=" + Session["SmsId"] + "&pwd=" + Session["SmsPass"] + "&Message=" + Sms + "&Contacts=" + TxtMobl.Text + "&SenderId=" + Session["ClientId"] + "";
        try
        {
            data = client.OpenRead(baseurl);
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            data.Close();
            reader.Close();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }
    }
    public bool SendToMemberMail()
    {
        try
        {
            if (TxtEmail.Text != "")
            {
                string StrMsg = "";

                MailAddress SendFrom = new MailAddress(Session["CompMail"].ToString());
                MailAddress SendTo = new MailAddress(TxtEmail.Text.Trim());

                MailMessage MyMessage = new MailMessage(SendFrom, SendTo);

                StrMsg = "<table style='margin:0; padding:10px; font-size:14px; font-family:verdana; line-height:23px; width:100%'>" +
                         "<tr><td>" +
                         "<span style='font-weight:bold;'> Dear " + TxtName.Text.Trim() + ",</span><br/>" +
                         "We have successfully registered your Query. Your Query details are:<br/><br/>" +
                         "<strong>Direct Seller ID:</strong> " + TxtDirectSeller.Text + "<br/>" +
                         "<strong>Name:</strong> " + TxtName.Text + "<br/>" +
                         "<strong>Complaint No:</strong> " + LblCompalin.Text + "<br/>" +
                         "<strong>Complaint Type:</strong> " + CmbCmplntType.SelectedItem.Text + "<br/>" +
                         "<strong>Subject:</strong> " + TxtSubject.Text + "<br/>" +
                         "<strong>Description:</strong> " + TxtDesc.Text + "<br/>" +
                         "<strong>Status:</strong> Open<br/>" +
                         "<strong>Mobile No:</strong> " + TxtMobl.Text + "<br/>" +
                         "<strong>Email Id:</strong> " + TxtEmail.Text + "<br/><br/>" +
                         "Our customer service representative will get in touch with you soon.<br/><br/>" +
                         "Warm regards,<br/>" +
                         "<a href='" + Session["CompWeb"] + "' target='_blank'>" + Session["CompName"] + "</a><br/><br/>" +
                         "</td></tr>" +
                         "<tr><td>Note: This is an automated mail. DO NOT REPLY.<br/>" +
                         "For queries, visit: " + Session["CompWeb"] + "<br/><br/>" +
                         "<b>DISCLAIMER</b><br/>" +
                         "This email contains confidential information. If not intended recipient, delete immediately.</td></tr>" +
                         "</table>";

                MyMessage.Subject = "Complaint Confirmation";
                MyMessage.Body = StrMsg;
                MyMessage.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient(Session["MailHost"].ToString());
                smtp.Credentials = new NetworkCredential(Session["CompMail"].ToString(), Session["MailPass"].ToString());

                smtp.Send(MyMessage);
                return true;
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ": " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }

        return false;
    }
    public string GenerateRandomString(int length)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        string result = "";

        for (int i = 0; i < length; i++)
        {
            result += allowChrs[rdm.Next(0, allowChrs.Length)];
        }
        return result;
    }
    private string DisableTheButton(Control page, Control btn)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
        sb.Append("if (Page_ClientValidate() == false) { return false; }}");
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
        sb.Append("this.value = 'Please wait...';");
        sb.Append("this.disabled = true;");
        sb.Append(page.Page.GetPostBackEventReference(btn));
        sb.Append(";");

        return sb.ToString();
    }

}