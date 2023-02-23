<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.JadwalMapel.aspx.cs" Inherits="AI_ERP.Application_Modules.MASTER.wf_MapelJadwal" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </style>
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
                    ShowJenisInput();
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowPengaturan %>":
                    $('#ui_modal_input_pengaturan').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowPengaturanJamJadwal %>":
                    $('#ui_modal_input_jam_jadwal').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.AddWithMessage %>":
                    HideModal();
                    //$('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoShowData %>":
                    ShowJenisInput();
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmHapus %>":
                    $('#ui_modal_confirm_hapus').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowPengaturanMapelJadwal %>":
                    $('#ui_modal_input_mapel_jadwal').modal({ backdrop: 'static', keyboard: false, show: true });
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
            
            InitPicker();
            RenderDropDownOnTables();
            InitModalFocus();
            document.getElementById("<%= txtKeyAction.ClientID %>").value = "";            
            
            Sys.Browser.WebKit = {};
            if (navigator.userAgent.indexOf('WebKit/') > -1) {
                Sys.Browser.agent = Sys.Browser.WebKit;
                Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
                Sys.Browser.name = 'WebKit';
            }

            SetDivDetScroll();
        }

        function InitModalFocus(){
            $('#ui_modal_input_data').on('shown.bs.modal', function () {
                if(document.getElementById("<%= cboJenisPengaturan.ClientID %>") !== undefined && document.getElementById("<%= cboJenisPengaturan.ClientID %>") !== null){
                    document.getElementById("<%= cboJenisPengaturan.ClientID %>").focus();
                    ShowJenisInput();
                }
            });
        }

        function TriggerSave(){
            tinyMCE.triggerSave();
        }

        function DoInitPicker(id)
        {
            $('#' + id).pickdate({ cancel: '', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });
        }

        function InitPicker() {
            DoInitPicker('<%= txtDariTanggal.ClientID %>');
            DoInitPicker('<%= txtSampaiTanggal.ClientID %>');
        }

        function ShowJenisInput() {
            var jenis_input = document.getElementById("<%= cboJenisPengaturan.ClientID %>");
            var copy_dari = document.getElementById("div_copy_dari");
            var buat_baru = document.getElementById("div_buat_baru");
            var dari_tanggal = document.getElementById("div_dari_tanggal");

            if (
                    jenis_input !== null && jenis_input !== undefined &&
                    copy_dari !== null && copy_dari !== undefined &&
                    buat_baru !== null && buat_baru !== undefined &&
                    dari_tanggal !== null && dari_tanggal !== undefined

               ) {
                switch (jenis_input.value) {
                    case "0":
                        copy_dari.style.display = "none";
                        buat_baru.style.display = "";
                        dari_tanggal.style.display = "";
                        break;
                    case "1":
                        copy_dari.style.display = "";
                        buat_baru.style.display = "none";
                        dari_tanggal.style.display = "";
                        break;
                    default:
                        copy_dari.style.display = "none";
                        buat_baru.style.display = "none";
                        dari_tanggal.style.display = "none";
                        break;
                }
            }
        }

        function GetCtlXpos() { return document.getElementById("<%= txtXpos.ClientID %>"); }
        function GetCtlYpos() { return document.getElementById("<%= txtYpos.ClientID %>"); }
        function GetCtlDivDet() { return document.getElementById("div_det"); }

        function GetDivDetScroll() {
            if(GetCtlDivDet() !== null && GetCtlDivDet() !== undefined){
                GetCtlXpos().value = GetCtlDivDet().scrollLeft.toString();
                GetCtlYpos().value = GetCtlDivDet().scrollTop.toString();
            }
        }

        function SetDivDetScroll() {
            if (
                    GetCtlDivDet() !== null && GetCtlDivDet() !== undefined &&
                    GetCtlXpos().value.trim() !== "" && GetCtlYpos().value.trim() !== ""
                ) {
                GetCtlDivDet().scrollLeft = parseInt(GetCtlXpos().value);
                GetCtlDivDet().scrollTop = parseInt(GetCtlYpos().value);
            }
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
            <asp:HiddenField runat="server" ID="txtIDItemPukul" />
            <asp:HiddenField runat="server" ID="txtIDItemMapel" />
            <asp:HiddenField runat="server" ID="txtIDRelKelasDet" />
            <asp:HiddenField runat="server" ID="txtIDItemIsLibur" />
            <asp:HiddenField runat="server" ID="txtXpos" />
            <asp:HiddenField runat="server" ID="txtYpos" />

            <asp:Button runat="server" OnClientClick="GetDivDetScroll();" UseSubmitBehavior="false" ID="btnDoCari" OnClick="btnDoCari_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" OnClientClick="GetDivDetScroll();" UseSubmitBehavior="false" ID="btnShowDetail" OnClick="btnShowDetail_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" OnClientClick="GetDivDetScroll();" UseSubmitBehavior="false" ID="btnShowDetailJadwal" OnClick="btnShowDetailJadwal_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" OnClientClick="GetDivDetScroll();" UseSubmitBehavior="false" ID="btnShowConfirmDelete" OnClick="btnShowConfirmDelete_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" OnClientClick="GetDivDetScroll();" UseSubmitBehavior="false" ID="btnShowPengaturanJamJadwal" OnClick="btnShowPengaturanJamJadwal_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" OnClientClick="GetDivDetScroll();" UseSubmitBehavior="false" ID="btnShowPengaturanMapelJadwal" OnClick="btnShowPengaturanMapelJadwal_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" OnClientClick="GetDivDetScroll();" UseSubmitBehavior="false" ID="btnUpdateIsLibur" OnClick="btnUpdateIsLibur_Click" style="position: absolute; left: -1000px; top: -1000px;" />

            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-12">

                    <div runat="server" id="div_container_main" class="col-md-8 col-md-offset-2" style="padding: 0px;">
                        <div class="card" style="margin-top: 40px;" id="div_card_main" runat="server">
				            <div class="card-main">
					            <div class="card-inner" runat="server" id="div_card_inner" style="margin: 0px; padding: 0px; margin-right: -0.5px; ">
                                    <table id="tbl_caption" runat="server" style="width: 100%;">
                                        <tr>
                                            <td style="background-color: #295BC8; padding: 10px; font-weight: normal; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                                <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/text-lines.svg") %>" 
                                                    style="margin: 0 auto; height: 25px; width: 25px;" />
                                                &nbsp;
                                                Data Jadwal Mata Pelajaran
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
                                                                            Pengaturan Periode
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            &nbsp;
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
                                                                        AI_ERP.Application_Libs.Libs.GetTanggalIndonesiaFromDate(Convert.ToDateTime(Eval("PeriodeDariTanggal")), false)
                                                                    %>
                                                                    &nbsp;-&nbsp;
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetTanggalIndonesiaFromDate(Convert.ToDateTime(Eval("PeriodeSampaiTanggal")), false)
                                                                    %>
                                                                </span>
                                                            </td>
                                                                                                                                                                       
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none; float: left;">
                                                                    <label 
                                                                        onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetailJadwal.ClientID %>.click();"
                                                                        title=" Lihat Data Detail Jadwal "
                                                                        style="float: right; padding: 0px; padding-left: 15px; padding-right: 15px; cursor: pointer; color: cadetblue; border-width: 1px; border-style: solid; border-color: cadetblue; border-radius: 10px; font-size: 9pt;">
                                                                        Detail
                                                                    </label>
                                                                    &nbsp;                                                                 
                                                                </span>     
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
                                                                            Pengaturan Periode
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            &nbsp;
									                                    </th>
								                                    </tr>
							                                    </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="4" style="text-align: center; padding: 10px;">
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
                                                        <asp:LinkButton ToolTip=" Lihat Data " runat="server" ID="btnDoPengaturan" CssClass="fbtn fbtn-blue waves-attach waves-circle waves-effect" style="background-color: #1a1a7e;" OnClick="btnDoPengaturan_Click">
                                                            <span class="fbtn-text fbtn-text-left">Lihat Data</span>
                                                            <i class="fa fa-eye" style="color: white;"></i>
                                                        </asp:LinkButton>
			                                        </div>
		                                        </div>
	                                        </div>

                                        </asp:View>

                                        <asp:View ID="vDetail" runat="server">
                                            <asp:Literal runat="server" ID="ltrDetailJadwal"></asp:Literal>
                                            <div class="fbtn-container" ID="div_view_detail" runat="server">
		                                        <div class="fbtn-inner">
			                                        <asp:LinkButton runat="server" ID="btnKembaliKeList" OnClick="btnKembaliKeList_Click" CssClass="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" style="background-color: #a91212;" title=" Kembali ">
                                                        <i class="fa fa-arrow-left"></i>
                                                    </asp:LinkButton>
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
                                        Buat Pengaturan Jadwal
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-8" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtTahunAjaran.ClientID %>" style="text-transform: none;">Tahun Ajaran</label>
                                                        <asp:TextBox Enabled="true" ValidationGroup="vldInput" CssClass="form-control" runat="server" ID="txtTahunAjaran"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-xs-4" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtSemester.ClientID %>" style="text-transform: none;">Semester</label>
                                                        <asp:TextBox Enabled="true" ValidationGroup="vldInput" CssClass="form-control" runat="server" ID="txtSemester"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>    

                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboJenisPengaturan.ClientID %>" style="text-transform: none;">Jenis Pengaturan</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldJenisPengaturan"
                                                                ControlToValidate="cboJenisPengaturan" Display="Dynamic" style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList runat="server" ID="cboJenisPengaturan" CssClass="form-control" onchange="ShowJenisInput();">
                                                            <asp:ListItem></asp:ListItem>
                                                            <asp:ListItem Value="0" Text="Buat Baru"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Copy Dari Periode"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div id="div_copy_dari">
                                                <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                    <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= cboPeriode.ClientID %>" style="text-transform: none;">Pilih Periode</label>
                                                            <asp:DropDownList runat="server" ID="cboPeriode" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div id="div_dari_tanggal">
                                                <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                    <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= txtDariTanggal.ClientID %>" style="text-transform: none;">Dari Tanggal</label>
                                                            <asp:TextBox ValidationGroup="vldInput" CssClass="form-control" runat="server" ID="txtDariTanggal"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div> 
                                            </div>

                                            <div id="div_buat_baru">
                                                <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                    <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= txtSampaiTanggal.ClientID %>" style="text-transform: none;">Sampai Tanggal</label>
                                                            <asp:TextBox ValidationGroup="vldInput" CssClass="form-control" runat="server" ID="txtSampaiTanggal"></asp:TextBox>
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
                                <asp:LinkButton OnClientClick="TriggerSave()" ValidationGroup="vldInput" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInput" OnClick="lnkOKInput_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" onclick="TriggerSave()" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_pengaturan" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Lihat Data
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div runat="server" id="div1" class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboPeriodeJadwal.ClientID %>" style="text-transform: none;">Pilih Periode</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputPengaturan" runat="server" ID="vldPeriodeJadwal"
                                                                ControlToValidate="cboPeriodeJadwal" Display="Dynamic" style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList ValidationGroup="vldInputPengaturan" runat="server" ID="cboPeriodeJadwal" CssClass="form-control"></asp:DropDownList>
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
                                <asp:LinkButton ValidationGroup="vldInputPengaturan" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKPengaturanJadwal" OnClick="lnkOKPengaturanJadwal_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_jam_jadwal" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Pengaturan Jam
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
                                <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <asp:Literal runat="server" ID="ltrDeskripsiPengaturanJam"></asp:Literal>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-5" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboDariJam_Jam.ClientID %>" style="text-transform: none;">Dari Jam</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputPengaturanJam" runat="server" ID="vldDariJam_Jam"
                                                                ControlToValidate="cboDariJam_Jam" Display="Dynamic" style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList ValidationGroup="vldInputPengaturanJam" runat="server" ID="cboDariJam_Jam" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-xs-2" style="padding-left:  0px; padding-right: 0px; text-align: center; font-weight: bold;">
                                                    <br />&nbsp:&nbsp;
                                                </div>
                                                <div class="col-xs-5" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboDariJam_Menit.ClientID %>" style="text-transform: none;">&nbsp;</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputPengaturanJam" runat="server" ID="vldDariJam_Menit"
                                                                ControlToValidate="cboDariJam_Menit" Display="Dynamic" style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList ValidationGroup="vldInputPengaturanJam" runat="server" ID="cboDariJam_Menit" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>   
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-5" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboSampaiJam_Jam.ClientID %>" style="text-transform: none;">Sampai Jam</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputPengaturanJam" runat="server" ID="vldSampaiJam_Jam"
                                                                ControlToValidate="cboSampaiJam_Jam" Display="Dynamic" style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList ValidationGroup="vldInputPengaturanJam" runat="server" ID="cboSampaiJam_Jam" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-xs-2" style="padding-left:  0px; padding-right: 0px; text-align: center; font-weight: bold;">
                                                    <br />&nbsp:&nbsp;
                                                </div>
                                                <div class="col-xs-5" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboSampaiJam_Menit.ClientID %>" style="text-transform: none;">&nbsp;</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputPengaturanJam" runat="server" ID="vldSampaiJam_Menit"
                                                                ControlToValidate="cboSampaiJam_Menit" Display="Dynamic" style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList ValidationGroup="vldInputPengaturanJam" runat="server" ID="cboSampaiJam_Menit" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>                                        
										</div>  

                                    </div>
                                </div>	
                                
                                <div class="row" style="background-color: white; margin-left: 0px; margin-right: 0px; padding-top: 15px; padding-left: 15px; padding-right: 15px;">
                                    <div class="col-lg-12" style="background-color: white;">
                                        <div style="float: left;">
                                            <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapusPengaturanPukul" OnClick="lnkOKHapusPengaturanPukul_Click" Text="   HAPUS   "></asp:LinkButton>
                                        </div>

					                    <div style="float: right;">
                                            <asp:LinkButton ValidationGroup="vldInputPengaturanJam" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKPengaturanJam" OnClick="lnkOKPengaturanJam_Click" Text="   OK   "></asp:LinkButton>
                                            &nbsp;&nbsp;
                                            <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                            <br /><br />
					                    </div>
                                    </div>
				                </div>							                            
							</div>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_mapel_jadwal" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Pengaturan Mata Pelajaran
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
                                <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <asp:Literal runat="server" ID="ltrDeskripsiPengaturanMapel"></asp:Literal>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboMapelPengaturanMapel.ClientID %>" style="text-transform: none;">Mata Pelajaran</label>
                                                        <asp:DropDownList runat="server" ID="cboMapelPengaturanMapel" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>                                      
										</div>  

                                    </div>
                                </div>	
                                <div class="row" style="background-color: white; margin-left: 0px; margin-right: 0px; padding-top: 15px; padding-left: 15px; padding-right: 15px;">
                                    <div class="col-lg-12" style="background-color: white;">
                                        <div style="float: left;">
                                            <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapusPengaturanMapel" OnClick="lnkOKHapusPengaturanMapel_Click" Text="   HAPUS   "></asp:LinkButton>
                                        </div>
					                    <div style="float: right;">
                                            <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKPengaturanMapel" OnClick="lnkOKPengaturanMapel_Click" Text="   OK   "></asp:LinkButton>
                                            &nbsp;&nbsp;
                                            <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                            <br /><br />
					                    </div>
                                    </div>
                                </div>							                            
							</div>
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

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnRefresh" />
            <asp:PostBackTrigger ControlID="btnDoCari" />            
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
        InitPicker();
        RenderDropDownOnTables();
        InitModalFocus();
    </script>
</asp:Content>
