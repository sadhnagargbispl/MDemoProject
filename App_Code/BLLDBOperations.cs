using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for BLLDBOperations
/// </summary>
public class BLLDBOperations
{


    DbProviderFactory provider;
    protected SqlConnection ConnStr = new SqlConnection();

    decimal MinimumCharges;
    //public ClassOperations()
    //{
    //    provider = DbProviderFactories.GetFactory("System.Data.SqlClient");
    //}

    public BLLDBOperations()
    {
        provider = DbProviderFactories.GetFactory("System.Data.SqlClient");
        if (ConnStr.State.ToString() == "Open")
        {
            ConnStr.Close();
        }
        try
        {
            ConnStr.ConnectionString = ConfigurationManager.ConnectionStrings["sqlConn"].ToString();
            // ConnStr.ConnectionString = ConfigurationManager.AppSettings["Mycon"].ToString();
        }
        catch (Exception er)
        {
            HttpContext.Current.Response.Write(er.Message);
        }
    }
    public BLLDBOperations(string SqlConn)
    {
        provider = DbProviderFactories.GetFactory("System.Data.SqlClient");
        if (ConnStr.State.ToString() == "Open")
        {
            ConnStr.Close();
        }
        try
        {
            ConnStr.ConnectionString = SqlConn;
            // ConnStr.ConnectionString = ConfigurationManager.AppSettings["Mycon"].ToString();
        }
        catch (Exception er)
        {
            HttpContext.Current.Response.Write(er.Message);
        }
    }

    protected void OpenConnection()
    {
        if (ConnStr.State.ToString() == "Closed")
        {
            ConnStr.Open();
        }
    }

    protected void CloseConnection()
    {
        if (ConnStr.State.ToString() == "Open")
            ConnStr.Close();
    }


    private void AttachParameters(DbCommand command, Hashtable hstParameters)
    {
        try
        {
            IEnumerator parameters = hstParameters.Keys.GetEnumerator();
            while (parameters.MoveNext())
            {
                DbParameter param = provider.CreateParameter();
                param.ParameterName = "@" + parameters.Current;
                param.Value = hstParameters[parameters.Current];
                command.Parameters.Add(param);
            }

        }
        catch (Exception exception)
        {
            HttpContext.Current.Response.Write("Error : " + exception.Message + " </br> Source : " + exception.Source);
        }
    }

    private DbCommand CreateDbCommand(String commandText, CommandType commandType)
    {
        DbCommand command = provider.CreateCommand();
        try
        {
            command.CommandTimeout = 0;
            command.Connection = ConnStr;
            command.CommandText = commandText;
            command.CommandType = commandType;
        }
        catch
        {
            // HttpContext.Current.Response.Write("Error : " + exception.Message + " </br> Source : " + exception.Source);
        }
        return command;
    }

    public DataSet GetDataSet(String commandText, CommandType commandType, Hashtable hstParameters, String tableName)
    {
        DbCommand command = (DbCommand)CreateDbCommand(commandText, commandType);
        command.CommandTimeout = 0;
        if (hstParameters != null)
            if (hstParameters.Count > 0)
                AttachParameters(command, hstParameters);

        DataSet dataset = new DataSet();

        DbDataAdapter adapter = provider.CreateDataAdapter();
        adapter.SelectCommand = command;
        if (tableName != "" && tableName != null)
            adapter.Fill(dataset, tableName);
        else
            adapter.Fill(dataset);
        return dataset;
    }
    public DataSet GetDataSet(String commandText, CommandType commandType, Hashtable hstParameters)
    {
        return GetDataSet(commandText, commandType, hstParameters, null);
    }
    public DataSet GetDataSet(String commandText, CommandType commandType, String tableName)
    {
        return GetDataSet(commandText, commandType, null, tableName);
    }
    public DataSet GetDataSet(String commandText, CommandType commandType)
    {
        return GetDataSet(commandText, commandType, null, null);
    }
    public DataSet GetDataSet(String commandText, String tableName)
    {
        return GetDataSet(commandText, CommandType.Text, null, tableName);
    }
    public DataSet GetDataSet(String commandText)
    {
        return GetDataSet(commandText, CommandType.Text, null, null);
    }

