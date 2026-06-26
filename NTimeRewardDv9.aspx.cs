using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NTimeRewardDv9 : System.Web.UI.Page
{
    SqlConnection Conn;
    SqlCommand Comm;
    DataTable Dt;
    SqlDataAdapter Ad;
    string str = "";

    // DAL obj;
    DAL Obj;
    clsGeneral objGen = new clsGeneral();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                Obj = new DAL();
                if (!Page.IsPostBack)
                {
                    AchievePair();
                    AchieveReward();
                    NextReward();
                    PendingReward();
                }
            }
            else
            {
                Response.Redirect("logout.aspx");
                return;
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void PendingReward()
    {
        try
        {
            if (Session["CompID"].ToString() == "1108")
            {
                div4.Visible = true;
                divpendingreward.Visible = false;
            }
            else
            {
                div4.Visible = false;
                divpendingreward.Visible = true;
            }
            string str = "select * from MstBonanzaAchievers where formno='" + Session["Formno"] + "'";
            Dt = new DataTable();
            Dt = Obj.GetData(str);

            string sql;

            if (Dt.Rows.Count > 0)
            {
                //sql = " select Rank,Reward,Amount,Cast(RDays as int) as RDays ," +
                //      " 0 as RemainingDays," +
                //      " Notimelimit,newpair from MstRewards as a " +
                //      " Left Join M_membermaster as c On c.Formno='" + Session["FormNo"] + "' where " +
                //      " RewardId Not In(Select Distinct(Rewardid)+1 " +
                //      " from M_RewardFinal where Formno='" + Session["FormNo"] + "' Group by Formno,Rewardid ) and RewardId>1 ";


                 sql = "select Rank,Reward,Amount, 0 as  Cummrdays,0 as Notimelimit," +
             "0 as RemainingDays,0 as Rdays  ,a.NewPair ,c.ActiveStatus" +
             " from MstRewards as a " +
             " Left Join M_membermaster as c On c.Formno='" + Session["FormNo"] + "' where " +
             " RewardId Not In(Select Distinct(Rewardid)+1 " +
             " from MstBonanzaAchievers where Formno='" + Session["FormNo"] + "' Group by Formno,Rewardid ) and RewardId>1 ";

                Dt = new DataTable();
                Dt = Obj.GetData(sql);

                if (Dt.Rows.Count > 0)
                {
                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            Dt.Rows[i]["RemainingDays"] =
                                Convert.ToInt32(Session["RemainDays"]) +
                                Convert.ToInt32(Dt.Rows[i]["Rdays"]);
                        }
                        else
                        {
                            Dt.Rows[i]["RemainingDays"] =
                                Convert.ToInt32(Dt.Rows[i - 1]["RemainingDays"]) +
                                Convert.ToInt32(Dt.Rows[i]["Rdays"]);
                        }
                    }

                    Session["RemainDays"] = "0";
                    if (Session["CompID"].ToString() == "1108")
                    {
                        GridView1.DataSource = Dt;
                        GridView1.DataBind();
                    }
                    else
                    {

                        GrdPending.DataSource = Dt;
                        GrdPending.DataBind();
                    }

                }
                else
                {
                    sql = "select * from M_Rewardmaster where Rewardid>1";

                    Dt = new DataTable();
                    Dt = Obj.GetData(sql);
                    if (Session["CompID"].ToString() == "1108")
                    {
                        GridView1.DataSource = Dt;
                        GridView1.DataBind();
                    }
                    else
                    {
                        GrdPending.DataSource = Dt;
                        GrdPending.DataBind();

                    }

                }
            }
            else
            {
                string sql1 = "select Rank,Reward,Amount, 0 as  Cummrdays,0 as Notimelimit," +
                             "0 as RemainingDays  ,a.NewPair ,c.ActiveStatus" +
                             " from MstRewards as a " +
                             " Left Join M_membermaster as c On c.Formno='" + Session["FormNo"] + "' where " +
                             " RewardId Not In(Select Distinct(Rewardid)+1 " +
                             " from MstBonanzaAchievers where Formno='" + Session["FormNo"] + "' Group by Formno,Rewardid ) and RewardId>1 ";

                //string sql1 = " select Rank,Reward,Amount,Cast(RDays as int) as RDays ," +
                //              " Cast(CummRdays as Int) as Cummrdays, " +
                //              " 0 as RemainingDays, " +
                //              " Case when DateDiff(Day,c.UpgradeDate,Getdate())>Cast(CummRDays as int) then " +
                //              " 0 else Cast(CummRDays as int)-DateDiff(Day,c.UpgradeDate,Getdate()) end as RemainDays," +
                //              " Notimelimit,newpair,c.ActiveStatus from MstRewards as a " +
                //              " Left Join M_membermaster as c On c.Formno='" + Session["FormNo"] + "' where " +
                //              " RewardId Not In(Select Distinct(Rewardid)+1 " +
                //              " from M_RewardFinal where Formno='" + Session["FormNo"] + "' Group by Formno,Rewardid ) and RewardId>1 ";

                Dt = new DataTable();
                Dt = Obj.GetData(sql1);

                int k = 0;

                if (Dt.Rows.Count > 0)
                {
                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                        if (Dt.Rows[0]["activeStatus"].ToString() == "N")
                        {
                            Dt.Rows[i]["RemainingDays"] =
                                Convert.ToInt32(Dt.Rows[i]["CummRdays"]);
                        }
                        //else if (i == 0 &&
                        //         Convert.ToInt32(Dt.Rows[i]["RemainDays"]) == 0)
                        //{
                        //    Dt.Rows[i]["RemainingDays"] = 0;
                        //}
                        //else if (Convert.ToInt32(Dt.Rows[i]["RemainDays"]) > 0 && k == 0)
                        //{
                        //    Dt.Rows[i]["RemainingDays"] =
                        //        Convert.ToInt32(Session["RemainDays"]) +
                        //        Convert.ToInt32(Dt.Rows[i]["Remaindays"]);

                        //    k++;
                        //}
                        //else
                        //{
                        //    Dt.Rows[i]["RemainingDays"] =
                        //        Convert.ToInt32(Dt.Rows[i - 1]["RemainingDays"]) +
                        //        Convert.ToInt32(Dt.Rows[i]["Rdays"]);
                        //}
                    }

                    Session["RemainDays"] = "0";
                    if (Session["CompID"].ToString() == "1108")
                    {
                        GridView1.DataSource = Dt;
                        GridView1.DataBind();
                    }
                    else
                    {
                        GrdPending.DataSource = Dt;
                        GrdPending.DataBind();

                    }

                }
                else
                {
                    sql = "select * from MstRewards where Rewardid>1";

                    Dt = new DataTable();
                    Dt = Obj.GetData(sql);
                    if (Session["CompID"].ToString() == "1108")
                    {
                        GridView1.DataSource = Dt;
                        GridView1.DataBind();
                    }
                    else
                    {
                        GrdPending.DataSource = Dt;
                        GrdPending.DataBind();

                    }

                }
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff " + Environment.NewLine);

            Obj.WriteToFile(text + ex.Message);

            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }
    }

    private void NextReward()
    {
        try
        {
            string s = "Exec Sp_NextReward_DV9 '" + Session["Formno"] + "'";

            Dt = new DataTable();
            Dt = Obj.GetData(s);

            if (Dt.Rows.Count > 0)
            {
                //Session["RemainDays"] = Dt.Rows[0]["Remaining Days"];

                GrdNext.DataSource = Dt;
                GrdNext.DataBind();
            }
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff " + Environment.NewLine);

            Obj.WriteToFile(text + ex.Message);

            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }
    }

    private void AchievePair()
    {
        try
        {
            str = "select * from V#RpInfo where FormNo='" + Session["FormNo"] + "'";

            Dt = Obj.GetData(str);

            if (Session["CompID"].ToString() == "1108")
            {
                GrdRewardPair.Columns[0].HeaderText = "Left RP";
                GrdRewardPair.Columns[1].HeaderText = "Right RP";
            }
            else
            {
                GrdRewardPair.Columns[0].HeaderText = "Left PV";
                GrdRewardPair.Columns[1].HeaderText = "Right PV";
            }

            GrdRewardPair.DataSource = Dt;
            GrdRewardPair.DataBind();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff " + Environment.NewLine);

            Obj.WriteToFile(text + ex.Message);

            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }
    }

    private void AchieveReward()
    {
        try
        {
            str = "Exec Sp_AchievedReward_DV9 '" + Session["FormNo"] + "'";

            Dt = Obj.GetData(str);

            GrdRewards.DataSource = Dt;
            GrdRewards.DataBind();
        }
        catch (Exception ex)
        {
            string path = HttpContext.Current.Request.Url.AbsoluteUri;
            string text = path + ":  " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss:fff " + Environment.NewLine);

            Obj.WriteToFile(text + ex.Message);

            Response.Write("Try later.");
            Response.Write(ex.Message);
            Response.End();
        }
    }

    protected void Reedembtn(object sender, EventArgs e)
    {
        GridViewRow GridvwRow;
        string R = "";

        GridvwRow = (GridViewRow)((Control)sender).Parent.Parent;

        Label lblRewards = (Label)GridvwRow.FindControl("lblreward");

        string dl = "Update M_RewardFinal set IsRedeem='Y',Redeemdate=Getdate() " +
                    " where FormNo='" + Session["FormNo"] + "' and Rewardid='" +
                    Convert.ToInt32(lblRewards.Text) + "' ";

        int updateeffect = 0;

        string scrname = "";

        updateeffect = Obj.SaveData(dl);

        if (updateeffect != 0)
        {
            scrname = "<SCRIPT language='javascript'>alert('Redeem Successfully!');</SCRIPT>";
        }
        else
        {
            scrname = "<SCRIPT language='javascript'>alert('Redeem Unsuccessful! ');</SCRIPT>";
        }

        ScriptManager.RegisterClientScriptBlock(
            this.Page,
            this.GetType(),
            "Reward",
            scrname,
            false);

        AchieveReward();
    }
}