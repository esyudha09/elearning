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


        function LoadTinyMCESoal() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                mode: "exact",
                selector: ".mcetiny_soal",
                plugins: 'anchor autolink charmap codesample emoticons image link lists media searchreplace table visualblocks wordcount checklist mediaembed casechange export formatpainter pageembed linkchecker a11ychecker  permanentpen powerpaste advtable advcode editimage tinycomments tableofcontents footnotes mergetags autocorrect typography inlinecss',
                toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck typography | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
                tinycomments_mode: 'embedded',
                tinycomments_author: 'Author name',
                mergetags_list: [
                    { value: 'First.Name', title: 'First Name' },
                    { value: 'Email', title: 'Email' },
                ],
                statusbar: true,
                menubar: true,
                height: 300,
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtSoalVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function () {
                        ed.getBody().style.fontSize = '14px';
                    });
                }
            });
        }

        function LoadTinyMCEjwbEssay() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                mode: "exact",
                selector: ".mcetiny_jwbEssay",
                plugins: 'anchor autolink charmap codesample emoticons image link lists media searchreplace table visualblocks wordcount checklist mediaembed casechange export formatpainter pageembed linkchecker a11ychecker  permanentpen powerpaste advtable advcode editimage tinycomments tableofcontents footnotes mergetags autocorrect typography inlinecss',
                toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck typography | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
                tinycomments_mode: 'embedded',
                tinycomments_author: 'Author name',
                mergetags_list: [
                    { value: 'First.Name', title: 'First Name' },
                    { value: 'Email', title: 'Email' },
                ],
                statusbar: true,
                menubar: true,
                height: 300,
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtJwbEssayVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function () {
                        ed.getBody().style.fontSize = '14px';
                    });
                }
            });
        }

        function LoadTinyMCEjwbGanda1() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                mode: "exact",
                selector: ".mcetiny_jwbGanda1",
                plugins: 'anchor autolink charmap codesample emoticons image link lists media searchreplace table visualblocks wordcount checklist mediaembed casechange export formatpainter pageembed linkchecker a11ychecker  permanentpen powerpaste advtable advcode editimage tinycomments tableofcontents footnotes mergetags autocorrect typography inlinecss',
                toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck typography | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
                tinycomments_mode: 'embedded',
                tinycomments_author: 'Author name',
                mergetags_list: [
                    { value: 'First.Name', title: 'First Name' },
                    { value: 'Email', title: 'Email' },
                ],
                statusbar: true,
                menubar: true,
                height: 300,
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtJwbGanda1Val.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function () {
                        ed.getBody().style.fontSize = '14px';
                    });
                }
            });
        }

        function LoadTinyMCEjwbGanda2() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                mode: "exact",
                selector: ".mcetiny_jwbGanda2",
                plugins: 'anchor autolink charmap codesample emoticons image link lists media searchreplace table visualblocks wordcount checklist mediaembed casechange export formatpainter pageembed linkchecker a11ychecker  permanentpen powerpaste advtable advcode editimage tinycomments tableofcontents footnotes mergetags autocorrect typography inlinecss',
                toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck typography | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
                tinycomments_mode: 'embedded',
                tinycomments_author: 'Author name',
                mergetags_list: [
                    { value: 'First.Name', title: 'First Name' },
                    { value: 'Email', title: 'Email' },
                ],
                statusbar: true,
                menubar: true,
                height: 300,
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtJwbGanda2Val.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function () {
                        ed.getBody().style.fontSize = '14px';
                    });
                }
            });
        }


        function LoadTinyMCEjwbGanda3() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                mode: "exact",
                selector: ".mcetiny_jwbGanda3",
                plugins: 'anchor autolink charmap codesample emoticons image link lists media searchreplace table visualblocks wordcount checklist mediaembed casechange export formatpainter pageembed linkchecker a11ychecker  permanentpen powerpaste advtable advcode editimage tinycomments tableofcontents footnotes mergetags autocorrect typography inlinecss',
                toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck typography | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
                tinycomments_mode: 'embedded',
                tinycomments_author: 'Author name',
                mergetags_list: [
                    { value: 'First.Name', title: 'First Name' },
                    { value: 'Email', title: 'Email' },
                ],
                statusbar: true,
                menubar: true,
                height: 300,
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtJwbGanda3Val.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function () {
                        ed.getBody().style.fontSize = '14px';
                    });
                }
            });
        }

        function LoadTinyMCEjwbGanda4() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                mode: "exact",
                selector: ".mcetiny_jwbGanda4",
                plugins: 'anchor autolink charmap codesample emoticons image link lists media searchreplace table visualblocks wordcount checklist mediaembed casechange export formatpainter pageembed linkchecker a11ychecker  permanentpen powerpaste advtable advcode editimage tinycomments tableofcontents footnotes mergetags autocorrect typography inlinecss',
                toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck typography | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
                tinycomments_mode: 'embedded',
                tinycomments_author: 'Author name',
                mergetags_list: [
                    { value: 'First.Name', title: 'First Name' },
                    { value: 'Email', title: 'Email' },
                ],
                statusbar: true,
                menubar: true,
                height: 300,
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtJwbGanda4Val.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function () {
                        ed.getBody().style.fontSize = '14px';
                    });
                }
            });
        }

        function LoadTinyMCEjwbGanda5() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                mode: "exact",
                selector: ".mcetiny_jwbGanda5",
                plugins: 'anchor autolink charmap codesample emoticons image link lists media searchreplace table visualblocks wordcount checklist mediaembed casechange export formatpainter pageembed linkchecker a11ychecker  permanentpen powerpaste advtable advcode editimage tinycomments tableofcontents footnotes mergetags autocorrect typography inlinecss',
                toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck typography | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
                tinycomments_mode: 'embedded',
                tinycomments_author: 'Author name',
                mergetags_list: [
                    { value: 'First.Name', title: 'First Name' },
                    { value: 'Email', title: 'Email' },
                ],
                statusbar: true,
                menubar: true,
                height: 300,
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtJwbGanda5Val.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function () {
                        ed.getBody().style.fontSize = '14px';
                    });
                }
            });
        }

        function RemoveTinyMCE() {
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtSoal.ClientID %>');
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtJwbEssay.ClientID %>');
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtJwbGanda1.ClientID %>');
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtJwbGanda2.ClientID %>');
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtJwbGanda3.ClientID %>');
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtJwbGanda4.ClientID %>');
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtJwbGanda5.ClientID %>');

        }
        function ReInitTinyMCE() {
            RemoveTinyMCE();
            LoadTinyMCESoal();
            LoadTinyMCEjwbEssay();
            LoadTinyMCEjwbGanda1();
            LoadTinyMCEjwbGanda2();
            LoadTinyMCEjwbGanda3();
            LoadTinyMCEjwbGanda4();
            LoadTinyMCEjwbGanda5();
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
                    ReInitTinyMCE();
                    window.scrollTo(0, 0);
                    break;
                case "<%= JenisAction.Add %>":
                    ReInitTinyMCE();
                    //$('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.AddWithMessage %>":
                    ReInitTinyMCE();
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
                    ReInitTinyMCE();
                    //$('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmHapus %>":
                    ReInitTinyMCE();
                    //$('#ui_modal_confirm_hapus').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.Update %>":
                    ReInitTinyMCE();
                    //HideModal();
                    break;
                case "<%= JenisAction.Delete %>":
                    ReInitTinyMCE();

                    break;
                case "<%= JenisAction.DoAdd %>":
                    ReInitTinyMCE();

                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoUpdate %>":
                    ReInitTinyMCE();
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
                    ReInitTinyMCE();
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
                    document.getElementById("<%= txtSoal.ClientID %>").focus();
                }
            });--%>
        }

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
            <asp:HiddenField runat="server" ID="txtSoalVal" />
            <asp:HiddenField runat="server" ID="txtJwbEssayVal" />
            <asp:HiddenField runat="server" ID="txtJwbGanda1Val" />
            <asp:HiddenField runat="server" ID="txtJwbGanda2Val" />
            <asp:HiddenField runat="server" ID="txtJwbGanda3Val" />
            <asp:HiddenField runat="server" ID="txtJwbGanda4Val" />
            <asp:HiddenField runat="server" ID="txtJwbGanda5Val" />

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
                                        <asp:LinkButton OnClientClick="TriggerSave()" ValidationGroup="vldInput" CssClass="btn btn-brand" runat="server" ID="lnkOKInput" OnClick="lnkOKInput_Click" Text="Simpan"></asp:LinkButton>
                                    </div>
                                    <div class="col-md-12 row">
                                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">

                                            <label for="<%= txtSoal.ClientID %>" style="color: #B7770D; font-size: small;">
                                                SOAL :
                                            </label>
                                            <asp:TextBox ValidationGroup="vldInput" CssClass="form-control mcetiny_soal" runat="server" ID="txtSoal" Height="200px"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-3 row">
                                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                            <label for="<%= cboJenis.ClientID %>" style="color: #B7770D; font-size: small;">
                                                JENIS SOAL :
                                            </label>
                                            <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboJenis" CssClass="input-box" onchange="JenisCheck(this);">
                                                <asp:ListItem Text="" Value=""></asp:ListItem>
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
                                            <asp:TextBox ValidationGroup="vldInput" CssClass="form-control  mcetiny_jwbEssay" runat="server" ID="txtJwbEssay" TextMode="MultiLine" Height="200px"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-12 row" runat="server" id="GandaDiv" style="display: none">
                                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                            <label style="color: #B7770D; font-size: small;">
                                                JAWABAN PILIHAN GANDA :
                                            </label>
                                            <ol type="a">
                                                <li>
                                                    <asp:RadioButton ID="ChkJwbGanda1" runat="server" Text="" GroupName="ganda" />
                                                    <asp:TextBox ValidationGroup="vldInput" CssClass="form-control mcetiny_jwbGanda1  " runat="server" ID="txtJwbGanda1" TextMode="MultiLine"></asp:TextBox></li>
                                                <li>
                                                    <asp:RadioButton ID="ChkJwbGanda2" runat="server" Text="" GroupName="ganda" />
                                                    <asp:TextBox ValidationGroup="vldInput" CssClass="form-control mcetiny_jwbGanda2 " runat="server" ID="txtJwbGanda2" TextMode="MultiLine"></asp:TextBox></li>
                                                <li>
                                                    <asp:RadioButton ID="ChkJwbGanda3" runat="server" Text="" GroupName="ganda" />
                                                    <asp:TextBox ValidationGroup="vldInput" CssClass="form-control mcetiny_jwbGanda3 " runat="server" ID="txtJwbGanda3" TextMode="MultiLine"></asp:TextBox></li>
                                                <li>
                                                    <asp:RadioButton ID="ChkJwbGanda4" runat="server" Text="" GroupName="ganda" />
                                                    <asp:TextBox ValidationGroup="vldInput" CssClass="form-control mcetiny_jwbGanda4 " runat="server" ID="txtJwbGanda4" TextMode="MultiLine"></asp:TextBox></li>
                                                <li>
                                                    <asp:RadioButton ID="ChkJwbGanda5" runat="server" Text="" GroupName="ganda" />
                                                    <asp:TextBox ValidationGroup="vldInput" CssClass="form-control mcetiny_jwbGanda5 " runat="server" ID="txtJwbGanda5" TextMode="MultiLine"></asp:TextBox></li>
                                            </ol>
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
            <div class="content-header ui-content-header"
                style="background-color: #00198d; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 33px; right: 50px; width: 160px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                <div style="padding-left: 0px;">
                    <asp:LinkButton ToolTip=" Kembali " runat="server" ID="btnShowDataList" CssClass="btn-trans waves-attach waves-circle waves-effect" OnClick="btnBackToMenu_Click" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;Data Soal
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




        LoadTinyMCESoal();
        LoadTinyMCEjwbEssay();
        LoadTinyMCEjwbGanda1();
        LoadTinyMCEjwbGanda2();
        LoadTinyMCEjwbGanda3();
        LoadTinyMCEjwbGanda4();
        LoadTinyMCEjwbGanda5();

    </script>

    <script type="text/javascript">
        RenderDropDownOnTables();
        InitModalFocus();
        //DoAutoSave();
    </script>
</asp:Content>
