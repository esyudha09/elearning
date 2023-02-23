<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.Dashboard.aspx.cs" Inherits="AI_ERP.wf_Dashboard" %>

<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .menu-item {
            cursor: pointer;
        }

            .menu-item:hover {
                background-color: white;
                border-style: solid;
                border-width: 1px;
                border-color: #65CEFF;
                border-radius: 5px;
                background-color: #D4F1FF;
            }

        .page-button {
            font-weight: normal;
            color: grey;
            border-style: solid;
            border-color: #F1F1F1;
            border-radius: 5px;
            border-width: 1px;
            padding: 6px;
            font-size: small;
        }
    </style>
    <script type="text/javascript">
        function HideModal() {
            $('#ui_modal_biodata_siswa').modal('hide');
            $('#ui_modal_tampilkan_ledger').modal('hide');
            $('#ui_modal_kunjungan_perpus').modal('hide');
            $('#ui_modal_list_kelas_mapel').modal('hide');
            $('#ui_modal_buka_semester').modal('hide');
            $('#ui_modal_absensi').modal('hide');

            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();

            document.body.style.paddingRight = "0px";
        }

        function GoToURL(url) {
            document.location.href = url;
        }

        function EndRequestHandler() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        }

        function EndRequest() {
            var jenis_act = document.getElementById("<%= txtKeyAction.ClientID %>").value;

            switch (jenis_act) {
                case "<%= JenisAction.ShowBukaSemester %>":
                    $('#ui_modal_buka_semester').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.ShowOptionBiodataSiswa %>":
                    $('#ui_modal_biodata_siswa').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.ShowOptionLedgerSMA %>":
                    $('#ui_modal_tampilkan_ledger').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.ShowInfoKunjungan %>":
                    $('#ui_modal_kunjungan_perpus').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.ShowPilihKelas %>":
                    $('#ui_modal_list_kelas_mapel').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.ShowAbsensiWalas %>":
                    $('#ui_modal_absensi').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoSaveAbsensi %>":
                    HideModal();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data absensi sudah disimpan',
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
        }

        function ShowKelasByUnit(unit) {
            var txt_arr = document.getElementById("<%= txtParseKelasUnit.ClientID %>");
            var cbo_kelas = document.getElementById("<%= cboKelasBiodataSiswa.ClientID %>");
            if (txt_arr != null && txt_arr != undefined && cbo_kelas != null && cbo_kelas != undefined) {
                if (unit.trim() != "") {
                    var arr_kelas = txt_arr.value.split(";");
                    if (arr_kelas.length > 0) {
                        if (cbo_kelas.options.length > 0) {
                            for (var i = cbo_kelas.options.length - 1; i >= 0; i--) {
                                cbo_kelas.options[i] = null;
                            }
                        }
                        for (var i = 0; i < arr_kelas.length; i++) {
                            var kk_unit = unit + '->';
                            if (arr_kelas[i].indexOf(kk_unit) >= 0) {
                                var s_kelas = arr_kelas[i].replace(kk_unit, "");
                                var arr_item_kelas = s_kelas.split("|");
                                if (arr_item_kelas.length === 2) {
                                    var option = document.createElement("option");
                                    option.value = arr_item_kelas[0];
                                    option.text = arr_item_kelas[1];
                                    cbo_kelas.add(option);
                                }
                            }
                        }
                    }
                }
            }
        }

        function ReportProcessBukaSemester() {
            var url = "<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.BUKA_SEMESTER.ROUTE) %>";
            
            url += "?";
            url += "&t=" + <%= txtTahunPelajaranBukaSemester.ClientID %>.value;
            url += "&s=" + <%= txtSemesterPelajaranBukaSemester.ClientID %>.value;
            url += "&u=" + <%= cboUnitBukaSemester.ClientID %>.value;
            url += "&" + <%= cboBukaSemesterCopyDari.ClientID %>.value;            

            if (navigator.appName == 'Microsoft Internet Explorer') {
                window.frames['fra_buka_semester'].document.location.href = url;
            } else {
                window.frames['fra_buka_semester'].location.href = url;
            }
        }

        function ShowProsesBukaSemester(show) {
            pb_proses_buka_semester.style.display = (show ? "" : "none");
            div_command_buka_semester.style.display = (show ? "none" : "");
            if (!show) {
                if (navigator.appName == 'Microsoft Internet Explorer') {
                    window.frames['fra_buka_semester'].document.execCommand('Stop');
                } else {
                    window.frames['fra_buka_semester'].stop();
                }
            }
        }

        function StopProsesBukaSemester(m)
        {
            if (navigator.appName == 'Microsoft Internet Explorer') {
                window.frames['fra_buka_semester'].document.execCommand('Stop');
            } else {
                window.frames['fra_buka_semester'].stop();
            }

            ShowProsesBukaSemester(false);
            HideModal();

            $('body').snackbar({
                alive: 2000,
                content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;' + m,
                show: function () {
                    snackbarText++;
                }
            });
        }

        function StopProsesBukaSemesterError(m)
        {
            if (navigator.appName == 'Microsoft Internet Explorer') {
                window.frames['fra_buka_semester'].document.execCommand('Stop');
            } else {
                window.frames['fra_buka_semester'].stop();
            }

            ShowProsesBukaSemester(false);
            HideModal();

            $('body').snackbar({
                alive: 2000,
                content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;' + m,
                show: function () {
                    snackbarText++;
                }
            });
        }

        function ParseAbsensi(){
            var txt = document.getElementById("<%= txtParseAbsensi.ClientID %>");
            if(txt !== undefined && txt !== null){
                txt.value = "";
                var arr_siswa = document.getElementsByName("arr_txt_siswa[]");
                var arr_sakit = document.getElementsByName("arr_txt_sakit[]");
                var arr_izin = document.getElementsByName("arr_txt_izin[]");
                var arr_alpa = document.getElementsByName("arr_txt_alpa[]");
                var arr_terlambat = document.getElementsByName("arr_txt_terlambat[]");
                if(arr_siswa.length > 0){
                    if(arr_siswa.length === arr_sakit.length && arr_siswa.length === arr_izin.length && arr_siswa.length === arr_terlambat.length){
                        for (var i = 0; i < arr_siswa.length; i++) {
                            txt.value += arr_siswa[i].value + "|" +
                                         arr_sakit[i].value + "|" +
                                         arr_izin[i].value + "|" +
                                         arr_terlambat[i].value + "|" +
                                         arr_alpa[i].value + ";";
                        }
                    }
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-inner" style="margin-top: 10px;">
        <asp:UpdateProgress runat="server" ID="upProgressMain" AssociatedUpdatePanelID="upMain">
            <ProgressTemplate>
                <ucl:PostbackUpdateProgress runat="server" ID="pbUpdateProgress" />
            </ProgressTemplate>
        </asp:UpdateProgress>

        <asp:UpdatePanel ID="upMain" runat="server">
            <ContentTemplate>

                <asp:HiddenField runat="server" ID="txtKeyAction" />
                <asp:HiddenField runat="server" ID="txtParseKelasUnit" />
                <asp:HiddenField runat="server" ID="txtIDKunjungan" />
                <asp:HiddenField runat="server" ID="txtTahunAjaran" />
                <asp:HiddenField runat="server" ID="txtSemester" />
                <asp:HiddenField runat="server" ID="txtRelSekolah" />
                <asp:HiddenField runat="server" ID="txtRelKelasDet" />
                <asp:HiddenField runat="server" ID="txtParseAbsensi" />

                <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInfoKunjungan" OnClick="btnShowInfoKunjungan_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
                <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowListKelasMapel" OnClick="btnShowListKelasMapel_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
                <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowBukaSemester" OnClick="btnShowBukaSemester_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
                <asp:Button UseSubmitBehavior="false" runat="server" ID="btnSHowAbsensi" OnClick="btnSHowAbsensi_Click" Style="position: absolute; left: -1000px; top: -1000px;" />

                <div class="row" style="margin-left: 15px; margin-right: 15px; margin-top: 40px;">

                    <div class="col-md-8">
                        <div class="row" style="padding: 30px; padding-top: 10px; padding-bottom: 10px; font-weight: bold; color: grey; display: none;">
                            <i class="fa fa-desktop"></i>
                            &nbsp;
                            e-Learning
                            <hr style="margin: 0px; margin-top: 10px; margin-bottom: 10px;" />
                        </div>
                        <div id="div_menu_ortu_1" runat="server" class="row" style="padding: 0px; padding-top: 10px; padding-bottom: 10px;">
                            <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_PROFIL_SISWA.ROUTE)  %>');" class="col-md-3 menu-item" style="padding: 10px;">
                                <img src="Application_CLibs/images/svg/bedge.svg"
                                    style="margin: 0 auto; display: table; height: 60px; width: 60px;" />
                                <br />
                                <label style="margin: 0 auto; display: table; font-weight: bold;">
                                    Profil
                                </label>
                                <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                    Lihat detail Profil Siswa
                                </label>
                            </div>

                            <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_ABSENSI_SISWA.ROUTE)  %>');" class="col-md-3 menu-item" style="padding: 10px;">
                                <img src="Application_CLibs/images/svg/placeholder.svg"
                                    style="margin: 0 auto; display: table; height: 60px; width: 60px;" />
                                <br />
                                <label style="margin: 0 auto; display: table; font-weight: bold;">
                                    Presensi
                                </label>
                                <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                    Lihat data presensi Siswa
                                </label>
                            </div>

                            <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_TUGAS_SISWA.ROUTE)  %>');" class="col-md-3 menu-item" style="padding: 10px; display: none;">
                                <div>
                                    <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/open-book.svg)">
                                        <span class="badge" style="background-color: darkorange; float: right;">4</span>
                                    </div>
                                </div>
                                <br />
                                <label style="margin: 0 auto; display: table; font-weight: bold;">
                                    Tugas
                                </label>
                                <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                    Lihat Tugas dan status<br />
                                    pengerjaan tugas oleh Siswa
                                </label>
                            </div>

                            <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_MATERI.ROUTE)  %>');" class="col-md-3 menu-item" style="padding: 10px;">
                                <div>
                                    <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/open-book.svg)">
                                    </div>
                                </div>
                                <br />
                                <label style="margin: 0 auto; display: table; font-weight: bold;">
                                    Materi Pembelajaran
                                </label>
                                <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                    Lihat Materi Pembelajaran<br />
                                    per Mata Pelajaran
                                </label>
                            </div>

                            <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_KALENDER_AKADEMIK.ROUTE)  %>');" class="col-md-3 menu-item" style="padding: 10px;">
                                <img src="Application_CLibs/images/svg/calendar.svg"
                                    style="margin: 0 auto; display: table; height: 60px; width: 60px;" />
                                <br />
                                <label style="margin: 0 auto; display: table; font-weight: bold;">
                                    Kalender
                                </label>
                                <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                    Kalender Akademik<br />
                                    Al-Izhar Pondok Labu
                                </label>
                            </div>

                        </div>
                        <div id="div_menu_ortu_2" runat="server" class="row" style="padding: 0px; padding-top: 10px; padding-bottom: 10px;">
                            <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_NILAI_SISWA.ROUTE)  %>');" class="col-md-3 menu-item" style="padding: 10px;">
                                <div>
                                    <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/touchscreen.svg)">
                                        <span class="badge" style="background-color: red; float: right; display: none;">34</span>
                                    </div>
                                </div>
                                <br />
                                <label style="margin: 0 auto; display: table; font-weight: bold;">
                                    Penilaian
                                </label>
                                <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                    Lihat informasi nilai siswa
                                </label>
                            </div>

                            <div class="col-md-3 menu-item" style="padding: 10px; display: none;">
                                <div>
                                    <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/book.svg)">
                                        <span class="badge" style="background-color: red; float: right; display: none;">34</span>
                                    </div>
                                </div>
                                <br />
                                <label style="margin: 0 auto; display: table; font-weight: bold;">
                                    Buku Penghubung
                                </label>
                                <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                    Kelola informasi<br />
                                    melalui Buku Penghubung
                                </label>
                            </div>

                            <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_UANG_SEKOLAH.ROUTE)  %>');" class="col-md-3 menu-item" style="padding: 10px; display: none;">
                                <span class="badge" style="position: absolute; right: 10%; background-color: red; float: right; font-weight: bold;">Rp.
                                    <asp:Label runat="server" ID="lblTagihan"></asp:Label>
                                </span>
                                <div>
                                    <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/022-money.svg)">
                                        &nbsp;
                                    </div>
                                </div>
                                <br />
                                <label style="margin: 0 auto; display: table; font-weight: bold;">
                                    Uang Sekolah
                                </label>
                                <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                    Informasi tagihan dan<br />
                                    pembayaran uang sekolah
                                </label>
                            </div>

                            <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_TRANSAKSI_KANTIN.ROUTE)  %>');" class="col-md-3 menu-item" style="padding: 10px; display: none;">
                                <span class="badge" style="position: absolute; right: 10%; background-color: green; float: right; font-weight: bold;">Rp.
                                    <asp:Label runat="server" ID="lblSaldoKantin"></asp:Label>
                                </span>
                                <div>
                                    <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/009-shop.svg)">
                                        &nbsp;
                                    </div>
                                </div>
                                <br />
                                <label style="margin: 0 auto; display: table; font-weight: bold;">
                                    Kantin
                                </label>
                                <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                    Informasi transaksi dan<br />
                                    saldo uang Kantin
                                </label>
                            </div>

                            <div onclick="#" class="col-md-3 menu-item" style="padding: 10px; display: none;">
                                <div>
                                    <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/network-1.svg)">
                                        &nbsp;
                                    </div>
                                </div>
                                <br />
                                <label style="margin: 0 auto; display: table; font-weight: bold;">
                                    Volunteer
                                </label>
                                <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                    Informasi kegiatan<br />
                                    Volunteer Siswa
                                </label>
                            </div>
                            <div onclick="#" class="col-md-3 menu-item" style="padding: 10px;">
                                <div>
                                    <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/running.svg)">
                                        &nbsp;
                                    </div>
                                </div>
                                <br />
                                <label style="margin: 0 auto; display: table; font-weight: bold;">
                                    Ekstrakurikuler
                                </label>
                                <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                    Informasi kegiatan<br />
                                    Ekstrakurikuler Siswa
                                </label>
                            </div>
                            <div onclick="#" class="col-md-3 menu-item" style="padding: 10px;">
                                <div>
                                    <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/curriculum.svg)">
                                        &nbsp;
                                    </div>
                                </div>
                                <br />
                                <label style="margin: 0 auto; display: table; font-weight: bold;">
                                    Perkembangan Anak
                                </label>
                                <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                    Informasi<br />
                                    Perkembangan Anak
                                </label>
                            </div>
                        </div>
                        <div id="div_menu_ortu_3" runat="server" class="row" style="padding: 0px; padding-top: 10px; padding-bottom: 10px;">
                            
                        </div>
                        <div>
                            <asp:Literal runat="server" ID="ltrKelas"></asp:Literal>
                        </div>
                        <div runat="server" id="div_master_data">
                            <div class="row" style="padding: 30px; padding-top: 10px; padding-bottom: 10px; font-weight: bold; color: grey;">
                                <i class="fa fa-database"></i>
                                &nbsp;
                                Master Data
                                <hr style="margin: 0px; margin-top: 10px; margin-bottom: 10px;" />
                            </div>
                            <div runat="server" id="div_row_data_master_data_1" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">
                                <div runat="server" id="div_data_divisi" class="col-md-3 menu-item" style="padding: 10px;">
                                    <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.DIVISI.ROUTE)  %>');" style="cursor: pointer;">
                                        <div>
                                            <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/flats.svg);">
                                                &nbsp;
                                            </div>
                                        </div>
                                        <br />
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Data Divisi
                                        </label>
                                        <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                            Pengaturan data divisi
                                        </label>
                                    </div>
                                </div>
                                <div runat="server" id="div_data_unit_non_sekolah" class="col-md-3 menu-item" style="padding: 10px;">
                                    <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.UNIT_NON_SEKOLAH.ROUTE)  %>');" style="cursor: pointer;">
                                        <div>
                                            <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/020-office.svg);">
                                                &nbsp;
                                            </div>
                                        </div>
                                        <br />
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Data Unit Non Sekolah
                                        </label>
                                        <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                            Pengaturan data Unit Non Sekolah
                                        </label>
                                    </div>
                                </div>
                                <div runat="server" id="div_data_unit_sekolah" class="col-md-3 menu-item" style="padding: 10px;">
                                    <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.UNIT_SEKOLAH.ROUTE)  %>');" style="cursor: pointer;">
                                        <div>
                                            <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/school.svg);">
                                                &nbsp;
                                            </div>
                                        </div>
                                        <br />
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Data Unit Sekolah
                                        </label>
                                        <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                            Pengaturan data Unit Sekolah
                                        </label>
                                    </div>
                                </div>
                                <div runat="server" id="div_data_kelas" class="col-md-3 menu-item" style="padding: 10px;">
                                    <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.KELAS.ROUTE)  %>');" style="cursor: pointer;">
                                        <div>
                                            <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/folder-1.svg);">
                                                &nbsp;
                                            </div>
                                        </div>
                                        <br />
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Data Level & Kelas
                                        </label>
                                        <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                            Pengaturan data Level & Kelas
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <div runat="server" id="div_row_data_master_data_2" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">
                                <div runat="server" id="div_data_karyawan" class="col-md-3 menu-item" style="padding: 10px;">
                                    <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.PEGAWAI.ROUTE) %>?token=<%= AI_ERP.Application_Libs.Constantas.TOKEN_ADMIN %>');" style="cursor: pointer;">
                                        <div>
                                            <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/student-3.svg);">
                                                &nbsp;
                                            </div>
                                        </div>
                                        <br />
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Data Pegawai, Guru
                                        </label>
                                        <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                            Profil/Biodata Pegawai, Guru
                                        </label>
                                    </div>
                                </div>
                                <div runat="server" id="div_data_siswa" class="col-md-3 menu-item" style="padding: 10px;">
                                    <a onclick="pb_ok_biodata_siswa.style.display = 'none'; <%= lnkOKShowBiodataSiswa.ClientID %>.style.display = '';" data-backdrop="static" data-toggle="modal" href="#ui_modal_biodata_siswa" style="cursor: pointer">
                                        <div>
                                            <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/student.svg);">
                                                &nbsp;
                                            </div>
                                        </div>
                                        <br />
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Data Siswa
                                        </label>
                                        <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                            Lihat Profil/Biodata Siswa
                                        </label>
                                    </a>
                                </div>
                                <div runat="server" id="div_data_mapel" class="col-md-3 menu-item" style="padding: 10px;">
                                    <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.MAPEL.ROUTE)  %>');" style="cursor: pointer;">
                                        <div>
                                            <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/document.svg);">
                                                &nbsp;
                                            </div>
                                        </div>
                                        <br />
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Mata Pelajaran
                                        </label>
                                        <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                            Pengaturan data Mata Pelajaran
                                        </label>
                                    </div>
                                </div>
                                <div runat="server" id="div_data_ruang_kelas" class="col-md-3 menu-item" style="padding: 10px;">
                                    <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.RUANG_KELAS.ROUTE)  %>');" style="cursor: pointer;">
                                        <div>
                                            <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/university-1.svg);">
                                                &nbsp;
                                            </div>
                                        </div>
                                        <br />
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Ruang Kelas
                                        </label>
                                        <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                            Pengaturan data Ruang Kelas
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <div runat="server" id="div_row_data_master_data_3" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">
                                <div runat="server" id="div_data_formasi_guru_kelas" class="col-md-3 menu-item" style="padding: 10px;">
                                    <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.FORMASI_GURU_KELAS.ROUTE)  %>');" style="cursor: pointer;">
                                        <div>
                                            <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/boss-1.svg);">
                                                &nbsp;
                                            </div>
                                        </div>
                                        <br />
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Formasi Guru Kelas/Wali Kelas
                                        </label>
                                        <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                            Pengaturan formasi guru kelas/wali kelas.
                                        </label>
                                    </div>
                                </div>
                                <div runat="server" id="div_data_formasi_guru_mapel" class="col-md-3 menu-item" style="padding: 10px;">
                                    <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.FORMASI_GURU_MAPEL.ROUTE)  %>');" style="cursor: pointer;">
                                        <div>
                                            <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/network.svg);">
                                                &nbsp;
                                            </div>
                                        </div>
                                        <br />
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Formasi Guru Mata Pelajaran
                                        </label>
                                        <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                            Pengaturan formasi guru mata pelajaran.
                                        </label>
                                    </div>
                                </div>
                                <div visible="false" runat="server" id="div_wali_kelas" class="col-md-3 menu-item" style="padding: 10px;">
                                    <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.FORMASI_WALI_KELAS.ROUTE)  %>');" style="cursor: pointer;">
                                        <div>
                                            <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/professor-1.svg);">
                                                &nbsp;
                                            </div>
                                        </div>
                                        <br />
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Wali Kelas
                                        </label>
                                        <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                            Pengaturan data Wali Kelas
                                        </label>
                                    </div>
                                </div>
                                <div runat="server" id="div_jadwal_mapel" class="col-md-3 menu-item" style="padding: 10px;">
                                    <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.JADWAL_MAPEL.ROUTE)  %>');" style="cursor: pointer;">
                                        <div>
                                            <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/text-lines.svg);">
                                                &nbsp;
                                            </div>
                                        </div>
                                        <br />
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Jadwal Mata Pelajaran
                                        </label>
                                        <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                            Pengaturan jadwal mata pelajaran
                                        </label>
                                    </div>
                                </div>
                                <div runat="server" id="div_buat_semester" class="col-md-3 menu-item" style="padding: 10px;">
                                    <div onclick="<%= btnShowBukaSemester.ClientID %>.click();" style="cursor: pointer;">
                                        <div>
                                            <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/folder-2.svg);">
                                                &nbsp;
                                            </div>
                                        </div>
                                        <br />
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Buka Semester
                                        </label>
                                        <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                            Buat Pengaturan semester baru
                                        </label>
                                    </div>
                                </div>
                            </div>
                            <div runat="server" id="div_perpustakaan" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">
                                <div runat="server" id="div_pengaturan_kunjungan_perpus" class="col-md-3 menu-item" style="padding: 10px;">
                                    <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LIBRARY.PENGATURAN_KUNJUNGAN_PERPUSTAKAAN.ROUTE)  %>');" style="cursor: pointer;">
                                        <div>
                                            <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/test-2.svg);">
                                                &nbsp;
                                            </div>
                                        </div>
                                        <br />
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Pengaturan Kunjungan
                                        </label>
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Perpustakaan
                                        </label>
                                        <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                            Lakukan pengaturan waktu kunjungan perpustakaan
                                        </label>
                                    </div>
                                </div>
                                <div runat="server" id="div_jadwal_kunjungan" class="col-md-3 menu-item" style="padding: 10px;">
                                    <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LIBRARY.PENGATURAN_KUNJUNGAN_PERPUSTAKAAN_RUTIN.ROUTE)  %>');" style="cursor: pointer;">
                                        <div>
                                            <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/093-calendar-1.svg);">
                                                &nbsp;
                                            </div>
                                        </div>
                                        <br />
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Pengaturan Kunjungan
                                        </label>
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Perpustakaan (Rutin)
                                        </label>
                                        <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                            Pengaturan jadwal kunjungan perpustakaan (rutin)
                                        </label>
                                    </div>
                                </div>
                                <div runat="server" id="div_kunjungan_perpus" class="col-md-3 menu-item" style="padding: 10px;">
                                    <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LIBRARY.LIST_KUNJUNGAN_PERPUSTAKAAN.ROUTE)  %>');" style="cursor: pointer;">
                                        <div>
                                            <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/wall-calendar.svg);">
                                                &nbsp;
                                            </div>
                                        </div>
                                        <br />
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Jadwal Kunjungan
                                        </label>
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Perpustakaan
                                        </label>
                                        <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                            Lihat jadwal kunjungan perpustakaan
                                        </label>
                                    </div>
                                </div>

                            </div>

                        </div>

                        <div runat="server" id="div_materi_pembelajaran_oleh_guru" class="row" style="display: none; padding: 30px;">
                            <label style="font-weight: bold; color: grey;">
                                <i class="fa fa-hashtag"></i>
                                Pilihan
                            </label>
                            <hr style="margin-top: 10px; margin-bottom: 10px;" />
                            <div runat="server" id="div_materi_pembelajaran" class="col-md-3 menu-item" style="padding: 10px;">
                                <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_PRAOTA.ROUTE) + (AI_ERP.Application_Libs.Libs.LOGGED_USER_M.UserID == "ortu" ? "?" + AI_ERP.Application_Libs.Libs.URL_IDENTIFIER_ORTU.URL_ID_ORTU : "")  %>');" style="cursor: pointer">
                                    <div>
                                        <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/school-material-0.svg);">
                                            &nbsp;
                                        </div>
                                    </div>
                                    <br />
                                    <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                        Materi Pembelajaran
                                    </label>
                                    <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                        Informasi materi pembelajaran diinformasikan untuk orang tua
                                    </label>
                                </div>
                            </div>
                        </div>

                        <div runat="server" id="div_pilih_kelas_smp_by_guru" class="col-md-3 menu-item" style="padding: 10px;">
                            <div onclick="<%= btnShowListKelasMapel.ClientID %>.click(); " style="cursor: pointer">
                                <div>
                                    <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/browser.svg);">
                                        &nbsp;
                                    </div>
                                </div>
                                <br />
                                <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                    Pilih Kelas
                                </label>
                                <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                    Pilih kelas per mata pelajaran<br />
                                    atau rombongan belajar
                                </label>
                            </div>
                        </div>

                        <div runat="server" id="div_struktur_penilaian_smp_by_guru" class="col-md-3 menu-item" style="padding: 10px;">
                            <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.STRUKTUR_PENILAIAN.ROUTE)  %>');" style="cursor: pointer">
                                <div>
                                    <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/browser-2.svg);">
                                        &nbsp;
                                    </div>
                                </div>
                                <br />
                                <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                    Struktur Penilaian
                                </label>
                                <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                    Pengaturan struktur penilaian
                                    <br />
                                    per mata pelajaran
                                </label>
                            </div>
                        </div>

                        <div runat="server" id="div_pengaturan_admin_all" visible="false">
                            <div class="row" style="padding: 30px; padding-top: 10px; padding-bottom: 10px; font-weight: bold; color: grey;">
                                <i class="fa fa-cogs"></i>
                                &nbsp;
                                Pengaturan
                                <hr style="margin: 0px; margin-top: 10px; margin-bottom: 10px;" />
                            </div>
                            <div runat="server" id="div_pengaturan_admin">
                                <div runat="server" id="div_pengaturan_admin_1" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">

                                    <div runat="server" id="div1" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.USER_MANAGEMENT.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/user.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Jenis User
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan jenis user
                                            </label>
                                        </div>
                                    </div>
                                    <div runat="server" id="div_user_management" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.USER_MANAGEMENT.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/men.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                User Management
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan hak akses user
                                            </label>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>

                        <div runat="server" id="div_pembelajaran_siswa_unit_kb">
                            <div class="row" style="padding: 30px; padding-top: 10px; padding-bottom: 10px; font-weight: bold; color: grey;">
                                <i class="fa fa-book"></i>
                                &nbsp;
                                Unit KB
                                <hr style="margin: 0px; margin-top: 10px; margin-bottom: 10px;" />
                            </div>
                            <div runat="server" id="div_pembelajaran_kb">
                                <div runat="server" id="div_row_master_kb_1" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">
                                    <div runat="server" id="div_data_karyawan_kb" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.PEGAWAI.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.KB) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.KB) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/student-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Data Pegawai, Guru
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Profil/Biodata Pegawai, Guru
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_data_siswa_kb" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.SISWA.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.KB) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.KB) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/student.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Data Siswa
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Lihat Profil/Biodata Siswa
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_data_mapel_kb" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.MAPEL.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.KB) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.KB) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/document.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Mata Pelajaran
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan data Mata Pelajaran
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_data_formasi_guru_kelas_kb" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.FORMASI_GURU_KELAS.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.KB) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.KB) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/boss-1.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Formasi Guru Kelas/
                                            </label>
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Wali Kelas
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan formasi guru kelas/wali kelas.
                                            </label>
                                        </div>
                                    </div>
                                </div>

                                <div runat="server" id="div_row_penilaian_kb_1" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">

                                    <div runat="server" id="div_kb_kriteria_pencapaian" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.KRITERIA_PENILAIAN.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/vocabulary.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Kriteria Penilaian
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan kriteria penilaian perkembangan anak
                                            </label>
                                        </div>
                                    </div>
                                    <div runat="server" id="div_kb_pengaturan_ekskul" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.PENGATURAN_EKSKUL.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/check-box.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Ekstrakurikuler
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan ekstrakurikuler rapor perkembangan anak
                                            </label>
                                        </div>
                                    </div>
                                    <div runat="server" id="div_kb_desain_rapor_lts" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.DESAIN_RAPOR.ROUTE) %>' + '?tr=' + '<%= AI_ERP.Application_Libs.TipeRapor.LTS.ToLower() %>');"
                                            style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/ebook-2.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Desain Rapor <span style="color: red; font-weight: bold;">LTS</span>
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Buat desain rapor LTS perkembangan anak
                                            </label>
                                        </div>
                                    </div>
                                    <div runat="server" id="div_kb_nilai_rapor_lts" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.LIST_NILAI_SISWA.ROUTE) %>' + 
                                                              '?' + 
                                                              '<%= AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT %>' +
                                                              '&tr=' +
                                                              '<%= AI_ERP.Application_Libs.TipeRapor.LTS.ToLower() %>');"
                                            style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/office-material-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Nilai Rapor <span style="color: red; font-weight: bold;">LTS</span>
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Lihat Data Nilai Rapor LTS Siswa
                                            </label>
                                        </div>
                                    </div>

                                </div>
                            
                                <div runat="server" id="div_row_penilaian_kb_2" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">

                                    <div runat="server" id="div_kb_desain_rapor" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.DESAIN_RAPOR.ROUTE) %>'  +
                                                              '?tr=' +
                                                              '<%= AI_ERP.Application_Libs.TipeRapor.SEMESTER.ToLower() %>');"
                                            style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/ebook-2.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Desain Rapor <span style="color: green; font-weight: bold;">Semester</span>
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Buat desain rapor semester perkembangan anak
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_kb_nilai_siswa" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.LIST_NILAI_SISWA.ROUTE) %>' + 
                                                              '?' + 
                                                              '<%= AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT %>'  +
                                                              '&tr=' +
                                                              '<%= AI_ERP.Application_Libs.TipeRapor.SEMESTER.ToLower() %>');"
                                            style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/office-material-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Nilai Rapor <span style="color: green; font-weight: bold;">Semester</span>
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Lihat Data Nilai Rapor Semester Siswa
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_kb_proses_rapor" class="col-md-3 menu-item" style="padding: 10px;" visible="false">
                                        <div>
                                            <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/browser-1.svg);">
                                                &nbsp;
                                            </div>
                                        </div>
                                        <br />
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Proses Rapor
                                        </label>
                                        <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                            Proses dan cetak rapor perkembangan anak
                                        </label>
                                    </div>

                                    <div runat="server" id="div_ekskul_kb" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.LIST_NILAI_EKSKUL.ROUTE) %>' + 
                                                              '?' + 
                                                              '<%= AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT %>');"
                                            style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/test.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Nilai Ekstrakurikuler
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Lihat Data Nilai Ekstrakurikuler Siswa
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_file_rapor_kb_lts" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.FILE_RAPOR.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.KB) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.KB) %>' + '&tr=' + '<%= AI_ERP.Application_Libs.TipeRapor.LTS.ToLower() %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/packing-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                File Rapor <span style="color: red; font-weight: bold;">LTS</span>
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengelolaan file Rapor<br />LTS
                                            </label>
                                        </div>
                                    </div>
                                </div>

                                <div runat="server" id="div_row_penilaian_kb_3" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">
                                    <div runat="server" id="div_file_rapor_kb_semester" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.FILE_RAPOR.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.KB) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.KB) %>' + '&tr=' + '<%= AI_ERP.Application_Libs.TipeRapor.SEMESTER.ToLower() %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/packing-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                File Rapor <span style="color: green; font-weight: bold;">Semester</span>
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengelolaan file Rapor<br />Semester
                                            </label>
                                        </div>
                                    </div>

                                    <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.PROSES_RAPOR.ROUTE) %>');"
                                        id="div_tk_proses_rapor" class="col-md-3 menu-item" style="cursor: pointer; padding: 10px;">
                                        <div>
                                            <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/browser-1.svg);">
                                                &nbsp;
                                            </div>
                                        </div>
                                        <br />
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Pengaturan Rapor
                                        </label>
                                        <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                            Pengaturan rapor perkembangan anak
                                        </label>
                                    </div>

                                    <div runat="server" id="div_laporan_presensi_kb" visible="false" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.RPT_ABSENSI_SISWA.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.KB) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.KB) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/circular-graphic.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Rekapitulasi Presensi<br />Murid & Kedisiplinan
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Download & Lihat Rekapitulasi<br />Presensi Murid & Kedisiplinan
                                            </label>
                                        </div>
                                    </div>

                                </div>

                            </div>
                        </div>

                        <div runat="server" id="div_pembelajaran_siswa_unit_tk">
                            <div class="row" style="padding: 30px; padding-top: 10px; padding-bottom: 10px; font-weight: bold; color: grey;">
                                <i class="fa fa-book"></i>
                                &nbsp;
                                Unit TK
                                <hr style="margin: 0px; margin-top: 10px; margin-bottom: 10px;" />
                            </div>
                            <div runat="server" id="div_pembelajaran_tk">
                                <div runat="server" id="div_row_master_tk_1" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">
                                    <div runat="server" id="div_data_karyawan_tk" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.PEGAWAI.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.TK) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.TK) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/student-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Data Pegawai, Guru
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Profil/Biodata Pegawai, Guru
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_data_siswa_tk" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.SISWA.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.TK) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.TK) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/student.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Data Siswa
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Lihat Profil/Biodata Siswa
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_data_mapel_tk" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.MAPEL.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.TK) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.TK) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/document.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Mata Pelajaran
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan data Mata Pelajaran
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_data_formasi_guru_kelas_tk" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.FORMASI_GURU_KELAS.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.TK) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.TK) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/boss-1.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Formasi Guru Kelas/
                                            </label>
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Wali Kelas
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan formasi guru kelas/wali kelas.
                                            </label>
                                        </div>
                                    </div>
                                </div>

                                <div runat="server" id="div_row_penilaian_tk_1" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">

                                    <div runat="server" id="div_data_formasi_guru_mapel_tk" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.FORMASI_GURU_MAPEL.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.TK) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.TK) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/network.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Formasi Guru Mata Pelajaran
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan formasi guru mata pelajaran.
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_tk_kriteria_pencapaian" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.KRITERIA_PENILAIAN.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/vocabulary.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Kriteria Penilaian
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan kriteria penilaian perkembangan anak
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_tk_pengaturan_ekskul" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div 
                                            onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.PENGATURAN_EKSKUL.ROUTE) %>' + 
                                                            '?' + 
                                                            '<%= AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT %>' +
                                                            '&tr=' +
                                                            '<%= AI_ERP.Application_Libs.TipeRapor.SEMESTER.ToLower() %>');"
                                            style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/check-box.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Ekstrakurikuler
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan ekstrakurikuler rapor perkembangan anak
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_tk_desain_rapor_lts" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.DESAIN_RAPOR.ROUTE)  %>' + '?tr=' + '<%= AI_ERP.Application_Libs.TipeRapor.LTS.ToLower() %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/ebook-2.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Desain Rapor <span style="color: red; font-weight: bold;">LTS</span>
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Buat desain rapor LTS perkembangan anak
                                            </label>
                                        </div>
                                    </div>
                                </div>

                                <div runat="server" id="div_row_penilaian_tk_2" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">

                                    <div runat="server" id="div_tk_nilai_siswa_lts" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.LIST_NILAI_SISWA.ROUTE) %>' + 
                                                              '?' + 
                                                              '<%= AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT %>' +
                                                              '&tr=' +
                                                              '<%= AI_ERP.Application_Libs.TipeRapor.LTS.ToLower() %>');"
                                            style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/office-material-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Nilai Rapor <span style="color: red; font-weight: bold;">LTS</span>
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Lihat Data Nilai Rapor LTS Siswa
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_desain_rapor_semester" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.DESAIN_RAPOR.ROUTE)  %>' + '?tr=' + '<%= AI_ERP.Application_Libs.TipeRapor.SEMESTER.ToLower() %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/ebook-2.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Desain Rapor <span style="color: green; font-weight: bold;">Semester</span>
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Buat desain rapor Semester perkembangan anak
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_tk_nilai_siswa_rapor" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.LIST_NILAI_SISWA.ROUTE) %>' + 
                                                              '?' + 
                                                              '<%= AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT %>' +
                                                              '&tr=' +
                                                              '<%= AI_ERP.Application_Libs.TipeRapor.SEMESTER.ToLower() %>');"
                                            style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/office-material-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Nilai Rapor <span style="color: green; font-weight: bold;">Semester</span>
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Lihat Data Nilai Rapor Semester Siswa
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_nilai_ekskul_siswa" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.LIST_NILAI_EKSKUL.ROUTE + 
                                                                   '?' + 
                                                                   AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT) %>');"
                                            style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/test.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Nilai Ekstrakurikuler
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Lihat Data Nilai Ekstrakurikuler Siswa
                                            </label>
                                        </div>
                                    </div>

                                </div>
                                <div runat="server" id="div_row_penilaian_tk_3" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">

                                    <div runat="server" id="div_file_rapor_tk_lts" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.FILE_RAPOR.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.TK) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.TK) %>' + '&tr=' + '<%= AI_ERP.Application_Libs.TipeRapor.LTS.ToLower() %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/packing-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                File Rapor <span style="color: red; font-weight: bold;">LTS</span>
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengelolaan file Rapor LTS
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_file_rapor_tk_semester" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.FILE_RAPOR.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.TK) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.TK) %>' + '&tr=' + '<%= AI_ERP.Application_Libs.TipeRapor.SEMESTER.ToLower() %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/packing-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                File Rapor <span style="color: green; font-weight: bold;">Semester</span>
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengelolaan file Rapor Semester
                                            </label>
                                        </div>
                                    </div>

                                    <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.PROSES_RAPOR.ROUTE) %>');"
                                        id="div_tk_proses_rapor" class="col-md-3 menu-item" style="cursor: pointer; padding: 10px;">
                                        <div>
                                            <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/browser-1.svg);">
                                                &nbsp;
                                            </div>
                                        </div>
                                        <br />
                                        <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                            Pengaturan Rapor
                                        </label>
                                        <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                            Pengaturan rapor perkembangan anak
                                        </label>
                                    </div>

                                    <div runat="server" id="div_laporan_presensi_tk" visible="false" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.RPT_ABSENSI_SISWA.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.TK) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.TK) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/circular-graphic.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Rekapitulasi Presensi<br />Murid & Kedisiplinan
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Download & Lihat Rekapitulasi<br />Presensi Murid & Kedisiplinan
                                            </label>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>

                        <div runat="server" id="div_pembelajaran_siswa_unit_sd">
                            <div class="row" style="padding: 30px; padding-top: 10px; padding-bottom: 10px; font-weight: bold; color: grey;">
                                <i class="fa fa-book"></i>
                                &nbsp;
                                Unit SD
                                <hr style="margin: 0px; margin-top: 10px; margin-bottom: 10px;" />
                            </div>
                            <div runat="server" id="div_pembelajaran_sd">
                                <div runat="server" id="div_row_master_sd_1" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">
                                    <div runat="server" id="div_data_karyawan_sd" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.PEGAWAI.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SD) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SD) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/student-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Data Pegawai, Guru
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Profil/Biodata Pegawai, Guru
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_data_siswa_sd" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.SISWA.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SD) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SD) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/student.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Data Siswa
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Lihat Profil/Biodata Siswa
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_data_mapel_sd" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.MAPEL.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SD) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SD) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/document.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Mata Pelajaran
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan data Mata Pelajaran
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_data_formasi_guru_kelas_sd" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.FORMASI_GURU_KELAS.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SD) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SD) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/boss-1.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Formasi Guru Kelas/Wali Kelas
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan formasi guru kelas/wali kelas.
                                            </label>
                                        </div>
                                    </div>
                                </div>

                                <div runat="server" id="div_penilaian_sd_1" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">
                                    <div runat="server" id="div_data_formasi_guru_mapel_sd" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.FORMASI_GURU_MAPEL.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SD) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SD) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/network.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Formasi Guru Mata Pelajaran
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan formasi guru mata pelajaran.
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_struktur_penilaian_sd" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.STRUKTUR_PENILAIAN.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/browser-2.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Struktur Penilaian
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan struktur penilaian
                                                <br />
                                                per mata pelajaran
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_desain_rapor_lts_sd" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.DESAIN_RAPOR_LTS.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/ebook-2.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Desain Rapor LTS
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Buat desain/pengaturan rapor LTS
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_desain_rapor_sd" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.DESAIN_RAPOR.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/036-certificate.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Desain Rapor
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Buat desain/pengaturan rapor
                                            </label>
                                        </div>
                                    </div>
                                </div>

                                <div runat="server" id="div_penilaian_sd2" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">
                                    <div runat="server" id="div_proses_rapor_sd" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.PROSES_RAPOR.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/browser-1.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Proses Rapor
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Proses Rapor LTS & Rapor Semester
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_data_nilai_sd" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.LIST_NILAI_SISWA.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/office-material-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Nilai Siswa
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Lihat Data Nilai Siswa
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_nilai_ekskul_sd" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.LIST_NILAI_EKSKUL.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/text-lines.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Nilai Ekstrakurikuler
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Lihat Data Nilai Ekstrakurikuler
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_pengaturan_volunteer" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.PENGATURAN_VOLUNTEER.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/check-box.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Pengaturan Volunteer
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Lakukan pengaturan Volunteer
                                            </label>
                                        </div>
                                    </div>
                                </div>

                                <div runat="server" id="div_penilaian_sd3" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">
                                    <div runat="server" id="div_file_rapor_sd_lts" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.FILE_RAPOR.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SD) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SD) %>' + '&tr=' + '<%= AI_ERP.Application_Libs.TipeRapor.LTS.ToLower() %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/packing-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                File Rapor <span style="color: red; font-weight: bold;">LTS</span>
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengelolaan file Rapor LTS
                                            </label>
                                        </div>
                                    </div>
                                    <div runat="server" id="div_file_rapor_sd_semester" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.FILE_RAPOR.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SD) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SD) %>' + '&tr=' + '<%= AI_ERP.Application_Libs.TipeRapor.SEMESTER.ToLower() %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/packing-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                File Rapor <span style="color: green; font-weight: bold;">Semester</span>
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengelolaan file Rapor Semester
                                            </label>
                                        </div>
                                    </div>
                                    <div runat="server" id="div_pengaturan_sd" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.PENGATURAN_RAPOR_SD.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/check-mark.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Pengaturan Rapor
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Lakukan pengaturan Rapor
                                            </label>
                                        </div>
                                    </div>
                                    <div runat="server" id="div_pengaturan_umum_sd" class="col-md-3 menu-item" style="display: none; padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.PENGATURAN_SD.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/002-website.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Pengaturan Umum
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan aplikasi secara umum
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_laporan_presensi_sd" visible="false" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.RPT_ABSENSI_SISWA.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SD) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SD) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/circular-graphic.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Rekapitulasi Presensi<br />Murid & Kedisiplinan
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Download & Lihat Rekapitulasi<br />Presensi Murid & Kedisiplinan
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div runat="server" id="div_pembelajaran_siswa_unit_smp">
                            <div class="row" style="padding: 30px; padding-top: 10px; padding-bottom: 10px; font-weight: bold; color: grey;">
                                <i class="fa fa-book"></i>
                                &nbsp;
                                Unit SMP
                                <hr style="margin: 0px; margin-top: 10px; margin-bottom: 10px;" />
                            </div>
                            <div runat="server" id="div_pembelajaran_smp">
                                <div runat="server" id="div_row_master_smp_1" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">
                                    <div runat="server" id="div_data_karyawan_smp" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.PEGAWAI.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMP) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMP) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/student-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Data Pegawai, Guru
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Profil/Biodata Pegawai, Guru
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_data_siswa_smp" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.SISWA.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMP) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMP) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/student.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Data Siswa
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Lihat Profil/Biodata Siswa
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_data_mapel_smp" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.MAPEL.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMP) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMP) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/document.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Mata Pelajaran
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan data Mata Pelajaran
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_data_formasi_guru_kelas_smp" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.FORMASI_GURU_KELAS.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMP) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMP) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/boss-1.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Formasi Guru Kelas/Wali Kelas
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan formasi guru kelas/wali kelas.
                                            </label>
                                        </div>
                                    </div>
                                </div>

                                <div runat="server" id="div_penilaian_smp_1" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">
                                    <div runat="server" id="div_data_formasi_guru_mapel_smp" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.FORMASI_GURU_MAPEL.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMP) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMP) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/network.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Formasi Guru Mata Pelajaran
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan formasi guru mata pelajaran.
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_struktur_penilaian_smp" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.STRUKTUR_PENILAIAN.ROUTE)  %>' + '?' + '<%= AI_ERP.Application_Libs.Libs.URL_IDENTIFIER.URL_ID_ADMIN_UNIT %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/browser-2.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Struktur Penilaian
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan struktur penilaian
                                                <br />
                                                per mata pelajaran
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_formasi_ekskul_smp" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.FORMASI_EKSKUL.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/running.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Ekstrakurikuler
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan dan pengisian nilai
                                                <br />
                                                Ekstrakurikuler Siswa
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_desain_rapor_lts_smp" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.DESAIN_RAPOR_LTS.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/ebook-2.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Desain Rapor LTS
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Buat desain/pengaturan rapor LTS
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div runat="server" id="div_penilaian_smp_2" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">
                                    <div runat="server" id="div_desain_rapor_smp" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.DESAIN_RAPOR.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/036-certificate.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Desain Rapor
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Buat desain/pengaturan rapor
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_proses_rapor_smp" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.PROSES_RAPOR.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/browser-1.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Proses Rapor
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Proses Rapor LTS & Rapor Semester
                                            </label>
                                        </div>
                                    </div>
                                    <div runat="server" id="div_data_nilai_smp" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.LIST_NILAI_SISWA.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/office-material-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Nilai Siswa
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Lihat Data Nilai Siswa
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_file_rapor_smp_lts" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.FILE_RAPOR.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMP) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMP) %>' + '&tr=' + '<%= AI_ERP.Application_Libs.TipeRapor.LTS.ToLower() %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/packing-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                File Rapor <span style="color: red; font-weight: bold;">LTS</span>
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengelolaan file Rapor LTS
                                            </label>
                                        </div>
                                    </div>

                                    <div visible="false" runat="server" id="div_materi_pembelajaran_smp" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_PRAOTA.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/school-material-0.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Materi Pembelajaran
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Informasi materi pembelajaran untuk orang tua
                                            </label>
                                        </div>
                                    </div>
                                </div>

                                <div runat="server" id="div_penilaian_smp_3" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">

                                    <div runat="server" id="div_file_rapor_smp_semester" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.FILE_RAPOR.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMP) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMP) %>' + '&tr=' + '<%= AI_ERP.Application_Libs.TipeRapor.SEMESTER.ToLower() %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/packing-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                File Rapor <span style="color: green; font-weight: bold;">Semester</span>
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengelolaan file Rapor Semester
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_pengaturan_smp" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.PENGATURAN_SMP.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/check-mark.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Pengaturan Rapor
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Lakukan pengaturan Rapor
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_laporan_presensi_smp" visible="false" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.RPT_ABSENSI_SISWA.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMP) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMP) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/circular-graphic.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Rekapitulasi Presensi<br />Murid & Kedisiplinan
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Download & Lihat Rekapitulasi<br />Presensi Murid & Kedisiplinan
                                            </label>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>

                        <div runat="server" id="div_laporan">
                            <div class="row" style="padding: 30px; padding-top: 10px; padding-bottom: 10px; font-weight: bold; color: grey;">
                                <i class="fa fa-book"></i>
                                &nbsp;
                                Laporan
                                <hr style="margin: 0px; margin-top: 10px; margin-bottom: 10px;" />
                            </div>
                            <div runat="server" id="div_laporan_presensi_0">

                                <div runat="server" id="div_laporan_presensi_1" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">
                                    <div runat="server" id="div_laporan_presensi_2" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.RPT_ABSENSI_SISWA.ROUTE) %>?token=8kfdsjhfsdf2fsdf234fdf');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/circular-graphic.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Rekapitulasi Presensi<br />Murid & Kedisiplinan
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Download & Lihat Rekapitulasi<br />Presensi Murid & Kedisiplinan
                                            </label>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>

                        <div runat="server" id="div_pembelajaran_siswa_unit_sma">
                            <div class="row" style="padding: 30px; padding-top: 10px; padding-bottom: 10px; font-weight: bold; color: grey;">
                                <i class="fa fa-book"></i>
                                &nbsp;
                                Unit SMA
                                <hr style="margin: 0px; margin-top: 10px; margin-bottom: 10px;" />
                            </div>
                            <div runat="server" id="div_pembelajaran_sma">

                                <div runat="server" id="div_row_master_sma_1" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">
                                    <div runat="server" id="div_data_karyawan_sma" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.PEGAWAI.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMA) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMA) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/student-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Data Pegawai, Guru
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Profil/Biodata Pegawai, Guru
                                            </label>
                                        </div>
                                    </div>
                                    <div runat="server" id="div_data_siswa_sma" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.SISWA.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMA) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMA) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/student.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Data Siswa
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Lihat Profil/Biodata Siswa
                                            </label>
                                        </div>
                                    </div>
                                    <div runat="server" id="div_data_mapel_sma" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.MAPEL.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMA) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMA) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/document.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Mata Pelajaran
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan data Mata Pelajaran
                                            </label>
                                        </div>
                                    </div>
                                    <div runat="server" id="div_data_jadwal_mapel_sma" class="col-md-3 menu-item" style="padding: 10px;" visible="true">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.JADWAL_MAPEL.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMA) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMA) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/text-lines.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Jadwal Mata Pelajaran
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan data Jadwal Mata Pelajaran
                                            </label>
                                        </div>
                                    </div>
                                </div>

                                <div runat="server" id="div_penilaian_sma_1" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">

                                    <div runat="server" id="div_data_formasi_guru_kelas_sma" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.FORMASI_GURU_KELAS.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMA) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMA) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/boss-1.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Formasi Guru Kelas/Wali Kelas
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan formasi guru kelas/wali kelas.
                                            </label>
                                        </div>
                                    </div>
                                    <div runat="server" id="div_data_formasi_guru_mapel_sma" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.FORMASI_GURU_MAPEL.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMA) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMA) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/network.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Formasi Guru Mata Pelajaran
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan formasi guru mata pelajaran.
                                            </label>
                                        </div>
                                    </div>
                                    <div runat="server" id="div_struktur_penilaian_sma" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.STRUKTUR_PENILAIAN.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/browser-2.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Struktur Penilaian
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan struktur penilaian
                                                <br />
                                                per mata pelajaran
                                            </label>
                                        </div>
                                    </div>
                                    <div runat="server" id="div_formasi_ekskul_sma" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.FORMASI_EKSKUL.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/running.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Ekstrakurikuler
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan dan pengisian nilai
                                                <br />
                                                Ekstrakurikuler Siswa
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div runat="server" id="div_desain_rapor_sma" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">
                                    <div runat="server" id="div_desain_rapor_lts_sma" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.DESAIN_RAPOR_LTS.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/ebook-2.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Desain Rapor LTS
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Buat desain/pengaturan rapor LTS
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div6" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.DESAIN_RAPOR.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/036-certificate.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Desain Rapor
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Buat desain/pengaturan rapor
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_proses_rapor_sma" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.PROSES_RAPOR.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/browser-1.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Proses Rapor
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Proses Rapor LTS & Rapor Semester
                                            </label>
                                        </div>
                                    </div>
                                    <div runat="server" id="div_data_nilai_sma" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.LIST_NILAI_SISWA.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/office-material-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Nilai Siswa
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Lihat Data Nilai Siswa
                                            </label>
                                        </div>
                                    </div>
                                </div>
                                <div runat="server" id="div_menu_sma_1" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">
                                    <div runat="server" id="div_file_rapor_sma_lts" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.FILE_RAPOR.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMA) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMA) %>' + '&tr=' + '<%= AI_ERP.Application_Libs.TipeRapor.LTS.ToLower() %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/packing-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                File Rapor <span style="color: red; font-weight: bold;">LTS</span>
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengelolaan file Rapor LTS
                                            </label>
                                        </div>
                                    </div>

                                    <div runat="server" id="div_file_rapor_sma_semester" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.FILE_RAPOR.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMA) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMA) %>' + '&tr=' + '<%= AI_ERP.Application_Libs.TipeRapor.SEMESTER.ToLower() %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/packing-3.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                File Rapor <span style="color: green; font-weight: bold;">Semester</span>
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengelolaan file Rapor Semester
                                            </label>
                                        </div>
                                    </div>

                                    <div visible="true" runat="server" id="div_pengaturan_sma" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.PENGATURAN_RAPOR_SMA.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/check-mark.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Pengaturan Rapor
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Lakukan pengaturan rapor
                                            </label>
                                        </div>
                                    </div>

                                    <div visible="true" runat="server" id="div_pengaturan_umum_sma" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.MASTER.PENGATURAN_SMA.ROUTE)  %>');" style="cursor: pointer">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/002-website.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Pengaturan Umum
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Pengaturan aplikasi secara umum
                                            </label>
                                        </div>
                                    </div>
                                </div>

                                <div runat="server" visible="false" id="div_menu_sma_2" class="row" style="padding: 30px; padding-top: 0px; font-weight: bold; color: grey;">
                                    <div runat="server" id="div_laporan_presensi_sma" class="col-md-3 menu-item" style="padding: 10px;">
                                        <div onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.RPT_ABSENSI_SISWA.ROUTE) %>?unit=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetKodeSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMA) %>&token=<%= AI_ERP.Application_DAOs.DAO_Sekolah.GetTokenSekolah(AI_ERP.Application_Libs.Libs.UnitSekolah.SMA) %>');" style="cursor: pointer;">
                                            <div>
                                                <div style="margin: 0 auto; display: table; height: 60px; width: 60px; background: url(Application_CLibs/images/svg/circular-graphic.svg);">
                                                    &nbsp;
                                                </div>
                                            </div>
                                            <br />
                                            <label style="margin: 0 auto; display: table; font-weight: bold; color: black;">
                                                Rekapitulasi Presensi<br />Murid & Kedisiplinan
                                            </label>
                                            <label style="width: 100%; text-align: center; font-weight: normal; color: grey; font-size: small;">
                                                Download & Lihat Rekapitulasi<br />Presensi Murid & Kedisiplinan
                                            </label>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>

                    </div>
                    <div class="col-md-4">

                        <div class="row">
                            <div class="col-xs-12">

                                <label style="font-weight: bold; color: white; background-color: mediumvioletred; width: 100%; padding: 15px; border-style: none; border-width: 1px; border-color: #E4DFDF; margin-bottom: 0px; margin-top: 5px; box-shadow: 0 -1px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);">
                                    <i class="fa fa-calendar"></i>
                                    &nbsp;<%= AI_ERP.Application_Libs.Libs.Array_Bulan[DateTime.Now.Month - 1] + " " + DateTime.Now.Year.ToString() %>
                                    <label class="pull-right" style="font-weight: bold; color: #00A7CC; cursor: pointer; display: none;">
                                        <img src="Application_CLibs/images/svg/calendar.svg"
                                            style="margin: 0 auto; height: 16px; width: 16px;" />
                                        &nbsp;
                                        Selengkapnya...
                                    </label>
                                    <label class="pull-right" style="font-weight: normal; cursor: pointer;">
                                        <span style="font-weight: bold;">
                                            <%= AI_ERP.Application_Libs.Libs.GetNamaHariFromTanggal(DateTime.Now) %>,
                                        </span>
                                        <span>
                                            <%= AI_ERP.Application_Libs.Libs.GetTanggalIndonesiaSingkatFromDate(DateTime.Now, false) %>
                                        </span>
                                    </label>
                                </label>
                                <br />
                                <div style="width: 100%; background-color: white; padding: 10px; padding-top: 5px; border-style: solid; border-width: 1px; border-color: #e9e9e9;">
                                    <asp:Literal runat="server" ID="ltrKalender"></asp:Literal>
                                </div>
                                <div style="display: none; font-weight: bold; color: grey; width: 100%; background-color: white; padding: 15px; border-style: none; border-width: 1px; border-color: #E4DFDF; margin-bottom: 5px; margin-top: 0px; border-top-style: none;">
                                    <label style="color: grey; width: 100%; font-size: small;">
                                        <span style="color: mediumvioletred;"><i class="fa fa-calendar"></i>&nbsp; 1 Libur Nasional Maulid Nabi Muhammad SAW</span><br />
                                        <hr style="margin: 0px; margin-top: 5px; margin-bottom: 5px;" />
                                        <i class="fa fa-calendar"></i>&nbsp; 4 - 8 Ulangan Akhir Semester (UAS) I
                                        <hr style="margin: 0px; margin-top: 5px; margin-bottom: 5px;" />
                                        <span style="color: mediumvioletred;"><i class="fa fa-calendar"></i>&nbsp; 25 - 26 Libur Nasional Hari Raya Natal</span><br />
                                    </label>
                                </div>

                            </div>
                        </div>

                        <div class="row" runat="server" id="div_info_perpustakaan" style="margin-top: 5px;">
                            <div class="col-xs-12">

                                <label style="font-weight: bold; color: white; background-color: #44877b; width: 100%; padding: 15px; border-style: none; border-width: 1px; border-color: #E4DFDF; margin-bottom: 0px; margin-top: 0px; box-shadow: 0 0px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);">
                                    <i class="fa fa-book"></i>
                                    &nbsp;
                                    Kunjungan Perpustakaan
                                </label>
                                <div style="border-style: solid; border-width: 0px; border-color: #dddddd;">
                                    <asp:Literal runat="server" ID="ltrInfoPerpustakaan"></asp:Literal>
                                </div>
                                <div style="padding: 0px; margin: 0px; background-color: white;">
                                    <asp:ListView ID="lvDataKunjunganPerpus" DataSourceID="sql_ds_kunjungan_perpus" runat="server" OnSorting="lvDataKunjunganPerpus_Sorting" OnPagePropertiesChanging="lvDataKunjunganPerpus_PagePropertiesChanging">
                                        <LayoutTemplate>
                                            <div class="table-responsive" style="margin: 0px; box-shadow: none;">
                                                <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                    <tbody>
                                                        <tr id="itemPlaceholder" runat="server"></tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                            <div style="padding: 0px; margin: 0px; background-color: white; padding: 10px;">
                                                <div style="margin: 0 auto; display: table;">
                                                    <asp:DataPager ID="dpDataKunjunganPerpus" runat="server" PageSize="5" PagedControlID="lvDataKunjunganPerpus">
                                                        <Fields>
                                                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="page-button" FirstPageText='&nbsp;<i class="fa fa-backward"></i>&nbsp;' ShowFirstPageButton="true" ShowPreviousPageButton="True" PreviousPageText='&nbsp;<i class="fa fa-arrow-left"></i>&nbsp;&nbsp;Sebelumnya&nbsp;' ShowNextPageButton="false" />
                                                            <asp:TemplatePagerField>
                                                                <PagerTemplate>
                                                                    <label style="font-size: small; color: grey; font-weight: normal; padding: 5px; border-style: solid; border-color: #F1F1F1; border-width: 1px; padding-left: 10px; padding-right: 10px; border-radius: 5px;">
                                                                        Hal.
                                                                        <%# ((Container.StartRowIndex + 1) / (Container.PageSize)) + 1 %>
                                                                        &nbsp;/&nbsp;
                                                                        <%# 
                                                                            Container.TotalRowCount % Container.PageSize == 0
                                                                            ? (
                                                                                Container.TotalRowCount == 0
                                                                                ? Math.Floor(Convert.ToDecimal(1))
                                                                                : Math.Floor(Convert.ToDecimal((Container.TotalRowCount) / (Container.PageSize)))
                                                                              )
                                                                            : (
                                                                                Container.TotalRowCount == 0
                                                                                ? Math.Floor(Convert.ToDecimal(1))
                                                                                : Math.Floor(Convert.ToDecimal((Container.TotalRowCount) / (Container.PageSize))) + 1 
                                                                              )
                                                                        %>
                                                                    </label>
                                                                    &nbsp;
                                                                </PagerTemplate>
                                                            </asp:TemplatePagerField>
                                                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="page-button" LastPageText='&nbsp;<i class="fa fa-forward"></i>&nbsp;' ShowLastPageButton="true" ShowNextPageButton="True" NextPageText='&nbsp;Berikutnya&nbsp;&nbsp;<i class="fa fa-arrow-right"></i>&nbsp;' ShowPreviousPageButton="false" />
                                                            <asp:TemplatePagerField>
                                                                <PagerTemplate>
                                                                </PagerTemplate>
                                                            </asp:TemplatePagerField>
                                                        </Fields>
                                                    </asp:DataPager>
                                                </div>
                                            </div>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr class="<%# (Container.DisplayIndex % 2 == 0 ? "standardrow" : "oddrow") %>">
                                                <td style="text-align: center; padding: 10px; vertical-align: top; color: #bfbfbf;">
                                                    <span style="font-weight: normal;">
                                                        <%# (int)(this.Session[SessionViewDataName_KunjunganPerpustakaan] == null ? 0 : this.Session[SessionViewDataName_KunjunganPerpustakaan]) + (Container.DisplayIndex + 1) %>.
                                                    </span>
                                                </td>
                                                <td style="padding: 10px; vertical-align: middle; text-align: left; font-size: small;">
                                                    <div style="cursor: pointer;" onclick="<%= txtIDKunjungan.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowInfoKunjungan.ClientID %>.click();">
                                                        <i class="fa fa-calendar" style="color: #bfbfbf;"></i>&nbsp;
                                                        <label style="cursor: pointer; color: #44877b; font-weight: bold; text-transform: none; text-decoration: none;">
                                                            <%# 
                                                                AI_ERP.Application_Libs.Libs.GetTanggalIndonesiaFromDate(Convert.ToDateTime(Eval("Tanggal")), false)
                                                            %>
                                                        </label>
                                                        <sup class="badge" style="font-size: x-small; color: grey; background-color: white; border-style: solid; border-width: 1px; border-color: #bfbfbf; font-weight: normal; text-transform: none; text-decoration: none;">
                                                            <%# 
                                                                Eval("Kelas").ToString()
                                                            %>
                                                        </sup>
                                                        <div style="color: grey; font-weight: normal; text-transform: none; text-decoration: none; margin-left: 20px;">
                                                            <%# 
                                                                AI_ERP.Application_Modules.LIBRARY.wf_KunjunganPerpustakaan.GetJamKe(Eval("Kode").ToString(), false)
                                                            %>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td style="padding: 10px; vertical-align: middle; text-align: left;">
                                                    <span style="font-size: small; color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                        <%# 
                                                            AI_ERP.Application_Libs.JenisStatusKunjungan.GetJenisStatus(
                                                                Convert.ToInt16(Eval("Status"))
                                                            )
                                                        %>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="padding: 0px;">
                                                    <hr style="margin: 0px; border-color: #F1F1F1;" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyItemTemplate>
                                            <tbody>
                                                <tr>
                                                    <td colspan="3" style="text-align: center; padding: 10px;">..:: Data Kosong ::..
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </EmptyItemTemplate>
                                    </asp:ListView>
                                    <asp:SqlDataSource ID="sql_ds_kunjungan_perpus" runat="server"></asp:SqlDataSource>
                                </div>
                            </div>
                        </div>

                        <div class="row" style="margin-top: 5px;">
                            <div class="col-xs-12">

                                <label style="font-weight: bold; color: white; background-color: #3367d6; width: 100%; padding: 15px; border-style: none; border-width: 1px; border-color: #E4DFDF; margin-bottom: 0px; margin-top: 0px; box-shadow: 0 0px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);">
                                    <i class="fa fa-newspaper-o"></i>
                                    &nbsp;
                                    Berita Terkini
                                </label>
                                <div style="display: none; font-weight: bold; color: grey; width: 100%; background-color: white; padding: 15px; border-style: none; border-width: 1px; border-color: #E4DFDF; margin-bottom: 5px; margin-top: 0px; text-align: justify;">
                                    <span style="color: grey; font-weight: bold;"><i class="fa fa-newspaper-o"></i>&nbsp; PSB SMA Tahun Pelajaran 2018/2019</span><br />
                                    <span style="color: grey; font-weight: normal;">Telah dibuka pendaftaran siswa baru SMA Al-Izhar Pondok Labu tahun pelajaran 2018/2019
                                        <a href="http://psb.alizhar.sch.id/sma" target="_blank" style="color: blue;">Selengkapnya...</a>
                                    </span>
                                </div>
                                <div style="font-weight: bold; color: grey; width: 100%; background-color: white; padding: 15px; border-style: solid; border-width: 1px; border-color: #e9e9e9; margin-bottom: 5px; margin-top: 0px; text-align: justify;">
                                    <span style="margin: 0 auto; display: table; color: #bfbfbf; font-weight: normal;">..:: Kosong ::..
                                    </span>
                                </div>

                            </div>
                        </div>

                    </div>

                </div>

                <div aria-hidden="true" class="modal fade" id="ui_modal_biodata_siswa" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                    <div class="modal-dialog modal-xs">
                        <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                            <div class="modal-inner"
                                style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                                <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                    <div class="media-object margin-right-sm pull-left">
                                        <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                    </div>
                                    <div class="media-inner">
                                        <span style="font-weight: bold;">Tampilkan Biodata Siswa
                                        </span>
                                    </div>
                                </div>
                                <div style="width: 100%;">
                                    <div class="row">
                                        <div class="col-lg-12">

                                            <div style="width: 100%; background-color: white; padding-top: 15px;">
                                                <div runat="server" id="div_unit_sekolah_biodata_siswa" class="row" style="margin-left: 15px; margin-right: 15px;">
                                                    <div class="col-xs-12">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= cboUnitSekolahBiodataSiswa.ClientID %>" style="text-transform: none; margin-bottom: 6px;">
                                                                Unit Sekolah
                                                            </label>
                                                            <asp:RequiredFieldValidator ValidationGroup="vldInputBiodataSiswa" runat="server" ID="vldUnitSekolahBiodataSiswa"
                                                                ControlToValidate="cboUnitSekolahBiodataSiswa" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:DropDownList ValidationGroup="vldInputBiodataSiswa" runat="server" ID="cboUnitSekolahBiodataSiswa" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                    <div class="col-xs-12">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= cboTahunAjaranBiodataSiswa.ClientID %>" style="text-transform: none; margin-bottom: 6px;">
                                                                Tahun Pelajaran
                                                            </label>
                                                            <asp:RequiredFieldValidator ValidationGroup="vldInputBiodataSiswa" runat="server" ID="vldTahunAjaranBiodataSiswa"
                                                                ControlToValidate="cboTahunAjaranBiodataSiswa" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:DropDownList ValidationGroup="vldInputBiodataSiswa" runat="server" ID="cboTahunAjaranBiodataSiswa" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                    <div class="col-xs-12">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= cboKelasBiodataSiswa.ClientID %>" style="text-transform: none;">Kelas</label>
                                                            <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldKelasBiodataSiswa"
                                                                ControlToValidate="cboKelasBiodataSiswa" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:DropDownList ValidationGroup="vldInputBiodataSiswa" runat="server" ID="cboKelasBiodataSiswa" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:LinkButton OnClick="lnkOKShowSemuaBiodataSiswa_Click" Visible="false" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKShowSemuaBiodataSiswa" Text="   Semua Data   " Style="float: left;"></asp:LinkButton>
                                <p class="text-right">
                                    <label id="pb_ok_biodata_siswa" style="display: none; font-size: medium; color: grey; font-weight: bold;">
                                        <img src="../../../../Application_CLibs/images/giphy.gif" style="height: 22px; width: 22px;" />
                                        &nbsp;&nbsp;Proses...
                                    </label>
                                    <asp:LinkButton ValidationGroup="vldInputBiodataSiswa" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKShowBiodataSiswa" Text="   Tampilkan   "></asp:LinkButton>
                                    &nbsp;&nbsp;
                                    <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                    <br />
                                    <br />
                                </p>
                            </div>
                        </div>
                    </div>
                </div>

                <div aria-hidden="true" class="modal fade" id="ui_modal_kunjungan_perpus" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                    <div class="modal-dialog modal-xs">
                        <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                            <div class="modal-inner"
                                style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                                <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                    <div class="media-object margin-right-sm pull-left">
                                        <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                    </div>
                                    <div class="media-inner">
                                        <span style="font-weight: bold;">Kunjungan Perpustakaan
                                        </span>
                                    </div>
                                </div>
                                <div style="width: 100%;">
                                    <div class="row">
                                        <div class="col-lg-12">

                                            <div style="width: 100%; background-color: white; padding-top: 15px;">
                                                <div runat="server" id="div3" class="row" style="margin-left: 15px; margin-right: 15px;">
                                                    <div class="col-xs-12">
                                                        <asp:Literal runat="server" ID="ltrInfoKunjungan"></asp:Literal>
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

                <div aria-hidden="true" class="modal fade" id="ui_modal_tampilkan_ledger" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                    <div class="modal-dialog modal-xs">
                        <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                            <div class="modal-inner"
                                style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                                <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                    <div class="media-object margin-right-sm pull-left">
                                        <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                    </div>
                                    <div class="media-inner">
                                        <span style="font-weight: bold;">Tampilkan Ledger Nilai
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
                                                            <label class="label-input" for="<%= cboTahunPelajaranLedgerNilai.ClientID %>" style="text-transform: none; margin-bottom: 6px;">
                                                                Tahun Pelajaran
                                                            </label>
                                                            <asp:RequiredFieldValidator ValidationGroup="vldInputTampilkanLedger" runat="server" ID="vldTahunPelajaranLedgerNilai"
                                                                ControlToValidate="cboTahunPelajaranLedgerNilai" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:DropDownList ValidationGroup="vldInputTampilkanLedger" runat="server" ID="cboTahunPelajaranLedgerNilai" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                    <div class="col-xs-12">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= cboKelasLedgerNilai.ClientID %>" style="text-transform: none;">Kelas</label>
                                                            <asp:RequiredFieldValidator ValidationGroup="vldInputTampilkanLedger" runat="server" ID="vldKelasLedgerNilai"
                                                                ControlToValidate="cboKelasLedgerNilai" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:DropDownList ValidationGroup="vldInputTampilkanLedger" runat="server" ID="cboKelasLedgerNilai" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                    <div class="col-xs-12">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= cboKelasLedgerNilai.ClientID %>" style="text-transform: none;">Semester</label>
                                                            <asp:RequiredFieldValidator ValidationGroup="vldInputTampilkanLedger" runat="server" ID="vldSemester"
                                                                ControlToValidate="cboSemesterLedgerNilai" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:DropDownList ValidationGroup="vldInputTampilkanLedger" runat="server" ID="cboSemesterLedgerNilai" CssClass="form-control">
                                                                <asp:ListItem></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                    <div class="col-xs-12">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= cboKurikulum.ClientID %>" style="text-transform: none;">Kurikulum</label>
                                                            <asp:RequiredFieldValidator ValidationGroup="vldInputTampilkanLedger" runat="server" ID="vldKurikulum"
                                                                ControlToValidate="cboKurikulum" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:DropDownList ValidationGroup="vldInputTampilkanLedger" runat="server" ID="cboKurikulum" CssClass="form-control">
                                                                <asp:ListItem></asp:ListItem>
                                                                <asp:ListItem Value="KURTILAS" Text="KURTILAS"></asp:ListItem>
                                                                <asp:ListItem Value="KTSP" Text="KTSP"></asp:ListItem>
                                                            </asp:DropDownList>
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
                                    <label id="pb_ok_ledger_rapor_sma" style="display: none; font-size: medium; color: grey; font-weight: bold;">
                                        <img src="../../../../Application_CLibs/images/giphy.gif" style="height: 22px; width: 22px;" />
                                        &nbsp;&nbsp;Proses...
                                    </label>
                                    <asp:LinkButton ValidationGroup="vldInputTampilkanLedger" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKTampilkanLedger" Text="   Tampilkan   "></asp:LinkButton>
                                    &nbsp;&nbsp;
                                    <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                    <br />
                                    <br />
                                </p>
                            </div>
                        </div>
                    </div>
                </div>

                <div aria-hidden="true" class="modal fade" id="ui_modal_list_kelas_mapel" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                        <span style="font-weight: bold;">Pilih Kelas & Mata Ekstrakurikuler
                                        </span>
                                    </div>
                                </div>
                                <div style="width: 100%;">
                                    <div class="row">
                                        <div class="col-lg-12">

                                            <div style="width: 100%; background-color: white; padding-top: 0px;">

                                                <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                    <div class="col-xs-12">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= cboTahunAjaranEkskul.ClientID %>" style="text-transform: none;">Kelas Ekskul</label>
                                                            <asp:RequiredFieldValidator ValidationGroup="vldInputEkskul" runat="server" ID="vldTahunAjaranEkskul"
                                                                ControlToValidate="cboTahunAjaranEkskul" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:DropDownList ValidationGroup="vldInputEkskul" runat="server" ID="cboTahunAjaranEkskul" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                    <div class="col-xs-12">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= cboListKelasEkskul.ClientID %>" style="text-transform: none;">Kelas Ekskul</label>
                                                            <asp:RequiredFieldValidator ValidationGroup="vldInputEkskul" runat="server" ID="vldListKelasEKskul"
                                                                ControlToValidate="cboListKelasEkskul" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:DropDownList ValidationGroup="vldInputEkskul" runat="server" ID="cboListKelasEkskul" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                    <div class="col-xs-12">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= cboListMapelEkskul.ClientID %>" style="text-transform: none;">Mata Ekskul</label>
                                                            <asp:RequiredFieldValidator ValidationGroup="vldInputEkskul" runat="server" ID="vldMapelEkskul"
                                                                ControlToValidate="cboListMapelEkskul" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:DropDownList ValidationGroup="vldInputEkskul" runat="server" ID="cboListMapelEkskul" CssClass="form-control">
                                                            </asp:DropDownList>
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
                                    <a onclick="window.open(
                                        <%= 
                                            "'" +
                                            ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_TIMELINE.ROUTE) +
                                            "'" + 
                                            " + '?t=' + " + cboTahunAjaranEkskul.ClientID + ".value " +
                                            " + '&ft=' + '" + AI_ERP.Application_Libs.Libs.Encryptdata("b.png") + "' " +
                                            " + '&m=' + " + cboListMapelEkskul.ClientID + ".value " +
                                            " + '&kd=' + " + cboListKelasEkskul.ClientID + ".value "
                                        %>, '_blank');"
                                        class="btn btn-flat btn-brand-accent waves-attach waves-effect">Buka</a>
                                    <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Tutup</a>
                                    <br />
                                    <br />
                                </p>
                            </div>
                        </div>
                    </div>
                </div>

                <div aria-hidden="true" class="modal fade" id="ui_modal_buka_semester" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                    <div class="modal-dialog modal-xs">
                        <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                            <div class="modal-inner"
                                style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                                <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                    <div class="media-object margin-right-sm pull-left">
                                        <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                    </div>
                                    <div class="media-inner">
                                        <span style="font-weight: bold;">Buka Semester
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
                                                                    <label class="label-input" for="<%= cboUnitBukaSemester.ClientID %>" style="text-transform: none;">Unit</label>
                                                                    <asp:RequiredFieldValidator ValidationGroup="vldInputBukaSemester" runat="server" ID="vldUnitBukaSemester"
                                                                        ControlToValidate="cboUnitBukaSemester" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                                        Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                    <asp:DropDownList ValidationGroup="vldInputBukaSemester" runat="server" ID="cboUnitBukaSemester" CssClass="form-control"></asp:DropDownList>
                                                                </div>
                                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                    <label class="label-input" for="<%= txtTahunPelajaranBukaSemester.ClientID %>" style="text-transform: none;">Tahun Pelajaran</label>
                                                                    <asp:TextBox ValidationGroup="vldInputBukaSemester" runat="server" Enabled="false" ID="txtTahunPelajaranBukaSemester" CssClass="form-control"></asp:TextBox>
                                                                </div>
                                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                    <label class="label-input" for="<%= txtSemesterPelajaranBukaSemester.ClientID %>" style="text-transform: none;">Semester</label>
                                                                    <asp:TextBox ValidationGroup="vldInputBukaSemester" runat="server" Enabled="false" ID="txtSemesterPelajaranBukaSemester" CssClass="form-control"></asp:TextBox>
                                                                </div>
                                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                    <label class="label-input" for="<%= cboBukaSemesterCopyDari.ClientID %>" style="text-transform: none;">Copy Dari Periode</label>
                                                                    <asp:RequiredFieldValidator ValidationGroup="vldInputBukaSemester" runat="server" ID="vldBukaSemesterCopyDari"
                                                                        ControlToValidate="cboBukaSemesterCopyDari" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                                        Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                    <asp:DropDownList ValidationGroup="vldInputBukaSemester" runat="server" ID="cboBukaSemesterCopyDari" CssClass="form-control"></asp:DropDownList>
                                                                </div>
                                                                <br />
                                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                    <label style="color: darkorange; width: 100%; text-align: justify;">
                                                                        <i class="fa fa-exclamation-triangle"></i>
                                                                        <label style="font-weight: bold; margin-bottom: 10px;">Perhatian</label>
                                                                        <br />
                                                                        Fitur ini digunakan saat awal semester untuk membuat pengaturan baru dalam satu semester 
                                                                    berdasarkan pengaturan pada semester sebelumnya.
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
									<br />
                                        <br />
                                    </div>
                                </div>

                                <div class="row" id="div_command_buka_semester" style="margin-left: 0px; margin-right: 0px;">
                                    <p class="text-right">
                                        <asp:LinkButton ValidationGroup="vldInputBukaSemester" CausesValidation="true" OnClientClick="if(Page_ClientValidate('vldInputBukaSemester')) { ShowProsesBukaSemester(true); ReportProcessBukaSemester(); return false; }" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKBukaSemester" Text="  OK  "></asp:LinkButton>
                                        <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                        <br />
                                        <br />
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div aria-hidden="true" class="modal fade" id="ui_modal_absensi" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

                    <label title=" Tutup " data-dismiss="modal" style="padding: 10px; padding-top: 6px; padding-bottom: 6px; background-color: white; cursor: pointer; position: fixed; right: 35px; top: 20px; z-index: 999999; border-radius: 100%;">
                        <i class="fa fa-times" style="color: black; font-size: large; font-weight: normal;"></i>
                    </label>

                    <div class="modal-dialog modal-md">
                        <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                            <div class="modal-inner"
                                style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                                <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                    <div class="media-object margin-right-sm pull-left">
                                        <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                    </div>
                                    <div class="media-inner">
                                        <span style="font-weight: bold;">Absensi Siswa
                                        </span>
                                    </div>
                                </div>
                                <div style="width: 100%;">
                                    <div class="row">
                                        <div class="col-lg-12">

                                            <div style="width: 100%; background-color: white; padding-top: 0px;">

                                                <div class="row" style="">
                                                    <div class="col-xs-12">
                                                        <asp:Literal runat="server" ID="ltrAbsensi"></asp:Literal>
                                                    </div>
                                                </div>

                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <p class="text-right">
                                    <asp:LinkButton OnClientClick="ParseAbsensi();" class="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKAbsensi" OnClick="lnkOKAbsensi_Click">SIMPAN</asp:LinkButton>
                                    <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                    <br />
                                    <br />
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <iframe name="fra_buka_semester" id="fra_buka_semester" height="0" width="0" style="position: absolute; left: -1000px; top: -1000px;"></iframe>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
</asp:Content>
