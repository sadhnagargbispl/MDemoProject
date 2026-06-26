using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI;
using Newtonsoft.Json;

public partial class UploadForm : Page
{
    private readonly string ApiUrl = "https://cpanel.makeandgrowth.com/ProcessAPIWithK.aspx";

    public Getkycres Userkycres = new Getkycres();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                LoadKycStatus();
            }
            else
            {
                Response.Redirect("logout.aspx");
            }
        }
    }

    private void LoadKycStatus()
    {
        try
        {
            Getkycreq req = new Getkycreq();

            req.islogin = "N";
            req.reqtype = "getkyc";
            req.userid = Convert.ToString(Session["IDNO"]);
            req.passwd = Convert.ToString(Session["MemPassw"]);

            string jsonreq =
                JsonConvert.SerializeObject(req);

            var response =
                CallPostFunction(
                    jsonreq,
                    "https://cpanel.makeandgrowth.com/ProcessAPIWithK.aspx"
                );

            Userkycres =
                JsonConvert.DeserializeObject<Getkycres>(response);
        }
        catch (Exception ex) { }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        string action = Request.QueryString["action"];

        if (action == "download")
            DownloadPdf();
        else if (action == "upload" && Request.HttpMethod == "POST")
            UserFormUpload();
    }

    // PDF Download
    private void DownloadPdf()
    {
        string filePath = Server.MapPath("~/SiteDoc/DIRECTSELLERCONTRACT.pdf");

        if (!File.Exists(filePath))
        {
            Response.StatusCode = 404;
            Response.Write("PDF file server par nahi mili.");
            Response.End();
            return;
        }

        string userId = Convert.ToString(Session["IDNO"]);
        string downloadName = userId + "_Direct_Seller_Agreement.pdf";

        Response.Clear();
        Response.ContentType = "application/pdf";
        Response.AddHeader("Content-Disposition",
            "attachment; filename=\"" + downloadName + "\"");
        Response.TransmitFile(filePath);
        Response.End();
    }

    // KYC PDF Upload (AJAX)
    private void UserFormUpload()
    {
        Response.Clear();
        Response.ContentType = "application/json";

        try
        {
            HttpPostedFile kycdoc = Request.Files["kycdoc"];

            if (kycdoc == null || kycdoc.ContentLength == 0)
            {
                WriteJson(false, "Please select PDF file.");
                return;
            }

            string ext = Path.GetExtension(kycdoc.FileName).ToLower();

            if (ext != ".pdf")
            {
                WriteJson(false, "Only PDF allowed.");
                return;
            }

            if (kycdoc.ContentLength > 5 * 1024 * 1024)
            {
                WriteJson(false, "PDF size 5MB se zyada nahi honi chahiye.");
                return;
            }

            string uploadPath = Server.MapPath("~/Uploads/KYC/");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            string uniqueName = Guid.NewGuid().ToString("N") + ".pdf";

            string fullPath = Path.Combine(uploadPath, uniqueName);

            kycdoc.SaveAs(fullPath);

            string fileUrl = "https://makeandgrowth.com/Uploads/KYC/" + uniqueName;

            // API REQUEST MODEL
            var requestData = new
            {
                islogin = "N",
                reqtype = "formupload",
                userid = Convert.ToString(Session["IDNO"]),
                passwd = Convert.ToString(Session["MemPassw"]),
                formupload = fileUrl
            };

            string detail = JsonConvert.SerializeObject(requestData);

            string response = CallPostFunction(detail, ApiUrl);

            dynamic output = JsonConvert.DeserializeObject(response);

            if (output.response == "OK")
            {
                WriteJson(true, "Form uploaded successfully.");
            }
            else
            {
                WriteJson(false, "Upload failed.");
            }
        }
        catch (Exception ex)
        {
            WriteJson(false, ex.Message);
        }

        Response.End();
    }


    private void WriteJson(bool success, string message)
    {
        Response.Write(JsonConvert.SerializeObject(new
        {
            success = success,
            message = message
        }));
    }
    // Apna existing CallPostFunction yahan paste karo
    public string CallPostFunction(string detail, string url)
    {
        try
        {
            // Create a request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(detail);
                streamWriter.Flush();
            }
            // Get the response
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    return result; // Optionally deserialize this JSON string to an object
                }
            }
        }
        catch (Exception ex)
        {
            var message = ex.Message;
        }
        return "";
    }
}
public class FormUploadRequest
{
    public string islogin { get; set; }
    public string reqtype { get; set; }
    public string userid { get; set; }
    public string passwd { get; set; }
    public string formupload { get; set; }
}

