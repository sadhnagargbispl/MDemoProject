<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="GroupbusinessDate.aspx.cs" Inherits="GroupbusinessDate" %>

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

                                <div class="col-md-3" id="startdate" runat="server" >
                                    <div class="form-group">
                                        <asp:Label ID="lblStartDate" runat="server" Text="Choose Start Date : "></asp:Label>
                                        <asp:TextBox ID="TxtFromDate" runat="server" class="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtFromDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TxtFromDate" ErrorMessage="Invalid Start Date" Font-Names="arial" Font-Size="10px"
                                            SetFocusOnError="True" ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$" ValidationGroup="Form-submit"> </asp:RegularExpressionValidator>

                                    </div>
                                </div>
                                <div class="col-md-3" id="enddate" runat="server" >
                                    <asp:Label ID="lblEndDate" runat="server" Text="Choose End Date : "></asp:Label>
                                    <asp:TextBox ID="TxtToDate" runat="server" class="form-control"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="TxtToDate" Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtToDate" ErrorMessage="Invalid End Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True" ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$" ValidationGroup="Form-submit"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-3" style="margin-top: 20px;" id="idbtnsearch" runat="server" >
                                    <div class="form-group" style="margin-top: 9px;">
                                        <asp:Button ID="BtnSearch" runat="server" Text="Search" TabIndex="3" class="btn btn-primary" OnClick="BtnSearch_Click" />
                                    </div>
                                </div>
                                <div id="DivSideA" runat="server" class="col-md-12">
                                    <asp:Label ID="Label2" runat="server" Text="Total Records" Visible="false"></asp:Label>
                                    <asp:Label ID="lbltotal" runat="server"></asp:Label>
                                    <div class="table-responsive" style="overflow: scroll;">
                                        <table id="Table1" class="table table-bordered table-striped">
                                            <thead>
                                                <tr style="background-color: #102f6d;">
                                                    <th>SNo
                                                    </th>
                                                    <th>User ID
                                                    </th>
                                                    <th>Name
                                                    </th>
                                                    <th>Activation Date
                                                    </th>
                                                    <th>Rank
                                                    </th>
                                                    <th>Self Business
                                                    </th>
                                                    <th>Team Business
                                                    </th>
                                                    <th>Total Business
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="Grdtotal" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <%--  <asp:Label ID="lblID" runat="server" Text='<%#Eval("FormNo")%>' Visible="false"></asp:Label>--%>
                                                                <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                            </td>
                                                            <td>
                                                                <%#Eval("IDno")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("MemberName")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("Activation Date")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("Rank")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("Investment")%>
                                                            </td>

                                                            <td>
                                                                <%#Eval("TeamInvestment")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("Total")%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>
                                        <br />
                                        <table id="customers2" class="table datatable">
                                            <thead>
                                                <tr>
                                                    <th>Leg No.
                                                    </th>
                                                    <th>User ID
                                                    </th>
                                                    <th>Name
                                                    </th>
                                                    <th>Joining Date
                                                    </th>
                                                    <th>Activation Date
                                                    </th>
                                                    <th>Rank
                                                    </th>
                                                    <th>Self Business
                                                    </th>
                                                    <th>Team Business
                                                    </th>
                                                    <th>Total Business
                                                    </th>
                                                    <th>Downline
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="DLDirects" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblID" runat="server" Text='<%#Eval("FormNo")%>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                            </td>
                                                            <td>
                                                                <%#Eval("IDno")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("MemFirstName")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("Doj")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("UpgradeDate")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("Rank")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("Investment")%>
                                                            </td>

                                                            <td>
                                                                <%#Eval("TeamInvestment")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("Total")%>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="LblStatus" runat="server" Text="Downline"></asp:Label>
                                                                <asp:ImageButton ID="edit" runat="server" ImageUrl="images/down.png" OnClick="PerformData"
                                                                    Style="background-color: White;" />

                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>
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
