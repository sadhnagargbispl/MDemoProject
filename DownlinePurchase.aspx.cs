using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DownlinePurchase : System.Web.UI.Page
{
    SqlConnection Conn;
    SqlCommand Comm;
    SqlDataAdapter Ad;
    DataTable dt;
    DAL obj;
    clsGeneral objGen = new clsGeneral();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                obj = new DAL();
                Conn = new SqlConnection(HttpContext.Current.Session["MlmDatabase" + Session["CompID"]].ToString());
                Conn.Open();
                if (!IsPostBack)
                {
                    if (Session["CompID"].ToString() == "1107")
                    {
                        Label2.Text = "Total EV : ";
                    }
                    else
                    {
                        Label2.Text = "Total BV : ";
                    }

                    RbtLegNo.Items[1].Text = "Left";
                    RbtLegNo.Items[2].Text = "Right";
                    Filldate();
                    FillLevel();
                    DdlLevel.SelectedValue = "0";
                    LevelDetail();
                    FillLevel_Ra();
                }
            }
            else
            {
                Response.Redirect("logout.aspx");
            }
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    }
    private void Filldate()
    {
        try
        {
            DAL objDAL = new DAL();
            string str = "Select Replace(Convert(Varchar,Getdate(),106),' ','-') as CurrentDate";
            DataTable dtData = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];
        }
        catch { }
    }
    protected void FillLevel()
    {
        try
        {
            string str =
               obj.IsoStart + "Select distinct * from (" +
                "Select 0 As MLevel,'-- ALL --' As LevelName " +
                "Union ALL select MLevel,'Level :' + convert(varchar,MLevel) " +
                "from " + obj.dBName + "..R_MemTreeRelation with(nolock) where FormNo='" + Session["FormNo"] + "') Temp order by MLevel" + obj.IsoEnd;
            dt = new DataTable();
            dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];
            DdlLevel.DataSource = dt;
            DdlLevel.DataTextField = "LevelName";
            DdlLevel.DataValueField = "MLevel";
            DdlLevel.DataBind();
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    }
    protected void LevelDetail()
    {
        string compid = Session["CompID"].ToString();
        try
        {

            string str = "";
            string condition = "";
            if (RbtProduct.SelectedValue == "R")
            {
                if ((compid == "1007") | (compid == "1010") | (compid == "1091") | (compid == "1095") | (compid == "1097" | compid == "1100" | compid == "1103"))
                {
                    if (RbtLegNo.SelectedValue != "0")
                        condition = condition + " and d.LegNo='" + RbtLegNo.SelectedValue + "'";
                }
                else if (Session["CompID"].ToString() == "1108" || (compid == "1109") || (compid == "1102") | (compid == "1107") | (compid == "1110"))
                {
                    if (RbtLegNo.SelectedValue != "0")
                        condition = condition + " and a.LegNo='" + RbtLegNo.SelectedValue + "'";
                }
                else if (Convert.ToInt32(DdlLevel.SelectedValue) > 0)
                    condition = condition + "AND d.MLevel='" + DdlLevel.SelectedValue + "'";
            }
            else if (RbtLegNo.SelectedValue != "0")
                condition = condition + " and d.LegNo='" + RbtLegNo.SelectedValue + "'";

            string scrname = "";
            string FrmDate = "";
            string ToDate = "";
            FrmDate = TxtFromDate.Text;
            ToDate = TxtToDate.Text;
            if (FrmDate != "")
            {
                try
                {
                    DateTime Dt = Convert.ToDateTime(FrmDate);
                }
                catch (Exception ex)
                {
                    scrname = "<SCRIPT language='javascript'>alert('Check Start Date.. ');" + "</SCRIPT>";
                    this.RegisterStartupScript("MyAlert", scrname);
                    return;
                }
            }
            if (ToDate != "")
            {
                try
                {
                    DateTime Dt = Convert.ToDateTime(ToDate);
                }
                catch (Exception ex)
                {
                    scrname = "<SCRIPT language='javascript'>alert('Check End Date.. ');" + "</SCRIPT>";
                    this.RegisterStartupScript("MyAlert", scrname);
                    return;
                }
            }
            if (FrmDate != "" & ToDate != "")
                condition = condition + " And Cast(Convert(Varchar,b.BillDate,106)as Date)>='" + FrmDate + "' And Cast(Convert(Varchar,b.BillDate,106)as Date)<='" + ToDate + "'";

            if (compid == "1010" | compid == "1103")
            {
                if (TxtFromDate.Text != "")
                    FrmDate = TxtFromDate.Text;
                if (TxtToDate.Text != "")
                    ToDate = TxtToDate.Text;
            }
            if (RbtProduct.SelectedValue == "R")
            {
                if ((compid == "1007"))
                {
                    str = obj.IsoStart + "  select Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date] ,a.Idno  as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As  ";
                    str += "  [Member Name],  Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name],";
                    str += "  Isnull(Isnull(c.OrderAmt,s.NetPayable),Ds.NetPayable) as [Bill Amount],b.Pvvalue as BV  ";
                    str += "  from " + obj.dBName + "..M_MemberMaster as a with(nolock) inner Join  " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn ";
                    str += "  Inner Join " + obj.dBName + "..RepurchIncome as b with(nolock) on  a.Formno=b.Formno  ";
                    str += " Left Join " + obj.dBName + "..TrnOrder as c with(nolock)  On   b.Formno=c.formno  AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo ";
                    str += "  Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s with(nolock) On  b.Formno=s.Formno  and s.BillNo=b.BillNo";
                    str += "  Left join " + HttpContext.Current.Session["InvDatabase" + compid] + "..DeletedBillMain  As Ds On  b.Formno=Ds.Formno  and ds.BillNo=b.BillNo";
                    str += "  where  d.Formno='" + Session["Formno"] + "'";
                    str += " " + condition + " and  a.Formno=b.Formno and b.BillType in ('R','O')";
                    str += " Order by b.rectimestamp " + obj.IsoEnd;
                }
                else if (Session["CompID"].ToString() == "1108" || (compid == "1010" | compid == "1103" | compid == "1105" | compid == "1107" | compid == "1109" | compid == "1110"))
                {
                    str = obj.IsoStart + " exec Sp_DowmLineRepurchaseReport '" + Session["Formno"] + "','" + FrmDate + "','" + ToDate + "' " + obj.IsoEnd;
                }
                else if (compid == "1074")
                {
                    str = obj.IsoStart + "  select Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date] ,a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As  ";
                    str += "  [Member Name],  Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name],";
                    str += "  Isnull(Isnull(c.OrderAmt,s.NetPayable),Ds.NetPayable) as [Bill Amount],b.RepurchIncome as [Repurchase PV]  ";
                    str += "  from " + obj.dBName + "..M_MemberMaster as a with(nolock) inner Join  " + obj.dBName + "..R_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn ";
                    str += "  Inner Join " + obj.dBName + "..RepurchIncome as b with(nolock) on  a.Formno=b.Formno  ";
                    str += " Left Join " + obj.dBName + "..TrnOrder as c with(nolock)  On   b.Formno=c.formno  AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo ";
                    str += "  Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s with(nolock) On  b.Formno=s.Formno  and s.BillNo=b.BillNo";
                    str += "  Left join " + HttpContext.Current.Session["InvDatabase" + compid] + "..DeletedBillMain  As Ds On  b.Formno=Ds.Formno  and ds.BillNo=b.BillNo";
                    str += "  where  d.Formno='" + Session["Formno"] + "'";
                    str += " " + condition + " and  a.Formno=b.Formno and b.BillType in ('R','O')";
                    str += " Order by b.rectimestamp " + obj.IsoEnd;
                }
                else if (compid == "1091")
                {
                    str = obj.IsoStart + "  select Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date] ,a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As  ";
                    str += "  [Distributor Name],  Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name],";
                    str += "  Isnull(Isnull(c.OrderAmt,s.NetPayable),Ds.NetPayable) as [Bill Amount],b.RepurchIncome as [Repurchase BV]  ";
                    str += "  from " + obj.dBName + "..M_MemberMaster as a with(nolock) inner Join  " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn ";
                    str += "  Inner Join " + obj.dBName + "..RepurchIncome as b with(nolock) on  a.Formno=b.Formno  ";
                    str += " Left Join " + obj.dBName + "..TrnOrder as c with(nolock)  On   b.Formno=c.formno  AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo ";
                    str += "  Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s with(nolock) On  b.Formno=s.Formno  and s.BillNo=b.BillNo";
                    str += "  Left join " + HttpContext.Current.Session["InvDatabase" + compid] + "..DeletedBillMain  As Ds On  b.Formno=Ds.Formno  and ds.BillNo=b.BillNo";
                    str += "  where  d.Formno='" + Session["Formno"] + "'";
                    str += " " + condition + " and  a.Formno=b.Formno and b.BillType in ('R','O')";
                    str += " Order by b.rectimestamp " + obj.IsoEnd;
                }
                else if (compid == "1095")
                {
                    str = obj.IsoStart + "  select Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date] ,a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As  ";
                    str += "  [Member Name],  Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name],";
                    str += "  Isnull(Isnull(c.OrderAmt,s.NetPayable),Ds.NetPayable) as [Bill Amount],b.RepurchIncome as BV  ";
                    str += "  from " + obj.dBName + "..M_MemberMaster as a with(nolock) inner Join  " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn ";
                    str += "  Inner Join " + obj.dBName + "..RepurchIncome as b with(nolock) on  a.Formno=b.Formno  ";
                    str += " Left Join " + obj.dBName + "..TrnOrder as c with(nolock)  On   b.Formno=c.formno  AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo ";
                    str += "  Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s with(nolock) On  b.Formno=s.Formno  and s.BillNo=b.BillNo";
                    str += "  Left join " + HttpContext.Current.Session["InvDatabase" + compid] + "..DeletedBillMain  As Ds On  b.Formno=Ds.Formno  and ds.BillNo=b.BillNo";
                    str += "  where  d.Formno='" + Session["Formno"] + "'";
                    str += " " + condition + " and  a.Formno=b.Formno and b.BillType in ('R','O')";
                    str += " Order by b.rectimestamp " + obj.IsoEnd;
                }
                else if (compid == "1097" | compid == "1100")
                {
                    str = obj.IsoStart + "  select Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date] ,a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As  ";
                    str += "  [Member Name],  Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name],";
                    str += "  Isnull(Isnull(c.OrderAmt,s.NetPayable),Ds.NetPayable) as [Bill Amount],b.RepurchIncome as BV  ";
                    str += "  from " + obj.dBName + "..M_MemberMaster as a with(nolock) inner Join  " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn ";
                    str += "  Inner Join " + obj.dBName + "..RepurchIncome as b with(nolock) on  a.Formno=b.Formno  ";
                    str += " Left Join " + obj.dBName + "..TrnOrder as c with(nolock)  On   b.Formno=c.formno  AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo ";
                    str += "  Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s with(nolock) On  b.Formno=s.Formno  and s.BillNo=b.BillNo";
                    str += "  Left join " + HttpContext.Current.Session["InvDatabase" + compid] + "..DeletedBillMain  As Ds On  b.Formno=Ds.Formno  and ds.BillNo=b.BillNo";
                    str += "  where  d.Formno='" + Session["Formno"] + "'";
                    str += " " + condition + " and  a.Formno=b.Formno and b.BillType in ('R','O')";
                    str += " Order by b.rectimestamp " + obj.IsoEnd;
                }
                else
                {
                    str = obj.IsoStart + "  select Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date] ,a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As  ";
                    str += "  [Member Name],  Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name],";
                    str += " b.RepurchIncome as BV ";
                    str += "  from " + obj.dBName + "..M_MemberMaster as a with(nolock) inner Join  " + obj.dBName + "..R_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn ";
                    str += "  Inner Join " + obj.dBName + "..RepurchIncome as b with(nolock) on  a.Formno=b.Formno  ";
                    str += " Left Join " + obj.dBName + "..TrnOrder as c with(nolock)  On   b.Formno=c.formno  AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo ";
                    str += "  Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s with(nolock) On  b.Formno=s.Formno  and s.BillNo=b.BillNo";
                    str += "  Left join " + HttpContext.Current.Session["InvDatabase" + compid] + "..DeletedBillMain  As Ds On  b.Formno=Ds.Formno  and ds.BillNo=b.BillNo";
                    str += "  where  d.Formno='" + Session["Formno"] + "'";
                    str += " " + condition + " and  a.Formno=b.Formno and b.BillType in ('R','O')";
                    str += " Order by b.rectimestamp " + obj.IsoEnd;
                }
            }
            else if ((compid == "1007"))
            {
                str = obj.IsoStart + "select a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], " + " Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name]," +
                    "Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],  " + "isnull(Isnull(Isnull(c.OrderAmt,s.NetPayable),ds.Netpayable),k.KitAmount) as [Amount]," +
                    "" + "isnull(k.KitName,'') as [Package Name],B.PVValue as BV  " +
                    "from  " + obj.dBName + "..M_MemberMaster as a with(nolock) " + " Inner Join " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn  ," +
                    "" + " " + obj.dBName + "..RepurchIncome as b with(nolock)  Left Join " + obj.dBName + "..TrnOrder as c with(nolock)   On  " + " c.Formno=b.Formno  " +
                    "AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo " + " " +
                    "Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno " + " and s.BillNo=b.BillNo 	" +
                    "Left join " + HttpContext.Current.Session["InvDatabase" + compid] + "..DeletedBillMain  As Ds" + " On  b.Formno=Ds.Formno  and ds.BillNo=b.BillNo	" +
                    "Left Join " + obj.dBName + "..M_KitMaster as k with(nolock)on  k.RowStatus='Y' and b.Kitid=k.KitId where d.Formno='" + Session["Formno"] + "'  " + condition + " " +
                    "and " + " a.Formno=b.Formno and b.BillType<>'R'     Order by b.rectimestamp" + obj.IsoEnd;
            }
            else if ((compid == "1010"))
            {
                str += obj.IsoStart + " Select  a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], ";
                str += " Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name],";
                str += " Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],";
                str += "   Case when b.kitid=0 then RoyaltyNmWise  else kitname end  as  [Package Name],Case when b.kitid=0 then b.amount  else isnull(kitamount,0) end as Amount,Repurchincome as BV,isnull(royalty,0) as [Royalty BV]  ";
                str += " from  " + obj.dBName + "..M_MemberMaster as a with(nolock) ";
                str += " Inner Join " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn ";
                str += " Inner join " + obj.dBName + "..RepurchIncome b  on d.FormnoDwn  = b.Formno And  RBV in ('N','Y')  and b.BillType<>'R' 	" +
                    " left Join " + obj.dBName + "..M_KitMaster as k with(nolock)on b.Kitid=k.KitId  And  k.RowStatus='Y' ";
                str += " Where  d.Formno='" + Session["Formno"] + "'  " + condition + " " + obj.IsoEnd;
            }
            else if (compid == "1108" || compid == "1109" || compid == "1103" | compid == "1105" | compid == "1110")
            {
                str += obj.IsoStart + " Select  a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], ";
                str += " Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name],";
                str += " Replace(Convert(Varchar,b.Rectimestamp,106),' ','-')+' '+CONVERT(varchar(15),CAST(b.Rectimestamp AS TIME),22) as [Bill Date],";
                str += " kitname as [Package Name],Case when b.kitid=0 then b.amount  else isnull(kitamount,0) end as Amount,Repurchincome as BV  ";
                str += " from  " + obj.dBName + "..M_MemberMaster as a with(nolock) ";
                str += " Inner Join " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn ";
                str += " Inner join " + obj.dBName + "..RepurchIncome b  on d.FormnoDwn  = b.Formno and b.BillType<>'R' 	 left Join " + obj.dBName + "..M_KitMaster as k with(nolock)on b.Kitid=k.KitId  And  k.RowStatus='Y' ";
                str += " Where  d.Formno='" + Session["Formno"] + "'  " + condition + " order by [Bill Date]  desc   " + obj.IsoEnd;
            }
            else if (Session["CompID"].ToString() == "1108" || compid == "1107" || compid == "1109" | compid == "1110")
            {
                str += obj.IsoStart + " Select  a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], ";
                str += " Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name],";
                str += " Replace(Convert(Varchar,b.Rectimestamp,106),' ','-')+' '+CONVERT(varchar(15),CAST(b.Rectimestamp AS TIME),22) as [Bill Date],";
                str += " kitname as [Package Name],Case when b.kitid=0 then b.amount  else isnull(kitamount,0) end as Amount,Repurchincome as EV  ";
                str += " from  " + obj.dBName + "..M_MemberMaster as a with(nolock) ";
                str += " Inner Join " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn ";
                str += " Inner join " + obj.dBName + "..RepurchIncome b  on d.FormnoDwn  = b.Formno and b.BillType<>'R' 	 left Join " + obj.dBName + "..M_KitMaster as k with(nolock)on b.Kitid=k.KitId  And  k.RowStatus='Y' ";
                str += " Where  d.Formno='" + Session["Formno"] + "'  " + condition + " order by [Bill Date]  desc   " + obj.IsoEnd;
            }
            else if (compid == "1074")
            {
                str = obj.IsoStart + "  select a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name]," + " Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name]," +
                    "Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date], " + " Isnull(Isnull(c.OrderAmt,s.NetPayable),k.KitAmount) as [Amount],k.KitName as [Package Name]," +
                    "B.Repurchincome as PV  from " + " " + obj.dBName + "..M_MemberMaster as a with(nolock) " +
                    "Inner Join " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn " + " " +
                    "inner Join " + obj.dBName + "..M_KitMaster as k with(nolock)on  k.RowStatus='Y'," + " " + obj.dBName + "..RepurchIncome as b with(nolock) " +
                    "Left Join " + obj.dBName + "..TrnOrder as c with(nolock)   On   " + " c.Formno=b.Formno   " +
                    "AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo   " +
                    "Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno " + " and s.BillNo=b.BillNo " +
                    " where  b.Kitid=k.KitId and d.Formno='" + Session["Formno"] + "'  " + condition + " and " + " a.Formno=b.Formno and b.BillType<>'R' Order by b.rectimestamp" + obj.IsoEnd;
            }
            else if (compid == "1091")
            {
                str = obj.IsoStart + "  select a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As [Distributor Name]," + " Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name]," +
                    "Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date], " + " Isnull(Isnull(c.OrderAmt,s.NetPayable),k.KitAmount) as [Amount],k.KitName as [Package Name]," +
                    "B.Repurchincome as BV  from " + " " + obj.dBName + "..M_MemberMaster as a with(nolock) " +
                    "Inner Join " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn " + " " +
                    "inner Join " + obj.dBName + "..M_KitMaster as k with(nolock)on  k.RowStatus='Y'," + " " +
                    " " + obj.dBName + "..RepurchIncome as b with(nolock) Left Join " + obj.dBName + "..TrnOrder as c with(nolock)   On   " + " c.Formno=b.Formno   " +
                    "AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo  " +
                    " Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno " + " " +
                    "and s.BillNo=b.BillNo  where  b.Kitid=k.KitId and d.Formno='" + Session["Formno"] + "'  " + condition + " and " + " a.Formno=b.Formno" +
                    " and b.BillType<>'R'     Order by b.rectimestamp" + obj.IsoEnd;
            }
            else if (compid == "1095")
            {
                str = obj.IsoStart + "select a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], " + " Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name]," +
                    "Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],  " + "isnull(k.KitName,'') as [Package Name],B.RepurchIncome as BV  " +
                    "from  " + obj.dBName + "..M_MemberMaster as a with(nolock) " + " " +
                    "Inner Join " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn  ," +
                    "" + " " + obj.dBName + "..RepurchIncome as b with(nolock)  " +
                    "Left Join " + obj.dBName + "..TrnOrder as c with(nolock)   On  " + " c.Formno=b.Formno  AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo " + " " +
                    "Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno " + " " +
                    "and s.BillNo=b.BillNo 	Left join " + HttpContext.Current.Session["InvDatabase" + compid] + "..DeletedBillMain  As Ds" + " On  b.Formno=Ds.Formno " +
                    " and ds.BillNo=b.BillNo	Left Join " + obj.dBName + "..M_KitMaster as k with(nolock)on  k.RowStatus='Y' " +
                    "and b.Kitid=k.KitId where d.Formno='" + Session["Formno"] + "'  " + condition + " and " + " a.Formno=b.Formno and b.BillType<>'R'     Order by b.rectimestamp" + obj.IsoEnd;
            }
            else if (compid == "1097" | compid == "1100")
            {
                str = obj.IsoStart + "select a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], " + " Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name]," +
                    "Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],  " + "isnull(k.KitName,'') as [Package Name],B.RepurchIncome as BV  " +
                    "from  " + obj.dBName + "..M_MemberMaster as a with(nolock) " + " Inner Join " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn  ," +
                    "" + " " + obj.dBName + "..RepurchIncome as b with(nolock)  Left Join " + obj.dBName + "..TrnOrder as c with(nolock)   On  " + " c.Formno=b.Formno  " +
                    "AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo " + " " +
                    "Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno " +
                    "" + " and s.BillNo=b.BillNo 	" +
                    "Left join " + HttpContext.Current.Session["InvDatabase" + compid] + "..DeletedBillMain  As Ds" + " On  b.Formno=Ds.Formno  " +
                    "and ds.BillNo=b.BillNo	Left Join " + obj.dBName + "..M_KitMaster as k with(nolock)on  k.RowStatus='Y' and b.Kitid=k.KitId " +
                    "where d.Formno='" + Session["Formno"] + "'  " + condition + " and " + " a.Formno=b.Formno and b.BillType<>'R'     Order by b.rectimestamp" + obj.IsoEnd;
            }
            else
            {
                str = obj.IsoStart + "  select a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name]," + " Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name]," +
                    "Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],B.Repurchincome as BV  from " + " " + obj.dBName + "..M_MemberMaster as a with(nolock) " +
                    "Inner Join " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn " + " inner Join " + obj.dBName + "..M_KitMaster as k with(nolock)on  " +
                    "k.RowStatus='Y'," + " " + obj.dBName + "..RepurchIncome as b with(nolock) Left Join " + obj.dBName + "..TrnOrder as c with(nolock)   On   " + " c.Formno=b.Formno  " +
                    " AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo  " +
                    " Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno " + " and s.BillNo=b.BillNo " +
                    " where  b.Kitid=k.KitId and d.Formno='" + Session["Formno"] + "'  " + condition + " and " + " a.Formno=b.Formno and b.BillType<>'R'  " +
                    "   Order by b.rectimestamp" + obj.IsoEnd;
            }
            dt = new DataTable();
            obj = new DAL();
            dt = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];
            Session["DirectData1"] = dt;
            GrdDirects.DataSource = dt;
            LblttlRcd.Text = dt.Rows.Count.ToString();
            LblttlRcd1.Text = dt.Rows.Count.ToString();
            GrdDirects.CurrentPageIndex = 0;
            GrdDirects.PageSize = Convert.ToInt32(ddlPazeSize.SelectedValue);
            GrdDirects.DataBind();
            if ((RbtProduct.SelectedValue == "T"))
            {
                divR.Visible = false;
                divT.Visible = true;
                lblleftbv.Visible = true;
                lblbv.Visible = true;
                DataRow aDr1 = dt.NewRow();
                if (Convert.ToInt32(RbtLegNo.SelectedValue) == 1)
                {
                    try
                    {
                        if (compid == "1107")
                        {
                            Lblleft.Text = "Left EV : " + dt.Compute("Sum(EV)", "");
                            Lblright.Text = "Right EV : 0.00";
                            lblleftbv.Text = "Left EV : " + dt.Compute("Sum(EV)", "");
                            lblbv.Text = "Right EV : 0.00";
                        }
                        else
                        {
                            Lblleft.Text = "Left BV : " + dt.Compute("Sum(BV)", "");
                            Lblright.Text = "Right BV : 0.00";
                            lblleftbv.Text = "Left BV : " + dt.Compute("Sum(BV)", "");
                            lblbv.Text = "Right BV : 0.00";
                        }

                    }
                    catch (Exception ex)
                    {
                    }

                    if (compid == "1010")
                    {
                        try
                        {
                            object result = dt.Compute("Sum([Royalty BV])", "");
                            if (result == DBNull.Value)
                            {
                                LblLeftRoyalty.Text = "0.00";
                                lblroyaltileftbv.Text = "Left Royalty BV :" + "0.00";
                            }
                            else
                            {
                                LblLeftRoyalty.Text = dt.Compute("Sum([Royalty BV])", "").ToString();
                                lblroyaltileftbv.Text = "Left Royalty BV :" + dt.Compute("Sum([Royalty BV])", "");
                            }
                            LblLeftRoyalty.Visible = true;
                            LblRightRoyalty.Visible = true;
                            lblroyaltileftbv.Visible = true;
                            lblroyaltirightbv.Visible = true;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                    {
                        LblLeftRoyalty.Text = "0.00";
                        lblroyaltileftbv.Text = "Left Royalty BV :" + "0.00";
                        LblLeftRoyalty.Visible = false;
                        LblRightRoyalty.Visible = false;
                        lblroyaltileftbv.Visible = false;
                        lblroyaltirightbv.Visible = false;
                    }
                }
                if (Convert.ToInt32(RbtLegNo.SelectedValue) == 2)
                {
                    try
                    {

                        if (compid == "1107")
                        {
                            Lblright.Text = "Right EV : " + dt.Compute("Sum(EV)", "");
                            Lblleft.Text = "Left EV : 0.00";
                            lblbv.Text = "Right EV : " + dt.Compute("Sum(EV)", "");
                            lblleftbv.Text = "Left EV : 0.00";
                        }
                        else
                        {
                            Lblright.Text = "Right BV : " + dt.Compute("Sum(BV)", "");
                            Lblleft.Text = "Left BV : 0.00";
                            lblbv.Text = "Right BV : " + dt.Compute("Sum(BV)", "");
                            lblleftbv.Text = "Left BV : 0.00";
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    if (compid == "1010")
                    {
                        try
                        {
                            object result = dt.Compute("Sum([Royalty BV])", "");
                            if (result == DBNull.Value)
                            {
                                LblRightRoyalty.Text = "0.00";
                                lblroyaltirightbv.Text = "Right Royalty BV :" + "0.00";
                            }
                            else
                            {
                                LblRightRoyalty.Text = dt.Compute("Sum([Royalty BV])", "").ToString();
                                lblroyaltirightbv.Text = "Right Royalty BV :" + dt.Compute("Sum([Royalty BV])", "");
                            }
                            //if (IsDBNull(dt.Compute("Sum([Royalty BV])", "")))
                            //{
                            //    LblRightRoyalty.Text = "0.00";
                            //    lblroyaltirightbv.Text = "Right Royalty BV :" + "0.00";
                            //}
                            //else
                            //{
                            //    LblRightRoyalty.Text = dt.Compute("Sum([Royalty BV])", "").ToString();
                            //    lblroyaltirightbv.Text = "Right Royalty BV :" + dt.Compute("Sum([Royalty BV])", "");
                            //}
                            LblLeftRoyalty.Visible = true;
                            LblRightRoyalty.Visible = true;
                            lblroyaltileftbv.Visible = true;
                            lblroyaltirightbv.Visible = true;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                    {
                        LblRightRoyalty.Text = "0.00";
                        lblroyaltirightbv.Text = "Right Royalty BV :" + "0.00";
                        LblLeftRoyalty.Visible = false;
                        LblRightRoyalty.Visible = false;
                        lblroyaltileftbv.Visible = false;
                        lblroyaltirightbv.Visible = false;
                    }
                }
                if (Convert.ToInt32(RbtLegNo.SelectedValue) == 0)
                {
                    DataTable dt1 = new DataTable();
                    dt1 = dt;

                    DataView dv = new DataView(dt1);
                    dv.RowFilter = "[Group Name] = 'Left'";
                    dt1 = dv.ToTable();
                    try
                    {
                        if (compid == "1107")
                        {
                            if ((dt1.Rows.Count > 0))
                            {
                                DataRow aDr2 = dt1.NewRow();


                                Lblleft.Text = dt1.Compute("Sum(Ev)", "").ToString();
                                lblleftbv.Text = "Left EV : " + dt1.Compute("Sum(EV)", "");
                            }
                            else
                            {
                                Lblleft.Text = "0.00";
                                lblleftbv.Text = "Left EV : " + "0.00";
                            }
                        }
                        else
                        {
                            if ((dt1.Rows.Count > 0))
                            {
                                DataRow aDr2 = dt1.NewRow();


                                Lblleft.Text = dt1.Compute("Sum(Bv)", "").ToString();


                                if (compid == "1108" | compid == "1007" | compid == "1010" | compid == "1095" | compid == "1097" | compid == "1100" | compid == "1103" | compid == "1109" | compid == "1110")
                                    lblleftbv.Text = "Left BV : " + dt1.Compute("Sum(BV)", "");
                                else
                                    lblleftbv.Text = "Left BV : " + dt1.Compute("Sum(BV)", "");
                            }
                            else
                            {
                                Lblleft.Text = "0.00";
                                if (compid == "1108" | compid == "1007" | compid == "1010" | compid == "1095" | compid == "1097" | compid == "1100" | compid == "1103" | compid == "1109" | compid == "1110")
                                    lblleftbv.Text = "Left BV : " + "0.00";
                                else
                                    lblleftbv.Text = "Left BV : " + "0.00";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    try
                    {
                        if (compid == "1010")
                        {
                            object result = dt.Compute("Sum([Royalty BV])", "");
                            if (result == DBNull.Value)
                            {
                                LblLeftRoyalty.Text = "0.00";
                                lblroyaltileftbv.Text = "Left Royalty BV :" + "0.00";
                            }
                            else
                            {
                                LblLeftRoyalty.Text = dt.Compute("Sum([Royalty BV])", "").ToString();
                                lblroyaltileftbv.Text = "Left Royalty BV :" + dt.Compute("Sum([Royalty BV])", "");
                            }
                            //if (IsDBNull(dt1.Compute("Sum([Royalty BV])", "")))
                            //{
                            //    LblLeftRoyalty.Text = "0.00";
                            //    lblroyaltileftbv.Text = "Left Royalty BV:" + "0.00";
                            //}
                            //else
                            //{
                            //    LblLeftRoyalty.Text = dt1.Compute("Sum([Royalty BV])", "").ToString();
                            //    lblroyaltileftbv.Text = "Left Royalty BV :" + dt1.Compute("Sum([Royalty BV])", "");
                            //}
                            LeftSmartcardbv.Visible = false;

                            // If IsDBNull(dt1.Compute("Sum([Smartcard BV])", "")) Then
                            // LblLeftRoyalty.Text = "0.00"
                            // LeftSmartcardbv.Text = "Left Well Smart BV:" & "0.00"
                            // Else
                            // LblLeftRoyalty.Text = dt1.Compute("Sum([Smartcard BV])", "")
                            // LeftSmartcardbv.Text = "Left Well Smart BV :" & dt1.Compute("Sum([Smartcard BV])", "")
                            // End If

                            LblLeftRoyalty.Visible = true;
                            LblRightRoyalty.Visible = true;
                            lblroyaltileftbv.Visible = true;
                            lblroyaltirightbv.Visible = true;
                        }
                        else
                        {
                            LblLeftRoyalty.Text = "0.00";
                            lblroyaltileftbv.Text = "Left Royalty BV :" + "0.00";
                            LblLeftRoyalty.Visible = false;
                            LblRightRoyalty.Visible = false;
                            lblroyaltileftbv.Visible = false;
                            lblroyaltirightbv.Visible = false;
                        }
                    }
                    catch (Exception ex)
                    {
                    }


                    dt1 = dt;
                    DataView dv1 = new DataView(dt1);
                    dv1.RowFilter = "[Group Name] = 'Right'";
                    dt1 = dv1.ToTable();
                    try
                    {
                        if (compid == "1107")
                        {
                            if ((dt1.Rows.Count > 0))
                            {
                                DataRow aDr3 = dt1.NewRow();

                                Lblright.Text = dt1.Compute("Sum(Ev)", "").ToString();
                                lblbv.Text = "Right EV : " + dt1.Compute("Sum(EV)", "");
                            }
                            else
                            {
                                Lblright.Text = "0.00";
                                lblbv.Text = "Right EV : 0.00";
                            }
                        }
                        else
                        {
                            if ((dt1.Rows.Count > 0))
                            {
                                DataRow aDr3 = dt1.NewRow();

                                Lblright.Text = dt1.Compute("Sum(Bv)", "").ToString();

                                if (compid == "1108" | compid == "1007" | compid == "1010" | compid == "1095" | compid == "1097" | compid == "1100" | compid == "1103" | compid == "1109" | compid == "1110")
                                    lblbv.Text = "Right BV : " + dt1.Compute("Sum(BV)", "");
                                else if (compid == "1074")
                                    lblbv.Text = "Right PV : " + dt1.Compute("Sum(BV)", "");
                                else
                                    lblbv.Text = "Right BV : " + dt1.Compute("Sum(BV)", "");
                            }
                            else
                            {
                                Lblright.Text = "0.00";
                                if (compid == "1108" | compid == "1007" | compid == "1010" | compid == "1095" | compid == "1097" | compid == "1100" | compid == "1103" | compid == "1109" | compid == "1110")
                                    lblbv.Text = "Right BV : 0.00";
                                else if (compid == "1074")
                                    lblbv.Text = "Right PV : 0.00";
                                else
                                    lblbv.Text = "Right BV : 0.00";
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                    try
                    {
                        if (compid == "1010")
                        {
                            object result = dt.Compute("Sum([Royalty BV])", "");
                            if (result == DBNull.Value)
                            {
                                LblRightRoyalty.Text = "0.00";
                                lblroyaltirightbv.Text = "Right Royalty BV :" + "0.00";
                            }
                            else
                            {
                                LblRightRoyalty.Text = dt1.Compute("Sum([Royalty BV])", "").ToString();
                                lblroyaltirightbv.Text = "Right Royalty BV :" + dt1.Compute("Sum([Royalty BV])", "");
                                LblLeftRoyalty.Visible = true;
                                LblRightRoyalty.Visible = true;
                                lblroyaltileftbv.Visible = true;
                                lblroyaltirightbv.Visible = true;
                            }
                            //if (IsDBNull(dt1.Compute("Sum([Royalty BV])", "")))
                            //{
                            //    LblRightRoyalty.Text = "0.00";
                            //    lblroyaltirightbv.Text = "Right Royalty BV :" + "0.00";
                            //}
                            //else
                            //{
                            //    LblRightRoyalty.Text = dt1.Compute("Sum([Royalty BV])", "").ToString();
                            //    lblroyaltirightbv.Text = "Right Royalty BV :" + dt1.Compute("Sum([Royalty BV])", "");
                            //    LblLeftRoyalty.Visible = true;
                            //    LblRightRoyalty.Visible = true;
                            //    lblroyaltileftbv.Visible = true;
                            //    lblroyaltirightbv.Visible = true;
                            //}
                            LeftSmartcardbv.Visible = false;
                        }
                        else
                        {
                            LblRightRoyalty.Text = "0.00";
                            lblroyaltirightbv.Text = "Right Royalty BV :" + "0.00";
                            LblLeftRoyalty.Visible = false;
                            LblRightRoyalty.Visible = false;
                            lblroyaltileftbv.Visible = false;
                            lblroyaltirightbv.Visible = false;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            if ((RbtProduct.SelectedValue == "R"))
            {
                if (Session["CompID"].ToString() == "1108" || (compid == "1007") | (compid == "1010" | compid == "1095" | compid == "1097" | compid == "1100" | compid == "1103" | compid == "1107" | compid == "1109" | compid == "1110"))
                {
                    divR.Visible = false;
                    divT.Visible = true;

                    DataRow aDr1 = dt.NewRow();
                    try
                    {
                        if (Convert.ToInt32(RbtLegNo.SelectedValue) == 1)
                        {
                            try
                            {
                                if (compid == "1107")
                                {
                                    Lblleft.Text = dt.Compute("Sum(EV)", "").ToString();
                                }
                                else
                                {
                                    Lblleft.Text = dt.Compute("Sum(BV)", "").ToString();
                                }

                            }
                            catch (Exception ex)
                            {
                            }

                            Lblright.Text = "0";
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    try
                    {
                        if (Convert.ToInt32(RbtLegNo.SelectedValue) == 2)
                        {
                            if ((dt.Rows.Count > 0))
                            {
                                try
                                {
                                    if (compid == "1107")
                                    {
                                        Lblright.Text = dt.Compute("Sum(EV)", "").ToString();
                                    }
                                    else
                                    {
                                        Lblright.Text = dt.Compute("Sum(BV)", "").ToString();
                                    }

                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            Lblleft.Text = "0";
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    if (Convert.ToInt32(RbtLegNo.SelectedValue) == 0)
                    {
                        DataTable dt1 = new DataTable();
                        dt1 = dt;

                        DataView dv = new DataView(dt1);
                        dv.RowFilter = "[Group Name] = 'Left'";
                        dt1 = dv.ToTable();
                        if ((dt1.Rows.Count > 0))
                        {
                            DataRow aDr2 = dt1.NewRow();
                            try
                            {
                                if (compid == "1107")
                                {
                                    Lblleft.Text = dt1.Compute("Sum(EV)", "").ToString();
                                }
                                else
                                {
                                    Lblleft.Text = dt1.Compute("Sum(BV)", "").ToString();
                                }

                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else
                        {
                            Lblleft.Text = "0.00";
                            LblLeftRoyalty.Text = "0.00";
                        }




                        dt1 = dt;
                        DataView dv1 = new DataView(dt1);
                        dv1.RowFilter = "[Group Name] = 'Right'";
                        dt1 = dv1.ToTable();
                        if ((dt1.Rows.Count > 0))
                        {
                            DataRow aDr3 = dt1.NewRow();
                            try
                            {
                                if (compid == "1107")
                                {
                                    Lblright.Text = dt1.Compute("Sum(EV)", "").ToString();
                                }
                                else
                                {
                                    Lblright.Text = dt1.Compute("Sum(BV)", "").ToString();
                                }

                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else
                            Lblright.Text = "0.00";
                        if (compid == "1010")
                        {
                            if (RbtProduct.SelectedValue == "R")
                                divRoyalty.Visible = false;
                            else
                                divRoyalty.Visible = true;
                        }
                        else
                            divRoyalty.Visible = false;
                    }
                }
                else if (compid == "1075")
                {
                    divR.Visible = false;
                    divT.Visible = true;

                    DataRow aDr1 = dt.NewRow();
                    try
                    {
                        if (Convert.ToInt32(RbtLegNo.SelectedValue) == 1)
                        {
                            try
                            {
                                Lblleft.Text = dt.Compute("Sum(RBV)", "").ToString();
                            }
                            catch (Exception ex)
                            {
                            }

                            Lblright.Text = "0";
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    try
                    {
                        if (Convert.ToInt32(RbtLegNo.SelectedValue) == 2)
                        {
                            if ((dt.Rows.Count > 0))
                            {
                                try
                                {
                                    Lblright.Text = dt.Compute("Sum(RBV)", "").ToString();
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            Lblleft.Text = "0";
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    if (Convert.ToInt32(RbtLegNo.SelectedValue) == 0)
                    {
                        DataTable dt1 = new DataTable();
                        dt1 = dt;

                        DataView dv = new DataView(dt1);
                        dv.RowFilter = "[Group Name] = 'Left'";
                        dt1 = dv.ToTable();
                        if ((dt1.Rows.Count > 0))
                        {
                            DataRow aDr2 = dt1.NewRow();
                            try
                            {
                                Lblleft.Text = dt1.Compute("Sum(RBV)", "").ToString();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else
                        {
                            Lblleft.Text = "0.00";
                            LblLeftRoyalty.Text = "0.00";
                        }




                        dt1 = dt;
                        DataView dv1 = new DataView(dt1);
                        dv1.RowFilter = "[Group Name] = 'Right'";
                        dt1 = dv1.ToTable();
                        if ((dt1.Rows.Count > 0))
                        {
                            DataRow aDr3 = dt1.NewRow();
                            try
                            {
                                Lblright.Text = dt1.Compute("Sum(RBV)", "").ToString();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else
                            Lblright.Text = "0.00";
                        if (compid == "1010")
                        {
                            if (RbtProduct.SelectedValue == "R")
                                divRoyalty.Visible = false;
                            else
                                divRoyalty.Visible = true;
                        }
                        else
                            divRoyalty.Visible = false;
                    }
                }
                else if (Session["CompID"].ToString() == "1108" || compid == "1102" | compid == "1107" | compid == "1109" | compid == "1110")
                {
                    divR.Visible = false;
                    divT.Visible = true;
                    lblleftbv.Visible = true;
                    lblbv.Visible = true;
                    DataRow aDr1 = dt.NewRow();
                    if (Convert.ToInt32(RbtLegNo.SelectedValue) == 1)
                    {
                        try
                        {
                            Lblleft.Text = "Left BV : " + dt.Compute("Sum(BV)", "");
                            Lblright.Text = "Right BV : 0.00";
                            lblleftbv.Text = "Left BV : " + dt.Compute("Sum(BV)", "");
                            lblbv.Text = "Right BV : 0.00";
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    if (Convert.ToInt32(RbtLegNo.SelectedValue) == 2)
                    {
                        try
                        {
                            Lblright.Text = "Right BV : " + dt.Compute("Sum(BV)", "");
                            Lblleft.Text = "Left BV : 0.00";
                            lblbv.Text = "Right BV : " + dt.Compute("Sum(BV)", "");
                            lblleftbv.Text = "Left BV : 0.00";
                            //Lblright.Text = dt.Compute("Sum(Bv)", "").ToString();
                            //Lblleft.Text = "0";
                            //lblbv.Text = dt.Compute("Sum(BV)", "").ToString();
                            //lblbv.Text = "0";
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    if (Convert.ToInt32(RbtLegNo.SelectedValue) == 0)
                    {
                        DataTable dt1 = new DataTable();
                        dt1 = dt;

                        DataView dv = new DataView(dt1);
                        dv.RowFilter = "[Group Name] = 'Left'";
                        dt1 = dv.ToTable();
                        try
                        {
                            if ((dt1.Rows.Count > 0))
                            {
                                DataRow aDr2 = dt1.NewRow();


                                Lblleft.Text = dt1.Compute("Sum(Bv)", "").ToString();
                                lblleftbv.Text = "Left BV : " + dt1.Compute("Sum(BV)", "");
                            }
                            else
                            {
                                Lblleft.Text = "0.00";
                                lblleftbv.Text = "Left BV : " + "0.00";
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        dt1 = dt;
                        DataView dv1 = new DataView(dt1);
                        dv1.RowFilter = "[Group Name] = 'Right'";
                        dt1 = dv1.ToTable();
                        try
                        {
                            if ((dt1.Rows.Count > 0))
                            {
                                DataRow aDr3 = dt1.NewRow();

                                Lblright.Text = dt1.Compute("Sum(Bv)", "").ToString();
                                lblbv.Text = "Right BV : " + dt1.Compute("Sum(BV)", "");
                            }
                            else
                            {
                                lblbv.Text = "Right BV : 0.00";
                            }
                        }
                        catch (Exception ex)
                        {
                        }

                    }
                }
                else
                {
                    divR.Visible = true;
                    divT.Visible = false;
                    if ((dt.Rows.Count > 0))
                    {
                        // below commit 22 March 2022
                        try
                        {
                            lblTotalBV.Text = dt.Compute("Sum([Repurchase BV])", "").ToString();

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                        lblTotalBV.Text = "0";
                }
                if (Session["CompID"].ToString() == "1108" || compid == "1109" || compid == "1102" | compid == "1107" | compid == "1109" | compid == "1110")
                {
                    if ((dt.Rows.Count > 0))
                    {
                        // below commit 22 March 2022
                        try
                        {
                            lblTotalBV.Text = dt.Compute("Sum(BV)", "").ToString();

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                        lblTotalBV.Text = "0";
                    LblttlBv.Text = lblTotalBV.Text.ToString();
                }
                else
                {
                    LblttlBv.Text = lblTotalBV.Text.ToString();
                }

                if (Session["CompID"].ToString() == "1108" || compid != "1109" ||compid != "1102" | compid == "1107" | compid == "1109" | compid == "1110")
                {
                    if (Convert.ToInt32(RbtLegNo.SelectedValue) == 1)
                    {
                        try
                        {
                            Lblleft.Text = dt.Compute("Sum(BV)", "").ToString();
                            Lblright.Text = "0";
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    if (Convert.ToInt32(RbtLegNo.SelectedValue) == 2)
                    {
                        if ((dt.Rows.Count > 0))
                        {
                            try
                            {
                                Lblright.Text = dt.Compute("Sum(BV)", "").ToString();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        Lblleft.Text = "0";
                    }
                    if (Convert.ToInt32(RbtLegNo.SelectedValue) == 0)
                    {
                        DataTable dt1 = new DataTable();
                        dt1 = dt;

                        DataView dv = new DataView(dt1);
                        dv.RowFilter = "[Group Name] = 'Left'";
                        dt1 = dv.ToTable();
                        if ((dt1.Rows.Count > 0))
                        {
                            DataRow aDr2 = dt1.NewRow();
                            try
                            {
                                Lblleft.Text = dt1.Compute("Sum([Repurchase BV])", "").ToString();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else
                        {
                            Lblleft.Text = "0.00";
                            LblLeftRoyalty.Text = "0.00";
                        }




                        dt1 = dt;
                        DataView dv1 = new DataView(dt1);
                        dv1.RowFilter = "[Group Name] = 'Right'";
                        dt1 = dv1.ToTable();
                        if ((dt1.Rows.Count > 0))
                        {
                            DataRow aDr3 = dt1.NewRow();
                            // If RbtProduct.SelectedValue = "R" Then
                            try
                            {
                                Lblright.Text = dt1.Compute("Sum([Repurchase BV])", "").ToString();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        else
                            Lblright.Text = "0.00";
                        if (compid == "1010")
                        {
                            if (RbtProduct.SelectedValue == "R")
                                divRoyalty.Visible = false;
                            else
                                divRoyalty.Visible = true;
                        }
                        else
                            divRoyalty.Visible = false;
                    }
                }
            }
            else
            {
                divR.Visible = true;
                divT.Visible = false;
                if ((dt.Rows.Count > 0))
                {
                    // below commit 22 March 2022
                    // lblTotalBV.Text = dt.Compute("Sum([Repurchase BV])", "")
                    try
                    {
                        if ((compid == "1074"))
                            lblTotalBV.Text = dt.Compute("Sum(PV)", "").ToString();
                        else
                            if (compid == "1107")
                        {
                            lblTotalBV.Text = dt.Compute("Sum(EV)", "").ToString();
                        }
                        else
                        {
                            lblTotalBV.Text = dt.Compute("Sum(BV)", "").ToString();
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                }

                else
                    lblTotalBV.Text = "0";


                LblttlBv.Text = lblTotalBV.Text.ToString();
                if (compid == "1010")
                {
                    if (RbtProduct.SelectedValue == "R")
                        divRoyalty.Visible = false;
                    else
                        divRoyalty.Visible = true;
                }
                else
                    divRoyalty.Visible = false;


                if (compid == "1108" | (compid == "1007") | compid == "1010" | compid == "1095" | compid == "1097" | compid == "1100" | compid == "1103" | compid == "1109" | compid == "1109" | compid == "1110")
                {
                    dt.Columns["BV"].ColumnName = Session["ColName1"].ToString();
                    GrdDirects.DataSource = dt;
                    GrdDirects.PageSize = Convert.ToInt32(ddlPazeSize.SelectedValue);
                    GrdDirects.DataBind();
                }
                else if (compid == "1107")
                {
                    dt.Columns["EV"].ColumnName = Session["ColName1"].ToString();
                    GrdDirects.DataSource = dt;
                    GrdDirects.PageSize = Convert.ToInt32(ddlPazeSize.SelectedValue);
                    GrdDirects.DataBind();
                }
                else
                {

                    // below commit 22 March 2022
                    // dt.Columns("Repurchase BV").ColumnName = Session("ColName1")
                    dt.Columns["BV"].ColumnName = "BV";
                    GrdDirects.DataSource = dt;
                    GrdDirects.PageSize = Convert.ToInt32(ddlPazeSize.SelectedValue);
                    GrdDirects.DataBind();
                }


                if (compid == "1007" | compid == "1095" | compid == "1097" | compid == "1100" | compid == "1103")
                {
                    if (RbtProduct.SelectedValue == "T")
                        divtotal.Visible = false;
                    else
                        divtotal.Visible = true;
                }
                else
                    divtotal.Visible = true;
            }

        }
        catch (Exception ex)
        {
            if (compid == "1010")
            {
                if (RbtProduct.SelectedValue == "R")
                    divRoyalty.Visible = false;
                else
                    divRoyalty.Visible = true;
            }
            else
                divRoyalty.Visible = false;
        }
    }
    protected void GrdDirects_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        try
        {
            GrdDirects.CurrentPageIndex = e.NewPageIndex;
            GrdDirects.DataSource = Session["DirectData1"];
            GrdDirects.PageSize = Convert.ToInt32(ddlPazeSize.SelectedValue);
            GrdDirects.DataBind();
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    }
    protected void DdlLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LevelDetail();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff") +
                          Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
        }
    }
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        LevelDetail();
    }
    protected void ddlPazeSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GrdDirects.DataSource = Session["DirectData1"];
            GrdDirects.PageSize = Convert.ToInt32(ddlPazeSize.SelectedValue);
            GrdDirects.DataBind();
        }
        catch (Exception ex)
        {
            // handle exception if needed
        }
    }
    protected void RbtProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillLevel_Ra();
    }
    protected void FillLevel_Ra()
    {
        if (RbtProduct.SelectedValue == "R")
        {
            if (Session["CompID"].ToString() == "1007" ||
                Session["CompID"].ToString() == "1010" ||
                Session["CompID"].ToString() == "1091" ||
                Session["CompID"].ToString() == "1095" ||
                Session["CompID"].ToString() == "1097")
            {
                LblLevel.Text = "Group Wise";
                DdlLevel.Visible = false;
                RbtLegNo.Visible = true;
            }
            else if (Session["CompID"].ToString() == "1102" || Session["CompID"].ToString() == "1107")
            {
                LblLevel.Text = "Group Wise";
                DdlLevel.Visible = false;
                RbtLegNo.Visible = true;
                lblleftbv.Visible = true;
                lblbv.Visible = true;
            }
            else
            {
                LblLevel.Text = "Level Wise";
                DdlLevel.Visible = true;
                RbtLegNo.Visible = false;
                lblleftbv.Visible = false;
                lblbv.Visible = false;
            }

        }
        else
        {
            LblLevel.Text = "Group Wise";
            DdlLevel.Visible = false;
            RbtLegNo.Visible = true;
            //lblleftbv.Visible = false;
            //lblbv.Visible = false;
        }
    }
    private void LogError(Exception ex)
    {
        string path = HttpContext.Current.Request.Url.AbsoluteUri;
        string text = path + " : " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff") + Environment.NewLine;
        objGen.WriteToFile(text + ex.Message);
    }
    protected void BtnExportToExcel_Click(object sender, EventArgs e)
    {
        string compid = Session["CompID"].ToString();
        try
        {

            string str = "";
            string condition = "";
            if (RbtProduct.SelectedValue == "R")
            {
                if ((compid == "1007") | (compid == "1010") | (compid == "1091") | (compid == "1095") | (compid == "1097" | compid == "1100" | compid == "1103"))
                {
                    if (RbtLegNo.SelectedValue != "0")
                        condition = condition + " and d.LegNo='" + RbtLegNo.SelectedValue + "'";
                }
                else if (Session["CompID"].ToString() == "1108" || (compid == "1109") || (compid == "1102") | (compid == "1107") | (compid == "1110"))
                {
                    if (RbtLegNo.SelectedValue != "0")
                        condition = condition + " and a.LegNo='" + RbtLegNo.SelectedValue + "'";
                }
                else if (Convert.ToInt32(DdlLevel.SelectedValue) > 0)
                    condition = condition + "AND d.MLevel='" + DdlLevel.SelectedValue + "'";
            }
            else if (RbtLegNo.SelectedValue != "0")
                condition = condition + " and d.LegNo='" + RbtLegNo.SelectedValue + "'";

            string scrname = "";
            string FrmDate = "";
            string ToDate = "";
            FrmDate = TxtFromDate.Text;
            ToDate = TxtToDate.Text;
            if (FrmDate != "")
            {
                try
                {
                    DateTime Dt = Convert.ToDateTime(FrmDate);
                }
                catch (Exception ex)
                {
                    scrname = "<SCRIPT language='javascript'>alert('Check Start Date.. ');" + "</SCRIPT>";
                    this.RegisterStartupScript("MyAlert", scrname);
                    return;
                }
            }
            if (ToDate != "")
            {
                try
                {
                    DateTime Dt = Convert.ToDateTime(ToDate);
                }
                catch (Exception ex)
                {
                    scrname = "<SCRIPT language='javascript'>alert('Check End Date.. ');" + "</SCRIPT>";
                    this.RegisterStartupScript("MyAlert", scrname);
                    return;
                }
            }
            if (FrmDate != "" & ToDate != "")
                condition = condition + " And Cast(Convert(Varchar,b.BillDate,106)as Date)>='" + FrmDate + "' And Cast(Convert(Varchar,b.BillDate,106)as Date)<='" + ToDate + "'";

            if (compid == "1010" | compid == "1103")
            {
                if (TxtFromDate.Text != "")
                    FrmDate = TxtFromDate.Text;
                if (TxtToDate.Text != "")
                    ToDate = TxtToDate.Text;
            }
            if (RbtProduct.SelectedValue == "R")
            {
                if ((compid == "1007"))
                {
                    str = obj.IsoStart + "  select Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date] ,a.Idno  as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As  ";
                    str += "  [Member Name],  Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name],";
                    str += "  Isnull(Isnull(c.OrderAmt,s.NetPayable),Ds.NetPayable) as [Bill Amount],b.Pvvalue as BV  ";
                    str += "  from " + obj.dBName + "..M_MemberMaster as a with(nolock) inner Join  " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn ";
                    str += "  Inner Join " + obj.dBName + "..RepurchIncome as b with(nolock) on  a.Formno=b.Formno  ";
                    str += " Left Join " + obj.dBName + "..TrnOrder as c with(nolock)  On   b.Formno=c.formno  AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo ";
                    str += "  Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s with(nolock) On  b.Formno=s.Formno  and s.BillNo=b.BillNo";
                    str += "  Left join " + HttpContext.Current.Session["InvDatabase" + compid] + "..DeletedBillMain  As Ds On  b.Formno=Ds.Formno  and ds.BillNo=b.BillNo";
                    str += "  where  d.Formno='" + Session["Formno"] + "'";
                    str += " " + condition + " and  a.Formno=b.Formno and b.BillType in ('R','O')";
                    str += " Order by b.rectimestamp " + obj.IsoEnd;
                }
                else if (Session["CompID"].ToString() == "1108" || (compid == "1010" | compid == "1103" | compid == "1105" | compid == "1107" | compid == "1109" | compid == "1110"))
                {
                    str = obj.IsoStart + " exec Sp_DowmLineRepurchaseReport '" + Session["Formno"] + "','" + FrmDate + "','" + ToDate + "' " + obj.IsoEnd;
                }
                else if (compid == "1074")
                {
                    str = obj.IsoStart + "  select Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date] ,a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As  ";
                    str += "  [Member Name],  Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name],";
                    str += "  Isnull(Isnull(c.OrderAmt,s.NetPayable),Ds.NetPayable) as [Bill Amount],b.RepurchIncome as [Repurchase PV]  ";
                    str += "  from " + obj.dBName + "..M_MemberMaster as a with(nolock) inner Join  " + obj.dBName + "..R_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn ";
                    str += "  Inner Join " + obj.dBName + "..RepurchIncome as b with(nolock) on  a.Formno=b.Formno  ";
                    str += " Left Join " + obj.dBName + "..TrnOrder as c with(nolock)  On   b.Formno=c.formno  AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo ";
                    str += "  Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s with(nolock) On  b.Formno=s.Formno  and s.BillNo=b.BillNo";
                    str += "  Left join " + HttpContext.Current.Session["InvDatabase" + compid] + "..DeletedBillMain  As Ds On  b.Formno=Ds.Formno  and ds.BillNo=b.BillNo";
                    str += "  where  d.Formno='" + Session["Formno"] + "'";
                    str += " " + condition + " and  a.Formno=b.Formno and b.BillType in ('R','O')";
                    str += " Order by b.rectimestamp " + obj.IsoEnd;
                }
                else if (compid == "1091")
                {
                    str = obj.IsoStart + "  select Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date] ,a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As  ";
                    str += "  [Distributor Name],  Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name],";
                    str += "  Isnull(Isnull(c.OrderAmt,s.NetPayable),Ds.NetPayable) as [Bill Amount],b.RepurchIncome as [Repurchase BV]  ";
                    str += "  from " + obj.dBName + "..M_MemberMaster as a with(nolock) inner Join  " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn ";
                    str += "  Inner Join " + obj.dBName + "..RepurchIncome as b with(nolock) on  a.Formno=b.Formno  ";
                    str += " Left Join " + obj.dBName + "..TrnOrder as c with(nolock)  On   b.Formno=c.formno  AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo ";
                    str += "  Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s with(nolock) On  b.Formno=s.Formno  and s.BillNo=b.BillNo";
                    str += "  Left join " + HttpContext.Current.Session["InvDatabase" + compid] + "..DeletedBillMain  As Ds On  b.Formno=Ds.Formno  and ds.BillNo=b.BillNo";
                    str += "  where  d.Formno='" + Session["Formno"] + "'";
                    str += " " + condition + " and  a.Formno=b.Formno and b.BillType in ('R','O')";
                    str += " Order by b.rectimestamp " + obj.IsoEnd;
                }
                else if (compid == "1095")
                {
                    str = obj.IsoStart + "  select Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date] ,a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As  ";
                    str += "  [Member Name],  Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name],";
                    str += "  Isnull(Isnull(c.OrderAmt,s.NetPayable),Ds.NetPayable) as [Bill Amount],b.RepurchIncome as BV  ";
                    str += "  from " + obj.dBName + "..M_MemberMaster as a with(nolock) inner Join  " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn ";
                    str += "  Inner Join " + obj.dBName + "..RepurchIncome as b with(nolock) on  a.Formno=b.Formno  ";
                    str += " Left Join " + obj.dBName + "..TrnOrder as c with(nolock)  On   b.Formno=c.formno  AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo ";
                    str += "  Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s with(nolock) On  b.Formno=s.Formno  and s.BillNo=b.BillNo";
                    str += "  Left join " + HttpContext.Current.Session["InvDatabase" + compid] + "..DeletedBillMain  As Ds On  b.Formno=Ds.Formno  and ds.BillNo=b.BillNo";
                    str += "  where  d.Formno='" + Session["Formno"] + "'";
                    str += " " + condition + " and  a.Formno=b.Formno and b.BillType in ('R','O')";
                    str += " Order by b.rectimestamp " + obj.IsoEnd;
                }
                else if (compid == "1097" | compid == "1100")
                {
                    str = obj.IsoStart + "  select Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date] ,a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As  ";
                    str += "  [Member Name],  Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name],";
                    str += "  Isnull(Isnull(c.OrderAmt,s.NetPayable),Ds.NetPayable) as [Bill Amount],b.RepurchIncome as BV  ";
                    str += "  from " + obj.dBName + "..M_MemberMaster as a with(nolock) inner Join  " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn ";
                    str += "  Inner Join " + obj.dBName + "..RepurchIncome as b with(nolock) on  a.Formno=b.Formno  ";
                    str += " Left Join " + obj.dBName + "..TrnOrder as c with(nolock)  On   b.Formno=c.formno  AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo ";
                    str += "  Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s with(nolock) On  b.Formno=s.Formno  and s.BillNo=b.BillNo";
                    str += "  Left join " + HttpContext.Current.Session["InvDatabase" + compid] + "..DeletedBillMain  As Ds On  b.Formno=Ds.Formno  and ds.BillNo=b.BillNo";
                    str += "  where  d.Formno='" + Session["Formno"] + "'";
                    str += " " + condition + " and  a.Formno=b.Formno and b.BillType in ('R','O')";
                    str += " Order by b.rectimestamp " + obj.IsoEnd;
                }
                else
                {
                    str = obj.IsoStart + "  select Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date] ,a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As  ";
                    str += "  [Member Name],  Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name],";
                    str += " b.RepurchIncome as BV ";
                    str += "  from " + obj.dBName + "..M_MemberMaster as a with(nolock) inner Join  " + obj.dBName + "..R_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn ";
                    str += "  Inner Join " + obj.dBName + "..RepurchIncome as b with(nolock) on  a.Formno=b.Formno  ";
                    str += " Left Join " + obj.dBName + "..TrnOrder as c with(nolock)  On   b.Formno=c.formno  AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo ";
                    str += "  Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s with(nolock) On  b.Formno=s.Formno  and s.BillNo=b.BillNo";
                    str += "  Left join " + HttpContext.Current.Session["InvDatabase" + compid] + "..DeletedBillMain  As Ds On  b.Formno=Ds.Formno  and ds.BillNo=b.BillNo";
                    str += "  where  d.Formno='" + Session["Formno"] + "'";
                    str += " " + condition + " and  a.Formno=b.Formno and b.BillType in ('R','O')";
                    str += " Order by b.rectimestamp " + obj.IsoEnd;
                }
            }
            else if ((compid == "1007"))
            {
                str = obj.IsoStart + "select a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], " + " Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name]," +
                    "Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],  " + "isnull(Isnull(Isnull(c.OrderAmt,s.NetPayable),ds.Netpayable),k.KitAmount) as [Amount]," +
                    "" + "isnull(k.KitName,'') as [Package Name],B.PVValue as BV  " +
                    "from  " + obj.dBName + "..M_MemberMaster as a with(nolock) " + " Inner Join " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn  ," +
                    "" + " " + obj.dBName + "..RepurchIncome as b with(nolock)  Left Join " + obj.dBName + "..TrnOrder as c with(nolock)   On  " + " c.Formno=b.Formno  " +
                    "AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo " + " " +
                    "Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno " + " and s.BillNo=b.BillNo 	" +
                    "Left join " + HttpContext.Current.Session["InvDatabase" + compid] + "..DeletedBillMain  As Ds" + " On  b.Formno=Ds.Formno  and ds.BillNo=b.BillNo	" +
                    "Left Join " + obj.dBName + "..M_KitMaster as k with(nolock)on  k.RowStatus='Y' and b.Kitid=k.KitId where d.Formno='" + Session["Formno"] + "'  " + condition + " " +
                    "and " + " a.Formno=b.Formno and b.BillType<>'R'     Order by b.rectimestamp" + obj.IsoEnd;
            }
            else if ((compid == "1010"))
            {
                str += obj.IsoStart + " Select  a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], ";
                str += " Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name],";
                str += " Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],";
                str += "   Case when b.kitid=0 then RoyaltyNmWise  else kitname end  as  [Package Name],Case when b.kitid=0 then b.amount  else isnull(kitamount,0) end as Amount,Repurchincome as BV,isnull(royalty,0) as [Royalty BV]  ";
                str += " from  " + obj.dBName + "..M_MemberMaster as a with(nolock) ";
                str += " Inner Join " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn ";
                str += " Inner join " + obj.dBName + "..RepurchIncome b  on d.FormnoDwn  = b.Formno And  RBV in ('N','Y')  and b.BillType<>'R' 	" +
                    " left Join " + obj.dBName + "..M_KitMaster as k with(nolock)on b.Kitid=k.KitId  And  k.RowStatus='Y' ";
                str += " Where  d.Formno='" + Session["Formno"] + "'  " + condition + " " + obj.IsoEnd;
            }
            else if (compid == "1108" || compid == "1109" || compid == "1103" | compid == "1105" | compid == "1110")
            {
                str += obj.IsoStart + " Select  a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], ";
                str += " Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name],";
                str += " Replace(Convert(Varchar,b.Rectimestamp,106),' ','-')+' '+CONVERT(varchar(15),CAST(b.Rectimestamp AS TIME),22) as [Bill Date],";
                str += " kitname as [Package Name],Case when b.kitid=0 then b.amount  else isnull(kitamount,0) end as Amount,Repurchincome as BV  ";
                str += " from  " + obj.dBName + "..M_MemberMaster as a with(nolock) ";
                str += " Inner Join " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn ";
                str += " Inner join " + obj.dBName + "..RepurchIncome b  on d.FormnoDwn  = b.Formno and b.BillType<>'R' 	 left Join " + obj.dBName + "..M_KitMaster as k with(nolock)on b.Kitid=k.KitId  And  k.RowStatus='Y' ";
                str += " Where  d.Formno='" + Session["Formno"] + "'  " + condition + " order by [Bill Date]  desc   " + obj.IsoEnd;
            }
            else if (Session["CompID"].ToString() == "1108" || compid == "1107" || compid == "1109"| compid == "1110")
            {
                str += obj.IsoStart + " Select  a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], ";
                str += " Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name],";
                str += " Replace(Convert(Varchar,b.Rectimestamp,106),' ','-')+' '+CONVERT(varchar(15),CAST(b.Rectimestamp AS TIME),22) as [Bill Date],";
                str += " kitname as [Package Name],Case when b.kitid=0 then b.amount  else isnull(kitamount,0) end as Amount,Repurchincome as EV  ";
                str += " from  " + obj.dBName + "..M_MemberMaster as a with(nolock) ";
                str += " Inner Join " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn ";
                str += " Inner join " + obj.dBName + "..RepurchIncome b  on d.FormnoDwn  = b.Formno and b.BillType<>'R' 	 left Join " + obj.dBName + "..M_KitMaster as k with(nolock)on b.Kitid=k.KitId  And  k.RowStatus='Y' ";
                str += " Where  d.Formno='" + Session["Formno"] + "'  " + condition + " order by [Bill Date]  desc   " + obj.IsoEnd;
            }
            else if (compid == "1074")
            {
                str = obj.IsoStart + "  select a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name]," + " Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name]," +
                    "Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date], " + " Isnull(Isnull(c.OrderAmt,s.NetPayable),k.KitAmount) as [Amount],k.KitName as [Package Name]," +
                    "B.Repurchincome as PV  from " + " " + obj.dBName + "..M_MemberMaster as a with(nolock) " +
                    "Inner Join " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn " + " " +
                    "inner Join " + obj.dBName + "..M_KitMaster as k with(nolock)on  k.RowStatus='Y'," + " " + obj.dBName + "..RepurchIncome as b with(nolock) " +
                    "Left Join " + obj.dBName + "..TrnOrder as c with(nolock)   On   " + " c.Formno=b.Formno   " +
                    "AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo   " +
                    "Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno " + " and s.BillNo=b.BillNo " +
                    " where  b.Kitid=k.KitId and d.Formno='" + Session["Formno"] + "'  " + condition + " and " + " a.Formno=b.Formno and b.BillType<>'R' Order by b.rectimestamp" + obj.IsoEnd;
            }
            else if (compid == "1091")
            {
                str = obj.IsoStart + "  select a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As [Distributor Name]," + " Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name]," +
                    "Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date], " + " Isnull(Isnull(c.OrderAmt,s.NetPayable),k.KitAmount) as [Amount],k.KitName as [Package Name]," +
                    "B.Repurchincome as BV  from " + " " + obj.dBName + "..M_MemberMaster as a with(nolock) " +
                    "Inner Join " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn " + " " +
                    "inner Join " + obj.dBName + "..M_KitMaster as k with(nolock)on  k.RowStatus='Y'," + " " +
                    " " + obj.dBName + "..RepurchIncome as b with(nolock) Left Join " + obj.dBName + "..TrnOrder as c with(nolock)   On   " + " c.Formno=b.Formno   " +
                    "AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo  " +
                    " Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno " + " " +
                    "and s.BillNo=b.BillNo  where  b.Kitid=k.KitId and d.Formno='" + Session["Formno"] + "'  " + condition + " and " + " a.Formno=b.Formno" +
                    " and b.BillType<>'R'     Order by b.rectimestamp" + obj.IsoEnd;
            }
            else if (compid == "1095")
            {
                str = obj.IsoStart + "select a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], " + " Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name]," +
                    "Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],  " + "isnull(k.KitName,'') as [Package Name],B.RepurchIncome as BV  " +
                    "from  " + obj.dBName + "..M_MemberMaster as a with(nolock) " + " " +
                    "Inner Join " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn  ," +
                    "" + " " + obj.dBName + "..RepurchIncome as b with(nolock)  " +
                    "Left Join " + obj.dBName + "..TrnOrder as c with(nolock)   On  " + " c.Formno=b.Formno  AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo " + " " +
                    "Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno " + " " +
                    "and s.BillNo=b.BillNo 	Left join " + HttpContext.Current.Session["InvDatabase" + compid] + "..DeletedBillMain  As Ds" + " On  b.Formno=Ds.Formno " +
                    " and ds.BillNo=b.BillNo	Left Join " + obj.dBName + "..M_KitMaster as k with(nolock)on  k.RowStatus='Y' " +
                    "and b.Kitid=k.KitId where d.Formno='" + Session["Formno"] + "'  " + condition + " and " + " a.Formno=b.Formno and b.BillType<>'R'     Order by b.rectimestamp" + obj.IsoEnd;
            }
            else if (compid == "1097" | compid == "1100")
            {
                str = obj.IsoStart + "select a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name], " + " Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name]," +
                    "Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],  " + "isnull(k.KitName,'') as [Package Name],B.RepurchIncome as BV  " +
                    "from  " + obj.dBName + "..M_MemberMaster as a with(nolock) " + " Inner Join " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn  ," +
                    "" + " " + obj.dBName + "..RepurchIncome as b with(nolock)  Left Join " + obj.dBName + "..TrnOrder as c with(nolock)   On  " + " c.Formno=b.Formno  " +
                    "AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo " + " " +
                    "Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno " +
                    "" + " and s.BillNo=b.BillNo 	" +
                    "Left join " + HttpContext.Current.Session["InvDatabase" + compid] + "..DeletedBillMain  As Ds" + " On  b.Formno=Ds.Formno  " +
                    "and ds.BillNo=b.BillNo	Left Join " + obj.dBName + "..M_KitMaster as k with(nolock)on  k.RowStatus='Y' and b.Kitid=k.KitId " +
                    "where d.Formno='" + Session["Formno"] + "'  " + condition + " and " + " a.Formno=b.Formno and b.BillType<>'R'     Order by b.rectimestamp" + obj.IsoEnd;
            }
            else
            {
                str = obj.IsoStart + "  select a.Idno as [Member ID],(a.MemFirstName+ ' '+a.MemLastName)As [Member Name]," + " Case when d.LegNo=1 then 'Left' else 'Right' end as [Group Name]," +
                    "Replace(Convert(Varchar,b.BillDate,106),' ','-') as [Bill Date],B.Repurchincome as BV  from " + " " + obj.dBName + "..M_MemberMaster as a with(nolock) " +
                    "Inner Join " + obj.dBName + "..M_MemTreeRelation as d with(nolock) on a.Formno=d.FormnoDwn " + " inner Join " + obj.dBName + "..M_KitMaster as k with(nolock)on  " +
                    "k.RowStatus='Y'," + " " + obj.dBName + "..RepurchIncome as b with(nolock) Left Join " + obj.dBName + "..TrnOrder as c with(nolock)   On   " + " c.Formno=b.Formno  " +
                    " AND 'Order '+CAst(OrderNo as nvarchar(100))=b.BillNo  " +
                    " Left Join " + HttpContext.Current.Session["InvDatabase" + compid] + "..TrnBillMain as s  with(nolock) On  b.Formno=s.Formno " + " and s.BillNo=b.BillNo " +
                    " where  b.Kitid=k.KitId and d.Formno='" + Session["Formno"] + "'  " + condition + " and " + " a.Formno=b.Formno and b.BillType<>'R'  " +
                    "   Order by b.rectimestamp" + obj.IsoEnd;
            }
            obj = new DAL();
            DataTable dtTemp = new DataTable();
            DataGrid dg = new DataGrid();
            dtTemp = SqlHelper.ExecuteDataset(HttpContext.Current.Session["MlmSelectDatabase" + Session["CompID"]].ToString(), CommandType.Text, str).Tables[0];
            if (Session["CompID"].ToString() == "1007" && dtTemp.Columns.Contains("BV"))
            {
                dtTemp.Columns["BV"].ColumnName = "PV";
            }
            dg.DataSource = dtTemp;
            dg.DataBind();
            ExportToExcel("Downline" + RbtProduct.SelectedItem.Text + ".xls", dg);
        }
        catch (Exception ex)
        {

        }
    }
    private void ExportToExcel(string strFileName, DataGrid dg)
    {
        try
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htw;

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.xls";
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
            Response.Charset = "";

            dg.EnableViewState = false;

            htw = new HtmlTextWriter(sw);
            dg.RenderControl(htw);

            Response.Write(sw.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " +
                          DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:fff") +
                          Environment.NewLine;

            objGen.WriteToFile(text + ex.Message);
        }
    }
}
