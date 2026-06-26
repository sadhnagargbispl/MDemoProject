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
using ClosedXML.Excel;
using System.Collections.Generic;
using System.Configuration;
using System.Collections;
//using DocumentFormat.OpenXml.Presentation;

public partial class IdactivationPostal : System.Web.UI.Page
{
    string scrName;
    DAL ObjDAL;
    DAL obj;
    clsGeneral objGen = new clsGeneral();
    DAL Obj;
    public IEnumerable<E_RegisterUser> RegisterUserDetails { get; set; }
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
                    Fill_State();
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
    private void Fill_State()
    {
        try
        {
            Obj = new DAL();
            DataTable dtMaster = new DataTable();

            string str = Obj.IsoStart + "Select StateCode, StateName from " + Obj.dBName + "..M_StateDivMaster Where ActiveStatus = 'Y' And RowStatus = 'Y' Order by StateName" + Obj.IsoEnd;
            dtMaster = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];
            if (dtMaster.Rows.Count > 0)
            {
                ddlPostSate.DataSource = dtMaster;
                ddlPostSate.DataValueField = "StateCode";
                ddlPostSate.DataTextField = "StateName";
                ddlPostSate.DataBind();
            }
        }
        catch (Exception ex)
        {
            // optional: log exception
            // Obj.WriteToFile(ex.Message);
        }
    }
    private string GetName()
    {
        try
        {
            string str = "";
            DataTable dt = new DataTable();

            str = Obj.IsoStart + "Select a.passw,a.Formno,a.Idno,a.MemFirstName + ' ' + a.MemLastName as MemName," +
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
                    if (compId == "1108")
                    {
                        lblFormno.Text = dt.Rows[0]["Formno"].ToString();
                        TxtMemberName.Text = dt.Rows[0]["MemName"].ToString();
                        LblMobile.Text = dt.Rows[0]["Mobl"].ToString();
                        lblemail.Text = dt.Rows[0]["email"].ToString();
                        Session["UserMemPassw"] = dt.Rows[0]["passw"].ToString();
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
                            Session["UserMemPassw"] = dt.Rows[0]["passw"].ToString();
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
            else if (compId == "1097" || compId == "1100" || compId == "1103" || compId == "1108" || compId == "1109")
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
    private string ClearInject(string StrObj)
    {
        try
        {
            StrObj = StrObj.Replace(";", "")
                           .Replace("'", "")
                           .Replace("=", "");

            return StrObj.Trim();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff")
                          + Environment.NewLine;

            Obj.WriteToFile(text + ex.Message);
            Response.Write("Try later.");
        }

        return string.Empty;
    }
    public DataSet InserttblTrnOrderWeb(string regXML, decimal Formno, decimal Amount, decimal ordertransId, string orderType, string Idno, string Narration, string BillNo)
    {
        DataSet dsReturn = null;
        try
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", "saveTrnOrderWeb");
            hst.Add("Xmll", regXML);
            hst.Add("Formno", Formno);
            hst.Add("OrderTransId", ordertransId);
            hst.Add("orderType", orderType);
            hst.Add("Idno", Idno);
            hst.Add("Amount", Amount);
            hst.Add("Narration", Narration);
            hst.Add("Wallettype", "R");
            hst.Add("BillNo", BillNo);
            dsReturn = blldb.GetDataSet("Sp_tblTrnOrderWeb", CommandType.StoredProcedure, hst);
            return dsReturn;
        }
        catch (Exception ex)
        {
            var Exception = ex.Message;
        }
        return dsReturn;
    }
    public IEnumerable<E_RegisterUser> SaveAddressDetail(string Action, string Id, string UserName, string Password, string Email, string FirstName, string Lastname, string Mobile, string FormNo, string StateCode, string District, string City, string Address, string PinCode, string AlternateMobileno, string BillingAddress, string BillingCity, string BillingPinCode, string BillingStateCodebState)
    {
        IEnumerable<E_RegisterUser> lst = null;
        try
        {
            BLLDBOperations blldb = new BLLDBOperations();
            Hashtable hst = new Hashtable();
            DataTable dt;
            hst.Add("Action", Action);
            hst.Add("UserName", UserName);
            hst.Add("Password", Password);
            hst.Add("id", Id);
            hst.Add("FirstName", FirstName);
            hst.Add("LastName", Lastname);
            hst.Add("MobileNo", Mobile);
            hst.Add("FormNo", FormNo);
            hst.Add("Address", Address);
            hst.Add("City", City);
            hst.Add("District", District);
            hst.Add("StateCode", StateCode);
            hst.Add("PinCode", PinCode);
            hst.Add("Email", Email);
            dt = blldb.GetDataTable("sp_Login", CommandType.StoredProcedure, hst);
            lst = DbOperation.ConvertDataTable<E_RegisterUser>(dt);

            return lst;
        }
        catch (Exception ex)
        {

        }
        return lst;
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

                        if (Session["CompID"].ToString() == "1108")
                        {
                            string str = Obj.IsoStart + "select * from " + Obj.dBName + "..M_MemberMaster where Formno = '" + lblFormno.Text + "'" + Obj.IsoEnd;
                            DataTable Dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];
                            string Remark = "";
                            if (Dt1.Rows.Count > 0)
                            {
                                if (ClearInject(Dt1.Rows[0]["DeliveryAddress"].ToString()) != ClearInject(TxtPostalAddress.Text))
                                {
                                    Remark = " PostalAddress Changed From " + ClearInject(Dt1.Rows[0]["DeliveryAddress"].ToString()) +
                                              " to " + ClearInject(TxtPostalAddress.Text) + ",";
                                }

                                if (ClearInject(Dt1.Rows[0]["PostalPin"].ToString()) != ClearInject(TxtPostPincode.Text))
                                {
                                    Remark += " Postal PinCode Changed From " + ClearInject(Dt1.Rows[0]["PostalPin"].ToString()) +
                                              " to " + ClearInject(TxtPostPincode.Text) + ",";
                                }

                                if (ClearInject(Dt1.Rows[0]["PostalDistrict"].ToString()) != ClearInject(TxtPostDistrict.Text))
                                {
                                    Remark += " Postal District Changed From " + ClearInject(Dt1.Rows[0]["PostalDistrict"].ToString()) +
                                              " to " + ClearInject(TxtPostDistrict.Text) + ",";
                                }

                                if (ClearInject(Dt1.Rows[0]["PostalCity"].ToString()) != ClearInject(TxtPostCity.Text))
                                {
                                    Remark += " Postal City Changed From " + ClearInject(Dt1.Rows[0]["PostalCity"].ToString()) +
                                              " to " + ClearInject(TxtPostCity.Text) + ",";
                                }

                                if (Dt1.Rows[0]["PostalStateCode"].ToString() != ddlPostSate.SelectedItem.Text.ToString())
                                {
                                    Remark += " Postal State Changed From " + Dt1.Rows[0]["PostalStateCode"] +
                                              " to " + ddlPostSate.SelectedItem.Text + ",";
                                }
                            }

                            // ---------- Company Check Logic ----------
                            string strQry = "Update M_MemberMaster Set DeliveryAddress='" + TxtPostalAddress.Text.ToUpper() + "',Postalpin='" + TxtPostPincode.Text.ToUpper() + "',PostalDistrict='" + TxtPostDistrict.Text.ToUpper() +
                                           "',PostalCity='" + TxtPostCity.Text.ToUpper() + "',PostalStateCode='" + ddlPostSate.SelectedValue + "',PostalState = '" + ddlPostSate.SelectedItem.Text + "' Where FormNo=" + lblFormno.Text;

                            // Insert History
                            string Qry = " insert into UserHistory(UserId,UserName,PageName,Activity,ModifiedFlds,RecTimeStamp,MemberId)Values " +
                                   "(0,'" + Session["MemName"] + "','IdactivationPostal','Postal Profile Update','" +
                                   Remark + "',Getdate(),'" + lblFormno.Text + "') ; ";
                            Qry += "Insert into purchaseReq(FormNo,FromFormNo,Kitid,UserAddress,Pincode,StateName,City,District,ReqDate,Statecodes)";
                            Qry += "Values('" + lblFormno.Text + "','" + Session["formno"] + "','" + Convert.ToInt32(CmbKit.SelectedValue) + "'," +
                                "'" + TxtPostalAddress.Text.ToUpper() + "','" + TxtPostPincode.Text.ToUpper() + "','" + ddlPostSate.SelectedItem.Text + "'," +
                                "'" + TxtPostCity.Text.ToUpper() + "','" + TxtPostDistrict.Text.ToUpper() + "',GETDATE(),'" + ddlPostSate.SelectedValue + "');";
                            Qry += strQry;

                            int j = Obj.SaveData(Qry);

                            string Bill_No = GenerateRandomStringactive(6);
                            string compId = Session["CompID"].ToString();
                            string query1 = Obj.IsoStart + "Select ProductID From " + Obj.dBName + "..M_KitMaster where kitid = '" + CmbKit.SelectedValue + "' " + Obj.IsoEnd;
                            DataTable Dt_Kit = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, query1).Tables[0];
                            if (Dt_Kit.Rows.Count > 0)
                            {
                                string str1 = "Exec GetProducts 'WR','" + Dt_Kit.Rows[0]["ProductID"] + "'";
                                DataTable Dt11 = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqlConn"].ToString(), CommandType.Text, str1).Tables[0];

                                if (Dt11.Rows.Count > 0)
                                {
                                    //int updateeffect = 0;

                                    try
                                    {
                                        string str11 = "Exec sp_AddtoCart 'AddTocartDeatils','" +
                                                       Dt_Kit.Rows[0]["ProductID"] + "','" +
                                                       Dt11.Rows[0]["ProductName"] + "','','" +
                                                       Dt11.Rows[0]["Price"] + "','',1," +
                                                       lblFormno.Text + ",1," +
                                                       Dt11.Rows[0]["bv"] + "," +
                                                       Dt11.Rows[0]["pv"] + ",0,'" +
                                                       Dt11.Rows[0]["BatchNo"] + "'";

                                        updateeffect = SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["sqlConn"].ToString(), CommandType.Text, str11);

                                        if (updateeffect > 0)
                                        {
                                            //var UserName = Convert.ToString(Session["IDNO"]);
                                            //var Password = Convert.ToString(Session["MemPassw"]);
                                            var UserName = Convert.ToString(txtMemberId.Text);
                                            var Password = Convert.ToString(Session["UserMemPassw"]);
                                            var Id = "1";
                                            var FormNo = Convert.ToString(lblFormno.Text);

                                            Session["PartyCode"] = "WR";

                                            RegisterUserDetails = SaveAddressDetail(
                                                "SaveUserShippingDetail",
                                                Id,
                                                UserName,
                                                Password,
                                                 lblemail.Text.ToString(),
                                                TxtMemberName.Text,
                                                "",
                                                "",
                                                FormNo,
                                                ddlPostSate.SelectedValue,
                                                TxtPostDistrict.Text,
                                                TxtPostCity.Text,
                                                TxtPostalAddress.Text,
                                                TxtPostPincode.Text,
                                                "",
                                                TxtPostalAddress.Text,
                                                TxtPostCity.Text,
                                                TxtPostPincode.Text,
                                                ddlPostSate.SelectedValue
                                            );

                                            decimal qty = 1;
                                            decimal price = Convert.ToDecimal(Dt11.Rows[0]["Price"]);
                                            decimal bv = Convert.ToDecimal(Dt11.Rows[0]["BV"]);
                                            decimal pv = Convert.ToDecimal(Dt11.Rows[0]["PV"]);
                                            var Sessionid = 1;
                                            var userid = 1;
                                            var uniqueId = Bill_No;
                                            var idNo = txtMemberId.Text;
                                            //var FormNo = Session["FormNo"];
                                            var ShopType = "";/*Session["ShopTye"];*/
                                            var PartyCode = "WR";
                                            var OrderType = "";
                                            decimal CourierCharge = 0;
                                            string hostName = Dns.GetHostName();
                                            string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
                                            //DataTable dtidstatus = iprod.GetIDStatus(Convert.ToString(Session["IDNO"]));
                                            if (Convert.ToString(Session["St"]) == "Y")
                                            {
                                                OrderType = "O";
                                            }
                                            else
                                            {
                                                OrderType = "T";
                                            }
                                            StringBuilder sb = new StringBuilder();

                                            if (Convert.ToDecimal(Session["remainbv"]) == 0)
                                            {
                                                Session["remainbv"] = (qty * bv);
                                            }

                                            sb.AppendLine("<orders>");
                                            sb.AppendLine("<OrderData>");

                                            sb.AppendLine("<ProdId>" + Dt_Kit.Rows[0]["ProductID"] + "</ProdId>");
                                            sb.AppendLine("<ProdName>" + Dt11.Rows[0]["ProductName"].ToString().Replace("&", "") + "</ProdName>");
                                            sb.AppendLine("<Qty>" + qty + "</Qty>");
                                            sb.AppendLine("<Price>" + (qty * price) + "</Price>");
                                            sb.AppendLine("<BV>0</BV>");
                                            sb.AppendLine("<PV>0</PV>");

                                            sb.AppendLine("<myIP>" + myIP + "</myIP>");
                                            sb.AppendLine("<Sessionid>" + Convert.ToString(Sessionid) + "</Sessionid>");
                                            sb.AppendLine("<userid>" + Convert.ToString(userid) + "</userid>");
                                            sb.AppendLine("<idNo>" + Convert.ToString(idNo) + "</idNo>");
                                            sb.AppendLine("<FormNo>" + Convert.ToString(FormNo) + "</FormNo>");
                                            sb.AppendLine("<ShopingBillType>" + Convert.ToString(ShopType) + "</ShopingBillType>");
                                            sb.AppendLine("<OrderType>" + Convert.ToString(OrderType) + "</OrderType>");
                                            sb.AppendLine("<PartyCode>" + Convert.ToString(PartyCode) + "</PartyCode>");
                                            sb.AppendLine("<Mode>Wallet</Mode>");
                                            sb.AppendLine("<CourierCharge>" + Convert.ToString(CourierCharge) + "</CourierCharge>");

                                            sb.AppendLine("<Color>0</Color>");
                                            sb.AppendLine("<Size>0</Size>");

                                            sb.AppendLine("<TRNCharge>0</TRNCharge>");
                                            sb.AppendLine("<coupon>0</coupon>");
                                            sb.AppendLine("<couponamount>0</couponamount>");
                                            sb.AppendLine("<paidbv>0</paidbv>");
                                            sb.AppendLine("<Shoppingwallet>0</Shoppingwallet>");
                                            sb.AppendLine("<Repurchasewallet>0</Repurchasewallet>");
                                            sb.AppendLine("<Batchcode>" + Convert.ToString(Dt11.Rows[0]["BatchNo"]) + "</Batchcode>");

                                            sb.AppendLine("</OrderData>");
                                            sb.AppendLine("</orders>");

                                            var ordertransId = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                                            DataSet ds = InserttblTrnOrderWeb(sb.ToString(), Convert.ToDecimal(FormNo), Convert.ToDecimal(txtAmount.Text), Convert.ToDecimal(ordertransId), "WR", idNo.ToString(), "", Bill_No);

                                            if (ds != null && ds.Tables[0].Rows.Count > 0)
                                            {
                                                sql = "exec Sp_ActivateMember '" + txtMemberId.Text.Trim() + "','" + Convert.ToInt32(CmbKit.SelectedValue) + "','" + Convert.ToInt32(Session["Formno"]) + "','" + Bill_No + "'";
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
                                                        "location.replace('RepurchaseNow.aspx');</SCRIPT>";

                                                    ScriptManager.RegisterClientScriptBlock(
                                                        this.Page, this.GetType(), "Upgraded", scrName, false);

                                                    Clear();
                                                    fillkit();
                                                }
                                            }

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }
                                }
                            }
                            //sql = "exec Sp_ActivateMember '" + txtMemberId.Text.Trim() + "','" +  Convert.ToInt32(CmbKit.SelectedValue) + "','" +
                            //      Convert.ToInt32(Session["Formno"]) + "','" + Bill_No + "'";
                        }
                        //query =
                        //    " Begin Try Begin Transaction " + sql +
                        //    " Commit Transaction End Try " +
                        //    " BEGIN CATCH ROLLBACK Transaction END CATCH";

                        //int i = Convert.ToInt32(
                        //    SqlHelper.ExecuteNonQuery(
                        //        HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(),
                        //        CommandType.Text,
                        //        query
                        //    )
                        //);

                        //if (i > 0)
                        //{
                        //    string sms =
                        //        "Dear " + TxtMemberName.Text.Trim() +
                        //        ", Your id " + txtMemberId.Text.Trim().ToUpper() +
                        //        " is successfully topup by " +
                        //        CmbKit.SelectedItem.Text +
                        //        ". Best of luck, Regards: " +
                        //        Session["CompName"];

                        //    sendSMS(sms);

                        //    GetBalance();

                        //    string scrName =
                        //        "<SCRIPT>alert('ID : " + txtMemberId.Text.Trim().ToUpper() +
                        //        ". Name : " + TxtMemberName.Text +
                        //        ". Package Name : " + CmbKit.SelectedItem.Text.Trim() +
                        //        ". Activated successfully!!');" +
                        //        "location.replace('IdactivationPostal.aspx');</SCRIPT>";

                        //    ScriptManager.RegisterClientScriptBlock(
                        //        this.Page, this.GetType(), "Upgraded", scrName, false);

                        //    Clear();
                        //    fillkit();
                        //}
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
                // Response.Redirect("IdactivationPostal.aspx");
                string scrName = "<SCRIPT>alert('Try Again.!');" + "location.replace('IdactivationPostal.aspx');</SCRIPT>";
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
            if (postalshowdiv.Visible == true)
            {
                string postalAddress = TxtPostalAddress.Text.Trim();
                string postalPincode = TxtPostPincode.Text.Trim();
                string postalState = ddlPostSate.SelectedValue;
                string postalDistrict = TxtPostDistrict.Text.Trim();
                string postalCity = TxtPostCity.Text.Trim();

                // Example: check empty
                if (postalAddress == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                        "msg", "alert('Please enter Address');", true);
                    return;
                }
                if (postalPincode == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                        "msg", "alert('Please enter Pincode');", true);
                    return;
                }
                if (postalState == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                        "msg", "alert('Please Select State');", true);
                    return;
                }
                if (postalDistrict == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                        "msg", "alert('Please enter District');", true);
                    return;
                }
                if (postalCity == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(),
                        "msg", "alert('Please enter city');", true);
                    return;
                }

            }
            if (CmbKit.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                    "msg", "alert('Please Choose Package');", true);
                return;
            }
            if (CmbKit.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                    "msg", "alert('Please Choose Package');", true);
                return;
            }
            if (txtMemberId.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                   "msg", "alert('Please enter Member ID');", true);
                return;
            }
            if (Convert.ToDecimal(txtAmount.Text) == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                   "msg", "alert('Valid Amount');", true);
                return;
            }
            if (TxtPassword.Text.Trim() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                   "msg", "alert('Please enter Transaction Password');", true);
                return;
            }
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

            if (compId == "1097" || compId == "1100" || compId == "1103" || compId == "1108")
            {
                if (Check_IdNo())
                {
                    if (compId == "1108")
                    {
                        postalshowdiv.Visible = true;
                        try
                        {
                            string idverified = "";
                            Obj = new DAL();

                            string sql = Obj.IsoStart + "exec sp_MemDtl ' and mMst.Formno=''" + lblFormno.Text + "''' " + Obj.IsoEnd;
                            DataTable dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                TxtPostPincode.Text = dt.Rows[0]["PostalPin"].ToString();
                                ddlPostSate.SelectedValue = dt.Rows[0]["PostalStateCode"].ToString();

                                TxtPostDistrict.Text = dt.Rows[0]["PostalDistrict"] == DBNull.Value ? "" : dt.Rows[0]["PostalDistrict"].ToString();
                                TxtPostDistrict.Enabled = true;

                                TxtPostCity.Text = dt.Rows[0]["PostalCity"] == DBNull.Value ? "" : dt.Rows[0]["PostalCity"].ToString();
                                TxtPostCity.Enabled = true;

                                TxtPostalAddress.Text = dt.Rows[0]["DeliveryAddress"].ToString();
                                TxtPostalAddress.Enabled = true;

                                if (dt.Rows[0]["PostalStatecode"] == DBNull.Value || ddlPostSate.SelectedValue == "0")
                                {
                                    ddlPostSate.Enabled = true;
                                }
                                else
                                {
                                    ddlPostSate.Text = dt.Rows[0]["PostalStatecode"].ToString();
                                    ddlPostSate.Enabled = true;
                                }

                                TxtPostPincode.Enabled = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            string path = HttpContext.Current.Request.Url.AbsoluteUri;
                            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff") +
                                          Environment.NewLine;

                            Obj.WriteToFile(text + ex.Message);
                            Response.Write("Try later.");
                        }
                    }
                    else
                    {
                        postalshowdiv.Visible = false;
                    }
                    fillkit(LblCondition.Text.Trim());

                    if (CmbKit.SelectedValue == "")
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

public class E_RegisterUser
{
    public string Firstname { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string MobileNo { get; set; }
    public string AlternateMobileno { get; set; }
    public string Fax { get; set; }
    public string FormNo { get; set; }
    public string ActiveStatus { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public int CityCode { get; set; }
    public string District { get; set; }
    public int DistrictCode { get; set; }
    public int CountryId { get; set; }
    public decimal StateCode { get; set; }
    public string PinCode { get; set; }
    public string CountryName { get; set; }
    public string randomId { get; set; }
    public int Id { get; set; }
    public string PartyCode { get; set; }
    public string DeliveryAddress { get; set; }
    public decimal BillingStateCode { get; set; }
}
