<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Second.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="wf.Attempt.aspx.cs" Inherits="AI_ERP.Application_Modules.CBT.wf_Attempt" %>

<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        var currentValue = 0;
        document.getElementById("<%= txtKeyAction.ClientID %>").value = "";
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

                    break;
                case "<%= JenisAction.AddWithMessage %>":
                    // ReInitTinyMCE();

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

                    break;
                case "<%= JenisAction.Clear %>":
                    document.getElementById("<%= txtKeyAction.ClientID %>").value = "";
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

        document.getElementById("<%= txtKeyAction.ClientID %>").value = "";

        function LoadTinyMCEjwbEssay() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.init({
                mode: "exact",
                selector: ".mcetiny_jwbEssay",
                plugins: 'anchor autolink charmap codesample emoticons image link lists media searchreplace table visualblocks wordcount ',
                toolbar: 'undo redo | blocks fontfamily fontsize | bold italic underline strikethrough | link image media table mergetags | addcomment showcomments | spellcheckdialog a11ycheck typography | align lineheight | checklist numlist bullist indent outdent | emoticons charmap | removeformat',
                tinycomments_mode: 'embedded',
                tinycomments_author: 'Author name',
                mergetags_list: [
                    { value: 'First.Name', title: 'First Name' },
                    { value: 'Email', title: 'Email' },
                ],
                statusbar: true,
                menubar: true,
                height: 400,
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

        function RemoveTinyMCE() {
            console.log("oii")
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtJwbEssay.ClientID %>');
        }
        function ReInitTinyMCE() {
            RemoveTinyMCE();
            LoadTinyMCEjwbEssay();

            //document.getElementById("<%= txtKeyAction.ClientID %>").value = "";
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
            <asp:HiddenField runat="server" ID="txtJwbEssayVal" />
            <asp:HiddenField runat="server" ID="hdIdx" />

            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnLinkClick" OnClick="btnLink_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-9">

                    <div class="card">
                        <div class="card-main">
                            <div class="card-header row" style="background-position: top right; background-color: #4AA4A4; padding-left: 20px; padding-right: 20px; border-top-left-radius: 6px; border-top-right-radius: 6px; margin-left: -1px; margin-top: -1px; margin-right: -1px;">
                                <div class="col-md-2">
                                    <span style="font-weight: bold; color: white; font-weight: bold; font-size: larger;">
                                        <asp:Literal ID="txtMapel" runat="server"></asp:Literal>
                                        -
                                    <asp:Literal ID="txtKelas" runat="server"></asp:Literal>
                                    </span>
                                    <br />
                                    <span style="font-size: medium; color: white;">
                                        <asp:Literal ID="txtTahunAjaran" runat="server"></asp:Literal>
                                        -
                                <asp:Literal ID="txtSemester" runat="server"></asp:Literal>
                                    </span>


                                </div>

                                <div class="col-md-10" style="font-weight: bold; color: white; font-weight: bold; font-size: larger; text-align: right">
                                    <asp:Literal ID="txtNamaKP" runat="server"></asp:Literal>
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
                                    </div>
                                </div>


                                <div class="col-md-12 row" runat="server" id="EssayDiv" style="display: none;">
                                    <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">

                                        <label style="color: #B7770D; font-size: small;">
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
                            <div class="card-action" style="padding-left: 10px; padding-right: 10px;">
                                <div class="card-action-btn pull-left text-center col-md-12 row" style="margin-left: 0px; margin-right: 0px;">

                                    <div class="text-left col-md-2">
                                        <asp:LinkButton Style="font-size: larger" OnClick="btnPrev_Click" runat="server" ID="btnPrev" CssClass="btn btn-grey margin-right-lg"><i class="fa fa-arrow-left"></i> Sebelumnya</asp:LinkButton>
                                    </div>
                                    <div class="text-center col-md-8">
                                        <asp:LinkButton Style="font-size: larger" OnClick="lnkOKInput_Click" runat="server" ID="LinkButton1" CssClass="btn btn-grey margin-left-lg"><i class="fa fa-save" ></i> Simpan</asp:LinkButton>
                                    </div>
                                    <div class="text-right  col-md-2">
                                        <asp:LinkButton Style="font-size: larger" OnClick="btnNext_Click" runat="server" ID="btnNext" CssClass="btn btn-grey margin-left-lg"><i class="fa fa-arrow-right" ></i> Lanjutkan</asp:LinkButton>

                                    </div>
                                </div>


                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">

                    <div class="card">
                        <div class="card-main">
                            <div class="card-header" style="background-color: #4AA4A4; padding-left: 20px; padding-right: 20px; border-top-left-radius: 6px; border-top-right-radius: 6px; margin-left: -1px; margin-top: -1px; margin-right: -1px;">
                                <span></span>
                                <%--<button class="btn btn-brand">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" style="font-weight: bold; color: white; font-weight: bold; font-size: larger;">
                                        <ContentTemplate>
                                            <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick">
                                            </asp:Timer>
                                            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </button>--%>
                                <div>
                                   
                                    <button class="btn btn-brand">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTime" Text="text" runat="server" />
                                                <asp:Timer ID="timer" runat="server" Interval="1000">
                                                </asp:Timer>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </button>
                                </div>

                                <%--<asp:LinkButton OnClick="CountStop" runat="server" ID="LinkButton2" CssClass="btn btn-grey margin-left-lg"><i class="fa fa-paper-plane" ></i> Selesai</asp:LinkButton>--%>
                                <%--<asp:LinkButton OnClick="CountStart" runat="server" ID="LinkButton3" CssClass="btn btn-grey margin-left-lg"><i class="fa fa-arrow-right" ></i> star timer</asp:LinkButton>--%>
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

</script>

    <script type="text/javascript">
        RenderDropDownOnTables();
        InitModalFocus();
        //DoAutoSave();
    </script>
</asp:Content>
