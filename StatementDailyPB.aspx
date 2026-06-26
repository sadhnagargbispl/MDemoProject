<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StatementDailyPB.aspx.cs" Inherits="StatementDailyPB" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="fren/bootstrap.min.css" rel="stylesheet" type="text/css" />

    <script src="fren/bootstrap.min.js" type="text/javascript"></script>

    <script src="fren/jquery.min.js" type="text/javascript"></script>

</head>
</head>
<body>
    <form id="form1" runat="server">

        <div class="container">
            <center>
                <div class="row" style="border: 1px solid black; width: 50pc;">
                    <div class="col-md-12">
                        <div class="row">
                            <table class="table-bordered" width="98%">
                                <tr>
                                    <td width="30%">
                                        <div class="col-md-12">
                                            <img src="Images/logo.jpg" style="width: 150px; float: left;" id="imglogo" runat="server" />
                                        </div>
                                    </td>
                                    <td width="70%">
                                        <div class="col-md-12" style="text-align: left">
                                            <b><%= Session["CompName"] %></b>
                                            <br />
                                            Address:- <%= Session["CompAdd"] %>
                                            <br />
                                            Email:- <%= Session["CompMail"] %>
                                            <br />
                                            Mobile No.:- <%= Session["CompMobile"] %>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="row">
                            <div class="col-md-12" style="background-color: #999999; text-align: center; color: White;">
                                <b><span style="font-size: 18px; font-family: Arial Baltic;">Income Statement </span>
                                </b>
                            </div>
                        </div>
                        <div class="row">
                            <table class="table-bordered" width="98%">
                                <tr>
                                    <td width="40%">
                                        <div class="col-md-12" style="text-align: center;">
                                            <b><span style="font-size: 16px; font-family: Arial Baltic;">Distributor Detail </span>
                                            </b>
                                            <div class="col-md-12">
                                                <table class="table-bordered" width="100%">
                                                    <tr>
                                                        <td style="text-align: left; width: 30%;">Name
                                                        </td>
                                                        <td style="text-align: center; width: 10%;">:
                                                        </td>
                                                        <td style="text-align: left; width: 60%;">
                                                            <div id="MemName" runat="server">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left; width: 30%;">IDNO
                                                        </td>
                                                        <td style="text-align: center; width: 10%;">:
                                                        </td>
                                                        <td style="text-align: left; width: 60%;">
                                                            <div id="IDNO" runat="server">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left; width: 30%;">Address
                                                        </td>
                                                        <td style="text-align: center; width: 10%;">:
                                                        </td>
                                                        <td style="text-align: left; width: 60%;">
                                                            <div id="Add" runat="server">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left; width: 30%;">Mob. No.
                                                        </td>
                                                        <td style="text-align: center; width: 10%;">:
                                                        </td>
                                                        <td style="text-align: left; width: 60%;">
                                                            <div id="Mobile" runat="server">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left; width: 30%;">City
                                                        </td>
                                                        <td style="text-align: center; width: 10%;">:
                                                        </td>
                                                        <td style="text-align: left; width: 60%;">
                                                            <div id="City" runat="server">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left; width: 30%;">District
                                                        </td>
                                                        <td style="text-align: center; width: 10%;">:
                                                        </td>
                                                        <td style="text-align: left; width: 60%;">
                                                            <div id="District" runat="server">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left; width: 30%;">Pin Code
                                                        </td>
                                                        <td style="text-align: center; width: 10%;">:
                                                        </td>
                                                        <td style="text-align: left; width: 60%;">
                                                            <div id="PinCode" runat="server">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left; width: 30%;">State
                                                        </td>
                                                        <td style="text-align: center; width: 10%;">:
                                                        </td>
                                                        <td style="text-align: left; width: 60%;">
                                                            <div id="State" runat="server">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </td>
                                    <td width="40%" valign="top">
                                        <div class="col-md-12" style="text-align: center;">
                                            <b><span style="font-size: 16px; font-family: Arial Baltic;">Payout Detail </span>
                                            </b>
                                            <div class="col-md-12">
                                                <table class="table-bordered" width="100%">
                                                    <tr>
                                                        <td style="text-align: left; width: 10%;">Payout No.
                                                        </td>
                                                        <td style="text-align: center; width: 5%;">:
                                                        </td>
                                                        <td style="text-align: left; width: 85%;">
                                                            <div id="PayoutTime" runat="server">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left; width: 10%;">Period
                                                        </td>
                                                        <td style="text-align: center; width: 5%;">:
                                                        </td>
                                                        <td style="text-align: left; width: 85%;">
                                                            <div id="Period" runat="server">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12" style="background-color: #999999; text-align: center; color: White;">
                                <b><span style="font-size: 18px; font-family: Arial Baltic;">Income Details </span>
                                </b>
                            </div>
                        </div>
                        <div class="row">
                            <table class="table-bordered" width="98%">
                                <tr>
                                    <td width="40%" valign="top" runat="server" id="DivGeMartIncomeElse" visible="true">
                                        <div class="col-md-12">
                                            <table class="table-bordered" width="100%" cellpadding="1">
                                                <tr>
                                                    <th style="text-align: left;">Earnings
                                                    </th>
                                                    <th style="text-align: right;">Amount In Rs.
                                                    </th>
                                                </tr>
                                                <asp:Repeater ID="gvincome" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td style="text-align: left;">
                                                                <%#Eval("Earnings")%>
                                                            </td>
                                                            <td style="text-align: right;">
                                                                <%#Eval("Amount")%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>
                                    </td>
                                    <td width="40%" valign="top" runat="server" id="DivGeMartIncome" visible="true">
                                        <div class="col-md-12">
                                            <table class="table-bordered" width="100%" cellpadding="1">
                                                <tr>
                                                    <td style="text-align: left; width: 65%;">
                                                        <% if (Session["Compid"].ToString() == "1091")
                                                            { %>
        Team Performance Incentive
    <% }
        else if (Session["Compid"].ToString() == "1074")
        { %>
        Direct Bonus
    <% }
        else if (Session["Compid"].ToString() == "1100")
        { %>
        Direct Referral Bonus
    <% }
        else
        { %>
        Matching Income
    <% } %>
                                                    </td>
                                                    <td style="text-align: center; width: 5%;">:
                                                    </td>
                                                    <td style="text-align: right; width: 30%;">
                                                        <asp:Label ID="LblMatchingIncome" runat="server" Text="0.00"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width: 65%;">
                                                        <% 
                                                            if (Session["Compid"].ToString() == "1091")
                                                            {
                                                        %>
        Direct Referral Incentive
    <% 
        }
        else if (Session["Compid"].ToString() == "1074")
        {
    %>
        Reward Bonus
    <% 
        }
        else if (Session["Compid"].ToString() == "1095" || Session["CompID"].ToString() == "1097")
        {
    %>
        Sponsor Matching Income
    <% 
        }
        else if (Session["Compid"].ToString() == "1100")
        {
    %>
        Team Performance Bonus
    <% 
        }
        else
        {
    %>
        Direct Sponsor Income
    <% 
        }
    %>
                                                    </td>

                                                    <td style="text-align: center; width: 10%;">:
                                                    </td>
                                                    <td style="text-align: right; width: 30%;">
                                                        <asp:Label ID="LblDirectSponsorIncome" runat="server" Text="0.00"></asp:Label>
                                                    </td>
                                                </tr>
                                                <% if (Session["Compid"].ToString() == "1091")
                                                    { %>

                                                <% }
                                                    else
                                                    { %>
                                                <tr>
                                                    <td style="text-align: left; width: 65%;">
                                                        <% 
                                                            if (Session["Compid"].ToString() == "1074")
                                                            {
                                                        %>
            Matching Bonus
        <% 
            }
            else if (Session["Compid"].ToString() == "1095" || Session["CompID"].ToString() == "1097")
            {
        %>
            Reward Level Income
        <% 
            }
            else if (Session["Compid"].ToString() == "1100")
            {
        %>
            Solar Connection Bonus
        <% 
            }
            else
            {
        %>
            Sponsor Matching Income
        <% 
            }
        %>
                                                    </td>

                                                    <td style="text-align: center; width: 10%;">:</td>
                                                    <td style="text-align: right; width: 30%;">
                                                        <asp:Label ID="LblSponsorMatchingIncome" runat="server" Text="0.00"></asp:Label>
                                                    </td>
                                                </tr>
                                                <% } %>

                                                <% if (Session["Compid"].ToString() == "1095"
                || Session["CompID"].ToString() == "1097"
                || Session["Compid"].ToString() == "1100")
                                                    { %>

                                                <tr>
                                                    <td style="text-align: left; width: 65%;">
                                                        <% 
                                                            if (Session["Compid"].ToString() == "1100")
                                                            {
                                                        %>
            Bonus Incentive / Trip
        <% 
            }
            else
            {
        %>
            Self Repurchase Income
        <% 
            }
        %>
                                                    </td>
                                                    <td style="text-align: center; width: 10%;">:</td>
                                                    <td style="text-align: right; width: 30%;">
                                                        <asp:Label ID="lblselfincome" runat="server" Text="0.00"></asp:Label>
                                                    </td>
                                                </tr>

                                                <% } %>

                                                <% if (Session["Compid"].ToString() == "1074"
                || Session["CompID"].ToString() == "1095"
                || Session["CompID"].ToString() == "1097"
                || Session["Compid"].ToString() == "1100")
                                                    { %>

                                                <% }
                                                    else
                                                    { %>
                                                <tr>
                                                    <td style="text-align: left; width: 65%;">
                                                        <% 
                                                            if (Session["Compid"].ToString() == "1091")
                                                            {
                                                        %>
            Leadership Rank Incentive
        <% 
            }
            else
            {
        %>
            Reward Income
        <% 
            }
        %>
                                                    </td>
                                                    <td style="text-align: center; width: 10%;">:</td>
                                                    <td style="text-align: right; width: 30%;">
                                                        <asp:Label ID="LblRewardIncome" runat="server" Text="0.00"></asp:Label>
                                                    </td>
                                                </tr>
                                                <% } %>

                                                <% if (Session["Compid"].ToString() == "1074")
                                                    { %>

                                                <% }
                                                    else
                                                    { %>
                                                <tr>
                                                    <td style="text-align: left; width: 65%;">
                                                        <% 
                                                            if (Session["Compid"].ToString() == "1091")
                                                            {
                                                        %>
            Solar Connection Incentive
        <% 
            }
            else if (Session["Compid"].ToString() == "1095" || Session["CompID"].ToString() == "1097")
            {
        %>
            Royalty Income
        <% 
            }
            else if (Session["Compid"].ToString() == "1100")
            {
        %>
            Retail Bonus
        <% 
            }
            else
            {
        %>
            Royalty Income
        <% 
            }
        %>
                                                    </td>
                                                    <td style="text-align: center; width: 10%;">:</td>
                                                    <td style="text-align: right; width: 30%;">
                                                        <asp:Label ID="LblRoyaltyIncome" runat="server" Text="0.00"></asp:Label>
                                                    </td>
                                                </tr>
                                                <% } %>

                                                <tr>
                                                    <% 
                                                        if (Session["Compid"].ToString() == "1091"
                                                            || Session["Compid"].ToString() == "1095"
                                                            || Session["Compid"].ToString() == "1074"
                                                            || Session["CompID"].ToString() == "1097"
                                                            || Session["Compid"].ToString() == "1100")
                                                        {
                                                    %>

                                                    <% 
                                                        }
                                                        else
                                                        {
                                                    %>
                                                    <td style="text-align: left; width: 65%;">Leadership Dev. Bonus</td>
                                                    <td style="text-align: center; width: 10%;">:</td>
                                                    <td style="text-align: left; width: 30%;">
                                                        <asp:Label ID="LblLeadershipDevBonus" runat="server" Text="0.00"></asp:Label>
                                                    </td>
                                                    <% } %>
                                                </tr>

                                                <% 
                                                    if (Session["Compid"].ToString() == "1091"
                                                        || Session["Compid"].ToString() == "1074")
                                                    {
                                                %>

                                                <% 
                                                    }
                                                    else
                                                    {
                                                %>
                                                <tr>
                                                    <td style="text-align: left; width: 65%;">
                                                        <b>Total Earnings</b>
                                                    </td>
                                                    <td style="text-align: center; width: 10%;">:</td>
                                                    <td style="text-align: right; width: 30%;">
                                                        <b>
                                                            <asp:Label ID="LblTotalEarnings" runat="server" Text="0.00"></asp:Label>
                                                        </b>
                                                    </td>
                                                </tr>
                                                <% } %>
                                            </table>
                                        </div>
                                    </td>
                                    <td width="40%" valign="top" runat="server" id="DivGeMartElse" visible="true">
                                        <div class="col-md-12">
                                            <table class="table-bordered" width="100%" cellpadding="1">
                                                <tr>
                                                    <th style="text-align: left;">Deductions
                                                    </th>
                                                    <th style="text-align: right;">Amount In Rs.
                                                    </th>
                                                </tr>
                                                <asp:Repeater ID="gvDeduction" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td style="text-align: left;">
                                                                <%#Eval("Deductions")%>
                                                            </td>
                                                            <td style="text-align: right;">
                                                                <%#Eval("Amount")%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>
                                    </td>
                                    <td width="40%" valign="top" runat="server" id="DivGeMart" visible="false">
                                        <div class="col-md-12">
                                            <table class="table-bordered" width="100%" cellpadding="1">
                                                <tr>
                                                    <td style="text-align: left; width: 65%;">TDS Amount
                                                    </td>
                                                    <td style="text-align: center; width: 10%;">:
                                                    </td>
                                                    <td style="text-align: left; width: 30%;">
                                                        <asp:Label ID="LblTDSAmount" runat="server" Text="0.00"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width: 65%;">Admin Charge
                                                    </td>
                                                    <td style="text-align: center; width: 5%;">:
                                                    </td>
                                                    <td style="text-align: left; width: 30%;">
                                                        <asp:Label ID="LblAdminCharge" runat="server" Text="0.00"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width: 65%;">Repurchase Deduction
                                                    </td>
                                                    <td style="text-align: center; width: 5%;">:
                                                    </td>
                                                    <td style="text-align: left; width: 30%;">
                                                        <asp:Label ID="LblRepurchaseDeduction" runat="server" Text="0.00"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: left; width: 65%;">
                                                        <b>Total Deduction</b>
                                                    </td>
                                                    <td style="text-align: center; width: 5%;">
                                                        <b>:</b>
                                                    </td>
                                                    <td style="text-align: left; width: 30%;">
                                                        <b>
                                                            <asp:Label ID="LblTotalDeducation" runat="server" Text="0.00"></asp:Label></b>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="row">
                            <table class="table-bordered" width="98%">
                                <tr>
                                    <td width="40%" valign="top" runat="server" id="DivTotalEar" visible="true">
                                        <div class="col-md-12">
                                            <table class="table-bordered" width="100%" cellpadding="1">
                                                <tr>
                                                    <th style="text-align: left;">
                                                        <b>Total Earnings </b>
                                                    </th>
                                                    <th style="text-align: right;">
                                                        <div id="TotalEarnings" runat="server">
                                                        </div>
                                                    </th>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                    <td width="40%" valign="top" runat="server" id="DivTotaldis" visible="true">
                                        <div class="col-md-12">
                                            <table class="table-bordered" width="100%" cellpadding="1">
                                                <tr>
                                                    <th style="text-align: left;">
                                                        <b>Total Deductions</b>
                                                    </th>
                                                    <th style="text-align: right;">
                                                        <div id="TotalDeductions" runat="server">
                                                        </div>
                                                    </th>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="row">
                            <table class="table-bordered" width="98%">
                                <tr>
                                    <td width="40%" valign="top">
                                        <div class="col-md-12">
                                            <table class="table-bordered" width="100%" cellpadding="1">
                                                <tr>
                                                    <th style="text-align: left;">
                                                        <b>Net Payble Amount (Rs.) </b>
                                                    </th>
                                                    <th style="text-align: right;">
                                                        <div id="NetPayble" runat="server">
                                                        </div>
                                                    </th>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                    <td width="40%" valign="top"></td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <div class="row">
                            <table class="table-bordered" width="98%">
                                <tr>
                                    <td width="40%" valign="top">
                                        <div class="col-md-12">
                                            <table class="table-bordered" width="100%" cellpadding="1">
                                                <tr>
                                                    <td colspan="3" style="background-color: #999999; text-align: center; color: White;">
                                                        <b>Business Details</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td style="text-align: right;">
                                                        <% if (Session["CompID"].ToString() == "1095"
                || Session["CompID"].ToString() == "1097"
                || Session["Compid"].ToString() == "1100")
                                                            { %>
                                                        <b>Left</b>
                                                        <% }
                                                            else
                                                            { %>
                                                        <b>Power Group</b>
                                                        <% } %>
                                                    </td>

                                                    <td style="text-align: right;">
                                                        <% if (Session["CompID"].ToString() == "1095"
                || Session["CompID"].ToString() == "1097"
                || Session["Compid"].ToString() == "1100")
                                                            { %>
                                                        <b>Right</b>
                                                        <% }
                                                            else
                                                            { %>
                                                        <b>Weaker Group</b>
                                                        <% } %>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>B/F <%= Session["ColName1"] %>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <div id="BfXBV" runat="server"></div>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <div id="BfYBV" runat="server"></div>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>New <%= Session["ColName1"] %>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <div id="NewXBV" runat="server"></div>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <div id="NewYBV" runat="server"></div>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>Matched <%= Session["ColName1"] %>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <div id="MatchedXBV" runat="server"></div>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <div id="MatchedYBV" runat="server"></div>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>C/F <%= Session["ColName1"] %>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <div id="CfXBV" runat="server"></div>
                                                    </td>
                                                    <td style="text-align: right;">
                                                        <div id="CfYBV" runat="server"></div>
                                                    </td>
                                                </tr>

                                            </table>
                                        </div>
                                        <div id="divMentorshipLife" runat="server" visible="false">
                                            <table class="table-bordered" width="100%" cellpadding="1">
                                                <tr>
                                                    <td colspan="6" style="background-color: #999999; text-align: center; color: White;">
                                                        <% if (Session["Compid"].ToString() == "1075")
                                                        { %>
                                                        <b>Single Side Bonus Detail</b>
                                                        <% }
                                                            else
                                                            { %>
                                                        <b>Mentorship Bonus Detail</b>
                                                        <% } %>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <th style="text-align: left;">IDNO</th>

                                                    <% if (Session["Compid"].ToString() == "1075")
                                                    { %>
                                                    <th style="text-align: left;">Name</th>
                                                    <% } %>

                                                    <th style="text-align: right;">Level</th>

                                                    <% if (Session["Compid"].ToString() != "1075")
                                                    { %>
                                                    <th style="text-align: right;">Matching Bonus</th>
                                                    <th style="text-align: right;">Slab</th>
                                                    <% } %>

                                                    <th style="text-align: right;">Comm.</th>
                                                </tr>

                                                <asp:Repeater ID="repMentorship" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td style="text-align: left;">
                                                                <%# Eval("IdNo") %>
                                                            </td>

                                                            <% if (Session["Compid"].ToString() == "1075")
                                                                { %>
                                                            <td style="text-align: left;">
                                                                <%# Eval("Name") %>
                                                            </td>
                                                            <% } %>

                                                            <td style="text-align: right;">
                                                                <%# Eval("MLevel") %>
                                                            </td>

                                                            <% if (Session["Compid"].ToString() != "1075")
                                                                { %>
                                                            <td style="text-align: right;">
                                                                <%# Eval("PairIncome") %>
                                                            </td>
                                                            <td style="text-align: right;">
                                                                <%# Eval("Slab") %>
                                                            </td>
                                                            <% } %>

                                                            <td style="text-align: right;">
                                                                <%# Eval("Comm") %>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>

                                        </div>
                                    </td>
                                    <td width="40%" valign="top">
                                        <div id="divlevelIncomeGoldwings" runat="server" visible="false">
                                            <table class="table-bordered" width="100%" cellpadding="1">
                                                <tr>
                                                    <td colspan="6" style="background-color: #999999; text-align: center; color: White;">
                                                        <b>
                                                            <% 
                                                                if (Session["Compid"].ToString() == "1023")
                                                                {
                                                            %>
                    Upline Bonus
                <% 
                    }
                    else if (Session["Compid"].ToString() == "1075")
                    {
                %>
                    Direct Income
                <% 
                    }
                    else if (Session["Compid"].ToString() == "1090")
                    {
                %>
                    Sponsor Matching Income
                <% 
                    }
                    else if (Session["Compid"].ToString() == "1074")
                    {
                %>
                    Matching Income
                <% 
                    }
                    else
                    {
                %>
                    Level Income
                <% 
                    }
                %>
                                                        </b>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <th style="text-align: left;">IDNO</th>

                                                    <th style="text-align: right;">
                                                        <% if (Session["Compid"].ToString() == "1075")
                                                        { %>
                Business
            <% }
                else
                { %>
                Level
            <% } %>
                                                    </th>

                                                    <% if (Session["Compid"].ToString() != "1075")
                                                    { %>
                                                    <th style="text-align: right;">
                                                        <% 
                                                            if (Session["Compid"].ToString() == "1023")
                                                            {
                                                        %>
                    Upline Bonus
                <% 
                    }
                    else if (Session["Compid"].ToString() == "1090")
                    {
                %>
                    Business
                <% 
                    }
                    else
                    {
                %>
                    Matching Bonus
                <% 
                    }
                %>
                                                    </th>
                                                    <% } %>

                                                    <th style="text-align: right;">Slab</th>

                                                    <th style="text-align: right;">
                                                        <% if (Session["Compid"].ToString() == "1090")
                                                        { %>
                Income
            <% }
                else
                { %>
                Comm.
            <% } %>
                                                    </th>
                                                </tr>

                                                <asp:Repeater ID="RepLevelGoldwings" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td style="text-align: left;">
                                                                <%# Eval("IdNo") %>
                                                            </td>

                                                            <td style="text-align: right;">
                                                                <% if (Session["Compid"].ToString() == "1075")
                                                                    { %>
                                                                <%# Eval("Business") %>
                                                                <% }
                                                                    else
                                                                    { %>
                                                                <%# Eval("MLevel") %>
                                                                <% } %>
                                                            </td>

                                                            <% if (Session["Compid"].ToString() != "1075")
                                                                { %>
                                                            <td style="text-align: right;">
                                                                <%# Eval("PairIncome") %>
                                                            </td>
                                                            <% } %>

                                                            <td style="text-align: right;">
                                                                <%# Eval("Slab") %>
                                                            </td>

                                                            <td style="text-align: right;">
                                                                <%# Eval("Comm") %>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>

                                        </div>
                                        <div id="divDownlineIncome" runat="server" visible="false">
                                            <table class="table-bordered" width="100%" cellpadding="1">
                                                <tr>
                                                    <td colspan="6" style="background-color: #999999; text-align: center; color: White;">
                                                        <b>
                                                            <% 
                                                                if (Session["Compid"].ToString() == "1090"
                                                                    || Session["Compid"].ToString() == "1091")
                                                                {
                                                            %>
                    Direct Sponsor Income
                <% 
                    }
                    else if (Session["Compid"].ToString() == "1074")
                    {
                %>
                    Reward Bonus
                <% 
                    }
                    else
                    {
                %>
                    Downline Income Detail
                <% 
                    }
                %>
                                                        </b>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <th style="text-align: left;">IDNO</th>

                                                    <% 
                                                        if (!(Session["Compid"].ToString() == "1090"
                                                            || Session["Compid"].ToString() == "1091"
                                                            || Session["Compid"].ToString() == "1095"
                                                            || Session["CompID"].ToString() == "1097"
                                                            || Session["Compid"].ToString() == "1100"))
                                                        {
                                                    %>
                                                    <th style="text-align: right;">
                                                        <% 
                                                            if (Session["Compid"].ToString() == "1090"
                                                                || Session["Compid"].ToString() == "1091"
                                                                || Session["Compid"].ToString() == "1095"
                                                                || Session["CompID"].ToString() == "1097"
                                                                || Session["Compid"].ToString() == "1100")
                                                            {
                                                        %>
                    Member Name
                <% 
                    }
                    else
                    {
                %>
                    Level
                <% 
                    }
                %>
                                                    </th>
                                                    <% } %>

                                                    <th style="text-align: right;">
                                                        <% 
                                                            if (Session["Compid"].ToString() == "1090"
                                                                || Session["Compid"].ToString() == "1091")
                                                            {
                                                        %>
                Business
            <% 
                }
                else
                {
            %>
                Downline Bonus
            <% 
                }
            %>
                                                    </th>

                                                    <th style="text-align: right;">Slab</th>

                                                    <th style="text-align: right;">
                                                        <% 
                                                            if (Session["Compid"].ToString() == "1090"
                                                                || Session["Compid"].ToString() == "1091")
                                                            {
                                                        %>
                Income
            <% 
                }
                else
                {
            %>
                Comm.
            <% 
                }
            %>
                                                    </th>
                                                </tr>

                                                <asp:Repeater ID="repdownlineincome" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td style="text-align: left;">
                                                                <%# Eval("IdNo") %>
                                                            </td>

                                                            <% 
                                                                if (!(Session["Compid"].ToString() == "1090"
                                                                    || Session["Compid"].ToString() == "1091"
                                                                    || Session["Compid"].ToString() == "1095"
                                                                    || Session["CompID"].ToString() == "1097"
                                                                    || Session["Compid"].ToString() == "1100"))
                                                                {
                                                            %>
                                                            <td style="text-align: right;">
                                                                <% 
                                                                    if (Session["Compid"].ToString() == "1090"
                                                                        || Session["Compid"].ToString() == "1091"
                                                                        || Session["Compid"].ToString() == "1095"
                                                                        || Session["CompID"].ToString() == "1097"
                                                                        || Session["Compid"].ToString() == "1100")
                                                                    {
                                                                %>
                                                                <%# Eval("Name") %>
                                                                <% 
                                                                    }
                                                                    else
                                                                    {
                                                                %>
                                                                <%# Eval("MLevel") %>
                                                                <% 
                                                                    }
                                                                %>
                                                            </td>
                                                            <% } %>

                                                            <td style="text-align: right;">
                                                                <% 
                                                                    if (Session["Compid"].ToString() == "1090"
                                                                        || Session["Compid"].ToString() == "1091")
                                                                    {
                                                                %>
                                                                <%# Eval("Business") %>
                                                                <% 
                                                                    }
                                                                    else
                                                                    {
                                                                %>
                                                                <%# Eval("PairIncome") %>
                                                                <% 
                                                                    }
                                                                %>
                                                            </td>

                                                            <td style="text-align: right;">
                                                                <%# Eval("Slab") %>
                                                            </td>

                                                            <td style="text-align: right;">
                                                                <%# Eval("Comm") %>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>

                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="row" style="text-align: right; margin-top: 10px;">
                            <div class="col-md-12">
                                Authorised Signatory
                            </div>
                        </div>
                    </div>
                </div>
            </center>
        </div>
    </form>
</body>
</html>
