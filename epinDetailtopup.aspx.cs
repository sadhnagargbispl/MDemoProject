using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class epinDetailtopup : System.Web.UI.Page
{
    cls_DataAccess dbConnect;
    SqlConnection Conn;
    SqlCommand Comm;
    SqlDataAdapter Adp;
    SqlDataReader dRead;
    DataSet Ds = new DataSet();
    DataTable dt = new DataTable();
    string StrQuery;
    string ScrName;
    DAL obj;
    SqlConnection Connselect;
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        FillDetail();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                obj = new DAL();
                Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
                Conn.Open();
                Connselect = new SqlConnection(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString());
                Connselect.Open();
                if (!Page.IsPostBack)
                {
                    HdnCheckTrnns.Value = GenerateRandomStringJoining(6);
                    Fillkit();
                    FillDetail();
                    divconfirm.Visible = false;
                }
            }
            else
            {
                Response.Redirect("logout.aspx");
            }
        }
        catch (Exception ex)
        {
            // dbConnect.closeConnection();
            // if (Conn.State == ConnectionState.Open)
            // {
            //     Conn.Close();
            // }
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
    private void Fillkit()
    {
        try
        {
            SqlParameter[] prms = new SqlParameter[1];
            prms[0] = new SqlParameter("@IDNO", Session["IDNO"]);

            Ds = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), "sp_GetKit", prms);

            CmbKit.DataSource = Ds.Tables[0];
            CmbKit.DataValueField = "KitID";
            CmbKit.DataTextField = "Kitname";
            CmbKit.DataBind();
        }
        catch (Exception ex)
        {
            // handle exception if needed
        }
    }
    private void FillDetail()
    {
        try
        {
            string Condition = "";

            if (Convert.ToInt32(CmbKit.SelectedValue) != 0)
            {
                Condition += " And KitID=" + CmbKit.SelectedValue;
            }

            if (rbtnStatus.SelectedValue == "USED")
            {
                Condition += " And [Status]='Used'";
            }
            else if (rbtnStatus.SelectedValue == "UN-USED")
            {
                Condition += " And [Status]='UnUsed'";
            }
            string StrQuery = "";
            StrQuery = obj.IsoStart + "Select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SNo,* From V#EpinStatus Where 1 = 1 AND ReqFormNo='" + Session["IDNo"] + "' " + Condition + " order by SNo Desc" + obj.IsoEnd;
            DataSet ds1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, StrQuery);
            DgPayment.CurrentPageIndex = 0;
            DgPayment.DataSource = ds1.Tables[0];
            DgPayment.DataBind();

            Session["epinData"] = ds1.Tables[0];
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
    protected void rbtnStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDetail();
    }
    protected void DgPayment_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            ClrCtrl();
            string sql;

            if (e.CommandArgument.ToString() == "Topup" ||
                e.CommandArgument.ToString() == "Join")
            {
                string PinNo = ((Label)e.Item.FindControl("lblCardNo")).Text;
                string ScratchNo = ((Label)e.Item.FindControl("lblScratchNo")).Text;
                string Kitid = ((Label)e.Item.FindControl("lblKitID")).Text;

                if (((Label)e.Item.FindControl("lblStatus")).Text == "UnUsed")
                {
                    if (e.CommandArgument.ToString() == "Topup")
                    {
                        if (((Label)e.Item.FindControl("IsTopup")).Text == "Y")
                        {
                            DivTopup.Visible = true;
                            lblPinNo.Text = PinNo;
                            TxtScratchNo.Text = ScratchNo;

                            sql = obj.IsoStart + "Select a.KitName,b.FormNo,b.ScratchNo,b.GeneratedBy," +
                                  "b.UsedBy,a.Allowtopup,b.ProdId,a.MACAdrs,a.TopUpSeq," +
                                  "a.KitAmount,a.KitId,0 as TravelPoint,a.RP " +
                                  "FROM " + obj.dBName + "..M_KitMaster as a," + obj.dBName + "..M_FormGeneration as b " +
                                  "WHERE a.KitID=b.ProdID AND b.FormNo='" + lblPinNo.Text.Trim() +
                                  "' AND a.Allowtopup='Y' and a.RowStatus='Y' " +
                            "AND b.Usedby='0' AND GeneratedBy='Y' " + obj.IsoEnd;
                            DataTable Dt_ = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, sql).Tables[0];
                            Session["NewKitName"] = Dt_.Rows[0]["KitName"];
                            Session["TopUpSeq"] = Dt_.Rows[0]["TopUpSeq"];
                            Session["MACAdrs"] = Dt_.Rows[0]["MACAdrs"];
                            Session["NewKitAmount"] = Dt_.Rows[0]["KitAmount"];
                            Session["NewKitId"] = Dt_.Rows[0]["KitId"];
                        }
                        else
                        {
                            ScrName = "<SCRIPT language='javascript'>alert('Invalid Topup Pin.');</SCRIPT>";
                            Page.RegisterStartupScript("MyAlert", ScrName);
                        }
                    }
                    else
                    {
                        Response.Redirect(
                            "NewJoining.aspx?pin=" + PinNo + "&scratch=" + ScratchNo,
                            false
                        );
                    }
                }
                else
                {
                    ScrName = "<SCRIPT language='javascript'>alert('ePin Already Used.');</SCRIPT>";
                    Page.RegisterStartupScript("MyAlert", ScrName);
                }
            }
        }
        catch (Exception ex)
        {
            // optional logging
        }
    }
    protected void DgPayment_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        DataGridItem dgi = e.Item;

        if (dgi.ItemType == ListItemType.Item ||
            dgi.ItemType == ListItemType.AlternatingItem)
        {
            Button btnRegister = (Button)e.Item.FindControl("btnRegister");
            Button btnTopup = (Button)e.Item.FindControl("btnTopup");

            string isTopup = ((Label)e.Item.FindControl("IsTopup")).Text;
            string status = ((Label)e.Item.FindControl("lblStatus")).Text;
            string kitId = ((Label)e.Item.FindControl("lblKitID")).Text;

            if (isTopup == "Y" && status == "UnUsed" && kitId != "7")
            {
                btnRegister.Visible = false;
                btnTopup.Visible = true;
            }
            else if (isTopup == "N" && status == "UnUsed" && kitId != "7")
            {
                btnRegister.Visible = true;
                btnTopup.Visible = false;
            }
            else if (kitId == "7" && status == "UnUsed")
            {
                btnRegister.Visible = true;
                btnTopup.Visible = true;
            }
            else
            {
                btnRegister.Visible = false;
                btnTopup.Visible = false;
            }
        }
    }
    protected void DgPayment_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        DgPayment.CurrentPageIndex = e.NewPageIndex;
        DgPayment.DataSource = Session["epinData"];
        DgPayment.DataBind();
    }
    protected void btnTopup_Click(object sender, EventArgs e)
    {

        if (IsValidEntry() == true)
        {
            trcommand.Visible = false;
            divconfirm.Visible = true;
        }
    }
    private void ClrCtrl()
    {
        errMsg.Text = "";
        TxtIDNo.Text = "";
        lblPinNo.Text = "";
        TxtScratchNo.Text = "";
        TxtIDNo1.Text = "";
        TxtName.Text = "";
        TxtPackage.Text = "";

        trcommand.Visible = true;
        divconfirm.Visible = false;
        DivTopup.Visible = false;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClrCtrl();
    }
    protected void TxtIDNo_TextChanged(object sender, EventArgs e)
    {
        IsValidID();
        // divconfirm.Visible = false;
    }
    private bool IsValidID()
    {
        try
        {
            DataTable dt;
            DataTable dt1;
            DataTable dt2 = new DataTable();
            string SponsorId = "";
            string MemberName = "";
            string Activestatus = "";
            int i;
            string LegNo = "";
            string Formno = "";
            bool BoolResult = false;
            errMsg.Text = "";
            TxtIDNo.Text = TxtIDNo.Text.Trim().Replace(";", "").Replace("'", "").Replace("=", "");
            string q = "";
            obj = new DAL();
            dt = new DataTable();
            string qr1 =
               obj.IsoStart + "Select a.Formno,a.activestatus," +
                "a.MemFirstName + ' ' + a.MemLastName as MemName," +
                "IsNull(c.Idno,'') as SponsorId," +
                "IsNull((c.MemFirstName+' '+c.MemLastname),'') as SponsorName," +
                "a.IsTopup ,a.KitId,b.MACAdrs,b.TopUpSeq,a.LegNo " +
                "from " + obj.dBName + "..M_KitMaster as b," + obj.dBName + "..M_MemberMaster as a " +
                "Left Join " + obj.dBName + "..M_MemberMaster as c on a.RefFormno=c.Formno " +
                "where a.KitId=b.KitId and (b.RowStatus='Y') " +
                "and a.IDNo='" + TxtIDNo.Text + "' " +
                "and a.IsBlock='N' and b.Joinamount!=3600" + obj.IsoEnd;
            dt2 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, qr1).Tables[0];
            if (dt2.Rows.Count > 0)
            {
                Formno = dt2.Rows[0]["Formno"].ToString();
                lblformno.Text = Formno;
                SponsorId = dt2.Rows[0]["SponsorId"].ToString();
                MemberName = dt2.Rows[0]["MemName"].ToString();
                Activestatus = dt2.Rows[0]["activestatus"].ToString();
                lblkitid.Text = dt2.Rows[0]["kitid"].ToString();
                obj = new DAL();
                dt = new DataTable();
                string str = obj.IsoStart + "Select * from " + obj.dBName + "..M_MemTreeRelation where formnodwn='" + Formno + "' Or Formno='" + Formno + "'" + obj.IsoEnd;
                dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (Session["IDNo"].ToString().ToUpper() != TxtIDNo.Text.ToUpper())
                    {
                        q =
                            obj.IsoStart + "Select * from " + obj.dBName + "..M_MemberMaster as a," + obj.dBName + "..M_MemTreeRelation as b " +
                            "where a.Formno=b.FormnoDwn and a.IdNo='" + TxtIDNo.Text + "'" + obj.IsoEnd;

                        obj = new DAL();
                        dt1 = new DataTable();
                        dt1 = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, q).Tables[0];
                        i = dt1.Rows.Count;
                    }
                    else
                    {
                        i = 1;
                    }

                    if (i > 0)
                    {
                        StrQuery =
                           obj.IsoStart + "Select a.Formno,a.MemFirstName + ' ' + a.MemLastName as MemName," +
                            "IsNull(c.Idno,'') as SponsorId," +
                            "(c.MemFirstName+' '+c.MemLastname) as SponsorName," +
                            "a.IsTopup ,a.KitId,b.MACAdrs,b.TopUpSeq,a.LegNo " +
                            "from " + obj.dBName + "..M_KitMaster as b," + obj.dBName + "..M_MemberMaster as a " +
                            "Left Join " + obj.dBName + "..M_MemberMaster as c on a.RefFormno=c.Formno " +
                            "where a.KitId=b.KitId and (b.RowStatus='Y') " +
                            "and a.IDNo='" + TxtIDNo.Text + "' and a.IsBlock='N'" + obj.IsoEnd;

                        // string StrResult = dbConnect.ExistOrNot(StrQuery).ToString();
                        dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, StrQuery).Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            RbtLeg.SelectedValue = dt.Rows[0]["LegNo"].ToString();
                            tdleg1.Visible = false;

                            BoolResult = true;

                            LblSponsor.Text = dt.Rows[0]["SponsorId"].ToString();
                            LblSponsor.ForeColor = System.Drawing.Color.Black;
                            LblSponserN.Visible = true;
                            Label1.Visible = true;
                            LblSponsor.Visible = true;

                            errMsg.Text = dt.Rows[0]["MemName"].ToString();
                            errMsg.ForeColor = System.Drawing.Color.Black;

                            btnTopup.Enabled = true;
                            if (Activestatus.ToString() == "Y")
                            {
                                errMsg.ForeColor = System.Drawing.Color.Red;
                                errMsg.Text = "Already Active.!";
                                btnTopup.Enabled = false;
                                LblSponserN.Visible = false;
                                Label1.Visible = false;
                                LblSponsor.Visible = false;
                                return false;
                            }
                            else
                            {
                                btnTopup.Enabled = true;
                                return true;
                            }

                        }

                    }
                    else
                    {
                        errMsg.Text = "Invalid ID!!";
                        btnTopup.Enabled = false;
                    }
                }
                else
                {
                    errMsg.ForeColor = System.Drawing.Color.Red;
                    TxtIDNo.Text = "";
                    LblSponserN.Visible = false;
                    Label1.Visible = false;
                    LblSponsor.Visible = false;
                    btnTopup.Enabled = false;
                    errMsg.Text = "Invalid Id!!";
                    btnTopup.Enabled = false;
                }
            }

            else
            {
                errMsg.Text = "Invalid ID.!";
                btnTopup.Enabled = false;
            }

            return BoolResult;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    private string ClearInject(string StrObj)
    {
        StrObj = StrObj.Replace(";", "")
                       .Replace("'", "")
                       .Replace("=", "");
        return StrObj;
    }
    protected bool Updtmaster()
    {
        try
        {
            string strQry, Result;
            SqlDataReader Dr;
            SqlDataReader Dr1;
            string TotalOrder = "0";
            string TotalQty = "0";
            string TotalAmount = "0";
            string Orderno = "";
            string formno = "";
            string Partycode = "";
            string Address = "";

            try
            {
                string deliveryAddress = "";
                string Deliverycenter = "";

                if (RbtDelivery.SelectedValue == "C" || RbtDelivery.SelectedValue == "S")
                {
                    deliveryAddress = TxtDeliveryAddress.Text;
                    Deliverycenter = RbtDelivery.SelectedItem.Text;
                }
                else
                {
                    deliveryAddress = "";
                    Deliverycenter = DDlDeliveryCenter.SelectedItem.Text;
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
                    Result = "Hello";

                    strQry = "Exec Sp_Activate '" +
                             TxtIDNo.Text.Replace("'", "").Replace("-", "") +
                             "'," + lblPinNo.Text;

                    Comm = new SqlCommand(strQry, Conn);
                    Dr = Comm.ExecuteReader();

                    if (Dr.Read())
                    {
                        Result = Dr["Result"].ToString();
                        Dr.Close();
                    }

                    double Kitamount = 0;

                    if (Result == "SUCCESS")
                    {
                        //ScrName = "<SCRIPT language='javascript'>alert('Successfuly TopUp');</SCRIPT>";
                        //Page.RegisterStartupScript("MyAlert", ScrName);
                        string url = "epinDetailtopup.aspx";
                        string script = "window.onload = function(){ alert('";
                        script += "Successfuly TopUp";
                        script += "');";
                        script += "window.location = '";
                        script += url;
                        script += "'; }";
                        ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                        Comm = new SqlCommand(
                            obj.IsoStart + "Select A.*,B.KitName,B.Kitamount " +
                            "from " + obj.dBName + "..m_MemberMaster As A," + obj.dBName + "..M_KitMaster As B " +
                            "where A.KitID=B.KitID And A.IDNo = '" + TxtIDNo.Text + "'" + obj.IsoEnd,
                            Connselect
                        );

                        dRead = Comm.ExecuteReader();

                        if (dRead.Read())
                        {
                            Kitamount = Convert.ToDouble(dRead["Kitamount"]);
                            Session["UPGRDID"] = dRead["IDNo"].ToString();
                            Session["UPGRDName"] = dRead["MemFirstName"].ToString();
                            Session["UPGRDMobileNo"] = dRead["Mobl"].ToString();
                            Session["UPGRDKit"] = dRead["KitName"].ToString();
                        }

                        dRead.Close();

                        string sms =
                            "Dear " + Session["UPGRDName"].ToString().Trim() +
                            ", Your id " + Session["UPGRDID"].ToString().Trim() +
                            " is successfully topup by " + Session["UPGRDKit"].ToString().Trim() +
                            " of Rs." + Kitamount.ToString().Trim() +
                            " %26 check your Package Wallet for Purchase Product. " +
                            "Best of luck, Regards https://stanvee.com/";

                        //sendSMS(TxtIDNo.Text, Orderno, sms);

                        return true;
                    }
                }
                else
                {
                    Response.Redirect("epinDetailtopup.Aspx");
                }
            }
            catch (Exception ex)
            {
                errMsg.Text = ex.Message + " Error In Updation";
                return false;
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }

        return false;
    }
    protected bool IsValidEntry()
    {
        try
        {
            bool result = false;

            string IDNo = TxtIDNo.Text.Trim()
                            .Replace("'", "")
                            .Replace("=", "")
                            .Replace(";", "");

            string PinNo = lblPinNo.Text.Trim()
                            .Replace("'", "")
                            .Replace("=", "")
                            .Replace(";", "");

            string ScratchNo = TxtScratchNo.Text.Trim()
                            .Replace("'", "")
                            .Replace("=", "")
                            .Replace(";", "");

            // Validate IdNo
            Comm = new SqlCommand(
               obj.IsoStart + "Select *, MemFirstName + ' ' + MemLastName as MemName " +
                "from " + obj.dBName + "..m_MemberMaster where IdNo = '" + IDNo + "'" + obj.IsoEnd,
               Connselect
            );

            dRead = Comm.ExecuteReader();

            if (!dRead.Read())
            {
                ScrName = "<SCRIPT language='javascript'>alert('Invalid IdNo.');</SCRIPT>";
                Page.RegisterStartupScript("MyAlert", ScrName);
                dRead.Close();
                return false;
            }
            else
            {
                TxtIDNo1.Text = dRead["IDNo"].ToString();
                TxtName.Text = dRead["MemName"].ToString();

                Session["PrevKitId"] = dRead["KitId"];
                Session["PrevPIN"] = dRead["CardNo"];
                Session["PrevStatus"] = dRead["ActiveStatus"];
            }

            dRead.Close();

            // Validate PIN & Scratch No
            Comm = new SqlCommand(
               obj.IsoStart + "Select km.Pv,km.kitid,km.KitName,JoinStatus " +
                "from " + obj.dBName + "..m_formgeneration sno " +
                "inner join " + obj.dBName + "..m_KitMaster km on km.kitid = sno.prodid " +
                "where FormNo = " + PinNo +
                " and ScratchNo = '" + ScratchNo + "'" +
                " and IsIssued = 'N' and AllowTopUp = 'Y' " + obj.IsoEnd,
                Connselect
            );

            dRead = Comm.ExecuteReader();

            if (!dRead.Read())
            {
                ScrName = "<SCRIPT language='javascript'>alert('Check PIN or ScratchNo.');</SCRIPT>";
                Page.RegisterStartupScript("MyAlert", ScrName);
                dRead.Close();
                return false;
            }
            else
            {
                TxtPackage.Text = dRead["KitName"].ToString();
                Session["KitId"] = dRead["KitId"];
                Session["JStatus"] = dRead["JoinStatus"];
                Session["BV"] = dRead["PV"];
            }

            dRead.Close();

            //StrQuery = "exec sp_Retopup " + lblformno.Text + "," + Session["KitId"];
            //string StrResult = dbConnect.ExistOrNot(StrQuery).ToString();
            //dt = obj.GetData(StrQuery);

            //if (dt.Rows.Count > 0)
            //{
            //    if (Convert.ToDateTime(dt.Rows[0]["Date_Topup"]) <
            //        Convert.ToDateTime(dt.Rows[0]["CurrentDate"]))
            //    {
            //        string billdate = dt.Rows[0]["Date_Topup"].ToString();
            //        ScrName = "<SCRIPT language='javascript'>alert('You can purchase this package after (" + billdate + ").');</SCRIPT>";
            //        Page.RegisterStartupScript("MyAlert", ScrName);
            //        DivTopup.Visible = false;
            //        return false;
            //    }
            //}

            result = true;
            return result;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    protected void BtnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidID())
            {
                if (IsValidEntry())
                {
                    if (Updtmaster())
                    {
                        ClrCtrl();
                        FillDetail();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // optional logging
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        TxtIDNo1.Text = "";
        TxtName.Text = "";
        TxtPackage.Text = "";
        divconfirm.Visible = false;
        trcommand.Visible = true;
    }

}