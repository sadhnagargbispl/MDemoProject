<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UploadForm.aspx.cs" Inherits="UploadForm" ResponseEncoding="utf-8" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="hi">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>KYC - Direct Seller PDF Upload</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <style>
        *, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }

        body {
            font-family: 'Segoe UI', Arial, sans-serif;
            background: #f4f6fb;
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 2rem 1rem;
        }

        .page-wrap {
            width: 100%;
            max-width: 560px;
        }

        /* ── Step Tracker ── */
        .step-track {
            display: flex;
            align-items: center;
            margin-bottom: 1.75rem;
        }
        .step-item {
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 5px;
        }
        .step-circle {
            width: 34px; height: 34px;
            border-radius: 50%;
            display: flex; align-items: center; justify-content: center;
            font-size: 13px; font-weight: 600;
        }
        .step-circle.done   { background: #4f46e5; color: #fff; }
        .step-circle.active { background: #fff; border: 2px solid #4f46e5; color: #4f46e5; }
        .step-circle.future { background: #e5e7eb; color: #9ca3af; }
        .step-label { font-size: 10px; font-weight: 500; color: #6b7280; }
        .step-label.active  { color: #4f46e5; }
        .step-line { flex: 1; height: 2px; background: #e5e7eb; margin: 0 4px; margin-bottom: 18px; }
        .step-line.done { background: #4f46e5; }

        /* ── Card ── */
        .card {
            background: #fff;
            border-radius: 16px;
            border: 1px solid #e5e7eb;
            overflow: hidden;
        }

        .card-header {
            padding: 1.25rem 1.5rem;
            background: #fafafa;
            border-bottom: 1px solid #f0f0f0;
            display: flex;
            align-items: center;
            gap: 10px;
        }
        .card-header-icon {
            width: 34px; height: 34px;
            background: #ede9fe;
            border-radius: 8px;
            display: flex; align-items: center; justify-content: center;
        }
        .card-header-icon svg { width: 17px; height: 17px; fill: #4f46e5; }
        .card-title { font-size: 15px; font-weight: 600; color: #111827; }
        .step-pill {
            margin-left: auto;
            font-size: 11px; font-weight: 600;
            background: #ede9fe; color: #4f46e5;
            padding: 3px 10px;
            border-radius: 20px;
        }

        .card-body { padding: 1.5rem; }

        /* ── Download Box ── */
        .download-box {
            background: #f8f7ff;
            border: 1px solid #ddd8fc;
            border-radius: 10px;
            padding: 1rem 1.1rem;
            display: flex;
            align-items: flex-start;
            gap: 12px;
            margin-bottom: 1.25rem;
        }
        .dl-icon {
            width: 38px; height: 38px; flex-shrink: 0;
            background: #ede9fe;
            border-radius: 8px;
            display: flex; align-items: center; justify-content: center;
        }
        .dl-icon svg { width: 18px; height: 18px; fill: #4f46e5; }
        .dl-content { flex: 1; }
        .dl-title { font-size: 13px; font-weight: 600; color: #1f2937; margin-bottom: 3px; }
        .dl-hindi {
            font-family: 'Mangal', 'Nirmala UI', 'Arial Unicode MS', 'Segoe UI', sans-serif;
            font-size: 13px; color: #6b7280; line-height: 1.8;
        }
        .dl-warning {
            font-size: 11px; color: #dc2626;
            margin-top: 5px;
            display: flex; align-items: center; gap: 4px;
            font-family: 'Mangal', 'Nirmala UI', 'Arial Unicode MS', sans-serif;
        }
        .dl-warning svg { width: 12px; height: 12px; fill: #dc2626; flex-shrink: 0; }
        .btn-dl {
            display: inline-flex; align-items: center; gap: 5px;
            margin-top: 10px;
            padding: 6px 14px;
            background: #4f46e5;
            color: #fff;
            font-size: 12px; font-weight: 500;
            border-radius: 7px;
            text-decoration: none;
            transition: background 0.15s;
        }
        .btn-dl:hover { background: #4338ca; }
        .btn-dl svg { width: 13px; height: 13px; fill: #fff; }

        /* ── Reject Alert ── */
        .reject-alert {
            background: #fff5f5;
            border: 1px solid #fecaca;
            border-radius: 10px;
            padding: 1rem 1.1rem;
            margin-bottom: 1.25rem;
        }
        .reject-alert-title {
            display: flex; align-items: center; gap: 7px;
            font-size: 13px; font-weight: 600; color: #991b1b;
            margin-bottom: 8px;
        }
        .reject-alert-title svg { width: 15px; height: 15px; fill: #dc2626; }
        .reject-row { font-size: 12.5px; color: #374151; margin-bottom: 4px; }
        .reject-row span { font-weight: 600; color: #991b1b; }
        .reject-value { color: #4b5563; }

        /* ── Upload Zone ── */
        .upload-zone {
            border: 2px dashed #d1d5db;
            border-radius: 10px;
            padding: 1.75rem 1rem;
            text-align: center;
            cursor: pointer;
            transition: border-color 0.2s, background 0.2s;
            position: relative;
            margin-bottom: 1rem;
        }
        .upload-zone:hover, .upload-zone.drag-active {
            border-color: #4f46e5;
            background: #f8f7ff;
        }
        .upload-zone input[type="file"] {
            position: absolute; inset: 0;
            width: 100%; height: 100%;
            opacity: 0; cursor: pointer;
        }
        .upload-zone-icon { margin-bottom: 8px; }
        .upload-zone-icon svg { width: 38px; height: 38px; fill: #9ca3af; }
        .upload-zone.drag-active .upload-zone-icon svg { fill: #4f46e5; }
        .upload-zone-text { font-size: 13px; color: #6b7280; font-family: 'Mangal', 'Nirmala UI', 'Arial Unicode MS', sans-serif; }
        .upload-zone-text b { color: #4f46e5; font-weight: 600; }
        .upload-zone-sub { font-size: 11px; color: #9ca3af; margin-top: 3px; }

        /* ── File Selected Preview ── */
        .file-preview {
            display: none;
            align-items: center;
            gap: 10px;
            background: #f0fdf4;
            border: 1px solid #bbf7d0;
            border-radius: 8px;
            padding: 10px 14px;
            margin-bottom: 1rem;
        }
        .file-preview.show { display: flex; }
        .file-preview svg { width: 20px; height: 20px; fill: #16a34a; flex-shrink: 0; }
        .file-preview-name { font-size: 13px; font-weight: 500; color: #15803d; flex: 1; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
        .file-preview-size { font-size: 11px; color: #6b7280; white-space: nowrap; }
        .file-preview-remove {
            background: none; border: none; cursor: pointer;
            color: #9ca3af; font-size: 16px; padding: 0 2px;
            line-height: 1;
        }
        .file-preview-remove:hover { color: #dc2626; }

        /* ── Error Message ── */
        .error-msg {
            display: none;
            background: #fff5f5;
            border: 1px solid #fecaca;
            border-radius: 8px;
            padding: 9px 13px;
            font-size: 12.5px;
            color: #dc2626;
            margin-bottom: 1rem;
            align-items: center;
            gap: 6px;
        }
        .error-msg.show { display: flex; }
        .error-msg svg { width: 14px; height: 14px; fill: #dc2626; flex-shrink: 0; }

        /* ── Pending / Approved Status ── */
        .status-badge {
            display: flex; align-items: center; gap: 10px;
            border-radius: 10px; padding: 12px 14px;
            font-size: 13px; font-weight: 500;
            margin-bottom: 1rem;
            font-family: 'Mangal', 'Nirmala UI', 'Arial Unicode MS', sans-serif;
        }
        .status-badge svg { width: 18px; height: 18px; flex-shrink: 0; }
        .status-pending  { background: #fffbeb; border: 1px solid #fde68a; color: #92400e; }
        .status-pending svg { fill: #d97706; }
        .status-approved { background: #f0fdf4; border: 1px solid #bbf7d0; color: #166534; }
        .status-approved svg { fill: #16a34a; }

        /* ── Button Row ── */
        .btn-row {
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 10px;
            padding-top: 0.25rem;
        }
        .btn {
            display: inline-flex; align-items: center; gap: 6px;
            padding: 10px 20px;
            border-radius: 8px;
            font-size: 13px; font-weight: 500;
            border: none; cursor: pointer;
            transition: all 0.15s;
            font-family: 'Inter', sans-serif;
        }
        .btn-secondary {
            background: #f3f4f6;
            color: #374151;
            border: 1px solid #e5e7eb;
        }
        .btn-secondary:hover { background: #e5e7eb; }
        .btn-secondary svg { width: 14px; height: 14px; fill: #6b7280; }

        .btn-primary {
            background: #4f46e5;
            color: #fff;
        }
        .btn-primary:hover:not(:disabled) { background: #4338ca; }
        .btn-primary:disabled { opacity: 0.45; cursor: not-allowed; }
        .btn-primary svg { width: 14px; height: 14px; fill: #fff; }

        .btn-loader {
            display: none;
            background: #4338ca;
            color: #fff;
            cursor: not-allowed;
        }
        .btn-loader .spin {
            width: 14px; height: 14px;
            border: 2px solid rgba(255,255,255,0.3);
            border-top-color: #fff;
            border-radius: 50%;
            animation: spin 0.6s linear infinite;
        }
        @keyframes spin { to { transform: rotate(360deg); } }
    </style>
</head>
<body>
<form id="form1" runat="server">
<div class="page-wrap">

    <!-- Step Tracker -->
    <%--<div class="step-track">
        <div class="step-item">
            <div class="step-circle done">1</div>
            <div class="step-label">Basic Info</div>
        </div>
        <div class="step-line done"></div>
        <div class="step-item">
            <div class="step-circle done">2</div>
            <div class="step-label">Bank Details</div>
        </div>
        <div class="step-line done"></div>
        <div class="step-item">
            <div class="step-circle done">3</div>
            <div class="step-label">Documents</div>
        </div>
        <div class="step-line done"></div>
        <div class="step-item">
            <div class="step-circle active">4</div>
            <div class="step-label active">Seller PDF</div>
        </div>
    </div>--%>

    <!-- Main Card -->
    <div class="card" id="step4">

        <div class="card-header">
            <div class="card-header-icon">
                <svg viewBox="0 0 24 24"><path d="M14 2H6c-1.1 0-2 .9-2 2v16c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V8l-6-6zm2 16H8v-2h8v2zm0-4H8v-2h8v2zm-3-5V3.5L18.5 9H13z"/></svg>
            </div>
            <span class="card-title">Direct Seller PDF</span>
            <%--<span class="step-pill">Step 4 of 4</span>--%>
        </div>

        <div class="card-body">

            <!-- Download instruction box -->
            <div class="download-box">
                <div class="dl-icon">
                    <svg viewBox="0 0 24 24"><path d="M12 16l-5-5 1.41-1.41L11 13.17V4h2v9.17l2.59-2.58L17 11l-5 5zM5 20v-2h14v2H5z"/></svg>
                </div>
                <div class="dl-content">
                    <div class="dl-title">PDF Template &#x0921;&#x093E;&#x0909;&#x0928;&#x0932;&#x094B;&#x0921; &#x0915;&#x0930;&#x0947;&#x0902;</div>
                    <div class="dl-hindi">&#x0915;&#x0943;&#x092A;&#x092F;&#x093E; &#x0909;&#x092A;&#x0930; &#x0926;&#x0940; &#x0917;&#x0908; PDF &#x092B;&#x093C;&#x093E;&#x0907;&#x0932; &#x0921;&#x093E;&#x0909;&#x0928;&#x0932;&#x094B;&#x0921; &#x0915;&#x0930;&#x0947;&#x0902;,<br />&#x0906;&#x0935;&#x0936;&#x094D;&#x092F;&#x0915; &#x0935;&#x093F;&#x0935;&#x0930;&#x0923; &#x092D;&#x0930;&#x0947;&#x0902; &#x0914;&#x0930; &#x092B;&#x093F;&#x0930; &#x092F;&#x0939;&#x093E;&#x0901; Upload &#x0915;&#x0930;&#x0947;&#x0902;&#x0964;</div>
                    <div class="dl-warning">
                        <svg viewBox="0 0 24 24"><path d="M12 2L1 21h22L12 2zm0 3.5L20.5 19h-17L12 5.5zM11 10v4h2v-4h-2zm0 6v2h2v-2h-2z"/></svg>
                        PDF file size 5 MB &#x0938;&#x0947; &#x0905;&#x0927;&#x093F;&#x0915; &#x0928;&#x0939;&#x0940;&#x0902; &#x0939;&#x094B;&#x0928;&#x0940; &#x091A;&#x093E;&#x0939;&#x093F;&#x090F;
                    </div>
                    <a href="UploadForm.aspx?action=download" class="btn-dl">
                        <svg viewBox="0 0 24 24"><path d="M12 16l-5-5 1.41-1.41L11 13.17V4h2v9.17l2.59-2.58L17 11l-5 5zM5 20v-2h14v2H5z"/></svg>
                        Download PDF
                    </a>
                </div>
            </div>

            <!-- Reject reason (only when status = R) -->
            <% if (Userkycres != null && Userkycres.Isformupload == "R") { %>
            <div class="reject-alert">
                <div class="reject-alert-title">
                    <svg viewBox="0 0 24 24"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
                    Document Rejected
                </div>
                <div class="reject-row">
                    <span>Reject Reason: </span>
                    <span class="reject-value"><%= Userkycres.rejectreason %></span>
                </div>
                <div class="reject-row">
                    <span>Reject Remark: </span>
                    <span class="reject-value"><%= Userkycres.rejectremark %></span>
                </div>
            </div>
            <% } %>

            <!-- Approved status -->
            <% if (Userkycres != null && Userkycres.Isformupload == "A") { %>
            <div class="status-badge status-approved">
                <svg viewBox="0 0 24 24"><path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41L9 16.17z"/></svg>
                &#x0906;&#x092A;&#x0915;&#x093E; document approved &#x0939;&#x094B; &#x091A;&#x0941;&#x0915;&#x093E; &#x0939;&#x0948;&#x0964;
            </div>
            <% } %>

            <!-- Upload form (only when can upload) -->
            <% if (Userkycres == null || Userkycres.Isformupload == "P" || Userkycres.Isformupload == "R" || Userkycres.Isformupload == "") { %>

            <!-- Pending notice -->
            <% if (Userkycres != null && Userkycres.Isformupload == "P") { %>
            <div class="status-badge status-pending">
                <svg viewBox="0 0 24 24"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm0 18c-4.41 0-8-3.59-8-8s3.59-8 8-8 8 3.59 8 8-3.59 8-8 8zm.5-13H11v6l5.25 3.15.75-1.23-4.5-2.67V7z"/></svg>
                Document submitted &#x0939;&#x0948;, review &#x092E;&#x0947;&#x0902; &#x0939;&#x0948;&#x0964; &#x0906;&#x092A; reupload &#x0915;&#x0930; &#x0938;&#x0915;&#x0924;&#x0947; &#x0939;&#x0948;&#x0902;&#x0964;
            </div>
            <% } %>

            <!-- Error message area -->
            <div class="error-msg" id="errMsg">
                <svg viewBox="0 0 24 24"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>
                <span id="errText"></span>
            </div>

            <!-- Drag & Drop Upload Zone -->
            <div class="upload-zone" id="uploadZone">
                <input type="file" id="kycdoc" accept="application/pdf"
                    onchange="onFileSelected(this)" />
                <div class="upload-zone-icon">
                    <svg viewBox="0 0 24 24"><path d="M9 16h6v-6h4l-7-7-7 7h4zm-4 2h14v2H5z"/></svg>
                </div>
                <div class="upload-zone-text">
                    <b>Click &#x0915;&#x0930;&#x0947;&#x0902;</b> &#x092F;&#x093E; file &#x092F;&#x0939;&#x093E;&#x0901; drag &#x0915;&#x0930;&#x0947;&#x0902;
                </div>
                <div class="upload-zone-sub">PDF only · Maximum 5 MB</div>
            </div>

            <!-- File selected preview -->
            <div class="file-preview" id="filePreview">
                <svg viewBox="0 0 24 24"><path d="M14 2H6c-1.1 0-2 .9-2 2v16c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V8l-6-6zm2 16H8v-2h8v2zm0-4H8v-2h8v2zm-3-5V3.5L18.5 9H13z"/></svg>
                <span class="file-preview-name" id="previewName">file.pdf</span>
                <span class="file-preview-size" id="previewSize">0 KB</span>
                <button type="button" class="file-preview-remove" onclick="removeFile()" title="Remove">&#x2715;</button>
            </div>

            <!-- Buttons -->
            <div class="btn-row">
              <%--  <button type="button" class="btn btn-secondary" onclick="prevStep(4)">
                    <svg viewBox="0 0 24 24"><path d="M20 11H7.83l5.59-5.59L12 4l-8 8 8 8 1.41-1.41L7.83 13H20v-2z"/></svg>
                    Previous
                </button>--%>

                <div style="display:flex;gap:8px;">
                    <button class="btn btn-primary btn-loader" type="button" id="formloader" disabled>
                        <span class="spin"></span>
                        Uploading...
                    </button>
                    <button type="button" class="btn btn-primary" id="formbtn"
                        onclick="FormUpload()" disabled>
                        <svg viewBox="0 0 24 24"><path d="M9 16h6v-6h4l-7-7-7 7h4zm-4 2h14v2H5z"/></svg>
                        Submit
                    </button>
                </div>
            </div>

            <% } %>

        </div><!-- /.card-body -->
    </div><!-- /.card -->

</div><!-- /.page-wrap -->
</form>

<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>

<script>
    /* ── Auto scroll ── */
    window.onload = function () {
        var status = "<%= Userkycres != null ? Userkycres.Isformupload : "" %>";
        if (status === "P" || status === "") {
            document.getElementById("step4").scrollIntoView({ behavior: "smooth" });
        }
    };

    /* ── Drag & Drop highlight ── */
    var zone = document.getElementById("uploadZone");
    if (zone) {
        zone.addEventListener("dragover",  function(e){ e.preventDefault(); zone.classList.add("drag-active"); });
        zone.addEventListener("dragleave", function(){ zone.classList.remove("drag-active"); });
        zone.addEventListener("drop",      function(){ zone.classList.remove("drag-active"); });
    }

    /* ── File selected callback ── */
    function onFileSelected(input) {
        clearError();
        var file = input.files[0];
        if (!file) { removeFile(); return; }

        var ext = file.name.substring(file.name.lastIndexOf('.')).toLowerCase();
        if (ext !== ".pdf") {
            showError("\u0915\u0947\u0935\u0932 PDF file allowed \u0939\u0948: " + file.name);
            input.value = "";
            return;
        }

        var maxSize = 5 * 1024 * 1024;
        if (file.size > maxSize) {
            showError("PDF file size 5 MB \u0938\u0947 \u0905\u0927\u093F\u0915 \u0928\u0939\u0940\u0902 \u0939\u094B\u0928\u0940 \u091A\u093E\u0939\u093F\u090F\u0964");
            input.value = "";
            return;
        }

        document.getElementById("previewName").textContent = file.name;
        document.getElementById("previewSize").textContent = (file.size / 1024 / 1024).toFixed(2) + " MB";
        document.getElementById("filePreview").classList.add("show");
        document.getElementById("formbtn").disabled = false;
    }

    function removeFile() {
        document.getElementById("kycdoc").value = "";
        document.getElementById("filePreview").classList.remove("show");
        document.getElementById("formbtn").disabled = true;
        clearError();
    }

    function showError(msg) {
        document.getElementById("errText").textContent = msg;
        document.getElementById("errMsg").classList.add("show");
    }

    function clearError() {
        document.getElementById("errMsg").classList.remove("show");
    }

    /* ── AJAX Upload (same logic as original, just cleaned up) ── */
    function FormUpload() {

        var fileInput = document.getElementById("kycdoc");

        if (!fileInput.files.length) {
            alert("Please select PDF file");
            return;
        }

        var file = fileInput.files[0];

        if (!file.name.toLowerCase().endsWith(".pdf")) {
            alert("Only PDF allowed");
            return;
        }

        if (file.size > 5 * 1024 * 1024) {
            alert("PDF size must be under 5MB");
            return;
        }

        $("#formbtn").hide();
        $("#formloader").show();

        var formData = new FormData();

        formData.append("kycdoc", file);

        $.ajax({

            url: "UploadForm.aspx?action=upload",

            type: "POST",

            data: formData,

            contentType: false,

            processData: false,

            success: function (response) {

                if (response.success === true) {

                    alert(response.message);

                    window.location.href = "IndexTb.aspx";
                }
                else {

                    alert(response.message);

                    $("#formbtn").show();
                    $("#formloader").hide();
                }
            },

            error: function () {

                alert("Upload failed. Server error.");

                $("#formbtn").show();
                $("#formloader").hide();
            }

        });

    }
</script>

</body>
</html>
