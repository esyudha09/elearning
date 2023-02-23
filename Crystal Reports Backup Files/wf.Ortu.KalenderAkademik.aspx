<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.Ortu.KalenderAkademik.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Elearning.wf_Ortu_KalenderAkademik" %>
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
                                        <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/calendar.svg") %>" 
                                            style="margin: 0 auto; height: 25px; width: 25px;" />
                                        &nbsp;
                                        Kalender Akademik
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

                            <div class="col-md-12" style="padding-top: 15px; color: black;">
                                
                                <label style="font-weight: bold; color: white; background-color: mediumvioletred; width: 100%; padding: 15px; border-style: none; border-width: 1px; border-color: #E4DFDF; margin-bottom: 0px; margin-top: 5px; box-shadow: 0 -1px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24);">
                                    <i class="fa fa-calendar"></i>
                                    &nbsp;<%= AI_ERP.Application_Libs.Libs.Array_Bulan[DateTime.Now.Month - 1] + " " + DateTime.Now.Year.ToString() %>
                                    <label class="pull-right" style="font-weight: bold; color: #00A7CC; cursor: pointer; display: none;">
                                        <img src="Application_CLibs/images/svg/calendar.svg" 
                                            style="margin: 0 auto; height: 16px; width: 16px;" />
                                        &nbsp;
                                        Selengkapnya...
                                    </label>
                                    <label class="pull-right" style="font-weight: normal; cursor: pointer;">
                                        <span style="font-weight: bold;">
                                            <%= AI_ERP.Application_Libs.Libs.GetNamaHariFromTanggal(DateTime.Now) %>,
                                        </span>
                                        <span>
                                            <%= AI_ERP.Application_Libs.Libs.GetTanggalIndonesiaSingkatFromDate(DateTime.Now, false) %>
                                        </span>
                                    </label>
                                </label>
                                <br />
                                <asp:Literal runat="server" ID="ltrKalender"></asp:Literal>
                                <div style="font-weight: bold; color: grey; width: 100%; background-color: white; padding: 15px; border-style: none; border-width: 1px; border-color: #E4DFDF; margin-bottom: 5px; margin-top: 0px; border-top-style: none;">
                                    <label style="color: grey; width: 100%; font-size: small;">
                                        <span style="color: mediumvioletred;"><i class="fa fa-calendar"></i>&nbsp; 1 Libur Nasional Maulid Nabi Muhammad SAW</span><br />
                                        <hr style="margin: 0px; margin-top: 5px; margin-bottom: 5px;" />
                                        <i class="fa fa-calendar"></i>&nbsp; 4 - 8 Ulangan Akhir Semester (UAS) I
                                        <hr style="margin: 0px; margin-top: 5px; margin-bottom: 5px;" />
                                        <span style="color: mediumvioletred;"><i class="fa fa-calendar"></i>&nbsp; 25 - 26 Libur Nasional Hari Raya Natal</span><br />
                                    </label>
                                </div>

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
