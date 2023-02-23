<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="wf.FileRapor.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Penilaian.ALL.wf_FileRapor" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function HideModal(){
            $('#<%= mdl_confirm_buat_file_rapor.ClientID %>').modal('hide');            
            $('#ui_modal_input_data').modal('hide');
            $('#ui_modal_ubah_email').modal('hide');            
            $('#ui_modal_upload_file_rapor').modal('hide');
            $('#ui_modal_kenaikan_kelas').modal('hide');
            $('#ui_modal_email_rapor').modal('hide');
            $('#ui_modal_hist_email_rapor').modal('hide');
            
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();    
            
            document.body.style.paddingRight = "0px";
        }

        function EndRequestHandler() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        }

        function EndRequest() {
            var jenis_act = document.getElementById("<%= txtKeyAction.ClientID %>").value;

            switch (jenis_act) {
                case "<%= JenisAction.DoShowListFileRaporFromBack %>":
                    SetScrollPosFileRapor();
                    break;
                case "<%= JenisAction.DoShowListFileRaporAfterProses %>":
                    HideModal();
                    SetScrollPosFileRapor();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah diproses',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoShowShowFileRapor %>":
                    window.scrollTo(0,0); 
                    break;
                case "<%= JenisAction.DoShowListNilai %>":
                    SetScrollPos();
                    break;
                case "<%= JenisAction.DoShowUploadFileRapor %>":
                    $('#ui_modal_upload_file_rapor').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowUbahEmail %>":
                    $('#ui_modal_ubah_email').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowPengaturan %>":
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowKontenEmailRapor %>":
                    $('#ui_modal_email_rapor').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowListHistoryEmailRapor %>":
                    $('#ui_modal_hist_email_rapor').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.AddWithMessage %>":
                    HideModal();
                    ReInitTinyMCE();   
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoKirimEmail %>":
                    HideModal();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Email sedang dikirim...',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoRefresh %>":
                    HideModal();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah direfresh',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoShowData %>":
                    ReInitTinyMCE();   
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowListKenaikanKelas %>":
                    ShowPengaturan(false);
                    $('#ui_modal_kenaikan_kelas').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.Update %>":
                    HideModal();
                    break;
                case "<%= JenisAction.Delete %>":
                    HideModal();
                    break;
                case "<%= JenisAction.DoAdd %>":    
                    HideModal();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
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
                default:
                    HideModal();
                    if (jenis_act.trim() != ""){
                        $('body').snackbar({
                            alive: 6000,
                            content: '<i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;&nbsp;PERHATIAN : ' + jenis_act,
                            show: function () {
                                snackbarText++;
                            }
                        });
                    }
                    break;
            }
            
            RenderDropDownOnTables();
            InitModalFocus();
            document.getElementById("<%= txtKeyAction.ClientID %>").value = "";            
            
            Sys.Browser.WebKit = {};
            if (navigator.userAgent.indexOf('WebKit/') > -1) {
                Sys.Browser.agent = Sys.Browser.WebKit;
                Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
                Sys.Browser.name = 'WebKit';
            }
        }

        function ReInitTinyMCE(){
            LoadTinyMCENama();            
        }

        function InitModalFocus(){
            $('#ui_modal_input_data').on('shown.bs.modal', function () {
                document.getElementById("<%= cboPeriode.ClientID %>").focus();
            });

            $('#ui_modal_upload_file_rapor').on('shown.bs.modal', function () {
                var fra = document.getElementById("fraUploaderFileRapor");
                if (fra !== null && fra !== undefined) {
                    fra.src = "<%= ResolveUrl("~/Application_Resources/Uploader.aspx?jenis=file_rapor") %>" +
                              "&id=" + document.getElementById("<%= txtIDSiswa.ClientID %>").value +
                              "&id2=" + document.getElementById("<%= txtTahunAjaran.ClientID %>").value.replace("/", "_") + ";" +
                                        document.getElementById("<%= txtSemester.ClientID %>").value + ";" +
                                        document.getElementById("<%= txtKelasDet.ClientID %>").value +
                              "&tr=" + document.getElementById("<%= txtTipeRapor.ClientID %>").value;
                }
            });

            $('#<%= mdl_confirm_buat_file_rapor.ClientID %>').on('shown.bs.modal', function () {
                var txt_hal = document.getElementById("<%= txtHalamanRapor.ClientID %>");
                var div_halaman_rapor = document.getElementById("<%= div_halaman_rapor.ClientID %>");
                if(
                    txt_hal !== null && txt_hal !== undefined &&
                    div_halaman_rapor !== null && div_halaman_rapor !== undefined
                ){
                    if(div_halaman_rapor.style.display !== "none"){
                        txt_hal.focus();
                    }
                }                
            });
        }

        function TriggerSave(){
            tinyMCE.triggerSave();
        }

        function ShowProsesLaporanRapor(show) {
            pb_proses_download.style.display = (show ? "" : "none");
            div_button_proses.style.display = (show ? "none" : "");
            if (!show) {
                if (navigator.appName == 'Microsoft Internet Explorer') {
                    window.frames['fra_download'].document.execCommand('Stop');
                } else {
                    window.frames['fra_download'].stop();
                }
            }
        }

        function ReportProsesLaporanRapor() {
            var url = "<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.CREATE_REPORT.ROUTE) %>";
            url += "?<%= AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY %>=<%= AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SD %>";
            url += "&t=" + "";
            url += "&s=" + "";
            url += "&kd=" + "";

            if (navigator.appName == 'Microsoft Internet Explorer') {
                window.frames['fra_download'].document.location.href = url;
            } else {
                window.frames['fra_download'].location.href = url;
            }
        }

        function ShowPengaturan(show) {
            var div_pengaturan = document.getElementById("<%= div_button_settings.ClientID %>");
            var div_pager = document.getElementById("<%= div_pager.ClientID %>");
            var div_info_periode = document.getElementById("<%= div_info_periode.ClientID %>");

            if (
                div_pengaturan != null && div_pengaturan != undefined &&
                div_pager != null && div_pager != undefined &&
                div_info_periode != null && div_info_periode != undefined
               ) {
                if (show) {
                    div_pengaturan.style.display = "";
                    div_pager.style.display = "";
                    div_info_periode.style.display = "";
                } else {
                    div_pengaturan.style.display = "none";
                    div_pager.style.display = "none";
                    div_info_periode.style.display = "none";
                }
            }
        }

        function HideProseLaporan() {
            ShowProsesLaporanRapor(false);
        }

        function SetKenaikanKelas() {
            var txt = document.getElementById("<%= txtParseKenaikanKelas.ClientID %>");
            var arr_kenaikan_kelas = document.getElementsByName("cbo_kenaikan_kelas[]");
            var arr_siswa = document.getElementsByName("txt_siswa_kenaikan_kelas[]");
            if (txt !== null && txt !== undefined) {
                txt.value = "";
                if (arr_kenaikan_kelas.length > 0) {
                    for (var i = 0; i < arr_kenaikan_kelas.length; i++) {
                        txt.value += arr_kenaikan_kelas[i].value +
                                     "|" +
                                     arr_siswa[i].value +
                                     ";";
                    }
                }
            }
        }

        function DoCheckSiswaRapor(value) {
            var arr_siswa = document.getElementsByName("chk_pilih_siswa_rapor[]");
            if (arr_siswa.length > 0) {
                for (var i = 0; i < arr_siswa.length; i++) {
                    arr_siswa[i].checked = value;
                }
            }
        }

        function IsCheckedSiswaRapor() {
            var arr_siswa = document.getElementsByName("chk_pilih_siswa_rapor[]");
            if (arr_siswa.length > 0) {
                for (var i = 0; i < arr_siswa.length; i++) {
                    if(arr_siswa[i].checked) return true;
                }
            }

            $('body').snackbar({
                alive: 2000,
                content: '<i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;&nbsp;PERHATIAN : Siswa belum dipilih/diceklist.',
                show: function () {
                    snackbarText++;
                }
            });
            return false;
        }

        function DoCheckSiswaEmailRaporLTS(value) {
            var arr_siswa = document.getElementsByName("chk_pilih_siswa_email_rapor_lts[]");
            if (arr_siswa.length > 0) {
                for (var i = 0; i < arr_siswa.length; i++) {
                    arr_siswa[i].checked = value;
                }
            }
        }

        function GetCheckedSiswaRapor() {
            var arr_siswa = document.getElementsByName("chk_pilih_siswa_rapor[]");
            var s_hasil = "";
            if (arr_siswa.length > 0) {
                for (var i = 0; i < arr_siswa.length; i++) {
                    if (arr_siswa[i].checked) {
                        s_hasil += arr_siswa[i].value + ";";
                    }
                }
            }
            return s_hasil;
        }

        function ValidateCheckedSiswaRapor() {
            var arr_siswa = document.getElementsByName("chk_pilih_siswa_rapor[]");
            if (arr_siswa.length > 0) {
                for (var i = 0; i < arr_siswa.length; i++) {
                    if (arr_siswa[i].checked) {
                        return true;
                    }
                }
            }
            return false;
        }

        function ShowSBMessage(msg) {
            $('body').snackbar({
                alive: 2000,
                content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;' + msg,
                show: function () {
                    snackbarText++;
                }
            });
        }

        function GetCheckedSiswaRapor() {
            var arr_siswa = document.getElementsByName("chk_pilih_siswa_rapor[]");
            var s_hasil = "";
            if (arr_siswa.length > 0) {
                for (var i = 0; i < arr_siswa.length; i++) {
                    if (arr_siswa[i].checked) {
                        s_hasil += arr_siswa[i].value + ";";
                    }
                }
            }
            return s_hasil;
        }

        function DoScrollPos(){
            var txt_x = document.getElementById("<%= txtXpos.ClientID %>");
            var txt_y = document.getElementById("<%= txtYpos.ClientID %>");
            if(
                txt_x !== null && txt_x !== undefined &&
                txt_y !== null && txt_y !== undefined
              ){
                txt_x.value = window.pageXOffset;
                txt_y.value = window.pageYOffset;
            }
        }

        function SetScrollPos(){
            var txt_x = document.getElementById("<%= txtXpos.ClientID %>");
            var txt_y = document.getElementById("<%= txtYpos.ClientID %>");
            if(
                txt_x !== null && txt_x !== undefined &&
                txt_y !== null && txt_y !== undefined
              ) {
                window.scrollTo(txt_x.value,txt_y.value); 
            }
        }

        function DoScrollPosFileRapor() {
            var txt_x = document.getElementById("<%= txtXposFileRapor.ClientID %>");
            var txt_y = document.getElementById("<%= txtYposFileRapor.ClientID %>");
            if(
                txt_x !== null && txt_x !== undefined &&
                txt_y !== null && txt_y !== undefined
              ){
                txt_x.value = window.pageXOffset;
                txt_y.value = window.pageYOffset;
            }
        }

        function SetScrollPosFileRapor(){
            var txt_x = document.getElementById("<%= txtXposFileRapor.ClientID %>");
            var txt_y = document.getElementById("<%= txtYposFileRapor.ClientID %>");
            if(
                txt_x !== null && txt_x !== undefined &&
                txt_y !== null && txt_y !== undefined
              ) {
                window.scrollTo(txt_x.value,txt_y.value); 
            }
        }

        function RefreshListFileRapor() {
            document.getElementById("<%= btnBukaFileRaporDetAfterDeleted.ClientID %>").click();
        }

        function ShowProsesBuatRapor(show) {
            pb_proses_buat_file_rapor.style.display = (show ? "" : "none");
            div_button_buat_file_rapor.style.display = (show ? "none" : "");
            if (!show) {
                if (navigator.appName == 'Microsoft Internet Explorer') {
                    window.frames['fra_download'].document.execCommand('Stop');
                } else {
                    window.frames['fra_download'].stop();
                }
            }
        }

        function GetURLProsesRapor(){
            var url = <%= txtURLProsesRapor.ClientID %>.value;

            return url;
        }

        function ReportProcessBuatRapor() {
            var s_params = "";
            var arr_siswa = document.getElementsByName("chk_pilih_siswa_rapor[]");
            if (arr_siswa.length > 0) {
                for (var i = 0; i < arr_siswa.length; i++) {
                    if (arr_siswa[i].checked) {
                        s_params += arr_siswa[i].value + ";";
                    }
                }
            }

            var s_hal = "";
            var txt_hal = document.getElementById("<%= txtHalamanRapor.ClientID %>");
            if(txt_hal !== null && txt_hal !== undefined){
                s_hal = txt_hal.value;
            }

            var url = GetURLProsesRapor() + "?" +
                      "<%= AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY %>=<%= AI_ERP.Application_Libs.Downloads.JenisDownload.BUAT_FILE_RAPOR %>" +
                      "&sis=" + s_params +
                      "&t=" + <%= txtTahunAjaranEnc.ClientID %>.value.replace("/") +
                      "&s=" + <%= txtSemester.ClientID %>.value +
                      "&kd=" + <%= txtKelasDet.ClientID %>.value +
                      "&tr=" + <%= txtTipeRapor.ClientID %>.value +
                      (
                        s_hal.trim() !== "" ? "&hal=" + s_hal : ""
                      );
            
            if (navigator.appName == 'Microsoft Internet Explorer') {
                window.frames['fra_download'].document.location.href = url;
            } else {
                window.frames['fra_download'].location.href = url;
            }
        }

        function ShowProgress(value){
            return value;
        }

        function SetSelectedEmail(){
            var txt = document.getElementById("<%= txtIDSiswaList.ClientID %>");
            var arr_siswa = document.getElementsByName("chk_pilih_siswa_rapor[]");
            if(txt !== null && txt !== undefined){
                txt.value = "";
                if(arr_siswa.length > 0){
                    for (var i = 0; i < arr_siswa.length; i++) {
                        if(arr_siswa[i].checked){
                            txt.value += arr_siswa[i].value + ";";
                        }
                    }
                }
                if(txt.value.trim() !== ""){
                     setTimeout(function() { $('#<%= mdlConfirmEmailRapor.ClientID %>').modal('show'); }, 300);
                }else {
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;&nbsp;PERHATIAN : Siswa belum dipilih.',
                        show: function () {
                            snackbarText++;
                        }
                    });
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress runat="server" ID="upProgressMain" AssociatedUpdatePanelID="upMain">
        <ProgressTemplate>
            <ucl:PostbackUpdateProgress runat="server" ID="pbUpdateProgress" />
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel runat="server" ID="upMain">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="txtKeyAction" />
            <asp:HiddenField runat="server" ID="txtID" />
            <asp:HiddenField runat="server" ID="txtIDSiswaList" />
            <asp:HiddenField runat="server" ID="txtIDEmail" />
            <asp:HiddenField runat="server" ID="txtNamaVal" />
            <asp:HiddenField runat="server" ID="txtTahunAjaran" />
            <asp:HiddenField runat="server" ID="txtTahunAjaranEnc" />
            <asp:HiddenField runat="server" ID="txtSemester" />
            <asp:HiddenField runat="server" ID="txtKelas" />
            <asp:HiddenField runat="server" ID="txtKelasDet" />
            <asp:HiddenField runat="server" ID="txtParseKenaikanKelas" />
            <asp:HiddenField runat="server" ID="txtURLProsesRapor" />
            <asp:HiddenField runat="server" ID="txtJenisLihatCetak" />
            <asp:HiddenField runat="server" ID="txtIDSiswa" />
            <asp:HiddenField runat="server" ID="txtTipeRapor" />

            <asp:HiddenField runat="server" ID="txtYpos" />
            <asp:HiddenField runat="server" ID="txtXpos" />

            <asp:HiddenField runat="server" ID="txtYposFileRapor" />
            <asp:HiddenField runat="server" ID="txtXposFileRapor" />

            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoCari" OnClick="btnDoCari_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDetail" OnClick="btnShowDetail_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowListKenaikanKelas" OnClick="btnShowListKenaikanKelas_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowUploadFileRapor" OnClick="btnShowUploadFileRapor_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnBindDataList" OnClick="btnBindDataList_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnBukaFileRapor" OnClick="btnBukaFileRapor_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnBukaFileRaporDet" OnClick="btnBukaFileRaporDet_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnBukaFileRaporDetAfterDeleted" OnClick="btnBukaFileRaporDetAfterDeleted_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowUbahEmail" OnClick="btnShowUbahEmail_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowKontenEmailFileRapor" OnClick="btnShowKontenEmailFileRapor_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowKontenHistEmailRapor" OnClick="btnShowKontenHistEmailRapor_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnRefreshListFileRapor" OnClick="btnRefreshListFileRapor_Click" style="position: absolute; left: -1000px; top: -1000px;" />

            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-12">

                    <div class="col-md-8 col-md-offset-2" style="padding: 0px;">
                        <div class="card" style="margin-top: 40px;">
				            <div class="card-main">
					            <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px; ">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="background-color: #295BC8; padding: 10px; font-weight: normal; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                                <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/packing-3.svg") %>" 
                                                    style="margin: 0 auto; height: 25px; width: 25px;" />
                                                &nbsp;
                                                File Rapor<asp:Literal runat="server" ID="ltrTipeRapor"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="background-color: #295BC8; padding: 0px;">
                                                <hr style="margin: 0px; border-style: solid; border-width: 1px; border-color: #2555BE;" />
                                            </td>
                                        </tr>
                                    </table>

						            <asp:MultiView runat="server" ID="mvMain" ActiveViewIndex="0">

                                        <asp:View runat="server" ID="vList">

                                            <div style="padding: 0px; margin: 0px;">
                                                <asp:ListView ID="lvData" DataSourceID="sql_ds" runat="server" OnSorting="lvData_Sorting" OnPagePropertiesChanging="lvData_PagePropertiesChanging">
                                                    <LayoutTemplate>
                                                        <div class="table-responsive" style="margin: 0px; box-shadow: none;">
							                                <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
								                                <thead>
								                                    <tr style="background-color: #3367d6;">
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle; width: 30px;">
                                                                            #
									                                    </th>
                                                                        <th style="text-align: center; background-color: #3367d6; width: 30px; vertical-align: middle;">
                                                                            <i class="fa fa-cog"></i>
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Kelas & Guru
									                                    </th>
								                                    </tr>
							                                    </thead>
							                                    <tbody>     
                                                                    <tr id="itemPlaceholder" runat="server"></tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr <%# txtKelasDet.Value == Eval("Rel_KelasDet").ToString() ? " style=\"background-color: #E7F7FF;\" " : "" %> class="<%# (Container.DisplayIndex % 2 == 0 ? "standardrow" : "oddrow") %>">
                                                            <td style="text-align: center; padding: 10px; vertical-align: middle; color: #bfbfbf;">
                                                                <%# (int)(this.Session[SessionViewDataName] == null ? 0 : this.Session[SessionViewDataName]) + (Container.DisplayIndex + 1) %>.
                                                            </td>
                                                            <td style="padding: 0px; text-align: center; width: 30px; vertical-align: middle;">
                                                                <div style="margin: 0 auto; display: table;">
                                                                    <ul class="nav nav-list margin-no pull-left">
										                                <li class="dropdown">
                                                                            <a class="dropdown-toggle text-black waves-attach waves-effect" data-toggle="dropdown" style="color: grey; line-height: 15px; padding: 10px; margin: 0px; min-height: 15px; z-index: 0;" title=" Menu ">
                                                                                <i class="fa fa-cog"></i>
                                                                                <i class="fa fa-caret-down" style="font-size: xx-small;"></i>
                                                                            </a>
											                                <ul class="dropdown-menu-list-table">
												                                <li style="background-color: white; padding: 10px;">
													                                <label 
                                                                                        onclick="
                                                                                            DoScrollPos();                                                                                            
                                                                                            <%= txtKelasDet.ClientID %>.value = '<%# Eval("Rel_KelasDet").ToString() %>';
                                                                                            <%= btnBukaFileRapor.ClientID %>.click();
                                                                                        " 
                                                                                        id="btnDetail" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                            <i class="fa fa-folder-open" title=" Buka File Rapor "></i>&nbsp;&nbsp;&nbsp;Buka File Rapor&nbsp;&nbsp;&nbsp;
													                                </label>
												                                </li>
											                                </ul>
										                                </li>
									                                </ul>
                                                                </div>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="font-weight: normal; color: #bfbfbf;">Kelas</span>&nbsp;
                                                                <span style="color: #1DA1F2; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Modules.EDUCATION.Penilaian.ALL.wf_FileRapor.GetUnitSekolah() == AI_ERP.Application_Libs.Libs.UnitSekolah.KB ||
                                                                        AI_ERP.Application_Modules.EDUCATION.Penilaian.ALL.wf_FileRapor.GetUnitSekolah() == AI_ERP.Application_Libs.Libs.UnitSekolah.TK
                                                                        ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("NamaKelasDet").ToString())
                                                                        : AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kelas").ToString())
                                                                    %>
                                                                </span>
                                                                <br />
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("GuruKelas").ToString()) +
                                                                        (
                                                                            AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("GuruPendamping").ToString()).Trim() != ""
                                                                            ? "<span style=\"font-weight: bold;\"> & </span>" + AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("GuruPendamping").ToString())
                                                                            : ""
                                                                        )
                                                                    %>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
								                                    <tr style="background-color: #3367d6;">
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle; width: 30px;">
                                                                            #
									                                    </th>
                                                                        <th style="text-align: center; background-color: #3367d6; width: 30px; vertical-align: middle;">
                                                                            <i class="fa fa-cog"></i>
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Kelas & Guru
									                                    </th>
								                                    </tr>
							                                    </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="3" style="text-align: center; padding: 10px;">
                                                                            ..:: Data Kosong ::..
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </div>
                                            <asp:SqlDataSource ID="sql_ds" runat="server"></asp:SqlDataSource>

                                            <div runat="server" id="div_info_periode" class="content-header ui-content-header" 
                                                style="background-color: #a91212;
                                                        box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
                                                        background-image: none; 
                                                        color: white;
                                                        display: block;
                                                        z-index: 5;
                                                        position: fixed; bottom: 28px; right: 25px; width: 440px; border-radius: 25px;
                                                        padding: 8px; margin: 0px;">                	
                                                <div style="padding-left: 15px; font-weight: bold; padding-top: 6px; padding-bottom: 6px;">
                                                    <asp:Literal runat="server" ID="ltrPeriode"></asp:Literal>
                                                </div>
                                            </div>

                                            <div runat="server" id="div_pager" class="content-header ui-content-header" 
                                                style="background-color: white;
                                                        background-image: none; 
                                                        color: white;
                                                        display: block;
                                                        z-index: 5;
                                                        position: fixed; bottom: 28px; right: 25px; width: 320px; border-radius: 25px;
                                                        padding: 8px; margin: 0px;">
                	
                                                <div style="padding-left: 15px;">
				                                    <asp:DataPager ID="dpData" runat="server" PageSize="100" PagedControlID="lvData">
                                                        <Fields>
                                                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="btn-trans" ShowFirstPageButton="True" FirstPageText='&nbsp;<i class="fa fa-backward"></i>&nbsp;' ShowPreviousPageButton="True" PreviousPageText='&nbsp;<i class="fa fa-arrow-left"></i>&nbsp;' ShowNextPageButton="false" />
                                                            <asp:TemplatePagerField>
                                                                <PagerTemplate>
                                                                    <label style="color: grey; font-weight: normal; padding: 5px; border-style: solid; border-color: #F1F1F1; border-width: 1px; padding-left: 10px; padding-right: 10px; border-radius: 5px;">
                                                                        Hal.
                                                                        <%# ((Container.StartRowIndex + 1) / (Container.PageSize)) + 1 %>
                                                                        &nbsp;/&nbsp;
                                                                        <%# Math.Floor(Convert.ToDecimal((Container.TotalRowCount) / (Container.PageSize))) + 1 %>
                                                                    </label>
                                                                </PagerTemplate>
                                                            </asp:TemplatePagerField>
                                                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="btn-trans" ShowLastPageButton="True" LastPageText='&nbsp;<i class="fa fa-forward"></i>&nbsp;' ShowNextPageButton="True" NextPageText='&nbsp;<i class="fa fa-arrow-right"></i>&nbsp;' ShowPreviousPageButton="false" />
                                                            <asp:TemplatePagerField>
                                                                <PagerTemplate>                                                                        
                                                                    <span style="padding-top: 10px; padding-bottom: 10px; color: gray;">
                                                                        <span class="badge">
                                                                            <asp:Label ID="ttlRcrd" runat="server" Text="<%#Container.TotalRowCount%>"></asp:Label>
                                                                        </span>
                                                                    </span>
                                                                </PagerTemplate>
                                                            </asp:TemplatePagerField>
                                                        </Fields>
                                                    </asp:DataPager>
                                                </div>
		                                    </div>    

                                            <div class="fbtn-container" id="div_button_settings" runat="server">
		                                        <div class="fbtn-inner">
			                                        <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #a91212;" title=" Pilihan ">
                                                        <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                                                        <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                                                    </a>
                                                    <div class="fbtn-dropup" style="z-index: 999999;">
				                                        <asp:LinkButton OnClick="btnRefresh_Click" CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" id="btnRefresh" title=" Refresh " style="background-color: #601B70; color: white;">
                                                            <span class="fbtn-text fbtn-text-left">Refresh Data</span>
                                                            <i class="fa fa-refresh"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ToolTip=" Pengaturan " runat="server" ID="btnDoPengaturan" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" style="background-color: #257228;" OnClick="btnDoPengaturan_Click">
                                                            <span class="fbtn-text fbtn-text-left">Pengaturan</span>
                                                            <i class="fa fa-cog" style="color: white;"></i>
                                                        </asp:LinkButton>
			                                        </div>
		                                        </div>
	                                        </div>

                                        </asp:View>

                                        <asp:View runat="server" ID="vFileRapor">
                                            <div style="padding: 0px; margin: 0px;">
                                                <div class="table-responsive" style="margin: 0px; box-shadow: none;">
                                                    <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
								                        <thead>
								                            <tr style="background-color: #3367d6;">
                                                                <th style="text-align: left; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle;">
                                                                    <asp:Literal runat="server" ID="ltrCaptionSelectedKelas"></asp:Literal>
                                                                    &nbsp;
									                            </th>
								                            </tr>
							                            </thead>
                                                        <tbody>
								                            <tr style="background-color: white;">
                                                                <td style="text-align: left; font-weight: bold; color: grey; background-color: white; vertical-align: middle;">
                                                                    <asp:Literal runat="server" ID="ltrFileRapor"></asp:Literal>
									                            </td>
								                            </tr>
							                            </tbody>
                                                    </table>
                                                </div>
                                            </div>

                                            <div style="position: absolute; top: 45px; right: 15px; float: right;">
                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                    <br /><br />
                                                    <label onclick="DoCheckSiswaRapor(true);" style="cursor: pointer; background-color: dodgerblue; font-weight: normal; font-size: smaller; color: white; padding: 3px; border-radius: 4px; margin-top: 10px;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-check-square-o"></i>
                                                        &nbsp;
                                                        Ceklist Semua
                                                        &nbsp;&nbsp;
                                                    </label>
                                                    &nbsp;
                                                    <label onclick="DoCheckSiswaRapor(false);" style="cursor: pointer; background-color: lightcoral; font-weight: normal; font-size: smaller; color: white; padding: 3px; border-radius: 4px; margin-top: 10px;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-square-o"></i>
                                                        &nbsp;
                                                        Un Ceklist Semua
                                                        &nbsp;&nbsp;
                                                    </label>
                                                    <br /><br />
                                                </div>
                                            </div>

                                            

                                            <div class="content-header ui-content-header" 
							                    style="background-color: white;
									                    box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
									                    background-image: none; 
									                    color: white;
									                    display: block;
									                    position: fixed; bottom: 33px; right: 25px; width: 300px; border-radius: 25px;
									                    padding: 8px; margin: 0px;">
							                    <div style="padding-left: 15px;">
								                    <asp:LinkButton ToolTip=" Kembali " runat="server" ID="lnkBackToList" CssClass="btn-trans" OnClick="lnkBackToList_Click">
									                    <i class="fa fa-arrow-left"></i>
								                    </asp:LinkButton>
                                                    <span style="font-weight: normal; color: #bfbfbf;">
                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
                                                    </span>
                                                    <asp:LinkButton ToolTip=" Refresh " runat="server" ID="lnkRefreshFileRapor" CssClass="btn-trans" OnClick="lnkRefreshFileRapor_Click">
									                    <i class="fa fa-refresh"></i>
                                                        &nbsp;Refresh
								                    </asp:LinkButton>
                                                    <span style="font-weight: normal; color: #bfbfbf;">
                                                        &nbsp;&nbsp;|&nbsp;&nbsp;
                                                    </span>
                                                    <asp:LinkButton OnClientClick="if(!IsCheckedSiswaRapor()) { return false; }; SetSelectedEmail(); return false;" ToolTip=" Bagikan " runat="server" ID="lnkBagikanFileRapor" CssClass="btn-trans" OnClick="lnkBagikanFileRapor_Click">
									                    <i class="fa fa-share-alt"></i>
                                                        &nbsp;Bagikan
								                    </asp:LinkButton>
							                    </div>
						                    </div>

						                    <div class="fbtn-container">
							                    <div class="fbtn-inner">
								                    <a id="btnDoSavePengaturanPSB" 
                                                        onmouseup="if(!IsCheckedSiswaRapor()) { return false; }; setTimeout(function() { $('#<%= mdl_confirm_buat_file_rapor.ClientID %>').modal({ backdrop: 'static', keyboard: false, show: true }); }, 300);" 
                                                        class="fbtn fbtn-lg fbtn-brand waves-attach waves-circle waves-light" 
                                                        data-toggle="dropdown" 
                                                        style="background-color: #00198d;" 
                                                        title=" Buat File Rapor ">
									                    <span class="fbtn-ori icon"><span class="fa fa-file-text"></span></span>
									                    <span class="fbtn-sub icon"><span class="fa fa-file-text"></span></span>
                                                        <span class="fa fa-star" style="position: fixed; font-size: x-small; right: 15px; color: yellow;">
								                    </a>
							                    </div>
						                    </div>

                                            <div aria-hidden="true" class="modal modal-va-middle fade modal-va-middle-show in" runat="server" id="mdl_confirm_buat_file_rapor" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 1500;">
												<div class="modal-dialog modal-xs">
													<div class="modal-content">
														<div class="modal-inner">
															<div class="media margin-bottom margin-top">
																<div class="media-object margin-right-sm pull-left">
																	<span class="icon icon-lg text-brand-accent">info_outline</span>
																</div>
																<div class="media-inner">
																	<span style="font-weight: bold;">
																		KONFIRMASI
																	</span>
																</div>
															</div>
															Anda yakin akan membuat file rapor?
                                                            <div class="row" runat="server" id="div_halaman_rapor">
                                                                <div class="col-xs-12">
                                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                        <label class="label-input" for="<%= txtHalamanRapor.ClientID %>" style="text-transform: none;">Halaman</label>
                                                                        <asp:TextBox runat="server" ID="txtHalamanRapor" CssClass="form-control" Text="1"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
														</div>
														<div class="modal-footer">
                                                            <div class="row" id="pb_proses_buat_file_rapor" style="display: none; margin-left: -24px; margin-right: -24px; background-color: #B50000; color: white; margin-bottom: -10px;">
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
                                                                    <div id="div_info_proses_buat_rapor">
													                    Sedang proses...
													                </div>
													                <br />
												                </div>
											                </div>

															<div id="div_button_buat_file_rapor" class="text-right">
																<asp:LinkButton OnClientClick="DoScrollPosFileRapor(); ShowProsesBuatRapor(true); ReportProcessBuatRapor(); return false;" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect"  runat="server" ID="btnOKBuatFileRapor">
																	<i class="fa fa-check"></i>
																	&nbsp;
																	Buat File Rapor
																</asp:LinkButton>
																<a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                                                <br /><br />
															</div>
														</div>
													</div>
												</div>
											</div>

                                            <div aria-hidden="true" class="modal modal-va-middle fade modal-va-middle-show in" runat="server" id="mdlConfirmEmailRapor" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 1500;">
												<div class="modal-dialog modal-xs">
													<div class="modal-content">
														<div class="modal-inner">
															<div class="media margin-bottom margin-top">
																<div class="media-object margin-right-sm pull-left">
																	<span class="icon icon-lg text-brand-accent">info_outline</span>
																</div>
																<div class="media-inner">
																	<span style="font-weight: bold;">
																		KONFIRMASI
																	</span>
																</div>
															</div>
															Anda yakin akan mengirim email?
														</div>
														<div class="modal-footer">
															<p class="text-right">
																<asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect"  runat="server" ID="btnOKKirimEmailRapor" OnClick="btnOKKirimEmailRapor_Click" style="border-radius: 0px; background-color: #1DA1F2; color: white; font-weight: bold;">
                                                                    <span style="font-weight: bold; color: white;">
																	    &nbsp;&nbsp;
                                                                        <i class="fa fa-envelope"></i>
                                                                        &nbsp;&nbsp;
																	    KIRIM EMAIL
                                                                        &nbsp;&nbsp;
                                                                    </span>
																</asp:LinkButton>
																<a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
															</p>
														</div>
													</div>
												</div>
											</div>

                                        </asp:View>

                                        <asp:View runat="server" ID="vFileRaporDet">
                                            <div style="padding: 0px; margin: 0px;">
                                                <div class="table-responsive" style="margin: 0px; box-shadow: none;">
                                                    <table class="table" id="Table1" runat="server" style="width: 100%; margin: 0px;">
								                        <thead>
								                            <tr style="background-color: #3367d6;">
                                                                <th style="text-align: left; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle;">
                                                                    <asp:Literal runat="server" ID="ltrCaptionSelectedSiswa"></asp:Literal>
                                                                    &nbsp;
									                            </th>
								                            </tr>
							                            </thead>
                                                        <tbody>
								                            <tr style="background-color: white;">
                                                                <td style="text-align: left; font-weight: bold; color: grey; background-color: white; vertical-align: middle;">
                                                                    <asp:Literal runat="server" ID="ltrFileRaporDet"></asp:Literal>
									                            </td>
								                            </tr>
							                            </tbody>
                                                    </table>
                                                </div>
                                            </div>

                                            <div class="content-header ui-content-header" 
							                    style="background-color: white;
									                    box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
									                    background-image: none; 
									                    color: white;
									                    display: block;
									                    position: fixed; bottom: 33px; right: 25px; width: 100px; border-radius: 25px;
									                    padding: 8px; margin: 0px;">
							                    <div style="padding-left: 15px;">
								                    <asp:LinkButton ToolTip=" Kembali " runat="server" ID="lnkOKBackToFileRapor" CssClass="btn-trans" OnClick="lnkOKBackToFileRapor_Click">
									                    <i class="fa fa-arrow-left"></i>
								                    </asp:LinkButton>
							                    </div>
						                    </div>

						                    <div class="fbtn-container">
							                    <div class="fbtn-inner">
								                    <a id="btnDoUploadFileRapor" 
                                                        onmouseup="setTimeout(function() { document.getElementById('<%= btnShowUploadFileRapor.ClientID %>').click(); }, 300);" 
                                                        class="fbtn fbtn-lg fbtn-brand waves-attach waves-circle waves-light" 
                                                        data-toggle="dropdown" 
                                                        style="background-color: #00198d;" 
                                                        title=" Upload File ">
									                    <span class="fbtn-ori icon"><span class="fa fa-upload"></span></span>
									                    <span class="fbtn-sub icon"><span class="fa fa-upload"></span></span>
								                    </a>
							                    </div>
						                    </div>

                                        </asp:View>

                                    </asp:MultiView>

					            </div>
				            </div>
			            </div>
                    </div>

                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_data" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Pengaturan
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboPeriode.ClientID %>" style="text-transform: none;">Periode</label>
                                                        <asp:DropDownList runat="server" ID="cboPeriode" CssClass="form-control"></asp:DropDownList>
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
                                <asp:LinkButton ValidationGroup="vldInput" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInput" OnClick="lnkOKInput_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_email_rapor" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

                <label title=" Tutup " data-dismiss="modal" style="padding: 10px; padding-top: 6px; padding-bottom: 6px; background-color: white; cursor: pointer; position: fixed; right: 35px; top: 20px; z-index: 999999; border-radius: 100%;">
                    <i class="fa fa-times" style="color: black; font-size: large; font-weight: normal;"></i>
                </label>

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
                                        Email Rapor
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtEMailRapor.ClientID %>" style="text-transform: none;">Email</label>
                                                        <asp:TextBox runat="server" ID="txtEMailRapor" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <asp:Literal runat="server" ID="ltrEMailRaporContent"></asp:Literal>
                                                </div>
                                            </div>
										</div>  

                                    </div>
                                </div>								                            
							</div>
				        </div>
				        <div class="modal-footer">
					        <p class="text-right">
                                <asp:LinkButton OnClick="lnkOKKirimEmaiRapor_Click" ValidationGroup="vldInput" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKKirimEmaiRapor" Text="   KIRIM EMAIL   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_upload_file_rapor" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
		        <div class="modal-dialog modal-xs">
			        <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
				        <div class="modal-inner" 
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; 
                            padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; 
                            border-top-left-radius: 5px;
                            border-top-right-radius: 5px;
                            background-color: #B93221;
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
                                        Upload File Rapor
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
                                <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="media margin-bottom-no" 
						                                            style="margin-top: 7px; 
							                                            border-left-style: solid; 
							                                            border-left-color: #1DA1F2; 
							                                            padding-left: 8px; 
							                                            border-left-width: 4px;
							                                            background-color: #E2F1FA;
							                                            padding: 15px;">
					                                            <div class="media-object margin-right-sm pull-left">
						                                            <span class="icon icon-lg text-brand-accent" style="color: #1DA1F2;">info_outline</span>
					                                            </div>
					                                            <div class="media-inner" style="text-align: justify;">
						                                            <span style="color: #1DA1F2;">
							                                            <span style="font-weight: normal; color: #1DA1F2;">
                                                                            <span style="font-weight: bold;">
                                                                                Upload File Rapor
                                                                            </span>
                                                                            <br />
                                                                            PDF, DOC, DOCX, JPG, JPEG, PNG, BMP
                                                                            <br /><span style="font-weight: bold; color: mediumvioletred;">Ukuran maksimal <%= AI_ERP.Application_Libs.Libs.Constantas.MSG_MAX_UPLOAD %></span>
							                                            </span>
						                                            </span>
					                                            </div>
				                                            </div>             
                                                        </div>   
                                                    </div>

                                                    <div class="row">
				                                        <div class="col-md-12">
					                                        
                                                            <div style="margin-top: 10px; width: 100%; overflow: hidden; padding: 0px;">
                                                                <iframe id="fraUploaderFileRapor" style="width: 100%; height: 67px; overflow: hidden; border-style: none; border-width: 1px; border-color: #E5E5E5;" frameborder="0" scrolling="no"></iframe>                                                                
                                                                <div id="divUploadFileRapor" style="width: 100%; position: absolute; left: -10000px; top: -10000px;">
                                                                    <asp:FileUpload runat="server" ID="flUploadFileRapor"/>
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
				        </div>
				        <div class="modal-footer">
					        <p class="text-right" style="margin-top: 0px;">
                                <br />
                                <asp:LinkButton runat="server" ID="lnkOKUploadFileRapor" OnClick="lnkOKUploadFileRapor_Click"
                                    CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" style="font-weight: bold;">
                                    TUTUP
                                </asp:LinkButton>
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_ubah_email" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Ubah Alamat Email
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtNamaSiswaEmail.ClientID %>" style="text-transform: none;">Nama Siswa</label>
                                                        <asp:TextBox runat="server" ID="txtNamaSiswaEmail" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtEmailSiswaEmail.ClientID %>" style="text-transform: none;">Email</label>
                                                        <asp:TextBox runat="server" ID="txtEmailSiswaEmail" Enabled="true" CssClass="form-control"></asp:TextBox>
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
                                <asp:LinkButton OnClick="lnkOKUpdateEmail_Click" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKUpdateEmail" style="font-weight: bold;">
                                    <span style="font-weight: bold;">
                                        &nbsp;&nbsp;
                                        SIMPAN
                                        &nbsp;&nbsp;
                                    </span>
                                </asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_hist_email_rapor" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        History Pengiriman Email
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <asp:Literal ID="ltrHistoryEmailRapor" runat="server"></asp:Literal>
                                                </div>
                                            </div>
										</div>  

                                    </div>
                                </div>								                            
							</div>
				        </div>
				        <div class="modal-footer">
					        <p class="text-right">
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Tutup</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <iframe name="fra_download" id="fra_download" height="0" width="0" style="position: absolute; left: -1000px; top: -1000px;"></iframe>
    <iframe id="fraDelete" style="position: absolute; left: -1000px; top: -1000px; height: 100px; width: 100px;"></iframe>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
        RenderDropDownOnTables();
        InitModalFocus();

        $(document).ready(function () {
            setTimeout(GetSessionProgress, 100);
        });

        function ShowInfoProgressFileRapor(teks) {
            var div_info = document.getElementById("div_info_proses_buat_rapor");
            if (div_info !== null && div_info !== undefined) {
                div_info.innerHTML = teks;
            }
        }

        function GetSessionProgress() {
            $(function () {             
                $.ajax({  
                    type: "POST",  
                    contentType: "application/json; charset=utf-8",  
                    url: GetURLProsesRapor() + '/GetData',  
                    data: "{}",
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        if (data.d !== null && data.d !== "null" && data.d !== "Selesai") {
                            if(data.d.trim() !== ""){
                                ShowProsesBuatRapor(true);
                                ShowInfoProgressFileRapor(data.d);
                            }
                        }
                        setTimeout(GetSessionProgress, 100);
                    },                    
                    error: function (result) {  
                        console.log(result);  
                    }  
                });  
  
            });    
        }

        function SelesaiProses() {
            HideModal();
            ShowInfoProgressFileRapor("Sedang proses...");
            ShowProsesBuatRapor(false);
            document.getElementById("<%= btnRefreshListFileRapor.ClientID %>").click();
        }
    </script>
</asp:Content>
