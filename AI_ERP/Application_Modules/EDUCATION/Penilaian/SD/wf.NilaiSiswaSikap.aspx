<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.NilaiSiswaSikap.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Penilaian.SD.wf_NilaiSiswaSikap" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- handsontable -->
    <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/handson-table/bower_components/handsontable/dist/handsontable.full.js") %>"></script>
    <link rel="stylesheet" media="screen" href="<%= ResolveUrl("~/Application_CLibs/handson-table/bower_components/handsontable/dist/handsontable.full.min.css") %>">
    <link rel="stylesheet" media="screen" href="<%= ResolveUrl("~/Application_CLibs/handson-table/css/samples.css") %>">

    <!-- ruleJS -->
    <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/handson-table/bower_components/ruleJS/dist/full/ruleJS.all.full.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/handson-table/src/handsontable.formula.js") %>"></script>
    <link rel="stylesheet" media="screen" href="<%= ResolveUrl("~/Application_CLibs/handson-table/src/handsontable.formula.css") %>">

    <script type="text/javascript">
        function HideModal() {
            $('#ui_modal_pilih_kd_lts').modal('hide');
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
            }
        }
    </script>

    <script type="text/javascript">
        var resizeTimer;

        function Resize() {
            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(function () {

                if (typeof Maximize === 'function') Maximize();

            }, 100);
        }

        function ResponseRedirect(url) {
            document.location.href = url;
        }
    </script>

    <style type="text/css">
        .zebraStyle > tbody > tr:nth-child(2n) > td {
            font-size: 9pt;
            vertical-align: middle;
        }

        .zebraStyle > tbody > tr:nth-child(2n+1) > td {
            font-size: 9pt;
            vertical-align: middle;
        }

        .zebraStyle > tbody > tr:nth-child(1) > td {
            font-size: 9pt;
            background: white;
            color: black;
            font-weight: bold;
            padding-left: 5px;
            padding-right: 5px;
            text-align: center;
            /*box-shadow: 1px 2px 1px #d7d7d7;*/
        }

        .zebraStyle > tbody > tr:nth-child(2) > td {
            font-size: 9pt;
            background: white;
            color: black;
            padding-left: 5px;
            padding-right: 5px;
            text-align: center;
            padding-top: 1px;
            padding-bottom: 1px;
            font-weight: bold;
        }

        .zebraStyle > tbody > tr:nth-child(3) > td {
            font-size: 9pt;
            background: white;
            font-weight: bold;
            padding-left: 5px;
            padding-right: 5px;
            text-align: center;
        }

        .handsontable col.rowHeader {
            width: 0px;
        }
    </style>

    <style type="text/css">
        .modal-backdrop {
            opacity: 0 !important;
        }

        /* Messy stack of paper */
        .paper {
            background: #fff;
            padding: 30px;
            position: relative;
        }

            .paper,
            .paper::before,
            .paper::after {
                /* Styles to distinguish sheets from one another */
                box-shadow: 1px 1px 1px rgba(0,0,0,0.25);
                border: 1px solid #bbb;
            }

                .paper::before,
                .paper::after {
                    content: "";
                    position: absolute;
                    height: 95%;
                    width: 99%;
                    background-color: #eee;
                }

                .paper::before {
                    right: 15px;
                    top: 0;
                    transform: rotate(-1deg);
                    z-index: -1;
                }

                .paper::after {
                    top: 5px;
                    right: -5px;
                    transform: rotate(1deg);
                    z-index: -2;
                }
    </style>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpNomor2" runat="server">
    <asp:HiddenField runat="server" ID="txtKKM" />
    <asp:HiddenField runat="server" ID="txtKTSPColKD" />
    <asp:HiddenField runat="server" ID="txtSemester" />
    <asp:HiddenField runat="server" ID="txtMapel" />

    <asp:Button UseSubmitBehavior="false" runat="server" ID="btnOKShowBySemester" OnClick="btnOKShowBySemester_Click" Style="position: absolute; left: -1000px; top: -1000px;" />

    <div id="div_nilai"
        style="overflow: hidden; position: absolute; top: 60px; bottom: 0px; left: 5px; right: 10px; font-size: small; color: grey;">
        <asp:Literal runat="server" ID="ltrCenter"></asp:Literal>        
    </div>

    <div id="div_statusbar" runat="server">
        <asp:Literal runat="server" ID="ltrStatusBar"></asp:Literal>
    </div>

    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="txtKeyAction" />
            <asp:HiddenField runat="server" ID="txtParseKD" />

            <div class="fbtn-container" id="div_button_settings" runat="server" style="z-index: 999999;">
                <div class="fbtn-inner">
                    <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #329CC3;" title=" Pengaturan ">
                        <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                        <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                    </a>
                    <div class="fbtn-dropup" style="z-index: 999999;">
                        <a data-toggle="modal" href="#ui_modal_pilihan" class="fbtn fbtn-green waves-attach waves-circle waves-effect" style="cursor: pointer; background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Pilih Data Nilai</span>
                            <i class="fa fa-list" style="color: white;"></i>
                        </a>
                        <asp:LinkButton Visible="false" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkPilihKelas" OnClick="lnkPilihKelas_Click" Style="background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Pilih Kelas Lain</span>
                            <i class="fa fa-th-large" style="color: white;"></i>
                        </asp:LinkButton>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_pilihan" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
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
                                            <div runat="server" id="div_unit_sekolah_biodata_siswa" class="row" style="padding-left: 0px; padding-right: 0px;">
                                                <div class="col-xs-12" style="margin-left: 0px; margin-right: 0px;">

                                                    <div class="card" style="margin-bottom: 0px; margin-top: 0px; box-shadow: none; border-style: none;">
                                                        <div class="card-main">
                                                            <nav class="tab-nav margin-top-no margin-bottom-no">
                                                                <ul class="nav nav-justified" style="background-color: #f1f1f1; <asp:Literal runat="server" ID="ltrHeaderTab"></asp:Literal>border-top-style: solid; border-top-width: 0px; border-top-color: #bfbfbf;">
                                                                    <li class="active">
                                                                        <a class="waves-attach" data-toggle="tab" href="#ui_tab_akademik" style="font-weight: bold; text-transform: none; color: black; line-height: 15px; padding-top: 15px; padding-bottom: 15px; color: white;">
                                                                            Nilai<br />Akademik
                                                                        </a>
                                                                    </li>
                                                                    <li runat="server" id="li_nilai_ekskul">
                                                                        <a class="waves-attach" data-toggle="tab" href="#ui_tab_ekskul" style="font-weight: bold; text-transform: none; color: black; line-height: 15px; padding-top: 15px; padding-bottom: 15px; color: white;">
                                                                            Ekstra<br />Kurikuler
                                                                        </a>
                                                                    </li>
                                                                    <li>
                                                                        <a class="waves-attach" data-toggle="tab" href="#ui_tab_sikap" style="font-weight: bold; text-transform: none; color: black; line-height: 15px; padding-top: 15px; padding-bottom: 15px; color: white;">
                                                                            Nilai<br />Sikap
                                                                        </a>
                                                                    </li>
                                                                    <li runat="server" id="li_nilai_volunteer">
                                                                        <a class="waves-attach" data-toggle="tab" href="#ui_tab_volunteer" style="font-weight: bold; text-transform: none; color: black; line-height: 15px; padding-top: 15px; padding-bottom: 15px; color: white;">
                                                                            Kegiatan<br />Volunteer
                                                                        </a>
                                                                    </li>
                                                                    <li runat="server" id="li_nilai_rapor">
                                                                        <a class="waves-attach" data-toggle="tab" href="#ui_tab_lts" style="font-weight: bold; text-transform: none; color: black; line-height: 15px; padding-top: 15px; padding-bottom: 15px; color: white;">
                                                                            Nilai<br />Rapor
                                                                        </a>
                                                                    </li>
                                                                </ul>
                                                            </nav>
                                                            <div class="card-inner" style="margin-left: 0px; margin-right: 0px; margin-top: 0px;">
                                                                <div class="tab-content">
                                                                    <div class="tab-pane fade active in" id="ui_tab_akademik">
                                                                        <asp:Literal runat="server" ID="ltrListNilaiAkademik"></asp:Literal>
                                                                    </div>
                                                                    <div class="tab-pane fade" id="ui_tab_ekskul">
                                                                        <asp:Literal runat="server" ID="ltrListNilaiEkskul"></asp:Literal>
                                                                    </div>
                                                                    <div class="tab-pane fade" id="ui_tab_sikap">
                                                                        <asp:Literal runat="server" ID="ltrListNilaiSikap"></asp:Literal>
                                                                    </div>
                                                                    <div class="tab-pane fade" id="ui_tab_volunteer">
                                                                        <asp:Literal runat="server" ID="ltrListNilaiVolunteer"></asp:Literal>
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
                        </div>
                        <div class="modal-footer">
                            <p class="text-center">
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Tutup</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <iframe name="fra_loader" id="fra_loader" height="0" width="0" style="position: absolute; left: -1000px; top: -1000px;"></iframe>

    <script type="text/javascript">
        setTimeout(function () {

            var txt = document.getElementById("<%= txtSemester.ClientID %>");
            if (txt != null && txt != undefined) {
                if (txt.value.trim() === "") {
                    $('#ui_modal_pilihan').modal({ show: true });
                }
            }

        }, 200);
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <asp:MultiView runat="server" ID="mvMain" ActiveViewIndex="0">
        <asp:View runat="server" ID="vNilaiSiswa">
            <asp:Literal runat="server" ID="ltrHOT"></asp:Literal>
        </asp:View>
    </asp:MultiView>
</asp:Content>
