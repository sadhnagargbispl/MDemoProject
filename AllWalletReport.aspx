<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="AllWalletReport.aspx.cs" Inherits="AllWalletReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="main-content">
        <style>
            .red {
                color: red;
                font-size: 1.5em;
                padding-left: 4px;
                font-weight: bold;
            }
        </style>
        <section class="section">
            <ul class="breadcrumb breadcrumb-style ">
            </ul>
            <div class="row">
                <div class="col-12 col-sm-12 col-lg-12">
                    <div class="card">
                        <div class="card-header">
                            <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>Wallet Detail</h5>
                        </div>
                        <div class="card-body">
                            <div class="row g-1">
                                <div class="clr">
                                    <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlVoucherType" runat="server" CssClass="form-control ">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3" id="joiningtype">
                                    <div class="form-group">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary " OnClick="btnSearch_Click" />
                                    </div>
                                </div>


                                <div class="col-md-12 table-responsive">
                                    <table id="ctl00_cphData_tbUserDetails" class="rounded_colhead" cellspacing="0" cellpadding="0"
                                        width="430" border="0">
                                        <tr>
                                            <td style="width: 7px; height: 25px" class="textB" valign="middle" align="right"
                                                rowspan="1"></td>
                                            <td class="textB" valign="middle" align="left" colspan="3" height="35">
                                                <h5>
                                                    <b>
                                                        <asp:Label ID="lblHeading" runat="server" Text="Label"></asp:Label>
                                                    </b>
                                                </h5>
                                            </td>
                                            <td style="height: 25px; width: 50px; font-size: 12px; font-family: Verdana"></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 7px" class="textB" valign="middle" align="right" rowspan="9"></td>
                                            <td class="PanelHeaderBackground" valign="middle" align="right" colspan="3" height="1"></td>
                                            <td height="1"></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 6px" valign="middle" align="right" colspan="3"></td>
                                            <td style="height: 6px"></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 108px; height: 25px" class="textBP" valign="middle" align="right">Deposit
                                            </td>
                                            <td style="width: 8px; height: 25px" class="colon" valign="middle" align="center">:
                                            </td>
                                            <td id="MCredit" runat="server" style="height: 25px" class="textBP" valign="middle"
                                                align="right">0.00
                                            </td>
                                            <td style="height: 25px">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="PanelHeaderBackground" valign="middle" align="right" colspan="3" height="1"></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 108px; height: 25px" class="textBP" valign="middle" align="right">Used
                                            </td>
                                            <td style="width: 8px; height: 25px" class="colon" valign="middle" align="center">:
                                            </td>
                                            <td id="MDebit" runat="server" style="height: 25px" class="textBP" valign="middle"
                                                align="right">0.00
                                            </td>
                                            <td style="height: 25px"></td>
                                        </tr>
                                        <tr>
                                            <td class="PanelHeaderBackground" valign="middle" align="right" colspan="3" height="1"></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 108px; height: 25px" class="textBP" align="right">Balance
                                            </td>
                                            <td style="width: 8px; height: 25px" class="colon" align="center">:
                                            </td>
                                            <td id="MBal" runat="server" style="height: 25px" class="textBP" valign="middle"
                                                align="right">0.00
                                            </td>
                                            <td style="height: 25px"></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 7px; height: 15px" class="text" valign="top" align="right"></td>
                                            <td style="width: 108px; height: 15px" class="text" valign="top" align="right"></td>
                                            <td style="width: 8px; height: 15px" class="text" valign="top" align="center"></td>
                                            <td style="height: 15px" class="text" valign="top" align="left"></td>
                                            <td style="height: 15px" class="text" valign="middle" align="left"></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="col-md-12 table-responsive" runat="server" id="DivH" visible="true">
                                    <asp:Label ID="Label1" runat="server" Text="Total Records"></asp:Label>
                                    <asp:Label ID="lbltotal" runat="server"></asp:Label>
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <table id="customers2" class="table datatable">
                                                <thead>
                                                    <tr>
                                                        <th>SNo
                                                        </th>
                                                        <th>Date
                                                        </th>
                                                        <th>Remark
                                                        </th>
                                                        <th style="text-align: right;">Deposit
                                                        </th>
                                                        <th style="text-align: right;">Used
                                                        </th>
                                                        <%-- <th>
                                                            Balance
                                                        </th>--%>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="RptDirects" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <%# Eval("Rn") %>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("Voucherdate") %>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("Narration") %>
                                                                </td>
                                                                <td style="text-align: right;">
                                                                    <%# Eval("Deposit") %>
                                                                </td>
                                                                <td style="text-align: right;">
                                                                    <%#Eval("used") %>
                                                                </td>
                                                                <%-- <td>
                                                                    <%# Eval("Balance") %>
                                                                </td>--%>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <%-- <asp:AsyncPostBackTrigger ControlID="CmdSave" EventName="Click" />--%>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <div class="pagination-controls">
                                        <asp:LinkButton ID="lnkPrev" runat="server" Text="<< Prev" OnClick="lnkPrev_Click" />
                                        &nbsp;|&nbsp;
                                <asp:LinkButton ID="lnkNext" runat="server" Text="Next >>" OnClick="lnkNext_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>

