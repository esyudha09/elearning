<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.Ortu.ProfilSiswa.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Elearning.wf_ProfilSiswa" %>
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
                                        <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/bedge.svg") %>" 
                                            style="margin: 0 auto; height: 25px; width: 25px;" />
                                        &nbsp;
                                        Profil Siswa
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

                            <div class="col-md-2">
                                <div style="margin: 0 auto; display: table; margin-bottom: 20px; margin-top: 20px;">
                                    <span class="avatar avatar-lg">
                                        <asp:Image runat="server" ID="imgFoto" />
                                    </span>
						        </div>
                            </div>

                            <div class="col-md-10">
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="color: grey; width: 30%; font-weight: normal; text-align: right; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            NIS Keuangan / NIS Sekolah 
                                        </td>
                                        <td style="color: grey; width: 70%; font-weight: bold; text-align: left; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            <asp:Label runat="server" ID="lblNIS"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="background-color: white; padding-top: 2px; padding-bottom: 2px;">
                                            <hr style="margin : 0px; border-color: #EEEEEE;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color: grey; width: 30%; font-weight: normal; text-align: right; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            Nama Siswa
                                        </td>
                                        <td style="color: grey; width: 70%; font-weight: bold; text-align: left; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            <asp:Label runat="server" ID="lblNamaSiswa"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="background-color: white; padding-top: 2px; padding-bottom: 2px;">
                                            <hr style="margin : 0px; border-color: #EEEEEE;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color: grey; width: 30%; font-weight: normal; text-align: right; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            Panggilan
                                        </td>
                                        <td style="color: grey; width: 70%; font-weight: bold; text-align: left; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            <asp:Label runat="server" ID="lblPanggilan"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="background-color: white; padding-top: 2px; padding-bottom: 2px;">
                                            <hr style="margin : 0px; border-color: #EEEEEE;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color: grey; width: 30%; font-weight: normal; text-align: right; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            Unit Sekolah
                                        </td>
                                        <td style="color: grey; width: 70%; font-weight: bold; text-align: left; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            <asp:Label runat="server" ID="lblUnitSekolah"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="background-color: white; padding-top: 2px; padding-bottom: 2px;">
                                            <hr style="margin : 0px; border-color: #EEEEEE;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color: grey; width: 30%; font-weight: normal; text-align: right; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            Kelas
                                        </td>
                                        <td style="color: grey; width: 70%; font-weight: bold; text-align: left; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            <asp:Label runat="server" ID="lblKelas"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="background-color: white; padding-top: 2px; padding-bottom: 2px;">
                                            <hr style="margin : 0px; border-color: #EEEEEE;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color: grey; width: 30%; font-weight: normal; text-align: right; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            Status
                                        </td>
                                        <td style="color: grey; width: 70%; font-weight: bold; text-align: left; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            <asp:Literal runat="server" ID="ltrStatusSiswa"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="background-color: white; padding-top: 2px; padding-bottom: 2px;">
                                            <hr style="margin : 0px; border-color: #EEEEEE;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color: grey; width: 30%; font-weight: normal; text-align: right; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            Tempat, Tanggal lahir
                                        </td>
                                        <td style="color: grey; width: 70%; font-weight: bold; text-align: left; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            <asp:Label runat="server" ID="lblTTL"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="background-color: white; padding-top: 2px; padding-bottom: 2px;">
                                            <hr style="margin : 0px; border-color: #EEEEEE;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color: grey; width: 30%; font-weight: normal; text-align: right; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            Alamat
                                        </td>
                                        <td style="color: grey; width: 70%; font-weight: bold; text-align: left; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            <asp:Label runat="server" ID="lblAlamat"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="background-color: white; padding-top: 2px; padding-bottom: 2px;">
                                            <hr style="margin : 0px; border-color: #EEEEEE;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color: grey; width: 30%; font-weight: normal; text-align: right; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            Email
                                        </td>
                                        <td style="color: grey; width: 70%; font-weight: bold; text-align: left; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            <i class="fa fa-envelope" style="color: #40B3D2;"></i>
                                            &nbsp;
                                            <asp:Label runat="server" ID="lblEmail" style="color: #40B3D2;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="background-color: white; padding-top: 2px; padding-bottom: 2px;">
                                            <hr style="margin : 0px; border-color: #EEEEEE;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color: grey; width: 30%; font-weight: normal; text-align: right; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            Nama Ayah
                                        </td>
                                        <td style="color: grey; width: 70%; font-weight: bold; text-align: left; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            <asp:Label runat="server" ID="lblNamaAyah"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="background-color: white; padding-top: 2px; padding-bottom: 2px;">
                                            <hr style="margin : 0px; border-color: #EEEEEE;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color: grey; width: 30%; font-weight: normal; text-align: right; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            Nama Ibu
                                        </td>
                                        <td style="color: grey; width: 70%; font-weight: bold; text-align: left; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            <asp:Label runat="server" ID="lblNamaIbu"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="background-color: white; padding-top: 2px; padding-bottom: 2px;">
                                            <hr style="margin : 0px; border-color: #EEEEEE;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="color: grey; width: 30%; font-weight: normal; text-align: right; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            Tahun Pelajaran
                                        </td>
                                        <td style="color: grey; width: 70%; font-weight: bold; text-align: left; background-color: white; padding-left: 8px; padding-right: 8px;">
                                            <asp:Label runat="server" ID="lblTahunAjaran"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
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
