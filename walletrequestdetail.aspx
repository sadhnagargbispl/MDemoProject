<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="walletrequestdetail.aspx.cs" Inherits="walletrequestdetail" %>


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
                            <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>Wallet Request Detail</h5>
                        </div>
                        <div class="card-body">
                            <div>
                                <div class="clr">
                                    <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                </div>
                                <div class="row g-1" runat="server" id="DivH" visible="true">
                                    <asp:Label ID="Label1" runat="server" Text="Total Records"></asp:Label>
                                    <asp:Label ID="lbltotal" runat="server"></asp:Label>
                                    <div class="table-responsive">
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <table id="customers2" class="table datatable">
                                                    <thead>
                                                        <tr>
                                                            <th>Req. No
                                                            </th>
                                                            <th>Request Date
                                                            </th>
                                                            <th>Payment Mode
                                                            </th>
                                                            <th>Transaction No
                                                            </th>
                                                            <th>Transaction Date
                                                            </th>
                                                            <th>Amount
                                                            </th>
                                                            <th>Remark
                                                            </th>
                                                            <th>Admin Remark
                                                            </th>
                                                            <th>Status
                                                            </th>
                                                            <th>Image
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater ID="RptDirects" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <%# Eval("ReqNo") %>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("ReqDate") %>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("PayMode") %>
                                                                    </td>
                                                                    <td>
                                                                        <%# Eval("ChqNo") %>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("ChequeDate") %>
                                                                    </td>

                                                                    <td>
                                                                        <%# Eval("Amount") %>
                                                                    </td>
                                                                    <td>
                                                                        <%# Eval("Remarks") %>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("ApproveRemark")%>
                                                                    </td>
                                                                    <td>
                                                                        <%# Eval("Status") %>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Image
                                                                            ID="Image1"
                                                                            runat="server"
                                                                            ImageUrl='<%# Eval("ScannedFile") %>'
                                                                            Height="100px"
                                                                            Width="100px"
                                                                            Style="cursor: pointer"
                                                                            Visible='<%# Convert.ToBoolean(Eval("ScannedFileStatus")) %>'
                                                                            onclick='<%# "openPhotoModal(" + Eval("Reqno") + ")" %>' />
                                                                        <%-- <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("ScannedFile") %>' Height="100px" Width="100px"
                                                                            Visible='<%# Convert.ToBoolean(Eval("ScannedFileStatus")) %>' onclick="openPhotoModal()" />--%>
                                                                        <%-- <a href='<%# "Img.aspx?type=Payment&ID=" + Eval("Reqno") %>'
                                                                            onclick="return hs.htmlExpand(this, { objectType: 'iframe',width: 785,height: 350,marginTop : 0 });">
                                                                           
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
                                    </div>
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
        </section>
    </div>
    <div class="modal fade" id="profilePhotoModal" tabindex="-1">
        <div class="modal-dialog modal-dialog-centered modal-lg">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title">Image Preview</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body text-center">
                    <img id="photoPreview"
                        class="img-fluid"
                        style="max-height: 500px;" />
                </div>

            </div>
        </div>
    </div>
    <script>
        function openPhotoModal(reqno) {
         
            var modalEl = document.getElementById('profilePhotoModal');
            var img = document.getElementById("photoPreview");

            img.src = "";

            fetch("Img.aspx?type=Payment&ID=" + reqno)
                .then(response => response.text())
                .then(path => {

                    if (path.trim() !== "") {
                        img.src = path + "?t=" + new Date().getTime(); // cache bypass
                    }
                });

            var modal = new bootstrap.Modal(modalEl);
            modal.show();
        }
    </script>

</asp:Content>

