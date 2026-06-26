<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="Rptwithdrawls.aspx.cs" Inherits="Rptwithdrawls" %>



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
                            <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>Fund Withdrawal Report</h5>
                        </div>
                        <div class="card-body">
                            <div>
                                <div class="clr">
                                    <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                </div>
                                <div class="row g-1" runat="server" id="DivH" visible="true">
                                 
                                    <div class="table-responsive">
                                        <div class="form-group " style="display: none">
                                            <asp:Button ID="BtnExportA" runat="server" Text="Export To Excel" class="btn btn-primary" Style="float: right;" />
                                        </div>
                                        <asp:Label ID="Label2" runat="server" Text="Total Records : "></asp:Label>
                                         <asp:Label ID="lbltotal" runat="server"></asp:Label>

                                        <table id="customers2" class="table datatable">
                                            <thead>
                                                <tr>
                                                    <th>S.No.
                                                    </th>
                                                    <th>Request Date
                                                    </th>
                                                    <th>Withdraw Amount
                                                    </th>
                                                    <th>Account No.
                                                    </th>
                                                    <th>Bank Name
                                                    </th>
                                                    <th>Bank Branch
                                                    </th>
                                                    <th>IFSC Code
                                                    </th>



                                                    <th>Approve Date
                                                    </th>
                                                    <th>Status
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="GrdDirects" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblRowNumber" Text='<%# Container.ItemIndex + 1 %>' runat="server" />
                                                            </td>
                                                            <%--<td>
                                                            <%#Eval("ReqID")%>
                                                        </td>--%>
                                                            <td>
                                                                <%#Eval("ReqDate")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("ReqAmount")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("AcNo")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("BankName")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("BranchName")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("IfsCode")%>
                                                            </td>



                                                            <td>
                                                                <%#Eval("IssueDate")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("Status")%>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>
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

