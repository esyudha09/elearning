<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="wf.StrukturPenilaian.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Penilaian.SMA.wf_StrukturPenilaian" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<%@ Register TagPrefix="ucl" TagName="AutocompleteAspekPenilaian" Src="~/Application_Controls/Elearning/SMA/AutocompleteAspekPenilaian/AutocompleteAspekPenilaian.ascx" %>
<%@ Register TagPrefix="ucl" TagName="AutocompleteKompetensiDasar" Src="~/Application_Controls/Elearning/SMA/AutocompleteKompetensiDasar/AutocompleteKompetensiDasar.ascx" %>
<%@ Register TagPrefix="ucl" TagName="AutocompleteKomponenPenilaian" Src="~/Application_Controls/Elearning/SMA/AutocompleteKomponenPenilaian/AutocompleteKomponenPenilaian.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .input-box {
            border-style: solid;
            border-width: 1px;
            border-color: #DBDBDB;
            padding: 6px;
            outline : none;
            width: 100%;
            margin-top: 5px;
            font-size: small;
        }

        .text-input {
            display: inline-block;
            padding: 6px;
            padding-left: 15px;
            padding-right: 15px;
            margin: 0;
            outline: 0;
            background-color: #F5F8FA;
            border: 1px solid #E6ECF0;
            border-radius: 30px;
            font-size: small;
            width: 100%;
        }

        .div-like-text-input {
            display: inline-block;
            padding: 6px;
            padding-left: 15px;
            padding-right: 15px;
            margin: 0;
            outline: 0;
            background-color: #F5F8FA;
            border: 1px solid #E6ECF0;
            border-radius: 30px;
            font-size: small;
            width: 100%;
            border-radius: 6px;
        }

        .reset-this > p {
            margin: 0px;
        }

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

        .reset-this {
            animation : none;
            animation-delay : 0;
            animation-direction : normal;
            animation-duration : 0;
            animation-fill-mode : none;
            animation-iteration-count : 1;
            animation-name : none;
            animation-play-state : running;
            animation-timing-function : ease;
            backface-visibility : visible;
            background : 0;
            background-attachment : scroll;
            background-clip : border-box;
            background-color : transparent;
            background-image : none;
            background-origin : padding-box;
            background-position : 0 0;
            background-position-x : 0;
            background-position-y : 0;
            background-repeat : repeat;
            background-size : auto auto;
            border : 0;
            border-style : none;
            border-width : medium;
            border-color : inherit;
            border-bottom : 0;
            border-bottom-color : inherit;
            border-bottom-left-radius : 0;
            border-bottom-right-radius : 0;
            border-bottom-style : none;
            border-bottom-width : medium;
            border-collapse : separate;
            border-image : none;
            border-left : 0;
            border-left-color : inherit;
            border-left-style : none;
            border-left-width : medium;
            border-radius : 0;
            border-right : 0;
            border-right-color : inherit;
            border-right-style : none;
            border-right-width : medium;
            border-spacing : 0;
            border-top : 0;
            border-top-color : inherit;
            border-top-left-radius : 0;
            border-top-right-radius : 0;
            border-top-style : none;
            border-top-width : medium;
            bottom : auto;
            box-shadow : none;
            box-sizing : content-box;
            caption-side : top;
            clear : none;
            clip : auto;
            color : inherit;
            columns : auto;
            column-count : auto;
            column-fill : balance;
            column-gap : normal;
            column-rule : medium none currentColor;
            column-rule-color : currentColor;
            column-rule-style : none;
            column-rule-width : none;
            column-span : 1;
            column-width : auto;
            content : normal;
            counter-increment : none;
            counter-reset : none;
            cursor : auto;
            direction : ltr;
            display : inline;
            empty-cells : show;
            float : none;
            font : normal;
            font-family : inherit;
            font-size : medium;
            font-style : normal;
            font-variant : normal;
            font-weight : normal;
            height : auto;
            hyphens : none;
            left : auto;
            letter-spacing : normal;
            line-height : normal;
            list-style : none;
            list-style-image : none;
            list-style-position : outside;
            list-style-type : disc;
            margin : 0;
            margin-bottom : 0;
            margin-left : 0;
            margin-right : 0;
            margin-top : 0;
            max-height : none;
            max-width : none;
            min-height : 0;
            min-width : 0;
            opacity : 1;
            orphans : 0;
            outline : 0;
            outline-color : invert;
            outline-style : none;
            outline-width : medium;
            overflow : visible;
            overflow-x : visible;
            overflow-y : visible;
            padding : 0;
            padding-bottom : 0;
            padding-left : 0;
            padding-right : 0;
            padding-top : 0;
            page-break-after : auto;
            page-break-before : auto;
            page-break-inside : auto;
            perspective : none;
            perspective-origin : 50% 50%;
            position : static;
            /* May need to alter quotes for different locales (e.g fr) */
            quotes : '\201C' '\201D' '\2018' '\2019';
            right : auto;
            tab-size : 8;
            table-layout : auto;
            text-align : inherit;
            text-align-last : auto;
            text-decoration : none;
            text-decoration-color : inherit;
            text-decoration-line : none;
            text-decoration-style : solid;
            text-indent : 0;
            text-shadow : none;
            text-transform : none;
            top : auto;
            transform : none;
            transform-style : flat;
            transition : none;
            transition-delay : 0s;
            transition-duration : 0s;
            transition-property : none;
            transition-timing-function : ease;
            unicode-bidi : normal;
            vertical-align : baseline;
            visibility : visible;
            white-space : normal;
            widows : 0;
            width : auto;
            word-spacing : normal;
            z-index : auto;
            /* basic modern patch */
            all: initial;
            all: unset;
        }

        /* basic modern patch */

        #reset-this-root {
            all: initial;
            * {
                all: unset;
            }
        }
    </style>
    <script type="text/javascript">
        function HideModal(){
            $('#ui_modal_input_data').modal('hide');
            $('#ui_modal_confirm_hapus').modal('hide');   
            $('#ui_modal_input_aspek_penilaian').modal('hide');    
            $('#ui_modal_input_kompetensi_dasar_kurtilas').modal('hide');    
            $('#ui_modal_input_kompetensi_dasar').modal('hide');    
            $('#ui_modal_input_komponen_penilaian').modal('hide'); 
            $('#ui_modal_predikat_penilaian').modal('hide');     
            $('#ui_modal_input_kompetensi_dasar_sikap').modal('hide');                 
            $('#ui_modal_deskripsi_lts_rapor').modal('hide');  
            $('#ui_modal_preview').modal('hide');  
            
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
                    ValidateInputByKurikulum();
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowPreviewNilai %>":
                    $('#ui_modal_preview').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.AddWithMessage %>":
                    HideModal();
                    ReInitTinyMCE();
                    ValidateInputByKurikulum();
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
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
                    LoadTinyMCEAspekPenilaian();
                    $('#ui_modal_input_aspek_penilaian').modal({ backdrop: 'static', keyboard: false, show: true });
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.AddKDKurtilasWithMessage %>":
                    HideModal();   
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
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.AddKPWithMessage %>":
                    HideModal();
                    LoadTinyMCEKomponenPenilaian();
                    $('#ui_modal_input_komponen_penilaian').modal({ backdrop: 'static', keyboard: false, show: true });
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;                    
                case "<%= JenisAction.AddKPWithMessageKURTILAS %>":
                    HideModal();
                    LoadTinyMCEKomponenPenilaianKURTILAS();
                    $('#ui_modal_input_komponen_penilaian_kurtilas').modal({ backdrop: 'static', keyboard: false, show: true });
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
                case "<%= JenisAction.DoShowData %>":
                    ReInitTinyMCE();
                    ValidateInputByKurikulum();
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });                    
                    break;
                case "<%= JenisAction.DoShowConfirmHapus %>":
                    $('#ui_modal_confirm_hapus').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowInputAspekPenilaian %>":
                    LoadTinyMCEAspekPenilaian();
                    $('#ui_modal_input_aspek_penilaian').modal({ backdrop: 'static', keyboard: false, show: true });                    
                    break;
                case "<%= JenisAction.DoShowInputKompetensiDasar %>":
                    LoadTinyMCEKompetensiDasar();
                    $('#ui_modal_input_kompetensi_dasar').modal({ backdrop: 'static', keyboard: false, show: true });                    
                    break;
                case "<%= JenisAction.DoShowInputKompetensiDasarKurtilas %>":
                    $('#ui_modal_input_kompetensi_dasar_kurtilas').modal({ backdrop: 'static', keyboard: false, show: true });                    
                    break;
                case "<%= JenisAction.DoShowInputKomponenPenilaian %>":
                    LoadTinyMCEKomponenPenilaian();
                    $('#ui_modal_input_komponen_penilaian').modal({ backdrop: 'static', keyboard: false, show: true });                    
                    break;
                case "<%= JenisAction.DoShowInputKomponenPenilaianKURTILAS %>":
                    LoadTinyMCEKomponenPenilaianKURTILAS();
                    $('#ui_modal_input_komponen_penilaian_kurtilas').modal({ backdrop: 'static', keyboard: false, show: true });                    
                    break;
                case "<%= JenisAction.DoShowInputKompetensiDasarKurtilasSikap %>":
                    ReInitTinyMCE();   
                    $('#ui_modal_input_kompetensi_dasar_sikap').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowBukaSemester %>":
                    $('#ui_modal_buka_semester').modal({ backdrop: 'static', keyboard: false, show: true });                    
                    break;
                case "<%= JenisAction.DoShowLihatData %>":
                    $('#ui_modal_lihat_data').modal({ backdrop: 'static', keyboard: false, show: true });                    
                    break;       
                case "<%= JenisAction.DoShowInputDeskripsiLTSRapor %>":
                    ReInitTinyMCE();   
                    $('#ui_modal_deskripsi_lts_rapor').modal({ backdrop: 'static', keyboard: false, show: true });                    
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
                case "<%= JenisAction.DoShowInfoAdaStrukturNilai %>":
                    HideModal();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Pengaturan struktur nilai sudah ada',
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

            ShowProgress(false);
        }

        function InitModalFocus(){
            $('#ui_modal_input_data').on('shown.bs.modal', function () {
                <%= txtTahunAjaran.ClientID %>.focus();
            });
            $('#ui_modal_input_aspek_penilaian').on('shown.bs.modal', function () {
                <%= txtPoinAspekPenilaian.ClientID %>.focus();
            });
            $('#ui_modal_input_kompetensi_dasar').on('shown.bs.modal', function () {
                <%= txtPoinKompetensiDasar.ClientID %>.focus();
            });
            $('#ui_modal_input_komponen_penilaian').on('shown.bs.modal', function () {
                tinyMCE.execCommand('mceFocus',false,'<%= txtKomponenPenilaian.ClientID %>');
            });
            $('#ui_modal_input_kompetensi_dasar_kurtilas').on('shown.bs.modal', function () {
                <%= txtPoinKompetensiDasarKURTILAS.ClientID %>.focus();
            });
            $('#ui_modal_input_komponen_penilaian_kurtilas').on('shown.bs.modal', function () {
                tinyMCE.execCommand('mceFocus',false,'<%= txtKomponenPenilaianKURTILAS.ClientID %>');
            });
            $('#ui_modal_predikat_penilaian').on('shown.bs.modal', function () {
                <%= txtPredikat1.ClientID %>.focus();
            });
            $('#ui_modal_input_kompetensi_dasar_sikap').on('shown.bs.modal', function () {
                <%= txtPoinKompetensiDasarSikap.ClientID %>.focus();
            });
            $('#ui_modal_deskripsi_lts_rapor').on('shown.bs.modal', function () {
                tinyMCE.execCommand('mceFocus',false,'<%= txtDeskripsiLTSRapor.ClientID %>');
            });
        }

        function ParseSelectedItemStrukturPenilaian(){
            var txt_aspek_penilaian = document.getElementById("<%= txtParseIDAspekPenilaian.ClientID %>");
            var txt_komp_dasar = document.getElementById("<%= txtParseIDKompetensiDasar.ClientID %>");
            var txt_komp_dasar_sikap = document.getElementById("<%= txtParseIDKompetensiDasarSikap.ClientID %>");
            var txt_komp_penilaian = document.getElementById("<%= txtParseIDKomponenPenilaian.ClientID %>");

            if(
                txt_aspek_penilaian != null && txt_aspek_penilaian != undefined && 
                txt_komp_dasar != null && txt_komp_dasar != undefined && 
                txt_komp_dasar_sikap != null && txt_komp_dasar_sikap != undefined && 
                txt_komp_penilaian != null && txt_komp_penilaian != undefined
            ){
                txt_aspek_penilaian.value = "";
                txt_komp_dasar.value = "";
                txt_komp_dasar_sikap.value = "";
                txt_komp_penilaian.value = "";

                var arr_chk_aspek_penilaian = document.getElementsByName("chk_ap[]");
                var arr_chk_kom_dasar = document.getElementsByName("chk_kd[]");
                var arr_chk_kom_dasar_sikap = document.getElementsByName("chk_kd_sikap[]");
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

                if(arr_chk_kom_dasar_sikap.length > 0){
                    for (var i = 0; i < arr_chk_kom_dasar_sikap.length; i++) {
                        if(arr_chk_kom_dasar_sikap[i].checked){
                            txt_komp_dasar_sikap.value += arr_chk_kom_dasar_sikap[i].value + ";";
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

        function ReInitTinyMCE(){
            RemoveTinyMCE();
            LoadTinyMCEDeskripsi();     
            LoadTinyMCEKompetensiDasarSikapKURTILAS();       
            LoadTinyMCEDeskripsiLTSRapor();       
            LoadTinyMCEDeskripsiPerMapel();
            LoadTinyMCEDeskripsiSikapSpiritual();
            LoadTinyMCEDeskripsiSikapSosial();
        }

        function ShowConfirmHapusItemStrukturNilai(){
            ParseSelectedItemStrukturPenilaian();
            $('#ui_modal_hapus_item_struktur_penilaian').modal({ backdrop: 'static', keyboard: false, show: true });
        }

        function ShowInputBobotRaporPengetahuan(show){
            var div_caption = document.getElementById("div_bobot_rapor_pengetahuan_caption");
            var div_input = document.getElementById("div_bobot_rapor_pengetahuan_input");
            if(
                div_caption != null && div_caption != undefined &&
                div_input != null && div_input != undefined
            ){
                div_caption.style.display = (show === true ? "" : "none");
                div_input.style.display = (show === true ? "" : "none");
            }
        }

        function ShowInputPilihanEkskul(show){
            var div_pilihan_ekskul = document.getElementById("div_pilihan_ekskul");
            if(
                div_pilihan_ekskul != null && div_pilihan_ekskul != undefined
            ){
                div_pilihan_ekskul.style.display = (show === true ? "" : "none");
            }
        }

        function ValidateInputByKurikulum(){
            var cbo = document.getElementById("<%= cboKurikulum.ClientID %>");
            var cbo_mapel = document.getElementById("<%= cboMapel.ClientID %>");
            var txt_kkm = document.getElementById("<%= txtKKM.ClientID %>");
            var chk_nilai_akhir = document.getElementById("<%= chkIsNilaiAkhir.ClientID %>");
            var s_sikap = "Sikap&nbsp;&nbsp;→&nbsp;&nbsp;Sikap";

            div_deskripsi.style.display = "";
            div_deskripsi_sikap.style.display = "none";
            div_kkm.style.display = "";
            cbo_mapel.disabled = false;

            if(cbo != null && cbo != undefined){
                if(cbo.value === "<%= AI_ERP.Application_Libs.Libs.JenisKurikulum.SMA.KTSP %>"){
                    txt_kkm.value = "0";
                    div_kkm.style.display = "none";
                    ShowInputBobotRaporPengetahuan(false);
                }
                else if(cbo.value === "<%= AI_ERP.Application_Libs.Libs.JenisKurikulum.SMA.KURTILAS %>"){
                    ShowInputBobotRaporPengetahuan(true);
                }
                else if(cbo.value === "<%= AI_ERP.Application_Libs.Libs.JenisKurikulum.SMA.KURTILAS_SIKAP %>"){
                    if(cbo_mapel.options.length > 0){
                        for (var i = 0; i < cbo_mapel.options.length; i++) {
                            var text = cbo_mapel.options[i].innerHTML;
                            if(text === s_sikap){
                                cbo_mapel.value = "";
                                break;
                            }
                        }
                    }

                    chk_nilai_akhir.checked = true;
                    cbo_mapel.disabled = true;
                    if(cbo_mapel.options.length > 0){
                        for (var i = 0; i < cbo_mapel.options.length; i++) {
                            var text = cbo_mapel.options[i].innerHTML;
                            if(text === s_sikap){
                                cbo_mapel.options[i].selected = true;
                                break;
                            }
                        }
                    }
                    txt_kkm.value = "0";
                    div_deskripsi.style.display = "none";
                    div_kkm.style.display = "none";
                    div_deskripsi_sikap.style.display = "";
                    ShowInputBobotRaporPengetahuan(false);
                }
                else {
                    ShowInputBobotRaporPengetahuan(false);
                }

                if(cbo.options[cbo.selectedIndex].text.trim().toLowerCase().indexOf("ekstrakurikuler") >= 0){
                    ShowInputPilihanEkskul(true);
                }
                else {
                    ShowInputPilihanEkskul(false);
                }        
            }
        }

        function ValidateInputKDKurtilas(){
            var rdo = document.getElementsByName("rdo_kd_kurtilas[]");
            var div = document.getElementById("div_info_kd_kurtilas");

            if(div != null && div != undefined){
                if(rdo.length > 0){
                    for (var i = 0; i < rdo.length; i++) {
                        if(rdo[i].checked){
                            div.style.display = "none";

                            if(Page_ClientValidate('vldInputKompetensiDasarKURTILAS')){ ShowProgress(true); } 
                            return true;
                        }
                    }
                }
            }

            div.style.display = "";
            return false;
        }

        function ValidateInputKDKurtilasNoProgress(){
            var rdo = document.getElementsByName("rdo_kd_kurtilas[]");
            var div = document.getElementById("div_info_kd_kurtilas");

            if(div != null && div != undefined){
                if(rdo.length > 0){
                    for (var i = 0; i < rdo.length; i++) {
                        if(rdo[i].checked){
                            div.style.display = "none";
                            return true;
                        }
                    }
                }
            }

            div.style.display = "";
            return false;
        }

        function SetPilihStrukturKD(){
            var rdo = document.getElementsByName("rdo_kd_kurtilas[]");
            var div = document.getElementById("div_info_kd_kurtilas");
            var s_kode = "";

            if(div != null && div != undefined){
                if(rdo.length > 0){
                    for (var i = 0; i < rdo.length; i++) {
                        if(rdo[i].checked){
                            s_kode = rdo[i].value;
                            break;
                        }
                    }
                }
            }

            var txt = document.getElementById("<%= txtIDRelKompetensiDasar.ClientID %>");
            if(txt != null && txt != undefined){
                txt.value = s_kode;
            }
        }

        function SetAttrDeskripsi(kode, jenis, id_deskripsi){
            var txt_kode_des = document.getElementById("<%= txtKodeDeskripsi.ClientID %>");                
            var txt_jenis_des = document.getElementById("<%= txtJenisDeskripsi.ClientID %>");
            var txt_des = document.getElementById("<%= txtIDTeksDeskripsi.ClientID %>");
            if(
                    txt_kode_des != null && txt_kode_des != undefined &&
                    txt_jenis_des != null && txt_jenis_des != undefined &&
                    txt_des != null && txt_des != undefined
            ){
                txt_kode_des.value = kode;
                txt_jenis_des.value = jenis;
                txt_des.value = id_deskripsi;                
            }
        }

        function DoAutoSave(){
            setInterval(function(){
                var txt = document.getElementById("<%= txtIsAutoSave.ClientID %>");
                var txt_kode_des = document.getElementById("<%= txtKodeDeskripsi.ClientID %>");                
                var txt_jenis_des = document.getElementById("<%= txtJenisDeskripsi.ClientID %>");
                var txt_des = document.getElementById(document.getElementById("<%= txtIDTeksDeskripsi.ClientID %>").value);

                if(
                    txt != null && txt != undefined &&
                    txt_kode_des != null && txt_kode_des != undefined &&
                    txt_jenis_des != null && txt_jenis_des != undefined &&
                    txt_des != null && txt_des != undefined
                ){                    
                    if(txt.value == "1"){
                        var s_kode = txt_kode_des.value;
                        var s_jenis = txt_jenis_des.value;
                        var s_deskripsi = txt_des.value;
                        var s_ssid = "<%= AI_ERP.Application_Libs.Libs.Enkrip(AI_ERP.Application_Libs.Libs.LOGGED_USER_M.NoInduk) %>";
                        var s_url = "<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APIS.SMA.STRUKTUR_NILAI_SISWA.DO_SAVE.FILE) %>/Do?" +
                                    "k=" + s_kode + "&" +
                                    "j=" + s_jenis + "&" +
                                    "d=" + s_deskripsi + "&" +
                                    "ssid=" + s_ssid;

                        $.ajax({
                            url: s_url,
                            dataType: 'json',
                            type: 'GET', 
                            contentType: 'application/json; charset=utf-8', 
                            success: function(data) { 
                                }, 
                            error: function(response) { 
                                    alert(response.responseText); 
                                }, 
                            failure: function(response) { 
                                    alert(response.responseText); 
                                } 
                        });
                        
                        txt.value = "0";
                    }
                }
            }, 500);            
        }

        function SetIsAutosave(value){
            var txt = document.getElementById("<%= txtIsAutoSave.ClientID %>");
            if(txt != null && txt != undefined){
                txt.value = value;
            }
        }

        function ReportProcessBukaSemester() {
            var url = "<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.SD.BUKA_SEMESTER.ROUTE) %>";
            
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

        function ShowProgress(value){
            if(value){
                pb_top.style.display = "";
            } else {
                pb_top.style.display = "none";
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress runat="server" ID="upProgressMain" AssociatedUpdatePanelID="upMain">
        <ProgressTemplate>
            <div style="background: rgba(0, 0, 0, 0.7); position: fixed; left: 0px; top: 0px; bottom: 0px; right: 0px; z-index: 9999999999">
                <div class="progress progress-position-absolute-top" style="position: fixed; top: 0px; right: 0px; z-index: 9999999;">
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
                <div style="margin: 0 auto; display: table; color: white; padding-top: 50px; font-weight: bold;">
                    <i class="fa fa-hourglass-o"></i>&nbsp;&nbsp;&nbsp;Sedang Proses...
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div id="pb_top" style="display: none; position: fixed; left: 0px; top: 0px; bottom: 0px; right: 0px; z-index: 9999999999">
        <div class="progress progress-position-absolute-top" style="position: fixed; top: 0px; right: 0px; z-index: 9999999;">
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
    </div>

    <asp:UpdatePanel runat="server" ID="upMain">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="txtKeyAction" />
            <asp:HiddenField runat="server" ID="txtID" />
            <asp:HiddenField runat="server" ID="txtIDAspekPenilaian" />
            <asp:HiddenField runat="server" ID="txtIDRelKompetensiDasar" />
            <asp:HiddenField runat="server" ID="txtIDKompetensiDasar" />
            <asp:HiddenField runat="server" ID="txtIDKompetensiDasarSikap" />
            <asp:HiddenField runat="server" ID="txtIDKomponenPenilaian" />
            <asp:HiddenField runat="server" ID="txtParseIDAspekPenilaian" />
            <asp:HiddenField runat="server" ID="txtParseIDKompetensiDasar" />
            <asp:HiddenField runat="server" ID="txtParseIDKompetensiDasarSikap" />
            <asp:HiddenField runat="server" ID="txtParseIDKomponenPenilaian" />
            <asp:HiddenField runat="server" ID="txtNamaVal" />
            <asp:HiddenField runat="server" ID="txtAspekPenilaianVal" />        
            <asp:HiddenField runat="server" ID="txtKompetensiDasarVal" />
            <asp:HiddenField runat="server" ID="txtKomponenPenilaianVal" />  
            <asp:HiddenField runat="server" ID="txtKompetensiDasarSikapKURTILASVal" />
            <asp:HiddenField runat="server" ID="txtKomponenPenilaianKURTILASVal" />                                      
            <asp:HiddenField runat="server" ID="txtIsAutoSave" />
            <asp:HiddenField runat="server" ID="txtKodeDeskripsi" />
            <asp:HiddenField runat="server" ID="txtJenisDeskripsiRaporLTS" />
            <asp:HiddenField runat="server" ID="txtIDTeksDeskripsi" />
            <asp:HiddenField runat="server" ID="txtDeskripsiKompetensiDasarSikapVal" />
            <asp:HiddenField runat="server" ID="txtDeskripsiLTSRaporVal" />
            <asp:HiddenField runat="server" ID="txtDeskripsiPerMapelVal" />
            <asp:HiddenField runat="server" ID="txtDeskripsiSikapSosialVal" />
            <asp:HiddenField runat="server" ID="txtDeskripsiSikapSpiritualVal" />

            <asp:HiddenField runat="server" ID="txtKodePredikat1" />
            <asp:HiddenField runat="server" ID="txtKodePredikat2" />
            <asp:HiddenField runat="server" ID="txtKodePredikat3" />
            <asp:HiddenField runat="server" ID="txtKodePredikat4" />
            <asp:HiddenField runat="server" ID="txtKodePredikat5" />

            <asp:HiddenField runat="server" ID="txtYpos" />
            <asp:HiddenField runat="server" ID="txtXpos" />

            <asp:HiddenField runat="server" ID="txtTahunAjaranNew" />
            <asp:HiddenField runat="server" ID="txtSemesterNew" />
            <asp:HiddenField runat="server" ID="txtTahunAjaranOld" />
            <asp:HiddenField runat="server" ID="txtSemesterOld" />
            <asp:HiddenField runat="server" ID="txtIDDeskripsi" />
            <asp:HiddenField runat="server" ID="txtJenisDeskripsi" />

            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoCari" OnClick="btnDoCari_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDetail" OnClick="btnShowDetail_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowConfirmDelete" OnClick="btnShowConfirmDelete_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowStrukturNilai" OnClick="btnShowStrukturNilai_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputOnStrukturPenilaian" OnClick="btnShowInputOnStrukturPenilaian_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputAspekPenilaian" OnClick="btnShowInputAspekPenilaian_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputEditAspekPenilaian" OnClick="btnShowInputEditAspekPenilaian_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputKompetensiDasar" OnClick="btnShowInputKompetensiDasar_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputEditKompetensiDasar" OnClick="btnShowInputEditKompetensiDasar_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputKompetensiDasarKURTILAS" OnClick="btnShowInputKompetensiDasarKURTILAS_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputEditKompetensiDasarKURTILAS" OnClick="btnShowInputEditKompetensiDasarKURTILAS_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputKomponenPenilaian" OnClick="btnShowInputKomponenPenilaian_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputEditKomponenPenilaian" OnClick="btnShowInputEditKomponenPenilaian_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputKomponenPenilaianKURTILAS" OnClick="btnShowInputKomponenPenilaianKURTILAS_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputEditKomponenPenilaianKURTILAS" OnClick="btnShowInputEditKomponenPenilaianKURTILAS_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputPredikatPenilaianKURTILAS" OnClick="btnShowInputPredikatPenilaianKURTILAS_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputKompetensiDasarKURTILASSikap" OnClientClick="ShowProgress(true);" OnClick="btnShowInputKompetensiDasarKURTILASSikap_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputEditKompetensiDasarKURTILASSikap" OnClick="btnShowInputEditKompetensiDasarKURTILASSikap_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputDeskripsiLTSRapor" OnClick="btnShowInputDeskripsiLTSRapor_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDataList" OnClick="btnShowDataList_Click" style="position: absolute; left: -1000px; top: -1000px;" />                        
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoRefreshBukaSemester" OnClick="btnDoRefreshBukaSemester_Click" style="position: absolute; left: -1000px; top: -1000px;" />      
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowPreviewNilai" OnClick="btnShowPreviewNilai_Click" style="position: absolute; left: -1000px; top: -1000px;" />      
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowPreviewNilaiKelasPerwalian" OnClick="btnShowPreviewNilaiKelasPerwalian_Click" style="position: absolute; left: -1000px; top: -1000px;" />      

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
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle; width: 50px;">
                                                                            #
									                                    </th>
                                                                        <th style="text-align: center; background-color: #3367d6; width: 80px; vertical-align: middle;">
                                                                            <i class="fa fa-cog"></i>
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Tahun Ajaran, Mapel
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: center; padding-left: 10px; vertical-align: middle;">
                                                                            Kelas
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: right; padding-left: 10px; vertical-align: middle;">
                                                                            Status
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            KKM
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
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
                                                            <td style="padding: 0px; text-align: center; width: 50px; vertical-align: middle;">
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
                                                                                        onclick="DoScrollPos(); ShowProgress(true); <%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetail.ClientID %>.click(); " 
                                                                                        id="btnDetail" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-info-circle" title=" Lihat Detail "></i>&nbsp;&nbsp;&nbsp;Lihat Detail</label>
												                                </li>
                                                                                <li style="padding: 0px;">
                                                                                    <hr style="margin: 0px; padding: 0px;" />
                                                                                </li>
												                                <li style="background-color: white; padding: 10px;">
													                                <label
                                                                                        onclick="DoScrollPos(); ShowProgress(true); <%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmDelete.ClientID %>.click(); " 
                                                                                        id="btnHapus" style="color: #9E0000; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-times" title=" Hapus Data "></i>&nbsp;&nbsp;&nbsp;Hapus Data</label>
												                                </li>
											                                </ul>
										                                </li>
									                                </ul>
                                                                </div>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; padding-top: 0px; padding-bottom: 0px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none; font-size: x-small;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("TahunAjaran").ToString())
                                                                    %>
                                                                </span>
                                                                <sup title=" Semester " style="font-weight: normal; text-transform: none; text-decoration: none; font-weight: bold; font-size: x-small;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Semester").ToString())
                                                                    %>
                                                                </sup>
                                                                <br />
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Mapel").ToString())
                                                                    %>
                                                                </span>                                                                
                                                                <span style="color: #bfbfbf; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        (
                                                                            Eval("JenisMapel").ToString() == AI_ERP.Application_Libs.Libs.JENIS_MAPEL.EKSKUL ||
                                                                            Eval("JenisMapel").ToString() == AI_ERP.Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER
                                                                            ? ""
                                                                            : "<br />" +
                                                                              AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kurikulum").ToString())
                                                                        )
                                                                    %>
                                                                </span>
                                                                <span style="color: grey; font-weight: normal; font-style: italic; text-transform: none; text-decoration: none; color: #278BF4; font-size: small;">
                                                                    <%# 
                                                                        Eval("JenisMapel").ToString().Trim() != ""
                                                                        ? (
                                                                            Eval("Kurikulum").ToString().Trim() == "" ||
                                                                            (
                                                                                Eval("JenisMapel").ToString() == AI_ERP.Application_Libs.Libs.JENIS_MAPEL.EKSKUL ||
                                                                                Eval("JenisMapel").ToString() == AI_ERP.Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER
                                                                            )
                                                                            ? "<br />"
                                                                            : "&nbsp;"
                                                                          ) +
                                                                          AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("JenisMapel").ToString())
                                                                        : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center;">
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        (
                                                                            AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kelas").ToString()).Trim() != ""
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kelas").ToString())
                                                                            : "<span style=\"font-weight: normal; color: red;\">(Semua)</span>"
                                                                        )
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: right;">
                                                                <%--
                                                                <%# 
                                                                    AI_ERP.Application_Modules.EDUCATION.Penilaian.SMA.wf_StrukturPenilaian.GetHTMLKelasMapelDetIsiNilai(
                                                                        Eval("TahunAjaran").ToString(), Eval("Semester").ToString(), Eval("Rel_Kelas").ToString(), Eval("Rel_Mapel").ToString(), Eval("Kurikulum").ToString()
                                                                    )
                                                                %>
                                                                --%>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("KKM").ToString())
                                                                    %>
                                                                </span>
									                        </td>
                                                            <td style="text-align: right; vertical-align: middle;">
                                                                <label title=" Pengaturan Struktur Penilaian " 
                                                                       id="lbl_<%# Eval("Kode").ToString().Replace("-", "_") %>" 
                                                                       onclick="DoScrollPos(); ShowProgress(true); <%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>';<%= btnShowStrukturNilai.ClientID %>.click();" 
                                                                       style="
                                                                                cursor: pointer; font-weight: normal; font-size: small; width: 100%; color: grey; font-weight: bold; width: 20px; text-align: center;
                                                                                <%# 
                                                                                    Convert.ToBoolean(Eval("IsNilaiAkhir") == DBNull.Value ? false : Eval("IsNilaiAkhir"))
                                                                                    ? "display : none;"
                                                                                    : ""
                                                                                %>
                                                                            ">
                                                                    <i class="fa fa-file-text-o"></i>
                                                                </label>
                                                                <span style="color: #d9d9d9;
                                                                             <%# 
                                                                                (
                                                                                    Convert.ToBoolean(Eval("IsNilaiAkhir") == DBNull.Value ? false : Eval("IsNilaiAkhir"))
                                                                                    ? "display: none;"
                                                                                    : (
                                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kurikulum").ToString()).ToUpper() != AI_ERP.Application_Libs.Libs.JenisKurikulum.SMA.KURTILAS.ToUpper() &&
                                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kurikulum").ToString()).ToUpper() != AI_ERP.Application_Libs.Libs.JenisKurikulum.SMA.KURTILAS_SIKAP.ToUpper()
                                                                                        ? (
                                                                                            AI_ERP.Application_Modules.EDUCATION.Penilaian.SMA.wf_StrukturPenilaian.IsMapelEkskul(
                                                                                                Eval("Rel_Mapel").ToString()
                                                                                            ) ? ""
                                                                                                : "display: none;"
                                                                                            )
                                                                                        : ""
                                                                                      )
                                                                                )
                                                                             %>">&nbsp;|&nbsp;</span>
                                                                <label title=" Pengaturan Predikat Penilaian " id="lbl_<%# Eval("Kode").ToString().Replace("-", "_") %>_1" onclick="DoScrollPos(); ShowProgress(true); <%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>';<%= btnShowInputPredikatPenilaianKURTILAS.ClientID %>.click();" 
                                                                    style="<%# 
                                                                                AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kurikulum").ToString()).ToUpper() != AI_ERP.Application_Libs.Libs.JenisKurikulum.SMA.KURTILAS.ToUpper() &&
                                                                                AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kurikulum").ToString()).ToUpper() != AI_ERP.Application_Libs.Libs.JenisKurikulum.SMA.KURTILAS_SIKAP.ToUpper()
                                                                                ? (
                                                                                    AI_ERP.Application_Modules.EDUCATION.Penilaian.SMA.wf_StrukturPenilaian.IsMapelEkskul(
                                                                                        Eval("Rel_Mapel").ToString()
                                                                                    ) ? ""
                                                                                      : "display: none;"
                                                                                  )
                                                                                : ""
                                                                           %>cursor: pointer; font-weight: normal; font-size: small; width: 100%; color: grey; font-weight: bold; width: 20px; text-align: center;">
                                                                    <i class="fa fa-sort-alpha-asc"></i>
                                                                </label>
                                                                <span style="color: #d9d9d9;">&nbsp;|&nbsp;</span>
                                                                <label title=" Lihat Nilai " id="lbl_<%# Eval("Kode").ToString().Replace("-", "_") %>_2" 
                                                                    onclick="
                                                                            <%# 
                                                                                Eval("JenisMapel").ToString() == AI_ERP.Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER &&
                                                                                Eval("Mapel").ToString().ToLower().IndexOf("pramuka") < 0
                                                                                ? "DoScrollPos(); ShowProgress(true); " + txtID.ClientID + ".value = '" + Eval("Kode").ToString() + "';" + btnShowDataList.ClientID + ".click(); " +
                                                                                  "setTimeout(function(){ " +
                                                                                            AI_ERP.Application_Modules.EDUCATION.Penilaian.SMA.wf_StrukturPenilaian.GetJSPreviewNilaiEkskul(
                                                                                                this.Page, Eval("Kode").ToString(), Eval("TahunAjaran").ToString(), Eval("Semester").ToString(), Eval("Rel_Kelas").ToString(), "", Eval("Rel_Mapel").ToString()
                                                                                            ) +
                                                                                        "}, " +
                                                                                        "10" +
                                                                                  ");"
                                                                                : txtID.ClientID + ".value = '" + Eval("Kode").ToString() + "'; " +
                                                                                  (
                                                                                    Eval("Mapel").ToString().ToLower().IndexOf("pramuka") >= 0
                                                                                    ? btnShowPreviewNilaiKelasPerwalian.ClientID + ".click();"
                                                                                    : btnShowPreviewNilai.ClientID + ".click();"
                                                                                  )
                                                                            %>
                                                                            "
                                                                    style="cursor: pointer; font-weight: normal; font-size: small; width: 100%; color: grey; font-weight: bold; width: 20px; text-align: center;">
                                                                    <i class="fa fa-table"></i>
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
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Tahun Ajaran, Mapel
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: center; padding-left: 10px; vertical-align: middle;">
                                                                            Kelas
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Status
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            KKM
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                        </th>
								                                    </tr>
							                                    </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="7" style="text-align: center; padding: 10px;">
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
                                                        <asp:LinkButton ToolTip=" Tambah Data " runat="server" ID="btnDoAdd" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" style="background-color: black;" OnClick="btnDoAdd_Click">
                                                            <span class="fbtn-text fbtn-text-left">Tambah Data</span>
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
                                                <table class="table" style="width: 100%; margin: 0px;">
                                                    <tr style="background-color: #3367d6;">
                                                        <td style="text-align: left; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle;">
                                                            <asp:Literal runat="server" ID="ltrCaptionKTSPDet"></asp:Literal>
									                    </td>
                                                    </tr>
                                                </table>
                                                <asp:Literal runat="server" ID="ltrKTSPDet"></asp:Literal>
                                            </div>
                                            
                                            <div class="content-header ui-content-header" 
						                        style="background-color: #00198d;
								                        box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
								                        background-image: none; 
								                        color: white;
								                        display: block;
								                        z-index: 5;
								                        position: fixed; bottom: 33px; right: 50px; width: 295px; border-radius: 25px;
								                        padding: 8px; margin: 0px; height: 35px;">
						                        <div style="padding-left: 0px;">
							                        <asp:LinkButton ToolTip=" Kembali " runat="server" ID="btnShowDataListFromKTSPDet" CssClass="btn-trans waves-attach waves-circle waves-effect" OnClick="btnShowDataListFromKTSPDet_Click" style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;
                                                    </asp:LinkButton>
						                        </div>
					                        </div>

                                            <div class="content-header ui-content-header" 
						                        style="background-color: #076c07;
								                        box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
								                        background-image: none; 
								                        color: white;
								                        display: block;
								                        z-index: 6;
								                        position: fixed; bottom: 33px; right: 50px; width: 250px; border-radius: 25px;
								                        padding: 8px; margin: 0px; height: 35px;">
						                        <div style="padding-left: 0px;">
							                        <asp:LinkButton ToolTip=" Lihat Nilai " runat="server" ID="lnkOKPreviewNilaiKTSP" CssClass="btn-trans waves-attach waves-circle waves-effect" style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-table"></i>
                                                        &nbsp;
                                                        Lihat Nilai
                                                        &nbsp;&nbsp;
                                                    </asp:LinkButton>
						                        </div>
					                        </div>

                                            <div class="content-header ui-content-header" 
						                        style="background-color: #8A0083;
								                        background-image: none; 
								                        color: white;
								                        display: block;
								                        z-index: 7;
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

                                            <div class="fbtn-container" id="div_button_settings_ktsp" runat="server">
		                                        <div class="fbtn-inner">
			                                        <a onclick="setTimeout(
                                                            function(){
                                                                <%= btnShowInputOnStrukturPenilaian.ClientID %>.click();
                                                            }, 500
                                                        ); return false;" class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #a91212;" title=" Tambah Data ">
                                                        <span class="fbtn-ori icon" style="padding-left: 2px;"><span class="fa fa-plus"></span></span>
                                                        <span class="fbtn-sub icon" style="padding-left: 2px;"><span class="fa fa-plus"></span></span>
                                                    </a>
		                                        </div>
	                                        </div>

                                        </asp:View>

                                        <asp:View runat="server" ID="vListKURTILASDet">

                                            <div style="padding: 0px; margin: 0px;">
                                                <table class="table" style="width: 100%; margin: 0px;">
                                                    <tr style="background-color: #3367d6;">
                                                        <td style="text-align: left; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle;">
                                                            <asp:Literal runat="server" ID="ltrCaptionKURTILASDet"></asp:Literal>
									                    </td>
                                                    </tr>
                                                </table>
                                                
                                                <div runat="server" id="div_penilaian_sikap" visible="false" class="card" style="margin: 7px; margin-right: 8px; margin-bottom: 10px;">
				                                    <div class="card-main">
					                                    <div class="card-inner" style="margin: 0px; padding: 0px;">

                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td style="background-color: #830000; font-weight: normal; color: white; width: 40px;">
                                                                        <i class="fa fa-hashtag"></i>
                                                                    </td>
                                                                    <td colspan="7" style="background-color: #830000; font-weight: bold; color: white; padding-left: 0px;">
                                                                        Penilaian Sikap
                                                                    </td>
                                                                    <td style="background-color: #830000; text-align: right;">
                                                                        <label onclick="<%= btnShowInputKompetensiDasarKURTILASSikap.ClientID %>.click();" title=" Tambah Kompetensi Dasar " class="badge" style="cursor: pointer; font-weight: normal; font-size: x-small; background-color: grey;">
                                                                            <i class="fa fa-plus"></i>
                                                                            &nbsp;
                                                                            KD
                                                                        </label>
                                                                    </td>
                                                                </tr>
                                                                <asp:Literal runat="server" ID="ltrKurtilasSikap"></asp:Literal>
                                                            </table>

                                                        </div>
                                                    </div>
                                                </div>
                                                
                                                <div class="card" style="margin: 7px; margin-right: 8px; margin-bottom: 10px;">
				                                    <div class="card-main">
					                                    <div class="card-inner" style="margin: 0px; padding: 0px;">

                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td style="background-color: #890000; font-weight: normal; color: white; width: 40px;">
                                                                        <i class="fa fa-hashtag"></i>
                                                                    </td>
                                                                    <td colspan="7" style="background-color: #890000; font-weight: bold; color: white; padding-left: 0px;">
                                                                        Penilaian Pengetahuan, Keterampilan & UAS
                                                                    </td>
                                                                    <td style="background-color: #890000;">
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                            <div>
                                                                <asp:Literal runat="server" ID="ltrKURTILASDet"></asp:Literal>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>                                                
                                            </div>
                                            
                                            <div class="content-header ui-content-header" 
						                        style="background-color: #00198d;
								                        box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
								                        background-image: none; 
								                        color: white;
								                        display: block;
								                        z-index: 5;
								                        position: fixed; bottom: 33px; right: 50px; width: 295px; border-radius: 25px;
								                        padding: 8px; margin: 0px; height: 35px;">
						                        <div style="padding-left: 0px;">
							                        <asp:LinkButton ToolTip=" Kembali " runat="server" ID="btnShowDataListFromKURTILASDet" CssClass="btn-trans waves-attach waves-circle waves-effect" OnClick="btnShowDataListFromKURTILASDet_Click" style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;
                                                    </asp:LinkButton>
						                        </div>
					                        </div>

                                            <div class="content-header ui-content-header" 
						                        style="background-color: #076c07;
								                        box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
								                        background-image: none; 
								                        color: white;
								                        display: block;
								                        z-index: 6;
								                        position: fixed; bottom: 33px; right: 50px; width: 250px; border-radius: 25px;
								                        padding: 8px; margin: 0px; height: 35px;">
						                        <div style="padding-left: 0px;">
							                        <asp:LinkButton OnClick="lnkOKPreviewNilaiKURTILAS_Click" ToolTip=" Lihat Nilai " runat="server" ID="lnkOKPreviewNilaiKURTILAS" CssClass="btn-trans waves-attach waves-circle waves-effect" style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-table"></i>
                                                        &nbsp;
                                                        Lihat Nilai
                                                        &nbsp;&nbsp;
                                                    </asp:LinkButton>
						                        </div>
					                        </div>

                                            <div class="content-header ui-content-header" 
						                        style="background-color: #8A0083;
								                        background-image: none; 
								                        color: white;
								                        display: block;
								                        z-index: 7;
								                        position: fixed; bottom: 33px; right: 50px; width: 120px; border-radius: 25px;
								                        padding: 8px; margin: 0px; height: 35px;">
						                        <div style="padding-left: 0px;">
							                        <asp:LinkButton OnClientClick="ShowConfirmHapusItemStrukturNilai(); return false;" ToolTip=" Hapus " runat="server" ID="lnkHapusItemKURTILASDet" CssClass="btn-trans waves-attach waves-circle waves-effect" style="color: ghostwhite;">
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

                                            <div class="fbtn-container" id="div_button_settings_kurtilas" runat="server">
		                                        <div class="fbtn-inner">
			                                        <a onclick="setTimeout(
                                                            function(){
                                                                <%= btnShowInputOnStrukturPenilaian.ClientID %>.click();
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
                                        Isi Data
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
                                                        <label class="label-input" for="<%= txtTahunAjaran.ClientID %>" style="text-transform: none;">Tahun Ajaran</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldTahunAjaran"
                                                            ControlToValidate="txtTahunAjaran" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtTahunAjaran"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
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
                                                        <label class="label-input" for="<%= cboKurikulum.ClientID %>" style="text-transform: none;">Kurikulum</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldKurikulum"
                                                            ControlToValidate="cboKurikulum" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList onchange="ValidateInputByKurikulum();" CssClass="form-control" runat="server" ID="cboKurikulum">
                                                            <asp:ListItem Value="KURTILAS" Text="KURTILAS"></asp:ListItem>
                                                            <asp:ListItem Value="KTSP" Text="KURTILAS-EKSTRAKURIKULER"></asp:ListItem>
                                                            <asp:ListItem Value="KURTILAS-SIKAP" Text="KURTILAS-SIKAP"></asp:ListItem>
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
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboKelas.ClientID %>" style="text-transform: none;">Kelas</label>
                                                        <asp:DropDownList CssClass="form-control" runat="server" ID="cboKelas">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div> 
                                            <div id="div_kkm" class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtKKM.ClientID %>" style="text-transform: none;">KKM</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldKKM"
                                                            ControlToValidate="txtKKM" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtKKM"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="div_bobot_rapor_pengetahuan_caption" class="row" style="margin-left: 15px; margin-right: 15px; margin-top: 15px;">
                                                <div class="col-xs-12">
                                                    <span style="font-weight: bold; color: grey;">
                                                        Bobot Rapor 
                                                        <span style="color: mediumvioletred; border-radius: 0px;">PENGETAHUAN</span>
                                                    </span>
                                                </div>
                                            </div>
                                            <div id="div_bobot_rapor_pengetahuan_input" class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtBobotRaporPengetahuan.ClientID %>" style="text-transform: none;">Dari Pengetahuan %</label>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtBobotRaporPengetahuan"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtBobotRaporUAS.ClientID %>" style="text-transform: none;">Dari UAS %</label>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtBobotRaporUAS"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="div_pilihan_ekskul" class="row" style="margin-left: 15px; margin-right: 15px; padding-bottom: 15px; padding-top: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="checkbox checkbox-adv">
													    <label for="<%= chkIsNilaiAkhir.ClientID %>">
														    <input runat="server" class="access-hide" id="chkIsNilaiAkhir" type="checkbox">
														    <span class="checkbox-circle"></span><span class="checkbox-circle-check"></span><span class="checkbox-circle-icon icon">done</span>
                                                            <span style="font-weight: bold; font-size: 14px; color: black;">
																Hanya pengisian nilai akhir
															</span>
													    </label>
												    </div>
                                                </div>
                                            </div>
                                            <div class="row" id="div_deskripsi">
                                                <div class="col-lg-12">
                                        
                                                    <div style="width: 100%; background-color: white; padding-top: 0px;">
                                                        <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                            <div class="col-xs-12">
                                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                    <label class="label-input" for="<%= txtDeskripsiPerMapel.ClientID %>" style="text-transform: none; margin-bottom: 6px;">
                                                                        Deskripsi
                                                                    </label>
                                                                    <asp:TextBox style="height: 100px;" runat="server" ID="txtDeskripsiPerMapel" CssClass="mcetiny_deskripsi_per_mapel"></asp:TextBox>
                                                                </div>
                                                            </div>
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
                                <asp:LinkButton OnClientClick="if(Page_ClientValidate('vldInput')){ ShowProgress(true); }" ValidationGroup="vldInput" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInput" OnClick="lnkOKInput_Click" Text="   OK   "></asp:LinkButton>
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
                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtPoinAspekPenilaian.ClientID %>" style="text-transform: none;">Poin</label>
                                                        <asp:TextBox runat="server" ID="txtPoinAspekPenilaian" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtAspekPenilaian.ClientID %>" style="text-transform: none; margin-bottom: 15px;">Aspek Penilaian</label>
                                                        <asp:TextBox CssClass="mcetiny_aspek_penilaian" runat="server" ID="txtAspekPenilaian" style="height: 100px;"></asp:TextBox>
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
                                <asp:LinkButton OnClientClick="if(Page_ClientValidate('vldInputAspekPenilaian')){ ShowProgress(true); }" ValidationGroup="vldInputAspekPenilaian" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKAspekPenilaian" OnClick="lnkOKAspekPenilaian_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_kompetensi_dasar_sikap" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Kompetensi Dasar Sikap
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtPoinKompetensiDasarSikap.ClientID %>" style="text-transform: none;">Poin</label>
                                                        <asp:TextBox runat="server" ID="txtPoinKompetensiDasarSikap" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtKompetensiDasarSikapKURTILAS.ClientID %>" style="text-transform: none; margin-bottom: 15px;">Kompetensi Dasar</label>
                                                        <asp:TextBox style="height: 120px;" runat="server" ID="txtKompetensiDasarSikapKURTILAS" CssClass="mcetiny_kompetensi_dasar_sikap_kurtilas"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtDeskripsiKompetensiDasarSikap.ClientID %>" style="text-transform: none; margin-bottom: 6px;">Deskripsi</label>
                                                        <asp:TextBox style="height: 120px;" runat="server" ID="txtDeskripsiKompetensiDasarSikap" ValidationGroup="vldInputKompetensiDasarSikap" CssClass="mcetiny_deskripsi_sikap"></asp:TextBox>
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
                                <asp:LinkButton OnClientClick="if(Page_ClientValidate('vldInputKompetensiDasarSikap')){ ShowProgress(true); }" ValidationGroup="vldInputKompetensiDasarSikap" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKKompetensiDasarSikap" OnClick="lnkOKKompetensiDasarSikap_Click" Text="   OK   "></asp:LinkButton>
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
                                                        <label class="label-input" for="<%= txtKompetensiDasar.ClientID %>" style="text-transform: none; margin-bottom: 15px;">Kompetensi Dasar</label>
                                                        <asp:TextBox CssClass="mcetiny_kompetensi_dasar" runat="server" ID="txtKompetensiDasar" style="height: 100px;"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboJenisPerhitunganKompetensiDasar.ClientID %>" style="text-transform: none;">
                                                            Jenis Perhitungan
                                                        </label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputKompetensiDasar" runat="server" ID="vldJenisPerhitunganKompetensiDasar"
                                                            ControlToValidate="cboJenisPerhitunganKompetensiDasar" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList ValidationGroup="vldInputKompetensiDasar" CssClass="form-control" runat="server" ID="cboJenisPerhitunganKompetensiDasar"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="div_bobot_rapor_ktsp" class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtBobotRaporPPK.ClientID %>" style="text-transform: none;">
                                                            Bobot Rapor
                                                            <sup class="badge" style="background-color: #40B3D2; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: x-small; padding-bottom: 4px; padding-top: 2px; display: initial;">PPK</sup>
                                                        </label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputKompetensiDasar" runat="server" ID="vldBobotRaporPPK"
                                                            ControlToValidate="txtBobotRaporPPK" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtBobotRaporPPK"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtBobotRaporP.ClientID %>" style="text-transform: none;">
                                                            Bobot Rapor
                                                            <sup class="badge" style="background-color: #68217A; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: x-small; padding-bottom: 4px; padding-top: 2px; display: initial;">Praktik</sup>
                                                        </label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputKompetensiDasar" runat="server" ID="vldBobotRaporP"
                                                            ControlToValidate="txtBobotRaporP" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtBobotRaporP"></asp:TextBox>
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
                                <asp:LinkButton OnClientClick="if(Page_ClientValidate('vldInputKompetensiDasar')){ ShowProgress(true); }" ValidationGroup="vldInputKompetensiDasar" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKStrukturNilai" OnClick="lnkOKStrukturNilai_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_kompetensi_dasar_kurtilas" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                            <div runat="server" id="div_poin_kd_kurtilas" class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtPoinKompetensiDasarKURTILAS.ClientID %>" style="text-transform: none;">Poin</label>
                                                        <asp:TextBox runat="server" ID="txtPoinKompetensiDasarKURTILAS" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="div_kd_kurtilas" class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <br />
                                                    <asp:Literal runat="server" ID="ltrKDKurtilas"></asp:Literal>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboJenisPerhitunganKompetensiDasarKURTILAS.ClientID %>" style="text-transform: none;">
                                                            Jenis Perhitungan
                                                        </label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputKompetensiDasarKURTILAS" runat="server" ID="vldJenisPerhituganKD"
                                                            ControlToValidate="cboJenisPerhitunganKompetensiDasarKURTILAS" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList CssClass="form-control" runat="server" ID="cboJenisPerhitunganKompetensiDasarKURTILAS"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px; padding-bottom: 20px;">
                                                <div class="col-xs-12">
                                                    <div class="checkbox checkbox-adv">
													    <label for="<%= chkNKDIsKomponenRapor.ClientID %>">
														    <input runat="server" class="access-hide" id="chkNKDIsKomponenRapor" type="checkbox">
														    <span class="checkbox-circle"></span><span class="checkbox-circle-check"></span><span class="checkbox-circle-icon icon">done</span>
                                                            <span style="font-weight: normal; font-size: small; color: grey;">
																n-KD Set sebagai Komponen Rapor
															</span>
													    </label>
												    </div>
                                                </div>
                                            </div>
                                            <div id="div_info_kd_kurtilas" class="row" style="margin-left: 15px; margin-right: 15px; display: none;">
                                                <div class="col-xs-12" style="font-weight: bold; color: red;">
                                                    <i class="fa fa-exclamation-triangle"></i>
                                                    &nbsp;Kompetensi dasar harus dipilih salah satu
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>								                            
							</div>
				        </div>
				        <div class="modal-footer">
					        <p class="text-right">
                                <asp:LinkButton OnClientClick="SetPilihStrukturKD(); return ValidateInputKDKurtilas();" ValidationGroup="vldInputKompetensiDasarKURTILAS" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKKompetensiDasar" OnClick="lnkOKKompetensiDasar_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_komponen_penilaian_kurtilas" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtKomponenPenilaianKURTILAS.ClientID %>" style="text-transform: none; margin-bottom: 15px;">Deskripsi Komponen Penilaian</label>
                                                        <asp:TextBox CssClass="mcetiny_komponen_penilaian_kurtilas" runat="server" ID="txtKomponenPenilaianKURTILAS" style="height: 100px;"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 0px; margin-right: 0px; padding-bottom: 20px;">
                                                <div class="col-xs-12">
                                                    <div class="checkbox checkbox-adv">
													    <label for="<%= chkIsKomponenRapor.ClientID %>">
														    <input runat="server" class="access-hide" id="chkIsKomponenRapor" type="checkbox">
														    <span class="checkbox-circle"></span><span class="checkbox-circle-check"></span><span class="checkbox-circle-icon icon">done</span>
                                                            <span style="font-weight: normal; font-size: small; color: grey;">
																Set sebagai Komponen Rapor
															</span>
													    </label>
												    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="div_bobot_kd_dari_kp" class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtBobotNKDKurtilas.ClientID %>" style="text-transform: none;">Bobot n-KD %</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputKomponenPenilaianKURTILAS" runat="server" ID="vldBobotKURTILAS"
                                                            ControlToValidate="txtBobotNKDKurtilas" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtBobotNKDKurtilas"></asp:TextBox>
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
                                <asp:LinkButton OnClientClick="if(Page_ClientValidate('vldInputKomponenPenilaianKURTILAS')){ ShowProgress(true); }" ValidationGroup="vldInputKomponenPenilaianKURTILAS" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKKomponenPenilaianKURTILAS" OnClick="lnkOKKomponenPenilaianKURTILAS_Click" Text="   OK   "></asp:LinkButton>
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
                                                        <label class="label-input" for="<%= txtKomponenPenilaian.ClientID %>" style="text-transform: none; margin-bottom: 15px;">Komponen Penilaian</label>
                                                        <asp:TextBox CssClass="mcetiny_komponen_penilaian" runat="server" ID="txtKomponenPenilaian" style="height: 100px;"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="div_jenis_komponen_penilaian_ktsp" class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboJenisKomponenPenilaian.ClientID %>" style="text-transform: none;">Jenis Komponen</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputKomponenPenilaian" runat="server" ID="vldJenisKomponenPenilaian"
                                                            ControlToValidate="cboJenisKomponenPenilaian" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList CssClass="form-control" runat="server" ID="cboJenisKomponenPenilaian"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="div_bobot_komponen_penilaian_ktsp" class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtBobotNKD.ClientID %>" style="text-transform: none;">Bobot n-KD %</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputKomponenPenilaian" runat="server" ID="vldBobotNKD"
                                                            ControlToValidate="txtBobotNKD" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtBobotNKD"></asp:TextBox>
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
                                <asp:LinkButton OnClientClick="if(Page_ClientValidate('vldInputKomponenPenilaian')){ ShowProgress(true); }" ValidationGroup="vldInputKomponenPenilaian" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKKomponenPenilaian" OnClick="lnkOKKomponenPenilaian_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>
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
                                <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapus" OnClick="lnkOKHapus_Click" Text="  OK  "></asp:LinkButton>
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
                                <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapusItemStrukturPenilaian" OnClick="lnkOKHapusItemStrukturPenilaian_Click" Text="  OK  "></asp:LinkButton>
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
                                                        <div class="col-xs-6" style="color: grey; vertical-align: bottom">
                                                            Kelas :
                                                            <br />
                                                            <asp:Label runat="server" ID="lblKelasPredikat" style="font-weight: bold;"></asp:Label>
                                                            <hr style="margin-top: 5px; margin-bottom: 15px;" />
                                                        </div>
                                                        <div class="col-xs-6" style="color: grey; vertical-align: bottom">
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
            
            <div aria-hidden="true" class="modal fade" id="ui_modal_deskripsi_lts_rapor" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        <asp:Literal runat="server" ID="ltrCaptionDeskripsiLTSRapor"></asp:Literal>
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                <div class="col-xs-12">
                                                    <asp:Literal runat="server" ID="ltrInfoDeskripsiLTSRapor"></asp:Literal>
                                                </div>
                                            </div>                                            
                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtDeskripsiLTSRapor.ClientID %>" style="text-transform: none; margin-bottom: 6px; color: #bfbfbf;">
                                                            <i class="fa fa-hashtag"></i>&nbsp;
                                                            Deskripsi
                                                        </label>
                                                        <asp:TextBox style="height: 120px;" runat="server" ID="txtDeskripsiLTSRapor" CssClass="mcetiny_deskripsi_lts_rapor"></asp:TextBox>
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
                                <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKDeskripsiLTSRapor" OnClick="lnkOKDeskripsiLTSRapor_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_preview" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                        Lihat Nilai
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 0px;">
                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                    <asp:Literal runat="server" ID="ltrListMengajarPreviewPenilaian"></asp:Literal>
                                                </div>
                                            </div>
										</div>  

                                    </div>
                                </div>								                            
							</div>
				        </div>
				        <div class="modal-footer">
					        <p class="text-right">
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
        function LoadTinyMCEAspekPenilaian() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.remove();
            tinymce.init({
                selector: ".mcetiny_aspek_penilaian",
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
                height: 200,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtAspekPenilaianVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
        }

        function LoadTinyMCEKompetensiDasar() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.remove();
            tinymce.init({                
                selector: ".mcetiny_kompetensi_dasar",
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
                height: 200,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtKompetensiDasarVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('keydown',function(e) {
                        if (e.keyCode == 13) {
                            document.getElementById('<%= txtBobotRaporPPK.ClientID %>').focus();
                            e.preventDefault();
                        }
                    });

                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
        }

        function LoadTinyMCEKompetensiDasarSikapKURTILAS() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({                
                mode : "exact",
                selector: ".mcetiny_kompetensi_dasar_sikap_kurtilas",
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
                height: 200,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtKompetensiDasarSikapKURTILASVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
        }

        function LoadTinyMCEKomponenPenilaian() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.remove();
            tinymce.init({
                selector: ".mcetiny_komponen_penilaian",
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
                height: 200,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtKomponenPenilaianVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
        }

        function LoadTinyMCEKomponenPenilaianKURTILAS() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.remove();
            tinymce.init({
                selector: ".mcetiny_komponen_penilaian_kurtilas",
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
                height: 200,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtKomponenPenilaianKURTILASVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
        }

        function LoadTinyMCEDeskripsi() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                mode : "exact",
                selector: ".mcetiny_deskripsi_sikap",
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
                height: 200,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtDeskripsiKompetensiDasarSikapVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
        }

        function LoadTinyMCEDeskripsiLTSRapor() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                mode : "exact",
                selector: ".mcetiny_deskripsi_lts_rapor",
                theme: "modern",
                plugins: [
                    "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                    "searchreplace wordcount visualblocks visualchars code fullscreen",
                    "insertdatetime media nonbreaking save table contextmenu directionality",
                    "emoticons template paste textcolor tinyfilemanager.net"
                ],
                //toolbar1: "bold italic underline | alignleft aligncenter alignright alignjustify",
                toolbar1: "bold italic underline",
                image_advtab: true,
                templates: [
                    { title: 'Test template 1', content: 'Test 1' },
                    { title: 'Test template 2', content: 'Test 2' }
                ],
                resize: "vertical",
                statusbar: false,
                menubar: false,
                height: 200,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtDeskripsiLTSRaporVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
        }

        function LoadTinyMCEDeskripsiPerMapel() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                mode : "exact",
                selector: ".mcetiny_deskripsi_per_mapel",
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
                height: 120,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtDeskripsiPerMapelVal.ClientID %>').value = ed.getContent();
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
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtDeskripsiKompetensiDasarSikap.ClientID %>');
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtKompetensiDasarSikapKURTILAS.ClientID %>');
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtDeskripsiLTSRapor.ClientID %>');
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtDeskripsiPerMapel.ClientID %>');
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtDeskripsiSikapSpiritual.ClientID %>');
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtDeskripsiSikapSosial.ClientID %>');            
        }
    </script>

    <script type="text/javascript">
        RenderDropDownOnTables();
        InitModalFocus();
        DoAutoSave();
    </script>
</asp:Content>
