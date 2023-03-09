<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="wf.Attempt.aspx.cs" Inherits="AI_ERP.Application_Modules.CBT.wf_Attempt" %>

<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
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
            CKEDITOR.config.sharedSpaces = { top: 'toolbar1' };
            CKEDITOR.replace('<%= txtJwbEssay.ClientID %>', {
                extraPlugins: 'ckeditor_wiris,indentblock',
                language: 'en',
                startupFocus: true
            });

            
        }

        function UpdateCk() {

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

            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnLinkClick" OnClick="btnLink_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <%--<asp:Button runat="server" UseSubmitBehavior="false" ID="counterClick" OnClick="counter_Click" Style="position: absolute; left: -1000px; top: -1000px;" />--%>
            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-9">

                    <div class="card">
                        <div class="card-main">
                            <div class="card-action" style="background-color: #4AA4A4; padding-left: 20px; padding-right: 20px; border-top-left-radius: 6px; border-top-right-radius: 6px; background-repeat: no-repeat; margin-left: -1px; margin-top: -1px; margin-right: -1px;">
                                <div style="float: left">
                                    <p class="card-heading" style="margin-bottom: 0px; margin-top: 15px; color: white;">
                                        <span style="font-weight: bold; color: white; font-weight: bold; font-size: larger;">
                                            <asp:Literal ID="txtMapel" runat="server"></asp:Literal>
                                            -
                                    <asp:Literal ID="txtKelas" runat="server"></asp:Literal></span>
                                    </p>
                                    <div style="font-size: medium; color: white;">
                                        <asp:Literal ID="txtTahunAjaran" runat="server"></asp:Literal>
                                        -
                                    <asp:Literal ID="txtSemester" runat="server"></asp:Literal>
                                    </div>

                                    <div>
                                        <label class="badge" style="font-size: medium; color: white; font-weight: bold; color: white;">
                                            <asp:Literal ID="txtNamaKP" runat="server"></asp:Literal></label>
                                        <p></p>
                                    </div>
                                </div>
                                <div class="float-right">
                                    <p class="text-right">
                                        <asp:LinkButton OnClick="btnSelesai_Click" runat="server" ID="LinkButton2" CssClass="btn btn-grey" Style="font-weight: bold; color: white; font-weight: bold; font-size: small;"><i class="fa fa-paper-plane" ></i> Selesai</asp:LinkButton>
                                    </p>
                                    <p class="text-right">
                                        <label id="counter" class="margin-left-sm" style="font-weight: bold; color: white; font-weight: bold; font-size: large;"></label>
                                    </p>
                                </div>
                            </div>
                            <div class="card-inner">

                                <div class="col-md-12 row">
                                    <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">

                                        <label for="<%= txtSoal.ClientID %>" style="color: #B7770D; font-size: small;">
                                            SOAL :
                                        </label>
                                        <%--<asp:TextBox CssClass="form-control mcetiny_soal" runat="server" ID="txtSoal" Height="200px"></asp:TextBox>--%>
                                        <asp:Literal runat="server" ID="txtSoal"> </asp:Literal>
                                        <hr />
                                    </div>
                                </div>


                                <div class="col-md-12 row" runat="server" id="EssayDiv" style="display: none;">
                                    <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">

                                        <label style="color: #B7770D; font-size: small;">
                                            JAWABAN ESSAY :
                                        </label>
                                        <asp:TextBox contenteditable="true" CssClass="form-control" runat="server" ID="txtJwbEssay" TextMode="MultiLine" Height="200px"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-12 row" runat="server" id="GandaDiv" style="display: none">
                                    <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                        <label style="color: #B7770D; font-size: small;">
                                            JAWABAN PILIHAN GANDA :
                                        </label>

                                        <asp:HiddenField ID="hdKodejwbGanda1" runat="server" />
                                        <div class="row form-group">
                                            <div class="col-md-1 text-right">
                                                <asp:RadioButton ID="ChkJwbGanda1" runat="server" Text="" GroupName="ganda" />
                                            </div>
                                            <div class="col-md-11">
                                                <asp:Literal runat="server" ID="txtJwbGanda1"></asp:Literal>

                                            </div>
                                        </div>

                                        <asp:HiddenField ID="hdKodejwbGanda2" runat="server" />
                                        <div class="row form-group">
                                            <div class="col-md-1 text-right">
                                                <asp:RadioButton ID="ChkJwbGanda2" runat="server" Text="" GroupName="ganda" />
                                            </div>
                                            <div class="col-md-11">
                                                <asp:Literal runat="server" ID="txtJwbGanda2"></asp:Literal>

                                            </div>
                                        </div>

                                        <asp:HiddenField ID="hdKodejwbGanda3" runat="server" />
                                        <div class="row form-group">
                                            <div class="col-md-1 text-right">
                                                <asp:RadioButton ID="ChkJwbGanda3" runat="server" Text="" GroupName="ganda" />
                                            </div>
                                            <div class="col-md-11">
                                                <asp:Literal runat="server" ID="txtJwbGanda3"></asp:Literal>

                                            </div>
                                        </div>

                                        <asp:HiddenField ID="hdKodejwbGanda4" runat="server" />
                                        <div class="row form-group">
                                            <div class="col-md-1 text-right">
                                                <asp:RadioButton ID="ChkJwbGanda4" runat="server" Text="" GroupName="ganda" />
                                            </div>
                                            <div class="col-md-11">
                                                <asp:Literal runat="server" ID="txtJwbGanda4"></asp:Literal>

                                            </div>
                                        </div>

                                        <asp:HiddenField ID="hdKodejwbGanda5" runat="server" />
                                        <div class="row form-group">
                                            <div class="col-md-1 text-right">
                                                <asp:RadioButton ID="ChkJwbGanda5" runat="server" Text="" GroupName="ganda" />
                                            </div>
                                            <div class="col-md-11">
                                                <asp:Literal runat="server" ID="txtJwbGanda5"></asp:Literal>

                                            </div>
                                        </div>


                                    </div>
                                </div>
                            </div>


                            <div class="card-action-btn pull-left col-md-12" style="margin-left: 0px; margin-right: 0px; color: grey;">
                                <button style="font-size: small" onclick="ClearJwb()" id="lnkClrJwb" class="btn btn-red;">Bersihkan Jawaban </button>
                                <hr />
                                <asp:LinkButton Style="font-size: small; float: left; margin-bottom: 5px" OnClientClick="UpdateCk()" OnClick="btnPrev_Click" runat="server" ID="btnPrev" CssClass="btn btn-brand btnSave"><i class="fa fa-arrow-left"></i> Sebelumnya</asp:LinkButton>
                                <asp:LinkButton Style="font-size: small; float: right; margin-bottom: 5px" OnClientClick="UpdateCk()" OnClick="btnNext_Click" runat="server" ID="btnNext" CssClass="btn btn-brand btnSave"> Berikutnya <i class="fa fa-arrow-right"></i></asp:LinkButton>

                            </div>




                        </div>
                    </div>
                </div>
                <div class="col-md-3">

                    <div class="card">
                        <div class="card-main">
                            <div class="card-header" style="background-color: #4AA4A4; padding-left: 20px; padding-right: 20px; border-top-left-radius: 6px; border-top-right-radius: 6px; margin-left: -1px; margin-top: -1px; margin-right: -1px;">
                            </div>
                            <div class="card-inner row">

                                <asp:Literal ID="txtLink" runat="server"></asp:Literal>

                            </div>

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
    <script type="text/javascript">
        loadCkEditor();
        function ClearJwb() {
            $('input[type="radio"]').attr('checked', false);

            $("#<%=txtJwbEssay.ClientID%>").html("");
        }

        $('input[type="radio"]').change(function () {
            if (this.checked) {
                $("#lnkClrJwb").css("display", "block");
            }
        });

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