    public DataTable GetDataTable(String commandText, CommandType commandType, Hashtable hstParameters)
    {
        DbCommand command = (DbCommand)CreateDbCommand(commandText, commandType);
        command.CommandTimeout = 0;
        if (hstParameters != null)
            if (hstParameters.Count > 0)
                AttachParameters(command, hstParameters);

        DataTable dt = new DataTable();
        DbDataAdapter adapter = provider.CreateDataAdapter();
        adapter.SelectCommand = command;
        adapter.Fill(dt);
        return dt;
    }
    public DataTable GetDataTable(String commandText, CommandType commandType)
    {
        return GetDataTable(commandText, commandType, null);
    }
    public DataTable GetDataTable(String commandText)
    {
        return GetDataTable(commandText, CommandType.Text, null);
    }

    public DbDataReader GetDataReader(String commandText, CommandType commandType, Hashtable hstParameters)
    {
        DbCommand command = (DbCommand)CreateDbCommand(commandText, commandType);
        command.Connection = ConnStr;
        command.CommandText = commandText;
        command.CommandType = commandType;

        if (hstParameters != null)
            AttachParameters(command, hstParameters);

        DbDataReader dr;
        CloseConnection();
        OpenConnection();
        dr = command.ExecuteReader();
        return dr;
    }
    public DbDataReader GetDataReader(String commandText, CommandType commandType)
    {
        return GetDataReader(commandText, commandType, null);
    }
    public DbDataReader GetDataReader(String commandText)
    {
        return GetDataReader(commandText, CommandType.Text, null);
    }

    public Int32 ExecuteNonQuery(String commandText, CommandType commandType, Hashtable hstParameters)
    {
        DbCommand command = (DbCommand)CreateDbCommand(commandText, commandType);
        command.Connection = ConnStr;
        command.CommandText = commandText;
        command.CommandType = commandType;

        if (hstParameters != null)
            if (hstParameters.Count > 0)
                AttachParameters(command, hstParameters);

        Int32 iRowsAffected = 0;

        OpenConnection();
        iRowsAffected = command.ExecuteNonQuery();
        CloseConnection();

        return iRowsAffected;
    }
    public Int32 ExecuteNonQuery(String commandText, CommandType commandType)
    {
        return ExecuteNonQuery(commandText, commandType, null);
    }
    public Int32 ExecuteNonQuery(String commandText)
    {
        return ExecuteNonQuery(commandText, CommandType.Text, null);
    }

    public Object ExecuteScalar(String commandText, CommandType commandType, Hashtable hstParameters)
    {
        DbCommand command = (DbCommand)CreateDbCommand(commandText, commandType);
        command.Connection = ConnStr;
        command.CommandText = commandText;
        command.CommandType = commandType;
        command.CommandTimeout = 0;

        if (hstParameters != null)
            if (hstParameters.Count > 0)
                AttachParameters(command, hstParameters);

        Object result = null;

        OpenConnection();
        result = command.ExecuteScalar();
        CloseConnection();

        return result;
    }
    public Object ExecuteScalar(String commandText, CommandType commandType)
    {
        return ExecuteScalar(commandText, commandType, null);
    }
    public Object ExecuteScalar(String commandText)
    {
        return ExecuteScalar(commandText, CommandType.Text, null);
    }

    public RadioButtonList FillRadioButtonListing(RadioButtonList RBL, string commandText, Hashtable hstParameters, string strFieldText, string strFieldValue)
    {
        RBL.Items.Clear();

        DbCommand command = (DbCommand)CreateDbCommand(commandText, CommandType.Text);
        command.Connection = ConnStr;
        command.CommandText = commandText;
        command.CommandType = CommandType.Text;

        if (hstParameters != null)
            if (hstParameters.Count > 0)
                AttachParameters(command, hstParameters);

        DataSet dataset = new DataSet();
        try
        {
            DbDataAdapter adapter = provider.CreateDataAdapter();
            adapter.SelectCommand = command;
            adapter.Fill(dataset);

            RBL.DataSource = dataset;
            RBL.DataTextField = strFieldText;
            RBL.DataValueField = strFieldValue;
            RBL.DataBind();
            RBL.Items[0].Selected = true;
        }
        catch (Exception ex)
        {
            //  HttpContext.Current.Response.Write("Error : " + ex.Message);
        }
        dataset.Dispose();

        return RBL;
    }
    public RadioButtonList FillRadioButtonList(RadioButtonList RBL, string commandText, string strFieldText, string strFieldValue)
    {
        return FillRadioButtonListing(RBL, commandText, null, strFieldText, strFieldValue);
    }

