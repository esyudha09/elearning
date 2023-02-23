<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.Ortu.Tugas.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Elearning.wf_Ortu_Tugas" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function EndRequestHandler() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        }

        function EndRequest() {
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row" style="margin-left: 15px; margin-right: 15px;">
        <div class="col-xs-12">

            <div class="card" style="margin: 0 auto; display: table; width: 70%; min-width: 300px; max-width: 1400px; margin-top: 40px;">
				<div class="card-main">
					<div class="card-inner" style="margin-top: 5px;">
                        <div class="row">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="background-color: white; padding: 10px; font-weight: normal; vertical-align: middle; color: grey; padding-left: 20px; font-size: large;">
                                        <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/open-book.svg") %>" 
                                            style="margin: 0 auto; height: 25px; width: 25px;" />
                                        &nbsp;
                                        Tugas
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color: white; padding: 0px; padding-top: 2px; padding-bottom: 2px;">
                                        <hr style="margin : 0px;" />
                                    </td>
                                </tr>
                            </table>
                        </div>

						<div class="row">

                            <div class="col-md-12" style="padding-top: 15px; color: grey;">
                                
                                <div class="row">
                                    <div class="col-xs-4">
                                        <span style="font-weight: bold;">Bahasa Indonesia</span>&nbsp;|
                                        <span style="font-weight: bold; color: #009696;">"Majas dan Pribahasa"</span><br />
                                        <span style="font-weight: normal;">Drs. Priyono</span><br />
                                        <span style="font-weight: normal;">10-12-2017 12:34</span><br />
                                    </div>
                                    <div class="col-xs-4">
                                        <br />
                                        <span style="font-weight: normal;">Jatuh Tempo</span><br />
                                        <span style="font-weight: normal;">17-12-2017</span><br />
                                    </div>
                                    <div class="col-xs-4">
                                        <br />
                                        <span class="pull-right" style="font-weight: bold; color: orange;">
                                            <i class="fa fa-exclamation-triangle"></i>
                                            &nbsp;
                                            Belum Dikerjakan
                                        </span><br />
                                    </div>
                                </div>
                                <hr style="margin: 0px; margin-top: 10px; margin-bottom: 10px;" />

                                <div class="row">
                                    <div class="col-xs-4">
                                        <span style="font-weight: bold;">Bahasa Inggris</span>&nbsp;|
                                        <span style="font-weight: bold; color: #009696;">"Grammatical Tense"</span><br />
                                        <span style="font-weight: normal;">Annissa Asnil, SPd.</span><br />
                                        <span style="font-weight: normal;">10-12-2017 09:34</span><br />
                                    </div>
                                    <div class="col-xs-4">
                                        <br />
                                        <span style="font-weight: normal;">Jatuh Tempo</span><br />
                                        <span style="font-weight: normal;">18-12-2017</span><br />
                                    </div>
                                    <div class="col-xs-4">
                                        <br />
                                        <span class="pull-right" style="font-weight: bold; color: green;">
                                            <i class="fa fa-check-circle"></i>
                                            &nbsp;
                                            Sudah Dikerjakan
                                        </span><br />
                                    </div>
                                </div>
                                <hr style="margin: 0px; margin-top: 10px; margin-bottom: 10px;" />

                                <div class="row">
                                    <div class="col-xs-4">
                                        <span style="font-weight: bold;">Fisika</span>&nbsp;|
                                        <span style="font-weight: bold; color: #009696;">"Gravitasi dan Ruang Hampa Udara"</span><br />
                                        <span style="font-weight: normal;">Drs. Yuli Supriyanto</span><br />
                                        <span style="font-weight: normal;">01-12-2017 09:34</span><br />
                                    </div>
                                    <div class="col-xs-4">
                                        <br />
                                        <span style="font-weight: normal;">Jatuh Tempo</span><br />
                                        <span style="font-weight: normal;">18-12-2017</span><br />
                                    </div>
                                    <div class="col-xs-4">
                                        <br />
                                        <span class="pull-right" style="font-weight: bold; color: orange;">
                                            <i class="fa fa-exclamation-triangle"></i>
                                            &nbsp;
                                            Belum Dikerjakan
                                        </span><br />
                                    </div>
                                </div>
                                <hr style="margin: 0px; margin-top: 10px; margin-bottom: 10px;" />

                                <div class="row">
                                    <div class="col-xs-4">
                                        <span style="font-weight: bold;">Pendidikan Kewarganegaraan</span>&nbsp;|
                                        <span style="font-weight: bold; color: #009696;">"Demokrasi Pancasila"</span><br />
                                        <span style="font-weight: normal;">Drs. Masya Asyurur</span><br />
                                        <span style="font-weight: normal;">11-12-2017 11:34</span><br />
                                    </div>
                                    <div class="col-xs-4">
                                        <br />
                                        <span style="font-weight: normal;">Jatuh Tempo</span><br />
                                        <span style="font-weight: normal;">13-12-2017</span><br />
                                    </div>
                                    <div class="col-xs-4">
                                        <br />
                                        <span class="pull-right" style="font-weight: bold; color: green;">
                                            <i class="fa fa-check-circle"></i>
                                            &nbsp;
                                            Sudah Dikerjakan
                                        </span>
                                    </div>
                                </div>
                                <hr style="margin: 0px; margin-top: 10px; margin-bottom: 10px;" />

                                <div class="row">
                                    <div class="col-xs-4">
                                        <span style="font-weight: bold;">Bahasa Indonesia</span>&nbsp;|
                                        <span style="font-weight: bold; color: #009696;">"Majas dan Pribahasa"</span><br />
                                        <span style="font-weight: normal;">Drs. Priyono</span><br />
                                        <span style="font-weight: normal;">10-12-2017 12:34</span><br />
                                    </div>
                                    <div class="col-xs-4">
                                        <br />
                                        <span style="font-weight: normal;">Jatuh Tempo</span><br />
                                        <span style="font-weight: normal;">17-12-2017</span><br />
                                    </div>
                                    <div class="col-xs-4">
                                        <br />
                                        <span class="pull-right" style="font-weight: bold; color: orange;">
                                            <i class="fa fa-exclamation-triangle"></i>
                                            &nbsp;
                                            Belum Dikerjakan
                                        </span><br />
                                    </div>
                                </div>
                                <hr style="margin: 0px; margin-top: 10px; margin-bottom: 10px;" />

                                <div class="row">
                                    <div class="col-xs-4">
                                        <span style="font-weight: bold;">Bahasa Inggris</span>&nbsp;|
                                        <span style="font-weight: bold; color: #009696;">"Grammatical Tense"</span><br />
                                        <span style="font-weight: normal;">Annissa Asnil, SPd.</span><br />
                                        <span style="font-weight: normal;">10-12-2017 09:34</span><br />
                                    </div>
                                    <div class="col-xs-4">
                                        <br />
                                        <span style="font-weight: normal;">Jatuh Tempo</span><br />
                                        <span style="font-weight: normal;">18-12-2017</span><br />
                                    </div>
                                    <div class="col-xs-4">
                                        <br />
                                        <span class="pull-right" style="font-weight: bold; color: green;">
                                            <i class="fa fa-check-circle"></i>
                                            &nbsp;
                                            Sudah Dikerjakan
                                        </span><br />
                                    </div>
                                </div>
                                <hr style="margin: 0px; margin-top: 10px; margin-bottom: 10px;" />

                                <div class="row">
                                    <div class="col-xs-4">
                                        <span style="font-weight: bold;">Fisika</span>&nbsp;|
                                        <span style="font-weight: bold; color: #009696;">"Gravitasi dan Ruang Hampa Udara"</span><br />
                                        <span style="font-weight: normal;">Drs. Yuli Supriyanto</span><br />
                                        <span style="font-weight: normal;">01-12-2017 09:34</span><br />
                                    </div>
                                    <div class="col-xs-4">
                                        <br />
                                        <span style="font-weight: normal;">Jatuh Tempo</span><br />
                                        <span style="font-weight: normal;">18-12-2017</span><br />
                                    </div>
                                    <div class="col-xs-4">
                                        <br />
                                        <span class="pull-right" style="font-weight: bold; color: orange;">
                                            <i class="fa fa-exclamation-triangle"></i>
                                            &nbsp;
                                            Belum Dikerjakan
                                        </span><br />
                                    </div>
                                </div>
                                <hr style="margin: 0px; margin-top: 10px; margin-bottom: 10px;" />

                                <div class="row">
                                    <div class="col-xs-4">
                                        <span style="font-weight: bold;">Pendidikan Kewarganegaraan</span>&nbsp;|
                                        <span style="font-weight: bold; color: #009696;">"Demokrasi Pancasila"</span><br />
                                        <span style="font-weight: normal;">Drs. Masya Asyurur</span><br />
                                        <span style="font-weight: normal;">11-12-2017 11:34</span><br />
                                    </div>
                                    <div class="col-xs-4">
                                        <br />
                                        <span style="font-weight: normal;">Jatuh Tempo</span><br />
                                        <span style="font-weight: normal;">13-12-2017</span><br />
                                    </div>
                                    <div class="col-xs-4">
                                        <br />
                                        <span class="pull-right" style="font-weight: bold; color: green;">
                                            <i class="fa fa-check-circle"></i>
                                            &nbsp;
                                            Sudah Dikerjakan
                                        </span>
                                    </div>
                                </div>
                                <hr style="margin: 0px; margin-top: 10px; margin-bottom: 10px;" />
                                
                            </div>
                        </div>

					</div>
				</div>
			</div>

        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
</asp:Content>
