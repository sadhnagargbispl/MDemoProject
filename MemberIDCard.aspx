<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MemberIDCard.aspx.cs" Inherits="MemberIDCard" %>

<html>
<head>
    <meta charset="UTF-8">
    <title>ID Card</title>

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css">


    <style>
        @media print {
            body {
                -webkit-print-color-adjust: exact;
            }
        }

        .idcardbackground {
            width: 750px;
            height: 558px;
            background-image: url('images/idcard.png');
            background-repeat: no-repeat;
            position: relative;
        }

        .photoboximg {
            width: 150px;
            height: 150px;
            position: absolute;
            left: 115px;
            top: 125px;
            border-radius: 150px;
        }

        .username {
            height: 30px;
            position: absolute;
            left: 130px;
            top: 280px;
            font-size: 15px;
            color: #000;
            text-align: left;
            font-weight: bold;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
            display: block;
            max-width: 180px;
        }

        .idnumber {
            width: 340px;
            height: 30px;
            position: absolute;
            left: 130px;
            top: 301px;
            font-size: 14px;
            color: #000;
            text-align: left;
        }

        .mobileno {
            width: 340px;
            height: 30px;
            position: absolute;
            left: 130px;
            top: 322px;
            font-size: 14px;
            color: #000;
            text-align: left;
        }

        .dob {
            width: 340px;
            height: 30px;
            position: absolute;
            left: 170px;
            top: 360px;
            font-size: 14px;
            color: #000;
            text-align: left;
        }

        /* .validupto {
    width: 340px;
    height: 30px;
    position: absolute;
    left: 170px;
    top: 380px;
    font-size: 14px;
    color: #000;
    text-align: left;
} */

        .rank {
            width: 340px;
            height: 30px;
            position: absolute;
            left: 170px;
            top: 380px;
            font-size: 14px;
            color: #000;
            text-align: left;
        }

        .city {
            width: 340px;
            height: 30px;
            position: absolute;
            left: 170px;
            top: 397px;
            font-size: 14px;
            color: #000;
            text-align: left;
        }

        .state {
            width: 340px;
            height: 30px;
            position: absolute;
            left: 170px;
            top: 415px;
            font-size: 14px;
            color: #000;
            text-align: left;
        }

        .pancard {
            width: 340px;
            height: 30px;
            position: absolute;
            left: 170px;
            top: 434px;
            font-size: 14px;
            color: #000;
            text-align: left;
        }

        .print-btn {
            margin-top: 20px;
        }
    </style>
    <script>
        function PrintDiv() {
            var div = document.getElementById("dvContents");

            var iframe = document.createElement("iframe");
            iframe.style.position = "fixed";
            iframe.style.right = "0";
            iframe.style.bottom = "0";
            iframe.style.width = "0";
            iframe.style.height = "0";
            iframe.style.border = "0";
            document.body.appendChild(iframe);

            var doc = iframe.contentWindow.document;
            doc.open();
            doc.write(`
        <html>
        <head>
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <style>${document.querySelector("style").innerHTML}</style>
        </head>
        <body>
            ${div.outerHTML}
        </body>
        </html>
    `);
            doc.close();

            setTimeout(() => {
                iframe.contentWindow.focus();
                iframe.contentWindow.print();
                document.body.removeChild(iframe);
            }, 800); // 🔥 mobile ke liye must
        }
    </script>

    <%--    <script>
        function PrintDiv() {
            debugger;
            var divToPrint = document.getElementById("dvContents");

            // Create a hidden iframe for printing
            var iframe = document.createElement('iframe');
            iframe.style.position = 'fixed';
            iframe.style.right = '0';
            iframe.style.bottom = '0';
            iframe.style.width = '0';
            iframe.style.height = '0';
            iframe.style.border = '0';
            document.body.appendChild(iframe);

            var doc = iframe.contentWindow.document;
            doc.open();
            doc.write('<html><head><title>ID Card</title>');

            // Copy styles
            document.querySelectorAll('link[rel="stylesheet"], style').forEach(el => {
                doc.write(el.outerHTML);
            });

            doc.write('</head><body>');
            doc.write(divToPrint.outerHTML);
            doc.write('</body></html>');
            doc.close();

            // Focus and print
            iframe.contentWindow.focus();
            iframe.contentWindow.print();

            // Remove iframe after printing
            setTimeout(() => {
                document.body.removeChild(iframe);
            }, 2000);
        }
    </script>--%>
</head>

<body>

    <div class="container">

        <div class="row">

            <div class="col-sm-2"></div>

            <div class="col-sm-12">
                <div id="dvContents" class="idcardbackground">

                    <img src="images/idcard.png" class="idcardbg">

                    <asp:Image ID="Image2" runat="server" class="photoboximg" />

                    <div class="username">
                        <asp:Label ID="lblName" runat="server"></asp:Label>
                    </div>

                    <div class="idnumber">
                        <asp:Label ID="lblIDNo" runat="server"></asp:Label>
                    </div>

                    <div class="mobileno">
                        <asp:Label ID="lblMobile" runat="server"></asp:Label>
                    </div>

                    <div class="dob">
                        <asp:Label ID="lblValid" runat="server"></asp:Label>
                    </div>

                    <div class="rank">
                        <asp:Label ID="lblRank" runat="server"></asp:Label>
                    </div>

                    <div class="city">
                        <asp:Label ID="lblCity" runat="server"></asp:Label>
                    </div>

                    <div class="state">
                        <asp:Label ID="lblState" runat="server"></asp:Label>
                    </div>

                    <div class="pancard">
                        <asp:Label ID="lblPan" runat="server"></asp:Label>
                    </div>

                </div>

                <%--<div id="dvContents" class="idcardbackground">

                    <img src="images/idcard.png" style="position: absolute; width: 750px; height: 558px; z-index: -1;">
                     <asp:Image ID="Image2" runat="server" class="photoboximg" />
                 

                    <div class="username" align="left">

                        <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="idnumber">
                        <asp:Label ID="lblIDNo" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="mobileno">
                        <asp:Label ID="lblMobile" runat="server" Text=""></asp:Label>
                    </div>

                    <div class="dob">
                        <asp:Label ID="lblValid" runat="server" Text=""></asp:Label>
                    </div>
                   
                    <div class="rank">
                        <asp:Label ID="lblRank" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="city">
                        <asp:Label ID="lblCity" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="state">
                        <asp:Label ID="lblState" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="pancard">
                        <asp:Label ID="lblPan" runat="server" Text=""></asp:Label>
                    </div>



                </div>--%>

                <button onclick="javascript:PrintDiv();" class="btn btn-primary print-btn">Print</button>

                <button onclick="window.location.href='IndexTb.aspx';" class="btn btn-default print-btn">Back</button>


            </div>
        </div>
    </div>

    <script defer src="https://static.cloudflareinsights.com/beacon.min.js/vcd15cbe7772f49c399c6a5babf22c1241717689176015" integrity="sha512-ZpsOmlRQV6y907TI0dKBHq9Md29nnaEIPlkf84rnaERnq6zvWvPUqr2ft8M1aS28oN72PdrCzSjY4U6VaAw1EQ==" data-cf-beacon='{"version":"2024.11.0","token":"50f0eef2700146498b2e02d2aa94a76c","r":1,"server_timing":{"name":{"cfCacheStatus":true,"cfEdge":true,"cfExtPri":true,"cfL4":true,"cfOrigin":true,"cfSpeedBrain":true},"location_startswith":null}}' crossorigin="anonymous"></script>
</body>
</html>
