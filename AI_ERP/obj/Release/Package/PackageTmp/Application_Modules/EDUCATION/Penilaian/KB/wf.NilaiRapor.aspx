<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="wf.NilaiRapor.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
    <script type="text/javascript">
        function HideModal(){
            $('#ui_modal_list_siswa').modal('hide');
            $('#ui_modal_proses').modal('hide');
            $('#ui_modal_nilai_standar').modal('hide');
            $('#ui_modal_input_rekomendasi').modal('hide');      
            $('#ui_modal_confirm_upload').modal('hide');
            $('#ui_modal_periode').modal('hide');            

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
                case "<%= JenisAction.DoDataBinded %>":
                    if (txtIDSiswa() !== null && txtIDSiswa() !== undefined) {
                        ShowProsesPilihSiswa(txtIDSiswa().value, false);
                    }
                    HideModal();
                    break;
                case "<%= JenisAction.Update %>":
                    HideModal();
                    break;
                case "<%= JenisAction.Delete %>":
                    HideModal();
                    break;
                case "<%= JenisAction.DoShowPengaturanPeriode %>":
                    ShowProgress(false);
                    document.body.style.paddingRight = "0px";
                    $('#ui_modal_periode').modal({ backdrop: 'static', keyboard: false, show: true });
                    document.body.style.paddingRight = "14.75px";
                    break;
                case "<%= JenisAction.DoShowConfirmPostingByKelas %>":
                case "<%= JenisAction.DoShowConfirmPostingBySiswa %>":
                    ShowProgress(false);
                    document.body.style.paddingRight = "0px";
                    $('#ui_modal_confirm_upload').modal({ backdrop: 'static', keyboard: false, show: true });
                    document.body.style.paddingRight = "14.75px";
                    break;
                case "<%= JenisAction.DoShowPengaturanNilaiStandar %>":
                case "<%= JenisAction.DoShowPengaturanNilaiStandarPerAnak %>":
                    ShowProgress(false);
                    document.body.style.paddingRight = "0px";
                    $('#ui_modal_nilai_standar').modal({ backdrop: 'static', keyboard: false, show: true });
                    document.body.style.paddingRight = "14.75px";
                    break;
                case "<%= JenisAction.DoShowInputRekomendasi %>":
                    ShowProgress(false);
                    LoadTinyMCERekomendasi();
                    document.body.style.paddingRight = "0px";
                    $('#ui_modal_input_rekomendasi').modal({ backdrop: 'static', keyboard: false, show: true });
                    document.body.style.paddingRight = "14.75px";
                    break;          
                case "<%= JenisAction.DoShowPrintRapor %>":
                    ShowPrintRapor();
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
                case "<%= JenisAction.DoPostingPerKelas %>":    
                case "<%= JenisAction.DoPostingPerSiswa %>":    
                    HideModal();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah di-upload',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoUpdate %>":
                case "<%= JenisAction.DoUpdateItem %>":
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

        function ShowPrintRapor(){
            
        }

        function InitModalFocus(){
            $('#ui_modal_input_rekomendasi').on('shown.bs.modal', function () {
                tinyMCE.execCommand('mceFocus',false,'<%= txtRekomendasi.ClientID %>');
            });
        }

        function guid() {
            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000)
                  .toString(16)
                  .substring(1);
            }
            return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
              s4() + '-' + s4() + s4() + s4();
        }


        function stopRKey(evt) {
            var evt = (evt) ? evt : ((event) ? event : null);
            var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
            if ((evt.keyCode == 13) && (node.type == "text")) { return false; }
        }
        document.onkeypress = stopRKey;

        function CheckKriteriaPenilaian(chk_name, chk_id) {
            var arr_chk = document.getElementsByName(chk_name);
            var chk_id = document.getElementById(chk_id);

            if (arr_chk.length > 0) {
                if (chk_id != null && chk_id != undefined) {
                    for (var i = 0; i < arr_chk.length; i++) {
                        if (arr_chk[i].id !== chk_id.id) {
                            arr_chk[i].checked = false;
                        }
                    }
                }
            }
        }

        function LoadTinyMCERekomendasi() {
            tinymce.remove();
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                mode : "exact",
                selector: ".mcetiny_rekomendasi",
                theme: "modern",
                plugins: [
                    "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                    "searchreplace wordcount visualblocks visualchars code fullscreen",
                    "insertdatetime media nonbreaking save table contextmenu directionality",
                    "emoticons template paste textcolor tinyfilemanager.net"
                ],
                toolbar1: "bold italic underline | forecolor backcolor | bullist numlist | undo redo",
                image_advtab: true,
                templates: [
                    { title: 'Test template 1', content: 'Test 1' },
                    { title: 'Test template 2', content: 'Test 2' }
                ],
                resize: "vertical",
                statusbar: false,
                menubar: false,
                height: 300,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtRekomendasiVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
        }

        function ReInitTinyMCE(){
            //untuk rapor            
            var arr_txt = document.getElementsByName("txt_rekomendasi[]");
            var arr_txt_val = document.getElementsByName("txt_rekomendasi_val[]");

            tinymce.remove();
            if(arr_txt.length === arr_txt_val.length){
                if(arr_txt.length > 0){
                    for (var i = 0; i < arr_txt.length; i++) {
                        LoadTinyMCE(arr_txt[i].id, arr_txt[i].className, arr_txt_val[i].id);
                    }
                }
            }

            //untuk ekskul
            arr_txt = document.getElementsByName("txt_rekomendasi_ekskul[]");
            arr_txt_val = document.getElementsByName("txt_rekomendasi_val_ekskul[]");

            if(arr_txt.length === arr_txt_val.length){
                if(arr_txt.length > 0){
                    for (var i = 0; i < arr_txt.length; i++) {
                        LoadTinyMCE(arr_txt[i].id, arr_txt[i].className, arr_txt_val[i].id);
                    }
                }
            }
        }

        function TriggerSave(){
            tinyMCE.triggerSave();
        }

        function txtIndexSiswa() { return document.getElementById("<%= txtIndexSiswa.ClientID %>"); }
        function txtCountSiswa() { return document.getElementById("<%= txtCountSiswa.ClientID %>"); }
        function btnShowNilaiSiswa() { return document.getElementById("<%= btnShowNilaiSiswa.ClientID %>"); }

        function FirstSiswa() {
            if (
                    txtIndexSiswa() !== null && txtIndexSiswa() !== undefined &&
                    txtCountSiswa() !== null && txtCountSiswa() !== undefined &&
                    btnShowNilaiSiswa() !== null && btnShowNilaiSiswa() !== undefined
               ) {
                var id = parseInt(txtIndexSiswa().value);
                var count = parseInt(txtCountSiswa().value);

                if (id >= 0) {
                    txtIndexSiswa().value = 0;
                    btnShowNilaiSiswa().click();
                }
            }
        }

        function PrevSiswa() {
            if (
                    txtIndexSiswa() !== null && txtIndexSiswa() !== undefined &&
                    txtCountSiswa() !== null && txtCountSiswa() !== undefined &&
                    btnShowNilaiSiswa() !== null && btnShowNilaiSiswa() !== undefined
               ) {
                var id = parseInt(txtIndexSiswa().value);
                var count = parseInt(txtCountSiswa().value);

                if (id - 1 >= 0) {
                    txtIndexSiswa().value = id - 1;
                    btnShowNilaiSiswa().click();
                }
            }
        }

        function NextSiswa() {
            if (
                    txtIndexSiswa() !== null && txtIndexSiswa() !== undefined &&
                    txtCountSiswa() !== null && txtCountSiswa() !== undefined &&
                    btnShowNilaiSiswa() !== null && btnShowNilaiSiswa() !== undefined
               ) {
                var id = parseInt(txtIndexSiswa().value);
                var count = parseInt(txtCountSiswa().value);

                if (id + 1 < count) {
                    txtIndexSiswa().value = id + 1;
                    btnShowNilaiSiswa().click();
                }
            }
        }

        function LastSiswa() {
            if (
                    txtIndexSiswa() !== null && txtIndexSiswa() !== undefined &&
                    txtCountSiswa() !== null && txtCountSiswa() !== undefined &&
                    btnShowNilaiSiswa() !== null && btnShowNilaiSiswa() !== undefined
               ) {
                var id = parseInt(txtIndexSiswa().value);
                var count = parseInt(txtCountSiswa().value);

                if (id <= count - 1) {
                    txtIndexSiswa().value = count - 1;
                    btnShowNilaiSiswa().click();
                }
            }
        }

        function txtTahunAjaran() { return document.getElementById("<%= txtTahunAjaran.ClientID %>"); }
        function txtSemester() { return document.getElementById("<%= txtSemester.ClientID %>"); }
        function txtIDSiswa() { return document.getElementById("<%= txtIDSiswa.ClientID %>"); }
        function txtKelasDet() { return document.getElementById("<%= txtKelasDet.ClientID %>"); }
        function txtPoinPenilaian() { return document.getElementById("<%= txtPoinPenilaian.ClientID %>"); }
        function txtKriteriaPenilaian() { return document.getElementById("<%= txtKriteriaPenilaian.ClientID %>"); }
        function txtPoinPenilaianDeskripsi() { return document.getElementById("<%= txtPoinPenilaianDeskripsi.ClientID %>"); }
        function txtDeskripsiPenilaian() { return document.getElementById("<%= txtDeskripsi.ClientID %>"); }
        function txtBeratBadan() { return document.getElementById("<%= txtBeratBadan.ClientID %>"); }
        function txtTinggiBadan() { return document.getElementById("<%= txtTinggiBadan.ClientID %>"); }
        function txtLingkarKepala() { return document.getElementById("<%= txtLingkarKepala.ClientID %>"); }
        function txtUsia() { return document.getElementById("<%= txtUsia.ClientID %>"); }
        function txtIDNilaiSiswa() { return document.getElementById("<%= txtIDNilaiSiswa.ClientID %>"); }
        
        function save_perbaikan()
        {
            var arr_1 = document.getElementsByName("chk_desain_1[]");
            var arr_2 = document.getElementsByName("chk_desain_2[]");
            var arr_3 = document.getElementsByName("chk_desain_3[]");

            for (var i = 0; i < arr_1.length; i++) {
                var kriteria = "";
                if(arr_1[i].checked){
                    kriteria = arr_1[i].value;
                }
                if(arr_2[i].checked){
                    kriteria = arr_2[i].value;
                }
                if(arr_3[i].checked){
                    kriteria = arr_3[i].value;
                }

                txtPoinPenilaian().value = arr_1[i].lang;
                txtKriteriaPenilaian().value = kriteria;
                SaveNilaiRapor("");
            }

            <%= btnDoBindData.ClientID %>.click();
        }

        function SaveNilaiRapor(deskripsi) {
            if (
                    txtTahunAjaran() !== null && txtTahunAjaran() !== undefined &&
                    txtSemester() !== null && txtSemester() !== undefined &&
                    txtIDSiswa() !== null && txtIDSiswa() !== undefined &&
                    txtKelasDet() !== null && txtKelasDet() !== undefined &&
                    txtPoinPenilaian() !== null && txtPoinPenilaian() !== undefined &&
                    txtKriteriaPenilaian() !== null && txtKriteriaPenilaian() !== undefined &&
                    txtIDNilaiSiswa() !== null && txtIDNilaiSiswa() !== undefined
               )
            {
                var tahun_ajaran = txtTahunAjaran().value;
                var semester = txtSemester().value;
                var siswa = txtIDSiswa().value;
                var rel_kelasdet = txtKelasDet().value;
                var poinpenilaian = txtPoinPenilaian().value;
                var rel_kriteria = txtKriteriaPenilaian().value;
                var rel_nilaisiswa = txtIDNilaiSiswa().value;
                
                var s_url = '<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APIS.KB.NILAI_SISWA.DO_SAVE.FILE + "/Do") %>' +
                            '?' +
                            't=' + tahun_ajaran + '&' +
                            'sm=' + semester + '&' +
                            's=' + siswa + '&' +
                            'kd=' + rel_kelasdet + '&' +
                            'pp=' + poinpenilaian + '&' +
                            'kr=' + rel_kriteria + '&' +
                            'ds=' + deskripsi + '&' +
                            'ns=' + rel_nilaisiswa;

                $.ajax({
                    url: s_url,
                    dataType: 'json',
                    type: 'GET',
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {

                    },
                    error: function (response) {
                        HideModal();
                        if (jenis_act.trim() != ""){
                            $('body').snackbar({
                                alive: 6000,
                                content: '<i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;&nbsp;DATA GAGAL DISIMPAN : ' + response.responseText,
                                show: function () {
                                    snackbarText++;
                                }
                            });
                        }
                    },
                    failure: function (response) {
                        HideModal();
                        if (jenis_act.trim() != ""){
                            $('body').snackbar({
                                alive: 6000,
                                content: '<i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;&nbsp;DATA GAGAL DISIMPAN : ' + response.responseText,
                                show: function () {
                                    snackbarText++;
                                }
                            });
                        }
                    }
                });
            }
        }

        function SavePertumbuhanFisik(){
            if (
                    txtTahunAjaran() !== null && txtTahunAjaran() !== undefined &&
                    txtSemester() !== null && txtSemester() !== undefined &&
                    txtIDSiswa() !== null && txtIDSiswa() !== undefined &&
                    txtKelasDet() !== null && txtKelasDet() !== undefined &&
                    txtBeratBadan() !== null && txtBeratBadan() !== undefined &&
                    txtTinggiBadan() !== null && txtTinggiBadan() !== undefined &&
                    txtLingkarKepala() !== null && txtLingkarKepala !== undefined &&
                    txtUsia() !== null && txtUsia() !== undefined
               )
            {
                var tahun_ajaran = txtTahunAjaran().value;
                var semester = txtSemester().value;
                var siswa = txtIDSiswa().value;
                var rel_kelasdet = txtKelasDet().value;
                var berat_badan = txtBeratBadan().value;
                var tinggi_badan = txtTinggiBadan().value;
                var lingkar_kepala = txtLingkarKepala().value;
                var usia = txtUsia().value;
                
                var s_url = '<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APIS.KB.NILAI_SISWA.DO_SAVE.FILE + "/DoPF") %>' +
                            '?' +
                            't=' + tahun_ajaran + '&' +
                            'sm=' + semester + '&' +
                            's=' + siswa + '&' +
                            'kd=' + rel_kelasdet + '&' +
                            'bb=' + berat_badan + '&' +
                            'tb=' + tinggi_badan + '&' +
                            'lk=' + lingkar_kepala + '&' +
                            'u=' + usia;

                $.ajax({
                    url: s_url,
                    dataType: 'json',
                    type: 'GET',
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {

                    },
                    error: function (response) {
                        HideModal();
                        if (jenis_act.trim() != ""){
                            $('body').snackbar({
                                alive: 6000,
                                content: '<i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;&nbsp;DATA GAGAL DISIMPAN : ' + response.responseText,
                                show: function () {
                                    snackbarText++;
                                }
                            });
                        }
                    },
                    failure: function (response) {
                        HideModal();
                        if (jenis_act.trim() != ""){
                            $('body').snackbar({
                                alive: 6000,
                                content: '<i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;&nbsp;DATA GAGAL DISIMPAN : ' + response.responseText,
                                show: function () {
                                    snackbarText++;
                                }
                            });
                        }
                    }
                });
            }
        }

        function ShowProsesPilihSiswa(id_siswa, show) {
            var id = id_siswa.replaceAll("-", "_");
            var id_img = "img_" + id;
            var id_lbl = "lbl_" + id;
            var lbl = document.getElementById(id_lbl);
            var img = document.getElementById(id_img);
            if (
                lbl !== null && lbl !== undefined &&
                img !== null && img !== undefined
            ) {
                img.style.display = (show ? "" : "none");
                lbl.style.display = (show ? "none" : "");
            }
        }

        function ShowProgress(show) {
            if (show) {
                $('#ui_modal_proses').modal({ backdrop: 'static', keyboard: false, show: true });
            }
            else {
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                $('#ui_modal_proses').modal('hide');
            }
        }

        function GetIDSiswa(){
            var s_siswa = "";
            var txt_siswa = document.getElementById("<%= txtJenisNilaiDefault.ClientID %>");
            if(txt_siswa !== null && txt_siswa !== undefined){
                s_siswa = txt_siswa.value;
            }
            return s_siswa;
        }

        function ProcessPengaturan() {
            var url = "<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.BUAT_NILAI_RAPOR_STANDAR.ROUTE) %>";
            var arr_kriteria = document.getElementsByName("rdo_kriteria[]");
            var s_kriteria = "";
            for (var i = 0; i < arr_kriteria.length; i++) {
                if(arr_kriteria[i].checked){
                    s_kriteria = arr_kriteria[i].value;
                    break;
                }
            }

            if(s_kriteria.trim() !== ""){
                url += "?";
                url += "rd=" + <%= txtID.ClientID %>.value;
                url += "&kd=" + "<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.QS.GetKelas() %>";
                url += (GetIDSiswa().trim() !== "" ? "&s=" + GetIDSiswa() : "");
                url += "&kt=" + s_kriteria;

                if (navigator.appName == 'Microsoft Internet Explorer') {
                    window.frames['fra_download'].document.location.href = url;
                } else {
                    window.frames['fra_download'].location.href = url;
                }
            }
            else {
                ShowProcessPengaturan(false);
                HideModal();
                $('body').snackbar({
                    alive: 3000,
                    content: '<i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;&nbsp;PERHATIAN : Anda belum memilih nilai standar',
                    show: function () {
                        snackbarText++;
                    }
                });
            }
        }

        function ShowProcessPengaturan(show) {
            pb_proses_nilai_standar.style.display = (show ? "" : "none");
            div_command_pengaturan.style.display = (show ? "none" : "");
            if (!show) {
                if (navigator.appName == 'Microsoft Internet Explorer') {
                    window.frames['fra_download'].document.execCommand('Stop');
                } else {
                    window.frames['fra_download'].stop();
                }
            }
        }

        function StopProses()
        {
            if (navigator.appName == 'Microsoft Internet Explorer') {
                window.frames['fra_download'].document.execCommand('Stop');
            } else {
                window.frames['fra_download'].stop();
            }
            if(GetIDSiswa() === ""){
                <%= btnDoBindListViewNilai.ClientID %>.click();
            } else {
                <%= btnDoBindData.ClientID %>.click();
            }
            setTimeout(function(){
                ShowProcessPengaturan(false);
                HideModal();
            }, 2000);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="upMainDeskripsi" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="txtPoinPenilaianDeskripsi" />
            <asp:HiddenField runat="server" ID="txtDeskripsi" />            

            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoSaveDeskripsi" OnClick="btnDoSaveDeskripsi_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoBindData" OnClick="btnDoBindData_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button OnClientClick="ShowProgress(true);" UseSubmitBehavior="false" runat="server" ID="btnShowDeskripsi" OnClick="btnShowDeskripsi_Click" style="position: absolute; left: -1000px; top: -1000px;" />

        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server" ID="upMain">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="txtKeyAction" />
            <asp:HiddenField runat="server" ID="txtID" />
            <asp:HiddenField runat="server" ID="txtIDSiswa" />
            <asp:HiddenField runat="server" ID="txtIDRekomendasi" />
            <asp:HiddenField runat="server" ID="txtJenisRekomendasi" />
            <asp:HiddenField runat="server" ID="txtJenisNilaiDefault" />
            <asp:HiddenField runat="server" ID="txtIDNilaiSiswa" />
            <asp:HiddenField runat="server" ID="txtIndexSiswa" />
            <asp:HiddenField runat="server" ID="txtCountSiswa" />
            <asp:HiddenField runat="server" ID="txtTahunAjaran" />
            <asp:HiddenField runat="server" ID="txtJenisRapor" />            
            <asp:HiddenField runat="server" ID="txtSemester" />
            <asp:HiddenField runat="server" ID="txtKelasDet" />
            <asp:HiddenField runat="server" ID="txtPoinPenilaian" />
            <asp:HiddenField runat="server" ID="txtKriteriaPenilaian" />
            <asp:HiddenField runat="server" ID="txtRekomendasiVal" />
            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoCari" OnClick="btnDoCari_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoBindListViewNilai" OnClick="btnDoBindListViewNilai_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDesain" OnClientClick="ShowProgress(true);" OnClick="btnShowDesain_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowPengaturanNilaiStandar" OnClientClick="ShowProgress(true);" OnClick="btnShowPengaturanNilaiStandar_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowNilaiSiswa" OnClientClick="HideModal(); ShowProgress(true);" OnClick="btnShowNilaiSiswa_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowConfirmPostingByKelas" OnClick="btnShowConfirmPostingByKelas_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            
            <div class="row" style="margin-left: 0px; margin-right: 0px; margin-bottom: 200px;">
                <div class="col-xs-12">

                    <div class="col-md-8 col-md-offset-2" style="padding: 0px;">
                        <div class="card" style="margin-top: 40px;">
				            <div class="card-main">
					            <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px; ">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="background-color: #4b4b4b; padding: 10px; font-weight: bold; vertical-align: middle; color: white; padding-left: 70px; font-size: 15px;
                                                <asp:Literal runat="server" ID="ltrBGHeader"></asp:Literal>">
                                                Nilai Rapor
                                                <asp:Literal runat="server" ID="ltrTipeRapor"></asp:Literal>
                                                <br />
                                                <span style="font-weight: normal;">
                                                    Lihat/Edit data nilai siswa
                                                </span>
                                                <br />
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
								                                <tbody>     
                                                                    <tr id="itemPlaceholder" runat="server"></tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr class="<%# (Container.DisplayIndex % 2 == 0 ? "standardrow" : "oddrow") %>">
                                                            <td style="text-align: center; padding: 10px; vertical-align: middle; width: 30px;">
                                                                <div class="tile-side pull-left" data-ignore="tile" style="margin-top: 10px; margin-right: 10px;">
                                                                    <div class="avatar avatar-sm" style="background-color: #cee7ed;">
                                                                        <i class="fa fa-folder" style="color: #0097c0;"></i>
                                                                    </div>
                                                                </div>                                                                
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("TahunAjaran").ToString())
                                                                    %>
                                                                </span>
                                                                <sup title=" Semester " style="font-weight: normal; text-transform: none; text-decoration: none; font-weight: bold;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Semester").ToString())
                                                                    %>
                                                                </sup>
                                                                <%# 
                                                                    AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.IsPosted(
                                                                        Eval("TahunAjaran").ToString(),
                                                                        Eval("Semester").ToString(),
                                                                        AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.QS.GetKelas()
                                                                    ) ? "<sup title=\" Sudah Di-upload \" style=\"color: green;\"><i class=\"fa fa-check-circle\"></i></sup>"
                                                                      : ""
                                                                %>
                                                                <br />
                                                                <span style="color: #1DA1F2; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        (
                                                                            AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                                                            ? Eval("NamaKelasDet").ToString() +
                                                                              "<br />" +
                                                                              "<span style=\"font-weight: normal; color: grey;\">" +
                                                                              AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("GuruKelas").ToString()) +
                                                                              (
                                                                                    AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("GuruPendamping").ToString()).Trim() != ""
                                                                                    ? "<span style=\"font-weight: bold;\"> & </span>" + AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("GuruPendamping").ToString())
                                                                                    : ""
                                                                              ) +
                                                                              "</span>"
                                                                            : AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.GetNamaKelas()
                                                                        )
                                                                    %>
                                                                </span>
                                                                <br />
                                                                <%# 
                                                                    Eval("JenisRapor").ToString()
                                                                %>
                                                                &nbsp;
                                                                <%# 
                                                                    !AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                                                    ? (
                                                                            Eval("TipeRapor").ToString() == AI_ERP.Application_Libs.TipeRapor.LTS
                                                                            ? "<span class=\"badge\" style=\"background-color: red; font-weight: bold;\">Rapor LTS</span>"
                                                                            : "<span class=\"badge\" style=\"background-color: green; font-weight: bold;\">Rapor Semester</span>"
                                                                      )
                                                                    : ""
                                                                %>
                                                                <br />
                                                                <%# 
                                                                    (
                                                                        !AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.IsUdahDiaturNilaiDefault(
                                                                            Eval("Kode").ToString(),
                                                                            (
                                                                                AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                                                                ? Eval("KodeKelasDet").ToString()
                                                                                : AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.QS.GetKelas()
                                                                            )                                                                            
                                                                        ) == true
                                                                        ? (
                                                                                AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit() 
                                                                                ? ""
                                                                                : "<label title=\" Pengaturan \" onclick=\"" + txtJenisNilaiDefault.ClientID + ".value = ''; ShowProgress(true); setTimeout(function(){ " + txtID.ClientID + ".value = '" + Eval("Kode").ToString() + "';" + btnShowPengaturanNilaiStandar.ClientID + ".click(); }, 500);\" style=\"cursor: pointer; font-weight: normal; font-size: small; color: green; margin-top: 10px;\">" +
                                                                                    "<i class=\"fa fa-cogs\"></i>" +
                                                                                    "&nbsp;" +
                                                                                    "Pengaturan" +
                                                                                  "</label>" +
                                                                                  "<span style=\"color: #dddddd;\">&nbsp;&nbsp;|&nbsp;&nbsp;</span>"
                                                                          ) +  
                                                                          "<label title=\" Buka \" style=\"font-weight: normal; font-size: small; color: #bfbfbf; margin-top: 10px;\">" +
                                                                            "&nbsp;" +
                                                                            "<i class=\"fa fa-folder-open\"></i>" +
                                                                            "&nbsp;" +
                                                                            "Buka" +
                                                                            "&nbsp;" +
                                                                          "</label>" +
                                                                          "<span style=\"color: #dddddd;\">&nbsp;&nbsp;|&nbsp;&nbsp;</span>" +
                                                                          "<label title=\" Upload \" style=\"font-weight: normal; font-size: small; color: #bfbfbf; margin-top: 10px;\">" +
                                                                            "&nbsp;" +
                                                                            "<i class=\"fa fa-upload\"></i>" +
                                                                            "&nbsp;" +
                                                                            "Upload" +
                                                                            "&nbsp;" +
                                                                          "</label>"

                                                                        : (
                                                                                AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit() 
                                                                                ? ""
                                                                                : "<label title=\" Pengaturan \" style=\"font-weight: normal; font-size: small; color: #bfbfbf; margin-top: 10px;\">" +
                                                                                    "<i class=\"fa fa-cogs\"></i>" +
                                                                                    "&nbsp;" +
                                                                                    "Pengaturan" +
                                                                                  "</label>" +
                                                                                  "<span style=\"color: #dddddd;\">&nbsp;&nbsp;|&nbsp;&nbsp;</span>" 
                                                                          ) +
                                                                          "<label title=\" Buka \" onclick=\"ShowProgress(true); " + (AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()  ? txtKelasDet.ClientID + ".value = '" + Eval("KodeKelasDet").ToString() + "'; " : "") + txtID.ClientID + ".value = '" + Eval("Kode").ToString() + "';" + btnShowDesain.ClientID + ".click();\" style=\"cursor: pointer; font-weight: normal; font-size: small; color: green; margin-top: 10px;\">" +
                                                                            "&nbsp;" +
                                                                            "<i class=\"fa fa-folder-open\"></i>" +
                                                                            "&nbsp;" +
                                                                            "Buka" +
                                                                            "&nbsp;" +
                                                                            AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.GetHTMLNotifJumlahSiswa(
                                                                                (
                                                                                    AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                                                                    ? Eval("KodeKelasDet").ToString()
                                                                                    : AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.QS.GetKelas()
                                                                                ),
                                                                                Eval("Kode").ToString()
                                                                            ) +
                                                                          "</label>" +
                                                                          "<span style=\"color: #dddddd;\">&nbsp;&nbsp;|&nbsp;&nbsp;</span>" +
                                                                          (
                                                                            AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.GetJumlahNoReadOnly(
                                                                                (
                                                                                    AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()
                                                                                    ? Eval("KodeKelasDet").ToString()
                                                                                    : AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.QS.GetKelas()
                                                                                ),
                                                                                Eval("Kode").ToString()
                                                                            ) == 0
                                                                            ? "<label title=\" Upload \" style=\"font-weight: normal; font-size: small; color: #bfbfbf; margin-top: 10px;\">" +
                                                                                "&nbsp;" +
                                                                                "<i class=\"fa fa-upload\"></i>" +
                                                                                "&nbsp;" +
                                                                                "Upload" +
                                                                                "&nbsp;" +
                                                                              "</label>"
                                                                            : "<label title=\" Upload \" onclick=\"ShowProgress(true); " + (AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit()  ? txtKelasDet.ClientID + ".value = '" + Eval("KodeKelasDet").ToString() + "'; " : "") + txtID.ClientID + ".value = '" + Eval("Kode").ToString() + "';" + btnShowConfirmPostingByKelas.ClientID + ".click();\" style=\"cursor: pointer; font-weight: normal; font-size: small; color: green; margin-top: 10px;\">" +
                                                                                "&nbsp;" +
                                                                                "<i class=\"fa fa-upload\"></i>" +
                                                                                "&nbsp;" +
                                                                                "Upload" +
                                                                                "&nbsp;" +
                                                                              "</label>" 
                                                                          )
                                                                    )
                                                                %>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="2" style="text-align: center; padding: 10px;">
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

                                            <div id="div_periode_title" runat="server" class="content-header ui-content-header" 
                                                style="background-color: #a91212;
                                                        box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
                                                        background-image: none; 
                                                        color: white;
                                                        display: block;
                                                        z-index: 5;
                                                        position: fixed; bottom: 28px; right: 25px; width: 420px; border-radius: 25px;
                                                        padding: 8px; margin: 0px;">                	
                                                <div style="padding-left: 15px; font-weight: bold; padding-top: 6px; padding-bottom: 6px;">
                                                    <asp:Literal runat="server" ID="ltrPeriode"></asp:Literal>
                                                </div>
                                            </div>

                                            <div class="content-header ui-content-header" 
                                                style="background-color: white;
                                                        box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
                                                        background-image: none; 
                                                        color: white;
                                                        display: block;
                                                        z-index: 5;
                                                        position: fixed; bottom: 28px; right: 25px; width: 300px; border-radius: 25px;
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
                                                        <asp:LinkButton OnCLientClick="ShowProgress(true);" ToolTip=" Tampilan Data " runat="server" ID="btnDoTampilanData" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" style="background-color: #9f5000; color: white;" OnClick="btnDoTampilanData_Click">
                                                            <span class="fbtn-text fbtn-text-left">Tampilan Data</span>
                                                            <i class="fa fa-eye" style="color: white;"></i>
                                                        </asp:LinkButton>
			                                        </div>
		                                        </div>
	                                        </div>

                                        </asp:View>
                                        <asp:View runat="server" ID="vDesain">

                                            <div style="padding: 0px; margin: 0px; background-color: #e3e3e3;">
                                                <div class="row">
                                                    <div class="col-lg-12" style="text-align: center; padding: 15px; color: grey; padding-left: 30px; padding-right: 30px;">
                                                        <asp:Literal runat="server" ID="ltrInfoDesain"></asp:Literal>
                                                    </div>
                                                </div>
                                            </div>
                                            <div style="padding: 0px; margin: 0px;">
                                                <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                    <div class="col-lg-12" style="padding: 0px;">
                                                        <div style="display: none;">
                                                            Nama Siswa : <span style="font-weight: bold;">
                                                                            <asp:Literal runat="server" ID="lblNamaSiswa"></asp:Literal>
                                                                         </span>
                                                            Kelas : <span style="font-weight: bold;">
                                                                        <asp:Literal runat="server" ID="lblKelasSiswa"></asp:Literal>
                                                                    </span>
                                                        </div>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
					                                        <div class="media" 
							                                        style="margin-top: 0px;
                                                                        margin-bottom: 0px;
								                                        padding-left: 8px; 
								                                        border-left-width: 4px;
								                                        background-color: #EEEEEE;
								                                        padding: 10px;">
						                                        <div class="media-object margin-right-sm pull-left">
							                                        <span class="icon icon-lg text-brand-accent" style="color: #6A6F75;">info_outline</span>
						                                        </div>
						                                        <div class="media-inner">
							                                        <span style="color: #6A6F75;">
								                                        <span style="font-weight: bold">Pencapaian Perkembangan</span>
							                                        </span>                                                                
						                                        </div>
					                                        </div>

                                                            <asp:ListView ID="lvDesain" DataSourceID="sql_ds_desain" runat="server" OnPagePropertiesChanging="lvDesain_PagePropertiesChanging">
                                                                <LayoutTemplate>                                                                
							                                        <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
								                                        <tbody>     
                                                                            <tr id="itemPlaceholder" runat="server"></tr>
                                                                        </tbody>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="<%# (Container.DisplayIndex % 2 == 0 ? "standardrow" : "oddrow") %>">
                                                                        <td 
                                                                            <%# 
                                                                                
                                                                                (
                                                                                    (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                        AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.Rekomendasi
                                                                                    ? "colspan=\"" +  
                                                                                      AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.GetColspanKriteriaPenilaian(
                                                                                            txtID.Value, 1
                                                                                      ) +  
                                                                                      "\" "
                                                                                    : ""
                                                                                )
                                                                            %>
                                                                            style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                                                <div "<%#                                                                                 
                                                                                        (
                                                                                            (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.Rekomendasi
                                                                                            ? " class=\"col-md-12\" "
                                                                                            : " class=\"col-md-8\" "
                                                                                        )
                                                                                     %>" 
                                                                                    style="padding: 0px; padding-right: 5px;">
                                                                                    <table style="margin: 0px; width: 100%; box-shadow: none;">
                                                                                        <tr>
                                                                                            <td style="width: 30px; padding: 0px; background: transparent;">

                                                                                                <label for="chk_desain_<%# Eval("Kode").ToString().Replace("-", "_") %>" style="color: grey; cursor: pointer;
                                                                                                              text-transform: none; 
                                                                                                              text-decoration: none; 
                                                                                                              <%# 
                                                                                                                  (
                                                                                                                    (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                        AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.KategoriPencapaian
                                                                                                                    ? "font-weight: bold; margin-right: 15px;"
                                                                                                                    : (
                                                                                                                        (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                            AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.SubKategoriPencapaian
                                                                                                                        ? "font-weight: bold; margin-left: 5px; margin-right: 15px;"
                                                                                                                        : (
                                                                                                                            (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                                AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.PoinKategoriPencapaian
                                                                                                                            ? "font-weight: normal; margin-left: 15px; margin-right: 15px;"
                                                                                                                            : ""
                                                                                                                          )
                                                                                                                      )
                                                                                                                  )
                                                                                                              %>">
                                                                                                    <%# 
                                                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Poin").ToString())
                                                                                                    %>
                                                                                                </label>

                                                                                            </td>
                                                                                            <td style="padding: 0px; background: transparent; font-weight: normal;">

                                                                                                <label for="chk_desain_<%# Eval("Kode").ToString().Replace("-", "_") %>" style="color: grey; cursor: pointer;
                                                                                                              text-transform: none; 
                                                                                                              text-decoration: none;
                                                                                                              <%# 
                                                                                                                  (
                                                                                                                    (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                        AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.KategoriPencapaian
                                                                                                                    ? "font-weight: bold;"
                                                                                                                    : (
                                                                                                                        (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                            AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.SubKategoriPencapaian
                                                                                                                        ? "font-weight: bold;"
                                                                                                                        : (
                                                                                                                            (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                                AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.PoinKategoriPencapaian
                                                                                                                            ? "font-weight: normal;"
                                                                                                                            : (
                                                                                                                                (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                                    AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.Rekomendasi
                                                                                                                                ? "font-weight: bold;"
                                                                                                                                : ""
                                                                                                                              )
                                                                                                                          )
                                                                                                                      )
                                                                                                                  )
                                                                                                              %>">
                                                                                                    <%# 
                                                                                                        AI_ERP.Application_Libs.Libs.GetHTMLNoParagraphDiAwal(Eval("NamaKomponen").ToString())
                                                                                                    %>
                                                                                                </label>
                                                                                                <%# 
                                                                                
                                                                                                    (
                                                                                                        (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) ==
                                                                                                            AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.Rekomendasi
                                                                                                        ?
                                                                                                            (

                                                                                                                AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.IsReadonly(
                                                                                                                    txtTahunAjaran.Value,
                                                                                                                    txtSemester.Value,
                                                                                                                    txtKelasDet.Value,
                                                                                                                    txtIDSiswa.Value,
                                                                                                                    txtJenisRapor.Value
                                                                                                                ) ? "<div style=\"padding: 10px; border-style: solid; border-width: 1px; border-color: #dfdfdf; background-color: white; min-width: 100px; min-height: 100px; color: black; font-family: Times New Roman; font-size: 12pt;\">" +
                                                                                                                        AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.GetHTMLDeskripsiPenilaian(
                                                                                                                                txtTahunAjaran.Value,
                                                                                                                                txtSemester.Value,
                                                                                                                                txtKelasDet.Value,
                                                                                                                                txtIDSiswa.Value,
                                                                                                                                txtID.Value,
                                                                                                                                Eval("Kode").ToString(),
                                                                                                                                (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen"))
                                                                                                                            ) +
                                                                                                                    "</div>"
                                                                                                                  : "<div name=\"txt_rekomendasi[]\" class=\"txt_rekomendasi_" + Eval("Kode").ToString().Replace("-", "_") + "\" "+
                                                                                                                         "id=\"txt_rekomendasi_" + Eval("Kode").ToString().Replace("-", "_") + "\" " +
                                                                                                                         "onclick='txtPoinPenilaian().value = \"" + Eval("Kode").ToString() + "\"; " + txtJenisRekomendasi.ClientID + ".value =\"0\" ;" + txtIDRekomendasi.ClientID + ".value = \"" + Eval("Kode").ToString() + "\"; " + btnShowDeskripsi.ClientID + ".click();' " +
                                                                                                                         "style=\"cursor: pointer; background-color: white; border-style: solid; border-width: 1px; border-color: #E3E3E3; width: 100%; min-height: 100px; margin-left: 0px;\">" +
                                                                                                                         "<table style=\"margin: 0px; width: 100%;\">" +
                                                                                                                            "<tr>" +
                                                                                                                                "<td style=\"width: 20px; background-color: white; padding: 10px;\">" +
                                                                                                                                    "<i class=\"fa fa-file-text-o\"></i>" +
                                                                                                                                "</td>" +
                                                                                                                                "<td style=\"background-color: white; padding: 10px; padding-left: 0px; color: black; font-family: Times New Roman; font-size: 12pt;\">" +
                                                                                                                                    AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.GetHTMLDeskripsiPenilaian(
                                                                                                                                        txtTahunAjaran.Value,
                                                                                                                                        txtSemester.Value,
                                                                                                                                        txtKelasDet.Value,
                                                                                                                                        txtIDSiswa.Value,
                                                                                                                                        txtID.Value,
                                                                                                                                        Eval("Kode").ToString(),
                                                                                                                                        (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen"))
                                                                                                                                    ) +
                                                                                                                                "</td>" +
                                                                                                                            "</tr>" +
                                                                                                                         "</table>" +
                                                                                                                    "</div>" +
                                                                                                                    "<input name=\"txt_rekomendasi_val[]\" " +
                                                                                                                         "id=\"txt_rekomendasi_val_" + Eval("Kode").ToString().Replace("-", "_") + "\" " +
                                                                                                                         "value='" +
                                                                                                                                AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.GetHTMLDeskripsiPenilaian(
                                                                                                                                        txtTahunAjaran.Value,
                                                                                                                                        txtSemester.Value,
                                                                                                                                        txtKelasDet.Value,
                                                                                                                                        txtIDSiswa.Value,
                                                                                                                                        txtID.Value,
                                                                                                                                        Eval("Kode").ToString(),
                                                                                                                                        (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen"))
                                                                                                                                    )
                                                                                                                                + "' " +
                                                                                                                         "type=\"hidden\" />"
                                                                                                            )

                                                                                                        : ""
                                                                                                    )
                                                                                                %>

                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>  
                                                                                <div "<%#                                                                                 
                                                                                        (
                                                                                            (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.Rekomendasi
                                                                                            ? ""
                                                                                            : " class=\"col-md-4\" "
                                                                                        )
                                                                                     %>" 
                                                                                     style="padding-right: 0px;
                                                                                                <%#                                                                                 
                                                                                                    (
                                                                                                        (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                            AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.Rekomendasi
                                                                                                        ? " padding-bottom: 15px; "
                                                                                                        : ""
                                                                                                    )
                                                                                                %>
                                                                                            ">
                                                                                    <table style="width: 100%; margin: 0px;
                                                                                                  <%# 
                                                                                                    (
                                                                                                    (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                        AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.KategoriPencapaian
                                                                                                    ? " display: none; "
                                                                                                    : (
                                                                                                        (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                            AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.SubKategoriPencapaian
                                                                                                        ? " display: none; "
                                                                                                        : (
                                                                                                            (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.PoinKategoriPencapaian
                                                                                                            ? ""
                                                                                                            : ""
                                                                                                            )
                                                                                                        )
                                                                                                    )
                                                                                                %>
                                                                                                 ">
                                                                                        <tr>

                                                                                            <%# 
                                                                                                AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.GetHTMLRdoKriteriaPenilaian(
                                                                                                        txtTahunAjaran.Value,
                                                                                                        txtSemester.Value,
                                                                                                        txtKelasDet.Value,
                                                                                                        txtIDSiswa.Value,
                                                                                                        txtID.Value,
                                                                                                        Eval("Kode").ToString(),
                                                                                                        Eval("Rel_Rapor_Kriteria").ToString(),
                                                                                                        (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen"))
                                                                                                    )
                                                                                            %>

                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </div>                                                                            
                                                                        </td>                                                                        
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <EmptyDataTemplate>                                                                
                                                                    <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td style="text-align: center; padding: 20px; background-color: #F9F9F9; color: #B0B0B0; font-weight: bold;">
                                                                                    ..:: Data Kosong ::..
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </EmptyDataTemplate>
                                                            </asp:ListView>
                                                            <asp:SqlDataSource ID="sql_ds_desain" runat="server"></asp:SqlDataSource>

                                                            <div id="div_body_nilai_rapor" runat="server"
                                                                 class="content-header ui-content-header" 
                                                                 style="background-color: #3367D6;
                                                                        box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
                                                                        background-image: none; 
                                                                        color: white;
                                                                        display: block;
                                                                        z-index: 5;
                                                                        position: fixed; bottom: 50px; right: -25px; width: 320px; border-radius: 25px;
                                                                        padding: 12px; 
                                                                        padding-top: 0px;
                                                                        padding-left: 0px;
                                                                        margin: 0px;">

                                                                <div id="div_header_nilai_rapor"  runat="server" style="width: 100%; background-color: #295BC8; padding: 10px;">
                                                                    <span class="icon icon-lg text-brand-accent" style="color: white;">info_outline</span>
                                                                    &nbsp;
                                                                    <span style="font-weight: bold; color: white;">Nilai Rapor</span>
                                                                    <asp:Literal runat="server" ID="ltrHeaderNilaiRapor"></asp:Literal>
                                                                </div>

                                                                <div class="tile-wrap" style="margin-top: 0px; margin-bottom: 0px; padding-left: 10px;">
							                                        <div class="tile" style="background: transparent; box-shadow: none;">
								                                        <div class="tile-side pull-left">
									                                        
                                                                            <img
                                                                                src="<%=
                                                                                        ResolveUrl(AI_ERP.Application_Libs.Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + "xxx" + ".jpg"))
                                                                                     %>"
                                                                                style="height: 60px; width: 60px; border-radius: 100%; margin-bottom: 90px;" />

								                                        </div>
								                                        <div class="tile-inner">

									                                        <span style="font-weight: bold; font-size: small; color: white;">
                                                                                <asp:Literal runat="server" ID="lblNamaSiswaInfo"></asp:Literal>
                                                                            </span>
                                                                            <br />
                                                                            <span style="font-weight: normal; font-size: small; color: white;">
                                                                                <asp:Literal runat="server" ID="lblKelasSiswaInfo"></asp:Literal>
                                                                            </span>
                                                                            <br />
                                                                            <span style="font-weight: normal; font-size: x-small; color: white;">
                                                                                <asp:Literal runat="server" ID="lblInfoPeriode"></asp:Literal>
                                                                            </span>
                                                                            <br />
                                                                            <label style="font-weight: normal; font-size: x-small; color: white; margin-top: 5px;">
                                                                                Tinggi Badan&nbsp;(Cm)&nbsp;&nbsp;<br />
                                                                                <asp:TextBox onblur="SavePertumbuhanFisik();" runat="server" ID="txtTinggiBadan" style="background: transparent; border-style: none; border-bottom-style: dashed; border-bottom-width: 1px; padding: 5px; font-weight: bold; font-size: small; outline: none; border-bottom-color: #2bb4c2;"></asp:TextBox>
                                                                            </label>
                                                                            &nbsp;&nbsp;
                                                                            <label style="font-weight: normal; font-size: x-small; color: white;">
                                                                                Berat Badan&nbsp;(Kg)&nbsp;&nbsp;<br />
                                                                                <asp:TextBox onblur="SavePertumbuhanFisik();" runat="server" ID="txtBeratBadan" style="background: transparent; border-style: none; border-bottom-style: dashed; border-bottom-width: 1px; padding: 5px; font-weight: bold; font-size: small; outline: none; border-bottom-color: #2bb4c2;"></asp:TextBox>
                                                                            </label>     
                                                                            &nbsp;&nbsp;
                                                                            <label style="font-weight: normal; font-size: x-small; color: white;">
                                                                                Lingkar Kepala&nbsp;(Cm)&nbsp;&nbsp;<br />
                                                                                <asp:TextBox onblur="SavePertumbuhanFisik();" runat="server" ID="txtLingkarKepala" style="background: transparent; border-style: none; border-bottom-style: dashed; border-bottom-width: 1px; padding: 5px; font-weight: bold; font-size: small; outline: none; border-bottom-color: #2bb4c2;"></asp:TextBox>
                                                                            </label>                                                                           
                                                                            &nbsp;&nbsp;
                                                                            <label style="font-weight: normal; font-size: x-small; color: white;">
                                                                                Usia&nbsp;&nbsp;<br />
                                                                                <asp:TextBox onblur="SavePertumbuhanFisik();" runat="server" ID="txtUsia" style="background: transparent; border-style: none; border-bottom-style: dashed; border-bottom-width: 1px; padding: 5px; font-weight: bold; font-size: small; outline: none; border-bottom-color: #2bb4c2;"></asp:TextBox>
                                                                            </label>                                                                            

                                                                            <label style="display: none; padding: 2px; background-color: #0053a9; cursor: pointer; color: #bebebe; font-weight: bold; font-size: x-small; margin-top: 10px; border-style: solid; border-color: #003ab8;">
                                                                                &nbsp;&nbsp;
                                                                                <i class="fa fa-edit"></i>
                                                                                &nbsp;
                                                                                Ubah Informasi
                                                                                &nbsp;&nbsp;
                                                                            </label>

								                                        </div>
							                                        </div>
						                                        </div>

                                                                <div style="color: yellow; padding-left: 10px;">
                                                                    <a class="btn btn-flat waves-attach waves-effect" 
                                                                       onclick="FirstSiswa()"
                                                                       title=" Data Siswa Pertama "
                                                                       style="padding-left: 4px; padding-right: 4px; text-transform: none; font-weight: bold; color: white; border-style: solid; border-width: 0px; border-color: lightskyblue; padding-top: 3px; padding-bottom: 3px; font-size: small; border-radius: 0px;">
                                                                        <i class="fa fa-backward"></i>
                                                                    </a>
                                                                    <a class="btn btn-flat waves-attach waves-effect" 
                                                                       onclick="PrevSiswa()"
                                                                       title=" Data Siswa Sebelumnya "
                                                                       style="padding-left: 4px; padding-right: 4px; text-transform: none; font-weight: bold; color: white; border-style: solid; border-width: 0px; border-color: lightskyblue; padding-top: 3px; padding-bottom: 3px; font-size: small; border-radius: 0px;">
                                                                        <i class="fa fa-arrow-left"></i>
                                                                    </a>
                                                                    <a class="btn btn-flat waves-attach waves-effect" 
                                                                       onclick="NextSiswa()"
                                                                       title=" Data Siswa Berikutnya "
                                                                       style="padding-left: 4px; padding-right: 4px; text-transform: none; font-weight: bold; color: white; border-style: solid; border-width: 0px; border-color: lightskyblue; padding-top: 3px; padding-bottom: 3px; font-size: small; border-radius: 0px;">
                                                                        <i class="fa fa-arrow-right"></i>
                                                                    </a>
                                                                    <a class="btn btn-flat waves-attach waves-effect" 
                                                                       onclick="LastSiswa()"
                                                                       title=" Data Siswa Terakhir "
                                                                       style="padding-left: 4px; padding-right: 4px; text-transform: none; font-weight: bold; color: white; border-style: solid; border-width: 0px; border-color: lightskyblue; padding-top: 3px; padding-bottom: 3px; font-size: small; border-radius: 0px;">
                                                                        <i class="fa fa-forward"></i>
                                                                    </a>
                                                                    <a class="btn btn-flat waves-attach waves-effect" 
                                                                       title=" Cari Siswa "
                                                                       style="display: none; padding-left: 4px; padding-right: 4px; text-transform: none; font-weight: bold; color: lightskyblue; border-style: solid; border-width: 0px; border-color: lightskyblue; padding-top: 3px; padding-bottom: 3px; font-size: small; border-radius: 0px;">
                                                                        <i class="fa fa-search"></i>
                                                                        &nbsp;Cari
                                                                    </a>
                                                                    <a onclick="$('#ui_modal_list_siswa').modal({ backdrop: 'static', keyboard: false, show: true });"
                                                                       class="btn btn-flat waves-attach waves-effect" 
                                                                       title=" Pilih Siswa "
                                                                       style="text-transform: none; font-weight: bold; color: yellow; border-style: solid; border-width: 0px; border-color: lightskyblue; padding-top: 3px; padding-bottom: 3px; font-size: small; border-radius: 0px;">
                                                                        <i class="fa fa-user"></i>
                                                                        &nbsp;Pilih Siswa
                                                                    </a>

                                                                </div>                                                                
                                                                <br />
                                                            </div>

                                                            <div class="content-header ui-content-header" 
                                                                style="background-color: #E6E6E6;
                                                                        box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
                                                                        background-image: none; 
                                                                        color: white;
                                                                        display: block;
                                                                        z-index: 5;
                                                                        position: fixed; bottom: 26.5px; right: 25px; width: 270px; border-radius: 25px; border-top-left-radius: 0px;
                                                                        padding: 8px; margin: 0px;">
                	
                                                                <div style="padding-left: 15px;">
				                                                    <asp:DataPager ID="dpDesain" runat="server" PageSize="300" PagedControlID="lvDesain">
                                                                        <Fields>
                                                                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="btn-trans" ShowFirstPageButton="True" FirstPageText='&nbsp;<i class="fa fa-backward"></i>&nbsp;' ShowPreviousPageButton="True" PreviousPageText='&nbsp;<i class="fa fa-arrow-left"></i>&nbsp;' ShowNextPageButton="false" />
                                                                            <asp:TemplatePagerField>
                                                                                <PagerTemplate>
                                                                                    <label style="color: black; font-weight: normal; padding: 5px; border-style: solid; border-color: #F1F1F1; border-width: 1px; padding-left: 10px; padding-right: 10px; border-radius: 5px;">
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
                                                                                </PagerTemplate>
                                                                            </asp:TemplatePagerField>
                                                                        </Fields>
                                                                    </asp:DataPager>
                                                                </div>
		                                                    </div>    

                                                        </div>
				                                    </div>
                                                </div>

                                                <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                    <div class="col-lg-12" style="padding: 0px;">
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
					                                        <div class="media" 
							                                        style="margin-top: 0px;
                                                                        margin-bottom: 0px;
								                                        padding-left: 8px; 
								                                        border-left-width: 4px;
								                                        background-color: #EEEEEE;
								                                        padding: 10px;">
						                                        <div class="media-object margin-right-sm pull-left">
							                                        <span class="icon icon-lg text-brand-accent" style="color: #6A6F75;">info_outline</span>
						                                        </div>
						                                        <div class="media-inner">
							                                        <span style="color: #6A6F75;">
								                                        <span style="font-weight: bold">Program Ekstrakurikuler</span>
							                                        </span>                                                                
						                                        </div>
					                                        </div>

                                                            <asp:ListView ID="lvEkskul" DataSourceID="sql_ds_ekskul" runat="server">
                                                                <LayoutTemplate>                                                                
							                                        <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
								                                        <tbody>     
                                                                            <tr id="itemPlaceholder" runat="server"></tr>
                                                                        </tbody>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="<%# (Container.DisplayIndex % 2 == 0 ? "standardrow" : "oddrow") %>">
                                                                        <td 
                                                                            <%# 
                                                                                
                                                                                (
                                                                                    (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                        AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.Rekomendasi
                                                                                    ? "colspan=\"" +  
                                                                                      AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.GetColspanKriteriaPenilaian(
                                                                                            txtID.Value, 1
                                                                                      ) +  
                                                                                      "\" "
                                                                                    : ""
                                                                                )
                                                                            %>
                                                                            style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                                                <div "<%#                                                                                 
                                                                                        (
                                                                                            (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.Rekomendasi
                                                                                            ? " class=\"col-md-12\" "
                                                                                            : " class=\"col-md-8\" "
                                                                                        )
                                                                                     %>" 
                                                                                     style="padding: 0px; padding-right: 5px;">
                                                                                    <table style="margin: 0px; width: 100%; box-shadow: none;">
                                                                                        <tr>
                                                                                            <td style="width: 30px; padding: 0px; background: transparent;">

                                                                                                <label for="chk_desain_<%# Eval("Kode").ToString().Replace("-", "_") %>" style="color: grey; cursor: pointer;
                                                                                                              text-transform: none; 
                                                                                                              text-decoration: none; 
                                                                                                              <%# 
                                                                                                                  (
                                                                                                                    (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                        AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.KategoriPencapaian
                                                                                                                    ? "font-weight: bold; margin-right: 15px;"
                                                                                                                    : (
                                                                                                                        (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                            AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.SubKategoriPencapaian
                                                                                                                        ? "font-weight: bold; margin-left: 5px; margin-right: 15px;"
                                                                                                                        : (
                                                                                                                            (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                                AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.PoinKategoriPencapaian
                                                                                                                            ? "font-weight: normal; margin-left: 15px; margin-right: 15px;"
                                                                                                                            : ""
                                                                                                                          )
                                                                                                                      )
                                                                                                                  )
                                                                                                              %>">
                                                                                                    <%# 
                                                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Poin").ToString())
                                                                                                    %>
                                                                                                </label>

                                                                                            </td>
                                                                                            <td style="padding: 0px; background: transparent; font-weight: normal;">

                                                                                                <label for="chk_desain_<%# Eval("Kode").ToString().Replace("-", "_") %>" style="color: grey; cursor: pointer;
                                                                                                              text-transform: none; 
                                                                                                              text-decoration: none;
                                                                                                              <%# 
                                                                                                                  (
                                                                                                                    (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                        AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.KategoriPencapaian
                                                                                                                    ? "font-weight: bold;"
                                                                                                                    : (
                                                                                                                        (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                            AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.SubKategoriPencapaian
                                                                                                                        ? "font-weight: bold;"
                                                                                                                        : (
                                                                                                                            (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                                AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.PoinKategoriPencapaian
                                                                                                                            ? "font-weight: normal;"
                                                                                                                            : (
                                                                                                                                (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                                    AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.Rekomendasi
                                                                                                                                ? "font-weight: bold;"
                                                                                                                                : ""
                                                                                                                              )
                                                                                                                          )
                                                                                                                      )
                                                                                                                  )
                                                                                                              %>">
                                                                                                    <%# 
                                                                                                        AI_ERP.Application_Libs.Libs.GetHTMLNoParagraphDiAwal(Eval("NamaKomponen").ToString())
                                                                                                    %>
                                                                                                </label>
                                                                                                <%# 
                                                                                                                                                                                    
                                                                                                    (
                                                                                                        (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) ==
                                                                                                            AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.Rekomendasi
                                                                                                        ? (

                                                                                                                //AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.IsReadonly(
                                                                                                                //    txtTahunAjaran.Value,
                                                                                                                //    txtSemester.Value,
                                                                                                                //    txtKelasDet.Value,
                                                                                                                //    txtIDSiswa.Value
                                                                                                                //) 
                                                                                                                true  
                                                                                                                  ? "<div style=\"padding: 10px; border-style: solid; border-width: 1px; border-color: #dfdfdf; background-color: white; color: black; min-height: 100px; font-family: Times New Roman; font-size: 12pt;\">" +
                                                                                                                        AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.GetHTMLDeskripsiPenilaian(
                                                                                                                                txtTahunAjaran.Value,
                                                                                                                                txtSemester.Value,
                                                                                                                                txtKelasDet.Value,
                                                                                                                                txtIDSiswa.Value,
                                                                                                                                txtID.Value,
                                                                                                                                Eval("Kode").ToString(),
                                                                                                                                (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen"))
                                                                                                                            ) +
                                                                                                                    "</div>"
                                                                                                                  : "<div name='txt_rekomendasi_ekskul[]' class='txt_rekomendasi_ekskul_" + Eval("Kode").ToString().Replace("-", "_") + "' "+
                                                                                                                             "id='txt_rekomendasi_ekskul_" + Eval("Kode").ToString().Replace("-", "_") + "' " +
                                                                                                                             "onclick='txtPoinPenilaian().value = \"" + Eval("Kode").ToString() + "\"; " + txtJenisRekomendasi.ClientID + ".value =\"1\" ;" + txtIDRekomendasi.ClientID + ".value = \"" + Eval("Kode").ToString() + "\"; " + btnShowDeskripsi.ClientID + ".click();' " +
                                                                                                                             "style=\"cursor: pointer; background-color: white; border-style: solid; border-width: 1px; border-color: #E3E3E3; width: 100%; min-height: 100px; margin-left: 0px;\">" +
                                                                                                                         "<table style=\"margin: 0px; width: 100%;\">" +
                                                                                                                            "<tr>" +
                                                                                                                                "<td style=\"width: 20px; background-color: white; padding: 10px;\">" +
                                                                                                                                    "<i class=\"fa fa-file-text-o\"></i>" +
                                                                                                                                "</td>" +
                                                                                                                                "<td style=\"background-color: white; padding: 10px; padding-left: 0px; color: black; font-family: Times New Roman; font-size: 12pt;\">" +
                                                                                                                                    AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.GetHTMLDeskripsiPenilaian(
                                                                                                                                        txtTahunAjaran.Value,
                                                                                                                                        txtSemester.Value,
                                                                                                                                        txtKelasDet.Value,
                                                                                                                                        txtIDSiswa.Value,
                                                                                                                                        txtID.Value,
                                                                                                                                        Eval("Kode").ToString(),
                                                                                                                                        (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen"))
                                                                                                                                    ) +
                                                                                                                                "</td>" +
                                                                                                                            "</tr>" +
                                                                                                                         "</table>" +
                                                                                                                    "</div>" +
                                                                                                                    "<input name='txt_rekomendasi_val_ekskul[]' " +
                                                                                                                             "id='txt_rekomendasi_val_ekskul_" + Eval("Kode").ToString().Replace("-", "_") + "' " +
                                                                                                                             "value='" +
                                                                                                                                    AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.GetHTMLDeskripsiPenilaian(
                                                                                                                                            txtTahunAjaran.Value,
                                                                                                                                            txtSemester.Value,
                                                                                                                                            txtKelasDet.Value,
                                                                                                                                            txtIDSiswa.Value,
                                                                                                                                            txtID.Value,
                                                                                                                                            Eval("Kode").ToString(),
                                                                                                                                            (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen"))
                                                                                                                                        )
                                                                                                                                    + "' " +
                                                                                                                             "type='hidden' />"
                                                                                                            )

                                                                                                        : ""
                                                                                                    )
                                                                                                %>

                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>  
                                                                                <div "<%#                                                                                 
                                                                                        (
                                                                                            (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.Rekomendasi
                                                                                            ? ""
                                                                                            : " class=\"col-md-4\" "
                                                                                        )
                                                                                     %>" 
                                                                                     style="padding-right: 0px;">
                                                                                    <table style="width: 100%; margin: 0px;
                                                                                                  <%# 
                                                                                                    (
                                                                                                    (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                        AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.KategoriPencapaian
                                                                                                    ? " display: none; "
                                                                                                    : (
                                                                                                        (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                            AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.SubKategoriPencapaian
                                                                                                        ? " display: none; "
                                                                                                        : (
                                                                                                            (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor.PoinKategoriPencapaian
                                                                                                            ? ""
                                                                                                            : ""
                                                                                                            )
                                                                                                        )
                                                                                                    )
                                                                                                %>
                                                                                                 ">
                                                                                        <tr>

                                                                                            <%# 
                                                                                                AI_ERP.Application_Modules.EDUCATION.Penilaian.KB.wf_NilaiRapor.GetHTMLRdoKriteriaPenilaian(
                                                                                                        txtTahunAjaran.Value,
                                                                                                        txtSemester.Value,
                                                                                                        txtKelasDet.Value,
                                                                                                        txtIDSiswa.Value,
                                                                                                        txtID.Value,
                                                                                                        Eval("Kode").ToString(),
                                                                                                        Eval("Rel_Rapor_Kriteria").ToString(),
                                                                                                        (AI_ERP.Application_Entities.Elearning.KB.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")),
                                                                                                        true,
                                                                                                        "1"
                                                                                                    )
                                                                                            %>

                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </div>                                                                            
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <EmptyDataTemplate>                                                                
                                                                    <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td style="text-align: center; padding: 20px; background-color: #F9F9F9; color: #B0B0B0; font-weight: bold;">
                                                                                    ..:: Data Kosong ::..
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </EmptyDataTemplate>
                                                            </asp:ListView>
                                                            <asp:SqlDataSource ID="sql_ds_ekskul" runat="server"></asp:SqlDataSource>
                                                        </div>
				                                    </div>
                                                </div>

                                            </div>           
                                            
                                            <div class="fbtn-container" id="div_button_input_rapor" runat="server">
		                                        <div class="fbtn-inner">
			                                        <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #a91212;" title=" Pilihan ">
                                                        <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                                                        <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                                                    </a>
                                                    <div class="fbtn-dropup" style="z-index: 999999;">
				                                        <div class="fbtn-dropup" style="z-index: 999999;">
                                                            <asp:LinkButton CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" id="btnDoShowPrintDitampilkan" title=" Cetak Nilai Ditampilkan " style="background-color: black; color: white;">
                                                                <span class="fbtn-text fbtn-text-left">Cetak Nilai Ditampilkan</span>
                                                                <i class="fa fa-print"></i>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton OnCLientClick="ShowProgress(true);" OnClick="btnDoPengaturan_Click" CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" id="btnDoPengaturan" title=" Pengaturan " style="background-color: #0e2557; color: white;">
                                                                <span class="fbtn-text fbtn-text-left">Pengaturan</span>
                                                                <i class="fa fa-cogs"></i>
                                                            </asp:LinkButton>
				                                            <asp:LinkButton OnClientClick="ShowProgress(true);" OnClick="btnDoPostingData_Click" CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" id="btnDoPostingData" title=" Upload Nilai " style="background-color: #601B70; color: white;">
                                                                <span class="fbtn-text fbtn-text-left">Upload Data</span>
                                                                <i class="fa fa-upload"></i>
                                                            </asp:LinkButton>                                                            
                                                            <asp:LinkButton OnCLientClick="ShowProgress(true);" ToolTip=" Kembali " runat="server" ID="btnDoKembali" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" style="background-color: #257228;" OnClick="btnDoKembali_Click">
                                                                <span class="fbtn-text fbtn-text-left">Kembali</span>
                                                                <i class="fa fa-arrow-left" style="color: white;"></i>
                                                            </asp:LinkButton>
			                                            </div>
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

            <div aria-hidden="true" class="modal fade" id="ui_modal_confirm_upload" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                        Upload Penilaian
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
                                                            <span style="font-weight: bold;">
                                                                Anda yakin akan melakukan upload data nilai?
                                                            </span>
                                                            <br /><br />
                                                            <span style="color: grey;">
                                                                Perhatian : 
                                                            </span>
                                                            <br />
                                                            <span style="font-weight: bold; color: darkorange;">
                                                                <i class="fa fa-exclamation-triangle"></i>
                                                                &nbsp;
                                                                Data yang sudah di-upload tidak bisa diubah lagi
                                                            </span>
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
                                <asp:LinkButton OnClientClick="HideModal(); ShowProgress(true);" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKPosting" OnClick="lnkOKPosting_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_list_siswa" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                        Pilih Siswa
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 5px;">
                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                <div class="col-xs-12">
                                                    <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-12">
                                                            <asp:Literal runat="server" ID="ltrListSiswa"></asp:Literal>
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
					        <p class="text-center">
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Tutup</a>
                                <br /><br />
					        </p>
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

            <div aria-hidden="true" class="modal fade" id="ui_modal_nilai_standar" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                        Pengaturan Nilai Standar
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
                                                            
                                                            Pilih kriteria penilaian yang akan dijadikan nilai standar atau nilai default :
                                                            <br /><br />
                                                            <asp:Literal runat="server" ID="ltrKriteriaPenilaian"></asp:Literal>

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
                            <div class="row" id="pb_proses_nilai_standar" style="display: none; margin-left: -24px; margin-right: -24px; background-color: #B50000; color: white;">
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

                            <div class="row" id="div_command_pengaturan" style="margin-left: 0px; margin-right: 0px;">
					            <p class="text-right">
                                    <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKPengaturanNilaiStandar" OnClientClick="ShowProcessPengaturan(true); ProcessPengaturan(); return false;" Text="  OK  "></asp:LinkButton>
                                    <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                    <br /><br />                              
					            </p>
                            </div>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_rekomendasi" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
		        <div class="modal-dialog" style="max-width: 800px;">
			        <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px; background-color: #F6F6F6;">
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
                                        Rekomendasi
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 0px;">
                                            <div class="row" style="margin-left: 0px; margin-right: 0px; padding: 0px; background-color: #F6F6F6;">
                                                <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px; background-color: #F6F6F6;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 0px; background-color: #F6F6F6;">
                                                        <asp:TextBox style="height: 300px;" runat="server" ID="txtRekomendasi" CssClass="mcetiny_rekomendasi"></asp:TextBox>
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
                                <asp:LinkButton 
                                    CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" 
                                    runat="server" 
                                    ID="lnkOKRekomendasi" 
                                    OnClientClick="HideModal(); ShowProgress(true);"
                                    OnClick="lnkOKRekomendasi_Click"
                                    style="background-color: #295BC8;">
                                    <span style="color: white">
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        OK
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                    </span>
                                </asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_periode" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Tampilan Data
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
                                <asp:LinkButton ValidationGroup="vldInput" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKPeriode" OnClick="lnkOKPeriode_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnShowNilaiSiswa" />
            <asp:PostBackTrigger ControlID="btnShowDesain" />
            <asp:PostBackTrigger ControlID="btnDoKembali" />    
            <asp:PostBackTrigger ControlID="btnRefresh" />                
        </Triggers>
    </asp:UpdatePanel>

    <iframe name="fra_download" id="fra_download" height="0" width="0" style="position: absolute; left: -1000px; top: -1000px;"></iframe>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
        function LoadTinyMCE(e, css_selector, txtidval) {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                elements : e,
                selector: "." + css_selector,
                theme: "modern",
                plugins: [
                    "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                    "searchreplace wordcount visualblocks visualchars code fullscreen",
                    "insertdatetime media nonbreaking save table contextmenu directionality",
                    "emoticons template paste textcolor tinyfilemanager.net"
                ],
                toolbar1: "bold italic underline | forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | undo redo",
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
                        if (txtPoinPenilaian() !== null && txtPoinPenilaian() !== undefined
                        ) {
                            txtPoinPenilaian().value = txtidval.replaceAll("txt_rekomendasi_val_ekskul_", "");
                            txtPoinPenilaian().value = txtPoinPenilaian().value.replaceAll("txt_rekomendasi_val_", "");
                            txtPoinPenilaian().value = txtPoinPenilaian().value.replaceAll("_", "-");

                            document.getElementById(txtidval).value = ed.getContent();
                            SaveNilaiRapor(ed.getContent().replaceAll("#", "<%= AI_ERP.Application_Libs.Constantas.HASHTAG_REP %>").replaceAll("\n", ""));
                        }                        
                    });
                    ed.on('focusout', function (e) {
                        if (txtPoinPenilaian() !== null && txtPoinPenilaian() !== undefined
                        ) {
                            txtPoinPenilaian().value = txtidval.replaceAll("txt_rekomendasi_val_ekskul_", "");
                            txtPoinPenilaian().value = txtPoinPenilaian().value.replaceAll("txt_rekomendasi_val_", "");
                            txtPoinPenilaian().value = txtPoinPenilaian().value.replaceAll("_", "-");

                            document.getElementById(txtidval).value = ed.getContent();
                            SaveNilaiRapor(ed.getContent().replaceAll("#", "<%= AI_ERP.Application_Libs.Constantas.HASHTAG_REP %>").replaceAll("\n", ""));
                        }                        
                    });
                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
        }
    </script>

    <script type="text/javascript">
        RenderDropDownOnTables();
        InitModalFocus();
    </script>
</asp:Content>
