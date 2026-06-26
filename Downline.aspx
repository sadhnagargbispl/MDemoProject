<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="Downline.aspx.cs" Inherits="Downline" %>

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
                            <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>Downline Detail</h5>
                        </div>
                        <div class="card-body">
                            <div class="row g-1">
                                <div class="clr">
                                    <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                </div>

                                <div class="col-md-3">
                                    <div class="form-group">
                                        Search By
                                                      <asp:RadioButtonList AutoPostBack="True" ID="rbleg" RepeatDirection="Horizontal" runat="server" OnSelectedIndexChanged ="rbleg_SelectedIndexChanged">
                                                          <asp:ListItem Selected="True">Both </asp:ListItem>
                                                          <asp:ListItem>Left Downline</asp:ListItem>
                                                          <asp:ListItem>Right Downline</asp:ListItem>
                                                      </asp:RadioButtonList>
                                    </div>
                                </div>
                                <div class="col-md-3" id="joiningtype" runat="server" visible="false">
                                    <div class="form-group">
                                        Level
                                                               <asp:Label ID="lbltype" runat="server" Text="Search Type: "></asp:Label>
                                        <asp:DropDownList ID="DDltype" runat="server" class="form-control" AutoPostBack="true">
                                            <asp:ListItem Text="Joining" Value="J"></asp:ListItem>
                                            <asp:ListItem Text="Activation" Value="A"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3" id="startdate" runat="server" visible="false">
                                    <div class="form-group">
                                        <asp:Label ID="lblStartDate" runat="server" Text="Choose Start Date : "></asp:Label>
                                        <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate" ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True" ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$" ValidationGroup="Form-submit"> </asp:RegularExpressionValidator>

                                    </div>
                                </div>
                                <div class="col-md-3" id="enddate" runat="server" visible="false">
                                    <asp:Label ID="lblEndDate" runat="server" Text="Choose End Date : "></asp:Label>
                                    <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate" ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True" ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$" ValidationGroup="Form-submit"></asp:RegularExpressionValidator>

                                </div>
                                <div class="col-md-3" style="margin-top: 20px;" id="idbtnsearch" runat="server" visible="false">
                                    <div class="form-group" style="margin-top: 9px;">
                                        <asp:Button ID="BtnSubmit" runat="server" Text="Search" TabIndex="3" class="btn btn-primary" OnClick="BtnSubmit_Click" />
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="table-responsive">
                                        <table class="table table-bordered">
                                            <tbody>
                                                <tr valign="top">
                                                    <td class="box-body1-clear" align="center" colspan="6">
                                                        <b>Status</b>
                                                    </td>
                                                </tr>
                                                <tr valign="top">
                                                    <td align="center"><strong>Total Left Joined </strong></td>
                                                    <td align="center"><strong>Total Right Joined </strong></td>
                                                    <td align="center"><strong>Total Left Active </strong></td>
                                                    <td align="center"><strong>Total Right Active </strong></td>
                                                    <td align="center">
                                                        <% if (Session["CompID"].ToString() == "1074")
                                                            { %>
                                                        <strong>Total Left BV </strong>
                                                        <% }
                                                            else
                                                            { %>
                                                        <strong>Total Left <%= Session["ColName1"] %> </strong>
                                                        <% } %>
                                                    </td>
                                                    <td align="center">
                                                        <% if (Session["CompID"].ToString() == "1074")
                                                            { %>
                                                        <strong>Total Right BV </strong>
                                                        <% }
                                                            else
                                                            { %>
                                                        <strong>Total Right <%= Session["ColName1"] %> </strong>
                                                        <% } %>
                                                    </td>
                                                </tr>

                                                <tr valign="top">
                                                    <td align="center"><span id="LblMemLJ" runat="server">0</span></td>
                                                    <td align="center"><span id="LblMemRJ" runat="server">0</span></td>
                                                    <td align="center"><span id="LblMemLT" runat="server">0</span></td>
                                                    <td align="center"><span id="LblMemRT" runat="server">0</span></td>
                                                    <td align="center"><span id="LblLeftBv" runat="server">0</span></td>
                                                    <td align="center"><span id="LblRightBv" runat="server">0</span></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>

                                <div id="DivSideA" runat="server" class="col-md-12">
                                    <h4>Left Downline</h4>
                                    <div class="form-group">
                                        <asp:Button ID="BtnExportA" runat="server" Text="Export" CssClass="btn btn-primary" OnClick="BtnExportA_Click" />
                                    </div>
                                    <div class="spacedivider2"></div>

                                    <div class="table-responsive">
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>

                                                <table id="customers2" class="table datatable">
                                                    <thead>
                                                        <tr>
                                                            <th>SNo</th>
                                                            <th>ID No</th>
                                                            <th>
                                                                <% if (Session["CompID"].ToString() == "1091")
                                                                    { %>
                                     Distributor Name
                                <% }
                                    else
                                    { %>
                                     Member Name
                                <% } %>
                                                            </th>
                                                            <th>Sponsor ID</th>
                                                            <th>Date Of Joining</th>
                                                            <th>Package Name</th>
                                                            <th>Activation Date</th>
                                                           <th>
    <% if (Session["CompID"] != null && Session["CompID"].ToString() == "1105") { %>
        Package
    <% } else { %>
        Package MRP
    <% } %>