public class FormUploadResponse
{
    public string response { get; set; }
    public string msg { get; set; }
}

public class M_UserKYC
{
    public Getkycres Userkycres { get; set; }
    public List<State> states { get; set; }
    public List<BankList> BankLists { get; set; }
    public List<KycTypeMaster> kycTypeMasters { get; set; }
}

public class BankList
{
    public string BankName { get; set; }
    public decimal BankCode { get; set; }
}

public class KycTypeMaster
{
    public decimal Id { get; set; }
    public string IdType { get; set; }
}
public class KycSavereq
{
    public string islogin { get; set; }
    public string reqtype { get; set; }
    public string userid { get; set; }
    public string passwd { get; set; }
    public string address { get; set; }
    public string citycode { get; set; }
    public string cityname { get; set; }
    public string pincode { get; set; }
    public string statecode { get; set; }
    public string districtcode { get; set; }
    public string district { get; set; }
    public string areaname { get; set; }
    public string areacode { get; set; }
    public string idproofid { get; set; }
    public string idproofno { get; set; }
    public string frontaddressproof { get; set; }
    public string backaddressproof { get; set; }
    public string accounttype { get; set; }
    public string accountno { get; set; }
    public string bankcode { get; set; }
    public string bankname { get; set; }
    public string branchname { get; set; }
    public string ifsccode { get; set; }
    public string bankimage { get; set; }
    public string panno { get; set; }
    public string panimage { get; set; }
    public string formupload { get; set; }
}

public class KycSaveres
{
    public string response { get; set; }
    public string msg { get; set; }
}

public class Getkycreq
{
    public string islogin { get; set; }
    public string passwd { get; set; }
    public string reqtype { get; set; }
    public string userid { get; set; }
}
public class Getkycres
{
    public string idno { get; set; }
    public string addrsverf { get; set; }
    public string idverf { get; set; }
    public string rejectreason { get; set; }
    public string rejectremark { get; set; }
    public string vaerifydate { get; set; }
    public string isBankverified { get; set; }
    public string BankVerf { get; set; }
    public string BankRejectReason { get; set; }
    public string BankRejectRemark { get; set; }
    public string BankProofDate { get; set; }
    public string IsPanVerified { get; set; }
    public string PanVerf { get; set; }
    public string PanRejectReason { get; set; }
    public string PanRejectRemark { get; set; }
    public string PanVerifyDate { get; set; }
    public string formuploadVerf { get; set; }
    public string formuploadVerifyDate { get; set; }
    public Addressdetail addressdetail { get; set; }
    public Bankdetail bankdetail { get; set; }
    public Pandetail pandetail { get; set; }
    public Formuploaddetail formuploaddetail { get; set; }
    public string response { get; set; }
    public string msg { get; set; }
    public string Isformupload { get; set; }
}

public class Formuploaddetail
{
    public string formupload { get; set; }
}

public class Addressdetail
{
    public string idproof { get; set; }
    public string address { get; set; }
    public string pincode { get; set; }
    public string city { get; set; }
    public string district { get; set; }
    public string statecode { get; set; }
    public string statename { get; set; }
    public string addrproof { get; set; }
    public string IdproofNo { get; set; }
    public string backaddressproof { get; set; }
    public string backaddressdate { get; set; }
    public string idtype { get; set; }
    public string areacode { get; set; }
}

public class Bankdetail
{
    public string bankid { get; set; }
    public string acno { get; set; }
    public string ifscode { get; set; }
    public string accounttype { get; set; }
    public string branchname { get; set; }
    public string bankproof { get; set; }
}

public class Pandetail
{
    public string panno { get; set; }
    public string panimage { get; set; }
}
//using Newtonsoft.Json;
//using System;
//using System.Activities.Statements;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//public partial class UploadForm : System.Web.UI.Page
//{
//    public Getkycres Userkycres = new Getkycres();
//    protected void Page_Load(object sender, EventArgs e)
//    {
//        if (!Page.IsPostBack)
//        {
//            if (Session["CompID"] != null &&
//                Session["CompID"].ToString() == "1102")
//            {
//                Getkycreq req = new Getkycreq();

//                req.islogin = "N";
//                req.reqtype = "getkyc";
//                req.userid = Convert.ToString(Session["IDNO"]);
//                req.passwd = Convert.ToString(Session["MemPassw"]);

//                string jsonreq =
//                    JsonConvert.SerializeObject(req);

//                var response =
//                    CallPostFunction(
//                        jsonreq,
//                        "https://cpanel.makeandgrowth.com/ProcessAPIWithK.aspx"
//                    );

