<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="wf.RumahSoalSMP.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP.wf_RumahSoal_SMP" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<%@ Register TagPrefix="ucl" TagName="AutocompleteAspekPenilaian" Src="~/Application_Controls/Elearning/SMP/AutocompleteAspekPenilaian/AutocompleteAspekPenilaian.ascx" %>
<%@ Register TagPrefix="ucl" TagName="AutocompleteKompetensiDasar" Src="~/Application_Controls/Elearning/SMP/AutocompleteKompetensiDasar/AutocompleteKompetensiDasar.ascx" %>
<%@ Register TagPrefix="ucl" TagName="AutocompleteKomponenPenilaian" Src="~/Application_Controls/Elearning/SMP/AutocompleteKomponenPenilaian/AutocompleteKomponenPenilaian.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .text-input2 {
            display: inline-block;
            padding: 6px;
            padding-left: 5px;
            padding-right: 5px;
            margin: 0;
            outline: 0;
            background-color: #F5F8FA;
            border: 1px solid #E6ECF0;
            font-size: small;
            width: 100%;
        }
    </style>
    <script type="text/javascript">
        function HideModal(){
            $('#ui_modal_input_data').modal('hide');
            $('#ui_modal_confirm_hapus').modal('hide');   
            $('#ui_modal_input_aspek_penilaian').modal('hide');    
            $('#ui_modal_input_kompetensi_dasar').modal('hide');    
            $('#ui_modal_input_komponen_penilaian').modal('hide');   
            $('#ui_modal_predikat_penilaian').modal('hide');   
            $('#ui_modal_buka_semester').modal('hide');
            $('#ui_modal_lihat_data').modal('hide');            

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
                case "<%= JenisAction.DoChangePage %>":
                case "<%= JenisAction.DoShowStrukturNilai %>":
                    window.scrollTo(0,0); 
                    break;
                case "<%= JenisAction.ShowDataList %>":
                    SetScrollPos();
                    break;                    
                case "<%= JenisAction.Add %>":
                    ReInitTinyMCE();
                    ValidateInputByMapel();
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.AddWithMessage %>":
                    HideModal();
                    //$('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.AddAPWithMessage %>":
                    HideModal();
                    $('#ui_modal_input_aspek_penilaian').modal({ backdrop: 'static', keyboard: false, show: true });
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.AddKDWithMessage %>":
                    HideModal();
                    $('#ui_modal_input_kompetensi_dasar').modal({ backdrop: 'static', keyboard: false, show: true });
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoShowInputPredikat %>":
                    $('#ui_modal_predikat_penilaian').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.AddKPWithMessage %>":
                    HideModal();
                    $('#ui_modal_input_komponen_penilaian').modal({ backdrop: 'static', keyboard: false, show: true });
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
                    ValidateInputByMapel();
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmHapus %>":
                    $('#ui_modal_confirm_hapus').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowInputAspekPenilaian %>":
                    $('#ui_modal_input_aspek_penilaian').modal({ backdrop: 'static', keyboard: false, show: true });                    
                    break;
                case "<%= JenisAction.DoShowInputKompetensiDasar %>":
                    $('#ui_modal_input_kompetensi_dasar').modal({ backdrop: 'static', keyboard: false, show: true });                    
                    break;
                case "<%= JenisAction.DoShowInputKomponenPenilaian %>":
                    ReInitTinyMCE();   
                    $('#ui_modal_input_komponen_penilaian').modal({ backdrop: 'static', keyboard: false, show: true });                    
                    break;
                case "<%= JenisAction.DoShowInputDeskripsiKTSP %>":
                    ReInitTinyMCE();   
                    $('#ui_modal_deskripsi_ktsp').modal({ backdrop: 'static', keyboard: false, show: true });                    
                    break;
                case "<%= JenisAction.DoShowInputDeskripsiKURTILAS %>":
                    ReInitTinyMCE();
                    $('#ui_modal_deskripsi_kurtilas').modal({ backdrop: 'static', keyboard: false, show: true });                    
                    break;
                case "<%= JenisAction.DoShowBukaSemester %>":
                    $('#ui_modal_buka_semester').modal({ backdrop: 'static', keyboard: false, show: true });                    
                    break;
                case "<%= JenisAction.DoShowLihatData %>":
                    $('#ui_modal_lihat_data').modal({ backdrop: 'static', keyboard: false, show: true });                    
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
            
            Sys.Browser.WebKit = {};
            if (navigator.userAgent.indexOf('WebKit/') > -1) {
                Sys.Browser.agent = Sys.Browser.WebKit;
                Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
                Sys.Browser.name = 'WebKit';
            }

            RenderDropDownOnTables();
            InitModalFocus();
            <%= txtAspekPenilaian.NamaClientID %>_SHOW_AUTOCOMPLETE();
            <%= txtKompetensiDasar.NamaClientID %>_SHOW_AUTOCOMPLETE();
            document.getElementById("<%= txtKeyAction.ClientID %>").value = "";            
        }

        function ReInitTinyMCE(){
            RemoveTinyMCE();
            LoadTinyMCEMateri();
            LoadTinyMCEDeskripsi();
            LoadTinyMCEDeskripsiPengetahuan();
            LoadTinyMCEDeskripsiKeterampilan();
            LoadTinyMCEDeskripsiMataPelajaran();
            LoadTinyMCEDeskripsiSikapSosial();
            LoadTinyMCEDeskripsiSikapSpiritual();
        }

        function InitModalFocus(){
            $('#ui_modal_input_data').on('shown.bs.modal', function () {
                if(!<%= txtTahunAjaran.ClientID %>.disabled){
                    <%= txtTahunAjaran.ClientID %>.focus();
                }
                else {
                    <%= txtKKM.ClientID %>.focus();
                }
            });
            $('#ui_modal_input_aspek_penilaian').on('shown.bs.modal', function () {
                <%= txtPoinAspekPenilaian.ClientID %>.focus();
            });
            $('#ui_modal_input_kompetensi_dasar').on('shown.bs.modal', function () {
                <%= txtPoinKompetensiDasar.ClientID %>.focus();
            });       
            $('#ui_modal_deskripsi_ktsp').on('shown.bs.modal', function () {
                <%= txtDeskripsiMapel.ClientID %>.focus();
            });       
            $('#ui_modal_deskripsi_kurtilas').on('shown.bs.modal', function () {
                <%= txtDeskripsiPengetahuan.ClientID %>.focus();
            });
            $('#ui_modal_lihat_data').on('shown.bs.modal', function () {
                <%= cboPeriodeLihatData.ClientID %>.focus();
            });            
        }

        function ParseSelectedItemStrukturPenilaian(){
            var txt_aspek_penilaian = document.getElementById("<%= txtParseIDAspekPenilaian.ClientID %>");
            var txt_komp_dasar = document.getElementById("<%= txtParseIDKompetensiDasar.ClientID %>");
            var txt_komp_penilaian = document.getElementById("<%= txtParseIDKomponenPenilaian.ClientID %>");

            if(txt_aspek_penilaian != null && txt_aspek_penilaian != undefined && 
               txt_komp_dasar != null && txt_komp_dasar != undefined && 
               txt_komp_penilaian != null && txt_komp_penilaian != undefined
            ){
                txt_aspek_penilaian.value = "";
                txt_komp_dasar.value = "";
                txt_komp_penilaian.value = "";

                var arr_chk_aspek_penilaian = document.getElementsByName("chk_ap[]");
                var arr_chk_kom_dasar = document.getElementsByName("chk_kd[]");
                var arr_chk_kom_penilaian = document.getElementsByName("chk_kp[]");

                if(arr_chk_aspek_penilaian.length > 0){
                    for (var i = 0; i < arr_chk_aspek_penilaian.length; i++) {
                        if(arr_chk_aspek_penilaian[i].checked){
                            txt_aspek_penilaian.value += arr_chk_aspek_penilaian[i].value + ";";
                        }
                    }
                }

                if(arr_chk_kom_dasar.length > 0){
                    for (var i = 0; i < arr_chk_kom_dasar.length; i++) {
                        if(arr_chk_kom_dasar[i].checked){
                            txt_komp_dasar.value += arr_chk_kom_dasar[i].value + ";";
                        }
                    }
                }

                if(arr_chk_kom_penilaian.length > 0){
                    for (var i = 0; i < arr_chk_kom_penilaian.length; i++) {
                        if(arr_chk_kom_penilaian[i].checked){
                            txt_komp_penilaian.value += arr_chk_kom_penilaian[i].value + ";";
                        }
                    }
                }
            }
        }

        function ShowConfirmHapusItemStrukturNilai(){
            ParseSelectedItemStrukturPenilaian();
            $('#ui_modal_hapus_item_struktur_penilaian').modal({ backdrop: 'static', keyboard: false, show: true });
        }

        function SelectKomponenPenilaian(id, checked){
            var arr_chk = document.getElementsByName("chkIsAdaPB[]");
            if(arr_chk.length > 0){
                if(checked){
                    for (var i = 0; i < parseInt(id); i++) {
                        if(id > i){
                            arr_chk[i].checked = true;
                        }
                    }
                }else {
                    if(id > 0){
                        for (var i = (id - 1); i < arr_chk.length; i++) {
                            arr_chk[i].checked = false;
                        }
                    }
                }
            }
        }

        function ListSelectedKomponenPenilaian(){
            var txt_parse_kp = document.getElementById("<%= txtParseListKomponenPenilaian.ClientID %>");
            var ada_sel = false;
            if(txt_parse_kp != null && txt_parse_kp != undefined){
                txt_parse_kp.value = "";
                var arr_chk = document.getElementsByName("chkIsAdaPB[]");
                if(arr_chk.length > 0){
                    for (var i = 0; i < arr_chk.length; i++) {
                        if(arr_chk[i].checked){
                            txt_parse_kp.value += arr_chk[i].value + ";";
                            ada_sel = true;
                        }
                    }
                }
            }

            if(ada_sel){
                return true;
            }
            else {
                return false;
            }
        }

        function ReportProcessBukaSemester() {
            var url = "<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.SMP.BUKA_SEMESTER.ROUTE) %>";
            
            url += "?";
            url += "&tn=" + <%= txtTahunAjaranNew.ClientID %>.value;
            url += "&sn=" + <%= txtSemesterNew.ClientID %>.value;
            url += "&t=" + <%= txtTahunAjaranOld.ClientID %>.value;
            url += "&s=" + <%= txtSemesterOld.ClientID %>.value;

            if (navigator.appName == 'Microsoft Internet Explorer') {
                window.frames['fra_download'].document.location.href = url;
            } else {
                window.frames['fra_download'].location.href = url;
            }
        }

        function ShowProsesBukaSemester(show) {
            pb_proses_buka_semester.style.display = (show ? "" : "none");
            div_command_buka_semester.style.display = (show ? "none" : "");
            if (!show) {
                if (navigator.appName == 'Microsoft Internet Explorer') {
                    window.frames['fra_download'].document.execCommand('Stop');
                } else {
                    window.frames['fra_download'].stop();
                }
            }
        }

        function StopProsesDownload()
        {
            if (navigator.appName == 'Microsoft Internet Explorer') {
                window.frames['fra_download'].document.execCommand('Stop');
            } else {
                window.frames['fra_download'].stop();
            }

            ShowProsesBukaSemester(false);
            HideModal();
            <%= btnDoRefreshBukaSemester.ClientID %>.click();
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
              ){
                window.scrollTo(txt_x.value,txt_y.value); 
            }
        }

        function ValidateInputByMapel(){
            var cbo_mapel = document.getElementById("<%= cboMapel.ClientID %>");
            var txt_kkm = document.getElementById("<%= txtKKM.ClientID %>");
            var cbo_jenisperhitungan = document.getElementById("<%= cboJenisPerhitunganRapor.ClientID %>");
            var s_sikap = "Sikap&nbsp;&nbsp;→&nbsp;&nbsp;Sikap";

            div_deskripsi_sikap.style.display = "none";
            div_input_sn_reguler.style.display = "";
            cbo_mapel.disabled = false;
            if(cbo_mapel != null && cbo_mapel != undefined){
                if(cbo_mapel.options[cbo_mapel.selectedIndex].text.toLowerCase().indexOf("sikap") >= 0){
                    txt_kkm.value = "0";
                    cbo_jenisperhitungan.selectedIndex = "1";
                    div_deskripsi_sikap.style.display = "";
                    div_input_sn_reguler.style.display = "none";
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
            <asp:HiddenField runat="server" ID="txtIDAspekPenilaian" />
            <asp:HiddenField runat="server" ID="txtIDKompetensiDasar" />
            <asp:HiddenField runat="server" ID="txtIDKomponenPenilaian" />
            <asp:HiddenField runat="server" ID="txtParseIDAspekPenilaian" />
            <asp:HiddenField runat="server" ID="txtParseIDKompetensiDasar" />
            <asp:HiddenField runat="server" ID="txtParseIDKomponenPenilaian" />
            <asp:HiddenField runat="server" ID="txtParseListKomponenPenilaian" />
            <asp:HiddenField runat="server" ID="txtNamaVal" />
            <asp:HiddenField runat="server" ID="txtDeskripsiKPVal" />
            <asp:HiddenField runat="server" ID="txtMateriKPVal" />
            <asp:HiddenField runat="server" ID="txtActKP" />
            <asp:HiddenField runat="server" ID="txtDeskripsiPengetahuanVal" />
            <asp:HiddenField runat="server" ID="txtDeskripsiKeterampilanVal" />
            <asp:HiddenField runat="server" ID="txtDeskripsiSikapSosialVal" />
            <asp:HiddenField runat="server" ID="txtDeskripsiSikapSpiritualVal" />
            <asp:HiddenField runat="server" ID="txtDeskripsiMapelVal" />       
            <asp:HiddenField runat="server" ID="txtYpos" />
            <asp:HiddenField runat="server" ID="txtXpos" />

            <asp:HiddenField runat="server" ID="txtTahunAjaranNew" />
            <asp:HiddenField runat="server" ID="txtSemesterNew" />
            <asp:HiddenField runat="server" ID="txtTahunAjaranOld" />
            <asp:HiddenField runat="server" ID="txtSemesterOld" /> 
            
            <asp:HiddenField runat="server" ID="txtKodePredikat1" />
            <asp:HiddenField runat="server" ID="txtKodePredikat2" />
            <asp:HiddenField runat="server" ID="txtKodePredikat3" />
            <asp:HiddenField runat="server" ID="txtKodePredikat4" />
            <asp:HiddenField runat="server" ID="txtKodePredikat5" />

            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoCari" OnClick="btnDoCari_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDetail" OnClick="btnShowDetail_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowConfirmDelete" OnClick="btnShowConfirmDelete_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowStrukturNilai" OnClick="btnShowStrukturNilai_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputAspekPenilaian" OnClick="btnShowInputAspekPenilaian_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputEditAspekPenilaian" OnClick="btnShowInputEditAspekPenilaian_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputKompetensiDasar" OnClick="btnShowInputKompetensiDasar_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputEditKompetensiDasar" OnClick="btnShowInputEditKompetensiDasar_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputKomponenPenilaian" OnClick="btnShowInputKomponenPenilaian_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputEditKomponenPenilaian" OnClick="btnShowInputEditKomponenPenilaian_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputEditDeskripsiKTSP" OnClick="btnShowInputEditDeskripsiKTSP_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputEditDeskripsiKURTILAS" OnClick="btnShowInputEditDeskripsiKURTILAS_Click" style="position: absolute; left: -1000px; top: -1000px;" />      
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoRefreshBukaSemester" OnClick="btnDoRefreshBukaSemester_Click" style="position: absolute; left: -1000px; top: -1000px;" />      
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputPredikatPenilaian" OnClick="btnShowInputPredikatPenilaian_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            
            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-12">

                    <div class="col-md-8 col-md-offset-2" style="padding: 0px;">
                        <div class="card" style="margin-top: 40px;">
				            <div class="card-main">
					            <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px; ">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="background-color: #295BC8; padding: 10px; font-weight: normal; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                                <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/browser-2.svg") %>" 
                                                    style="margin: 0 auto; height: 25px; width: 25px;" />
                                                &nbsp;
                                                Struktur Penilaian
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
                                                                        <th colspan="2" style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Tahun Ajaran, Mapel
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            &nbsp;
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: center; padding-left: 10px; vertical-align: middle;">
                                                                            Kelas
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            KKM
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
                                                        <tr class="<%# 
                                                                        Eval("Kode").ToString() == txtID.Value
                                                                        ? "selectedrow" 
                                                                        : (Container.DisplayIndex % 2 == 0 ? "standardrow" : "oddrow") 
                                                                   %>">
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
                                                                                        onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetail.ClientID %>.click(); " 
                                                                                        id="btnDetail" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-info-circle" title=" Lihat Detail "></i>&nbsp;&nbsp;&nbsp;Lihat Detail</label>
												                                </li>
                                                                                <li style="padding: 0px;<%= (AI_ERP.Application_DAOs.DAO_FormasiGuruMapel.IsSebagaiGuru(AI_ERP.Application_Libs.Libs.LOGGED_USER_M.NoInduk, (int)AI_ERP.Application_Libs.Libs.UnitSekolah.SMP) ? " display: none; " : "") %>">
                                                                                    <hr style="margin: 0px; padding: 0px;" />
                                                                                </li>
												                                <li style="background-color: white; padding: 10px;<%= (AI_ERP.Application_DAOs.DAO_FormasiGuruMapel.IsSebagaiGuru(AI_ERP.Application_Libs.Libs.LOGGED_USER_M.NoInduk, (int)AI_ERP.Application_Libs.Libs.UnitSekolah.SMP) ? " display: none; " : "") %>">                                                                                    
													                                <label
                                                                                        onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmDelete.ClientID %>.click(); " 
                                                                                        id="btnHapus" style="color: #9E0000; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-times" title=" Hapus Data "></i>&nbsp;&nbsp;&nbsp;Hapus Data</label>
												                                </li>
											                                </ul>
										                                </li>
									                                </ul>
                                                                </div>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; padding-top: 2px; padding-bottom: 2px; color: grey;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none; font-size: xx-small;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("TahunAjaran").ToString())
                                                                    %>
                                                                </span>
                                                                <sup title=" Semester " style="font-weight: normal; text-transform: none; text-decoration: none; font-weight: bold; font-size: xx-small;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Semester").ToString())
                                                                    %>
                                                                </sup>
                                                                <%# 
                                                                    Eval("JenisPerhitungan").ToString() == ((int)AI_ERP.Application_Libs.Libs.JenisPerhitunganNilai.Bobot).ToString() && Eval("Mapel").ToString().ToLower().IndexOf("sikap") < 0
                                                                    ? "<sup class=\"badge\" " +
                                                                            "style=\"float: right; background-color: #40B3D2; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 4px; padding-top: 4px; display: initial; border-radius: 3px; margin-top: 10px;\">Bobot</sup>"
                                                                    : (
                                                                        Eval("JenisPerhitungan").ToString() == ((int)AI_ERP.Application_Libs.Libs.JenisPerhitunganNilai.RataRata).ToString() && Eval("Mapel").ToString().ToLower().IndexOf("sikap") < 0
                                                                        ? "<sup class=\"badge\" "+
                                                                                "style=\"float: right; background-color: #68217A; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 4px; padding-top: 4px; display: initial; border-radius: 3px; margin-top: 10px;\">Rata-Rata</sup>"
                                                                        : ""
                                                                        )
                                                                %>
                                                                <br />
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none; color: black;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Mapel").ToString())
                                                                    %>
                                                                </span>    
                                                                <br />
                                                                <span style="color: grey; font-weight: normal; font-style: italic; text-transform: none; text-decoration: none; color: #278BF4; font-size: small;">
                                                                    <%# 
                                                                        Eval("JenisMapel").ToString().Trim() != ""
                                                                        ? (
                                                                            Eval("JenisMapel").ToString().Trim().ToUpper() == "EKSKUL"
                                                                            ? "Ekstrakurikuler"
                                                                            : Eval("JenisMapel").ToString() 
                                                                          )                                                 
                                                                        : ""
                                                                    %>
                                                                </span>
                                                                <br />
                                                                <label title=" Struktur Nilai " onclick="DoScrollPos(); <%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>';<%= btnShowStrukturNilai.ClientID %>.click();"
                                                                    style="<%# (AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP.wf_StrukturPenilaian.LockInput() || Eval("Mapel").ToString().ToLower().IndexOf("sikap") >= 0 ? " display: none; " : "") %> cursor: pointer; font-weight: normal; font-size: small; border-radius: 0px; color: darkblue;">
                                                                    <i class="fa fa-file-text-o"></i>
                                                                    &nbsp;Struktur Nilai
                                                                </label>
                                                                <span style="color: #d9d9d9;<%# (Eval("Mapel").ToString().ToLower().IndexOf("sikap") >= 0 ? "display: none;" : "") %>">&nbsp;|&nbsp;</span>
                                                                <label title=" Deskripsi Mapel " onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>';<%= btnShowInputEditDeskripsiKTSP.ClientID %>.click();" 
                                                                    style="
                                                                            <%#
                                                                                (
                                                                                    AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_StrukturNilai.GetKurikulumByLevel(
                                                                                        Eval("Rel_Kelas").ToString(), Eval("TahunAjaran").ToString(), Eval("Semester").ToString()
                                                                                    ) == AI_ERP.Application_Libs.Libs.JenisKurikulum.SMP.KURTILAS || Eval("Mapel").ToString().ToLower().IndexOf("sikap") >= 0
                                                                                    ? " display: none; "
                                                                                    : ""
                                                                                )
                                                                            %> cursor: pointer; font-weight: normal; font-size: small; border-radius: 0px; color: darkblue;">
                                                                    <i class="fa fa-file-text"></i>
                                                                    &nbsp;Deskripsi
                                                                </label>
                                                                <label title=" Deskripsi Mapel " onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>';<%= btnShowInputEditDeskripsiKURTILAS.ClientID %>.click();" 
                                                                    style="
                                                                            <%#
                                                                                (
                                                                                    AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_StrukturNilai.GetKurikulumByLevel(
                                                                                        Eval("Rel_Kelas").ToString(), Eval("TahunAjaran").ToString(), Eval("Semester").ToString()
                                                                                    ) == AI_ERP.Application_Libs.Libs.JenisKurikulum.SMP.KTSP || Eval("Mapel").ToString().ToLower().IndexOf("sikap") >= 0
                                                                                    ? " display: none; "
                                                                                    : ""
                                                                                )
                                                                            %> cursor: pointer; font-weight: normal; font-size: small; border-radius: 0px; color: darkblue;">
                                                                    <i class="fa fa-file-text"></i>
                                                                    &nbsp;Deskripsi
                                                                </label>
                                                            </td>
                                                            <td style="text-align: right; vertical-align: middle;">
                                                                <%--<%# 
                                                                    AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP.wf_StrukturPenilaian.GetHTMLKelasMapelDetIsiNilai(
                                                                        Eval("TahunAjaran").ToString(), Eval("Semester").ToString(), Eval("Rel_Kelas").ToString(), Eval("Rel_Mapel").ToString()
                                                                    )
                                                                %>--%>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; width: 100px;">
                                                                &nbsp;
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center;">
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("Kelas").ToString() +
                                                                        (
                                                                            Eval("Kelas").ToString().Trim() != "" && 
                                                                            Eval("Kelas2").ToString().Trim() != ""
                                                                            ? ", "
                                                                            : ""
                                                                        ) +
                                                                        Eval("Kelas2").ToString() +
                                                                        (
                                                                            (
                                                                                Eval("Kelas2").ToString().Trim() != "" || Eval("Kelas").ToString().Trim() != ""
                                                                            ) && Eval("Kelas3").ToString().Trim() != ""
                                                                            ? ", "
                                                                            : ""
                                                                        ) +
                                                                        Eval("Kelas3").ToString()
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;
                                                                             <%# 
                                                                                Eval("Mapel").ToString().ToLower().IndexOf("sikap") >= 0
                                                                                ? "display: none;"
                                                                                : ""
                                                                             %>
                                                                            ">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("KKM").ToString())
                                                                    %>
                                                                </span>
                                                                <label title=" Pengaturan Predikat Penilaian " id="lbl_<%# Eval("Kode").ToString().Replace("-", "_") %>_1" onclick="DoScrollPos(); <%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>';<%= btnShowInputPredikatPenilaian.ClientID %>.click();" 
                                                                    style="<%# 
                                                                                Eval("Mapel").ToString().ToLower().IndexOf("sikap") < 0
                                                                                ? "display: none;"
                                                                                : ""
                                                                           %>cursor: pointer; font-weight: normal; font-size: small; width: 100%; color: grey; font-weight: bold; width: 20px; text-align: center;">
                                                                    <i class="fa fa-sort-alpha-asc"></i>
                                                                </label>
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
                                                                        <th colspan="2" style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Tahun Ajaran, Mapel
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: center; padding-left: 10px; vertical-align: middle;">
                                                                            Kelas
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            KKM
									                                    </th>
								                                    </tr>
							                                    </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="5" style="text-align: center; padding: 10px;">
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

                                            <div class="content-header ui-content-header" 
                                                style="background-color: white;
                                                        box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
                                                        background-image: none; 
                                                        color: white;
                                                        display: block;
                                                        z-index: 5;
                                                        position: fixed; bottom: 28px; right: 25px; width: 320px; border-radius: 25px;
                                                        padding: 8px; margin: 0px;">
                	
                                                <div style="padding-left: 15px;">
				                                    <asp:DataPager ID="dpData" runat="server" PageSize="10" PagedControlID="lvData">
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
				                                        <asp:LinkButton OnClick="btnRefresh_Click" CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" id="btnRefresh" title=" Refresh " style="background-color: black; color: white;">
                                                            <span class="fbtn-text fbtn-text-left">Refresh Data</span>
                                                            <i class="fa fa-refresh"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ToolTip=" Tambah Data Ekstrakurikuler " runat="server" ID="btnDoAddEkskul" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" style="background-color: black;" OnClick="btnDoAddEkskul_Click">
                                                            <span class="fbtn-text fbtn-text-left">Tambah Data Ekstrakurikuler</span>
                                                            <i class="fa fa-plus-circle" style="color: white;"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ToolTip=" Tambah Data Non Ekstrakurikuler " runat="server" ID="btnDoAdd" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" style="background-color: black;" OnClick="btnDoAdd_Click">
                                                            <span class="fbtn-text fbtn-text-left">Tambah Data Non Ekstrakurikuler</span>
                                                            <i class="fa fa-plus" style="color: white;"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton OnClick="bntLihatData_Click" CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" id="bntLihatData" title=" Lihat Data " style="background-color: black; color: white;">
                                                            <span class="fbtn-text fbtn-text-left">Lihat Data</span>
                                                            <i class="fa fa-eye"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton Visible="false" OnClick="btnBukaSemester_Click" CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" id="btnBukaSemester" title=" Buka Semester " style="background-color: black; color: white;">
                                                            <span class="fbtn-text fbtn-text-left">Buka Semester</span>
                                                            <i class="fa fa-file-text"></i>
                                                        </asp:LinkButton>
			                                        </div>
		                                        </div>
	                                        </div>

                                        </asp:View>

                                        <asp:View runat="server" ID="vListKTSPDet">

                                            <div style="padding: 0px; margin: 0px;">
                                                <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                    <tr style="background-color: #3367d6;">
                                                        <td style="text-align: left; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle;">
                                                            <asp:Literal runat="server" ID="ltrCaptionKTSPDet"></asp:Literal>
									                    </td>
                                                    </tr>
                                                </table>
                                                <asp:Literal runat="server" ID="ltrStrukturNilaiDet"></asp:Literal>
                                            </div>
                                            
                                            <div runat="server" id="div_kembali_ke_header" class="content-header ui-content-header" 
						                        style="background-color: #00198d;
								                        box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
								                        background-image: none; 
								                        color: white;
								                        display: block;
								                        z-index: 5;
								                        position: fixed; bottom: 33px; right: 50px; width: 160px; border-radius: 25px;
								                        padding: 8px; margin: 0px; height: 35px;">
						                        <div style="padding-left: 0px;">
							                        <asp:LinkButton ToolTip=" Kembali " runat="server" ID="btnShowDataListFromKTSPDet" CssClass="btn-trans waves-attach waves-circle waves-effect" OnClick="btnShowDataListFromKTSPDet_Click" style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;
                                                        Kembali
                                                    </asp:LinkButton>
						                        </div>
					                        </div>

                                            <div runat="server" id="div_hapus_struktur_nilai" class="content-header ui-content-header" 
						                        style="background-color: #8A0083;
								                        background-image: none; 
								                        color: white;
								                        display: block;
								                        z-index: 5;
								                        position: fixed; bottom: 33px; right: 50px; width: 120px; border-radius: 25px;
								                        padding: 8px; margin: 0px; height: 35px;">
						                        <div style="padding-left: 0px;">
							                        <asp:LinkButton OnClientClick="ShowConfirmHapusItemStrukturNilai(); return false;" ToolTip=" Hapus " runat="server" ID="lnkHapusItemKTSPDet" CssClass="btn-trans waves-attach waves-circle waves-effect" style="color: ghostwhite;">
                                                        <span style="color: ghostwhite;">
                                                            &nbsp;&nbsp;
                                                            <i class="fa fa-times"></i>
                                                            &nbsp;
                                                            Hapus
                                                            &nbsp;&nbsp;
                                                        </span>                                                    
                                                    </asp:LinkButton>                                                
						                        </div>
					                        </div> 

                                            <div class="fbtn-container" id="div_button_settings_struktur_penilaian" runat="server">
		                                        <div class="fbtn-inner">
			                                        <a onclick="setTimeout(
                                                            function(){
                                                                <%= btnShowInputAspekPenilaian.ClientID %>.click();
                                                            }, 500
                                                        ); return false;" class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #a91212;" title=" Tambah Data ">
                                                        <span class="fbtn-ori icon" style="padding-left: 2px;"><span class="fa fa-plus"></span></span>
                                                        <span class="fbtn-sub icon" style="padding-left: 2px;"><span class="fa fa-plus"></span></span>
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
                                        Isi Data
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-8">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtTahunAjaran.ClientID %>" style="text-transform: none;">Tahun Ajaran</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldTahunAjaran"
                                                            ControlToValidate="txtTahunAjaran" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtTahunAjaran"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-xs-4">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboSemester.ClientID %>" style="text-transform: none;">Semester</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldSemester"
                                                            ControlToValidate="cboSemester" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList CssClass="form-control" runat="server" ID="cboSemester">
                                                            <asp:ListItem></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div> 
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboMapel.ClientID %>" style="text-transform: none;">Mata Pelajaran</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldMapel"
                                                            ControlToValidate="cboMapel" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList CssClass="form-control" runat="server" ID="cboMapel">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div> 
                                            <div runat="server" id="div_kelas_non_ekskul" class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-8">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboKelas.ClientID %>" style="text-transform: none;">Kelas</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldKelas" Enabled="false"
                                                            ControlToValidate="cboKelas" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList CssClass="form-control" runat="server" ID="cboKelas">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="div_kelas_ekskul" class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-4">
					                                <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                                <label for="<%= chkKelasVII.ClientID %>" style="color: #B7770D; font-size: small;">
                                                            <div class="checkbox switch" style="padding-bottom: 0px;">
									                            <label for="<%= chkKelasVII.ClientID %>" style="color: grey; font-weight: bold;">
										                            <input runat="server" class="access-hide" id="chkKelasVII" type="checkbox" />
										                            <span class="switch-toggle"></span>Kelas VII
									                            </label>
								                            </div>
						                                </label>
					                                </div>
				                                </div>
                                                <div class="col-xs-4">
					                                <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                                <label for="<%= chkKelasVIII.ClientID %>" style="color: #B7770D; font-size: small;">
                                                            <div class="checkbox switch" style="padding-bottom: 0px;">
									                            <label for="<%= chkKelasVIII.ClientID %>" style="color: grey; font-weight: bold;">
										                            <input runat="server" class="access-hide" id="chkKelasVIII" type="checkbox" />
										                            <span class="switch-toggle"></span>Kelas VIII
									                            </label>
								                            </div>
						                                </label>
					                                </div>
				                                </div>
                                                <div class="col-xs-4">
					                                <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                                <label for="<%= chkKelasIX.ClientID %>" style="color: #B7770D; font-size: small;">
                                                            <div class="checkbox switch" style="padding-bottom: 0px;">
									                            <label for="<%= chkKelasIX.ClientID %>" style="color: grey; font-weight: bold;">
										                            <input runat="server" class="access-hide" id="chkKelasIX" type="checkbox" />
										                            <span class="switch-toggle"></span>Kelas IX
									                            </label>
								                            </div>
						                                </label>
					                                </div>
				                                </div>
                                            </div>
                                            <div id="div_input_sn_reguler">
                                                <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                    <div class="col-xs-4">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= txtKKM.ClientID %>" style="text-transform: none;">KKM</label>
                                                            <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldKKM"
                                                                ControlToValidate="txtKKM" Display="Dynamic" style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:TextBox CssClass="form-control" runat="server" ID="txtKKM"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                    <div class="col-xs-12">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= cboJenisPerhitunganRapor.ClientID %>" style="text-transform: none;">
                                                                Jenis Perhitungan
                                                            </label>
                                                            <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldJenisPerhitunganRapor"
                                                                ControlToValidate="cboJenisPerhitunganRapor" Display="Dynamic" style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:DropDownList CssClass="form-control" runat="server" ID="cboJenisPerhitunganRapor"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                    <div class="col-xs-12">
					                                    <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 5px;">
						                                    <label for="<%= chkIs_PH_PTS_PAS.ClientID %>" style="color: #B7770D; font-size: small;">
                                                                <div class="checkbox switch" style="padding-bottom: 0px;">
									                                <label for="<%= chkIs_PH_PTS_PAS.ClientID %>" style="color: grey; font-weight: bold;">
										                                <input runat="server" class="access-hide" id="chkIs_PH_PTS_PAS" type="checkbox" />
										                                <span class="switch-toggle"></span>Gunakan PH, PTS, PAS untuk nilai PENGETAHUAN
									                                </label>
								                                </div>
						                                    </label>
					                                    </div>
				                                    </div>
                                                </div>
                                                <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                    <div class="col-xs-4">
					                                    <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 5px;">
						                                    <label for="<%= txtBobotPH.ClientID %>" style="color: #B7770D; font-size: small;">Bobot PH</label>
                                                            <asp:TextBox CssClass="form-control" runat="server" ID="txtBobotPH"></asp:TextBox>
					                                    </div>
				                                    </div>
                                                    <div class="col-xs-4">
					                                    <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 5px;">
						                                    <label for="<%= txtBobotPTS.ClientID %>" style="color: #B7770D; font-size: small;">Bobot PTS</label>
                                                            <asp:TextBox CssClass="form-control" runat="server" ID="txtBobotPTS"></asp:TextBox>
					                                    </div>
				                                    </div>
                                                    <div class="col-xs-4">
					                                    <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 5px;">
						                                    <label for="<%= txtBobotPAS.ClientID %>" style="color: #B7770D; font-size: small;">Bobot PAS</label>
                                                            <asp:TextBox CssClass="form-control" runat="server" ID="txtBobotPAS"></asp:TextBox>
					                                    </div>
				                                    </div>
                                                </div>
                                            </div>
                                            <div  id="div_deskripsi_sikap" style="display: none;">
                                                <div class="row">
                                                    <div class="col-lg-12">
                                        
                                                        <div style="width: 100%; background-color: white; padding-top: 0px;">
                                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                                <div class="col-xs-12">
                                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                        <label class="label-input" for="<%= txtDeskripsiSikapSpiritual.ClientID %>" style="text-transform: none; margin-bottom: 6px;">
                                                                            Deskripsi Sikap Spiritual
                                                                        </label>
                                                                        <asp:TextBox style="height: 100px;" runat="server" ID="txtDeskripsiSikapSpiritual" CssClass="mcetiny_deskripsi_sikap_spiritual"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-12">
                                        
                                                        <div style="width: 100%; background-color: white; padding-top: 0px;">
                                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                                <div class="col-xs-12">
                                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                        <label class="label-input" for="<%= txtDeskripsiSikapSosial.ClientID %>" style="text-transform: none; margin-bottom: 6px;">
                                                                            Deskripsi Sikap Sosial
                                                                        </label>
                                                                        <asp:TextBox style="height: 100px;" runat="server" ID="txtDeskripsiSikapSosial" CssClass="mcetiny_deskripsi_sikap_sosial"></asp:TextBox>
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

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_aspek_penilaian" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Aspek Penilaian
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtPoinAspekPenilaian.ClientID %>" style="text-transform: none;">Poin</label>
                                                        <asp:TextBox runat="server" ID="txtPoinAspekPenilaian" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtAspekPenilaian.NamaClientID %>" style="text-transform: none;">Aspek Penilaian</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputAspekPenilaian" runat="server" ID="vldAspekPenilaian"
                                                            ControlToValidate="txtAspekPenilaian$txtNama" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <ucl:AutocompleteAspekPenilaian runat="server" ID="txtAspekPenilaian" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboJenisPerhitunganAspekPenilaian.ClientID %>" style="text-transform: none;">
                                                            Jenis Perhitungan
                                                        </label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputAspekPenilaian" runat="server" ID="vldJenisPerhitunganAspekPenilaian"
                                                            ControlToValidate="cboJenisPerhitunganAspekPenilaian" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList CssClass="form-control" runat="server" ID="cboJenisPerhitunganAspekPenilaian"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div runat="server" id="div_bobot_rapor_dari_ap" class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtBobotRapor.ClientID %>" style="text-transform: none;">
                                                            Bobot Rapor %
                                                        </label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputAspekPenilaian" runat="server" ID="vldBobotRapor"
                                                            ControlToValidate="txtBobotRapor" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtBobotRapor"></asp:TextBox>
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
                                <asp:LinkButton ValidationGroup="vldInputAspekPenilaian" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKAspekPenilaian" OnClick="lnkOKAspekPenilaian_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_kompetensi_dasar" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Kompetensi Dasar
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtPoinKompetensiDasar.ClientID %>" style="text-transform: none;">Poin</label>
                                                        <asp:TextBox runat="server" ID="txtPoinKompetensiDasar" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtKompetensiDasar.NamaClientID %>" style="text-transform: none;">Kompetensi Dasar</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputKompetensiDasar" runat="server" ID="vldKompetensiDasar"
                                                            ControlToValidate="txtKompetensiDasar$txtNama" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <ucl:AutocompleteKompetensiDasar runat="server" ID="txtKompetensiDasar" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboJenisPerhitunganKompetensiDasar.ClientID %>" style="text-transform: none;">
                                                            Jenis Perhitungan
                                                        </label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputKompetensiDasar" runat="server" ID="vldJenisPerhituganKD"
                                                            ControlToValidate="cboJenisPerhitunganKompetensiDasar" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList CssClass="form-control" runat="server" ID="cboJenisPerhitunganKompetensiDasar"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div runat="server" id="div_bobot_ap_dari_kd" class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtBobotAP.ClientID %>" style="text-transform: none;">
                                                            Bobot Aspek Penilaian %
                                                        </label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputKompetensiDasar" runat="server" ID="vldBobotAP"
                                                            ControlToValidate="txtBobotAP" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtBobotAP"></asp:TextBox>
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
                                <asp:LinkButton ValidationGroup="vldInputKompetensiDasar" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKKompetensiDasar" OnClick="lnkOKKompetensiDasar_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_komponen_penilaian" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Komponen Penilaian
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
                                                        <asp:Literal runat="server" ID="ltrKomponenPenilaian"></asp:Literal>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="div_bobot_kd_dari_kp" class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtBobotNKD.ClientID %>" style="text-transform: none;">Bobot n-KD %</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputKomponenPenilaian" runat="server" ID="vldBobotNKD"
                                                            ControlToValidate="txtBobotNKD" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtBobotNKD"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
					                                <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 5px;">
						                                <label for="<%= chkIsAdaPB.ClientID %>" style="color: #B7770D; font-size: small;">
                                                            <div class="checkbox switch" style="padding-bottom: 0px;">
									                            <label for="<%= chkIsAdaPB.ClientID %>" style="color: grey; font-weight: bold;">
										                            <input runat="server" class="access-hide" id="chkIsAdaPB" type="checkbox" />
										                            <span class="switch-toggle"></span>Ada Perbaikan Nilai
									                            </label>
								                            </div>
						                                </label>
					                                </div>
				                                </div>
                                            </div>
                                            <div runat="server" id="div_tampilan_lts" class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
					                                <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 15px;">
						                                <label for="<%= chkIsLTS.ClientID %>" style="color: #B7770D; font-size: small;">
                                                            <div class="checkbox switch" style="padding-bottom: 0px;">
									                            <label for="<%= chkIsLTS.ClientID %>" style="color: grey; font-weight: bold;">
										                            <input runat="server" class="access-hide" id="chkIsLTS" type="checkbox" />
										                            <span class="switch-toggle"></span>Tampilkan di LTS
									                            </label>
								                            </div>
						                                </label>
					                                </div>
				                                </div>
                                            </div>
                                            <div runat="server" id="div_kode_kd" class="row" style="margin-left: 30px; margin-right: 30px; margin-top: 15px;">
                                                <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 0px;">
                                                        <label class="label-input" for="<%= txtKodeKD.ClientID %>" style="color: grey; text-transform: none; margin-bottom: 6px;">
                                                            Kode
                                                        </label>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtKodeKD" Style="height: 30px;"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="div_materi_kp" class="row" style="margin-left: 30px; margin-right: 30px; margin-top: 15px;">
                                                <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 0px;">
                                                        <label class="label-input" for="<%= txtMateriKP.ClientID %>" style="color: grey; text-transform: none; margin-bottom: 6px;">
                                                            Kompetensi Dasar
                                                        </label>
                                                        <asp:TextBox CssClass="mcetiny_materi" TextMode="MultiLine" runat="server" ID="txtMateriKP" Style="outline: none; resize: none; height: 200px; width: 100%; border-style: solid; border-width: 1px; border-color: #d3d3d3; padding: 5px;"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="div_deskripsi_kp" class="row" style="margin-left: 30px; margin-right: 30px; margin-top: 15px;">
                                                <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 0px;">
                                                        <label class="label-input" for="<%= txtDeskripsiKP.ClientID %>" style="color: grey; text-transform: none; margin-bottom: 6px;">
                                                            Ringkasan Deskripsi
                                                        </label>
                                                        <asp:TextBox CssClass="mcetiny_deskripsi" TextMode="MultiLine" runat="server" ID="txtDeskripsiKP" Style="outline: none; resize: none; height: 200px; width: 100%; border-style: solid; border-width: 1px; border-color: #d3d3d3; padding: 5px;"></asp:TextBox>
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
                                <asp:LinkButton ValidationGroup="vldInputKomponenPenilaian" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKKomponenPenilaian" OnClick="lnkOKKomponenPenilaian_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>
            
            <div aria-hidden="true" class="modal fade" id="ui_modal_buka_semester" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Buka Semester
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
                                                            <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                <label class="label-input" for="<%= txtBukaSemester.ClientID %>" style="text-transform: none;">Semester</label>
                                                                <asp:TextBox Enabled="false" CssClass="form-control" runat="server" ID="txtBukaSemester"></asp:TextBox>
                                                                <br />
                                                                <label style="color: darkorange; width: 100%; text-align: justify;">
                                                                    <i class="fa fa-exclamation-triangle"></i>
                                                                    <label style="font-weight: bold; margin-bottom: 10px;">Perhatian</label>
                                                                    <br />
                                                                    Fitur ini digunakan saat awal semester untuk membuat template nilai baru dalam satu semester 
                                                                    berdasarkan template nilai pada semester sebelumnya.
                                                                </label>
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

                            <div class="row" id="pb_proses_buka_semester" style="display: none; margin-left: -24px; margin-right: -24px; background-color: #B50000; color: white;">
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

                            <div class="row" id="div_command_buka_semester" style="margin-left: 0px; margin-right: 0px;">
					            <p class="text-right">
                                    <asp:LinkButton OnClientClick="ShowProsesBukaSemester(true); ReportProcessBukaSemester(); return false;" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKBukaSemester" Text="  OK  "></asp:LinkButton>
                                    <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                    <br /><br />                              
					            </p>
                            </div>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_confirm_hapus" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                                            <asp:Literal runat="server" ID="ltrMsgConfirmHapus"></asp:Literal>
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapus" OnClick="lnkOKHapus_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_hapus_item_struktur_penilaian" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Hapus Data
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
                                                        <div class="col-xs-12" style="color: grey;">
                                                            
                                                            Anda yakin akan menghapus data yang dipilih?

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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapusItemStrukturPenilaian" OnClick="lnkOKHapusItemStrukturPenilaian_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_deskripsi_kurtilas" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Deskripsi Nilai
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 10px; margin-right: 10px;">
                                                <div class="col-xs-6" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboKelasKURTILAS.ClientID %>" style="text-transform: none;">Kelas</label>
                                                        <asp:DropDownList Enabled="false" CssClass="form-control" runat="server" ID="cboKelasKURTILAS" Visible="false">
                                                        </asp:DropDownList>
                                                        <asp:TextBox runat="server" ID="txtKelasKurtilas" Enabled="false" Style="outline: none; resize: none; background-color: white; padding: 5px;" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 10px; margin-right: 10px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboMapelKURTILAS.ClientID %>" style="text-transform: none;">Mata Pelajaran</label>
                                                        <asp:DropDownList Enabled="false" CssClass="form-control" runat="server" ID="cboMapelKURTILAS">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div> 
                                            <div class="row" style="margin-left: 10px; margin-right: 10px; margin-top: 15px;">
                                                <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 0px;">
                                                        <label class="label-input" for="<%= txtMateriKP.ClientID %>" style="color: grey; text-transform: none; margin-bottom: 6px;">
                                                            Deskripsi Pengetahuan
                                                        </label>
                                                        <asp:TextBox CssClass="mcetiny_deskripsi_pengetahuan" runat="server" ID="txtDeskripsiPengetahuan" TextMode="MultiLine" Style="outline: none; resize: none; height: 200px; width: 100%; border-style: solid; border-width: 1px; border-color: #d3d3d3; padding: 5px;"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 10px; margin-right: 10px; margin-top: 15px;">
                                                <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 0px;">
                                                        <label class="label-input" for="<%= txtDeskripsiKP.ClientID %>" style="color: grey; text-transform: none; margin-bottom: 6px;">
                                                            Deskripsi Keterampilan
                                                        </label>
                                                        <asp:TextBox CssClass="mcetiny_deskripsi_keterampilan" runat="server" ID="txtDeskripsiKeterampilan" TextMode="MultiLine" Style="outline: none; resize: none; height: 200px; width: 100%; border-style: solid; border-width: 1px; border-color: #d3d3d3; padding: 5px;"></asp:TextBox>
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKDeskripsiKURTILAS" OnClick="lnkOKDeskripsiKURTILAS_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_deskripsi_ktsp" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Deskripsi Nilai
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 10px; margin-right: 10px;">
                                                <div class="col-xs-6" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboKelasKTSP.ClientID %>" style="text-transform: none;">Kelas</label>
                                                        <asp:DropDownList Enabled="false" CssClass="form-control" runat="server" id="cboKelasKTSP" Visible="false">
                                                        </asp:DropDownList>
                                                        <asp:TextBox runat="server" ID="txtKelasKTSP" Enabled="false" Style="outline: none; resize: none; background-color: white; padding: 5px;" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 10px; margin-right: 10px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboMapelKTSP.ClientID %>" style="text-transform: none;">Mata Pelajaran</label>
                                                        <asp:DropDownList Enabled="false" CssClass="form-control" runat="server" ID="cboMapelKTSP">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div> 
                                            <div class="row" style="margin-left: 10px; margin-right: 10px; margin-top: 15px;">
                                                <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 0px;">
                                                        <label class="label-input" for="<%= txtDeskripsiMapel.ClientID %>" style="color: grey; text-transform: none; margin-bottom: 6px;">
                                                            Deskripsi Mata Pelajaran
                                                        </label>
                                                        <asp:TextBox CssClass="mcetiny_deskripsi_matapelajaran" runat="server" ID="txtDeskripsiMapel" TextMode="MultiLine" Style="outline: none; resize: none; height: 200px; width: 100%; border-style: solid; border-width: 1px; border-color: #d3d3d3; padding: 5px;"></asp:TextBox>
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKDeskripsiKTSP" OnClick="lnkOKDeskripsiKTSP_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_lihat_data" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Lihat Data
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
                                                        <div class="col-xs-12" style="color: grey;">
                                                            
                                                            <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                <label class="label-input" for="<%= cboPeriodeLihatData.ClientID %>" style="text-transform: none;">Pilih Periode</label>
                                                                <asp:DropDownList CssClass="form-control" runat="server" ID="cboPeriodeLihatData" style="color: black;"></asp:DropDownList>
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
					        <p class="text-right">
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKLihatData" OnClick="lnkOKLihatData_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>


            <div aria-hidden="true" class="modal fade" id="ui_modal_predikat_penilaian" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Predikat Penilaian
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">

                                                    <div class="row">
                                                        <div class="col-xs-12" style="color: grey; vertical-align: bottom">
                                                            Tahun Ajaran :         
                                                            <br />                                                   
                                                            <asp:Label runat="server" ID="lblTahunAjaranPredikat" style="font-weight: bold;"></asp:Label>
                                                            <hr style="margin-top: 5px; margin-bottom: 15px;" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-xs-12" style="color: grey; vertical-align: bottom">
                                                            Mata Pelajaran :
                                                            <br />
                                                            <asp:Label runat="server" ID="lblMapelPredikat" style="font-weight: bold;"></asp:Label>
                                                            <hr style="margin-top: 5px; margin-bottom: 15px;" />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-xs-12" style="color: grey; vertical-align: bottom">
                                                            Kelas :
                                                            <br />
                                                            <asp:Label runat="server" ID="lblKelasPredikat" style="font-weight: bold;"></asp:Label>
                                                            <hr style="margin-top: 5px; margin-bottom: 15px;" />
                                                        </div>
                                                        <div class="col-xs-6" style="color: grey; vertical-align: bottom; display: none;">
                                                            KKM :
                                                            <br />
                                                            <asp:Label runat="server" ID="lblKKM" style="font-weight: bold;"></asp:Label>
                                                            <hr style="margin-top: 5px; margin-bottom: 15px;" />
                                                        </div>
                                                    </div>
                                                    <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-3" style="color: grey; vertical-align: bottom">
                                                            Predikat
                                                        </div>
                                                        <div class="col-xs-2" style="color: grey; vertical-align: bottom">
                                                            Min.
                                                        </div>
                                                        <div class="col-xs-2" style="color: grey; vertical-align: bottom">
                                                            Maks.
                                                        </div>
                                                        <div class="col-xs-5" style="color: grey; vertical-align: bottom">
                                                            Deskripsi Rapor
                                                        </div>
                                                    </div>                                                    
                                                    <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-3" style="color: grey;">
                                                            <asp:TextBox runat="server" ID="txtPredikat1" Font-Bold="true" CssClass="text-input2"></asp:TextBox>
                                                        </div>
                                                        <div class="col-xs-2" style="color: grey;">
                                                            <asp:TextBox runat="server" ID="txtMinimal1" Font-Bold="true" CssClass="text-input2" style="text-align: right;"></asp:TextBox>
                                                        </div>
                                                        <div class="col-xs-2" style="color: grey;">
                                                            <asp:TextBox runat="server" ID="txtMaksimal1" Font-Bold="true" CssClass="text-input2" style="text-align: right;"></asp:TextBox>
                                                        </div>
                                                        <div class="col-xs-5" style="color: grey;">
                                                            <asp:TextBox runat="server" ID="txtDeskripsi1" Font-Bold="true" CssClass="text-input2" style="text-align: left;"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-3" style="color: grey;">
                                                            <asp:TextBox runat="server" ID="txtPredikat2" Font-Bold="true" CssClass="text-input2"></asp:TextBox>
                                                        </div>
                                                        <div class="col-xs-2" style="color: grey;">
                                                            <asp:TextBox runat="server" ID="txtMinimal2" Font-Bold="true" CssClass="text-input2" style="text-align: right;"></asp:TextBox>
                                                        </div>
                                                        <div class="col-xs-2" style="color: grey;">
                                                            <asp:TextBox runat="server" ID="txtMaksimal2" Font-Bold="true" CssClass="text-input2" style="text-align: right;"></asp:TextBox>
                                                        </div>
                                                        <div class="col-xs-5" style="color: grey;">
                                                            <asp:TextBox runat="server" ID="txtDeskripsi2" Font-Bold="true" CssClass="text-input2" style="text-align: left;"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-3" style="color: grey;">
                                                            <asp:TextBox runat="server" ID="txtPredikat3" Font-Bold="true" CssClass="text-input2"></asp:TextBox>
                                                        </div>
                                                        <div class="col-xs-2" style="color: grey;">
                                                            <asp:TextBox runat="server" ID="txtMinimal3" Font-Bold="true" CssClass="text-input2" style="text-align: right;"></asp:TextBox>
                                                        </div>
                                                        <div class="col-xs-2" style="color: grey;">
                                                            <asp:TextBox runat="server" ID="txtMaksimal3" Font-Bold="true" CssClass="text-input2" style="text-align: right;"></asp:TextBox>
                                                        </div>
                                                        <div class="col-xs-5" style="color: grey;">
                                                            <asp:TextBox runat="server" ID="txtDeskripsi3" Font-Bold="true" CssClass="text-input2" style="text-align: left;"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-3" style="color: grey;">
                                                            <asp:TextBox runat="server" ID="txtPredikat4" Font-Bold="true" CssClass="text-input2"></asp:TextBox>
                                                        </div>
                                                        <div class="col-xs-2" style="color: grey;">
                                                            <asp:TextBox runat="server" ID="txtMinimal4" Font-Bold="true" CssClass="text-input2" style="text-align: right;"></asp:TextBox>
                                                        </div>
                                                        <div class="col-xs-2" style="color: grey;">
                                                            <asp:TextBox runat="server" ID="txtMaksimal4" Font-Bold="true" CssClass="text-input2" style="text-align: right;"></asp:TextBox>
                                                        </div>
                                                        <div class="col-xs-5" style="color: grey;">
                                                            <asp:TextBox runat="server" ID="txtDeskripsi4" Font-Bold="true" CssClass="text-input2" style="text-align: left;"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-3" style="color: grey;">
                                                            <asp:TextBox runat="server" ID="txtPredikat5" Font-Bold="true" CssClass="text-input2"></asp:TextBox>
                                                        </div>
                                                        <div class="col-xs-2" style="color: grey;">
                                                            <asp:TextBox runat="server" ID="txtMinimal5" Font-Bold="true" CssClass="text-input2" style="text-align: right;"></asp:TextBox>
                                                        </div>
                                                        <div class="col-xs-2" style="color: grey;">
                                                            <asp:TextBox runat="server" ID="txtMaksimal5" Font-Bold="true" CssClass="text-input2" style="text-align: right;"></asp:TextBox>
                                                        </div>
                                                        <div class="col-xs-5" style="color: grey;">
                                                            <asp:TextBox runat="server" ID="txtDeskripsi5" Font-Bold="true" CssClass="text-input2" style="text-align: left;"></asp:TextBox>
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
                                <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKPredikatPenilaian" OnClick="lnkOKPredikatPenilaian_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkOKLihatData" />
        </Triggers>
    </asp:UpdatePanel>

    <iframe name="fra_download" id="fra_download" height="0" width="0" style="position: absolute; left: -1000px; top: -1000px;"></iframe>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
        function LoadTinyMCEDeskripsi() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                selector: ".mcetiny_deskripsi",
                theme: "modern",
                plugins: [
                    "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                    "searchreplace wordcount visualblocks visualchars code fullscreen",
                    "insertdatetime media nonbreaking save table contextmenu directionality",
                    "emoticons template paste textcolor tinyfilemanager.net"
                ],
                toolbar1: "bold italic underline",
                image_advtab: true,
                templates: [
                    { title: 'Test template 1', content: 'Test 1' },
                    { title: 'Test template 2', content: 'Test 2' }
                ],
                resize: "vertical",
                statusbar: false,
                menubar: false,
                height: 150,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtDeskripsiKPVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
        }

        function LoadTinyMCEMateri() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                selector: ".mcetiny_materi",
                theme: "modern",
                plugins: [
                    "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                    "searchreplace wordcount visualblocks visualchars code fullscreen",
                    "insertdatetime media nonbreaking save table contextmenu directionality",
                    "emoticons template paste textcolor tinyfilemanager.net"
                ],
                toolbar1: "bold italic underline",
                image_advtab: true,
                templates: [
                    { title: 'Test template 1', content: 'Test 1' },
                    { title: 'Test template 2', content: 'Test 2' }
                ],
                resize: "vertical",
                statusbar: false,
                menubar: false,
                height: 150,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtMateriKPVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
        }

        function LoadTinyMCEDeskripsiPengetahuan() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                selector: ".mcetiny_deskripsi_pengetahuan",
                theme: "modern",
                plugins: [
                    "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                    "searchreplace wordcount visualblocks visualchars code fullscreen",
                    "insertdatetime media nonbreaking save table contextmenu directionality",
                    "emoticons template paste textcolor tinyfilemanager.net"
                ],
                toolbar1: "bold italic underline",
                image_advtab: true,
                templates: [
                    { title: 'Test template 1', content: 'Test 1' },
                    { title: 'Test template 2', content: 'Test 2' }
                ],
                resize: "vertical",
                statusbar: false,
                menubar: false,
                height: 150,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtDeskripsiPengetahuanVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
        }

        function LoadTinyMCEDeskripsiKeterampilan() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                selector: ".mcetiny_deskripsi_keterampilan",
                theme: "modern",
                plugins: [
                    "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                    "searchreplace wordcount visualblocks visualchars code fullscreen",
                    "insertdatetime media nonbreaking save table contextmenu directionality",
                    "emoticons template paste textcolor tinyfilemanager.net"
                ],
                toolbar1: "bold italic underline",
                image_advtab: true,
                templates: [
                    { title: 'Test template 1', content: 'Test 1' },
                    { title: 'Test template 2', content: 'Test 2' }
                ],
                resize: "vertical",
                statusbar: false,
                menubar: false,
                height: 150,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtDeskripsiKeterampilanVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
        }

        function LoadTinyMCEDeskripsiMataPelajaran() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                selector: ".mcetiny_deskripsi_matapelajaran",
                theme: "modern",
                plugins: [
                    "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                    "searchreplace wordcount visualblocks visualchars code fullscreen",
                    "insertdatetime media nonbreaking save table contextmenu directionality",
                    "emoticons template paste textcolor tinyfilemanager.net"
                ],
                toolbar1: "bold italic underline",
                image_advtab: true,
                templates: [
                    { title: 'Test template 1', content: 'Test 1' },
                    { title: 'Test template 2', content: 'Test 2' }
                ],
                resize: "vertical",
                statusbar: false,
                menubar: false,
                height: 150,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtDeskripsiMapelVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
        }

        
        function LoadTinyMCEDeskripsiSikapSpiritual() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                mode : "exact",
                selector: ".mcetiny_deskripsi_sikap_spiritual",
                theme: "modern",
                plugins: [
                    "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                    "searchreplace wordcount visualblocks visualchars code fullscreen",
                    "insertdatetime media nonbreaking save table contextmenu directionality",
                    "emoticons template paste textcolor tinyfilemanager.net"
                ],
                toolbar1: "bold italic underline | bullist numlist",
                image_advtab: true,
                templates: [
                    { title: 'Test template 1', content: 'Test 1' },
                    { title: 'Test template 2', content: 'Test 2' }
                ],
                resize: "vertical",
                statusbar: false,
                menubar: false,
                height: 120,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtDeskripsiSikapSpiritualVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
        }

        function LoadTinyMCEDeskripsiSikapSosial() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                mode : "exact",
                selector: ".mcetiny_deskripsi_sikap_sosial",
                theme: "modern",
                plugins: [
                    "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                    "searchreplace wordcount visualblocks visualchars code fullscreen",
                    "insertdatetime media nonbreaking save table contextmenu directionality",
                    "emoticons template paste textcolor tinyfilemanager.net"
                ],
                toolbar1: "bold italic underline | bullist numlist",
                image_advtab: true,
                templates: [
                    { title: 'Test template 1', content: 'Test 1' },
                    { title: 'Test template 2', content: 'Test 2' }
                ],
                resize: "vertical",
                statusbar: false,
                menubar: false,
                height: 120,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtDeskripsiSikapSosialVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
        }

        function RemoveTinyMCE(){
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtDeskripsiKP.ClientID %>');
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtMateriKP.ClientID %>');
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtDeskripsiMapel.ClientID %>');
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtDeskripsiPengetahuan.ClientID %>');
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtDeskripsiKeterampilan.ClientID %>');
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtDeskripsiSikapSpiritual.ClientID %>');
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtDeskripsiSikapSosial.ClientID %>');        
        }
    </script>

    <script type="text/javascript">
        RenderDropDownOnTables();
        InitModalFocus();
        <%= txtAspekPenilaian.NamaClientID %>_SHOW_AUTOCOMPLETE();
        <%= txtKompetensiDasar.NamaClientID %>_SHOW_AUTOCOMPLETE();
    </script>
</asp:Content>
