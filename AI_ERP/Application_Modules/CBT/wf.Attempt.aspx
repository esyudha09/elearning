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

                   
                    break;
            }



        }

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
            tinyMCE.execCommand('mceRemoveEditor', true, '<%= txtJwbEssay.ClientID %>');
        }
        function ReInitTinyMCE() { 
            RemoveTinyMCE();
            LoadTinyMCEjwbEssay();
            
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
                            <div class="card-header" style="background-color: #295BC8; padding: 10px; font-weight: bold; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                <span></span>

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

                                <%-- <div class="col-md-3 row">
                                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
                                            <label style="color: #B7770D; font-size: small;">
                                                JENIS SOAL :
                                            </label>
                                             <asp:Literal runat="server" id="txtJenis"> </asp:Literal>
                                        </div>
                                    </div>--%>

                                <div class="col-md-12 row" runat="server" id="EssayDiv" style="display: none">
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
                                        <ul type="none">
                                            <li>
                                                <asp:HiddenField ID="hdKodejwbGanda1" runat="server" />
                                                <div class="row form-group">
                                                    <div class="col-md-1 text-right">
                                                        <asp:RadioButton ID="ChkJwbGanda1" runat="server" Text="" GroupName="ganda" />
                                                    </div>
                                                    <div class="col-md-11">
                                                        <asp:Literal runat="server" ID="txtJwbGanda1"></asp:Literal>

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
                                                        <asp:Literal runat="server" ID="txtJwbGanda2"></asp:Literal>

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
                                                        <asp:Literal runat="server" ID="txtJwbGanda3"></asp:Literal>

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
                                                        <asp:Literal runat="server" ID="txtJwbGanda4"></asp:Literal>

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
                                                        <asp:Literal runat="server" ID="txtJwbGanda5"></asp:Literal>

                                                    </div>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="card-action" style="padding-left: 10px; padding-right: 10px;">
                                <div class="card-action-btn pull-left text-center col-md-12" style="margin-left: 0px; margin-right: 0px; color: grey;">


                                    <asp:LinkButton OnClick="btnPrev_Click" runat="server" ID="btnPrev" CssClass="btn btn-brand margin-right-lg"><i class="fa fa-arrow-left"></i> Sebelumnya</asp:LinkButton>
                                    <asp:LinkButton OnClick="btnNext_Click" runat="server" ID="btnNext" CssClass="btn btn-brand margin-left-lg"><i class="fa fa-arrow-right" ></i> Lanjutkan</asp:LinkButton>

                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3">

                    <div class="card">
                        <div class="card-main">
                            <div class="card-header" style="background-color: #295BC8; padding: 10px; font-weight: bold; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                <span></span>

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



        LoadTinyMCEjwbEssay();

    </script>

    <script type="text/javascript">
        RenderDropDownOnTables();
        InitModalFocus();
        //DoAutoSave();
    </script>
</asp:Content>
