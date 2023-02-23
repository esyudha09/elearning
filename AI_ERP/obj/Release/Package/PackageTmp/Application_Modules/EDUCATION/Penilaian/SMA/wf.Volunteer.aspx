<%@ Page Title="" Language="C#" ValidateRequest="false" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.Volunteer.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Penilaian.SMA.wf_Volunteer" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function HideModal(){
            $('#ui_modal_input_data').modal('hide');
            $('#ui_modal_confirm_hapus').modal('hide');
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
                case "<%= JenisAction.Add %>":
                    ReInitTinyMCE();   
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.AddWithMessage %>":
                    HideModal();
                    ReInitTinyMCE();   
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
                    ReInitTinyMCE();   
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmHapus %>":
                    $('#ui_modal_confirm_hapus').modal({ backdrop: 'static', keyboard: false, show: true });
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
            InitPicker();
            InitModalFocus();
            document.getElementById("<%= txtKeyAction.ClientID %>").value = "";            
            
            Sys.Browser.WebKit = {};
            if (navigator.userAgent.indexOf('WebKit/') > -1) {
                Sys.Browser.agent = Sys.Browser.WebKit;
                Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
                Sys.Browser.name = 'WebKit';
            }

            ShowProgress(false);
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

        function ReInitTinyMCE(){
            LoadTinyMCENama();            
        }

        function InitModalFocus(){
            $('#ui_modal_input_data').on('shown.bs.modal', function () {
                tinyMCE.execCommand('mceFocus',false,'<%= txtKegiatan.ClientID %>');
            });
        }

        function ResponseRedirect(url) {
            document.location.href = url;
        }

        function TriggerSave(){
            tinyMCE.triggerSave();
        }

        function InitPicker() {
            $('#<%= txtTanggal.ClientID %>').pickdate({ cancel: '', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });
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

        function ShowProgress(value) {
            if (value) {
                pb_top.style.display = "";
            } else {
                pb_top.style.display = "none";
            }
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
            <asp:HiddenField runat="server" ID="txtIDSiswa" />
            <asp:HiddenField runat="server" ID="txtIndexSiswa" />
            <asp:HiddenField runat="server" ID="txtCountSiswa" />
            <asp:HiddenField runat="server" ID="txtKegiatanVal" />   
            <asp:HiddenField runat="server" ID="txtSemester" />    
            <asp:HiddenField runat="server" ID="txtTahunAjaran" />
            <asp:HiddenField runat="server" ID="txtKelasDet" />     

            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoCari" OnClick="btnDoCari_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDetail" OnClick="btnShowDetail_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowNilaiSiswa" OnClick="btnShowNilaiSiswa_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowConfirmDelete" OnClick="btnShowConfirmDelete_Click" style="position: absolute; left: -1000px; top: -1000px;" />

            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-12">

                    <div class="col-md-8 col-md-offset-2" style="padding: 0px;">
                        <div class="card" style="margin-top: 40px;">
				            <div class="card-main">
					            <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px; ">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="background-color: #44877B; padding: 10px; font-weight: normal; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                                <i class="fa fa-handshake-o" style="font-size: 16pt"></i>
                                                &nbsp;
                                                Volunteer & Kerja Sosial
                                                <asp:Literal runat="server" ID="ltrCaptionHeader"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="background-color: #44877B; padding: 0px;">
                                                <hr style="margin: 0px; border-style: solid; border-width: 1px; border-color: #3b786d;" />
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
								                                    <tr style="background-color: #4B9687;">
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #4B9687; vertical-align: middle; width: 80px;">
                                                                            #
									                                    </th>
                                                                        <th style="text-align: center; background-color: #4B9687; width: 80px; vertical-align: middle;">
                                                                            <i class="fa fa-cog"></i>
                                                                        </th>
                                                                        <th style="background-color: #4B9687; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            <asp:LinkButton ID="H_Kegiatan" runat="server" CommandName="Sort" CommandArgument="Kegiatan" style="color: white; font-weight: bold;">
                                                                                Kegiatan
                                                                            </asp:LinkButton>
                                                                            <asp:Literal runat="server" ID="imgh_kegiatan" Visible="false"></asp:Literal>
									                                    </th>
                                                                        <th style="background-color: #4B9687; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            <asp:LinkButton ID="H_Tanggal" runat="server" CommandName="Sort" CommandArgument="Tanggal" style="color: white; font-weight: bold;">
                                                                                Tanggal
                                                                            </asp:LinkButton>
                                                                            <asp:Literal runat="server" ID="imgh_tanggal" Visible="false"></asp:Literal>
									                                    </th>
                                                                        <th style="background-color: #4B9687; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            <asp:LinkButton ID="H_JumlahJam" runat="server" CommandName="Sort" CommandArgument="JumlahJam" style="color: white; font-weight: bold;">
                                                                                Jumlah Jam
                                                                            </asp:LinkButton>
                                                                            <asp:Literal runat="server" ID="imgh_jumlahjam" Visible="false"></asp:Literal>
									                                    </th>
                                                                        <th style="background-color: #4B9687; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            <asp:LinkButton ID="H_Keterangan" runat="server" CommandName="Sort" CommandArgument="Keterangan" style="color: white; font-weight: bold;">
                                                                                Keterangan
                                                                            </asp:LinkButton>
                                                                            <asp:Literal runat="server" ID="imgh_keterangan" Visible="false"></asp:Literal>
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
												                                <li style="background-color: white; padding: 10px;">
													                                <label 
                                                                                        onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetail.ClientID %>.click(); " 
                                                                                        id="btnDetail" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-info-circle" title=" Lihat Detail "></i>&nbsp;&nbsp;&nbsp;Lihat Detail</label>
												                                </li>
                                                                                <li style="padding: 0px;">
                                                                                    <hr style="margin: 0px; padding: 0px;" />
                                                                                </li>
												                                <li style="background-color: white; padding: 10px;">
													                                <label
                                                                                        onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmDelete.ClientID %>.click(); " 
                                                                                        id="btnHapus" style="color: #9E0000; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-times" title=" Hapus Data "></i>&nbsp;&nbsp;&nbsp;Hapus Data</label>
												                                </li>
											                                </ul>
										                                </li>
									                                </ul>
                                                                </div>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kegiatan").ToString())
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        Convert.ToDateTime(Eval("Tanggal")).ToString("dd/MM/yyyy")
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("JumlahJam").ToString())
                                                                    %>
                                                                </span>
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
								                                    <tr style="background-color: #4B9687;">
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #4B9687; vertical-align: middle; width: 80px;">
                                                                            #
									                                    </th>
                                                                        <th style="text-align: center; background-color: #4B9687; width: 80px; vertical-align: middle;">
                                                                            <i class="fa fa-cog"></i>
                                                                        </th>
                                                                        <th style="background-color: #4B9687; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Kegiatan
									                                    </th>
                                                                        <th style="background-color: #4B9687; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Tanggal
									                                    </th>
                                                                        <th style="background-color: #4B9687; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Jumlah Jam
									                                    </th>
                                                                        <th style="background-color: #4B9687; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Keterangan
									                                    </th>
								                                    </tr>
							                                    </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="6" style="text-align: center; padding: 10px;">
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
                                                style="background-color: #E6E6E6;
                                                        box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
                                                        background-image: none; 
                                                        color: white;
                                                        display: block;
                                                        display: none;
                                                        z-index: 6;
                                                        position: fixed; bottom: 26.5px; right: 25px; width: 270px; border-radius: 25px; border-top-left-radius: 0px;
                                                        padding: 8px; margin: 0px;">
                	
                                                <div style="padding-left: 15px;">
				                                    <asp:DataPager ID="dpData" runat="server" PageSize="1000" PagedControlID="lvData">
                                                        <Fields>
                                                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="btn-trans" ShowFirstPageButton="True" FirstPageText='&nbsp;<i class="fa fa-backward"></i>&nbsp;' ShowPreviousPageButton="True" PreviousPageText='&nbsp;<i class="fa fa-arrow-left"></i>&nbsp;' ShowNextPageButton="false" />
                                                            <asp:TemplatePagerField>
                                                                <PagerTemplate>
                                                                    <label style="color: grey; font-weight: normal; padding: 5px; border-style: solid; border-color: #F1F1F1; border-width: 1px; padding-left: 10px; padding-right: 10px; border-radius: 5px;">
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
			                                        <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #329CC3;" title=" Pilihan ">
                                                        <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                                                        <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                                                    </a>
                                                    <div class="fbtn-dropup" style="z-index: 999999;">
                                                        <asp:LinkButton Visible="false" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkNilaiAkademik" OnClick="lnkNilaiAkademik_Click" style="background-color: #424242;">
                                                            <span class="fbtn-text fbtn-text-left">Nilai Akademik</span>
                                                            <i class="fa fa-th" style="color: white;"></i>
                                                        </asp:LinkButton>
				                                        <asp:LinkButton OnClick="btnRefresh_Click" CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" id="btnRefresh" title=" Refresh " style="background-color: #601B70; color: white;">
                                                            <span class="fbtn-text fbtn-text-left">Refresh Data</span>
                                                            <i class="fa fa-refresh"></i>
                                                        </asp:LinkButton>
                                                        <a onclick="$('#ui_modal_pilihan').modal({ backdrop: 'static', keyboard: false, show: true });"
                                                            class="fbtn fbtn-green waves-attach waves-circle waves-effect" 
                                                            title=" Tampilan Data "
                                                            style="background-color: black;">
                                                            <span class="fbtn-text fbtn-text-left">Tampilan Data</span>
                                                            <i class="fa fa-eye" style="color: white;"></i>
                                                        </a>
                                                        <a data-toggle="modal" href="#ui_modal_pilih_semester" class="fbtn fbtn-green waves-attach waves-circle waves-effect" style="cursor: pointer; background-color: #424242;">
                                                            <span class="fbtn-text fbtn-text-left">Pilih Data Nilai</span>
                                                            <i class="fa fa-list" style="color: white;"></i>
                                                        </a>
                                                        <asp:LinkButton OnClientClick="ShowProgress(true);" ToolTip=" Tambah Data " runat="server" ID="btnDoAdd" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" style="background-color: #257228;" OnClick="btnDoAdd_Click">
                                                            <span class="fbtn-text fbtn-text-left">Tambah Data</span>
                                                            <i class="fa fa-plus" style="color: white;"></i>
                                                        </asp:LinkButton>
			                                        </div>
		                                        </div>
	                                        </div>

                                            <div class="content-header ui-content-header" 
                                                style="background-color: whitesmoke;
                                                        box-shadow: -1px 2px 6px rgba(0,0,0,0.16), 0 -1px 6px rgba(0,0,0,0.23);
                                                        background-image: none; 
                                                        color: white;
                                                        display: block;
                                                        z-index: 5;
                                                        position: fixed; bottom: 50px; right: -25px; width: 320px; border-radius: 5px;
                                                        padding: 12px; 
                                                        padding-top: 0px;
                                                        padding-left: 0px;
                                                        margin: 0px;">

                                                <div style="width: 100%; background-color: whitesmoke; padding: 10px; border-bottom-color: #d3d3d3; border-bottom-style: solid; border-bottom-width: 1px;">
                                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                                    &nbsp;
                                                    <span style="font-weight: bold; color: black;">Volunteer & Kerja Sosial</span>
                                                </div>

                                                <div class="tile-wrap" style="margin-top: 5px; margin-bottom: 0px; padding-left: 10px;">
							                        <div class="tile" style="background: transparent; box-shadow: none;">
								                        <div class="tile-side pull-left">
									                                        
                                                            <img
                                                                src="<%=
                                                                        ResolveUrl(AI_ERP.Application_Libs.Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + "xxx" + ".jpg"))
                                                                        %>"
                                                                style="height: 60px; width: 60px; border-radius: 100%; margin-bottom: 10px;" />

								                        </div>
								                        <div class="tile-inner">

									                        <span style="font-weight: bold; font-size: medium; color: black;">
                                                                <asp:Literal runat="server" ID="lblNamaSiswaInfo"></asp:Literal>
                                                            </span>
                                                            <br />
                                                            <span style="font-weight: normal; font-size: small; color: grey;">
                                                                <asp:Literal runat="server" ID="lblKelasSiswaInfo"></asp:Literal>
                                                            </span>
                                                            <br />
                                                            <span style="font-weight: normal; font-size: x-small; color: grey;">
                                                                <asp:Literal runat="server" ID="lblInfoPeriode"></asp:Literal>
                                                            </span>

								                        </div>
							                        </div>
						                        </div>

                                                <div style="color: yellow; padding-left: 10px;">
                                                    <a class="btn btn-flat waves-attach waves-effect" 
                                                        onclick="FirstSiswa()"
                                                        title=" Data Siswa Pertama "
                                                        style="padding-left: 4px; padding-right: 4px; text-transform: none; font-weight: bold; color: darkblue; border-style: solid; border-width: 0px; border-color: lightskyblue; padding-top: 3px; padding-bottom: 3px; font-size: small; border-radius: 0px;">
                                                        <i class="fa fa-backward"></i>
                                                    </a>
                                                    <a class="btn btn-flat waves-attach waves-effect" 
                                                        onclick="PrevSiswa()"
                                                        title=" Data Siswa Sebelumnya "
                                                        style="padding-left: 4px; padding-right: 4px; text-transform: none; font-weight: bold; color: darkblue; border-style: solid; border-width: 0px; border-color: lightskyblue; padding-top: 3px; padding-bottom: 3px; font-size: small; border-radius: 0px;">
                                                        <i class="fa fa-arrow-left"></i>
                                                    </a>
                                                    <a class="btn btn-flat waves-attach waves-effect" 
                                                        onclick="NextSiswa()"
                                                        title=" Data Siswa Berikutnya "
                                                        style="padding-left: 4px; padding-right: 4px; text-transform: none; font-weight: bold; color: darkblue; border-style: solid; border-width: 0px; border-color: lightskyblue; padding-top: 3px; padding-bottom: 3px; font-size: small; border-radius: 0px;">
                                                        <i class="fa fa-arrow-right"></i>
                                                    </a>
                                                    <a class="btn btn-flat waves-attach waves-effect" 
                                                        onclick="LastSiswa()"
                                                        title=" Data Siswa Terakhir "
                                                        style="padding-left: 4px; padding-right: 4px; text-transform: none; font-weight: bold; color: darkblue; border-style: solid; border-width: 0px; border-color: lightskyblue; padding-top: 3px; padding-bottom: 3px; font-size: small; border-radius: 0px;">
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
                                                        style="text-transform: none; font-weight: bold; color: darkblue; border-style: solid; border-width: 0px; border-color: lightskyblue; padding-top: 3px; padding-bottom: 3px; font-size: small; border-radius: 0px;">
                                                        <i class="fa fa-user"></i>
                                                        &nbsp;Pilih Siswa
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
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 0px;">
                                                        <label class="label-input" for="<%= txtKegiatan.ClientID %>" style="color: black; text-transform: none; margin-bottom: 6px;">
                                                            Kegiatan
                                                        </label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldKegiatan"
                                                            ControlToValidate="txtKegiatan" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox CssClass="mcetiny_nama" runat="server" ID="txtKegiatan" style="height: 60px;"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtTanggal.ClientID %>" style="text-transform: none;">Tanggal</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldTanggal"
                                                            ControlToValidate="txtTanggal" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtTanggal"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-6">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtJumlahJam.ClientID %>" style="text-transform: none;">Jumlah Jam</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldJumlahJam"
                                                            ControlToValidate="txtJumlahJam" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtJumlahJam"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtKeterangan.ClientID %>" style="text-transform: none;">Keterangan</label>
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
                                <asp:LinkButton OnClientClick="TriggerSave()" ValidationGroup="vldInput" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInput" OnClick="lnkOKInput_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" onclick="TriggerSave()" data-dismiss="modal">Batal</a>                                    
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

            <div aria-hidden="true" class="modal fade" id="ui_modal_pilih_semester" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                <asp:Literal runat="server" ID="ltrCaptionPilihan"></asp:Literal>
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
                                                                <div class="tab-pane fade active in" id="ui_tab_akademik">
                                                                    <asp:Literal runat="server" ID="ltrListNilaiAkademik"></asp:Literal>
                                                                </div>
                                                                <div class="tab-pane fade" id="ui_tab_sikap">
                                                                    <asp:Literal runat="server" ID="ltrListSikap"></asp:Literal>
                                                                </div>
                                                                <div class="tab-pane fade" id="ui_tab_ekskul">
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

            <div aria-hidden="true" class="modal fade" id="ui_modal_pilihan" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                        Pilihan
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
                                                        <asp:DropDownList CssClass="form-control" runat="server" ID="cboPeriode">
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
                                <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKPilihan" OnClick="lnkOKPilihan_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkOKPilihan" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
        function LoadTinyMCENama() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.remove();
            tinymce.init({
                selector: ".mcetiny_nama",
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
                height: 50,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtKegiatanVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('keydown',function(e) {
                        if (e.keyCode == 13) {
                            document.getElementById('<%= txtTanggal.ClientID %>').focus();
                            e.preventDefault();
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
        InitPicker();
        InitModalFocus();
    </script>
</asp:Content>
