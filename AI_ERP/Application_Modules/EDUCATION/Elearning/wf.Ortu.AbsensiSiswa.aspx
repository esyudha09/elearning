<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.Ortu.AbsensiSiswa.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Elearning.wf_AbsensiSiswa" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function EndRequestHandler() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        }

        function EndRequest() {
            InitPicker();
            ResizeIFrame();
        }

        function InitPicker() {
            $('#<%= txtDariTanggal.ClientID %>').pickdate({ cancel: '', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });
            $('#<%= txtSampaiTanggal.ClientID %>').pickdate({ cancel: '', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });
        }

        function ShowProses(show, triggered_by) {
            var div = document.getElementById("div_proses");
            if (div !== null && div !== undefined) {
                if (triggered_by !== null && triggered_by !== undefined) {
                    triggered_by.style.display = (show === false ? "" : "none");
                }
                else {
                    var btn = document.getElementById("btnProsesDownload");
                    if (btn !== null && btn !== undefined && show === false) {
                        btn.style.display = "";

                        $('#ui_modal_presensi_siswa').modal({ backdrop: 'static', keyboard: false, show: true });
                        ResizeIFrame();
                    }
                }
                div.style.display = (show === true ? "" : "none");
            }
        }

        function DoProsesListAbsen() {
            var periode = document.getElementById("<%= cboTahunAjaran.ClientID %>").value;
            var jenis_presensi = document.getElementById("<%= cboJenisAbsensi.ClientID %>").value;
            var dari_tanggal = document.getElementById("<%= txtDariTanggal.ClientID %>").value;
            var sampai_tanggal = document.getElementById("<%= txtSampaiTanggal.ClientID %>").value;
            var jenis_laporan = document.getElementById("<%= cboJenisLaporan.ClientID %>").value;
            var matpel = document.getElementById("<%= cboMatpel.ClientID %>").value;
            var siswa = document.getElementById("<%= cboSiswa.ClientID %>").value;
            var kelasdet = document.getElementById("<%= txtKD.ClientID %>").value;

            var frm = document.getElementById("frm_presensi");
            if(frm !== null && frm !== undefined){
                frm.src = "<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.LIST_ABSENSI_SISWA.ROUTE) %>?" +
                            "p=" + periode + "&" +
                            "kd=" + kelasdet + "&" +
                            "jp=" + jenis_presensi + "&" +
                            "dt=" + dari_tanggal + "&" +
                            "st=" + sampai_tanggal + "&" +
                            "jl=" + jenis_laporan + "&" +
                            "m=" + matpel + "&" +
                            "sw=" + siswa;   
            }
        }

        function ResizeIFrame() {
            setInterval(
                function () {
                    var frm = document.getElementById("frm_presensi");
                    var div = document.getElementById("div_konten_presensi");
                    if (frm !== null && frm !== undefined) {
                        //if (frm.style.height !== frm.contentWindow.document.body.scrollHeight + 'px') {
                            //frm.style.height = (frm.contentWindow.document.body.scrollHeight + 30) + 'px';
                            //frm.style.height = '1000px';
                            //frm.style.height = '87%';
                        //}
                        frm.style.height = '87%';
                    }
                }, 300
            );
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField runat="server" ID="txtKD" />
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
                                
                                <div runat="server" id="div_label_kelas" class="row" style="margin-bottom: 13px;">
                                    <div class="col-xs-12">
                                        <h1 style="margin-top: 0px;">Kelas <asp:Literal runat="server" ID="ltrLabelKelas"></asp:Literal></h1>
                                    </div>
                                </div>
                                <div class="row" style="margin-bottom: 13px;">
                                    <div class="col-xs-3">
                                        <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                    <label class="label-input" for="<%= cboTahunAjaran.ClientID %>" style="text-transform: none;">Periode</label>
                                                    <asp:DropDownList runat="server" ID="cboTahunAjaran" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-3">
                                        <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                    <label class="label-input" for="<%= cboJenisAbsensi.ClientID %>" style="text-transform: none;">Jenis Presensi</label>
                                                    <asp:DropDownList runat="server" ID="cboJenisAbsensi" CssClass="form-control">
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
                                                    <asp:DropDownList runat="server" ID="cboJenisLaporan" CssClass="form-control">
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
                                                    <label class="label-input" for="<%= cboMatpel.ClientID %>" style="text-transform: none;">Mata Pelajaran</label>
                                                    <asp:DropDownList runat="server" ID="cboMatpel" CssClass="form-control">
                                                        <asp:ListItem Value="0" Text="Semua"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="margin-bottom: 13px;">
                                    <div class="col-xs-12">
                                        <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                    <label class="label-input" for="<%= cboSiswa.ClientID %>" style="text-transform: none;">Siswa</label>
                                                    <asp:DropDownList runat="server" ID="cboSiswa" CssClass="form-control">
                                                        <asp:ListItem Value="0" Text="Semua"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="margin-bottom: 13px;">
                                    <div class="col-xs-12">
                                        <button id="btnProsesDownload" onclick="ShowProses(true, this); DoProsesListAbsen(); return false;" class="btn btn-flat btn-brand-accent waves-attach waves-effect" style="border-style: solid; border-color: dodgerblue; border-width: 1px; background-color: dodgerblue; color: white; float: right;">&nbsp;&nbsp;<span style="color: white;">Lihat Data</span>&nbsp;&nbsp;</button>
                                    </div>
                                </div>

                                <div style="display: none;">
                                    <asp:Literal runat="server" ID="ltrAbsensi"></asp:Literal>
                                </div>
                                <div id="div_proses" style="margin: 0 auto; display: table; display: none;">
                                    <img src="<%= ResolveUrl("~/Application_CLibs/images/loading-animation.gif?t=") + Guid.NewGuid().ToString() %>"  style="margin: 0 auto; display: table; height: 70px; width: 70px;" />
                                </div>

                            </div>
                        </div>

					</div>
				</div>
			</div>

        </div>
    </div>

    <div aria-hidden="true" class="modal fade" id="ui_modal_presensi_siswa" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

        <label title=" Tutup " data-dismiss="modal" style="padding: 10px; padding-top: 6px; padding-bottom: 6px; background-color: white; cursor: pointer; position: fixed; right: 35px; top: 20px; z-index: 999999; border-radius: 100%;">
            <i class="fa fa-times" style="color: black; font-size: large; font-weight: normal;"></i>
        </label>

        <div class="modal-dialog" style="width: 98%; height: 95%;">
            <div id="div_konten_presensi" class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px; height: 95%;">
                <div class="modal-inner"
                    style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                    <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                        <div class="media-object margin-right-sm pull-left">
                            <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                        </div>
                        <div class="media-inner">
                            <span style="font-weight: bold;">
                                Laporan Presensi Siswa
                            </span>
                        </div>
                    </div>
                    <div style="width: 100%;">
                        <div class="row" style="margin-left: -15px; margin-right: -14px;">
                            <div class="col-lg-12">

                                <div style="width: 100%; background-color: white; padding-top: 15px;">
                                    <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                        <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                            <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">

                                                    <div id="div_frm_presensi">
                                                        <iframe src="" id="frm_presensi" frameborder="0" scrolling="auto" style="background-color: white; width: 100%; position: fixed; bottom: 10px; top: 70px;"></iframe>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer" style="display: none;">
                    <p class="text-center">
                        <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Tutup</a>
                        <br />
                        <br />
                    </p>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
        InitPicker();
        ResizeIFrame();
    </script>
</asp:Content>
