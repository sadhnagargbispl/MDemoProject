using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Xml;
using System.Configuration;
using System.Web.Services.Description;


public partial class CheckLogin : System.Web.UI.Page
{
    SqlConnection Conn;
    SqlConnection Connselect;
    SqlCommand Comm;
    DataTable Dt;
    SqlDataAdapter Ad;
    string str = "";
    string _RefFormNo = "";
    string _UpLnFormNo = "";
    string _Company = "";
    clsGeneral objGen = new clsGeneral();
    DAL objAccess;
    DAL Obj;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // GetCompID();
            HttpContext.Current.Session["CompID"] = "1108";
            objGen.GetConnectionByComp();
            objGen.GetInvDataBaseByComp();
            //ColumnName();
           // Pages();
            Obj = new DAL();
            Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
            Conn.Open();
            Connselect = new SqlConnection(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString());
            Connselect.Open();
            getData();

            string sRequestData = HttpContext.Current.Request.Url.ToString();
            _Company = Session["CompName"].ToString();

            if (Request["token"] == "abUnMar5489pidlAewUF4875brlE8a4i5n6" + Session["CompID"])
            {
                try
                {
                    Comm = new SqlCommand(
                        "insert into ReqType(reqtype) values('" +
                        sRequestData.Replace("//n", "\\n") + "')", Conn);
                    Comm.ExecuteNonQuery();
                    Conn.Close();
                }
                catch { }

                string action = Request["action"] == null ? "" : Request["action"].ToUpper();

                if (string.IsNullOrEmpty(action))
                {
                    CheckInfo();
                }
                else if (action == "LOGIN")
                {
                    CheckInfoDetail();
                }
                else if (action == "FROMEMALLID")
                {
                    string Uname = Clean(Request["Username"]);
                    GetrankFormNo(Uname);
                }
                else if (action == "ADDBV")
                {
                    AddBV();
                }
                else if (action == "DEBITBV")
                {
                    DrBV();
                }
                else if (action == "GETBALANCE")
                {
                    string Uname = Clean(Request["Username"]);
                    string Pwd = Clean(Request["Password"]);
                    string WallettType = Clean(Request["Wallertype"]);

                    int FormNo = GetFormNo(Uname, DecodePwd(Pwd));

                    if (FormNo > 0)
                    {
                        double AvailBal = Convert.ToDouble(GetBalance(FormNo.ToString(), WallettType.ToUpper()));
                        Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"OK\",\"walletBalance\": \"" +
                                       AvailBal + "\",\"msg\": \"success\",\"wallettype\": \"" + WallettType + "\"}");
                    }
                    else
                    {
                        Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"walletBalance\": \"0\",\"msg\": \"Invalid Login details\",\"wallettype\": \"" + WallettType + "\"}");
                    }
                }
                else if (action == "GETCOUPONDATA")
                {
                    GetCouponDataList();
                }
                else if (action == "GETCOUPONTYPE")
                {
                    GetCouponDataTypeList();
                }
                else if (action == "USECOUPON")
                {
                    SaveUSECOUPON();
                }
                else if (action == "USECOUPONSTATUSCHECK")
                {
                    USECOUPONSttausCheck();
                }
                else if (action == "DEDUCTWALLETAMOUNT")
                {
                    string WallettType = Clean(Request["Wallertype"]);
                    DeductWalletAmount(WallettType);
                }
                else if (action == "REFUNDWALLETAMOUNT")
                {
                    string WallettType = Clean(Request["Wallertype"]);
                    RefundWalletAmount(WallettType);
                }
                else if (action == "ADDWALLETAMOUNT" &&
                        (Context.Request.UserHostAddress == "116.202.49.124" ||
                         Context.Request.UserHostAddress == "103.71.99.8"))
                {
                    string WallettType = Clean(Request["Wallertype"]);
                    AddWalletAmount(WallettType);
                }
                else if (action == "CONFIRMVOUCHER")
                {
                    string Uname = Clean(Request["Username"]);
                    string Pwd = Clean(Request["Password"]);
                    int FormNo = GetFormNo(Uname, DecodePwd(Pwd));

                    string VoucherRequest = Clean(Request["voucherno"]);
                    string VoucherResponse = Clean(Request["response"]);
                    string WallettType = Clean(Request["Wallertype"]);

                    if (FormNo > 0)
                    {
                        Confirmvoucher(VoucherRequest, VoucherResponse, WallettType);
                    }
                    else
                    {
                        Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"msg\":\"Invalid Login details\"}");
                    }
                }
                else if (action == "CHECKRWALLET")
                {
                    string Uname = Clean(Request["Username"]);
                    string Pwd = Clean(Request["Password"]);
                    int FormNo = GetFormNo(Uname, DecodePwd(Pwd));

                    if (FormNo > 0)
                    {
                        double AvailBal = Convert.ToDouble(GetSWalletBalance(FormNo.ToString()));
                        Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"OK\",\"rwallet\": \"" +
                                       AvailBal + "\",\"msg\": \"success\",\"wallettype\": \"" +
                                       Session["RWalletType"] + "\"}");
                    }
                    else
                    {
                        Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"rwallet\": \"0\",\"msg\": \"Invalid Login details\"}");
                    }
                }
                else if (action == "DEDUCTRWALLET")
                {
                    DeductWallet();
                }
                else if (action == "CHECKEWALLET")
                {
                    string Uname = Clean(Request["Username"]);
                    string Pwd = Clean(Request["Password"]);
                    int FormNo = GetFormNo(Uname, DecodePwd(Pwd));

                    if (FormNo > 0)
                    {
                        double AvailBal = Convert.ToDouble(GetMWalletBalance(FormNo.ToString()));
                        Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"OK\",\"ewallet\": \"" +
                                       AvailBal + "\",\"msg\": \"success\",\"wallettype\": \"" +
                                       Session["MWalletType"] + "\"}");
                    }
                    else
                    {
                        Response.Write("{\"loginid\": \"" + Uname + "\",\"response\":\"FAILED\",\"ewallet\": \"0\",\"msg\": \"Invalid Login details\"}");
                    }
                }
                else if (action == "CHECKEVOUCHER")
                {
                    string Uname = Clean(Request["Username"]);
                    string Pwd = Clean(Request["Password"]);
                    int FormNo = GetFormNo(Uname, DecodePwd(Pwd));

                    string VoucherRequest = Clean(Request["voucherno"]);
                    string VoucherResponse = Clean(Request["response"]);

                    if (FormNo > 0)
                    {
                        checkEvoucher(VoucherRequest, VoucherResponse);
                    }
                    else
                    {
                        Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"msg\":\"Invalid Login details\"}");
                    }
                }
                else if (action == "DEDUCTEWALLET")
                {
                    DeductMainWallet();
                }
                else if (action == "STATELIST")
                {
                    FillState();
                }
                else if (action == "BANKLIST")
                {
                    FillBank();
                }
                else if (action == "JOINING")
                {
                    SaveIntoDB(
                        Clean(Request["sponsorid"]),
                        "0",
                        Clean(Request["legno"]),
                        "0",
                        "0",
                        Clean(Request["name"]),
                        Clean(Request["fathername"]),
                        Clean(Request["dob"]),
                        Clean(Request["email"]),
                        Clean(Request["mobileno"]),
                        Clean(Request["address"]),
                        Clean(Request["statecode"].ToString()),
                        Clean(Request["district"]),
                        Clean(Request["city"]),
                        Clean(Request["pincode"].ToString()),
                        Clean(Request["nominee"]),
                        Clean(Request["relation"]),
                        Clean(Request["bankcode"].ToString()),
                        Clean(Request["panno"]),
                        Clean(Request["accountno"]),
                        Clean(Request["ifsccode"]),
                        Clean(Request["branchname"]),
                        Clean(Request["password"])
                    );
                }
                else if (action == "CHECKVOUCHER")
                {
                    string Uname = Clean(Request["Username"]);
                    string Pwd = Clean(Request["Password"]);
                    int FormNo = GetFormNo(Uname, DecodePwd(Pwd));

                    string VoucherRequest = Clean(Request["voucherno"]);
                    string VoucherResponse = Clean(Request["response"]);

                    if (FormNo > 0)
                    {
                        checkvoucher(VoucherRequest, VoucherResponse);
                    }
                    else
                    {
                        Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"msg\":\"Invalid Login details\"}");
                    }
                }
                else if (action == "ADDWALLET")
                {
                    AddWalletAmount();
                }
                else
                {
                    Response.Write("{\"response\":\"FAILED\"}");
                }
            }
            else
            {
                Response.Write("{\"response\":\"FAILED\"}");
            }
        }
        catch
        {
            Response.Write("{\"response\":\"FAILED\"}");
        }
    }
    private void USECOUPONSttausCheck()
    {
        try
        {

            string username = Request["Username"];
            string password = Request["Password"]
                                .Replace("%25", "%")
                                .Replace("%23", "#")
                                .Replace("%26", "&")
                                .Replace("%22", "'")
                                .Replace("%40", "@");
            string orderNo = Request["orderno"];
            string q1 = Obj.IsoStart + "SELECT FormNo FROM " + Obj.dBName + "..M_MemberMaster WHERE IDNo = @uid" + Obj.IsoEnd;
            DataTable dtMember = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, q1, new SqlParameter("@uid", username)).Tables[0];
            if (dtMember.Rows.Count == 0)
            {
                Response.Write("{\"status\":\"FAIL\",\"message\":\"Invalid Username\"}");
                return;
            }
            string FormNo = dtMember.Rows[0]["FormNo"].ToString();
            string qry = Obj.IsoStart + "Exec CheckCouponStatus '" + orderNo + "', '" + FormNo + "'" + Obj.IsoEnd;
            DataTable dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, qry).Tables[0];
            Response.Clear();
            Response.ContentType = "application/json";
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                CouponCode = dt.Rows[0]["CouponCode"].ToString(),
                UsedDate = dt.Rows[0]["UsedDate"].ToString(),
                Txid = dt.Rows[0]["Txid"].ToString(),
                UsedType = dt.Rows[0]["UsedType"].ToString()
            });
            Response.Write(json);
        }
        catch
        {
            Response.Write("{\"status\":\"ERROR\",\"message\":\"Something went wrong\"}");
        }
    }
    private void GetCouponDataTypeList()
    {
        try
        {
            string username = Request["Username"];
            string password = Request["Password"].Replace("%25", "%").Replace("%23", "#").Replace("%26", "&").Replace("%22", "'").Replace("%40", "@");
            // 2️⃣ Get Coupon List from Stored Procedure
            string q2 = Obj.IsoStart + "Exec GetCouponList" + Obj.IsoEnd;
            DataTable dtCoupons = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, q2).Tables[0];
            // 3️⃣ JSON Response
            Response.Clear();
            Response.ContentType = "application/json";

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                status = "SUCCESS",
                coupons = dtCoupons
            });

            Response.Write(json);
        }
        catch
        {
            Response.Write("{\"status\":\"ERROR\",\"message\":\"Something went wrong\"}");
        }
    }
    private void GetCouponDataList()
    {
        try
        {
            string username = Request["Username"];
            string password = Request["Password"].Replace("%25", "%").Replace("%23", "#").Replace("%26", "&").Replace("%22", "'").Replace("%40", "@");

            // 1️⃣ Get FormNo
            string q1 = Obj.IsoStart + "SELECT FormNo FROM " + Obj.dBName + "..M_MemberMaster WHERE IDNo = @uid" + Obj.IsoEnd;

            DataTable dtMember = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, q1, new SqlParameter("@uid", username)).Tables[0];

            if (dtMember.Rows.Count == 0)
            {
                Response.Write("{\"status\":\"FAIL\",\"message\":\"Invalid Username\"}");
                return;
            }

            string FormNo = dtMember.Rows[0]["FormNo"].ToString();

            // 2️⃣ Get Coupon List from Stored Procedure
            string q2 = Obj.IsoStart + "Exec GetCouponData @formno" + Obj.IsoEnd;
            DataTable dtCoupons = SqlHelper.ExecuteDataset(
                                    HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                                    CommandType.Text,
                                    q2,
                                    new SqlParameter("@formno", FormNo)
                                 ).Tables[0];

            // 3️⃣ JSON Response
            Response.Clear();
            Response.ContentType = "application/json";

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                status = "SUCCESS",
                coupons = dtCoupons
            });

            Response.Write(json);
        }
        catch
        {
            Response.Write("{\"status\":\"ERROR\",\"message\":\"Something went wrong\"}");
        }
    }
    private void SaveUSECOUPON()
    {
        try
        {

            string username = Request["Username"];
            string password = Request["Password"]
                                .Replace("%25", "%")
                                .Replace("%23", "#")
                                .Replace("%26", "&")
                                .Replace("%22", "'")
                                .Replace("%40", "@");
            string orderNo = Request["orderno"];
            string type = Request["useservi"];
            string couponcode = Request["couponcode"];
            // If needed
            // 1️⃣ Get FormNo
            string q1 = Obj.IsoStart + "SELECT FormNo FROM " + Obj.dBName + "..M_MemberMaster WHERE IDNo = @uid" + Obj.IsoEnd;
            DataTable dtMember = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, q1, new SqlParameter("@uid", username)).Tables[0];
            if (dtMember.Rows.Count == 0)
            {
                Response.Write("{\"status\":\"FAIL\",\"message\":\"Invalid Username\"}");
                return;
            }
            string FormNo = dtMember.Rows[0]["FormNo"].ToString();
            string qry = "Exec CheckCouponBeforeSave '" + orderNo + "', '" + FormNo + "', '" + type + "','" + couponcode + "'";
            DataTable dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(), CommandType.Text, qry).Tables[0];
            Response.Clear();
            Response.ContentType = "application/json";
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                Result = dt.Rows[0]["Result"].ToString(),
                status = dt.Rows[0]["Status"].ToString(),
                message = dt.Rows[0]["Message"].ToString(),
                orderno = dt.Rows[0]["orderno"].ToString(),
                CouponCode = dt.Rows[0]["CouponCode"].ToString(),
                UsedDate = dt.Rows[0]["UsedDate"].ToString(),
                Txid = dt.Rows[0]["Txid"].ToString()
            });
            Response.Write(json);
        }
        catch
        {
            Response.Write("{\"status\":\"ERROR\",\"message\":\"Something went wrong\"}");
        }
    }
    private string Clean(string val)
    {
        if (val == null) return "";
        return val.Trim().Replace("'", "").Replace("=", "").Replace(";", "");
    }
    private string DecodePwd(string pwd)
    {
        if (pwd == null) return "";
        return pwd.Replace("%25", "%").Replace("%23", "#").Replace("%26", "&")
                  .Replace("%22", "'").Replace("%40", "@");
    }
    private void WalletAmountAdd(string WalletType)
    {
        string VoucherNo = "";

        string Uname = Clean(Request["Username"]);
        string Pwd = Clean(Request["Password"]);
        string orderno = Clean(Request["orderno"]);
        string Amount = Clean(Request["amount"]);

        try
        {
            int LoginSuccess = 0;
            int FormNo = GetFormNo(Uname, Pwd);

            if (WalletType != "M")
            {
                Response.Write("{\"response\":\"FAILED\",\"msg\":\"Invalid wallet type!\"}");
                return;
            }

            string WalletName = GetWalletName(WalletType);

            if (FormNo > 0)
            {
                LoginSuccess = 1;
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"pointwalletamount\":\"0\",\"orderno\":\"0\",\"voucherno\":\"0\",\"msg\":\"Invalid User\",\"wallettype\":\"" + WalletType + "\"}");
                return;
            }

            double AvailBal = Convert.ToDouble(GetBalance(FormNo.ToString(), WalletType));

            if (AvailBal < Convert.ToDouble(Amount) && LoginSuccess == 1)
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"pointwalletamount\":\"0\",\"orderno\":\"0\",\"voucherno\":\"0\",\"msg\":\"Available Balance is " + AvailBal + "\",\"wallettype\":\"" + WalletType + "\"}");
                return;
            }
            else
            {
                LoginSuccess = 2;
            }

            if (LoginSuccess == 2)
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                if (Convert.ToDecimal(Amount) > 0)
                {
                    string sessn = "";

                    string q = Obj.IsoStart + "SELECT MAX(sessid) AS sessid FROM " + Obj.dBName + "..D_Sessnmaster" + Obj.IsoEnd;
                    DataTable DtQ = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, q).Tables[0];

                    if (DtQ.Rows.Count > 0)
                        sessn = DtQ.Rows[0]["sessid"].ToString();

                    string sql = Obj.IsoStart + "SELECT * FROM " + Obj.dBName + "..TrnVoucher WHERE Crto='" + FormNo + "' AND OrderNo_='" + orderno + "' AND Amount='" + Amount + "' AND Actype='M'" + Obj.IsoEnd;
                    DataTable dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"pointwalletamount\":\"0\",\"orderno\":\"0\",\"voucherno\":\"0\",\"msg\":\"order no. already exist.\",\"wallettype\":\"" + WalletType + "\"}");
                    }
                    else
                    {
                        str = "INSERT RefTrnVoucher (VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,Actype,VType,sessid,WSessID,OrderNo)";
                        str += " SELECT ISNULL(MAX(VoucherNo)+1,1001),CAST(CONVERT(varchar,GETDATE(),106) AS datetime),'0','" + FormNo + "',";
                        str += "'" + Amount + "','" + WalletName + " Used by against order no." + orderno + "',";
                        str += "'" + orderno + "','" + WalletType + "','C',CONVERT(varchar,CAST(CAST(GETDATE() AS varchar) AS datetime),112),'1','" + orderno + "' FROM RefTrnVoucher";

                        int i = SqlHelper.ExecuteNonQuery(Conn, CommandType.Text, str);

                        if (i > 0)
                        {
                            Fund_ConfirmOrderAmountStatus_Check(Uname, WalletName, WalletType, orderno, FormNo.ToString(), Amount);
                        }
                        else
                        {
                            Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"pointwalletamount\":\"0\",\"orderno\":\"0\",\"voucherno\":\"0\",\"msg\":\"Failed\",\"wallettype\":\"" + WalletType + "\"}");
                        }
                    }
                }
                else
                {
                    Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"pointwalletamount\":\"0\",\"orderno\":\"0\",\"voucherno\":\"0\",\"msg\":\"Failed\",\"wallettype\":\"" + WalletType + "\"}");
                }
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"pointwalletamount\":\"0\",\"orderno\":\"0\",\"voucherno\":\"0\",\"msg\":\"Login failed\",\"wallettype\":\"" + WalletType + "\"}");
            }
        }
        catch
        {
            Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"pointwalletamount\":\"0\",\"orderno\":\"0\",\"voucherno\":\"0\",\"msg\":\"Failed\",\"wallettype\":\"" + WalletType + "\"}");
        }
    }
    public string Fund_ConfirmOrderAmountStatus_Check(string Uname, string WalletName, string WalletType, string OrderNo, string Formno_V, string Amount)
    {
        string URL = "";
        string sResult = "";
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        sResult = current_datetime + random_number.ToString().PadLeft(3, '0');

        string postData = "";
        string statusApi = "";
        string responseString = "";
        string To_WalletAddress = "";

        try
        {
            string value = "";
            string Code = "";
            DataSet data = new DataSet();

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                URL = "http://gvapi.bisplindia.in/api/v1/Order/ConfirmOrderAmountStatus";

                WebRequest tRequest = WebRequest.Create(URL);
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";

                postData = "{\"orderno\":\"" + OrderNo.Trim() + "\",\"amount\":\"" + Amount.Trim() + "\"}";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                string sql_req = "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID,Formno,Request,postdata,ApiType) VALUES ('" +
                                 sResult + "','" + Formno_V + "','" + URL + "','" + postData + "','CONFIRMORDERAMOUNTSTATUS')";
                SqlHelper.ExecuteNonQuery(Conn, CommandType.Text, sql_req);

                tRequest.ContentLength = byteArray.Length;
                using (var dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                WebResponse tResponse = tRequest.GetResponse();
                using (var dataStream = tResponse.GetResponseStream())
                using (var tReader = new StreamReader(dataStream))
                {
                    str = tReader.ReadToEnd();
                }

                string sql_res = "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response='" + str +
                                 "' WHERE ReqID='" + sResult + "' AND ApiType='CONFIRMORDERAMOUNTSTATUS'";
                SqlHelper.ExecuteNonQuery(Conn, CommandType.Text, sql_res);

                // Parse JSON
                data = convertJsonStringToDataSet(str);
                statusApi = data.Tables[0].Rows[0]["status"].ToString().ToUpper();

                if (statusApi == "SUCCESS")
                {
                    if (data.Tables[1].Rows.Count > 0)
                    {
                        string orderID = data.Tables[1].Rows[0]["orderID"].ToString();
                        string OrderStatus = data.Tables[1].Rows[0]["orderStatus"].ToString();
                        string totalNetAmt = data.Tables[1].Rows[0]["totalNetAmt"].ToString();

                        if (orderID == OrderNo.Trim() && Convert.ToDecimal(totalNetAmt) == Convert.ToDecimal(Amount))
                        {
                            if (OrderStatus.ToUpper() == "PENDING")
                            {
                                string sql = "SELECT * FROM TrnVoucher WHERE OrderNo_='" + orderID + "' AND Amount='" + totalNetAmt + "' AND Actype='M'";
                                DataTable dt = SqlHelper.ExecuteDataset(Conn, CommandType.Text, sql).Tables[0];

                                if (dt.Rows.Count > 0)
                                {
                                    Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"pointwalletamount\":\"0\",\"orderno\":\"0\",\"voucherno\":\"0\",\"msg\":\"order no. already exist.\",\"wallettype\":\"" + WalletType + "\"}");
                                }
                                else
                                {
                                    string insert = "INSERT TrnVoucher (VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,Actype,VType,sessid,WSessID,OrderNo_)";
                                    insert += " SELECT ISNULL(MAX(VoucherNo)+1,1001),CAST(CONVERT(varchar,GETDATE(),106) AS datetime),'0','" + Formno_V + "',";
                                    insert += "'" + totalNetAmt + "','" + WalletName + " Used by against order no." + orderID + "','" + orderID + "','" +
                                              WalletType + "','C',CONVERT(varchar,CAST(CAST(GETDATE() AS varchar) AS datetime),112),'1','" + orderID + "' FROM TrnVoucher";

                                    int i = SqlHelper.ExecuteNonQuery(Conn, CommandType.Text, insert);

                                    if (i > 0)
                                    {
                                        string s = "SELECT TOP 1 VoucherNo,Amount FROM TrnVoucher WHERE CrTo='" + Formno_V +
                                                   "' AND OrderNo_='" + orderID + "' AND Actype='M' ORDER BY Voucherid DESC";
                                        DataTable dt_ = SqlHelper.ExecuteDataset(Conn, CommandType.Text, s).Tables[0];

                                        string VoucherNoX = dt_.Rows[0]["VoucherNo"].ToString();

                                        Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"OK\",\"pointwalletamount\":\"" + Amount +
                                                       "\",\"orderno\":\"" + OrderNo + "\",\"voucherno\":\"" + VoucherNoX +
                                                       "\",\"msg\":\"SUCCESS\",\"wallettype\":\"" + WalletType + "\"}");
                                    }
                                }
                            }
                            else
                            {
                                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"pointwalletamount\":\"0\",\"orderno\":\"0\",\"voucherno\":\"0\",\"msg\":\"order no. already exist.\",\"wallettype\":\"" + WalletType + "\"}");
                            }
                        }
                        else
                        {
                            Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"pointwalletamount\":\"0\",\"orderno\":\"0\",\"voucherno\":\"0\",\"msg\":\"Amount Not Match.\",\"wallettype\":\"" + WalletType + "\"}");
                        }
                    }
                }
                else
                {
                    Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"pointwalletamount\":\"0\",\"orderno\":\"0\",\"voucherno\":\"0\",\"msg\":\"api response failed.\",\"wallettype\":\"" + WalletType + "\"}");
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;

                string errorQry = "INSERT INTO TrnLogData(ErrorText,LogDate,Url,WalletAddress,PostData,Formno) VALUES('" +
                                  errorMsg + "',GETDATE(),'" + URL + "','" + To_WalletAddress + "','" + str + "','" + Formno_V + "')";
                SqlHelper.ExecuteNonQuery(Conn, CommandType.Text, errorQry);

                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"pointwalletamount\":\"0\",\"orderno\":\"0\",\"voucherno\":\"0\",\"msg\":\"" + errorMsg + "\",\"wallettype\":\"" + WalletType + "\"}");
            }
        }
        catch { }

        return sResult;
    }
    public DataSet convertJsonStringToDataSet(string jsonString)
    {
        XmlDocument xd = new XmlDocument();

        jsonString = "{ \"rootNode\": {" +
                     jsonString.Trim().TrimStart('{').TrimEnd('}') +
                     "} }";

        xd = JsonConvert.DeserializeXmlNode(jsonString);

        DataSet ds = new DataSet();
        ds.ReadXml(new XmlNodeReader(xd));

        return ds;
    }
    public string GetCompID()
    {
        string url = string.Empty;
        SqlConnection conn = null;

        try
        {
            url = HttpContext.Current.Request.Url.Host.ToUpper()
                .Replace("HTTP://", "").Replace("HTTPS://", "")
                .Replace("WWW.", "").Replace("BASICMLM.", "")
                .Replace("CPANEL.", "").Replace("LOGIN.", "")
                .Replace("CONSULTANT.", "").Replace("NETWORK.", "");

            string str = "";

            if (url == "LOCALHOST")
            {
                str = "SELECT ID,Logo,PartyCode,WalletType,WalletTypeR " +
                      "FROM M_CompanyMasterNew WHERE IsActive = 1 AND ID='" +
                      ConfigurationManager.AppSettings["CompanyID"] + "'";
            }
            else
            {
                str = "SELECT ID,Logo,PartyCode,WalletType,WalletTypeR " +
                      "FROM M_CompanyMasterNew WHERE IsActive = 1 AND (Upper(URL)='" +
                      url.Trim() + "')";
            }

            conn = new SqlConnection(Application["sConnect"].ToString());
            conn.Open();

            SqlCommand cmd = new SqlCommand(str, conn);
            SqlDataReader dRead = cmd.ExecuteReader();

            if (dRead.Read())
            {
                Session["CompID"] = dRead["ID"];
                Session["Logo"] = "http://superadmin.bisplindia.in/images/Logo/" + dRead["Logo"];
                Session["WRPartyCode"] = dRead["PartyCode"];
                Session["MWalletType"] = dRead["WalletType"];
                Session["RWalletType"] = dRead["WalletTypeR"];
            }

            dRead.Close();
            conn.Close();
        }
        catch
        {
            if (conn != null && conn.State == ConnectionState.Open)
                conn.Close();
        }

        return url;
    }
    private void ColumnName()
    {
        try
        {
            DataSet ds = new DataSet();

            string str = "SELECT ColName1,ColName2,ColName3 " +
                         "FROM M_CompanyColSetting WHERE CompanyID='" +
                         HttpContext.Current.Session["CompID"] + "'";

            ds = SqlHelper.ExecuteDataset(Application["sConnect"].ToString(), CommandType.Text, str);

            DataTable dtMenu = ds.Tables[0];

            Session["ColName1"] = dtMenu.Rows[0]["ColName1"];
            Session["ColName2"] = dtMenu.Rows[0]["ColName2"];
            Session["ColName3"] = dtMenu.Rows[0]["ColName3"];
        }
        catch { }
    }
    private void Pages()
    {
        try
        {
            DataSet ds = new DataSet();

            string str = "SELECT a.MenuId,a.MenuName,a.ParentId,a.OnSelect " +
                         "FROM M_CompWiseWebMenuMasterDis a " +
                         "WHERE MenuId IN (1,3) AND a.ActiveStatus='Y' AND a.RowStatus='Y' " +
                         "AND a.CompanyID='" + HttpContext.Current.Session["CompID"] + "' " +
                         "ORDER BY CONVERT(decimal,RTRIM(LTRIM(a.Hierar))), a.MenuId";

            ds = SqlHelper.ExecuteDataset(Application["sConnect"].ToString(), CommandType.Text, str);

            DataTable dtMenu = ds.Tables[0];

            Session["IndexPage"] = dtMenu.Rows[0]["OnSelect"];
            Session["JoinPage"] = dtMenu.Rows[1]["OnSelect"];
        }
        catch { }
    }
    private void getData()
    {
        try
        {
            cls_DataAccess dbConnect = new cls_DataAccess(
                HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString());

            dbConnect.OpenConnection();

            SqlCommand cmd;
            SqlDataReader dRead;

            // Company master
            cmd = new SqlCommand(Obj.IsoStart + "SELECT * FROM " + Obj.dBName + "..M_CompanyMaster" + Obj.IsoEnd, dbConnect.cnnObject);
            dRead = cmd.ExecuteReader();

            if (dRead.Read())
            {
                Session["CompName"] = dRead["CompName"];
                Session["CompAdd"] = dRead["CompAdd"];
                Session["CompWeb"] = (dRead["WebSite"].ToString() == "" ? "index.asp" : dRead["WebSite"]);
                Session["Title"] = dRead["CompTitle"];
                Session["CompMail"] = dRead["CompMail"];
                Session["CompMobile"] = dRead["MobileNo"];
                Session["ClientId"] = dRead["smsSenderId"];
                Session["SmsId"] = dRead["smsUserNm"];
                Session["SmsPass"] = dRead["smPass"];
                Session["MailPass"] = dRead["mailPass"];
                Session["MailHost"] = dRead["mailHost"];
                Session["AdminWeb"] = dRead["AdminWeb"];
                Session["CompCST"] = dRead["CompCSTNo"];
                Session["CompState"] = dRead["CompState"];
                Session["CompDate"] = Convert.ToDateTime(dRead["RecTimeStamp"]).ToString("dd-MMM-yyyy");
                Session["Spons"] = "PR10000001";
                Session["CompWeb1"] = dRead["WebSite"];
                Session["CompMovieWeb"] = "";
                Session["SmsAPI"] = "";

                string baseUrl = HttpContext.Current.Request.Url.Host;
                string protocol = HttpContext.Current.Request.Url.Host.ToUpper().StartsWith("HTTPS") ? "https://" : "http://";

                Session["CompShortUrl"] = protocol + baseUrl + "/" + Session["JoinPage"];
            }
            else
            {
                Session["CompName"] = "";
                Session["CompAdd"] = "";
                Session["CompWeb"] = "";
                Session["Title"] = "Welcome";
            }

            dRead.Close();

            // Config master
            cmd = new SqlCommand(Obj.IsoStart + "SELECT * FROM " + Obj.dBName + "..M_ConfigMaster" + Obj.IsoEnd, dbConnect.cnnObject);
            dRead = cmd.ExecuteReader();

            if (dRead.Read())
            {
                Session["IsGetExtreme"] = dRead["IsGetExtreme"];
                Session["IsTopUp"] = dRead["IsTopUp"];
                Session["IsSendSMS"] = dRead["IsSendSMS"];
                Session["IdNoPrefix"] = dRead["IdNoPrefix"];
                Session["IsFreeJoin"] = dRead["IsFreeJoin"];
                Session["IsStartJoin"] = dRead["IsStartJoin"];
                Session["JoinStartFrm"] = dRead["JoinStartFrm"];
                Session["IsSubPlan"] = dRead["IsSubPlan"];
                Session["Logout"] = dRead["LogoutPg"];
            }
            else
            {
                Session["IsGetExtreme"] = "N";
                Session["IsTopUp"] = "N";
                Session["IsSendSMS"] = "N";
                Session["IdNoPrefix"] = "";
                Session["IsFreeJoin"] = "N";
                Session["IsStartJoin"] = "N";
                Session["JoinStartFrm"] = "01-Sep-2011";
                Session["IsSubPlan"] = "N";
                Session["Logout"] = "Default.aspx";
            }

            dRead.Close();

            // Session values
            string[] sessionQueries = new string[]
            {
           Obj.IsoStart +  "SELECT MAX(SEssid) AS SessID FROM " + Obj.dBName + "..D_Monthlypaydetail" + Obj.IsoEnd,
           Obj.IsoStart +  "SELECT MAX(SEssid) AS SessID FROM " + Obj.dBName + "..m_SessnMaster" + Obj.IsoEnd,
           Obj.IsoStart +  "SELECT MAX(SEssid) AS SessID FROM " + Obj.dBName + "..M_MonthSessnMaster" + Obj.IsoEnd
            };

            string[] sessionKeys = new string[]
            {
            "MaxSessn",
            "CurrentSessn",
            "MonthSessn"
            };

            for (int i = 0; i < sessionQueries.Length; i++)
            {
                cmd = new SqlCommand(sessionQueries[i], dbConnect.cnnObject);
                dRead = cmd.ExecuteReader();

                if (dRead.Read())
                    Session[sessionKeys[i]] = dRead["SessID"];
                else
                    Session[sessionKeys[i]] = "";

                dRead.Close();
            }
        }
        catch
        {
            Session["CompName"] = "";
            Session["CompAdd"] = "";
            Session["CompWeb"] = "";
        }
    }
    private string GetWalletName(string WalletType)
    {
        try
        {
            string RtrVal = "";
            string str = Obj.IsoStart + "SELECT * FROM " + Obj.dBName + "..VoucherType WHERE AcType='" + WalletType + "'" + Obj.IsoEnd;
            DataSet ds = SqlHelper.ExecuteDataset(
                HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                CommandType.Text,
                str
            );

            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
                RtrVal = dt.Rows[0]["WalletName"].ToString();
            else
                RtrVal = "";

            return RtrVal;
        }
        catch
        {
            return "";
        }
    }
    private bool GetReferenceNo(string Formno, string Refno)
    {
        bool RtrVal = false;

        try
        {
            Conn = new SqlConnection(
                HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString()
            );
            Conn.Open();

            string sql = Obj.IsoStart + "SELECT * FROM " + Obj.dBName + "..TrnVoucher WHERE DrTo='" + Formno +
                         "' AND RefNo='" + Refno + "'" + Obj.IsoEnd;

            Comm = new SqlCommand(sql, Conn);
            SqlDataReader Dr = Comm.ExecuteReader();

            if (Dr.Read())
                RtrVal = true;
            else
                RtrVal = false;

            Dr.Close();
            Comm.Cancel();
            Conn.Close();

            return RtrVal;
        }
        catch
        {
            return false;
        }
    }
    private double GetBalance(string FormNo, string WalletType)
    {
        try
        {
            double RtrVal = 0;

            Conn = new SqlConnection(
                HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString()
            );
            Conn.Open();

            string sql = Obj.IsoStart + "SELECT Balance FROM dbo.ufnGetBalance('" + FormNo + "','" + WalletType + "')" + Obj.IsoEnd;

            Comm = new SqlCommand(sql, Conn);
            SqlDataReader Dr = Comm.ExecuteReader();

            if (Dr.Read())
                RtrVal = Convert.ToDouble(Dr["Balance"]);

            Dr.Close();
            Comm.Cancel();
            Conn.Close();

            return RtrVal;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            return 0;
        }
    }
    private void DeductWalletAmount(string WalletType)
    {
        string VoucherNo = "";

        string Uname = Clean(Request["Username"]);
        string Pwd = Clean(Request["Password"])
            .Replace("%25", "%").Replace("%23", "#")
            .Replace("%26", "&").Replace("%22", "'")
            .Replace("%40", "@");

        string TxnData = Request["TxnData"];

        try
        {
            int LoginSuccess = 0;
            int FormNo = 0;

            Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
            if (Conn.State == ConnectionState.Closed) Conn.Open();

            FormNo = GetFormNo(Uname, Pwd);
            string WalletName = GetWalletName(WalletType);

            if (FormNo > 0)
                LoginSuccess = 1;
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Invalid User\",\"wallettype\":\"" + WalletType + "\"}");
                return;
            }

            if (WalletName == "")
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Wallet Type Can't Blank\",\"wallettype\":\"" + WalletType + "\"}");
                return;
            }

            string[] TData = TxnData.Split(';');
            string TxnID, Remarks;
            double Bv;

            if (TData.Length == 3)
            {
                TxnID = TData[0].Trim();
                Bv = Convert.ToDouble(TData[1]);
                Remarks = TData[2].Trim();
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Insufficient Data\",\"wallettype\":\"" + WalletType + "\"}");
                return;
            }

           
            bool refExists = false;

            if (Session["CompID"].ToString() == "1093")
                refExists = GetReferenceNo(FormNo.ToString(), TxnID);

            double AvailBal = Convert.ToDouble(GetBalance(FormNo.ToString(), WalletType));

            if (AvailBal < Bv && LoginSuccess == 1)
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Available Balance is " + AvailBal + "\",\"wallettype\":\"" + WalletType + "\"}");
                return;
            }
            else if (refExists)
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"TxnId already Exists " + TxnID + "\",\"wallettype\":\"" + WalletType + "\"}");
                return;
            }
            else
            {
                LoginSuccess = 2;
            }

            if (LoginSuccess == 2)
            {
                if (Conn.State == ConnectionState.Closed) Conn.Open();

                if (Bv > 0)
                {
                    string sessn = "";
                    string q = Obj.IsoStart + "select Isnull(Max(Sessid),Convert(Varchar,Getdate(),112)) as Sessid from " + Obj.dBName + "..D_Sessnmaster " + Obj.IsoEnd;
                    Comm = new SqlCommand(q, Connselect);
                    SqlDataReader Dr = Comm.ExecuteReader();
                    if (Dr.Read())
                        sessn = Dr["SessId"].ToString();
                    Dr.Close();

                    string sql = Obj.IsoStart + "SELECT * FROM " + Obj.dBName + "..TrnVoucher WHERE refno='" + TxnID + "'" + Obj.IsoEnd;
                    DataTable dt = SqlHelper.ExecuteDataset(Connselect, CommandType.Text, sql).Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Already Deducted Amount\",\"wallettype\":\"" + WalletType + "\"}");
                    }
                    else
                    {
                        
                            str = "INSERT TrnVoucher (VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,Actype,VType,sessid,WSessID) " +
                                  "SELECT ISNULL(MAX(VoucherNo)+1,1001),CAST(CONVERT(varchar,GETDATE(),106) AS DateTime),'" + FormNo + "','0','" + Bv + "'," +
                                  "'" + WalletName + " Used by " + Remarks + " against order no." + TxnID + "'," +
                                  "'" + TxnID + "','" + WalletType + "','D',CONVERT(varchar,CAST(CAST(GETDATE() AS varchar) AS datetime),112)," +
                                  "'" + Session["CurrentSessn"] + "' FROM TrnVoucher";
                        

                        int i = SqlHelper.ExecuteNonQuery(Conn, CommandType.Text, str);
                        string s = Obj.IsoStart + "SELECT Max(VoucherNo) AS VoucherNo FROM " + Obj.dBName + "..TrnVoucher WHERE Actype='" + WalletType + "' AND Drto='" + FormNo + "' AND Amount='" + Bv + "'" + Obj.IsoEnd;
                        Comm = new SqlCommand(s, Connselect);
                        Dr = Comm.ExecuteReader();
                        if (Dr.Read())
                            VoucherNo = Dr["VoucherNo"].ToString();
                        Dr.Close();

                        Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"OK\",\"deductamount\":\"" + Bv + "\",\"voucherno\":\"" + VoucherNo + "\",\"msg\":\"success\",\"wallettype\":\"" + WalletType + "\"}");
                    }
                }
                else
                {
                    Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Failed\",\"wallettype\":\"" + WalletType + "\"}");
                }
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Login failed\",\"wallettype\":\"" + WalletType + "\"}");
            }
        }
        catch
        {
            Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Failed\",\"wallettype\":\"" + WalletType + "\"}");
        }
    }
    private void RefundWalletAmount(string WalletType)
    {
        string VoucherNo = "";
        string Uname = Clean(Request["Username"]);
        string Pwd = Clean(Request["Password"]);
        string TxnData = Request["TxnData"];
        string Wallet_ = "";

        try
        {
            int LoginSuccess = 0;
            int FormNo = 0;

            Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
            if (Conn.State == ConnectionState.Closed) Conn.Open();

            FormNo = GetFormNo_1(Uname);
            string WalletName = GetWalletName(WalletType);

            if (FormNo > 0)
                LoginSuccess = 1;
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Invalid User\",\"wallettype\":\"" + WalletType + "\"}");
                return;
            }

            if (WalletName == "")
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Wallet Type Can't Blank\",\"wallettype\":\"" + WalletType + "\"}");
                return;
            }

            string[] TData = TxnData.Split(';');
            string TxnID, Remarks;
            double Bv;

            if (TData.Length == 3)
            {
                TxnID = TData[0].Trim();
                Bv = Convert.ToDouble(TData[1]);
                Remarks = TData[2].Trim();
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Amount Not Refunded\",\"wallettype\":\"" + WalletType + "\"}");
                return;
            }

            // Validate order debit amount
            string str123 =
               Obj.IsoStart + "SELECT ISNULL(SUM(Debit),0) AS Debit, ISNULL(SUM(Credit),0) AS Credit FROM (" +
                " SELECT Amount AS Debit, 0 AS Credit FROM " + Obj.dBName + "..TrnVoucher WHERE Drto='" + FormNo + "' AND RefNo='" + TxnID + "' AND VType='D' " +
                " UNION ALL " +
                " SELECT 0 AS Debit, Amount AS Credit FROM " + Obj.dBName + "..TrnVoucher WHERE CrTo='" + FormNo + "' AND RefNo='" + TxnID + "' AND VType='C' " +
                ") AS RR" + Obj.IsoEnd;

            DataTable dt123 = SqlHelper.ExecuteDataset(Connselect, CommandType.Text, str123).Tables[0];

            if (Convert.ToDouble(dt123.Rows[0]["Debit"]) > 0)
            {
                if (Convert.ToDouble(dt123.Rows[0]["Debit"]) <
                    Convert.ToDouble(dt123.Rows[0]["Credit"]) + Bv)
                {
                    Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Amount greater than Debit Amount.\",\"wallettype\":\"" + WalletType + "\"}");
                    return;
                }
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Order Number Not Exists.\",\"wallettype\":\"" + WalletType + "\"}");
                return;
            }

            LoginSuccess = 2;

            if (LoginSuccess == 2)
            {
                if (Conn.State == ConnectionState.Closed) Conn.Open();

                if (Bv > 0)
                {
                    string sessn = "";
                    string q = Obj.IsoStart + "select Isnull(Max(Sessid),Convert(Varchar,Getdate(),112)) as Sessid from " + Obj.dBName + "..D_Sessnmaster " + Obj.IsoEnd;

                    Comm = new SqlCommand(q, Connselect);
                    SqlDataReader Dr = Comm.ExecuteReader();
                    if (Dr.Read())
                        sessn = Dr["SessId"].ToString();
                    Dr.Close();

                    str = "INSERT TrnVoucher (VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,Actype,VType,sessid,WSessID) " +
                             "SELECT ISNULL(MAX(VoucherNo)+1,1001),CAST(CONVERT(varchar,GETDATE(),106) AS DateTime),'0','" + FormNo + "','" + Bv + "'," +
                             "'" + WalletName + " Credited " + Remarks + " against tanscation no." + TxnID + "'," +
                             "'" + TxnID + "','" + WalletType + "','C',CONVERT(varchar,CAST(CAST(GETDATE() AS varchar) AS datetime),112)," +
                             "'" + Session["CurrentSessn"] + "' FROM TrnVoucher";

                    Comm = new SqlCommand(str, Conn);
                    Comm.ExecuteNonQuery();

                    string s = Obj.IsoStart + "SELECT Max(VoucherNo) AS VoucherNo FROM " + Obj.dBName + "..TrnVoucher WHERE Actype='" + WalletType + "' AND CrTo='" + FormNo + "' AND Amount='" + Bv + "'" + Obj.IsoEnd;

                    Comm = new SqlCommand(s, Connselect);
                    Dr = Comm.ExecuteReader();

                    if (Dr.Read())
                        VoucherNo = Dr["VoucherNo"].ToString();

                    Dr.Close();

                    Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"OK\",\"refundamount\":\"" + Bv + "\",\"voucherno\":\"" + VoucherNo + "\",\"msg\":\"success\",\"wallettype\":\"" + WalletType + "\"}");
                }
                else
                {
                    Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Failed\",\"wallettype\":\"" + WalletType + "\"}");
                }
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Login failed\",\"wallettype\":\"" + WalletType + "\"}");
            }
        }
        catch
        {
            Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Failed\",\"wallettype\":\"" + WalletType + "\"}");
        }
    }
    private void Confirmvoucher(string VoucherRequest, string VoucherResponse, string WalletType)
    {
        try
        {
            Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
            Conn.Open();

            double AvailBal;
            int i = 0;

            if (VoucherResponse.ToUpper() == "OK")
            {
                int FormNo = GetFormNo(Request["UserName"], Request["Password"]);

                string sql = "UPDATE TrnVoucher SET Narration = Narration + '-" + VoucherResponse +
                             "' WHERE VoucherNo='" + VoucherRequest +
                             "' AND DrTo='" + FormNo + "' AND Actype='" + WalletType + "'";

                Comm = new SqlCommand(sql, Conn);
                i = Comm.ExecuteNonQuery();

                if (FormNo > 0)
                    AvailBal = Convert.ToDouble(GetBalance(FormNo.ToString(), WalletType));
                else
                    AvailBal = 0;

                if (i > 0)
                {
                    Response.Write("{\"Login\":\"" + Request["UserName"] + "\",\"response\":\"OK\",\"ewallet\":\"" + AvailBal + "\",\"wallettype\":\"" + WalletType + "\"}");
                }
                else
                {
                    Response.Write("{\"Login\":\"" + Request["UserName"] + "\",\"response\":\"FAILED\",\"msg\":\"Invalid Login details\",\"wallettype\":\"" + WalletType + "\"}");
                }
            }
            else if (VoucherResponse.ToUpper() == "FAILED")
            {
                int FormNo = GetFormNo(Request["UserName"], Request["Password"]);

                string sql1 =
                    "INSERT INTO TempTrnVoucher (VoucherId,VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,Actype,RecTimeStamp,VType,Sessid,WSessid) " +
                    "SELECT VoucherId,VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration + '-" + VoucherResponse +
                    "',RefNo,Actype,RecTimeStamp,VType,Sessid,WSessid FROM TrnVoucher WHERE VoucherNo='" + VoucherRequest + "'";

                Comm = new SqlCommand(sql1, Conn);
                Comm.ExecuteNonQuery();

                string sqlDel = "DELETE FROM TrnVoucher WHERE VoucherNo='" + VoucherRequest +
                                "' AND DrTo='" + FormNo + "' AND Actype='" + WalletType + "'";

                Comm = new SqlCommand(sqlDel, Conn);
                i = Comm.ExecuteNonQuery();

                if (FormNo > 0)
                    AvailBal = Convert.ToDouble(GetBalance(FormNo.ToString(), WalletType));
                else
                    AvailBal = 0;

                Response.Write("{\"Login\":\"" + Request["UserName"] + "\",\"response\":\"FAILED\",\"msg\":\"Invalid Login details\",\"wallettype\":\"" + WalletType + "\"}");
            }

            Conn.Close();
        }
        catch
        {
            Response.Write("{\"response\":\"FAILED\",\"ewallet\":\"0\"}");
        }
    }
    private void checkvoucher(string VoucherRequest, string VoucherResponse)
    {
        try
        {
            Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
            Conn.Open();

            double AvailBal = 0;
            int i = 0;

            if (VoucherResponse.ToUpper() == "OK")
            {
                int FormNo = GetFormNo(Request["UserName"], Request["Password"]);

                string sql = "UPDATE TrnVoucher SET Narration = Narration + '-" + VoucherResponse +
                             "' WHERE VoucherNo='" + VoucherRequest +
                             "' AND DrTo='" + FormNo +
                             "' AND Actype='" + Session["RWalletType"] + "'";

                Comm = new SqlCommand(sql, Conn);
                i = Comm.ExecuteNonQuery();

                if (FormNo > 0)
                    AvailBal = Convert.ToDouble(GetSWalletBalance(FormNo.ToString()));

                if (i > 0)
                {
                    Response.Write("{\"Login\":\"" + Request["UserName"] +
                                   "\",\"response\":\"OK\",\"ewallet\":\"" + AvailBal +
                                   "\",\"wallettype\":\"" + Session["RWalletType"] + "\"}");
                }
                else
                {
                    Response.Write("{\"Login\":\"" + Request["UserName"] +
                                   "\",\"response\":\"FAILED\",\"msg\":\"Invalid Login details\"}");
                }
            }
            else if (VoucherResponse.ToUpper() == "FAILED")
            {
                int FormNo = GetFormNo(Request["UserName"], Request["Password"]);

                string sql1 =
                    "INSERT INTO TempTrnVoucher (VoucherId,VoucherNo,VoucherDate,DrTo,Crto,Amount," +
                    "Narration,RefNo,Actype,RecTimeStamp,VType,Sessid,WSessid) " +
                    "SELECT VoucherId,VoucherNo,VoucherDate,DrTo,Crto,Amount," +
                    "Narration + '-" + VoucherResponse + "',RefNo,Actype," +
                    "RecTimeStamp,VType,Sessid,WSessid FROM TrnVoucher WHERE VoucherNo='" + VoucherRequest + "'";

                Comm = new SqlCommand(sql1, Conn);
                Comm.ExecuteNonQuery();

                string sqlDel =
                    "DELETE FROM TrnVoucher WHERE VoucherNo='" + VoucherRequest +
                    "' AND DrTo='" + FormNo +
                    "' AND Actype='" + Session["RWalletType"] + "'";

                Comm = new SqlCommand(sqlDel, Conn);
                Comm.ExecuteNonQuery();

                if (FormNo > 0)
                    AvailBal = Convert.ToDouble(GetSWalletBalance(FormNo.ToString()));

                Response.Write("{\"Login\":\"" + Request["UserName"] +
                               "\",\"response\":\"FAILED\",\"msg\":\"Invalid Login details\"}");
            }

            Conn.Close();
        }
        catch
        {
            Response.Write("{\"response\":\"FAILED\",\"ewallet\":\"0\"}");
        }
    }
    private void CheckInfoDetail()
    {
        try
        {
            string username = Request["Username"];
            string password = (Request["Password"] ?? "").Replace("%25", "%").Replace("%23", "#").Replace("%26", "&").Replace("%22", "'").Replace("%40", "@");
            string connStr = HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]]?.ToString();

            var outObj = new CheckInfoResponse();
            outObj.ismovie = "N";

            // call stored proc ProcCheckInfoNew
            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("ProcCheckInfoNew", conn))
            {
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Username", username ?? "");
                //cmd.Parameters.AddWithValue("@Password", password ?? "");

                // Username / IdNo
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open(); // Important

                // Get procedure parameters
                SqlCommandBuilder.DeriveParameters(cmd);

                // Username / IdNo
                if (cmd.Parameters.Contains("@Username"))
                    cmd.Parameters["@Username"].Value = username ?? "";
                else if (cmd.Parameters.Contains("@Idno"))
                    cmd.Parameters["@Idno"].Value = username ?? "";

                // Password / Passw
                if (cmd.Parameters.Contains("@Password"))
                    cmd.Parameters["@Password"].Value = password ?? "";
                else if (cmd.Parameters.Contains("@Passw"))
                    cmd.Parameters["@Passw"].Value = password ?? "";

                conn.Close(); // Optional

                var ds = new DataSet();
                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(ds);
                }
                //var ds = new DataSet();
                //using (var da = new SqlDataAdapter(cmd))
                //{
                //    da.Fill(ds);
                //}

                DataTable dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;

                if (dt != null && dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    int formno = Convert.ToInt32(row["FormNo"]);

                    outObj.rwallet = Convert.ToDouble(GetSWalletBalance(formno.ToString()));
                    outObj.ewallet = Convert.ToDouble(GetMWalletBalance(formno.ToString()));

                    // map commonly used fields
                    outObj.formno = row["FormNo"].ToString();
                    outObj.loginid = row["IDNo"]?.ToString();
                    outObj.name = row["MemName"]?.ToString();
                    outObj.doj = row["JoinDate"]?.ToString();
                    outObj.email = row["Email"]?.ToString();
                    outObj.mobileno = row["Mobl"]?.ToString();
                    outObj.city = row["City"]?.ToString();
                    outObj.isactive = row["ActiveStatus"]?.ToString();
                    outObj.kitid = row.Table.Columns.Contains("KitId") ? row["KitId"]?.ToString() : "";
                    outObj.kitname = row.Table.Columns.Contains("KitName") ? row["KitName"]?.ToString() : "";
                    outObj.kitstatus = row.Table.Columns.Contains("KitStatus") ? row["KitStatus"]?.ToString() : "";
                    outObj.status = row.Table.Columns.Contains("STATUS1") ? row["STATUS1"]?.ToString() : "";
                    outObj.activedate = row.Table.Columns.Contains("DOA") ? row["DOA"]?.ToString() : "";
                    outObj.kitamount = row.Table.Columns.Contains("KitAmount") ? row["KitAmount"]?.ToString() : "";
                    outObj.isholiday = row.Table.Columns.Contains("isholiday") ? row["isholiday"]?.ToString() : "";
                    outObj.shoppoint = row.Table.Columns.Contains("shoppoint") ? row["shoppoint"]?.ToString() : "";
                    outObj.promoid = row.Table.Columns.Contains("promoid") ? row["promoid"]?.ToString() : "";
                    outObj.promovalue = row.Table.Columns.Contains("promovalue") ? row["promovalue"]?.ToString() : "";
                    outObj.rcouponvalue = row.Table.Columns.Contains("rcouponvalue") ? row["rcouponvalue"]?.ToString() : "0";
                    outObj.rcoupon = row.Table.Columns.Contains("rcoupon") ? row["rcoupon"]?.ToString() : "0";

                    // For backward compatibility with clients expecting "data" to hold profile table JSON:
                    outObj.data = JsonConvert.DeserializeObject(ConvertDataTableToJson(dt));
                }
                else
                {
                    // invalid credentials response
                    outObj.formno = "0";
                    outObj.loginid = username;
                    outObj.name = "Invalid Credentials";
                    outObj.doj = "";
                    outObj.email = "";
                    outObj.mobileno = "0";
                    outObj.city = "";
                    outObj.isactive = "";
                    outObj.kitid = "0";
                    outObj.kitname = "";
                    outObj.kitstatus = "";
                    outObj.status = "FAIL";
                    outObj.rwallet = 0;
                    outObj.ewallet = 0;
                    outObj.ismovie = "N";
                    outObj.activedate = "";
                    outObj.kitamount = "0";
                    outObj.isholiday = "";
                    outObj.shoppoint = "0";
                    outObj.promoid = "";
                }
            }

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(outObj));
        }
        catch (Exception ex)
        {
            var fail = new CheckInfoResponse
            {
                formno = "0",
                loginid = "Invalid",
                name = "Invalid Credentials",
                doj = "",
                email = "",
                mobileno = "0",
                city = "",
                isactive = "",
                kitid = "0",
                kitname = "",
                kitstatus = "",
                status = "FAIL",
                rwallet = 0,
                ewallet = 0
            };
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(fail));
        }
    }
    //private void CheckInfoDetailData()
    //{
    //    try
    //    {
    //        string username = Request["Username"];
    //        string password = (Request["Password"] ?? "").Replace("%25", "%").Replace("%23", "#").Replace("%26", "&").Replace("%22", "'").Replace("%40", "@");
    //        string connStr = HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]]?.ToString();

    //        var result = new Dictionary<string, object>();
    //        List<OrderItem> orders = new List<OrderItem>();

    //        using (var conn = new SqlConnection(connStr))
    //        using (var cmd = new SqlCommand("ProcCheckInfoNew", conn))
    //        {
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            cmd.Parameters.AddWithValue("@Idno", username ?? "");
    //            cmd.Parameters.AddWithValue("@Passw", password ?? "");
    //            var ds = new DataSet();
    //            using (var da = new SqlDataAdapter(cmd)) da.Fill(ds);

    //            DataTable dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;

    //            if (dt != null && dt.Rows.Count > 0)
    //            {
    //                // First table is profile/user; some implementations may return multiple tables
    //                var profileRow = dt.Rows[0];
    //                int formno = Convert.ToInt32(profileRow["FormNo"]);
    //                double rwallet = Convert.ToDouble(GetSWalletBalance(formno.ToString()));
    //                double ewallet = Convert.ToDouble(GetMWalletBalance(formno.ToString()));

    //                // Map orders: if proc also returns order rows in same table, you may need to call another table.
    //                // The VB code iterated dt — so attempt to convert each row into an order entry (defensive).
    //                foreach (DataRow dr in dt.Rows)
    //                {
    //                    var o = new OrderItem
    //                    {
    //                        OrderNo = dr.Table.Columns.Contains("OrderNo") ? dr["OrderNo"]?.ToString() : "",
    //                        kitid = dr.Table.Columns.Contains("FrankitId") ? dr["FrankitId"]?.ToString() : dr.Table.Columns.Contains("kitid") ? dr["kitid"]?.ToString() : "",
    //                        kitname = dr.Table.Columns.Contains("Frankitname") ? dr["Frankitname"]?.ToString() : dr.Table.Columns.Contains("kitname") ? dr["kitname"]?.ToString() : "",
    //                        purchasedate = dr.Table.Columns.Contains("Billdate") ? dr["Billdate"]?.ToString() : "",
    //                        isusable = dr.Table.Columns.Contains("isusable") ? dr["isusable"]?.ToString() : "",
    //                        isfranchise = dr.Table.Columns.Contains("Is_FranKit") ? dr["Is_FranKit"]?.ToString() : "",
    //                        isholiday = dr.Table.Columns.Contains("isholiday") ? dr["isholiday"]?.ToString() : "",
    //                        franKitamount = dr.Table.Columns.Contains("franKitamount") ? dr["franKitamount"]?.ToString() : "",
    //                        franKitname = dr.Table.Columns.Contains("franKitname") ? dr["franKitname"]?.ToString() : "",
    //                        FranKitid = dr.Table.Columns.Contains("FranKitid") ? dr["FranKitid"]?.ToString() : "",
    //                        CouponMRP = dr.Table.Columns.Contains("CouponMRP") ? dr["CouponMRP"]?.ToString() : "",
    //                        shoppoint = dr.Table.Columns.Contains("shoppoint") ? dr["shoppoint"]?.ToString() : "",
    //                        promoid = dr.Table.Columns.Contains("promoid") ? dr["promoid"]?.ToString() : "",
    //                        promovalue = dr.Table.Columns.Contains("promovalue") ? dr["promovalue"]?.ToString() : "",
    //                        coupon = dr.Table.Columns.Contains("coupon") ? dr["coupon"]?.ToString() : "",
    //                        couponamount = dr.Table.Columns.Contains("couponamount") ? dr["couponamount"]?.ToString() : "",
    //                        hcount = dr.Table.Columns.Contains("hcount") ? dr["hcount"]?.ToString() : "",
    //                        isfood = dr.Table.Columns.Contains("isfood") ? dr["isfood"]?.ToString() : "",
    //                        isbigbazzar = dr.Table.Columns.Contains("IsBigBazzar") ? dr["IsBigBazzar"]?.ToString() : "",
    //                        couponvalue = dr.Table.Columns.Contains("CouponMRP") ? dr["CouponMRP"]?.ToString() : "",
    //                        RegistrationType = dr.Table.Columns.Contains("RegistrationType") ? dr["RegistrationType"]?.ToString() : ""
    //                    };
    //                    orders.Add(o);
    //                }

    //                // Construct result
    //                result["data"] = orders;
    //                result["formno"] = profileRow["FormNo"]?.ToString();
    //                result["loginid"] = profileRow["IDNo"]?.ToString();
    //                result["name"] = profileRow["MemName"]?.ToString();
    //                result["doj"] = profileRow["JoinDate"]?.ToString();
    //                result["email"] = profileRow["Email"]?.ToString();
    //                result["mobileno"] = profileRow["Mobl"]?.ToString();
    //                result["city"] = profileRow["City"]?.ToString();
    //                result["isactive"] = profileRow["ActiveStatus"]?.ToString();
    //                result["kitid"] = profileRow.Table.Columns.Contains("KitId") ? profileRow["KitId"]?.ToString() : "";
    //                result["kitname"] = profileRow.Table.Columns.Contains("KitName") ? profileRow["KitName"]?.ToString() : "";
    //                result["kitstatus"] = profileRow.Table.Columns.Contains("KitStatus") ? profileRow["KitStatus"]?.ToString() : "";
    //                result["status"] = profileRow.Table.Columns.Contains("STATUS1") ? profileRow["STATUS1"]?.ToString() : "";
    //                result["rwallet"] = rwallet;
    //                result["ewallet"] = ewallet;
    //                result["ismovie"] = "N";
    //                result["activedate"] = profileRow.Table.Columns.Contains("DOA") ? profileRow["DOA"]?.ToString() : "";
    //                result["kitamount"] = profileRow.Table.Columns.Contains("KitAmount") ? profileRow["KitAmount"]?.ToString() : "";
    //                result["isholiday"] = profileRow.Table.Columns.Contains("isholiday") ? profileRow["isholiday"]?.ToString() : "";
    //                result["shoppoint"] = profileRow.Table.Columns.Contains("shoppoint") ? profileRow["shoppoint"]?.ToString() : "";
    //                result["promoid"] = profileRow.Table.Columns.Contains("promoid") ? profileRow["promoid"]?.ToString() : "";
    //                result["promovalue"] = profileRow.Table.Columns.Contains("PromoValue") ? profileRow["PromoValue"]?.ToString() : profileRow.Table.Columns.Contains("promovalue") ? profileRow["promovalue"]?.ToString() : "";
    //                result["rcouponvalue"] = profileRow.Table.Columns.Contains("rcouponvalue") ? profileRow["rcouponvalue"]?.ToString() : "0";
    //                result["rcoupon"] = profileRow.Table.Columns.Contains("rcoupon") ? profileRow["rcoupon"]?.ToString() : "0";
    //                result["RegistrationType"] = profileRow.Table.Columns.Contains("RegistrationType") ? profileRow["RegistrationType"]?.ToString() : "";
    //            }
    //            else
    //            {
    //                // No data: produce fail structure similar to VB
    //                result["data"] = new List<OrderItem>();
    //                result["formno"] = "0";
    //                result["loginid"] = username;
    //                result["name"] = "Invalid Credentials";
    //                result["doj"] = "";
    //                result["email"] = "";
    //                result["mobileno"] = "0";
    //                result["city"] = "";
    //                result["isactive"] = "";
    //                result["kitid"] = "0";
    //                result["kitname"] = "";
    //                result["kitstatus"] = "";
    //                result["status"] = "FAIL";
    //                result["rwallet"] = 0;
    //                result["ewallet"] = 0;
    //                result["ismovie"] = "N";
    //                result["activedate"] = "";
    //                result["kitamount"] = "0";
    //                result["isholiday"] = "";
    //                result["shoppoint"] = "0";
    //                result["promoid"] = "";
    //                result["promovalue"] = "";
    //                result["rcouponvalue"] = "0";
    //                result["rcoupon"] = "0";
    //                result["RegistrationType"] = "";
    //            }
    //        }

    //        HttpContext.Current.Response.Clear();
    //        HttpContext.Current.Response.ContentType = "application/json";
    //        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
    //    }
    //    catch (Exception ex)
    //    {
    //        var fail = new
    //        {
    //            formno = "0",
    //            loginid = "Invalid",
    //            name = "Invalid Credentials",
    //            doj = "",
    //            email = "",
    //            mobileno = "0",
    //            city = "",
    //            isactive = "",
    //            kitid = "0",
    //            kitname = "",
    //            kitstatus = "",
    //            status = "FAIL",
    //            rwallet = 0,
    //            ewallet = 0
    //        };
    //        HttpContext.Current.Response.Write(JsonConvert.SerializeObject(fail));
    //    }
    //}
    public static string ConvertDataTableToJson(DataTable dataTable)
    {
        return JsonConvert.SerializeObject(dataTable, (Newtonsoft.Json.Formatting)System.Xml.Formatting.Indented);
    }
    private void CheckInfo()
    {
        try
        {
            string username = Request["Username"] ?? "";
            string password = (Request["Password"] ?? "")
                .Replace("%25", "%")
                .Replace("%23", "#")
                .Replace("%26", "&")
                .Replace("%40", "@")
                .Replace("%22", "'");

            string connStr = HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]]?.ToString();
            if (string.IsNullOrEmpty(connStr))
            {
                WriteFail(username);
                return;
            }

            DataTable dt;

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("ProcCheckInfo", conn))
            {
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Idno", username);
                //cmd.Parameters.AddWithValue("@Passw", password);

                //DataSet ds = new DataSet();
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da.Fill(ds);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open(); // Important

                // Get procedure parameters
                SqlCommandBuilder.DeriveParameters(cmd);

                // Username / IdNo
                if (cmd.Parameters.Contains("@Username"))
                    cmd.Parameters["@Username"].Value = username ?? "";
                else if (cmd.Parameters.Contains("@Idno"))
                    cmd.Parameters["@Idno"].Value = username ?? "";

                // Password / Passw
                if (cmd.Parameters.Contains("@Password"))
                    cmd.Parameters["@Password"].Value = password ?? "";
                else if (cmd.Parameters.Contains("@Passw"))
                    cmd.Parameters["@Passw"].Value = password ?? "";

                conn.Close(); // Optional

                var ds = new DataSet();
                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(ds);
                }
                dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                WriteFail(username);
                return;
            }

            DataRow r = dt.Rows[0];
            string formNo = r["FormNo"].ToString();
            string ismovie = r.Table.Columns.Contains("ismovie") ? r["ismovie"].ToString() : "N";

            double rwallet = Convert.ToDouble(GetSWalletBalance(formNo));
            double ewallet = Convert.ToDouble(GetMWalletBalance(formNo));

            string compid = (Session["CompID"] ?? "").ToString();

            // Generate matching JSON object
            var obj = new Dictionary<string, object>
            {
                ["formno"] = formNo,
                ["loginid"] = r["IDNo"],
                ["name"] = r["MemName"],
                ["doj"] = r["JoinDate"],
                ["email"] = r["Email"],
                ["mobileno"] = r["Mobl"],
                ["city"] = r["City"],
                ["isactive"] = r["ActiveStatus"],
                ["kitid"] = r["KitId"],
                ["kitname"] = r["KitName"],
                ["kitstatus"] = r["KitStatus"],
                ["status"] = r["STATUS1"],
                ["rwallet"] = rwallet,
                ["ewallet"] = ewallet,
                ["ismovie"] = ismovie,
                ["activedate"] = r["DOA"],
                ["kitamount"] = r["KitAmount"],
                ["isholiday"] = r["isholiday"],
                ["shoppoint"] = r["shoppoint"],
                ["promoid"] = r["promoid"]
            };

            // company-based variations
            obj["coupon"] = "0";
            obj["promovalue"] = "0";

            // Write JSON response
            Response.Write(JsonConvert.SerializeObject(obj));
        }
        catch
        {
            WriteFail("Invalid");
        }
    }
    private void WriteFail(string username)
    {
        var fail = new Dictionary<string, object>
        {
            ["formno"] = "0",
            ["loginid"] = username,
            ["name"] = "Invalid Credentials",
            ["doj"] = "",
            ["email"] = "",
            ["mobileno"] = "0",
            ["city"] = "",
            ["isactive"] = "",
            ["kitid"] = "0",
            ["kitname"] = "",
            ["kitstatus"] = "",
            ["status"] = "FAIL",
            ["rwallet"] = "0",
            ["ewallet"] = "0",
            ["ismovie"] = "N",
            ["activedate"] = "",
            ["kitamount"] = "0",
            ["isholiday"] = ""
        };

        Response.Write(JsonConvert.SerializeObject(fail));
    }
    private void AddBV()
    {
        objAccess = new DAL();
        string Uname = Clean(Request["Username"]);
        string Pwd = Clean(Request["Password"].Replace("%25", "%").Replace("%23", "#").Replace("%26", "&").Replace("%22", "'").Replace("%40", "@"));
        string Apiamount = "0";
        string ApiTotalPVamount = "0";
        string gst = "0";
        string netamount = "0";
        string billtype = "R";
        string kitid = "0";
        string TxnData = Request["TxnData"];
        try { Apiamount = Clean(Request["amount"]); }
        catch { }
        try { ApiTotalPVamount = Clean(Request["totalpv"]); }
        catch { }
        try { gst = Clean(Request["gst"]); }
        catch { }
        try { netamount = Clean(Request["netamount"]); }
        catch { }
        try { billtype = Clean(Request["billtype"]); }
        catch { }
        try { kitid = Clean(Request["kitid"]); }
        catch { }
        try
        {
            int LoginSuccess = 0;
            int FormNo = 0;
            int fromrankid = 0;

            Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());

            string str = Obj.IsoStart + "Select * FROM " + Obj.dBName + "..M_MemberMaster WHERE IDNo='" + Uname + "'" + Obj.IsoEnd;
            DataTable Dt_ = SqlHelper.ExecuteDataset(
                HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                CommandType.Text, str
            ).Tables[0];

            if (Dt_.Rows.Count > 0)
            {
                LoginSuccess = 1;
                FormNo = Convert.ToInt32(Dt_.Rows[0]["FormNo"]);
            }

            string[] TData = TxnData.Split(';');
            string TxnID = "";
            string Remarks = "";
            double Bv = 0;

            string fromidno = "";
            string fromtype = "";

            if (Session["CompID"].ToString() == "1093")
            {
                if (TData.Length == 5)
                {
                    TxnID = TData[0].Trim();
                    Bv = Convert.ToDouble(TData[1]);
                    Remarks = TData[2].Trim();
                    fromidno = TData[3].Trim();
                    fromtype = TData[4].Trim();
                    fromrankid = GetrankFormNoNew(fromidno);
                }
                else
                {
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"Insufficient Data.\"}");
                    return;
                }
            }
            else
            {
                if (TData.Length == 3)
                {
                    TxnID = TData[0].Trim();
                    Bv = Convert.ToDouble(TData[1]);
                    Remarks = TData[2].Trim();
                }
                else
                {
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"Insufficient Data.\"}");
                    return;
                }
            }

            if (Bv <= 0)
            {
                Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"BV must be greater than 0. Please enter a valid value.\"}");
                return;
            }

            if (LoginSuccess == 1)
            {
                string sqlTable =
                    "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TrnAddBV]') AND type = N'U') " +
                    "BEGIN CREATE TABLE [dbo].[TrnAddBV]([Id] INT IDENTITY(1,1) NOT NULL,[BillNo] NVARCHAR(250) NULL,[Billdate] DATETIME NULL CONSTRAINT DF_TrnAddBV_Billdate DEFAULT (GETDATE()),[BV] NUMERIC(18,2) NULL,[AddType] NVARCHAR(10) NULL,CONSTRAINT [PK_TrnAddBV] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80) ON [PRIMARY]) ON [PRIMARY] END";

                SqlHelper.ExecuteNonQuery(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(), CommandType.Text, sqlTable);

                string sqlCreateProc =
                    "IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Sp_TrnAddBV') " +
                    "EXEC('CREATE PROCEDURE Sp_TrnAddBV AS BEGIN SET NOCOUNT ON; END')";

                SqlHelper.ExecuteNonQuery(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(), CommandType.Text, sqlCreateProc);

                string sqlAlterProc =
                    "ALTER PROCEDURE Sp_TrnAddBV @BillNo NVARCHAR(250),@BV NUMERIC(18,2),@AddType NVARCHAR(10) " +
                    "AS BEGIN SET NOCOUNT ON; " +
                    "IF NOT EXISTS (SELECT 1 FROM TrnAddBV WHERE BillNo=@BillNo AND AddType=@AddType) BEGIN " +
                    "INSERT INTO TrnAddBV(BillNo,Billdate,BV,AddType) VALUES(@BillNo,GETDATE(),@BV,@AddType); SELECT SCOPE_IDENTITY() AS ID; " +
                    "END ELSE BEGIN SELECT 0 AS ID; END END";

                SqlHelper.ExecuteNonQuery(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(), CommandType.Text, sqlAlterProc);

                string strCheck = "Exec Sp_TrnAddBV '" + TxnID + "','" + Bv + "','CR'";
                DataTable dtCheck = SqlHelper.ExecuteDataset(
                    HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(),
                    CommandType.Text, strCheck).Tables[0];

                if (Convert.ToInt32(dtCheck.Rows[0]["ID"]) > 0)
                {
                    // *** ALL VB IF BLOCKS CONVERTED AS-IS ***
                    if (Session["CompID"].ToString() == "1083")
                    {
                        str = "INSERT INTO RepurchIncome (SessId,FormNo,BillNo,BillDate,RepurchIncome,Imported,BillType,SoldBy," +
                              "Msessid,KitID,Remarks,DSessID,PVValue,RecTimeStamp,RpValue,MRP,Prodid,ActiveStatus,Amount," +
                              "type,Entrytype,LegNo,UserId,BvStatus,BvStatusRTS,BvStatusRemarks,UpdateLegNo,UpdateSessID,UpdateDSessid,FromID) " +
                              "VALUES (" + Session["CurrentSessn"] + ",'" + FormNo + "','" + TxnID + "',CAST(Convert(varchar,Getdate(),106) AS DateTime)," +
                              "'" + Bv + "','N','R','WR','" + Session["MonthSessn"] + "',0,'" + Remarks + TxnID + "',Convert(Varchar,Getdate(),112),0,getdate(),0,0,0," +
                              "'N',0,'N','',0,'0','','','',0,0,0,0);" +
                              "Declare @RID int ; Select @RID = Max(RID) from RepurchIncome Where Formno = '" + FormNo + "';" +
                              "Exec InstantRPayout @RID;";
                    }
                    else if (Session["CompID"].ToString() == "1093")
                    {
                        str = "Exec Sp_TrncreditNew " + FormNo + "," + Bv + ",'" + Remarks + "','" + TxnID + "'," +
                              Session["CurrentSessn"] + "," + Session["MonthSessn"] + "," + fromrankid + ",'" + fromtype + "'";
                    }
                    else if (Session["CompID"].ToString() == "1095")
                    {
                        string BillType = "A";
                        decimal TotalBV = 0;

                        string Str_ = Obj.IsoStart + "SELECT ISNULL(SUM(repurchincome), 0) AS TotalBV FROM " + Obj.dBName + "..repurchincome WHERE formno = '" + FormNo + "'" + Obj.IsoEnd;
                        DataSet Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Str_);
                        DataTable dt = Ds.Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            TotalBV = Convert.ToDecimal(dt.Rows[0]["TotalBV"]);
                            BillType = TotalBV > 1000 ? "R" : "A";
                        }

                        str = "INSERT INTO RepurchIncome (SessID,FormNo,BillNo,BillDate,RepurchIncome,Imported,BillType,SoldBy,Msessid,Remarks,DSessID,Amount) VALUES (" +
                              Session["CurrentSessn"] + ",'" + FormNo + "','" + TxnID + "',CAST(Convert(varchar,Getdate(),106) AS DateTime),'" + Bv + "','N','" + BillType + "','WR'," +
                              "'" + Session["MonthSessn"] + "','" + Remarks + TxnID + "',Convert(Varchar,Getdate(),112),'" + Apiamount + "')";
                    }
                    else if (Session["CompID"].ToString() == "1106")
                    {
                        if (billtype.ToString() == "R")
                        {
                            kitid = "0";
                        }
                        str = "INSERT INTO RepurchIncome (SessID,FormNo,BillNo,BillDate,RepurchIncome,Imported,BillType,SoldBy,Msessid,Remarks,DSessID,Amount,kitid,PVValue,GST,NetAmount) VALUES (" +
                              Session["CurrentSessn"] + ",'" + FormNo + "','" + TxnID + "',CAST(Convert(varchar,Getdate(),106) AS DateTime),'" + Bv + "','N','" + billtype + "','WR'," +
                              "'" + Session["MonthSessn"] + "','" + Remarks + TxnID + "',Convert(Varchar,Getdate(),112),'" + Apiamount + "','" + kitid + "','" + ApiTotalPVamount + "','" + gst + "','" + netamount + "');";
                        if (Convert.ToDouble(ApiTotalPVamount) > 0)
                        {
                            str += " EXEC Sp_ActivateMember '" + Uname + "'";
                        }
                    }
                    else
                    {
                        if (billtype.ToString() == "R")
                        {
                            kitid = "0";
                        }
                        str = "INSERT INTO RepurchIncome (SessID,FormNo,BillNo,BillDate,RepurchIncome,Imported,BillType,SoldBy,Msessid,Remarks,DSessID,Amount,kitid) VALUES (" +
                              Session["CurrentSessn"] + ",'" + FormNo + "','" + TxnID + "',CAST(Convert(varchar,Getdate(),106) AS DateTime),'" + Bv + "','N','" + billtype + "','WR'," +
                              "'" + Session["MonthSessn"] + "','" + Remarks + TxnID + "',Convert(Varchar,Getdate(),112),'" + Apiamount + "','" + kitid + "');";

                        //if (Convert.ToInt32(kitid) > 0)
                        //{
                        //    str += " EXEC Sp_ActivateMember '" + Uname + "', '" + kitid + "'";
                        //}

                    }
                    int i = SqlHelper.ExecuteNonQuery(Session["MlmDatabase" + Session["CompID"]].ToString(), CommandType.Text, str);

                    if (i > 0)
                    {
                        string Rid = "";
                        string fromid = "";
                        string fromtype1 = "";

                        string str_Rid = Obj.IsoStart + "Select * FROM " + Obj.dBName + "..RepurchIncome WHERE FormNo = '" + FormNo + "' AND BillNo = '" + TxnID + "' order by rid desc" + Obj.IsoEnd;
                        DataTable Dt = SqlHelper.ExecuteDataset(
                            Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                            CommandType.Text, str_Rid).Tables[0];

                        if (Dt.Rows.Count > 0)
                        {
                            Rid = Dt.Rows[0]["Rid"].ToString();
                            if (Session["CompId"].ToString() == "1093")
                            {
                                fromid = Dt.Rows[0]["Fromemallid"].ToString();
                                fromtype1 = Dt.Rows[0]["fromtype"].ToString();
                            }
                        }

                        if (Session["CompId"].ToString() == "1093")
                        {
                            CallTestApi();
                            Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"success\",\"status\": \"SUCCESS\",\"voucherno\": \"" + Rid + "\",\"emallid\": \"" + fromid + "\",\"fromtype\": \"" + fromtype1 + "\"}");
                        }
                        else
                        {
                            Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"success\",\"status\": \"SUCCESS\",\"voucherno\": \"" + Rid + "\"}");
                        }
                    }
                    else
                    {
                        if (Session["CompId"].ToString() == "1093")
                        {
                            CallTestApi();
                            Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"FAILED\",\"status\": \"FAILED\",\"voucherno\": \"0\",\"emallid\": \"0\",\"fromtype\": \"\"}");
                        }
                        else
                        {
                            Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"FAILED\",\"status\": \"FAILED\",\"voucherno\": \"0\"}");
                        }
                    }
                }
                else
                {
                    Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"Order No. Already Exists\",\"status\": \"FAILED\",\"voucherno\": \"0\"}");
                }
            }
            else
            {
                Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"Login failed.\",\"status\": \"FAILED\",\"voucherno\": \"0\"}");
            }
        }
        catch
        {
            Response.Write("{\"loginid\": \"" + Uname + "\",\"msg\": \"Failed.\",\"status\": \"FAILED\",\"voucherno\": \"0\"}");
        }
    }
    public string CallTestApi()
    {
        string current_datetime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        int random_number = new Random().Next(0, 999);
        string formatted_datetime = current_datetime + random_number.ToString().PadLeft(3, '0');

        string sResult = formatted_datetime;
        string url = "https://sollywood.in/testapi.aspx";

        string sql_req =
            "INSERT INTO Tbl_ApiRequest_ResponseQrCode (ReqID, Request, postdata, CompID) " +
            "VALUES ('" + sResult.Trim() + "','" + url.Trim() + "', 'Sollywood Notification Send By Check Login','" +
            Session["CompID"] + "')";

        int x_Req = Convert.ToInt32(
            SqlHelper.ExecuteNonQuery(
                HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(),
                CommandType.Text,
                sql_req
            )
        );

        string responseText = "";

        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    responseText = reader.ReadToEnd();

                    string sql_res =
                        "UPDATE Tbl_ApiRequest_ResponseQrCode SET Response = '" +
                        responseText.Trim() +
                        "' WHERE ReqID = '" +
                        sResult.Trim() +
                        "'";

                    int x_res = Convert.ToInt32(
                        SqlHelper.ExecuteNonQuery(
                            HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(),
                            CommandType.Text,
                            sql_res
                        )
                    );
                }
            }
        }
        catch (Exception ex)
        {
            responseText = "Error: " + ex.Message;
        }

        return responseText;
    }
    private void DrBV()
    {
        objAccess = new DAL();

        string Uname = Clean(Request["Username"]);
        string Pwd = Clean(
            Request["Password"]
            .Replace("%25", "%").Replace("%23", "#")
            .Replace("%26", "&").Replace("%22", "'")
            .Replace("%40", "@")
        );

        string TxnData = Request["TxnData"];

        try
        {
            int LoginSuccess = 0;
            int FormNo = 0;
            int fromrankid = 0;

            Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());

            string str = Obj.IsoStart + "Select * FROM " + Obj.dBName + "..M_MemberMaster WHERE IDNo = '" + Uname + "'" + Obj.IsoEnd;
            DataTable Dt_ = SqlHelper.ExecuteDataset(
                HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                CommandType.Text,
                str
            ).Tables[0];

            if (Dt_.Rows.Count > 0)
            {
                LoginSuccess = 1;
                FormNo = Convert.ToInt32(Dt_.Rows[0]["FormNo"]);
            }

            string[] TData = TxnData.Split(';');
            string TxnID = "";
            string Remarks = "";
            double Bv = 0;

            string fromidno = "";
            string fromtype = "";

            if (Session["CompID"].ToString() == "1093")
            {
                if (TData.Length == 5)
                {
                    TxnID = TData[0].Trim();
                    Bv = Convert.ToDouble(TData[1]);
                    Remarks = TData[2].Trim();
                    fromidno = TData[3].Trim();
                    fromtype = TData[4].Trim();
                    fromrankid = GetrankFormNoNew(fromidno);
                }
                else
                {
                    Response.Write("{\"loginid\":\"" + Uname + "\",\"msg\":\"Insufficient Data.\"}");
                    return;
                }
            }
            else
            {
                if (TData.Length == 3)
                {
                    TxnID = TData[0].Trim();
                    Bv = Convert.ToDouble(TData[1]);
                    Remarks = TData[2].Trim();
                }
                else
                {
                    Response.Write("{\"loginid\":\"" + Uname + "\",\"msg\":\"Insufficient Data.\"}");
                    return;
                }
            }

            // Check already refunded
            string str_Rid1 = Obj.IsoStart + "Select sum(Repurchincome) As Amount FROM " + Obj.dBName + "..RepurchIncome WHERE FormNo = '" + FormNo + "' AND BillNo = '" + TxnID + "'" + Obj.IsoEnd;
            DataTable Dt1 = SqlHelper.ExecuteDataset(
                HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                CommandType.Text,
                str_Rid1
            ).Tables[0];

            if (Convert.ToDouble(Dt1.Rows[0]["Amount"]) == 0)
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"msg\":\"Same bill no already refunded Amount.\"}");
                return;
            }

            if (LoginSuccess == 1)
            {
                if (Conn.State == ConnectionState.Closed) Conn.Open();

                // 1083
                if (Session["CompID"].ToString() == "1083")
                {
                    str = "INSERT INTO RepurchIncome (SessId,FormNo,BillNo,BillDate,RepurchIncome,Imported,BillType,SoldBy," +
                          "Msessid,KitID,Remarks,DSessID,PVValue,RecTimeStamp,RpValue,MRP,Prodid,ActiveStatus,Amount," +
                          "type,Entrytype,LegNo,UserId,BvStatus,BvStatusRTS,BvStatusRemarks,UpdateLegNo,UpdateSessID,UpdateDSessid,FromID) " +
                          "VALUES (" + Session["CurrentSessn"] + ",'" + FormNo + "','" + TxnID + "',Cast(Convert(varchar,Getdate(),106) as DateTime)," +
                          "'" + (Bv - Bv * 2) + "','N','R','WR','" + Session["MonthSessn"] + "',0,'" + Remarks + TxnID + "',Convert(Varchar,Getdate(),112),0,getdate(),0,0,0," +
                          "'N',0,'N','',0,'0','','','',0,0,0,0)";
                }
                // 1093
                else if (Session["CompID"].ToString() == "1093")
                {
                    str = "INSERT RepurchIncome (SessID,FormNo,BillNo,BillDate,RepurchIncome,Imported,BillType,SoldBy,Msessid,Remarks,DSessID,ActiveStatus) VALUES (" +
                          Session["CurrentSessn"] + ",'" + FormNo + "','" + TxnID + "',Cast(Convert(varchar,Getdate(),106) as DateTime),'" + (Bv - Bv * 2) + "','N'," +
                          "'R','WR','" + Session["MonthSessn"] + "','" + Remarks + TxnID + "',Convert(Varchar,Getdate(),112),'Y')";
                }
                else
                {
                    str = "INSERT INTO RepurchIncome (SessID,FormNo,BillNo,BillDate,RepurchIncome,Imported,BillType,SoldBy,Msessid,Remarks,DSessID) VALUES (" +
                          Session["CurrentSessn"] + ",'" + FormNo + "','" + TxnID + "',Cast(Convert(varchar,Getdate(),106) as DateTime),'" + (Bv - Bv * 2) + "','N','R','WR','" +
                          Session["MonthSessn"] + "','" + Remarks + TxnID + "',Convert(Varchar,Getdate(),112))";
                }

                int i = SqlHelper.ExecuteNonQuery(
                    HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString(),
                    CommandType.Text,
                    str
                );

                if (i > 0)
                {
                    string Rid = "";
                    string str_Rid = Obj.IsoStart + "Select * FROM " + Obj.dBName + "..RepurchIncome WHERE FormNo = '" + FormNo + "' AND BillNo = '" + TxnID + "' order by rid desc" + Obj.IsoEnd;
                    DataTable Dt = SqlHelper.ExecuteDataset(
                        HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                        CommandType.Text,
                        str_Rid).Tables[0];

                    if (Dt.Rows.Count > 0)
                    {
                        Rid = Dt.Rows[0]["Rid"].ToString();
                    }

                    Response.Write("{\"loginid\":\"" + Uname + "\",\"msg\":\"success\",\"status\":\"SUCCESS\",\"voucherno\":\"" + Rid + "\"}");
                }
                else
                {
                    Response.Write("{\"loginid\":\"" + Uname + "\",\"msg\":\"FAILED\",\"status\":\"FAILED\",\"voucherno\":\"0\"}");
                }
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"msg\":\"Login failed.\",\"status\":\"FAILED\",\"voucherno\":\"0\"}");
            }
        }
        catch
        {
            Response.Write("{\"loginid\":\"" + Uname + "\",\"msg\":\"Failed.\",\"status\":\"FAILED\",\"voucherno\":\"0\"}");
        }
    }
    private int GetFormNo(string Uname, string Pwd)
    {
        int islog = 0;

        try
        {
            islog = Convert.ToInt32(
                Clean(Request["islog"])
            );
        }
        catch { }

        Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
        if (Conn.State == ConnectionState.Closed) Conn.Open();

        string str = "";

        if (islog == 1)
        {
            str = Obj.IsoStart + "Select * FROM " + Obj.dBName + "..M_MemberMaster WHERE IDNo='" + Uname + "' AND Passw='" + Pwd + "'" + Obj.IsoEnd;
        }
        else
        {
            
                str = Obj.IsoStart + "Select * FROM " + Obj.dBName + "..M_MemberMaster WHERE IDNo='" + Uname + "' AND EPassw='" + Pwd + "'" + Obj.IsoEnd;
            
        }

        Comm = new SqlCommand(str, Connselect);
        SqlDataReader Dr = Comm.ExecuteReader();

        int FormNo = 0;

        if (Dr.Read())
        {
            FormNo = Convert.ToInt32(Dr["FormNo"]);
        }

        Dr.Close();
        return FormNo;
    }
    private int GetFormNo_1(string Uname)
    {
        Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
        if (Conn.State == ConnectionState.Closed) Conn.Open();

        string str = Obj.IsoStart + "Select * FROM " + Obj.dBName + "..M_MemberMaster WHERE IDNo='" + Uname + "'" + Obj.IsoEnd;
        Comm = new SqlCommand(str, Connselect);

        SqlDataReader Dr = Comm.ExecuteReader();
        int FormNo = 0;

        if (Dr.Read())
        {
            FormNo = Convert.ToInt32(Dr["FormNo"]);
        }

        Dr.Close();
        return FormNo;
    }
    private int GetrankFormNo(string Uname)
    {
        Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
        if (Conn.State == ConnectionState.Closed) Conn.Open();

        string str = Obj.IsoStart + "Select * FROM " + Obj.dBName + "..M_MemberMaster WHERE Idno='" + Uname + "' and Planid=5" + Obj.IsoEnd;
        Comm = new SqlCommand(str, Connselect);

        SqlDataReader Dr = Comm.ExecuteReader();
        int FormNo = 0;

        if (Dr.Read())
        {
            FormNo = Convert.ToInt32(Dr["FormNo"]);

            Response.Write("{\"formno\": \"" + Dr["FormNo"] + "\",\"response\":\"success\"}");
        }
        else
        {
            Response.Write("{\"formno\": \"0\",\"response\":\"FAILED\"}");
        }

        Dr.Close();
        return FormNo;
    }
    private int GetrankFormNoNew(string Uname)
    {
        Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
        if (Conn.State == ConnectionState.Closed) Conn.Open();

        string str = Obj.IsoStart + "Select * FROM " + Obj.dBName + "..M_MemberMaster WHERE Idno='" + Uname + "' and Planid=5" + Obj.IsoEnd;
        Comm = new SqlCommand(str, Connselect);

        SqlDataReader Dr = Comm.ExecuteReader();
        int FormNo = 0;

        if (Dr.Read())
        {
            FormNo = Convert.ToInt32(Dr["FormNo"]);
            // No Response.Write here (same as VB)
        }

        Dr.Close();
        return FormNo;
    }
    private void checkEvoucher(string VoucherRequest, string VoucherResponse)
    {
        try
        {
            Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
            Conn.Open();

            double AvailBal = 0;
            int i = 0;

            if (VoucherResponse.ToUpper() == "OK")
            {
                int FormNo = GetFormNo(
                    Request["UserName"],
                    Request["Password"]
                        .Replace("%25", "%")
                        .Replace("%23", "#")
                        .Replace("%26", "&")
                        .Replace("%22", "'")
                        .Replace("%40", "@")
                );

                string updateQry =
                    "Update TrnVoucher Set Narration = Narration + '" + "-" + VoucherResponse +
                    "' where VoucherNo='" + VoucherRequest + "' and Drto='" + FormNo +
                    "' and Actype='" + Session["MWalletType"] + "'";

                Comm = new SqlCommand(updateQry, Conn);
                i = Comm.ExecuteNonQuery();

                if (FormNo > 0)
                {
                    AvailBal = Convert.ToDouble(GetMWalletBalance(FormNo.ToString()));
                }

                if (i > 0)
                {
                    Response.Write("{\"Login\":\"" + Request["UserName"] +
                                   "\",\"response\":\"OK\",\"ewallet\":\"" + AvailBal +
                                   "\",\"wallettype\":\"" + Session["MWalletType"] + "\"}");
                }
                else
                {
                    Response.Write("{\"Login\":\"" + Request["UserName"] +
                                   "\",\"response\":\"FAILED\",\"msg\":\"Invalid Login details\"}");
                }
            }
            else if (VoucherResponse.ToUpper() == "FAILED")
            {
                int FormNo = GetFormNo(
                    Request["UserName"],
                    Request["Password"]
                        .Replace("%25", "%")
                        .Replace("%23", "#")
                        .Replace("%26", "&")
                        .Replace("%22", "'")
                        .Replace("%40", "@")
                );

                string insertQry =
                    "Insert into TempTrnVoucher (VoucherId,VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration,Refno,Actype,RecTimeStamp,VType,Sessid,WSessid)" +
                    " select VoucherId,VoucherNo,VoucherDate,DrTo,Crto,Amount,Narration + '" + "-" + VoucherResponse +
                    "',RefNo,Actype,RecTimeStamp,VType,Sessid,WSessid from TrnVoucher where VoucherNo='" + VoucherRequest + "'";

                Comm = new SqlCommand(insertQry, Conn);
                Comm.ExecuteNonQuery();

                string deleteQry =
                    "Delete from TrnVoucher where VoucherNo='" + VoucherRequest +
                    "' and Drto='" + FormNo + "' and Actype='" + Session["MWalletType"] + "'";

                Comm = new SqlCommand(deleteQry, Conn);
                i = Comm.ExecuteNonQuery();

                if (FormNo > 0)
                {
                    AvailBal = Convert.ToDouble(GetMWalletBalance(FormNo.ToString()));
                }

                Response.Write("{\"Login\":\"" + Request["UserName"] +
                               "\",\"response\":\"FAILED\",\"msg\":\"Invalid Login details\"}");
            }

            Conn.Close();
        }
        catch
        {
            Response.Write("{\"response\":\"FAILED\",\"ewallet\":\"0\"}");
        }
    }
    private void DeductWallet() // 29Aug16, NJ
    {
        string VoucherNo = "";
        string Uname = Clean(Request["Username"]);
        string Pwd = Clean(
            Request["Password"]
            .Replace("%25", "%")
            .Replace("%23", "#")
            .Replace("%26", "&")
            .Replace("%22", "'")
            .Replace("%40", "@")
        );

        string TxnData = Request["TxnData"];

        try
        {
            int LoginSuccess = 0, FormNo = 0;

            Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
            if (Conn.State == ConnectionState.Closed) Conn.Open();

            FormNo = GetFormNo(Uname, Pwd);

            if (FormNo > 0)
            {
                LoginSuccess = 1;
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Invalid User\",\"wallettype\":\"" + Session["RWalletType"] + "\"}");
                return;
            }

            string[] TData = TxnData.Split(';');
            string TxnID = "", Remarks = "";
            double Bv = 0;

            if (TData.Length == 3)
            {
                TxnID = TData[0].Trim();
                Bv = Convert.ToDouble(TData[1]);
                Remarks = TData[2].Trim();
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Insufficient Data\",\"wallettype\":\"" + Session["RWalletType"] + "\"}");
                return;
            }

            double AvailBal = Convert.ToDouble(GetSWalletBalance(FormNo.ToString()));

            if (AvailBal < Bv && LoginSuccess == 1)
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Available Balance is " + AvailBal + "\",\"wallettype\":\"" + Session["RWalletType"] + "\"}");
                return;
            }
            else
            {
                LoginSuccess = 2;
            }

            if (LoginSuccess == 2)
            {
                if (Conn.State == ConnectionState.Closed) Conn.Open();

                if (Bv > 0)
                {
                    string sessn = "";
                    string q = Obj.IsoStart + "select Isnull(Max(Sessid),Convert(Varchar,Getdate(),112)) as Sessid from " + Obj.dBName + "..D_Sessnmaster" + Obj.IsoEnd;
                    Comm = new SqlCommand(q, Connselect);
                    SqlDataReader Dr = Comm.ExecuteReader();

                    if (Dr.Read())
                    {
                        sessn = Dr["SessId"].ToString();
                    }
                    Dr.Close();

                    string str =
                        "INSERT TrnVoucher (VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,Actype,VType,sessid,WSessID) " +
                        " Select ISNULL(MAX(VoucherNo)+1,1001),Cast(Convert(varchar,Getdate(),106) as DateTime),'" + FormNo + "','0','" + Bv +
                        "','R Wallet Used by " + Remarks + " against order no." + TxnID + "', '" + TxnID + "','" + Session["RWalletType"] +
                        "','D', convert(varchar,cast(cast(getdate() as varchar) as datetime),112),'" + Session["CurrentSessn"] + "' FROM TrnVoucher";

                    Comm = new SqlCommand(str, Conn);
                    Comm.ExecuteNonQuery();

                    string s = Obj.IsoStart + " select Max(VoucherNo) as VoucherNo from " + Obj.dBName + "..TrnVoucher " + Obj.IsoEnd;
                    Comm = new SqlCommand(s, Connselect);
                    Dr = Comm.ExecuteReader();

                    if (Dr.Read())
                    {
                        VoucherNo = Dr["VoucherNo"].ToString();
                    }
                    Dr.Close();

                    Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"OK\",\"deductamount\":\"" + Bv + "\",\"voucherno\":\"" + VoucherNo + "\",\"msg\":\"success\",\"wallettype\":\"" + Session["RWalletType"] + "\"}");
                }
                else
                {
                    Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Failed\",\"wallettype\":\"" + Session["RWalletType"] + "\"}");
                }
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Login failed\",\"wallettype\":\"" + Session["RWalletType"] + "\"}");
            }
        }
        catch
        {
            Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Failed\",\"wallettype\":\"" + Session["RWalletType"] + "\"}");
        }
    }
    private void AddWalletAmount(string WalletType) // 29Aug16, NJ
    {
        string VoucherNo = "";
        string Uname = Clean(
            Request["Username"]
            .Replace("%25", "%")
            .Replace("%23", "#")
            .Replace("%26", "&")
            .Replace("%22", "'")
            .Replace("%40", "@")
        );

        string TxnData = Request["TxnData"];

        try
        {
            int LoginSuccess = 0, FormNo = 0;

            Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
            if (Conn.State == ConnectionState.Closed) Conn.Open();

            FormNo = GetFormNo_1(Uname);
            string WalletName = GetWalletName(WalletType);

            if (FormNo > 0)
            {
                LoginSuccess = 1;
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Invalid User\",\"wallettype\":\"" + WalletType + "\"}");
                return;
            }

            string[] TData = TxnData.Split(';');
            string TxnID = "", Remarks = "";
            double Bv = 0;

            if (TData.Length == 3)
            {
                TxnID = TData[0].Trim();
                Bv = Convert.ToDouble(TData[1]);
                Remarks = TData[2].Trim();
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Amount Not Refunded\",\"wallettype\":\"" + WalletType + "\"}");
                return;
            }

            string str123 = Obj.IsoStart + "";
            str123 += "Select Isnull(SUM(Debit),0) As Debit ,  Isnull(SUM(credit),0) As Credit from ( ";
            str123 += " Select Amount As Debit, 0 As credit  from  " + Obj.dBName + "..TrnVoucher  ";
            str123 += " Where Drto = '" + FormNo + "' And RefNo = '" + TxnID + "' And VType = 'D'";
            str123 += " union All ";
            str123 += " Select 0 As Debit, Amount As credit from " + Obj.dBName + "..TrnVoucher ";
            str123 += " Where Crto = '" + FormNo + "' And RefNo = '" + TxnID + "' And VType = 'C' ) As RR" + Obj.IsoEnd;

            DataTable dt123 = SqlHelper.ExecuteDataset(
                HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                CommandType.Text,
                str123
            ).Tables[0];

            if (Convert.ToDouble(dt123.Rows[0]["Debit"]) > 0)
            {
                if (Convert.ToDouble(dt123.Rows[0]["Debit"]) < Convert.ToDouble(dt123.Rows[0]["credit"]) + Bv)
                {
                    Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Amount greater then Debit Amount.\",\"wallettype\":\"" + WalletType + "\"}");
                    return;
                }
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Order Number Not Exists.\",\"wallettype\":\"" + WalletType + "\"}");
                return;
            }

            LoginSuccess = 2;

            if (LoginSuccess == 2)
            {
                if (Conn.State == ConnectionState.Closed) Conn.Open();

                if (Bv > 0)
                {
                    string sessn = "";
                    string q = Obj.IsoStart + "select Isnull(Max(Sessid),Convert(Varchar,Getdate(),112)) as Sessid from " + Obj.dBName + "..D_Sessnmaster" + Obj.IsoEnd;
                    Comm = new SqlCommand(q, Connselect);
                    SqlDataReader Dr = Comm.ExecuteReader();

                    if (Dr.Read())
                    {
                        sessn = Dr["SessId"].ToString();
                    }
                    Dr.Close();

                    string str =
                        "INSERT TrnVoucher (VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,Actype,VType,sessid,WSessID) " +
                        " Select ISNULL(MAX(VoucherNo)+1,1001),Cast(Convert(varchar,Getdate(),106) as DateTime),'0','" + FormNo +
                        "','" + Bv + "','" + WalletName + "  Credited " + Remarks + " against tanscation no." + TxnID +
                        "', '" + TxnID + "','" + WalletType + "','C', convert(varchar,cast(cast(getdate() as varchar) as datetime),112),'" + sessn + "' FROM TrnVoucher";

                    Comm = new SqlCommand(str, Conn);
                    Comm.ExecuteNonQuery();

                    string s =
                        Obj.IsoStart + "select Max(VoucherNo) as VoucherNo from " + Obj.dBName + "..TrnVoucher Where Actype='" + WalletType +
                        "' And CrTo='" + FormNo + "' And Amount='" + Bv + "'" + Obj.IsoEnd;
                    Comm = new SqlCommand(s, Connselect);
                    Dr = Comm.ExecuteReader();

                    if (Dr.Read())
                    {
                        VoucherNo = Dr["VoucherNo"].ToString();
                    }
                    Dr.Close();

                    Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"OK\",\"refundamount\":\"" + Bv + "\",\"voucherno\":\"" + VoucherNo + "\",\"msg\":\"success\",\"wallettype\":\"" + WalletType + "\"}");
                }
                else
                {
                    Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Failed\",\"wallettype\":\"" + WalletType + "\"}");
                }
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Login failed\",\"wallettype\":\"" + WalletType + "\"}");
            }
        }
        catch
        {
            Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"refundamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Failed\",\"wallettype\":\"" + WalletType + "\"}");
        }
    }
    private double GetSWalletBalance(string FormNo)
    {
        try
        {
            double RtrVal = 0;

            Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
            Conn.Open();

            string query = Obj.IsoStart + "Select balance From dbo.ufnGetBalance('" + FormNo + "','" + Session["RWalletType"] + "')" + Obj.IsoEnd;
            Comm = new SqlCommand(query, Connselect);

            SqlDataReader Dr = Comm.ExecuteReader();
            if (Dr.Read())
            {
                RtrVal = Convert.ToDouble(Dr["balance"]);
            }

            Dr.Close();
            Comm.Cancel();
            Conn.Close();

            return RtrVal;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            return 0;
        }
    }
    private double GetMWalletBalance(string FormNo)
    {
        try
        {
            double RtrVal = 0;

            Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
            Conn.Open();

            string query = Obj.IsoStart + "Select balance From dbo.ufnGetBalance('" + FormNo + "','" + Session["MWalletType"] + "')" + Obj.IsoEnd;
            Comm = new SqlCommand(query, Connselect);

            SqlDataReader Dr = Comm.ExecuteReader();
            if (Dr.Read())
            {
                RtrVal = Convert.ToDouble(Dr["balance"]);
            }

            Dr.Close();
            Comm.Cancel();
            Conn.Close();

            return RtrVal;
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            return 0;
        }
    }
    private void DeductMainWallet()  // 29Aug16, NJ
    {
        string VoucherNo = "";
        string Uname = Clean(Request["Username"]);
        string Pwd = Clean(
            Request["Password"]
            .Replace("%25", "%")
            .Replace("%23", "#")
            .Replace("%26", "&")
            .Replace("%22", "'")
            .Replace("%40", "@")
        );

        string TxnData = Request["TxnData"];

        try
        {
            int LoginSuccess = 0, FormNo = 0;

            Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
            if (Conn.State == ConnectionState.Closed) Conn.Open();

            FormNo = GetFormNo(Uname, Pwd);

            if (FormNo > 0)
            {
                LoginSuccess = 1;
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Invalid User\"}");
                return;
            }

            string[] TData = TxnData.Split(';');
            string TxnID = "", Remarks = "";
            double Bv = 0;
            int i = 0;

            if (TData.Length == 3)
            {
                TxnID = TData[0].Trim();
                Bv = Convert.ToDouble(TData[1]);
                Remarks = TData[2].Trim();
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Insufficient Data\",\"wallettype\":\"" + Session["MWalletType"] + "\"}");
                return;
            }

            double AvailBal = Convert.ToDouble(GetMWalletBalance(FormNo.ToString()));

            if (AvailBal < Bv && LoginSuccess == 1)
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Available Balance is " + AvailBal + "\",\"wallettype\":\"" + Session["MWalletType"] + "\"}");
                return;
            }
            else
            {
                LoginSuccess = 2;
            }

            if (LoginSuccess == 2)
            {
                if (Conn.State == ConnectionState.Closed) Conn.Open();

                if (Bv > 0)
                {
                    string sessn = "";
                    string q = Obj.IsoStart + "select Isnull(Max(Sessid),Convert(Varchar,Getdate(),112)) as Sessid from " + Obj.dBName + "..D_Sessnmaster" + Obj.IsoEnd;
                    Comm = new SqlCommand(q, Connselect);
                    SqlDataReader Dr = Comm.ExecuteReader();

                    if (Dr.Read())
                    {
                        sessn = Dr["SessId"].ToString();
                    }
                    Dr.Close();

                    string str =
                        "INSERT TrnVoucher (VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,Actype,VType,sessid,WSessID) " +
                        " Select ISNULL(MAX(VoucherNo)+1,1001),Cast(Convert(varchar,Getdate(),106) as DateTime),'" + FormNo +
                        "','0','" + Bv + "','E Wallet Used by " + Remarks + " against order no." + TxnID + "', '" + TxnID +
                        "','" + Session["MWalletType"] + "','D', convert(varchar,cast(cast(getdate() as varchar) as datetime),112),'" +
                        Session["CurrentSessn"] + "' FROM TrnVoucher";

                    Comm = new SqlCommand(str, Conn);
                    i = Comm.ExecuteNonQuery();

                    if (i > 0)
                    {
                        string s = Obj.IsoStart + "select Max(VoucherNo) as VoucherNo from " + Obj.dBName + "..TrnVoucher " + Obj.IsoEnd;
                        Comm = new SqlCommand(s, Connselect);
                        Dr = Comm.ExecuteReader();

                        if (Dr.Read())
                        {
                            VoucherNo = Dr["VoucherNo"].ToString();
                        }
                        Dr.Close();

                        Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"OK\",\"deductamount\":\"" + Bv + "\",\"voucherno\":\"" + VoucherNo + "\",\"msg\":\"success\",\"wallettype\":\"" + Session["MWalletType"] + "\"}");
                    }
                    else
                    {
                        Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Failed\",\"wallettype\":\"" + Session["MWalletType"] + "\"}");
                    }
                }
                else
                {
                    Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Failed\",\"wallettype\":\"" + Session["MWalletType"] + "\"}");
                }
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Login failed\",\"wallettype\":\"" + Session["MWalletType"] + "\"}");
            }
        }
        catch
        {
            Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"deductamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Failed\",\"wallettype\":\"" + Session["MWalletType"] + "\"}");
        }
    }
    private string SaveIntoDB(
        string Referral,
        string Sponsor,
        string Side,
        string PinNo,
        string ScratchNo,
        string Name,
        string FName,
        string MemDOB,
        string Email,
        string Mobl,
        string _ReqAdrs,
        string _ReqStateCode,
        string _ReqDistrict,
        string _ReqCity,
        string _Reqpincode,
        string _Nominee,
        string _Relation,
        string _BankCode,
        string _PanNo,
        string _AccountNo,
        string _IFSC,
        string _Branch,
        string _Password)
    {
        string _Output = "";

        try
        {
            string strQry = "";
            string dblDistrict, dblTehsil, IfSC, dblPlan, JoinStatus, Category;
            int SessID, InVoiceNo, KitID;
            double Bv, Rp;
            string dblState, dblBank;
            char cGender = 'M';
            char cMarried = 'N';
            string HostIp = Context.Request.UserHostAddress.ToString();

            SqlDataReader DRead;
            string LastId = "";
            string _Response = "";

            try
            {
                // ----- VALIDATIONS -----

                if (string.IsNullOrWhiteSpace(_ReqCity))
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"city can not be blank\"}";
                    Response.Write(_Output);
                    return _Output;
                }

                if (_ReqStateCode == "0")
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"state can not be blank\"}";
                    Response.Write(_Output);
                    return _Output;
                }

                if (string.IsNullOrWhiteSpace(_Password))
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"password can not be blank\"}";
                    Response.Write(_Output);
                    return _Output;
                }

                if (string.IsNullOrWhiteSpace(Name))
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"name can not be blank\"}";
                    Response.Write(_Output);
                    return _Output;
                }

                if (string.IsNullOrWhiteSpace(Mobl))
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"mobile no can not be blank\"}";
                    Response.Write(_Output);
                    return _Output;
                }

                if (Mobl.Length != 10)
                {
                    _Output = "{\"response\":\"FAILED\",\"msg\":\"invalid mobile no\"}";
                    Response.Write(_Output);
                    return _Output;
                }

                // ---- Validate Referral/Sponsor/Side ----
                _Response = Validate_(Referral, Sponsor, Side);

                if (_Response == "OK")
                {
                    Bv = 0;
                    Rp = 0;
                    Category = "Registration";
                    KitID = 1;
                    JoinStatus = "N";

                    dblDistrict = _ReqDistrict;
                    dblTehsil = _ReqCity;
                    dblState = _ReqStateCode.ToString();
                    dblBank = _BankCode;
                    IfSC = _IFSC;
                    dblPlan = "0";
                    InVoiceNo = 0;

                    // ----------- Get Session ID -----------
                    Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
                    if (Conn.State == ConnectionState.Closed) Conn.Open();

                    Comm = new SqlCommand(Obj.IsoStart + "Select top 1 SessId as SessId from " + Obj.dBName + "..M_SessnMaster order by SessID desc" + Obj.IsoEnd, Connselect);
                    DRead = Comm.ExecuteReader();

                    if (DRead.Read())
                    {
                        SessID = Convert.ToInt32(DRead["SessID"]);
                    }
                    else
                    {
                        SessID = 0;
                    }

                    DRead.Close();
                    Comm.Cancel();

                    // ---- Parse DOB ----
                    DateTime Dtp;
                    try { Dtp = Convert.ToDateTime(MemDOB); }
                    catch { Dtp = DateTime.Now; }

                    string RandomNumber = _Password; // Using provided password

                    // ----- INSERT QUERY -----
                    strQry =
                        "Insert into M_MemberMaster (" +
                        "SessId,IdNo,CardNo,FormNo,KitId," +
                        "UpLnFormNo,RefId,LegNo,RefLegNo,RefFormNo,Prefix," +
                        "MemFirstName,MemLastName,MemRelation,MemFName,MemDOB,MemGender," +
                        "NomineeName,Address1,Address2,Post," +
                        "Tehsil,City,District,StateCode,CountryId," +
                        "PinCode,PhN1,Mobl,MarrgDate," +
                        "Passw,Doj,Relation,PanNo," +
                        "BankID,AcNo,IFSCode,EMail,BV," +
                        "UpGrdSessId,E_MainPassw,EPassw,ActiveStatus,billNo,RP,HostIp,BranchName,fld6," +
                        "PID,Paymode,ChDDNo,ChDDBankID,ChDDBank,ChddDate,ChDDBranch)";

                    strQry += " VALUES(" +
                        SessID + ",0,'" + PinNo + "'," +
                        "0," + KitID + "," + _UpLnFormNo + "," +
                        "0," + Side + ",0," +
                        _RefFormNo + ",'Mr.'," +
                        "'" + Name + "',''," +
                        "'C/O','" + FName + "','" + Dtp.ToString("dd-MMM-yyyy") + "','" + cGender + "'," +
                        "'" + _Nominee + "','" + _ReqAdrs + "','',''," +
                        "'" + dblTehsil + "','" + dblTehsil + "','" +
                        dblDistrict + "'," + dblState + ",1," +
                        "'" + _Reqpincode + "','0','" + Mobl + "'," +
                        "dbo.FormatDate(GetDate(),'dd-MMM-yyyy'),'" + RandomNumber + "',GetDate()," +
                        "'" + _Relation + "','" + _PanNo + "','" + dblBank + "','" + _AccountNo +
                        "','" + IfSC + "','" + Email + "'," +
                        Bv + ",0,'" + RandomNumber + "','" + RandomNumber + "','" + JoinStatus + "','" + InVoiceNo + "','" + Rp + "','" + HostIp + "','" + _Branch + "','App'," +
                        "'0','','0','0','','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','')";

                    if (Conn.State == ConnectionState.Closed) Conn.Open();

                    Comm = new SqlCommand(strQry, Conn);
                    Comm.CommandTimeout = 200000000;

                    int i = Comm.ExecuteNonQuery();
                    Comm.Cancel();

                    if (i != 0)
                    {
                        // Fetch newly created ID
                        string q = Obj.IsoStart + "SELECT TOP 1 a.IDNO,a.formno,b.IsBill,a.Passw FROM " + Obj.dBName + "..m_MemberMaster as a," + Obj.dBName + "..m_KitMaster as b " +
                                   "where a.kitid=b.kitid and a.fld6='App' AND a.KitID='" + KitID + "' AND MemFirstName='" + Name + "' ORDER BY mid DESC" + Obj.IsoEnd;

                        Comm = new SqlCommand(q, Connselect);
                        DRead = Comm.ExecuteReader();

                        if (DRead.Read())
                        {
                            LastId = DRead["IDNo"].ToString();
                            string _MSG = "Welcome To " + _Company + ", Thank You For Registration.Your ID Is " +
                                          DRead["IDNo"] + " and Password is " + DRead["Passw"] + ". Best of luck.";

                            sendSMS(_MSG, Mobl, LastId);
                        }

                        DRead.Close();
                        Comm.Cancel();

                        _Output = "{\"response\":\"OK\",\"idno\":\"" + LastId + "\",\"msg\":\"resgistration successfully\"}";
                    }
                    else
                    {
                        _Output = "{\"response\":\"FAILED\",\"idno\":\"\",\"msg\":\"resgistration failed\"}";
                    }
                }
                else
                {
                    _Output = "{\"response\":\"FAILED\",\"idno\":\"\",\"msg\":\"" + _Response + "\"}";
                }
            }
            catch (Exception e)
            {
                _Output = "{\"response\":\"FAILED\",\"idno\":\"\",\"msg\":\"" + e.Message + "\"}";
            }
        }
        catch (Exception ex)
        {
            _Output = "{\"response\":\"FAILED\",\"idno\":\"\",\"msg\":\"" + ex.Message + "\"}";
        }

        Response.Write(_Output);
        return _Output;
    }
    private void AddWalletAmount()   // 29Aug16, NJ
    {
        string VoucherNo = "";
        string Uname = Clean(Request["Username"]);
        string Pwd = Clean(
            Request["Password"]
                .Replace("%25", "%")
                .Replace("%23", "#")
                .Replace("%26", "&")
                .Replace("%22", "'")
                .Replace("%40", "@")
        );

        string TxnData = Request["TxnData"];

        try
        {
            int LoginSuccess = 0, FormNo = 0;

            Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
            if (Conn.State == ConnectionState.Closed) Conn.Open();

            FormNo = GetFormNo(Uname, Pwd);

            if (FormNo > 0)
            {
                LoginSuccess = 1;
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"addamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Invalid User\"}");
                return;
            }

            string[] TData = TxnData.Split(';');
            string TxnID = "", Remarks = "";
            double Bv = 0;

            if (TData.Length == 3)
            {
                TxnID = TData[0].Trim();
                Bv = Convert.ToDouble(TData[1]);
                Remarks = TData[2].Trim();
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"addamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Insufficient Data\"}");
                return;
            }

            if (Conn.State == ConnectionState.Closed) Conn.Open();

            if (Bv > 0)
            {
                string str =
                    "INSERT TrnVoucher (VoucherNo,VoucherDate,DrTo,CrTo,Amount,Narration,RefNo,Actype,VType,sessid,WSessID) " +
                    " Select ISNULL(MAX(VoucherNo)+1,1001),Cast(Convert(varchar,Getdate(),106) as DateTime),'0','" + FormNo + "','" + Bv + "'," +
                    " 'Shoppe Wallet Credited by " + Remarks + " against " + TxnID + "'," +
                    " '" + TxnID + "','R','C', convert(varchar,cast(cast(getdate() as varchar) as datetime),112)," +
                    " '" + Session["CurrentSessn"] + "' FROM TrnVoucher";

                Comm = new SqlCommand(str, Conn);
                Comm.ExecuteNonQuery();

                SqlDataReader Dr;
                string s = Obj.IsoStart + "select Max(VoucherNo) as VoucherNo from " + Obj.dBName + "..TrnVoucher" + Obj.IsoEnd;
                Comm = new SqlCommand(s, Connselect);
                Dr = Comm.ExecuteReader();

                if (Dr.Read())
                {
                    VoucherNo = Dr["VoucherNo"].ToString();
                }
                Dr.Close();

                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"OK\",\"addamount\":\"" + Bv + "\",\"voucherno\":\"" + VoucherNo + "\",\"msg\":\"success\"}");
            }
            else
            {
                Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"addamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Login failed\"}");
            }
        }
        catch
        {
            Response.Write("{\"loginid\":\"" + Uname + "\",\"response\":\"FAILED\",\"addamount\":\"0\",\"voucherno\":\"0\",\"msg\":\"Failed\"}");
        }
    }
    public string FillState()
    {
        string _output = "";

        try
        {
            SqlDataAdapter adp;
            DataTable Dt = new DataTable();

            Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
            Conn.Open();

            _output = "{\"states\": [";

            adp = new SqlDataAdapter(Obj.IsoStart + "Select * FROM " + Obj.dBName + "..M_stateDivMaster WHERE ActiveStatus='Y' order by StateName" + Obj.IsoEnd, Connselect);
            adp.Fill(Dt);

            if (Dt.Rows.Count > 0)
            {
                foreach (DataRow Dr in Dt.Rows)
                {
                    _output += "{\"statecode\":\"" + Dr["StateCode"] + "\",\"statename\":\"" + Dr["StateName"] + "\"},";
                }

                _output = _output.Remove(_output.Length - 1, 1);   // remove last comma
                _output += "],\"response\":\"OK\"}";
            }
            else
            {
                _output = "{\"response\":\"FAILED\",\"msg\":\"No State Found\"}";
            }
        }
        catch (Exception ex)
        {
            _output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        Response.Write(_output);
        return _output;
    }
    public string FillBank()
    {
        string _output = "";

        try
        {
            SqlDataAdapter adp;
            DataTable Dt = new DataTable();

            Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
            Conn.Open();

            _output = "{\"bank\": [";

            adp = new SqlDataAdapter(
                Obj.IsoStart + "SELECT BankCode as Bid, BANKNAME as Bank FROM " + Obj.dBName + "..M_BankMaster WHERE ACTIVESTATUS='Y' ORDER BY BANKNAME" + Obj.IsoEnd,
                Connselect
            );
            adp.Fill(Dt);

            if (Dt.Rows.Count > 0)
            {
                foreach (DataRow Dr in Dt.Rows)
                {
                    _output += "{\"bankcode\":\"" + Dr["Bid"] + "\",\"bankname\":\"" + Dr["Bank"] + "\"},";
                }

                _output = _output.Remove(_output.Length - 1, 1);
                _output += "],\"response\":\"OK\"}";
            }
            else
            {
                _output = "{\"response\":\"FAILED\",\"msg\":\"No Bank Found\"}";
            }
        }
        catch (Exception ex)
        {
            _output = "{\"response\":\"FAILED\",\"msg\":\"" + ex.Message + "\"}";
        }

        Response.Write(_output);
        return _output;
    }
    public string Validate_(string Referral, string Sponsor, string Side)
    {
        SqlDataReader Dread;

        Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
        Conn.Open();

        // -------------------- CHECK REFERRAL --------------------
        string q1 = Obj.IsoStart + "Select FormNo, MemFirstName + ' ' + MemLastName as MemName " +
                    "FROM " + Obj.dBName + "..M_MemberMaster " +
                    "WHERE IdNo='" + Referral + "' " +
                    "AND (Formno IN (Select FormnoDwn FROM " + Obj.dBName + "..M_MemTreeRelation) OR MID=1)" + Obj.IsoEnd;

        Comm = new SqlCommand(q1, Connselect);
        Dread = Comm.ExecuteReader();

        if (!Dread.Read())
        {
            return "Invalid Referral ID.";
        }

        _RefFormNo = Dread["FormNo"].ToString();
        Dread.Close();
        Comm.Cancel();

        // -------------------- READ CONFIG --------------------
        string _IsGetExtreme = "N";

        Comm = new SqlCommand(Obj.IsoStart + "SELECT * FROM " + Obj.dBName + "..M_ConfigMaster" + Obj.IsoEnd, Connselect);
        Dread = Comm.ExecuteReader();

        if (Dread.Read())
        {
            _IsGetExtreme = Dread["IsGetExtreme"].ToString();
        }

        Dread.Close();
        Comm.Cancel();

        // If extreme = N → validate full chain
        if (_IsGetExtreme == "N")
        {
            // -------------------- CHECK SPONSOR --------------------
            string q2 = Obj.IsoStart + "Select FormNo, MemFirstName + ' ' + MemLastName as MemName " +
                        "FROM " + Obj.dBName + "..M_MemberMaster WHERE IdNo='" + Sponsor + "'" + Obj.IsoEnd;

            Comm = new SqlCommand(q2, Connselect);
            Dread = Comm.ExecuteReader();

            if (!Dread.Read())
            {
                return "Invalid Sponsor ID.";
            }

            _UpLnFormNo = Dread["FormNo"].ToString();
            Dread.Close();
            Comm.Cancel();

            // -------------------- CHECK SIDE AVAILABLE --------------------
            string q3 =
                Obj.IsoStart + "SELECT COUNT(*) AS CNT " +
                "FROM " + Obj.dBName + "..M_MemberMaster " +
                "WHERE UpLnFormNo IN (Select FormNo FROM " + Obj.dBName + "..M_MemberMaster Where IDNo='" + Sponsor + "') " +
                "AND LegNo = " + Side + Obj.IsoEnd;

            Comm = new SqlCommand(q3, Connselect);
            Dread = Comm.ExecuteReader();

            if (!Dread.Read())
            {
                return "Selected Side Not Available.";
            }

            if (Convert.ToInt32(Dread["CNT"]) >= 1)
            {
                return "Selected Side Not Available.";
            }

            Dread.Close();
            Comm.Cancel();

            // -------------------- SPONSOR MUST BE IN REFERRAL DOWNLINE --------------------
            if (_RefFormNo != _UpLnFormNo)
            {
                string q4 =
                   Obj.IsoStart + "Select * FROM " + Obj.dBName + "..M_MemTreeRelation WHERE FormNo=" + _RefFormNo +
                    " And FormNoDwn=" + _UpLnFormNo + Obj.IsoEnd;

                Comm = new SqlCommand(q4, Connselect);
                Dread = Comm.ExecuteReader();

                if (!Dread.Read())
                {
                    return "Sponsor does not exist in referral downline.";
                }

                Dread.Close();
                Comm.Cancel();
            }
        }

        return "OK";
    }
    private void sendSMS(string MSG, string Mobl, string Idno)
    {
        SqlCommand cmd;
        SqlDataReader dread;

        cmd = new SqlCommand(
           Obj.IsoStart + "Select *, DATEADD(day, 15, Doj) as Maxdate from " + Obj.dBName + "..m_MemberMaster where IDNo = '" + Idno + "'" + Obj.IsoEnd,
            Connselect
        );

        dread = cmd.ExecuteReader();

        if (dread.Read())
        {
            Session["SMSIDNo"] = dread["IDNo"].ToString();
            Session["SMSIDPass"] = dread["Passw"].ToString();
            Session["Name"] = dread["MemFirstName"].ToString();
            Session["MaxDate"] = dread["MaxDate"].ToString();
        }

        dread.Close();

        if (Mobl.Length >= 10 && long.TryParse(Mobl, out _))
        {
            WebClient client = new WebClient();
            string baseurl;
            Stream data;

            string Sms =
                "Congratulations "
                + Session["Name"]
                + "! Welcome to "
                + Session["CompName"]
                + ". Your login details are ID: "
                + Session["SMSIDNo"]
                + "/Login Pwd:"
                + Session["SMSIDPass"]
                + "/Trans Pwd:"
                + Session["SMSIDPass"]
                + "Visit"
                + Session["CompWeb"]
                + "for more details.";

            try
            {
                baseurl =
                    "http://49.50.77.216/API/SMSHttp.aspx?UserId="
                    + Session["SmsId"]
                    + "&pwd="
                    + Session["SmsPass"]
                    + "&Message="
                    + Sms
                    + "&Contacts="
                    + Mobl
                    + "&SenderId="
                    + Session["ClientId"];

                data = client.OpenRead(baseurl);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                // VB code shows MsgBox, but in C# it is better to ignore or log
                // MessageBox.Show(ex.Message);
            }

            string sms1 =
                "Dear "
                + Session["Name"]
                + " ("
                + Session["SMSIDNo"]
                + "), Kindly submit scan copy of your Joining form, KYC, PAN copy and cancelled cheque on kyc@dreamtouchindia.com till "
                + Session["MaxDate"];

            try
            {
                baseurl =
                    "http://49.50.77.216/API/SMSHttp.aspx?UserId="
                    + Session["SmsId"]
                    + "&pwd="
                    + Session["SmsPass"]
                    + "&Message="
                    + sms1
                    + "&Contacts="
                    + Mobl
                    + "&SenderId="
                    + Session["ClientId"];

                data = client.OpenRead(baseurl);
                StreamReader reader = new StreamReader(data);
                string s = reader.ReadToEnd();
                data.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
            }
        }
    }



}
public class VoucherResponseModel
{
    public string Login { get; set; }
    public string response { get; set; }
    public object ewallet { get; set; }            // numeric or "0"
    public string wallettype { get; set; }
    public string msg { get; set; }
}

