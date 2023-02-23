<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.Kelas.aspx.cs" Inherits="AI_ERP.Application_Modules.MASTER.wf_Kelas" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function txtID(){ return document.getElementById("<%= txtID.ClientID %>"); }
        function HideModal(){
            $('#ui_modal_input_data').modal('hide');
            $('#ui_modal_confirm_hapus_item_kelas').modal('hide');
            $('#ui_modal_confirm_hapus_kelas').modal('hide');

            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();    
            
            document.body.style.paddingRight = "0px";
        }
        function EndRequestHandler() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        }
        function DivInput(){ return document.getElementById("div_input"); }
        function EndRequest() {
            var jenis_act = document.getElementById("<%= txtKeyAction.ClientID %>").value;
            switch (jenis_act) {
                case "<%= JenisAction.AddItemKelas %>":
                    document.body.style.paddingRight = "0px";
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    document.body.style.paddingRight = "14.75px";
                    break;
                case "<%= JenisAction.EditItemKelas %>":
                    document.body.style.paddingRight = "0px";
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    document.body.style.paddingRight = "14.75px";
                    break;
                case "<%= JenisAction.DeleteItemKelas %>":
                    document.body.style.paddingRight = "0px";
                    $('#ui_modal_confirm_hapus_item_kelas').modal({ backdrop: 'static', keyboard: false, show: true });
                    document.body.style.paddingRight = "14.75px";
                    break;
                case "<%= JenisAction.DoShowConfirmHapus %>":
                    document.body.style.paddingRight = "0px";
                    $('#ui_modal_confirm_hapus_kelas').modal({ backdrop: 'static', keyboard: false, show: true });
                    document.body.style.paddingRight = "14.75px";
                    break;
                case "<%= JenisAction.DoShowData %>":
                    document.body.style.paddingRight = "0px";
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    document.body.style.paddingRight = "14.75px";
                    break;
                case "<%= JenisAction.Update %>":
                    HideModal();
                    break;
                case "<%= JenisAction.Delete %>":
                    HideModal();
                    break;
                case "<%= JenisAction.ShowDataList %>":
                    SetScrollPos();
                    break;                    
                case "<%= JenisAction.ShowDataListWithUpdate %>":
                    SetScrollPos();
                    HideModal();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah diupdate',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;                    
                case "<%= JenisAction.DoShowEditKelas %>":
                    ResetScrollPos(); 
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
                case "<%= JenisAction.ItemKelasDetKosong %>":
                    HideModal();
                    $('body').snackbar({
                        alive: 4000,
                        content: '<i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;&nbsp;PERHATIAN : Detail kelas tidak boleh kosong.',
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
        }
        function InitModalFocus(){
            $('#ui_modal_input_data').on('shown.bs.modal', function () {
                <%= txtNamaItemKelas.ClientID %>.focus();
            });
        }
        function ListKeyItemsKelas() {
            var txtKeyItemsKelas = document.getElementById("<%= txtKeyItemsKelas.ClientID %>");
            var arr_itemskelas = document.getElementsByName("CHKITEMKELAS[]");
            if (arr_itemskelas.length > 0){
                for (var i = 0; i < arr_itemskelas.length; i++) {
                    if (arr_itemskelas[i].checked) {
                        txtKeyItemsKelas.value += arr_itemskelas[i].id.toString() + ";";
                    }
                }
            }
        }
        function DoScrollPos(){
            var txt_x = document.getElementById("<%= txtXpos.ClientID %>");
            var txt_y = document.getElementById("<%= txtYpos.ClientID %>");
            if(
                txt_x !== null && txt_x !== undefined &&
                txt_y !== null && txt_y !== undefined
              ){
                txt_x.value = window.pageXOffset;
                txt_y.value = window.pageYOffset;
            }
        }

        function SetScrollPos(){
            var txt_x = document.getElementById("<%= txtXpos.ClientID %>");
            var txt_y = document.getElementById("<%= txtYpos.ClientID %>");
            if(
                txt_x !== null && txt_x !== undefined &&
                txt_y !== null && txt_y !== undefined
              ){
                window.scrollTo(txt_x.value,txt_y.value); 
            }
        }

        function ResetScrollPos(){
            window.scrollTo(0,0); 
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
            <asp:HiddenField runat="server" ID="txtIDItemKelas" />
            <asp:HiddenField runat="server" ID="txtSelItem" />
            <asp:HiddenField runat="server" ID="txtYpos" />
            <asp:HiddenField runat="server" ID="txtXpos" />

            <asp:Button runat="server" ID="btnDoEditItemKelas" style="position: absolute; left: -1000px; top: -1000px;" OnClick="btnDoEditItemKelas_Click" />
            <asp:Button runat="server" ID="btnDoEditKelas" style="position: absolute; left: -1000px; top: -1000px;" OnClick="btnDoEditKelas_Click" />
            <asp:Button runat="server" ID="btnDoShowConfirmHapusKelas" style="position: absolute; left: -1000px; top: -1000px;" OnClick="btnDoShowConfirmHapusKelas_Click" />
            <asp:LinkButton ValidationGroup="vldInputCari" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKCari" OnClick="lnkOKCari_Click" Text="OK" style="position: absolute; left: -1000px; top: -1000px;"></asp:LinkButton>
            
            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-12">

                    <div class="col-md-8 col-md-offset-2" style="padding: 0px;">
                        <div class="card" style="margin-top: 40px;">
				            <div class="card-main">
					            <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px; ">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="background-color: #295BC8; padding: 10px; font-weight: normal; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                                <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/folder-1.svg") %>" 
                                                    style="margin: 0 auto; height: 25px; width: 25px;" />
                                                &nbsp;
                                                Level & Kelas
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="background-color: #295BC8; padding: 0px;">
                                                <hr style="margin: 0px; border-style: solid; border-width: 1px; border-color: #2555BE;" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>

                                <asp:MultiView runat="server" ID="mvMain" ActiveViewIndex="0">
                                    <asp:View runat="server" ID="vList">

                                        <div style="padding: 0px; margin: 0px;">
                                            <asp:ListView ID="lvData" DataSourceID="sql_ds" runat="server" OnSorting="lvData_Sorting" OnPagePropertiesChanging="lvData_PagePropertiesChanging">
                                                <LayoutTemplate>
                                                    <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px;">
							                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
								                            <thead>
								                                <tr style="background-color: #455A64;">
                                                                    <th style="text-align: center; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle; width: 80px;">
                                                                        #
									                                </th>
                                                                    <th style="text-align: center; background-color: #3367d6; width: 80px; vertical-align: middle;">
                                                                        <i class="fa fa-cog"></i>
                                                                    </th>
                                                                    <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                        Unit Sekolah, Level
									                                </th>
                                                                    <th style="background-color: #3367d6; text-align: center; padding-left: 10px; vertical-align: middle;">
                                                                        Urutan Level
									                                </th>
                                                                    <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                        Keterangan
									                                </th>
                                                                    <th style="background-color: #3367d6; text-align: center; padding-left: 10px; vertical-align: middle;">
                                                                        Aktif
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
                                                            Eval("Kode").ToString().Trim().ToUpper() == txtID.Value.Trim().ToUpper()
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
                                                                                    onclick="DoScrollPos(); <%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnDoEditKelas.ClientID %>.click();" 
                                                                                    id="btnDetail" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-info-circle" title=" Lihat Detail "></i>&nbsp;&nbsp;&nbsp;Lihat Detail</label>
												                            </li>
                                                                            <li style="padding: 0px;">
                                                                                <hr style="margin: 0px; padding: 0px;" />
                                                                            </li>
												                            <li style="background-color: white; padding: 10px;">
													                            <label
                                                                                    onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnDoShowConfirmHapusKelas.ClientID %>.click();" 
                                                                                    id="btnHapus" style="color: #9E0000; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-times" title=" Hapus Data "></i>&nbsp;&nbsp;&nbsp;Hapus Data</label>
												                            </li>
											                            </ul>
										                            </li>
									                            </ul>
                                                            </div>
                                                        </td>
                                                        <td style="font-weight: normal; padding: 10px; vertical-align: middle; text-align: left; cursor: pointer;" onclick="DoScrollPos(); <%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnDoEditKelas.ClientID %>.click();">
                                                            <asp:Label runat="server" ID="lblSekolah" Text='<%# Eval("Sekolah") %>' style="color: #1DA1F2; font-weight: bold;"></asp:Label>
                                                            <br>
                                                            <span style="font-weight: normal; color: #bfbfbf;">Level&nbsp;</span>
                                                            <asp:Label runat="server" ID="lblNama" Text='<%# Eval("Nama") %>' style="color: grey; font-weight: bold;"></asp:Label>
                                                        </td>                                                    
                                                        <td style="font-weight: normal; padding: 10px; vertical-align: middle; text-align: center; color: grey;">
                                                            # <asp:Label runat="server" ID="lblUrutanJenjang" Text='<%# Eval("UrutanLevel") %>'></asp:Label>
                                                        </td>                                                    
                                                        <td style="font-weight: normal; padding: 10px; vertical-align: middle; text-align: left; color: grey;">
                                                            <asp:Label runat="server" ID="lblKeterangan" Text='<%# (Eval("Keterangan").ToString() == "" ? "-" : Eval("Keterangan").ToString()) %>' style="color: grey;"></asp:Label>
                                                        </td>
                                                        <td style="font-weight: normal; padding: 10px; vertical-align: middle; text-align: center; color: grey;">
                                                            <%# 
                                                                (
                                                                    Eval("IsAktif") == DBNull.Value ? ""
                                                                    : (
                                                                        Convert.ToBoolean(Eval("IsAktif"))
                                                                        ? "<i class='fa fa-check'></i>"
                                                                        : ""
                                                                      )
                                                                )
                                                            %>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <EmptyDataTemplate>
                                                    <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                        <thead>
								                            <tr style="background-color: #455A64;">
                                                                <th style="text-align: center; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle; width: 80px;">
                                                                    #
									                            </th>
                                                                <th style="text-align: center; background-color: #3367d6; width: 80px; vertical-align: middle;">
                                                                    <i class="fa fa-cog"></i>
                                                                </th>
                                                                <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                    Unit Sekolah, Level
									                            </th>
                                                                <th style="background-color: #3367d6; text-align: center; padding-left: 10px; vertical-align: middle;">
                                                                    Urutan Level
									                            </th>
                                                                <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                    Keterangan
									                            </th>
                                                                <th style="background-color: #3367d6; text-align: center; padding-left: 10px; vertical-align: middle;">
                                                                    Aktif
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
                                                    position: fixed; bottom: 28px; right: 25px; width: 350px; border-radius: 25px;
                                                    padding: 8px; margin: 0px;">
                	
                                            <div style="padding-left: 15px;">
				                                <asp:DataPager ID="dpData" runat="server" PageSize="50" PagedControlID="lvData">
                                                    <Fields>
                                                        <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="btn-trans" ShowFirstPageButton="True" FirstPageText='&nbsp;<i class="fa fa-backward"></i>&nbsp;' ShowPreviousPageButton="True" PreviousPageText='&nbsp;<i class="fa fa-arrow-left"></i>&nbsp;' ShowNextPageButton="false" />
                                                        <asp:TemplatePagerField>
                                                            <PagerTemplate>
                                                                <label style="color: grey; font-weight: bold; padding: 5px; border-style: solid; border-color: rgba(0,0,0,0.16); border-width: 1px; padding-left: 10px; padding-right: 10px; border-radius: 5px;">
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
                                                                    &nbsp;
                                                                    <span class="badge badge-danger">
                                                                        <asp:Label ID="ttlRcrd" runat="server" Text="<%#Container.TotalRowCount%>"></asp:Label>
                                                                    </span>
                                                                    &nbsp;
                                                                </span>
                                                            </PagerTemplate>
                                                        </asp:TemplatePagerField>
                                                    </Fields>
                                                </asp:DataPager>
                                            </div>
		                                </div>    

                                    </asp:View>
                                    <asp:View runat="server" ID="vInput">

                                        <div style="padding-top: 15px; padding-bottom: 0px;">
                                                
                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                <div class="col-xs-4">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboUnitSekolah.ClientID %>" style="font-weight: normal; text-transform: none;">Unit Sekolah</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldUnitSekolah"
                                                            ControlToValidate="cboUnitSekolah" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboUnitSekolah" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-xs-8">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtNamaLevel.ClientID %>" style="font-weight: normal; text-transform: none;">Nama Level</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldNamaLevel"
                                                            ControlToValidate="txtNamaLevel" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaLevel" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                <div class="col-xs-4">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboUrutanLevel.ClientID %>" style="font-weight: normal; text-transform: none;">No.Urut Level</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldUrutanLevel"
                                                            ControlToValidate="cboUrutanLevel" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboUrutanLevel" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-xs-8">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtKeterangan.ClientID %>" style="font-weight: normal; text-transform: none;">Keterangan</label>
                                                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtKeterangan" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group" style="margin-top: 0px; margin-bottom: 0px;">
													    <div class="checkbox switch">
														    <label for="<%= chkIsAktifKelas.ClientID %>">
															    <input runat="server" class="access-hide" id="chkIsAktifKelas" type="checkbox"><span class="switch-toggle"></span>
															    <span style="font-weight: normal; font-size: medium; color: grey;">
																    Aktif
															    </span>
														    </label>
													    </div>
												    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 0px; margin-right: 0px; background-color: #295BC8; margin-top: 15px; color: grey;">
                                                <div class="col-xs-12" style="font-weight: bold; color: #7A7A7A; padding: 0px;">
                                                    <hr style="margin: 0px;" />
                                                </div>
                                                <div class="col-xs-12" style="font-weight: bold; color: #7A7A7A; padding-bottom: 5px; padding-top: 5px; padding-right: 0px;">
                                                    <div class="pull-left" style="margin-top: 10px; color: white; text-transform: uppercase;">
                                                        <i class="fa fa-th-list"></i>
                                                        &nbsp;&nbsp;
                                                        Detail&nbsp;&nbsp;Kelas
                                                    </div>                                                        
                                                    <div class="pull-right" style="padding-right: 15px;">
                                                        <asp:LinkButton runat="server" ID="btnAddItem" OnClick="btnAddItem_Click" CssClass="btn btn-flat" style="margin: 0px; color: #bfbfbf;">
                                                            &nbsp;
                                                            <i class="fa fa-plus"></i>
                                                            &nbsp;
                                                        </asp:LinkButton>
                                                        <label onclick="ListKeyItemsKelas();" data-backdrop="static" data-toggle="modal" href="#ui_modal_confirm_hapus_item_kelas" CssClass="btn btn-flat" style="margin: 0px; color: #bfbfbf; cursor: pointer;">
                                                            &nbsp;
                                                            <i class="fa fa-minus"></i>
                                                            &nbsp;
                                                        </label>
                                                    </div>
                                                </div>
                                                <div class="col-xs-12" style="font-weight: bold; color: #7A7A7A; padding: 0px;">
                                                    <hr style="margin: 0px;" />
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 0px; margin-right: 0px; background-color: #3367d6; margin-top: 0px; font-weight: bold; color: grey; margin-bottom: 0px;">
                                                <asp:HiddenField runat="server" ID="txtKeyItemsKelas" />
                                                <table style="width: 100%; margin: 0px;">
                                                    <thead>
                                                        <tr>
                                                            <td style="text-align: left; background-color: #3367d6; color: white; text-transform: none; padding: 5px; padding-left: 15px; padding-right: 15px;">
                                                                Nama&nbsp;&nbsp;Kelas
                                                            </td>
                                                            <td style="text-align: center; background-color: #3367d6; color: white; text-transform: none; padding-left: 15px; padding-right: 15px;">
                                                                No. Urut Kelas
                                                            </td>
                                                            <td style="text-align: center; background-color: #3367d6; color: white; text-transform: none; padding-left: 15px; padding-right: 15px;">
                                                                Jenis Kelas
                                                            </td>
                                                            <td style="text-align: center; background-color: #3367d6; color: white; text-transform: none; padding-left: 15px; padding-right: 15px;">
                                                                Aktif
                                                            </td>                                                               
                                                            <td style="text-align: left; background-color: #3367d6; color: white; text-transform: none; padding-left: 15px; padding-right: 15px;">
                                                                Keterangan
                                                            </td>                                                               
                                                        </tr>
                                                    </thead>
                                                    <asp:Literal runat="server" ID="ltrItemKelas"></asp:Literal>
                                                </table>
                                            </div>

                                            <div class="content-header ui-content-header" 
							                    style="background-color: white;
									                    box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
									                    background-image: none; 
									                    color: white;
									                    display: block;
									                    position: fixed; bottom: 33px; right: 25px; width: 100px; border-radius: 25px;
									                    padding: 8px; margin: 0px;">
							                    <div style="padding-left: 15px;">
								                    <asp:LinkButton ToolTip=" Kembali " runat="server" ID="btnCancelSave" CssClass="btn-trans" OnClick="btnCancelSave_Click">
									                    <i class="fa fa-arrow-left"></i>
								                    </asp:LinkButton>
							                    </div>
						                    </div>

                                            <div class="fbtn-container">
							                    <div class="fbtn-inner">
								                    <asp:LinkButton runat="server" 
                                                       ValidationGroup="vldInput" 
                                                       ID="btnSave" 
                                                       OnClick="btnSave_Click" 
                                                       class="fbtn fbtn-lg fbtn-brand waves-attach waves-circle waves-light" 
                                                       style="background-color: #00198d;" 
                                                       title=" Simpan Data ">
									                    <span class="fbtn-ori icon"><span class="fa fa-check"></span></span>
									                    <span class="fbtn-sub icon"><span class="fa fa-check"></span></span>
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

            <div class="fbtn-container" id="div_button_settings" runat="server">
		        <div class="fbtn-inner">
			        <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #a91212;" title=" Pilihan ">
                        <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                        <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                    </a>
                    <div class="fbtn-dropup">
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

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_data" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
		        <div class="modal-dialog modal-xs">
			        <div class="modal-content">
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
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: white; padding-bottom: 20px;">
							    <div class="media-object margin-right-sm pull-left">
								    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
							    </div>
							    <div class="media-inner">
								    <span style="font-weight: bold; color: black;">
                                        Detail Kelas
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
                                                        <label class="label-input" for="<%= txtNamaItemKelas.ClientID %>" style=" text-transform: none;">Nama Kelas</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputItemKelas" runat="server" ID="vldNamaItemKelas"
                                                            ControlToValidate="txtNamaItemKelas" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ValidationGroup="vldInputItemKelas" runat="server" ID="txtNamaItemKelas" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboNoUrutKelas.ClientID %>" style=" text-transform: none;">No. Urut Kelas</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputItemKelas" runat="server" ID="vldNoUrutKelas"
                                                            ControlToValidate="cboNoUrutKelas" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList ValidationGroup="vldInputItemKelas" runat="server" ID="cboNoUrutKelas" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtKeteranganKelas.ClientID %>" style=" text-transform: none;">Keterangan</label>
                                                        <asp:TextBox ValidationGroup="vldInputItemKelas" runat="server" ID="txtKeteranganKelas" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group" style="margin-top: 0px; margin-bottom: 0px;">
														<div class="radiobtn radiobtn-adv">
															<label for="<%= rdoKelasRombel.ClientID %>">
																<input runat="server" name="jenis_kelas" class="access-hide" id="rdoKelasRombel" type="radio">
																<span style="font-weight: normal; font-size: medium; color: grey;">
																	Kelas / Rombel
																</span>
                                                                <span class="radiobtn-circle"></span>
                                                                <span class="radiobtn-circle-check"></span>
															</label>
														</div>
													</div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group" style="margin-top: 0px; margin-bottom: 0px;">
														<div class="radiobtn radiobtn-adv">
															<label for="<%= rdoKelasJurusan.ClientID %>">
																<input runat="server" name="jenis_kelas" class="access-hide" id="rdoKelasJurusan" type="radio">
																<span style="font-weight: normal; font-size: medium; color: grey;">
																	Kelas Jurusan
																</span>
                                                                <span class="radiobtn-circle"></span>
                                                                <span class="radiobtn-circle-check"></span>
															</label>
														</div>
													</div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group" style="margin-top: 0px; margin-bottom: 0px;">
														<div class="radiobtn radiobtn-adv">
															<label for="<%= rdoKelasSosialisasi.ClientID %>">
																<input runat="server" name="jenis_kelas" class="access-hide" id="rdoKelasSosialisasi" type="radio">
																<span style="font-weight: normal; font-size: medium; color: grey;">
																	Kelas Sosialisasi
																</span>
                                                                <span class="radiobtn-circle"></span>
                                                                <span class="radiobtn-circle-check"></span>
															</label>
														</div>
													</div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="form-group" style="margin-top: 0px; margin-bottom: 0px;">
														<div class="checkbox switch">
															<label for="<%= chkAktif.ClientID %>">
																<input runat="server" class="access-hide" id="chkAktif" type="checkbox"><span class="switch-toggle"></span>
																<span style="font-weight: normal; font-size: medium; color: grey;">
																	Aktif
																</span>
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
				        <div class="modal-footer">
					        <p class="text-right">
                                <asp:LinkButton ValidationGroup="vldInputItemKelas" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInput" OnClick="lnkOKInput_Click" Text="SIMPAN DATA"></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_confirm_hapus_item_kelas" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
		        <div class="modal-dialog modal-xs">
			        <div class="modal-content">
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
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: white; padding-bottom: 20px;">
							    <div class="media-object margin-right-sm pull-left">
								    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
							    </div>
							    <div class="media-inner">
								    <span style="font-weight: bold; color: black;">
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
                                                            Anda yakin akan menghapus kelas yang dipilih?
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkHapusItemKelas" OnClick="lnkHapusItemKelas_Click" Text="HAPUS DATA"></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_confirm_hapus_kelas" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
		        <div class="modal-dialog modal-xs">
			        <div class="modal-content">
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
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: white; padding-bottom: 20px;">
							    <div class="media-object margin-right-sm pull-left">
								    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
							    </div>
							    <div class="media-inner">
								    <span style="font-weight: bold; color: black;">
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
                                                            <asp:Literal runat="server" ID="ltrMsgConfirmHapusKelas"></asp:Literal>
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapusKelas" OnClick="lnkOKHapusKelas_Click" Text="HAPUS DATA"></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
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