//                Userkycres =
//                    JsonConvert.DeserializeObject<Getkycres>(response);
//            }
//        }
//    }
//    public string CallPostFunction(string detail, string url)
//    {
//        try
//        {
//            // Create a request
//            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
//            request.Method = "POST";
//            request.ContentType = "application/json";
//            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
//            {
//                streamWriter.Write(detail);
//                streamWriter.Flush();
//            }
//            // Get the response
//            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
//            {
//                using (var streamReader = new StreamReader(response.GetResponseStream()))
//                {
//                    string result = streamReader.ReadToEnd();
//                    return result; // Optionally deserialize this JSON string to an object
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            var message = ex.Message;
//        }
//        return "";
//    }
//}

//public class M_UserKYC
//{
//    public Getkycres Userkycres { get; set; }
//    public List<State> states { get; set; }
//    public List<BankList> BankLists { get; set; }
//    public List<KycTypeMaster> kycTypeMasters { get; set; }
//}

//public class BankList
//{
//    public string BankName { get; set; }
//    public decimal BankCode { get; set; }
//}

//public class KycTypeMaster
//{
//    public decimal Id { get; set; }
//    public string IdType { get; set; }
//}
//public class KycSavereq
//{
//    public string islogin { get; set; }
//    public string reqtype { get; set; }
//    public string userid { get; set; }
//    public string passwd { get; set; }
//    public string address { get; set; }
//    public string citycode { get; set; }
//    public string cityname { get; set; }
//    public string pincode { get; set; }
//    public string statecode { get; set; }
//    public string districtcode { get; set; }
//    public string district { get; set; }
//    public string areaname { get; set; }
//    public string areacode { get; set; }
//    public string idproofid { get; set; }
//    public string idproofno { get; set; }
//    public string frontaddressproof { get; set; }
//    public string backaddressproof { get; set; }
//    public string accounttype { get; set; }
//    public string accountno { get; set; }
//    public string bankcode { get; set; }
//    public string bankname { get; set; }
//    public string branchname { get; set; }
//    public string ifsccode { get; set; }
//    public string bankimage { get; set; }
//    public string panno { get; set; }
//    public string panimage { get; set; }
//    public string formupload { get; set; }
//}

//public class KycSaveres
//{
//    public string response { get; set; }
//    public string msg { get; set; }
//}

//public class Getkycreq
//{
//    public string islogin { get; set; }
//    public string passwd { get; set; }
//    public string reqtype { get; set; }
//    public string userid { get; set; }
//}
//public class Getkycres
//{
//    public string idno { get; set; }
//    public string addrsverf { get; set; }
//    public string idverf { get; set; }
//    public string rejectreason { get; set; }
//    public string rejectremark { get; set; }
//    public string vaerifydate { get; set; }
//    public string isBankverified { get; set; }
//    public string BankVerf { get; set; }
//    public string BankRejectReason { get; set; }
//    public string BankRejectRemark { get; set; }
//    public string BankProofDate { get; set; }
//    public string IsPanVerified { get; set; }
//    public string PanVerf { get; set; }
//    public string PanRejectReason { get; set; }
//    public string PanRejectRemark { get; set; }
//    public string PanVerifyDate { get; set; }
//    public string formuploadVerf { get; set; }
//    public string formuploadVerifyDate { get; set; }
//    public Addressdetail addressdetail { get; set; }
//    public Bankdetail bankdetail { get; set; }
//    public Pandetail pandetail { get; set; }
//    public Formuploaddetail formuploaddetail { get; set; }
//    public string response { get; set; }
//    public string msg { get; set; }
//    public string Isformupload { get; set; }
//}

//public class Formuploaddetail
//{
//    public string formupload { get; set; }
//}

//public class Addressdetail
//{
//    public string idproof { get; set; }
//    public string address { get; set; }
//    public string pincode { get; set; }
//    public string city { get; set; }
//    public string district { get; set; }
//    public string statecode { get; set; }
//    public string statename { get; set; }
//    public string addrproof { get; set; }
//    public string IdproofNo { get; set; }
//    public string backaddressproof { get; set; }
//    public string backaddressdate { get; set; }
//    public string idtype { get; set; }
//    public string areacode { get; set; }
//}

//public class Bankdetail
//{
//    public string bankid { get; set; }
//    public string acno { get; set; }
//    public string ifscode { get; set; }
//    public string accounttype { get; set; }
//    public string branchname { get; set; }
//    public string bankproof { get; set; }
//}

//public class Pandetail
//{
//    public string panno { get; set; }
//    public string panimage { get; set; }
//}