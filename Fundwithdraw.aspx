<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="Fundwithdraw.aspx.cs" Inherits="Fundwithdraw" %>

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
                            <h5 class="mb-0">Fund Withdrawal Request</h5>
                        </div>
                        <div class="card-body">
                            <div class="row g-1">
                                <h4>Your Bank Details<span id="Span6" style="font-family: Verdana; font-size: 8px">* Update
                                    bank details if any changes.</span></h4>
                                <div class="table-responsive">
                                    <asp:DataGrid ID="GrdBankDetail" runat="server" CssClass="table table-striped table-bordered"
                                        CellPadding="3" HorizontalAlign="Center" AutoGenerateColumns="False" Width="100%"
                                        PageSize="1">
                                        <Columns>
                                            <asp:BoundColumn DataField="SNo" HeaderText="Sr.No." />

                                            <asp:TemplateColumn HeaderText="BankID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblBankId" runat="server"
                                                        Text='<%# Eval("BankID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>

                                            <asp:TemplateColumn HeaderText="Payee Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblName" runat="server"
                                                        Text='<%# Eval("PayeeName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>

                                            <asp:TemplateColumn HeaderText="Bank Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblBank" runat="server"
                                                        Text='<%# Eval("BankName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>

                                            <asp:TemplateColumn HeaderText="Branch Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblBranch" runat="server"
                                                        Text='<%# Eval("BranchName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>

                                            <asp:TemplateColumn HeaderText="Account No">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblAcNo" runat="server"
                                                        Text='<%# Eval("AcNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>

                                            <asp:TemplateColumn HeaderText="IFSC Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblIfsc" runat="server"
                                                        Text='<%# Eval("IfsCode") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>

                                            <asp:TemplateColumn HeaderText="PAN No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPanNo" runat="server"
                                                        Text='<%# Eval("PanNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>

                                        <PagerStyle Mode="NumericPages" CssClass="PagerStyle"></PagerStyle>
                                    </asp:DataGrid>
                                </div>
                                <div class="col-sm-6 pull-none">

                                    <% if (Session["CompID"] != null && Session["CompID"].ToString() == "1004")
                                        { %>
                                    <h4>Fund Withdrawal (Minimum Request Amount 100 /-)</h4>

                                    <% }
                                        else if (Session["CompID"].ToString() == "1015")
                                        { %>
                                    <h4>Fund Withdrawal (Minimum Request 10 EP)</h4>

                                    <% }
                                        else if (Session["CompID"].ToString() == "1055")
                                        { %>
                                    <h4>Fund Withdrawal (Minimum Request Amount 2000 /-)</h4>

                                    <% }
                                        else if (Session["CompID"].ToString() == "1009")
                                        { %>
                                    <h4>Fund Withdrawal (Minimum Request Amount 500 /-)</h4>
                                    <span style="color: Red;"><b>You Can Request Sent On Saturday.</b></span>

                                    <% }
                                        else if (Session["CompID"].ToString() == "1060")
                                        { %>
                                    <h4>Fund Withdrawal (Minimum Request Amount 300 /-)</h4>

                                    <% }
                                        else if (Session["CompID"].ToString() == "1105")
                                        { %>
                                    <h4>Fund Withdrawal (Minimum Request Amount 300 /-)</h4>

                                    <% }
                                        else if (Session["CompID"].ToString() == "1066")
                                        { %>
                                    <h4>Fund Withdrawal (Minimum Request Amount 1000 /-)</h4>

                                    <% }
                                        else if (Session["CompID"].ToString() == "1073")
                                        { %>
                                    <h4>Fund Withdrawal (Minimum Request Amount 100 /-)</h4>

                                    <% }
                                        else
                                        { %>
                                    <h4>Fund Withdrawal (Minimum Request Amount 500 /-)</h4>
                                    <% } %>


                                    <div class="form-group ">
                                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="16px"
                                            Style="color: Red; font-weight: bold"></asp:Label>
                                    </div>
                                    <div class="form-group ">
                                        <label for="inputdefault">
                                            Available Fund<span style="color: Red;">*</span></label>
                                        <asp:HiddenField ID="hdnSessn" runat="server" />
                                        <asp:TextBox ID="TxtCredit" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                                        <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                    </div>

                                    <div class="form-group ">
                                        <label for="inputdefault">
                                            Withdrawal Amount<span class="red">*</span></label>
                                        <asp:TextBox ID="TxtReqAmt" runat="server" CssClass="form-control" MaxLength="8"
                                            onkeypress="return isNumberKey(event);" Enabled="true" AutoPostBack="true">0</asp:TextBox>
                                    </div>
                                    <div class="form-group " runat="server" visible="false" id="divTds">
                                        <label for="inputdefault">
                                            <% if (Session["CompID"] != null && Session["CompID"].ToString() == "1083")
                                                { %>
    Social Charity
                                            <% }
                                                else
                                                { %>
    TDS Amount
                                            <% } %>
                                            <span class="red">*</span></label>
                                        <asp:TextBox ID="txtTDS" runat="server" CssClass="form-control" MaxLength="8" onkeypress="return isNumberKey(event);"
                                            ReadOnly="true">0</asp:TextBox>
                                    </div>
                                    <div class="form-group " runat="server" visible="false" id="divadmin">
                                        <label for="inputdefault">
                                            Admin Charge<span class="red">*</span></label>
                                        <asp:TextBox ID="txtIMPS" runat="server" CssClass="form-control" MaxLength="8" onkeypress="return isNumberKey(event);"
                                            ReadOnly="true">0</asp:TextBox>
                                    </div>
                                    <div class="form-group " runat="server" visible="false" id="divtotal">
                                        <label for="inputdefault">
                                            Amount Transferred<span class="red">*</span></label>
                                        <asp:TextBox ID="txtNetAmt" runat="server" CssClass="form-control" MaxLength="8"
                                            onkeypress="return isNumberKey(event);" ReadOnly="true">0</asp:TextBox>
                                    </div>

                                    <div class="form-group ">
                                        <asp:Button ID="BtnSubmit" runat="server" Text="Send Request" class="btn btn-primary" Visible="true" />
                                    </div>
                                    <div runat="server" id="Div1" visible="false">
                                        <div class="form-group">
                                            <label for="idproof">
                                                Enter Transaction Password:
                                            </label>
                                            <asp:TextBox ID="TxtPassword" runat="server" TextMode="Password" CssClass="form-control" Enabled="true"></asp:TextBox>
                                        </div>

                                        <div class="form-group ">
                                            <%-- <asp:Button ID="btnOtp" runat="server" Text="Send Request" class="btn btn-primary" ValidationGroup="Save1" 
                           OnClientClick ="this.disabled=true; this.value='Sending Request…';" UseSubmitBehavior="false"></asp:Button>
                                            --%>
                                            <asp:Button ID="BtnPassword" runat="server" Text="Submit" class="btn btn-primary"
                                                CausesValidation="false" Style="margin-top: 20px" OnClientClick="this.disabled=true; this.value='Sending…';" UseSubmitBehavior="false" />
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                                ShowSummary="False" ValidationGroup="Save1" />

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



    <div class="feedbackpop" id="feedbackpop1" runat="server" visible="false">
        <div id="feedbackwrap" runat="server">
            <div id="feedbackform" width="100%" runat="server" class="feedbackform">
                <div id="closeicon">
                    <a href="#" onclick="myFunction()"></a>
                </div>
                <div style="margin: 10px 10px 10px 10px; padding: 15px 15px 15px 15px; border: Solid 1px #0089e1; width: 200px">
                    <center>
                    </center>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            $("#toggle_pwd").click(function () {
                $(this).toggleClass("fa-eye fa-eye-slash");
                var type = $(this).hasClass("fa-eye-slash") ? "text" : "password";
                $(".showeye").attr("type", type);
            });
        });
    </script>
</asp:Content>

