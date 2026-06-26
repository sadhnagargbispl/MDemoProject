using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reply : System.Web.UI.Page
{
    DataTable Dt = new DataTable();
    string scrname;
    DAL objDal;
    cls_DataAccess DbConnect;
    string CIdQS;
    clsGeneral objGen = new clsGeneral();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Status"] != null && Session["Status"].ToString() == "OK")
        {
            objDal = new DAL();

            if (!string.IsNullOrEmpty(Request["CId"]))
            {
                CIdQS = Request["CId"];

                if (!Page.IsPostBack)
                {
                    if (!string.IsNullOrEmpty(Request["CId"]))
                    {
                        BindData();
                    }
                }
            }      
        }
        else
        {
            scrname = "<SCRIPT language='javascript'> window.top.location.reload();</SCRIPT>";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Close", scrname, false);
        }
        
    }
    private void BindData()
    {
        CIdQS = Request["CId"];

        string sql = objDal.IsoStart + " Select M.IDNo,M.MemName,ISNULL(Replace(CONVERT(varchar,M.RecTimeStamp,106),' ','-'),'') as CDate," +
                     " M.CType,M.Complaint ,ISNULL(S.Solution,'') as Solution," +
                     " ISNULL(Replace(CONVERT(varchar,S.RecTimeStamp,106),' ','-'),'') as SDate FROM" +
                     " (Select b.MemFirstName +' '+ b.MemLastName as MemName,a.*" +
                     "  FROM " + objDal.dBName + "..M_ComplaintMaster as a," + objDal.dBName + "..M_MemberMaster as b WHERE a.IDNo=b.IDNo AND a.CID='" + CIdQS + "') as M" +
                     "  LEFT JOIN " + objDal.dBName + "..M_SolutionMaster as S ON M.CID=S.CID " + objDal.IsoEnd;

        Dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];

        if (Dt.Rows.Count > 0)
        {
            LblCType.Text = Dt.Rows[0]["CType"].ToString();
            TxtComplaint.Text = Dt.Rows[0]["Complaint"].ToString();
            TxtPreReply.Text = "";

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                if (TxtPreReply.Text != "")
                {
                    TxtPreReply.Text += Environment.NewLine + "-----------------------------------------" + Environment.NewLine;
                }

                if (!string.IsNullOrWhiteSpace(Dt.Rows[i]["Solution"].ToString()))
                {
                    TxtPreReply.Text += Dt.Rows[i]["SDate"].ToString() + ": " +
                                        Environment.NewLine + Dt.Rows[i]["Solution"].ToString();
                }
            }
        }
    }

    private void ClearAll()
    {
        LblCType.Text = "";
        TxtComplaint.Text = "";
        TxtPreReply.Text = "";
    }
}