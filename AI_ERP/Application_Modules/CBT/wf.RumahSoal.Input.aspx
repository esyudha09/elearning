<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="wf.RumahSoal.Input.aspx.cs" Inherits="AI_ERP.Application_Modules.CBT.wf_RumahSoal_Input" %>

<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        var currentValue = 0; 

        function loadCkEditorDeskripsi() {
            CKEDITOR.config.toolbar_Full =
                [
                    { name: 'document', items: ['Source'] },
                    { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', '-', 'Undo', 'Redo'] },
                    { name: 'editing', items: ['Find'] },
                    { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline'] },
                    { name: 'paragraph', items: ['indent','JustifyLeft', 'JustifyCenter', 'JustifyRight'] }
                ];
            CKEDITOR.config.height = '40px';
            CKEDITOR.config.removePlugins = 'maximize';
            CKEDITOR.config.removePlugins = 'resize';
            CKEDITOR.config.sharedSpaces = { top: 'toolbar1' };
            //CKEDITOR.config.extraPlugins = 'textindent';
            CKEDITOR.replace('<%= txtDeskripsi.ClientID %>', {
                extraPlugins: 'ckeditor_wiris,indentblock,justify,textindent',
                language: 'en',
                startupFocus: true,
                //indentation: "10px"
                
            });
           

            $("#<%=lnkOKInput.ClientID %>").click(function () {
              
                   CKEDITOR.instances["<%= txtDeskripsi.ClientID %>"].updateElement();
               });
         }

        function GoToURL(url) {
            document.location.href = url;
        }

        function HideModal() {
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
                    //ReInitTinyMCE();
                    window.scrollTo(0, 0);
                    break;
                case "<%= JenisAction.Add %>":
                    //ReInitTinyMCE();
                    //$('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.AddWithMessage %>":
                    //ReInitTinyMCE();
                    loadCkEditorDeskripsi();
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
                    loadCkEditorDeskripsi();
                    //$('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmHapus %>":
                    //ReInitTinyMCE();
                    //$('#ui_modal_confirm_hapus').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.Update %>":
                    //ReInitTinyMCE();
                    loadCkEditorDeskripsi();
                    //HideModal();
                    break;
                case "<%= JenisAction.Delete %>":
                    //ReInitTinyMCE();

                    break;
                case "<%= JenisAction.DoAdd %>":
                    //ReInitTinyMCE();
                    loadCkEditorDeskripsi();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoUpdate %>":
                    //ReInitTinyMCE();
                    loadCkEditorDeskripsi();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah diupdate',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoDelete %>":
                    //ReInitTinyMCE();
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
        }

        function InitModalFocus() {
           <%-- $('#ui_modal_input_data').on('shown.bs.modal', function () {
                if (document.getElementById("<%= txtSoal.ClientID %>") !== undefined && document.getElementById("<%= txtSoal.ClientID %>") !== null) {
                    document.getElementById("<%= txtJawaban.ClientID %>").focus();
                }
                else {
                    document.getElementById("<%= txtSoal.ClientID %>").focus();
                }
            });--%>
        }


        function DoInitPicker(id) {
            $('#' + id).pickdate({ cancel: '', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });
        }

        function InitPicker() {
            DoInitPicker('<%= txtStartDate.ClientID %>');
            DoInitPicker('<%= txtEndDate.ClientID %>');

        }

        <%--function TriggerSave() {
            tinyMCE.triggerSave();
            document.getElementById("<%= btnSoal.ClientID %>").style.display = 'block';
        }--%>

       
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
            <asp:HiddenField runat="server" ID="txtNamaVal" />
            <asp:HiddenField runat="server" ID="txtDeskripsiVal" />


            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnShowConfirmDelete" OnClick="btnShowConfirmDelete_Click" Style="position: absolute; left: -1000px; top: -1000px;" />

            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-12">
                    <div class="col-md-8 col-md-offset-2" style="padding: 0px;">
                        <div class="card" style="margin-top: 40px;">
                            <div class="card-main">
                                <div class="card-header" style="background-color: #295BC8; padding: 10px; font-weight: bold; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                    <span>FORM RUMAH SOAL</span>

                                </div>
                                <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px;">


                                    <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                        <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                            <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                <label class="label-input" for="<%= txtNama.ClientID %>" style="text-transform: none;">Nama :</label>
                                                <asp:TextBox ValidationGroup="vldInput" CssClass="form-control " runat="server" ID="txtNama"></asp:TextBox>

                                            </div>
                                        </div>
                                    </div>



                                    <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                        <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                            <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                <label class="label-input" for="<%= txtDeskripsi.ClientID %>" style="text-transform: none;">Deskripsi :</label>
                                                <asp:TextBox contenteditable="true" id="txtDeskripsi" ValidationGroup="vldInput" CssClass="form-control " runat="server" TextMode="MultiLine" Height="200px"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                        <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">

                                            <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">


                                                <div class="col-xs-8">
                                                    <label class="label-input" for="<%= txtStartDate.ClientID %>" style="text-transform: none;">Tanggal Mulai :</label>
                                                    <asp:TextBox ValidationGroup="vldInput" CssClass="form-control col-xs-10" runat="server" ID="txtStartDate"></asp:TextBox>
                                                </div>
                                                <div class="col-xs-2">
                                                    <label class="label-input" for="<%= cboStartJam.ClientID %>" style="text-transform: none;">Jam</label>
                                                    <asp:DropDownList runat="server" ID="cboStartJam" CssClass="form-control  col-xs-2">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-xs-2">
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
                                                <div class="col-xs-2">
                                                    <label class="label-input" for="<%= cboEndJam.ClientID %>" style="text-transform: none;">Jam</label>
                                                    <asp:DropDownList runat="server" ID="cboEndJam" CssClass="form-control  col-xs-2">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-xs-2">
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
                                    </div>

                                    

                                    <p class="text-right">
                                        <asp:LinkButton OnClientClick="TriggerSave()" ValidationGroup="vldInput" CssClass="btn btn-brand margin-right-sm" runat="server" ID="lnkOKInput" OnClick="lnkOKInput_Click" Text="Simpan"></asp:LinkButton>

                                        <%--<asp:LinkButton OnClientClick="TriggerSave()" ValidationGroup="vldInput" CssClass="btn btn-brand margin-right-sm" runat="server" ID="LinkButton1" OnClick="btnSoal_Click" Text="Soal"></asp:LinkButton>--%>
                                        <br />
                                        <br />
                                    </p>

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
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">Konfirmasi
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapus" OnClick="lnkOKHapus_Click" Text="  Hapus "></asp:LinkButton>
                                <%--<a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>--%>
                                <br />
                                <br />
                            </p>
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
                    <div class="fbtn-dropup" style="z-index: 999999;">

                        <asp:LinkButton runat="server" ID="btnSoal" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" Style="background-color: #257228; display: none" OnClick="btnSoal_Click">
                                                            <span class="fbtn-text fbtn-text-left">Pengaturan Soal</span>
                                                            <i class="fa fa-plus" style="color: white;"></i>
                        </asp:LinkButton>
                        <%--<asp:LinkButton ToolTip=" Kembali " runat="server" ID="btnBack" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" Style="background-color: #257228;" OnClick="btnBackToMenu_Click">
                                                            <span class="fbtn-text fbtn-text-left">Kembali</span>
                                                            <i class="fa fa-arrow-left"></i>
                        </asp:LinkButton>--%>
                    </div>

                </div>
            </div>

            <div class="content-header ui-content-header"
                style="background-color: #00198d; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 33px; right: 50px; width: 260px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                <div style="padding-left: 0px;">
                    <asp:LinkButton ToolTip=" Kembali " runat="server" ID="LinkButton1"
                        OnClick="btnBackToMapel_Click"
                        CssClass="btn-trans waves-attach waves-circle waves-effect" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;Mata Pelajaran
                    </asp:LinkButton>
                </div>
            </div>
            <div class="content-header ui-content-header"
                style="background-color: red; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 6; position: fixed; bottom: 33px; right: 50px; width: 110px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                <div style="padding-left: 0px;">
                    <asp:LinkButton ToolTip=" Kembali " runat="server" ID="LinkButton2"
                        OnClick="btnBackToKelas_Click"
                        CssClass="btn-trans waves-attach waves-circle waves-effect" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;Kelas
                    </asp:LinkButton>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
  
        loadCkEditorDeskripsi();

      
        InitPicker();
    </script>

    <script type="text/javascript">
        RenderDropDownOnTables();
        InitModalFocus();
        //DoAutoSave();
    </script>

   
     

</asp:Content>
