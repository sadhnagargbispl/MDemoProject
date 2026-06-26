using DocumentFormat.OpenXml.Office2016.Drawing.Charts;
using Newtonsoft.Json;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class SitePage : System.Web.UI.MasterPage
{
    DAL Obj;
    clsGeneral objGen = new clsGeneral();
    string compId;
    DataTable Dt = new DataTable();
    string Strq = string.Empty;
    //public Getkycres Userkycres = new Getkycres();
    public Getkycres Userkycres = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                if (Session["CompID"] != null &&
                Session["CompID"].ToString() == "1102")
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
                DAL Obj = new DAL();
                compId = Session["CompId"].ToString();
                HtmlLink link = new HtmlLink();
                string folderName = Convert.ToString(HttpContext.Current.Session["MlmDatabaseName" + Session["CompId"]]);
                link.Href = "assets/" + folderName + "/admin-color.css";
                link.Attributes["rel"] = "stylesheet";
                link.Attributes["type"] = "text/css";
                Page.Header.Controls.Add(link);
                 Strq = Obj.IsoStart + "Select idno,formno,Isblock from " + Obj.dBName + "..M_MembeRMaster where Formno='" + Session["Formno"] + "'" + Obj.IsoEnd;      
                Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Strq).Tables[0];
                if (Dt != null && Dt.Rows.Count > 0)
                {
                    if (Dt.Rows[0]["Isblock"].ToString() == "Y")
                    {
                        Response.Redirect("Default.aspx");
                    }
                }
                // CompId 1010 specific block
                if (compId == "1010")
                {
                    
                    if (Session["Status"] != null && Session["Status"].ToString() == "OK")
                    {
                         Strq = Obj.IsoStart + "Select Case when Profilepic='' then 'images/no_photo.jpg' else  profilepic end as profilepic,idno,formno,Panno," +
                                      "MemFirstName as MemberName, Case when ActiveStatus='Y' then Replace(Convert(Varchar,UpgradeDate,106),' ','-') else 'Deactive' " +
                                      "end as ActivationDate from " + Obj.dBName + "..M_MembeRMaster where Formno='" + Session["Formno"] + "'" + Obj.IsoEnd;
                        Dt = new DataTable();
                        Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Strq).Tables[0];
                        if (Dt != null && Dt.Rows.Count > 0)
                        {
                            LblUSerID.Text = Dt.Rows[0]["Idno"].ToString();
                            LblName.Text = Dt.Rows[0]["Membername"].ToString();
                        }
                    }
                }

                // Logo handling
                if (Session["compid"]?.ToString() == "1089")
                {
                    imgLogo.Src = Session["Logo"]?.ToString();
                    LOGOHREF.HRef = "https://megamart.ai/";
                }
                else
                {
                    imgLogo.Src = Session["Logo"].ToString();
                }

                // The block executed only on first load
                if (!Page.IsPostBack)
                {
                    if (Session["Status"] != null && Session["Status"].ToString() == "OK")
                    {
                        Strq = Obj.IsoStart + "Select case when ProfilePic = '' then 'https://www.iconpacks.net/icons/2/free-user-icon-3296-thumb.png' else mlmurl + ProfilePic end as ProfilePic,a.idno,a.formno,a.Panno," +
                                  "a.MemFirstName as MemberName, Case when a.ActiveStatus='Y' then Replace(Convert(Varchar,a.UpgradeDate,106),' ','-') else 'Deactive' end " +
                                  "as ActivationDate,b.Kitname from " + Obj.dBName + "..m_companymaster as c," + Obj.dBName + "..M_MembeRMaster as a inner join " + Obj.dBName + "..M_KitMaster as b on a.kitid=b.kitid where Formno='" + Session["Formno"] + "'" + Obj.IsoEnd;
                        Dt = new DataTable();
                        Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, Strq).Tables[0];
                        if (Dt != null && Dt.Rows.Count > 0)
                        {
                            if (Convert.ToInt32(Session["Run"] ?? 0) == 0)
                            {
                                LblId.Text = Dt.Rows[0]["Idno"].ToString();
                                LblUSerID.Text = Dt.Rows[0]["Idno"].ToString();
                                LblName.Text = Dt.Rows[0]["Membername"].ToString();
                                datej.Text = Dt.Rows[0]["ActivationDate"].ToString();
                                Image2.ImageUrl = Dt.Rows[0]["profilepic"].ToString();
                            }
                        }
                    }
                }
            }
            //else
            //{
            //    Response.Redirect("logout.aspx");
            //}
           
        }
        catch (Exception)
        {
            // swallow exception as original code did; consider logging here
        }
    }
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
    private string Encryptdata(string Data)
    {

        string strmsg = string.Empty;
        try
        {
            byte[] encode = new byte[Data.Length];
            encode = Encoding.UTF8.GetBytes(Data);
            strmsg = Convert.ToBase64String(encode);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return strmsg;
    }
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