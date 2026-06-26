<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Forgot.aspx.cs" Inherits="Forgot" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, shrink-to-fit=no" name="viewport">
    <title><%=Session["Title"].ToString ()%></title>
    <!-- General CSS Files -->
    <link rel="stylesheet" href="assets/css/app.min.css">
    <!-- Template CSS -->
    <link rel="stylesheet" href="assets/css/style.css">
    <link rel="stylesheet" href="assets/css/components.css">
    <!-- Custom style CSS -->
    <link rel="stylesheet" href="assets/css/custom.css">
    <link rel='shortcut icon' type='image/x-icon' href='assets/img/favicon.ico' />
</head>
<body>
    <form id="form1" runat="server">
        <div class="loader"></div>
        <div id="app">
            <section class="section">
                <div class="container mt-5">
                    <div class="row">
                        <div class="col-12 col-sm-8 offset-sm-2 col-md-6 offset-md-3 col-lg-6 offset-lg-3 col-xl-4 offset-xl-4">
                            <div class="card card-primary">
                                <div class="card-header">
                                    <h4>Forgot Password</h4>
                                </div>
                                <div class="card-body">
                                    <p class="text-muted">We will send a link to reset your password</p>
                                    <div>
                                        <div class="form-group">
                                            <label for="email">Member ID</label>
                                            <asp:TextBox ID="txtIDNo" runat="server" class="form-control" MaxLength="15"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequireIDNo" runat="server" ControlToValidate="txtIDNo"
                                                ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                            <%--<input id="email" type="email" class="form-control" name="email" tabindex="1" required autofocus>--%>
                                        </div>
                                        <div class="form-group">
                                            <label for="email">Email</label>
                                            <asp:TextBox ID="TxtMobileNo" runat="server" class="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequireMoblNo" runat="server" ControlToValidate="TxtMobileNo"
                                                ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                            <%--<input id="email" type="email" class="form-control" name="email" tabindex="1" required autofocus>--%>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="Submit" runat="server" Text="Forgot Password" CssClass="btn btn-primary btn-lg btn-block" OnClick="Submit_Click" />
                                            <asp:HiddenField ID="hdnSms" runat="server" />
                                            <%--   <button type="submit" class="btn btn-primary btn-lg btn-block" tabindex="4">
                                                Forgot Password
                                            </button>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </form>
    <!-- General JS Scripts -->
    <script src="assets/js/app.min.js"></script>
    <!-- JS Libraies -->
    <!-- Page Specific JS File -->
    <!-- Template JS File -->
    <script src="assets/js/scripts.js"></script>
    <!-- Custom JS File -->
    <script src="assets/js/custom.js"></script>
    <script defer src="https://static.cloudflareinsights.com/beacon.min.js/vcd15cbe7772f49c399c6a5babf22c1241717689176015" integrity="sha512-ZpsOmlRQV6y907TI0dKBHq9Md29nnaEIPlkf84rnaERnq6zvWvPUqr2ft8M1aS28oN72PdrCzSjY4U6VaAw1EQ==" data-cf-beacon='{"version":"2024.11.0","token":"c04ce8d7704d4bc7b36f94c675da9c6e","r":1,"server_timing":{"name":{"cfCacheStatus":true,"cfEdge":true,"cfExtPri":true,"cfL4":true,"cfOrigin":true,"cfSpeedBrain":true},"location_startswith":null}}' crossorigin="anonymous"></script>
</body>

</html>
