<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="wf.NilaiSiswa.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP.wf_NilaiSiswa" %>
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
            $('#ui_modal_statistik_nilai').modal('hide');
            $('#ui_modal_pilih_siswa').modal('hide');
            $('#ui_modal_proses').modal('hide');               

            document.body.style.paddingRight = "0px";

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
                case "<%= JenisAction.DoShowStatistik %>":
                    $('#ui_modal_statistik_nilai').modal({ backdrop: 'static', keyboard: false, show: true });
                    ShowDataStatistik();
                    break;
                case "<%= JenisAction.DoShowPilihSiswa %>":
                    $('#ui_modal_pilih_siswa').modal({ backdrop: 'static', keyboard: false, show: true });
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
                    setTimeout(function(){
                        location.reload();
                    }, 500);
                    break;
            }

            document.getElementById("<%= txtKeyAction.ClientID %>").value = "";
        }

        function ResponseRedirect(url) {
            document.location.href = url;
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

    <script type="text/javascript">
	    var resizeTimer;

	    function Resize(){
	        clearTimeout(resizeTimer);
	        resizeTimer = setTimeout(function() {

	            if (typeof Maximize === 'function') Maximize();
				
	        }, 100);
	    }

        function ShowDataStatistik() {
	        if (hot != null && hot != undefined) {
	            <asp:Literal runat="server" ID="ltrJSStatistik"></asp:Literal>
	            var data = hot.getData();
	            var kkm = 0;
	            var txt_kkm = document.getElementById("<%= txtKKM.ClientID %>");
                var jml_des = <%= AI_ERP.Application_Libs.Constantas.PEMBULATAN_DESIMAL_NILAI_SMA.ToString() %>;
	            if(txt_kkm != undefined && txt_kkm != null) kkm = parseFloat(txt_kkm.value);
	            if(data != null){
	                for (var i = 0; i < arr_col.length; i++) {
	                    var arr_nilai = [];
	                    var c = arr_col[i];
	                    var sum = 0;
	                    var i_lb_kkm = 0;
	                    var i_lk_kkm = 0;
	                    var i_jml = 0;
	                    for(var r = 0; r < data.length; r++){
	                        if(r >= id_fixed_row){
	                            var nilai = -99;
	                            i_jml++;

	                            cellId = hot.plugin.utils.translateCellCoords({row: r, col: c});
	                            formula = hot.getDataAtCell(r, c);
	                            if(formula.indexOf("=") === 0){
	                                formula = formula.substr(1).toUpperCase();
	                                newValue = hot.plugin.parse(formula, {row: r, col: c, id: cellId});
	                                if(newValue.result.toString().trim() !== ""){
	                                    nilai = parseFloat(newValue.result == null ? 0 : newValue.result);	                            
	                                }
	                            }
	                            else {
	                                if(formula.trim() !== ""){
	                                    nilai = parseFloat(formula.trim());
	                                }
	                            }
	                            if(nilai !== -99){
	                                arr_nilai.push(nilai);
	                                sum += nilai;
	                                if(nilai >= kkm){
	                                    i_lb_kkm++;
	                                }
	                                else {
	                                    i_lk_kkm++;
	                                }
	                            }
	                            else {
	                                i_lk_kkm++;
	                            }
	                        }
	                    }

	                    var td_avg = document.getElementById("td_" + c.toString() + "_1");
	                    if(td_avg != undefined && td_avg != null){
	                        if(arr_nilai.length > 0){
	                            var hasil = Math.round((sum/arr_nilai.length), jml_des);
	                            if(hasil < kkm){
	                                td_avg.innerHTML = "<span style=\"color: red;\">" + hasil + "</span>";	                        
	                            } else {
	                                td_avg.innerHTML = hasil;
	                            }	                     
	                        }
	                        else {
	                            td_avg.innerHTML = "";
	                        }
	                    }

	                    var td_max = document.getElementById("td_" + c.toString() + "_2");
	                    if(td_max != undefined && td_max != null){
	                        if(arr_nilai.length > 0){
	                            var hasil = Math.max.apply(null, arr_nilai);
	                            if(hasil < kkm){
	                                td_max.innerHTML = "<span style=\"color: red;\">" + hasil + "</span>";	                        
	                            } else {
	                                td_max.innerHTML = hasil;
	                            }	                     
	                        }
	                        else {
	                            td_avg.innerHTML = "";
	                        }
	                    }

	                    var td_min = document.getElementById("td_" + c.toString() + "_3");
	                    if(td_min != undefined && td_min != null){
	                        if(arr_nilai.length > 0){
	                            var hasil = Math.min.apply(null, arr_nilai);
	                            if(hasil < kkm){
	                                td_min.innerHTML = "<span style=\"color: red;\">" + hasil + "</span>";	                        
	                            } else {
	                                td_min.innerHTML = hasil;	                        
	                            }	                     
	                        }
	                        else {
	                            td_avg.innerHTML = "";
	                        }
	                    }

	                    var td_lb_kkm = document.getElementById("td_" + c.toString() + "_4");
	                    if(td_lb_kkm != undefined && td_lb_kkm != null){
	                        td_lb_kkm.innerHTML = i_lb_kkm + "&nbsp;<sup style=\"font-weight: normal;\">siswa</sup>";
	                    }

	                    var td_lb_kkm_p = document.getElementById("td_" + c.toString() + "_5");
	                    if(td_lb_kkm_p != undefined && td_lb_kkm_p != null){
	                        td_lb_kkm_p.innerHTML = Math.round((i_lb_kkm/i_jml) * 100, jml_des).toString() + 
                                                    "&nbsp;<sup style=\"font-weight: normal;\">%</sup>";
	                    }

	                    var td_lk_kkm = document.getElementById("td_" + c.toString() + "_6");
	                    if(td_lk_kkm != undefined && td_lk_kkm != null){
	                        td_lk_kkm.innerHTML = i_lk_kkm + "&nbsp;<sup style=\"font-weight: normal;\">siswa</sup>";
	                    }

	                    var td_lk_kkm_p = document.getElementById("td_" + c.toString() + "_7");
	                    if(td_lk_kkm_p != undefined && td_lk_kkm_p != null){
	                        td_lk_kkm_p.innerHTML = Math.round((i_lk_kkm/i_jml) * 100, jml_des).toString() + 
                                                    "&nbsp;<sup style=\"font-weight: normal;\">%</sup>";
	                    }
	                }
	            }
	        }
        }

        function DoParseSiswaPilihan(){
            var txt = document.getElementById("<%= txtParseSiswaPilihan.ClientID %>");
            var arr_chk_siswa = document.getElementsByName("chk_siswa[]");
            if(txt !== undefined && txt !== null){
                if(arr_chk_siswa.length > 0){
                    for (var i = 0; i < arr_chk_siswa.length; i++) {
                        if(arr_chk_siswa[i].checked){
                            txt.value += arr_chk_siswa[i].value + ";";
                        }
                    }
                }
            }
        }

        function SelectPilihanSiswa(checked){
            var arr_chk_siswa = document.getElementsByName("chk_siswa[]");
            if(arr_chk_siswa.length > 0){
                for (var i = 0; i < arr_chk_siswa.length; i++) {
                    arr_chk_siswa[i].checked = checked;
                }
            }
        }

        function ShowProgressSaveData(teks){
            var div = document.getElementById("div_savenilai");
            if(div !== null & div !== undefined){
                div.style.position = 'absolute';
                div.style.left = '46.5%';
                div.style.top = '50%';
                div.style.zIndex = '9999999';
                div.style.padding = '15px';
                div.style.backgroundColor = 'black';
                div.style.color = 'white';
                div.style.width = '250px';
                div.style.textAlign = 'center';
                div.style.display = '';
                lblStatusProses.innerHTML = teks;
            }
        }

        function SetProgressSaveDataValue(teks){
            var lbl = document.getElementById("lblStatusProsesValue");
            if(lbl !== null & lbl !== undefined){
                lbl.value = teks;
            }
        }

        function HideProgressSaveData(){
            var div = document.getElementById("div_savenilai");
            if(div !== null & div !== undefined){
                div.style.display = 'none';
            }
        }

        function CekStatusSaveData(){
            var lbl = document.getElementById("lblStatusProsesValue");
            if(lbl !== null & lbl !== undefined){
                if(lbl.value === '0' || parseInt(lbl.value) >= 100 || lbl.value === '100'){
                    HideProgressSaveData();
                }
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

        .handsontable .htBG9{
            background-color: #C9DFEF;
            font-style: italic;
        }

        .handsontable .htBG10{
            background-color: #d0ebff;
            font-style: italic;
        }
    </style>

    <style type="text/css">
        .modal-backdrop
        {
            opacity:0 !important;
        }

        .hideColumn {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpNomor2" runat="server">
    
    <asp:HiddenField runat="server" ID="txtKKM" />
    <asp:HiddenField runat="server" ID="txtKTSPColKD" />
    <asp:HiddenField runat="server" ID="txtSemester" />
    <asp:HiddenField runat="server" ID="txtMapel" />    
    <asp:HiddenField runat="server" ID="txtParseSiswaPilihan" />    

    <asp:Button UseSubmitBehavior="false" runat="server" ID="btnOKShowBySemester" OnClick="btnOKShowBySemester_Click" style="position: absolute; left: -1000px; top: -1000px;" />

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

            <div class="fbtn-container" id="div_button_settings" runat="server" style="z-index: 999999;">
		        <div class="fbtn-inner">
			        <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #329CC3;" title=" Pengaturan ">
                        <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                        <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                    </a>
                    <div class="fbtn-dropup" style="z-index: 999999;">
                        <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkPilihSiswa" OnClick="lnkPilihSiswa_Click" style="background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Pilih Siswa</span>
                            <i class="fa fa-users" style="color: white;"></i>
                        </asp:LinkButton>
                        <a data-toggle="modal" href="#ui_modal_pilihan" class="fbtn fbtn-green waves-attach waves-circle waves-effect" style="cursor: pointer; background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Pilih Data Nilai</span>
                            <i class="fa fa-list" style="color: white;"></i>
                        </a>                        
                        <asp:LinkButton CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkShowStatistics" OnClick="lnkShowStatistics_Click" style="background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Statistik Penilaian</span>
                            <i class="fa fa-bar-chart" style="color: white;"></i>
                        </asp:LinkButton>
                    </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_statistik_nilai" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 9999999;">
		        <div class="modal-dialog" style="max-width: 1200px;">
			        <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
				        <div class="modal-inner" 
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; 
                            padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; 
                            border-top-left-radius: 5px;
                            border-top-right-radius: 5px;
                            background-color: #045AB0;
                            background-repeat: no-repeat;
                            background-size: auto;
                            background-position: right; 
                            background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
							    <div class="media-object margin-right-sm pull-left">
								    <span class="icon icon-lg text-brand-accent" style="color: white;">info_outline</span>
							    </div>
							    <div class="media-inner">
								    <span style="font-weight: bold; color: white;">
                                        Statistik Penilaian
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 10px;">
                                            <div class="row" style="margin-left: 1px; margin-right: 0px;">
                                                <div class="col-xs-12">

                                                    <div class="row">
                                                        <div class="col-xs-12" style="color: grey; vertical-align: bottom; padding-left: 12px; padding-right: 12px;">
                                                            <asp:Literal runat="server" ID="ltrStatistikPenilaian"></asp:Literal>
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
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Tutup</a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_pilihan" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
			        <div class="modal-content" style="border: none;">
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
                                            <div runat="server" id="div_popup_nilai" class="row" style="margin-left: 0px; margin-right: 0px;">

                                                <div class="card" style="margin-bottom: 0px; margin-top: 0px; box-shadow: none; border-style: none;">
                                                    <div class="card-main">
                                                        <nav class="tab-nav margin-top-no margin-bottom-no">
                                                            <ul class="nav nav-justified" style="background-color: #f1f1f1; <asp:Literal runat="server" ID="ltrHeaderTab"></asp:Literal>border-top-style: solid; border-top-width: 0px; border-top-color: #bfbfbf;">
                                                                <li class="active" runat="server" id="li_nilai_akademik">
                                                                    <a class="waves-attach" data-toggle="tab" href="#ui_tab_akademik" style="font-weight: bold; text-transform: none; color: white; line-height: 15px; padding-top: 15px; padding-bottom: 15px;">
                                                                        Nilai
                                                                        <br />
                                                                        Akademik
                                                                    </a>
                                                                </li>
                                                                <li runat="server" id="li_nilai_sikap">
                                                                    <a class="waves-attach" data-toggle="tab" href="#ui_tab_sikap" style="font-weight: bold; text-transform: none; color: white; line-height: 15px; padding-top: 15px; padding-bottom: 15px;">
                                                                        Nilai
                                                                        <br />
                                                                        Sikap
                                                                    </a>
                                                                </li>
                                                                <li runat="server" id="li_nilai_ekskul">
                                                                    <a class="waves-attach" data-toggle="tab" href="#ui_tab_ekskul" style="font-weight: bold; text-transform: none; color: white; line-height: 15px; padding-top: 15px; padding-bottom: 15px;">
                                                                        Nilai
                                                                        <br />
                                                                        Ekstrakurikuler
                                                                    </a>
                                                                </li>
                                                                <li runat="server" id="li_nilai_rapor">
                                                                    <a class="waves-attach" data-toggle="tab" href="#ui_tab_lts" style="font-weight: bold; text-transform: none; color: white; line-height: 15px; padding-top: 15px; padding-bottom: 15px;">
                                                                        Nilai
                                                                        <br />
                                                                        Rapor
                                                                    </a>
                                                                </li>
                                                            </ul>
                                                        </nav>
                                                        <div class="card-inner" style="margin-left: 0px; margin-right: 0px; margin-top: 0px; margin-bottom: 0px;">
                                                            <div class="tab-content">
                                                                <div 
                                                                    <%= 
                                                                        (
                                                                            AI_ERP.Application_DAOs.DAO_Mapel.GetJenisMapel(AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP.wf_NilaiSiswa.QS.GetMapel()) == AI_ERP.Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER ||
                                                                            AI_ERP.Application_DAOs.DAO_Mapel.GetJenisMapel(AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP.wf_NilaiSiswa.QS.GetMapel()) == AI_ERP.Application_Libs.Libs.JENIS_MAPEL.EKSKUL
                                                                        ) 
                                                                        ? " class=\"tab-pane fade\" "
                                                                        : " class=\"tab-pane fade active in\" "
                                                                    %>
                                                                    id="ui_tab_akademik">
                                                                    <asp:Literal runat="server" ID="ltrListNilaiAkademik"></asp:Literal>
                                                                </div>
                                                                <div 
                                                                    <%= 
                                                                        (
                                                                            AI_ERP.Application_DAOs.DAO_Mapel.GetJenisMapel(AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP.wf_NilaiSiswa.QS.GetMapel()) == AI_ERP.Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER ||
                                                                            AI_ERP.Application_DAOs.DAO_Mapel.GetJenisMapel(AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP.wf_NilaiSiswa.QS.GetMapel()) == AI_ERP.Application_Libs.Libs.JENIS_MAPEL.EKSKUL
                                                                        ) 
                                                                        ? " class=\"tab-pane fade active in\" "
                                                                        : " class=\"tab-pane fade\" "
                                                                    %>
                                                                    id="ui_tab_sikap">
                                                                    <asp:Literal runat="server" ID="ltrListSikap"></asp:Literal>
                                                                </div>
                                                                <div 
                                                                    <%= 
                                                                        (
                                                                            AI_ERP.Application_DAOs.DAO_Mapel.GetJenisMapel(AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP.wf_NilaiSiswa.QS.GetMapel()) == AI_ERP.Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER ||
                                                                            AI_ERP.Application_DAOs.DAO_Mapel.GetJenisMapel(AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP.wf_NilaiSiswa.QS.GetMapel()) == AI_ERP.Application_Libs.Libs.JENIS_MAPEL.EKSKUL
                                                                        ) 
                                                                        ? " class=\"tab-pane fade active in\" "
                                                                        : " class=\"tab-pane fade\" "
                                                                    %>
                                                                    id="ui_tab_ekskul">
                                                                    <asp:Literal runat="server" ID="ltrListEkskul"></asp:Literal>
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
				        <div class="modal-footer">
                            <p class="text-center">
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                <br />
                                <br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_pilih_siswa" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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

                                        <div style="width: 100%; background-color: white; padding-top: 10px;">
                                            <div class="row" style="margin-left: 1px; margin-right: 0px;">
                                                <div class="col-xs-12">

                                                    <div class="row">
                                                        <div class="col-xs-12" style="color: grey; vertical-align: bottom; padding-left: 0px; padding-right: 0px;">
                                                            <asp:Literal runat="server" ID="ltrListSiswaMapelPilihan"></asp:Literal>
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
                                <asp:LinkButton OnClientClick="ShowProgress(true); DoParseSiswaPilihan();" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKPilihSiswa" OnClick="lnkOKPilihSiswa_Click" Text="   SIMPAN   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a onclick="HideModal();" class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
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

            <div id="div_savenilai" style="position: absolute; left: 0px; top: 0px; z-index: 9999999; padding: 15px; display: none;">
                Sedang proses menyimpan data<br />
                <h2 style="margin: 0px;"><label style="margin: 0 auto; display: table; font-weight: bold; color: yellow;" id="lblStatusProses"></label></h2>
                <input type="hidden" id="lblStatusProsesValue" value="0"></input>
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

        function GetNilaiFromFormula(row, col){
            if(hot !== undefined){
                var cellId = hot.plugin.utils.translateCellCoords({row: row, col: col});
                var formula = hot.getDataAtCell(row, col);
                formula = formula.substr(1).toUpperCase();
                var newValue = hot.plugin.parse(formula, {row: row, col: col, id: cellId});
                var nr = (newValue.result);

                return nr;
            }
            return 0;
        }

        setInterval(function() { CekStatusSaveData(); }, 500);
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <asp:Literal runat="server" ID="ltrHOT"></asp:Literal>
</asp:Content>
