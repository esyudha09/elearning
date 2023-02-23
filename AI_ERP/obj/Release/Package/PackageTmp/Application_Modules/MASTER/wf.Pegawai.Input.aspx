<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.Pegawai.Input.aspx.cs" Inherits="AI_ERP.Application_Modules.MASTER.wf_Pegawai_Input" %>

<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<%@ Register TagPrefix="ucl" TagName="AutocompleteJurusanPendidikan" Src="~/Application_Controls/AutocompleteJurusanPendidikan/AutocompleteJurusanPendidikan.ascx" %>
<%@ Register TagPrefix="ucl" TagName="AutocompleteUniversitas" Src="~/Application_Controls/AutocompleteUniversitas/AutocompleteUniversitas.ascx" %>
<%@ Register TagPrefix="ucl" TagName="AutocompleteJabatan" Src="~/Application_Controls/AutocompleteJabatan/AutocompleteJabatan.ascx" %>

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
            font-weight: bold;
            color: grey;
        }
    </style>
    <script type="text/javascript">
        function HideModal() {
            $('#ui_modal_pendidikan_formal').modal('hide');
            $('#ui_modal_pendidikan_nonformal').modal('hide');
            $('#ui_modal_confirm_hapus_pendidikan').modal('hide');
            $('#ui_modal_confirm_hapus_pendidikan_non_formal').modal('hide');
            $('#ui_modal_confirm_hapus_pengalaman_kerja_dalam').modal('hide');
            $('#ui_modal_confirm_hapus_pengalaman_kerja_luar').modal('hide');
            $('#ui_modal_confirm_hapus_pengalaman_sharing').modal('hide');
            $('#ui_modal_confirm_hapus_pengalaman_kepanitiaan').modal('hide');
            $('#ui_modal_confirm_hapus_riwayat_kesehatan').modal('hide');
            $('#ui_modal_confirm_hapus_riwayat_mcu').modal('hide');
            $('#ui_modal_pengalaman_kerja_dalam').modal('hide');
            $('#ui_modal_pengalaman_kerja_luar').modal('hide');
            $('#ui_modal_pengalaman_sharing').modal('hide');            
            $('#ui_modal_pengalaman_kepanitiaan').modal('hide');            
            $('#ui_modal_riwayat_kesehatan').modal('hide');            
            $('#ui_modal_riwayat_mcu').modal('hide');                        
            $('#ui_modal_upload_file_pendukung').modal('hide');
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();

            ShowProgress(false);
            document.body.style.paddingRight = "0px";
        }

        function InitPicker() {
            $('#<%= txtTanggalLahir.ClientID %>').pickdate({ cancel: 'Hapus Tanggal', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 100, today: '' });            
            $('#<%= txtTanggalLahirAyah.ClientID %>').pickdate({ cancel: 'Hapus Tanggal', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 160, today: '' });            
            $('#<%= txtTanggalLahirIbu.ClientID %>').pickdate({ cancel: 'Hapus Tanggal', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 160, today: '' });            
            $('#<%= txtTanggalLahirAyahMertua.ClientID %>').pickdate({ cancel: 'Hapus Tanggal', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 160, today: '' });            
            $('#<%= txtTanggalLahirIbuMertua.ClientID %>').pickdate({ cancel: 'Hapus Tanggal', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 160, today: '' });            
            $('#<%= txtTanggalLahirSuamiIstri.ClientID %>').pickdate({ cancel: 'Hapus Tanggal', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });            
            $('#<%= txtTanggalLahirAnakKe1.ClientID %>').pickdate({ cancel: 'Hapus Tanggal', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });            
            $('#<%= txtTanggalLahirAnakKe2.ClientID %>').pickdate({ cancel: 'Hapus Tanggal', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });            
            $('#<%= txtTanggalLahirAnakKe3.ClientID %>').pickdate({ cancel: 'Hapus Tanggal', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });            
            $('#<%= txtTanggalLahirAnakKe4.ClientID %>').pickdate({ cancel: 'Hapus Tanggal', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });            
            $('#<%= txtTanggalLahirAnakKe5.ClientID %>').pickdate({ cancel: 'Hapus Tanggal', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });            
            $('#<%= txtTanggalLahirAnakKe6.ClientID %>').pickdate({ cancel: 'Hapus Tanggal', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });          
            $('#<%= txtRiwayatKesehatanDariTanggal.ClientID %>').pickdate({ cancel: 'Hapus Tanggal', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 10, today: '' });            
            $('#<%= txtRiwayatKesehatanSampaiTanggal.ClientID %>').pickdate({ cancel: 'Hapus Tanggal', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 10, today: '' });            
            $('#<%= txtRiwayatMCUTanggal.ClientID %>').pickdate({ cancel: 'Hapus Tanggal', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 10, today: '' });            
        }

        function EndRequestHandler() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        }

        function EndRequest() {
            var jenis_act = document.getElementById("<%= txtKeyAction.ClientID %>").value;
            switch (jenis_act) {
                case "<%= JenisAction.DoShowInputPendidikan %>":
                    $('#ui_modal_pendidikan_formal').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowInputPendidikanNonFormal %>":
                    $('#ui_modal_pendidikan_nonformal').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowInputPengalamanKerjaDalam %>":
                    $('#ui_modal_pengalaman_kerja_dalam').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowInputPengalamanKerjaLuar %>":
                    $('#ui_modal_pengalaman_kerja_luar').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowInputPengalamanSharing %>":
                    $('#ui_modal_pengalaman_sharing').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowInputPengalamanKepanitiaan %>":
                    $('#ui_modal_pengalaman_kepanitiaan').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowInputRiwayatKesehatan %>":
                    $('#ui_modal_riwayat_kesehatan').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowInputUploadFilePendukung %>":
                    $('#ui_modal_upload_file_pendukung').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowInputRiwayatMCU %>":
                    $('#ui_modal_riwayat_mcu').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowDataFilePendukung %>":
                    <%= btnShowUploadFilePendukung.ClientID %>.click();
                    break;
                case "<%= JenisAction.DoShowDataListFilePendukungWithNotifDelete %>":
                    HideModal();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;File sudah dihapus',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                    break;
                case "<%= JenisAction.DoShowDataListFilePendukung %>":
                    HideModal();
                    break;
                case "<%= JenisAction.DoShowDataPendidikanFormal %>":
                    HideModal();
                    break;
                case "<%= JenisAction.DoShowDataPendidikanNonFormal %>":
                    HideModal();
                    break;
                case "<%= JenisAction.DoShowDataPengalamanKerjaDalam %>":
                    HideModal();
                    break;
                case "<%= JenisAction.DoShowDataPengalamanKerjaLuar %>":
                    HideModal();
                    break;
                case "<%= JenisAction.DoShowDataPengalamanSharing %>":
                    HideModal();
                    break;
                case "<%= JenisAction.DoShowDataPengalamanKepanitiaan %>":
                    HideModal();
                    break;
                case "<%= JenisAction.DoShowDataRiwayatKesehatan %>":
                    HideModal();
                    break;
                case "<%= JenisAction.DoShowDataRiwayatMCU %>":
                    HideModal();
                    break;
                case "<%= JenisAction.DoShowDataRiwayatKesehatanAndDoDeleteDirectory %>":
                    HideModal();
                    DeleteDirectoryRiwayatKesehatan();
                    break;
                case "<%= JenisAction.DoShowDataRiwayatMCUAndDoDeleteDirectory %>":
                    HideModal();
                    DeleteDirectoryRiwayatMCU();
                    break;
                case "<%= JenisAction.DoShowConfirmHapusPendidikan %>":
                    $('#ui_modal_confirm_hapus_pendidikan').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmHapusPendidikanNonFormal %>":
                    $('#ui_modal_confirm_hapus_pendidikan_non_formal').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmHapusPengalamanKerjaDalam %>":
                    $('#ui_modal_confirm_hapus_pengalaman_kerja_dalam').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmHapusPengalamanKerjaLuar %>":
                    $('#ui_modal_confirm_hapus_pengalaman_kerja_luar').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmHapusPengalamanSharing %>":
                    $('#ui_modal_confirm_hapus_pengalaman_sharing').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmHapusPengalamanKepanitiaan %>":
                    $('#ui_modal_confirm_hapus_pengalaman_kepanitiaan').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmHapusRiwayatKesehatan %>":
                    $('#ui_modal_confirm_hapus_riwayat_kesehatan').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmHapusRiwayatMCU %>":
                    $('#ui_modal_confirm_hapus_riwayat_mcu').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoUpdate %>":
                    HideModal();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
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

            document.getElementById("<%= txtKeyAction.ClientID %>").value = "";
            InitPicker();
            InitModalFocus();
            RenderDropDownOnTables();
            <%= txtJurusanPendidikan.NamaClientID %>_SHOW_AUTOCOMPLETE();
            <%= txtUniversitas.NamaClientID %>_SHOW_AUTOCOMPLETE();
            <%= txtJabatanPengalamanKerjaDalam.NamaClientID %>_SHOW_AUTOCOMPLETE();
            <%= txtJabatanPengalamanKerjaLuar.NamaClientID %>_SHOW_AUTOCOMPLETE();
            document.getElementById("<%= txtKeyAction.ClientID %>").value = "";            
            
            Sys.Browser.WebKit = {};
            if (navigator.userAgent.indexOf('WebKit/') > -1) {
                Sys.Browser.agent = Sys.Browser.WebKit;
                Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
                Sys.Browser.name = 'WebKit';
            }

            setTimeout(function(){ ShowProgress(false); }, 500);
        }

        function ShowProgress(value){
            if(value){
                pb_top.style.display = "";
            } else {
                pb_top.style.display = "none";
            }
        }

        function InitModalFocus(){
            $('#ui_modal_pendidikan_formal').on('shown.bs.modal', function () {
                <%= cboJenisPendidikan.ClientID %>.focus();
            });

            $('#ui_modal_pendidikan_nonformal').on('shown.bs.modal', function () {
                ShowUploaderPendidikanNonFormal();
                <%= txtJenisPendidikanNonFormal.ClientID %>.focus();
            });

            $('#ui_modal_pengalaman_kerja_dalam').on('shown.bs.modal', function () {
                <%= cboDivisiPengalamanKerjaDalam.ClientID %>.focus();
            });

            $('#ui_modal_pengalaman_kerja_luar').on('shown.bs.modal', function () {
                <%= txtNamaPerusahaanPengalamanLuar.ClientID %>.focus();
            });

            $('#ui_modal_pengalaman_sharing').on('shown.bs.modal', function () {
                <%= txtTahunPengalamanSharing.ClientID %>.focus();
            });

            $('#ui_modal_pengalaman_kepanitiaan').on('shown.bs.modal', function () {
                <%= txtTahunPengalamanKepanitiaan.ClientID %>.focus();
            });

            $('#ui_modal_riwayat_kesehatan').on('shown.bs.modal', function () {
                ShowUploaderRiwayatKesehatan();
            });

            $('#ui_modal_riwayat_mcu').on('shown.bs.modal', function () {
                ShowUploaderRiwayatMCU();
            });

            $('#ui_modal_upload_file_pendukung').on('shown.bs.modal', function () {
                ShowUploaderFilePendukung();
            });
        }

        var intervalDelete = null;
        function DeleteBatalPendidikanNonFormal(){
            var fra = document.getElementById("fraUploaderPendidikanNonFormal");
            if(fra !== null && fra !== undefined) {
                var innerDoc = fra.contentDocument || fra.contentWindow.document;
                if(innerDoc != null && innerDoc != undefined){
                    ShowProsesBatal(true);

                    var id = 1;
                    var arr = innerDoc.getElementsByName("arr_delete[]");   
                    for (var i = arr.length - 1; i >= 0; i--) {
                        var btn = arr[i];
                        $.ajax({
                            url: btn.lang,
                            dataType: "json",
                            type: "GET",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) { },
                            error: function (response) { },
                            failure: function (response) { }
                        });
                        btn.click();
                        id++;
                    }

                    intervalDelete = setInterval(function(){
                        var arr = innerDoc.getElementsByName("arr_delete[]");                           
                        if(arr.length == 0){
                            ShowProsesBatal(false);
                            HideModal();
                            clearInterval(intervalDelete);   
                            DoDeleteDirectoryNewPendidikanNonFormalCancel();
                        }
                    }, 1000);
                }
            } 
            else {
                HideModal();
            }
        }

        function DeleteBatalRiwayatKesehatan(){
            var fra = document.getElementById("fraUploaderRiwayatKesehatan");
            if(fra !== null && fra !== undefined) {
                var innerDoc = fra.contentDocument || fra.contentWindow.document;
                if(innerDoc != null && innerDoc != undefined){
                    ShowProsesBatal(true);

                    var id = 1;
                    var arr = innerDoc.getElementsByName("arr_delete[]");   
                    for (var i = arr.length - 1; i >= 0; i--) {
                        var btn = arr[i];
                        $.ajax({
                            url: btn.lang,
                            dataType: "json",
                            type: "GET",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) { },
                            error: function (response) { },
                            failure: function (response) { }
                        });
                        btn.click();
                        id++;
                    }

                    intervalDelete = setInterval(function(){
                        var arr = innerDoc.getElementsByName("arr_delete[]");                           
                        if(arr.length == 0){
                            ShowProsesBatal(false);
                            HideModal();
                            clearInterval(intervalDelete);   
                            DoDeleteDirectoryNewRiwayatKesehatanCancel();
                        }
                    }, 1000);
                }
            } 
            else {
                HideModal();
            }
        }

        function DeleteBatalRiwayatMCU(){
            var fra = document.getElementById("fraUploaderRiwayatMCU");
            if(fra !== null && fra !== undefined) {
                var innerDoc = fra.contentDocument || fra.contentWindow.document;
                if(innerDoc != null && innerDoc != undefined){
                    ShowProsesBatal(true);

                    var id = 1;
                    var arr = innerDoc.getElementsByName("arr_delete[]");   
                    for (var i = arr.length - 1; i >= 0; i--) {
                        var btn = arr[i];
                        $.ajax({
                            url: btn.lang,
                            dataType: "json",
                            type: "GET",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) { },
                            error: function (response) { },
                            failure: function (response) { }
                        });
                        btn.click();
                        id++;
                    }

                    intervalDelete = setInterval(function(){
                        var arr = innerDoc.getElementsByName("arr_delete[]");                           
                        if(arr.length == 0){
                            ShowProsesBatal(false);
                            HideModal();
                            clearInterval(intervalDelete);   
                            DoDeleteDirectoryNewRiwayatMCUCancel();
                        }
                    }, 1000);
                }
            } 
            else {
                HideModal();
            }
        }

        function DoDeleteDirectoryNewPendidikanNonFormalCancel(){
            if(<%= txtIDPendidikanNonFormal.ClientID %>.value.trim() === "" && <%= txtIDPendidikanNonFormalNew.ClientID %>.value.trim() !== ""){
                DeleteDirectoryPendidikanNonFormal();
            }
        }

        function DoDeleteDirectoryNewRiwayatKesehatanCancel(){
            if(<%= txtIDRiwayatKesehatan.ClientID %>.value.trim() === "" && <%= txtIDRiwayatKesehatanNew.ClientID %>.value.trim() !== ""){
                DeleteDirectoryRiwayatKesehatan();
            }
        }

        function DoDeleteDirectoryNewRiwayatMCUCancel(){
            if(<%= txtIDRiwayatMCU.ClientID %>.value.trim() === "" && <%= txtIDRiwayatMCUNew.ClientID %>.value.trim() !== ""){
                DeleteDirectoryRiwayatMCU();
            }
        }

        function DeleteDirectoryPendidikanNonFormal(){
            var s_url = <%= txtURLDeletePendidikanNonFormal.ClientID %>.value ;
            $.ajax({
                url: s_url,
                dataType: "json",
                type: "GET",
                contentType: "application/json; charset=utf-8",
                success: function (data) { },
                error: function (response) { },
                failure: function (response) { }
            });            
        }

        function DeleteDirectoryRiwayatKesehatan(){
            var s_url = <%= txtURLDeleteRiwayatKesehatan.ClientID %>.value ;
            $.ajax({
                url: s_url,
                dataType: "json",
                type: "GET",
                contentType: "application/json; charset=utf-8",
                success: function (data) { },
                error: function (response) { },
                failure: function (response) { }
            });            
        }

        function DeleteDirectoryRiwayatMCU(){
            var s_url = <%= txtURLDeleteRiwayatMCU.ClientID %>.value ;
            $.ajax({
                url: s_url,
                dataType: "json",
                type: "GET",
                contentType: "application/json; charset=utf-8",
                success: function (data) { },
                error: function (response) { },
                failure: function (response) { }
            });            
        }
        
        function ShowProsesBatal(show){
            if(lbl_delete_batal_upload_pendidikan_non_formal !== undefined && lbl_delete_batal_upload_pendidikan_non_formal !== null)
                lbl_delete_batal_upload_pendidikan_non_formal.style.display = (show ? "" : " none ");
            if(btn_batal_upload_pendidikan_non_formal !== undefined && btn_batal_upload_pendidikan_non_formal !== null)
                btn_batal_upload_pendidikan_non_formal.style.display = (!show ? "" : " none ");

            if(lbl_delete_batal_upload_riwayat_kesehatan !== undefined && lbl_delete_batal_upload_riwayat_kesehatan !== null)
                lbl_delete_batal_upload_riwayat_kesehatan.style.display = (show ? "" : " none ");
            if(btn_batal_upload_riwayat_kesehatan !== undefined && btn_batal_upload_riwayat_kesehatan !== null)
                btn_batal_upload_riwayat_kesehatan.style.display = (!show ? "" : " none ");

            if(lbl_delete_batal_upload_riwayat_mcu !== undefined && lbl_delete_batal_upload_riwayat_mcu !== null)
                lbl_delete_batal_upload_riwayat_mcu.style.display = (show ? "" : " none ");

            if(btn_batal_upload_riwayat_mcu !== undefined && btn_batal_upload_riwayat_mcu !== null)
                btn_batal_upload_riwayat_mcu.style.display = (!show ? "" : " none ");
        }

        function DoDeleteFileClick(btn){
            btn.click();
        }

        function ShowUploaderPendidikanNonFormal(){
            var fra = document.getElementById("fraUploaderPendidikanNonFormal");
            if(fra !== null && fra !== undefined) {                
                fra.src = "<%= ResolveUrl("~/Application_Resources/Uploader.aspx?jenis=" + AI_ERP.Application_Libs.Libs.JENIS_UPLOAD.PENDIDIKAN_NON_FORMAL + "&id=") %>" + 
                           <%= txtNIKaryawan.ClientID %>.value + 
                          "&id2=" + 
                          (
                            <%= txtIDPendidikanNonFormal.ClientID %>.value.trim() === "" && <%= txtIDPendidikanNonFormalNew.ClientID %>.value.trim() !== ""
                            ? <%= txtIDPendidikanNonFormalNew.ClientID %>.value.trim()
                            : <%= txtIDPendidikanNonFormal.ClientID %>.value.trim()
                          );
            }
        }

        function ShowUploaderRiwayatKesehatan(){
            var fra = document.getElementById("fraUploaderRiwayatKesehatan");
            if(fra !== null && fra !== undefined) {                
                fra.src = "<%= ResolveUrl("~/Application_Resources/Uploader.aspx?jenis=" + AI_ERP.Application_Libs.Libs.JENIS_UPLOAD.RIWAYAT_KESEHATAN + "&id=") %>" + 
                           <%= txtNIKaryawan.ClientID %>.value + 
                          "&id2=" + 
                          (
                            <%= txtIDRiwayatKesehatan.ClientID %>.value.trim() === "" && <%= txtIDRiwayatKesehatanNew.ClientID %>.value.trim() !== ""
                            ? <%= txtIDRiwayatKesehatanNew.ClientID %>.value.trim()
                            : <%= txtIDRiwayatKesehatan.ClientID %>.value.trim()
                          );
            }
        }

        function ShowUploaderRiwayatMCU(){
            var fra = document.getElementById("fraUploaderRiwayatMCU");
            if(fra !== null && fra !== undefined) {                
                fra.src = "<%= ResolveUrl("~/Application_Resources/Uploader.aspx?jenis=" + AI_ERP.Application_Libs.Libs.JENIS_UPLOAD.RIWAYAT_MCU + "&id=") %>" + 
                           <%= txtNIKaryawan.ClientID %>.value + 
                          "&id2=" + 
                          (
                            <%= txtIDRiwayatMCU.ClientID %>.value.trim() === "" && <%= txtIDRiwayatMCUNew.ClientID %>.value.trim() !== ""
                            ? <%= txtIDRiwayatMCUNew.ClientID %>.value.trim()
                            : <%= txtIDRiwayatMCU.ClientID %>.value.trim()
                          );
            }
        }

        function ShowUploaderFilePendukung(){
            var fra = document.getElementById("fraUploaderFilePendukung");
            if(fra !== null && fra !== undefined) {
                fra.src = "<%= ResolveUrl("~/Application_Resources/Uploader.aspx?jenis=" + AI_ERP.Application_Libs.Libs.JENIS_UPLOAD.FILE_PENDUKUNG + "&id=") %>" + 
                           <%= txtNIKaryawan.ClientID %>.value + 
                          "&id2=Files";
            }
        }

        function ShowUploadedFilesPendukung(){
            <%= btnShowUploadFilePendukungWithNotifDelete.ClientID %>.click();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress runat="server" ID="upProgressMain" AssociatedUpdatePanelID="upMain">
        <ProgressTemplate>
            <%--<ucl:PostbackUpdateProgress runat="server" ID="pbUpdateProgress" />--%>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div id="pb_top" class="progress progress-position-absolute-top" style="display: none; position: fixed; top: 0px; right: 0px; z-index: 9999999;">
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

    <asp:UpdatePanel runat="server" ID="upMain">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="txtID" />
            <asp:HiddenField runat="server" ID="txtIDPendidikanFormal" />
            <asp:HiddenField runat="server" ID="txtIDPendidikanNonFormal" />
            <asp:HiddenField runat="server" ID="txtIDPendidikanNonFormalNew" />
            <asp:HiddenField runat="server" ID="txtIDPengalamanKerjaDalam" />
            <asp:HiddenField runat="server" ID="txtIDPengalamanKerjaLuar" />
            <asp:HiddenField runat="server" ID="txtIDPengalamanSharing" />
            <asp:HiddenField runat="server" ID="txtIDPengalamanKepanitiaan" />
            <asp:HiddenField runat="server" ID="txtIDRiwayatKesehatan" />
            <asp:HiddenField runat="server" ID="txtIDRiwayatKesehatanNew" />
            <asp:HiddenField runat="server" ID="txtIDRiwayatMCU" />
            <asp:HiddenField runat="server" ID="txtIDRiwayatMCUNew" />

            <asp:HiddenField runat="server" ID="txtURLDeletePendidikanNonFormal" />
            <asp:HiddenField runat="server" ID="txtURLDeleteRiwayatKesehatan" />
            <asp:HiddenField runat="server" ID="txtURLDeleteRiwayatMCU" />

            <asp:Button OnClientClick="ShowProgress(true);" UseSubmitBehavior="false" runat="server" ID="btnShowDetailPendidikanFormal" OnClick="btnShowDetailPendidikanFormal_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button OnClientClick="ShowProgress(true);" UseSubmitBehavior="false" runat="server" ID="btnShowConfirmDeletePendidikanFormal" OnClick="btnShowConfirmDeletePendidikanFormal_Click" Style="position: absolute; left: -1000px; top: -1000px;" />

            <asp:Button OnClientClick="ShowProgress(true);" UseSubmitBehavior="false" runat="server" ID="btnShowDetailPendidikanNonFormal" OnClick="btnShowDetailPendidikanNonFormal_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button OnClientClick="ShowProgress(true);" UseSubmitBehavior="false" runat="server" ID="btnShowConfirmDeletePendidikanNonFormal" OnClick="btnShowConfirmDeletePendidikanNonFormal_Click" Style="position: absolute; left: -1000px; top: -1000px;" />

            <asp:Button OnClientClick="ShowProgress(true);" UseSubmitBehavior="false" runat="server" ID="btnShowDetailPengalamanKerjaDalam" OnClick="btnShowDetailPengalamanKerjaDalam_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button OnClientClick="ShowProgress(true);" UseSubmitBehavior="false" runat="server" ID="btnShowConfirmDeletePengalamanKerjaDalam" OnClick="btnShowConfirmDeletePengalamanKerjaDalam_Click" Style="position: absolute; left: -1000px; top: -1000px;" />

            <asp:Button OnClientClick="ShowProgress(true);" UseSubmitBehavior="false" runat="server" ID="btnShowDetailPengalamanKerjaLuar" OnClick="btnShowDetailPengalamanKerjaLuar_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button OnClientClick="ShowProgress(true);" UseSubmitBehavior="false" runat="server" ID="btnShowConfirmDeletePengalamanKerjaLuar" OnClick="btnShowConfirmDeletePengalamanKerjaLuar_Click" Style="position: absolute; left: -1000px; top: -1000px;" />

            <asp:Button OnClientClick="ShowProgress(true);" UseSubmitBehavior="false" runat="server" ID="btnShowDetailPengalamanSharing" OnClick="btnShowDetailPengalamanSharing_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button OnClientClick="ShowProgress(true);" UseSubmitBehavior="false" runat="server" ID="btnShowConfirmDeletePengalamanSharing" OnClick="btnShowConfirmDeletePengalamanSharing_Click" Style="position: absolute; left: -1000px; top: -1000px;" />

            <asp:Button OnClientClick="ShowProgress(true);" UseSubmitBehavior="false" runat="server" ID="btnShowDetailPengalamanKepanitiaan" OnClick="btnShowDetailPengalamanKepanitiaan_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button OnClientClick="ShowProgress(true);" UseSubmitBehavior="false" runat="server" ID="btnShowConfirmDeletePengalamanKepanitiaan" OnClick="btnShowConfirmDeletePengalamanKepanitiaan_Click" Style="position: absolute; left: -1000px; top: -1000px;" />

            <asp:Button OnClientClick="ShowProgress(true);" UseSubmitBehavior="false" runat="server" ID="btnShowDetailRiwayatKesehatan" OnClick="btnShowDetailRiwayatKesehatan_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button OnClientClick="ShowProgress(true);" UseSubmitBehavior="false" runat="server" ID="btnShowConfirmDeleteRiwayatKesehatan" OnClick="btnShowConfirmDeleteRiwayatKesehatan_Click" Style="position: absolute; left: -1000px; top: -1000px;" />

            <asp:Button OnClientClick="ShowProgress(true);" UseSubmitBehavior="false" runat="server" ID="btnShowDetailRiwayatMCU" OnClick="btnShowDetailRiwayatMCU_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button OnClientClick="ShowProgress(true);" UseSubmitBehavior="false" runat="server" ID="btnShowConfirmDeleteRiwayatMCU" OnClick="btnShowConfirmDeleteRiwayatMCU_Click" Style="position: absolute; left: -1000px; top: -1000px;" />

            <asp:Button OnClientClick="ShowProgress(true);" UseSubmitBehavior="false" runat="server" ID="btnShowUploadFilePendukung" OnClick="btnShowUploadFilePendukung_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button OnClientClick="ShowProgress(true);" UseSubmitBehavior="false" runat="server" ID="btnShowUploadFilePendukungWithNotifDelete" OnClick="btnShowUploadFilePendukungWithNotifDelete_Click" Style="position: absolute; left: -1000px; top: -1000px;" />

            <iframe src="<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.ALL_UNIT.DELETE_FILE.ROUTE) %>" id="fraDelete" style="position: absolute; left: -1000px; width: -1000px;"></iframe>

            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-lg-8 col-lg-offset-2 col-md-8 col-md-offset-2">
                    <section class="content-inner margin-top-no" style="border-top-style: solid; border-top-color: #ff4081; border-top-width: 5px;">
                        <div class="card" style="box-shadow: 0 0px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24); border-top-left-radius: unset; border-top-right-radius: unset; margin-top: 0px;">
                            <div class="card-main">

                                <asp:HiddenField runat="server" ID="txtKeyAction" />
                                <asp:HiddenField runat="server" ID="txtIDPSB" />

                                <div class="row" style="margin-left: 25px; margin-right: 25px;">
                                    <div id="content_div_isi_biodata_calon_siswa" class="tile"
                                        style="box-shadow: none; margin-bottom: 10px; margin-top: 25px; border-width: 0px; border-radius: 0px; margin-top: 20px;">
                                        <div style="margin: 0  auto; display: table; height: 100px; width: 100px; border-radius: 100%; background: transparent;">
                                            <asp:Literal runat="server" ID="ltrFotoPegawai"></asp:Literal>
                                        </div>
                                        <sup style="position: absolute; left: 10px; top: 10px;">
                                            <label style="float: left; color: white; background-color: #DA0379; padding: 10px; margin: 0px;">
                                                Tanggal Masuk
                                            </label>
                                            <label style="float: left; color: white; background-color: #1da3d7; padding: 10px; margin: 0px; font-weight: bold; color: white;">
                                                <asp:Label runat="server" ID="lblTanggalMasuk"></asp:Label>
                                            </label>
                                            <label style="float: left; height: 0; width: 0; border-top: 10px solid transparent; border-left: 20px solid #1da3d7; border-bottom: 10px solid transparent;">
                                            </label>
                                        </sup>
                                    </div>

                                    <div class="row" style="margin-top: 0px;">
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                                <label for="<%= txtNIKaryawan.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    NIK (Karyawan)
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNIKaryawan"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-9">
                                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                                <label for="<%= txtNama.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Nama Lengkap
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNama"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                                <label for="<%= txtTempatLahir.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tempat Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtTempatLahir"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                                <label for="<%= txtTanggalLahir.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tanggal Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtTanggalLahir"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                                <label for="<%= cboJenisKelamin.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Jenis Kelamin
                                                </label>
                                                <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboJenisKelamin" CssClass="input-box">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Laki-Laki" Value="L"></asp:ListItem>
                                                    <asp:ListItem Text="Perempuan" Value="P"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                                <label for="<%= cboAgama.ClientID %>" style="color: #B7770D; font-size: small;">Agama</label>
                                                <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboAgama" CssClass="input-box">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Islam" Value="ISLAM"></asp:ListItem>
                                                    <asp:ListItem Text="Protestan" Value="PROTESTAN"></asp:ListItem>
                                                    <asp:ListItem Text="Katolik" Value="KATOLIK"></asp:ListItem>
                                                    <asp:ListItem Text="Hindu" Value="HINDU"></asp:ListItem>
                                                    <asp:ListItem Text="Budha" Value="BUDHA"></asp:ListItem>
                                                    <asp:ListItem Text="Konghuchu" Value="KONGHUCHU"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                                <label for="<%= cboTempatTinggal.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Menetap Di/Tinggal Di
                                                </label>
                                                <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboTempatTinggal" CssClass="input-box">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Kost" Value="KOST"></asp:ListItem>
                                                    <asp:ListItem Text="Rumah Orang Tua" Value="RUMAH ORANG TUA"></asp:ListItem>
                                                    <asp:ListItem Text="Rumah Saudara" Value="RUMAH SAUDARA"></asp:ListItem>
                                                    <asp:ListItem Text="Rumah Sendiri" Value="RUMAH SENDIRI"></asp:ListItem>
                                                    <asp:ListItem Text="Sewa/Kontrak" Value="SEWA/KONTRAK"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                                <label for="<%= txtAlamat.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Alamat Rumah
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtAlamat" TextMode="MultiLine" Rows="3" Style="resize: none;"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                                <label for="<%= txtKodePOS.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Kota
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtKota"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                                <label for="<%= txtKodePOS.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Kode POS
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtKodePOS"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                                <label for="<%= txtTelpon.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Telpon/No. HP
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtTelpon"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <label for="<%= cboStatusPerkawinan.ClientID %>" style="color: #B7770D; font-size: small;">
                                                Status Perkawinan
                                            </label>
                                            <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboStatusPerkawinan" CssClass="input-box">
                                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Belum Kawin" Value="BELUM KAWIN"></asp:ListItem>
                                                <asp:ListItem Text="Duda/Janda" Value="DUDA/JANDA"></asp:ListItem>
                                                <asp:ListItem Text="Kawin" Value="KAWIN"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                                <label for="<%= txtEmail.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Email
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtEmail"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                                <label for="<%= txtEmail.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Email Pribadi
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtEmailPribadi"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                                <label for="<%= txtNoKTP.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No. KTP/NIK
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNoKTP"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                                <label for="<%= txtNoKK.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No. KK
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNoKK"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <br />
                                            <hr style="margin: 0px; margin-top: 15px; border-color: #cbcbcb; border-width: 1px; margin-right: -25px;" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <sup style="position: absolute; left: -15px; top: -10.5px;">
                                                <label style="float: left; color: white; background-color: #1da3d7; padding: 10px; margin: 0px; font-weight: bold; color: white;">
                                                    Data Keluarga
                                                </label>
                                                <label style="float: left; height: 0; width: 0; border-top: 10px solid transparent; border-left: 20px solid #1da3d7; border-bottom: 10px solid transparent;">
                                                </label>
                                            </sup>
                                            &nbsp;&nbsp;
                                        </div>
                                    </div>

                                    <div class="row" style="margin-top: 10px;">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNamaAyah.ClientID %>" style="color: #B7770D; font-size: small; font-weight: bold;">
                                                    <i class="fa fa-tag"></i>
                                                    &nbsp;
                                                    Nama Ayah
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaAyah" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTempatLahirAyah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tempat Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTempatLahirAyah" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTanggalLahirAyah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tanggal Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTanggalLahirAyah" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNIKAyah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    NIK
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNIKAyah" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNoBPJSKesehatanAyah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.BPJS Kesehatan
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNoBPJSKesehatanAyah" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <hr style="margin: 0px; margin-top: 15px; border-color: white; border-width: 2px; margin-left: -25px; margin-right: -25px;" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNamaIbu.ClientID %>" style="color: #B7770D; font-size: small; font-weight: bold;">
                                                    <i class="fa fa-tag"></i>
                                                    &nbsp;
                                                    Nama Ibu
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaIbu" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTempatLahirIbu.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tempat Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTempatLahirIbu" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTanggalLahirIbu.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tanggal Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTanggalLahirIbu" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNIKIbu.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    NIK
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNIKIbu" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNoBPJSKesehatanIbu.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.BPJS Kesehatan
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNoBPJSKesehatanIbu" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <hr style="margin: 0px; margin-top: 15px; border-color: white; border-width: 2px; margin-left: -25px; margin-right: -25px;" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNamaAyahMertua.ClientID %>" style="color: #B7770D; font-size: small; font-weight: bold;">
                                                    <i class="fa fa-tag"></i>
                                                    &nbsp;
                                                    Nama Ayah Mertua
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaAyahMertua" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTempatLahirAyahMertua.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tempat Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTempatLahirAyahMertua" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTanggalLahirAyahMertua.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tanggal Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTanggalLahirAyahMertua" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNIKAyahMertua.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    NIK
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNIKAyahMertua" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNoBPJSKesehatanAyahMertua.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.BPJS Kesehatan
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNoBPJSKesehatanAyahMertua" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <hr style="margin: 0px; margin-top: 15px; border-color: white; border-width: 2px; margin-left: -25px; margin-right: -25px;" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNamaIbuMertua.ClientID %>" style="color: #B7770D; font-size: small; font-weight: bold;">
                                                    <i class="fa fa-tag"></i>
                                                    &nbsp;
                                                    Nama Ibu Mertua
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaIbuMertua" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTempatLahirIbuMertua.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tempat Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTempatLahirIbuMertua" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTanggalLahirIbuMertua.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tanggal Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTanggalLahirIbuMertua" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNIKIbuMertua.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    NIK
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNIKIbuMertua" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNoBPJSKesehatanIbuMertua.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.BPJS Kesehatan
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNoBPJSKesehatanIbuMertua" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <hr style="margin: 0px; margin-top: 15px; border-color: white; border-width: 2px; margin-left: -25px; margin-right: -25px;" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNamaSuamiIstri.ClientID %>" style="color: #B7770D; font-size: small; font-weight: bold;">
                                                    <i class="fa fa-tag"></i>
                                                    &nbsp;
                                                    Nama Suami/Istri
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaSuamiIstri" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTempatLahirSuamiIstri.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tempat Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTempatLahirSuamiIstri" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTanggalLahirSuamiIstri.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tanggal Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTanggalLahirSuamiIstri" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNIKSuamiIstri.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    NIK
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNIKSuamiIstri" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNoBPJSKesehatanSuamiIstri.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.BPJS Kesehatan
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNoBPJSKesehatanSuamiIstri" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <hr style="margin: 0px; margin-top: 15px; border-color: white; border-width: 2px; margin-left: -25px; margin-right: -25px;" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNamaAnakKe1.ClientID %>" style="color: #B7770D; font-size: small; font-weight: bold;">
                                                    <i class="fa fa-tag"></i>
                                                    &nbsp;
                                                    Nama Anak #1
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaAnakKe1" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTempatLahirAnakKe1.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tempat Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTempatLahirAnakKe1" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTanggalLahirAnakKe1.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tanggal Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTanggalLahirAnakKe1" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNIKAnakKe1.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    NIK
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNIKAnakKe1" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNoBPJSKesehatanAnakKe1.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.BPJS Kesehatan
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNoBPJSKesehatanAnakKe1" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <hr style="margin: 0px; margin-top: 15px; border-color: white; border-width: 2px; margin-left: -25px; margin-right: -25px;" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNamaAnakKe2.ClientID %>" style="color: #B7770D; font-size: small; font-weight: bold;">
                                                    <i class="fa fa-tag"></i>
                                                    &nbsp;
                                                    Nama Anak #2
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaAnakKe2" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTempatLahirAnakKe2.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tempat Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTempatLahirAnakKe2" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTanggalLahirAnakKe2.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tanggal Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTanggalLahirAnakKe2" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNIKAnakKe2.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    NIK
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNIKAnakKe2" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNoBPJSKesehatanAnakKe2.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.BPJS Kesehatan
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNoBPJSKesehatanAnakKe2" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <hr style="margin: 0px; margin-top: 15px; border-color: white; border-width: 2px; margin-left: -25px; margin-right: -25px;" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNamaAnakKe3.ClientID %>" style="color: #B7770D; font-size: small; font-weight: bold;">
                                                    <i class="fa fa-tag"></i>
                                                    &nbsp;
                                                    Nama Anak #3
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaAnakKe3" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTempatLahirAnakKe3.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tempat Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTempatLahirAnakKe3" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTanggalLahirAnakKe3.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tanggal Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTanggalLahirAnakKe3" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNIKAnakKe3.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    NIK
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNIKAnakKe3" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNoBPJSKesehatanAnakKe3.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.BPJS Kesehatan
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNoBPJSKesehatanAnakKe3" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <hr style="margin: 0px; margin-top: 15px; border-color: white; border-width: 2px; margin-left: -25px; margin-right: -25px;" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNamaAnakKe4.ClientID %>" style="color: #B7770D; font-size: small; font-weight: bold;">
                                                    <i class="fa fa-tag"></i>
                                                    &nbsp;
                                                    Nama Anak #4
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaAnakKe4" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTempatLahirAnakKe4.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tempat Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTempatLahirAnakKe4" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTanggalLahirAnakKe4.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tanggal Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTanggalLahirAnakKe4" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNIKAnakKe4.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    NIK
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNIKAnakKe4" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNoBPJSKesehatanAnakKe4.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.BPJS Kesehatan
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNoBPJSKesehatanAnakKe4" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <hr style="margin: 0px; margin-top: 15px; border-color: white; border-width: 2px; margin-left: -25px; margin-right: -25px;" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNamaAnakKe5.ClientID %>" style="color: #B7770D; font-size: small; font-weight: bold;">
                                                    <i class="fa fa-tag"></i>
                                                    &nbsp;
                                                    Nama Anak #5
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaAnakKe5" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTempatLahirAnakKe5.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tempat Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTempatLahirAnakKe5" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTanggalLahirAnakKe5.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tanggal Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTanggalLahirAnakKe5" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNIKAnakKe5.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    NIK
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNIKAnakKe5" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNoBPJSKesehatanAnakKe5.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.BPJS Kesehatan
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNoBPJSKesehatanAnakKe5" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <hr style="margin: 0px; margin-top: 15px; border-color: white; border-width: 2px; margin-left: -25px; margin-right: -25px;" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNamaAnakKe6.ClientID %>" style="color: #B7770D; font-size: small; font-weight: bold;">
                                                    <i class="fa fa-tag"></i>
                                                    &nbsp;
                                                    Nama Anak #6
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaAnakKe6" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTempatLahirAnakKe6.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tempat Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTempatLahirAnakKe6" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtTanggalLahirAnakKe6.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tanggal Lahir
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTanggalLahirAnakKe6" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNIKAnakKe6.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    NIK
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNIKAnakKe6" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
                                                <label for="<%= txtNoBPJSKesehatanAnakKe6.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.BPJS Kesehatan
                                                </label>
                                                <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNoBPJSKesehatanAnakKe6" CssClass="input-box"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="background-color: #e3e3e3; margin-left: -25px; margin-right: -25px; margin-top: 30px; border-style: none; border-top-style: solid; border-top-width: 1px; border-top-color: #cbcbcb;">
                                        <div class="col-md-12" style="padding-bottom: 8px; padding-top: 15px;">
                                            <sup style="position: absolute; left: -15px; top: -10.5px;">
                                                <label style="float: left; color: white; background-color: #1da3d7; padding: 10px; margin: 0px; font-weight: bold; color: white;">
                                                    Data Pendidikan Formal
                                                </label>
                                                <label style="float: left; height: 0; width: 0; border-top: 10px solid transparent; border-left: 20px solid #1da3d7; border-bottom: 10px solid transparent;">
                                                </label>
                                            </sup>
                                            <asp:LinkButton OnClientClick="ShowProgress(true);" ToolTip=" Tambah Data " runat="server" ID="lnkTambahDataPendidikanFormal" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" OnClick="lnkTambahDataPendidikanFormal_Click" Style="font-weight: bold; font-size: small; border-style: none; padding: 5px; margin-bottom: 0px; color: grey;">
                                                &nbsp;
                                                <span style="color: #1da3d7; text-transform: none;">
                                                    <i class="fa fa-plus"></i>
                                                    &nbsp;
								                    Tambah Data
                                                </span>
                                                &nbsp;
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                    <div class="row" style="margin-top: 10px; margin-left: -41px; margin-right: -41.5px; margin-top: -40px;">
                                        <div class="col-md-12">

                                            <div style="padding: 0px; margin: 0px;">
                                                <asp:ListView ID="lvDataPendidikan" runat="server">
                                                    <LayoutTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="text-align: center; background-color: #ebebeb; width: 80px; vertical-align: middle; color: grey; font-size: 13px;">
                                                                            <i class="fa fa-cogs" style="color: #a7a7a7;"></i>
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 5px; vertical-align: middle; color: grey; font-size: 13px;">Jenis Pendidikan, Lembaga
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: center; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Dari Tahun
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: center; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Sampai Tahun
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Nilai Akhir
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Jurusan
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
                                                            <td style="padding: 0px; text-align: center; width: 80px; vertical-align: middle;">
                                                                <div style="margin: 0 auto; <%# 
                                                                                Eval("JenisPendidikan").ToString().Trim() != "" 
                                                                                    ? " display: table; ": " display: none; "
                                                                            %>">
                                                                    <ul class="nav nav-list margin-no pull-left">
                                                                        <li class="dropdown">
                                                                            <a class="dropdown-toggle text-black waves-attach waves-effect" data-toggle="dropdown" style="color: grey; line-height: 15px; padding: 10px; margin: 0px; min-height: 15px; z-index: 0;" title=" Menu ">
                                                                                <i class="fa fa-cog"></i>
                                                                                <i class="fa fa-caret-down" style="font-size: xx-small;"></i>
                                                                            </a>
                                                                            <ul class="dropdown-menu-list-table">
                                                                                <li style="background-color: white; padding: 10px;">
                                                                                    <label
                                                                                        onclick="<%= txtIDPendidikanFormal.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetailPendidikanFormal.ClientID %>.click(); "
                                                                                        id="btnDetailPendidikanFormal" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                        <i class="fa fa-info-circle" title=" Lihat Detail "></i>&nbsp;&nbsp;&nbsp;Lihat Detail</label>
                                                                                </li>
                                                                                <li style="padding: 0px;">
                                                                                    <hr style="margin: 0px; padding: 0px;" />
                                                                                </li>
                                                                                <li style="background-color: white; padding: 10px;">
                                                                                    <label
                                                                                        onclick="<%= txtIDPendidikanFormal.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmDeletePendidikanFormal.ClientID %>.click(); "
                                                                                        id="btnHapusPendidikanFormal" style="color: #9E0000; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                        <i class="fa fa-times" title=" Hapus Data "></i>&nbsp;&nbsp;&nbsp;Hapus Data</label>
                                                                                </li>
                                                                            </ul>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 5px; vertical-align: middle; text-align: left;">
                                                                <label onclick="<%= txtIDPendidikanFormal.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetailPendidikanFormal.ClientID %>.click(); "
                                                                    style="color: grey; font-weight: normal; text-transform: none; text-decoration: none; cursor: pointer; color: #1da3d7; <%# Eval("JenisPendidikan").ToString().Trim() != "" ? "": " display: none; " %>">
                                                                    <span style="color: #1da3d7; font-weight: normal; text-transform: none; text-decoration: none; font-size: 13px;">
                                                                        <%# 
                                                                            Eval("JenisPendidikan").ToString().Trim() != "" 
                                                                                ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("JenisPendidikan").ToString())
                                                                                : ""
                                                                        %>
                                                                    </span>
                                                                </label>
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none; font-size: 13px;">
                                                                    <%# 
                                                                        Eval("JenisPendidikan").ToString().Trim() != "" 
                                                                            ? "<br />" + AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Lembaga").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none; font-size: 13px;">
                                                                    <%# 
                                                                        Eval("JenisPendidikan").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("DariTahun").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none; font-size: 13px;">
                                                                    <%# 
                                                                        Eval("JenisPendidikan").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("SampaiTahun").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none; font-size: 13px;">
                                                                    <%# 
                                                                        Eval("JenisPendidikan").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("NilaiAkhir").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none; font-size: 13px;">
                                                                    <%# 
                                                                        Eval("JenisPendidikan").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Jurusan").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="text-align: center; background-color: #ebebeb; width: 80px; vertical-align: middle; color: grey; font-size: 13px;">
                                                                            <i class="fa fa-cogs" style="color: #a7a7a7;"></i>
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Jenis Pendidikan, Lembaga
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: center; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Dari Tahun
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: center; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Sampai Tahun
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Nilai Akhir
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Jurusan
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="6" style="text-align: center; padding: 10px; font-size: 13px; color: #bfbfbf;">..:: Data Kosong ::..
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="background-color: #e3e3e3; margin-left: -25px; margin-right: -25px; margin-top: 0px;">
                                        <div class="col-md-12" style="padding-bottom: 8px; padding-top: 15px;">
                                            <sup style="position: absolute; left: -15px; top: -10px;">
                                                <label style="float: left; color: white; background-color: #1da3d7; padding: 10px; margin: 0px; font-weight: bold; color: white;">
                                                    Data Pendidikan Non Formal
                                                </label>
                                                <label style="float: left; height: 0; width: 0; border-top: 10px solid transparent; border-left: 20px solid #1da3d7; border-bottom: 10px solid transparent;">
                                                </label>
                                            </sup>
                                            <asp:LinkButton OnClientClick="ShowProgress(true);" ToolTip=" Tambah Data " runat="server" ID="lnkTambahDataPendidikanFormalNonFormal" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" OnClick="lnkTambahDataPendidikanFormalNonFormal_Click" Style="font-weight: bold; color: grey; font-size: small; border-style: none; padding: 5px; margin-bottom: 0px;">
                                                &nbsp;
                                                <span style="color: #1da3d7; text-transform: none;">
                                                    <i class="fa fa-plus"></i>
                                                    &nbsp;
								                    Tambah Data
                                                </span>
                                                &nbsp;
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                    <div class="row" style="margin-top: 10px; margin-left: -41px; margin-right: -41.5px; margin-top: -40px;">
                                        <div class="col-md-12">

                                            <div style="padding: 0px; margin: 0px;">
                                                <asp:ListView ID="lvDataPendidikanNonFormal" runat="server">
                                                    <LayoutTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="text-align: center; background-color: #ebebeb; width: 80px; vertical-align: middle; color: grey; font-size: 13px;">
                                                                            <i class="fa fa-cogs" style="color: #a7a7a7;"></i>
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 5px; vertical-align: middle; color: grey; font-size: 13px;">Jenis Pendidikan, Lembaga
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: center; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Dari
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: center; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Sampai
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Nilai Akhir
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Divisi
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Unit
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Keterangan
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
                                                            <td style="padding: 0px; text-align: center; width: 80px; vertical-align: middle;">
                                                                <div style="margin: 0 auto; <%# 
                                                                        Eval("JenisPendidikan").ToString().Trim() != "" 
                                                                            ? " display: table; ": " display: none; "
                                                                    %>">
                                                                    <ul class="nav nav-list margin-no pull-left">
                                                                        <li class="dropdown">
                                                                            <a class="dropdown-toggle text-black waves-attach waves-effect" data-toggle="dropdown" style="color: grey; line-height: 15px; padding: 10px; margin: 0px; min-height: 15px; z-index: 0;" title=" Menu ">
                                                                                <i class="fa fa-cog"></i>
                                                                                <i class="fa fa-caret-down" style="font-size: xx-small;"></i>
                                                                            </a>
                                                                            <ul class="dropdown-menu-list-table">
                                                                                <li style="background-color: white; padding: 10px;">
                                                                                    <label
                                                                                        onclick="<%= txtIDPendidikanNonFormal.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetailPendidikanNonFormal.ClientID %>.click(); "
                                                                                        id="btnDetailPendidikanNonFormal" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                        <i class="fa fa-info-circle" title=" Lihat Detail "></i>&nbsp;&nbsp;&nbsp;Lihat Detail</label>
                                                                                </li>
                                                                                <li style="padding: 0px;">
                                                                                    <hr style="margin: 0px; padding: 0px;" />
                                                                                </li>
                                                                                <li style="background-color: white; padding: 10px;">
                                                                                    <label
                                                                                        onclick="<%= txtIDPendidikanNonFormal.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmDeletePendidikanNonFormal.ClientID %>.click(); "
                                                                                        id="btnHapusPendidikanNonFormal" style="color: #9E0000; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                        <i class="fa fa-times" title=" Hapus Data "></i>&nbsp;&nbsp;&nbsp;Hapus Data</label>
                                                                                </li>
                                                                            </ul>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 5px; vertical-align: middle; text-align: left; font-size: 13px; position: relative">
                                                                <label onclick="<%= txtIDPendidikanNonFormal.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetailPendidikanNonFormal.ClientID %>.click(); "
                                                                    style="color: grey; font-weight: normal; text-transform: none; text-decoration: none; cursor: pointer; color: #1da3d7; <%# Eval("JenisPendidikan").ToString().Trim() != "" ? "": " display: none; " %>">
                                                                    <%# 
                                                                        Eval("JenisPendidikan").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("JenisPendidikan").ToString()) +
                                                                              "&nbsp;&nbsp;&nbsp;&nbsp;"
                                                                            : ""
                                                                    %>
                                                                </label>
                                                                <%# 
                                                                    AI_ERP.Application_Libs.Libs.GetHTMLListUploadedFiles(
                                                                        this.Page,
                                                                        AI_ERP.Application_Libs.Libs.GetFolderPendidikanNonFormal(txtNIKaryawan.Text, Eval("Kode").ToString()),
                                                                        AI_ERP.Application_Libs.Libs.JENIS_UPLOAD.PENDIDIKAN_NON_FORMAL,
                                                                        txtNIKaryawan.Text,
                                                                        Eval("Kode").ToString(),
                                                                        true
                                                                    ).Trim() != "" && Eval("JenisPendidikan").ToString().Trim() != "" 
                                                                    ? "<span style='float: right; color: darkorange; position: absolute; top: 5px; right: 5px;' title=' Ada Attachment '>&nbsp;<i class='fa fa-paperclip'></i></span>"
                                                                    : ""
                                                                %>
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("JenisPendidikan").ToString().Trim() != "" 
                                                                            ? "<br />" + AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Lembaga").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("JenisPendidikan").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("DariTahun").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("JenisPendidikan").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("SampaiTahun").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("JenisPendidikan").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("NilaiAkhir").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("JenisPendidikan").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Divisi").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("JenisPendidikan").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Unit").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("JenisPendidikan").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Keterangan").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="text-align: center; background-color: #ebebeb; width: 80px; vertical-align: middle; color: grey; font-size: 13px;">
                                                                            <i class="fa fa-cogs" style="color: #a7a7a7;"></i>
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Jenis Pendidikan, Lembaga
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: center; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Dari
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: center; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Sampai
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Nilai Akhir
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Divisi
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Unit
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Keterangan
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="8" style="text-align: center; padding: 10px; font-size: 13px; color: #bfbfbf;">..:: Data Kosong ::..
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="background-color: #e3e3e3; margin-left: -25px; margin-right: -25px; margin-top: 0px;">
                                        <div class="col-md-12" style="padding-bottom: 8px; padding-top: 15px;">
                                            <sup style="position: absolute; left: -15px; top: -10px;">
                                                <label style="float: left; color: white; background-color: #1da3d7; padding: 10px; margin: 0px; font-weight: bold; color: white;">
                                                    Pengalaman Kerja Dalam Perusahaan
                                                </label>
                                                <label style="float: left; height: 0; width: 0; border-top: 10px solid transparent; border-left: 20px solid #1da3d7; border-bottom: 10px solid transparent;">
                                                </label>
                                            </sup>
                                            <asp:LinkButton OnClientClick="ShowProgress(true);" ToolTip=" Tambah Data " runat="server" ID="lnkTambahDataPengalamanKerjaDalam" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" OnClick="lnkTambahDataPengalamanKerjaDalam_Click" Style="font-weight: bold; color: grey; font-size: small; border-style: none; padding: 5px; margin-bottom: 0px;">
                                                &nbsp;
                                                <span style="color: #1da3d7; text-transform: none;">
                                                    <i class="fa fa-plus"></i>
                                                    &nbsp;
								                    Tambah Data
                                                </span>
                                                &nbsp;
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                    <div class="row" style="margin-top: 10px; margin-left: -41px; margin-right: -41.5px; margin-top: -40px;">
                                        <div class="col-md-12">

                                            <div style="padding: 0px; margin: 0px;">
                                                <asp:ListView ID="lvDataPengalamanDalam" runat="server">
                                                    <LayoutTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="text-align: center; background-color: #ebebeb; width: 80px; vertical-align: middle; color: grey; font-size: 13px;">
                                                                            <i class="fa fa-cogs" style="color: #a7a7a7;"></i>
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 5px; vertical-align: middle; color: grey; font-size: 13px;">Divisi, Unit
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: center; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Dari
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: center; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Sampai
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Jabatan
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
                                                            <td style="padding: 0px; text-align: center; width: 80px; vertical-align: middle;">
                                                                <div style="margin: 0 auto; <%# 
                                                                        Eval("Divisi").ToString().Trim() != "" 
                                                                            ? " display: table; ": " display: none; "
                                                                    %>">
                                                                    <ul class="nav nav-list margin-no pull-left">
                                                                        <li class="dropdown">
                                                                            <a class="dropdown-toggle text-black waves-attach waves-effect" data-toggle="dropdown" style="color: grey; line-height: 15px; padding: 10px; margin: 0px; min-height: 15px; z-index: 0;" title=" Menu ">
                                                                                <i class="fa fa-cog"></i>
                                                                                <i class="fa fa-caret-down" style="font-size: xx-small;"></i>
                                                                            </a>
                                                                            <ul class="dropdown-menu-list-table">
                                                                                <li style="background-color: white; padding: 10px;">
                                                                                    <label
                                                                                        onclick="<%= txtIDPengalamanKerjaDalam.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetailPengalamanKerjaDalam.ClientID %>.click(); "
                                                                                        id="btnDetailPengalamanKerjaDalam" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                        <i class="fa fa-info-circle" title=" Lihat Detail "></i>&nbsp;&nbsp;&nbsp;Lihat Detail</label>
                                                                                </li>
                                                                                <li style="padding: 0px;">
                                                                                    <hr style="margin: 0px; padding: 0px;" />
                                                                                </li>
                                                                                <li style="background-color: white; padding: 10px;">
                                                                                    <label
                                                                                        onclick="<%= txtIDPengalamanKerjaDalam.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmDeletePengalamanKerjaDalam.ClientID %>.click(); "
                                                                                        id="btnHapusPengalamanKerjaDalam" style="color: #9E0000; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                        <i class="fa fa-times" title=" Hapus Data "></i>&nbsp;&nbsp;&nbsp;Hapus Data</label>
                                                                                </li>
                                                                            </ul>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 5px; vertical-align: middle; text-align: left; font-size: 13px; position: relative">
                                                                <label onclick="<%= txtIDPengalamanKerjaDalam.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetailPengalamanKerjaDalam.ClientID %>.click(); "
                                                                    style="color: grey; font-weight: normal; text-transform: none; text-decoration: none; cursor: pointer; color: #1da3d7; <%# Eval("Divisi").ToString().Trim() != "" ? "": " display: none; " %>">
                                                                    <%# 
                                                                        Eval("Divisi").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Divisi").ToString())
                                                                            : ""
                                                                    %>
                                                                </label>
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("Unit").ToString().Trim() != "" 
                                                                            ? "<br />" + AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Unit").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("Divisi").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Dari").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("Divisi").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Sampai").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("Divisi").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Jabatan").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="text-align: center; background-color: #ebebeb; width: 80px; vertical-align: middle; color: grey; font-size: 13px;">
                                                                            <i class="fa fa-cogs" style="color: #a7a7a7;"></i>
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 5px; vertical-align: middle; color: grey; font-size: 13px;">Divisi, Unit
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: center; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Dari
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: center; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Sampai
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Jabatan
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="5" style="text-align: center; padding: 10px; font-size: 13px; color: #bfbfbf;">..:: Data Kosong ::..
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="background-color: #e3e3e3; margin-left: -25px; margin-right: -25px; margin-top: 0px;">
                                        <div class="col-md-12" style="padding-bottom: 8px; padding-top: 15px;">
                                            <sup style="position: absolute; left: -15px; top: -10px;">
                                                <label style="float: left; color: white; background-color: #1da3d7; padding: 10px; margin: 0px; font-weight: bold; color: white;">
                                                    Pengalaman Kerja Luar Perusahaan
                                                </label>
                                                <label style="float: left; height: 0; width: 0; border-top: 10px solid transparent; border-left: 20px solid #1da3d7; border-bottom: 10px solid transparent;">
                                                </label>
                                            </sup>
                                            <asp:LinkButton OnClientClick="ShowProgress(true);" ToolTip=" Tambah Data " runat="server" ID="lnkTambahDataPengalamanKerjaLuar" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" OnClick="lnkTambahDataPengalamanKerjaLuar_Click" Style="font-weight: bold; color: grey; font-size: small; border-style: none; padding: 5px; margin-bottom: 0px;">
                                                &nbsp;
                                                <span style="color: #1da3d7; text-transform: none;">
                                                    <i class="fa fa-plus"></i>
                                                    &nbsp;
								                    Tambah Data
                                                </span>
                                                &nbsp;
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                    <div class="row" style="margin-top: 10px; margin-left: -41px; margin-right: -41.5px; margin-top: -40px;">
                                        <div class="col-md-12">

                                            <div style="padding: 0px; margin: 0px;">
                                                <asp:ListView ID="lvDataPengalamanLuar" runat="server">
                                                    <LayoutTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="text-align: center; background-color: #ebebeb; width: 80px; vertical-align: middle; color: grey; font-size: 13px;">
                                                                            <i class="fa fa-cogs" style="color: #a7a7a7;"></i>
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 5px; vertical-align: middle; color: grey; font-size: 13px;">Nama Perusahaan
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: center; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Dari
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: center; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Sampai
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Jabatan
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
                                                            <td style="padding: 0px; text-align: center; width: 80px; vertical-align: middle;">
                                                                <div style="margin: 0 auto; <%# 
                                                                        Eval("NamaPerusahaan").ToString().Trim() != "" 
                                                                            ? " display: table; ": " display: none; "
                                                                    %>">
                                                                    <ul class="nav nav-list margin-no pull-left">
                                                                        <li class="dropdown">
                                                                            <a class="dropdown-toggle text-black waves-attach waves-effect" data-toggle="dropdown" style="color: grey; line-height: 15px; padding: 10px; margin: 0px; min-height: 15px; z-index: 0;" title=" Menu ">
                                                                                <i class="fa fa-cog"></i>
                                                                                <i class="fa fa-caret-down" style="font-size: xx-small;"></i>
                                                                            </a>
                                                                            <ul class="dropdown-menu-list-table">
                                                                                <li style="background-color: white; padding: 10px;">
                                                                                    <label
                                                                                        onclick="<%= txtIDPengalamanKerjaLuar.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetailPengalamanKerjaLuar.ClientID %>.click(); "
                                                                                        id="btnDetailPengalamanKerjaLuar" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                        <i class="fa fa-info-circle" title=" Lihat Detail "></i>&nbsp;&nbsp;&nbsp;Lihat Detail</label>
                                                                                </li>
                                                                                <li style="padding: 0px;">
                                                                                    <hr style="margin: 0px; padding: 0px;" />
                                                                                </li>
                                                                                <li style="background-color: white; padding: 10px;">
                                                                                    <label
                                                                                        onclick="<%= txtIDPengalamanKerjaLuar.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmDeletePengalamanKerjaLuar.ClientID %>.click(); "
                                                                                        id="btnHapusPengalamanKerjaLuar" style="color: #9E0000; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                        <i class="fa fa-times" title=" Hapus Data "></i>&nbsp;&nbsp;&nbsp;Hapus Data</label>
                                                                                </li>
                                                                            </ul>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 5px; vertical-align: middle; text-align: left; font-size: 13px; position: relative">
                                                                <label onclick="<%= txtIDPengalamanKerjaLuar.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetailPengalamanKerjaLuar.ClientID %>.click(); "
                                                                    style="color: grey; font-weight: normal; text-transform: none; text-decoration: none; cursor: pointer; color: #1da3d7; <%# Eval("NamaPerusahaan").ToString().Trim() != "" ? "": " display: none; " %>">
                                                                    <%# 
                                                                        Eval("NamaPerusahaan").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("NamaPerusahaan").ToString())
                                                                            : ""
                                                                    %>
                                                                </label>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("NamaPerusahaan").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Dari").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("NamaPerusahaan").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Sampai").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("NamaPerusahaan").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Jabatan").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="text-align: center; background-color: #ebebeb; width: 80px; vertical-align: middle; color: grey; font-size: 13px;">
                                                                            <i class="fa fa-cogs" style="color: #a7a7a7;"></i>
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 5px; vertical-align: middle; color: grey; font-size: 13px;">Nama Perusahaan
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: center; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Dari
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: center; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Sampai
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Jabatan
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="5" style="text-align: center; padding: 10px; font-size: 13px; color: #bfbfbf;">..:: Data Kosong ::..
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="background-color: #e3e3e3; margin-left: -25px; margin-right: -25px; margin-top: 0px;">
                                        <div class="col-md-12" style="padding-bottom: 8px; padding-top: 15px;">
                                            <sup style="position: absolute; left: -15px; top: -10px;">
                                                <label style="float: left; color: white; background-color: #1da3d7; padding: 10px; margin: 0px; font-weight: bold; color: white;">
                                                    Data Pengalaman Sharing
                                                </label>
                                                <label style="float: left; height: 0; width: 0; border-top: 10px solid transparent; border-left: 20px solid #1da3d7; border-bottom: 10px solid transparent;">
                                                </label>
                                            </sup>
                                            <asp:LinkButton OnClientClick="ShowProgress(true);" ToolTip=" Tambah Data " runat="server" ID="lnkTambahDataPengalamanSharing" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" OnClick="lnkTambahDataPengalamanSharing_Click" Style="font-weight: bold; color: grey; font-size: small; border-style: none; padding: 5px; margin-bottom: 0px;">
                                                &nbsp;
                                                <span style="color: #1da3d7; text-transform: none;">
                                                    <i class="fa fa-plus"></i>
                                                    &nbsp;
								                    Tambah Data
                                                </span>
                                                &nbsp;
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                    <div class="row" style="margin-top: 10px; margin-left: -41px; margin-right: -41.5px; margin-top: -40px;">
                                        <div class="col-md-12">

                                            <div style="padding: 0px; margin: 0px;">
                                                <asp:ListView ID="lvPegawaiPengalamanSharing" runat="server">
                                                    <LayoutTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="text-align: center; background-color: #ebebeb; width: 80px; vertical-align: middle; color: grey; font-size: 13px;">
                                                                            <i class="fa fa-cogs" style="color: #a7a7a7;"></i>
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 5px; vertical-align: middle; color: grey; font-size: 13px;">Tahun
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Topik
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Penyelenggara
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Kota
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
                                                            <td style="padding: 0px; text-align: center; width: 80px; vertical-align: middle;">
                                                                <div style="margin: 0 auto; <%# 
                                                                        Eval("Tahun").ToString().Trim() != "" 
                                                                            ? " display: table; ": " display: none; "
                                                                    %>">
                                                                    <ul class="nav nav-list margin-no pull-left">
                                                                        <li class="dropdown">
                                                                            <a class="dropdown-toggle text-black waves-attach waves-effect" data-toggle="dropdown" style="color: grey; line-height: 15px; padding: 10px; margin: 0px; min-height: 15px; z-index: 0;" title=" Menu ">
                                                                                <i class="fa fa-cog"></i>
                                                                                <i class="fa fa-caret-down" style="font-size: xx-small;"></i>
                                                                            </a>
                                                                            <ul class="dropdown-menu-list-table">
                                                                                <li style="background-color: white; padding: 10px;">
                                                                                    <label
                                                                                        onclick="<%= txtIDPengalamanSharing.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetailPengalamanSharing.ClientID %>.click(); "
                                                                                        id="btnDetailPengalamanSharing" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                        <i class="fa fa-info-circle" title=" Lihat Detail "></i>&nbsp;&nbsp;&nbsp;Lihat Detail</label>
                                                                                </li>
                                                                                <li style="padding: 0px;">
                                                                                    <hr style="margin: 0px; padding: 0px;" />
                                                                                </li>
                                                                                <li style="background-color: white; padding: 10px;">
                                                                                    <label
                                                                                        onclick="<%= txtIDPengalamanSharing.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmDeletePengalamanSharing.ClientID %>.click(); "
                                                                                        id="btnHapusPengalamanSharing" style="color: #9E0000; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                        <i class="fa fa-times" title=" Hapus Data "></i>&nbsp;&nbsp;&nbsp;Hapus Data</label>
                                                                                </li>
                                                                            </ul>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 5px; vertical-align: middle; text-align: left; font-size: 13px; position: relative">
                                                                <label onclick="<%= txtIDPengalamanSharing.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetailPengalamanSharing.ClientID %>.click(); "
                                                                    style="color: grey; font-weight: normal; text-transform: none; text-decoration: none; cursor: pointer; color: #1da3d7; <%# Eval("Tahun").ToString().Trim() != "" ? "": " display: none; " %>">
                                                                    <%# 
                                                                        Eval("Tahun").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Tahun").ToString())
                                                                            : ""
                                                                    %>
                                                                </label>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("Topik").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Topik").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("Penyelenggara").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Penyelenggara").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("Kota").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kota").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="text-align: center; background-color: #ebebeb; width: 80px; vertical-align: middle; color: grey; font-size: 13px;">
                                                                            <i class="fa fa-cogs" style="color: #a7a7a7;"></i>
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 5px; vertical-align: middle; color: grey; font-size: 13px;">Tahun
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Topik
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Penyelenggara
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Kota
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="5" style="text-align: center; padding: 10px; font-size: 13px; color: #bfbfbf;">..:: Data Kosong ::..
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="background-color: #e3e3e3; margin-left: -25px; margin-right: -25px; margin-top: 0px;">
                                        <div class="col-md-12" style="padding-bottom: 8px; padding-top: 15px;">
                                            <sup style="position: absolute; left: -15px; top: -10px;">
                                                <label style="float: left; color: white; background-color: #1da3d7; padding: 10px; margin: 0px; font-weight: bold; color: white;">
                                                    Data Pengalaman Kepanitiaan
                                                </label>
                                                <label style="float: left; height: 0; width: 0; border-top: 10px solid transparent; border-left: 20px solid #1da3d7; border-bottom: 10px solid transparent;">
                                                </label>
                                            </sup>
                                            <asp:LinkButton OnClientClick="ShowProgress(true);" ToolTip=" Tambah Data " runat="server" ID="lnkTambahDataPengalamanKepanitiaan" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" OnClick="lnkTambahDataPengalamanKepanitiaan_Click" Style="font-weight: bold; color: grey; font-size: small; border-style: none; padding: 5px; margin-bottom: 0px;">
                                                &nbsp;
                                                <span style="color: #1da3d7; text-transform: none;">
                                                    <i class="fa fa-plus"></i>
                                                    &nbsp;
								                    Tambah Data
                                                </span>
                                                &nbsp;
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                    <div class="row" style="margin-top: 10px; margin-left: -41px; margin-right: -41.5px; margin-top: -40px;">
                                        <div class="col-md-12">

                                            <div style="padding: 0px; margin: 0px;">
                                                <asp:ListView ID="lvPegawaiPengalamanKepanitian" runat="server">
                                                    <LayoutTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="text-align: center; background-color: #ebebeb; width: 80px; vertical-align: middle; color: grey; font-size: 13px;">
                                                                            <i class="fa fa-cogs" style="color: #a7a7a7;"></i>
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 5px; vertical-align: middle; color: grey; font-size: 13px;">Tahun
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Kegiatan Kepanitiaan
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Jabatan
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">No. Surat Tugas
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Keterangan
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
                                                            <td style="padding: 0px; text-align: center; width: 80px; vertical-align: middle;">
                                                                <div style="margin: 0 auto; <%# 
                                                                        Eval("Tahun").ToString().Trim() != "" 
                                                                            ? " display: table; ": " display: none; "
                                                                    %>">
                                                                    <ul class="nav nav-list margin-no pull-left">
                                                                        <li class="dropdown">
                                                                            <a class="dropdown-toggle text-black waves-attach waves-effect" data-toggle="dropdown" style="color: grey; line-height: 15px; padding: 10px; margin: 0px; min-height: 15px; z-index: 0;" title=" Menu ">
                                                                                <i class="fa fa-cog"></i>
                                                                                <i class="fa fa-caret-down" style="font-size: xx-small;"></i>
                                                                            </a>
                                                                            <ul class="dropdown-menu-list-table">
                                                                                <li style="background-color: white; padding: 10px;">
                                                                                    <label
                                                                                        onclick="<%= txtIDPengalamanKepanitiaan.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetailPengalamanKepanitiaan.ClientID %>.click(); "
                                                                                        id="btnDetailPengalamanKepanitiaan" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                        <i class="fa fa-info-circle" title=" Lihat Detail "></i>&nbsp;&nbsp;&nbsp;Lihat Detail</label>
                                                                                </li>
                                                                                <li style="padding: 0px;">
                                                                                    <hr style="margin: 0px; padding: 0px;" />
                                                                                </li>
                                                                                <li style="background-color: white; padding: 10px;">
                                                                                    <label
                                                                                        onclick="<%= txtIDPengalamanKepanitiaan.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmDeletePengalamanKepanitiaan.ClientID %>.click(); "
                                                                                        id="btnHapusPengalamanKepanitiaan" style="color: #9E0000; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                        <i class="fa fa-times" title=" Hapus Data "></i>&nbsp;&nbsp;&nbsp;Hapus Data</label>
                                                                                </li>
                                                                            </ul>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 5px; vertical-align: middle; text-align: left; font-size: 13px; position: relative">
                                                                <label onclick="<%= txtIDPengalamanKepanitiaan.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetailPengalamanKepanitiaan.ClientID %>.click(); "
                                                                    style="color: grey; font-weight: normal; text-transform: none; text-decoration: none; cursor: pointer; color: #1da3d7; <%# Eval("Tahun").ToString().Trim() != "" ? "": " display: none; " %>">
                                                                    <%# 
                                                                        Eval("Tahun").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Tahun").ToString())
                                                                            : ""
                                                                    %>
                                                                </label>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("Kegiatan").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kegiatan").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("Jabatan").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Jabatan").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("NoSuratTugas").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("NoSuratTugas").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Eval("Keterangan").ToString().Trim() != "" 
                                                                            ? AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Keterangan").ToString())
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="text-align: center; background-color: #ebebeb; width: 80px; vertical-align: middle; color: grey; font-size: 13px;">
                                                                            <i class="fa fa-cogs" style="color: #a7a7a7;"></i>
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 5px; vertical-align: middle; color: grey; font-size: 13px;">Tahun
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Kegiatan Kepanitiaan
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Jabatan
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">No. Surat Tugas
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Keterangan
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="6" style="text-align: center; padding: 10px; font-size: 13px; color: #bfbfbf;">..:: Data Kosong ::..
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="background-color: #e3e3e3; margin-left: -25px; margin-right: -25px; margin-top: 0px;">
                                        <div class="col-md-12" style="padding-bottom: 8px; padding-top: 15px;">
                                            <sup style="position: absolute; left: -15px; top: -10px;">
                                                <label style="float: left; color: white; background-color: #1da3d7; padding: 10px; margin: 0px; font-weight: bold; color: white;">
                                                    Data Riwayat Kesehatan
                                                </label>
                                                <label style="float: left; height: 0; width: 0; border-top: 10px solid transparent; border-left: 20px solid #1da3d7; border-bottom: 10px solid transparent;">
                                                </label>
                                            </sup>
                                            <asp:LinkButton OnClientClick="ShowProgress(true);" ToolTip=" Tambah Data " runat="server" ID="lnkTambahDataRiwayatKesehatan" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" OnClick="lnkTambahDataRiwayatKesehatan_Click" Style="font-weight: bold; color: grey; font-size: small; border-style: none; padding: 5px; margin-bottom: 0px;">
                                                &nbsp;
                                                <span style="color: #1da3d7; text-transform: none;">
                                                    <i class="fa fa-plus"></i>
                                                    &nbsp;
								                    Tambah Data
                                                </span>
                                                &nbsp;
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                    <div class="row" style="margin-top: 10px; margin-left: -41px; margin-right: -41.5px; margin-top: -40px;">
                                        <div class="col-md-12">

                                            <div style="padding: 0px; margin: 0px;">
                                                <asp:ListView ID="lvPegawaiRiwayatKesehatan" runat="server">
                                                    <LayoutTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="text-align: center; background-color: #ebebeb; width: 80px; vertical-align: middle; color: grey; font-size: 13px;">
                                                                            <i class="fa fa-cogs" style="color: #a7a7a7;"></i>
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 5px; vertical-align: middle; color: grey; font-size: 13px;">Dari Tanggal - Sampai
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: center; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Izin Tidak Masuk
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Nama Penyakit
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">RS/Klinik
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Dokter
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Keterangan
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
                                                            <td style="padding: 0px; text-align: center; width: 80px; vertical-align: middle;">
                                                                <div style="margin: 0 auto; <%# 
                                                                        Convert.ToDateTime(Eval("DariTanggal")) != DateTime.MaxValue
                                                                            ? " display: table; ": " display: none; "
                                                                    %>">
                                                                    <ul class="nav nav-list margin-no pull-left">
                                                                        <li class="dropdown">
                                                                            <a class="dropdown-toggle text-black waves-attach waves-effect" data-toggle="dropdown" style="color: grey; line-height: 15px; padding: 10px; margin: 0px; min-height: 15px; z-index: 0;" title=" Menu ">
                                                                                <i class="fa fa-cog"></i>
                                                                                <i class="fa fa-caret-down" style="font-size: xx-small;"></i>
                                                                            </a>
                                                                            <ul class="dropdown-menu-list-table">
                                                                                <li style="background-color: white; padding: 10px;">
                                                                                    <label
                                                                                        onclick="<%= txtIDRiwayatKesehatan.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetailRiwayatKesehatan.ClientID %>.click(); "
                                                                                        id="btnDetailRiwayatKesehatan" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                        <i class="fa fa-info-circle" title=" Lihat Detail "></i>&nbsp;&nbsp;&nbsp;Lihat Detail</label>
                                                                                </li>
                                                                                <li style="padding: 0px;">
                                                                                    <hr style="margin: 0px; padding: 0px;" />
                                                                                </li>
                                                                                <li style="background-color: white; padding: 10px;">
                                                                                    <label
                                                                                        onclick="<%= txtIDRiwayatKesehatan.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmDeleteRiwayatKesehatan.ClientID %>.click(); "
                                                                                        id="btnHapusRiwayatKesehatan" style="color: #9E0000; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                        <i class="fa fa-times" title=" Hapus Data "></i>&nbsp;&nbsp;&nbsp;Hapus Data</label>
                                                                                </li>
                                                                            </ul>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 5px; vertical-align: middle; text-align: left; font-size: 13px; position: relative">
                                                                <label onclick="<%= txtIDRiwayatKesehatan.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetailRiwayatKesehatan.ClientID %>.click(); "
                                                                    style="color: grey; font-weight: normal; text-transform: none; text-decoration: none; cursor: pointer; color: #1da3d7; <%# Convert.ToDateTime(Eval("DariTanggal")) != DateTime.MaxValue ? "": " display: none; " %>">
                                                                    <%# 
                                                                        Convert.ToDateTime(Eval("DariTanggal")) != DateTime.MaxValue
                                                                            ? AI_ERP.Application_Libs.Libs.GetTanggalIndonesiaFromDate(Convert.ToDateTime(Eval("DariTanggal")), false) + " s.d <br />" +
                                                                              AI_ERP.Application_Libs.Libs.GetTanggalIndonesiaFromDate(Convert.ToDateTime(Eval("SampaiTanggal")), false) +
                                                                              "&nbsp;&nbsp;&nbsp;&nbsp;"
                                                                            : ""
                                                                    %>
                                                                </label>
                                                                <%# 
                                                                    AI_ERP.Application_Libs.Libs.GetHTMLListUploadedFiles(
                                                                        this.Page,
                                                                        AI_ERP.Application_Libs.Libs.GetFolderRiwayatKesehatan(txtNIKaryawan.Text, Eval("Kode").ToString()),
                                                                        AI_ERP.Application_Libs.Libs.JENIS_UPLOAD.RIWAYAT_KESEHATAN,
                                                                        txtNIKaryawan.Text,
                                                                        Eval("Kode").ToString(),
                                                                        true
                                                                    ).Trim() != ""
                                                                    ? "<label style='float: right; color: darkorange; position: absolute; top: 5px; right: 5px;' title=' Ada Attachment '>&nbsp;<i class='fa fa-paperclip'></i></label>"
                                                                    : ""
                                                                %>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Convert.ToBoolean(Eval("IsIzin"))
                                                                            ? "<i class=\"fa fa-check\"></i>"
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Convert.ToDateTime(Eval("DariTanggal")) != DateTime.MaxValue
                                                                            ? Eval("NamaPenyakit").ToString()
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Convert.ToDateTime(Eval("DariTanggal")) != DateTime.MaxValue
                                                                            ? Eval("RSKlinik").ToString()
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Convert.ToDateTime(Eval("DariTanggal")) != DateTime.MaxValue
                                                                            ? Eval("Dokter").ToString()
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Convert.ToDateTime(Eval("DariTanggal")) != DateTime.MaxValue
                                                                            ? Eval("Keterangan").ToString()
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="text-align: center; background-color: #ebebeb; width: 80px; vertical-align: middle; color: grey; font-size: 13px;">
                                                                            <i class="fa fa-cogs" style="color: #a7a7a7;"></i>
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 5px; vertical-align: middle; color: grey; font-size: 13px;">Dari Tanggal - Sampai
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: center; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Izin Tidak Masuk
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Nama Penyakit
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">RS/Klinik
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Dokter
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Keterangan
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="7" style="text-align: center; padding: 10px; font-size: 13px; color: #bfbfbf;">..:: Data Kosong ::..
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="background-color: #e3e3e3; margin-left: -25px; margin-right: -25px; margin-top: 0px;">
                                        <div class="col-md-12" style="padding-bottom: 8px; padding-top: 15px;">
                                            <sup style="position: absolute; left: -15px; top: -10px;">
                                                <label style="float: left; color: white; background-color: #1da3d7; padding: 10px; margin: 0px; font-weight: bold; color: white;">
                                                    Data Riwayat Medical Check Up
                                                </label>
                                                <label style="float: left; height: 0; width: 0; border-top: 10px solid transparent; border-left: 20px solid #1da3d7; border-bottom: 10px solid transparent;">
                                                </label>
                                            </sup>
                                            <asp:LinkButton OnClientClick="ShowProgress(true);" ToolTip=" Tambah Data " runat="server" ID="lnkTambahDataRiwayatMCU" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" OnClick="lnkTambahDataRiwayatMCU_Click" Style="font-weight: bold; color: grey; font-size: small; border-style: none; padding: 5px; margin-bottom: 0px;">
                                                &nbsp;
                                                <span style="color: #1da3d7; text-transform: none;">
                                                    <i class="fa fa-plus"></i>
                                                    &nbsp;
								                    Tambah Data
                                                </span>
                                                &nbsp;
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                    <div class="row" style="margin-top: 10px; margin-left: -41px; margin-right: -41.5px; margin-top: -40px;">
                                        <div class="col-md-12">

                                            <div style="padding: 0px; margin: 0px;">
                                                <asp:ListView ID="lvPegawaiRiwayatKesehatanMCU" runat="server">
                                                    <LayoutTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="text-align: center; background-color: #ebebeb; width: 80px; vertical-align: middle; color: grey; font-size: 13px;">
                                                                            <i class="fa fa-cogs" style="color: #a7a7a7;"></i>
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 5px; vertical-align: middle; color: grey; font-size: 13px;">Tanggal
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Kesimpulan
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Saran
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Keterangan
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
                                                            <td style="padding: 0px; text-align: center; width: 80px; vertical-align: middle;">
                                                                <div style="margin: 0 auto; <%# 
                                                                        Convert.ToDateTime(Eval("Tanggal")) != DateTime.MaxValue
                                                                            ? " display: table; ": " display: none; "
                                                                    %>">
                                                                    <ul class="nav nav-list margin-no pull-left">
                                                                        <li class="dropdown">
                                                                            <a class="dropdown-toggle text-black waves-attach waves-effect" data-toggle="dropdown" style="color: grey; line-height: 15px; padding: 10px; margin: 0px; min-height: 15px; z-index: 0;" title=" Menu ">
                                                                                <i class="fa fa-cog"></i>
                                                                                <i class="fa fa-caret-down" style="font-size: xx-small;"></i>
                                                                            </a>
                                                                            <ul class="dropdown-menu-list-table">
                                                                                <li style="background-color: white; padding: 10px;">
                                                                                    <label
                                                                                        onclick="<%= txtIDRiwayatMCU.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetailRiwayatMCU.ClientID %>.click(); "
                                                                                        id="btnDetailRiwayatMCU" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                        <i class="fa fa-info-circle" title=" Lihat Detail "></i>&nbsp;&nbsp;&nbsp;Lihat Detail</label>
                                                                                </li>
                                                                                <li style="padding: 0px;">
                                                                                    <hr style="margin: 0px; padding: 0px;" />
                                                                                </li>
                                                                                <li style="background-color: white; padding: 10px;">
                                                                                    <label
                                                                                        onclick="<%= txtIDRiwayatMCU.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmDeleteRiwayatMCU.ClientID %>.click(); "
                                                                                        id="btnHapusRiwayatMCU" style="color: #9E0000; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                        <i class="fa fa-times" title=" Hapus Data "></i>&nbsp;&nbsp;&nbsp;Hapus Data</label>
                                                                                </li>
                                                                            </ul>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px; position: relative;">
                                                                <label onclick="<%= txtIDRiwayatMCU.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetailRiwayatMCU.ClientID %>.click(); "
                                                                    style="color: grey; font-weight: normal; text-transform: none; text-decoration: none; cursor: pointer; color: #1da3d7; <%# Convert.ToDateTime(Eval("Tanggal")) != DateTime.MaxValue ? "": " display: none; " %>">
                                                                    <%# 
                                                                            Convert.ToDateTime(Eval("Tanggal")) != DateTime.MaxValue
                                                                                ? AI_ERP.Application_Libs.Libs.GetTanggalIndonesiaFromDate(Convert.ToDateTime(Eval("Tanggal")), false) +
                                                                                  "&nbsp;&nbsp;&nbsp;&nbsp;"
                                                                                : ""
                                                                    %>
                                                                </label>
                                                                <%# 
                                                                    AI_ERP.Application_Libs.Libs.GetHTMLListUploadedFiles(
                                                                        this.Page,
                                                                        AI_ERP.Application_Libs.Libs.GetFolderRiwayatMCU(txtNIKaryawan.Text, Eval("Kode").ToString()),
                                                                        AI_ERP.Application_Libs.Libs.JENIS_UPLOAD.RIWAYAT_MCU,
                                                                        txtNIKaryawan.Text,
                                                                        Eval("Kode").ToString(),
                                                                        true
                                                                    ).Trim() != ""
                                                                    ? "<label style='float: right; color: darkorange; position: absolute; top: 5px; right: 5px;' title=' Ada Attachment '>&nbsp;<i class='fa fa-paperclip'></i></label>"
                                                                    : ""
                                                                %>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Convert.ToDateTime(Eval("Tanggal")) != DateTime.MaxValue
                                                                            ? Eval("Kesimpulan").ToString()
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Convert.ToDateTime(Eval("Tanggal")) != DateTime.MaxValue
                                                                            ? Eval("Saran").ToString()
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; font-size: 13px;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Convert.ToDateTime(Eval("Tanggal")) != DateTime.MaxValue
                                                                            ? Eval("Keterangan").ToString()
                                                                            : ""
                                                                    %>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="text-align: center; background-color: #ebebeb; width: 80px; vertical-align: middle; color: grey; font-size: 13px;">
                                                                            <i class="fa fa-cogs" style="color: #a7a7a7;"></i>
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 5px; vertical-align: middle; color: grey; font-size: 13px;">Tanggal
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Kesimpulan
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Saran
                                                                        </th>
                                                                        <th style="background-color: #ebebeb; text-align: left; padding-left: 10px; vertical-align: middle; color: grey; font-size: 13px;">Keterangan
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="5" style="text-align: center; padding: 10px; font-size: 13px; color: #bfbfbf;">..:: Data Kosong ::..
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="background-color: #ebebeb; margin-left: -25px; margin-right: -25px; margin-top: 0px;">
                                        <div class="col-md-12" style="padding-bottom: 0px; padding-top: 15px; padding-left: 0px; padding-right: 0px;">
                                            <sup style="position: absolute; left: -15px; top: -10px;">
                                                <label style="float: left; color: white; background-color: #1da3d7; padding: 10px; margin: 0px; font-weight: bold; color: white;">
                                                    File Saya
                                                </label>
                                                <label style="float: left; height: 0; width: 0; border-top: 10px solid transparent; border-left: 20px solid #1da3d7; border-bottom: 10px solid transparent;">
                                                </label>
                                            </sup>
                                            <label style="padding: 10px; margin-left: 15px; margin-right: 15px; color: grey;">
                                                <i class="fa fa-info-circle"></i>&nbsp;
                                                Upload file sebagai pelengkap profil (Sertifikat keahlian, penghargaan, dll)
                                            </label>
                                            <br />
                                            <asp:LinkButton OnClientClick="ShowProgress(true);" ToolTip=" Tambah Data " runat="server" ID="lnkTambahUploadFilePendukung" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" OnClick="lnkTambahUploadFilePendukung_Click" Style="font-weight: bold; color: grey; font-size: small; border-style: none; padding: 5px; margin-bottom: 10px; margin-left: 15px; margin-right: 15px;">
                                                &nbsp;
                                                <span style="color: #1da3d7; text-transform: none;">
                                                    <i class="fa fa-plus"></i>
                                                    &nbsp;
								                    Tambah Data
                                                </span>
                                                &nbsp;
                                            </asp:LinkButton>
                                            <div style="padding: 10px; border-width: 1px; border-style: solid; border-color: #bfbfbf; background-color: white; width: 100%; border-left-style: none; border-right-style: none; padding-left: 20px; padding-right: 20px;">
                                                <asp:Literal runat="server" ID="ltrListFileUploadPendukung"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_input_data">
                                        <div runat="server" id="div_list_data_pegawai" class="content-header ui-content-header"
                                            style="background-color: #DA0379; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 33px; right: 25px; width: 300px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                                            <div style="padding-left: 15px;">
                                                <asp:LinkButton ToolTip=" List Daftar Pegawai " runat="server" ID="btnBackToMenu" CssClass="btn-trans" OnClick="btnBackToMenu_Click" Style="font-weight: bold; color: white;">
                                                    <i class="fa fa-list-alt"></i>
                                                    &nbsp;
                                                    Data Pegawai
                                                </asp:LinkButton>
                                            </div>
                                        </div>

                                        <div class="content-header ui-content-header"
                                            style="background-color: #00198d; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 33px; right: 25px; width: 150px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                                            <div style="padding-left: 15px;">
                                                <asp:LinkButton OnClientClick="ShowProgress(true);" ToolTip=" Simpan Data " runat="server" ID="btnSaveData" CssClass="btn-trans" OnClick="btnSaveData_Click" Style="font-weight: bold; color: white;">
                                                    <i class="fa fa-check"></i>
                                                    &nbsp;
								                    Simpan Data
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="display: none;">
                                        <div class="col-xs-12">
                                            &nbsp;
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </section>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_pendidikan_formal" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                    <span style="font-weight: bold;">Data Pendidikan Formal
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
                                                        <label class="label-input" for="<%= cboJenisPendidikan.ClientID %>" style="text-transform: none;">Jenis Pendidikan</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldJenisPendidikan"
                                                            ControlToValidate="cboJenisPendidikan" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList runat="server" ID="cboJenisPendidikan" CssClass="form-control">
                                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                                            <asp:ListItem Value="TK" Text="TK"></asp:ListItem>
                                                            <asp:ListItem Value="SD" Text="SD/MI"></asp:ListItem>
                                                            <asp:ListItem Value="SMP" Text="SMP/MTs"></asp:ListItem>
                                                            <asp:ListItem Value="SMA" Text="SMA/MA/SMK"></asp:ListItem>
                                                            <asp:ListItem Value="D1" Text="D1"></asp:ListItem>
                                                            <asp:ListItem Value="D2" Text="D2"></asp:ListItem>
                                                            <asp:ListItem Value="D3" Text="D3"></asp:ListItem>
                                                            <asp:ListItem Value="D4" Text="D4"></asp:ListItem>
                                                            <asp:ListItem Value="S1" Text="S1"></asp:ListItem>
                                                            <asp:ListItem Value="S2" Text="S2"></asp:ListItem>
                                                            <asp:ListItem Value="S3" Text="S3"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtUniversitas.NamaClientID %>" style="text-transform: none;">Lembaga Pendidikan</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldLembagaPendidikan"
                                                            ControlToValidate="txtUniversitas$txtNama" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <ucl:AutocompleteUniversitas runat="server" ID="txtUniversitas" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtJurusanPendidikan.NamaClientID %>" style="text-transform: none;">Jurusan Pendidikan</label>
                                                        <ucl:AutocompleteJurusanPendidikan runat="server" ID="txtJurusanPendidikan" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtPendidikanDariTahun.ClientID %>" style="text-transform: none;">Dari Tahun</label>
                                                        <asp:TextBox ValidationGroup="vldInput" CssClass="form-control" runat="server" ID="txtPendidikanDariTahun"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtPendidikanSampaiTahun.ClientID %>" style="text-transform: none;">Sampai Tahun</label>
                                                        <asp:TextBox ValidationGroup="vldInput" CssClass="form-control" runat="server" ID="txtPendidikanSampaiTahun"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtNilaiAkhir.ClientID %>" style="text-transform: none;">Nilai Akhir</label>
                                                        <asp:TextBox ValidationGroup="vldInput" CssClass="form-control" runat="server" ID="txtNilaiAkhir"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtKeterangan.ClientID %>" style="text-transform: none;">Keterangan</label>
                                                        <asp:TextBox ValidationGroup="vldInput" CssClass="form-control" runat="server" ID="txtKeterangan"></asp:TextBox>
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
                                <asp:LinkButton OnClientClick="ShowProgress(Page_ClientValidate('vldInput'));" ValidationGroup="vldInput" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInputPendidikan" OnClick="lnkOKInputPendidikan_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_confirm_hapus_pendidikan" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">Konfirmasi
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
                                                            <asp:Literal runat="server" ID="ltrMsgConfirmHapusPendidikan"></asp:Literal>
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapusPendidikan" OnClick="lnkOKHapusPendidikan_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_pendidikan_nonformal" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

                <label title=" Tutup " onclick="DeleteBatalPendidikanNonFormal();" data-dismiss="modal" style="padding: 10px; padding-top: 6px; padding-bottom: 6px; background-color: white; cursor: pointer; position: fixed; right: 35px; top: 20px; z-index: 999999; border-radius: 100%;">
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
                                    <span style="font-weight: bold;">Data Pendidikan Non Formal
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
                                                        <label class="label-input" for="<%= txtJenisPendidikanNonFormal.ClientID %>" style="text-transform: none;">Jenis Pendidikan</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputPendidikanNonFormal" runat="server" ID="vldJenisPendidikanNonFormal"
                                                            ControlToValidate="txtJenisPendidikanNonFormal" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ValidationGroup="vldInputPendidikanNonFormal" CssClass="form-control" runat="server" ID="txtJenisPendidikanNonFormal"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtLembagaPendidikanNonFormal.ClientID %>" style="text-transform: none;">Lembaga</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputPendidikanNonFormal" runat="server" ID="vldLembagaPendidikanNonFormal"
                                                            ControlToValidate="txtLembagaPendidikanNonFormal" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ValidationGroup="vldInputPendidikanNonFormal" CssClass="form-control" runat="server" ID="txtLembagaPendidikanNonFormal"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtDariPendidikanNonFormal.ClientID %>" style="text-transform: none;">Dari</label>
                                                        <asp:TextBox ValidationGroup="vldInputPendidikanNonFormal" CssClass="form-control" runat="server" ID="txtDariPendidikanNonFormal"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtSampaiPendidikanNonFormal.ClientID %>" style="text-transform: none;">Sampai</label>
                                                        <asp:TextBox ValidationGroup="vldInputPendidikanNonFormal" CssClass="form-control" runat="server" ID="txtSampaiPendidikanNonFormal"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtNilaiAkhirPendidikanNonFormal.ClientID %>" style="text-transform: none;">Nilai Akhir</label>
                                                        <asp:TextBox ValidationGroup="vldInputPendidikanNonFormal" CssClass="form-control" runat="server" ID="txtNilaiAkhirPendidikanNonFormal"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtDivisiPendidikanNonFormal.ClientID %>" style="text-transform: none;">Divisi</label>
                                                        <asp:TextBox ValidationGroup="vldInputPendidikanNonFormal" CssClass="form-control" runat="server" ID="txtDivisiPendidikanNonFormal"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtUnitPendidikanNonFormal.ClientID %>" style="text-transform: none;">Unit</label>
                                                        <asp:TextBox ValidationGroup="vldInputPendidikanNonFormal" CssClass="form-control" runat="server" ID="txtUnitPendidikanNonFormal"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtKeteranganPendidikanNonFormal.ClientID %>" style="text-transform: none;">Keterangan</label>
                                                        <asp:TextBox ValidationGroup="vldInputPendidikanNonFormal" CssClass="form-control" runat="server" ID="txtKeteranganPendidikanNonFormal"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div runat="server" id="div_upload_file_pendidikan_non_formal" class="row" style="margin-left: 30px; margin-right: 30px; margin-top: 15px;">
                                                <div style="padding: 10px; border-style: solid; border-width: 1px; border-color: #b1e1ff; border-radius: 5px; background-color: #ddf2ff;">
                                                    <span style="font-weight: normal; color: #1DA1F2;">
                                                        <span style="font-weight: bold;">Upload file hasil pendidikan non formal
                                                        </span>
                                                        <br />
                                                        Format File : 
                                                        <br />
                                                        PDF, DOC, DOCX, PPT, PPTX, XLS, XLSX
                                                        <br />
                                                        JPG, JPEG, PNG, BMP
                                                    </span>

                                                    <div class="row">
                                                        <div class="col-md-12">

                                                            <div style="margin-top: 10px; width: 100%; overflow: hidden; padding: 0px;">
                                                                <div runat="server" id="div_upload">
                                                                    <iframe id="fraUploaderPendidikanNonFormal" style="width: 100%; height: 67px; overflow: hidden; border-style: solid; border-width: 0px; border-color: #E5E5E5;" frameborder="0" scrolling="no"></iframe>
                                                                </div>
                                                                <div id="divUploadPendidikanNonFormal" style="width: 100%; position: absolute; left: -10000px; top: -10000px;">
                                                                    <asp:FileUpload runat="server" ID="flUploadPendidikanNonFormal" />
                                                                </div>
                                                                <div id="divUploadedFilesPendidikanNonFormal" runat="server" visible="false">
                                                                    <asp:Literal runat="server" ID="ltrUploadedFilesPendidikanNonFormal"></asp:Literal>
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
                                <asp:LinkButton OnClientClick="ShowProgress(Page_ClientValidate('vldInputPendidikanNonFormal'));" ValidationGroup="vldInputPendidikanNonFormal" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInputPendidikanNonFormal" OnClick="lnkOKInputPendidikanNonFormal_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <label id="lbl_delete_batal_upload_pendidikan_non_formal" style="display: none; font-size: small; color: grey; font-weight: bold;">
                                    &nbsp;
                                    <img src="../../../../Application_CLibs/images/giphy.gif" style="height: 16px; width: 20px;" />
                                    &nbsp;
                                </label>
                                <a id="btn_batal_upload_pendidikan_non_formal" onclick="DeleteBatalPendidikanNonFormal();" class="btn btn-flat btn-brand-accent waves-attach waves-effect">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_confirm_hapus_pendidikan_non_formal" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">Konfirmasi
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
                                                            <asp:Literal runat="server" ID="ltrMsgConfirmHapusPendidikanNonFormal"></asp:Literal>
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapusPendidikanNonFormal" OnClick="lnkOKHapusPendidikanNonFormal_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_pengalaman_kerja_dalam" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                    <span style="font-weight: bold;">Data Pengalaman Kerja Dalam Perusahaan
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
                                                        <label class="label-input" for="<%= cboDivisiPengalamanKerjaDalam.ClientID %>" style="text-transform: none;">Divisi</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputPengalamanKerjaDalam" runat="server" ID="vldDivisiPengalamanKerjaDalam"
                                                            ControlToValidate="cboDivisiPengalamanKerjaDalam" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList runat="server" ID="cboDivisiPengalamanKerjaDalam" CssClass="form-control">
                                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboUnitPengalamanKerjaDalam.ClientID %>" style="text-transform: none;">Unit</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputPengalamanKerjaDalam" runat="server" ID="vldUnitPengalamanKerjaDalam"
                                                            ControlToValidate="cboUnitPengalamanKerjaDalam" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList runat="server" ID="cboUnitPengalamanKerjaDalam" CssClass="form-control">
                                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtPengalamanKerjaDalamDari.ClientID %>" style="text-transform: none;">Dari</label>
                                                        <asp:TextBox ValidationGroup="vldInputPengalamanKerjaDalam" CssClass="form-control" runat="server" ID="txtPengalamanKerjaDalamDari"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtPengalamanKerjaDalamSampai.ClientID %>" style="text-transform: none;">Sampai</label>
                                                        <asp:TextBox ValidationGroup="vldInputPengalamanKerjaDalam" CssClass="form-control" runat="server" ID="txtPengalamanKerjaDalamSampai"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtJabatanPengalamanKerjaDalam.NamaClientID %>" style="text-transform: none;">Jabatan</label>
                                                        <ucl:AutocompleteJabatan runat="server" ID="txtJabatanPengalamanKerjaDalam" />
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
                                <asp:LinkButton OnClientClick="ShowProgress(Page_ClientValidate('vldInputPengalamanKerjaDalam'));" ValidationGroup="vldInputPengalamanKerjaDalam" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInputPegalamanKerjaDalam" OnClick="lnkOKInputPegalamanKerjaDalam_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_confirm_hapus_pengalaman_kerja_dalam" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">Konfirmasi
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
                                                            <asp:Literal runat="server" ID="ltrMsgConfirmHapusPengalamanKerjaDalam"></asp:Literal>
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapusPengalamanKerjaDalam" OnClick="lnkOKHapusPengalamanKerjaDalam_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_pengalaman_kerja_luar" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                    <span style="font-weight: bold;">Data Pengalaman Kerja Luar Perusahaan
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
                                                        <label class="label-input" for="<%= txtNamaPerusahaanPengalamanLuar.ClientID %>" style="text-transform: none;">Nama Perusahaan</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputPengalamanKerjaLuar" runat="server" ID="vldNamaPerusahaan"
                                                            ControlToValidate="txtNamaPerusahaanPengalamanLuar" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ValidationGroup="vldInputPengalamanKerjaLuar" CssClass="form-control" runat="server" ID="txtNamaPerusahaanPengalamanLuar"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtPengalamanKerjaLuarDari.ClientID %>" style="text-transform: none;">Dari</label>
                                                        <asp:TextBox ValidationGroup="vldInputPengalamanKerjaLuar" CssClass="form-control" runat="server" ID="txtPengalamanKerjaLuarDari"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtPengalamanKerjaLuarSampai.ClientID %>" style="text-transform: none;">Sampai</label>
                                                        <asp:TextBox ValidationGroup="vldInputPengalamanKerjaLuar" CssClass="form-control" runat="server" ID="txtPengalamanKerjaLuarSampai"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtJabatanPengalamanKerjaLuar.NamaClientID %>" style="text-transform: none;">Jabatan</label>
                                                        <ucl:AutocompleteJabatan runat="server" ID="txtJabatanPengalamanKerjaLuar" />
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
                                <asp:LinkButton OnClientClick="ShowProgress(Page_ClientValidate('vldInputPengalamanKerjaLuar'));" ValidationGroup="vldInputPengalamanKerjaLuar" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInputPegalamanKerjaLuar" OnClick="lnkOKInputPegalamanKerjaLuar_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_confirm_hapus_pengalaman_kerja_luar" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">Konfirmasi
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
                                                            <asp:Literal runat="server" ID="ltrMsgConfirmHapusPengalamanKerjaLuar"></asp:Literal>
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapusPengalamanKerjaLuar" OnClick="lnkOKHapusPengalamanKerjaLuar_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_pengalaman_sharing" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                    <span style="font-weight: bold;">Data Pengalaman Sharing
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
                                                        <label class="label-input" for="<%= txtTahunPengalamanSharing.ClientID %>" style="text-transform: none;">Tahun</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputPengalamanSharing" runat="server" ID="vldTahunPengalamanSharing"
                                                            ControlToValidate="txtTahunPengalamanSharing" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ValidationGroup="vldInputPengalamanSharing" CssClass="form-control" runat="server" ID="txtTahunPengalamanSharing"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtTopikPengalamanSharing.ClientID %>" style="text-transform: none;">Topik</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputPengalamanSharing" runat="server" ID="vldTopikPengalamanSharing"
                                                            ControlToValidate="txtTopikPengalamanSharing" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ValidationGroup="vldInputPengalamanSharing" CssClass="form-control" runat="server" ID="txtTopikPengalamanSharing"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtPenyelenggaraPengalamanSharing.ClientID %>" style="text-transform: none;">Penyelenggara</label>
                                                        <asp:TextBox ValidationGroup="vldInputPengalamanSharing" CssClass="form-control" runat="server" ID="txtPenyelenggaraPengalamanSharing"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtKotaPengalamanSharing.ClientID %>" style="text-transform: none;">Kota</label>
                                                        <asp:TextBox ValidationGroup="vldInputPengalamanSharing" CssClass="form-control" runat="server" ID="txtKotaPengalamanSharing"></asp:TextBox>
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
                                <asp:LinkButton OnClientClick="ShowProgress(Page_ClientValidate('vldInputPengalamanSharing'));" ValidationGroup="vldInputPengalamanSharing" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInputPegalamanSharing" OnClick="lnkOKInputPegalamanSharing_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_confirm_hapus_pengalaman_sharing" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">Konfirmasi
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
                                                            <asp:Literal runat="server" ID="ltrMsgConfirmHapusPengalamanSharing"></asp:Literal>
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapusPengalamanSharing" OnClick="lnkOKHapusPengalamanSharing_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_pengalaman_kepanitiaan" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                    <span style="font-weight: bold;">Data Pengalaman Kepanitiaan
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
                                                        <label class="label-input" for="<%= txtTahunPengalamanKepanitiaan.ClientID %>" style="text-transform: none;">Tahun</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputPengalamanKepanitiaan" runat="server" ID="vldTahunPengalamanKepanitiaan"
                                                            ControlToValidate="txtTahunPengalamanKepanitiaan" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ValidationGroup="vldInputPengalamanKepanitiaan" CssClass="form-control" runat="server" ID="txtTahunPengalamanKepanitiaan"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtKegiatanPengalamanKepanitiaan.ClientID %>" style="text-transform: none;">Kegiatan</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputPengalamanKepanitiaan" runat="server" ID="vldKegiatanPengalamanKepanitiaan"
                                                            ControlToValidate="txtKegiatanPengalamanKepanitiaan" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ValidationGroup="vldInputPengalamanKepanitiaan" CssClass="form-control" runat="server" ID="txtKegiatanPengalamanKepanitiaan"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtJabatanPengalamanKepanitiaan.ClientID %>" style="text-transform: none;">Jabatan</label>
                                                        <asp:TextBox ValidationGroup="vldInputPengalamanKepanitiaan" CssClass="form-control" runat="server" ID="txtJabatanPengalamanKepanitiaan"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtNoSuratTugasPengalamanKepanitiaan.ClientID %>" style="text-transform: none;">No.Surat Tugas</label>
                                                        <asp:TextBox ValidationGroup="vldInputPengalamanKepanitiaan" CssClass="form-control" runat="server" ID="txtNoSuratTugasPengalamanKepanitiaan"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtKeteranganPengalamanKepanitiaan.ClientID %>" style="text-transform: none;">Keterangan</label>
                                                        <asp:TextBox ValidationGroup="vldInputPengalamanKepanitiaan" CssClass="form-control" runat="server" ID="txtKeteranganPengalamanKepanitiaan"></asp:TextBox>
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
                                <asp:LinkButton OnClientClick="ShowProgress(Page_ClientValidate('vldInputPengalamanKepanitiaan'));" ValidationGroup="vldInputPengalamanKepanitiaan" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInputPegalamanKepanitiaan" OnClick="lnkOKInputPegalamanKepanitiaan_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_confirm_hapus_pengalaman_kepanitiaan" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">Konfirmasi
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
                                                            <asp:Literal runat="server" ID="ltrMsgConfirmHapusPengalamanKepanitiaan"></asp:Literal>
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapusPengalamanKepanitiaan" OnClick="lnkOKHapusPengalamanKepanitiaan_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_riwayat_kesehatan" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

                <label title=" Tutup " onclick="DeleteBatalRiwayatKesehatan();" data-dismiss="modal" style="padding: 10px; padding-top: 6px; padding-bottom: 6px; background-color: white; cursor: pointer; position: fixed; right: 35px; top: 20px; z-index: 999999; border-radius: 100%;">
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
                                    <span style="font-weight: bold;">Data Riwayat Kesehatan
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
                                                        <label class="label-input" for="<%= txtRiwayatKesehatanDariTanggal.ClientID %>" style="text-transform: none;">Dari Tanggal</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputRiwayatKesehatan" runat="server" ID="vldRiwayatKesehatanDariTanggal"
                                                            ControlToValidate="txtRiwayatKesehatanDariTanggal" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ValidationGroup="vldInputRiwayatKesehatan" CssClass="form-control" runat="server" ID="txtRiwayatKesehatanDariTanggal"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtRiwayatKesehatanSampaiTanggal.ClientID %>" style="text-transform: none;">Sampai Tanggal</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputRiwayatKesehatan" runat="server" ID="vldRiwayatKesehatanSampaiTanggal"
                                                            ControlToValidate="txtRiwayatKesehatanSampaiTanggal" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ValidationGroup="vldInputRiwayatKesehatan" CssClass="form-control" runat="server" ID="txtRiwayatKesehatanSampaiTanggal"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px; padding-top: 10px; padding-bottom: 10px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group" style="margin-top: 0px; margin-bottom: 0px;">
                                                        <div class="checkbox switch">
                                                            <label for="<%= chkIsIzin.ClientID %>">
                                                                <input runat="server" class="access-hide" id="chkIsIzin" type="checkbox"><span class="switch-toggle"></span>
                                                                <span style="font-weight: normal; font-size: 14px; color: grey;">Tidak Masuk Kerja
                                                                </span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtRiwayatKesehatanNamaPenyakit.ClientID %>" style="text-transform: none;">Diagnosa/Nama Penyakit</label>
                                                        <asp:TextBox ValidationGroup="vldInputRiwayatKesehatan" CssClass="form-control" runat="server" ID="txtRiwayatKesehatanNamaPenyakit"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtRiwayatKesehatanKlinikRumahSakit.ClientID %>" style="text-transform: none;">Klinik/Rumah Sakit</label>
                                                        <asp:TextBox ValidationGroup="vldInputRiwayatKesehatan" CssClass="form-control" runat="server" ID="txtRiwayatKesehatanKlinikRumahSakit"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtRiwayatKesehatanNamaDokter.ClientID %>" style="text-transform: none;">Dokter</label>
                                                        <asp:TextBox ValidationGroup="vldInputRiwayatKesehatan" CssClass="form-control" runat="server" ID="txtRiwayatKesehatanNamaDokter"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtRiwayatKesehatanKeterangan.ClientID %>" style="text-transform: none;">Keterangan</label>
                                                        <asp:TextBox ValidationGroup="vldInputRiwayatKesehatan" CssClass="form-control" runat="server" ID="txtRiwayatKesehatanKeterangan"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div runat="server" id="div_upload_file_riwayat_kesehatan" class="row" style="margin-left: 30px; margin-right: 30px; margin-top: 15px;">
                                                <div style="padding: 10px; border-style: solid; border-width: 1px; border-color: #b1e1ff; border-radius: 5px; background-color: #ddf2ff;">
                                                    <span style="font-weight: normal; color: #1DA1F2;">
                                                        <span style="font-weight: bold;">Upload file riwayat kesehatan
                                                        </span>
                                                        <br />
                                                        Format File : 
                                                        <br />
                                                        PDF, DOC, DOCX, PPT, PPTX, XLS, XLSX
                                                        <br />
                                                        JPG, JPEG, PNG, BMP
                                                    </span>

                                                    <div class="row">
                                                        <div class="col-md-12">

                                                            <div style="margin-top: 10px; width: 100%; overflow: hidden; padding: 0px;">
                                                                <div runat="server" id="div_upload_riwayat_kesehatan">
                                                                    <iframe id="fraUploaderRiwayatKesehatan" style="width: 100%; height: 67px; overflow: hidden; border-style: solid; border-width: 0px; border-color: #E5E5E5;" frameborder="0" scrolling="no"></iframe>
                                                                </div>
                                                                <div id="divUploadRiwayatKesehatan" style="width: 100%; position: absolute; left: -10000px; top: -10000px;">
                                                                    <asp:FileUpload runat="server" ID="flUploadRiwayatKesehatan" />
                                                                </div>
                                                                <div id="divUploadedFilesRiwayatKesehatan" runat="server" visible="false">
                                                                    <asp:Literal runat="server" ID="ltrUploadedFilesRiwayatKesehatan"></asp:Literal>
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
                                <asp:LinkButton OnClientClick="ShowProgress(Page_ClientValidate('vldInputRiwayatKesehatan'));" ValidationGroup="vldInputRiwayatKesehatan" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInputRiwayatKesehatan" OnClick="lnkOKInputRiwayatKesehatan_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <label id="lbl_delete_batal_upload_riwayat_kesehatan" style="display: none; font-size: small; color: grey; font-weight: bold;">
                                    &nbsp;
                                    <img src="../../../../Application_CLibs/images/giphy.gif" style="height: 16px; width: 20px;" />
                                    &nbsp;
                                </label>
                                <a id="btn_batal_upload_riwayat_kesehatan" onclick="DeleteBatalRiwayatKesehatan();" class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_confirm_hapus_riwayat_kesehatan" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">Konfirmasi
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
                                                            <asp:Literal runat="server" ID="ltrMsgConfirmHapusRiwayatKesehatan"></asp:Literal>
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapusRiwayatKesehatan" OnClick="lnkOKHapusRiwayatKesehatan_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_riwayat_mcu" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

                <label title=" Tutup " onclick="DeleteBatalRiwayatMCU();" data-dismiss="modal" style="padding: 10px; padding-top: 6px; padding-bottom: 6px; background-color: white; cursor: pointer; position: fixed; right: 35px; top: 20px; z-index: 999999; border-radius: 100%;">
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
                                    <span style="font-weight: bold;">Data Riwayat Medical Check Up
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
                                                        <label class="label-input" for="<%= txtRiwayatMCUTanggal.ClientID %>" style="text-transform: none;">Tanggal</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputRiwayatMCU" runat="server" ID="vldRiwayatMCUTanggal"
                                                            ControlToValidate="txtRiwayatMCUTanggal" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ValidationGroup="vldInputRiwayatMCU" CssClass="form-control" runat="server" ID="txtRiwayatMCUTanggal"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtRiwayatMCUKesimpulan.ClientID %>" style="text-transform: none;">Kesimpulan</label>
                                                        <asp:TextBox TextMode="MultiLine" Rows="3" ValidationGroup="vldInputRiwayatMCU" CssClass="form-control" runat="server" ID="txtRiwayatMCUKesimpulan" Style="resize: none;"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtRiwayatMCUSaran.ClientID %>" style="text-transform: none;">Saran</label>
                                                        <asp:TextBox TextMode="MultiLine" Rows="3" ValidationGroup="vldInputRiwayatMCU" CssClass="form-control" runat="server" ID="txtRiwayatMCUSaran" Style="resize: none;"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtRiwayatMCUKeterangan.ClientID %>" style="text-transform: none;">Keterangan</label>
                                                        <asp:TextBox ValidationGroup="vldInputRiwayatMCU" CssClass="form-control" runat="server" ID="txtRiwayatMCUKeterangan"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div runat="server" id="div_upload_file_riwayat_mcu" class="row" style="margin-left: 30px; margin-right: 30px; margin-top: 15px;">
                                                <div style="padding: 10px; border-style: solid; border-width: 1px; border-color: #b1e1ff; border-radius: 5px; background-color: #ddf2ff;">
                                                    <span style="font-weight: normal; color: #1DA1F2;">
                                                        <span style="font-weight: bold;">Upload file riwayat MCU
                                                        </span>
                                                        <br />
                                                        Format File : 
                                                        <br />
                                                        PDF, DOC, DOCX, PPT, PPTX, XLS, XLSX
                                                        <br />
                                                        JPG, JPEG, PNG, BMP
                                                    </span>

                                                    <div class="row">
                                                        <div class="col-md-12">

                                                            <div style="margin-top: 10px; width: 100%; overflow: hidden; padding: 0px;">
                                                                <div runat="server" id="div_upload_riwayat_mcu">
                                                                    <iframe id="fraUploaderRiwayatMCU" style="width: 100%; height: 67px; overflow: hidden; border-style: solid; border-width: 0px; border-color: #E5E5E5;" frameborder="0" scrolling="no"></iframe>
                                                                </div>
                                                                <div id="divUploadRiwayatMCU" style="width: 100%; position: absolute; left: -10000px; top: -10000px;">
                                                                    <asp:FileUpload runat="server" ID="flUploadRiwayatMCU" />
                                                                </div>
                                                                <div id="divUploadedFilesRiwayatMCU" runat="server" visible="false">
                                                                    <asp:Literal runat="server" ID="ltrUploadedFilesRiwayatMCU"></asp:Literal>
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
                                <asp:LinkButton OnClientClick="ShowProgress(Page_ClientValidate('vldInputRiwayatMCU'));" ValidationGroup="vldInputRiwayatMCU" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInputRiwayatMCU" OnClick="lnkOKInputRiwayatMCU_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <label id="lbl_delete_batal_upload_riwayat_mcu" style="display: none; font-size: small; color: grey; font-weight: bold;">
                                    &nbsp;
                                    <img src="../../../../Application_CLibs/images/giphy.gif" style="height: 16px; width: 20px;" />
                                    &nbsp;
                                </label>
                                <a id="btn_batal_upload_riwayat_mcu" onclick="DeleteBatalRiwayatMCU();" class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_confirm_hapus_riwayat_mcu" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">Konfirmasi
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
                                                            <asp:Literal runat="server" ID="ltrMsgConfirmHapusRiwayatMCU"></asp:Literal>
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapusRiwayatMCU" OnClick="lnkOKHapusRiwayatMCU_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_upload_file_pendukung" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">Upload File
                                    </span>
                                </div>
                            </div>
                            <div style="width: 100%;">
                                <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 10px;">
                                            <div runat="server" id="div_upload_file_pendukung" class="row" style="margin-left: 10px; margin-right: 10px; margin-top: 0px;">
                                                <div style="padding: 10px; border-style: solid; border-width: 1px; border-color: #b1e1ff; border-radius: 5px; background-color: #ddf2ff;">
                                                    <span style="font-weight: normal; color: #1DA1F2;">
                                                        <span style="font-weight: bold;">Upload file pendukung profil anda
                                                        </span>
                                                        <br />
                                                        Format File : 
                                                        <br />
                                                        PDF, DOC, DOCX, PPT, PPTX, XLS, XLSX
                                                        <br />
                                                        JPG, JPEG, PNG, BMP
                                                    </span>

                                                    <div class="row">
                                                        <div class="col-md-12">

                                                            <div style="margin-top: 10px; width: 100%; overflow: hidden; padding: 0px;">
                                                                <div runat="server" id="divUploadFilePendukung">
                                                                    <iframe id="fraUploaderFilePendukung" style="width: 100%; height: 67px; overflow: hidden; border-style: solid; border-width: 0px; border-color: #E5E5E5;" frameborder="0" scrolling="no"></iframe>
                                                                </div>
                                                                <div id="divUploadFilePendukung" style="width: 100%; position: absolute; left: -10000px; top: -10000px;">
                                                                    <asp:FileUpload runat="server" ID="flUploadFilePendukung" />
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
                                <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKUploadFilePendukung" OnClick="lnkOKUploadFilePendukung_Click" Text="   TUTUP   "></asp:LinkButton>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
        RenderDropDownOnTables();
        InitPicker();
        <%= txtJurusanPendidikan.NamaClientID %>_SHOW_AUTOCOMPLETE();
        <%= txtUniversitas.NamaClientID %>_SHOW_AUTOCOMPLETE();
        <%= txtJabatanPengalamanKerjaDalam.NamaClientID %>_SHOW_AUTOCOMPLETE();
        <%= txtJabatanPengalamanKerjaLuar.NamaClientID %>_SHOW_AUTOCOMPLETE();

        window.onbeforeunload = function (event) {
            DoDeleteDirectoryNewPendidikanNonFormalCancel();
        };
    </script>
</asp:Content>
