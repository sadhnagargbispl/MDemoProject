<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WUCMenu.ascx.cs" Inherits="WUCMenu" %>

<!-- This control renders the sidebar menu expected by SitePage.master -->
<div runat="server" id="menuContainer">
    <% if (Session["Status"] != null && Session["Status"].ToString() == "OK")
        { %>

    <ul class="sidebar-menu" id="menu" runat="server">
    </ul>

    <% }
        else if (Session["CompID"] != null && Session["CompID"].ToString() == "1057")
        { %>

    <ul class="sidebar-menu">
        <asp:Label ID="kit" runat="server"></asp:Label>
        <li><a href="" runat="server" id="zaranewjoining">Sign Up</a></li>
        <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
        <li><a href="Defaultzara.aspx">Sign In</a></li>
    </ul>

    <% }
        else if (Session["CompID"] != null && Session["CompID"].ToString() == "1074")
        { %>

    <ul class="sidebar-menu">
        <li><a href="" runat="server" id="AnewjoiningCashLess">Sign Up</a></li>
        <li><a href="Default.aspx">Sign In</a></li>
    </ul>

    <% }
        else
        { %>

    <ul class="sidebar-menu">
        <li><a href="" runat="server" id="Anewjoining">Registration</a></li>
        <li><a href="Default.aspx">Login</a></li>
    </ul>

    <% } %>
</div>
