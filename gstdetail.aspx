<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="gstdetail.aspx.cs" Inherits="gstdetail" %>

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
                            <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>GST Detail</h5>
                        </div>
                         <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                        <div class="card-body">
                            <div class="row g-1">
                                <div class="clr">
                                    <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <!-- Genex Business -->
                                    <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
                                        <div class="col-md-8">
                                            <div class="profile-bar-simple red-border clearfix">
                                            </div>
                                            <div class="form-group">
                                                <label for="pwd">
                                                    GST No.:</label>
                                                <asp:TextBox ID="txtpan" runat="server" CssClass="form-control validate[required]" MaxLength="14"></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <label for="email">
                                                    Pdf Upload: <span style="font-size:11px;color:#888;">(only PDF, max 2 MB)</span></label>
                                                <asp:FileUpload ID="Fuidentity" runat="server"
                                                    CssClass="form-control" accept=".pdf"
                                                    onchange="if(validatePdfUpload(this)){previewFile(this,'Gst');}else{removePrev('Gst');}" />
                                                <asp:Label ID="lblimage" runat="server" Visible="false"></asp:Label>

                                                <!-- Upload preview (PDF icon + name + size + view) -->
                                                <div class="kyc-prev" id="prevGst" style="display:none; margin-top:10px; border:1.5px solid #4CAF50; border-radius:10px; padding:10px 12px; background:#f6fff6; align-items:center; gap:12px;">
                                                    <div style="position:relative; flex-shrink:0; cursor:pointer;" onclick="openBig('Gst')">
                                                        <img id="thGst" src="" alt="thumb" style="width:72px; height:72px; object-fit:cover; border-radius:8px; border:1px solid #c8e6c9; display:block;">
                                                        <span style="position:absolute; bottom:3px; right:3px; background:rgba(0,0,0,0.55); color:#fff; font-size:10px; border-radius:4px; padding:1px 5px;">&#128269; view</span>
                                                    </div>
                                                    <div style="flex:1; min-width:0;">
                                                        <span id="nmGst" style="display:block; color:#2e7d32; font-weight:600; font-size:13px; white-space:nowrap; overflow:hidden; text-overflow:ellipsis;"></span>
                                                        <span id="szGst" style="display:block; color:#888; font-size:11px; margin-top:2px;"></span>
                                                    </div>
                                                    <button type="button" onclick="openBig('Gst')" style="background:#4CAF50; color:white; border:none; border-radius:6px; padding:6px 14px; font-size:12px; cursor:pointer;">View PDF</button>
                                                    <button type="button" onclick="removePrev('Gst','<%=Fuidentity.ClientID%>')" style="background:#e53935; color:white; border:none; border-radius:50%; width:26px; height:26px; cursor:pointer; font-size:15px;">&#x2715;</button>
                                                </div>
                                            </div>

                                            <asp:Button ID="BtnIdentity" runat="server" ValidationGroup="eInformation" CssClass="btn btn-primary"
                                                Text="submit" OnClientClick="return Page_ClientValidate('eInformation') && SaveButton();" OnClick="BtnIdentity_Click" />
                                        </div>
                                    </div>
                                    <!-- end dashboards/dashboard -->
                                </div>
                                <div class="col-md-4">
                                    <!-- Genex Business -->
                                    <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="clearfix gen-profile-box">
                                        <div class="col-md-12">
                                            <script src="popupassets/popper.min.js"></script>
                                            <script src="popupassets/lib.js"></script>
                                            <script src="popupassets/jquery.flagstrap.min.js"></script>
                                            <script type="text/javascript" src="popupassets/jquery.themepunch.tools.min.js"></script>
                                            <script type="text/javascript" src="popupassets/jquery.themepunch.revolution.min.js"></script>
                                            <script src="js/functions1.js"></script>

                                            <!-- Saved attachment preview (sirf attachment hone par dikhega) -->
                                            <div id="divAttach" runat="server" visible="false">
                                                <div class="profile-bar-simple red-border clearfix">
                                                    <h6>Attachment</h6>
                                                </div>
                                                <div style="margin-top:10px;">
                                                    <a id="FrontAddress" runat="server" target="_blank" style="display:inline-block; position:relative;">
                                                        <asp:Image ID="ShowIdentity" runat="server" Width="72px" Height="72px" Style="object-fit:cover; border:1px solid #c8e6c9; border-radius:8px;" />
                                                        <span style="position:absolute; bottom:3px; right:3px; background:rgba(0,0,0,0.55); color:#fff; font-size:10px; border-radius:4px; padding:1px 5px;">view</span>
                                                    </a>
                                                    <a id="aViewGst" runat="server" target="_blank" style="margin-left:8px; font-size:12px; vertical-align:top;">View PDF</a>
                                                    <%-- legacy (hidden) --%>
                                                    <a id="PanCard" runat="server" class="fbox" rel="group" style="display:none;">
                                                        <asp:Image ID="Image1" Width="130px" Height="130px" runat="server" Visible="false" />
                                                    </a>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div id="DivVerify" runat="server">
                                                <br />
                                                <asp:Label ID="LblVerification" Text="Verification Status :  " Font-Bold="true" runat="server"></asp:Label>
                                                <asp:Label ID="lblverstatus" runat="server"></asp:Label>
                                                <br />
                                                <asp:Label ID="VerifyDate" runat="server" Text="Verify/Reject Date : " Visible="false"
                                                    Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="Lblverdate" runat="server"></asp:Label>
                                                <br />
                                                <asp:Label ID="LblVerfRemark" Text="Reject Remark : " Visible="false" runat="server"
                                                    Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="LblRemark" runat="server"></asp:Label>
                                                <br />
                                                <asp:Label ID="LblVerfReason" Text="Reject Reason : " Visible="false" runat="server"
                                                    Style="font-weight: bold"></asp:Label>
                                                <asp:Label ID="LbLrejectRemark" runat="server"></asp:Label>
                                            </div>
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

    <!-- Bada preview overlay -->
    <div id="kycModalBg" style="display:none; position:fixed; inset:0; background:rgba(0,0,0,0.72); z-index:99999; align-items:center; justify-content:center; padding:20px;">
        <div style="background:#fff; border-radius:12px; width:100%; max-width:600px; overflow:hidden; border:0.5px solid #ddd;">
            <div style="display:flex; align-items:center; justify-content:space-between; padding:10px 14px; border-bottom:0.5px solid #eee;">
                <span style="font-size:13px; font-weight:600;">Document Preview</span>
                <button type="button" onclick="closeBig()" style="background:none; border:none; cursor:pointer; font-size:18px; color:#888;">&#x2715;</button>
            </div>
            <div style="padding:12px; text-align:center; background:#111;">
                <img id="kycBigImg" src="" alt="Preview" style="max-width:100%; max-height:70vh; border-radius:8px; object-fit:contain; display:block; margin:0 auto;">
            </div>
        </div>
    </div>

    <script type="text/javascript">
        // PDF only validation
        function validatePdfUpload(input) {
            if (input.files.length === 0) return true;
            var file = input.files[0];
            var maxSize = 2 * 1024 * 1024; // 2 MB
            if (file.type !== "application/pdf") {
                alert("Only PDF files are allowed.");
                input.value = ""; return false;
            }
            if (file.size > maxSize) {
                alert("PDF size must not exceed 2 MB.");
                input.value = ""; return false;
            }
            return true;
        }

        // Inline PDF icon (koi external file nahi chahiye)
        var PDF_ICON = "data:image/svg+xml;base64,PHN2ZyB4bWxucz0naHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmcnIHZpZXdCb3g9JzAgMCA2NCA2NCc+PHJlY3QgeD0nMTInIHk9JzQnIHdpZHRoPSczNicgaGVpZ2h0PSc1Nicgcng9JzQnIGZpbGw9JyNmZmZmZmYnIHN0cm9rZT0nI2U1MzkzNScgc3Ryb2tlLXdpZHRoPSczJy8+PHBhdGggZD0nTTQwIDQgbDggOCBoLTggeicgZmlsbD0nI2U1MzkzNScvPjxyZWN0IHg9JzgnIHk9JzM0JyB3aWR0aD0nNDAnIGhlaWdodD0nMTYnIHJ4PScyJyBmaWxsPScjZTUzOTM1Jy8+PHRleHQgeD0nMjgnIHk9JzQ2JyBmb250LWZhbWlseT0nQXJpYWwsSGVsdmV0aWNhLHNhbnMtc2VyaWYnIGZvbnQtc2l6ZT0nMTEnIGZvbnQtd2VpZ2h0PSdib2xkJyBmaWxsPScjZmZmZmZmJyB0ZXh0LWFuY2hvcj0nbWlkZGxlJz5QREY8L3RleHQ+PC9zdmc+";

        // Upload preview
        function previewFile(input, key) {
            var pv = document.getElementById('prev' + key);
            var th = document.getElementById('th' + key);
            var nm = document.getElementById('nm' + key);
            var sz = document.getElementById('sz' + key);
            if (!input.files || !input.files[0]) { if (pv) pv.style.display = 'none'; return; }
            var f = input.files[0];
            var url = URL.createObjectURL(f);
            var isPdf = /\.pdf$/i.test(f.name);
            var kb = f.size / 1024;
            if (th) {
                th.src = isPdf ? PDF_ICON : url;
                th.setAttribute('data-full', isPdf ? '' : url);
                th.setAttribute('data-pdf', isPdf ? url : '');
            }
            if (nm) nm.innerText = f.name;
            if (sz) sz.innerText = kb > 1024 ? (kb / 1024).toFixed(2) + ' MB' : kb.toFixed(1) + ' KB';
            if (pv) pv.style.display = 'flex';
        }
        function openBig(key) {
            var th = document.getElementById('th' + key);
            if (!th) return;
            var pdf = th.getAttribute('data-pdf');
            if (pdf) { window.open(pdf, '_blank'); return; }
            var full = th.getAttribute('data-full') || th.src;
            if (!full) return;
            document.getElementById('kycBigImg').src = full;
            document.getElementById('kycModalBg').style.display = 'flex';
        }
        function closeBig() {
            var m = document.getElementById('kycModalBg'); if (m) m.style.display = 'none';
        }
        function removePrev(key, fileId) {
            if (fileId) { var fu = document.getElementById(fileId); if (fu) fu.value = ''; }
            var pv = document.getElementById('prev' + key); if (pv) pv.style.display = 'none';
            var th = document.getElementById('th' + key); if (th) { th.src = ''; th.removeAttribute('data-full'); th.removeAttribute('data-pdf'); }
            var nm = document.getElementById('nm' + key); if (nm) nm.innerText = '';
            var sz = document.getElementById('sz' + key); if (sz) sz.innerText = '';
        }

        function SaveButton() {
            var gst = document.getElementById('<%=txtpan.ClientID%>').value.trim();
            if (gst === "") { alert("Please enter GST Number"); document.getElementById('<%=txtpan.ClientID%>').focus(); return false; }
            if (gst.length !== 14) { alert("GST Number must be exactly 14 characters."); document.getElementById('<%=txtpan.ClientID%>').focus(); return false; }
            var fu = document.getElementById('<%=Fuidentity.ClientID%>');
            var lbl = document.getElementById('<%=lblimage.ClientID%>');
            var hasOld = lbl && lbl.innerText && lbl.innerText.trim() !== "";
            if (fu && fu.files.length === 0 && !hasOld) { alert("Please upload PDF file."); return false; }
            return true;
        }
    </script>
</asp:Content>
