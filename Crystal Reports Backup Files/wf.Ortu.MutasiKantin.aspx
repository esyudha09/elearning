<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.Ortu.MutasiKantin.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Elearning.wf_Ortu_MutasiKantin" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function EndRequestHandler() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        }

        function EndRequest() {
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row" style="margin-left: 15px; margin-right: 15px;">
        <div class="col-xs-12">

            <div class="card" style="margin: 0 auto; display: table; width: 70%; min-width: 300px; max-width: 1400px; margin-top: 40px;">
				<div class="card-main">
					<div class="card-inner" style="margin-top: 5px;">
                        <div class="row">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="background-color: white; padding: 10px; font-weight: normal; vertical-align: middle; color: grey; padding-left: 20px; font-size: large;">
                                        <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/009-shop.svg") %>" 
                                            style="margin: 0 auto; height: 25px; width: 25px;" />
                                        &nbsp;
                                        Transaksi Kantin
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color: white; padding: 0px; padding-top: 2px; padding-bottom: 2px;">
                                        <hr style="margin : 0px;" />
                                    </td>
                                </tr>
                            </table>
                        </div>

						<div class="row">

                            <div class="col-md-12" style="padding-top: 15px; color: black;">
                                
                                <div style="padding: 10px; padding-left: 20px; background-color: #D4E5C3; border-left-style: solid;  border-left-width: 3px; border-left-color: green;">
                                    <span>Saldo Uang Kantin</span>
                                    <br />
                                    <%= DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") %>
                                    <br /><br />
                                    <span style="font-size: xx-large; font-weight: bold;">
                                        Rp.
                                        &nbsp;
                                        <asp:Label runat="server" ID="lblSaldoUangKantin"></asp:Label>
                                    </span>
                                </div>

                                <br />
                                <asp:Literal runat="server" ID="ltrListTransaksiKantin"></asp:Literal>

                            </div>
                        </div>

					</div>
				</div>
			</div>

        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
</asp:Content>
