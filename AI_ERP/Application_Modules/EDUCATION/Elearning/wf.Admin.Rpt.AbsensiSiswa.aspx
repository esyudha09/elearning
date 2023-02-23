<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.Admin.Rpt.AbsensiSiswa.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Elearning.wf_Admin_Rpt_AbsensiSiswa" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function HideModal(){
            $('#ui_modal_download_proses').modal('hide');     

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
                case "<%= JenisAction.DoDownloadRekapAbsensi %>":
                    HideModal();
                    ReportProcess();
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
            
            InitModalFocus();
            InitPicker();
            document.getElementById("<%= txtKeyAction.ClientID %>").value = "";            
            
            Sys.Browser.WebKit = {};
            if (navigator.userAgent.indexOf('WebKit/') > -1) {
                Sys.Browser.agent = Sys.Browser.WebKit;
                Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
                Sys.Browser.name = 'WebKit';
            }
        }

        function InitModalFocus(){
        }

        function TriggerSave(){
            tinyMCE.triggerSave();
        }

        function InitPicker() {
            $('#<%= txtTglAbsenPerHari.ClientID %>').pickdate({ cancel: '', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });
            $('#<%= txtTglAbsenPerPeriode_Dari.ClientID %>').pickdate({ cancel: '', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });
            $('#<%= txtTglAbsenPerPeriode_Sampai.ClientID %>').pickdate({ cancel: '', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });
        }

        var txt_tanggal_perhari = function () { return document.getElementById('<%= txtTglAbsenPerHari.ClientID %>'); }
        var txt_tanggal_perperiode_dari = function () { return document.getElementById('<%= txtTglAbsenPerPeriode_Dari.ClientID %>'); }
        var txt_tanggal_perperiode_sampai = function () { return document.getElementById('<%= txtTglAbsenPerPeriode_Sampai.ClientID %>'); }

        function ListDropDown() {
            document.getElementById('<%= btnShowDataList.ClientID %>').click();
        }

        function GetSelectedDataList(datalist_id, textbox_id) {
            var value = "";
            var options = document.querySelectorAll('#' + datalist_id + ' option');

            for (var i = 0; i < options.length; i++) {
                var option = options[i];

                if (option.innerText === $('#' + textbox_id).val()) {
                    value = option.getAttribute('data-value');
                    break;
                }
            }

            return value;
        }

        function GetQSFilterPresensiDanKedisiplinan() {
            var hasil = "&fp=";
            var arr = document.getElementsByName("chk_pilih_presensi[]");
            for (var i = 0; i < arr.length; i++) {
                hasil += (arr[i].checked === true ? "1" : "0") + "|";
            }

            hasil += "&fk=";
            arr = document.getElementsByName("chk_pilih_kedisiplinan[]");
            for (var i = 0; i < arr.length; i++) {
                hasil += (arr[i].checked === true ? arr[i].value : "0") + "|";
            }

            return hasil;
        }

        function ReportProcess() {
            var rdo_harian = document.getElementById("<%= rdoPilihHari.ClientID%>");
            var rdo_periode = document.getElementById("<%= rdoPilihPeriode.ClientID %>");

            var txt_tgl_harian = document.getElementById("<%= txtTglAbsenPerHari.ClientID%>");
            var txt_tgl_dari = document.getElementById("<%= txtTglAbsenPerPeriode_Dari.ClientID%>");
            var txt_tgl_sampai = document.getElementById("<%= txtTglAbsenPerPeriode_Sampai.ClientID%>");

            var txt_unit = document.getElementById("<%= txtFilter_UnitVal.ClientID%>");
            var txt_kelas = document.getElementById("<%= txtFilter_KelasVal.ClientID%>");
            var txt_mapel = document.getElementById("<%= txtFilter_MapelVal.ClientID%>");
            var txt_guru = document.getElementById("<%= txtFilter_GuruVal.ClientID%>");
            var txt_siswa1 = document.getElementById("<%= txtFilter_Siswa1.ClientID%>");

            var chk_hadir = document.getElementById("chk_id_hadir");
            var chk_sakit = document.getElementById("chk_id_sakit");
            var chk_izin = document.getElementById("chk_id_izin");
            var chk_alpa = document.getElementById("chk_id_alpa");

            var s_periode = "";
            var s_grupby = "";
            var s_daritanggal = "";
            var s_sampaitanggal = "";

            var s_unit = txt_unit.value;
            var s_kelas = txt_kelas.value;
            var s_mapel = txt_mapel.value;
            var s_guru = txt_guru.value;
            var s_siswa1 = txt_siswa1.value;
            
            var s_hadir = (chk_hadir.checked === true ? "1" : "0");
            var s_sakit = (chk_sakit.checked === true ? "1" : "0");
            var s_izin = (chk_izin.checked === true ? "1" : "0");
            var s_alpa = (chk_alpa.checked === true ? "1" : "0");

            if (rdo_harian.checked === true) s_periode = "0";
            if (rdo_periode.checked === true) s_periode = "1";

            if (s_periode === "0") {
                s_daritanggal = txt_tgl_harian.value;
                s_sampaitanggal = txt_tgl_harian.value;
            } else if (s_periode === "1") {
                s_daritanggal = txt_tgl_dari.value;
                s_sampaitanggal = txt_tgl_sampai.value;
            }

            var arr_grup = document.getElementsByName("ctl00$ContentPlaceHolder1$rdoGroupBy[]");
            for (var i = 0; i < arr_grup.length; i++) {
                if (arr_grup[i].checked === true) {
                    //s_grupby = i.toString();
                    s_grupby = arr_grup[i].lang;
                    break;
                }
            }

            var s_kedisiplinan = "";
            var arr_kedisiplinan = document.getElementsByName("chk_pilih_kedisiplinan[]");
            for (var i = 0; i < arr_kedisiplinan.length; i++) {
                s_kedisiplinan +=
                    arr_kedisiplinan[i].value + "|" +
                    (
                        arr_kedisiplinan[i].checked === true
                        ? "100"
                        : "0"
                    ) + ";";
            }
            
			$('#ui_modal_download_proses').modal({ backdrop: 'static', keyboard: false, show: true });      
			var url = "<%= ResolveUrl("~/Application_Resources/Download.aspx") %>";
			url += "?jd=" + "<%= AI_ERP.Application_Libs.Downloads.JenisDownload.REKAP_ABSENSI_SISWA_V2 %>";
            url += "&p=" + s_periode;
            url += "&dt=" + s_daritanggal;
            url += "&st=" + s_sampaitanggal;
            url += "&u=" + s_unit;
            url += "&k=" + s_kelas;
            url += "&m=" + s_mapel;
            url += "&g=" + s_guru;
            url += "&s1=" + s_siswa1;
            url += "&s2=";
            url += "&gb=" + s_grupby;
            url += "&h=" + s_hadir;
            url += "&s=" + s_sakit;
            url += "&i=" + s_izin;
            url += "&a=" + s_alpa;
            url += "&kd=" + s_kedisiplinan;
            url += GetQSFilterPresensiDanKedisiplinan();

			if (navigator.appName == 'Microsoft Internet Explorer') {
				window.frames['fra_download'].document.location.href = url;
			} else {
				window.frames['fra_download'].location.href = url;
			}
        }

        function StopProsesDownload()
        {
            if (navigator.appName == 'Microsoft Internet Explorer') {
                window.frames['fra_download'].document.execCommand('Stop');
            } else {
                window.frames['fra_download'].stop();
            }
			
            if(document.getElementById("ui_modal_download_proses") != null && document.getElementById("ui_modal_download_proses") != undefined){
                document.getElementById("ui_modal_download_proses").style.display = "none";
            }
            HideModal();
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
            <asp:HiddenField runat="server" ID="txtRel_Sekolah" />

            <asp:HiddenField runat="server" ID="txtFilter_UnitVal" />
            <asp:HiddenField runat="server" ID="txtFilter_KelasVal" />
            <asp:HiddenField runat="server" ID="txtFilter_MapelVal" />
            <asp:HiddenField runat="server" ID="txtFilter_GuruVal" />
            <asp:HiddenField runat="server" ID="txtFilter_Siswa1Val" />

            <asp:Literal runat="server" ID="ltrDataList"></asp:Literal>

            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDataList" OnClick="btnShowDataList_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnInitEnabledFilter" OnClick="btnInitEnabledFilter_Click" Style="position: absolute; left: -1000px; top: -1000px;" />

            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-12">

                    <%--<div class="col-md-8 col-md-offset-2" style="padding: 0px;">--%>
                    <div style="padding: 0px;">
                        <div class="card" style="margin-top: 40px;">
				            <div class="card-main">
					            <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px; border-right-style: solid; border-right-color: #dadada; border-right-width: 1px;">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="background-color: #295BC8; padding: 10px; font-weight: normal; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                                <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/circular-graphic.svg") %>" 
                                                    style="margin: 0 auto; height: 25px; width: 25px;" />
                                                &nbsp;
                                                Rekapitulasi Presensi Murid & Kedisiplinan
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="background-color: #295BC8; padding: 0px;">
                                                <hr style="margin: 0px; border-style: solid; border-width: 1px; border-color: #2555BE;" />
                                            </td>
                                        </tr>
                                    </table>

						            <asp:MultiView runat="server" ID="mvMain" ActiveViewIndex="0">
                                        <asp:View runat="server" ID="vInput">

                                            <div style="padding: 0px; margin: 0px;">
                                                <div class="row">
                                                    <div class="col-xs-3" style="text-align: left; padding-left: 50px; padding-top: 0px; padding-bottom: 40px;">
                                                        <br />
                                                        <span style="font-weight: bold; margin-left: -15px;">Pilihan Waktu</span>
                                                        <hr style="margin: 0px; margin-top: 15px; border-width: 4px; border-color: #70b4d5; margin-left: -15px;" />
                                                        <br />
                                                        <div class="row">
                                                            <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                                <div class="radiobtn radiobtn-adv">
                                                                    <label for="<%= rdoPilihHari.ClientID %>">
                                                                        <input
                                                                            name="rdoPilihTanggal"
                                                                            checked=true
                                                                            runat="server"
                                                                            class="access-hide" 
                                                                            id="rdoPilihHari"
                                                                            type="radio" />
                                                                        <span class="radiobtn-circle"></span>
                                                                        <span class="radiobtn-circle-check"></span>
                                                                        <span style="font-weight: bold; font-size: 14px; color: black;">
                                                                            Pilih Hari
                                                                        </span>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-xs-12" style="padding-left: 20px; padding-right: 15px;">
                                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                    <asp:TextBox style="font-weight: normal;" CssClass="form-control" runat="server" ID="txtTglAbsenPerHari"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 15px; padding-top: 15px;">
                                                                <div class="radiobtn radiobtn-adv">
                                                                    <label for="<%= rdoPilihPeriode.ClientID %>">
                                                                        <input
                                                                            name="rdoPilihTanggal"
                                                                            runat="server"
                                                                            class="access-hide" 
                                                                            id="rdoPilihPeriode"
                                                                            checked=true
                                                                            type="radio" />
                                                                        <span class="radiobtn-circle"></span>
                                                                        <span class="radiobtn-circle-check"></span>
                                                                        <span style="font-weight: bold; font-size: 14px; color: black;">
                                                                            Pilih Rentang Hari<br />
                                                                            (*Maksimal 31 Hari)
                                                                        </span>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-xs-12" style="padding-left: 20px; padding-right: 15px;">
                                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                    <asp:TextBox style="font-weight: normal;" CssClass="form-control" runat="server" ID="txtTglAbsenPerPeriode_Dari"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-xs-12" style="padding-left: 20px; padding-right: 15px;">
                                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                    <asp:TextBox style="font-weight: normal;" CssClass="form-control" runat="server" ID="txtTglAbsenPerPeriode_Sampai"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-xs-6" style="text-align: left; padding-bottom: 40px; padding-right: 0px;">
                                                        <br />
                                                        <span style="font-weight: bold; margin-left: -15px;">Pilihan Kategori</span>
                                                        <hr style="margin: 0px; margin-top: 15px; border-width: 4px; border-color: #70b4d5; margin-left: -15px;" />
                                                        <br />
                                                        <div class="row" runat="server" id="div_unit">
                                                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 15px;">
                                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                    <label class="label-input" for="<%= txtFilter_Unit.ClientID %>" style="text-transform: none;">Unit</label>
                                                                    <input type="text" placeholder="Unit..." runat="server" id="txtFilter_Unit" Class="form-control" list="dl_unit" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row" runat="server" id="div_kelas">
                                                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 15px;">
                                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                    <label class="label-input" for="<%= txtFilter_Kelas.ClientID %>" style="text-transform: none;">Kelas</label>
                                                                    <input type="text" placeholder="Kelas..." runat="server" id="txtFilter_Kelas" Class="form-control" list="dl_kelas" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row" runat="server" id="div_mapel">
                                                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 15px;">
                                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                    <label class="label-input" for="<%= txtFilter_Mapel.ClientID %>" style="text-transform: none;">Mata Pelajaran</label>
                                                                    <input type="text" placeholder="Mata Pelajaran..." runat="server" id="txtFilter_Mapel" Class="form-control" list="dl_mapel" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row" runat="server" id="div_guru">
                                                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 15px;">
                                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                    <label class="label-input" for="<%= txtFilter_Guru.ClientID %>" style="text-transform: none;">Guru</label>
                                                                    <input type="text" placeholder="Guru..." runat="server" id="txtFilter_Guru" Class="form-control" list="dl_guru" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row" runat="server" id="div_siswa">
                                                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 15px;">
                                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                    <label class="label-input" for="<%= txtFilter_Siswa1.ClientID %>" style="text-transform: none;">Murid</label>
                                                                    <input type="text" placeholder="Siswa..." runat="server" id="txtFilter_Siswa1" Class="form-control" list="dl_siswa_1" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-xs-3" style="padding-left: 35px; text-align: left; padding-bottom: 40px; padding-top: 0px; padding-right: 30px;">
                                                        <br />
                                                        <span style="font-weight: bold; margin-left: -15px;">Grup Berdasarkan</span>
                                                        <hr style="margin: 0px; margin-top: 15px; border-width: 4px; border-color: #70b4d5; margin-left: -15px;" />
                                                        <br />
                                                        <div class="row" runat="server" id="div_by_unit">
                                                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                                <div class="radiobtn radiobtn-adv">
                                                                    <label for="<%= rdoGroupByUnit.ClientID %>">
                                                                        <input
                                                                            name="rdoGroupBy[]"
                                                                            runat="server"
                                                                            class="access-hide" 
                                                                            id="rdoGroupByUnit"
                                                                            lang="0"
                                                                            type="radio" />
                                                                        <span class="radiobtn-circle"></span>
                                                                        <span class="radiobtn-circle-check"></span>
                                                                        <span style="font-weight: bold; font-size: 14px; color: black;">
                                                                            Unit [Kelas Perwalian & Matpel]
                                                                        </span>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <br /><br />
                                                        </div>
                                                        <div class="row" runat="server" id="div_by_kelas">
                                                            <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                                <div class="radiobtn radiobtn-adv">
                                                                    <label for="<%= rdoGroupByKelas.ClientID %>">
                                                                        <input
                                                                            name="rdoGroupBy[]"
                                                                            runat="server"
                                                                            class="access-hide" 
                                                                            id="rdoGroupByKelas"
                                                                            lang="1"
                                                                            type="radio" />
                                                                        <span class="radiobtn-circle"></span>
                                                                        <span class="radiobtn-circle-check"></span>
                                                                        <span style="font-weight: bold; font-size: 14px; color: black;">
                                                                            Kelas [Kelas Perwalian]
                                                                        </span>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <br /><br />
                                                        </div>
                                                        <div class="row" runat="server" id="div_by_mapel">
                                                            <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                                <div class="radiobtn radiobtn-adv">
                                                                    <label for="<%= rdoGroupByMapel.ClientID %>">
                                                                        <input
                                                                            name="rdoGroupBy[]"
                                                                            runat="server"
                                                                            class="access-hide" 
                                                                            id="rdoGroupByMapel"
                                                                            lang="2"
                                                                            type="radio" />
                                                                        <span class="radiobtn-circle"></span>
                                                                        <span class="radiobtn-circle-check"></span>
                                                                        <span style="font-weight: bold; font-size: 14px; color: black;">
                                                                            Mapel [Kelas Matpel]
                                                                        </span>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <br /><br />
                                                        </div>
                                                        <div class="row" runat="server" id="div_by_guru">
                                                            <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                                <div class="radiobtn radiobtn-adv">
                                                                    <label for="<%= rdoGroupByGuru.ClientID %>">
                                                                        <input
                                                                            name="rdoGroupBy[]"
                                                                            runat="server"
                                                                            class="access-hide" 
                                                                            id="rdoGroupByGuru"
                                                                            lang="3"
                                                                            type="radio" />
                                                                        <span class="radiobtn-circle"></span>
                                                                        <span class="radiobtn-circle-check"></span>
                                                                        <span style="font-weight: bold; font-size: 14px; color: black;">
                                                                            Guru [Kelas Perwalian & Matpel]
                                                                        </span>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <br /><br />
                                                        </div>
                                                        <div class="row" runat="server" id="div_by_siswa">
                                                            <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                                <div class="radiobtn radiobtn-adv">
                                                                    <label for="<%= rdoGroupBySiswa.ClientID %>">
                                                                        <input
                                                                            name="rdoGroupBy[]"
                                                                            runat="server"
                                                                            class="access-hide" 
                                                                            id="rdoGroupBySiswa"
                                                                            lang="4"
                                                                            type="radio" />
                                                                        <span class="radiobtn-circle"></span>
                                                                        <span class="radiobtn-circle-check"></span>
                                                                        <span style="font-weight: bold; font-size: 14px; color: black;">
                                                                            Murid [Kelas Perwalian & Matpel]
                                                                        </span>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <br /><br />
                                                        </div>
                                                        <div class="row" runat="server" id="div_by_mapel_siswa">
                                                            <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                                <div class="radiobtn radiobtn-adv">
                                                                    <label for="<%= rdoGroupByMapelBySiswa.ClientID %>">
                                                                        <input
                                                                            name="rdoGroupBy[]"
                                                                            runat="server"
                                                                            class="access-hide" 
                                                                            id="rdoGroupByMapelBySiswa"
                                                                            lang="5"
                                                                            type="radio" />
                                                                        <span class="radiobtn-circle"></span>
                                                                        <span class="radiobtn-circle-check"></span>
                                                                        <span style="font-weight: bold; font-size: 14px; color: black;">
                                                                            Detail Murid Per Mapel<br />[Kelas Perwalian & Matpel]
                                                                        </span>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <br /><br /><br />
                                                        </div>
                                                        <div class="row" runat="server" id="div_by_detail_absensi">
                                                            <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                                <div class="radiobtn radiobtn-adv">
                                                                    <label for="<%= rdoDetailAbsensi.ClientID %>">
                                                                        <input
                                                                            name="rdoGroupBy[]"
                                                                            runat="server"
                                                                            class="access-hide" 
                                                                            id="rdoDetailAbsensi"
                                                                            lang="6"
                                                                            type="radio" />
                                                                        <span class="radiobtn-circle"></span>
                                                                        <span class="radiobtn-circle-check"></span>
                                                                        <span style="font-weight: bold; font-size: 14px; color: black;">
                                                                            Detail Murid Per Absensi & Ketidak Disiplinan
                                                                            <br />
                                                                            [Kelas Perwalian]
                                                                        </span>
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <br /><br /><br />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="fbtn-container" id="div_button_settings" runat="server">
		                                        <div class="fbtn-inner">
			                                        <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #a91212;" title=" Pilihan ">
                                                        <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                                                        <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                                                    </a>
                                                    <div class="fbtn-dropup" style="z-index: 999999;">
                                                        <asp:LinkButton OnClick="btnDownloadExcel_Click" CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" id="btnDownloadExcel" title=" Download " style="background-color: green; color: white;">
                                                            <span class="fbtn-text fbtn-text-left">Download Data</span>
                                                            <i class="fa fa-file-excel-o"></i>
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

            <div aria-hidden="true" class="modal fade" id="ui_modal_download_proses" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
				<div class="modal-dialog modal-xs" style="width: 300px;">
					<div class="modal-content">
						<div class="modal-inner" 
							style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; 
							padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px;">
							
							<div class="row" style="margin-left: 0px; margin-right: 0px;">
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
									<i class="fa fa-info-circle"></i>
									&nbsp;
									Sedang proses...
									&nbsp;&nbsp;&nbsp;
									<br /><br />
								</div>
							</div>

						</div>
					</div>
				</div>
			</div>

        </ContentTemplate>
        <Triggers>       
        </Triggers>
    </asp:UpdatePanel>

    <div style="padding: 0px; margin: 0px; width: 100%;">
        <div class="row" style="width: 100%;">
            <div class="col-xs-3">&nbsp;</div>
            <div class="col-xs-6">
                <div class="row">
                    <div class="col-xs-6" style="padding-left: 32px; padding-right: 15px; text-align: center; padding-top: 15px;">
                        <span style="font-weight: bold;">Tampilkan/Sembunyikan Kolom Presensi</span>
                        <hr style="margin: 0px; margin-top: 15px; border-width: 4px; border-color: #70b4d5; margin-bottom: 15px;" />
                        <asp:Literal runat="server" ID="ltrFilterPresensi"></asp:Literal>
                    </div>
                    <div class="col-xs-6" style="padding-left: 0px; padding-right: 0px; text-align: center; padding-top: 15px;">
                        <span style="font-weight: bold;">Tampilkan/Sembunyikan Kolom Kedisiplinan</span>
                        <hr style="margin: 0px; margin-top: 15px; border-width: 4px; border-color: #70b4d5; margin-bottom: 15px;" />
                        <asp:Literal runat="server" ID="ltrFilterKedisiplinan"></asp:Literal>
                    </div>
                </div>
            </div>
            <div class="col-xs-3">&nbsp;</div>
        </div>
    </div>

    <iframe name="fra_download" onloadedmetadata="alert('ok')" id="fra_download" height="0" width="0" style="position: absolute; left: -1000px; top: -1000px;"></iframe>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
        InitPicker();
        InitModalFocus();
    </script>
</asp:Content>

