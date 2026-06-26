<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="NTimeRewardDv9.aspx.cs" Inherits="NTimeRewardDv9" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .rw-wrap * {
            box-sizing: border-box;
        }

        .rw-section-title {
            font-size: 12px;
            font-weight: 600;
            color: #888;
            letter-spacing: .06em;
            text-transform: uppercase;
            padding: 14px 20px 8px;
            margin: 0;
        }

        .rw-pair-row {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 10px;
            padding: 10px 20px 16px;
        }

        .rw-pair-card {
            background: #f8f8f6;
            border-radius: 8px;
            padding: 14px;
            text-align: center;
        }

        .rw-pair-label {
            font-size: 11px;
            color: #888;
            margin-bottom: 6px;
        }

        .rw-pair-val {
            font-size: 22px;
            font-weight: 600;
            color: #222;
        }

        .rw-stat-row {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(120px, 1fr));
            gap: 10px;
            padding: 10px 20px 16px;
        }

        .rw-stat-card {
            background: #f8f8f6;
            border-radius: 8px;
            padding: 12px 14px;
        }

        .rw-stat-label {
            font-size: 11px;
            color: #888;
            margin-bottom: 4px;
        }

        .rw-stat-val {
            font-size: 20px;
            font-weight: 600;
            color: #222;
        }

        .rw-divider {
            height: 1px;
            background: #ebebeb;
            margin: 0 20px;
        }

        .rw-tbl-wrap {
            padding: 0 12px 14px;
            overflow-x: auto;
        }

            .rw-tbl-wrap .table th {
                font-size: 12px;
                font-weight: 600;
                color: #666;
                background: #f8f8f6;
                text-transform: uppercase;
                letter-spacing: .04em;
            }

            .rw-tbl-wrap .table td {
                font-size: 13px;
                vertical-align: middle;
            }

        .badge-rw {
            display: inline-flex;
            align-items: center;
            padding: 3px 10px;
            border-radius: 20px;
            font-size: 11px;
            font-weight: 600;
        }

            .badge-rw.success {
                background: #EAF3DE;
                color: #3B6D11;
            }

            .badge-rw.warn {
                background: #FAEEDA;
                color: #854F0B;
            }

            .badge-rw.info {
                background: #E6F1FB;
                color: #185FA5;
            }

            .badge-rw.gray {
                background: #F1EFE8;
                color: #5F5E5A;
            }

        .btn-redeem-rw {
            display: inline-flex;
            align-items: center;
            gap: 4px;
            padding: 4px 12px;
            border-radius: 6px;
            border: 1px solid #ddd;
            background: #fff;
            font-size: 12px;
            font-weight: 500;
            color: #333;
            cursor: pointer;
        }

            .btn-redeem-rw:hover {
                background: #f5f5f5;
            }

        .card-header-rw {
            display: flex;
            align-items: center;
            gap: 10px;
            padding: 14px 20px;
            border-bottom: 1px solid #ebebeb;
            background: #f8f8f6;
        }

            .card-header-rw h5 {
                font-size: 15px;
                font-weight: 600;
                color: #222;
                margin: 0;
            }

            .card-header-rw .icon {
                font-size: 20px;
                color: #c0392b;
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="main-content rw-wrap">
        <section class="section">
            <div class="row">
                <div class="col-12">
                    <div class="card">

                        <%-- Card Header --%>
                        <div class="card-header-rw">
                            <span class="icon fa fa-trophy"></span>
                            <h5>Reward</h5>
                        </div>

                        <div class="card-body" style="padding: 0;">

                            <%-- Error Message --%>
                            <div style="padding: 10px 20px 0;" id="DivSideA" runat="server">
                                <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                            </div>

                            <%-- BV Pair Summary --%>
                            <div class="rw-tbl-wrap" style="padding-top: 14px;">
                                <asp:DataGrid ID="GrdRewardPair" runat="server" AutoGenerateColumns="False"
                                    ForeColor="Black" BackColor="White" HorizontalAlign="Center"
                                    CellPadding="3" Font-Size="Small"
                                    CssClass="table table-striped table-bordered">
                                    <Columns>
                                        <asp:BoundColumn DataField="LeftRp" HeaderText="Left">
                                            <HeaderStyle HorizontalAlign="Center" Width="150px" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="RightRp" HeaderText="Right">
                                            <HeaderStyle HorizontalAlign="Center" Width="150px" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:BoundColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </div>

                            <div class="rw-divider"></div>

                            <%-- Achieved Reward Status --%>
                            <p class="rw-section-title">Achieved Reward Status</p>
                            <div id="Div2" class="rw-tbl-wrap" runat="server">
                                <asp:GridView ID="GrdRewards" runat="server" PageSize="50"
                                    CssClass="table table-striped table-bordered"
                                    CellPadding="3" HorizontalAlign="Center"
                                    AutoGenerateColumns="False" AllowPaging="True" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No." HeaderStyle-Width="40px">
                                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                        </asp:TemplateField>
                                       <%-- <asp:BoundField DataField="Rank" HeaderText="Rank" HeaderStyle-HorizontalAlign="Center" />--%>
                                        <asp:BoundField DataField="Reward" HeaderText="Reward" HeaderStyle-HorizontalAlign="Center" />
                                       <%-- <asp:BoundField DataField="achievedReward" HeaderText="Reward Type" HeaderStyle-HorizontalAlign="Center" />--%>
                                        <asp:BoundField DataField="AchieveDate" HeaderText="Achieve Date" HeaderStyle-HorizontalAlign="Center" />
                                     <%--   <asp:BoundField DataField="PaidDate" HeaderText="Paid Date" HeaderStyle-HorizontalAlign="Center" />--%>
                                        <asp:TemplateField HeaderText="Redeem" Visible="false"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle"
                                            ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:Label ID="lblreward" runat="server" Text='<%# Eval("Rewardid") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblstatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                              <%--  <asp:LinkButton ID="btnredm" runat="server" CssClass="btn-redeem-rw"
                                                    Text="Redeem" OnClick="Reedembtn"
                                                    OnClientClick="return confirmation();"
                                                    Visible='<%# Eval("RedeemStaus") %>'></asp:LinkButton>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <%-- <asp:BoundField DataField="RedeemDate" HeaderText="Redeem Date" Visible="false"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle"
                                            ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />--%>
                                    </Columns>
                                </asp:GridView>

                                <asp:GridView ID="GrdPending1" runat="server"
                                    CssClass="table table-striped table-bordered"
                                    CellPadding="2" HorizontalAlign="Center"
                                    AutoGenerateColumns="False" Width="100%"
                                    EmptyDataText="No Data Display">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No." HeaderStyle-Width="40px">
                                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Reward" HeaderText="Reward" />
                                        <asp:BoundField DataField="NewPair" HeaderText="New Pair" />
                                        <asp:BoundField DataField="AchieveDate" HeaderText="Achieve Date" />
                                    </Columns>
                                </asp:GridView>
                            </div>

                            <div class="rw-divider"></div>

                            <%-- Next Reward Status --%>
                            <div id="divNextreward" runat="server">
                                <p class="rw-section-title">Next Reward Status</p>
                                <div id="Div3" class="rw-tbl-wrap" runat="server">
                                    <asp:GridView ID="GrdNext" runat="server"
                                        CssClass="table table-striped table-bordered"
                                        CellPadding="2" HorizontalAlign="Center"
                                        AutoGenerateColumns="true" Width="100%"
                                        EmptyDataText="No Data Display">
                                    </asp:GridView>
                                </div>
                            </div>

                            <div class="rw-divider"></div>

                            <%-- Pending Reward Status --%>
                            <div id="divpendingreward" runat="server">
                                <p class="rw-section-title">Pending Reward Status</p>
                                <div id="Div1" class="rw-tbl-wrap" runat="server">
                                    <asp:GridView ID="GrdPending" runat="server"
                                        CssClass="table table-striped table-bordered"
                                        CellPadding="2" HorizontalAlign="Center"
                                        AutoGenerateColumns="False" Width="100%"
                                        EmptyDataText="No Data Display">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No." HeaderStyle-Width="40px">
                                                <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="RDays" HeaderText="Total Days" />
                                            <asp:BoundField DataField="Newpair" HeaderText="Matching" />
                                            <%--<asp:BoundField DataField="Rank" HeaderText="Rank" />--%>
                                            <asp:BoundField DataField="Reward" HeaderText="Reward (Time Limit)" />
                                            <asp:BoundField DataField="Notimelimit" HeaderText="Reward (No Time Limits)" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div id="div4" runat="server" visible="false">
                                <p class="rw-section-title">Pending Reward Status</p>
                                <div id="Div5" class="rw-tbl-wrap" runat="server">
                                    <asp:GridView ID="GridView1" runat="server"
                                        CssClass="table table-striped table-bordered"
                                        CellPadding="2" HorizontalAlign="Center"
                                        AutoGenerateColumns="False" Width="100%"
                                        EmptyDataText="No Data Display">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No." HeaderStyle-Width="40px">
                                                <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:BoundField DataField="Newpair" HeaderText="Matching" />
                                          <%--  <asp:BoundField DataField="Rank" HeaderText="Rank" />--%>
                                            <asp:BoundField DataField="Reward" HeaderText="Reward (Time Limit)" />
                                           
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <%-- /card-body --%>
                    </div>
                    <%-- /card --%>
                </div>
            </div>
        </section>
    </div>
</asp:Content>

