<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.NilaiSiswaEkskul.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Penilaian.SMA.wf_NilaiSiswaEkskul" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function EndRequestHandler() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        }

        function EndRequest() {
            var jenis_act = document.getElementById("<%= txtKeyAction.ClientID %>").value;
        }

        function SaveNilaiEkskul(tahun_ajaran, semester, rel_mapel, rel_siswa, nilai, lts_hd) {
            var s_url = "<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APIS.SMA.NILAI_SISWA.DO_SAVE.FILE + "/DoUpdateEkskul") %>";
            s_url += "?t=" + tahun_ajaran +
                     "&sm=" + semester +
                     "&mp=" + rel_mapel +
                     "&s=" + rel_siswa +
                     "&n=" + nilai +
                     "&lts_hd=" + lts_hd;

            $.ajax({
                url: s_url, 
                dataType: 'json',
                type: 'GET', 
                contentType: 'application/json; charset=utf-8', 
                success: function(data) { 
                    }, 
                error: function(response) { 
                        alert(response.responseText); 
                    }, 
                failure: function(response) { 
                        alert(response.responseText); 
                    } 
            }); 
        }

        function SaveLTSHDEkskul(tahun_ajaran, semester, rel_mapel, rel_siswa, nilai) {
            var s_url = "<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APIS.SMA.NILAI_SISWA.DO_SAVE.FILE + "/DoUpdateLTSHDEkskul") %>";
            s_url += "?t=" + tahun_ajaran +
                     "&sm=" + semester +
                     "&mp=" + rel_mapel +
                     "&s=" + rel_siswa +
                     "&n=" + nilai;

            $.ajax({
                url: s_url, 
                dataType: 'json',
                type: 'GET', 
                contentType: 'application/json; charset=utf-8', 
                success: function(data) { 
                    }, 
                error: function(response) { 
                        alert(response.responseText); 
                    }, 
                failure: function(response) { 
                        alert(response.responseText); 
                    } 
            }); 
        }
    </script>
    <style type="text/css">
        .modal-backdrop
        {
            opacity:0 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpNomor2" runat="server">
    
    <div id="div_statusbar" style="color: black; height: 40px; background-color: #eeeeee; padding: 10px; position: fixed; left: 0px; bottom: 0px; right: 0px; z-index: 99; box-shadow: 0 -5px 5px -5px #bcbcbc;">
        <i class="fa fa-file-text-o" style="color: green;"></i>
        &nbsp;
        <asp:Literal runat="server" ID="ltrStatusBar"></asp:Literal>
    </div>

    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="txtKeyAction" />

            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-12">

                    <div class="col-md-8 col-md-offset-2" style="padding: 0px;">
                        <div class="card" style="margin-top: -8px;">
                            <div class="card-main">
                                <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px;">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="background-color: #295BC8; padding: 10px; font-weight: normal; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                                <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/running.svg") %>"
                                                    style="margin: 0 auto; height: 25px; width: 25px;" />
                                                &nbsp;
                                                Nilai Ekstrakurikuler
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="background-color: #295BC8; padding: 0px;">
                                                <hr style="margin: 0px; border-style: solid; border-width: 1px; border-color: #2555BE;" />
                                            </td>
                                        </tr>
                                    </table>

                                    <div style="padding: 0px; margin: 0px;">
                                        <asp:Literal runat="server" ID="ltrNilaiSiswa"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="fbtn-container" id="div_button_settings" runat="server" style="z-index: 999999;" visible="false">
		        <div class="fbtn-inner">
			        <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #329CC3;" title=" Pengaturan ">
                        <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                        <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                    </a>
                    <div class="fbtn-dropup" style="z-index: 999999;">
                        
                    </div>
		        </div>
	        </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <iframe name="fra_loader" id="fra_loader" height="0" width="0" style="position: absolute; left: -1000px; top: -1000px;"></iframe>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">    
</asp:Content>
