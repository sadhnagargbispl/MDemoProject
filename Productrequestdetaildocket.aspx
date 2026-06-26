<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="Productrequestdetaildocket.aspx.cs" Inherits="Productrequestdetaildocket" %>

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
                            <h5 class="mb-0">Order Detail</h5>
                        </div>
                        <div class="card-body">
                            <div class="row g-1">
                                <div class="clr">
                                    <asp:Label ID="Label2" runat="server" CssClass="error"></asp:Label>
                                    <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Total Record : "></asp:Label>
                                    <asp:Label ID="lbltotal" runat="server"></asp:Label>
                                </div>
                                <div class="form-horizontal">
                                    <div class="table-responsive">
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <div id="customer1" runat="server">
                                                    <table id="customers2" class="table table-bordered table-striped table-actions">
                                                        <thead>
                                                            <tr>
                                                                <th>SNo
                                                                </th>
                                                                <th>OrderNo/Bill NO.
                                                                </th>
                                                                <th>Order Date
                                                                </th>
                                                                <th>Request
                                                                </th>
                                                                <th>Order Amount
                                                                </th>
                                                                <th>BV
                                                                </th>
                                                                <th>Courier Name
                                                                </th>
                                                                <th>Docket No.
                                                                </th>
                                                                <th>Docket Date
                                                                </th>
                                                                <th>Website
                                                                </th>
                                                                <th>Status
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:Repeater ID="RptDirects" runat="server">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                                        </td>
                                                                        <td style="color: Blue">
                                                                            <%#Eval("Orderno")%>
                                                                        </td>
                                                                        <td>
                                                                            <%#Eval("OrderDate")%>
                                                                        </td>
                                                                        <td>
                                                                            <%#Eval("KitName")%>
                                                                        </td>
                                                                        <td>
                                                                            <%#Eval("OrderAmount")%>
                                                                        </td>

                                                                        <td>
                                                                            <%#Eval("BV")%>
                                                                        </td>

                                                                        <td>
                                                                            <%#Eval("CourierName")%>
                                                                        </td>
                                                                        <td>
                                                                            <%#Eval("DocketNo")%>
                                                                        </td>
                                                                        <td>
                                                                            <%#Eval("DocketDate")%>
                                                                        </td>
                                                                        <td style="color: Blue;">
                                                                            <%#Eval("Website")%>
                                                                        </td>
                                                                        <td>
                                                                            <%#Eval("Status")%>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
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
