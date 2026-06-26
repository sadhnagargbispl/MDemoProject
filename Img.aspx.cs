using System;
using System.Data;
using System.Web;
using System.Web.UI;

public partial class Img : System.Web.UI.Page
{
    public string FormNo;
    clsGeneral objGen = new clsGeneral();

    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt;
        DAL obj = new DAL();

        string type = Request["Type"];
        FormNo = Request["ID"];

        int idVal = 0;
        int.TryParse(Convert.ToString(Request["ID"]), out idVal);

        if (string.Equals(type, "ad", StringComparison.OrdinalIgnoreCase))
        {
            string sql = obj.IsoStart + "select '" + Convert.ToString(Session["AdminWeb"]) + "/images/UploadImage/' + ImgPath as Img1Path from " + obj.dBName + "..M_AdvertiseMaster where AdID='" + idVal.ToString() + "'" + obj.IsoEnd;
            dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];
            if (dt.Rows.Count > 0)
                Image1.ImageUrl = dt.Rows[0]["Img1Path"].ToString();
        }
        else if (string.Equals(type, "Glry", StringComparison.OrdinalIgnoreCase))
        {
            string sql = obj.IsoStart + "select '" + Convert.ToString(Session["AdminWeb"]) + "/images/UploadImage/' + ImagePath as ImgPath from " + obj.dBName + "..ProductGallery where PGID='" + idVal.ToString() + "'" + obj.IsoEnd;
            dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];
            if (dt.Rows.Count > 0)
                Image1.ImageUrl = dt.Rows[0]["ImgPath"].ToString();
        }
        else if (string.Equals(type, "PinRequest", StringComparison.OrdinalIgnoreCase))
        {
            string sql = obj.IsoStart + "select Case when ImgPath='' then '' when ImgPath like'http%' then ImgPath " +
                         " else '" + Convert.ToString(Session["CompWeb"]) + "/images/UploadImage/'+ ImgPath end as  ImagePath from " + obj.dBName + "..TrnPinReqMain where ReqNo='" + idVal.ToString() + "'" + obj.IsoEnd;
            dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];
            if (dt.Rows.Count > 0)
                Image1.ImageUrl = dt.Rows[0]["ImagePath"].ToString();
        }
        else if (string.Equals(type, "Payment", StringComparison.OrdinalIgnoreCase))
        {
            string sql = obj.IsoStart +
            "select Case when ScannedFile='' then '' " +
            "else MlmUrl+'images/UploadImage/'+'" + Session["compid"] + "/' + ScannedFile end as ImagePath " +
            "from " + obj.dBName + "..WalletReq as a," +
            obj.dBName + "..m_companymaster as b " +
            "where ReqNo='" + idVal.ToString() + "'" +
            obj.IsoEnd;

             dt = SqlHelper.ExecuteDataset(
                HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(),
                CommandType.Text,
                sql
            ).Tables[0];

            if (dt.Rows.Count == 0)
            {
                Response.Write("No record found");
                return;
            }
            string imgPath = dt.Rows[0]["ImagePath"].ToString();
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Write(imgPath);
            Response.End();
        //string sql = obj.IsoStart + "select Case when ScannedFile='' then ''  " +
        //                   " else MlmUrl+'images/UploadImage/'+'" + Session["compid"] + "/' + ScannedFile end as  ImagePath from " + obj.dBName + "..WalletReq as a," + obj.dBName + "..m_companymaster as b  where ReqNo='" + idVal.ToString() + "'" + obj.IsoEnd;
        //dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];
        //if (dt.Rows.Count > 0)
        //    Image1.ImageUrl = dt.Rows[0]["ImagePath"].ToString();
    }
        else if (string.Equals(type, "walletpayment", StringComparison.OrdinalIgnoreCase))
        {
            string sql = obj.IsoStart + "select CASE WHEN ScannedFile='' THEN '' WHEN ScannedFile like 'http%' THEN ScannedFile else 'https://cpanel.solfit.in/images/UploadImage/'+ScannedFile" +
                         " end as  ImagePath from " + obj.dBName + "..WalletReq where ReqNo='" + idVal.ToString() + "'" + obj.IsoEnd;
            dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];
            if (dt.Rows.Count > 0)
                Image1.ImageUrl = dt.Rows[0]["ImagePath"].ToString();
        }
        else if (string.Equals(type, "booking", StringComparison.OrdinalIgnoreCase))
        {
            string sql = obj.IsoStart + "select Case when ScanneFile='' then ''  " +
                         " else '" + Convert.ToString(Session["CompWeb"]) + "images/UploadImage/'+ ScanneFile end as  ImagePath from " + obj.dBName + "..BookingRequest where ReqNo='" + idVal.ToString() + "'" + obj.IsoEnd;
            dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];
            if (dt.Rows.Count > 0)
                Image1.ImageUrl = dt.Rows[0]["ImagePath"].ToString();
        }
        else if (string.Equals(type, "Invoice", StringComparison.OrdinalIgnoreCase))
        {
            // Request("Reqid") in VB; handle similarly
            int reqId = 0;
            int.TryParse(Convert.ToString(Request["Reqid"]), out reqId);

            string sql = obj.IsoStart + "select Invoiceurl as  ImagePath from " + obj.dBName + "..Invoice where Formno='" + idVal.ToString() + "' and Id='" + reqId.ToString() + "'" + obj.IsoEnd;
            dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];
            if (dt.Rows.Count > 0)
                Image1.ImageUrl = dt.Rows[0]["ImagePath"].ToString();
        }
        else if (string.Equals(type, "EventF", StringComparison.OrdinalIgnoreCase))
        {
            DataTable Dt_EventF = new DataTable();
            string sql = " exec Sp_EventImageShow '" + Convert.ToString(Session["CompWeb"]) + "','" + Request["ID"] + "' ";
            Dt_EventF = SqlHelper.ExecuteDataset((string)HttpContext.Current.Session["MlmDatabase" + Convert.ToString(Session["CompID"])], CommandType.Text, sql).Tables[0];
            if (Dt_EventF.Rows.Count > 0)
                Image1.ImageUrl = Dt_EventF.Rows[0]["ImagePath"].ToString();
        }
        else if (string.Equals(type, "EventB", StringComparison.OrdinalIgnoreCase))
        {
            DataTable Dt_EventB = new DataTable();
            string sql = " exec Sp_EventImageShow1 '" + Convert.ToString(Session["CompWeb"]) + "','" + Request["ID"] + "' ";
            Dt_EventB = SqlHelper.ExecuteDataset((string)HttpContext.Current.Session["MlmDatabase" + Convert.ToString(Session["CompID"])], CommandType.Text, sql).Tables[0];
            if (Dt_EventB.Rows.Count > 0)
                Image1.ImageUrl = Dt_EventB.Rows[0]["ImagePath"].ToString();
        }
        else if (type != null)
        {
            // intentionally left blank to mirror original VB branch
        }
        else
        {
            // Image1.ImageUrl = "ImgHandler.ashx?id=" + Request["ID"];
        }
    }
}
