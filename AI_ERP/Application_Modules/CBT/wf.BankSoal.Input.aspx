<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="wf.BankSoal.Input.aspx.cs" Inherits="AI_ERP.Application_Modules.CBT.wf_BankSoal_Input" %>

<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        var currentValue = 0;

        function JenisCheck() {

            if (document.getElementById("<%= cboJenis.ClientID %>").value == 'essay') {
                document.getElementById("<%= EssayDiv.ClientID %>").style.display = 'block';
                document.getElementById("<%= GandaDiv.ClientID %>").style.display = 'none';
            } else if (document.getElementById("<%= cboJenis.ClientID %>").value == 'ganda') {
                document.getElementById("<%= EssayDiv.ClientID %>").style.display = 'none';
                document.getElementById("<%= GandaDiv.ClientID %>").style.display = 'block';
            }
        }




        function loadCkEditor() {
            CKEDITOR.config.toolbar_Full =
                [
                    { name: 'document', items: ['Source'] },
                    { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', '-', 'Undo', 'Redo'] },
                    { name: 'editing', items: ['Find'] },
                    { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline'] },
                    { name: 'paragraph', items: ['JustifyLeft', 'JustifyCenter', 'JustifyRight'] }
                ];
            CKEDITOR.config.height = '40px';
            CKEDITOR.config.removePlugins = 'maximize';
            CKEDITOR.config.removePlugins = 'resize';
            CKEDITOR.config.sharedSpaces = { top: 'toolbar1' };
            CKEDITOR.config.pasteFilter = 'p; a[!href]';
            //CKEDITOR.config.imageUploadUrl = '/uploader/upload.php?type=Images';
            CKEDITOR.replace('<%= txtSoal.ClientID %>', {
                extraPlugins: 'ckeditor_wiris,indentblock,justify,textindent,uploadimage',
                language: 'en',
                startupFocus: true
            });
            CKEDITOR.replace('<%= txtJwbEssay.ClientID %>', {
                extraPlugins: 'ckeditor_wiris,indentblock,justify,textindent',
                language: 'en',

            });
            CKEDITOR.replace('<%= txtJwbGanda1.ClientID %>', {
                extraPlugins: 'ckeditor_wiris,indentblock,justify,textindent',
                language: 'en',

            });
            CKEDITOR.replace('<%= txtJwbGanda2.ClientID %>', {
                extraPlugins: 'ckeditor_wiris,indentblock,justify,textindent',
                language: 'en',

            });
            CKEDITOR.replace('<%= txtJwbGanda3.ClientID %>', {
                extraPlugins: 'ckeditor_wiris,indentblock,justify,textindent',
                language: 'en',

            });
            CKEDITOR.replace('<%= txtJwbGanda4.ClientID %>', {
                extraPlugins: 'ckeditor_wiris,indentblock,justify,textindent',
                language: 'en',

            });
            CKEDITOR.replace('<%= txtJwbGanda5.ClientID %>', {
                extraPlugins: 'ckeditor_wiris,indentblock,justify,textindent',
                language: 'en',

            });


            $("#<%=lnkOKInput.ClientID %>").click(function () {

                CKEDITOR.instances["<%= txtSoal.ClientID %>"].updateElement();
                CKEDITOR.instances["<%= txtJwbEssay.ClientID %>"].updateElement();
                CKEDITOR.instances["<%= txtJwbGanda1.ClientID %>"].updateElement();
                CKEDITOR.instances["<%= txtJwbGanda2.ClientID %>"].updateElement();
                CKEDITOR.instances["<%= txtJwbGanda3.ClientID %>"].updateElement();
                CKEDITOR.instances["<%= txtJwbGanda4.ClientID %>"].updateElement();
                CKEDITOR.instances["<%= txtJwbGanda5.ClientID %>"].updateElement();

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
                    loadCkEditor();
                    window.scrollTo(0, 0);
                    break;
                case "<%= JenisAction.Add %>":
                    loadCkEditor();
                    JenisCheck();
                    //$('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.AddWithMessage %>":
                    loadCkEditor();
                    //ReInitTinyMCE();
                    JenisCheck();
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
                    loadCkEditor();
                    //$('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmHapus %>":
                    loadCkEditor();
                    //$('#ui_modal_confirm_hapus').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.Update %>":
                    loadCkEditor();
                    JenisCheck();
                    //HideModal();
                    break;
                case "<%= JenisAction.Delete %>":
                    //ReInitTinyMCE();
                    loadCkEditor();
                    break;
                case "<%= JenisAction.DoAdd %>":
                    loadCkEditor();
                    //ReInitTinyMCE();

                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoUpdate %>":
                    loadCkEditor();
                    //ReInitTinyMCE();
                    //HideModal();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah diupdate',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoDelete %>":
                    loadCkEditor();
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

        function InitModalFocus() {
           <%-- $('#ui_modal_input_data').on('shown.bs.modal', function () {
                if (document.getElementById("<%= txtSoal.ClientID %>") !== undefined && document.getElementById("<%= txtSoal.ClientID %>") !== null) {
                    document.getElementById("<%= txtJawaban.ClientID %>").focus();
                }
                else {
                    document.getElementById(imgFile).focus();
                }
            });--%>
        }

        function ImageChange(input) {
            if (input.files[0].type == "image/png") {
                if (input.files && input.files[0]) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $("#<%=Image1.ClientID%>").prop('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                }
            }
        };
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
            

            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnShowConfirmDelete" OnClick="btnShowConfirmDelete_Click" Style="position: absolute; left: -1000px; top: -1000px;" />

            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-12">
                    <div class="col-md-8 col-md-offset-2" style="padding: 0px;">
                        <div class="card" style="margin-top: 40px;">
                            <div class="card-main">
                                <div class="card-header" style="background-color: #295BC8; padding: 10px; font-weight: bold; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                    <span>FORM SOAL</span>

                                </div>
                                <div class="card-inner">
                                    <div class="text-right">
                                        <asp:LinkButton OnClientClick="TriggerSave()" CssClass="btn btn-brand" runat="server" ID="lnkOKInput" OnClick="lnkOKInput_Click" Text="Simpan"></asp:LinkButton>
                                    </div>
                                    <div class="col-md-12 row">
                                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">

                                            <label for="<%= txtSoal.ClientID %>" style="color: #B7770D; font-size: small;">
                                                NAMA :
                                            </label>
                                            <asp:TextBox contenteditable="true" CssClass="form-control " runat="server" ID="txtNama"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6 row">
                                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                            <label for="<%= cboJenis.ClientID %>" style="color: #B7770D; font-size: small;">
                                                ASPEK PENILAIAN :
                                            </label>
                                            <asp:DropDownList runat="server" ID="cboAP" CssClass="input-box">
                                                <asp:ListItem Text="-Pilih Aspek Penilaian-" Value=""></asp:ListItem>
                                            </asp:DropDownList>

                                        </div>
                                    </div>
                                    <div class="col-md-12 row">
                                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                              <label for="<%= cboJenis.ClientID %>" style="color: #B7770D; font-size: small;">
                                                FILE MEDIA (png/jpg/jpeg/mp3/mp4) :
                                            </label>
                                       <%--     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>--%>
                                                    <asp:FileUpload ID="FileUpload1" runat="server" onchange="ImageChange(this)"></asp:FileUpload>
                                                    <%--<asp:Button ID="btnUpload" runat="server" OnClick="lnkOKInput_Click" Text="Upload" />--%>
                                               <%-- </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnUpload" />
                                                </Triggers>
                                            </asp:UpdatePanel>--%>
                                            <asp:Image ID="Image1" runat="server" Width="300" />
                                           
                                        
                                        </div>
                                    </div>
                                    <div class="col-md-12 row">
                                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">

                                            <label for="<%= txtSoal.ClientID %>" style="color: #B7770D; font-size: small;">
                                                SOAL :
                                            </label>
                                            <asp:TextBox contenteditable="true" CssClass="form-control " runat="server" ID="txtSoal" TextMode="MultiLine" Height="200px"></asp:TextBox>
                                        </div>
                                    </div>


                                    <div class="col-md-3 row">
                                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                            <label for="<%= cboJenis.ClientID %>" style="color: #B7770D; font-size: small;">
                                                JENIS SOAL :
                                            </label>
                                            <asp:DropDownList runat="server" ID="cboJenis" CssClass="input-box" onchange="JenisCheck(this);">
                                                <asp:ListItem Text="-Pilih Tipe Soal-" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Pilihan Ganda" Value="ganda"></asp:ListItem>
                                                <asp:ListItem Text="Essay" Value="essay"></asp:ListItem>

                                            </asp:DropDownList>

                                        </div>
                                    </div>

                                    <div class="col-md-12 row" runat="server" id="EssayDiv" style="display: none">
                                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">

                                            <label for="<%= txtJwbEssay.ClientID %>" style="color: #B7770D; font-size: small;">
                                                JAWABAN ESSAY :
                                            </label>
                                            <asp:TextBox contenteditable="true" CssClass="form-control  " runat="server" ID="txtJwbEssay" TextMode="MultiLine" Height="200px"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-12 row" runat="server" id="GandaDiv" style="display: none">
                                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                            <label style="color: #B7770D; font-size: small;">
                                                JAWABAN PILIHAN GANDA :
                                            </label>
                                            <div class="form-check form-switch">
                                            </div>
                                            <ul type="none">
                                                <li>
                                                    <asp:HiddenField ID="hdKodejwbGanda1" runat="server" />
                                                    <div class="row form-group">
                                                        <div class="col-md-1 text-right">
                                                            <asp:RadioButton ID="ChkJwbGanda1" runat="server" Text="" GroupName="ganda" />
                                                        </div>
                                                        <div class="col-md-11">
                                                            <asp:TextBox contenteditable="true" CssClass="form-control   " runat="server" ID="txtJwbGanda1" TextMode="MultiLine" Height="200px"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </li>
                                                <li>
                                                    <asp:HiddenField ID="hdKodejwbGanda2" runat="server" />
                                                    <div class="row form-group">
                                                        <div class="col-md-1 text-right">
                                                            <asp:RadioButton ID="ChkJwbGanda2" runat="server" Text="" GroupName="ganda" />
                                                        </div>
                                                        <div class="col-md-11">
                                                            <asp:TextBox contenteditable="true" CssClass="form-control   " runat="server" ID="txtJwbGanda2" TextMode="MultiLine"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </li>
                                                <li>
                                                    <asp:HiddenField ID="hdKodejwbGanda3" runat="server" />
                                                    <div class="row form-group">
                                                        <div class="col-md-1 text-right">
                                                            <asp:RadioButton ID="ChkJwbGanda3" runat="server" Text="" GroupName="ganda" />
                                                        </div>
                                                        <div class="col-md-11">
                                                            <asp:TextBox contenteditable="true" CssClass="form-control   " runat="server" ID="txtJwbGanda3" TextMode="MultiLine"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </li>
                                                <li>
                                                    <asp:HiddenField ID="hdKodejwbGanda4" runat="server" />
                                                    <div class="row form-group">
                                                        <div class="col-md-1 text-right">
                                                            <asp:RadioButton ID="ChkJwbGanda4" runat="server" Text="" GroupName="ganda" />
                                                        </div>
                                                        <div class="col-md-11">
                                                            <asp:TextBox contenteditable="true" CssClass="form-control   " runat="server" ID="txtJwbGanda4" TextMode="MultiLine"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </li>
                                                <li>
                                                    <asp:HiddenField ID="hdKodejwbGanda5" runat="server" />
                                                    <div class="row form-group">
                                                        <div class="col-md-1 text-right">
                                                            <asp:RadioButton ID="ChkJwbGanda5" runat="server" Text="" GroupName="ganda" />
                                                        </div>
                                                        <div class="col-md-11">
                                                            <asp:TextBox contenteditable="true" CssClass="form-control   " runat="server" ID="txtJwbGanda5" TextMode="MultiLine"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </li>
                                            </ul>
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
            <div id="fromBankSoal" runat="server" style="display: none">
                <div class="content-header ui-content-header"
                    style="background-color: #00198d; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 33px; right: 50px; width: 350px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
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
                    style="background-color: red; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 6; position: fixed; bottom: 33px; right: 50px; width: 200px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                    <div style="padding-left: 0px;">
                        <asp:LinkButton ToolTip=" Kembali " runat="server" ID="LinkButton6"
                            OnClick="btnBackToKelas_Click"
                            CssClass="btn-trans waves-attach waves-circle waves-effect" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;Kelas
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="content-header ui-content-header"
                    style="background-color: green; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 7; position: fixed; bottom: 33px; right: 50px; width: 100px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                    <div style="padding-left: 0px;">
                        <asp:LinkButton ToolTip=" Kembali " runat="server" ID="btnShowDataList" CssClass="btn-trans waves-attach waves-circle waves-effect" OnClick="btnBackToSoal_Click" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;Soal
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
            <div id="fromDesignSoal" runat="server" style="display: none">
                <div class="content-header ui-content-header"
                    style="background-color: #00198d; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 33px; right: 50px; width: 690px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                    <div style="padding-left: 0px;">
                        <asp:LinkButton ToolTip=" Kembali " runat="server" ID="LinkButton2"
                            OnClick="btnBackToMapel_Click"
                            CssClass="btn-trans waves-attach waves-circle waves-effect" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;Mata Pelajaran
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="content-header ui-content-header"
                    style="background-color: red; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 6; position: fixed; bottom: 33px; right: 50px; width: 540px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                    <div style="padding-left: 0px;">
                        <asp:LinkButton ToolTip=" Kembali " runat="server" ID="LinkButton3"
                            OnClick="btnBackToKelas_Click"
                            CssClass="btn-trans waves-attach waves-circle waves-effect" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;Kelas
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="content-header ui-content-header"
                    style="background-color: purple; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 7; position: fixed; bottom: 33px; right: 50px; width: 455px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                    <div style="padding-left: 0px;">
                        <asp:LinkButton ToolTip=" Kembali " runat="server" ID="LinkButton7"
                            OnClick="btnBackToStrukturNilai_Click"
                            CssClass="btn-trans waves-attach waves-circle waves-effect" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;Struktur Nilai
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="content-header ui-content-header"
                    style="background-color: green; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 8; position: fixed; bottom: 33px; right: 50px; width: 320px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                    <div style="padding-left: 0px;">
                        <asp:LinkButton ToolTip=" Kembali " runat="server" ID="LinkButton4"
                            OnClick="btnBackToFormRumahSoal_Click"
                            CssClass="btn-trans waves-attach waves-circle waves-effect" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;Form Rumah Soal
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="content-header ui-content-header"
                    style="background-color: orange; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 9; position: fixed; bottom: 33px; right: 50px; width: 150px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                    <div style="padding-left: 0px;">
                        <asp:LinkButton ToolTip=" Kembali " runat="server" ID="LinkButton5"
                            OnClick="btnBackToDesignSoal_Click"
                            CssClass="btn-trans waves-attach waves-circle waves-effect" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;Design Soal
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkOKInput" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">



        loadCkEditor();



    </script>

    <script type="text/javascript">
        RenderDropDownOnTables();
        InitModalFocus();
        //DoAutoSave();
    </script>
</asp:Content>
