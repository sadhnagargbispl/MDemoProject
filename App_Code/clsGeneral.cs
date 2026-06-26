using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

public class clsGeneral
{
    public SqlConnection objSQlConnection;
    public System.Data.SqlClient.SqlConnectionStringBuilder Connection = new System.Data.SqlClient.SqlConnectionStringBuilder();

    public void Fill_Date_box(DropDownList cday, DropDownList cMonth, DropDownList cYear, int YearStart = 1950, int yearEnd = 2010)
    {
        for (short i = 1; i <= 31; i++)
        {
            cday.Items.Add(i.ToString().PadLeft(2, '0'));
        }

        for (short i = 1; i <= 12; i++)
        {
            cMonth.Items.Add(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i).Substring(0, 3).Trim().ToUpper());
        }

        for (int i = YearStart; i <= yearEnd; i++)
        {
            cYear.Items.Add(i.ToString());
        }
    }

    public void FillCmb(ref DropDownList Cmb, ref DataTable strTbl, string strValFld, string strTxtFld)
    {
        Cmb.DataSource = strTbl;
        Cmb.DataValueField = strValFld;
        Cmb.DataTextField = strTxtFld;
        Cmb.DataBind();
    }

    private int RandomNumber(int min, int max)
    {
        var random = new Random();
        return random.Next(min, max);
    }

    private string RandomString(int size, bool lowerCase)
    {
        var builder = new StringBuilder();
        var random = new Random();
        for (int i = 0; i < size; i++)
        {
            char ch = Convert.ToChar(Convert.ToInt32(26 * random.NextDouble() + 65));
            builder.Append(ch);
        }

        var result = builder.ToString();
        return lowerCase ? result.ToLower() : result;
    }

    public string GenerateRandomCode()
    {
        var builder = new StringBuilder();
        builder.Append(RandomString(1, true));
        builder.Append(RandomNumber(1, 9));
        builder.Append(RandomString(1, true));
        builder.Append(RandomNumber(1, 9));
        builder.Append(RandomString(1, true));
        return builder.ToString();
    }

    public string myMsgBx(string sMessage)
    {
        var msg = new StringBuilder();
        msg.Append("<script language='javascript'>");
        msg.Append("alert('" + sMessage + "');");
        msg.Append("</script>");
        return msg.ToString();
    }

    public string ClrAllCtrl()
    {
        var msg = new StringBuilder();
        msg.Append("<script language='javascript'>");
        msg.Append(" rstCtrl(); ");
        msg.Append("</script>");
        return msg.ToString();
    }

    public void WriteToFile(string text)
    {
        try
        {
            string path = HttpContext.Current.Server.MapPath("~/images/ErrorLog.txt");
            string compId = HttpContext.Current.Session["CompID"] != null ? HttpContext.Current.Session["CompID"].ToString() : "";
            string str = " insert Into TrnErrorLog(Pth,txt) Values('" + path.Replace("'", "''") + "','" + (text + "--CompID = " + compId).Replace("'", "''") + "')";
            int i = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session["MlmDatabase" + compId].ToString(), CommandType.Text, str);
        }
        catch (Exception)
        {
            // swallow exception as original VB
        }
    }

    public string GetConnectionByComp()
    {
        DataSet ds = new DataSet();
        string msg;
        string CompID = "1108";
        //string str = " Exec Proc_GetConnection1 '" + CompID + "' ";
        //Connection.ConnectionString = ConfigurationManager.ConnectionStrings["sconstr"].ConnectionString;

        //objSQlConnection = new SqlConnection(Connection.ConnectionString);
        //ds = SqlHelper.ExecuteDataset(objSQlConnection, CommandType.Text, str);
        msg = "Data Source =103.193.74.91,1533;Initial Catalog=darju9;Integrated Security=false;User ID=usrdarju9;PWD=Ju9!7@#DrVsh;Max Pool Size=2000;Pooling=true";
        HttpContext.Current.Session["MlmDatabase" + CompID] = msg;
        HttpContext.Current.Session["MlmDatabaseName" + CompID] = "darju9";
        HttpContext.Current.Session["MlmSelectDatabase" + CompID] = "Data Source =103.193.74.91,1533;Initial Catalog=darju9select;Integrated Security=false;User ID=usrdarju9;PWD=Ju9!7@#DrVsh;Max Pool Size=2000;Pooling=true";
        HttpContext.Current.Session["MlmSelectDatabaseName" + CompID] = "darju9select";
        try
        {
            string str1 = "IF object_id('TrnTemp') IS NULL Create Table TrnTemp ( ID int identity(1,1),Formno int not null, transNo Numeric (18,0) Primary Key, Remark Nvarchar(100) , Rectimestamp Datetime Default(Getdate()))";
            int i = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session["MlmDatabase" + CompID].ToString(), CommandType.Text, str1);
        }
        catch (Exception)
        {
            // swallow
        }

        try
        {
            string str2 = "IF object_id('TrnErrorLog') IS NULL Create table TrnErrorLog ( Id int Identity(1,1), Pth nvarchar(500), txt nvarchar(4000), Rectimestamp Datetime Default Getdate())";
            int i = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session["MlmDatabase" + CompID].ToString(), CommandType.Text, str2);
        }
        catch (Exception)
        {
            // swallow
        }

        return msg;
    }

    public string GetInvDataBaseByComp()
    {
        DataSet ds = new DataSet();
        string msg;
        string CompID = "1108";
        //string str = " Exec Proc_GetConnection1 '" + CompID + "' ";
        //Connection.ConnectionString = ConfigurationManager.ConnectionStrings["sconstr"].ConnectionString;

        //objSQlConnection = new SqlConnection(Connection.ConnectionString);
        //ds = SqlHelper.ExecuteDataset(objSQlConnection, CommandType.Text, str);
        msg = "Data Source =103.193.74.91,1533;Initial Catalog=darju9;Integrated Security=false;User ID=usrdarju9;PWD=Ju9!7@#DrVsh;Max Pool Size=2000;Pooling=true";
        HttpContext.Current.Session["MlmDatabase" + CompID] = msg;
        HttpContext.Current.Session["MlmDatabaseName" + CompID] = "darju9";
        HttpContext.Current.Session["MlmSelectDatabase" + CompID] = "Data Source =103.193.74.91,1533;Initial Catalog=darju9select;Integrated Security=false;User ID=usrdarju9;PWD=Ju9!7@#DrVsh;Max Pool Size=2000;Pooling=true";
        HttpContext.Current.Session["MlmSelectDatabaseName" + CompID] = "darju9select";
        //string msg;
        //string CompID = HttpContext.Current.Session["CompID"] != null ? HttpContext.Current.Session["CompID"].ToString() : "";
        //string str = " Exec Proc_GetConnection1 '" + CompID + "' ";
        //Connection.ConnectionString = ConfigurationManager.ConnectionStrings["sconstr"].ConnectionString;
        //objSQlConnection = new SqlConnection(Connection.ConnectionString);
        //ds = SqlHelper.ExecuteDataset(objSQlConnection, CommandType.Text, str);
        //msg = ds.Tables[1].Rows[0]["DatabaseName"].ToString();
        //HttpContext.Current.Session["InvDatabase" + CompID] = msg;
        return msg;
    }

    public bool RegTrans(string TransID, string CompID, string Formno = "0")
    {
        bool @bool = false;
        try
        {
            int i = 0;
            string str = " insert Into TrnTemp(transNo,Formno) Values('" + TransID.Replace("'", "''") + "', '" + Formno.Replace("'", "''") + "')";
            i = SqlHelper.ExecuteNonQuery(HttpContext.Current.Session["MlmDatabase" + CompID].ToString(), CommandType.Text, str);
            if (i == 1)
            {
                @bool = true;
            }
            else
            {
                @bool = false;
            }
        }
        catch (Exception)
        {
            // swallow
        }
        return @bool;
    }
}
