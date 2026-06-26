<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="Groupdirects.aspx.cs" Inherits="Groupdirects" %>

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
                            <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>Level Wise Direct Report</h5>
                        </div>
                        <div class="card-body">
                            <div class="row g-1">
                                <div class="clr">
                                    <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                </div>
                                <div id="datewisepanel" runat="server" visible="false">
                                    <div class="col-md-4" id="Div1" runat="server">
                                        <div class="form-group">
                                            <asp:Label ID="lbltype" runat="server" Text="Search Type: "></asp:Label>
                                            <asp:DropDownList ID="DDlDate" runat="server" class="form-control" AutoPostBack="true">
                                                <asp:ListItem Text="All" Value="A"></asp:ListItem>
                                                <asp:ListItem Text="Joining" Value="J"></asp:ListItem>
                                                <asp:ListItem Text="Activation" Value="B"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-4" id="Div2" runat="server" style="margin-top: 15px;">
                                        <div class="form-group">
                                            <asp:Label ID="lblStartDate" runat="server" Text="Choose Start Date : "></asp:Label>
                                            <asp:TextBox ID="txtStartDate" runat="server" class="form-control"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate"
                                                Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDate"
                                                ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                        </div>

                                    </div>
                                    <div class="col-md-4" id="Div3" runat="server" style="margin-top: 15px;">
                                        <div class="form-group">
                                            <asp:Label ID="lblEndDate" runat="server" Text="Choose End Date : "></asp:Label>
                                            <asp:TextBox ID="txtEndDate" runat="server" class="form-control"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate"
                                                Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDate"
                                                ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                        </div>

                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group" runat="server" id="DivSearchBy" visible="true">
                                        Search By
                                                        <asp:DropDownList ID="rbtnsearch" AutoPostBack="true" runat="server" class="form-control">
                                                            <asp:ListItem Text="Level Wise" Selected="True" Value="L"></asp:ListItem>
                                                            <asp:ListItem Text="Left" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Right" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group" runat="server" id="DivSearchByGreen" visible="false">
                                        Search By
                      <asp:DropDownList ID="rbtnsearchr" AutoPostBack="true" runat="server" class="form-control">
                          <asp:ListItem Text="Level Wise" Selected="True" Value="L"></asp:ListItem>
                          <%--               <asp:ListItem Text="Group A" Value="1"></asp:ListItem>
                          <asp:ListItem Text="Group B" Value="2"></asp:ListItem>
                          <asp:ListItem Text="Group C" Value="3"></asp:ListItem>--%>
                      </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3" id="lbllevel" runat="server">
                                    <div class="form-group">
                                        Level
                                                            <asp:DropDownList ID="DdlLevel" CssClass="form-control" TabIndex="1" runat="server">
                                                            </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3" id="divSearch" runat="server">
                                    <div class="form-group">
                                        Search
                                                            <asp:DropDownList ID="DDlSearchby" CssClass="form-control" TabIndex="2" runat="server">
                                                                <asp:ListItem Text="All" Value="" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                                                <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                                                            </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3" id="div4" runat="server" style="margin-top: 20px;">
                                    <div class="form-group" style="margin-top: 9px;">
                                        <asp:Button ID="BtnSubmit" runat="server" Text="Search" TabIndex="3" class="btn btn-primary" OnClick="BtnSubmit_Click" />

                                    </div>
                                </div>
                                <div class="col-md-12 table-responsive" id="divall" runat="server" visible="false">
                                    <table id="table" class="table table-bordered">
                                        <tbody>
                                            <% if (Session["CompID"] != null && (Session["CompID"].ToString() == "1106") || (Session["CompID"].ToString() == "1109"))
                                                { %>

                                            <tr>
                                                <td></td>
                                                <th style="text-align: center">Direct
                                                </th>
                                                <th style="text-align: center">Indirect
                                                </th>
                                                <th style="text-align: center">Total
                                                </th>
                                            </tr>
                                            <% }
                                                else
                                                { %>
                                            <tr>
                                                <td></td>
                                                <th style="text-align: center">Left
                                                </th>
                                                <th style="text-align: center">Right
                                                </th>
                                                <th style="text-align: center">Total
                                                </th>
                                            </tr>
                                            <% } %>


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
                                            <tr id="trDirect" runat="server">
                                                <th>Direct
                                                               <%
                                                                   if (Session["CompId"].ToString() != null && Session["CompId"].ToString() == "1070")
                                                                   {
                                                               %>
        IP
                                                    <%
                                                        }
                                                        else if (Session["CompID"] != null && Session["CompID"].ToString() == "1074")
                                                        {
                                                    %>
        BV
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
                                                    <%= colName1 %>
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
                                <div class="col-md-12 table-responsive" id="divgreen" runat="server" visible="false">
                                    <table id="tablegreen" class="table table-bordered">
                                        <tbody>
                                            <% if (Session["CompID"] != null && (Session["CompID"].ToString() == "1109"))
                                                { %>

                                            <tr>
                                                <td></td>

                                                <th style="text-align: center">Total
                                                </th>
                                            </tr>
                                            <% }
                                                else
                                                { %>
                                            <tr>
                                                <td></td>

                                                <th style="text-align: center">Total
                                                </th>
                                            </tr>
                                            <% } %>


                                            <tr>
                                                <th>Total Direct
                                                </th>

                                                <td id="TotalDirect1" runat="server" style="text-align: center">0
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Active Direct
                                                </th>

                                                <td id="TotalActive1" runat="server" style="text-align: center">0
                                                </td>
                                            </tr>
                                            <tr id="tr2" runat="server">
                                                <th>Total Team</th>
                                                <td id="TotalTeam" runat="server" style="text-align: center">0
                                                </td>
                                            </tr>
                                            <tr id="tr1" runat="server">
                                                <th>Active Team</th>
                                                <td id="TotalActiveTeam" runat="server" style="text-align: center">0
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="col-md-12 table-responsive" runat="server" id="DivH" visible="true">
                                    <asp:Label ID="Label1" runat="server" Text="Total Records"></asp:Label>
                                    <asp:Label ID="lbltotal" runat="server"></asp:Label>
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="Label2" runat="server" Text="Total Records" Visible="false"></asp:Label>
                                            <asp:Label ID="Label3" runat="server"></asp:Label>
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
                                                            <% if (Session["CompId"] != null && Session["CompId"].ToString() == "1091")
                                                                {%>
                                                                            Distributor Name
                                                                            <% }
                                                                                else
                                                                                { %>
                                                                            Member Name
                                                                            <% } %>
                                                        </th>
                                                        <th>Sponsor ID
                                                        </th>
                                                        <th>Sponsor Name
                    
                                                        </th>
                                                        <% if (Session["CompId"] != null && Session["CompId"].ToString() == "1057")
                                                            {%>
                                                        <th>Position
                                                        </th>
                                                        <% } %>
                                                        <% if (Session["CompId"] == null || Session["CompId"].ToString() != "1105")
                                                            { %>
                                                        <th>
                                                            <% if (Session["CompId"] != null && Session["CompId"].ToString() == "1070")
                                                                { %>
        IP
    <% }
        else if (Session["CompId"] != null && Session["CompId"].ToString() == "1074")
        { %>
        BV
    <% }
        else if (Session["CompId"] != null && Session["CompId"].ToString() == "1110")
        { %>
            Business
        <% }
            else
            { %>
                                                            <%= Session["ColName1"] %>
                                                            <% } %>
                                                        </th>
                                                        <% } %>

                                                        <th>Package Name
                                                        </th>
                                                        <th>Active Status
                                                        </th>
                                                        <th>Joining Date
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
                                                                    <%#Eval("MLevel")%>
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
                                                                <% if (Session["CompId"] != null && Session["CompId"].ToString() == "1057")
                                                                    {%>
                                                                <td>
                                                                    <%#Eval("Position")%>
                                                                </td>
                                                                <% } %>
                                                                <td style='<%# (Session["CompId"] != null && Session["CompId"].ToString() == "1105")
        ? "display:none;": "" %>'>
                                                                    <%# Eval("BV") %>
                                                                </td>

                                                                <td>
                                                                    <%#Eval("PackageName")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("Status")%>
                                                                </td>
                                                                <td>
                                                                    <%#Eval("Doj")%>
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

