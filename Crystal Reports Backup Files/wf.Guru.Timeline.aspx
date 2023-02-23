<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.Guru.Timeline.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Elearning.wf_Guru_Timeline" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        ::placeholder {
            color: #bfbfbf;
            opacity: 1;
            font-weight: normal;
        }
    </style>
    <script type="text/javascript">
        function HideModal() {
            $('#ui_modal_tanggal_absen').modal('hide');
            $('#ui_modal_absensi').modal('hide');
            $('#ui_modal_absensi_lts').modal('hide');            
            $('#ui_modal_confirm_hapus_absen').modal('hide');   
            $('#ui_modal_rekap_absen').modal('hide');               
            $('#ui_modal_proses').modal('hide');

            document.body.style.paddingRight = "0px";

            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();

            document.body.style.paddingRight = "0px";            
        }

        function ShowPengaturan(show) {
            var div_pengaturan = document.getElementById("<%= div_button_settings.ClientID %>");
            var div_periode = document.getElementById("<%= div_periode.ClientID %>");
            var div_panel_cari = document.getElementById("div_cari");

            if (
                div_pengaturan != null && div_pengaturan != undefined &&
                div_periode != null && div_periode != undefined
               ) {
                if (show) {
                    div_pengaturan.style.display = "";
                    div_periode.style.display = "";
                } else {
                    div_pengaturan.style.display = "none";
                    div_periode.style.display = "none";
                }
            }

            if (div_panel_cari != null && div_panel_cari != null) {
                if (show) {
                    //div_panel_cari.style.display = "";
                } else {
                    //div_panel_cari.style.display = "none";
                }
            }
        }

        function EndRequestHandler() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        }   

        function EndRequest() {
            var jenis_act = document.getElementById("<%= txtKeyAction.ClientID %>").value;
            switch (jenis_act) {
                case "<%= JenisAction.DoShowTanggalInputAbsen %>":
                    ShowProgress(false);
                    ShowPengaturan(false);
                    $('#ui_modal_tanggal_absen').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowInputAbsen %>":
                    ShowProgress(false);
                    ShowPengaturan(false);
                    $('#ui_modal_absensi').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowInputAbsenLTS %>":
                    ShowProgress(false);
                    ShowPengaturan(false);
                    $('#ui_modal_absensi_lts').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmHapusAbsen %>":
                    ShowPengaturan(false);
                    $('#ui_modal_confirm_hapus_absen').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowDownloadRekapAbsen %>":
                    ShowProgress(false);
                    $('#ui_modal_rekap_absen').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DataTidakBisaDibuka %>":
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data tidak bisa dibuka',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoDelete %>":
                    HideModal();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah dihapus',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoUpdate %>":
                    HideModal();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah diupdate',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                default:
                    HideModal();
                    if (jenis_act.trim() != ""){
                        $('body').snackbar({
                            alive: 4000,
                            content: '<i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;&nbsp;PERHATIAN : ' + jenis_act,
                            show: function () {
                                snackbarText++;
                            }
                        });
                    }
                    break;
            }

            document.getElementById("<%= txtKeyAction.ClientID %>").value = "";
            InitPicker();
            InitModalOpen();
        }

        function InitHideModal(){
            $('#ui_modal_absensi').on('hidden.bs.modal', function () {
                ShowPengaturan(true);
            });

            $('#ui_modal_absensi_lts').on('hidden.bs.modal', function () {
                ShowPengaturan(true);
            });

            $('#ui_modal_tanggal_absen').on('hidden.bs.modal', function () {
                ShowPengaturan(true);
            });
        }

        function ShowProgress(show){
            if(show){
                $('#ui_modal_proses').modal({ backdrop: 'static', keyboard: false, show: true });
            }
            else {
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                $('#ui_modal_proses').modal('hide');
            }
        }

        function getParameterByName(name) {
            var url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        }

        function ShowprosesAbsen(show) {
            var div_proses_absen = document.getElementById("div_proses_absen");
            var div_tanggal_buka_absen = document.getElementById("div_tanggal_buka_absen");
            var div_tanggal_buka_absen_jamke = document.getElementById("<%= div_tanggal_buka_absen_jamke.ClientID %>");            
            var div_list_siswa_absen = document.getElementById("div_list_siswa_absen");

            if (div_proses_absen != null && div_proses_absen != undefined) {
                if (div_tanggal_buka_absen != null && div_tanggal_buka_absen != undefined) {
                    if (div_list_siswa_absen != null && div_list_siswa_absen != undefined) {
                        div_proses_absen.style.display = (show ? "" : "none");
                        div_tanggal_buka_absen.style.display = (!show ? "" : "none");
                        if(div_tanggal_buka_absen_jamke != null && div_tanggal_buka_absen_jamke != undefined){
                            div_tanggal_buka_absen_jamke.style.display = (!show ? "" : "none");
                        }
                        div_list_siswa_absen.style.display = (!show ? "" : "none");
                    }
                }
            }
        }

        setInterval(
            function () {
                var txt_jumlah_proses_absen = document.getElementById("<%= txtJumlahProsesAbsen.ClientID %>");
                var txt_proses_absen = document.getElementById("<%= txtStatusSaveAbsen.ClientID %>");
                if (txt_jumlah_proses_absen != null && txt_jumlah_proses_absen != undefined) {
                    if (txt_proses_absen != null && txt_proses_absen != undefined) {

                        if (txt_proses_absen.value.trim() != "" && txt_jumlah_proses_absen.value.trim() != "") {

                            var arr_proses = txt_proses_absen.value.split(";");    
                            if (arr_proses.length > 0 && txt_jumlah_proses_absen.value.trim() != "") {
                                if (arr_proses.length > parseInt(txt_jumlah_proses_absen.value.trim())) {                                    
                                    ShowPengaturan(true);
                                    HideModal();
                                    txt_jumlah_proses_absen.value = "";
                                    txt_proses_absen.value = "";
                                    $('body').snackbar({
                                        alive: 2000,
                                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                                        show: function () {
                                            snackbarText++;
                                        }
                                    });
                                    <%= btnDoShowLinimasa.ClientID %>.click();
                                }
                            }

                        }

                    }
                }
            }, 
        500);

        function AddProsesAbsen(banyak_proses)
        {
            var txt_proses_absen = document.getElementById("<%= txtStatusSaveAbsen.ClientID %>");
            if (txt_proses_absen != null && txt_proses_absen != undefined) {
                txt_proses_absen.value += banyak_proses.toString() + ";";                
            }
        }

        function InitModalOpen(){
            $('#ui_modal_absensi').on('shown.bs.modal', function () {
                ShowprosesAbsen(false);
            });

            $('#ui_modal_absensi_lts').on('shown.bs.modal', function () {
                ShowprosesAbsen(false);
            });
        }

        function ProsesAbsen() {
            ShowprosesAbsen(true);

            var arr_absen = document.getElementsByName("cbo_absen[]");
            var arr_ket_absen = document.getElementsByName("txt_keterangan_absen[]");
            var arr_siswa_absen = document.getElementsByName("txt_siswa_absen[]");
            var arr_kejadian = document.getElementsByName("txt_kejadian[]");
            var arr_butir_sikap = document.getElementsByName("cbo_butir_sikap[]");
            var arr_butir_sikap_lain = document.getElementsByName("txt_butir_sikap_lain[]");
            var arr_sikap = document.getElementsByName("cbo_sikap[]");
            var arr_tindak_lanjut = document.getElementsByName("txt_tindak_lanjut[]");

            var txt_absen = document.getElementById("<%= txtParseAbsen.ClientID %>");
            var txt_id = document.getElementById("<%= txtKodeLinimasa.ClientID %>");
            var txt_tanggal = document.getElementById("<%= txtTanggalAbsenBuka.ClientID %>");            
            var txt_jumlah_proses_absen = document.getElementById("<%= txtJumlahProsesAbsen.ClientID %>");
            var txt_proses_absen = document.getElementById("<%= txtStatusSaveAbsen.ClientID %>");
            var cbo_jam_ke = document.getElementById("<%= cboJamKe.ClientID %>");

            var separator = "<%= AI_ERP.Application_Modules.EDUCATION.Elearning.wf_Guru_Timeline.SEPARATOR_ABSEN %>";

            if (txt_proses_absen != null && txt_proses_absen != undefined) {
                txt_proses_absen.value = "";
            }
            
            if (txt_absen != null && txt_absen != undefined && txt_id != null && txt_id != undefined) {
                txt_absen.value = "";
                if (
                    arr_absen.length > 0 && arr_ket_absen.length > 0 && arr_siswa_absen.length > 0 &&
                    arr_absen.length === arr_ket_absen.length && arr_absen.length === arr_siswa_absen.length
                ) {
                    if (txt_jumlah_proses_absen != null && txt_jumlah_proses_absen != undefined)
                    {
                        txt_jumlah_proses_absen.value = arr_absen.length.toString();
                    }

                    var jam_ke = "";
                    if(cbo_jam_ke != null && cbo_jam_ke != undefined){
                        jam_ke = cbo_jam_ke.value;
                    }
                    
                    for (var i = 0; i < arr_absen.length; i++) {
                        txt_absen.value += arr_siswa_absen[i] + "|" +
                                           arr_absen[i] + "|" +
                                           arr_ket_absen[i];
                        
                        var s_kejadian = (arr_kejadian.length > 0 ? (arr_kejadian.length > i ? arr_kejadian[i].value : "") : "");
                        var s_butir_sikap = (arr_butir_sikap.length > 0 ? (arr_butir_sikap.length > i ? arr_butir_sikap[i].value : "") : "");
                        var s_butir_sikap_lain = (arr_butir_sikap_lain.length > 0 ? (arr_butir_sikap_lain.length > i ? arr_butir_sikap_lain[i].value : "") : "");
                        var s_sikap = (arr_sikap.length > 0 ? (arr_sikap.length > i ? arr_sikap[i].value : "") : "");
                        var s_tindak_lanjut = (arr_tindak_lanjut.length > 0 ? (arr_tindak_lanjut.length > i ? arr_tindak_lanjut[i].value : "") : "");
                        
                        var s_url = '<%= ResolveUrl(
                                            AI_ERP.Application_Libs.Routing.URL.APIS._GENERAL.ABSENSI_SISWA.DO_SAVE.FILE
                                        ) %>/Do' + "?" +
                                    "sw=" + arr_siswa_absen[i].value +
                                    "&a=" + arr_absen[i].value +
                                    "&k=" + arr_ket_absen[i].value +
                                    "&t=" + getParameterByName("t") +
                                    "&s=" + getParameterByName("s") +
                                    "&kd=" + getParameterByName("kd") +
                                    "&tgl=" + txt_tanggal.value +
                                    "&lm=" + txt_id.value +
                                    "&m=<%= AI_ERP.Application_Libs.Libs.GetQueryString("m") %>" +                                     
                                    "&jk=" + jam_ke +
                                    "&kj=" + s_kejadian +
                                    "&bs=" + s_butir_sikap +
                                    "&bsl=" + s_butir_sikap_lain +                                    
                                    "&skp=" + (s_sikap === "+" ? "plus" : s_sikap) + 
                                    "&tl=" + s_tindak_lanjut + 
                                    "&ssid=<%= AI_ERP.Application_Modules.EDUCATION.Elearning.wf_Guru_Timeline.GetSSID() %>";
                                                
                        $.ajax({
                            url: s_url,
                            dataType: "json",
                            type: "GET",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                AddProsesAbsen(1);
                            },
                            error: function (response) {
                                //alert(response.responseText);
                            },
                            failure: function (response) {
                                //alert(response.responseText);
                            }
                        });
                    }
                }
            }
        }

        function InitPicker() {
            $('#<%= txtTanggalAbsen.ClientID %>').pickdate({ cancel: '', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });
            $('#<%= txtTanggalMulai.ClientID %>').pickdate({ cancel: '', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });
            $('#<%= txtTanggalAkhir.ClientID %>').pickdate({ cancel: '', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });
        }

        function ShowProsesLaporanRekapAbsen(show) {
            pb_proses_download_rekap_absen.style.display = (show ? "" : "none");
            div_button_download_rekap_absen.style.display = (show ? "none" : "");
            if (!show) {
                if (navigator.appName == 'Microsoft Internet Explorer') {
                    window.frames['fra_download'].document.execCommand('Stop');
                } else {
                    window.frames['fra_download'].stop();
                }
                HideModal();
            }
        }

        var txt_tanggal_absen1 = function () { return document.getElementById('<%= txtTanggalMulai.ClientID %>'); }
        var txt_tanggal_absen2 = function () { return document.getElementById('<%= txtTanggalAkhir.ClientID %>'); }
        var txt_kelas = function () { return document.getElementById('<%= txtRel_KelasDet.ClientID %>'); }        
        var txt_mapel = function () { return document.getElementById('<%= txtMapel.ClientID %>'); }
        var txt_tahun_ajaran = function () { return document.getElementById('<%= txtTahunAjaran.ClientID %>'); }
        
        function ReportProcessAbsen(){
            var url = "<%= ResolveUrl("~/Application_Resources/Download.aspx") %>";
            var cbo_jenis = document.getElementById("<%= cboJenisLaporan.ClientID %>");
            if(cbo_jenis !== undefined && cbo_jenis !== null){
                if(cbo_jenis.value == "0"){
                    ReportProcessRekapAbsen(url);
                } else if(cbo_jenis.value == "1"){
                    ReportProcessDetailAbsen(url);
                }
            }
        }

        function ReportProcessRekapAbsen(s_url)
        {
            var url = s_url;
            url += "?<%= AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY %>=<%= AI_ERP.Application_Libs.Downloads.JenisDownload.REKAP_ABSENSI_SISWA %>";
            url += "&jl=0";
            url += "&tgl1=" + txt_tanggal_absen1().value;
            url += "&tgl2=" + txt_tanggal_absen2().value;
            url += "&kd=" + txt_kelas().value;
            url += "&m=" + txt_mapel().value;
            url += "&t=" + txt_tahun_ajaran().value;
            
            if (navigator.appName == 'Microsoft Internet Explorer') {
                window.frames['fra_download'].document.location.href = url;
            } else {
                window.frames['fra_download'].location.href = url;
            }
        }

        function ReportProcessDetailAbsen(s_url)
        {
            var url = s_url;
            url += "?<%= AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY %>=<%= AI_ERP.Application_Libs.Downloads.JenisDownload.REKAP_ABSENSI_SISWA %>";
            url += "&jl=1";
            url += "&tgl1=" + txt_tanggal_absen1().value;
            url += "&tgl2=" + txt_tanggal_absen2().value;
            url += "&kd=" + txt_kelas().value;
            url += "&m=" + txt_mapel().value;
            url += "&t=" + txt_tahun_ajaran().value;
            
            if (navigator.appName == 'Microsoft Internet Explorer') {
                window.frames['fra_download'].document.location.href = url;
            } else {
                window.frames['fra_download'].location.href = url;
            }
        }

        function ShowProsesDownload(show) {
            if (!show) {
                if (navigator.appName == 'Microsoft Internet Explorer') {
                    window.frames['fra_download'].document.execCommand('Stop');
                } else {
                    window.frames['fra_download'].stop();
                }
                HideModal();
            }
        }

        function ResponseRedirect(url) {
            document.location.href = url;
        }

        function ParseAbsenLTS(){
            var arr_lts_siswa = document.getElementsByName("txt_siswa_absen_lts[]");
            var arr_lts_sakit = document.getElementsByName("txt_sakit_lts[]");
            var arr_lts_izin = document.getElementsByName("txt_izin_lts[]");
            var arr_lts_alpa = document.getElementsByName("txt_alpa_lts[]");
            var txt_parse_absen_lts = document.getElementById("<%= txtParseAbsenLTS.ClientID %>");

            if(
                arr_lts_siswa.length > 0 &&
                arr_lts_sakit.length === arr_lts_siswa.length &&
                arr_lts_izin.length === arr_lts_siswa.length &&
                arr_lts_alpa.length === arr_lts_siswa.length &&
                txt_parse_absen_lts !== null && 
                txt_parse_absen_lts !== undefined
            ){
                for (var i = 0; i < arr_lts_sakit.length; i++) {
                    txt_parse_absen_lts.value 
                            += arr_lts_siswa[i].value + "|" +
                               arr_lts_sakit[i].value + "|" +
                               arr_lts_izin[i].value + "|" +
                               arr_lts_alpa[i].value + ";";
                }
            }
        }

        function ParseAbsenRapor(){
            var arr_lts_siswa = document.getElementsByName("txt_siswa_absen_rapor[]");
            var arr_lts_sakit = document.getElementsByName("txt_sakit_rapor[]");
            var arr_lts_izin = document.getElementsByName("txt_izin_rapor[]");
            var arr_lts_alpa = document.getElementsByName("txt_alpa_rapor[]");
            var txt_parse_absen_lts = document.getElementById("<%= txtParseAbsenLTS.ClientID %>");

            if(
                arr_lts_siswa.length > 0 &&
                arr_lts_sakit.length === arr_lts_siswa.length &&
                arr_lts_izin.length === arr_lts_siswa.length &&
                arr_lts_alpa.length === arr_lts_siswa.length &&
                txt_parse_absen_lts !== null && 
                txt_parse_absen_lts !== undefined
            ){
                for (var i = 0; i < arr_lts_sakit.length; i++) {
                    txt_parse_absen_lts.value 
                            += arr_lts_siswa[i].value + "|" +
                               arr_lts_sakit[i].value + "|" +
                               arr_lts_izin[i].value + "|" +
                               arr_lts_alpa[i].value + ";";
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="txtKeyAction" />
            <asp:HiddenField runat="server" ID="txtParseAbsen" />
            <asp:HiddenField runat="server" ID="txtStatusSaveAbsen" />
            <asp:HiddenField runat="server" ID="txtJumlahProsesAbsen" />
            <asp:HiddenField runat="server" ID="txtKodeLinimasa" />
            <asp:HiddenField runat="server" ID="txtID" />
            <asp:HiddenField runat="server" ID="txtRel_KelasDet" />
            <asp:HiddenField runat="server" ID="txtMapel" />
            <asp:HiddenField runat="server" ID="txtTahunAjaran" />
            <asp:HiddenField runat="server" ID="txtParseAbsenLTS" />
            <asp:HiddenField runat="server" ID="txtJenisAbsen" />

            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoShowAbsen" OnClick="btnDoShowAbsen_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoShowAbsenByLinimasa" OnClientClick="ShowProgress(true);" OnClick="btnDoShowAbsenByLinimasa_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoShowLinimasa" OnClick="btnDoShowLinimasa_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoShowConfirmDeleteAbsen" OnClick="btnDoShowConfirmDeleteAbsen_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoSaveAbsenLTS" OnClick="btnDoSaveAbsenLTS_Click" style="position: absolute; left: -1000px; top: -1000px;" />

            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-12">

                    <div class="col-md-6 col-md-offset-3" style="padding: 0px;">
                        <div class="card" style="margin-top: 0px; box-shadow: none; border-style: solid; border-width: 1px; border-color: #dddddd; box-shadow: none;">
				            <div class="card-main">
					            <div class="card-inner" style="margin: 0px; padding: 0px;">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="background-color: ghostwhite; padding: 10px; font-weight: bold; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;
                                                       <asp:Literal runat="server" ID="ltrBGHeader"></asp:Literal>">                                                
                                                <asp:Literal runat="server" ID="ltrCaptionTimeLine"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="background-color: white; padding: 0px;">
                                                <hr style="margin: 0px; border-style: solid; border-width: 1px; border-color: white;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="background-color: white; padding: 0px;">
                                                <asp:Literal runat="server" ID="ltrLiniMasa"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="fbtn-container" id="div_button_settings" runat="server" style="z-index: 91;">
		        <div class="fbtn-inner">
			        <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #329CC3;" title=" Pengaturan ">
                        <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                        <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                    </a>
                    <div class="fbtn-dropup" style=" z-index: 999999;">
                        <asp:LinkButton CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkPilihKelas" OnClick="lnkPilihKelas_Click" style="display: none; background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Pilih Kelas Lain</span>
                            <i class="fa fa-list" style="color: white;"></i>
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkTampilanData" OnClick="lnkTampilanData_Click" style="display: none; background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Tampilan Data</span>
                            <i class="fa fa-desktop" style="color: white;"></i>
                        </asp:LinkButton>
                        <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkOKShowDownloadAbsen" OnClick="lnkOKShowDownloadAbsen_Click" style="background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Download Laporan Absen</span>
                            <i class="fa fa-file-excel-o" style="color: white;"></i>
                        </asp:LinkButton>
                        <asp:LinkButton Visible="false" OnClientClick="ShowProgress(true);" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkAbsenSiswaLTS" OnClick="lnkAbsenSiswaLTS_Click" style="background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Absensi Siswa (LTS)</span>
                            <i class="fa fa-calendar-check-o" style="color: white;"></i>
                        </asp:LinkButton>
                        <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkAbsenSiswaRapor" OnClick="lnkAbsenSiswaRapor_Click" style="background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Absensi Siswa (Rapor)</span>
                            <i class="fa fa-calendar-check-o" style="color: white;"></i>
                        </asp:LinkButton>
                        <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkAbsenSiswa" OnClick="lnkAbsenSiswa_Click" style="background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Absensi Siswa</span>
                            <i class="fa fa-id-card-o" style="color: white;"></i>
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkOKBuatPengumuman" OnClick="lnkOKBuatPengumuman_Click" style="background-color: #424242; display: none;">
                            <span class="fbtn-text fbtn-text-left">Buat Pengumuman</span>
                            <i class="fa fa-commenting-o" style="color: white;"></i>
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkOKBuatTugas" OnClick="lnkOKBuatTugas_Click" style="background-color: #424242; display: none;">
                            <span class="fbtn-text fbtn-text-left">Buat Tugas</span>
                            <i class="fa fa-check-square-o" style="color: white;"></i>
                        </asp:LinkButton>
                    </div>
		        </div>
	        </div>

            <div id="div_periode" runat="server" class="content-header ui-content-header" 
                style="background-color: white;
                        box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
                        background-image: none; 
                        color: white;
                        display: block;
                        z-index: 5;
                        position: fixed; bottom: 32px; right: 25px; width: 300px; border-radius: 25px;
                        padding: 8px; margin: 0px;">
                	
                <div style="padding-left: 0px; color: black;">
                    <asp:Literal runat="server" ID="ltrPeriode"></asp:Literal> 
                </div>
            </div>

            <div id="div_cari" style="display: none; position: fixed; right: 25px; bottom: 31px; width: 230px; border-color: #cecece; border-width: 1px; padding: 5px; border-style: solid; background-color: white; z-index: 90; border-radius: 30px; padding-left: 15px;">
                <table style="margin: 0px; width: 100%;">
                    <tr>
                        <td style="width: 20px; padding: 5px; background-color: white; padding-right: 0px;">
                            <i class="fa fa-search"></i>
                        </td>
                        <td style="background-color: white; padding: 5px; padding-left: 10px; padding-right: 0px;">
                            <asp:TextBox placeholder="Cari data..." runat="server" ID="txtCari" style="padding: 0px; border-style: none; color: grey; outline: none; width: 100%;"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_absensi" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

                <label onclick="setTimeout(function() { ShowPengaturan(true); }, 500);" title=" Tutup " data-dismiss="modal" style="padding: 10px; padding-top: 6px; padding-bottom: 6px; background-color: white; cursor: pointer; position: fixed; right: 35px; top: 20px; z-index: 999999; border-radius: 100%;">
                    <i class="fa fa-times" style="color: black; font-size: large; font-weight: normal;"></i>
                </label>

		        <div class="modal-dialog" style="width: 60%; min-width: 300px;">
			        <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
				        <div class="modal-inner" 
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; 
                            padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; 
                            border-top-left-radius: 5px;
                            border-top-right-radius: 5px;
                            background-color: #EDEDED;
                            background-repeat: no-repeat;
                            background-size: auto;
                            background-position: right; 
                            background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
							    <div class="media-object margin-right-sm pull-left">
								    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
							    </div>
							    <div class="media-inner">
								    <span style="font-weight: bold;">
                                        Absensi Siswa
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">

                                <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                <div class="col-xs-12">

                                                    <div id="div_proses_absen" style="display: none; background-color: white; height: 150px; width: 100%; z-index: 9000; padding-top: 20px;">
                                                        <img src="<%= ResolveUrl("~/Application_CLibs/images/giphy.gif") %>" style="margin: 0 auto; display: table; height: 60px; width: 70px;" />
                                                        <label style="margin: 0 auto; display: table; color: grey; font-size: medium; font-weight: bold; margin-top: 20px;">Sedang Proses...</label>
                                                    </div>

                                                    <div id="div_tanggal_buka_absen" class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-12" style="color: grey;">
                                                            
                                                            &nbsp;
                                                            <i class="fa fa-calendar"></i>
                                                            &nbsp;
                                                            <label for="<%= txtTanggalAbsenBuka.ClientID %>" style="font-size: small; color: grey; margin-bottom: 10px;">Tanggal Absen</label>
                                                            &nbsp;
                                                            <asp:TextBox Enabled="false" runat="server" ID="txtTanggalAbsenBuka" CssClass="text-input" style="background-color: #F5F8FA; font-weight: bold; width: 100%;"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div id="div_tanggal_buka_absen_jamke" runat="server" class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-12" style="color: grey;">
                                                            
                                                            &nbsp;
                                                            <i class="fa fa-clock-o"></i>
                                                            &nbsp;
                                                            <label for="<%= cboJamKe.ClientID %>" style="font-size: small; color: grey; margin-bottom: 10px;">Jam Ke</label>
                                                            <asp:DropDownList runat="server" ID="cboJamKe" CssClass="text-input" style="background-color: #F5F8FA; font-weight: bold; width: 100%;">
                                                                <asp:ListItem Value=""></asp:ListItem>
                                                                <asp:ListItem Value="1-2"></asp:ListItem>
                                                                <asp:ListItem Value="3-4"></asp:ListItem>
                                                                <asp:ListItem Value="5-6"></asp:ListItem>
                                                                <asp:ListItem Value="7-8"></asp:ListItem>
                                                                <asp:ListItem Value="9-10"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div id="div_list_siswa_absen" class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-12" style="color: grey;">
                                                            
                                                            <asp:Literal runat="server" ID="ltrListSiswaAbsen"></asp:Literal>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
									    </div>  

                                    </div>
                                </div>								                            
						    </div>
				        </div>
                        <div class="modal-footer">
                            &nbsp;
                        </div>
			        </div>
		        </div>

                <div class="fbtn-container" id="div_footer_dimpan_absen" runat="server" style="position: fixed; right: 25px; bottom: 5px;">
		            <div class="fbtn-inner">
			            <a onclick="ProsesAbsen(); return false;" class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #329CC3;" title=" Simpan ">
                            <span class="fbtn-ori icon"><span class="fa fa-check"></span></span>
                            <span class="fbtn-sub icon"><span class="fa fa-check"></span></span>
                        </a>
		            </div>
	            </div>

                <div class="content-header ui-content-header" 
					style="background-color: white;
							box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
							background-image: none; 
							color: white;
							display: block;
							position: fixed; bottom: 25px; right: 35px; width: 100px; border-radius: 25px;
							padding: 0px; margin: 0px;">
					<div style="padding-left: 15px;">
						<a onclick="setTimeout(function() { ShowPengaturan(true); }, 500);" class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal" style="margin-left: 0px; margin-right: 0px; color: grey;">
							<i class="fa fa-times" style="color: grey;"></i>
						</a>
					</div>
				</div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_absensi_lts" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

                <label onclick="setTimeout(function() { ShowPengaturan(true); }, 500);" title=" Tutup " data-dismiss="modal" style="padding: 10px; padding-top: 6px; padding-bottom: 6px; background-color: white; cursor: pointer; position: fixed; right: 35px; top: 20px; z-index: 999999; border-radius: 100%;">
                    <i class="fa fa-times" style="color: black; font-size: large; font-weight: normal;"></i>
                </label>

		        <div class="modal-dialog" style="width: 60%; min-width: 300px;">
			        <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
				        <div class="modal-inner" 
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; 
                            padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; 
                            border-top-left-radius: 5px;
                            border-top-right-radius: 5px;
                            background-color: #EDEDED;
                            background-repeat: no-repeat;
                            background-size: auto;
                            background-position: right; 
                            background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
							    <div class="media-object margin-right-sm pull-left">
								    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
							    </div>
							    <div class="media-inner">
								    <span style="font-weight: bold;">
                                        Absensi Siswa (LTS)
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">

                                <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                <div class="col-xs-12">

                                                    <div id="div_proses_absen_lts" style="display: none; background-color: white; height: 150px; width: 100%; z-index: 9000; padding-top: 20px;">
                                                        <img src="<%= ResolveUrl("~/Application_CLibs/images/giphy.gif") %>" style="margin: 0 auto; display: table; height: 60px; width: 70px;" />
                                                        <label style="margin: 0 auto; display: table; color: grey; font-size: medium; font-weight: bold; margin-top: 20px;">Sedang Proses...</label>
                                                    </div>
                                                    <div id="div_list_siswa_absen_lts" class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-12" style="color: grey;">
                                                            
                                                            <asp:Literal runat="server" ID="ltrListSiswaAbsenTS"></asp:Literal>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
									    </div>  

                                    </div>
                                </div>								                            
						    </div>
				        </div>
                        <div class="modal-footer">
                            &nbsp;
                        </div>
			        </div>
		        </div>

                <div class="fbtn-container" id="div_footer_dimpan_absen_lts" runat="server" style="position: fixed; right: 25px; bottom: 5px;">
		            <div class="fbtn-inner">
			            <a onclick="HideModal(); ShowProgress(true); ParseAbsenRapor(); setTimeout(function(){ <%= btnDoSaveAbsenLTS.ClientID %>.click(); }, 1000);" class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #329CC3;" title=" Simpan ">
                            <span class="fbtn-ori icon"><span class="fa fa-check"></span></span>
                            <span class="fbtn-sub icon"><span class="fa fa-check"></span></span>
                        </a>
		            </div>
	            </div>

                <div class="content-header ui-content-header" 
					style="background-color: white;
							box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
							background-image: none; 
							color: white;
							display: block;
							position: fixed; bottom: 25px; right: 35px; width: 100px; border-radius: 25px;
							padding: 0px; margin: 0px;">
					<div style="padding-left: 15px;">
						<a onclick="setTimeout(function() { ShowPengaturan(true); }, 500);" class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal" style="margin-left: 0px; margin-right: 0px; color: grey;">
							<i class="fa fa-times" style="color: grey;"></i>
						</a>
					</div>
				</div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_tanggal_absen" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
		        <div class="modal-dialog modal-xs">
			        <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
				        <div class="modal-inner" 
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; 
                            padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; 
                            border-top-left-radius: 5px;
                            border-top-right-radius: 5px;
                            background-color: #EDEDED;
                            background-repeat: no-repeat;
                            background-size: auto;
                            background-position: right; 
                            background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
							    <div class="media-object margin-right-sm pull-left">
								    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
							    </div>
							    <div class="media-inner">
								    <span style="font-weight: bold;">
                                        Buka Absen
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                <div class="col-xs-12">
                                                    <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-12" style="color: grey;">
                                                            &nbsp;
                                                            <i class="fa fa-calendar"></i>
                                                            &nbsp;
                                                            <label for="<%= txtTanggalAbsen.ClientID %>" style="font-size: small; color: grey; margin-bottom: 10px;">Tanggal Absen</label>
                                                            &nbsp;
                                                            <asp:TextBox runat="server" ID="txtTanggalAbsen" CssClass="text-input" style="background-color: #F5F8FA; font-weight: bold; width: 100%;"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
									    </div>  

                                    </div>
                                </div>								                            
						    </div>
				        </div>
				        <div class="modal-footer">
					        <p class="text-right">
                                <br /><br /><br /><br />
                                <button onclick="HideModal(); setTimeout(function(){ <%= btnDoShowAbsen.ClientID %>.click(); }, 100); ShowProgress(true); return false;" class="btn btn-flat btn-brand-accent waves-attach waves-effect" style="background-color: #329CC3; color: white; font-size: small;">
                                    &nbsp;&nbsp;
                                    <span style="color: white;">Buka</span>
                                    &nbsp;&nbsp;
                                </button>      
                                <a onclick="setTimeout(function() { ShowPengaturan(true); }, 500);" class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal" style="margin-left: 0px; margin-right: 0px;">
                                    <span style="color: grey; font-size: small;">
                                        Batal
                                    </span>                                    
                                </a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_confirm_hapus_absen" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
		        <div class="modal-dialog modal-xs">
			        <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
				        <div class="modal-inner" 
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; 
                            padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; 
                            border-top-left-radius: 5px;
                            border-top-right-radius: 5px;
                            background-color: #EDEDED;
                            background-repeat: no-repeat;
                            background-size: auto;
                            background-position: right; 
                            background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
							    <div class="media-object margin-right-sm pull-left">
								    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
							    </div>
							    <div class="media-inner">
								    <span style="font-weight: bold;">
                                        Konfirmasi
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-12">                                                            
                                                            <asp:Literal runat="server" ID="ltrMsgConfirmHapusAbsen"></asp:Literal>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
										</div>  

                                    </div>
                                </div>								                            
							</div>
				        </div>
				        <div class="modal-footer">
					        <p class="text-right">
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapusAbsen" OnClick="lnkOKHapusAbsen_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_periode_absen" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

                <label title=" Tutup " data-dismiss="modal" style="padding: 10px; padding-top: 6px; padding-bottom: 6px; background-color: white; cursor: pointer; position: fixed; right: 35px; top: 20px; z-index: 999999; border-radius: 100%;">
                    <i class="fa fa-times" style="color: black; font-size: large; font-weight: normal;"></i>
                </label>

                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">
                                        Periode Absensi
                                    </span>
                                </div>
                            </div>
                            <div style="width: 100%;">
                                <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-12">

                                                            <asp:Literal runat="server" ID="ltrPeriodeAbsen"></asp:Literal>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <hr style="margin: 0px; display: none;" />

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">                            
                            <p class="text-center">
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Tutup</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_rekap_absen" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

                <div class="modal-dialog modal-xs">
			        <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
				        <div class="modal-inner" 
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; 
                            padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; 
                            border-top-left-radius: 5px;
                            border-top-right-radius: 5px;
                            background-color: #EDEDED;
                            background-repeat: no-repeat;
                            background-size: auto;
                            background-position: right; 
                            background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
							    <div class="media-object margin-right-sm pull-left">
								    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
							    </div>
							    <div class="media-inner">
								    <span style="font-weight: bold;">
                                        Download Laporan Absen
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtTanggalMulai.ClientID %>" style="text-transform: none;">Jenis Laporan</label>
                                                        <asp:DropDownList ValidationGroup="vldDownloadRekapAbsen" runat="server" ID="cboJenisLaporan" CssClass="form-control">
                                                            <asp:ListItem Value="0" Text="Rekap"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Detail"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>     
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtTanggalMulai.ClientID %>" style="text-transform: none;">Tanggal Awal</label>
                                                        <asp:TextBox ValidationGroup="vldDownloadRekapAbsen" CssClass="form-control" runat="server" ID="txtTanggalMulai"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>     
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtTanggalAkhir.ClientID %>" style="text-transform: none;">Tanggal Akhir</label>
                                                        <asp:TextBox ValidationGroup="vldDownloadRekapAbsen" CssClass="form-control" runat="server" ID="txtTanggalAkhir"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>     
										</div>  

                                    </div>
                                </div>								                            
							</div>
				        </div>
				        <div class="modal-footer">
					        
                            <div style="width: 100%;">
							    <div class="row" id="pb_proses_download_rekap_absen" style="display: none; margin-left: -24px; margin-right: -24px; background-color: #B50000; color: white;">
                                    <div class="col-lg-12" style="padding-left: 0px; padding-right: 0px;">
                                        <div class="progress" style="margin-top: 0px;">
								            <div class="load-bar">
									            <div class="load-bar-base">
										            <div class="load-bar-content">
											            <div class="load-bar-progress"></div>
											            <div class="load-bar-progress load-bar-progress-brand"></div>
											            <div class="load-bar-progress load-bar-progress-green"></div>
											            <div class="load-bar-progress load-bar-progress-orange"></div>
										            </div>
									            </div>
								            </div>
								            <div class="load-bar">
									            <div class="load-bar-base">
										            <div class="load-bar-content">
											            <div class="load-bar-progress"></div>
											            <div class="load-bar-progress load-bar-progress-orange"></div>
											            <div class="load-bar-progress load-bar-progress-green"></div>
											            <div class="load-bar-progress load-bar-progress-brand"></div>
										            </div>
									            </div>
								            </div>
							            </div>
                                        <div style="margin: 0 auto; display: table; font-weight: bold;">
                                            Sedang proses tunggu beberapa saat...
                                            &nbsp;&nbsp;&nbsp;
                                            <br /><br />
                                        </div>
                                    </div>
                                </div>
                                <div class="row" id="div_button_download_rekap_absen">
                                    <div class="col-xs-12" style="padding: 15px; font-weight: bold; padding-left: 45px; padding-right: 45px;">
                                        <p class="text-right">
                                            <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" onclick="ShowProsesLaporanRekapAbsen(true); ReportProcessAbsen(); return false;">OK</a>
                                            <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Tutup</a>
					                    </p>
                                    </div>
                                </div>                                
                            </div>

				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_proses" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
			        <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
				        <div class="modal-inner" 
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; 
                            padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; 
                            border-top-left-radius: 5px;
                            border-top-right-radius: 5px;
                            background-color: #F68B1F; 
                            background-repeat: no-repeat;
                            background-size: auto;
                            background-position: right; 
                            background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: white; padding-bottom: 20px;">
							    <div class="media-object margin-right-sm pull-left">
								    <span class="icon icon-lg text-brand-accent" style="color: white;">info_outline</span>
							    </div>
							    <div class="media-inner">
								    <span style="font-weight: bold;">
                                        Proses
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%; display: none;">
							    <div class="row">
                                    <div class="col-lg-12">

                                    </div>
                                </div>								                            
							</div>
				        </div>
				        <div class="modal-footer">
					        
                            <div style="width: 100%;">
							    <div class="row" id="pb_proses" style="margin-left: -24px; margin-right: -24px; background-color: #F68B1F; color: white; border-bottom-left-radius: 5px; border-bottom-right-radius: 5px;">
                                    <div class="col-lg-12" style="padding-left: 0px; padding-right: 0px;">
                                        <div class="progress" style="margin-top: 0px; margin-left: 20px; margin-right: 20px;">
								            <div class="load-bar">
									            <div class="load-bar-base">
										            <div class="load-bar-content">
											            <div class="load-bar-progress"></div>
											            <div class="load-bar-progress load-bar-progress-brand"></div>
											            <div class="load-bar-progress load-bar-progress-green"></div>
											            <div class="load-bar-progress load-bar-progress-orange"></div>
										            </div>
									            </div>
								            </div>
								            <div class="load-bar">
									            <div class="load-bar-base">
										            <div class="load-bar-content">
											            <div class="load-bar-progress"></div>
											            <div class="load-bar-progress load-bar-progress-orange"></div>
											            <div class="load-bar-progress load-bar-progress-green"></div>
											            <div class="load-bar-progress load-bar-progress-brand"></div>
										            </div>
									            </div>
								            </div>
							            </div>
                                        <div style="margin: 0 auto; display: table; font-weight: bold;">
                                            Sedang proses tunggu beberapa saat...
                                            &nbsp;&nbsp;&nbsp;
                                            <br /><br />
                                        </div>
                                    </div>
                                </div>                          
                            </div>

				        </div>
			        </div>
		        </div>
	        </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <iframe name="fra_loader" id="fra_loader" height="0" width="0" style="position: absolute; left: -1000px; top: -1000px;"></iframe>
    <iframe name="fra_download" onloadedmetadata="alert('ok')" id="fra_download" height="0" width="0" style="position: absolute; left: -1000px; top: -1000px;"></iframe>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
        InitPicker();
        InitModalOpen();
	</script>
</asp:Content>
