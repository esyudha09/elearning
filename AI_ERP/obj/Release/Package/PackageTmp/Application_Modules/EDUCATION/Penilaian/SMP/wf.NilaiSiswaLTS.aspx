<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.NilaiSiswaLTS.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP.wf_NilaiSiswaLTS" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        @media print {
          body * {
            visibility: hidden;
          }
          #printarea, #printarea * {
            visibility: visible;
          }
          #printarea {
            position: absolute;
            left: 0;
            top: 0;
          }
        }
    </style>
    <script type="text/javascript">
        function HideModal() {
            $('#ui_modal_list_siswa').modal('hide');
            $('#ui_modal_pilih_semester').modal('hide');
            $('#ui_modal_pengaturan').modal('hide');
            $('#ui_modal_proses').modal('hide');            

            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();

            document.body.style.paddingRight = "0px";
        }

        function InitModalFocus(){
            $('#ui_modal_pengaturan').on('shown.bs.modal', function () {
                <%= txtNamaGuru.ClientID %>.focus();
            });
        }

        function EndRequestHandler() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        }

        function EndRequest() {
            var jenis_act = document.getElementById("<%= txtKeyAction.ClientID %>").value;
            switch (jenis_act) {
                case "<%= JenisAction.ShowPengaturanLTS %>":
                    ShowProgress(false);
                    $('#ui_modal_pengaturan').modal({ backdrop: 'static', keyboard: false, show: true });
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
            InitModalFocus();
            ShowInfoLabel();
            window.scrollTo(0, 0);
        }

        function ResponseRedirect(url) {
            document.location.href = url;
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

        function ShowInfoLabel() {
            var lbl = document.getElementById("lbl_nomor");
            if (lbl !== null && lbl !== undefined){
                lbl.innerHTML = "Nomor&nbsp;" +
                                (parseInt(txtIndexSiswa().value) + 1).toString() +
                                "&nbsp;dari&nbsp;" +
                                (parseInt(txtCountSiswa().value)).toString();
            }
        }

        function InitPicker() {
            $('#<%= txtTanggalLTS.ClientID %>').pickdate({ cancel: '', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });
        }

        function ShowProgress(show){
            if(show){
                $('#ui_modal_proses').modal({ backdrop: 'static', keyboard: false, show: true });
            }
            else {
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                $('#ui_modal_proses').modal('hide');
                HideModal();
            }
        }
    </script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/QRCode/qrcode.min.js") %>"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="txtIDSiswa" />
            <asp:HiddenField runat="server" ID="txtIndexSiswa" />
            <asp:HiddenField runat="server" ID="txtCountSiswa" />
            <asp:HiddenField runat="server" ID="txtSemester" />    
            <asp:HiddenField runat="server" ID="txtTahunAjaran" />
            <asp:HiddenField runat="server" ID="txtKelasDet" />    
            <asp:HiddenField runat="server" ID="txtKeyAction" />

            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowNilaiSiswa" OnClick="btnShowNilaiSiswa_Click" style="position: absolute; left: -1000px; top: -1000px;" />

            <div class="fbtn-container" id="div_button_settings" runat="server" style="z-index: 999999;">
                <div class="fbtn-inner">
                    <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #329CC3;" title=" Pengaturan ">
                        <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                        <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                    </a>
                    <div class="fbtn-dropup" style="z-index: 999999;">
                        <asp:LinkButton CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkOKDeskripsiLTS" Style="background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Deskripsi Kompetensi Dasar</span>
                            <i class="fa fa-file-text-o" style="color: white;"></i>
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkOKCetakSatuKelas" Style="background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Cetak Nilai Satu Kelas</span>
                            <i class="fa fa-print" style="color: white;"></i>
                        </asp:LinkButton>
                        <asp:LinkButton CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkOKCetakAktif" Style="background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Cetak Nilai Ditampilan</span>
                            <i class="fa fa-print" style="color: white;"></i>
                        </asp:LinkButton>
                        <a data-toggle="modal" href="#ui_modal_pilih_semester" class="fbtn fbtn-green waves-attach waves-circle waves-effect" style="cursor: pointer; background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Pilih Data Nilai</span>
                            <i class="fa fa-list" style="color: white;"></i>
                        </a>
                        <asp:LinkButton Visible="false" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkPilihKelas" OnClick="lnkPilihKelas_Click" Style="background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Pilih Kelas Lain</span>
                            <i class="fa fa-th-large" style="color: white;"></i>
                        </asp:LinkButton>
                        <asp:LinkButton OnClientClick="ShowProgress(true);" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkPengaturanLTS" OnClick="lnkPengaturanLTS_Click" Style="background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Pengaturan</span>
                            <i class="fa fa-cog" style="color: white;"></i>
                        </asp:LinkButton>
                    </div>
                </div>
            </div>

            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-12">

                    <div class="col-md-8 col-md-offset-2" style="padding: 0px;">
                        <div class="card" style="margin-top: 0px; box-shadow: none; border-style: solid; border-color: #C6C6C6; border-width: 1px;">
				            <div class="card-main">
					            <div class="card-inner" style="margin: 0px; padding: 0px; ">
                                    <div style="margin: 0 auto; display: table; margin-top: 25px; margin-bottom: 25px;">
                                        <div id="printarea" style="margin: 0 auto; display: table; margin-top: -15px;">
                                            <asp:Literal runat="server" ID="ltrLTS"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                            </div>
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

            <div class="content-header ui-content-header" 
                style="background-color: whitesmoke;
                        box-shadow: -1px 2px 6px rgba(0,0,0,0.16), 0 -1px 6px rgba(0,0,0,0.23);
                        background-image: none; 
                        color: white;
                        display: block;
                        z-index: 5;
                        position: fixed; bottom: 50px; right: -25px; width: 300px; border-radius: 5px;
                        padding: 12px; 
                        padding-top: 0px;
                        padding-left: 0px;
                        margin: 0px;">

                <div style="width: 100%; background-color: whitesmoke; padding: 10px; border-bottom-color: #d3d3d3; border-bottom-style: solid; border-bottom-width: 1px;">
                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                    &nbsp;
                    <span style="font-weight: bold; color: black;">Nilai LTS</span>
                </div>

                <div style="padding: 15px;">
                    <span style="font-weight: bold; font-size: medium; color: black;">
                        <asp:Literal runat="server" ID="lblNamaSiswaInfo"></asp:Literal>
                    </span>
                    <br />
                    <span style="font-weight: normal; font-size: small; color: grey;">
                        <asp:Literal runat="server" ID="lblKelasSiswaInfo"></asp:Literal>
                    </span>
                    &nbsp;&nbsp;
                    <span style="font-weight: normal; font-size: small; color: grey;">
                        <asp:Literal runat="server" ID="lblInfoPeriode"></asp:Literal>
                    </span>
                    <hr style="margin: -15px; margin-top: 10px; margin-bottom: 10px;" />
                    <label id="lbl_nomor" style="color: grey;"></label>
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
                                <a onclick="HideModal();" class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Tutup</a>
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_pengaturan" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                        Pengaturan
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
                                                        <label class="label-input" for="<%= txtNamaGuru.ClientID %>" style="text-transform: none;">Nama Guru</label>
                                                        <asp:TextBox CssClass="form-control" runat="server" ID="txtNamaGuru"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>     
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtTanggalLTS.ClientID %>" style="text-transform: none;">Tanggal LTS</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputPengaturan" runat="server" ID="vldTanggalLTS"
                                                            ControlToValidate="txtTanggalLTS" Display="Dynamic" style="float: right; font-weight: bold;"
                                                            Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ValidationGroup="vldInputPengaturan" CssClass="form-control" runat="server" ID="txtTanggalLTS"></asp:TextBox>
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
                                <asp:LinkButton ValidationGroup="vldInputPengaturan" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKPengaturan" OnClick="lnkOKPengaturan_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a onclick="HideModal();" class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>

				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_proses" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
			        <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
				        <div class="modal-inner" 
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; 
                            padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; 
                            border-top-left-radius: 5px;
                            border-top-right-radius: 5px;
                            background-color: #F68B1F; 
                            background-repeat: no-repeat;
                            background-size: auto;
                            background-position: right; 
                            background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: white; padding-bottom: 20px;">
							    <div class="media-object margin-right-sm pull-left">
								    <span class="icon icon-lg text-brand-accent" style="color: white;">info_outline</span>
							    </div>
							    <div class="media-inner">
								    <span style="font-weight: bold;">
                                        Proses
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%; display: none;">
							    <div class="row">
                                    <div class="col-lg-12">

                                    </div>
                                </div>								                            
							</div>
				        </div>
				        <div class="modal-footer">
					        
                            <div style="width: 100%;">
							    <div class="row" id="pb_proses" style="margin-left: -24px; margin-right: -24px; background-color: #F68B1F; color: white; border-bottom-left-radius: 5px; border-bottom-right-radius: 5px;">
                                    <div class="col-lg-12" style="padding-left: 0px; padding-right: 0px;">
                                        <div class="progress" style="margin-top: 0px; margin-left: 20px; margin-right: 20px;">
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
                            </div>

				        </div>
			        </div>
		        </div>
	        </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnShowNilaiSiswa" />
            <asp:PostBackTrigger ControlID="lnkOKPengaturan" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpNomor2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
        InitPicker();
        ShowInfoLabel();
        InitModalFocus();
    </script>
</asp:Content>
