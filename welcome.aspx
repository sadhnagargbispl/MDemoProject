<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="welcome.aspx.cs" Inherits="welcome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <script>
        function PrintDiv() {
            var printContent = document.getElementById('dvContents').innerHTML;
            var WinPrint = window.open('', '', 'width=900,height=700');
            WinPrint.document.write('<html><head><title>Welcome Letter</title>');
            WinPrint.document.write('<style>');
            WinPrint.document.write(`
                *{box-sizing:border-box;margin:0;padding:0;}
                body{font-family:Segoe UI,Arial,sans-serif;font-size:14px;background:#fff;}
                .wl-card{max-width:700px;margin:0 auto;border:1px solid #ddd;border-radius:12px;overflow:hidden;}
                .wl-header{background:#b8860b;padding:20px 24px;display:flex;align-items:center;gap:16px;}
                .wl-logo-box{width:72px;height:72px;border-radius:10px;background:rgba(255,255,255,0.93);display:flex;align-items:center;justify-content:center;overflow:hidden;flex-shrink:0;}
                .wl-logo-box img{max-height:64px;width:auto;}
                .wl-title{color:#fff;font-size:20px;font-weight:700;margin:0;}
                .wl-sub{color:rgba(255,255,255,0.78);font-size:12px;margin:3px 0 0;}
                .wl-ref{color:rgba(255,255,255,0.6);font-size:11px;margin:5px 0 0;}
                .wl-body{padding:22px 26px;}
                .wl-dear{font-size:14px;font-weight:700;color:#b71c1c;margin-bottom:12px;}
                .wl-para{font-size:13px;color:#444;line-height:1.75;margin:0 0 10px;text-align:justify;}
                .wl-company{font-weight:700;color:#b75555;}
                .wl-divider{height:1px;background:#b8860b;opacity:0.4;margin:16px 0 14px;border:none;}
                .wl-section-label{font-size:10px;font-weight:700;letter-spacing:1.3px;color:#b8860b;text-transform:uppercase;margin:0 0 12px;display:block;}
                .wl-table{width:100%;border-collapse:collapse;}
                .wl-table tr{border-bottom:1px solid #f0f0f0;}
                .wl-table tr:last-child{border-bottom:none;}
                .wl-table td{padding:9px 8px;font-size:13px;vertical-align:middle;}
                .wl-lbl{color:#888;width:42%;font-size:12px;}
                .wl-dot{width:6px;height:6px;border-radius:50%;background:#b8860b;display:inline-block;margin-right:8px;vertical-align:middle;}
                .wl-val{color:#333;font-weight:600;word-break:break-all;}
                .wl-val.accent{color:#b71c1c;}
                .s-label{display:block;font-size:11px;color:#999;}
                .s-role{display:block;font-size:13px;font-weight:600;color:#333;}
                .s-company{display:block;font-size:14px;font-weight:700;color:#b8860b;}
                .s-contact{font-size:12px;color:#666;}
                .wl-sign{padding:4px 26px 20px;}
                .wl-footer{background:#fafaf7;padding:12px 26px;border-top:1px solid #eee;display:flex;align-items:center;justify-content:space-between;}
                .wl-footer-left{font-size:11px;color:#aaa;}
                .wl-badge{font-size:10px;font-weight:700;background:#fef3c7;color:#92400e;border-radius:20px;padding:3px 12px;border:1px solid #d97706;}
                .noprint{display:none !important;}
            `);
            WinPrint.document.write('</style></head><body>');
            WinPrint.document.write(printContent);
            WinPrint.document.write('</body></html>');
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
            return false;
        }
    </script>

    <style>
        * { box-sizing: border-box; }
        body { font-family: 'Segoe UI', Arial, sans-serif; background-color: #f0f2f8; color: #333; }

        /* CARD */
        .wl-card { background: #fff; border: 1px solid #e0e0e0; border-radius: 16px; max-width: 740px; margin: 24px auto; overflow: hidden; box-shadow: 0 4px 24px rgba(0,0,0,0.09); }

        /* HEADER */
        .wl-header { background: linear-gradient(135deg, #7B5A1E 0%, #b8860b 55%, #d4a843 100%); padding: 22px 28px; display: flex; align-items: center; gap: 18px; }
        .wl-logo-box { width: 80px; height: 80px; border-radius: 12px; background: rgba(255,255,255,0.95); display: flex; align-items: center; justify-content: center; border: 2px solid rgba(255,255,255,0.5); flex-shrink: 0; overflow: hidden; }
        .wl-logo-box img { max-height: 70px; max-width: 100%; width: auto; object-fit: contain; }
        .wl-header-text { flex: 1; min-width: 0; }
        .wl-title { color: #fff; font-size: 22px; font-weight: 700; margin: 0; }
        .wl-sub { color: rgba(255,255,255,0.78); font-size: 13px; margin: 4px 0 0; }
        .wl-ref { color: rgba(255,255,255,0.6); font-size: 11px; margin: 6px 0 0; }

        /* BODY */
        .wl-body { padding: 26px 30px; }
        .wl-dear { font-size: 14px; font-weight: 700; color: #b71c1c; margin: 0 0 14px; }
        .wl-para { font-size: 14px; color: #444; line-height: 1.8; margin: 0 0 11px; text-align: justify; }
        .wl-company { font-weight: 700; color: #b75555; }
        .wl-divider { height: 1px; background: linear-gradient(90deg, transparent, #b8860b, transparent); margin: 20px 0 16px; border: none; opacity: 0.5; }
        .wl-section-label { font-size: 11px; font-weight: 700; letter-spacing: 1.4px; color: #b8860b; text-transform: uppercase; margin: 0 0 13px; display: block; }

        /* TABLE */
        .wl-table { width: 100%; border-collapse: collapse; }
        .wl-table tr { border-bottom: 1px solid #f2f2f2; }
        .wl-table tr:last-child { border-bottom: none; }
        .wl-table tr:hover { background: #fffbf0; }
        .wl-table td { padding: 11px 8px; font-size: 14px; vertical-align: middle; }
        .wl-lbl { color: #888; width: 44%; font-size: 13px; }
        .wl-dot { width: 7px; height: 7px; border-radius: 50%; background: #b8860b; display: inline-block; margin-right: 8px; vertical-align: middle; }
        .wl-val { color: #333; font-weight: 600; word-break: break-all; }
        .wl-val.accent { color: #b71c1c; }

        /* SIGNATURE */
        .wl-sign { padding: 6px 30px 22px; }
        .s-label { display: block; font-size: 12px; color: #aaa; margin-bottom: 4px; }
        .s-role { display: block; font-size: 14px; font-weight: 600; color: #333; }
        .s-company { display: block; font-size: 15px; font-weight: 700; color: #b8860b; }
        .s-contact { font-size: 12px; color: #666; margin-top: 4px; display: block; }

        /* FOOTER */
        .wl-footer { background: #fafaf7; padding: 13px 30px; border-top: 1px solid #eeece0; display: flex; align-items: center; justify-content: space-between; flex-wrap: wrap; gap: 8px; }
        .wl-footer-left { font-size: 12px; color: #aaa; }
        .wl-badge { font-size: 11px; font-weight: 700; background: #fef3c7; color: #92400e; border-radius: 20px; padding: 4px 14px; border: 1px solid #d97706; white-space: nowrap; }

        /* BUTTONS */
        .btn-area { margin-bottom: 14px; display: flex; flex-wrap: wrap; gap: 6px; }
        .btn-area .btn { margin: 0; }

        /* PRINT */
        @media print {
            .noprint { display: none !important; visibility: hidden !important; }
            body { background: #fff !important; margin: 0; }
            .wl-card { box-shadow: none !important; border: 1px solid #ccc; margin: 0; border-radius: 8px; }
        }

        /* ---- MOBILE: up to 480px ---- */
        @media (max-width: 480px) {
            .wl-card { margin: 8px 4px; border-radius: 12px; }
            .wl-header { flex-direction: column; align-items: center; text-align: center; padding: 16px 14px; gap: 10px; }
            .wl-logo-box { width: 64px; height: 64px; border-radius: 9px; }
            .wl-logo-box img { max-height: 56px; }
            .wl-title { font-size: 16px; }
            .wl-sub { font-size: 11px; }
            .wl-ref { font-size: 10px; }
            .wl-body { padding: 14px 12px; }
            .wl-dear { font-size: 12px; margin-bottom: 9px; }
            .wl-para { font-size: 12px; line-height: 1.7; }
            .wl-section-label { font-size: 9px; }
            .wl-table td { padding: 8px 5px; font-size: 12px; }
            .wl-lbl { font-size: 11px; width: 46%; }
            .wl-dot { width: 6px; height: 6px; margin-right: 5px; }
            .wl-sign { padding: 4px 12px 16px; }
            .s-role { font-size: 12px; }
            .s-company { font-size: 13px; }
            .s-contact { font-size: 10px; }
            .wl-footer { flex-direction: column; align-items: center; text-align: center; padding: 10px 12px; gap: 6px; }
            .wl-footer-left { font-size: 10px; }
            .wl-badge { font-size: 10px; padding: 3px 10px; }
            .btn-area .btn { flex: 1 1 44%; font-size: 12px; padding: 6px 8px; }
        }

        /* ---- TABLET: 481px to 767px ---- */
        @media (min-width: 481px) and (max-width: 767px) {
            .wl-card { margin: 14px 8px; border-radius: 13px; }
            .wl-header { padding: 18px 20px; gap: 14px; }
            .wl-logo-box { width: 70px; height: 70px; }
            .wl-logo-box img { max-height: 60px; }
            .wl-title { font-size: 18px; }
            .wl-body { padding: 20px 18px; }
            .wl-para { font-size: 13px; }
            .wl-table td { padding: 9px 6px; font-size: 13px; }
            .wl-lbl { font-size: 12px; }
            .wl-sign { padding: 4px 18px 18px; }
            .wl-footer { padding: 11px 18px; }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="main-content">
        <section class="section">
            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-header">
                            <h5 class="mb-0"><i class="fa fa-trophy me-2"></i> Welcome</h5>
                        </div>
                        <div class="card-body">

                            <%-- ACTION BUTTONS --%>
                            <div class="btn-area noprint">
                                <asp:Label ID="errMsg" runat="server" CssClass="error" style="width:100%;"></asp:Label>
                                <asp:Button ID="BtnHome"    runat="server" Text="Home"               CssClass="btn btn-info    btn-sm" OnClick="BtnHome_Click" />
                                <button type="button" class="btn btn-warning btn-sm" id="BtnPrint"   runat="server" onclick="javascript:PrintDiv();">Print</button>
                                <asp:Button ID="BtnNewJoin" runat="server" Text="New Joining"        CssClass="btn btn-danger  btn-sm" OnClick="BtnNewJoin_Click" />
                                <button type="button" class="btn btn-danger btn-sm" id="btnActive"   runat="server">Purchase Product</button>
                                <button type="button" class="btn btn-danger btn-sm" id="btnPackage"  runat="server">Purchase Package</button>
                                <button type="button" class="btn btn-danger btn-sm" id="btnShopping" runat="server">Continue To Shopping</button>
                                <asp:HiddenField ID="hdnFormNo" runat="server" />
                            </div>

                            <%-- WELCOME CARD --%>
                            <div class="wl-card" id="dvContents">

                                <%-- HEADER --%>
                                <div class="wl-header">
                                    <div class="wl-logo-box">
                                        <img src="<%= Session["Logo"] %>" alt="Logo" onerror="this.style.display='none'" />
                                    </div>
                                    <div class="wl-header-text">
                                        <p class="wl-title">Welcome Letter</p>
                                        <p class="wl-sub">Enrollment Confirmation</p>
                                        <p class="wl-ref">
                                            Letter No: EMC /
                                            <asp:Label ID="LblId" runat="server"></asp:Label> /
                                            <asp:Label ID="LblYear" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                </div>

                                <%-- BODY --%>
                                <div class="wl-body">

                                    <p class="wl-dear">
                                        <% if (Session["CompID"].ToString() == "1078" || Session["CompID"].ToString() == "1093") { %>
                                            Dear <%= Session["Membername"] %>,
                                        <% } else { %>
                                            Dear Clients / Participants &amp; Families,
                                        <% } %>
                                    </p>

                                    <div id="divWelcomeAll" runat="server" visible="false">
                                        <p class="wl-para">We are <span class="wl-company"><%= Session["CompName"] %></span>, pleased to welcome you as our new client and take this opportunity to extend our warm greetings to you.</p>
                                        <p class="wl-para">We assure you that you will find it enjoyable and professionally beneficial to avail our services or to associate with us.</p>
                                        <p class="wl-para">We, at <span class="wl-company"><%= Session["CompName"] %></span>, respect the concern of our clients and associates and observe the highest degree of corporate ethics. We sincerely hope and believe that our services will exceed your expectations and add your name to our long list of satisfied clients and associates.</p>
                                        <p class="wl-para">We are confident that our customized packages will win your trust and appreciation. Our representatives are always available 24×7 with dedication to address all your concerns. We look forward to serving you for a long time.</p>
                                    </div>

                                    <div id="divWelcomeAction" runat="server" visible="false">
                                        <p class="wl-para">We are <span class="wl-company"><%= Session["CompName"] %></span>, pleased to welcome you as our new client and take this opportunity to extend our warm greetings to you.</p>
                                        <p class="wl-para">We assure you that you will find it enjoyable and professionally beneficial to avail our services or to associate with us.</p>
                                        <p class="wl-para">We, at <span class="wl-company"><%= Session["CompName"] %></span>, respect the concern of our clients and associates and observe the highest degree of corporate ethics. We sincerely hope and believe that our services will exceed your expectations and add your name to our long list of satisfied clients and associates.</p>
                                        <p class="wl-para">We are confident that our customized packages will win your trust and appreciation. Our representatives are always available 24×7 with dedication to address all your concerns. We look forward to serving you for a long time.</p>
                                    </div>

                                    <hr class="wl-divider" />
                                    <span class="wl-section-label">Enrollment Details</span>

                                    <table class="wl-table">
                                        <tr>
                                            <td class="wl-lbl"><span class="wl-dot"></span>ID No.</td>
                                            <td class="wl-val accent"><asp:Label ID="LblIdno" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="wl-lbl"><span class="wl-dot"></span>Name</td>
                                            <td class="wl-val accent"><asp:Label ID="LblName" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="wl-lbl"><span class="wl-dot"></span>Mobile No.</td>
                                            <td class="wl-val"><asp:Label ID="LblMobl" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="wl-lbl"><span class="wl-dot"></span>Email ID</td>
                                            <td class="wl-val"><asp:Label ID="LblEmail" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="wl-lbl"><span class="wl-dot"></span>Password</td>
                                            <td class="wl-val accent"><asp:Label ID="LblPassw" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="wl-lbl">
                                                <span class="wl-dot"></span>
                                                <asp:Label ID="lblEpasswl" runat="server" Text="T.Password"></asp:Label>
                                            </td>
                                            <td class="wl-val accent"><asp:Label ID="LblEPassw" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>

                                </div>

                                <%-- SIGNATURE --%>
                                <div class="wl-sign">
                                    <% if (Session["CompID"].ToString() == "1059") { %>
                                        <div id="signAll" runat="server" visible="false">
                                            <span class="s-label">Warm Regards,</span>
                                            <span class="s-role">CMD — Mr. Manish Maheshwari</span>
                                            <span class="s-company"><%= Session["CompName"] %></span>
                                        </div>
                                    <% } else if (Session["CompID"].ToString() == "1078") { %>
                                        <div id="signmanjar" runat="server" visible="false">
                                            <span class="s-label">Regards,</span>
                                            <span class="s-company"><%= Session["CompName"] %></span>
                                            <span class="s-contact">Email: manjardigitalpvtLtd@gmail.com &nbsp;|&nbsp; Mob: 8174041066</span>
                                        </div>
                                    <% } else { %>
                                        <div id="P1" runat="server" visible="false">
                                            <span class="s-label">Warm Regards,</span>
                                            <span class="s-role"><%= (Session["CompID"].ToString() == "1066") ? "CEO" : "CMD" %></span>
                                            <span class="s-company"><%= Session["CompName"] %></span>
                                        </div>
                                    <% } %>
                                    <div id="signUcm" runat="server" visible="false">
                                        <span class="s-label">Warm Regards,</span>
                                        <span class="s-role"><%= (Session["CompID"].ToString() == "1066") ? "CEO" : "CMD" %></span>
                                        <span class="s-company"><%= Session["CompName"] %></span>
                                    </div>
                                </div>

                                <%-- FOOTER --%>
                                <div class="wl-footer">
                                    <span class="wl-footer-left">&copy; <strong><%= Session["CompName"] %></strong> &nbsp;|&nbsp; Available 24×7</span>
                                    <span class="wl-badge">Active Member</span>
                                </div>

                                <%-- Social Icons for CompID 1024 --%>
                                <% if (Session["CompID"] != null && Session["CompID"].ToString() == "1024") { %>
                                <div style="padding: 12px 20px; text-align: center;" class="noprint">
                                    <a href="https://bstarm.in/" target="_blank" style="margin:0 8px;color:#555;"><i class="fa fa-globe" style="font-size:20px;"></i></a>
                                    <a href="https://www.instagram.com/invites/contact/?i=7m42z2fk8jxo&utm_content=wfpf9z" target="_blank" style="margin:0 8px;color:#e1306c;"><i class="fa fa-instagram" style="font-size:20px;"></i></a>
                                    <a href="https://www.facebook.com/myonlineshoppy1" target="_blank" style="margin:0 8px;color:#3b5998;"><i class="fa fa-facebook-square" style="font-size:20px;"></i></a>
                                    <a href="https://www.linkedin.com/in/myonline-shoppy-910692123" target="_blank" style="margin:0 8px;color:#0A66C2;"><i class="fa fa-linkedin-square" style="font-size:20px;"></i></a>
                                    <a href="https://twitter.com/MyOnlineShoppy1" target="_blank" style="margin:0 8px;color:#00acee;"><i class="fa fa-twitter-square" style="font-size:20px;"></i></a>
                                </div>
                                <% } %>

                            </div><%-- /dvContents --%>

                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
