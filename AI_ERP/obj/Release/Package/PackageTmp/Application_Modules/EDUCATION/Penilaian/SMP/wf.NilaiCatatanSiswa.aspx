<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="wf.NilaiCatatanSiswa.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP.wf_NilaiCatatanSiswa" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function HideModal() {
            $('#ui_modal_catatan').modal('hide');

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
                case "<%= JenisAction.DoUpdate %>":
                    HideModal();
                    $('body').snackbar({
                        alive: 1000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah diupdate',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.ShowIsiCatatan %>":
                    ReInitTinyMCE();
                    $('#ui_modal_catatan').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                default:
                    HideModal();
                    if (jenis_act.trim() != "") {
                        $('body').snackbar({
                            alive: 2000,
                            content: '<i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;&nbsp;PERHATIAN : ' + jenis_act,
                            show: function () {
                                snackbarText++;
                            }
                        });
                    }
                    break;
            }

            document.getElementById("<%= txtKeyAction.ClientID %>").value = "";
            InitModalFocus();

            Sys.Browser.WebKit = {};
            if (navigator.userAgent.indexOf('WebKit/') > -1) {
                Sys.Browser.agent = Sys.Browser.WebKit;
                Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
                Sys.Browser.name = 'WebKit';
            }

            ShowProgress(false);
        }

        function ReInitTinyMCE() {
            LoadTinyMCECatatan();
        }

        function ResponseRedirect(url) {
            document.location.href = url;
        }

        function InitModalFocus(){
            $('#ui_modal_catatan').on('shown.bs.modal', function () {
                tinyMCE.execCommand('mceFocus',false,'<%= txtCatatan.ClientID %>');
            });
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

            <asp:HiddenField runat="server" ID="txtID" />
            <asp:HiddenField runat="server" ID="txtKeyAction" />
            <asp:HiddenField runat="server" ID="txtCatatanVal" />
            <asp:HiddenField runat="server" ID="txtIDCatatan" />
            <asp:HiddenField runat="server" ID="txtRelSiswa" />

            <asp:Button UseSubmitBehavior="false" runat="server" OnClientClick="ShowProgress(true);" ID="btnShowIsiCatatan" OnClick="btnShowIsiCatatan_Click" Style="position: absolute; left: -1000px; top: -1000px;" />        

            <asp:MultiView runat="server" ID="mvMain" ActiveViewIndex="0">
                <asp:View runat="server" ID="vList">

                    <div class="row" style="margin-left: 0px; margin-right: 0px;">
                        <div class="col-xs-12">

                            <div class="col-md-6 col-md-offset-3" style="padding: 0px;">
                                <div class="card" style="margin-top: 0px;">
                                    <div class="card-main">
                                        <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px;">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="background-color: #295BC8; padding: 10px; font-weight: normal; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;
                                                                <asp:Literal runat="server" ID="ltrBGHeader"></asp:Literal>">
                                                        <asp:Literal runat="server" ID="ltrCaption"></asp:Literal>
                                                    </td>
                                                </tr>
                                            </table>

                                            <div style="padding: 0px; margin: 0px;">

                                                <asp:Literal runat="server" ID="ltrSiswa"></asp:Literal>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div aria-hidden="true" class="modal fade" id="ui_modal_catatan" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                            <span style="font-weight: bold;">
                                                Isi Catatan Wali Kelas
                                            </span>
                                        </div>
                                    </div>
                                    <div style="width: 100%;">
                                        <div class="row">
                                            <div class="col-lg-12">

                                                <div style="width: 100%; background-color: white; padding-top: 15px;">
                                                    <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                        <div class="col-xs-12" style="color: grey;">
                                                            <table style="width: 100%;">
                                                                <tr>
                                                                    <td style="background-color: white; text-align: center; font-weight: bold; padding: 5px; color: white;">
                                                                        <div runat="server" id="div_persentase_proses" style="margin: 0 auto; display: table; width: 60px; height: 60px; border-radius: 100%;">
                                                                            <asp:Literal runat="server" ID="ltrFotoSiswa"></asp:Literal>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="background-color: white; text-align: center; font-weight: bold; padding: 5px;">
                                                                        <asp:Label ID="lblNIS" runat="server" style="color: #0091EB;"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="background-color: white; text-align: center; font-weight: bold; padding: 5px;">
                                                                        <asp:Label ID="lblNamaSiswa" runat="server" style="color: grey;"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                    <div class="row" style="margin-left: 15px; margin-right: 15px; margin-top: 20px;">
                                                        <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                            <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 0px;">
                                                                <label class="label-input" for="<%= txtCatatan.ClientID %>" style="color: black; text-transform: none; margin-bottom: 6px;">
                                                                    Catatan untuk Siswa
                                                                </label>
                                                                <asp:TextBox CssClass="mcetiny_catatan" runat="server" ID="txtCatatan" style="height: 200px;"></asp:TextBox>
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
                                        <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" OnClientClick="ShowProgress(true);" runat="server" ID="lnkOKSaveCatatan" OnClick="lnkOKSaveCatatan_Click" Text="  SIMPAN  "></asp:LinkButton>
                                        <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Tutup</a>
                                        <br />
                                        <br />
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="fbtn-container" id="div_button_settings" runat="server" style="z-index: 99;">
                        <div class="fbtn-inner">
                            <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #329CC3;" title=" Pengaturan ">
                                <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                                <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                            </a>
                            <div class="fbtn-dropup" style="z-index: 999999;">
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
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkOKPilihan" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpNomor2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
        function LoadTinyMCECatatan() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.remove();
            tinymce.init({
                selector: ".mcetiny_catatan",
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
                height: 200,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtCatatanVal.ClientID %>').value = ed.getContent();
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
        InitModalFocus();
    </script>
</asp:Content>
