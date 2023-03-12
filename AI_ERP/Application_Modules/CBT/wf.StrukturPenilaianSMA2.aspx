<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="wf.StrukturPenilaianSMA2.aspx.cs" Inherits="AI_ERP.Application_Modules.CBT.wf_StrukturPenilaianSMA2" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .input-box {
            border-style: solid;
            border-width: 1px;
            border-color: #DBDBDB;
            padding: 6px;
            outline: none;
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
            animation: none;
            animation-delay: 0;
            animation-direction: normal;
            animation-duration: 0;
            animation-fill-mode: none;
            animation-iteration-count: 1;
            animation-name: none;
            animation-play-state: running;
            animation-timing-function: ease;
            backface-visibility: visible;
            background: 0;
            background-attachment: scroll;
            background-clip: border-box;
            background-color: transparent;
            background-image: none;
            background-origin: padding-box;
            background-position: 0 0;
            background-position-x: 0;
            background-position-y: 0;
            background-repeat: repeat;
            background-size: auto auto;
            border: 0;
            border-style: none;
            border-width: medium;
            border-color: inherit;
            border-bottom: 0;
            border-bottom-color: inherit;
            border-bottom-left-radius: 0;
            border-bottom-right-radius: 0;
            border-bottom-style: none;
            border-bottom-width: medium;
            border-collapse: separate;
            border-image: none;
            border-left: 0;
            border-left-color: inherit;
            border-left-style: none;
            border-left-width: medium;
            border-radius: 0;
            border-right: 0;
            border-right-color: inherit;
            border-right-style: none;
            border-right-width: medium;
            border-spacing: 0;
            border-top: 0;
            border-top-color: inherit;
            border-top-left-radius: 0;
            border-top-right-radius: 0;
            border-top-style: none;
            border-top-width: medium;
            bottom: auto;
            box-shadow: none;
            box-sizing: content-box;
            caption-side: top;
            clear: none;
            clip: auto;
            color: inherit;
            columns: auto;
            column-count: auto;
            column-fill: balance;
            column-gap: normal;
            column-rule: medium none currentColor;
            column-rule-color: currentColor;
            column-rule-style: none;
            column-rule-width: none;
            column-span: 1;
            column-width: auto;
            content: normal;
            counter-increment: none;
            counter-reset: none;
            cursor: auto;
            direction: ltr;
            display: inline;
            empty-cells: show;
            float: none;
            font: normal;
            font-family: inherit;
            font-size: medium;
            font-style: normal;
            font-variant: normal;
            font-weight: normal;
            height: auto;
            hyphens: none;
            left: auto;
            letter-spacing: normal;
            line-height: normal;
            list-style: none;
            list-style-image: none;
            list-style-position: outside;
            list-style-type: disc;
            margin: 0;
            margin-bottom: 0;
            margin-left: 0;
            margin-right: 0;
            margin-top: 0;
            max-height: none;
            max-width: none;
            min-height: 0;
            min-width: 0;
            opacity: 1;
            orphans: 0;
            outline: 0;
            outline-color: invert;
            outline-style: none;
            outline-width: medium;
            overflow: visible;
            overflow-x: visible;
            overflow-y: visible;
            padding: 0;
            padding-bottom: 0;
            padding-left: 0;
            padding-right: 0;
            padding-top: 0;
            page-break-after: auto;
            page-break-before: auto;
            page-break-inside: auto;
            perspective: none;
            perspective-origin: 50% 50%;
            position: static;
            /* May need to alter quotes for different locales (e.g fr) */
            quotes: '\201C' '\201D' '\2018' '\2019';
            right: auto;
            tab-size: 8;
            table-layout: auto;
            text-align: inherit;
            text-align-last: auto;
            text-decoration: none;
            text-decoration-color: inherit;
            text-decoration-line: none;
            text-decoration-style: solid;
            text-indent: 0;
            text-shadow: none;
            text-transform: none;
            top: auto;
            transform: none;
            transform-style: flat;
            transition: none;
            transition-delay: 0s;
            transition-duration: 0s;
            transition-property: none;
            transition-timing-function: ease;
            unicode-bidi: normal;
            vertical-align: baseline;
            visibility: visible;
            white-space: normal;
            widows: 0;
            width: auto;
            word-spacing: normal;
            z-index: auto;
            /* basic modern patch */
            all: initial;
            all: unset;
        }

        /* basic modern patch */

        #reset-this-root {
            all: initial;
            *

        {
            all: unset;
        }

        }
    </style>
    <script type="text/javascript">
        function HideModal() {
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
                    window.scrollTo(0, 0);
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
                    if (jenis_act.trim() != "") {
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
            //InitModalFocus();
            document.getElementById("<%= txtKeyAction.ClientID %>").value = "";

            Sys.Browser.WebKit = {};
            if (navigator.userAgent.indexOf('WebKit/') > -1) {
                Sys.Browser.agent = Sys.Browser.WebKit;
                Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
                Sys.Browser.name = 'WebKit';
            }

            ShowProgress(false);
        }


        function ParseSelectedItemStrukturPenilaian() {
            var txt_aspek_penilaian = document.getElementById("<%= txtParseIDAspekPenilaian.ClientID %>");
            var txt_komp_dasar = document.getElementById("<%= txtParseIDKompetensiDasar.ClientID %>");
            var txt_komp_dasar_sikap = document.getElementById("<%= txtParseIDKompetensiDasarSikap.ClientID %>");
            var txt_komp_penilaian = document.getElementById("<%= txtParseIDKomponenPenilaian.ClientID %>");

            if (
                txt_aspek_penilaian != null && txt_aspek_penilaian != undefined &&
                txt_komp_dasar != null && txt_komp_dasar != undefined &&
                txt_komp_dasar_sikap != null && txt_komp_dasar_sikap != undefined &&
                txt_komp_penilaian != null && txt_komp_penilaian != undefined
            ) {
                txt_aspek_penilaian.value = "";
                txt_komp_dasar.value = "";
                txt_komp_dasar_sikap.value = "";
                txt_komp_penilaian.value = "";

                var arr_chk_aspek_penilaian = document.getElementsByName("chk_ap[]");
                var arr_chk_kom_dasar = document.getElementsByName("chk_kd[]");
                var arr_chk_kom_dasar_sikap = document.getElementsByName("chk_kd_sikap[]");
                var arr_chk_kom_penilaian = document.getElementsByName("chk_kp[]");

                if (arr_chk_aspek_penilaian.length > 0) {
                    for (var i = 0; i < arr_chk_aspek_penilaian.length; i++) {
                        if (arr_chk_aspek_penilaian[i].checked) {
                            txt_aspek_penilaian.value += arr_chk_aspek_penilaian[i].value + ";";
                        }
                    }
                }

                if (arr_chk_kom_dasar.length > 0) {
                    for (var i = 0; i < arr_chk_kom_dasar.length; i++) {
                        if (arr_chk_kom_dasar[i].checked) {
                            txt_komp_dasar.value += arr_chk_kom_dasar[i].value + ";";
                        }
                    }
                }

                if (arr_chk_kom_dasar_sikap.length > 0) {
                    for (var i = 0; i < arr_chk_kom_dasar_sikap.length; i++) {
                        if (arr_chk_kom_dasar_sikap[i].checked) {
                            txt_komp_dasar_sikap.value += arr_chk_kom_dasar_sikap[i].value + ";";
                        }
                    }
                }

                if (arr_chk_kom_penilaian.length > 0) {
                    for (var i = 0; i < arr_chk_kom_penilaian.length; i++) {
                        if (arr_chk_kom_penilaian[i].checked) {
                            txt_komp_penilaian.value += arr_chk_kom_penilaian[i].value + ";";
                        }
                    }
                }
            }
        }

        function ReInitTinyMCE() {
            RemoveTinyMCE();
            LoadTinyMCEDeskripsi();
            LoadTinyMCEKompetensiDasarSikapKURTILAS();
            LoadTinyMCEDeskripsiLTSRapor();
            LoadTinyMCEDeskripsiPerMapel();
            LoadTinyMCEDeskripsiSikapSpiritual();
            LoadTinyMCEDeskripsiSikapSosial();
        }

        function ShowConfirmHapusItemStrukturNilai() {
            ParseSelectedItemStrukturPenilaian();
            $('#ui_modal_hapus_item_struktur_penilaian').modal({ backdrop: 'static', keyboard: false, show: true });
        }

        function ShowInputBobotRaporPengetahuan(show) {
            var div_caption = document.getElementById("div_bobot_rapor_pengetahuan_caption");
            var div_input = document.getElementById("div_bobot_rapor_pengetahuan_input");
            if (
                div_caption != null && div_caption != undefined &&
                div_input != null && div_input != undefined
            ) {
                div_caption.style.display = (show === true ? "" : "none");
                div_input.style.display = (show === true ? "" : "none");
            }
        }

        function ShowInputPilihanEkskul(show) {
            var div_pilihan_ekskul = document.getElementById("div_pilihan_ekskul");
            if (
                div_pilihan_ekskul != null && div_pilihan_ekskul != undefined
            ) {
                div_pilihan_ekskul.style.display = (show === true ? "" : "none");
            }
        }



        function ValidateInputKDKurtilas() {
            var rdo = document.getElementsByName("rdo_kd_kurtilas[]");
            var div = document.getElementById("div_info_kd_kurtilas");

            if (div != null && div != undefined) {
                if (rdo.length > 0) {
                    for (var i = 0; i < rdo.length; i++) {
                        if (rdo[i].checked) {
                            div.style.display = "none";

                            if (Page_ClientValidate('vldInputKompetensiDasarKURTILAS')) { ShowProgress(true); }
                            return true;
                        }
                    }
                }
            }

            div.style.display = "";
            return false;
        }

        function ValidateInputKDKurtilasNoProgress() {
            var rdo = document.getElementsByName("rdo_kd_kurtilas[]");
            var div = document.getElementById("div_info_kd_kurtilas");

            if (div != null && div != undefined) {
                if (rdo.length > 0) {
                    for (var i = 0; i < rdo.length; i++) {
                        if (rdo[i].checked) {
                            div.style.display = "none";
                            return true;
                        }
                    }
                }
            }

            div.style.display = "";
            return false;
        }

        function SetPilihStrukturKD() {
            var rdo = document.getElementsByName("rdo_kd_kurtilas[]");
            var div = document.getElementById("div_info_kd_kurtilas");
            var s_kode = "";

            if (div != null && div != undefined) {
                if (rdo.length > 0) {
                    for (var i = 0; i < rdo.length; i++) {
                        if (rdo[i].checked) {
                            s_kode = rdo[i].value;
                            break;
                        }
                    }
                }
            }

            var txt = document.getElementById("<%= txtIDRelKompetensiDasar.ClientID %>");
            if (txt != null && txt != undefined) {
                txt.value = s_kode;
            }
        }

        function SetAttrDeskripsi(kode, jenis, id_deskripsi) {
            var txt_kode_des = document.getElementById("<%= txtKodeDeskripsi.ClientID %>");
            var txt_jenis_des = document.getElementById("<%= txtJenisDeskripsi.ClientID %>");
            var txt_des = document.getElementById("<%= txtIDTeksDeskripsi.ClientID %>");
            if (
                txt_kode_des != null && txt_kode_des != undefined &&
                txt_jenis_des != null && txt_jenis_des != undefined &&
                txt_des != null && txt_des != undefined
            ) {
                txt_kode_des.value = kode;
                txt_jenis_des.value = jenis;
                txt_des.value = id_deskripsi;
            }
        }

        function DoAutoSave() {
            setInterval(function () {
                var txt = document.getElementById("<%= txtIsAutoSave.ClientID %>");
                var txt_kode_des = document.getElementById("<%= txtKodeDeskripsi.ClientID %>");
                var txt_jenis_des = document.getElementById("<%= txtJenisDeskripsi.ClientID %>");
                var txt_des = document.getElementById(document.getElementById("<%= txtIDTeksDeskripsi.ClientID %>").value);

                if (
                    txt != null && txt != undefined &&
                    txt_kode_des != null && txt_kode_des != undefined &&
                    txt_jenis_des != null && txt_jenis_des != undefined &&
                    txt_des != null && txt_des != undefined
                ) {
                    if (txt.value == "1") {
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
                            success: function (data) {
                            },
                            error: function (response) {
                                alert(response.responseText);
                            },
                            failure: function (response) {
                                alert(response.responseText);
                            }
                        });

                        txt.value = "0";
                    }
                }
            }, 500);
        }

        function SetIsAutosave(value) {
            var txt = document.getElementById("<%= txtIsAutoSave.ClientID %>");
            if (txt != null && txt != undefined) {
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

        function StopProsesDownload() {
            if (navigator.appName == 'Microsoft Internet Explorer') {
                window.frames['fra_download'].document.execCommand('Stop');
            } else {
                window.frames['fra_download'].stop();
            }

            ShowProsesBukaSemester(false);
            HideModal();
            <%--<%= btnDoRefreshBukaSemester.ClientID %>.click();--%>
        }

        function DoScrollPos() {
            var txt_x = document.getElementById("<%= txtXpos.ClientID %>");
            var txt_y = document.getElementById("<%= txtYpos.ClientID %>");
            if (
                txt_x !== null && txt_x !== undefined &&
                txt_y !== null && txt_y !== undefined
            ) {
                txt_x.value = window.pageXOffset;
                txt_y.value = window.pageYOffset;
            }
        }

        function SetScrollPos() {
            var txt_x = document.getElementById("<%= txtXpos.ClientID %>");
            var txt_y = document.getElementById("<%= txtYpos.ClientID %>");
            if (
                txt_x !== null && txt_x !== undefined &&
                txt_y !== null && txt_y !== undefined
            ) {
                window.scrollTo(txt_x.value, txt_y.value);
            }
        }

        function ShowProgress(value) {
            if (value) {
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
             <ucl:PostbackUpdateProgress runat="server" ID="pbUpdateProgress" />
        </ProgressTemplate>
    </asp:UpdateProgress>

   
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








            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-12">

                    <div class="col-md-8 col-md-offset-2" style="padding: 0px;">
                        <div class="card" style="margin-top: 40px;">
                            <div class="card-main">
                                <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px;">
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
                                                style="background-color: #00198d; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 33px; right: 50px; width: 295px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                                                <div style="padding-left: 0px;">
                                                    <%--<asp:LinkButton ToolTip=" Kembali " runat="server" ID="btnShowDataListFromKTSPDet" CssClass="btn-trans waves-attach waves-circle waves-effect" OnClick="btnShowDataListFromKTSPDet_Click" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;
                                                    </asp:LinkButton>--%>
                                                </div>
                                            </div>





                                            <div class="fbtn-container" id="div_button_settings_ktsp" runat="server">
                                                <%--<div class="fbtn-inner">
			                                        <a onclick="setTimeout(
                                                            function(){
                                                                <%= btnShowInputOnStrukturPenilaian.ClientID %>.click();
                                                            }, 500
                                                        ); return false;" class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #a91212;" title=" Tambah Data ">
                                                        <span class="fbtn-ori icon" style="padding-left: 2px;"><span class="fa fa-plus"></span></span>
                                                        <span class="fbtn-sub icon" style="padding-left: 2px;"><span class="fa fa-plus"></span></span>
                                                    </a>
		                                        </div>--%>
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
                                                                    <td colspan="7" style="background-color: #830000; font-weight: bold; color: white; padding-left: 0px;">Penilaian Sikap
                                                                    </td>
                                                                    <td style="background-color: #830000; text-align: right;">
                                                                        <%-- <label onclick="<%= btnShowInputKompetensiDasarKURTILASSikap.ClientID %>.click();" title=" Tambah Kompetensi Dasar " class="badge" style="cursor: pointer; font-weight: normal; font-size: x-small; background-color: grey;">
                                                                            <i class="fa fa-plus"></i>
                                                                            &nbsp;
                                                                            KD
                                                                        </label>--%>
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
                                                                    <td colspan="7" style="background-color: #890000; font-weight: bold; color: white; padding-left: 0px;">Penilaian Pengetahuan, Keterampilan & UAS
                                                                    </td>
                                                                    <td style="background-color: #890000;"></td>
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
                                                style="background-color: #00198d; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 33px; right: 50px; width: 100px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                                               <%-- <div style="padding-left: 0px;">
                                                    <asp:LinkButton ToolTip=" Kembali " runat="server" ID="btnShowDataListFromKURTILASDet" CssClass="btn-trans waves-attach waves-circle waves-effect" OnClick="btnShowDataListFromKURTILASDet_Click" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;
                                                    </asp:LinkButton>
                                                </div>--%>
                                            </div>


                                            <div class="fbtn-container" id="div_button_settings_kurtilas" runat="server">
                                                <div class="fbtn-inner">
                                                    <%-- <a onclick="setTimeout(
                                                            function(){
                                                                <%= btnShowInputOnStrukturPenilaian.ClientID %>.click();
                                                            }, 500
                                                        ); return false;" class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #a91212;" title=" Tambah Data ">
                                                        <span class="fbtn-ori icon" style="padding-left: 2px;"><span class="fa fa-plus"></span></span>
                                                        <span class="fbtn-sub icon" style="padding-left: 2px;"><span class="fa fa-plus"></span></span>
                                                    </a>--%>
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

            <div class="content-header ui-content-header"
                style="background-color: #00198d; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 33px; right: 50px; width: 450px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                <div style="padding-left: 0px;">
                    <asp:LinkButton ToolTip=" Kembali " runat="server" ID="LinkButton1" 
                        onclick="btnBackToMapel_Click"
                        CssClass="btn-trans waves-attach waves-circle waves-effect"  Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;Mata Pelajaran
                    </asp:LinkButton>
                </div>
            </div>
            <div class="content-header ui-content-header"
                style="background-color: red; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 6; position: fixed; bottom: 33px; right: 50px; width: 250px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                <div style="padding-left: 0px;">
                    <asp:LinkButton ToolTip=" Kembali " runat="server" ID="LinkButton2" 
                        onclick="btnBackToKelas_Click"
                        CssClass="btn-trans waves-attach waves-circle waves-effect"  Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;Kelas
                    </asp:LinkButton>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>

    <iframe name="fra_download" id="fra_download" height="0" width="0" style="position: absolute; left: -1000px; top: -1000px;"></iframe>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">

</script>

    <script type="text/javascript">
        RenderDropDownOnTables();
        //InitModalFocus();
        DoAutoSave();
    </script>
</asp:Content>