    public DropDownList FillDropDownList(DropDownList DDL, string commandText, CommandType commandType, Hashtable hstParameters, string strFieldText, string strFieldValue)
    {
        return FillDropDownList(DDL, commandText, commandType, hstParameters, strFieldText, strFieldValue, true);
    }
    public DropDownList FillDropDownList(DropDownList DDL, string commandText, CommandType commandType, Hashtable hstParameters, string strFieldText, string strFieldValue, bool IsDefault)
    {
        DDL.Items.Clear();

        DbCommand command = (DbCommand)CreateDbCommand(commandText, commandType);
        command.Connection = ConnStr;
        command.CommandText = commandText;
        command.CommandType = commandType;

        if (hstParameters != null)
            if (hstParameters.Count > 0)
                AttachParameters(command, hstParameters);

        DataSet dataset = new DataSet();
        try
        {
            DbDataAdapter adapter = provider.CreateDataAdapter();
            adapter.SelectCommand = command;
            adapter.Fill(dataset);

            DDL.DataSource = dataset;
            DDL.DataTextField = strFieldText;
            DDL.DataValueField = strFieldValue;
            DDL.DataBind();
            if (IsDefault)
                DDL.Items.Insert(0, new ListItem("Select One", "0"));
        }
        catch (Exception ex)
        {
            //  HttpContext.Current.Response.Write("Error : " + ex.Message);
        }
        dataset.Dispose();

        return DDL;
    }
    public DropDownList FillDropDownList(DropDownList DDL, string commandText, CommandType commandType, string strFieldText, string strFieldValue, bool IsDefatul)
    {
        return FillDropDownList(DDL, commandText, commandType, null, strFieldText, strFieldValue, IsDefatul);
    }
    public DropDownList FillDropDownList(DropDownList DDL, string commandText, CommandType commandType, string strFieldText, string strFieldValue)
    {
        return FillDropDownList(DDL, commandText, commandType, null, strFieldText, strFieldValue);
    }
    public DropDownList FillDropDownList(DropDownList DDL, string commandText, string strFieldText, string strFieldValue, bool IsDefatul)
    {
        return FillDropDownList(DDL, commandText, CommandType.Text, null, strFieldText, strFieldValue, IsDefatul);
    }
    public DropDownList FillDropDownList(DropDownList DDL, string commandText, string strFieldText, string strFieldValue)
    {
        return FillDropDownList(DDL, commandText, CommandType.Text, null, strFieldText, strFieldValue, true);
    }
    public ListBox FillListBox(ListBox LBX, string commandText, CommandType commandType, Hashtable hstParameters, string strFieldText, string strFieldValue)
    {
        LBX.Items.Clear();

        DbCommand command = (DbCommand)CreateDbCommand(commandText, commandType);
        command.Connection = ConnStr;
        command.CommandText = commandText;
        command.CommandType = commandType;

        if (hstParameters != null)
            if (hstParameters.Count > 0)
                AttachParameters(command, hstParameters);

        DataSet dataset = new DataSet();
        try
        {
            DbDataAdapter adapter = provider.CreateDataAdapter();
            adapter.SelectCommand = command;
            adapter.Fill(dataset);
            LBX.DataSource = dataset;
            LBX.DataTextField = strFieldText;
            LBX.DataValueField = strFieldValue;
            LBX.DataBind();
            if (LBX.ID == "lbReason2" && dataset.Tables.Count <= 0)
            {
                LBX.DataSource = null;
                LBX.DataBind();
                LBX.Visible = false;
            }
        }
        catch (Exception ex)
        {
            //   HttpContext.Current.Response.Write("Error : " + ex.Message);
        }
        dataset.Dispose();

        return LBX;
    }
    public ListBox FillListBox(ListBox LBX, string commandText, CommandType commandType, string strFieldText, string strFieldValue)
    {
        return FillListBox(LBX, commandText, commandType, null, strFieldText, strFieldValue);
    }
    public ListBox FillListBox(ListBox LBX, string commandText, string strFieldText, string strFieldValue)
    {
        return FillListBox(LBX, commandText, CommandType.Text, null, strFieldText, strFieldValue);
    }


