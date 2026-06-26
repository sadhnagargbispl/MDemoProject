<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="ProductRequestDetail.aspx.cs" Inherits="ProductRequestDetail" %>

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
                                                                <th>SNo</th>
                                                                <th>Order No.</th>
                                                                <th>Order Date</th>
                                                                <th>Package Name</th>
                                                                <th>Order Amount</th>
                                                                <th>Order Type</th>
                                                                <% if (Session["CompID"].ToString() == "1108" || Session["CompID"].ToString() == "1105")
                                                                    { %>
                                                                <th>Invoice</th>
                                                                <% } %>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:Repeater ID="RptDirects" runat="server">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td><%# Container.ItemIndex + 1 %></td>
                                                                        <td style="color: Blue"><%# Eval("Orderno") %></td>
                                                                        <td><%# Eval("OrderDate") %></td>
                                                                        <td><%# Eval("KitName") %></td>
                                                                        <td><%# Eval("OrderAmount") %></td>
                                                                        <td><%# Eval("Status") %></td>
                                                                        <% if (Session["CompID"].ToString() == "1108" || Session["CompID"].ToString() == "1105")
                                                                            { %>
                                                                        <td>
                                                                            <a href='Invoice.aspx?OrderNo=<%# Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Eval("Orderno").ToString())) %>'
                                                                                target="_blank"
                                                                                class="btn btn-sm btn-primary">Invoice
                                                                            </a>
                                                                        </td>
                                                                        <% } %>
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

