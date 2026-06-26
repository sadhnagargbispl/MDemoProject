<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="ComplainSolution.aspx.cs" Inherits="ComplainSolution" %>

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
                            <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>Complaint Detail</h5>
                        </div>
                        <div class="card-body">
                            <div class="row g-1">
                                <div class="clr">
                                    <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                    <h6>
                                        <asp:Label ID="lbludaan" runat="server" Text="" Visible="false"></asp:Label>
                                        <asp:Label ID="lblother" runat="server" Text="" Visible="false"></asp:Label>
                                        <%--Complaint Detail--%>
                                    </h6>
                                </div>


                                <div class="col-md-12" runat="server" id="DivH" visible="true">
                                    <asp:Label ID="Label1" runat="server" Text="Total Records"></asp:Label>
                                    <asp:Label ID="lbltotal" runat="server"></asp:Label>
                                    <div class="table-responsive">

                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <table id="customers2" class="table datatable">
                                                    <thead>
                                                        <tr>
                                                            <th>S No</th>

                                                            <th>
                                                                <% if (Session["CompID"].ToString() == "1056" && Session["Status"].ToString() == "OK")
                                                                    { %>
            Grievance Id
        <% }
            else
            { %>
            Complaint Id
        <% } %>
                                                            </th>

                                                            <th>
                                                                <% if (Session["CompID"].ToString() == "1056" && Session["Status"].ToString() == "OK")
                                                                    { %>
            Grievance Date
        <% }
            else
            { %>
            Complaint Date
        <% } %>
                                                            </th>

                                                            <th>
                                                                <% if (Session["CompID"].ToString() == "1056" && Session["Status"].ToString() == "OK")
                                                                    { %>
            Grievance
        <% }
            else
            { %>
            Complaint
        <% } %>
                                                            </th>

                                                            <th>Reply Date</th>
                                                            <th>Reply</th>
                                                            <th>View</th>
                                                        </tr>

                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater ID="RptDirects" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("CID")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("CDate")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("Complaint")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("SDate")%>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("Solution")%>
                                                                    </td>

                                                                    <td>
                                                                        <a href="javascript:void(0);"
                                                                            onclick='<%# "openReplyModal(" + Eval("VCid") + ")" %>'
                                                                            style="cursor: pointer;">View Detail
                                                                        </a>
                                                                        <%--<a href='<%# "Reply.aspx?CID=" + Eval("VCid") %>'
                                                                            onclick="return hs.htmlExpand(this, { objectType: 'iframe', width: 470, height: 260, marginTop: 0 })">

                                                                            <asp:Label ID="LBModify" runat="server" Text="View Detail" />
                                                                        </a>--%>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tbody>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <%-- <asp:AsyncPostBackTrigger ControlID="CmdSave" EventName="Click" />--%>
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <div class="pagination-controls">
                                            <asp:LinkButton ID="lnkPrev" runat="server" Text="<< Prev" OnClick="lnkPrev_Click" />
                                            &nbsp;|&nbsp;
                                            <asp:LinkButton ID="lnkNext" runat="server" Text="Next >>" OnClick="lnkNext_Click" />
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
    <div class="modal fade" id="replyModal" tabindex="-1">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title">View Detail</h5>
                    <button type="button"
                        class="btn-close"
                        data-bs-dismiss="modal">
                    </button>
                </div>

                <div class="modal-body p-0">
                    <iframe id="replyFrame"
                        style="width: 100%; height: 260px; border: none;"></iframe>
                </div>

            </div>
        </div>
    </div>

    <script>
        function openReplyModal(vcid) {

            var frame = document.getElementById("replyFrame");

            // reset iframe
            frame.src = "";

            // load page
            frame.src = "Reply.aspx?CID=" + vcid;

            // open modal
            var modal = new bootstrap.Modal(
                document.getElementById("replyModal")
            );
            modal.show();
        }
    </script>

</asp:Content>
