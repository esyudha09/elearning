<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Second.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="wf.BankSoal.View.aspx.cs" Inherits="AI_ERP.Application_Modules.CBT.wf_BankSoal_View" %>

<%@ MasterType VirtualPath="~/Application_Masters/Second.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        var currentValue = 0;

      

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

             <asp:HiddenField runat="server" ID="hdSourceAudio"/>
            <asp:HiddenField runat="server" ID="hdSourceVideo"/>
           

            <%--<asp:Button runat="server" UseSubmitBehavior="false" ID="btnShowConfirmDelete" OnClick="btnShowConfirmDelete_Click" Style="position: absolute; left: -1000px; top: -1000px;" />--%>

            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-12">

                    <div class="card">
                        <div class="card-main">
                            <div class="card-header" style="background-color: #295BC8; padding: 10px; font-weight: bold; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                <span></span>

                            </div>
                            <div class="card-inner">

                                <div class="col-md-12 row text-center">
                                    <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">

                                        <label for="<%= txtSoal.ClientID %>" style="color: #B7770D; font-size: small;">
                                            SOAL :
                                        </label>


                                        <img id="FileImageID"  width="300" src=""  style="display:none" runat="server"/>

                                        <video width="320" height="240" controls id="FileVideoID"  style="display:none"   runat="server">
                                            <source  type="video/mp4">
                                            Your browser does not support the video tag.
                                        </video>

                                        <audio controls id="FileAudioID" style="display:none"  runat="server">
                                            <source  type="audio/mpeg">
                                            Your browser does not support the audio element.
                                        </audio>
                                       
                                        <asp:Literal runat="server" ID="txtSoal"> </asp:Literal>
                                    </div>
                                </div>



                                <div class="col-md-12 row" runat="server" id="EssayDiv" style="display: none">
                                    <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">

                                        <label style="color: #B7770D; font-size: small;">
                                            JAWABAN ESSAY :
                                        </label>
                                        <asp:Literal runat="server" ID="txtJwbEssay"> </asp:Literal>
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

       
        var audio = document.getElementById("<%=FileAudioID.ClientID%>")
        audio.getElementsByTagName('source')[0].src = <%=hdSourceAudio.ClientID%>.value;
        audio.load();

        var video = document.getElementById("<%=FileVideoID.ClientID%>")
        video.getElementsByTagName('source')[0].src = <%=hdSourceVideo.ClientID%>.value;
        video.load();
        
       

    </script>

    <script type="text/javascript">
        //RenderDropDownOnTables();
        //InitModalFocus();
        //DoAutoSave();
    </script>
</asp:Content>
