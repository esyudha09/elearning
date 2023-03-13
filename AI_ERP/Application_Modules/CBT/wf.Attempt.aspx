<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Second.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="wf.Attempt.aspx.cs" Inherits="AI_ERP.Application_Modules.CBT.wf_Attempt" %>

<%@ MasterType VirtualPath="~/Application_Masters/Second.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        var currentValue = 0;
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
           //CKEDITOR.config.contentsCss = 'body { word - wrap: break-word;} ';
           
            CKEDITOR.config.sharedSpaces = { top: 'toolbar1' };
            CKEDITOR.replace('<%= txtJwbEssay.ClientID %>', {
                extraPlugins: 'ckeditor_wiris,indentblock,indent,justify,textindent',
                language: 'en',
                startupFocus: true
            });

           <%-- CKEDITOR.config.toolbar_Full =
                [
                    { name: 'document', items: ['Source'] },
                    { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', '-', 'Undo', 'Redo'] },
                    { name: 'editing', items: ['Find'] },
                    { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline'] },
                    { name: 'paragraph', items: ['JustifyLeft', 'JustifyCenter', 'JustifyRight'] }
                ];
            CKEDITOR.config.height = '40px';
            //CKEDITOR.plugins.addExternal('divarea', '../examples/extraplugins/divarea/', 'plugin.js');
            CKEDITOR.config.removePlugins = 'maximize';
            CKEDITOR.config.removePlugins = 'resize';
            CKEDITOR.replace('<%= txtJwbEssay.ClientID %>', {
                extraPlugins: 'divarea,ckeditor_wiris',
                language: 'en'
            });--%>


        }

        function UpdateCkEditor() {
            CKEDITOR.instances["<%= txtJwbEssay.ClientID %>"].updateElement();
        };

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

                    break;
                case "<%= JenisAction.AddWithMessage %>":
                    loadCkEditor();

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

                    break;
                case "<%= JenisAction.Clear %>":

                    break;

                default:

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
            <asp:HiddenField runat="server" ID="hdIdx" />
            <asp:HiddenField runat="server" ID="hdFormChange" Value="0" />
            <asp:HiddenField runat="server" ID="hdKodejwbGanda1" />
            <asp:HiddenField runat="server" ID="hdKodejwbGanda2" />
            <asp:HiddenField runat="server" ID="hdKodejwbGanda3" />
            <asp:HiddenField runat="server" ID="hdKodejwbGanda4" />
            <asp:HiddenField runat="server" ID="hdKodejwbGanda5" />

            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnLinkClick" OnClick="btnLink_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <%--<asp:Button runat="server" UseSubmitBehavior="false" ID="counterClick" OnClick="counter_Click" Style="position: absolute; left: -1000px; top: -1000px;" />--%>
            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-9">

                    <div class="card">
                        <div class="card-main">
                            <div class="card-action" style="background-color: #4AA4A4; padding-left: 20px; padding-right: 20px; border-top-left-radius: 6px; border-top-right-radius: 6px; background-repeat: no-repeat; margin-left: -1px; margin-top: -1px; margin-right: -1px;">
                                <div style="float: right">
                                    <p>
                                        <button onclick="btnSelesai_Click" data-toggle="modal" data-target="#ui_modal_confirm_selesai" id="LinkButton2" class="btn btn-grey" style="color: white; font-size: smaller;"><i class="fa fa-paper-plane"></i>Selesai</button>
                                        <br />
                                        <label id="counter" style="font-weight: bold; color: white; font-weight: bold; font-size: large; margin-top: 10px"></label>
                                    </p>
                                </div>
                                <div style="float: left">
                                    <p>
                                        <span style="font-weight: bold; color: white; font-weight: bold; font-size: medium;">
                                            <asp:Literal ID="txtMapel" runat="server"></asp:Literal>
                                            -
                                    <asp:Literal ID="txtKelas" runat="server"></asp:Literal></span>
                                        <br />
                                        <asp:Literal ID="txtTahunAjaran" runat="server"></asp:Literal>
                                        -
                                    <asp:Literal ID="txtSemester" runat="server"></asp:Literal>
                                        <br />
                                        <label class="badge" style="font-size: medium; color: white; font-weight: bold; color: white;">
                                            <asp:Literal ID="txtNamaKP" runat="server"></asp:Literal></label>
                                    </p>
                                </div>

                            </div>
                            <div class="card-inner margin-top-xs">

                                <div class="col-md-12 row">
                                    <div style="margin-top: 5px; margin-bottom: 5px;">

                                        <label for="<%= txtSoal.ClientID %>" style="color: #B7770D; font-size: small;">
                                            SOAL :
                                        </label>
                                        <%--<asp:TextBox CssClass="form-control mcetiny_soal" runat="server" ID="txtSoal" Height="200px"></asp:TextBox>--%>
                                        <asp:Literal runat="server" ID="txtSoal"> </asp:Literal>
                                        <hr />
                                    </div>
                                </div>


                                <div class="col-md-12 row" runat="server" id="EssayDiv" style="display: none;">
                                    <div>

                                        <%-- <label style="color: #B7770D; font-size: small;">
                                            JAWABAN ESSAY :
                                        </label>--%>
                                        <asp:TextBox contenteditable="true" CssClass="form-control" runat="server" ID="txtJwbEssay" TextMode="MultiLine" Height="200px"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-12 row" runat="server" id="GandaDiv" style="display: none">
                                    <div>
                                        <%--  <label style="color: #B7770D; font-size: small;">
                                            JAWABAN PILIHAN GANDA :
                                        </label>--%>
                                        <div class="row">
                                            <div class="col-md-1 text-right padding-top-sm">
                                                <asp:RadioButton ID="ChkJwbGanda1" runat="server" Text="" GroupName="ganda" OnClick="FormChangeCheck()"/>
                                            </div>
                                            <div class="col-md-11">
                                                <asp:Literal runat="server" ID="txtJwbGanda1"></asp:Literal>

                                            </div>
                                        </div>

                                       
                                        <div class="row">
                                            <div class="col-md-1 text-right padding-top-sm">
                                                <asp:RadioButton ID="ChkJwbGanda2" runat="server" Text="" GroupName="ganda" OnClick="FormChangeCheck()"/>
                                            </div>
                                            <div class="col-md-11">
                                                <asp:Literal runat="server" ID="txtJwbGanda2"></asp:Literal>

                                            </div>
                                        </div>

                                       
                                        <div class="row">
                                            <div class="col-md-1 text-right padding-top-sm">
                                                <asp:RadioButton ID="ChkJwbGanda3" runat="server" Text="" GroupName="ganda" OnClick="FormChangeCheck()"/>
                                            </div>
                                            <div class="col-md-11">
                                                <asp:Literal runat="server" ID="txtJwbGanda3"></asp:Literal>

                                            </div>
                                        </div>

                                       
                                        <div class="row">
                                            <div class="col-md-1 text-right padding-top-sm">
                                                <asp:RadioButton ID="ChkJwbGanda4" runat="server" Text="" GroupName="ganda" OnClick="FormChangeCheck()"/>
                                            </div>
                                            <div class="col-md-11">
                                                <asp:Literal runat="server" ID="txtJwbGanda4"></asp:Literal>

                                            </div>
                                        </div>

                                      
                                        <div class="row">
                                            <div class="col-md-1 text-right padding-top-sm">
                                                <asp:RadioButton ID="ChkJwbGanda5" runat="server" Text="" GroupName="ganda" OnClick="FormChangeCheck()"/>
                                            </div>
                                            <div class="col-md-11">
                                                <asp:Literal runat="server" ID="txtJwbGanda5"></asp:Literal>

                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>

                            <div class="card-action-btn pull-left col-md-12" style="margin-left: 0px; margin-right: 0px; color: grey;">
                                <button style="font-size: small" onclick="ClearJwb();FormChangeCheck()" id="lnkClrJwb" class="btn btn-red;">Bersihkan Jawaban </button>
                                <hr />
                                <asp:LinkButton Style="font-size: small; float: left; margin-bottom: 5px" OnClientClick="UpdateCkEditor();" OnClick="btnPrev_Click" runat="server" ID="btnPrev" CssClass="btn btn-brand btnSave"><i class="fa fa-arrow-left"></i> Sebelumnya</asp:LinkButton>
                                <asp:LinkButton Style="font-size: small; float: right; margin-bottom: 5px" OnClientClick="UpdateCkEditor();" OnClick="btnNext_Click" runat="server" ID="btnNext" CssClass="btn btn-brand btnSave"> Berikutnya <i class="fa fa-arrow-right"></i></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">

                    <div class="card">
                        <div class="card-main">
                            <div class="card-header" style="background-color: #4AA4A4; padding-left: 20px; padding-right: 20px; border-top-left-radius: 6px; border-top-right-radius: 6px; margin-left: -1px; margin-top: -1px; margin-right: -1px;">
                                <div style="font-size: medium; color: white;">
                                    Terjawab
                                    <span style="font-weight: bold">
                                        <asp:Literal ID="txtTerjawab" runat="server"></asp:Literal></span>
                                    sisa
                                    <span style="font-weight: bold">
                                        <asp:Literal ID="txtSisa" runat="server"></asp:Literal></span>
                                    dari
                                   <span style="font-weight: bold">
                                       <asp:Literal ID="txtTotalSoal" runat="server"></asp:Literal></span>
                                    Soal
                                </div>
                            </div>
                            <div class="card-inner row  " style="text-align: justify">
                                <asp:Literal ID="txtLink" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_confirm_selesai" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                                            Apakah yakin akan mengakhiri tes ini?
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapus" OnClick="btnSelesai_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="KontenBawah" runat="server">
    <style>
        input[type="radio"] {
            margin-right: 10px;
            margin-left: 10px;
        }

        .btnLnk {
            text-decoration: none;
            font-size: 14px;
            line-height: 20px;
            font-weight: 400;
            background-color: #fff;
            background-image: none;
            height: 40px;
            width: 30px;
            border-radius: 3px;
            border: 0;
            overflow: visible;
            margin: 0 6px 6px 0;
        }
    </style>
    <script type="text/javascript">

        loadCkEditor();

        function ClearJwb() {
            $('input[type="radio"]').attr('checked', false);
            $("#<%=txtJwbEssay.ClientID%>").html("");
        }

        function FormChangeCheck() {
            //alert(1);
            //$("#<%=hdFormChange.ClientID%>").val = "1";
            document.getElementById("<%=hdFormChange.ClientID%>").value = 1;
        }




        setInterval(function () {
            var pageUrl = '<%=ResolveUrl("wf.Attempt.aspx")%>';
            $.ajax({
                type: "POST",
                url: pageUrl + "/Counter_Click",
                data: '{name: "tes" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d == "end") {
                        const urlParams = new URLSearchParams(window.location.search);
                        const rs = urlParams.get('rs')
                        window.location.href = "sa?rs=" + rs;
                    } else {
                        $("#counter").html(data.d);
                    }

                },
                error: function (data) {
                    //console.log(JSON.stringify(error));
                }
            });
        }, 1000);



        //LoadTinyMCEjwbEssay();
        //RenderDropDownOnTables();
        //InitModalFocus();
        //DoAutoSave();
    </script>
</asp:Content>
