<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Application_Masters/Main.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="wf.FormasiGuruMapel.aspx.cs" Inherits="AI_ERP.Application_Modules.MASTER.wf_FormasiGuruMapel" %>

<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<%@ Register TagPrefix="ucl" TagName="AutocompletePegawaiByUnit" Src="~/Application_Controls/AutocompletePegawai/AutocompletePegawaiByUnit.ascx" %>
<%@ Register TagPrefix="ucl" TagName="AutocompleteSiswaNISSekolahByUnit" Src="~/Application_Controls/AutocompleteSiswa/AutocompleteSiswaNISSekolahByUnit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function HideModal(){
            $('#ui_modal_input_data').modal('hide');
            $('#ui_modal_confirm_hapus').modal('hide');    
            $('#ui_modal_input_guru_mapel').modal('hide');    
            $('#ui_modal_hapus_item_mengajar').modal('hide');    
            $('#ui_modal_tampilan_data').modal('hide');
            $('#ui_modal_input_siswa_mapel').modal('hide');
            $('#ui_modal_pilih_siswa').modal('hide');

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
                    HideModal();
                    ShowKelasByUnit(document.getElementById("<%= cboUnitSekolah.ClientID %>").value);
                    ShowMapelByUnit(document.getElementById("<%= cboUnitSekolah.ClientID %>").value);
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
                    ShowKelasByUnit(document.getElementById("<%= cboUnitSekolah.ClientID %>").value);
                    ShowMapelByUnit(document.getElementById("<%= cboUnitSekolah.ClientID %>").value);
                    break;
                case "<%= JenisAction.AddFormasiSiswaWithMessage %>":
                    HideModal();
                    $('#ui_modal_input_siswa_mapel').modal({ backdrop: 'static', keyboard: false, show: true });
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoShowData %>":
                    ShowKelasByUnit(document.getElementById("<%= cboUnitSekolah.ClientID %>").value);
                    ShowMapelByUnit(document.getElementById("<%= cboUnitSekolah.ClientID %>").value);
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmHapus %>":
                    $('#ui_modal_confirm_hapus').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowInputMengajar %>":
                    $('#ui_modal_input_guru_mapel').modal({ backdrop: 'static', keyboard: false, show: true });                    
                    break;
                case "<%= JenisAction.DoShowInputSiswaMapel %>":
                    $('#ui_modal_input_siswa_mapel').modal({ backdrop: 'static', keyboard: false, show: true });                    
                    break;
                case "<%= JenisAction.DoShowTampilanData %>":
                    $('#ui_modal_tampilan_data').modal({ backdrop: 'static', keyboard: false, show: true });                    
                    break;   
                case "<%= JenisAction.DoShowPilihSiswa %>":
                    $('#ui_modal_pilih_siswa').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;                    
                case "<%= JenisAction.Update %>":
                    HideModal();
                    break;
                case "<%= JenisAction.Delete %>":
                    HideModal();
                    break;
                case "<%= JenisAction.DoOKShowPilihSiswa %>":
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah diupdate',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    HideModal();
                    break;
                case "<%= JenisAction.DoTampilkanData %>":
                    HideModal();
                    window.scrollTo(0,0); 
                    break;                    
                case "<%= JenisAction.ShowDataList %>":
                    SetScrollPos();
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
                    ShowKelasByUnit(document.getElementById("<%= cboUnitSekolah.ClientID %>").value);
                    ShowMapelByUnit(document.getElementById("<%= cboUnitSekolah.ClientID %>").value);
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
                case "<%= JenisAction.DoChangePage %>":
                case "<%= JenisAction.ShowDataMengajar %>":
                case "<%= JenisAction.ShowDataFormasiSiswa %>":
                    window.scrollTo(0,0); 
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
            <%= txtGuru.NamaClientID %>_SHOW_AUTOCOMPLETE();
            <%= txtSiswaMapel.NamaClientID %>_SHOW_AUTOCOMPLETE();
            document.getElementById("<%= txtKeyAction.ClientID %>").value = "";            
            
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

            $('#ui_modal_input_guru_mapel').on('shown.bs.modal', function () {
                <%= txtGuru.NamaClientID %>.focus();
            });

            $('#ui_modal_input_siswa_mapel').on('shown.bs.modal', function () {
                <%= txtSiswaMapel.NamaClientID %>.focus();
            });
        }

        function ShowKelasByUnit(unit) {
            var txt_arr = document.getElementById("<%= txtParseKelasUnit.ClientID %>");
            var cbo_kelas = document.getElementById("<%= cboKelas.ClientID %>");
            if(cbo_kelas != null && cbo_kelas != undefined){
                if (cbo_kelas.options.length > 0) {
                    for (var i = cbo_kelas.options.length - 1; i >= 0; i--) {
                        cbo_kelas.options[i] = null;
                    }
                }
            }
            if(txt_arr != null && txt_arr != undefined && cbo_kelas != null && cbo_kelas != undefined){
                if (unit.trim() != "") {
                    var arr_kelas = txt_arr.value.split(";");
                    if (arr_kelas.length > 0) {
                        for (var i = 0; i < arr_kelas.length; i++) {
                            var kk_unit = unit + '->';
                            if (arr_kelas[i].indexOf(kk_unit) >= 0) {
                                var s_kelas = arr_kelas[i].replace(kk_unit, "");                                
                                var arr_item_kelas = s_kelas.split("|");
                                if (arr_item_kelas.length === 2) {
                                    var option = document.createElement("option");
                                    option.value = arr_item_kelas[0].toUpperCase();
                                    option.text = arr_item_kelas[1];
                                    if(option.value === <%= txtKelasUnit.ClientID %>.value.toUpperCase()){
                                        option.selected = true
                                    }
                                    cbo_kelas.add(option);
                                }
                            }
                        }
                    }
                }
            }
        }

        function ShowMapelByUnit(unit) {
            var txt_arr = document.getElementById("<%= txtParseMapelUnit.ClientID %>");
            var cbo_mapel = document.getElementById("<%= cboMapel.ClientID %>");
            if(cbo_mapel != null && cbo_mapel != undefined){
                if (cbo_mapel.options.length > 0) {
                    for (var i = cbo_mapel.options.length - 1; i >= 0; i--) {
                        cbo_mapel.options[i] = null;
                    }
                }
            }
            if(txt_arr != null && txt_arr != undefined && cbo_mapel != null && cbo_mapel != undefined){
                if (unit.trim() != "") {
                    var arr_mapel= txt_arr.value.split(";");
                    if (arr_mapel.length > 0) {
                        for (var i = 0; i < arr_mapel.length; i++) {
                            var kk_unit = unit + '->';
                            if (arr_mapel[i].indexOf(kk_unit) >= 0) {
                                var s_mapel = arr_mapel[i].replace(kk_unit, "");                                
                                var arr_item_mapel = s_mapel.split("|");
                                if (arr_item_mapel.length === 2) {
                                    var option = document.createElement("option");
                                    option.value = arr_item_mapel[0].toUpperCase();
                                    option.text = arr_item_mapel[1];
                                    if(option.value === <%= txtMapelUnit.ClientID %>.value.toUpperCase()){
                                        option.selected = true
                                    }
                                    cbo_mapel.add(option);
                                }
                            }
                        }
                    }
                }
            }
        }

        function SelectOrUnSelectAll(){
            var arr_mengajar = document.getElementsByName("chk_mengajar[]");
            if(arr_mengajar.length > 0){
                var ada_cek = false;
                for (var i = 0; i < arr_mengajar.length; i++) {
                    if(arr_mengajar[i].checked){
                        ada_cek = true;
                    }
                }
                for (var i = 0; i < arr_mengajar.length; i++) {
                    arr_mengajar[i].checked = (ada_cek ? false : true);
                }
            }
        }

        function ParseSelectItem(){
            var txt_sel_item = document.getElementById("<%= txtSelItem.ClientID %>");
            if(txt_sel_item != null && txt_sel_item != undefined){
                txt_sel_item.value = "";
                var arr_mengajar = document.getElementsByName("chk_mengajar[]");
                for (var i = 0; i < arr_mengajar.length; i++) {
                    if(arr_mengajar[i].checked){
                        txt_sel_item.value += arr_mengajar[i].value + ";";
                    }
                }
            }
        }

        function SelectOrUnSelectAllSiswa(){
            var arr_mengajar = document.getElementsByName("chk_formasi_siswa[]");
            if(arr_mengajar.length > 0){
                var ada_cek = false;
                for (var i = 0; i < arr_mengajar.length; i++) {
                    if(arr_mengajar[i].checked){
                        ada_cek = true;
                    }
                }
                for (var i = 0; i < arr_mengajar.length; i++) {
                    arr_mengajar[i].checked = (ada_cek ? false : true);
                }
            }
        }

        function ParseSelectItemSiswa(){
            var txt_sel_item = document.getElementById("<%= txtSelItem.ClientID %>");
            if(txt_sel_item != null && txt_sel_item != undefined){
                txt_sel_item.value = "";
                var arr_mengajar = document.getElementsByName("chk_formasi_siswa[]");
                for (var i = 0; i < arr_mengajar.length; i++) {
                    if(arr_mengajar[i].checked){
                        txt_sel_item.value += arr_mengajar[i].value + ";";
                    }
                }
            }
        }

        function ShowConfirmHapusMengajar(){
            ParseSelectItem();
            $('#ui_modal_hapus_item_mengajar').modal({ backdrop: 'static', keyboard: false, show: true });
        }

        function ShowConfirmHapusSiswa(){
            ParseSelectItemSiswa();
            $('#ui_modal_hapus_item_siswa').modal({ backdrop: 'static', keyboard: false, show: true });
        }

        function stopRKey(evt) {
            var evt = (evt) ? evt : ((event) ? event : null);
            var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
            if ((evt.keyCode == 13) && (node.type == "text")) { return false; }
        }
        document.onkeypress = stopRKey;

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

        function DoCheckSiswa(value){
            var arr_siswa = document.getElementsByName("chk_pilih_siswa[]");
            if(arr_siswa.length > 0){
                for (var i = 0; i < arr_siswa.length; i++) {
                    arr_siswa[i].checked = value;
                }
            }
        }

        function GetCheckedSiswa(){
            var arr_siswa = document.getElementsByName("chk_pilih_siswa[]");
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

        function ValidateCheckedSiswa(){
            var arr_siswa = document.getElementsByName("chk_pilih_siswa[]");
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
            <asp:HiddenField runat="server" ID="txtIDItem" />
            <asp:HiddenField runat="server" ID="txtIDItemSiswa" />
            <asp:HiddenField runat="server" ID="txtParseKelasUnit" />
            <asp:HiddenField runat="server" ID="txtParseMapelUnit" />
            <asp:HiddenField runat="server" ID="txtParsePilihSiswa" />
            <asp:HiddenField runat="server" ID="txtKelasUnit" />
            <asp:HiddenField runat="server" ID="txtKelasDet" />
            <asp:HiddenField runat="server" ID="txtMapelUnit" />
            <asp:HiddenField runat="server" ID="txtSelItem" />
            <asp:HiddenField runat="server" ID="txtYpos" />
            <asp:HiddenField runat="server" ID="txtXpos" />
            <asp:HiddenField runat="server" ID="txtJenisMapel" />
            <asp:HiddenField runat="server" ID="txtLevel" />
            <asp:HiddenField runat="server" ID="txtIsSiswaPilihan" />


            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoCari" OnClick="btnDoCari_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDetail" OnClick="btnShowDetail_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDataMengajar" OnClick="btnShowDataMengajar_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDataFormasiSiswa" OnClick="btnShowDataFormasiSiswa_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDataPilihSiswa" OnClick="btnShowDataPilihSiswa_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputGuruMengajar" OnClick="btnShowInputGuruMengajar_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputFormasiSiswa" OnClick="btnShowInputFormasiSiswa_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowConfirmDelete" OnClick="btnShowConfirmDelete_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowEditMengajar" OnClick="btnShowEditMengajar_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowEditFormasiSiswa" OnClick="btnShowEditFormasiSiswa_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoUpdateSiswaPilihan" OnClick="btnDoUpdateSiswaPilihan_Click" Style="position: absolute; left: -1000px; top: -1000px;" />

            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-12">

                    <div class="col-md-8 col-md-offset-2" style="padding: 0px;">
                        <div class="card" style="margin-top: 40px;">
                            <div class="card-main">
                                <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px;">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="background-color: #295BC8; padding: 10px; font-weight: normal; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                                <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/network.svg") %>"
                                                    style="margin: 0 auto; height: 25px; width: 25px;" />
                                                &nbsp;
                                                Formasi Guru Mata Pelajaran
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
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle; width: 80px;">#
                                                                        </th>
                                                                        <th style="text-align: center; background-color: #3367d6; width: 80px; vertical-align: middle;">
                                                                            <i class="fa fa-cog"></i>
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">Formasi Mata Pelajaran
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">Level
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">&nbsp;
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
                                                                                <li style="background-color: white; padding: 10px;
                                                                                    <%# 
                                                                                        !AI_ERP.Application_Modules.MASTER.wf_FormasiGuruMapel.IsShowDetail(Eval("Kode").ToString()) 
                                                                                        ? " display: none; "
                                                                                        : ""
                                                                                    %>
                                                                                    ">
                                                                                    <label
                                                                                        onclick="DoScrollPos(); setTimeout(function() { <%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>';<%= btnShowDetail.ClientID %>.click(); }, 100);"
                                                                                        id="btnDetail" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                        <i class="fa fa-info-circle" title=" Lihat Detail "></i>&nbsp;&nbsp;&nbsp;Lihat Detail</label>
                                                                                </li>
                                                                                <li style="padding: 0px;
                                                                                    <%# 
                                                                                        !AI_ERP.Application_Modules.MASTER.wf_FormasiGuruMapel.IsShowDetail(Eval("Kode").ToString()) 
                                                                                        ? " display: none; "
                                                                                        : ""
                                                                                    %>
                                                                                    ">
                                                                                    <hr style="margin: 0px; padding: 0px;" />
                                                                                </li>
                                                                                <li style="background-color: white; padding: 10px;">
                                                                                    <label
                                                                                        onclick="ParseSelectItem(); <%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmDelete.ClientID %>.click(); "
                                                                                        id="btnHapus" style="color: #9E0000; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                        <i class="fa fa-times" title=" Hapus Data "></i>&nbsp;&nbsp;&nbsp;Hapus Data</label>
                                                                                </li>
                                                                            </ul>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; cursor: pointer; padding-top: 5px; padding-bottom: 5px;" onclick="DoScrollPos(); <%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>';<%= btnShowDataMengajar.ClientID %>.click();">
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
                                                                <span style="color: #bfbfbf; font-size: smaller;">&nbsp;
                                                                    <i class="fa fa-arrow-right"></i>
                                                                    &nbsp;
                                                                </span>
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none; color: #1DA1F2; font-weight: bold;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Sekolah").ToString())
                                                                    %>
                                                                </span>
                                                                <br />
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Mapel").ToString())
                                                                    %>
                                                                </span>
                                                                <br />
                                                                <span style="color: #ff66d7; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("JenisMapel").ToString())
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kelas").ToString())
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="text-align: right; vertical-align: middle;">
                                                                <%# 
                                                                    Eval("JenisMapel").ToString().Trim() == AI_ERP.Application_Libs.Libs.JENIS_MAPEL.LINTAS_MINAT 
                                                                    ? "<label onclick=\" DoScrollPos(); " + txtID.ClientID + ".value = '" + Eval("Kode").ToString() + "';" + btnShowDataFormasiSiswa.ClientID + ".click();\" class=\"badge\" style=\"cursor: pointer; font-weight: normal; font-size: x-small; background-color: mediumvioletred;\">" +
                                                                        "&nbsp;<i class=\"fa fa-users\"></i>&nbsp;&nbsp;&nbsp;Pilih Siswa&nbsp;&nbsp;" +
                                                                      "</label>"
                                                                    : ""
                                                                %>
                                                                <label id="lbl_<%# Eval("Kode").ToString().Replace("-", "_") %>" onclick="<%# txtLevel.ClientID %>.value = '<%# Eval("Kelas").ToString().ToUpper() %>'; <%# txtJenisMapel.ClientID %>.value = '<%# Eval("JenisMapel").ToString() %>'; DoScrollPos(); <%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>';<%= btnShowDataMengajar.ClientID %>.click();" class="badge" style="cursor: pointer; font-weight: normal; font-size: x-small;">
                                                                    &nbsp;
                                                                    <i class="fa fa-edit"></i>
                                                                    &nbsp;
                                                                    Data Mengajar
                                                                    &nbsp;
                                                                </label>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr style="background-color: #3367d6;">
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle; width: 80px;">#
                                                                        </th>
                                                                        <th style="text-align: center; background-color: #3367d6; width: 80px; vertical-align: middle;">
                                                                            <i class="fa fa-cog"></i>
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">Formasi Mata Pelajaran
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">Level
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">&nbsp;
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="5" style="text-align: center; padding: 10px;">..:: Data Kosong ::..
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
                                                style="background-color: white; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 28px; right: 25px; width: 320px; border-radius: 25px; padding: 8px; margin: 0px;">

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
                                                        <asp:LinkButton OnClick="btnRefresh_Click" CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" ID="btnRefresh" title=" Refresh " Style="background-color: #601B70; color: white;">
                                                            <span class="fbtn-text fbtn-text-left">Refresh Data</span>
                                                            <i class="fa fa-refresh"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ToolTip=" Tampilan Data " runat="server" ID="btnDoShowTampilan" CssClass="fbtn waves-attach waves-circle waves-effect" Style="background-color: #2f569d;" OnClick="btnDoShowTampilan_Click">
                                                            <span class="fbtn-text fbtn-text-left">Tampilan Data</span>
                                                            <i class="fa fa-eye" style="color: white;"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ToolTip=" Tambah Data " runat="server" ID="btnDoAdd" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" Style="background-color: #257228;" OnClick="btnDoAdd_Click">
                                                            <span class="fbtn-text fbtn-text-left">Tambah Data</span>
                                                            <i class="fa fa-plus" style="color: white;"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>

                                        </asp:View>
                                        <asp:View ID="vListGuru" runat="server">

                                            <div style="padding: 0px; margin: 0px;">
                                                <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                    <tr style="background-color: #3367d6;">
                                                        <td style="text-align: left; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle;">
                                                            <asp:Literal runat="server" ID="ltrCaptionFormasi"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:ListView ID="lvListGuruMengajar" DataSourceID="sql_ds_formasi_guru" runat="server" OnSorting="lvListGuruMengajar_Sorting" OnPagePropertiesChanging="lvListGuruMengajar_PagePropertiesChanging">
                                                    <LayoutTemplate>
                                                        <div class="table-responsive" style="margin: 0px; box-shadow: none;">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr style="background-color: #3c70e1;">
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #3c70e1; vertical-align: middle; width: 80px;">#
                                                                        </th>
                                                                        <th style="text-align: left; background-color: #3c70e1; width: 50px; vertical-align: middle;">
                                                                            <label onclick="SelectOrUnSelectAll();" style="cursor: pointer;">
                                                                                <i class="fa fa-clone"></i>
                                                                            </label>
                                                                        </th>
                                                                        <th style="background-color: #3c70e1; text-align: left; padding-left: 10px; vertical-align: middle;">Guru
                                                                        </th>
                                                                        <th style="background-color: #3c70e1; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                        </th>
                                                                        <th style="background-color: #3c70e1; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                        </th>
                                                                        <th style="background-color: #3c70e1; text-align: left; padding-left: 10px; vertical-align: middle;">Keterangan
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
                                                                        Eval("Kode").ToString() == txtIDItem.Value
                                                                        ? "selectedrow" 
                                                                        : (Container.DisplayIndex % 2 == 0 ? "standardrow" : "oddrow") 
                                                                   %>">
                                                            <td style="text-align: center; padding: 10px; vertical-align: middle; color: #bfbfbf;">
                                                                <%# (Container.DisplayIndex + 1) %>.
                                                            </td>
                                                            <td style="width: 30px; text-align: center; vertical-align: middle;">
                                                                <div class="checkbox checkbox-adv" style="margin: 0 auto;">
                                                                    <label for="chk_mengajar_<%# Eval("Kode").ToString().Replace("-", "_") %>">
                                                                        <input value="<%# Eval("Kode").ToString() %>"
                                                                            class="access-hide"
                                                                            id="chk_mengajar_<%# Eval("Kode").ToString().Replace("-", "_") %>"
                                                                            name="chk_mengajar[]"
                                                                            type="checkbox">
                                                                        <span class="checkbox-circle"></span><span class="checkbox-circle-check"></span><span class="checkbox-circle-icon icon">done</span>
                                                                    </label>
                                                                </div>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: #bfbfbf; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Rel_Guru").ToString())
                                                                    %>
                                                                </span>
                                                                <br />
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Guru").ToString())
                                                                    %>
                                                                </span>
                                                                <%# 
                                                                    txtJenisMapel.Value != AI_ERP.Application_Libs.Libs.JENIS_MAPEL.LINTAS_MINAT
                                                                    ? "<br />" +
                                                                      "<span style=\"color: rgba(203,96,179,1); font-weight: bold; text-transform: none; text-decoration: none;\">" +
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("KelasDet").ToString()) +
                                                                      "</span>" +
                                                                      (
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("JenisKelas").ToString()).Trim() != ""
                                                                        ? "&nbsp;&nbsp;" +
                                                                          "<span style=\"color: rgb(255, 148, 38); font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                            AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("JenisKelas").ToString()) +
                                                                          "</span>"
                                                                        : ""
                                                                      )                                                                 
                                                                    : ""
                                                                %>
                                                                <button onclick="<%= txtIDItem.ClientID %>.value = '<%# Eval("Kode").ToString() %>';<%= btnShowEditMengajar.ClientID %>.click(); return false;" title=" Ubah " onclick="return false;" class="btn btn-flat btn-brand-accent waves-attach waves-effect pull-right"
                                                                    style="padding: 0px; padding-left: 5px; padding-right: 5px; float: right;">
                                                                    <span style="color: slategrey;">
                                                                        <i class="fa fa-pencil"></i>
                                                                    </span>
                                                                </button>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center; width: 150px;">
                                                                <div class="checkbox checkbox-adv" 
                                                                     style="margin: 0 auto;
                                                                            <%#
                                                                                txtJenisMapel.Value == AI_ERP.Application_Libs.Libs.JENIS_MAPEL.WAJIB_B_PILIHAN &&
                                                                                txtLevel.Value == "X"
                                                                                ? ""
                                                                                : " display: none; "
                                                                            %>
                                                                           ">
                                                                    <label for="chk_siswa_pilihan_<%# Eval("Kode").ToString().Replace("-", "_") %>">
                                                                        <input value="<%# Eval("Kode").ToString() %>"
                                                                            onclick="<%# txtIDItem.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%# txtIsSiswaPilihan.ClientID %>.value = (this.checked ? '1': '0'); <%# btnDoUpdateSiswaPilihan.ClientID %>.click(); "
                                                                            class="access-hide"
                                                                            id="chk_siswa_pilihan_<%# Eval("Kode").ToString().Replace("-", "_") %>"
                                                                            name="chk_siswa_pilihan[]"
                                                                            <%# (Eval("IsSiswaPilihan") == DBNull.Value ? false : Convert.ToBoolean(Eval("IsSiswaPilihan"))) ? "checked=\"checked\" " : "" %>
                                                                            type="checkbox">
                                                                        <span class="checkbox-circle"></span><span class="checkbox-circle-check"></span><span class="checkbox-circle-icon icon">done</span>
                                                                        <span style="font-weight: bold; color: grey;">Siswa Pilihan</span>
                                                                    </label>
                                                                </div>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; width: 100px;">
                                                                <%# 
                                                                    txtJenisMapel.Value == AI_ERP.Application_Libs.Libs.JENIS_MAPEL.WAJIB_B_PILIHAN &&
                                                                    txtLevel.Value != "X"
                                                                    ? "<label onclick=\" DoScrollPos(); " + txtIDItem.ClientID + ".value = '" + Eval("Kode").ToString() + "';" + btnShowDataPilihSiswa.ClientID + ".click();\" class=\"badge\" style=\"cursor: pointer; font-weight: normal; font-size: x-small; background-color: mediumvioletred; float: right;\">" +
                                                                        "&nbsp;<i class=\"fa fa-users\"></i>&nbsp;&nbsp;&nbsp;Pilih Siswa&nbsp;&nbsp;" +
                                                                      "</label>"
                                                                    : (
                                                                        (Eval("IsSiswaPilihan") == DBNull.Value ? false : Convert.ToBoolean(Eval("IsSiswaPilihan"))) &&
                                                                        txtJenisMapel.Value == AI_ERP.Application_Libs.Libs.JENIS_MAPEL.WAJIB_B_PILIHAN &&
                                                                        txtLevel.Value == "X"
                                                                        ? "<label onclick=\" DoScrollPos(); " + txtIDItem.ClientID + ".value = '" + Eval("Kode").ToString() + "';" + btnShowDataPilihSiswa.ClientID + ".click();\" class=\"badge\" style=\"cursor: pointer; font-weight: normal; font-size: x-small; background-color: mediumvioletred; float: right;\">" +
                                                                            "&nbsp;<i class=\"fa fa-users\"></i>&nbsp;&nbsp;&nbsp;Pilih Siswa&nbsp;&nbsp;" +
                                                                          "</label>"
                                                                        : ""
                                                                      )
                                                                %>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Keterangan").ToString())
                                                                    %>
                                                                </span>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr style="background-color: #3c70e1;">
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #3c70e1; vertical-align: middle; width: 80px;">#
                                                                        </th>
                                                                        <th style="text-align: left; background-color: #3c70e1; width: 80px; vertical-align: middle;">
                                                                            <label onclick="SelectOrUnSelectAll();" style="cursor: pointer;">
                                                                                <i class="fa fa-clone"></i>
                                                                            </label>
                                                                        </th>
                                                                        <th style="background-color: #3c70e1; text-align: left; padding-left: 10px; vertical-align: middle;">Guru
                                                                        </th>
                                                                        <th style="background-color: #3c70e1; text-align: left; padding-left: 10px; vertical-align: middle;">Keterangan
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td runat="server" colspan="4" style="text-align: center; padding: 10px;">..:: Data Kosong ::..
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </div>
                                            <asp:SqlDataSource ID="sql_ds_formasi_guru" runat="server"></asp:SqlDataSource>

                                            <div class="content-header ui-content-header"
                                                style="background-color: #00198d; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 33px; right: 50px; width: 160px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                                                <div style="padding-left: 0px;">
                                                    <asp:LinkButton ToolTip=" Kembali " runat="server" ID="btnShowDataList" CssClass="btn-trans waves-attach waves-circle waves-effect" OnClick="btnShowDataList_Click" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;
                                                    </asp:LinkButton>
                                                </div>
                                            </div>

                                            <div class="content-header ui-content-header"
                                                style="background-color: #8A0083; background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 33px; right: 50px; width: 120px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                                                <div style="padding-left: 0px;">
                                                    <asp:LinkButton OnClientClick="ShowConfirmHapusMengajar(); return false;" ToolTip=" Hapus " runat="server" ID="lnkHapusItem" CssClass="btn-trans waves-attach waves-circle waves-effect" Style="color: ghostwhite;">
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

                                            <div class="fbtn-container" id="div_button_settings_formasi_guru" runat="server">
                                                <div class="fbtn-inner">
                                                    <a onclick="setTimeout(
                                                            function(){
                                                                <%= btnShowInputGuruMengajar.ClientID %>.click();
                                                            }, 500
                                                        ); return false;"
                                                        class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #a91212;" title=" Tambah Data ">
                                                        <span class="fbtn-ori icon" style="padding-left: 2px;"><span class="fa fa-plus"></span></span>
                                                        <span class="fbtn-sub icon" style="padding-left: 2px;"><span class="fa fa-plus"></span></span>
                                                    </a>
                                                </div>
                                            </div>

                                        </asp:View>

                                        <asp:View ID="vListSiswa" runat="server">

                                            <div style="padding: 0px; margin: 0px;">
                                                <table class="table" id="Table1" runat="server" style="width: 100%; margin: 0px;">
                                                    <tr style="background-color: #3367d6;">
                                                        <td style="text-align: left; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle;">
                                                            <asp:Literal runat="server" ID="ltrCaptionFormasiSiswa"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:ListView ID="lvListSiswaMapel" DataSourceID="sql_ds_formasi_siswa" runat="server" OnSorting="lvListSiswaMapel_Sorting" OnPagePropertiesChanging="lvListSiswaMapel_PagePropertiesChanging">
                                                    <LayoutTemplate>
                                                        <div class="table-responsive" style="margin: 0px; box-shadow: none;">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr style="background-color: #3c70e1;">
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #3c70e1; vertical-align: middle; width: 80px;">#
                                                                        </th>
                                                                        <th style="text-align: left; background-color: #3c70e1; width: 50px; vertical-align: middle;">
                                                                            <label onclick="SelectOrUnSelectAllSiswa();" style="cursor: pointer;">
                                                                                <i class="fa fa-clone"></i>
                                                                            </label>
                                                                        </th>
                                                                        <th style="background-color: #3c70e1; text-align: left; padding-left: 10px; vertical-align: middle;">Siswa
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
                                                                        Eval("Kode").ToString() == txtIDItemSiswa.Value
                                                                        ? "selectedrow" 
                                                                        : (Container.DisplayIndex % 2 == 0 ? "standardrow" : "oddrow") 
                                                                   %>">
                                                            <td style="text-align: center; padding: 10px; vertical-align: middle; color: #bfbfbf;">
                                                                <%# (Container.DisplayIndex + 1) %>.
                                                            </td>
                                                            <td style="width: 30px; text-align: center; vertical-align: middle;">
                                                                <div class="checkbox checkbox-adv" style="margin: 0 auto;">
                                                                    <label for="chk_formasi_siswa_<%# Eval("Kode").ToString().Replace("-", "_") %>">
                                                                        <input value="<%# Eval("Kode").ToString() %>"
                                                                            class="access-hide"
                                                                            id="chk_formasi_siswa_<%# Eval("Kode").ToString().Replace("-", "_") %>"
                                                                            name="chk_formasi_siswa[]"
                                                                            type="checkbox">
                                                                        <span class="checkbox-circle"></span><span class="checkbox-circle-check"></span><span class="checkbox-circle-icon icon">done</span>
                                                                    </label>
                                                                </div>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: #bfbfbf; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("NISSekolah").ToString())
                                                                    %>
                                                                </span>
                                                                <br />
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetPerbaikiEjaanNama(Eval("Siswa").ToString())
                                                                    %>
                                                                </span>
                                                                <%# 
                                                                    Eval("kelasDet").ToString().Trim() != ""
                                                                    ? "<br />" +
                                                                      "<span style=\"color: rgba(203,96,179,1); font-weight: bold; text-transform: none; text-decoration: none;\">" +
                                                                        Eval("kelasDet").ToString().Trim() +
                                                                      "</span>"
                                                                    : ""
                                                                %>
                                                                <button onclick="<%= txtIDItemSiswa.ClientID %>.value = '<%# Eval("Kode").ToString() %>';<%= btnShowEditFormasiSiswa.ClientID %>.click(); return false;" title=" Ubah " onclick="return false;" class="btn btn-flat btn-brand-accent waves-attach waves-effect pull-right"
                                                                    style="padding: 0px; padding-left: 5px; padding-right: 5px; float: right;">
                                                                    <span style="color: slategrey;">
                                                                        <i class="fa fa-pencil"></i>
                                                                    </span>
                                                                </button>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr style="background-color: #3c70e1;">
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #3c70e1; vertical-align: middle; width: 80px;">#
                                                                        </th>
                                                                        <th style="text-align: left; background-color: #3c70e1; width: 80px; vertical-align: middle;">
                                                                            <label onclick="SelectOrUnSelectAllSiswa();" style="cursor: pointer;">
                                                                                <i class="fa fa-clone"></i>
                                                                            </label>
                                                                        </th>
                                                                        <th style="background-color: #3c70e1; text-align: left; padding-left: 10px; vertical-align: middle;">Siswa
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td runat="server" colspan="3" style="text-align: center; padding: 10px;">..:: Data Kosong ::..
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </div>
                                            <asp:SqlDataSource ID="sql_ds_formasi_siswa" runat="server"></asp:SqlDataSource>

                                            <div class="content-header ui-content-header"
                                                style="background-color: #00198d; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 33px; right: 50px; width: 160px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                                                <div style="padding-left: 0px;">
                                                    <asp:LinkButton ToolTip=" Kembali " runat="server" ID="btnShowDataListFromFormasiSiswa" CssClass="btn-trans waves-attach waves-circle waves-effect" OnClick="btnShowDataList_Click" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;
                                                    </asp:LinkButton>
                                                </div>
                                            </div>

                                            <div class="content-header ui-content-header"
                                                style="background-color: #8A0083; background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 33px; right: 50px; width: 120px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                                                <div style="padding-left: 0px;">
                                                    <asp:LinkButton OnClientClick="ShowConfirmHapusSiswa(); return false;" ToolTip=" Hapus " runat="server" ID="lnkHapusItemSiswa" CssClass="btn-trans waves-attach waves-circle waves-effect" Style="color: ghostwhite;">
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

                                            <div class="fbtn-container" id="div_button_settings_formasi_siswa" runat="server">
                                                <div class="fbtn-inner">
                                                    <a onclick="setTimeout(
                                                            function(){
                                                                <%= btnShowInputFormasiSiswa.ClientID %>.click();
                                                            }, 500
                                                        ); return false;"
                                                        class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #a91212;" title=" Tambah Data ">
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

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_guru_mapel" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">Isi Data Mengajar
                                    </span>
                                </div>
                            </div>
                            <div style="width: 100%;">
                                <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtGuru.NamaClientID %>" style="text-transform: none;">Guru</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputMengajar" runat="server" ID="vldGuru"
                                                            ControlToValidate="txtGuru$txtNama" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <ucl:AutocompletePegawaiByUnit runat="server" ID="txtGuru" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="div_kelas_guru" class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboKelasGuru.ClientID %>" style="text-transform: none;">Kelas</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputMengajar" runat="server" ID="vldKelasGuru"
                                                            ControlToValidate="cboKelasGuru" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList CssClass="form-control" runat="server" ID="cboKelasGuru">
                                                            <asp:ListItem></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtKeterangan.ClientID %>" style="text-transform: none;">Keterangan</label>
                                                        <asp:TextBox runat="server" ID="txtKeterangan" CssClass="form-control"></asp:TextBox>
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
                                <asp:LinkButton ValidationGroup="vldInputMengajar" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKGuruMengajar" OnClick="lnkOKGuruMengajar_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_data" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">Isi Data
                                    </span>
                                </div>
                            </div>
                            <div style="width: 100%;">
                                <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtTahunPelajaran.ClientID %>" style="text-transform: none;">Tahun Pelajaran</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldTahunPelajaran"
                                                            ControlToValidate="txtTahunPelajaran" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ValidationGroup="vldInput" CssClass="form-control" runat="server" ID="txtTahunPelajaran"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboSemester.ClientID %>" style="text-transform: none;">Semester</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldSemester"
                                                            ControlToValidate="cboSemester" Display="Dynamic" Style="float: right; font-weight: bold;"
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
                                                <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboUnitSekolah.ClientID %>" style="text-transform: none;">Unit Sekolah</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldUnitSekolah"
                                                            ControlToValidate="cboUnitSekolah" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList runat="server" ID="cboUnitSekolah" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboMapel.ClientID %>" style="text-transform: none;">Mata Pelajaran</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldMapel"
                                                            ControlToValidate="cboMapel" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList runat="server" ID="cboMapel" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboKelas.ClientID %>" style="text-transform: none;">Level</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldKelas"
                                                            ControlToValidate="cboKelas" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList runat="server" ID="cboKelas" CssClass="form-control"></asp:DropDownList>
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
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_siswa_mapel" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">Isi Data Formasi Siswa
                                    </span>
                                </div>
                            </div>
                            <div style="width: 100%;">
                                <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtSiswaMapel.NamaClientID %>" style="text-transform: none;">NIS Sekolah</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputSiswaMapel" runat="server" ID="vldSiswaMapel"
                                                            ControlToValidate="txtSiswaMapel$txtNama" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <ucl:AutocompleteSiswaNISSekolahByUnit runat="server" ID="txtSiswaMapel" />
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
                                <asp:LinkButton ValidationGroup="vldInputSiswaMapel" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInputSiswa" OnClick="lnkOKInputSiswa_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_confirm_hapus" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_hapus_item_mengajar" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">Hapus Data
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapusItemMengajar" OnClick="lnkOKHapusItemMengajar_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_tampilan_data" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">Tampilan Data
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
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboUnit.ClientID %>" style="text-transform: none;">Unit Sekolah</label>
                                                        <asp:DropDownList runat="server" ID="cboUnit" CssClass="form-control"></asp:DropDownList>
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKTampilanData" OnClick="lnkOKTampilanData_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>

            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_hapus_item_siswa" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">Hapus Data
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapusItemSiswa" OnClick="lnkOKHapusItemSiswa_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_pilih_siswa" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" style="text-transform: none; color: grey; font-weight: bold; width: 100%;">
                                                            <asp:Literal runat="server" ID="ltrPilihSiswaCaption"></asp:Literal>
                                                        </label>
                                                        <br /><br />
                                                        <label onclick="DoCheckSiswa(true);" style="cursor: pointer; background-color: dodgerblue; font-weight: normal; font-size: smaller; color: white; padding: 3px; border-radius: 4px;">
                                                            &nbsp;&nbsp;
                                                            <i class="fa fa-check-square-o"></i>
                                                            &nbsp;
                                                            Ceklist Semua
                                                            &nbsp;&nbsp;
                                                        </label>
                                                        &nbsp;
                                                        <label onclick="DoCheckSiswa(false);" style="cursor: pointer; background-color: lightcoral; font-weight: normal; font-size: smaller; color: white; padding: 3px; border-radius: 4px;">
                                                            &nbsp;&nbsp;
                                                            <i class="fa fa-square-o"></i>
                                                            &nbsp;
                                                            Un Ceklist Semua
                                                            &nbsp;&nbsp;
                                                        </label>
                                                        <br /><br />
                                                        <asp:Literal runat="server" ID="ltrPilihSiswa"></asp:Literal>
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKPilihSiswa" OnClick="lnkOKPilihSiswa_Click" Text="   OK   "></asp:LinkButton>
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
            <asp:PostBackTrigger ControlID="btnRefresh" />
            <asp:PostBackTrigger ControlID="btnDoCari" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
        RenderDropDownOnTables();
        InitModalFocus();
        <%= txtGuru.NamaClientID %>_SHOW_AUTOCOMPLETE();
        <%= txtSiswaMapel.NamaClientID %>_SHOW_AUTOCOMPLETE();
    </script>
</asp:Content>
