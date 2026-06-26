<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Referaltree.aspx.cs" Inherits="Referaltree" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NewTree</title>
    <link href="css/tree.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        #dhtmltooltip {
            BORDER-RIGHT: black 1px solid;
            PADDING-RIGHT: 2px;
            BORDER-TOP: black 1px solid;
            PADDING-LEFT: 2px;
            Z-INDEX: 100;
            FILTER: progid:DXImageTransform.Microsoft.Shadow(color=gray,direction=135);
            LEFT: 300px;
            VISIBILITY: hidden;
            PADDING-BOTTOM: 2px;
            BORDER-LEFT: black 1px solid;
            WIDTH: 300px;
            PADDING-TOP: 2px;
            BORDER-BOTTOM: black 1px solid;
            POSITION: absolute;
            BACKGROUND-COLOR: lightyellow;
            margin-left: 140px;
        }

        #dhtmlpointer {
            Z-INDEX: 101;
            LEFT: 300px;
            VISIBILITY: hidden;
            POSITION: absolute;
        }

        .style2 {
            FONT-SIZE: 12px
        }
    </style>
    <link href="dtree/dtree.css" type="text/css" rel="stylesheet" />
    <script src="dtree/dtree.js" type="text/javascript"></script>
    <script src="dtree/Referalvertdtree.js" type="text/javascript"></script>
    <script src="Js/generalEvent.js" type="text/javascript"></script>

    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <script type="text/javascript">

        var offsetfromcursorX = 12 //Customize x offset of tooltip
        var offsetfromcursorY = 10 //Customize y offset of tooltip

        var offsetdivfrompointerX = 10 //Customize x offset of tooltip DIV relative to pointer image
        var offsetdivfrompointerY = 12 //Customize y offset of tooltip DIV relative to pointer image. Tip: Set it to (height_of_pointer_image-1).

        document.write('<div id="dhtmltooltip"></div>') //write out tooltip DIV
        document.write('<img id="dhtmlpointer">') //write out pointer image

        var ie = document.all
        var ns6 = document.getElementById && !document.all
        var enabletip = false
        if (ie || ns6)
            var tipobj = document.all ? document.all["dhtmltooltip"] : document.getElementById ? document.getElementById("dhtmltooltip") : ""

        var pointerobj = document.all ? document.all["dhtmlpointer"] : document.getElementById ? document.getElementById("dhtmlpointer") : ""

        function ietruebody() {
            return (document.compatMode && document.compatMode != "BackCompat") ? document.documentElement : document.body
        }

        function ddrivetip(thetext, thewidth, thecolor) {
            if (ns6 || ie) {
                if (typeof thewidth != "undefined") tipobj.style.width = thewidth + "px"
                if (typeof thecolor != "undefined" && thecolor != "") tipobj.style.backgroundColor = thecolor
                tipobj.innerHTML = thetext
                enabletip = true
                return false
            }
        }

        function positiontip(e) {
            if (enabletip) {
                var nondefaultpos = false
                var curX = (ns6) ? e.pageX : event.clientX + ietruebody().scrollLeft;
                var curY = (ns6) ? e.pageY : event.clientY + ietruebody().scrollTop;
                //Find out how close the mouse is to the corner of the window
                var winwidth = ie && !window.opera ? ietruebody().clientWidth : window.innerWidth - 20
                var winheight = ie && !window.opera ? ietruebody().clientHeight : window.innerHeight - 20

                var rightedge = ie && !window.opera ? winwidth - event.clientX - offsetfromcursorX : winwidth - e.clientX - offsetfromcursorX
                var bottomedge = ie && !window.opera ? winheight - event.clientY - offsetfromcursorY : winheight - e.clientY - offsetfromcursorY

                var leftedge = (offsetfromcursorX < 0) ? offsetfromcursorX * (-1) : -1000

                //if the horizontal distance isn't enough to accomodate the width of the context menu
                if (rightedge < tipobj.offsetWidth) {
                    //move the horizontal position of the menu to the left by it's width
                    tipobj.style.left = curX - tipobj.offsetWidth + "px"
                    nondefaultpos = true
                }
                else if (curX < leftedge)
                    tipobj.style.left = "5px"
                else {
                    //position the horizontal position of the menu where the mouse is positioned
                    tipobj.style.left = curX + offsetfromcursorX - offsetdivfrompointerX + "px"
                    pointerobj.style.left = curX + offsetfromcursorX + "px"
                }

                //same concept with the vertical position
                if (bottomedge < tipobj.offsetHeight) {
                    tipobj.style.top = curY - tipobj.offsetHeight - offsetfromcursorY + "px"
                    nondefaultpos = true
                }
                else {
                    tipobj.style.top = curY + offsetfromcursorY + offsetdivfrompointerY + "px"
                    pointerobj.style.top = curY + offsetfromcursorY + "px"
                }
                tipobj.style.visibility = "visible"
                if (!nondefaultpos)
                    pointerobj.style.visibility = "visible"
                else
                    pointerobj.style.visibility = "hidden"
            }
        }

        function hideddrivetip() {
            if (ns6 || ie) {
                enabletip = false
                tipobj.style.visibility = "hidden"
                pointerobj.style.visibility = "hidden"
                tipobj.style.left = "-1000px"
                tipobj.style.backgroundColor = ''
                tipobj.style.width = ''
            }
        }

        document.onmousemove = positiontip

    </script>
    <script>
        function goBack() {
            window.history.back()
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <center>
            <style>
                /* Common button style */
                .btn {
                    height: 40px;
                    min-width: 120px;
                    padding: 0 16px;
                    line-height: 40px;
                    font-size: 14px;
                    color: #fff;
                    border-radius: 4px;
                    border: none;
                }

                .btn-home {
                    background-color: #198754; /* Green */
                }
                /* Hover effect */
                .btn:hover {
                    opacity: 0.9;
                }

                .btn-left {
                    background-color: #dc3545; /* Red */
                }

                .btn-right {
                    background-color: #dc3545; /* Purple */
                }

                .form-control {
                    /* proper height */
                    font-size: 14px;
                    padding: 6px 12px;
                    border-radius: 4px;
                }
            </style>
            <div>
                <div style="vertical-align: top; position: absolute; top: 8px; left: 0px;">
                    <table cellpadding="0" cellspacing="1" border="0" width="450px" style="vertical-align: top">
                        <tr style="font-weight: bold; font-size: 10px; font-family: Verdana;">
                            <%--<td style="WIDTH: 100px"> Downline ID</td>
				        <td style="WIDTH: 84px">
					        <input id="DownLineFormNo" style="BORDER-RIGHT: 1px solid; BORDER-TOP: 1px solid; BORDER-LEFT: 1px solid; BORDER-BOTTOM: 1px solid" maxlength="12" name="DownLineFormNo" runat="server" size="9" />
				        </td>
				        <td style="WIDTH: 100px">Down Level</td>
				        <td style="WIDTH: 75px">
					        <input id="deptlevel" style="BORDER-RIGHT: 1px solid; BORDER-TOP: 1px solid; BORDER-LEFT: 1px solid; BORDER-BOTTOM: 1px solid" maxlength="4" size="3" name="deptlevel" runat="server" />
				        </td>  
				        <td>
					        <asp:ImageButton id="cmdSave1" runat="server" ToolTip="Submit" ImageUrl="~/Images/submit.jpg"></asp:ImageButton>
				        </td>--%>
                            <td style="width: 100px">
                                <%--<asp:ImageButton id="cmdBack" runat="server" ToolTip="Back" ImageUrl="images/back.jpg"></asp:ImageButton>--%>
                                <asp:Button ID="cmdBack" runat="server" Text="Home" Class="btn btn-home" OnClick="cmdBack_Click" />
                                <asp:Button ID="BtnStepAbove" runat="server" Text="1 Step Above" Class="btn btn-home" OnClick="BtnStepAbove_Click" Visible="false" />
                                <input type="button" class="btn btn-home" value="1 Step Above" onclick="goBack()" style="display: none;">
                            </td>
                        </tr>
                    </table>
                    <table id="Table1" cellpadding="0" cellspacing="1" border="0" width="300px" style="vertical-align: top" runat="server">
                        <tr id="Tr1" runat="server" style="font-weight: bold; font-size: 10px; font-family: Verdana;">
                            <td id="td11" runat="server" style="width: 15%; height: 50Px">
                                <asp:Image ID="img11" runat="server" Visible="false" /></td>
                            <td id="td12" runat="server" style="width: 15%; height: 50Px">
                                <asp:Image ID="img12" runat="server" Visible="false" /></td>
                            <td id="td13" runat="server" style="width: 15%; height: 50Px">
                                <asp:Image ID="img13" runat="server" Visible="false" /></td>
                            <td id="td14" runat="server" style="width: 15%; height: 50Px">
                                <asp:Image ID="img14" runat="server" Visible="false" /></td>
                            <td id="td15" runat="server" style="width: 15%; height: 50Px">
                                <asp:Image ID="img15" runat="server" Visible="false" /></td>
                            <td id="td16" runat="server" style="width: 15%; height: 50Px">
                                <asp:Image ID="img16" runat="server" Visible="false" /></td>
                            <td id="td17" runat="server" style="width: 15%; height: 50Px">
                                <asp:Image ID="img17" runat="server" Visible="false" /></td>
                        </tr>
                        <tr style="font-weight: bold; font-size: 10px; font-family: Verdana;">
                            <td id="td21" style="width: 15%" align="center" runat="server"></td>
                            <td id="td22" style="width: 15%" align="center" runat="server"></td>
                            <td id="td23" style="width: 15%" align="center" runat="server"></td>
                            <td id="td24" style="width: 15%" align="center" runat="server"></td>
                            <td id="td25" style="width: 15%" align="center" runat="server"></td>
                            <td id="td26" style="width: 15%" align="center" runat="server"></td>
                            <td id="td27" style="width: 15%" align="center" runat="server"></td>
                        </tr>
                    </table>
                </div>
                <center>
                </center>
            </div>
        </center>
    </form>
</body>
</html>
