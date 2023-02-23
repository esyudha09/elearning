<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.ListNilaiSiswa.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP.wf_ListNilaiSiswa" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function HideModal(){
            $('#ui_modal_input_data').modal('hide');
            $('#ui_modal_input_halaman_nilai').modal('hide');            
            $('#ui_modal_download_rapor_dapodik').modal('hide');            

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
                case "<%= JenisAction.DoShowPengaturan %>":
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowDownloadNilaiDapodik %>":
                    $('#ui_modal_download_rapor_dapodik').modal({ backdrop: 'static', keyboard: false, show: true });
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
                case "<%= JenisAction.DoShowData %>":
                    ReInitTinyMCE();   
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowListKenaikanKelas %>":
                    ShowPengaturan(false);
                    $('#ui_modal_kenaikan_kelas').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowInputHalaman %>":
                    $('#ui_modal_input_halaman_nilai').modal({ backdrop: 'static', keyboard: false, show: true });
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
            $('#ui_modal_input_halaman_nilai').on('shown.bs.modal', function () {
                //document.getElementById("<%= txtHalaman.ClientID %>").focus();
            });            
        }

        function TriggerSave(){
            tinyMCE.triggerSave();
        }

        function ShowRapor(){
            var url = "";
            url = "<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.CREATE_REPORT.ROUTE) %>" +
                "?" + "<%= AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY + "=" + AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KTSP_SMP %>" +
                "&t=" + <%= txtTahunAjaran.ClientID %>.value +
                "&s=1" +
                "&h=" + <%= txtHalaman.ClientID %>.value +
                "&kd=" + <%= txtKelasDet.ClientID %>.value;
            window.open( 
                url, '_blank');

            return false;
        }

        function ShowRaporKurtilas(){
            var url = "";
            url = "<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.CREATE_REPORT.ROUTE) %>" +
                "?" + "<%= AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY + "=" + AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_KURTILAS_SMP %>" +
                "&t=" + <%= txtTahunAjaran.ClientID %>.value +
                "&s=1" +
                "&h=" + <%= txtHalaman.ClientID %>.value +
                "&kd=" + <%= txtKelasDet.ClientID %>.value;
            window.open( 
                url, '_blank');

            return false;
        }

        function ShowProsesDownloadRaporDapodik(show){
            pb_proses_download_rapor_dapodik.style.display = (show ? "" : "none");
            div_button_rapor_dapodik.style.display = (show ? "none" : "");
            if (!show) {
                if (navigator.appName == 'Microsoft Internet Explorer') {
                    window.frames['fra_download'].document.execCommand('Stop');
                } else {
                    window.frames['fra_download'].stop();
                }
            }
        }

        function txt_tahun_ajaran_dapodik() { return document.getElementById("<%= txtTahunAjaranDapodik.ClientID %>"); }
        function txt_semester_dapodik() { return document.getElementById("<%= txtSemesterDapodik.ClientID %>"); }
        function txt_kelas_dapodik() { return document.getElementById("<%= txtRelKelasDet.ClientID %>"); }
        function cbo_mapel_dapodik() { return document.getElementById("<%= cboMapelDapodik.ClientID %>"); }

        function ReportProcessDownloadRaporDapodik()
        {
            var url = "<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.CREATE_REPORT.ROUTE) %>";
            url += "?<%= AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY %>=<%= AI_ERP.Application_Libs.Downloads.JenisDownload.RAPOR_DAPODIK_SMP %>";
            url += "&t=" + txt_tahun_ajaran_dapodik().value;
            url += "&s=" + txt_semester_dapodik().value;
            url += "&kd=" + txt_kelas_dapodik().value;
            url += "&m=" + cbo_mapel_dapodik().value;

            if (navigator.appName == 'Microsoft Internet Explorer') {
                window.frames['fra_download'].document.location.href = url;
            } else {
                window.frames['fra_download'].location.href = url;
            }
        }

        function HideProseLaporan() {
            ShowProsesDownloadRaporDapodik(false);
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

        function DoCheckSiswaRapor(value){
            var arr_siswa = document.getElementsByName("chk_pilih_siswa_rapor[]");
            if(arr_siswa.length > 0){
                for (var i = 0; i < arr_siswa.length; i++) {
                    arr_siswa[i].checked = value;
                }
            }
        }

        function GetCheckedSiswaRapor(){
            var arr_siswa = document.getElementsByName("chk_pilih_siswa_rapor[]");
            var s_hasil = "";
            if(arr_siswa.length > 0){
                for (var i = 0; i < arr_siswa.length; i++) {
                    if(arr_siswa[i].checked){
                        s_hasil += arr_siswa[i].value + ";";
                    }
                }
            }
            return s_hasil;
        }

        function ValidateCheckedSiswaRapor(){
            var arr_siswa = document.getElementsByName("chk_pilih_siswa_rapor[]");
            if(arr_siswa.length > 0){
                for (var i = 0; i < arr_siswa.length; i++) {
                    if(arr_siswa[i].checked){
                        return true;
                    }
                }
            }
            return false;
        }

        function ShowSBMessage(msg){
            $('body').snackbar({
                alive: 2000,
                content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;' + msg,
                show: function () {
                    snackbarText++;
                }
            });
        }

        function GetCheckedSiswaRapor(){
            var arr_siswa = document.getElementsByName("chk_pilih_siswa_rapor[]");
            var s_hasil = "";
            if(arr_siswa.length > 0){
                for (var i = 0; i < arr_siswa.length; i++) {
                    if(arr_siswa[i].checked){
                        s_hasil += arr_siswa[i].value + ";";
                    }
                }
            }
            return s_hasil;
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
            <asp:HiddenField runat="server" ID="txtNamaVal" />
            <asp:HiddenField runat="server" ID="txtTahunAjaran" />
            <asp:HiddenField runat="server" ID="txtSemester" />
            <asp:HiddenField runat="server" ID="txtKelasDet" />
            <asp:HiddenField runat="server" ID="txtKelas" />
            <asp:HiddenField runat="server" ID="txtRelKelasDet" />
            <asp:HiddenField runat="server" ID="txtParseKenaikanKelas" />
            <asp:HiddenField runat="server" ID="txtJenisLihatCetak" />

            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoCari" OnClick="btnDoCari_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDetail" OnClick="btnShowDetail_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDownloadRaporDapodik" OnClick="btnShowDownloadRaporDapodik_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowListKenaikanKelas" OnClick="btnShowListKenaikanKelas_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnOKKenaikanKelas" OnClick="btnOKKenaikanKelas_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowCetakRapor" OnClick="btnShowCetakRapor_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnBindDataList" OnClick="btnBindDataList_Click" style="position: absolute; left: -1000px; top: -1000px;" />

            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-12">

                    <div class="col-md-8 col-md-offset-2" style="padding: 0px;">
                        <div class="card" style="margin-top: 40px;">
				            <div class="card-main">
					            <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px; ">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="background-color: #295BC8; padding: 10px; font-weight: normal; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                                <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/office-material-3.svg") %>" 
                                                    style="margin: 0 auto; height: 25px; width: 25px;" />
                                                &nbsp;
                                                Data Penilaian
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
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle; width: 80px;">
                                                                            #
									                                    </th>
                                                                        <th style="text-align: center; background-color: #3367d6; width: 80px; vertical-align: middle;">
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
                                                        <tr class="<%# (Container.DisplayIndex % 2 == 0 ? "standardrow" : "oddrow") %>">
                                                            <td style="text-align: center; padding: 10px; vertical-align: middle; color: #bfbfbf;">
                                                                <%# (int)(this.Session[SessionViewDataName] == null ? 0 : this.Session[SessionViewDataName]) + (Container.DisplayIndex + 1) %>.
                                                            </td>
                                                            <td style="padding: 0px; text-align: center; width: 80px; vertical-align: middle;">
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
                                                                                            <%= btnBindDataList.ClientID %>.click();
                                                                                            window.open( 
                                                                                            '<%# 
                                                                                                ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA.ROUTE) +
                                                                                                            "?t=" + AI_ERP.Application_Libs.RandomLibs.GetRndTahunAjaran(Eval("TahunAjaran").ToString()) +
                                                                                                            "&" + AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.URL_ID_GURU_KELAS +
                                                                                                            "&" + AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT +
                                                                                                            "&g=" + Eval("Rel_GuruKelas").ToString() +
                                                                                                            "&kd=" + Eval("Rel_KelasDet").ToString()	
                                                                                            %>', '_blank')
                                                                                        " 
                                                                                        style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                            <i class="fa fa-file-text-o" title=" Lihat Nilai Siswa "></i>&nbsp;&nbsp;&nbsp;Lihat Nilai Siswa&nbsp;&nbsp;&nbsp;
													                                </label>
												                                </li>
                                                                                <li style="<%# (Eval("Semester").ToString() == "2" ? "" : " display: none; ") %>background-color: white; padding: 0px;">                                                                                    
                                                                                    <hr style="margin: 0px;" />
                                                                                </li>
                                                                                <li style="<%# (Eval("Semester").ToString() == "2" ? "" : " display: none; ") %>background-color: white; padding: 10px;">
													                                <label 
                                                                                        onclick="<%= txtTahunAjaran.ClientID %>.value = '<%# Eval("TahunAjaran").ToString() %>'; <%= txtKelasDet.ClientID %>.value = '<%# Eval("Rel_KelasDet").ToString() %>'; <%= btnShowListKenaikanKelas.ClientID %>.click();" 
                                                                                        style="color: palevioletred; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                            <i class="fa fa-check-square-o" title=" Kenaikan Kelas "></i>&nbsp;&nbsp;&nbsp;Kenaikan Kelas/Kelulusan&nbsp;&nbsp;&nbsp;
													                                </label>
												                                </li>
                                                                                <li style="background-color: white; padding: 0px;">
                                                                                    <hr style="margin: 0px;" />
                                                                                </li>
                                                                                <li style="background-color: white; padding: 10px;">
													                                <label 
                                                                                        onclick="
                                                                                            <%= txtJenisLihatCetak.ClientID %>.value = '<%# AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP.wf_ListNilaiSiswa.JenisLihatCetak.LTS %>'; 
                                                                                            <%= txtTahunAjaran.ClientID %>.value = '<%# Eval("TahunAjaran").ToString() %>'; 
                                                                                            <%= txtSemester.ClientID %>.value = '<%# Eval("Semester").ToString() %>'; 
                                                                                            <%= txtKelas.ClientID %>.value = '<%# Eval("Kelas").ToString() %>';
                                                                                            <%= txtKelasDet.ClientID %>.value = '<%# Eval("Rel_KelasDet").ToString() %>';
                                                                                            <%= btnShowCetakRapor.ClientID %>.click();" 
                                                                                        style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                            <i class="fa fa-file-text-o" title=" Lihat Nilai LTS "></i>&nbsp;&nbsp;&nbsp;Lihat Nilai LTS&nbsp;&nbsp;&nbsp;
													                                </label>
												                                </li>
                                                                                <li style="<%# Eval("TahunAjaran").ToString() == "2020/2021" ? "display: none;" : "" %>background-color: white; padding: 0px;">
                                                                                    <hr style="margin: 0px;" />
                                                                                </li>
                                                                                <li style="background-color: white; padding: 10px;">
													                                <label 
                                                                                        onclick="
                                                                                            <%= btnBindDataList.ClientID %>.click();
                                                                                            window.open('<%# 
                                                                                                            ResolveUrl(
                                                                                                                AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP.wf_ListNilaiSiswa.s_url_ok + "?" +
                                                                                                                "t=" + AI_ERP.Application_Libs.RandomLibs.GetRndTahunAjaran(Eval("TahunAjaran").ToString()) +
                                                                                                                "&s=" + Eval("Semester").ToString() +
                                                                                                                "&kd=" + Eval("Rel_KelasDet").ToString() +
                                                                                                                "&j=deskripsi_nc"
                                                                                                            )
                                                                                                        %>', '_blank', 'left=0,top=0,width=900,height=900,toolbar=0,scrollbars=0,status=0');
                                                                                            "
                                                                                        style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                            <i class="fa fa-file-text-o" title=" Lihat Deskripsi LTS "></i>&nbsp;&nbsp;&nbsp;Lihat Deskripsi LTS&nbsp;&nbsp;&nbsp;
													                                </label>
												                                </li>
                                                                                <li style="background-color: white; padding: 0px;">
                                                                                    <hr style="margin: 0px;" />
                                                                                </li>
                                                                                <li style="background-color: white; padding: 10px;">
													                                <label 
                                                                                        onclick="
                                                                                            <%= txtJenisLihatCetak.ClientID %>.value = '<%# AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP.wf_ListNilaiSiswa.JenisLihatCetak.RAPOR %>'; 
                                                                                            <%= txtTahunAjaran.ClientID %>.value = '<%# Eval("TahunAjaran").ToString() %>'; 
                                                                                            <%= txtSemester.ClientID %>.value = '<%# Eval("Semester").ToString() %>'; 
                                                                                            <%= txtKelas.ClientID %>.value = '<%# Eval("Kelas").ToString() %>';
                                                                                            <%= txtKelasDet.ClientID %>.value = '<%# Eval("Rel_KelasDet").ToString() %>';
                                                                                            <%= btnShowCetakRapor.ClientID %>.click();" 
                                                                                        style="color: #67367c; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                            <i class="fa fa-file-pdf-o"></i>&nbsp;&nbsp;&nbsp;Lihat Nilai Rapor&nbsp;&nbsp;&nbsp;
													                                </label>
												                                </li>
                                                                                <li style="background-color: white; padding: 0px;">
                                                                                    <hr style="margin: 0px;" />
                                                                                </li>
                                                                                <li style="background-color: white; padding: 10px;">
													                                <label 
                                                                                        onclick="<%# txtWalikelasDapodik.ClientID %>.value = '<%# Eval("GuruKelas").ToString().Replace("'", "`") %>'; <%# txtKelasDapodik.ClientID %>.value = '<%# Eval("Rel_KelasDet").ToString() %>'; <%# txtRelKelasDet.ClientID %>.value = '<%# Eval("Rel_KelasDet").ToString() %>'; <%# btnShowDownloadRaporDapodik.ClientID %>.click(); "
                                                                                        style="color: #2a8164; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                            <i class="fa fa-file-excel-o" title=" Download Nilai Rapor (Dapodik) "></i>&nbsp;&nbsp;&nbsp;Download Nilai Rapor (Dapodik)&nbsp;&nbsp;&nbsp;
													                                </label>
												                                </li>
                                                                                <li style="background-color: white; padding: 0px;">
                                                                                    <hr style="margin: 0px;" />
                                                                                </li>
                                                                                <li style="background-color: white; padding: 10px;">
													                                <label 
                                                                                        onclick="
                                                                                            <%= btnBindDataList.ClientID %>.click();
                                                                                            window.open( 
                                                                                            '<%# 
                                                                                                ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.LIHAT_LEDGER.ROUTE) +
                                                                                                            "?t=" + AI_ERP.Application_Libs.RandomLibs.GetRndTahunAjaran(Eval("TahunAjaran").ToString()) +
                                                                                                            "&kd=" + Eval("Rel_KelasDet").ToString()	
                                                                                            %>' + '&s=' + <%# txtSemester.ClientID %>.value, '_blank')
                                                                                        " 
                                                                                        style="color: #2a8164; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                            <i class="fa fa-file-text" title=" Lihat Ledger Nilai "></i>&nbsp;&nbsp;&nbsp;Lihat Ledger Nilai&nbsp;&nbsp;&nbsp;
													                                </label>
												                                </li>
                                                                                <%# AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_StrukturNilai.GetKurikulumByKelas(Eval("Rel_KelasDet").ToString(), Eval("TahunAjaran").ToString(), Eval("Semester").ToString()) == AI_ERP.Application_Libs.Libs.JenisKurikulum.SMP.KTSP ? "<li style=\"background-color: white; padding: 10px; display: none;\">" : "<li style=\"background-color: white; padding: 10px;\">" %>
													                                <label 
                                                                                        onclick="
                                                                                            <%= btnBindDataList.ClientID %>.click();
                                                                                            window.open( 
                                                                                            '<%# 
                                                                                                ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.LIHAT_LEDGER.ROUTE) +
                                                                                                            "?t=" + AI_ERP.Application_Libs.RandomLibs.GetRndTahunAjaran(Eval("TahunAjaran").ToString()) +
                                                                                                            "&kd=" + Eval("Rel_KelasDet").ToString()	+
                                                                                                            "&lengkap=1"
                                                                                            %>' + '&s=' + <%# txtSemester.ClientID %>.value, '_blank')
                                                                                        " 
                                                                                        style="color: #2a8164; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                            <i class="fa fa-file-text" title=" Lihat Ledger Nilai "></i>&nbsp;&nbsp;&nbsp;Lihat Ledger Nilai KURTILAS (Lengkap)&nbsp;&nbsp;&nbsp;
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
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kelas").ToString())
                                                                    %>
                                                                </span>
                                                                <br />
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("GuruKelas").ToString())
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
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle; width: 80px;">
                                                                            #
									                                    </th>
                                                                        <th style="text-align: center; background-color: #3367d6; width: 80px; vertical-align: middle;">
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
            
            <div aria-hidden="true" class="modal fade" id="ui_modal_input_halaman_nilai" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                        Lihat/Cetak Rapor
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
                                                        <label class="label-input" style="text-transform: none; color: grey; font-weight: bold; width: 100%;">
                                                            <asp:Literal runat="server" ID="ltrPilihSiswaCaption"></asp:Literal>
                                                        </label>
                                                        <br /><br />
                                                        <label onclick="DoCheckSiswaRapor(true);" style="cursor: pointer; background-color: dodgerblue; font-weight: normal; font-size: smaller; color: white; padding: 3px; border-radius: 4px;">
                                                            &nbsp;&nbsp;
                                                            <i class="fa fa-check-square-o"></i>
                                                            &nbsp;
                                                            Ceklist Semua
                                                            &nbsp;&nbsp;
                                                        </label>
                                                        &nbsp;
                                                        <label onclick="DoCheckSiswaRapor(false);" style="cursor: pointer; background-color: lightcoral; font-weight: normal; font-size: smaller; color: white; padding: 3px; border-radius: 4px;">
                                                            &nbsp;&nbsp;
                                                            <i class="fa fa-square-o"></i>
                                                            &nbsp;
                                                            Un Ceklist Semua
                                                            &nbsp;&nbsp;
                                                        </label>
                                                        <br /><br />
                                                        <asp:Literal runat="server" ID="ltrPilihSiswaCetakRapor"></asp:Literal>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="div_halaman_rapor" class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtHalaman.ClientID %>" style="text-transform: none;">Halaman Rapor</label>
                                                        <asp:TextBox runat="server" ID="txtHalaman" CssClass="form-control"></asp:TextBox>
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
                                <asp:LinkButton ValidationGroup="vldInput" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHalamanRapor" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_download_rapor_dapodik" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Download Rapor (Dapodik)
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-4">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtTahunAjaranDapodik.ClientID %>" style="text-transform: none;">Tahun Pelajaran</label>
                                                        <asp:TextBox runat="server" ID="txtTahunAjaranDapodik" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-xs-4">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtSemesterDapodik.ClientID %>" style="text-transform: none;">Semester</label>
                                                        <asp:TextBox runat="server" ID="txtSemesterDapodik" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-xs-4">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtKelasDapodik.ClientID %>" style="text-transform: none;">Kelas</label>
                                                        <asp:TextBox runat="server" ID="txtKelasDapodik" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtWalikelasDapodik.ClientID %>" style="text-transform: none;">Wali Kelas</label>
                                                        <asp:TextBox runat="server" ID="txtWalikelasDapodik" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboMapelDapodik.ClientID %>" style="text-transform: none;">Mata Pelajaran</label>
                                                        <asp:DropDownList runat="server" ID="cboMapelDapodik" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
										</div>  

                                    </div>
                                </div>								                            
							</div>
				        </div>
				        <div class="modal-footer">
                            <div class="row" id="pb_proses_download_rapor_dapodik" style="display: none; margin-left: -24px; margin-right: -24px; background-color: #B50000; color: white;">
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

					        <p id="div_button_rapor_dapodik" class="text-right">
                                <asp:LinkButton OnClientClick="ShowProsesDownloadRaporDapodik(true); ReportProcessDownloadRaporDapodik(); return false;" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKDownloadRaporDapodik" Text="   DOWNLOAD   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_kenaikan_kelas" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                        Kenaikan Kelas
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

                                                    <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-12" style="color: grey;">                                                            
                                                            <asp:TextBox runat="server" ID="txtInfoSiswa" Enabled="false" CssClass="text-input" style="background-color: #F5F8FA; font-weight: bold; width: 100%;"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div id="div_list_siswa_absen" class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-12" style="color: grey;">
                                                            
                                                            <asp:Literal runat="server" ID="ltrListSiswaKenaikanKelas"></asp:Literal>

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
			            <a onclick="SetKenaikanKelas(); <%= btnOKKenaikanKelas.ClientID %>.click();" class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" style="background-color: #329CC3;" title=" Simpan ">
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
        </ContentTemplate>
    </asp:UpdatePanel>
    <iframe name="fra_download" id="fra_download" height="0" width="0" style="position: absolute; left: -1000px; top: -1000px;"></iframe>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
        RenderDropDownOnTables();
        InitModalFocus();
    </script>
</asp:Content>
