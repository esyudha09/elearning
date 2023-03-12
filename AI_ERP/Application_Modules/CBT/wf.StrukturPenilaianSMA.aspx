<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="wf.StrukturPenilaianSMA.aspx.cs" Inherits="AI_ERP.Application_Modules.CBT.wf_StrukturPenilaianSMA" %>

<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<%@ Register TagPrefix="ucl" TagName="AutocompleteAspekPenilaian" Src="~/Application_Controls/Elearning/SMA/AutocompleteAspekPenilaian/AutocompleteAspekPenilaian.ascx" %>
<%@ Register TagPrefix="ucl" TagName="AutocompleteKompetensiDasar" Src="~/Application_Controls/Elearning/SMA/AutocompleteKompetensiDasar/AutocompleteKompetensiDasar.ascx" %>
<%@ Register TagPrefix="ucl" TagName="AutocompleteKomponenPenilaian" Src="~/Application_Controls/Elearning/SMA/AutocompleteKomponenPenilaian/AutocompleteKomponenPenilaian.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">
        function GoToURL(url) {
            document.location.href = url;
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
                    //SetScrollPos();
                    break;
                case "<%= JenisAction.Add %>":
                    
                    ValidateInputByKurikulum();
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowPreviewNilai %>":
                    $('#ui_modal_preview').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.AddWithMessage %>":
                    
                    
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
                    
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.AddKDWithMessage %>":
                    
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.AddKPWithMessage %>":
                    
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
                    
                    $('#ui_modal_input_kompetensi_dasar_sikap').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowBukaSemester %>":
                    $('#ui_modal_buka_semester').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowLihatData %>":
                    $('#ui_modal_lihat_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowInputDeskripsiLTSRapor %>":
                    
                    $('#ui_modal_deskripsi_lts_rapor').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.Update %>":
                    
                    break;
                case "<%= JenisAction.Delete %>":
                    
                    break;
                case "<%= JenisAction.DoAdd %>":
                    
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoUpdate %>":
                    
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah diupdate',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoDelete %>":
                    
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah dihapus',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoShowInfoAdaStrukturNilai %>":
                    
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Pengaturan struktur nilai sudah ada',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                default:
                    
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
          
            ShowProgress(false);
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




            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowStrukturNilai" OnClick="btnShowStrukturNilai_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDataList" OnClick="btnShowDataList_Click" Style="position: absolute; left: -1000px; top: -1000px;" />





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

                                        <asp:View runat="server" ID="vList">

                                            <div style="padding: 0px; margin: 0px;">
                                                <asp:ListView ID="lvData" DataSourceID="sql_ds" runat="server" OnSorting="lvData_Sorting" OnPagePropertiesChanging="lvData_PagePropertiesChanging">
                                                    <LayoutTemplate>
                                                        <div class="table-responsive" style="margin: 0px; box-shadow: none;">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr style="background-color: #3367d6;">
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle; width: 50px;">#
                                                                        </th>

                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">Tahun Ajaran, Mapel
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: center; padding-left: 10px; vertical-align: middle;">Kelas
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: right; padding-left: 10px; vertical-align: middle;">Status
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">KKM
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;"></th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;"></th>
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
                                                                <label title=" Struktur Penilaian "
                                                                    id="lbl_<%# Eval("Kode").ToString().Replace("-", "_") %>"
                                                                    onclick=" <%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>';<%= btnShowStrukturNilai.ClientID %>.click();"
                                                                  >
                                                                    <i class="fa fa-sitemap"></i> STRUKTUR NILAI
                                                                </label>
                                                                </td>
                                                              <td style="text-align: right; vertical-align: middle;">
                                                                <label title=" Bank Soal "
                                                                    id="bankSoal"
                                                                   onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.CBT.SOAL.ROUTE) %>?m=<%#  AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Rel_Mapel").ToString()) %>&k=<%#  AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Rel_Kelas").ToString()) %>');"
                                                                   >
                                                                    <i class="fa fa-files-o"></i> BANK SOAL
                                                                </label>

<%--                                                                  <label
                                                                    onclick="GoToURL('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.CBT.SOAL.ROUTE) %>?m=<%#  AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kode").ToString()) %>');"
                                                                    title=" Lihat Data Soal" class="btn btn-brand"
                                                                    >
                                                                    Bank Soal
                                                                </label>--%>
                                                                    
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
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">Tahun Ajaran, Mapel
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: center; padding-left: 10px; vertical-align: middle;">Kelas
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">Status
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">KKM
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;"></th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="7" style="text-align: center; padding: 10px;">..:: Data Kosong ::..
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
                                                style="background-color: white; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 10; position: fixed; bottom: 28px; right: 25px; width: 320px; border-radius: 25px; padding: 8px; margin: 0px;">

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

                                            <div class="content-header ui-content-header"
                                                style="background-color: #00198d; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 33px; right: 50px; width: 450px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                                                <div style="padding-left: 0px;">
                                                    <asp:LinkButton ToolTip=" Kembali " runat="server" ID="LinkButton3"
                                                        OnClick="btnBackToMapel_Click"
                                                        CssClass="btn-trans waves-attach waves-circle waves-effect" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;Mata Pelajaran
                                                    </asp:LinkButton>
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
                                                style="background-color: #00198d; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 33px; right: 50px; width: 295px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                                                <div style="padding-left: 0px;">
                                                    <asp:LinkButton ToolTip=" Kembali " runat="server" ID="btnShowDataListFromKTSPDet" CssClass="btn-trans waves-attach waves-circle waves-effect" OnClick="btnShowDataListFromKTSPDet_Click" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;
                                                    </asp:LinkButton>
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
                                                <div style="padding-left: 0px;">
                                                    <asp:LinkButton ToolTip=" Kembali " runat="server" ID="btnShowDataListFromKURTILASDet" CssClass="btn-trans waves-attach waves-circle waves-effect" OnClick="btnShowDataListFromKURTILASDet_Click" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;
                                                    </asp:LinkButton>
                                                </div>
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
                style="background-color: #00198d; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 33px; right: 50px; width: 270px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
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
                style="background-color: red; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 6; position: fixed; bottom: 33px; right: 50px; width: 120px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
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
