<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.LinkPembelajaranEksternal.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Elearning.wf_LinkPembelajaranEksternal" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function HideModal(){
            $('#ui_modal_input_data').modal('hide');
            $('#ui_modal_confirm_hapus').modal('hide');            

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
                case "<%= JenisAction.DoChangePage %>":
                    window.scrollTo(0,0); 
                    break;
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
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowFilter %>":
                    $('#ui_modal_input_data_filter').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmHapus %>":
                    $('#ui_modal_confirm_hapus').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowDownloadHistLinkPembelajaran %>":
                    $('#ui_modal_hist_link_pembelajaran_eksternal').modal({ backdrop: 'static', keyboard: false, show: true });
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
            $('#ui_modal_input_data').on('shown.bs.modal', function () {
                if(document.getElementById("<%= txtLinkTautan.ClientID %>") !== undefined && document.getElementById("<%= txtLinkTautan.ClientID %>") !== null){
                    document.getElementById("<%= txtLinkTautan.ClientID %>").focus();
                }
                else {
                    document.getElementById("<%= txtLinkTautan.ClientID %>").focus();
                }
            });

            $('#ui_modal_input_data_filter').on('shown.bs.modal', function () {
                if(document.getElementById("<%= txtLinkTautan_FilterCredit_Filter.ClientID %>") !== undefined && document.getElementById("<%= txtLinkTautan_FilterCredit_Filter.ClientID %>") !== null){
                    document.getElementById("<%= txtLinkTautan_FilterCredit_Filter.ClientID %>").focus();
                }
                else {
                    document.getElementById("<%= txtLinkTautan_FilterCredit_Filter.ClientID %>").focus();
                }
            });
        }

        function TriggerSave(){
            tinyMCE.triggerSave();
        }

        function CekKategori() {
            var div = document.getElementById("div_kategori");
            if (div !== null && div !== undefined) {
                var cbo = document.getElementById("<%= cboKategori.ClientID %>");
                if (cbo !== null && cbo !== undefined) {
                    div.style.display = (cbo.value.toUpperCase() === "LAINNYA" ? "" : "none");
                    if (cbo.value.toUpperCase() === "LAINNYA") {
                        document.getElementById("<%= txtKategori.ClientID %>").focus();
                    }
                }
            }
        }

        function UnCheckAllUnit(value) {
            var arr = document.getElementsByName("arr_unit[]");
            if (arr.length > 0 && value === true) {
                for (var i = 0; i < arr.length; i++) {
                    arr[i].checked = false;
                }
            }
        }

        function UnCheckSemua(value) {
            var chk = document.getElementById("chk_semua");
            if (chk !== null && chk !== undefined && value === true) {
                chk.checked = false;
            }
        }

        function ParseCheckedUnit() {
            var txtunit = document.getElementById("<%= txtUnit.ClientID %>");
            if (txtunit !== null && txtunit !== undefined) {
                var chk = document.getElementById("chk_semua");
                if (chk !== null && chk !== undefined) {
                    if (chk.checked === true) {
                        txtunit.value = "Semua";
                        return;
                    }
                }

                txtunit.value = "";
                var arr = document.getElementsByName("arr_unit[]");
                if (arr.length > 0) {
                    for (var i = 0; i < arr.length; i++) {
                        if (arr[i].checked === true) {
                            txtunit.value += (txtunit.value.trim() !== "" ? "," : "") +
                                             arr[i].value;
                        }
                    }
                }
            }
        }

        function UnCheckAllUnit_Unit(value) {
            var arr = document.getElementsByName("arr_unit_filter[]");
            if (arr.length > 0 && value === true) {
                for (var i = 0; i < arr.length; i++) {
                    arr[i].checked = false;
                }
            }
        }

        function UnCheckSemua_Unit(value) {
            var chk = document.getElementById("chk_semua_filter");
            if (chk !== null && chk !== undefined && value === true) {
                chk.checked = false;
            }
        }

        function ParseCheckedUnit_Filter() {
            var txtunit = document.getElementById("<%= txtUnit_Filter.ClientID %>");
            if (txtunit !== null && txtunit !== undefined) {
                var chk = document.getElementById("chk_semua_filter");
                if (chk !== null && chk !== undefined) {
                    if (chk.checked === true) {
                        txtunit.value = " a.UNIT LIKE '%Semua%' ";
                        return;
                    }
                }

                txtunit.value = "";
                var arr = document.getElementsByName("arr_unit_filter[]");
                if (arr.length > 0) {
                    for (var i = 0; i < arr.length; i++) {
                        if (arr[i].checked === true) {
                            txtunit.value += (txtunit.value.trim() !== "" ? " OR " : "") +
                                             " a.UNIT LIKE '%" + arr[i].value.replaceAll("'", "''") + "%' ";
                        }
                    }
                }
            }
        }

        function InitPicker() {
            $('#<%= txtTanggalMulai_HistLinkPembelajaran.ClientID %>').pickdate({ cancel: '', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });
            $('#<%= txtTanggalAkhir_HistLinkPembelajaran.ClientID %>').pickdate({ cancel: '', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });
        }

        function ShowProsesLaporanHistLinkPembelajaran(show) {
            pb_proses_download_hist_link_pembelajaran.style.display = (show ? "" : "none");
            div_button_download_hist_link_pembelajaran.style.display = (show ? "none" : "");
            if (!show) {
                if (navigator.appName == 'Microsoft Internet Explorer') {
                    window.frames['fra_download'].document.execCommand('Stop');
                } else {
                    window.frames['fra_download'].stop();
                }
                HideModal();
            }
        }

        function ReportProcessHistLinkPembelajaran() {
            var url = "<%= ResolveUrl("~/Application_Resources/Download.aspx") %>";
            url += "?<%= AI_ERP.Application_Libs.Downloads.JENIS_DOWNLOAD_KEY %>=<%= AI_ERP.Application_Libs.Downloads.JenisDownload.HISTORY_LINK_PEMBELAJARAN_EKSTERNAL %>";
            url += "&tgl1=" + txt_tanggal_absen1().value;
            url += "&tgl2=" + txt_tanggal_absen2().value;
            url += "&p=<%= AI_ERP.Application_Libs.Libs.LOGGED_USER_M.NoInduk %>";

            if (navigator.appName == 'Microsoft Internet Explorer') {
                window.frames['fra_download'].document.location.href = url;
            } else {
                window.frames['fra_download'].location.href = url;
            }
        }

        var txt_tanggal_absen1 = function () { return document.getElementById('<%= txtTanggalMulai_HistLinkPembelajaran.ClientID %>'); }
        var txt_tanggal_absen2 = function () { return document.getElementById('<%= txtTanggalAkhir_HistLinkPembelajaran.ClientID %>'); }
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
            <asp:HiddenField runat="server" ID="txtUnit" />
            <asp:HiddenField runat="server" ID="txtUnit_Filter" />
            <asp:HiddenField runat="server" ID="txtUsingFilter" />

            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnDoCari" OnClick="btnDoCari_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnShowDetail" OnClick="btnShowDetail_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnShowConfirmDelete" OnClick="btnShowConfirmDelete_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnDoRefresh" OnClick="btnDoRefresh_Click" style="position: absolute; left: -1000px; top: -1000px;" />

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
                                                <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/science-book.svg") %>" 
                                                    style="margin: 0 auto; height: 25px; width: 25px;" />
                                                &nbsp;
                                                Link Pembelajaran Eksternal
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
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle; width: 80px;">
                                                                            #
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: center; padding-left: 10px; vertical-align: middle;">
                                                                            Aksi
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            <asp:LinkButton ID="H_Kategori" runat="server" CommandName="Sort" CommandArgument="Kategori" style="color: white; font-weight: bold;">
                                                                                Kategori
                                                                            </asp:LinkButton>&nbsp;
                                                                            <asp:Literal runat="server" ID="imgh_kategori" Visible="false"></asp:Literal>
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Level
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            <asp:LinkButton ID="H_Nama" runat="server" CommandName="Sort" CommandArgument="Nama" style="color: white; font-weight: bold;">
                                                                                Nama/Tentang
                                                                            </asp:LinkButton>&nbsp;
                                                                            <asp:Literal runat="server" ID="imgh_nama" Visible="false"></asp:Literal>
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: center; padding-left: 10px; vertical-align: middle;">
                                                                            <asp:LinkButton ID="H_JumlahAksesSemua" runat="server" CommandName="Sort" CommandArgument="JumlahAksesSemua" style="color: white; font-weight: bold;">
                                                                                Semua
                                                                            </asp:LinkButton>&nbsp;
                                                                            <asp:Literal runat="server" ID="imgh_JumlahAksesSemua" Visible="false"></asp:Literal>
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: center; padding-left: 10px; vertical-align: middle;">
                                                                            <asp:LinkButton ID="H_JumlahAksesGuruYbs" runat="server" CommandName="Sort" CommandArgument="JumlahAksesGuruYbs" style="color: white; font-weight: bold;">
                                                                                Saya
                                                                            </asp:LinkButton>&nbsp;
                                                                            <asp:Literal runat="server" ID="imgh_JumlahAksesGuruYbs" Visible="false"></asp:Literal>
									                                    </th>
                                                                        <th style="text-align: center; background-color: #3367d6; width: 80px; vertical-align: middle;">
                                                                            <i class="fa fa-cog"></i>
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
                                                            <td style="text-align: center; padding: 10px; vertical-align: middle; color: #bfbfbf;">
                                                                <%# (int)(this.Session[SessionViewDataName] == null ? 0 : this.Session[SessionViewDataName]) + (Container.DisplayIndex + 1) %>.
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center;">
                                                                <button onclick="window.open('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LINK_OPENER.ROUTE) %>?j=<%= AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LINK_OPENER.JENIS_LO_LINK_PEMBELAJARAN_EKSTERNAL %>&id=<%# Eval("Kode").ToString() %>', '_blank'); setTimeout(function(){ document.getElementById('<%= btnDoRefresh.ClientID %>').click(); }, 2000);" class="btn btn-flat btn-brand-accent waves-attach waves-effect" style="text-transform: none; background-color: #3db9f4; color: white; padding-top: 3px; padding-bottom: 3px; padding-left: 10px; padding-right: 10px;">
                                                                    <span style="color: white;"><i class="fa fa-external-link"></i>&nbsp;&nbsp;Buka</span>
                                                                </button>
                                                            </td>    
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none; color: #1DA1F2;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kategori").ToString())
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Unit").ToString()).Replace(",", ", ")
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Nama").ToString())
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        "<span style=\"font-weight: bold;\">" +
                                                                            AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("JumlahAksesSemua").ToString()) +
                                                                        "</span>"
                                                                    %>x
                                                                    dibuka semua
                                                                </span>
                                                            </td><td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        "<span style=\"font-weight: bold;\">" +
                                                                            AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("JumlahAksesGuruYbs").ToString()) +
                                                                        "</span>"
                                                                    %>x
                                                                    dibuka saya
                                                                </span>
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
												                                <li style="background-color: white; padding: 10px;">
													                                <label 
                                                                                        onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetail.ClientID %>.click(); " 
                                                                                        id="btnDetail" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-info-circle" title=" Lihat Detail "></i>&nbsp;&nbsp;&nbsp;Lihat Detail</label>
												                                </li>
                                                                                <li style="padding: 0px; display: none;">
                                                                                    <hr style="margin: 0px; padding: 0px;" />
                                                                                </li>
												                                <li style="background-color: white; padding: 10px; display: none;">
													                                <label
                                                                                        onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmDelete.ClientID %>.click(); " 
                                                                                        id="btnHapus" style="color: #9E0000; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-times" title=" Hapus Data "></i>&nbsp;&nbsp;&nbsp;Hapus Data</label>
												                                </li>
											                                </ul>
										                                </li>
									                                </ul>
                                                                </div>
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
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Aksi
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Kategori 
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Level
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Nama/Tentang
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Semua
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Saya
									                                    </th>
                                                                        <th style="text-align: center; background-color: #3367d6; width: 80px; vertical-align: middle;">
                                                                            <i class="fa fa-cog"></i>
                                                                        </th>
								                                    </tr>
							                                    </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="8" style="text-align: center; padding: 10px;">
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
                                                        <asp:LinkButton ToolTip=" Filter Pencarian " runat="server" ID="btnDoShowDownloadHist" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" style="background-color: #257228;" OnClick="btnDoShowDownloadHist_Click">
                                                            <span class="fbtn-text fbtn-text-left">Download History Saya</span>
                                                            <i class="fa fa-file-excel-o" style="color: white;"></i>
                                                        </asp:LinkButton>
				                                        <asp:LinkButton OnClick="btnRefresh_Click" CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" id="btnRefresh" title=" Refresh " style="background-color: #601B70; color: white;">
                                                            <span class="fbtn-text fbtn-text-left">Refresh Data</span>
                                                            <i class="fa fa-refresh"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ToolTip=" Filter Pencarian " runat="server" ID="btnDoFilter" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" style="background-color: #006aac;" OnClick="btnDoFilter_Click">
                                                            <span class="fbtn-text fbtn-text-left">Filter Pencarian</span>
                                                            <i class="fa fa-search" style="color: white;"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ToolTip=" Tambah Link " runat="server" ID="btnDoAdd" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" style="background-color: #7a00ff;" OnClick="btnDoAdd_Click">
                                                            <span class="fbtn-text fbtn-text-left">Tambah Link</span>
                                                            <i class="fa fa-plus" style="color: white;"></i>
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
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtLinkTautanCredit.ClientID %>" style="text-transform: none;">Link/Tautan <span style="font-style: italic">Credit</span></label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldNama"
                                                            ControlToValidate="txtLinkTautanCredit" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox Enabled="false" ValidationGroup="vldInput" placeholder="Masukan Tautan Credit..." CssClass="form-control" runat="server" ID="txtLinkTautanCredit"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtLinkTautan.ClientID %>" style="text-transform: none;">Link/Tautan</label>
                                                        <asp:TextBox ValidationGroup="vldInput" placeholder="Masukan Link/Tautan..." CssClass="form-control" runat="server" ID="txtLinkTautan"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>                                            
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtNamaTentang.ClientID %>" style="text-transform: none;">Nama/Tentang</label>
                                                        <asp:TextBox ValidationGroup="vldInput" placeholder="Masukan Nama/Tentang..." CssClass="form-control" runat="server" ID="txtNamaTentang"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>   
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboKategori.ClientID %>" style="text-transform: none;">Kategori</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldKategori"
                                                                ControlToValidate="cboKategori" Display="Dynamic" style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList runat="server" ID="cboKategori" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>                                              
                                            <div id="div_kategori" class="row" style="margin-left: 30px; margin-right: 30px; display: none;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtKategori.ClientID %>" style="text-transform: none;">Kategori Lainnya</label>
                                                        <asp:TextBox ValidationGroup="vldInput" placeholder="Isi Kategori..." CssClass="form-control" runat="server" ID="txtKategori"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>  
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" style="text-transform: none;">Level/Jenjang</label><br />
                                                        <asp:Literal runat="server" ID="ltrLevelJenjang"></asp:Literal>
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
                                <asp:LinkButton OnClientClick="ParseCheckedUnit(); TriggerSave();" ValidationGroup="vldInput" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInput" OnClick="lnkOKInput_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" onclick="TriggerSave()" data-dismiss="modal">Tutup</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_data_filter" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Filter Pencarian
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtLinkTautan_FilterCredit_Filter.ClientID %>" style="text-transform: none;">Link/Tautan <span style="font-style: italic">Credit</span></label>
                                                        <asp:TextBox placeholder="Masukan Tautan Credit..." CssClass="form-control" runat="server" ID="txtLinkTautan_FilterCredit_Filter"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtLinkTautan_Filter.ClientID %>" style="text-transform: none;">Link/Tautan</label>
                                                        <asp:TextBox placeholder="Masukan Link/Tautan..." CssClass="form-control" runat="server" ID="txtLinkTautan_Filter"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>                                            
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtNamaTentang_Filter.ClientID %>" style="text-transform: none;">Nama/Tentang</label>
                                                        <asp:TextBox placeholder="Masukan Nama/Tentang..." CssClass="form-control" runat="server" ID="txtNamaTentang_Filter"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>   
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboKategori_Filter.ClientID %>" style="text-transform: none;">Kategori</label>
                                                        <asp:DropDownList runat="server" ID="cboKategori_Filter" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" style="text-transform: none;">Level/Jenjang</label><br />
                                                        <asp:Literal runat="server" ID="ltrLevelJenjang_Filter"></asp:Literal>
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
                                <asp:LinkButton OnClientClick="ParseCheckedUnit_Filter(); TriggerSave();" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInput_Filter" OnClick="lnkOKInput_Filter_Click" Text="   Filter   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" onclick="TriggerSave()" data-dismiss="modal">Tutup</a>                                    
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

            <div aria-hidden="true" class="modal fade" id="ui_modal_hist_link_pembelajaran_eksternal" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                        Download History Link Pembelajaran
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtTanggalMulai_HistLinkPembelajaran.ClientID %>" style="text-transform: none;">Tanggal Awal</label>
                                                        <asp:TextBox ValidationGroup="vldDownloadistLinkPembelajaran" CssClass="form-control" runat="server" ID="txtTanggalMulai_HistLinkPembelajaran"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>     
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtTanggalAkhir_HistLinkPembelajaran.ClientID %>" style="text-transform: none;">Tanggal Akhir</label>
                                                        <asp:TextBox ValidationGroup="vldDownloadistLinkPembelajaran" CssClass="form-control" runat="server" ID="txtTanggalAkhir_HistLinkPembelajaran"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>     
										</div>  

                                    </div>
                                </div>								                            
							</div>
				        </div>
				        <div class="modal-footer">
					        
                            <div style="width: 100%;">
							    <div class="row" id="pb_proses_download_hist_link_pembelajaran" style="display: none; margin-left: -24px; margin-right: -24px; background-color: #B50000; color: white;">
                                    <div class="col-lg-12" style="padding-left: 0px; padding-right: 0px;">
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
                                </div>
                                <div class="row" id="div_button_download_hist_link_pembelajaran">
                                    <div class="col-xs-12" style="padding: 15px; font-weight: bold; padding-left: 45px; padding-right: 45px;">
                                        <p class="text-right">
                                            <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" onclick="ShowProsesLaporanHistLinkPembelajaran(true); ReportProcessHistLinkPembelajaran(); return false;">OK</a>
                                            <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Tutup</a>
					                    </p>
                                    </div>
                                </div>                                
                            </div>

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

    <iframe name="fra_download" onloadedmetadata="alert('ok')" id="fra_download" height="0" width="0" style="position: absolute; left: -1000px; top: -1000px;"></iframe>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
        RenderDropDownOnTables();
        InitPicker();
        InitModalFocus();
    </script>
</asp:Content>

