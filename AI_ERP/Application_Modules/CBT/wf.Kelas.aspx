<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master"  AutoEventWireup="true" CodeBehind="wf.Kelas.aspx.cs" Inherits="AI_ERP.Application_Modules.CBT.wf_Kelas" %>

<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">
       
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




            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnStrukturNilai" OnClick="btnShowStrukturNilai_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <%--<asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDataList" OnClick="btnShowDataList_Click" Style="position: absolute; left: -1000px; top: -1000px;" />--%>





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
                                                                <label title=" Pengaturan Struktur Penilaian "
                                                                    id="tes"
                                                                    onclick=" <%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>';<%= btnStrukturNilai.ClientID %>.click();"
                                                                    >
                                                                    <i class="fa fa-file-text-o"></i>
                                                                </label>
                                                                <%--<span style="color: #d9d9d9;
                                                                             <%# 
                                                                                (
                                                                                    Convert.ToBoolean(Eval("IsNilaiAkhir") == DBNull.Value ? false : Eval("IsNilaiAkhir"))
                                                                                    ? "display: none;"
                                                                                    : (
                                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kurikulum").ToString()).ToUpper() != AI_ERP.Application_Libs.Libs.JenisKurikulum.SMA.KURTILAS.ToUpper() &&
                                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kurikulum").ToString()).ToUpper() != AI_ERP.Application_Libs.Libs.JenisKurikulum.SMA.KURTILAS_SIKAP.ToUpper()
                                                                                        ? (
                                                                                            AI_ERP.Application_Modules.EDUCATION.Penilaian.SMA.wf_StrukturPenilaian.IsMapelEkskul(
                                                                                                Eval("Rel_Mapel").ToString()
                                                                                            ) ? ""
                                                                                                : "display: none;"
                                                                                            )
                                                                                        : ""
                                                                                      )
                                                                                )
                                                                             %>">--%>
                                                                    
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

                                            <%--<div class="fbtn-container" id="div_button_settings" runat="server">
                                                <div class="fbtn-inner">
                                                    <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #a91212;" title=" Pilihan ">
                                                        <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                                                        <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                                                    </a>
                                                    <div class="fbtn-dropup" style="z-index: 999999;">
                                                         <asp:LinkButton OnClick="btnRefresh_Click" CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" id="btnRefresh" title=" Refresh " style="background-color: black; color: white;">
                                                            <span class="fbtn-text fbtn-text-left">Refresh Data</span>
                                                            <i class="fa fa-refresh"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ToolTip=" Tambah Data " runat="server" ID="btnDoAdd" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" style="background-color: black;" OnClick="btnDoAdd_Click">
                                                            <span class="fbtn-text fbtn-text-left">Tambah Data</span>
                                                            <i class="fa fa-plus" style="color: white;"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton OnClick="bntLihatData_Click" CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" id="bntLihatData" title=" Lihat Data " style="background-color: black; color: white;">
                                                            <span class="fbtn-text fbtn-text-left">Lihat Data</span>
                                                            <i class="fa fa-eye"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton Visible="false" OnClick="btnBukaSemester_Click" CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" id="btnBukaSemester" title=" Buka Semester " style="background-color: black; color: white;">
                                                            <span class="fbtn-text fbtn-text-left">Buka Semester</span>
                                                            <i class="fa fa-file-text"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>--%>

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

  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">

</script>

    <script type="text/javascript">
        //RenderDropDownOnTables();
        //InitModalFocus();
        //DoAutoSave();
    </script>
</asp:Content>
