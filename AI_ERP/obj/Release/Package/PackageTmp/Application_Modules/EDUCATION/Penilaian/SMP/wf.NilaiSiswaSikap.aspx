<%@ Page Title="" Language="C#" ValidateRequest="false" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.NilaiSiswaSikap.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP.wf_NilaiSiswaSikap" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function HideModal() {
            $('#ui_modal_input_nilai_sikap').modal('hide');
            
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
                case "<%= JenisAction.ShowInputNilaiAkhir %>":
                    ReInitTinyMCE();
                    document.getElementById("lbl_walas_spiritual").innerHTML = document.getElementById("<%= txtNilaiWalasSpiritual.ClientID %>").value;
                    document.getElementById("lbl_walas_sosial").innerHTML = document.getElementById("<%= txtNilaiWalasSosial.ClientID %>").value;
                    document.getElementById("lbl_modus_spiritual").innerHTML = document.getElementById("<%= txtNilaiModusSpiritual.ClientID %>").value;
                    document.getElementById("lbl_modus_sosial").innerHTML = document.getElementById("<%= txtNilaiModusSosial.ClientID %>").value;
                    $('#ui_modal_input_nilai_sikap').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoUpdate %>":
                    HideModal();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DataLoaded %>":
                    HideModal();
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

            Sys.Browser.WebKit = {};
            if (navigator.userAgent.indexOf('WebKit/') > -1) {
                Sys.Browser.agent = Sys.Browser.WebKit;
                Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
                Sys.Browser.name = 'WebKit';
            }

            InitModalFocus();
            ShowProgress(false);
        }

        function SaveNilaiSikap(tahun_ajaran, semester, rel_mapel, rel_kelasdet, rel_siswa, sikap_spiritual, sikap_sosial) {
            var s_url = "<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APIS.SMP.NILAI_SISWA.DO_SAVE.FILE + "/DoUpdateSikap") %>";
            s_url += "?t=" + tahun_ajaran +
                     "&sm=" + semester +
                     "&mp=" + rel_mapel +
                     "&kd=" + rel_kelasdet +
                     "&s=" + rel_siswa +
                     "&ssp=" + sikap_spiritual +
                     "&sss=" + sikap_sosial;

            $.ajax({
                url: s_url, 
                dataType: 'json',
                type: 'GET', 
                contentType: 'application/json; charset=utf-8', 
                success: function (data) {
                        <%= btnDoLoadData.ClientID %>.click();
                        HideModal();
                        $('body').snackbar({
                            alive: 2000,
                            content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                            show: function () {
                                snackbarText++;
                            }
                        });
                    }, 
                error: function(response) { 
                        alert(response.responseText); 
                    }, 
                failure: function(response) { 
                        alert(response.responseText); 
                    } 
            }); 
        }

        function ResponseRedirect(url) {
            document.location.href = url;
        }

        function ReInitTinyMCE() {
            RemoveTinyMCE();
            LoadTinyMCEDeskripsiSikapSpiritual();
            LoadTinyMCEDeskripsiSikapSosial();
        }

        function InitModalFocus(){
            $('#ui_modal_input_nilai_sikap').on('shown.bs.modal', function () {
                //tinyMCE.execCommand('mceFocus',false,'<%= txtDeskripsiSikapSpiritual.ClientID %>');
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
    <style type="text/css">
        .modal-backdrop
        {
            opacity:0 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpNomor2" runat="server">
    
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

    <div id="div_statusbar" runat="server" style="color: black; height: 40px; background-color: #eeeeee; padding: 10px; position: fixed; left: 0px; bottom: 0px; right: 0px; z-index: 99; box-shadow: 0 -5px 5px -5px #bcbcbc;">
        <asp:Literal runat="server" ID="ltrStatusBar"></asp:Literal>
    </div>

    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="txtKeyAction" />
            <asp:HiddenField runat="server" ID="txtIDNilaiSikap" />
            <asp:HiddenField runat="server" ID="txtIDSiswa" />
            <asp:HiddenField runat="server" ID="txtDeskripsiSikapSosialVal" />
            <asp:HiddenField runat="server" ID="txtDeskripsiSikapSpiritualVal" />
            <asp:HiddenField runat="server" ID="txtKodeSN" />
            <asp:HiddenField runat="server" ID="txtNilaiWalasSpiritual" />
            <asp:HiddenField runat="server" ID="txtNilaiWalasSosial" />
            <asp:HiddenField runat="server" ID="txtNilaiModusSpiritual" />
            <asp:HiddenField runat="server" ID="txtNilaiModusSosial" />

            <asp:Button UseSubmitBehavior="false" runat="server" OnClientClick="ShowProgress(true);" ID="btnShowIsiNilaiSikap" OnClick="btnShowIsiNilaiSikap_Click" Style="position: absolute; left: -1000px; top: -1000px;" />        
            <asp:Button UseSubmitBehavior="false" runat="server" OnClientClick="ShowProgress(true);" ID="btnDoLoadData" OnClick="btnDoLoadData_Click" Style="position: absolute; left: -1000px; top: -1000px;" />        

            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-12">

                    <div runat="server" id="div_container" class="col-md-6 col-md-offset-3" style="padding: 0px;">
                        <div class="card" style="margin-top: -8px;">
                            <div class="card-main">
                                <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px;">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="background-color: #295BC8; padding: 10px; font-weight: normal; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                                <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/stats.svg") %>"
                                                    style="margin: 0 auto; height: 25px; width: 25px;" />
                                                &nbsp;
                                                Nilai Sikap
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="background-color: #295BC8; padding: 0px;">
                                                <hr style="margin: 0px; border-style: solid; border-width: 1px; border-color: #2555BE;" />
                                            </td>
                                        </tr>
                                    </table>

                                    <div style="padding: 0px; margin: 0px;">
                                        <asp:Literal runat="server" ID="ltrNilaiSiswa"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="content-header ui-content-header"
                style="
                    background-color: whitesmoke;
                    box-shadow: -1px 2px 6px rgba(0,0,0,0.16), 0 -1px 6px rgba(0,0,0,0.23);
                    background-image: none;
                    color: white;
                    display: block;
                    z-index: 5000;
                    position: fixed;
                    bottom: 40px;
                    right: 5px;
                    width: 300px;
                    border-radius: 5px;
                    padding: 12px;
                    padding-right: 0px;
                    padding-top: 0px;
                    padding-left: 0px;
                    padding-bottom: 0px;
                    margin: 0px;
                    margin-bottom: 5px;
                ">

                <div style="width: 100%; background-color: #227447; padding: 10px; border-bottom-color: #d3d3d3; border-bottom-style: solid; border-bottom-width: 1px;">
                    <span class="icon icon-lg text-brand-accent" style="color: white;">info_outline</span>
                    &nbsp;
                    <span style="font-weight: bold; color: white;">Deskripsi</span>
                </div>

                <div style="max-height: 200px; overflow-y: auto; background-color: white;">

                    <div style="margin-bottom: 60px; background-color: white; color: grey; padding: 15px;">

                        <asp:Literal runat="server" ID="ltrDeskripsiSikap"></asp:Literal>

                    </div>

                </div>

            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_pilihan" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                <asp:Literal runat="server" ID="ltrCaption"></asp:Literal>
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

            <div class="fbtn-container" id="div_button_settings" runat="server" style="z-index: 999999;">
		        <div class="fbtn-inner">
			        <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #329CC3;" title=" Pengaturan ">
                        <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                        <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                    </a>
                    <div class="fbtn-dropup" style="z-index: 999999;">
                        <a runat="server" id="a_pilih_data_nilai" data-toggle="modal" href="#ui_modal_pilihan" class="fbtn fbtn-green waves-attach waves-circle waves-effect" style="cursor: pointer; background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Pilih Data Nilai</span>
                            <i class="fa fa-list" style="color: white;"></i>
                        </a>
                    </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_nilai_sikap" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                        Isi Nilai Akhir Sikap
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div  id="div_deskripsi_sikap">
                                                <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                    <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= txtNamaSiswa.ClientID %>" style="text-transform: none;">Nama Siswa</label>
                                                            <asp:TextBox CssClass="form-control" runat="server" ID="txtNamaSiswa" Enabled="false"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div> 
                                                <div class="row" style="padding: 10px; border-style: solid; border-width: 1px; border-color: #bfbfbf; margin-bottom: 5px; margin-left: 0px; margin-right: 0px; border-left-style: none; border-right-style: none;">
                                                    <div class="col-md-12">
                                                        <span style="font-weight: bold;">Nilai Wali Kelas</span>
                                                        <table style="margin: 0px; width: 100%;">
                                                            <tr>
                                                                <td style="padding: 0px; font-size: small; width: 150px; padding-right: 15px; vertical-align: middle; padding-bottom: 7px; padding-top: 0px;">
                                                                    <span style="color: grey; font-weight: normal;">Sikap Spiritual</span><br />
                                                                    <label id="lbl_walas_spiritual" style="background-color: #F5F8FA; border: 1px solid #E6ECF0; font-weight: bold; width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px; padding: 0px; padding-top: 3px; padding-bottom: 3px; padding-left: 5px; padding-right: 5px;">
                                                                    </label>
                                                                </td>
                                                                <td style="padding: 0px; font-size: small; width: 150px; padding-right: 15px; vertical-align: middle; padding-bottom: 7px; padding-top: 0px;">
                                                                    <span style="color: grey; font-weight: normal;">Sikap Sosial</span><br />
                                                                    <label id="lbl_walas_sosial" style="background-color: #F5F8FA; border: 1px solid #E6ECF0; font-weight: bold; width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px; padding: 0px; padding-top: 3px; padding-bottom: 3px; padding-left: 5px; padding-right: 5px;">
                                                                    </label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                                <div class="row" style="padding: 10px; border-style: solid; border-width: 1px; border-color: #bfbfbf; margin-bottom: 5px; margin-left: 0px; margin-right: 0px; border-left-style: none; border-right-style: none;">
                                                    <div class="col-md-12">
                                                        <span style="font-weight: bold;">Modus</span>
                                                        <table style="margin: 0px; width: 100%;">
                                                            <tr>
                                                                <td style="padding: 0px; font-size: small; width: 150px; padding-right: 15px; vertical-align: middle; padding-bottom: 7px; padding-top: 0px;">
                                                                    <span style="color: grey; font-weight: normal;">Sikap Spiritual</span><br />
                                                                    <label id="lbl_modus_spiritual" style="background-color: #F5F8FA; border: 1px solid #E6ECF0; font-weight: bold; width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px; padding: 0px; padding-top: 3px; padding-bottom: 3px; padding-left: 5px; padding-right: 5px;">
                                                                    </label>
                                                                </td>
                                                                <td style="padding: 0px; font-size: small; width: 150px; padding-right: 15px; vertical-align: middle; padding-bottom: 7px; padding-top: 0px;">
                                                                    <span style="color: grey; font-weight: normal;">Sikap Sosial</span><br />
                                                                    <label id="lbl_modus_sosial" style="background-color: #F5F8FA; border: 1px solid #E6ECF0; font-weight: bold; width: 100%; border-radius: 0px; padding-left: 5px; padding-right: 5px; padding: 0px; padding-top: 3px; padding-bottom: 3px; padding-left: 5px; padding-right: 5px;">
                                                                    </label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                                <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                    <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= cboSikapSpiritual.ClientID %>" style="text-transform: none;">Sikap Spiritual (Akhir)</label>
                                                            <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldSikapSpiritual"
                                                                ControlToValidate="cboSikapSpiritual" Display="Dynamic" style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:DropDownList CssClass="form-control" runat="server" ID="cboSikapSpiritual">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div> 
                                                <div class="row">
                                                    <div class="col-lg-12">
                                        
                                                        <div style="width: 100%; background-color: white; padding-top: 0px;">
                                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                                <div class="col-xs-12">
                                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                        <label class="label-input" for="<%= txtDeskripsiSikapSpiritual.ClientID %>" style="text-transform: none; margin-bottom: 6px;">
                                                                            Deskripsi Sikap Spiritual (Akhir)
                                                                        </label>
                                                                        <asp:TextBox style="height: 100px;" runat="server" ID="txtDeskripsiSikapSpiritual" CssClass="mcetiny_deskripsi_sikap_spiritual"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                                <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                    <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= cboSikapSosial.ClientID %>" style="text-transform: none;">Sikap Sosial (Akhir)</label>
                                                            <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldSikapSosial"
                                                                ControlToValidate="cboSikapSosial" Display="Dynamic" style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:DropDownList CssClass="form-control" runat="server" ID="cboSikapSosial">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div> 
                                                <div class="row">
                                                    <div class="col-lg-12">
                                        
                                                        <div style="width: 100%; background-color: white; padding-top: 0px;">
                                                            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                                                                <div class="col-xs-12">
                                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                        <label class="label-input" for="<%= txtDeskripsiSikapSosial.ClientID %>" style="text-transform: none; margin-bottom: 6px;">
                                                                            Deskripsi Sikap Sosial (Akhir)
                                                                        </label>
                                                                        <asp:TextBox style="height: 100px;" runat="server" ID="txtDeskripsiSikapSosial" CssClass="mcetiny_deskripsi_sikap_sosial"></asp:TextBox>
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
				        </div>
				        <div class="modal-footer">
					        <p class="text-right">
                                <asp:LinkButton OnClientClick="if(Page_ClientValidate('vldInput')){ ShowProgress(true); }" ValidationGroup="vldInput" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInput" OnClick="lnkOKInput_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <iframe name="fra_loader" id="fra_loader" height="0" width="0" style="position: absolute; left: -1000px; top: -1000px;"></iframe>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server"> 
    <script type="text/javascript">
        function LoadTinyMCEDeskripsiSikapSpiritual() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                mode : "exact",
                selector: ".mcetiny_deskripsi_sikap_spiritual",
                theme: "modern",
                plugins: [
                    "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                    "searchreplace wordcount visualblocks visualchars code fullscreen",
                    "insertdatetime media nonbreaking save table contextmenu directionality",
                    "emoticons template paste textcolor tinyfilemanager.net"
                ],
                toolbar1: "bold italic underline | bullist numlist",
                image_advtab: true,
                templates: [
                    { title: 'Test template 1', content: 'Test 1' },
                    { title: 'Test template 2', content: 'Test 2' }
                ],
                resize: "vertical",
                statusbar: false,
                menubar: false,
                height: 120,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtDeskripsiSikapSpiritualVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
        }

        function LoadTinyMCEDeskripsiSikapSosial() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                mode : "exact",
                selector: ".mcetiny_deskripsi_sikap_sosial",
                theme: "modern",
                plugins: [
                    "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                    "searchreplace wordcount visualblocks visualchars code fullscreen",
                    "insertdatetime media nonbreaking save table contextmenu directionality",
                    "emoticons template paste textcolor tinyfilemanager.net"
                ],
                toolbar1: "bold italic underline | bullist numlist",
                image_advtab: true,
                templates: [
                    { title: 'Test template 1', content: 'Test 1' },
                    { title: 'Test template 2', content: 'Test 2' }
                ],
                resize: "vertical",
                statusbar: false,
                menubar: false,
                height: 120,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtDeskripsiSikapSosialVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
        }

        function RemoveTinyMCE(){
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtDeskripsiSikapSpiritual.ClientID %>');
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtDeskripsiSikapSosial.ClientID %>');            
        }
    </script>   
</asp:Content>
