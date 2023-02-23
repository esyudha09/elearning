<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="wf.DesainRapor.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<%@ Register TagPrefix="ucl" TagName="AutocompletePegawai" Src="~/Application_Controls/AutocompletePegawai/AutocompletePegawai.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function HideModal(){
            $('#ui_modal_input_data').modal('hide');
            $('#ui_modal_confirm_hapus').modal('hide');   
            $('#ui_modal_kategori_pencapaian').modal('hide');   
            $('#ui_modal_sub_kategori_pencapaian').modal('hide');   
            $('#ui_modal_poin_kategori_pencapaian').modal('hide');   
            $('#ui_modal_kriteria_pencapaian').modal('hide');   
            $('#ui_modal_item_penilaian').modal('hide');    
            $('#ui_modal_hapus_item_penilaian').modal('hide');        
            $('#ui_modal_posting').modal('hide');    
            $('#ui_modal_pengaturan_item_penilaian').modal('hide');       
            $('#ui_modal_list_pilih_ekskul').modal('hide');   
            $('#ui_modal_proses').modal('hide');               

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
                case "<%= JenisAction.Add %>":
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.AddWithMessage %>":
                    HideModal();
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
                    ShowProgress(false);
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmHapus %>":
                    ShowProgress(false);
                    $('#ui_modal_confirm_hapus').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmPosting %>":
                    ShowProgress(false);
                    $('#ui_modal_posting').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowAddEkskul %>":
                    ShowProgress(false);
                    $('#ui_modal_list_pilih_ekskul').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowPengaturanItemPenilaian %>":
                    ShowProgress(false);
                    $('#ui_modal_pengaturan_item_penilaian').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowInputDesainKategoriPencapaian %>":
                case "<%= JenisAction.DoShowEditKategoriPencapaian %>":
                    ShowProgress(false);
                    LoadTinyMCEKategoriPencapaian();
                    $('#ui_modal_kategori_pencapaian').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowInputDesainSubKategoriPencapaian %>":
                case "<%= JenisAction.DoShowEditSubKategoriPencapaian %>":
                    ShowProgress(false);
                    LoadTinyMCESubKategoriPencapaian();
                    $('#ui_modal_sub_kategori_pencapaian').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;                    
                case "<%= JenisAction.DoShowInputDesainPoinKategoriPencapaian %>":
                case "<%= JenisAction.DoShowEditPoinKategoriPencapaian %>":
                    ShowProgress(false);
                    LoadTinyMCEPoinKategoriPencapaian();
                    $('#ui_modal_poin_kategori_pencapaian').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;                    
                case "<%= JenisAction.DoShowKriteriaPencapaian %>":
                    ShowProgress(false);
                    $('#ui_modal_kriteria_pencapaian').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoUpdatePoinPenilaian %>":
                    HideModal();
                    setTimeout(function(){
                        LoadTinyMCEPoinKategoriPencapaian();
                        $('#ui_modal_poin_kategori_pencapaian').modal({ backdrop: 'static', keyboard: false, show: true });
                    }, 10);
                    break;   
                case "<%= JenisAction.DoUpdateUrut %>":
                    HideModal();
                    SetSelectedItemPenilaian();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah diupdate',
                        show: function () {
                            snackbarText++;
                        }
                    });
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
                case "<%= JenisAction.DoPosting %>":
                    HideModal();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah diposting',
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

            <%= txtGuru1.NamaClientID %>_SHOW_AUTOCOMPLETE();
            <%= txtGuru2.NamaClientID %>_SHOW_AUTOCOMPLETE();
            <%= txtGuru3.NamaClientID %>_SHOW_AUTOCOMPLETE();
            
            Sys.Browser.WebKit = {};
            if (navigator.userAgent.indexOf('WebKit/') > -1) {
                Sys.Browser.agent = Sys.Browser.WebKit;
                Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
                Sys.Browser.name = 'WebKit';
            }
        }

        function InitModalFocus(){
            $('#ui_modal_input_data').on('shown.bs.modal', function () {
                <%= txtTahunPelajaran.ClientID %>.focus();
            });
            $('#ui_modal_kategori_pencapaian').on('shown.bs.modal', function () {
                <%= txtPoinItemKategoriPencapaian.ClientID %>.focus();
            });
            $('#ui_modal_sub_kategori_pencapaian').on('shown.bs.modal', function () {
                <%= txtPoinItemSubKategoriPencapaian.ClientID %>.focus();
            });
            $('#ui_modal_poin_kategori_pencapaian').on('shown.bs.modal', function () {
                <%= txtPoinItemPoinKategoriPencapaian.ClientID %>.focus();
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

        function ShowTambahItem(){
            HideModal();
            lbl_modal_item_penilaian.innerHTML = "Tambah";
            <%= txtIDJenisInsert.ClientID %>.value = '0'; 

            var txt_list_item = document.getElementById("<%= txtListIDItemPenilaian.ClientID %>");
            var txt_sel_item = document.getElementById("<%= txtSelIDItemPenilaian.ClientID %>");
            var arr_item = document.getElementsByName("chk_desain[]");

            if(
                txt_list_item != null && txt_list_item != undefined &&
                txt_sel_item != null && txt_sel_item != undefined &&
                arr_item.length > 0
            ){
                txt_list_item.value = "";
                for (var i = 0; i < arr_item.length; i++) {
                    txt_list_item.value += arr_item[i].value + ";";
                    if(arr_item[i].value === txt_sel_item.value){
                        txt_list_item.value += "{{id}}" + ";";
                    }
                }
            }
            
            $('#ui_modal_item_penilaian').modal({ backdrop: 'static', keyboard: false, show: true });
        }

        function ShowSisipkanItem(){
            HideModal();
            lbl_modal_item_penilaian.innerHTML = "Sisipkan";
            <%= txtIDJenisInsert.ClientID %>.value = '1'; 

            var txt_list_item = document.getElementById("<%= txtListIDItemPenilaian.ClientID %>");
            var txt_sel_item = document.getElementById("<%= txtSelIDItemPenilaian.ClientID %>");
            var arr_item = document.getElementsByName("chk_desain[]");

            if(
                txt_list_item != null && txt_list_item != undefined &&
                txt_sel_item != null && txt_sel_item != undefined &&
                arr_item.length > 0
            ){
                txt_list_item.value = "";
                for (var i = 0; i < arr_item.length; i++) {
                    if(arr_item[i].value === txt_sel_item.value){
                        txt_list_item.value += "{{id}}" + ";";
                    }
                    txt_list_item.value += arr_item[i].value + ";";
                }
            }

            $('#ui_modal_item_penilaian').modal({ backdrop: 'static', keyboard: false, show: true });
        }

        function ParseSisipkanItemKriteria(){
            <%= txtIDJenisInsert.ClientID %>.value = '1'; 

            var txt_list_item = document.getElementById("<%= txtListIDItemPenilaian.ClientID %>");
            var txt_sel_item = document.getElementById("<%= txtSelIDItemPenilaian.ClientID %>");
            var arr_item = document.getElementsByName("chk_desain[]");

            if(
                txt_list_item != null && txt_list_item != undefined &&
                txt_sel_item != null && txt_sel_item != undefined &&
                arr_item.length > 0
            ){
                txt_list_item.value = "";
                for (var i = 0; i < arr_item.length; i++) {
                    if(arr_item[i].value === txt_sel_item.value){
                        txt_list_item.value += "{{id}}" + ";";
                    }
                    txt_list_item.value += arr_item[i].value + ";";
                }
            }
        }

        function ParseTambahItemkriteria(){
            <%= txtIDJenisInsert.ClientID %>.value = '0'; 

            var txt_list_item = document.getElementById("<%= txtListIDItemPenilaian.ClientID %>");
            var txt_sel_item = document.getElementById("<%= txtSelIDItemPenilaian.ClientID %>");
            var arr_item = document.getElementsByName("chk_desain[]");

            if(
                txt_list_item != null && txt_list_item != undefined &&
                txt_sel_item != null && txt_sel_item != undefined &&
                arr_item.length > 0
            ){
                txt_list_item.value = "";
                for (var i = 0; i < arr_item.length; i++) {
                    txt_list_item.value += arr_item[i].value + ";";
                    if(arr_item[i].value === txt_sel_item.value){
                        txt_list_item.value += "{{id}}" + ";";
                    }
                }
            }
        }

        function ParseSelIDItemPenilaian(){
            var chk = document.getElementsByName("chk_desain[]");
            if(chk.length > 0){
                var txt_id_item = document.getElementById("<%= txtIDItemPenilaian.ClientID %>");
                if(txt_id_item != null && txt_id_item != undefined){
                    txt_id_item.value = "";
                    for (var i = 0; i < chk.length; i++) {
                        if(chk[i].checked){
                            txt_id_item.value += chk[i].value + ";";
                        }
                    }
                }
            }
        }

        function ShowConfirmHapusItemPenilaian(){
            var ada_check = false;
            var chk = document.getElementsByName("chk_desain[]");
            if(chk.length > 0){
                for (var i = 0; i < chk.length; i++) {
                    if(chk[i].checked){
                        ada_check = true;
                        break;
                    }
                }
            }
            if(ada_check){
                $('#ui_modal_hapus_item_penilaian').modal({ backdrop: 'static', keyboard: false, show: true });            
            }
            else {
                HideModal();
                $('body').snackbar({
                    alive: 2000,
                    content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Anda belum memilih data yang akan dihapus',
                    show: function () {
                        snackbarText++;
                    }
                });
            }
        }

        function SetSelectedItemPenilaian(){
            var chk = document.getElementsByName("chk_desain[]");
            var txt_sel_id_item = document.getElementById("<%= txtSelIDItemPenilaian.ClientID %>");
            if(chk.length > 0 && txt_sel_id_item != null && txt_sel_id_item != undefined){
                var arr_kode = txt_sel_id_item.value.split(";");
                if(arr_kode.length > 0){
                    for (var i = 0; i < chk.length; i++) {
                        for (var j = 0; j < arr_kode.length; j++) {
                            if(arr_kode[j] == chk[i].value) {
                                chk[i].checked = true; 
                                break;
                            }
                        }
                    }
                }
            }            
        }

        function stopRKey(evt) {
            var evt = (evt) ? evt : ((event) ? event : null);
            var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
            if ((evt.keyCode == 13) && (node.type == "text")) { return false; }
        }
        document.onkeypress = stopRKey;

        function SetMoveUpItemPenilaian(){
            var arr_item = document.getElementsByName("chk_desain[]");
            var txt_id_item = document.getElementById("<%= txtListIDItemPenilaian.ClientID %>");
            var txt_id_sel_item = document.getElementById("<%= txtSelIDItemPenilaian.ClientID %>");
            var id_sel = 0;
            var s_id_sel = "";
            if(txt_id_item != null && txt_id_item != undefined && 
               txt_id_sel_item != null && txt_id_sel_item != undefined){
                if(arr_item.length > 0){
                    for (var i = 0; i < arr_item.length; i++) {
                        if(txt_id_sel_item.value === arr_item[i].value){
                            id_sel = i;
                            s_id_sel = arr_item[i].value;
                            break;
                        }
                    }
                    if(id_sel < 1) id_sel = 1;
                    txt_id_item.value = "";
                    for (var i = 0; i < arr_item.length; i++) {
                        if(i === (id_sel - 1)){
                            txt_id_item.value += s_id_sel + ";";
                        }
                        if(arr_item[i].value != s_id_sel) txt_id_item.value += (arr_item[i].value + ";");
                    }
                }
            }
        }

        function SetMoveDownItemPenilaian(){
            var arr_item = document.getElementsByName("chk_desain[]");
            var txt_id_item = document.getElementById("<%= txtListIDItemPenilaian.ClientID %>");
            var txt_id_sel_item = document.getElementById("<%= txtSelIDItemPenilaian.ClientID %>");
            var id_sel = 0;
            var s_id_sel = "";
            if(txt_id_item != null && txt_id_item != undefined && 
               txt_id_sel_item != null && txt_id_sel_item != undefined){
                if(arr_item.length > 0){
                    for (var i = 0; i < arr_item.length; i++) {
                        if(txt_id_sel_item.value === arr_item[i].value){
                            id_sel = i;
                            s_id_sel = arr_item[i].value;
                            break;
                        }
                    }
                    if(id_sel < 1) id_sel = 0;
                    txt_id_item.value = "";
                    for (var i = 0; i < arr_item.length; i++) {
                        if(arr_item[i].value != s_id_sel) txt_id_item.value += (arr_item[i].value + ";");
                        if(i === (id_sel + 1)){
                            txt_id_item.value += s_id_sel + ";";
                        }
                    }
                }
            }
        }

        function ParsePilihKriteria(){
            var txt_id_kriteria = document.getElementById("<%= txtIDKriteria.ClientID %>");
            if(txt_id_kriteria != null && txt_id_kriteria != undefined){
                txt_id_kriteria.value = "";
                var arr_kriteria = document.getElementsByName("rdo_kriteria[]");
                if(arr_kriteria.length > 0){
                    for (var i = 0; i < arr_kriteria.length; i++) {
                        if(arr_kriteria[i].checked){
                            txt_id_kriteria.value = arr_kriteria[i].value;
                            break;
                        }
                    }
                }
            }
        }

        function txtIndexSiswa() { return document.getElementById("<%= txtIndexSiswa.ClientID %>"); }
        function txtCountSiswa() { return document.getElementById("<%= txtCountSiswa.ClientID %>"); }
        function btnShowDesainEkskulSiswa() { return document.getElementById("<%= btnShowDesainEkskulSiswa.ClientID %>"); }

        function FirstSiswa() {
            if (
                    txtIndexSiswa() !== null && txtIndexSiswa() !== undefined &&
                    txtCountSiswa() !== null && txtCountSiswa() !== undefined &&
                    btnShowDesainEkskulSiswa() !== null && btnShowDesainEkskulSiswa() !== undefined
               ) {
                var id = parseInt(txtIndexSiswa().value);
                var count = parseInt(txtCountSiswa().value);

                if (id >= 0) {
                    txtIndexSiswa().value = 0;
                    btnShowDesainEkskulSiswa().click();
                }
                else {
                    ShowProgress(false);
                }
            }
        }

        function PrevSiswa() {
            if (
                    txtIndexSiswa() !== null && txtIndexSiswa() !== undefined &&
                    txtCountSiswa() !== null && txtCountSiswa() !== undefined &&
                    btnShowDesainEkskulSiswa() !== null && btnShowDesainEkskulSiswa() !== undefined
               ) {
                var id = parseInt(txtIndexSiswa().value);
                var count = parseInt(txtCountSiswa().value);

                if (id - 1 >= 0) {
                    txtIndexSiswa().value = id - 1;
                    btnShowDesainEkskulSiswa().click();
                }
                else {
                    ShowProgress(false);
                }
            }
        }

        function NextSiswa() {
            if (
                    txtIndexSiswa() !== null && txtIndexSiswa() !== undefined &&
                    txtCountSiswa() !== null && txtCountSiswa() !== undefined &&
                    btnShowDesainEkskulSiswa() !== null && btnShowDesainEkskulSiswa() !== undefined
               ) {
                var id = parseInt(txtIndexSiswa().value);
                var count = parseInt(txtCountSiswa().value);

                if (id + 1 < count) {
                    txtIndexSiswa().value = id + 1;
                    btnShowDesainEkskulSiswa().click();
                }
                else {
                    ShowProgress(false);
                }
            }
        }

        function LastSiswa() {
            if (
                    txtIndexSiswa() !== null && txtIndexSiswa() !== undefined &&
                    txtCountSiswa() !== null && txtCountSiswa() !== undefined &&
                    btnShowDesainEkskulSiswa() !== null && btnShowDesainEkskulSiswa() !== undefined
               ) {
                var id = parseInt(txtIndexSiswa().value);
                var count = parseInt(txtCountSiswa().value);

                if (id <= count - 1) {
                    txtIndexSiswa().value = count - 1;
                    btnShowDesainEkskulSiswa().click();
                }
                else {
                    ShowProgress(false);
                }
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

        function ShowProsesPilihKelas(id_kelas, show) {
            var id = id_kelas.replaceAll("-", "_");
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
                HideModal();
                $('#ui_modal_proses').modal({ backdrop: 'static', keyboard: false, show: true });
            }
            else {
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                $('#ui_modal_proses').modal('hide');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress runat="server" ID="upProgressMain" AssociatedUpdatePanelID="upMain">
        <ProgressTemplate>
            <%--<ucl:PostbackUpdateProgress runat="server" ID="pbUpdateProgress" />--%>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel runat="server" ID="upMain">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="txtKeyAction" />
            <asp:HiddenField runat="server" ID="txtID" />
            <asp:HiddenField runat="server" ID="txtIDSiswa" />
            <asp:HiddenField runat="server" ID="txtIndexSiswa" />
            <asp:HiddenField runat="server" ID="txtCountSiswa" />
            <asp:HiddenField runat="server" ID="txtTahunAjaran" />
            <asp:HiddenField runat="server" ID="txtSemester" />
            <asp:HiddenField runat="server" ID="txtKelasDet" />
            <asp:HiddenField runat="server" ID="txtIDItemPenilaian" />   
            <asp:HiddenField runat="server" ID="txtListIDItemPenilaian" />         
            <asp:HiddenField runat="server" ID="txtSelIDItemPenilaian" />
            <asp:HiddenField runat="server" ID="txtIDJenisInsert" />
            <asp:HiddenField runat="server" ID="txtIDJenisInput" />
            <asp:HiddenField runat="server" ID="txtIDKriteria" />
            <asp:HiddenField runat="server" ID="txtIDKriteriaEdit" />
            <asp:HiddenField runat="server" ID="txtIDMapelEkskul" />

            <asp:HiddenField runat="server" ID="txtKategoriPencapaianVal" />
            <asp:HiddenField runat="server" ID="txtSubKategoriPencapaianVal" />
            <asp:HiddenField runat="server" ID="txtPoinKategoriPencapaianVal" />
            <asp:HiddenField runat="server" ID="txtShowInputPencapaian" />
            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoCari" OnClick="btnDoCari_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDetail" OnClientClick="ShowProgress(true);" OnClick="btnShowDetail_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowConfirmDelete" OnClick="btnShowConfirmDelete_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowConfirmPosting" OnClick="btnShowConfirmPosting_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDesain" OnClick="btnShowDesain_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDesainEkskul" OnClick="btnShowDesainEkskul_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowEditItemPenilaian" OnClientClick="ShowProgress(true);" OnClick="btnShowEditItemPenilaian_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowPengaturanItemPenilaian" OnClientClick="ShowProgress(true);" OnClick="btnShowPengaturanItemPenilaian_Click" style="position: absolute; left: -1000px; top: -1000px;" />            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnUpdateUrut" OnClientClick="ShowProgress(true);" OnClick="btnUpdateUrut_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowEditItemKriteria" OnClientClick="ShowProgress(true);" OnClick="btnShowEditItemKriteria_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowKelas" OnClick="btnShowKelas_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDesainEkskulSiswa" OnClick="btnShowDesainEkskulSiswa_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnOKAddMapelEkskul" OnClick="btnOKAddMapelEkskul_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            
            <div class="row" style="margin-left: 0px; margin-right: 0px; margin-bottom: 200px;">
                <div class="col-xs-12">

                    <div class="col-md-8 col-md-offset-2" style="padding: 0px;">
                        <div class="card" style="margin-top: 40px;">
				            <div class="card-main">
					            <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px; ">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="background-color: #295BC8; padding: 10px; font-weight: normal; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                                <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/ebook-2.svg") %>" 
                                                    style="margin: 0 auto; height: 25px; width: 25px;" />
                                                &nbsp;
                                                Desain Rapor
                                                <asp:Literal runat="server" ID="ltrTipeRapor"></asp:Literal>
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
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle; width: 60px;">
                                                                            #
									                                    </th>
                                                                        <th style="text-align: center; background-color: #3367d6; width: 80px; vertical-align: middle;">
                                                                            <i class="fa fa-cog"></i>
                                                                        </th>
                                                                        <th style="text-align: center; background-color: #3367d6; width: 80px; vertical-align: middle;"></th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle; min-width: 100px;">
                                                                            Desain Rapor
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
                                                            <td style="text-align: center; padding: 10px; vertical-align: middle;">
                                                                <%# (Container.DisplayIndex + 1) %>.
                                                            </td>
                                                            <td style="padding: 0px; text-align: center; width: 40px; vertical-align: middle;">
                                                                <div style="margin: 0 auto; display: table;">
                                                                    <ul class="nav nav-list margin-no pull-left">
										                                <li class="dropdown">
                                                                            <a class="dropdown-toggle text-black waves-attach waves-effect" data-toggle="dropdown" 
                                                                                style="color: <%# 
                                                                                    (
                                                                                        (Eval("IsLocked") == DBNull.Value ? false : Convert.ToBoolean(Eval("IsLocked")))
                                                                                        ? " grey " //" #bfbfbf "
                                                                                        : " grey "
                                                                                    )
                                                                                %>;
                                                                                  line-height: 15px; padding: 10px; margin: 0px; min-height: 15px; z-index: 0;" title=" Menu ">
                                                                                <i class="fa fa-cog"></i>
                                                                                <i class="fa fa-caret-down" style="font-size: xx-small;"></i>
                                                                            </a>
											                                <ul class="dropdown-menu-list-table"
                                                                                <%# 
                                                                                    (
                                                                                        (Eval("IsLocked") == DBNull.Value ? false : Convert.ToBoolean(Eval("IsLocked")))
                                                                                        ? "" //" style=\"visibility: hidden;\" "
                                                                                        : ""
                                                                                    )
                                                                                %>
                                                                                >
												                                <li style="background-color: white; padding: 10px;">
													                                <label 
                                                                                        onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetail.ClientID %>.click(); " 
                                                                                        id="btnDetail" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-info-circle" title=" Lihat Detail "></i>&nbsp;&nbsp;&nbsp;Lihat Detail</label>
												                                </li>
                                                                                <li style="padding: 0px;<%# 
                                                                                    (
                                                                                        (Eval("IsLocked") == DBNull.Value ? false : Convert.ToBoolean(Eval("IsLocked")))
                                                                                        ? " display: none; "
                                                                                        : ""
                                                                                    )
                                                                                %>">
                                                                                    <hr style="margin: 0px; padding: 0px;" />
                                                                                </li>
                                                                                <li style="<%# 
                                                                                    (
                                                                                        (Eval("IsLocked") == DBNull.Value ? false : Convert.ToBoolean(Eval("IsLocked")))
                                                                                        ? " display: none; "
                                                                                        : ""
                                                                                    )
                                                                                %>background-color: white; padding: 10px;">
													                                <label 
                                                                                        onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmPosting.ClientID %>.click(); " 
                                                                                        id="btnPostingDetail" style="color: #07683c; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-upload" title=" Posting "></i>&nbsp;&nbsp;&nbsp;Posting</label>
												                                </li>
                                                                                <%--<li style="<%# 
                                                                                    (
                                                                                        (Eval("IsLocked") == DBNull.Value ? false : Convert.ToBoolean(Eval("IsLocked")))
                                                                                        ? " display: none; "
                                                                                        : ""
                                                                                    )
                                                                                %>padding: 0px;">
                                                                                    <hr style="margin: 0px; padding: 0px;" />
                                                                                </li>
												                                <li style="<%# 
                                                                                    (
                                                                                        (Eval("IsLocked") == DBNull.Value ? false : Convert.ToBoolean(Eval("IsLocked")))
                                                                                        ? " display: none; "
                                                                                        : ""
                                                                                    )
                                                                                %>background-color: white; padding: 10px;">
													                                <label
                                                                                        onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmDelete.ClientID %>.click(); " 
                                                                                        id="btnHapus" style="color: #9E0000; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-times" title=" Hapus Data "></i>&nbsp;&nbsp;&nbsp;Hapus Data</label>
												                                </li>--%>
											                                </ul>
										                                </li>
									                                </ul>
                                                                </div>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center; width: 100px;">
                                                                <label id="img_<%# Eval("Kode").ToString().Replace("-", "_") %>" style="display: none; font-size: small; color: grey; font-weight: bold;">
                                                                    <img src="../../../../Application_CLibs/images/giphy.gif" style="height: 16px; width: 20px;" />
                                                                    &nbsp;&nbsp;Proses...
                                                                </label>
                                                                <label title=" Desain Rapor " onclick="ShowProgress(true); setTimeout(function() { <%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>';<%= btnShowDesain.ClientID %>.click(); }, 500);" class="badge" style="background-color: dimgrey; cursor: pointer; font-weight: normal; font-size: x-small;">
                                                                    &nbsp;<i class="fa fa-edit"></i>&nbsp;Desain&nbsp;
                                                                </label>
                                                                <label title=" Desain Ekskul " onclick="<%# txtTahunAjaran.ClientID + ".value='" + Eval("TahunAjaran").ToString() + "';" %> <%# txtSemester.ClientID + ".value='" + Eval("Semester").ToString() + "';" %> ShowProgress(true); setTimeout(function() { <%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>';<%= btnShowDesainEkskul.ClientID %>.click(); }, 500);" class="badge" style="display: none; background-color: darkred; cursor: pointer; font-weight: normal; font-size: x-small; border-top-left-radius: 0px; border-bottom-left-radius: 0px;">
                                                                    <i class="fa fa-pencil"></i>
                                                                </label>
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
                                                                    (
                                                                        (Eval("IsLocked") == DBNull.Value ? false : Convert.ToBoolean(Eval("IsLocked")))
                                                                        ? "&nbsp;<sup title=\" Sudah Diposting \" style=\"color: #1DA1F2;\"><i class=\"fa fa-check-circle\"></i></sup>"
                                                                        : ""
                                                                    )
                                                                %>                                                                
                                                                <%# 
                                                                    Eval("NamaKelas").ToString().Trim() != ""
                                                                    ? "<br />" +
                                                                      (
                                                                        Eval("NamaKelas").ToString().Trim().ToLower().IndexOf("kelas") >= 0
                                                                        ? ""
                                                                        : "Kelas "
                                                                      ) +  
                                                                      "<span style=\"font-weight: normal;\">" + Eval("NamaKelas").ToString().Trim() + "</span>"
                                                                    : ""
                                                                %>
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
                                                                        <th style="text-align: center; background-color: #3367d6; width: 80px; vertical-align: middle;"></th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle; min-width: 100px;">
                                                                            Desain Rapor
									                                    </th>
								                                    </tr>
							                                    </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="4" style="text-align: center; padding: 10px;">
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
                                                        <asp:LinkButton ToolTip=" Tambah Data " runat="server" ID="btnDoAdd" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" style="background-color: #257228;" OnClick="btnDoAdd_Click">
                                                            <span class="fbtn-text fbtn-text-left">Tambah Data</span>
                                                            <i class="fa fa-plus" style="color: white;"></i>
                                                        </asp:LinkButton>
			                                        </div>
		                                        </div>
	                                        </div>

                                        </asp:View>
                                        <asp:View runat="server" ID="vDesain">

                                            <div style="padding: 0px; margin: 0px; background-color: #3367d6;">
                                                <div class="row">
                                                    <div class="col-lg-12" style="padding: 15px; color: white; padding-left: 30px; padding-right: 30px;">
                                                        <asp:Literal runat="server" ID="ltrInfoDesain"></asp:Literal>
                                                    </div>
                                                </div>
                                            </div>
                                            <div style="padding: 0px; margin: 0px;">
                                                <div runat="server" id="div_kriteria_penilaian" class="row" style="margin-left: 0px; margin-right: 0px;">
                                                    <div class="col-lg-12">
                                                        <div class="table-responsive" style="margin-top: 15px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
					                                        <div class="media" 
							                                        style="margin-top: 0px;
                                                                        margin-bottom: 0px;
								                                        padding-left: 8px; 
								                                        background-color: #EEEEEE;
								                                        padding: 10px;">
						                                        <div class="media-object margin-right-sm pull-left">
							                                        <span class="icon icon-lg text-brand-accent" style="color: #6A6F75;">info_outline</span>
						                                        </div>
						                                        <div class="media-inner">
							                                        <span style="color: #6A6F75;">
								                                        <span style="font-weight: bold">Kriteria Penilaian</span>
                                                                        <br />
                                                                        Diisi dengan kriteria penilaian dan akan ditampilkan berdasarkan urutan
							                                        </span>
                                                                    <div class="pull-right">
                                                                        <asp:LinkButton OnClientClick="ShowProgress(true);" ToolTip=" Tambah Kriteria " runat="server" ID="lnkAddKriteria" CssClass="badge" OnClick="lnkAddKriteria_Click" style="font-weight: normal; font-size: x-small; color: white; margin-bottom: 1px;">
                                                                            &nbsp;
                                                                            <i class="fa fa-plus-circle"></i>
                                                                            &nbsp;
                                                                            Tambah Kriteria
                                                                            &nbsp;&nbsp;
                                                                        </asp:LinkButton>
                                                                    </div>
						                                        </div>
					                                        </div>

                                                            <asp:ListView ID="lvKriteria" DataSourceID="sql_ds_kriteria" runat="server">
                                                                <LayoutTemplate>                                                                
							                                        <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
								                                        <tbody>     
                                                                            <tr id="itemPlaceholder" runat="server"></tr>
                                                                        </tbody>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr class="<%# 
                                                                                    Eval("Kode").ToString().Trim().ToUpper() == txtIDKriteriaEdit.Value.Trim().ToUpper()
                                                                                    ? "selectedrow" 
                                                                                    : (Container.DisplayIndex % 2 == 0 ? "standardrow" : "oddrow") 
                                                                               %>">
                                                                        <td style="text-align: center; padding: 10px; vertical-align: middle; width: 50px; color: #bfbfbf;">
                                                                            <%# (Container.DisplayIndex + 1) %>.
                                                                        </td>
                                                                        <td style="width: 30px;">                                                                            
                                                                            <div class="checkbox checkbox-adv"
                                                                                <%# 
                                                                                    AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.IsLocked(Eval("Rel_Rapor_Design").ToString()) 
                                                                                    ? " style=\"display: none;\" "
                                                                                    : ""
                                                                                %>
                                                                                >
													                            <label for="chk_desain_<%# Eval("Kode").ToString().Replace("-", "_") %>">
														                            <input value="<%# Eval("Kode").ToString() %>" class="access-hide" id="chk_desain_<%# Eval("Kode").ToString().Replace("-", "_") %>" name="chk_desain[]" type="checkbox">
														                            <span class="checkbox-circle"></span><span class="checkbox-circle-check"></span><span class="checkbox-circle-icon icon">done</span>
													                            </label>
												                            </div>
                                                                        </td>
                                                                        <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                                                <div class="col-md-10" style="padding: 0px; padding-right: 5px;">
                                                                                    <label for="chk_desain_<%# Eval("Kode").ToString().Replace("-", "_") %>" style="color: grey; font-weight: bold; text-transform: none; text-decoration: none; cursor: pointer;">
                                                                                        <%# 
                                                                                            AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Alias").ToString())
                                                                                        %>
                                                                                    </label>
                                                                                    <br />
                                                                                    <label for="chk_desain_<%# Eval("Kode").ToString().Replace("-", "_") %>" style="color: grey; font-weight: normal; text-transform: none; text-decoration: none; cursor: pointer;">
                                                                                        <%# 
                                                                                            AI_ERP.Application_Libs.Libs.GetHTMLNoParagraphDiAwal(Eval("NamaKriteria").ToString())
                                                                                        %>
                                                                                    </label>                                                                                    
                                                                                </div>
                                                                                <div class="col-md-2" style="padding-right: 0px;">
                                                                                    <div style="<%# 
                                                                                            AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.IsLocked(Eval("Rel_Rapor_Design").ToString()) 
                                                                                            ? " display: none; "
                                                                                            : ""
                                                                                        %>">
                                                                                        <sup>
                                                                                            <button onclick="<%= txtIDJenisInput.ClientID %>.value='<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemKriteria.ToString() %>'; <%= txtSelIDItemPenilaian.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; ParseTambahItemkriteria(); <%= lnkAddKriteria.ClientID %>.click(); return false;" title=" Tambahkan Item Penilaian " class="btn btn-flat waves-attach waves-effect" style="font-size: small; color: mediumvioletred; border-style: none; background: transparent; padding: 0px; text-transform: none;">
                                                                                                <i class="fa fa-plus-circle"></i>
                                                                                            </button>
                                                                                        </sup>
                                                                                        <sup>
                                                                                            <button onclick="<%= txtIDJenisInput.ClientID %>.value='<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemKriteria.ToString() %>'; <%= txtSelIDItemPenilaian.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; ParseSisipkanItemKriteria(); <%= lnkAddKriteria.ClientID %>.click(); return false;" title=" Sisipkan Item Penilaian " class="btn btn-flat waves-attach waves-effect" style="font-size: small; color: green; border-style: none; background: transparent; padding: 0px; text-transform: none;">
                                                                                                <i class="fa fa-plus-circle"></i>
                                                                                            </button>
                                                                                        </sup>
                                                                                        <sup>
                                                                                            <label onclick="<%= txtIDJenisInput.ClientID %>.value='<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemKriteria.ToString() %>'; <%= txtSelIDItemPenilaian.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; SetMoveUpItemPenilaian(); <%= btnUpdateUrut.ClientID %>.click(); return false;" title=" Pindahkan Ke Atas " class="btn btn-flat waves-attach waves-effect" style="font-size: small; color: darkblue; border-style: none; background: transparent; padding: 0px; text-transform: none;">
                                                                                                <i class="fa fa-arrow-circle-up"></i>
                                                                                            </label>
                                                                                        </sup>
                                                                                        <sup>
                                                                                            <label onclick="<%= txtIDJenisInput.ClientID %>.value='<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemKriteria.ToString() %>'; <%= txtSelIDItemPenilaian.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; SetMoveDownItemPenilaian(); <%= btnUpdateUrut.ClientID %>.click(); return false;" title=" Pindahkan Ke Bawah " class="btn btn-flat waves-attach waves-effect" style="font-size: small; color: dodgerblue; border-style: none; background: transparent; padding: 0px; text-transform: none;">
                                                                                                <i class="fa fa-arrow-circle-down"></i>
                                                                                            </label>
                                                                                        </sup>
                                                                                        <sup>
                                                                                            <button onclick="<%= txtIDKriteriaEdit.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= txtIDJenisInput.ClientID %>.value = '<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemKriteria.ToString() %>'; <%= btnShowEditItemKriteria.ClientID %>.click(); return false;" title=" Ubah " onclick="return false;" class="btn btn-flat btn-brand-accent waves-attach waves-effect pull-right" 
                                                                                                style="padding: 0px; padding-left: 5px; padding-right: 5px;">
                                                                                                <span style="color: slategrey;">
                                                                                                    <i class="fa fa-pencil"></i>
                                                                                                </span>
                                                                                            </button>
                                                                                        </sup>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <EmptyDataTemplate>                                                                
                                                                    <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td colspan="3" style="text-align: center; padding: 10px; background-color: #F9F9F9; color: #B0B0B0;">
                                                                                    ..:: Data Kosong ::..
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </EmptyDataTemplate>
                                                            </asp:ListView>
                                                            <asp:SqlDataSource ID="sql_ds_kriteria" runat="server"></asp:SqlDataSource>

                                                        </div>

				                                    </div>
                                                </div>

                                                <div runat="server" id="div_pencapaian_perkembangan" class="row" style="margin-left: 0px; margin-right: 0px;">
                                                    <div class="col-lg-12">
                                                        <div class="table-responsive" style="margin-top: 15px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
					                                        <div class="media" 
							                                        style="margin-top: 0px;
                                                                        margin-bottom: 0px;
								                                        padding-left: 8px; 
								                                        border-left-width: 4px;
								                                        background-color: #8A0083;
								                                        padding: 10px;">
						                                        <div class="media-object margin-right-sm pull-left">
							                                        <span class="icon icon-lg text-brand-accent" style="color: white;">info_outline</span>
						                                        </div>
						                                        <div class="media-inner">
							                                        <span style="color: white;">
								                                        <span style="font-weight: bold">Pencapaian Perkembangan</span>
                                                                        <br />
                                                                        Diisi dengan kategori pencapaian, sub kategori pencapaian atau poin kategori pencapaian
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
                                                                    <tr class="<%# 
                                                                                    Eval("Kode").ToString() == txtIDItemPenilaian.Value
                                                                                    ? "selectedrow" 
                                                                                    : (Container.DisplayIndex % 2 == 0 ? "standardrow" : "oddrow") 
                                                                               %>">
                                                                        <td style="color: #bfbfbf; width: 50px;">
                                                                            <%# (int)(this.Session[SessionViewDataName] == null ? 0 : this.Session[SessionViewDataName]) + (Container.DisplayIndex + 1) %>.
                                                                        </td>
                                                                        <td style="width: 30px;">                                                                            
                                                                            <div class="checkbox checkbox-adv"
                                                                                <%# 
                                                                                    AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.IsLocked(Eval("Rel_Rapor_Design").ToString()) 
                                                                                    ? "" //" style=\"display: none;\" "
                                                                                    : ""
                                                                                %>>
													                            <label for="chk_desain_<%# Eval("Kode").ToString().Replace("-", "_") %>">
														                            <input value="<%# Eval("Kode").ToString() %>" class="access-hide" id="chk_desain_<%# Eval("Kode").ToString().Replace("-", "_") %>" name="chk_desain[]" type="checkbox">
														                            <span class="checkbox-circle"></span><span class="checkbox-circle-check"></span><span class="checkbox-circle-icon icon">done</span>
													                            </label>
												                            </div>
                                                                        </td>
                                                                        <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">

                                                                                <div class="col-md-<%# 
                                                                                                    AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.IsLocked(Eval("Rel_Rapor_Design").ToString()) 
                                                                                                    ? "10" //"12"
                                                                                                    : "10"
                                                                                                   %>"
                                                                                     style="padding: 0px; padding-right: 5px;">

                                                                                        <table style="margin: 0px; width: 100%; box-shadow: none;">
                                                                                            <tr>
                                                                                                <td style="width: 30px; padding: 0px; background: transparent;
                                                                                                    <%# (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                            AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.Rekomendasi   
                                                                                                        ? " display : none; "
                                                                                                        : ""
                                                                                                    %>">

                                                                                                    <label for="chk_desain_<%# Eval("Kode").ToString().Replace("-", "_") %>" style="color: grey; cursor: pointer;
                                                                                                                  text-transform: none; 
                                                                                                                  text-decoration: none; 
                                                                                                                  <%# 
                                                                                                                      (
                                                                                                                        (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                            AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.KategoriPencapaian
                                                                                                                        ? "font-weight: bold; margin-right: 15px;"
                                                                                                                        : (
                                                                                                                            (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                                AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.SubKategoriPencapaian
                                                                                                                            ? "font-weight: bold; margin-left: 30px; margin-right: 15px;"
                                                                                                                            : (
                                                                                                                                (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                                    AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.PoinKategoriPencapaian
                                                                                                                                ? "font-weight: normal; margin-left: 45px; margin-right: 15px;"
                                                                                                                                : ""
                                                                                                                              )
                                                                                                                          )
                                                                                                                      )
                                                                                                                  %>">
                                                                                                        <%# 
                                                                                                            AI_ERP.Application_Libs.Libs.GetHTMLNoParagraphDiAwal(Eval("Poin").ToString())
                                                                                                        %>
                                                                                                    </label>

                                                                                                </td>
                                                                                                <td <%# (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                            AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.Rekomendasi  
                                                                                                        ? " colspan=\"2\" "
                                                                                                        : ""
                                                                                                    %>
                                                                                                    style="padding: 0px; background: transparent;">

                                                                                                    <label for="chk_desain_<%# Eval("Kode").ToString().Replace("-", "_") %>" style="color: grey; cursor: pointer;
                                                                                                                  text-transform: none; 
                                                                                                                  text-decoration: none;
                                                                                                                  <%# 
                                                                                                                      (
                                                                                                                        (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                            AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.KategoriPencapaian
                                                                                                                        ? "font-weight: bold;"
                                                                                                                        : (
                                                                                                                            (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                                AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.SubKategoriPencapaian
                                                                                                                            ? "font-weight: bold;"
                                                                                                                            : (
                                                                                                                                (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                                    AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.PoinKategoriPencapaian
                                                                                                                                ? "font-weight: normal;"
                                                                                                                                : (
                                                                                                                                    (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                                        AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.Rekomendasi
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
                                                                                                            (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.Rekomendasi
                                                                                                            ? "<div style=\"background-color: white; border-style: solid; border-width: 1px; border-color: #E3E3E3; width: 100%; height: 100px; margin-left: 3px;\"></div>"
                                                                                                            : ""
                                                                                                        )
                                                                                                    %>

                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                </div>

                                                                                <div class="col-md-2"
                                                                                     style="padding-right: 0px;
                                                                                            <%# 
                                                                                                AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.IsLocked(Eval("Rel_Rapor_Design").ToString()) 
                                                                                                ? "" //" display: none; "
                                                                                                : ""
                                                                                            %>">

                                                                                     <div style="<%# 
                                                                                                    AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.IsLocked(Eval("Rel_Rapor_Design").ToString()) 
                                                                                                    ? "" //" display: none; "
                                                                                                    : ""
                                                                                                %>">

                                                                                        <sup>
                                                                                            <button onclick="<%= txtIDItemPenilaian.ClientID %>.value = ''; <%= txtIDJenisInput.ClientID %>.value='<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemReguler.ToString() %>'; <%= txtSelIDItemPenilaian.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; ShowTambahItem(); return false;" title=" Tambahkan Item Penilaian " class="btn btn-flat waves-attach waves-effect" style="font-size: small; color: mediumvioletred; border-style: none; background: transparent; padding: 0px; text-transform: none;">
                                                                                                <i class="fa fa-plus-circle"></i>
                                                                                            </button>
                                                                                        </sup>
                                                                                        <sup>
                                                                                            <button onclick="<%= txtIDItemPenilaian.ClientID %>.value = ''; <%= txtIDJenisInput.ClientID %>.value='<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemReguler.ToString() %>'; <%= txtSelIDItemPenilaian.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; ShowSisipkanItem(); return false;" title=" Sisipkan Item Penilaian " class="btn btn-flat waves-attach waves-effect" style="font-size: small; color: green; border-style: none; background: transparent; padding: 0px; text-transform: none;">
                                                                                                <i class="fa fa-plus-circle"></i>
                                                                                            </button>
                                                                                        </sup>
                                                                                        <sup>
                                                                                            <label onclick="<%= txtIDItemPenilaian.ClientID %>.value = ''; <%= txtIDJenisInput.ClientID %>.value='<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemReguler.ToString() %>'; <%= txtSelIDItemPenilaian.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; SetMoveUpItemPenilaian(); <%= btnUpdateUrut.ClientID %>.click(); return false;" title=" Pindahkan Ke Atas " class="btn btn-flat waves-attach waves-effect" style="font-size: small; color: darkblue; border-style: none; background: transparent; padding: 0px; text-transform: none;">
                                                                                                <i class="fa fa-arrow-circle-up"></i>
                                                                                            </label>
                                                                                        </sup>
                                                                                        <sup>
                                                                                            <label onclick="<%= txtIDItemPenilaian.ClientID %>.value = ''; <%= txtIDJenisInput.ClientID %>.value='<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemReguler.ToString() %>'; <%= txtSelIDItemPenilaian.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; SetMoveDownItemPenilaian(); <%= btnUpdateUrut.ClientID %>.click(); return false;" title=" Pindahkan Ke Bawah " class="btn btn-flat waves-attach waves-effect" style="font-size: small; color: dodgerblue; border-style: none; background: transparent; padding: 0px; text-transform: none;">
                                                                                                <i class="fa fa-arrow-circle-down"></i>
                                                                                            </label>
                                                                                        </sup> 
                                                                                    </div>

                                                                                </div>

                                                                                <button onclick="<%= txtSelIDItemPenilaian.ClientID %>.value = ''; <%= txtIDItemPenilaian.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= txtIDJenisInput.ClientID %>.value = '<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemReguler.ToString() %>'; <%= btnShowEditItemPenilaian.ClientID %>.click(); return false;" title=" Ubah " onclick="return false;" class="btn btn-flat btn-brand-accent waves-attach waves-effect pull-right" 
                                                                                    style="<%# (
                                                                                                    (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                        AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.Rekomendasi ||
                                                                                                     AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.IsLocked(Eval("Rel_Rapor_Design").ToString()) 
                                                                                                        ? "" //"display: none;"
                                                                                                        : ""
                                                                                                ) %>padding: 0px; padding-left: 5px; padding-right: 5px; position: absolute; right: 60px;">
                                                                                    <span style="color: slategrey;">
                                                                                        <i class="fa fa-pencil"></i>
                                                                                    </span>
                                                                                </button>      

                                                                                <button onclick="<%= txtSelIDItemPenilaian.ClientID %>.value = ''; <%= txtIDItemPenilaian.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= txtIDJenisInput.ClientID %>.value = '<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemReguler.ToString() %>'; <%= btnShowPengaturanItemPenilaian.ClientID %>.click(); return false;" title=" Pengaturan " onclick="return false;" class="btn btn-flat btn-brand-accent waves-attach waves-effect pull-right" 
                                                                                    style="padding: 0px; padding-left: 5px; padding-right: 5px; float: right; position: absolute; right: 30px;">
                                                                                    <span style="color: slategrey;">
                                                                                        <i class="fa fa-cog" 
                                                                                            <%# 
                                                                                                (Eval("IsNewPage") == DBNull.Value ? false : Convert.ToBoolean(Eval("IsNewPage"))) ||
                                                                                                (Eval("IsLockGuruKelas") == DBNull.Value ? false : Convert.ToBoolean(Eval("IsLockGuruKelas"))) ||
                                                                                                AI_ERP.Application_DAOs.Elearning.TK.DAO_Rapor_DesignDet_GuruKhusus.GetByHeader_Entity(Eval("Kode").ToString()).Count > 0
                                                                                                ? " style=\"color: #ff6a00;\" "
                                                                                                : ""
                                                                                            %>></i>
                                                                                    </span>
                                                                                </button>
                                                                            </div>                                                                            
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <EmptyDataTemplate>                                                                
                                                                    <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td colspan="3" style="text-align: center; padding: 10px; background-color: #F9F9F9; color: #B0B0B0;">
                                                                                    ..:: Data Kosong ::..
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </EmptyDataTemplate>
                                                            </asp:ListView>
                                                            <asp:SqlDataSource ID="sql_ds_desain" runat="server"></asp:SqlDataSource>

                                                            <div runat="server" id="div_nav_siswa_dp_desain" class="content-header ui-content-header" 
                                                                style="background-color: #E6E6E6;
                                                                        box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
                                                                        background-image: none; 
                                                                        color: white;
                                                                        display: block;
                                                                        z-index: 6;
                                                                        position: fixed; bottom: 26.5px; right: 25px; width: 320px; border-radius: 25px; border-top-left-radius: 0px;
                                                                        padding: 8px; margin: 0px;">
                	
                                                                <div style="padding-left: 15px;">
				                                                    <asp:DataPager ID="dpDesain" runat="server" PageSize="300" PagedControlID="lvDesain">
                                                                        <Fields>
                                                                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="btn-trans" ShowFirstPageButton="True" FirstPageText='&nbsp;<i class="fa fa-backward"></i>&nbsp;' ShowPreviousPageButton="True" PreviousPageText='&nbsp;<i class="fa fa-arrow-left"></i>&nbsp;' ShowNextPageButton="false" />
                                                                            <asp:TemplatePagerField>
                                                                                <PagerTemplate>
                                                                                    <label title=" Halaman " style="color: grey; font-weight: normal; padding: 5px; border-style: solid; border-color: #F1F1F1; border-width: 1px; padding-left: 10px; padding-right: 10px; border-radius: 5px;">
                                                                                        <%# ((Container.StartRowIndex + 1) / (Container.PageSize)) + 1 %>
                                                                                        &nbsp;/&nbsp;
                                                                                        <%# Math.Floor(Convert.ToDecimal((Container.TotalRowCount) / (Container.PageSize))) + 1 %>
                                                                                    </label>
                                                                                </PagerTemplate>
                                                                            </asp:TemplatePagerField>
                                                                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="btn-trans" ShowLastPageButton="True" LastPageText='&nbsp;<i class="fa fa-forward"></i>&nbsp;' ShowNextPageButton="True" NextPageText='&nbsp;<i class="fa fa-arrow-right"></i>&nbsp;' ShowPreviousPageButton="false" />
                                                                            <asp:TemplatePagerField>
                                                                                <PagerTemplate>                                                                        
                                                                                    <span style="display: none; padding-top: 10px; padding-bottom: 10px; color: gray;">
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

                                                        </div>
				                                    </div>
                                                </div>

                                                <div id="div_pop_up_siswa" runat="server" class="content-header ui-content-header" 
                                                    style="background-color: #3367D6;
                                                           box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
                                                           background-image: none; 
                                                           color: white;
                                                           display: block;
                                                           z-index: 5;
                                                           position: fixed; bottom: 50px; right: -25px; width: 370px; border-radius: 25px;
                                                           padding: 12px; 
                                                           padding-top: 0px;
                                                           padding-left: 0px;
                                                           margin: 0px;"
                                                        >
                                                    <div style="width: 100%; background-color: #295BC8; padding: 10px; margin-bottom: 15px;">
                                                        <span class="icon icon-lg text-brand-accent" style="color: white;">info_outline</span>
                                                        &nbsp;
                                                        <span style="font-weight: bold; color: white;">Program Ekstrakurikuler</span>
                                                    </div>

                                                    <div class="tile-wrap" style="margin-top: 0px; margin-bottom: 0px; padding-left: 10px;">
							                            <div class="tile" style="background: transparent; box-shadow: none;">
								                            <div class="tile-side pull-left">
									                                        
                                                                <img
                                                                    src="<%=
                                                                            ResolveUrl(AI_ERP.Application_Libs.Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + "xxx" + ".jpg"))
                                                                            %>"
                                                                    style="height: 60px; width: 60px; border-radius: 100%; margin-bottom: 10px;" />

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

								                            </div>
							                            </div>
						                            </div>

                                                    <div style="color: yellow; padding-left: 10px;">
                                                        <a class="btn btn-flat waves-attach waves-effect" 
                                                            onclick="ShowProgress(true); FirstSiswa()"
                                                            title=" Data Siswa Pertama "
                                                            style="padding-left: 4px; padding-right: 4px; text-transform: none; font-weight: bold; color: white; border-style: solid; border-width: 0px; border-color: lightskyblue; padding-top: 3px; padding-bottom: 3px; font-size: small; border-radius: 0px;">
                                                            <i class="fa fa-backward"></i>
                                                        </a>
                                                        <a class="btn btn-flat waves-attach waves-effect" 
                                                            onclick="ShowProgress(true); PrevSiswa()"
                                                            title=" Data Siswa Sebelumnya "
                                                            style="padding-left: 4px; padding-right: 4px; text-transform: none; font-weight: bold; color: white; border-style: solid; border-width: 0px; border-color: lightskyblue; padding-top: 3px; padding-bottom: 3px; font-size: small; border-radius: 0px;">
                                                            <i class="fa fa-arrow-left"></i>
                                                        </a>
                                                        <a class="btn btn-flat waves-attach waves-effect" 
                                                            onclick="ShowProgress(true); NextSiswa()"
                                                            title=" Data Siswa Berikutnya "
                                                            style="padding-left: 4px; padding-right: 4px; text-transform: none; font-weight: bold; color: white; border-style: solid; border-width: 0px; border-color: lightskyblue; padding-top: 3px; padding-bottom: 3px; font-size: small; border-radius: 0px;">
                                                            <i class="fa fa-arrow-right"></i>
                                                        </a>
                                                        <a class="btn btn-flat waves-attach waves-effect" 
                                                            onclick="ShowProgress(true); LastSiswa()"
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
                                                        <a onclick="$('#ui_modal_list_kelas').modal({ backdrop: 'static', keyboard: false, show: true });"
                                                            class="btn btn-flat waves-attach waves-effect" 
                                                            title=" Pilih Siswa "
                                                            style="text-transform: none; font-weight: bold; color: lightcyan; border-style: solid; border-width: 0px; border-color: lightskyblue; padding-top: 3px; padding-bottom: 3px; font-size: small; border-radius: 0px;">
                                                            <i class="fa fa-file-text"></i>
                                                            &nbsp;Pilih Kelas
                                                        </a>
                                                        <a onclick="$('#ui_modal_list_siswa').modal({ backdrop: 'static', keyboard: false, show: true });"
                                                            class="btn btn-flat waves-attach waves-effect" 
                                                            title=" Pilih Siswa "
                                                            style="text-transform: none; font-weight: bold; color: lightcyan; border-style: solid; border-width: 0px; border-color: lightskyblue; padding-top: 3px; padding-bottom: 3px; font-size: small; border-radius: 0px;">
                                                            <i class="fa fa-user"></i>
                                                            &nbsp;Pilih Siswa
                                                        </a>
                                                    </div>                                                                

                                                    <br /><br />
                                                </div>

                                                <div id="div_list_desain_ekskul" runat="server" class="row" style="margin-left: 0px; margin-right: 0px;">
                                                    <div class="col-lg-12">
                                                        <div class="table-responsive" style="margin-top: 15px; margin-bottom: 15px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
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
                                                                        <br />
                                                                        Diisi dengan kategori pencapaian, sub kategori pencapaian atau poin kategori pencapaian untuk program ekstrakurikuler
							                                        </span> 
                                                                    <div style="margin-top: 10px;">
                                                                        Nama Siswa : <span style="font-weight: bold;">
                                                                                        <asp:Literal runat="server" ID="lblNamaSiswa"></asp:Literal>
                                                                                     </span>
                                                                        Kelas : <span style="font-weight: bold;">
                                                                                    <asp:Literal runat="server" ID="lblKelasSiswa"></asp:Literal>
                                                                                </span>
                                                                        <div class="pull-right">
                                                                            <asp:LinkButton OnClientClick="ShowProgress(true);" ToolTip=" Tambah Ekstrakurikuler " runat="server" ID="lnkAddEkskul" CssClass="badge" OnClick="lnkAddEkskul_Click" style="font-weight: normal; font-size: x-small; color: white; margin-bottom: 1px;">
                                                                                &nbsp;
                                                                                <i class="fa fa-plus-circle"></i>
                                                                                &nbsp;
                                                                                Tambah Ekstrakurikuler
                                                                                &nbsp;&nbsp;
                                                                            </asp:LinkButton>
                                                                        </div>                                                            
                                                                    </div>                                                                       
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
                                                                    <tr class="<%# 
                                                                                    Eval("Kode").ToString() == txtIDItemPenilaian.Value
                                                                                    ? "selectedrow" 
                                                                                    : (Container.DisplayIndex % 2 == 0 ? "standardrow" : "oddrow") 
                                                                               %>">
                                                                        <td style="color: #bfbfbf; width: 50px;">
                                                                            <%# (int)(this.Session[SessionViewDataName] == null ? 0 : this.Session[SessionViewDataName]) + (Container.DisplayIndex + 1) %>.
                                                                        </td>
                                                                        <td style="width: 30px;">                                                                            
                                                                            <div class="checkbox checkbox-adv"
                                                                                <%# 
                                                                                    AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.IsLocked(Eval("Rel_Rapor_Design").ToString()) &&
                                                                                    txtShowInputPencapaian.Value == "1"
                                                                                    ? " style=\"display: none;\" "
                                                                                    : ""
                                                                                %>>
													                            <label for="chk_desain_<%# Eval("Kode").ToString().Replace("-", "_") %>">
														                            <input value="<%# Eval("Kode").ToString() %>" class="access-hide" id="chk_desain_<%# Eval("Kode").ToString().Replace("-", "_") %>" name="chk_desain[]" type="checkbox">
														                            <span class="checkbox-circle"></span><span class="checkbox-circle-check"></span><span class="checkbox-circle-icon icon">done</span>
													                            </label>
												                            </div>
                                                                        </td>
                                                                        <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">

                                                                                <div class="col-md-<%# 
                                                                                                AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.IsLocked(Eval("Rel_Rapor_Design").ToString()) &&
                                                                                                txtShowInputPencapaian.Value == "1"
                                                                                                ? "12"
                                                                                                : "10"
                                                                                            %>"
                                                                                     style="padding: 0px; padding-right: 5px;">

                                                                                        <table style="margin: 0px; width: 100%; box-shadow: none;">
                                                                                            <tr>
                                                                                                <td style="width: 30px; padding: 0px; background: transparent; 
                                                                                                    <%# (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                            AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.MapelEkskul ||
                                                                                                        (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                            AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.Rekomendasi   
                                                                                                        ? " display : none; "
                                                                                                        : ""
                                                                                                    %>">

                                                                                                    <label for="chk_desain_<%# Eval("Kode").ToString().Replace("-", "_") %>" style="color: grey; cursor: pointer;
                                                                                                                  text-transform: none; 
                                                                                                                  text-decoration: none; 
                                                                                                                  <%# 
                                                                                                                      (
                                                                                                                        (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                            AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.KategoriPencapaian
                                                                                                                        ? "font-weight: bold; margin-right: 15px;"
                                                                                                                        : (
                                                                                                                            (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                                AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.SubKategoriPencapaian
                                                                                                                            ? "font-weight: bold; margin-left: 30px; margin-right: 15px;"
                                                                                                                            : (
                                                                                                                                (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                                    AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.PoinKategoriPencapaian
                                                                                                                                ? "font-weight: normal; margin-left: 45px; margin-right: 15px;"
                                                                                                                                : ""
                                                                                                                              )
                                                                                                                          )
                                                                                                                      )
                                                                                                                  %>">
                                                                                                        <%# 
                                                                                                            AI_ERP.Application_Libs.Libs.GetHTMLNoParagraphDiAwal(Eval("Poin").ToString())
                                                                                                        %>
                                                                                                    </label>

                                                                                                </td>
                                                                                                <td 
                                                                                                    <%# (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                            AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.MapelEkskul ||
                                                                                                        (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                            AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.Rekomendasi  
                                                                                                        ? " colspan=\"2\" "
                                                                                                        : ""
                                                                                                    %>
                                                                                                    style="padding: 0px; background: transparent;">

                                                                                                    <label for="chk_desain_<%# Eval("Kode").ToString().Replace("-", "_") %>" style="color: grey; cursor: pointer;
                                                                                                                  text-transform: none; 
                                                                                                                  text-decoration: none;
                                                                                                                  <%# 
                                                                                                                      (
                                                                                                                        (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                            AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.KategoriPencapaian
                                                                                                                        ? "font-weight: bold;"
                                                                                                                        : (
                                                                                                                            (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                                AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.SubKategoriPencapaian
                                                                                                                            ? "font-weight: bold;"
                                                                                                                            : (
                                                                                                                                (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                                    AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.PoinKategoriPencapaian
                                                                                                                                ? "font-weight: normal;"
                                                                                                                                : (
                                                                                                                                    (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                                        AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.Rekomendasi
                                                                                                                                    ? "font-weight: bold;"
                                                                                                                                    : ""
                                                                                                                                  )
                                                                                                                              )
                                                                                                                          )
                                                                                                                      ) +
                                                                                                                      (
                                                                                                                          (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                          AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.MapelEkskul
                                                                                                                            ? " display: none; "
                                                                                                                            : ""
                                                                                                                      )
                                                                                                                  %>">
                                                                                                        <%# 
                                                                                                            AI_ERP.Application_Libs.Libs.GetHTMLNoParagraphDiAwal(Eval("NamaKomponen").ToString())
                                                                                                        %>
                                                                                                    </label>
                                                                                                    <%# 
                                                                                
                                                                                                        (
                                                                                                            (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.Rekomendasi
                                                                                                            ? "<div style=\"background-color: white; border-style: solid; border-width: 1px; border-color: #E3E3E3; width: 100%; height: 100px; margin-left: 3px;\"></div>"
                                                                                                            : (
                                                                                                                    (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                        AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.MapelEkskul
                                                                                                                    ? "<div style=\"width: 100%; margin: 0px; padding: 0px; border-style: solid; border-width: 1px; border-color: #e7cc9a; background-color: lightgoldenrodyellow; padding-bottom: 0px; padding-top: 0px; border-radius: 0px;\">" +
                                                                                                                        "<i class=\"fa fa-lock\" style=\"position: absolute; left: 8px; top: 8px; color: darkorange;\"></i>" +
                                                                                                                        AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.GetHTMLMapelEkskul(Eval("Rel_KomponenRapor").ToString()) +
                                                                                                                      "</div>"
                                                                                                                    : ""
                                                                                                              )
                                                                                                        )
                                                                                                    %>

                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                </div>

                                                                                <div class="col-md-2"
                                                                                     style="padding-right: 0px;
                                                                                            <%# 
                                                                                                AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.IsLocked(Eval("Rel_Rapor_Design").ToString()) &&
                                                                                                txtShowInputPencapaian.Value == "1"
                                                                                                ? " display: none; "
                                                                                                : ""
                                                                                            %>">

                                                                                    <div style="<%# 
                                                                                            AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.IsLocked(Eval("Rel_Rapor_Design").ToString()) &&
                                                                                            txtShowInputPencapaian.Value == "1"
                                                                                            ? " display: none; "
                                                                                            : ""
                                                                                        %>">
                                                                                        <sup>
                                                                                            <button onclick="<%= txtIDJenisInput.ClientID %>.value='<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemEkskul.ToString() %>'; <%= txtSelIDItemPenilaian.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; ShowTambahItem(); return false;" title=" Tambahkan Item Penilaian " class="btn btn-flat waves-attach waves-effect" style="font-size: small; color: mediumvioletred; border-style: none; background: transparent; padding: 0px; text-transform: none;">
                                                                                                <i class="fa fa-plus-circle"></i>
                                                                                            </button>
                                                                                        </sup>
                                                                                        <sup>
                                                                                            <button onclick="<%= txtIDJenisInput.ClientID %>.value='<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemEkskul.ToString() %>'; <%= txtSelIDItemPenilaian.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; ShowSisipkanItem(); return false;" title=" Sisipkan Item Penilaian " class="btn btn-flat waves-attach waves-effect" style="font-size: small; color: green; border-style: none; background: transparent; padding: 0px; text-transform: none;">
                                                                                                <i class="fa fa-plus-circle"></i>
                                                                                            </button>
                                                                                        </sup>
                                                                                        <sup>
                                                                                            <button onclick="<%= txtIDJenisInput.ClientID %>.value='<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemEkskul.ToString() %>'; <%= txtSelIDItemPenilaian.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; SetMoveUpItemPenilaian(); <%= btnUpdateUrut.ClientID %>.click(); return false;" title=" Pindahkan Ke Atas " class="btn btn-flat waves-attach waves-effect" style="font-size: small; color: darkblue; border-style: none; background: transparent; padding: 0px; text-transform: none;">
                                                                                                <i class="fa fa-arrow-circle-up"></i>
                                                                                            </button>
                                                                                        </sup>
                                                                                        <sup>
                                                                                            <button onclick="<%= txtIDJenisInput.ClientID %>.value='<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemEkskul.ToString() %>'; <%= txtSelIDItemPenilaian.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; SetMoveDownItemPenilaian(); <%= btnUpdateUrut.ClientID %>.click(); return false;" title=" Pindahkan Ke Bawah " class="btn btn-flat waves-attach waves-effect" style="font-size: small; color: dodgerblue; border-style: none; background: transparent; padding: 0px; text-transform: none;">
                                                                                                <i class="fa fa-arrow-circle-down"></i>
                                                                                            </button>
                                                                                        </sup>
                                                                                        <sup style="<%# 
                                                                                            (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.MapelEkskul
                                                                                            ? " display: none; "
                                                                                            : ""
                                                                                        %>">
                                                                                            <button onclick="<%= txtIDItemPenilaian.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= txtIDJenisInput.ClientID %>.value = '<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemEkskul.ToString() %>'; <%= btnShowEditItemPenilaian.ClientID %>.click(); return false;" title=" Ubah " onclick="return false;" class="btn btn-flat btn-brand-accent waves-attach waves-effect pull-right" 
                                                                                                style="<%# (
                                                                                                                (AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor)Convert.ToInt16(Eval("JenisKomponen")) == 
                                                                                                                 AI_ERP.Application_Entities.Elearning.TK.JenisKomponenRapor.Rekomendasi
                                                                                                                 ? "display: none;"
                                                                                                                 : ""
                                                                                                           ) %>padding: 0px; padding-left: 5px; padding-right: 5px;">
                                                                                                <span style="color: slategrey;">
                                                                                                    <i class="fa fa-pencil"></i>
                                                                                                </span>
                                                                                            </button>
                                                                                        </sup>
                                                                                    </div>

                                                                                </div>
                                                                            </div>                                                                            
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <EmptyDataTemplate>                                                                
                                                                    <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td colspan="3" style="text-align: center; padding: 10px; background-color: #F9F9F9; color: #B0B0B0;">
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
                                            
                                            <div class="content-header ui-content-header" 
						                        style="background-color: #00198d;
								                        box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
								                        background-image: none; 
								                        color: white;
								                        display: block;
								                        z-index: 6;
								                        position: fixed; bottom: 33px; right: 30px; width: 130px; border-radius: 25px;
								                        padding: 8px; margin: 0px; height: 35px;">
						                        <div style="padding-left: 0px;">
							                        <asp:LinkButton OnClientClick="ShowProgress(true); " ToolTip=" Kembali " runat="server" ID="btnShowDataList" CssClass="btn-trans waves-attach waves-circle waves-effect" OnClick="btnShowDataList_Click" style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;&nbsp;
                                                        Kembali
                                                    </asp:LinkButton>
						                        </div>
					                        </div>

                                            <div runat="server" id="div_hapus_item_rapor" class="content-header ui-content-header" 
						                        style="background-color: #8A0083;
								                        background-image: none; 
								                        color: white;
								                        display: block;
								                        z-index: 6;
								                        position: fixed; bottom: 33px; right: 19px; width: 95px; border-radius: 25px;
								                        padding: 8px; margin: 0px; height: 35px;">
						                        <div style="padding-left: 0px;">
							                        <asp:LinkButton OnClientClick="ParseSelIDItemPenilaian(); ShowConfirmHapusItemPenilaian(); return false;" ToolTip=" Hapus " runat="server" ID="lnkHapusDesainItem" CssClass="btn-trans waves-attach waves-circle waves-effect" style="color: ghostwhite;">
                                                        <span style="color: ghostwhite;">
                                                            &nbsp;&nbsp;
                                                            <i class="fa fa-times"></i>
                                                            &nbsp;&nbsp;
                                                            <span style="font-weight: normal; font-size: small;">Hapus</span>
                                                        </span>                                                    
                                                    </asp:LinkButton>                                                
						                        </div>
					                        </div> 

                                            <div class="fbtn-container" id="div_button_settings_rapor_design" runat="server">
		                                        <div class="fbtn-inner">
			                                        <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #a91212;" title=" Pilihan ">
                                                        <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                                                        <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                                                    </a>
                                                    <div class="fbtn-dropup" style="z-index: 999999;">
				                                        <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" id="lnkAddKategoriPencapaianEkskul" style="display: none; background-color: black; color: white;" OnClick="lnkAddKategoriPencapaianEkskul_Click">
                                                            <span class="fbtn-text fbtn-text-left" style="font-weight: bold;">Tambah Kategori Pencapaian <span style="font-weight: bold; color: yellow;">EKSKUL</span></span>
                                                            <img style="height: 16px; width: 16px; display: initial;" src="<%= ResolveUrl("~/Application_CLibs/images/svg/browser.svg") %>">
                                                        </asp:LinkButton>
                                                        <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkAddSubKategoriPencapaianEkskul" style="display: none; background-color: black;" OnClick="lnkAddSubKategoriPencapaianEkskul_Click">
                                                            <span class="fbtn-text fbtn-text-left" style="font-weight: bold;">Tambah Sub Kategori Pencapaian <span style="font-weight: bold; color: yellow;">EKSKUL</span></span>
                                                            <img style="height: 16px; width: 16px; display: initial;" src="<%= ResolveUrl("~/Application_CLibs/images/svg/questions.svg") %>">
                                                        </asp:LinkButton>
                                                        <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkAddPoinKategoriPencapaianEkskul" style="display: none; background-color: black;" OnClick="lnkAddPoinKategoriPencapaianEkskul_Click">
                                                            <span class="fbtn-text fbtn-text-left" style="font-weight: bold;">Tambah Poin Kategori Pencapaian <span style="font-weight: bold; color: yellow;">EKSKUL</span></span>
                                                            <img style="height: 16px; width: 16px; display: initial;" src="<%= ResolveUrl("~/Application_CLibs/images/svg/test.svg") %>">
                                                        </asp:LinkButton>
                                                        <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkAddRekomendasiEkskul" style="display: none; background-color: black;" OnClick="lnkAddRekomendasiEkskul_Click">
                                                            <span class="fbtn-text fbtn-text-left" style="font-weight: bold;">Tambah Rekomendasi <span style="font-weight: bold; color: yellow;">EKSKUL</span></span>
                                                            <img style="height: 16px; width: 16px; display: initial;" src="<%= ResolveUrl("~/Application_CLibs/images/svg/chat.svg") %>">
                                                        </asp:LinkButton>

                                                        <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" id="lnkAddKategoriPencapaian" style="background-color: #61005c; color: white;" OnClick="lnkAddKategoriPencapaian_Click">
                                                            <span class="fbtn-text fbtn-text-left" style="font-weight: bold;">Tambah Kategori Pencapaian</span>
                                                            <img style="height: 16px; width: 16px; display: initial;" src="<%= ResolveUrl("~/Application_CLibs/images/svg/browser.svg") %>">
                                                        </asp:LinkButton>
                                                        <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkAddSubKategoriPencapaian" style="background-color: #61005c;" OnClick="lnkAddSubKategoriPencapaian_Click">
                                                            <span class="fbtn-text fbtn-text-left" style="font-weight: bold;">Tambah Sub Kategori Pencapaian</span>
                                                            <img style="height: 16px; width: 16px; display: initial;" src="<%= ResolveUrl("~/Application_CLibs/images/svg/questions.svg") %>">
                                                        </asp:LinkButton>
                                                        <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkAddPoinKategoriPencapaian" style="background-color: #61005c;" OnClick="lnkAddPoinKategoriPencapaian_Click">
                                                            <span class="fbtn-text fbtn-text-left" style="font-weight: bold;">Tambah Poin Kategori Pencapaian</span>
                                                            <img style="height: 16px; width: 16px; display: initial;" src="<%= ResolveUrl("~/Application_CLibs/images/svg/test.svg") %>">
                                                        </asp:LinkButton>
                                                        <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkAddRekomendasi" style="background-color: #61005c;" OnClick="lnkAddRekomendasi_Click">
                                                            <span class="fbtn-text fbtn-text-left" style="font-weight: bold;">Tambah Rekomendasi</span>
                                                            <img style="height: 16px; width: 16px; display: initial;" src="<%= ResolveUrl("~/Application_CLibs/images/svg/chat.svg") %>">
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
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 0px;">
                                                        <label class="label-input" for="<%= txtTahunPelajaran.ClientID %>" style="text-transform: none; margin-bottom: 6px;">
                                                            Tahun Pelajaran
                                                        </label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldTahunAjaran"
                                                            ControlToValidate="txtTahunPelajaran" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtTahunPelajaran"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboSemester.ClientID %>" style="text-transform: none;">Semester</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldSemester"
                                                            ControlToValidate="cboSemester" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList runat="server" ID="cboSemester" CssClass="form-control">
                                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="display: none; margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboJenisRapor.ClientID %>" style="text-transform: none;">Jenis Rapor</label>                                                        
                                                        <asp:DropDownList runat="server" ID="cboJenisRapor" CssClass="form-control">
                                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                                            <asp:ListItem Value="Kemampuan Dasar" Text="Kemampuan Dasar"></asp:ListItem>
                                                            <asp:ListItem Value="Sosial Emosi" Text="Sosial Emosi"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboKelas.ClientID %>" style="text-transform: none;">Kelas</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldKelas"
                                                            ControlToValidate="cboKelas" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList runat="server" ID="cboKelas" CssClass="form-control">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 0px;">
                                                        <label class="label-input" for="<%= txtKeterangan.ClientID %>" style="text-transform: none; margin-bottom: 6px;">
                                                            Keterangan
                                                        </label>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtKeterangan"></asp:TextBox>
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

            <div aria-hidden="true" class="modal fade" id="ui_modal_posting" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                                            <span style="font-weight: bold;">
                                                                Anda yakin akan melakukan posting?
                                                            </span>
                                                            <br /><br />
                                                            <span style="color: grey;">
                                                                Perhatian : 
                                                            </span>
                                                            <br />
                                                            <span style="font-weight: bold; color: darkorange;">
                                                                <i class="fa fa-exclamation-triangle"></i>
                                                                &nbsp;
                                                                Data yang sudah diposting tidak bisa diubah lagi
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKPosting" OnClick="lnkOKPosting_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_kategori_pencapaian" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Kategori Pencapaian
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
                                                        <div class="col-xs-6">

                                                            <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                <label class="label-input" for="<%= txtPoinItemKategoriPencapaian.ClientID %>" style="text-transform: none;">Poin</label>
                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txtPoinItemKategoriPencapaian"></asp:TextBox>
                                                            </div>

                                                        </div>
                                                    </div>
                                                    <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-12">
                                                            
                                                            <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                <label class="label-input" for="<%= txtKategoriPencapaian.ClientID %>" style="text-transform: none; margin-bottom: 15px;">Kategori Pencapaian</label>
                                                                <asp:TextBox CssClass="mcetiny_kategori_pencapaian" runat="server" ID="txtKategoriPencapaian" style="height: 100px;"></asp:TextBox>
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
                                <asp:LinkButton OnClientClick="ShowProgress(true);" ValidationGroup="vldInputKategoriPencapaian" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKKategoriPencapaian" OnClick="lnkOKKategoriPencapaian_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_sub_kategori_pencapaian" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Sub Kategori Pencapaian
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
                                                        <div class="col-xs-6">

                                                            <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                <label class="label-input" for="<%= txtPoinItemSubKategoriPencapaian.ClientID %>" style="text-transform: none;">Poin</label>
                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txtPoinItemSubKategoriPencapaian"></asp:TextBox>
                                                            </div>

                                                        </div>
                                                    </div>
                                                    <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-12">
                                                            
                                                            <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                <label class="label-input" for="<%= txtSubKategoriPencapaian.ClientID %>" style="text-transform: none; margin-bottom: 15px;">Sub Kategori Pencapaian</label>
                                                                <asp:TextBox CssClass="mcetiny_sub_kategori_pencapaian" runat="server" ID="txtSubKategoriPencapaian" style="height: 100px;"></asp:TextBox>
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
                                <asp:LinkButton OnClientClick="ShowProgress(true);" ValidationGroup="vldInputSubKategoriPencapaian" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKSubKategoriPencapaian" OnClick="lnkOKSubKategoriPencapaian_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_poin_kategori_pencapaian" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Poin Kategori Pencapaian
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
                                                        <div class="col-xs-6">

                                                            <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                <label class="label-input" for="<%= txtPoinItemPoinKategoriPencapaian.ClientID %>" style="text-transform: none;">Poin</label>
                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txtPoinItemPoinKategoriPencapaian"></asp:TextBox>
                                                            </div>

                                                        </div>
                                                    </div>
                                                    <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-12">
                                                            
                                                            <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                <label class="label-input" for="<%= txtPoinKategoriPencapaian.ClientID %>" style="text-transform: none; margin-bottom: 15px;">Poin Pencapaian</label>
                                                                <asp:TextBox CssClass="mcetiny_poin_kategori_pencapaian" runat="server" ID="txtPoinKategoriPencapaian" style="height: 100px;"></asp:TextBox>
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
                                <asp:LinkButton OnClientClick="ShowProgress(true);" ValidationGroup="vldInputPoinPencapaian" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKPoinPencapaian" OnClick="lnkOKPoinPencapaian_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_kriteria_pencapaian" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Kriteria Pencapaian
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
                                                            
                                                            Pilih kriteria penilaian sesuai dengan urutan yang akan ditampilkan dirapor :
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
					        <p class="text-right">
                                <asp:LinkButton OnClientClick="ShowProgress(true); ParsePilihKriteria();" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKKriteriaPencapaian" OnClick="lnkOKKriteriaPencapaian_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_hapus_item_penilaian" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Hapus Item Penilaian
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
                                                            
                                                            Anda yakin akan menghapus item penilaian yang dipilih?

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
                                <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapusItemPenilaian" OnClick="lnkOKHapusItemPenilaian_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_item_penilaian" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        <label id="lbl_modal_item_penilaian" style="font-weight: bold;"></label>
                                        Item Penilaian
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
                                                            
                                                            <button onclick="HideModal();                                                                              
                                                                             setTimeout(function(){ 
                                                                                if(<%= txtIDJenisInput.ClientID %>.value === '<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemReguler.ToString() %>'){ 
                                                                                    <%= lnkAddKategoriPencapaian.ClientID %>.click(); 
                                                                                } else if(<%= txtIDJenisInput.ClientID %>.value === '<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemEkskul.ToString() %>'){ 
                                                                                    <%= lnkAddKategoriPencapaianEkskul.ClientID %>.click();     
                                                                                }
                                                                             }, 50); 
                                                                             return false;" class="btn btn-flat btn-brand-accent waves-attach waves-effect" style="width: 100%; text-align: left; color: grey; text-transform: none;">
                                                                <label style="height: 40px; width: 40px; text-align: center; background-color: #6A6F75; border-radius: 100%; padding-top: 10px;">
                                                                    <img style="height: 20px; width: 20px; display: initial;" src="<%= ResolveUrl("~/Application_CLibs/images/svg/browser.svg") %>">
                                                                </label>
                                                                &nbsp;
                                                                <span style="color: grey;">
                                                                    Kategori Pencapaian
                                                                </span>
                                                            </button>
                                                            <hr style="margin: 5px;" />
                                                            <button onclick="HideModal(); 
                                                                             setTimeout(function(){ 
                                                                                if(<%= txtIDJenisInput.ClientID %>.value === '<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemReguler.ToString() %>'){ 
                                                                                    <%= lnkAddSubKategoriPencapaian.ClientID %>.click(); 
                                                                                } else if(<%= txtIDJenisInput.ClientID %>.value === '<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemEkskul.ToString() %>'){ 
                                                                                    <%= lnkAddSubKategoriPencapaianEkskul.ClientID %>.click();     
                                                                                }
                                                                             }, 50); 
                                                                             return false;" class="btn btn-flat btn-brand-accent waves-attach waves-effect" style="width: 100%; text-align: left; color: grey; text-transform: none;">
                                                                <label style="height: 40px; width: 40px; text-align: center; background-color: #6A6F75; border-radius: 100%; padding-top: 9px;">
                                                                    <img style="height: 20px; width: 20px; display: initial;" src="<%= ResolveUrl("~/Application_CLibs/images/svg/questions.svg") %>">
                                                                </label>
                                                                &nbsp;
                                                                <span style="color: grey;">
                                                                    Sub Kategori Pencapaian
                                                                </span>
                                                            </button>
                                                            <hr style="margin: 5px;" />
                                                            <button onclick="HideModal(); 
                                                                             setTimeout(function(){ 
                                                                                if(<%= txtIDJenisInput.ClientID %>.value === '<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemReguler.ToString() %>'){ 
                                                                                    <%= lnkAddPoinKategoriPencapaian.ClientID %>.click(); 
                                                                                } else if(<%= txtIDJenisInput.ClientID %>.value === '<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemEkskul.ToString() %>'){ 
                                                                                    <%= lnkAddPoinKategoriPencapaianEkskul.ClientID %>.click();     
                                                                                }
                                                                             }, 50); 
                                                                             return false;" class="btn btn-flat btn-brand-accent waves-attach waves-effect" style="width: 100%; text-align: left; color: grey; text-transform: none;">
                                                                <label style="height: 40px; width: 40px; text-align: center; background-color: #6A6F75; border-radius: 100%; padding-top: 8px;">
                                                                    <img style="height: 20px; width: 20px; display: initial;" src="<%= ResolveUrl("~/Application_CLibs/images/svg/test.svg") %>">
                                                                </label>
                                                                &nbsp;
                                                                <span style="color: grey;">
                                                                    Poin Kategori Pencapaian
                                                                </span>
                                                            </button>
                                                            <hr style="margin: 5px;" />
                                                            <button onclick="HideModal(); 
                                                                             setTimeout(function(){ 
                                                                                if(<%= txtIDJenisInput.ClientID %>.value === '<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemReguler.ToString() %>'){ 
                                                                                    <%= lnkAddRekomendasi.ClientID %>.click(); 
                                                                                } else if(<%= txtIDJenisInput.ClientID %>.value === '<%= AI_ERP.Application_Modules.EDUCATION.Penilaian.TK.wf_DesainRapor.JenisInput.ItemEkskul.ToString() %>'){ 
                                                                                    <%= lnkAddRekomendasiEkskul.ClientID %>.click();     
                                                                                }
                                                                             }, 50); 
                                                                             return false;" class="btn btn-flat btn-brand-accent waves-attach waves-effect" style="width: 100%; text-align: left; color: grey; text-transform: none;">
                                                                <label style="height: 40px; width: 40px; text-align: center; background-color: #6A6F75; border-radius: 100%; padding-top: 10px;">
                                                                    <img style="height: 20px; width: 20px; display: initial;" src="<%= ResolveUrl("~/Application_CLibs/images/svg/chat.svg") %>">
                                                                </label>
                                                                &nbsp;
                                                                <span style="color: grey;">
                                                                    Rekomendasi
                                                                </span>
                                                            </button>

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

            <div aria-hidden="true" class="modal fade" id="ui_modal_list_kelas" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                        Pilih Kelas
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
                                                        <div class="col-xs-12">
                                                            <asp:Literal runat="server" ID="ltrListKelas"></asp:Literal>
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

                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
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

            <div aria-hidden="true" class="modal fade" id="ui_modal_pengaturan_item_penilaian" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                        Pengaturan
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 25px; margin-right: 25px;">
                                                <div class="col-xs-12">
                                                    <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-12">
                                                            <label for="<%= chkIsNewPage.ClientID %>" style="cursor: pointer;">
														        <input runat="server" class="access-hide" id="chkIsNewPage" type="checkbox">
														        <span class="checkbox-circle"></span><span class="checkbox-circle-check"></span><span class="checkbox-circle-icon icon">done</span>
                                                                &nbsp;Tampilkan pada halaman baru (saat dicetak)
													        </label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="div_guru_khusus">
                                                <div class="row" style="margin-left: 10px; margin-right: 10px; margin-top: 15px;">
                                                    <div class="col-xs-12">
                                                        <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                            <div class="col-xs-12" style="font-weight: bold;">
                                                                Guru khusus pengisi nilai
                                                            </div>
                                                        </div>
                                                        <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                            <div class="col-xs-12">
                                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 5px;">
                                                                    <label class="label-input" for="<%= txtGuru1.NamaClientID %>" style="text-transform: none;">
                                                                        Guru 1
                                                                    </label>
                                                                    <ucl:AutocompletePegawai runat="server" ID="txtGuru1" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                            <div class="col-xs-12">
                                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 5px;">
                                                                    <label class="label-input" for="<%= txtGuru2.NamaClientID %>" style="text-transform: none;">
                                                                        Guru 2
                                                                    </label>
                                                                    <ucl:AutocompletePegawai runat="server" ID="txtGuru2" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                            <div class="col-xs-12">
                                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 5px;">
                                                                    <label class="label-input" for="<%= txtGuru3.NamaClientID %>" style="text-transform: none;">
                                                                        Guru 3
                                                                    </label>
                                                                    <ucl:AutocompletePegawai runat="server" ID="txtGuru3" />
                                                                </div>
                                                            </div>
                                                        </div>                                                    
                                                    </div>
                                                </div>
                                                <div class="row" style="margin-left: 25px; margin-right: 25px;">
                                                    <div class="col-xs-12">
                                                        <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                            <div class="col-xs-12">
                                                                <label for="<%= chkKunciSelainGuruKhusus.ClientID %>" style="cursor: pointer;">
														            <input runat="server" class="access-hide" id="chkKunciSelainGuruKhusus" type="checkbox">
														            <span class="checkbox-circle"></span><span class="checkbox-circle-check"></span><span class="checkbox-circle-icon icon">done</span>
                                                                    &nbsp;Kunci pengisian nilai selain guru khusus
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
					        <p class="text-right">
                                <asp:LinkButton OnClientClick="ShowProgress(true);"  CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKItemPengaturan" OnClick="lnkOKItemPengaturan_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_list_pilih_ekskul" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                        Pilih Ekstrakurikuler
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 0px;">
                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                <div class="col-xs-12">
                                                    <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-12">
                                                            <asp:Literal runat="server" ID="ltrListEkskul"></asp:Literal>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
        function LoadTinyMCEKategoriPencapaian() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.remove();
            tinymce.init({
                selector: ".mcetiny_kategori_pencapaian",
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
                        document.getElementById('<%= txtKategoriPencapaianVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
        }

        function LoadTinyMCESubKategoriPencapaian() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.remove();
            tinymce.init({
                selector: ".mcetiny_sub_kategori_pencapaian",
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
                        document.getElementById('<%= txtSubKategoriPencapaianVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
        }

        function LoadTinyMCEPoinKategoriPencapaian() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.remove();
            tinymce.init({
                selector: ".mcetiny_poin_kategori_pencapaian",
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
                        document.getElementById('<%= txtPoinKategoriPencapaianVal.ClientID %>').value = ed.getContent();
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
        <%= txtGuru1.NamaClientID %>_SHOW_AUTOCOMPLETE();
        <%= txtGuru2.NamaClientID %>_SHOW_AUTOCOMPLETE();
        <%= txtGuru3.NamaClientID %>_SHOW_AUTOCOMPLETE();
    </script>
</asp:Content>
