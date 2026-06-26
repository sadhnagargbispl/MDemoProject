<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="ChangePass.aspx.cs" Inherits="ChangePass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        language = "javascript" >
            function isNumberKey(evt) {
                //    if (!(((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)) || (event.keyCode == 8) || (event.keyCode == 9) || (event.keyCode == 37) || (event.keyCode == 39) || (event.keyCode == 46) || (event.keyCode == 190) || (event.keyCode == 35) || (event.keyCode == 36)))
                //event.returnValue=false;

                var charCode = (evt.which) ? evt.which : event.keyCode
                if (charCode > 31 && charCode < 48 || charCode > 57)
                    if (event.keyCode != 46)
                        if (event.keyCode != 110)

                            return false;

                return true;



            }


    </script>


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
                            <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>Change Password</h5>
                        </div>
                        <div class="card-body">
                            <div class="row g-1">
                                <div class="clr">
                                    <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label for="inputdefault">
                                            Old Password<span class="red">*</span></label>
                                        <asp:HiddenField ID="hdnSessn" runat="server" />
                                        <asp:TextBox ID="oldpass" class="validate[required] form-control" TextMode="Password"
                                            runat="server"></asp:TextBox>
                                        <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="oldpass"
                                                    runat="server">Old Password can't left blank</asp:RequiredFieldValidator>--%>
                                    </div>
                                    <div class="form-group">
                                        <label for="inputdefault">
                                            New Password<span class="red">*</span>
                                        </label>

                                        <asp:TextBox ID="pass1" TextMode="Password" runat="server" class="validate[required,minSize[5],maxSize[10]] form-control showeye"
                                            onkeypress="return isNumberKey(event)">
                                                    
                                        </asp:TextBox>

                                    </div>
                                    <i class="fa fa-eye fa-eye field_icon" aria-hidden="true" style="transform: translateY(-371%);float: right;padding-right: 10px;" id="toggle_pwd"></i>
                                    <div class="form-group">
                                        <label for="inputdefault">
                                            Confirm Password<span class="red">*</span></label>
                                        <asp:TextBox ID="pass2" class="validate[required,minSize[5],maxSize[10]] form-control  eye"
                                            TextMode="Password" runat="server"></asp:TextBox>
                                        <%-- <span id="toggle_pwd1" class="fa fa-eye field_icon" style="transform: translateY(-155%); margin-left: 43em"></span>--%>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="Dynamic" ControlToValidate="pass1"
                                            runat="server" ErrorMessage="RequiredFieldValidator">confirm New Password can't left blank</asp:RequiredFieldValidator>

                                    </div>
                                    <i class="fa fa-eye fa-eye field_icon" aria-hidden="true" style="transform: translateY(-371%);float: right;padding-right: 10px;" id="toggle_pwd1"></i>
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
                debugger;
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
                debugger;
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
                debugger;
                $(this).toggleClass("fa-eye fa-eye-slash");
                var type = $(this).hasClass("fa-eye-slash") ? "text" : "password";
                $(".showeye").attr("type", type);
            });
        });
    </script>

    <script type="text/javascript">
        $(function () {
            $("#toggle_pwd1").click(function () {
                debugger;
                $(this).toggleClass("fa-eye fa-eye-slash");
                var type = $(this).hasClass("fa-eye-slash") ? "text" : "password";
                $(".eye").attr("type", type);
            });
        });
    </script>

</asp:Content>