</th>

                                                            <th>

                                                                <% if (Session["CompID"].ToString() == "1074")
                                                                    { %>
                                    BV
                                <% }
                                    else
                                    { %>
                                                                <%= Session["ColName1"] %>
                                                                <% } %>
                                                            </th>
                                                        </tr>
                                                    </thead>

                                                    <tbody>
                                                        <asp:Repeater ID="RptDirects" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblRowNumber" runat="server"
                                                                            Text='<%# Container.ItemIndex + 1 %>' /></td>
                                                                    <td><%# Eval("IDNo") %></td>
                                                                    <td><%# Eval("MemName") %></td>
                                                                    <td><%# Eval("Refformno") %></td>
                                                                    <td><%# Eval("Doj") %></td>
                                                                    <td><%# Eval("KitName") %></td>
                                                                    <td><%# Eval("TopupDate") %></td>
                                                                    <td><%# Eval("KitAmount") %></td>
                                                                    <td><%# Eval("Bv") %></td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tbody>
                                                </table>

                                            </ContentTemplate>
                                            <Triggers></Triggers>
                                        </asp:UpdatePanel>
                                        <div class="pagination-controls">
                                            <asp:LinkButton ID="lnkPrevA" runat="server" Text="<< Prev" OnClick="lnkPrevA_Click" />
                                            &nbsp;|&nbsp;
    <asp:LinkButton ID="lnkNextA" runat="server" Text="Next >>" OnClick="lnkNextA_Click" />
                                        </div>

                                    </div>
                                </div>

                                <div id="DivSideB" runat="server" class="col-md-12">
                                    <h4>Right Downline</h4>
                                    <div class="form-group">
                                        <asp:Button ID="BtnExportB" runat="server" Text="Export" CssClass="btn btn-primary" OnClick="BtnExportB_Click" />
                                    </div>
                                    <div class="spacedivider2"></div>

                                    <div class="table-responsive">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>

                                                <table id="customers3" class="table datatable">
                                                    <thead>
                                                        <tr>
                                                            <th>SNo</th>
                                                            <th>ID No</th>
                                                            <th>
                                                                <% if (Session["CompID"].ToString() == "1091")
                                                                    { %>
                                      Distributor Name
                                <% }
                                    else
                                    { %>
                                      Member Name
                                <% } %>
                                                            </th>
                                                            <th>Sponsor ID</th>
                                                            <th>Date Of Joining</th>
                                                            <th>Package Name</th>
                                                            <th>Activation Date</th>
                                                                                                                      <th>
    <% if (Session["CompID"] != null && Session["CompID"].ToString() == "1105") { %>
        Package
    <% } else { %>
        Package MRP
    <% } %>
</th>

                                                            <th>
                                                                <% if (Session["CompID"].ToString() == "1074")
                                                                    { %>
                                      BV
                                <% }
                                    else
                                    { %>
                                                                <%= Session["ColName1"] %>
                                                                <% } %>
                                                            </th>
                                                        </tr>
                                                    </thead>

                                                    <tbody>
                                                        <asp:Repeater ID="Repeater3" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblRowNumber" runat="server"
                                                                            Text='<%# Container.ItemIndex + 1 %>' /></td>
                                                                    <td><%# Eval("IDNo") %></td>
                                                                    <td><%# Eval("MemName") %></td>
                                                                    <td><%# Eval("Refformno") %></td>
                                                                    <td><%# Eval("Doj") %></td>
                                                                    <td><%# Eval("KitName") %></td>
                                                                    <td><%# Eval("TopupDate") %></td>
                                                                    <td><%# Eval("KitAmount") %></td>
                                                                    <td><%# Eval("Bv") %></td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tbody>
                                                </table>

                                            </ContentTemplate>
                                            <Triggers></Triggers>
                                        </asp:UpdatePanel>
                                        <div class="pagination-controls">
                                            <asp:LinkButton ID="lnkPrevB" runat="server" Text="<< Prev" OnClick="lnkPrevB_Click" />
                                            &nbsp;|&nbsp;
    <asp:LinkButton ID="lnkNextB" runat="server" Text="Next >>" OnClick="lnkNextB_Click" />
                                        </div>

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



