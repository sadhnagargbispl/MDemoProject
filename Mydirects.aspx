<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="Mydirects.aspx.cs" Inherits="Mydirects" %>

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
                <%--      <li class="breadcrumb-item">
             <h4 class="page-title m-b-0">Dashboard</h4>
         </li>
         <li class="breadcrumb-item">
             <a href="#"><i data-feather="home"></i></a>
         </li>
         <li class="breadcrumb-item active">Dashboard</li>--%>
            </ul>
            <div class="row">
                <div class="col-12 col-sm-12 col-lg-12">
                    <div class="card">
                        <div class="card-header">
                            <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>My Direct Report</h5>
                        </div>
                        <div class="card-body">
                            <div class="row g-1">
                                <div class="clr">
                                    <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group" runat="server" id="DivSearchBy" visible="true">
                                        <label for="inputdefault">
                                            Search By</label>
                                        <asp:HiddenField ID="hdnSessn" runat="server" />
                                        <asp:DropDownList ID="rbtnsearch" runat="server" class="form-control">
                                            <asp:ListItem Text="Both" Selected="True" Value="L"></asp:ListItem>
                                            <asp:ListItem Text="Left" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Right" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group"  runat="server" id="DivSearchByGreen" visible="false">
                                        <label for="inputdefault">
                                            Search By</label>
                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                        <asp:DropDownList ID="rbtnsearchr" runat="server" class="form-control">
                                            <asp:ListItem Text="Both" Selected="True" Value="L"></asp:ListItem>
                                            <asp:ListItem Text="Group A" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Group B" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Group C" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-4" id="divSearch" runat="server">
                                    <div class="form-group">
                                        <label for="inputdefault">
                                            Search</label>
                                        <asp:DropDownList ID="DDlSearchby" CssClass="form-control" TabIndex="2" runat="server">
                                            <asp:ListItem Text="All" Value="" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <asp:Button ID="BtnSubmit" runat="server" Text="Search" TabIndex="3" class="btn btn-primary"
                                            Style="margin-top: 4%" OnClick="BtnSubmit_Click" />
                                    </div>

                                </div>
                                <div class="col-md-12 table-responsive">
                                    <table id="table" class="table table-bordered">
                                        <tbody>
                                            <tr>
                                                <td></td>
                                                <th style="text-align: center">Left
                                                </th>
                                                <th style="text-align: center">Right
                                                </th>
                                                <th style="text-align: center">Total
                                                </th>
                                            </tr>
                                            <tr>
                                                <th>Total Direct
                                                </th>
                                                <td id="tdDirectleft" runat="server" style="text-align: center">0
                                                </td>
                                                <td id="tdDirectright" runat="server" style="text-align: center">0
                                                </td>
                                                <td id="TotalDirect" runat="server" style="text-align: center">0
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Active Direct
                                                </th>
                                                <td id="tddirectActive" runat="server" style="text-align: center">0
                                                </td>
                                                <td id="tdindirectActive" runat="server" style="text-align: center">0
                                                </td>
                                                <td id="TotalActive" runat="server" style="text-align: center">0
                                                </td>
                                            </tr>

                                            <tr id="trDirectBV" runat="server">
                                                <th>
                                                    <%
                                                        if (Session["CompID"] != null && Session["CompID"].ToString() == "1074")
                                                        {
                                                    %>
        Direct BV
                                                    <%
                                                        }
                                                        else if (Session["CompID"].ToString() == "1110")
                                                        {
                                                    %>
            Direct Business
                                                    <%
                                                        }

                                                        else
                                                        {
                                                            string colName1 = Session["ColName1"]?.ToString() ?? "";
                                                    %>
        Direct <%= colName1 %>
                                                    <%
                                                        }
                                                    %>

                                                                 
                                                </th>
                                                <td id="Directunit" runat="server" style="text-align: center">0
                                                </td>
                                                <td id="indirectunit" runat="server" style="text-align: center">0
                                                </td>
                                                <td id="totalunit" runat="server" style="text-align: center">0
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="col-md-12 table-responsive">
                                    <asp:Label ID="Label1" runat="server" Text="Total Records"></asp:Label>
                                    <asp:Label ID="lbltotal" runat="server"></asp:Label>
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <table id="customers2" class="table datatable">
                                                <thead>
                                                    <tr>
                                                        <th>SNo
                                                        </th>
                                                        <th>Level
                                                        </th>

                                                        <th>ID No
                                                        </th>
                                                        <th>
                                                            <%
                                                                if (Session["CompID"] != null && Session["CompID"].ToString() == "1091")
                                                                {
                                                            %>
        Distributor Name
                                                            <%
                                                            }
                                                            else
                                                            {
                                                            %>
        Member Name
                                                            <%
                                                                }
                                                            %>

                                                        </th>
                                                        <th>Sponsor ID
                                                        </th>
                                                        <th>Sponsor Name
                                                        </th>
                                                        <th>Group Name
                                                        </th>

                                                        <% if (Session["CompID"] == null || Session["CompID"].ToString() != "1105")
                                                            { %>
                                                        <th>
                                                            <%
                                                                if (Session["CompID"] != null && Session["CompID"].ToString() == "1074")
                                                                {
                                                            %>
        Direct BV
    <%

        }
        else if (Session["CompID"].ToString() == "1110")
        {
    %>
             Business
                                                            <%
                                                                }
                                                                else
                                                                {
                                                                    string colName1 = Session["ColName1"]?.ToString() ?? "";
                                                            %>
        Direct <%= colName1 %>
                                                            <%
                                                                }
                                                            %>
                                                        </th>
                                                        <% } %>

                                                        <th>Package Name
                                                        </th>
                                                        <th>Active Status
                                                        </th>
                                                        <th>Activation Date
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
                                                                <td>
                                                                    <%#Eval("Mlevel")%>
                                                                </td>

                                                                <td>
                                                                    <%#Eval("IDNo")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("MemName")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("SponsorId")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("MemberName")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("Position")%>
                                                                </td>
                                                                <% if (Session["CompID"] == null || Session["CompID"].ToString() != "1105")
                                                                    { %>
                                                                <td>
                                                                    <%# Eval("BV") %>
                                                                </td>
                                                                <% } %>

                                                                <td>
                                                                    <%#Eval("PackageName")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("Status")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("UpgradeDate") %>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="BtnSubmit" EventName="Click" />
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

