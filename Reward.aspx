<%@ Page Title="" Language="C#" MasterPageFile="~/SitePage.master" AutoEventWireup="true" CodeFile="Reward.aspx.cs" Inherits="Reward" %>

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
                         <h5 class="mb-0"><i class="fa fa-trophy me-2"></i>Reward</h5>
                     </div>
                     <div class="card-body">
                         <div class="row g-1">
                                <div class="clr">
                                    <asp:Label ID="errMsg" runat="server" CssClass="error"></asp:Label>
                                </div>
                 


                                <div id="DivSideA" runat="server" class="col-md-12">
  
                                <div id="Div1" runat="server" class="col-md-12">

                                    <div class="table-responsive">
                                         <asp:GridView ID="gv" runat="server" CssClass="table table-striped table-bordered"
     CellPadding="2" HorizontalAlign="Center" AutoGenerateColumns="true" Width="100%"
     EmptyDataText="No Data Display">
 </asp:GridView>
                                    </div>
                                </div>
                                        <%--<div id="cBAZAR" runat="server" visible="false">
                                           
                                        </div>--%>
                                    </div>
                                </div>



                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>