public class CheckInfoResponse
{
    // For single-object responses (CheckInfoDetail)
    public object data { get; set; }   // left as object so you can store table->JSON or list
    public string formno { get; set; }
    public string loginid { get; set; }
    public string name { get; set; }
    public string doj { get; set; }
    public string email { get; set; }
    public string mobileno { get; set; }
    public string city { get; set; }
    public string isactive { get; set; }
    public string kitid { get; set; }
    public string kitname { get; set; }
    public string kitstatus { get; set; }
    public string status { get; set; }
    public double rwallet { get; set; }
    public double ewallet { get; set; }
    public string ismovie { get; set; }
    public string activedate { get; set; }
    public string kitamount { get; set; }
    public string isholiday { get; set; }
    public string shoppoint { get; set; }
    public string promoid { get; set; }
    public string promovalue { get; set; }
    public string rcouponvalue { get; set; }
    public string rcoupon { get; set; }
    // other fields can be added as needed
}

public class OrderItem
{
    public string OrderNo { get; set; }
    public string kitid { get; set; }
    public string kitname { get; set; }
    public string purchasedate { get; set; }
    public string isusable { get; set; }
    public string isfranchise { get; set; }
    public string isholiday { get; set; }
    public string franKitamount { get; set; }
    public string franKitname { get; set; }
    public string FranKitid { get; set; }
    public string CouponMRP { get; set; }
    public string shoppoint { get; set; }
    public string promoid { get; set; }
    public string promovalue { get; set; }
    public string coupon { get; set; }
    public string couponamount { get; set; }
    public string hcount { get; set; }
    public string isfood { get; set; }
    public string isbigbazzar { get; set; }
    public string couponvalue { get; set; }
    public string RegistrationType { get; set; }
}
