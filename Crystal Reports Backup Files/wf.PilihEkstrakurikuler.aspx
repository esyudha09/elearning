<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.PilihEkstrakurikuler.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Penilaian.SD.wf_PilihEkstrakurikuler" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function HideModal() {
            $('#ui_modal_mapel_ekskul').modal('hide');
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
                        alive: 1000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah diupdate',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.ShowPilihanEkskul %>":
                    $('#ui_modal_mapel_ekskul').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.AdaEkskul %>":
                    HideModal();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;&nbsp;PERHATIAN : Ekstrakurikuler sudah dipilih',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    document.getElementById(document.getElementById("<%= txtIDCtlEkskul.ClientID %>").value).value = 
                        document.getElementById("<%= txtPilihEkskulAwal.ClientID %>").value;
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

            Sys.Browser.WebKit = {};
            if (navigator.userAgent.indexOf('WebKit/') > -1) {
                Sys.Browser.agent = Sys.Browser.WebKit;
                Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
                Sys.Browser.name = 'WebKit';
            }
        }

        function ShowPilihEkskul(nomor_ekskul, kode_pilih, rel_siswa, kode_mapel) {
            var txt_nomor_pilih_ekskul = document.getElementById("<%= txtNomorEkskul.ClientID %>");
            var txt_kode_pilih_ekskul = document.getElementById("<%= txtKodePilihEkskul.ClientID %>");
            var txt_kode_pilih_siswa = document.getElementById("<%= txtRelSiswa.ClientID %>");
            var txt_kode_mapel = document.getElementById("<%= txtKodeMapel.ClientID %>");

            if (txt_nomor_pilih_ekskul !== null && txt_nomor_pilih_ekskul !== undefined) {
                txt_nomor_pilih_ekskul.value = nomor_ekskul;
            }
            if (txt_kode_pilih_ekskul !== null && txt_kode_pilih_ekskul !== undefined) {
                txt_kode_pilih_ekskul.value = kode_pilih;
            }
            if (txt_kode_pilih_siswa !== null && txt_kode_pilih_siswa !== undefined) {
                txt_kode_pilih_siswa.value = rel_siswa;
            }
            if (txt_kode_mapel !== null && txt_kode_mapel !== undefined) {
                txt_kode_mapel.value = kode_mapel;
            }

            var arr_mapel = document.getElementsByName("arr_mapel[]");
            if (arr_mapel.length > 0) {
                for (var i = 0; i < arr_mapel.length; i++) {
                    if (kode_mapel === arr_mapel[i].id) {
                        arr_mapel[i].style.display = "";
                    }
                    else {
                        arr_mapel[i].style.display = "none";
                    }
                }
            }

            $('#ui_modal_mapel_ekskul').modal({ backdrop: 'static', keyboard: false, show: true });
        }

        function DoPilihEkskul(kode_mapel, nama_mapel) {
            var txt_kode_mapel = document.getElementById("<%= txtKodePilihMapel.ClientID %>");
            var txt_nomor_pilih_ekskul = document.getElementById("<%= txtNomorEkskul.ClientID %>");
            var btn_save = document.getElementById("<%= btnOKSavePilihEkskul.ClientID %>");
            var txt_kode_pilih_ekskul = document.getElementById("<%= txtKodePilihEkskul.ClientID %>");
            var label_mapel = document.getElementById("label_" + txt_kode_pilih_ekskul.value + "_" + txt_nomor_pilih_ekskul.value);
            var div_mapel = document.getElementById("div_" + txt_kode_pilih_ekskul.value + "_" + txt_nomor_pilih_ekskul.value);
            var div_mapel_1 = document.getElementById("div_" + txt_kode_pilih_ekskul.value + "_1");
            var div_mapel_2 = document.getElementById("div_" + txt_kode_pilih_ekskul.value + "_2");
            var div_mapel_3 = document.getElementById("div_" + txt_kode_pilih_ekskul.value + "_3");
            var div_mapel_4 = document.getElementById("div_" + txt_kode_pilih_ekskul.value + "_4");

            if (txt_kode_mapel !== null && txt_kode_mapel !== undefined) {
                txt_kode_mapel.value = kode_mapel;
            }

            if (btn_save !== null && btn_save !== undefined) {
                btn_save.click();
                HideModal();

                var valid = true;
                switch (txt_nomor_pilih_ekskul.value) {
                    case "1":
                        if (
                            kode_mapel === div_mapel_2.lang ||
                            kode_mapel === div_mapel_3.lang ||
                            kode_mapel === div_mapel_4.lang
                          ) valid = false;
                        break;
                    case "2":
                        if (
                            kode_mapel === div_mapel_1.lang ||
                            kode_mapel === div_mapel_3.lang ||
                            kode_mapel === div_mapel_4.lang
                          ) valid = false;
                        break;
                    case "3":
                        if (
                            kode_mapel === div_mapel_1.lang ||
                            kode_mapel === div_mapel_2.lang ||
                            kode_mapel === div_mapel_4.lang
                          ) valid = false;
                        break;
                }

                if (valid) {
                    if (kode_mapel.trim() === "") {
                        label_mapel.innerHTML = "&nbsp;&nbsp;" +
                                                "Pilih Ekstrakurikuler <span class=\"badge\" style=\"cursor: pointer; background-color: #dcdcdc;\">" + txt_nomor_pilih_ekskul.value + "</span>";
                    }
                    else {
                        label_mapel.innerHTML = "&nbsp;&nbsp;" +
                                                "<span style=\"color: black; font-weight: bold;\">" +
                                                    nama_mapel +
                                                "</span>";
                    }

                    if (div_mapel !== null && div_mapel !== undefined) {
                        div_mapel.lang = kode_mapel;
                    }
                }
                else {
                    if (kode_mapel.trim() === "") {
                        label_mapel.innerHTML = "&nbsp;&nbsp;" +
                                                "Pilih Ekstrakurikuler <span class=\"badge\" style=\"cursor: pointer; background-color: #dcdcdc;\">" + txt_nomor_pilih_ekskul.value + "</span>";

                        if (div_mapel !== null && div_mapel !== undefined) {
                            div_mapel.lang = kode_mapel;
                        }
                    }
                }
            }
        }

        function ResponseRedirect(url) {
            document.location.href = url;
        }

        function SetPilihMatpelEkskul(ctl_name, rel_siswa, id_ctl) {
            var txt_ekskul1 = document.getElementById("<%= txtPilihEkskul1.ClientID %>");
            var txt_ekskul2 = document.getElementById("<%= txtPilihEkskul2.ClientID %>");
            var txt_ekskul3 = document.getElementById("<%= txtPilihEkskul3.ClientID %>");
            var txt_ekskul4 = document.getElementById("<%= txtPilihEkskul4.ClientID %>");

            var arr_ekskul = document.getElementsByName(ctl_name);
            for (var i = 0; i < arr_ekskul.length; i++) {
                if (i === 0) {
                    txt_ekskul1.value = arr_ekskul[i].value;
                }
                else if (i === 1) {
                    txt_ekskul2.value = arr_ekskul[i].value;
                }
                else if (i === 2) {
                    txt_ekskul3.value = arr_ekskul[i].value;
                }
                else if (i === 3) {
                    txt_ekskul4.value = arr_ekskul[i].value;
                }
            }

            document.getElementById("<%= txtRelSiswa.ClientID %>").value = rel_siswa;
            document.getElementById("<%= txtIDCtlEkskul.ClientID %>").value = id_ctl;
            document.getElementById("<%= btnOKSavePilihEkskul.ClientID %>").click();
        }

        function SetNilaiAwalMapelEkskul(rel_mapel) {
            document.getElementById("<%= txtPilihEkskulAwal.ClientID %>").value = rel_mapel;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="upMain">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="txtID" />
            <asp:HiddenField runat="server" ID="txtKeyAction" />
            <asp:HiddenField runat="server" ID="txtCatatanVal" />
            <asp:HiddenField runat="server" ID="txtIDCatatan" />
            <asp:HiddenField runat="server" ID="txtKodePilihEkskul" />
            <asp:HiddenField runat="server" ID="txtKodePilihMapel" />
            <asp:HiddenField runat="server" ID="txtKodeMapel" />
            <asp:HiddenField runat="server" ID="txtNomorEkskul" />
            <asp:HiddenField runat="server" ID="txtRelSiswa" />

            <asp:HiddenField runat="server" ID="txtIDCtlEkskul" />
            <asp:HiddenField runat="server" ID="txtPilihEkskulAwal" />
            <asp:HiddenField runat="server" ID="txtPilihEkskul1" />
            <asp:HiddenField runat="server" ID="txtPilihEkskul2" />
            <asp:HiddenField runat="server" ID="txtPilihEkskul3" />
            <asp:HiddenField runat="server" ID="txtPilihEkskul4" />

            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnOKSavePilihEkskul" OnClick="btnOKSavePilihEkskul_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
        </ContentTemplate>
    </asp:UpdatePanel>

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
                                                <asp:Literal runat="server" ID="ltrCaptionMain"></asp:Literal>
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

            <div aria-hidden="true" class="modal fade" id="ui_modal_mapel_ekskul" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                        Pilih Ekstrakurikuler
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

                                                            <asp:Literal runat="server" ID="ltrMapelEkskul"></asp:Literal>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <hr style="margin: 0px;" />

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

            <div class="fbtn-container" id="div_button_settings" runat="server" style="z-index: 99;">
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

        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpNomor2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="KontenBawah" runat="server">
</asp:Content>
