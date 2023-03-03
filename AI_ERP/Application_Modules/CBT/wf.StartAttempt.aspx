<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.StartAttempt.aspx.cs" Inherits="AI_ERP.Application_Modules.CBT.wf_StartAttempt" %>

<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function HideModal() {
            $('#ui_modal_input_data').modal('hide');
            $('#ui_modal_confirm_hapus').modal('hide');

            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();

            document.body.style.paddingRight = "0px";
        }

        function GoToURL(url) {
            document.location.href = url;
        }

        function EndRequestHandler() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        }

        function EndRequest() {
            alert("asdf")
            var jenis_act = document.getElementById("<%= txtKeyAction.ClientID %>").value;

            switch (jenis_act) {
                case "<%= JenisAction.DoChangePage %>":
                    window.scrollTo(0, 0);
                    break;
                case "<%= JenisAction.Add %>":
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

        <%--function InitModalFocus() {
            $('#ui_modal_input_data').on('shown.bs.modal', function () {
                if (document.getElementById("<%= cboUnit.ClientID %>") !== undefined && document.getElementById("<%= cboUnit.ClientID %>") !== null) {
                    document.getElementById("<%= cboUnit.ClientID %>").focus();
                }
                else {
                    document.getElementById("<%= txtNama.ClientID %>").focus();
                }
            });
        }--%>

        function TriggerSave() {
            tinyMCE.triggerSave();
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

            <%--<asp:Button runat="server" UseSubmitBehavior="false" ID="btnDoCari" OnClick="btnDoCari_Click" Style="position: absolute; left: -1000px; top: -1000px;" />--%>
            <%--<asp:Button runat="server" UseSubmitBehavior="false" ID="btnShowDetail" OnClick="btnShowDetail_Click" Style="position: absolute; left: -1000px; top: -1000px;" />--%>
            <%--<asp:Button runat="server" UseSubmitBehavior="false" ID="btnShowConfirmDelete" OnClick="btnShowConfirmDelete_Click" Style="position: absolute; left: -1000px; top: -1000px;" />--%>

            <div class="card" style="margin-top: 10px; border-radius: 5px; border-style: solid; border-width: 1px; border-color: #dddddd; box-shadow: none;">
                <div class="card-main" style="background: url(); background-position: bottom right; background-repeat: no-repeat;">
                    <div class="card-action" style="background: url(/Application_CLibs/images/kelas/b.png); background-position: top right; background-color: #0AC6AE; padding-left: 20px; padding-right: 20px; border-top-left-radius: 6px; border-top-right-radius: 6px; background-repeat: no-repeat; margin-left: -1px; margin-top: -1px; margin-right: -1px;">
                        <p class="card-heading" style="margin-bottom: 0px; margin-top: 15px; color: white;">
                            <span style="font-weight: bold; color: white; font-weight: bold; font-size: larger;"><asp:Literal ID="txtMapel" runat="server"></asp:Literal> - <asp:Literal ID="txtKelas" runat="server"></asp:Literal></span>                        
                        </p>
                        <div style="font-size: medium; color: white;">
                            <asp:Literal ID="txtTahunAjaran" runat="server"></asp:Literal> - <asp:Literal ID="txtSemester" runat="server"></asp:Literal>
                        </div>
                        <div style="font-size: medium; color: white; font-size: small; font-weight: bold; color: yellow;">
                            <label class="badge" style="margin-top: 10px; margin-left: -5px;">
                                <asp:Literal ID="txtNamaKP" runat="server"></asp:Literal></label>
                        </div>
                        <p></p>
                    </div>
                    <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px;">


                        <div class="row" style="margin-left: 30px; margin-right: 30px;">
                        </div>



                        <div class="row" style="margin-left: 30px; margin-right: 30px;">
                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                    <asp:Literal ID="txtDeskripsi" runat="server">Deskripsi :</asp:Literal>

                                </div>
                            </div>
                        </div>

                        <%-- <div class="row" style="margin-left: 30px; margin-right: 30px;">
                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">

                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">


                                    <div class="col-xs-8">
                                        <label class="label-input" for="<%= txtStartDate.ClientID %>" style="text-transform: none;">Tanggal Mulai :</label>
                                        <asp:TextBox ValidationGroup="vldInput" CssClass="form-control col-xs-10" runat="server" ID="txtStartDate"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-1">
                                        <label class="label-input" for="<%= cboStartJam.ClientID %>" style="text-transform: none;">Jam</label>
                                        <asp:DropDownList runat="server" ID="cboStartJam" CssClass="form-control  col-xs-2">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-xs-1">
                                        <label class="label-input" for="<%= cboStartMenit.ClientID %>" style="text-transform: none;">Menit</label>
                                        <asp:DropDownList runat="server" ID="cboStartMenit" CssClass="form-control  col-xs-2">
                                        </asp:DropDownList>
                                    </div>

                                </div>
                            </div>
                        </div>


                        <div class="row" style="margin-left: 30px; margin-right: 30px;">
                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">

                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">


                                    <div class="col-xs-8">
                                        <label class="label-input" for="<%= txtEndDate.ClientID %>" style="text-transform: none;">Tanggal Selesai :</label>
                                        <asp:TextBox ValidationGroup="vldInput" CssClass="form-control col-xs-10" runat="server" ID="txtEndDate"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-1">
                                        <label class="label-input" for="<%= cboEndJam.ClientID %>" style="text-transform: none;">Jam</label>
                                        <asp:DropDownList runat="server" ID="cboEndJam" CssClass="form-control  col-xs-2">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-xs-1">
                                        <label class="label-input" for="<%= cboEndMenit.ClientID %>" style="text-transform: none;">Menit</label>
                                        <asp:DropDownList runat="server" ID="cboEndMenit" CssClass="form-control  col-xs-2">
                                        </asp:DropDownList>
                                    </div>

                                </div>
                            </div>
                        </div>

                        <div class="row" style="margin-left: 30px; margin-right: 30px;">
                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">

                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                    <div class="col-xs-12">
                                        <label class="label-input" for="<%= txtTimeLimit.ClientID %>" style="text-transform: none;">Batas Waktu :</label>
                                    </div>
                                    <div class="col-xs-2">

                                        <asp:TextBox ValidationGroup="vldInput" CssClass="form-control col-xs-10" runat="server" ID="txtTimeLimit"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-2">
                                        <asp:DropDownList runat="server" ID="cboLimitSatuan" CssClass="form-control  col-xs-2">
                                        </asp:DropDownList>
                                    </div>


                                </div>
                            </div>
                        </div>--%>
                    </div>
                    <div class="card-action" style="padding-left: 10px; padding-right: 10px;">
                        <div class="card-action-btn pull-left" style="margin-left: 0px; margin-right: 0px; color: grey;">
                            <div class="tooltip">
                                <a class="btn btn-flat waves-attach waves-effect" href="/e/xMzY4ODda/tl?t=99689637&amp;ft=Yi5wbmc=&amp;kd=f9d0d215-a4cb-47f6-aa52-8a303d69e6bf" style="margin-left: 0px; color: #8f8f8f; font-weight: bold; text-transform: none; padding-top: 5px; padding-bottom: 5px; margin-top: 2px;"><i class="fa fa-folder-open"></i></a>
                                <div class="top">Buka<i></i></div>
                            </div>
                        </div>
                        <div class="card-action-btn pull-left" style="margin-left: 0px; padding-left: 0px; margin-right: 0px; color: grey;">
                            <div class="tooltip">
                                <a class="btn btn-flat waves-attach waves-effect" href="/l/93notypjdshkljfs/kp?t=99689637&amp;ft=Yi5wbmc=&amp;kd=f9d0d215-a4cb-47f6-aa52-8a303d69e6bf" style="margin-left: 0px; color: #8f8f8f; font-weight: bold; text-transform: none; padding-top: 5px; padding-bottom: 5px; margin-top: 2px;"><i class="fa fa-address-book-o"></i></a>
                                <div class="top">Perpustakaan<i></i></div>
                            </div>
                        </div>
                        <div class="card-action-btn pull-left" style="margin-left: 0px; padding-left: 0px; margin-right: 0px; color: grey;">
                            <div class="tooltip">
                                <a class="btn btn-flat waves-attach waves-effect" href="/e/zcgfu16/vs?t=99689637&amp;ft=Yi5wbmc=&amp;s=2&amp;kd=f9d0d215-a4cb-47f6-aa52-8a303d69e6bf" style="margin-left: 0px; color: #8f8f8f; font-weight: bold; text-transform: none; padding-top: 5px; padding-bottom: 5px; margin-top: 2px;"><i class="fa fa-handshake-o"></i></a>
                                <div class="top">Volunteer<i></i></div>
                            </div>
                        </div>
                        <div class="card-action-btn pull-left" style="margin-left: 0px; padding-left: 0px; margin-right: 0px; color: grey;">
                            <div class="tooltip">
                                <a class="btn btn-flat waves-attach waves-effect" href="/e/zcgfu17/csw?t=99689637&amp;ft=Yi5wbmc=&amp;s=2&amp;kd=f9d0d215-a4cb-47f6-aa52-8a303d69e6bf" style="margin-left: 0px; color: #8f8f8f; font-weight: bold; text-transform: none; padding-top: 5px; padding-bottom: 5px; margin-top: 2px;"><i class="fa fa-edit"></i></a>
                                <div class="top">Catatan Wali Kelas<i></i></div>
                            </div>
                        </div>
                        <div class="card-action-btn pull-left" style="margin-left: 0px; padding-left: 0px; margin-right: 0px; color: grey;">
                            <div class="tooltip">
                                <label class="btn btn-flat waves-attach waves-effect" onclick="document.location.href='/e/las-NdYWuo9OFAw?kd=f9d0d215-a4cb-47f6-aa52-8a303d69e6bf';" style="cursor: pointer; margin-left: 0px; color: #8f8f8f; font-weight: bold; text-transform: none; padding-top: 5px; padding-bottom: 5px; margin-top: 2px;"><i class="fa fa-file-text-o"></i></label>
                                <div class="top">Laporan Presensi Siswa<i></i></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnRefresh" />--%>
            <%--<asp:PostBackTrigger ControlID="btnDoCari" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
        RenderDropDownOnTables();
        InitModalFocus();
    </script>
</asp:Content>
