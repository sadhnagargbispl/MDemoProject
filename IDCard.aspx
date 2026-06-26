<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IDCard.aspx.cs" Inherits="IDCard" %>

<!DOCTYPE html>
<html>
<head runat="server">

    <style>
        body {
            background: #eef2f6;
            font-family: Segoe UI;
        }


        /* CARD */

        .idcard {
            width: 320px;
            height: 500px;
            margin: 40px auto;
            background: white;
            border-radius: 16px;
            box-shadow: 0px 8px 20px rgba(0,0,0,.15);
            overflow: hidden;
            position: relative;
        }


        /* HEADER */

        .header {
            background: #eef2f6;
            height: 120px;
            text-align: center;
            position: relative;
            display: flex;
            justify-content: center;
            align-items: flex-start;
            padding-top: 12px;
        }


        /* HEADER LOGO */

        .logo {
            height: 65px;
            object-fit: contain;
        }


        /* PHOTO */

        .photo-box {
            position: absolute;
            top: 85px;
            width: 100%;
            text-align: center;
        }


        .photo {
            width: 95px;
            height: 95px;
            border-radius: 14px;
            border: 5px solid white;
            object-fit: cover;
            box-shadow: 0px 4px 10px rgba(0,0,0,.2);
        }


        /* BODY */

        .info {
            margin-top: 80px;
            padding: 22px;
            font-size: 14px;
        }


        .row {
            margin-bottom: 10px;
        }


        /* LABEL */

        .label {
            font-weight: 600;
            width: 80px;
            display: inline-block;
            font-size: 14px;
        }


        /* NAME FIELD */

        .name-field {
            font-size: 16px;
            font-weight: 600;
            color: #1c8dc9;
            display: inline-block;
            width: 180px;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
            vertical-align: middle;
        }


        /* ADDRESS FIELD */

        .address-field {
            display: inline-block;
            width: 180px;
            font-size: 13px;
            white-space: nowrap;
            text-overflow: ellipsis;
            vertical-align: middle;
            overflow: hidden;
        }


        /* SIGN */

        .sign {
            text-align: right;
            padding-right: 25px;
            font-size: 13px;
            margin-top: 5px;
        }


        /* FOOTER */

        .footer {
            position: absolute;
            bottom: 0;
            width: 100%;
            background: #eef2f6;
            color: black;
            font-size: 12px;
            text-align: center;
            padding: 10px;
        }
    </style>

</head>

<body>

    <form runat="server">
        <div id="cardDiv" runat="server">
            <asp:Repeater ID="RptIdCard" runat="server">

                <ItemTemplate>

                    <div class="idcard">


                        <!-- HEADER -->

                        <div class="header">

                            <img src="Images/logo.png" class="logo" />

                        </div>


                        <!-- PHOTO -->

                        <div class="photo-box">

                            <asp:Image
                                ID="Image1"
                                runat="server"
                                CssClass="photo"
                                ImageUrl='<%# Eval("ProfilePic") %>' />

                        </div>


                        <!-- INFO -->

                        <div class="info">


                            <div class="row">

                                <span class="label">Name :</span>

                                <asp:Label
                                    ID="LblMemName"
                                    runat="server"
                                    CssClass="name-field"
                                    Text='<%# Eval("MemName") %>' />

                            </div>


                            <div class="row">

                                <span class="label">Mobile :</span>

                                <asp:Label
                                    ID="LblMobl"
                                    runat="server"
                                    Text='<%# Eval("MobileNo") %>' />

                            </div>


                            <div class="row">

                                <span class="label">ID No :</span>

                                <asp:Label
                                    ID="LblId"
                                    runat="server"
                                    Text='<%# Eval("IdNo") %>' />

                            </div>
                            <div class="row">

                                <span class="label">Rank :</span>

                                <asp:Label
                                    ID="LblRank"
                                    runat="server"
                                    Text='<%# Eval("Rank") %>' />

                            </div>

                            <%-- <div class="row">

                                <span class="label">Address :</span>

                                <asp:Label
                                    ID="LblAddress"
                                    runat="server"
                                    CssClass="address-field"
                                    Text='<%# Eval("Address") %>' />

                            </div>--%>


                            <div class="row">

                                <span class="label">DOJ :</span>

                                <asp:Label
                                    ID="LblDOb"
                                    runat="server"
                                    Text='<%# Eval("MemDOB") %>' />

                            </div>


                            <%--  <div class="row">

                                <span class="label">Issue Date :</span>

                                <asp:Label
                                    ID="LblDoActive"
                                    runat="server"
                                    Text='<%# Eval("UpgradeDate") %>' />

                            </div>
                            --%>
                        </div>


                        <!-- SIGN -->

                        <div class="sign">
                            Authorised Sign

                        </div>


                        <!-- FOOTER -->

                        <div class="footer">
                            Bhilwara - Rajasthan, PIN Code : 311001

                        </div>


                    </div>

                </ItemTemplate>

            </asp:Repeater>
        </div>

        <center>

            <br />
            <asp:Button
                ID="BtnBack"
                runat="server"
                Text="Back"
                OnClick="BtnBack_Click"
                Style="background: #24a950; width: 100px; height: 43px; color: white; border: 1px solid #24a950;" />
            <asp:Button
                ID="btnPrint"
                runat="server"
                Text="Print"
                OnClientClick="printCard(); return false;"
                Style="background: #24a950; width: 100px; height: 43px; color: white; border: 1px solid #24a950;" />

        </center>


    </form>

</body>
<script>

    function printCard() {

        var printContents =
            document.getElementById("cardDiv").innerHTML;

        var originalContents =
            document.body.innerHTML;

        document.body.innerHTML =
            printContents;

        window.print();

        document.body.innerHTML =
            originalContents;

        location.reload();

    }

</script>
</html>
