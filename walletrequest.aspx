<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="walletrequest.aspx.cs" Inherits="walletrequest" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        /* ============== WALLET REQUEST – PROFESSIONAL THEME ============== */
        .wallet-pro {
            --wp-primary: #0d8a5f;
            --wp-primary-dark: #0a6e4b;
            --wp-primary-soft: #e7f5ef;
            --wp-text: #1f2937;
            --wp-muted: #6b7280;
            --wp-border: #e5e9ee;
            --wp-bg: #f7f9f9;
            --wp-danger: #dc2626;
            --wp-radius: 14px;
        }

            .wallet-pro * {
                box-sizing: border-box;
            }

            /* ---- Page header card ---- */
            .wallet-pro .wp-card {
                background: #fff;
                border: 1px solid var(--wp-border);
                border-radius: var(--wp-radius);
                box-shadow: 0 6px 24px rgba(16, 40, 32, .06);
                overflow: hidden;
            }

            .wallet-pro .wp-card-header {
                display: flex;
                align-items: center;
                gap: 10px;
                padding: 18px 24px;
                background: linear-gradient(90deg, var(--wp-primary) 0%, var(--wp-primary-dark) 100%);
                color: #fff;
            }

                .wallet-pro .wp-card-header h5 {
                    margin: 0;
                    font-size: 1.05rem;
                    font-weight: 700;
                    letter-spacing: .2px;
                }

                .wallet-pro .wp-card-header i {
                    font-size: 1.05rem;
                    opacity: .95;
                }

            .wallet-pro .wp-card-body {
                padding: 26px 24px;
            }

            /* ---- Two column shell ---- */
            .wallet-pro .wp-grid {
                display: flex;
                flex-wrap: wrap;
                gap: 22px;
                align-items: flex-start;
            }

            .wallet-pro .wp-col-form {
                flex: 1 1 460px;
                min-width: 300px;
            }

            .wallet-pro .wp-col-side {
                flex: 1 1 280px;
                min-width: 260px;
            }

            /* ---- Form fields ---- */
            .wallet-pro .form-group {
                margin-bottom: 18px;
            }

                .wallet-pro .form-group > label,
                .wallet-pro .wp-label {
                    display: block;
                    font-size: .82rem;
                    font-weight: 600;
                    color: var(--wp-text);
                    margin-bottom: 7px;
                    letter-spacing: .15px;
                }

            .wallet-pro .red {
                color: var(--wp-danger);
                font-size: 1em;
                font-weight: 700;
                padding-left: 3px;
            }

            .wallet-pro .form-control {
                width: 100%;
                height: 46px;
                padding: 10px 14px;
                font-size: .92rem;
                color: var(--wp-text);
                background: #fff;
                border: 1.4px solid var(--wp-border);
                border-radius: 10px;
                transition: border-color .18s ease, box-shadow .18s ease, background .18s ease;
                outline: none;
            }

            .wallet-pro select.form-control {
                cursor: pointer;
            }

            .wallet-pro .form-control:hover {
                border-color: #cfd6dd;
            }

            .wallet-pro .form-control:focus {
                border-color: var(--wp-primary);
                background: #fff;
                box-shadow: 0 0 0 3.5px rgba(13, 138, 95, .14);
            }

            .wallet-pro .form-control::placeholder {
                color: #aab2bb;
            }

            /* ---- Submit button ---- */
            .wallet-pro .btn-primary,
            .wallet-pro .wp-card-body .btn {
                display: inline-flex;
                align-items: center;
                justify-content: center;
                gap: 8px;
                min-width: 150px;
                height: 46px;
                padding: 0 26px;
                font-size: .92rem;
                font-weight: 600;
                color: #fff !important;
                background: var(--wp-primary) !important;
                border: none !important;
                border-radius: 10px;
                cursor: pointer;
                box-shadow: 0 4px 14px rgba(13, 138, 95, .28);
                transition: transform .12s ease, background .18s ease, box-shadow .18s ease;
            }

                .wallet-pro .btn-primary:hover {
                    background: var(--wp-primary-dark) !important;
                    transform: translateY(-1px);
                    box-shadow: 0 6px 18px rgba(13, 138, 95, .34);
                }

                .wallet-pro .btn-primary:active {
                    transform: translateY(0);
                }

            /* ---- Notes box ---- */
            .wallet-pro .wp-note {
                margin-top: 18px;
                padding: 14px 16px;
                background: var(--wp-primary-soft);
                border: 1px solid #cfe9dd;
                border-left: 4px solid var(--wp-primary);
                border-radius: 10px;
                font-size: .82rem;
                line-height: 1.7;
                color: #2f4a40;
            }

                .wallet-pro .wp-note span {
                    color: #2f4a40 !important;
                }

            /* ---- Error message ---- */
            .wallet-pro .error {
                display: block;
                color: var(--wp-danger);
                font-size: .85rem;
                font-weight: 600;
            }

            /* ---- Side info panel (bank / QR) ---- */
            .wallet-pro .wp-side-card {
                background: #fff;
                border: 1px solid var(--wp-border);
                border-radius: var(--wp-radius);
                box-shadow: 0 4px 18px rgba(16, 40, 32, .05);
                overflow: hidden;
            }

            .wallet-pro .wp-side-head {
                padding: 14px 18px;
                background: var(--wp-primary-soft);
                border-bottom: 1px solid #d6ece2;
                color: var(--wp-primary-dark);
                font-weight: 700;
                font-size: .92rem;
                display: flex;
                align-items: center;
                gap: 8px;
            }

                .wallet-pro .wp-side-head h6 {
                    margin: 0;
                    font-size: .92rem;
                    font-weight: 700;
                }

            .wallet-pro .wp-side-body {
                padding: 18px;
            }

                .wallet-pro .wp-side-body img {
                    border-radius: 10px;
                    border: 1px solid var(--wp-border);
                    max-width: 100%;
                }

                /* tidy the default bank-label rows */
                .wallet-pro .wp-side-body span[id^="Label"] {
                    font-size: .88rem;
                    color: var(--wp-text);
                }

        /* ---- Responsive ---- */
        @media (max-width: 575px) {
            .wallet-pro .wp-card-body {
                padding: 20px 16px;
            }

            .wallet-pro .btn-primary {
                width: 100%;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="main-content">
        <section class="section">
            <div class="wallet-pro">
                <ul class="breadcrumb breadcrumb-style"></ul>

                <div class="row">
                    <div class="col-12">
                        <div class="wp-card">
                            <div class="wp-card-header">
                                <i class="fa fa-trophy"></i>
                                <h5>Wallet Request</h5>
                            </div>

                            <div class="wp-card-body">
                                <div class="clr" style="margin-bottom: 6px;">
                                    <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                </div>

                                <div class="wp-grid">
                                    <!-- ============ LEFT : FORM ============ -->
                                    <div class="wp-col-form">
                                        <div id="ctl00_ContentPlaceHolder1_divgenexbusiness" class="gen-profile-box">
                                            <div id="divpay" runat="server">

                                                <!-- Enter Amount -->
                                                <div class="form-group">
                                                    <label for="inputdefault">Enter Amount<span class="red">*</span></label>
                                                    <asp:HiddenField ID="hdnSessn" runat="server" />
                                                    <asp:TextBox runat="server" onkeypress="return isNumberKey(event);" MaxLength="8"
                                                        TabIndex="29" ID="TxtAmount" class="form-control validate[required]"
                                                        placeholder="0.00"></asp:TextBox>
                                                    <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                                </div>

                                                <!-- Paymode -->
                                                <div class="form-group">
                                                    <label for="inputdefault">Select Paymode<span class="red">*</span></label>
                                                    <asp:DropDownList ID="DdlPaymode" runat="server" AutoPostBack="true" CssClass="form-control"
                                                        TabIndex="31" OnSelectedIndexChanged="DdlPaymode_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </div>

                                                <!-- Draft / Cheque / Txn No.  (AJAX duplicate check via onblur — NO postback) -->
                                                <div class="form-group" id="divDDno" runat="server" visible="false">
                                                    <label>
                                                        <asp:Label ID="LblDDNo" runat="server" Text="Draft/CHEQUE No."></asp:Label>
                                                        <span class="red">*</span>
                                                    </label>
                                                    <asp:TextBox ID="TxtDDNo" class="form-control validate[required,custom[onlyLetterNumberChar]]"
                                                        TabIndex="34" runat="server" MaxLength="35"
                                                        onkeypress="return AvoidSpace(event)"
                                                        onblur="checkDuplicateTxn(this);"></asp:TextBox>
                                                    <span id="ddnoMsg" class="error" style="margin-top:4px;"></span>
                                                </div>

                                                <!-- Transaction Date -->
                                                <div class="form-group" id="divDDDate" runat="server">
                                                    <label>
                                                        <asp:Label ID="LblDDDate" runat="server" Text="Transaction Date"></asp:Label>
                                                        <span class="red">*</span>
                                                    </label>
                                                    <asp:TextBox ID="TxtDDDate" runat="server" class="form-control validate[required]"
                                                        TabIndex="37" placeholder="dd-MMM-yyyy"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtDDDate"
                                                        Format="dd-MMM-yyyy"></ajaxToolkit:CalendarExtender>
                                                </div>

                                                <!-- Bank dropdown -->
                                                <div class="form-group" id="DivBank" runat="server">
                                                    <label>Select Your Bank Name <span class="red">*</span></label>
                                                    <asp:DropDownList ID="DDlBank" runat="server" TabIndex="38" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>

                                                <!-- Branch -->
                                                <div class="form-group greybt" id="DivBranch" runat="server">
                                                    <label>Branch Name <span class="red">*</span></label>
                                                    <asp:TextBox ID="TxtIssueBranch" class="form-control validate[required]" TabIndex="39"
                                                        runat="server"></asp:TextBox>
                                                </div>

                                                <!-- ===== Upload copy + preview ===== -->
                                                <div class="form-group" id="divupcopy" runat="server" visible="false">
                                                    <label class="wp-label">Upload Copy: <span class="red">*</span></label>
                                                    <asp:FileUpload runat="server" ID="FlDoc"
                                                        class="form-control validate[required]"
                                                        onchange="previewUploadedFile(this)" />
                                                    <asp:CustomValidator ID="CustomValidator1" OnServerValidate="ValidateFileSize"
                                                        ForeColor="Red" runat="server" ValidationGroup="eInformation" />
                                                    <asp:Label ID="LblImage" runat="server" Visible="false"></asp:Label>

                                                    <!-- Preview small row -->
                                                    <div id="smallPreview" style="display: none; margin-top: 10px; border: 1.5px solid #4CAF50; border-radius: 10px; padding: 10px 12px; background: #f6fff6; flex-direction: row; align-items: center; gap: 12px; width: 100%;">
                                                        <div style="position: relative; flex-shrink: 0; cursor: pointer;" onclick="openModal()">
                                                            <img id="imgThumb" src="" alt="thumb"
                                                                style="width: 72px; height: 72px; object-fit: cover; border-radius: 8px; border: 1px solid #c8e6c9; display: block;">
                                                            <span style="position: absolute; bottom: 3px; right: 3px; background: rgba(0,0,0,0.55); color: #fff; font-size: 10px; border-radius: 4px; padding: 1px 5px;">&#128269; zoom</span>
                                                        </div>
                                                        <div style="flex: 1; min-width: 0;">
                                                            <span id="fileNameText" style="display: block; color: #2e7d32; font-weight: 600; font-size: 13px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;"></span>
                                                            <span id="fileSizeText" style="display: block; color: #888; font-size: 11px; margin-top: 2px;"></span>
                                                            <span style="font-size: 11px; color: #4CAF50; margin-top: 3px; display: block;">&#10004; Click image to view full size</span>
                                                        </div>
                                                        <button type="button" onclick="openModal()"
                                                            style="background: #4CAF50; color: white; border: none; border-radius: 6px; padding: 6px 14px; font-size: 12px; cursor: pointer;">
                                                            View Details</button>
                                                        <button type="button" onclick="removePreview()"
                                                            style="background: #e53935; color: white; border: none; border-radius: 50%; width: 26px; height: 26px; cursor: pointer; font-size: 15px;">
                                                            &#x2715;</button>
                                                    </div>

                                                    <!-- Big modal (inline) -->
                                                    <div id="modalBg" style="display: none; background: rgba(0,0,0,0.72); min-height: 520px; border-radius: 12px; margin-top: 14px; flex-direction: column; align-items: center; padding: 16px; gap: 14px;">
                                                        <div style="background: #fff; border-radius: 12px; width: 100%; max-width: 520px; overflow: hidden; border: 0.5px solid #ddd;">
                                                            <div style="display: flex; align-items: center; justify-content: space-between; padding: 10px 14px; border-bottom: 0.5px solid #eee;">
                                                                <span style="font-size: 13px; font-weight: 600;">Payment Screenshot</span>
                                                                <button type="button" onclick="closeModal()"
                                                                    style="background: none; border: none; cursor: pointer; font-size: 18px; color: #888;">
                                                                    &#x2715;</button>
                                                            </div>
                                                            <div style="padding: 12px; text-align: center; background: #111;">
                                                                <img id="bigImg" src="" alt="Payment"
                                                                    style="max-width: 100%; max-height: 340px; border-radius: 8px; object-fit: contain; display: block; margin: 0 auto;">
                                                            </div>
                                                            <div style="padding: 12px 14px; display: none;">
                                                                <span style="font-size: 11px; font-weight: 600; color: #888; text-transform: uppercase; letter-spacing: 0.06em; display: block; margin-bottom: 10px;">Transaction Details</span>
                                                                <div style="display: flex; justify-content: space-between; align-items: center; padding: 7px 0; border-bottom: 0.5px solid #eee;">
                                                                    <span style="font-size: 12px; color: #888;">Transaction / UTR no.</span>
                                                                    <span style="font-size: 13px; font-weight: 600; font-family: monospace; display: flex; align-items: center; gap: 6px;">
                                                                        <span id="txnText">&#8212;</span>
                                                                        <button type="button" id="copyTxnBtn" onclick="copyTxn()"
                                                                            style="border: 0.5px solid #ccc; background: none; border-radius: 4px; padding: 2px 7px; font-size: 11px; cursor: pointer;">
                                                                            Copy</button>
                                                                    </span>
                                                                </div>
                                                                <div style="margin-top: 12px;">
                                                                    <span style="font-size: 12px; color: #888; display: block; margin-bottom: 6px;">Manual entry
                                                                        <span id="confirmedBadge" style="display: none; background: #e8f5e9; color: #2e7d32; border: 0.5px solid #a5d6a7; border-radius: 20px; font-size: 11px; padding: 2px 10px; margin-left: 6px;">&#10004; Confirmed</span>
                                                                    </span>
                                                                    <div style="display: flex; gap: 8px;">
                                                                        <input type="text" id="manualInput" placeholder="e.g. T2501234567890"
                                                                            style="flex: 1; font-size: 13px; border: 0.5px solid #ccc; border-radius: 6px; padding: 6px 10px; font-family: monospace;">
                                                                        <button type="button" onclick="confirmManual()"
                                                                            style="background: #4CAF50; color: white; border: none; border-radius: 6px; padding: 6px 14px; font-size: 13px; cursor: pointer;">
                                                                            Confirm</button>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <!-- Remarks -->
                                                <div class="form-group greybt" id="divupcopyREm" runat="server" visible="false">
                                                    <label>Remarks <span class="red">*</span></label>
                                                    <asp:TextBox ID="TxtRemarks" class="form-control validate[required]" MaxLength="240"
                                                        TabIndex="41" runat="server"></asp:TextBox>
                                                </div>

                                                <!-- Transaction Password -->
                                                <div class="form-group">
                                                    <label>Enter Transaction Password: <span class="red">*</span></label>
                                                    <asp:TextBox ID="TxtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                                </div>

                                                <!-- Submit (OnClientClick => JS validation) -->
                                                <div class="form-group" style="margin-bottom: 0;">
                                                    <asp:Button ID="cmdSave1" runat="server" Text="Submit" TabIndex="45" class="btn btn-primary"
                                                        ValidationGroup="eInformation"
                                                        OnClientClick="return ValidateWalletRequest();"
                                                        OnClick="cmdSave1_Click" />
                                                </div>

                                                <!-- Notes -->
                                                <div class="wp-note">
                                                    <% if (Session["CompId"] != null && Session["CompId"].ToString() == "1074")
                                                        { %>
                                                    <span id="divnotes" runat="server" visible="false">Note:- 1. Post 5 P.M. payment will get approved on next working day.<br />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;2. There must be a single transaction against a request.</span>
                                                    <div id="divthirdpoint" runat="server" visible="false">
                                                        Note:- Go to Id activation and check your balance - to upgrade for Rs 4000 affiliate or 15000 vendor
                                                    </div>
                                                    <% }
                                                        else
                                                        { %>
                                                    <span>Note:- 1. Post 5 P.M. payment will get approved on next working day.<br />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;2. There must be a single transaction against a request.</span>
                                                    <% } %>
                                                </div>

                                                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="True"
                                                    ShowSummary="False" ValidationGroup="eInformation" />
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TxtDDDate"
                                                    ErrorMessage="Invalid Date" Font-Names="arial" Font-Size="10px" SetFocusOnError="True"
                                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                    ValidationGroup="Form-submit"></asp:RegularExpressionValidator>

                                            </div>
                                        </div>
                                    </div>

                                    <!-- ============ RIGHT : INFO PANELS (dynamic) ============ -->
                                    <div class="wp-col-side">
                                        <% if (Session["compid"] != null && (Session["compid"].ToString() == "1110" || Session["compid"].ToString() == "1108"))
                                            { %>
                                        <div class="wp-side-card" runat="server" id="Div2" visible="false" style="margin-bottom: 18px;">
                                            <div class="wp-side-head">
                                                <i class="fa fa-qrcode"></i>
                                                <h6>Scan the QR code using your UPI apps</h6>
                                            </div>
                                            <div class="wp-side-body" style="text-align: center;">
                                                <img id="imgQr" runat="server" src="" style="width: 70%;" />
                                            </div>
                                        </div>

                                        <div class="wp-side-card" runat="server" id="Div3" visible="false">
                                            <div class="wp-side-head">
                                                <i class="fa fa-university"></i>
                                                <h6>Our Bank Details</h6>
                                            </div>
                                            <div class="wp-side-body">
                                                <div id="Div16" runat="server" style="line-height: 2.05;">
                                                    <asp:Label ID="Label25" Text="A/c Holder Name : " Font-Bold="true" runat="server"></asp:Label>
                                                    <asp:Label ID="Label26" runat="server"></asp:Label>
                                                    <br />
                                                    <asp:Label ID="Label37" Text="Bank Name : " Font-Bold="true" runat="server"></asp:Label>
                                                    <asp:Label ID="Label38" runat="server"></asp:Label>
                                                    <br />
                                                    <asp:Label ID="Label39" Text="A/c NO : " Style="font-weight: bold" runat="server"></asp:Label>
                                                    <asp:Label ID="Label40" runat="server"></asp:Label>
                                                    <br />
                                                    <asp:Label ID="Label41" Text="IFSC CODE : " runat="server" Style="font-weight: bold"></asp:Label>
                                                    <asp:Label ID="Label42" runat="server"></asp:Label>
                                                    <br />
                                                    <asp:Label ID="Label43" Text="Branch : " runat="server" Style="font-weight: bold"></asp:Label>
                                                    <asp:Label ID="Label44" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <% } %>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>

    <script type="text/javascript">
        // duplicate-txn flag (onblur AJAX se set hota hai)
        var __txnDuplicate = false;

        // ============================================================
        //  MAIN VALIDATION  (Submit button OnClientClick se call hota hai)
        //  Jo div visible="false" hai uske control DOM me hote hi nahi,
        //  isliye har field pe  if(el)  null-guard => wo auto-skip.
        // ============================================================
        function ValidateWalletRequest() {
            var el;

            // ---- Amount (always) ----
            el = document.getElementById('<%= TxtAmount.ClientID %>');
            if (el) {
                var amt = el.value.trim();
                if (amt === "" || isNaN(amt) || parseFloat(amt) <= 0) {
                    alert("Please enter valid Amount");
                    el.focus();
                    return false;
                }
            }

            // ---- Paymode ----
            el = document.getElementById('<%= DdlPaymode.ClientID %>');
            if (el && (el.value === "0" || el.selectedIndex <= 0)) {
                alert("Please select Paymode");
                el.focus();
                return false;
            }

            // ---- Draft/Cheque/Txn No. (only when visible) ----
            el = document.getElementById('<%= TxtDDNo.ClientID %>');
            if (el) {
                if (el.value.trim() === "") {
                    alert("Please enter Draft/Cheque/Transaction No.");
                    el.focus();
                    return false;
                }
                if (__txnDuplicate) {
                    alert("This transaction number already exists.");
                    el.focus();
                    return false;
                }
            }

            // ---- Transaction Date (only when visible) ----
            el = document.getElementById('<%= TxtDDDate.ClientID %>');
            if (el && el.value.trim() === "") {
                alert("Please select Transaction Date");
                el.focus();
                return false;
            }

            // ---- Bank dropdown (only when visible) ----
            el = document.getElementById('<%= DDlBank.ClientID %>');
            if (el && (el.value === "0" || el.selectedIndex === -1)) {
                alert("Please select Bank");
                el.focus();
                return false;
            }

            // ---- Branch (only when visible) ----
            el = document.getElementById('<%= TxtIssueBranch.ClientID %>');
            if (el && el.value.trim() === "") {
                alert("Please enter Branch Name");
                el.focus();
                return false;
            }

            // ---- Upload Copy (only when divupcopy visible) ----
            el = document.getElementById('<%= FlDoc.ClientID %>');
            if (el && !el.disabled && el.files.length === 0) {
                alert("Please upload Payment Copy");
                return false;
            }

            // ---- Remarks (only when visible) ----
            el = document.getElementById('<%= TxtRemarks.ClientID %>');
            if (el && el.value.trim() === "") {
                alert("Please enter Remarks");
                el.focus();
                return false;
            }

            // ---- Transaction Password (always) ----
            el = document.getElementById('<%= TxtPassword.ClientID %>');
            if (el && el.value.trim() === "") {
                alert("Please enter Transaction Password");
                el.focus();
                return false;
            }

            // All passed -> allow postback
            return true;
        }

        // ===== AJAX duplicate check (NO postback => image preview safe) =====
        function checkDuplicateTxn(elInput) {
            var ddno = (elInput.value || '').trim();
            var msg = document.getElementById('ddnoMsg');
            if (msg) msg.innerText = '';
            __txnDuplicate = false;
            if (!ddno) return;

            // paymode value
            var pm = '';
            var pmEl = document.getElementById('<%= DdlPaymode.ClientID %>');
        if (pmEl) pm = pmEl.value;

        fetch('walletrequest.aspx/CheckDuplicateTxn', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json; charset=utf-8' },
            body: JSON.stringify({ ddno: ddno, paymode: pm })
        })
            .then(function (r) { return r.json(); })
            .then(function (data) {
                if (data.d === '1') {
                    __txnDuplicate = true;
                    if (msg) msg.innerText = 'This transaction number already exists.';
                    elInput.focus();
                } else {
                    __txnDuplicate = false;
                    if (msg) msg.innerText = '';
                }
            })
            .catch(function () { /* network error - ignore */ });
    }
        // ===== File preview =====
        function previewUploadedFile(input) {
            if (!input.files || !input.files[0]) return;
            var file = input.files[0];
            var kb = file.size / 1024;
            document.getElementById('fileNameText').innerText = file.name;
            document.getElementById('fileSizeText').innerText =
                kb > 1024 ? (kb / 1024).toFixed(2) + ' MB' : kb.toFixed(1) + ' KB';
            var reader = new FileReader();
            reader.onload = function (e) {
                document.getElementById('imgThumb').src = e.target.result;
                document.getElementById('bigImg').src = e.target.result;
            };
            reader.readAsDataURL(file);
            var match = file.name.match(/T\d{10,20}/i) || file.name.match(/\d{12,20}/);
            document.getElementById('txnText').innerText =
                match ? match[0].toUpperCase() : 'Not detected — enter manually';
            document.getElementById('confirmedBadge').style.display = 'none';
            document.getElementById('smallPreview').style.display = 'flex';
        }
        function openModal() {
            var m = document.getElementById('modalBg');
            if (m) m.style.display = 'flex';
        }
        function closeModal() {
            var m = document.getElementById('modalBg');
            if (m) m.style.display = 'none';
        }
        function confirmManual() {
            var el = document.getElementById('manualInput');
            if (!el) return;
            var val = el.value.trim();
            if (!val) return;
            document.getElementById('txnText').innerText = val.toUpperCase();
            document.getElementById('confirmedBadge').style.display = 'inline-block';
            el.value = '';
        }
        function copyTxn() {
            var text = document.getElementById('txnText').innerText;
            navigator.clipboard.writeText(text).then(function () {
                var btn = document.getElementById('copyTxnBtn');
                btn.innerText = 'Copied!'; btn.style.color = '#2e7d32';
                setTimeout(function () { btn.innerText = 'Copy'; btn.style.color = ''; }, 1800);
            });
        }
        function removePreview() {
            var inp = document.getElementById('<%= FlDoc.ClientID %>') || document.querySelector('input[type="file"]');
            if (inp) inp.value = '';
            var sp = document.getElementById('smallPreview'); if (sp) sp.style.display = 'none';
            var mb = document.getElementById('modalBg'); if (mb) mb.style.display = 'none';
            var it = document.getElementById('imgThumb'); if (it) it.src = '';
            var bi = document.getElementById('bigImg'); if (bi) bi.src = '';
            var fn = document.getElementById('fileNameText'); if (fn) fn.innerText = '';
            var fs = document.getElementById('fileSizeText'); if (fs) fs.innerText = '';
            var tx = document.getElementById('txnText'); if (tx) tx.innerText = '—';
        }

        // null-guard: manualInput tabhi exist karega jab divupcopy visible ho
        (function () {
            var mi = document.getElementById('manualInput');
            if (mi) {
                mi.addEventListener('keydown', function (e) {
                    if (e.key === 'Enter') confirmManual();
                });
            }
        })();
    </script>
</asp:Content>
