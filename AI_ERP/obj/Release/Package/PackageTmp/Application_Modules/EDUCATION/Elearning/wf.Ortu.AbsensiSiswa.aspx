<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.Ortu.AbsensiSiswa.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Elearning.wf_AbsensiSiswa" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function EndRequestHandler() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        }

        function EndRequest() {
            InitPicker();
        }

        function InitPicker() {
            $('#<%= txtDariTanggal.ClientID %>').pickdate({ cancel: '', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });
            $('#<%= txtSampaiTanggal.ClientID %>').pickdate({ cancel: '', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });
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
                                        <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/placeholder.svg") %>" 
                                            style="margin: 0 auto; height: 25px; width: 25px;" />
                                        &nbsp;
                                        Presensi Siswa
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

                            <div class="col-md-12" style="padding-top: 15px;">
                                
                                <div class="row" style="margin-bottom: 13px;">
                                    <div class="col-xs-3">
                                        <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                    <label class="label-input" for="<%= cboTahunAjaran.ClientID %>" style="text-transform: none;">Periode</label>
                                                    <asp:DropDownList runat="server" ID="cboTahunAjaran" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-3">
                                        <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                    <label class="label-input" for="<%= cboJenisAbsensi.ClientID %>" style="text-transform: none;">Jenis Presensi</label>
                                                    <asp:DropDownList runat="server" ID="cboJenisAbsensi" CssClass="form-control" AutoPostBack="true">
                                                        <asp:ListItem Value="0" Text="Wali Kelas"></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Mata Pelajaran"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="Gabungan"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-3">
                                        <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                    <label class="label-input" for="<%= txtDariTanggal.ClientID %>" style="text-transform: none;">Dari Tanggal</label>
                                                    <asp:TextBox runat="server" ID="txtDariTanggal" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-3">
                                        <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                    <label class="label-input" for="<%= txtSampaiTanggal.ClientID %>" style="text-transform: none;">Sampai Tanggal</label>
                                                    <asp:TextBox runat="server" ID="txtSampaiTanggal" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="margin-bottom: 13px;">
                                    <div class="col-xs-6">
                                        <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                    <label class="label-input" for="<%= cboJenisLaporan.ClientID %>" style="text-transform: none;">Jenis Laporan</label>
                                                    <asp:DropDownList runat="server" ID="cboJenisLaporan" CssClass="form-control" AutoPostBack="true">
                                                        <asp:ListItem Value="0" Text="Detail"></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Rekap"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-6">
                                        <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                    <label class="label-input" for="<%= cboSiswa.ClientID %>" style="text-transform: none;">Siswa</label>
                                                    <asp:DropDownList runat="server" ID="cboSiswa" CssClass="form-control" AutoPostBack="true">
                                                        <asp:ListItem Value="0" Text="Semua"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <asp:Literal runat="server" ID="ltrAbsensi"></asp:Literal>

                            </div>
                        </div>

					</div>
				</div>
			</div>

        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
        InitPicker();
    </script>
</asp:Content>