    public bool BulkEnterData(DataTable Dt, string tblName)
    {
        SqlBulkCopy bulk = new SqlBulkCopy(ConnStr);
        bulk.DestinationTableName = tblName;
        if (ConnStr.State == ConnectionState.Closed)
        {
            ConnStr.Open();
        }
        bulk.WriteToServer(Dt);
        ConnStr.Close();
        return true;

    }
    public bool XMLImportBulkData(DataTable Dt, string tblName)
    {
        try
        {
            SqlBulkCopy bulk = new SqlBulkCopy(ConnStr);
            bulk.DestinationTableName = tblName;

            bulk.ColumnMappings.Add(Dt.Columns["uCompId"].ToString(), "uCompId");
            bulk.ColumnMappings.Add(Dt.Columns["nClientId"].ToString(), "nClientId");
            bulk.ColumnMappings.Add(Dt.Columns["finYear"].ToString(), "finYear");
            bulk.ColumnMappings.Add(Dt.Columns["TagName"].ToString(), "TagName");
            bulk.ColumnMappings.Add(Dt.Columns["Value"].ToString(), "TagValue");
            bulk.ColumnMappings.Add(Dt.Columns["ReportType"].ToString(), "ReportType");
            bulk.ColumnMappings.Add(Dt.Columns["TagId"].ToString(), "TagId");
            bulk.ColumnMappings.Add(Dt.Columns["ContextRef"].ToString(), "ContextRef");
            bulk.ColumnMappings.Add(Dt.Columns["AxisName"].ToString(), "Axis1");
            bulk.ColumnMappings.Add(Dt.Columns["AxisName1"].ToString(), "Member1");
            bulk.ColumnMappings.Add(Dt.Columns["AxisName2"].ToString(), "Axis2");
            bulk.ColumnMappings.Add(Dt.Columns["AxisName3"].ToString(), "Member2");
            bulk.ColumnMappings.Add(Dt.Columns["AxisName4"].ToString(), "Axis3");
            bulk.ColumnMappings.Add(Dt.Columns["AxisName5"].ToString(), "Member3");
            if (ConnStr.State.ToString() == "Open")
            {
                ConnStr.Close();
            }
            ConnStr.Open();
            bulk.WriteToServer(Dt);
            ConnStr.Close();
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }
    public bool BulkEnterDataMapping(DataTable Dt, string tblName)
    {
        SqlBulkCopy bulk = new SqlBulkCopy(ConnStr);
        bulk.DestinationTableName = tblName;
        string strSql = "";
        string strExcel = "";
        foreach (DataColumn dc in Dt.Columns)
        {

            if (dc.ColumnName.ToLower() == "sfinyear") continue;
            if (dc.ColumnName.ToLower() == "validate") continue;

            if (dc.ColumnName.ToLower() == "sfinyear1")
            {
                bulk.ColumnMappings.Add("sFinYear1", "sFinYear");
                strSql = strSql + "," + "sFinYear";
                strExcel = strExcel + "," + "sFinYear1";
            }
            else if (dc.ColumnName.ToLower() == "amountofissueallottedforcontractswithoutpaymentreceivedincashdur" && tblName.ToLower() == "tblMCA101600_DetailsOfEveryClassOfShareCapitalIssuedDuringPeriod".ToLower())  // Excel Table Hold Only 64 Char Name In Column
            {
                bulk.ColumnMappings.Add(dc.ColumnName, "AmountOfIssueAllottedForContractsWithoutPaymentReceivedInCashDuringPeriod");
                strSql = strSql + "," + "AmountOfIssueAllottedForContractsWithoutPaymentReceivedInCashDuringPeriod";
                strExcel = strExcel + "," + dc.ColumnName;
            }
            else if (dc.ColumnName.ToLower() == "reasonforfollowingdifferenttreatmentforreservesfromprescribedin" && tblName.ToLower() == "tblMCA400700_DetailsOfAmalgamation".ToLower())  // Excel Table Hold Only 64 Char Name In Column
            {
                bulk.ColumnMappings.Add(dc.ColumnName, "ReasonForFollowingDifferentTreatmentForReservesFromPrescribedInStatementOfAmalgamatingCompany");
                strSql = strSql + "," + "ReasonForFollowingDifferentTreatmentForReservesFromPrescribedInStatementOfAmalgamatingCompany";
                strExcel = strExcel + "," + dc.ColumnName;
            }
            else if (dc.ColumnName.ToLower() == "DeviationInAccountingTreatmentOfReservesFollowedAsPrescribedInSt".ToLower() && tblName.ToLower() == "tblMCA400700_DetailsOfAmalgamation".ToLower())  // Excel Table Hold Only 64 Char Name In Column
            {
                bulk.ColumnMappings.Add(dc.ColumnName, "DeviationInAccountingTreatmentOfReservesFollowedAsPrescribedInStatute");
                strSql = strSql + "," + "DeviationInAccountingTreatmentOfReservesFollowedAsPrescribedInSt";
                strExcel = strExcel + "," + dc.ColumnName;
            }
            else if (dc.ColumnName.ToLower() == "DetailsOfProportionsOfItemsForWhichDifferentAccountingPoliciesFo".ToLower() && tblName.ToLower() == "tblMCA401600_DetailsOfInvestmentsInJointVentures".ToLower())  // Excel Table Hold Only 64 Char Name In Column
            {
                bulk.ColumnMappings.Add(dc.ColumnName, "DetailsOfProportionsOfItemsForWhichDifferentAccountingPoliciesFollowedByJointVenture");
                strSql = strSql + "," + "DetailsOfProportionsOfItemsForWhichDifferentAccountingPoliciesFo";
                strExcel = strExcel + "," + dc.ColumnName;
            }
            else if (dc.ColumnName.ToLower() == "ContingentLiabilitiesIncurredByVenturerInRelationToInterestsInJo".ToLower() && tblName.ToLower() == "tblMCA401600_DetailsOfInvestmentsInJointVentures".ToLower())  // Excel Table Hold Only 64 Char Name In Column
            {
                bulk.ColumnMappings.Add(dc.ColumnName, "ContingentLiabilitiesIncurredByVenturerInRelationToInterestsInJointVentures");
                strSql = strSql + "," + "ContingentLiabilitiesIncurredByVenturerInRelationToInterestsInJo";
                strExcel = strExcel + "," + dc.ColumnName;
            }
            else if (dc.ColumnName.ToLower() == "ContingentLiabilitiesForWhichrVenturersAreLiableForLiabilitiesOf".ToLower() && tblName.ToLower() == "tblMCA401600_DetailsOfInvestmentsInJointVentures".ToLower())  // Excel Table Hold Only 64 Char Name In Column
            {
                bulk.ColumnMappings.Add(dc.ColumnName, "ContingentLiabilitiesForWhichrVenturersAreLiableForLiabilitiesOfOtherVenturers");
                strSql = strSql + "," + "ContingentLiabilitiesForWhichrVenturersAreLiableForLiabilitiesOf";
                strExcel = strExcel + "," + dc.ColumnName;
            }
            else if (dc.ColumnName.ToLower() == "WhetherFinancialYearofSubsidiaryCoincidesWithFinancialYearofHold".ToLower() && tblName.ToLower() == "tblMCA401300_DetailsOfSubsidiaries".ToLower())  // Excel Table Hold Only 64 Char Name In Column
            {
                bulk.ColumnMappings.Add(dc.ColumnName, "WhetherFinancialYearofSubsidiaryCoincidesWithFinancialYearofHoldingCompany");
                strSql = strSql + "," + "WhetherFinancialYearofSubsidiaryCoincidesWithFinancialYearofHold";
                strExcel = strExcel + "," + dc.ColumnName;
            }
            else if (dc.ColumnName.ToLower() == "AggregateAmountOfProfitLossOfSubsidiaryForPreviousYearsSinceItBe".ToLower() && tblName.ToLower() == "tblMCA401300_DetailsOfSubsidiaries".ToLower())  // Excel Table Hold Only 64 Char Name In Column
            {
                bulk.ColumnMappings.Add(dc.ColumnName, "AggregateAmountOfProfitLossOfSubsidiaryForPreviousYearsSinceItBecameSubsidiary");
                strSql = strSql + "," + "AggregateAmountOfProfitLossOfSubsidiaryForPreviousYearsSinceItBe";
                strExcel = strExcel + "," + dc.ColumnName;
            }
            else if (dc.ColumnName.ToLower() == "AggregateAmountOfSubsidiaryNotAccountedForPreviousYearsSinceItBe".ToLower() && tblName.ToLower() == "tblMCA401300_DetailsOfSubsidiaries".ToLower())  // Excel Table Hold Only 64 Char Name In Column
            {
                bulk.ColumnMappings.Add(dc.ColumnName, "AggregateAmountOfSubsidiaryNotAccountedForPreviousYearsSinceItBecameSubsidiary");
                strSql = strSql + "," + "AggregateAmountOfSubsidiaryNotAccountedForPreviousYearsSinceItBe";
                strExcel = strExcel + "," + dc.ColumnName;
            }
            else if (dc.ColumnName.ToLower() == "AggregateAmountOfSubsidiaryAccountedForPreviousYearsSinceItBecam".ToLower() && tblName.ToLower() == "tblMCA401300_DetailsOfSubsidiaries".ToLower())  // Excel Table Hold Only 64 Char Name In Column
            {
                bulk.ColumnMappings.Add(dc.ColumnName, "AggregateAmountOfSubsidiaryAccountedForPreviousYearsSinceItBecameSubsidiary");
                strSql = strSql + "," + "AggregateAmountOfSubsidiaryAccountedForPreviousYearsSinceItBecam";
                strExcel = strExcel + "," + dc.ColumnName;
            }
            else if (dc.ColumnName.ToLower() == "DetailsOfAssetsLiabilitiesOfSubsidiaryIncludedInConsolidatedStat".ToLower() && tblName.ToLower() == "tblMCA401300_DetailsOfSubsidiaries".ToLower())  // Excel Table Hold Only 64 Char Name In Column
            {
                bulk.ColumnMappings.Add(dc.ColumnName, "DetailsOfAssetsLiabilitiesOfSubsidiaryIncludedInConsolidatedStatements");
                strSql = strSql + "," + "DetailsOfAssetsLiabilitiesOfSubsidiaryIncludedInConsolidatedStat";
                strExcel = strExcel + "," + dc.ColumnName;
            }
            else if (dc.ColumnName.ToLower() == "NatureOfRelationshipWithSubsidiaryWhereParentHasDirectlyOrIndire".ToLower() && tblName.ToLower() == "tblMCA401300_DetailsOfSubsidiaries".ToLower())  // Excel Table Hold Only 64 Char Name In Column
            {
                bulk.ColumnMappings.Add(dc.ColumnName, "NatureOfRelationshipWithSubsidiaryWhereParentHasDirectlyOrIndirectlyLessThanHalfOfVotingPower");
                strSql = strSql + "," + "NatureOfRelationshipWithSubsidiaryWhereParentHasDirectlyOrIndire";
                strExcel = strExcel + "," + dc.ColumnName;
            }
            else
            {
                bulk.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                strSql = strSql + "," + dc.ColumnName;
                strExcel = strExcel + "," + dc.ColumnName;
            }
        }
        if (ConnStr.State.ToString() == "Open")
        {
            ConnStr.Close();
        }
        ConnStr.Open();
        bulk.WriteToServer(Dt);
        ConnStr.Close();
        return true;

    }
    public DataTable CreateDataTable(string tblName, params object[] Parame)
    {
        DataTable oDt = new DataTable();
        if (Parame.Length > 0 && Parame.Length % 2 == 0)
        {
            oDt.TableName = tblName;
            for (int cnt = 0; cnt < Parame.Length; cnt += 2)
            {
                DataColumn dc = new DataColumn(Parame[cnt].ToString());
                dc.DataType = Type.GetType("System." + Parame[cnt + 1].ToString());
                oDt.Columns.Add(dc);
            }
        }
        return oDt;
    }

    public void AddRow(DataTable Dt, params object[] Params)
    {
        if (Dt == null)
            return;

        DataRow Dr = Dt.NewRow();
        for (int cnt = 0; cnt < Params.Length; cnt++)
        {
            Dr[cnt] = Params[cnt];
        }
        Dt.Rows.Add(Dr);
    }

    public Int32 Round(Int32 Val)
    {
        if (Val <= 0)
            return Val;

        int reminder = Val % 10;
        if (reminder > 4)
            Val = Val + (10 - reminder);
        else
            Val = Val - reminder;

        return Val;
    }

    public void DeleteOneDayBeforeFiles()
    {
        try
        {
            DirectoryInfo dirinfo = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/attachments"));
            FileInfo[] fileinfo = dirinfo.GetFiles("*.*", SearchOption.AllDirectories);

            Array.Sort(fileinfo, delegate (FileInfo f1, FileInfo f2)
            {
                return f1.LastWriteTime.CompareTo(f2.LastWriteTime);
            });

            foreach (FileInfo fi in fileinfo)
            {
                if (fi.LastWriteTime < System.DateTime.Now.AddDays(-1))
                {
                    fi.Delete();
                }
                else
                {
                    break;
                }
            }
            DirectoryInfo[] dd = dirinfo.GetDirectories("*.*", SearchOption.AllDirectories);
            Array.Sort(dd, delegate (DirectoryInfo d1, DirectoryInfo d2)
            {
                return d1.CreationTime.CompareTo(d2.CreationTime);
            });

            foreach (DirectoryInfo d1 in dd)
            {
                if (d1.CreationTime < System.DateTime.Now.AddDays(-1))
                {
                    if (d1.Exists)
                    {
                        FileInfo[] F1 = d1.GetFiles("*.*", SearchOption.AllDirectories);
                        if (F1.Length == 0)
                        {
                            d1.Delete(true);
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public long DeleteAllTempfile(string beforeHours)
    {
        if (string.IsNullOrEmpty(beforeHours))
        {
            beforeHours = "0";
        }
        long numFiles = 1;
        double hours = -(Convert.ToDouble(beforeHours));
        // double hours = Convert.ToDouble(beforeHours);
        try
        {
            DirectoryInfo dirinfo = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/attachments"));
            FileInfo[] fileinfo = dirinfo.GetFiles("*.*", SearchOption.AllDirectories);
            numFiles = fileinfo.Length;

            Array.Sort(fileinfo, delegate (FileInfo f1, FileInfo f2)
            {
                return f1.LastWriteTime.CompareTo(f2.LastWriteTime);
            });

            foreach (FileInfo fi in fileinfo)
            {
                if (fi.LastWriteTime < System.DateTime.Now.AddHours(hours))
                {
                    fi.Delete();
                }
                else
                {
                    break;
                }
            }
            DirectoryInfo[] dd = dirinfo.GetDirectories("*.*", SearchOption.AllDirectories);
            Array.Sort(dd, delegate (DirectoryInfo d1, DirectoryInfo d2)
            {
                return d1.CreationTime.CompareTo(d2.CreationTime);
            });

            foreach (DirectoryInfo d1 in dd)
            {
                if (d1.CreationTime < System.DateTime.Now.AddHours(hours))
                {
                    if (d1.Exists)
                    {
                        FileInfo[] F1 = d1.GetFiles("*.*", SearchOption.AllDirectories);
                        if (F1.Length == 0)
                        {
                            d1.Delete(true);
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }
        catch (Exception ex)
        {

        }
        return numFiles;
    }
    //public string[] GetName(string fullName);
}