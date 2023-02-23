<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.NilaiSiswa.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Penilaian.SD.wf_NilaiSiswa" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- handsontable -->
    <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/handson-table/bower_components/handsontable/dist/handsontable.full.js") %>"></script>
    <link rel="stylesheet" media="screen" href="<%= ResolveUrl("~/Application_CLibs/handson-table/bower_components/handsontable/dist/handsontable.full.min.css") %>">
    <link rel="stylesheet" media="screen" href="<%= ResolveUrl("~/Application_CLibs/handson-table/css/samples.css") %>">

    <!-- ruleJS -->
    <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/handson-table/bower_components/ruleJS/dist/full/ruleJS.all.full.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/handson-table/src/handsontable.formula.js") %>"></script>
    <link rel="stylesheet" media="screen" href="<%= ResolveUrl("~/Application_CLibs/handson-table/src/handsontable.formula.css") %>">

    <style type="text/css">
        .handsontable .htBG19{
          background-color: #FFCBC9;
          font-weight: bold;
        }

        .handsontable .htBG20{
          background-color: #FFE0DF;
          font-weight: bold;
        }

        .handsontable .htBorderRightNKDUKK {
            border-right-style: solid;
            border-right-width: 3px;
            border-right-color: #b71d0d;
        }
    </style>

    <script type="text/javascript">
        function HideModal() {
            $('#ui_modal_pilih_kd_lts').modal('hide');
            $('#ui_modal_isi_cust_nilai').modal('hide');
            $('#ui_modal_proses').modal('hide');     
            $('#ui_modal_pilihan').modal('hide');     

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
                case "<%= JenisAction.ShowPengaturanLTS %>":
                    $('#ui_modal_pilih_kd_lts').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoUpdate %>":
                    document.getElementById("<%= txtKeyAction.ClientID %>").value = "";
                    HideModal();
                    OKSaveNilaiRapor();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah diupdate',
                        show: function () {
                            snackbarText++;
                        }
                    });                    
                    break;
                case "<%= JenisAction.DoUpdateNilaiRapor %>":
                    document.getElementById("<%= txtKeyAction.ClientID %>").value = "";
                    break;
            }

            document.getElementById("<%= txtKeyAction.ClientID %>").value = "";
            InitModalFocus();
        }
    </script>

    <script type="text/javascript">
        var resizeTimer;

        function Resize() {
            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(function () {

                if (typeof Maximize === 'function') Maximize();

            }, 100);
        }

        function ResponseRedirect(url) {
            document.location.href = url;
        }

        function ParseKDLTS() {
            var txt = document.getElementById("<%= txtParseKD.ClientID %>");
	        if (txt !== null && txt !== undefined) {
	            var arr = document.getElementsByName("chk_kd[]");
	            var id = 1;
	            var s_value = "";
	            if (arr.length > 0) {
	                for (var i = 0; i < arr.length; i++) {
	                    if (arr[i].checked) {
	                        s_value += arr[i].value + "|" + id.toString() + ";"
	                        id++;
	                    }
	                }
	            }
	            txt.value = s_value;
	        }
        }

        function InitModalFocus(){
            $('#ui_modal_isi_cust_nilai').on('shown.bs.modal', function () {
                <%= txtUbahNilai.ClientID %>.focus();
                <%= txtUbahNilai.ClientID %>.select();
            });
        }

        function ShowUpdateNilai(nilai, row, col, rel_siswa, nis, nama, ap, kd){
            var txt_nis = document.getElementById("<%= txtNIS.ClientID %>");
            var txt_nama = document.getElementById("<%= txtNamaSiswa.ClientID %>");
            var txt_siswa = document.getElementById("<%= txtRelSiswa.ClientID %>");
            var txt_nilai = document.getElementById("<%= txtUbahNilai.ClientID %>");
            var txt_col = document.getElementById("<%= txtCol.ClientID %>");
            var txt_row = document.getElementById("<%= txtRow.ClientID %>");
            var txt_ap = document.getElementById("<%= txtRelRaporStrukturNilai_AP.ClientID %>");
            var txt_kd = document.getElementById("<%= txtRelRaporStrukturNilai_KD.ClientID %>");

            if(txt_nis !== null && txt_nis !== undefined){
                txt_nis.value = nis; 
            }
            if(txt_nama !== null && txt_nama !== undefined){
                txt_nama.value = nama; 
            }
            if(txt_nilai !== null && txt_nilai !== undefined){
                txt_nilai.value = nilai; 
            }
            if(txt_col !== null && txt_col !== undefined){
                txt_col.value = col; 
            }
            if(txt_row !== null && txt_row !== undefined){
                txt_row.value = row; 
            }
            if(txt_siswa !== null && txt_siswa !== undefined){
                txt_siswa.value = rel_siswa; 
            }
            if(txt_ap !== null && txt_ap !== undefined){
                txt_ap.value = ap; 
            }
            if(txt_kd !== null && txt_kd !== undefined){
                txt_kd.value = kd; 
            }
            $('#ui_modal_isi_cust_nilai').modal({ backdrop: 'static', keyboard: false, show: true });
        }

        function GetFindFormula(row, col){                   
            if(arr_formula.length > 0){
                for (var i = 0; i < arr_formula.length; i++) {
                    if(arr_formula[i].row.toString().trim() === row.toString().trim() && arr_formula[i].col.toString().trim() === col.toString().trim()){                        
                        return arr_formula[i].formula;
                    }
                }
            }
            return "";
        }

        function GetNilaiRapor(){
            var txt_col = document.getElementById("<%= txtCol.ClientID %>");
            var txt_row = document.getElementById("<%= txtRow.ClientID %>");
            var txt_id_col_rapor = document.getElementById("<%= txtIDColRapor.ClientID %>");
            if(txt_col !== null && txt_col !== undefined &&
               txt_row !== null && txt_row !== undefined &&
               txt_id_col_rapor !== null && txt_id_col_rapor !== undefined){
                var cellId = hot.plugin.utils.translateCellCoords({row: + txt_row.value, col: txt_col.value});
                var formula = hot.getDataAtCell(txt_row.value, txt_id_col_rapor.value);
                if(formula !== null && formula !== undefined){
                    if(formula.length > 0){
                        formula = formula.substr(1).toUpperCase();
                        var newValue = hot.plugin.parse(formula, {row: txt_row.value, col: txt_id_col_rapor.value, id: cellId});
                        var nr = (newValue.result);
                        return nr;             
                    }
                }
            }

            return "";
        }

        function OKSaveNilaiRapor(){
            var txt_nilai_rapor = document.getElementById("<%= txtNilaiRapor.ClientID %>");
            if(txt_nilai_rapor !== null && txt_nilai_rapor !== undefined){
                txt_nilai_rapor.value = GetNilaiRapor();
                <%= btnDoSaveNilaiRapor.ClientID %>.click();
            }
        }

        function OKUpdateNilai(){
            var txt_col = document.getElementById("<%= txtCol.ClientID %>");
            var txt_row = document.getElementById("<%= txtRow.ClientID %>");
            var txt_nilai = document.getElementById("<%= txtUbahNilai.ClientID %>");
            var txt_kkm = document.getElementById("<%= txtKKM.ClientID %>");
            
            if(txt_col !== null && txt_col !== undefined &&
               txt_row !== null && txt_row !== undefined &&
               txt_nilai !== null && txt_nilai !== undefined &&
               txt_kkm !== null && txt_kkm !== undefined
               ){

                if(parseFloat(txt_nilai.value) > parseFloat(txt_kkm.value)){
                    txt_nilai.value = txt_kkm.value;
                }
                data_nilai[txt_row.value][txt_col.value] = "=" + txt_nilai.value;

                hot.render(); HideModal();
                $(hot.getCell(txt_row.value, txt_col.value)).css({"background-color": "yellow"});
            }
        }

        function OKUpdateKembalikanNilai(){
            var txt_col = document.getElementById("<%= txtCol.ClientID %>");
            var txt_row = document.getElementById("<%= txtRow.ClientID %>");
            var txt_nilai = document.getElementById("<%= txtUbahNilai.ClientID %>");
            var txt_kkm = document.getElementById("<%= txtKKM.ClientID %>");
            
            if(txt_col !== null && txt_col !== undefined &&
               txt_row !== null && txt_row !== undefined &&
               txt_nilai !== null && txt_nilai !== undefined &&
               txt_kkm !== null && txt_kkm !== undefined
               ){

                data_nilai[txt_row.value][txt_col.value] = GetFindFormula(txt_row.value, txt_col.value);
                hot.render(); HideModal();
                $(hot.getCell(txt_row.value, txt_col.value)).css({"background-color": "#ACEBF9"});
            }
        }

        function GetNilaiFromFormula(row, col){
            var cellId = hot.plugin.utils.translateCellCoords({row: row, col: col});
            var formula = hot.getDataAtCell(row, col);
            formula = formula.substr(1).toUpperCase();
            var newValue = hot.plugin.parse(formula, {row: row, col: col, id: cellId});
            var nr = (newValue.result);

            return nr;
        }

        function ShowProgress(show){
            if(show){
                $('#ui_modal_proses').modal({ backdrop: 'static', keyboard: false, show: true });
            }
            else {
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                $('#ui_modal_proses').modal('hide');
                HideModal();
            }
        }
    </script>

    <style type="text/css">
        .zebraStyle > tbody > tr:nth-child(2n) > td {
            font-size: 9pt;
            vertical-align: middle;
        }

        .zebraStyle > tbody > tr:nth-child(2n+1) > td {
            font-size: 9pt;
            vertical-align: middle;
        }

        .zebraStyle > tbody > tr:nth-child(1) > td {
            font-size: 9pt;
            background: white;
            color: black;
            font-weight: bold;
            padding-left: 5px;
            padding-right: 5px;
            text-align: center;
            /*box-shadow: 1px 2px 1px #d7d7d7;*/
        }

        .zebraStyle > tbody > tr:nth-child(2) > td {
            font-size: 9pt;
            background: white;
            color: black;
            padding-left: 5px;
            padding-right: 5px;
            text-align: center;
            padding-top: 1px;
            padding-bottom: 1px;
            font-weight: bold;
        }

        .zebraStyle > tbody > tr:nth-child(3) > td {
            font-size: 9pt;
            background: white;
            font-weight: bold;
            padding-left: 5px;
            padding-right: 5px;
            text-align: center;
        }

        .handsontable col.rowHeader {
            width: 0px;
        }

        .bgManual {
            background-color: yellow;
            font-weight: bold;
        }
    </style>

    <style type="text/css">
        .modal-backdrop {
            opacity: 0 !important;
        }

        /* Messy stack of paper */
        .paper {
            background: #fff;
            padding: 30px;
            position: relative;
        }

            .paper,
            .paper::before,
            .paper::after {
                /* Styles to distinguish sheets from one another */
                box-shadow: 1px 1px 1px rgba(0,0,0,0.25);
                border: 1px solid #bbb;
            }

                .paper::before,
                .paper::after {
                    content: "";
                    position: absolute;
                    height: 95%;
                    width: 99%;
                    background-color: #eee;
                }

                .paper::before {
                    right: 15px;
                    top: 0;
                    transform: rotate(-1deg);
                    z-index: -1;
                }

                .paper::after {
                    top: 5px;
                    right: -5px;
                    transform: rotate(1deg);
                    z-index: -2;
                }
    </style>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpNomor2" runat="server">
    <asp:HiddenField runat="server" ID="txtKTSPColKD" />
    <asp:HiddenField runat="server" ID="txtSemester" />
    <asp:HiddenField runat="server" ID="txtMapel" />    
    <asp:HiddenField runat="server" ID="txtIDColRapor" />    

    <asp:Button UseSubmitBehavior="false" runat="server" ID="btnOKShowBySemester" OnClick="btnOKShowBySemester_Click" Style="position: absolute; left: -1000px; top: -1000px;" />

    <div id="div_nilai"
        style="overflow: hidden; position: absolute; top: 100px; bottom: 0px; left: 5px; right: 10px; font-size: small; color: grey;">
        <asp:Literal runat="server" ID="ltrCenter"></asp:Literal>        
    </div>

    <div id="div_statusbar" runat="server">
        <asp:Literal runat="server" ID="ltrStatusBar"></asp:Literal>
    </div>

    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="txtKeyAction" />
            <asp:HiddenField runat="server" ID="txtKKM" />
            <asp:HiddenField runat="server" ID="txtParseKD" />
            <asp:HiddenField runat="server" ID="txtRow" />
            <asp:HiddenField runat="server" ID="txtCol" />
            <asp:HiddenField runat="server" ID="txtRelSiswa" />
            <asp:HiddenField runat="server" ID="txtRelRaporNilaiSiswa" />
            <asp:HiddenField runat="server" ID="txtRelRaporStrukturNilai_AP" />
            <asp:HiddenField runat="server" ID="txtRelRaporStrukturNilai_KD" />
            <asp:HiddenField runat="server" ID="txtNilaiRapor" />

            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoSaveNilaiRapor" OnClick="btnDoSaveNilaiRapor_Click" Style="position: absolute; left: -1000px; top: -1000px;" />

            <div class="fbtn-container" id="div_button_settings" runat="server" style="z-index: 999999;">
                <div class="fbtn-inner">
                    <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #329CC3;" title=" Pengaturan ">
                        <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                        <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                    </a>
                    <div class="fbtn-dropup" style="z-index: 999999;">
                        <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkPengaturanLTS" OnClick="lnkPengaturanLTS_Click" Style="background-color: #ff0000;">
                            <span class="fbtn-text fbtn-text-left">Pengaturan LTS</span>
                            <i class="fa fa-file-archive-o" style="color: white;"></i>
                        </asp:LinkButton>
                        <a data-toggle="modal" href="#ui_modal_pilihan" class="fbtn fbtn-green waves-attach waves-circle waves-effect" style="cursor: pointer; background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Pilih Data Nilai</span>
                            <i class="fa fa-list" style="color: white;"></i>
                        </a>
                        <asp:LinkButton Visible="false" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkPilihKelas" OnClick="lnkPilihKelas_Click" Style="background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Pilih Kelas Lain</span>
                            <i class="fa fa-th-large" style="color: white;"></i>
                        </asp:LinkButton>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_pilihan" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; 
                            padding-left: 0px; padding-right: 0px; padding-bottom: 0px;
                            background-color: #EDEDED;
                            background-repeat: no-repeat;
                            background-position-y: -1px;
                            background-size: auto;
                            background-position: right;">
                            <div style="background-color: #295BC8; padding: 10px; font-weight: normal; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px; padding-left: 25px;
                                        <asp:Literal runat="server" ID="ltrHeaderPilihan"></asp:Literal>">
                                <asp:Literal runat="server" ID="ltrCaption"></asp:Literal>
                            </div>
                            <div style="width: 100%;">
                                <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 0px;">
                                            <div runat="server" id="div_unit_sekolah_biodata_siswa" class="row" style="padding-left: 0px; padding-right: 0px;">
                                                <div class="col-xs-12" style="margin-left: 0px; margin-right: 0px;">

                                                    <div class="card" style="margin-bottom: 0px; margin-top: 0px; box-shadow: none; border-style: none;">
                                                        <div class="card-main">
                                                            <nav class="tab-nav margin-top-no margin-bottom-no">
                                                                <ul class="nav nav-justified" style="background-color: #f1f1f1; <asp:Literal runat="server" ID="ltrHeaderTab"></asp:Literal>border-top-style: solid; border-top-width: 0px; border-top-color: #bfbfbf;">
                                                                    <li class="active">
                                                                        <a class="waves-attach" data-toggle="tab" href="#ui_tab_akademik" style="font-weight: bold; text-transform: none; color: black; line-height: 15px; padding-top: 15px; padding-bottom: 15px; color: white;">
                                                                            Nilai<br />Akademik
                                                                        </a>
                                                                    </li>
                                                                    <li runat="server" id="li_nilai_ekskul">
                                                                        <a class="waves-attach" data-toggle="tab" href="#ui_tab_ekskul" style="font-weight: bold; text-transform: none; color: black; line-height: 15px; padding-top: 15px; padding-bottom: 15px; color: white;">
                                                                            Ekstra<br />Kurikuler
                                                                        </a>
                                                                    </li>
                                                                    <li>
                                                                        <a class="waves-attach" data-toggle="tab" href="#ui_tab_sikap" style="font-weight: bold; text-transform: none; color: black; line-height: 15px; padding-top: 15px; padding-bottom: 15px; color: white;">
                                                                            Nilai<br />Sikap
                                                                        </a>
                                                                    </li>
                                                                    <li runat="server" id="li_nilai_volunteer">
                                                                        <a class="waves-attach" data-toggle="tab" href="#ui_tab_volunteer" style="font-weight: bold; text-transform: none; color: black; line-height: 15px; padding-top: 15px; padding-bottom: 15px; color: white;">
                                                                            Kegiatan<br />Volunteer
                                                                        </a>
                                                                    </li>
                                                                    <li runat="server" id="li_nilai_rapor">
                                                                        <a class="waves-attach" data-toggle="tab" href="#ui_tab_lts" style="font-weight: bold; text-transform: none; color: black; line-height: 15px; padding-top: 15px; padding-bottom: 15px; color: white;">
                                                                            Nilai<br />Rapor
                                                                        </a>
                                                                    </li>
                                                                </ul>
                                                            </nav>
                                                            <div class="card-inner" style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
                                                                <div class="tab-content">
                                                                    <div class="tab-pane fade active in" id="ui_tab_akademik">
                                                                        <asp:Literal runat="server" ID="ltrListNilaiAkademik"></asp:Literal>
                                                                    </div>
                                                                    <div class="tab-pane fade" id="ui_tab_ekskul">
                                                                        <asp:Literal runat="server" ID="ltrListNilaiEkskul"></asp:Literal>
                                                                    </div>
                                                                    <div class="tab-pane fade" id="ui_tab_sikap">
                                                                        <asp:Literal runat="server" ID="ltrListNilaiSikap"></asp:Literal>
                                                                    </div>
                                                                    <div class="tab-pane fade" id="ui_tab_volunteer">
                                                                        <asp:Literal runat="server" ID="ltrListNilaiVolunteer"></asp:Literal>
                                                                    </div>
                                                                    <div class="tab-pane fade" id="ui_tab_lts">
                                                                        <asp:Literal runat="server" ID="ltrListNilaiRapor"></asp:Literal>
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
                            <p class="text-center">
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Tutup</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_pilih_kd_lts" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Pilih Kompetensi Dasar
                                    </span>
                                </div>
                            </div>
                            <div style="width: 100%;">
                                <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div runat="server" id="div1" class="row" style="padding-left: 15px; padding-right: 15px;">
                                                <div class="col-xs-12" style="margin-left: 0px; margin-right: 0px;">
                                                    <label style="color: grey; font-weight: bold; font-weight: normal; margin-bottom: 10px;">
                                                        Pilih kompetensi dasar (KD) untuk di tampilkan di LTS 
                                                    </label>

                                                    <div class="card" style="margin-bottom: 0px; margin-top: 0px; box-shadow: none; border-style: none;">
                                                        <div class="card-main">
                                                            <asp:Literal runat="server" ID="ltrPilihKD"></asp:Literal>
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
                                <asp:LinkButton OnClientClick="ShowProgress(true); ParseKDLTS();" ValidationGroup="vldInput" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKPilihKD" OnClick="lnkOKPilihKD_Click" Text="   SIMPAN   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a onclick="ShowProgress(false);" class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_isi_cust_nilai" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Isi Nilai Manual
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
                                                        <label class="label-input" for="<%= txtNIS.ClientID %>" style="text-transform: none;">NIS</label>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtNIS" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtNamaSiswa.ClientID %>" style="text-transform: none;">Nama Siswa</label>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtNamaSiswa" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtUbahNilai.ClientID %>" style="text-transform: none;">Ubah Nilai</label>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtUbahNilai"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <br />
                            <asp:LinkButton OnClientClick="OKUpdateKembalikanNilai();" ValidationGroup="vldInput" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKUpdateKembalikan" OnClick="lnkOKUpdateKembalikan_Click" Text="   KEMBALIKAN NILAI  " ToolTip=" Kembalikan Nilai Sesuai Formula " style="float: left;"></asp:LinkButton>
                            <a onclick="ShowProgress(false);" class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal" style="float: right;">Batal</a>                            
                            &nbsp;&nbsp;
                            <asp:LinkButton OnClientClick="OKUpdateNilai();" ValidationGroup="vldInput" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKUpdateNilai" OnClick="lnkOKUpdateNilai_Click" Text="   SIMPAN   " style="float: right;"></asp:LinkButton>                            
                            <br /><br /><br />
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

    <script type="text/javascript">
        setTimeout(function () {

            var txt = document.getElementById("<%= txtSemester.ClientID %>");
            if (txt != null && txt != undefined) {
                if (txt.value.trim() === "") {
                    $('#ui_modal_pilihan').modal({ show: true });
                }
            }

        }, 200);
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <asp:MultiView runat="server" ID="mvMain" ActiveViewIndex="0">
        <asp:View runat="server" ID="vNilaiSiswa">
            <asp:Literal runat="server" ID="ltrHOT"></asp:Literal>
        </asp:View>
    </asp:MultiView>

    <script type="text/javascript">
        InitModalFocus();
    </script>
</asp:Content>
