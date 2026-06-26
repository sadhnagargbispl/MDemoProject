<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">
    <meta charset="UTF-8">
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, shrink-to-fit=no" name="viewport">
    <title><%=Session["Title"].ToString ()%></title>
    <link rel="stylesheet" href="assets/css/app.min.css">
    <link rel="stylesheet" href="assets/bundles/bootstrap-social/bootstrap-social.css">
    <link rel="stylesheet" href="assets/css/style.css">
    <link rel="stylesheet" href="assets/css/components.css">
    <script type="text/javascript" src="highslide/highslide-full.js"></script>

    <link rel="stylesheet" type="text/css" href="highslide/highslide.css" />

    <script type="text/javascript">
        hs.graphicsDir = 'highslide/graphics/';
        hs.align = 'center';
        hs.transitions = ['expand', 'crossfade'];
        hs.fadeInOut = true;
        hs.dimmingOpacity = 0.8;
        hs.outlineType = 'rounded-white';
        hs.marginTop = 60;
        hs.marginBottom = 40;
        hs.numberPosition = '';
        hs.wrapperClassName = 'custom';
        hs.width = 600;
        hs.height = 500;
        hs.number = 'Page %1 of %2';
        hs.captionOverlay.fade = 0;

        // Add the slideshow providing the controlbar and the thumbstrip

    </script>
</head>
<body class="background-image-body">
    <div class="loader"></div>
    <form id="form1" runat="server" class="needs-validation">
        <div>

            <div id="app">
                <section class="section">
                    <div class="container mt-5">
                        <div class="row">
                            <div class="col-12 col-sm-8 offset-sm-2 col-md-6 offset-md-3 col-lg-6 offset-lg-3 col-xl-4 offset-xl-4">
                                <div class="card card-auth">
                                    <div class="login-brand login-brand-color">
                                        <% if (Session["CompID"] != null && Session["CompID"].ToString() == "1069")
                                            { %>
                                        <img src="images/logo.png" width="180"
                                            runat="server" id="imgLogoZienva" />
                                        <% }
                                            else
                                            { %>
                                        <img src="" width="180" runat="server" id="imgLogo" />
                                        <% } %>

                                        <%-- <img alt="image" src="<%=Session["LogoUrl"].ToString ()%>" width="180" />--%>
                                    </div>
                                    <div class="card-header card-header-auth">
                                        <h4>Login Account</h4>
                                    </div>
                                    <div class="card-body">
                                        <div>
                                            <div class="form-group">
                                                <label for="email">User ID</label>
                                                <input class="form-control" type="text" runat="server" id="Txtuid" name="uid" placeholder="Distributor/Customer ID" tabindex="1" autofocus>
                                                <div class="invalid-feedback">
                                                    Please fill in Customer ID
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="d-block">
                                                    <label for="password" class="control-label">Password</label>
                                                    <div class="float-right">
                                                        <a class="text-small" href="Forgot.aspx"
                                                            onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 525,height: 280,marginTop : 0 } )"
                                                            style="color: Blue; cursor: pointer;">Forgot Password?
                                                        </a>
                                                        <%--     <a href="forgot.aspx" class="text-small">Forgot Password?
                                                        </a>--%>
                                                    </div>
                                                </div>
                                                <input class="form-control showeye" type="password" runat="server" id="Txtpwd" name="pwd" placeholder="Password" tabindex="2">
                                                <i class="fa fa-eye fa-eye field_icon" aria-hidden="true" style="transform: translateY(-190%); float: right; padding-right: 10px;" id="toggle_pwd"></i>
                                                <div class="invalid-feedback">
                                                    please fill in your password
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="custom-control custom-checkbox">
                                                    <input type="checkbox" name="remember" class="custom-control-input" tabindex="3" id="remember-me">
                                                    <label class="custom-control-label" for="remember-me">Remember Me</label>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Button ID="BtnSubmit" runat="server" class="btn btn-lg btn-primary w-100" Text="Login" TabIndex="4" OnClick="BtnSubmit_Click" />
                                                <%--        <button type="submit" class="btn btn-lg btn-primary w-100" tabindex="4">
                                                    Login
                                                </button>--%>
                                            </div>
                                        </div>
                                        <div class="text-center mt-4 mb-3">
                                            <div class="text-job text-muted"></div>
                                        </div>
                                        <!-- Default.aspx mein sirf yeh change karo -->

                                       <div class="row no-gutters justify-content-center">
    <div class="col-6 px-1" id="Regdiv" runat="server" visible="false">
        <a class="btn btn-block w-100 text-center"
            runat="server" id="aJoining"
            style="background: #23345a; color: #fff; border-radius: 6px; border: none; padding: 10px 16px; font-weight: 500;">
            <i class="fa fa-user-plus" aria-hidden="true" style="margin-right: 5px;"></i>Registration
        </a>
    </div>
    <div class="col-6 px-1">
        <a class="btn btn-block w-100 text-center"
            runat="server" id="ahome"
            style="background: #55acee; color: #fff; border-radius: 6px; border: none; padding: 10px 16px; font-weight: 500;">
            <i class="fa fa-globe" aria-hidden="true" style="margin-right: 5px;"></i>Go To Website
        </a>
    </div>
</div>
                                        <%-- <div class="row sm-gutters justify-content-center">
                                            <div class="col-6" id="Regdiv" runat="server" visible="false">
                                                <a class="btn btn-block btn-social btn-facebook w-100 text-center" runat="server"
                                                    id="aJoining">
                                                
                                                    Registration
                                                </a>
                                            </div>
                                            <div class="col-6">
                                                <a class="btn btn-block btn-social btn-twitter w-100 text-center" runat="server" id="ahome">
                                             
                                                    Go To Website
                                                </a>
                                            </div>
                                        </div>--%>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </section>
            </div>
        </div>
        <div class="modal fade" id="incomeModal" tabindex="-1">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">

                    <%--                    <div class="modal-header">
                        <h5 class="modal-title">Forgot Password</h5>
                        <button type="button"
                            class="btn-close"
                            data-bs-dismiss="modal">
                        </button>
                    </div>--%>

                    <div class="modal-body p-0">
                        <iframe id="incomeFrame"
                            style="width: 100%; height: 400px; border: none;"></iframe>
                    </div>

                </div>
            </div>
        </div>
    </form>

</body>
<script src="assets/js/app.min.js"></script>
<!-- JS Libraies -->
<!-- Page Specific JS File -->
<!-- Template JS File -->
<script src="assets/js/scripts.js"></script>
<script src="disable.js"></script>
<script type="text/javascript">
    $(function () {
        $("#toggle_pwd").click(function () {
            $(this).toggleClass("fa-eye fa-eye-slash");
            var type = $(this).hasClass("fa-eye-slash") ? "text" : "password";
            $(".showeye").attr("type", type);
        });
    });
</script>
<script>
    function openIncomeModal(page) {
        const frame = document.getElementById("incomeFrame");
        const modalEl = document.getElementById("incomeModal");
        if (!frame || !modalEl) {
            console.error("Modal or iframe not found");
            return;
        }
        const url = page;
        frame.src = "about:blank";
        frame.src = url;
        const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
        modal.show();
    }
</script>
</html>
