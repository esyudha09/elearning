<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="wf.Pengaturan.SMA.aspx.cs" Inherits="AI_ERP.Application_Modules.MASTER.wf_Pengaturan_SMA" %>
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
                case "<%= JenisAction.Add %>":
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmHapus %>":
                    $('#ui_modal_confirm_hapus').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowData %>":
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowDataTemplateEmailLTS %>":
                    ReInitTinyMCE();
                    break;
                case "<%= JenisAction.DoUpdate %>":
                    ReInitTinyMCE();
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
            
        }

        function ReInitTinyMCE(){
            RemoveTinyMCE();
            LoadTinyMCEKontenJudulKopSurat();
            LoadTinyMCEKontenAlamatKopSurat();
            LoadTinyMCEKontenHTMLLinkExpired();
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
            <asp:HiddenField runat="server" ID="txtTemplateJudulKopEmailVal" />  
            <asp:HiddenField runat="server" ID="txtTemplateAlamatKopEmailVal" />  
            <asp:HiddenField runat="server" ID="txtTemplateKontenHTMLLinkExpiredVal" />              

            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-12">

                    <div class="col-md-8 col-md-offset-2" style="padding: 0px;">
                        <div class="card" style="margin-top: 40px;">
				            <div class="card-main">
					            <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px; ">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="background-color: #295BC8; padding: 10px; font-weight: normal; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                                <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/002-website.svg") %>" 
                                                    style="margin: 0 auto; height: 25px; width: 25px;" />
                                                &nbsp;
                                                Pengaturan Umum
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
                                                
                                                <div class="row" style="margin-left: -10px; margin-right: -10px; margin-top: 2px;">
													<div class="col-md-12" style="padding-left: 12px; padding-right: 12px;">

														<div class="form-group form-group-label" style="margin-bottom: 2px; padding-bottom: 0px; margin-top: 0px;">
															<div class="row" style="padding: 0px; margin: 0px; background-color: #f1f3c1; border-style: solid; border-width: 1px; border-color: #C0C0C0; border-bottom-style: none;">
                                                                <div class="col-xs-12" style="padding: 10px; text-align: justify;">
                                                                    <i class="fa fa-info-circle"></i>
                                                                    &nbsp;
                                                                    Judul Kop Email
                                                                </div>
                                                            </div>
															<asp:TextBox runat="server" ID="txtTemplateJudulKopEmail" CssClass="mcetiny_data_judul_kop_email"></asp:TextBox>
														</div>

                                                        <div class="form-group form-group-label" style="margin-bottom: 2px; padding-bottom: 0px; margin-top: 0px;">
															<div class="row" style="padding: 0px; margin: 0px; background-color: #f1f3c1; border-style: solid; border-width: 1px; border-color: #C0C0C0; border-bottom-style: none;">
                                                                <div class="col-xs-12" style="padding: 10px; text-align: justify;">
                                                                    <i class="fa fa-home"></i>
                                                                    &nbsp;
                                                                    Alamat Kop Email
                                                                </div>
                                                            </div>
															<asp:TextBox runat="server" ID="txtTemplateAlamatKopEmail" CssClass="mcetiny_data_alamat_kop_email"></asp:TextBox>
														</div>

													</div>
												</div>

                                                <div class="row">
                                                    <div class="col-xs-12" style="padding: 5px; padding-left: 25px;">
                                                        <div class="checkbox checkbox-adv">
                                                            <label for="<%= chkIsTestEmail.ClientID %>">
                                                                <input id="chkIsTestEmail"
                                                                       runat="server"
                                                                       class="access-hide"
                                                                       type="checkbox" />
                                                                <span class="checkbox-circle"></span><span class="checkbox-circle-check"></span><span class="checkbox-circle-icon icon">done</span>
                                                                <span style="font-weight: bold; font-size: 14px; color: black;">
                                                                    Mode Tes Email
                                                                </span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="margin-left: -8px; margin-right: -8px;">
                                                    <div class="col-xs-6">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= txtEmailTestEmail.ClientID %>" style="text-transform: none;">Email untuk mode tes</label>
                                                            <asp:TextBox runat="server" ID="txtEmailTestEmail" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-xs-6">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= txtTeksLinkLTS.ClientID %>" style="text-transform: none;">Teks link LTS</label>
                                                            <asp:TextBox runat="server" ID="txtTeksLinkLTS" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="margin-left: -8px; margin-right: -8px;">
                                                    <div class="col-xs-12">
                                                        <span style="font-weight: bold; font-size: 14px; color: black;">
                                                            Expired Link
                                                        </span>
                                                    </div>
                                                </div>

                                                <div class="row" style="margin-left: -8px; margin-right: -8px;">
                                                    <div class="col-xs-4">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= txtExpiredLinkHari.ClientID %>" style="text-transform: none;">Hari</label>
                                                            <asp:TextBox runat="server" ID="txtExpiredLinkHari" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-xs-4">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= txtExpiredLinkJam.ClientID %>" style="text-transform: none;">Jam</label>
                                                            <asp:TextBox runat="server" ID="txtExpiredLinkJam" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-xs-4">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= txtExpiredLinkMenit.ClientID %>" style="text-transform: none;">Menit</label>
                                                            <asp:TextBox runat="server" ID="txtExpiredLinkMenit" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="margin-left: -8px; margin-right: -8px;">
                                                    <div class="col-xs-4">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= cboJenisFileRapor.ClientID %>" style="text-transform: none;">Jenis File Rapor</label>
                                                            <asp:DropDownList runat="server" ID="cboJenisFileRapor" CssClass="form-control">
                                                                <asp:ListItem Value="PDF" Text="PDF"></asp:ListItem>
                                                                <asp:ListItem Value="HTML" Text="HTML"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" style="margin-left: -10px; margin-right: -10px; margin-top: 2px;">
													<div class="col-md-12" style="padding-left: 12px; padding-right: 12px;">

														<div class="form-group form-group-label" style="margin-bottom: 2px; padding-bottom: 0px; margin-top: 0px;">
															<div class="row" style="padding: 0px; margin: 0px; background-color: #f1f3c1; border-style: solid; border-width: 1px; border-color: #C0C0C0; border-bottom-style: none;">
                                                                <div class="col-xs-12" style="padding: 10px; text-align: justify;">
                                                                    <i class="fa fa-home"></i>
                                                                    &nbsp;
                                                                    Template Konten Link Expired
                                                                </div>
                                                            </div>
															<asp:TextBox runat="server" ID="txtTemplateKontenHTMLLinkExpired" CssClass="mcetiny_data_html_link_expired"></asp:TextBox>
														</div>

													</div>
												</div>

                                            </div>

                                            <div class="fbtn-container">
							                    <div class="fbtn-inner">
								                    <a id="btnDoSavePengaturanPSB" 
                                                        onmouseup="setTimeout(function() { $('#<%= mdlConfirmSavePengaturan.ClientID %>').modal('show'); }, 300);" 
                                                        class="fbtn fbtn-lg fbtn-brand waves-attach waves-circle waves-light" 
                                                        data-toggle="dropdown" 
                                                        style="background-color: #00198d;" 
                                                        title=" Simpan Pengaturan ">
									                    <span class="fbtn-ori icon"><span class="fa fa-check"></span></span>
									                    <span class="fbtn-sub icon"><span class="fa fa-check"></span></span>
								                    </a>
							                    </div>
						                    </div>

                                            <div aria-hidden="true" class="modal modal-va-middle fade modal-va-middle-show in" runat="server" id="mdlConfirmSavePengaturan" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 1500;">
												<div class="modal-dialog modal-xs">
													<div class="modal-content">
														<div class="modal-inner">
															<div class="media margin-bottom margin-top">
																<div class="media-object margin-right-sm pull-left">
																	<span class="icon icon-lg text-brand-accent">info_outline</span>
																</div>
																<div class="media-inner">
																	<span style="font-weight: bold;">
																		KONFIRMASI
																	</span>
																</div>
															</div>
															Anda yakin akan menyimpan data?
														</div>
														<div class="modal-footer">
															<p class="text-right">
																<asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect"  runat="server" ID="btnOKSavePengaturan" OnClick="btnOKSavePengaturan_Click">
																	<i class="fa fa-check"></i>
																	&nbsp;
																	Simpan Data
																</asp:LinkButton>
																<a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
															</p>
														</div>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpNomor2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
		function LoadTinyMCEKontenJudulKopSurat() {
	        tfm_path = 'Application_CLibs/fileman';
	        tinymce.init({
	            selector: ".mcetiny_data_judul_kop_email",
	            theme: "modern",
	            plugins: [
					"advlist autolink lists link image charmap print preview hr anchor pagebreak",
					"searchreplace wordcount visualblocks visualchars code fullscreen",
					"insertdatetime media nonbreaking save table contextmenu directionality",
					"emoticons template paste textcolor tinyfilemanager.net"
	            ],
	            toolbar1: "insertfile undo redo | bold italic | forecolor backcolor | fontselect fontsizeselect",
	            toolbar2: "",
	            image_advtab: true,
	            templates: [
					{ title: 'Test template 1', content: 'Test 1' },
					{ title: 'Test template 2', content: 'Test 2' }
	            ],
	            resize: "vertical",
	            menubar: false,
	            height: 30,
	            statusbar: false,
	            convert_urls: false,
	            contextmenu: "cut copy paste selectall | link image inserttable | cell row column deletetable",
	            setup: function (ed) {
	                ed.on('change', function (e) {
	                    document.getElementById('<%= txtTemplateJudulKopEmailVal.ClientID %>').value = ed.getContent();
	                });
	            }
	        });
        }

        function LoadTinyMCEKontenAlamatKopSurat() {
	        tfm_path = 'Application_CLibs/fileman';
	        tinymce.init({
	            selector: ".mcetiny_data_alamat_kop_email",
	            theme: "modern",
	            plugins: [
					"advlist autolink lists link image charmap print preview hr anchor pagebreak",
					"searchreplace wordcount visualblocks visualchars code fullscreen",
					"insertdatetime media nonbreaking save table contextmenu directionality",
					"emoticons template paste textcolor tinyfilemanager.net"
	            ],
	            toolbar1: "insertfile undo redo | bold italic | forecolor backcolor | fontselect fontsizeselect",
	            toolbar2: "",
	            image_advtab: true,
	            templates: [
					{ title: 'Test template 1', content: 'Test 1' },
					{ title: 'Test template 2', content: 'Test 2' }
	            ],
	            resize: "vertical",
	            menubar: false,
	            height: 100,
	            statusbar: false,
	            convert_urls: false,
	            contextmenu: "cut copy paste selectall | link image inserttable | cell row column deletetable",
	            setup: function (ed) {
	                ed.on('change', function (e) {
	                    document.getElementById('<%= txtTemplateAlamatKopEmailVal.ClientID %>').value = ed.getContent();
	                });
	            }
	        });
        }

        function LoadTinyMCEKontenHTMLLinkExpired() {
	        tfm_path = 'Application_CLibs/fileman';
	        tinymce.init({
	            selector: ".mcetiny_data_html_link_expired",
	            theme: "modern",
	            plugins: [
					"advlist autolink lists link image charmap print preview hr anchor pagebreak",
					"searchreplace wordcount visualblocks visualchars code fullscreen",
					"insertdatetime media nonbreaking save table contextmenu directionality",
					"emoticons template paste textcolor tinyfilemanager.net"
	            ],
	            toolbar1: "insertfile undo redo | bold italic | forecolor backcolor | fontselect fontsizeselect",
	            toolbar2: "",
	            image_advtab: true,
	            templates: [
					{ title: 'Test template 1', content: 'Test 1' },
					{ title: 'Test template 2', content: 'Test 2' }
	            ],
	            resize: "vertical",
	            menubar: false,
	            height: 300,
	            statusbar: false,
	            convert_urls: false,
	            contextmenu: "cut copy paste selectall | link image inserttable | cell row column deletetable",
	            setup: function (ed) {
	                ed.on('change', function (e) {
	                    document.getElementById('<%= txtTemplateKontenHTMLLinkExpiredVal.ClientID %>').value = ed.getContent();
	                });
	            }
	        });
	    }

        ReInitTinyMCE();

		Sys.Browser.WebKit = {};
		if (navigator.userAgent.indexOf('WebKit/') > -1) {
			Sys.Browser.agent = Sys.Browser.WebKit;
			Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
			Sys.Browser.name = 'WebKit';
		}

		function RemoveTinyMCE() {
		    tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtTemplateJudulKopEmail.ClientID %>');
		    tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtTemplateAlamatKopEmail.ClientID %>');
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtTemplateKontenHTMLLinkExpired.ClientID %>');
		}
	</script>
</asp:Content>
