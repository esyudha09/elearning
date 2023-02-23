<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.PengaturanJadwalKunjunganRutin.aspx.cs" Inherits="AI_ERP.Application_Modules.MASTER.Perpustakaan.wf_PengaturanJadwalKunjunganRutin" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<%@ Register TagPrefix="ucl" TagName="AutocompletePegawai" Src="~/Application_Controls/AutocompletePegawai/AutocompletePegawai.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function HideModal(){
            $('#ui_modal_input_data').modal('hide');
            $('#ui_modal_confirm_hapus').modal('hide');    
            $('#ui_modal_input_pengaturan_jadwal').modal('hide');    
            $('#ui_modal_hapus_item_mengajar').modal('hide');    

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
                case "<%= JenisAction.DoShowConfirmHapus %>":
                    $('#ui_modal_confirm_hapus').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmHapusJadwal %>":
                    $('#ui_modal_hapus_item_mengajar').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowInputJadwal %>":
                    $('#ui_modal_input_pengaturan_jadwal').modal({ backdrop: 'static', keyboard: false, show: true });                    
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
            $('#ui_modal_input_pengaturan_jadwal').on('shown.bs.modal', function () {
                <%= cboJadwalHari.ClientID %>.focus();
            });
        }
                
        function stopRKey(evt) {
            var evt = (evt) ? evt : ((event) ? event : null);
            var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
            if ((evt.keyCode == 13) && (node.type == "text")) { return false; }
        }
        document.onkeypress = stopRKey;
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
            <asp:HiddenField runat="server" ID="txtParseKelasUnit" />
            <asp:HiddenField runat="server" ID="txtParseMapelUnit" />
            <asp:HiddenField runat="server" ID="txtKelasUnit" />
            <asp:HiddenField runat="server" ID="txtMapelUnit" />
            
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoCari" OnClick="btnDoCari_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDetail" OnClick="btnShowDetail_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDetailItem" OnClick="btnShowDetailItem_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowPengaturan" OnClick="btnShowPengaturan_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowInputJadwal" OnClick="btnShowInputJadwal_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowConfirmDelete" OnClick="btnShowConfirmDelete_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowConfirmDeleteItem" OnClick="btnShowConfirmDeleteItem_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            
            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-12">

                    <div class="col-md-8 col-md-offset-2" style="padding: 0px;">
                        <div class="card" style="margin-top: 40px;">
				            <div class="card-main">
					            <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px; ">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="background-color: #295BC8; padding: 10px; font-weight: normal; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                                <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/093-calendar-1.svg") %>" 
                                                    style="margin: 0 auto; height: 25px; width: 25px;" />
                                                &nbsp;
                                                Pengaturan Kunjungan Rutin
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
                                                                        <th style="text-align: center; background-color: #3367d6; width: 80px; vertical-align: middle;">
                                                                            <i class="fa fa-cog"></i>
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            <asp:LinkButton ID="H_TahunAjaran" runat="server" CommandName="Sort" CommandArgument="TahunAjaran" style="color: white; font-weight: bold;">
                                                                                Tahun Pelajaran
                                                                            </asp:LinkButton>
                                                                            <asp:Literal runat="server" ID="imgh_tahunajaran" Visible="false"></asp:Literal>
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: center; padding-left: 10px; vertical-align: middle;">
                                                                            <asp:LinkButton ID="H_Semester1" runat="server" CommandName="Sort" CommandArgument="IsSemester_1" style="color: white; font-weight: bold;">
                                                                                Semester 1
                                                                            </asp:LinkButton>
                                                                            <asp:Literal runat="server" ID="imgh_semester1" Visible="false"></asp:Literal>
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: center; padding-left: 10px; vertical-align: middle;">
                                                                            <asp:LinkButton ID="H_Semester2" runat="server" CommandName="Sort" CommandArgument="IsSemester_2" style="color: white; font-weight: bold;">
                                                                                Semester 2
                                                                            </asp:LinkButton>
                                                                            <asp:Literal runat="server" ID="imgh_semester2" Visible="false"></asp:Literal>
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            <asp:LinkButton ID="H_Sekolah" runat="server" CommandName="Sort" CommandArgument="Sekolah" style="color: white; font-weight: bold;">
                                                                                Unit Sekolah
                                                                            </asp:LinkButton>
                                                                            <asp:Literal runat="server" ID="imgh_sekolah" Visible="false"></asp:Literal>
									                                    </th>                                                                        
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            <asp:LinkButton ID="H_Keterangan" runat="server" CommandName="Sort" CommandArgument="Keterangan" style="color: white; font-weight: bold;">
                                                                                Keterangan
                                                                            </asp:LinkButton>
                                                                            <asp:Literal runat="server" ID="imgh_keterangan" Visible="false"></asp:Literal>
									                                    </th>                                                                        
                                                                        <th style="background-color: #3367d6; text-align: right; padding-left: 10px; vertical-align: middle;">
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
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("TahunAjaran").ToString())
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center;">
                                                                <%# 
                                                                    Convert.ToBoolean(Eval("IsSemester_1"))
                                                                    ? "<i class=\"fa fa-check-circle\" style=\"color: green\"></i>"
                                                                    : "<i class=\"fa fa-check-circle\" style=\"color: #bfbfbf\"></i>"
                                                                %>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center;">
                                                                <%# 
                                                                    Convert.ToBoolean(Eval("IsSemester_2"))
                                                                    ? "<i class=\"fa fa-check-circle\" style=\"color: green\"></i>"
                                                                    : "<i class=\"fa fa-check-circle\" style=\"color: #bfbfbf\"></i>"
                                                                %>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Unit").ToString())
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
                                                            <td style="text-align: right;">
                                                                <label id="img_<%# Eval("Kode").ToString().Replace("-", "_") %>" style="display: none; font-size: small; color: grey; font-weight: bold;">
                                                                    <img src="../../../../Application_CLibs/images/giphy.gif" style="height: 16px; width: 20px;" />
                                                                    &nbsp;&nbsp;Proses...
                                                                </label>
                                                                <label id="lbl_<%# Eval("Kode").ToString().Replace("-", "_") %>" onclick="this.style.display = 'none'; img_<%# Eval("Kode").ToString().Replace("-", "_") %>.style.display = ''; setTimeout(function() { <%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>';<%= btnShowPengaturan.ClientID %>.click(); }, 1000);" class="badge" style="cursor: pointer; font-weight: normal; font-size: x-small;">
                                                                    &nbsp;
                                                                    <i class="fa fa-edit"></i>
                                                                    &nbsp;
                                                                    Pengaturan
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
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle; width: 80px;">
                                                                            #
									                                    </th>
                                                                        <th style="text-align: center; background-color: #3367d6; width: 80px; vertical-align: middle;">
                                                                            <i class="fa fa-cog"></i>
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Tahun Pelajaran
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Semester
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Unit Sekolah
									                                    </th>                                                                        
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
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
                                        <asp:View ID="vListGuru" runat="server">

                                            <div style="padding: 0px; margin: 0px;">
                                                <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                    <tr style="background-color: #3367d6;">
                                                        <td style="text-align: left; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle;">
                                                            <asp:Literal runat="server" ID="ltrCaptionFormasi"></asp:Literal>
									                    </td>
                                                    </tr>
                                                </table>
                                                <asp:ListView ID="lvListPengaturan" DataSourceID="sql_ds_pengaturan" runat="server" OnSorting="lvListPengaturan_Sorting" OnPagePropertiesChanging="lvListPengaturan_PagePropertiesChanging">
                                                    <LayoutTemplate>
                                                        <div class="table-responsive" style="margin: 0px; box-shadow: none;">
							                                <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
								                                <thead>
								                                    <tr style="background-color: #3c70e1;">
                                                                        <th style="text-align: center; background-color: #3c70e1; width: 80px; vertical-align: middle;">
                                                                            <i class="fa fa-cog"></i>
                                                                        </th>
                                                                        <th style="background-color: #3c70e1; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Hari
									                                    </th>
                                                                        <th style="background-color: #3c70e1; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Waktu
									                                    </th>
                                                                        <th style="background-color: #3c70e1; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Kelas
									                                    </th>
                                                                        <th style="background-color: #3c70e1; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Guru
									                                    </th>
                                                                        <th style="background-color: #3c70e1; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Keterangan
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
                                                            <td style="background-color:<%# AI_ERP.Application_Libs.Kalender.Arr_WarnaHeader[Convert.ToInt16(Eval("Hari").ToString())] %>; padding: 0px; text-align: center; width: 80px; vertical-align: middle;">
                                                                <div style="margin: 0 auto; display: table;">
                                                                    <ul class="nav nav-list margin-no pull-left">
										                                <li class="dropdown">
                                                                            <a class="dropdown-toggle text-black waves-attach waves-effect" data-toggle="dropdown" style="color: grey; line-height: 15px; padding: 10px; margin: 0px; min-height: 15px; z-index: 0; color: white;" title=" Menu ">
                                                                                <i class="fa fa-cog"></i>
                                                                                <i class="fa fa-caret-down" style="font-size: xx-small;"></i>
                                                                            </a>
											                                <ul class="dropdown-menu-list-table">
												                                <li style="background-color: white; padding: 10px;">
													                                <label 
                                                                                        onclick="<%= txtIDItem.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetailItem.ClientID %>.click(); " 
                                                                                        id="btnDetail" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-info-circle" title=" Lihat Detail "></i>&nbsp;&nbsp;&nbsp;Lihat Detail</label>
												                                </li>
                                                                                <li style="padding: 0px;">
                                                                                    <hr style="margin: 0px; padding: 0px;" />
                                                                                </li>
												                                <li style="background-color: white; padding: 10px;">
													                                <label
                                                                                        onclick="<%= txtIDItem.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmDeleteItem.ClientID %>.click(); " 
                                                                                        id="btnHapus" style="color: #9E0000; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-times" title=" Hapus Data "></i>&nbsp;&nbsp;&nbsp;Hapus Data</label>
												                                </li>
											                                </ul>
										                                </li>
									                                </ul>
                                                                </div>
                                                            </td>
                                                            <td style="background-color:<%# AI_ERP.Application_Libs.Kalender.Arr_WarnaHeader[Convert.ToInt16(Eval("Hari").ToString())] %>; font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: white; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetNamaHariFromUrutHari(Eval("Hari").ToString())
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Waktu").ToString())
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
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Modules.MASTER.Perpustakaan.wf_PengaturanJadwalKunjunganRutin.GetGuruFromKode(Eval("Kode").ToString())
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
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
                                                                        <th style="text-align: center; background-color: #3c70e1; width: 80px; vertical-align: middle;">
                                                                            <i class="fa fa-cog"></i>
                                                                        </th>
                                                                        <th style="background-color: #3c70e1; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Hari
									                                    </th>
                                                                        <th style="background-color: #3c70e1; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Waktu
									                                    </th>
                                                                        <th style="background-color: #3c70e1; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Kelas
									                                    </th>
                                                                        <th style="background-color: #3c70e1; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Guru
									                                    </th>
                                                                        <th style="background-color: #3c70e1; text-align: left; padding-left: 10px; vertical-align: middle;">
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
                                            <asp:SqlDataSource ID="sql_ds_pengaturan" runat="server"></asp:SqlDataSource>

                                            <div class="content-header ui-content-header" 
						                        style="background-color: #00198d;
								                        box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
								                        background-image: none; 
								                        color: white;
								                        display: block;
								                        z-index: 5;
								                        position: fixed; bottom: 33px; right: 50px; width: 70px; border-radius: 25px;
								                        padding: 8px; margin: 0px; height: 35px;">
						                        <div style="padding-left: 0px;">
							                        <asp:LinkButton ToolTip=" Kembali " runat="server" ID="btnShowDataList" CssClass="btn-trans waves-attach waves-circle waves-effect" OnClick="btnShowDataList_Click" style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;
                                                    </asp:LinkButton>
						                        </div>
					                        </div>

                                            <div class="fbtn-container" id="div_button_settings_formasi_guru" runat="server">
		                                        <div class="fbtn-inner">
			                                        <a onclick="setTimeout(
                                                            function(){
                                                                <%= btnShowInputJadwal.ClientID %>.click();
                                                            }, 500
                                                        ); return false;" class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #a91212;" title=" Tambah Data ">
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

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_pengaturan_jadwal" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Isi Jadwal Rutin
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
                                                        <label class="label-input" for="<%= cboJadwalHari.ClientID %>" style="text-transform: none;">Hari</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputJadwal" runat="server" ID="vldJadwalHari"
                                                            ControlToValidate="cboJadwalHari" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList CssClass="form-control" runat="server" ID="cboJadwalHari" ValidationGroup="vldInputJadwal">
                                                            <asp:ListItem></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtJamJadwalHari.ClientID %>" style="text-transform: none;">Jam</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputJadwal" runat="server" ID="vldJamJadwalHari"
                                                            ControlToValidate="cboKelas" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ValidationGroup="vldInputJadwal" CssClass="form-control" runat="server" ID="txtJamJadwalHari"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboKelas.ClientID %>" style="text-transform: none;">Kelas</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputJadwal" runat="server" ID="vldKelasJadwalHari"
                                                            ControlToValidate="cboKelas" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList CssClass="form-control" runat="server" ID="cboKelas" ValidationGroup="vldInputJadwal">
                                                            <asp:ListItem></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>                                            
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtKeteranganJadwalHari.ClientID %>" style="text-transform: none;">Keterangan</label>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtKeteranganJadwalHari" ValidationGroup="vldInputJadwal"></asp:TextBox>
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
                                <asp:LinkButton ValidationGroup="vldInputJadwal" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKJadwalRutin" OnClick="lnkOKJadwalRutin_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>
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
                                                        <label class="label-input" for="<%= txtTahunPelajaran.ClientID %>" style="text-transform: none;">Tahun Pelajaran</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldTahunPelajaran"
                                                            ControlToValidate="txtTahunPelajaran" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ValidationGroup="vldInput" CssClass="form-control" runat="server" ID="txtTahunPelajaran"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboUnitSekolah.ClientID %>" style="text-transform: none;">Unit Sekolah</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldUnitSekolah"
                                                            ControlToValidate="cboUnitSekolah" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList runat="server" ID="cboUnitSekolah" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>                                           
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-6" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 5px;">
														<div class="checkbox switch">
															<label for="<%= chkSemester1.ClientID %>" style="font-weight: bold; color: white;">
																<input type="checkbox" runat="server" id="chkSemester1" class="access-hide" />
																<span class="switch-toggle"></span>                                                                
															</label>
                                                            Semester 1
														</div>
													</div>  
                                                </div>
                                                <div class="col-xs-6" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 5px;">
														<div class="checkbox switch">
															<label for="<%= chkSemester2.ClientID %>" style="font-weight: bold; color: white;">
																<input type="checkbox" runat="server" id="chkSemester2" class="access-hide" />
																<span class="switch-toggle"></span>                                                                
															</label>
                                                            Semester 2
														</div>
													</div>  
                                                </div>
                                            </div>                                           
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
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

            <div aria-hidden="true" class="modal fade" id="ui_modal_hapus_item_mengajar" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Hapus Data
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapusItemJadwal" OnClick="lnkOKHapusItemJadwal_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
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
        RenderDropDownOnTables();
        InitModalFocus();
    </script>
</asp:Content>
