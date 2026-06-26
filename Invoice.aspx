<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Invoice.aspx.cs" Inherits="Invoice" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice  <%=Session["Title"] ?? Session["CompName"]%></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="https://fonts.googleapis.com/css2?family=DM+Serif+Display&family=DM+Sans:ital,wght@0,300;0,400;0,500;0,600;1,400&display=swap" rel="stylesheet" />
    <style>
        *, *::before, *::after {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
        }

        :root {
            --ink: #0f0e0c;
            --paper: #faf8f3;
            --cream: #f0ece0;
            --accent: #c84b2f;
            --muted: #8a8070;
            --rule: #d8d0c0;
            --serif: 'DM Serif Display', Georgia, serif;
            --sans: 'DM Sans', system-ui, sans-serif;
        }

        @page {
            size: A4;
            margin: 0;
        }

        body {
            background: #e8e4d8;
            font-family: var(--sans);
            color: var(--ink);
            min-height: 100vh;
            display: flex;
            align-items: flex-start;
            justify-content: center;
            padding: 40px 16px 60px;
        }

        .page {
            background: var(--paper);
            width: 100%;
            max-width: 960px;
            position: relative;
            box-shadow: 0 8px 60px rgba(0,0,0,0.18), 0 2px 12px rgba(0,0,0,0.08);
            overflow: hidden;
            animation: rise 0.6s cubic-bezier(0.22,1,0.36,1) both;
            padding-bottom: 58px;
        }

        @keyframes rise {
            from {
                opacity: 0;
                transform: translateY(28px);
            }

            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        .page::before {
            content: '';
            position: absolute;
            top: 0;
            right: 0;
            width: 170px;
            height: 170px;
            background: var(--accent);
            clip-path: polygon(100% 0, 0 0, 100% 100%);
            opacity: 0.08;
            pointer-events: none;
        }

        /* ─── HEADER ─── */
        .header {
            display: flex;
            flex-wrap: wrap;
            align-items: flex-start;
            justify-content: space-between;
            gap: 18px;
            padding: 44px 48px 30px;
            border-bottom: 1.5px solid var(--rule);
        }

        .brand-mark {
            display: flex;
            align-items: center;
            gap: 14px;
        }

        .logo-circle {
            height: 75px;
            flex-shrink: 0;
        }

        .brand-name {
            font-family: var(--serif);
            font-size: 22px;
            line-height: 1;
            text-transform: uppercase;
        }

        .brand-tagline {
            font-size: 10px;
            color: var(--muted);
            letter-spacing: 0.07em;
            margin-top: 5px;
            text-transform: uppercase;
        }

        .invoice-badge {
            text-align: right;
        }

        .invoice-word {
            font-family: var(--serif);
            font-size: 34px;
            color: var(--accent);
            letter-spacing: -1px;
            line-height: 1;
        }

        .invoice-num {
            font-size: 11px;
            color: var(--muted);
            letter-spacing: 0.08em;
            margin-top: 5px;
            text-transform: uppercase;
        }

        /* ─── GST TYPE BADGE ─── */
        .gst-type-badge {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            padding: 5px 12px;
            border-radius: 20px;
            font-size: 11px;
            font-weight: 600;
            letter-spacing: 0.07em;
            text-transform: uppercase;
            margin-top: 8px;
        }

            .gst-type-badge.intra {
                background: #dcfce7;
                color: #166534;
                border: 1px solid #bbf7d0;
            }

            .gst-type-badge.inter {
                background: #ede9fe;
                color: #5b21b6;
                border: 1px solid #ddd6fe;
            }

            .gst-type-badge::before {
                content: '';
                width: 7px;
                height: 7px;
                border-radius: 50%;
                flex-shrink: 0;
            }

            .gst-type-badge.intra::before {
                background: #16a34a;
            }

            .gst-type-badge.inter::before {
                background: #7c3aed;
            }

        /* ─── META STRIP ─── */
        .meta-strip {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            gap: 16px;
            padding: 16px 48px;
            background: var(--cream);
            border-bottom: 1.5px solid var(--rule);
        }

        .meta-cell label {
            display: block;
            font-size: 9px;
            text-transform: uppercase;
            letter-spacing: 0.1em;
            color: var(--muted);
            margin-bottom: 4px;
        }

        .meta-cell span {
            font-size: 13px;
            font-weight: 500;
        }

        .status-badge {
            display: inline-block;
            background: #d97706;
            color: #fff;
            font-size: 9px;
            letter-spacing: 0.1em;
            text-transform: uppercase;
            padding: 3px 9px;
            border-radius: 20px;
            font-weight: 600;
        }

        /* ─── ADDRESSES ─── */
        .address-row {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 32px;
            padding: 28px 48px;
            border-bottom: 1.5px solid var(--rule);
        }

        .address-block label {
            font-size: 9px;
            text-transform: uppercase;
            letter-spacing: 0.12em;
            color: var(--muted);
            display: block;
            margin-bottom: 8px;
        }

        .address-block .name {
            font-family: var(--serif);
            font-size: 16px;
            margin-bottom: 5px;
            text-transform: uppercase;
        }

        .address-block p {
            font-size: 12.5px;
            line-height: 1.75;
            color: #4a4540;
        }

        .address-block .state-pill {
            display: inline-block;
            margin-top: 6px;
            padding: 2px 10px;
            border-radius: 12px;
            font-size: 10px;
            font-weight: 600;
            background: var(--cream);
            border: 1px solid var(--rule);
            color: var(--muted);
        }

        /* ─── DESKTOP TABLE ─── */
        .table-wrap {
            padding: 26px 48px 0;
            overflow-x: auto;
        }

        .items-table {
            width: 100%;
            border-collapse: collapse;
            min-width: 700px;
        }

            .items-table thead tr {
                border-bottom: 1.5px solid var(--ink);
            }

            .items-table thead th {
                font-size: 8.5px;
                text-transform: uppercase;
                letter-spacing: 0.11em;
                color: #161615;
                padding: 0 4px 10px;
                font-weight: 500;
                text-align: center;
                white-space: nowrap;
            }

                .items-table thead th:first-child {
                    text-align: left;
                    padding-left: 0;
                }

                .items-table thead th:last-child {
                    text-align: right;
                    padding-right: 0;
                }

            .items-table tbody tr {
                border-bottom: 1px solid var(--rule);
                transition: background 0.12s;
            }

                .items-table tbody tr:hover {
                    background: rgba(200,75,47,0.03);
                }

            .items-table tbody td {
                padding: 12px 4px;
                font-size: 12px;
                vertical-align: middle;
                text-align: center;
            }

                .items-table tbody td:first-child {
                    text-align: left;
                    padding-left: 0;
                }

                .items-table tbody td:last-child {
                    text-align: right;
                    padding-right: 0;
                    font-variant-numeric: tabular-nums;
                    font-weight: 600;
                }

        .product-name {
            font-weight: 600;
            font-size: 12.5px;
        }

        .product-desc {
            font-size: 10px;
            color: var(--muted);
            margin-top: 2px;
        }

        .num {
            font-variant-numeric: tabular-nums;
        }

        /* Tax columns */
        .cgst-col {
            color: #1a5c9e;
        }

        .sgst-col {
            color: #1a7c4e;
        }

        .igst-col {
            color: #7c3aed;
        }

        .dash-cell {
            color: var(--rule);
            font-size: 14px;
        }

        /* ─── TOTALS ─── */
        .totals-wrap {
            display: flex;
            justify-content: flex-end;
            padding: 18px 48px 26px;
        }

        .totals-box {
            width: 100%;
            max-width: 340px;
        }

        .totals-row {
            display: flex;
            justify-content: space-between;
            align-items: center;
            font-size: 12px;
            padding: 7px 0;
            border-bottom: 1px solid var(--rule);
            color: #4a4540;
            gap: 14px;
        }

            .totals-row:last-child {
                border-bottom: none;
            }

            .totals-row .lbl {
                flex: 1;
            }

            .totals-row .val {
                font-variant-numeric: tabular-nums;
                white-space: nowrap;
            }

            .totals-row.section-head {
                font-size: 9px;
                text-transform: uppercase;
                letter-spacing: 0.1em;
                color: var(--muted);
                border-bottom: none;
                padding-top: 14px;
                padding-bottom: 2px;
            }

            .totals-row .val.cgst {
                color: #1a5c9e;
                font-weight: 500;
            }

            .totals-row .val.sgst {
                color: #1a7c4e;
                font-weight: 500;
            }

            .totals-row .val.igst {
                color: #7c3aed;
                font-weight: 500;
            }

            .totals-row.grand {
                font-family: var(--serif);
                font-size: 18px;
                color: var(--ink);
                padding-top: 12px;
                margin-top: 6px;
                border-top: 2px solid var(--ink);
                border-bottom: none;
            }

                .totals-row.grand .val {
                    color: var(--accent);
                }

        /* ─── BOTTOM GRID ─── */
        .bottom-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 28px;
            padding: 22px 48px 34px;
            background: var(--cream);
            border-top: 1.5px solid var(--rule);
        }

        .bottom-section label {
            font-size: 9px;
            text-transform: uppercase;
            letter-spacing: 0.12em;
            color: var(--muted);
            display: block;
            margin-bottom: 12px;
        }

        .bank-table {
            width: 100%;
            border-collapse: collapse;
        }

            .bank-table td {
                font-size: 11.5px;
                padding: 4px 0;
                vertical-align: top;
            }

                .bank-table td:first-child {
                    color: var(--muted);
                    width: 88px;
                }

                .bank-table td:last-child {
                    font-weight: 500;
                }

        .note-text {
            font-size: 11.5px;
            line-height: 1.75;
            color: #5a5248;
        }

        /* ─── FOOTER ─── */
        .footer {
            position: absolute;
            bottom: 0;
            left: 0;
            right: 0;
            display: flex;
            flex-wrap: wrap;
            align-items: center;
            justify-content: space-between;
            gap: 6px;
            padding: 12px 48px;
            background: var(--ink);
            color: rgba(255,255,255,0.42);
            font-size: 10px;
            letter-spacing: 0.05em;
        }

            .footer .pg {
                color: var(--accent);
            }

            .footer .gst-type-footer {
                color: rgba(255,255,255,0.6);
                font-size: 9px;
                letter-spacing: 0.08em;
            }

        /* ════ PRINT ════ */
        .print-btn {
            position: fixed;
            bottom: 28px;
            right: 28px;
            background: var(--accent);
            color: #fff;
            border: none;
            cursor: pointer;
            font-family: var(--sans);
            font-size: 13px;
            font-weight: 600;
            padding: 12px 24px;
            border-radius: 40px;
            box-shadow: 0 4px 20px rgba(200,75,47,0.4);
            letter-spacing: 0.04em;
            transition: transform 0.15s, box-shadow 0.15s;
            z-index: 100;
        }

            .print-btn:hover {
                transform: translateY(-2px);
                box-shadow: 0 6px 28px rgba(200,75,47,0.5);
            }

        /* ════════════════════════════════
           MOBILE CARD VIEW
        ════════════════════════════════ */
        .mobile-items {
            display: none;
            padding: 18px 16px 0;
        }

        .m-item {
            background: var(--paper);
            border: 1px solid var(--rule);
            border-radius: 10px;
            padding: 14px;
            margin-bottom: 10px;
        }

        .m-item-header {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            gap: 8px;
            margin-bottom: 4px;
        }

        .m-item-name {
            font-weight: 600;
            font-size: 13px;
            line-height: 1.3;
        }

        .m-item-total {
            font-weight: 700;
            font-size: 14px;
            color: var(--accent);
            white-space: nowrap;
        }

        .m-item-desc {
            font-size: 10.5px;
            color: var(--muted);
            margin-bottom: 10px;
        }

        .m-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 8px;
            background: var(--cream);
            border-radius: 7px;
            padding: 10px;
        }

            .m-grid .gc label {
                display: block;
                font-size: 8.5px;
                text-transform: uppercase;
                letter-spacing: 0.09em;
                color: var(--muted);
                margin-bottom: 2px;
            }

            .m-grid .gc span {
                font-size: 12px;
                font-weight: 500;
            }

            .m-grid .gc.cgst span {
                color: #1a5c9e;
            }

            .m-grid .gc.sgst span {
                color: #1a7c4e;
            }

            .m-grid .gc.igst span {
                color: #7c3aed;
            }

        /* ════════════
           BREAKPOINTS
        ════════════ */
        @media (max-width: 780px) {
            body {
                padding: 0 0 40px;
            }

            .page {
                box-shadow: none;
                border-radius: 0;
            }

            .header {
                padding: 26px 20px 20px;
            }

            .invoice-word {
                font-size: 28px;
            }

            .meta-strip {
                grid-template-columns: 1fr 1fr;
                padding: 13px 20px;
            }

            .address-row {
                padding: 20px 20px;
                gap: 20px;
            }

            .table-wrap {
                display: none;
            }

            .mobile-items {
                display: block;
            }

            .totals-wrap {
                padding: 16px 20px 20px;
            }

            .totals-box {
                max-width: 100%;
            }

            .bottom-grid {
                grid-template-columns: 1fr;
                gap: 18px;
                padding: 18px 20px 26px;
            }

            .footer {
                padding: 10px 20px;
                font-size: 9.5px;
            }
        }

        @media (max-width: 480px) {
            .header {
                flex-direction: column;
                gap: 12px;
                padding: 20px 16px 16px;
            }

            .invoice-badge {
                text-align: left;
            }

            .invoice-word {
                font-size: 26px;
            }

            .meta-strip {
                grid-template-columns: 1fr 1fr;
                padding: 11px 16px;
                gap: 10px;
            }

            .address-row {
                grid-template-columns: 1fr;
                padding: 16px 16px;
                gap: 16px;
            }

            .mobile-items {
                padding: 14px 12px 0;
            }

            .totals-wrap {
                padding: 12px 16px 16px;
            }

            .bottom-grid {
                padding: 16px 16px 22px;
            }

            .footer {
                flex-direction: column;
                align-items: flex-start;
                padding: 10px 16px;
                font-size: 9px;
            }
        }

        @media print {
            body {
                background: white;
                padding: 0;
            }

            .page {
                box-shadow: none;
                max-width: 100%;
            }

            .mobile-items {
                display: none !important;
            }

            .table-wrap {
                display: block !important;
            }

            .print-btn {
                display: none !important;
            }

            .back-btn {
                display: none !important;
            }
        }

        .back-btn {
            background: var(--accent);
            color: #fff;
            border: none;
            cursor: pointer;
            font-family: var(--sans);
            font-size: 13px;
            font-weight: 600;
            padding: 12px 24px;
            border-radius: 40px;
            box-shadow: 0 4px 20px rgba(200,75,47,0.4);
            letter-spacing: 0.04em;
            transition: transform 0.15s, box-shadow 0.15s;
            z-index: 100;
        }

            .back-btn:hover {
                transform: translateY(-2px);
                box-shadow: 0 6px 28px rgba(200,75,47,0.5);
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!-- ── PRINT BUTTON ── -->
        <button class="print-btn" type="button" onclick="window.print()">Print / Save PDF</button>
        <asp:Button ID="BtnBack" runat="server" Text="Back" class="back-btn" OnClick="BtnBack_Click" />
        <div class="page">

            <!-- ══════════════════════════════
                 HEADER
            ══════════════════════════════ -->
            <div class="header">
                <div class="brand-mark">
                    <img class="logo-circle" src="images/logo.png" runat="server" id="imgLogo" alt="Logo" />
                    <div>
                        <div class="brand-name"><%=Session["CompName"]%></div>
                        <div class="brand-tagline">
                            <%=Session["CompAdd"]%><br />
                            GSTIN:
                           
                            <asp:Label ID="GSTIN" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="invoice-badge">
                    <div class="invoice-word">Invoice</div>
                    <div class="invoice-num">
                        # INV-2026-<asp:Label ID="LblBillno" runat="server" />
                    </div>
                    <!-- Dynamic GST type badge rendered from code-behind -->
                    <asp:Literal ID="LitGstBadge" runat="server" />
                </div>
            </div>

            <!-- ══════════════════════════════
                 META STRIP
            ══════════════════════════════ -->
            <div class="meta-strip">
                <div class="meta-cell">
                    <label>Bill No</label>
                    <span>
                        <asp:Label ID="LblBillnumber" runat="server" /></span>
                </div>
                <div class="meta-cell">
                    <label>Bill Date</label>
                    <span>
                        <asp:Label ID="LblBillDate" runat="server" /></span>
                </div>
                <div class="meta-cell">
                    <label>Member ID</label>
                    <span>
                        <asp:Label ID="LblMemberID" runat="server" /></span>
                </div>
                <div class="meta-cell">
                    <label>Member Name</label>
                    <span>
                        <asp:Label ID="LblMemberName" runat="server" /></span>
                </div>
            </div>

            <!-- ══════════════════════════════
                 ADDRESSES
            ══════════════════════════════ -->
            <div class="address-row">
                <div class="address-block">
                    <label>From</label>
                    <div class="name"><%=Session["CompName"]%></div>
                    <p><%=Session["CompAdd"]%></p>
                </div>
                <div class="address-block">
                    <label>Bill To</label>
                    <div class="name">
                        <asp:Label ID="LblName" runat="server" />
                    </div>
                    <p>
                        <asp:Label ID="LblAddress" runat="server" />
                    </p>
                    <!-- Mobile number -->
                    <p style="margin-top: 6px; font-size: 13px; font-weight: 600; color: var(--ink);">
                        &#128222;
                       
                        <asp:Label ID="LblMobile" runat="server" />
                    </p>
                    <asp:Literal ID="LitCustStatePill" runat="server" />
                </div>
            </div>

            <!-- ══════════════════════════════
                 DESKTOP TABLE
            ══════════════════════════════ -->
            <div class="table-wrap">
                <table class="items-table">
                    <thead>
                        <asp:Literal ID="LitTableHead" runat="server" />
                    </thead>
                    <tbody>
                        <asp:Literal ID="LitTableBody" runat="server" />
                    </tbody>
                </table>
            </div>
            <div class="footer">
                <span style="color: white; font-size: 1.1em;">This is computer generated invoice hence no stamp required</span>
               <%-- <span class="pg" style="color: white; font-size: 1.1em;">Page 1 of 1</span>--%>
            </div>
            <!-- ══════════════════════════════
                 MOBILE CARDS
            ══════════════════════════════ -->
            <div class="mobile-items">
                <asp:Literal ID="LitMobileCards" runat="server" />
            </div>

            <!-- ══════════════════════════════
                 TOTALS
            ══════════════════════════════ -->
            <div class="totals-wrap">
                <div class="totals-box">
                    <asp:Literal ID="LitTotals" runat="server" />
                </div>
            </div>

        </div>
        <!-- /.page -->
    </form>
</body>
</html>
