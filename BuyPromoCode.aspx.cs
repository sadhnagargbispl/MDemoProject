using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class BuyPromoCode : System.Web.UI.Page
{
    string scrname;
    DAL Obj;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                this.CmdSave.Attributes.Add("onclick", DisableTheButton(this.Page, this.CmdSave));
                if (!Page.IsPostBack)
                {
                    Session["OtpCount"] = 0;
                    Session["OtpTime"] = null;
                    Session["Retry"] = null;
                    Session["OTP_"] = null;
                    HdnCheckTrnns.Value = GenerateRandomStringJoining(6);
                    FillBalance();
                    FillDrop();
                }

            }

            else
            {
                Response.Redirect("Default.aspx", false);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private void FillBalance()
    {
        try
        {
            Obj = new DAL();
            DataTable Dt = new DataTable();
            string  query = Obj.IsoStart + "Select * From dbo.ufnGetBalance('" + Session["FormNo"] + "','P')" + Obj.IsoEnd;
            Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, query).Tables[0];
            if (Dt.Rows.Count > 0)
            {
                TxtCredit.Text = Dt.Rows[0]["Balance"].ToString();
                Session["ServiceWallet"] = Dt.Rows[0]["Balance"].ToString();
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void FillDrop()
    {
        try
        {
            Obj = new DAL();
            DataTable dtMaster = new DataTable();
            string str = Obj.IsoStart + "select * from " + Obj.dBName + "..PurchaseMaster where Activestatus = 'Y'" + Obj.IsoEnd;
            dtMaster = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];
            if (dtMaster.Rows.Count > 0)
            {
                DDlCode.DataSource = dtMaster;
                DDlCode.DataValueField = "Type";
                DDlCode.DataTextField = "Name";
                DDlCode.DataBind();
                TxtAmount.Text = dtMaster.Rows[0]["Amount"].ToString();
            }
        }
        catch (Exception ex)
        {
            // optional: log exception
            // Obj.WriteToFile(ex.Message);
        }
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
    private string ClearInject(string StrObj)
    {
        try
        {
            StrObj = StrObj.Replace(";", "").Replace("'", "").Replace("=", "").Trim();
            return StrObj;
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = $"{path}:  {DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff")} {Environment.NewLine}";
            Obj.WriteToFile($"{text}{ex.Message}");
            Response.Write("Try later.");
            return ""; // Or handle the exception as per your application's requirements
        }
    }
    protected void Txtqty_TextChanged(object sender, EventArgs e)
    {
        decimal qty = decimal.Parse(Txtqty.Text);
        decimal amount = decimal.Parse(TxtAmount.Text);

        decimal totalAmount = qty * amount;

        TxtFinalAmount.Text = totalAmount.ToString();
        CmdSave.Enabled = true;
        errMsg.Visible = false;
        if (Convert.ToDouble(Session["ServiceWallet"]) < Convert.ToDouble(TxtFinalAmount.Text))
        {
            Label2.Text = "Insufficient Balance";
            Label2.ForeColor = System.Drawing.Color.Red;
            Label2.Visible = true;
            CmdSave.Enabled = false;
            return;
        }
        else
        {
            CmdSave.Enabled = true;
            Label2.Visible = false;
            return;
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
    protected void CmdSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Txtqty.Text))
        {
            errMsg.Text = "Please Enter Qty.!";
            CmdSave.Enabled = false;
            errMsg.Visible = true;
            return;
        }
        else
        {
            CmdSave.Enabled = true;
            errMsg.Visible = false;
        }
        int qty = int.Parse(Txtqty.Text);
        if (qty < 6)
        {
            errMsg.Text = "Please purchase at least 6 coupons.";
            CmdSave.Enabled = false;
            errMsg.Visible = true;
            return;
        }
        else
        {
            CmdSave.Enabled = true;
            errMsg.Visible = false;
        }
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        sResult = current_datetime + random_number.ToString().PadLeft(3, '0');
        if (Convert.ToDecimal(Session["ServiceWallet"]) >= Convert.ToDecimal(TxtFinalAmount.Text))
        {
            try
            {
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
                    string query = "INSERT INTO CouponPurchase (FormNo, Code, Amount, Qty, FinalAmount, OrderNo) " +
           "VALUES ('" + Session["formno"] + "', '" + DDlCode.SelectedValue + "', '" + TxtAmount.Text + "','" + Txtqty.Text + "', '" + TxtFinalAmount.Text + "', '" + HdnCheckTrnns.Value + "');" +
           "Exec InsertMemberCoupons '" + Session["formno"] + "','" + TxtAmount.Text + "','" + Txtqty.Text + "','" + HdnCheckTrnns.Value + "','" + DDlCode.SelectedValue + "','" + TxtFinalAmount.Text + "'";
                    int i = Convert.ToInt32(SqlHelper.ExecuteNonQuery(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(), CommandType.Text, query));
                    string message1 = "";
                    if (i > 0)
                        message1 = "Coupon purchase successfully.!";
                    else
                        message1 = "Try Again Later.!";

                    string url = "BuyPromoCode.aspx";
                    string script = "window.onload = function(){ alert('";
                    script += message1;
                    script += "');";
                    script += "window.location = '";
                    script += url;
                    script += "'; }";
                    ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                    CmdSave.Visible = true;
                }
                else
                {
                    Response.Redirect("BuyPromoCode.Aspx");
                }
            }
            catch (Exception ex)
            {
                Label1.Text = "Error while saving. Please try again.";
            }
        }
        else
        {
            var scrName = "<SCRIPT language='javascript'>alert('Insufficient Balance In Fund Wallet.!');</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Error", scrName, false);
        }
        
    }
    public DataSet convertJsonStringToDataSet(string jsonString)
    {
        XmlDocument xd = new XmlDocument();
        jsonString = "{ \"rootNode\": {" + jsonString.Trim().TrimStart('{').TrimEnd('}') + "} }";
        xd = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonString);
        DataSet ds = new DataSet();
        ds.ReadXml(new XmlNodeReader(xd));
        return ds;
    }
}