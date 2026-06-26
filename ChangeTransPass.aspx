<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="ChangeTransPass.aspx.cs" Inherits="ChangeTransPass" %>

<%--<script runat="server">

    protected void BtnUpdate_Click(object sender, EventArgs e)
    {

    }
</script>--%>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <style>
        .container i {
            margin-left: -30px;
            cursor: pointer;
        }
    </style>

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
                            <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>Change Transaction Password</h5>
                        </div>
                        <div class="card-body">
                            <div class="row g-1">
                                <div class="clr">
                                    <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="inputdefault">
                                            Old Transaction Password<span class="red">*</span></label>
                                        <asp:HiddenField ID="hdnSessn" runat="server" />
                                        <asp:TextBox ID="oldpass" class="validate[required] form-control" TextMode="Password"
                                            runat="server"></asp:TextBox>

                                    </div>
                                    <div class="form-group">
                                        <label for="inputdefault">
                                            New Transaction Password<span class="red">*</span></label>
                                        <asp:TextBox ID="pass1" class="validate[required,minSize[5],maxSize[10]] form-control showeye"
                                            TextMode="Password" runat="server"></asp:TextBox>
                                        <%--  <span id="toggle_pwd" class="fa fa-fw fa-eye field_icon" style="transform: translateY(-155%); margin-left: 43em"></span>
                                        --%>
                                    </div>
                                    <i class="fa fa-eye fa-eye field_icon" aria-hidden="true" style="transform: translateY(-371%); float: right; padding-right: 10px;" id="toggle_pwd"></i>
                                    <div class="form-group">
                                        <label for="inputdefault">
                                            Confirm Transaction Password<span class="red">*</span></label>
                                        <asp:TextBox ID="pass2" class="validate[required,minSize[5],maxSize[10]] form-control eye"
                                            TextMode="Password" runat="server"></asp:TextBox>
                                        <%-- <span id="toggle_pwd1" class="fa fa-fw fa-eye field_icon" style="transform: translateY(-155%); margin-left: 43em"></span>
                                        --%>
                                    </div>
                                    <i class="fa fa-eye fa-eye field_icon" aria-hidden="true" style="transform: translateY(-371%); float: right; padding-right: 10px;" id="toggle_pwd1"></i>
                                    <asp:CompareValidator ID="CompareValidator1" ControlToValidate="Pass1" ControlToCompare="Pass2"
                                        Type="String" Operator="Equal" Text="Passwords must match!" runat="Server" ForeColor="Red" />
                                    <div class="form-group">
                                        <asp:Button ID="BtnUpdate" runat="server" Text="Submit" class="btn btn-primary" OnClick="BtnUpdate_Click" />
                                    </div>
                                </div>

                            </div>


                        </div>
                    </div>
                </div>
            </div>
        </section>


    </div>

    <script>
        $(function () {
            $('#ctl00_ContentPlaceHolder1_pass1').on('keypress', function (e) {
                if (e.which == 32) {
                    console.log('Space Detected');
                    alert("Space Not Allowed.");
                    return false;

                }
            });
        });
    </script>

    <script>
        $(function () {
            $('#ctl00_ContentPlaceHolder1_pass2').on('keypress', function (e) {
                if (e.which == 32) {
                    console.log('Space Detected');
                    alert("Space Not Allowed.");
                    return false;

                }
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            $("#toggle_pwd").click(function () {
                $(this).toggleClass("fa-eye fa-eye-slash");
                var type = $(this).hasClass("fa-eye-slash") ? "text" : "password";
                $(".showeye").attr("type", type);
            });
        });
    </script>

    <script type="text/javascript">
        $(function () {
            $("#toggle_pwd1").click(function () {
                $(this).toggleClass("fa-eye fa-eye-slash");
                var type = $(this).hasClass("fa-eye-slash") ? "text" : "password";
                $(".eye").attr("type", type);
            });
        });
    </script>

</asp:Content>
