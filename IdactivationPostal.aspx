<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="IdactivationPostal.aspx.cs" Inherits="IdactivationPostal" %>


<asp:Content ContentPlaceHolderID="head" runat="server" ID="content2">










    <style>
        .container i {
            margin-left: -30px;
            cursor: pointer;
        }
    </style>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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
                            <h5 class="mb-0">Id Activation</h5>
                        </div>
                        <div class="card-body">
                            <div class="row g-1">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <h4>Available Balance:<span class="red" id="AvailableBal" style="color: Red" runat="server"></span>
                                            <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                        </h4>
                                    </div>
                                    <div class="form-group" id="DiMemberId" runat="server">
                                        <label for="inputdefault">
                                            Member Id</label>
                                        <asp:TextBox ID="txtMemberId" runat="server" CssClass="form-control validate[required]"
                                            AutoPostBack="true" OnTextChanged="txtMemberId_TextChanged"></asp:TextBox>
                                        <asp:Label ID="lblFormno" runat="server" Visible="false"></asp:Label>
                                    </div>
                                    <div class="form-group" id="DivMemberName" runat="server">
                                        <label for="inputdefault">
                                            Member Name</label>
                                        <asp:HiddenField ID="HDnTopupSeq" runat="server" />
                                        <asp:Label ID="LblMobile" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lblemail" runat="server" Visible="false"></asp:Label>
                                        <asp:TextBox ID="TxtMemberName" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </div>

                                    <div runat="server" id="postalshowdiv" visible="false">
                                        <h4>
                                            <span>Postal Address</span>
                                        </h4>
                                        <div class="form-group greybt ">
                                            <label class="control-label col-sm-2">
                                                Address <span class="red">*</span></label>
                                            <asp:TextBox ID="TxtPostalAddress" CssClass="form-control validate[required]" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form-group greybt ">
                                            <label class="control-label col-sm-2">
                                                Pincode<span class="red">*</span></label>
                                            <asp:TextBox ID="TxtPostPincode" CssClass="form-control validate[required]" onkeypress="return isNumberKey(event);"
                                                runat="server" MaxLength="6" autocomplete="off"></asp:TextBox>
                                        </div>
                                        <div class="form-group greybt ">
                                            <label class="control-label col-sm-2">
                                                State<span class="red">*</span></label>
                                            <asp:DropDownList ID="ddlPostSate" runat="server" CssClass="form-control ">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group greybt ">
                                            <label class="control-label col-sm-2">
                                                District<span class="red">*</span></label>
                                            <asp:TextBox ID="TxtPostDistrict" CssClass="form-control validate[required]" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="form-group greybt ">
                                            <label class="control-label col-sm-2">
                                                City <span class="red">*</span></label>
                                            <asp:TextBox ID="TxtPostCity" CssClass="form-control validate[required]" runat="server"></asp:TextBox>
                                        </div>

                                    </div>






                                    <asp:Label ID="kitid" runat="server" Visible="false"></asp:Label>
                                    <div class="form-group" id="Div1" runat="server">
                                        <label for="inputdefault">
                                            <%
                                                string compId = Session["CompID"].ToString();

                                                if (compId == "1078" || compId == "1093")
                                                {
                                            %>
        Choose
                                            <%
                                                }
                                                else
                                                {
                                            %>
        Choose Package
                                            <%
                                                }
                                            %>
                                        </label>
                                        <asp:Label ID="LblCondition" runat="server" Visible="false"></asp:Label>
                                        <asp:DropDownList ID="CmbKit" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CmbKit_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputdefault">
                                            Amount <span class="red">*</span></label>
                                        <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control validate[required]"
                                            onchange="checkWAmt();" onkeypress="return isNumberKey(event);" Enabled="false" OnTextChanged="txtAmount_TextChanged" Text="0"></asp:TextBox><asp:Label
                                                ID="LblAmount" runat="server" Visible="false"></asp:Label>

                                    </div>
                                    <div class="form-group" id="divEP" runat="server" visible="false">
                                        <label for="inputdefault">
                                            EP <span class="red">*</span></label>
                                        <asp:TextBox ID="txtEP" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </div>
                                    <%
                                        if (Session["CompID"] != null && Session["CompID"].ToString() == "1059")
                                        {
                                    %>

                                    <div class="form-group" id="txtgoldnameDiv2" runat="server" visible="false">
                                        <label for="inputdefault">
                                            Enter Gold Product Code</label>
                                        <asp:TextBox ID="txtgoldname" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="form-group" id="TxtaddressDiv2" runat="server" visible="false">
                                        <label for="inputdefault">
                                            Address</label>
                                        <asp:TextBox ID="Txtaddress" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <%
                                        }
                                    %>

                                    <div class="form-group">
                                        Enter Transaction Password:
                                             <asp:TextBox ID="TxtPassword" runat="server" TextMode="Password" CssClass="form-control showeye"></asp:TextBox>
                                        <i class="fa fa-eye fa-eye field_icon" aria-hidden="true" style="transform: translateY(-190%); float: right; padding-right: 10px;" id="toggle_pwd"></i>
                                        <%--<span id="toggle_pwd" class="fa fa-eye fa-eye field_icon" ></span>--%>
                                    </div>
                                    <div class="form-group">
                                        <asp:Button ID="cmdSave1" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Validation" OnClick="cmdSave1_Click" />
                                    </div>
                                    <div class="form-group ">
                                        <asp:Label ID="LblError" runat="server" Visible="false"></asp:Label>
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
