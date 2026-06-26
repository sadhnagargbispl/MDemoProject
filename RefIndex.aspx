<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="RefIndex.aspx.cs" Inherits="RefIndex" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function copyLinkClient() {

            var range, selection, worked;
            var element = document.getElementById("ContentPlaceHolder1_lblLink");
            if (document.body.createTextRange) {
                range = document.body.createTextRange();
                range.moveToElementText(element);
                range.select();
            } else if (window.getSelection) {
                selection = window.getSelection();
                range = document.createRange();
                range.selectNodeContents(element);
                selection.removeAllRanges();
                selection.addRange(range);
            }

            try {
                document.execCommand('copy');
                alert('link copied');
            }
            catch (err) {
                alert('unable to copy link');
            }
            return false;
        }

    </script>

    <script type="text/javascript">
        function copyLinkClient1() {

            var range, selection, worked;
            var element = document.getElementById("ContentPlaceHolder1_lblLink1");
            if (document.body.createTextRange) {
                range = document.body.createTextRange();
                range.moveToElementText(element);
                range.select();
            } else if (window.getSelection) {
                selection = window.getSelection();
                range = document.createRange();
                range.selectNodeContents(element);
                selection.removeAllRanges();
                selection.addRange(range);
            }

            try {
                document.execCommand('copy');
                alert('link copied');
            }
            catch (err) {
                alert('unable to copy link');
            }
            return false;
        }

    </script>
    <div class="main-content">


        <!-- Welcome + Profile Card -->
        <div class="row mb-4">
            <div class="col-lg-12">
                <div class="card member-card">
                    <div class="card-body">

                        <!-- PROFILE IMAGE -->
                        <div class="profile-wrap">
                            <!-- asp:Image keeps server control; CssClass used to style -->
                            <asp:Image ID="Image2" runat="server" CssClass="profile-img" AlternateText="" />
                        </div>

                        <!-- DETAILS -->
                        <div class="member-text">

                            <h5 class="member-title mb-1">
                                <span class="label">Member Name :</span>
                                <span class="value">
                                    <asp:Label ID="LblMemName" runat="server"></asp:Label>
                                </span>
                            </h5>

                            <p class="member-sub mb-0">
                                <span><strong>Member ID :</strong>
                                    <span class="value">
                                        <asp:Label ID="LblMemId" runat="server"></asp:Label></span>
                                </span>
                            </p>
                            <%-- <span class="dot">•</span>--%>
                            <p class="member-sub mb-0">
                                <span><strong>Rank : 
                                </strong>
                                    <span class="rank-badge">
                                        <asp:Label ID="lblRankTitle" runat="server"></asp:Label>
                                    </span>
                                </span>
                            </p>

                        </div>

                    </div>
                </div>
            </div>


        </div>



        <!-- Income Summary (repMyIncome rendered as table) -->


        <div class="row g-4">

            <!-- Referral Link Card -->
            <div class="col-lg-8" id="Div13" runat="server">
                <div class="card ref-card h-100">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa fa-link me-2"></i>My Referral Link</h5>
                    </div>

                    <div class="card-body">

                        <!-- responsive row -->
                        <div class="ref-row" id="refRow">

                            <!-- LABEL -->
                            <div class="ref-label" id="tdReferalHead" runat="server">
                                <% 
                                    string cid = Session["Compid"]?.ToString();
                                    if (cid == "1056") { Response.Write("ODD"); }
                                    else if (cid == "1066" || cid == "1067" || cid == "1068" || cid == "1072" ||
                                             cid == "1073" || cid == "1077" || cid == "1078" || cid == "1079" ||
                                             cid == "1082" || cid == "1083" || cid == "1089" || cid == "1093" || cid == "1101")
                                    { Response.Write("Referral Link"); }
                                    else { Response.Write("LEFT"); }
                                %>
                            </div>

                            <!-- LINK -->
                            <div class="ref-link">
                                <a id="aRfLink" runat="server" target="_blank">
                                    <asp:Label ID="lblLink" runat="server" CssClass="d-inline-block" />
                                </a>
                            </div>

                            <!-- ACTION -->
                            <div class="ref-action">
                                <button type="button" class="copy-btn" onclick="copyLinkClient()" id="btnCopyTop">
                                    <i class="fa fa-copy me-1"></i>Copy
                                </button>
                            </div>
                        </div>

                        <!-- Optional: second row (RIGHT/EVEN) - kept responsive -->
                        <div class="ref-row" style="margin-top: 12px;" id="trReferalHead" runat="server">
                            <div class="ref-label">
                                <% 
                                    string cid = Session["Compid"].ToString();
                                    if (cid == "1056") { Response.Write("EVEN"); }
                                    else if (cid == "1066" || cid == "1068" || cid == "1072" || cid == "1073" ||
                                             cid == "1077" || cid == "1078" || cid == "1079" || cid == "1093" || cid == "1101")
                                    { /* blank as original */ }
                                    else { Response.Write("RIGHT"); }
                                %>
                            </div>

                            <div class="ref-link">
                                <% if (!(cid == "1066" || cid == "1067" || cid == "1072" || cid == "1073" ||
                                   cid == "1077" || cid == "1078" || cid == "1079" || cid == "1093" || cid == "1101"))
                                    { %>
                                <a id="aRfLink1" runat="server" target="_blank">
                                    <asp:Label ID="lbllink1" runat="server" Style="color: Blue;" />
                                </a>
                                <% } %>
                            </div>

                            <div class="ref-action">
                                <% if (!(cid == "1066" || cid == "1067" || cid == "1072" || cid == "1073" ||
                                   cid == "1077" || cid == "1078" || cid == "1079" || cid == "1093" || cid == "1101"))
                                    { %>
                                <button type="button" class="copy-btn" onclick="copyLinkClient1()" id="btnCopy2">
                                    <i class="fa fa-copy me-1"></i>Copy
                                </button>
                                <% } %>
                            </div>
                        </div>

                    </div>
                </div>
            </div>


            <!-- News Card -->
            <div class="col-lg-4">
                <div class="card h-100">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa fa-bullhorn me-2"></i>Latest Updates</h5>
                    </div>
                    <div class="card-body news-marquee p-0">
                        <div class="p-3">
                            <asp:Repeater ID="RptNews" runat="server">
                                <ItemTemplate>
                                    <div class="border-bottom pb-3 mb-3">
                                        <p class="mb-1 small"><%# Eval("NewsDetail") %></p>
                                        <small class="text-muted"><i class="fa fa-calendar"></i><%# Eval("NewsDate") %></small>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-6">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>Income Summary</h5>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-hover table-bordered mb-0">
                                <thead class="table-light">
                                    <tr>
                                        <th>Income Type</th>
                                        <th class="text-end">Amount</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="repMyIncome" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td><strong><%# Eval("Name") %></strong></td>
                                                <td class="text-end fw-bold "><%# Eval("Amount") %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Wallet Summary -->
            <div class="col-lg-6">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa fa-wallet me-2"></i>Wallet Summary</h5>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-hover mb-0">
                                <thead class="table-light">
                                    <tr>
                                        <th>Wallet</th>
                                        <th>Credit</th>
                                        <th>Debit</th>
                                        <th>Balance</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="gvBalance" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td><strong><%# Eval("WalletName") %></strong></td>
                                                <td><%# Eval("Credit") %></td>
                                                <td><%# Eval("Debit") %></td>
                                                <td class="fw-bold"><%# Eval("Balnace") %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Team Summary -->
            <div class="col-lg-6">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa fa-users-cog me-2"></i>My Team Summary</h5>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table id="table" class="table table-bordered mb-0">
                                <tbody>
                                    <tr class="infoclr">
                                        <td></td>
                                        <th>Direct
                                        </th>
                                        <th>Indirect
                                        </th>
                                        <th>Total
                                        </th>
                                    </tr>

                                    <% string comp = Session["Compid"].ToString(); %>

                                    <% if (comp != "1078" || comp != "1093")
                                        { %>
                                    <tr>
                                        <th>Today's Registration
                                        </th>
                                        <td id="TodayDirectRegister" runat="server">0
                                        </td>
                                        <td id="TodayIndirectRegister" runat="server">0
                                        </td>
                                        <td id="TotalTodayRegister" runat="server">0
                                        </td>
                                    </tr>
                                    <% } %>

                                    <% if (comp == "1093")
                                        { %>
                                    <% }
                                        else
                                        { %>
                                    <tr class="backclr">
                                        <th>Today's Activation
                                        </th>
                                        <td id="TodayDirectActive" runat="server">0
                                        </td>
                                        <td id="TodayIndirectActive" runat="server">0
                                        </td>
                                        <td id="TodayTotalActive" runat="server">0
                                        </td>
                                    </tr>
                                    <% } %>

                                    <% if (comp != "1078" || comp != "1093")
                                        { %>
                                    <tr>
                                        <th>Total Registration
                                        </th>
                                        <td id="TotalDirectJoin" runat="server">0
                                        </td>
                                        <td id="TotalIndirectJoin" runat="server">0
                                        </td>
                                        <td id="TotalJoin" runat="server">0
                                        </td>
                                    </tr>
                                    <% } %>

                                    <% if (comp == "1093")
                                        { %>
                                    <% }
                                        else
                                        { %>
                                    <tr class="backclr">
                                        <th>Total Activation
                                        </th>
                                        <td id="TotalDirectActivation" runat="server">0
                                        </td>
                                        <td id="TotalIndirectActivation" runat="server">0
                                        </td>
                                        <td id="TotalActivation" runat="server">0
                                        </td>
                                    </tr>
                                    <% } %>

                                    <% if (comp == "1067")
                                        { %>
                                    <tr class="backclr">
                                        <th>Total BV
                                        </th>
                                        <td id="totalDirectBV" runat="server">0
                                        </td>
                                        <td id="TotalIndirectBv" runat="server">0
                                        </td>
                                        <td id="TotalBV1" runat="server">0
                                        </td>
                                    </tr>
                                    <% } %>
                                </tbody>
                            </table>

                            <%--<table class="table table-bordered mb-0">
                                <thead class="table-light">
                                    <tr>
                                        <th></th>
                                        <th>Direct</th>
                                        <th>Indirect</th>
                                        <th>Total</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <th>Today's Join</th>
                                        <td id="TodayDirectRegister" runat="server">0</td>
                                        <td id="TodayIndirectRegister" runat="server">0</td>
                                        <td id="TotalTodayRegister" runat="server">0</td>
                                    </tr>
                                    <tr class="table-secondary">
                                        <th>Total Join</th>
                                        <td id="Td1" runat="server">0</td>
                                        <td id="TotalIndirectJoin" runat="server">0</td>
                                        <td id="Td2" runat="server">0</td>
                                    </tr>
                                    <tr>
                                        <th>Today's Active</th>
                                        <td id="TodayDirectActive" runat="server">0</td>
                                        <td id="TodayIndirectActive" runat="server">0</td>
                                        <td id="TodayTotalActive" runat="server">0</td>
                                    </tr>
                                    <tr class="table-secondary">
                                        <th>Total Active</th>
                                        <td id="TotalDirectActivation" runat="server">0</td>
                                        <td id="TotalIndirectActivation" runat="server">0</td>
                                        <td id="TotalActivation" runat="server">0</td>
                                    </tr>
                                </tbody>
                            </table>--%>
                        </div>
                    </div>
                </div>
            </div>

            <!-- My Directs + Rewards -->
            <div class="col-lg-6" id="dvVPRequest" runat="server">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa fa-user-friends me-2"></i>My Direct Members</h5>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-hover table-mc-light-blue table-bordered">
                                <thead>
                                    <tr>
                                        <th>ID</th>
                                        <th>Name</th>
                                        <th>Join Date</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="RptDirects" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%# Eval("IDNo") %></td>
                                                <td><%# Eval("MemName") %></td>
                                                <td><%# Eval("Doj") %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>

            <% if (Session["compid"] != null && Session["compid"].ToString() == "1101")
                { %>
            <%} %>
            <% else
                {  %>
            <div class="col-lg-6">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0"><i class="fa fa-award me-2"></i>
                            <% if (Session["compid"].ToString() == "1077")
                                { %>
 Upcoming Reward:
 <asp:Label ID="LblRichnetreward" runat="server" Visible="false" ForeColor="Red" Font-Bold="true"></asp:Label>
                            <% } %>
                            <% else
                                {  %>
 Reward<% } %>
                        </h5>
                    </div>
                    <div class="card-body">
                        <asp:DataGrid ID="DataGrid1" runat="server" Class="table table-striped table-sm" AutoGenerateColumns="False"
                            AllowPaging="True" PageSize="8">
                            <Columns>
                                <asp:TemplateColumn HeaderText="S.No">
                                    <ItemTemplate>
                                        <%#Container.DataSetIndex + 1%>.
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="Reward" HeaderText="Reward"></asp:BoundColumn>
                                <asp:BoundColumn DataField="AchieveDate" HeaderText="AchieveDate"></asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>
                        <asp:DataGrid ID="RichnetReward" runat="server" Class="table" AutoGenerateColumns="False"
                            Visible="false">
                            <Columns>
                                <asp:BoundColumn DataField="Remaining Leg1 Business" HeaderText="Remaining Leg1 Business"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Remaining Leg2 Business" HeaderText="Remaining Leg2 Business"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Remaining Leg3 Business" HeaderText="Remaining Leg3 Business"></asp:BoundColumn>
                            </Columns>
                        </asp:DataGrid>

                    </div>
                </div>
            </div>
            <% } %>
        </div>
    </div>

</asp:Content>


