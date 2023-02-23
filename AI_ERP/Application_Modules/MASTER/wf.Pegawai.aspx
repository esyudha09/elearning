<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.Pegawai.aspx.cs" Inherits="AI_ERP.Application_Modules.MASTER.wf_Pegawai" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function EndRequestHandler() {
			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        }

        function EndRequest() {
            var jenis_act = document.getElementById("<%= txtKeyAction.ClientID %>").value;
            switch (jenis_act) {
                case "<%= JenisAction.DoChangePage %>":
                    window.scrollTo(0,0); 
                    break;
                case "<%= JenisAction.DoUpdate %>":
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                default:
                    if (jenis_act.trim() != ""){
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
    <asp:UpdatePanel runat="server" ID="upMain">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="txtID" />
            <asp:HiddenField runat="server" ID="txtKeyAction" />

            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDetail" OnClick="btnShowDetail_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowConfirmDelete" OnClick="btnShowConfirmDelete_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnDoCari" OnClick="btnDoCari_Click" style="position: absolute; left: -1000px; top: -1000px;" />

            <asp:MultiView runat="server" ID="mvMain" ActiveViewIndex="0">
                <asp:View runat="server" ID="vList">

                    <div class="row" style="margin-left: 0px; margin-right: 0px;">
                        <div class="col-xs-12">

                            <div class="col-md-8 col-md-offset-2" style="padding: 0px;">
                                <div class="card" style="margin-top: 0px;">
				                    <div class="card-main">
					                    <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px; ">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="background-color: #295BC8; padding: 10px; font-weight: normal; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                                        <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/student-3.svg") %>" 
                                                            style="margin: 0 auto; height: 25px; width: 25px;" />
                                                        &nbsp;
                                                        Data Pegawai
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="background-color: #295BC8; padding: 0px;">
                                                        <hr style="margin: 0px; border-style: solid; border-width: 1px; border-color: #2555BE;" />
                                                    </td>
                                                </tr>
                                            </table>

                                            <div style="padding: 0px; margin: 0px;">
                                                <asp:ListView ID="lvData" DataSourceID="sql_ds" runat="server" OnSorting="lvData_Sorting" OnPagePropertiesChanging="lvData_PagePropertiesChanging">
                                                    <LayoutTemplate>
                                                        <div class="table-responsive" style="margin: 0px; box-shadow: none;">
							                                <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
								                                <thead>
								                                    <tr style="background-color: #3367d6;">
                                                                        <th colspan="2" style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle; min-width: 100px;">
                                                                            &nbsp;&nbsp;&nbsp;
                                                                            Identitas Pegawai
									                                    </th>
								                                    </tr>
							                                    </thead>
							                                    <tbody>     
                                                                    <tr id="itemPlaceholder" runat="server"></tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr class="<%# (Container.DisplayIndex % 2 == 0 ? "standardrow" : "oddrow") %>">
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center;">
                                                                <div style="margin: 0 auto; display: table;">
                                                                    <ul class="nav nav-list margin-no pull-left">
										                                <li class="dropdown">
                                                                            <a class="dropdown-toggle text-black waves-attach waves-effect" data-toggle="dropdown" style="color: grey; line-height: 15px; padding: 2px; margin: 0px; min-height: 15px; z-index: 0;" title=" Menu ">
                                                                                <img
                                                                                    src="<%# 
                                                                                            ResolveUrl("~/Application_Controls/Res/ImageViewer.aspx?ID=" + Eval("Kode").ToString() + "&Jenis=Pegawai" + "&Time=" + DateTime.Now.ToString())
                                                                                         %>"
                                                                                    style="height: 60px; width: 60px; border-radius: 100%; margin-bottom: 10px;" />
                                                                                <%# 
                                                                                    AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kode").ToString().ToUpper()).Trim() != ""
                                                                                    ? "<br />" : ""
                                                                                %>
                                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none; font-size: small;">
                                                                                    <%# 
                                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kode").ToString().ToUpper())
                                                                                    %>
                                                                                </span>
                                                                                <sup style="font-size: x-small; color: black; float: right; margin-top: -65px;">
                                                                                    <%# (int)(this.Session[SessionViewDataName] == null ? 0 : this.Session[SessionViewDataName]) + (Container.DisplayIndex + 1) %>.
                                                                                </sup>
                                                                            </a>
											                                <ul class="dropdown-menu-list-table">
												                                <li style="background-color: white; padding: 10px;">
													                                <label 
                                                                                        onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetail.ClientID %>.click(); " 
                                                                                        id="btnDetail" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-info-circle" title=" Lihat Detail "></i>&nbsp;&nbsp;&nbsp;Lihat Detail</label>
												                                </li>
                                                                                <li style="padding: 0px;">
                                                                                    <hr style="margin: 0px; padding: 0px;" />
                                                                                </li>
												                                <li style="background-color: white; padding: 10px;">
													                                <label
                                                                                        onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmDelete.ClientID %>.click(); " 
                                                                                        id="btnHapus" style="color: #9E0000; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-times" title=" Hapus Data "></i>&nbsp;&nbsp;&nbsp;Hapus Data</label>
												                                </li>
											                                </ul>
										                                </li>
									                                </ul>
                                                                </div>                                                    
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Nama").ToString().ToUpper())
                                                                    %>   
                                                                    &nbsp;                                                                 
                                                                </span>
                                                                <sup class="badge" style="color: white; <%# AI_ERP.Application_Libs.Libs.GetValueToBoolean(Eval("IsNonAktif")) ? " background-color: #B7B7B7; ": " background-color: #40B3D2; " %> border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;">
                                                                    <%# 
                                                                        (
                                                                            AI_ERP.Application_Libs.Libs.GetValueToBoolean(Eval("IsNonAktif"))
                                                                            ? "Non Aktif"
                                                                            : "Aktif"
                                                                        )
                                                                    %>
                                                                </sup>
                                                                <sup class="badge" 
                                                                    style="display: initial;<%# Eval("Unit").ToString() == "" ? " display: none; " : "" %>color: white; font-weight: bold; font-size: x-small; text-transform: none; text-decoration: none; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px;">
                                                                    <%# 
                                                                        (
                                                                            Eval("Unit").ToString() != ""
                                                                            ? Eval("Unit").ToString()
                                                                            : ""
                                                                        )
                                                                    %>
                                                                </sup>
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none; float: right;">
                                                                    <label 
                                                                        onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetail.ClientID %>.click();"
                                                                        title=" Lihat Data Detail "
                                                                        style="float: right; padding: 0px; padding-left: 15px; padding-right: 15px; cursor: pointer; color: cadetblue; border-width: 1px; border-style: solid; border-color: cadetblue; border-radius: 10px; font-size: 9pt;">
                                                                        Detail
                                                                    </label>
                                                                    &nbsp;                                                                 
                                                                </span>     
                                                                <div class="row" style="width: 100%; padding-left: 10px; color: grey; <%# Eval("AlamatRumah").ToString().Trim() == "" ? " display: none; " : "" %>">
                                                                    <table style="margin: 0px;">
                                                                        <tr>
                                                                            <td style="width: 25px; padding: 0px; font-weight: normal; padding-left: 5px;">
                                                                                <%#
                                                                                    (
                                                                                        Eval("AlamatRumah").ToString().Trim() != "" 
                                                                                        ? "<i class=\"fa fa-home\" style=\"color: #bfbfbf;\"></i>"
                                                                                        : ""
                                                                                    )
                                                                                %>
                                                                            </td>
                                                                            <td style="padding: 0px; font-weight: normal; padding-right: 5px;">
                                                                                <%#
                                                                                    (
                                                                                        Eval("AlamatRumah").ToString().Trim() != "" 
                                                                                        ? 
                                                                                            (AI_ERP.Application_Libs.Libs.GetQ().Trim() != ""  ? AI_ERP.Application_Libs.Libs.GetHTMLHighLightSearch(Eval("AlamatRumah").ToString(), AI_ERP.Application_Libs.Libs.GetQ().Trim(), true) : Eval("AlamatRumah"))
                                                                                        : ""
                                                                                    )
                                                                                %>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div class="row" style="width: 100%; padding-left: 10px; color: grey; <%# Eval("Telpon").ToString().Trim() == "" && Eval("NoHP").ToString().Trim() == "" ? " display: none; " : "" %>">
                                                                    <table style="margin: 0px;">
                                                                        <tr>
                                                                            <td style="width: 25px; padding: 0px; font-weight: normal; padding-left: 5px;">
                                                                                <%#
                                                                                    (
                                                                                        Eval("Telpon").ToString().Trim().Length >= 6 || Eval("NoHP").ToString().Trim().Length >= 6 
                                                                                        ? "<i class=\"fa fa-phone\" style=\"color: #bfbfbf;\"></i>"
                                                                                        : ""
                                                                                    )
                                                                                %>
                                                                            </td>
                                                                            <td style="padding: 0px; font-weight: normal; padding-right: 5px;">
                                                                                <%#
                                                                                    (
                                                                                        Eval("Telpon").ToString().Trim().Length >= 6 
                                                                                        ? 
                                                                                            (AI_ERP.Application_Libs.Libs.GetQ().Trim() != ""  ? AI_ERP.Application_Libs.Libs.GetHTMLHighLightSearch(Eval("Telpon").ToString().Replace(",", "&nbsp;"), AI_ERP.Application_Libs.Libs.GetQ().Trim().ToString().Replace(",", "&nbsp;"), true) : Eval("Telpon").ToString().Replace(",", "&nbsp;")) +
                                                                                            "&nbsp;&nbsp;&nbsp;"
                                                                                        : ""
                                                                                    ) +
                                                                                    (
                                                                                        Eval("NoHP").ToString().Trim().Length >= 6 
                                                                                        ? 
                                                                                            (AI_ERP.Application_Libs.Libs.GetQ().Trim() != ""  ? AI_ERP.Application_Libs.Libs.GetHTMLHighLightSearch(Eval("NoHP").ToString().ToString().Replace(",", "&nbsp;"), AI_ERP.Application_Libs.Libs.GetQ().Trim().ToString().Replace(",", "&nbsp;"), true) : Eval("NoHP").ToString().Replace(",", "&nbsp;"))
                                                                                        : ""
                                                                                    )
                                                                                %>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div class="row" style="width: 100%; padding-left: 10px; color: grey; <%# Eval("Email").ToString().Trim() == "" ? " display: none; " : "" %>">
                                                                    <table style="margin: 0px;">
                                                                        <tr>
                                                                            <td style="width: 25px; padding: 0px; font-weight: normal; padding-left: 5px;">
                                                                                <%#
                                                                                    (
                                                                                        Eval("Email").ToString().Trim() != ""
                                                                                        ? "<i class=\"fa fa-envelope-o\" style=\"color: #bfbfbf; font-size: small;\"></i>"
                                                                                        : ""
                                                                                    )
                                                                                %>
                                                                            </td>
                                                                            <td style="padding: 0px; font-weight: normal; padding-right: 5px;">
                                                                                <%#
                                                                                    "<span style=\"color: #3CA1BD; font-weight: normal;\">" + Eval("Email").ToString().ToLower() + "</span>"
                                                                                %>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
								                                    <tr style="background-color: #3367d6;">
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle; min-width: 100px;">
                                                                            &nbsp;&nbsp;&nbsp;
                                                                            Identitas Siswa
									                                    </th>
								                                    </tr>
							                                    </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="text-align: center; padding: 10px; color: grey;">
                                                                            ..:: Data Kosong ::..
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </div>
                                            <asp:SqlDataSource ID="sql_ds" runat="server"></asp:SqlDataSource>

                                            <div class="content-header ui-content-header" 
                                                style="background-color: white;
                                                        box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
                                                        background-image: none; 
                                                        color: white;
                                                        display: block;
                                                        z-index: 5;
                                                        position: fixed; bottom: 28px; right: 25px; width: 350px; border-radius: 25px;
                                                        padding: 8px; margin: 0px;">
                	
                                                <div style="padding-left: 15px;">
				                                    <asp:DataPager ID="dpData" runat="server" PageSize="10" PagedControlID="lvData">
                                                        <Fields>
                                                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="btn-trans" ShowFirstPageButton="True" FirstPageText='&nbsp;<i class="fa fa-backward"></i>&nbsp;' ShowPreviousPageButton="True" PreviousPageText='&nbsp;<i class="fa fa-arrow-left"></i>&nbsp;' ShowNextPageButton="false" />
                                                            <asp:TemplatePagerField>
                                                                <PagerTemplate>
                                                                    <label style="color: grey; font-weight: bold; padding: 5px; border-style: solid; border-color: rgba(0,0,0,0.16); border-width: 1px; padding-left: 10px; padding-right: 10px; border-radius: 5px;">
                                                                        <%# ((Container.StartRowIndex + 1) / (Container.PageSize)) + 1 %>
                                                                        &nbsp;/&nbsp;
                                                                        <%# Math.Floor(Convert.ToDecimal((Container.TotalRowCount) / (Container.PageSize))) + 1 %>
                                                                    </label>
                                                                </PagerTemplate>
                                                            </asp:TemplatePagerField>
                                                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="btn-trans" ShowLastPageButton="True" LastPageText='&nbsp;<i class="fa fa-forward"></i>&nbsp;' ShowNextPageButton="True" NextPageText='&nbsp;<i class="fa fa-arrow-right"></i>&nbsp;' ShowPreviousPageButton="false" />
                                                            <asp:TemplatePagerField>
                                                                <PagerTemplate>                                                                        
                                                                    <span style="padding-top: 10px; padding-bottom: 10px; color: gray;">
                                                                        &nbsp;
                                                                        <span class="badge badge-danger">
                                                                            <asp:Label ID="ttlRcrd" runat="server" Text="<%#Container.TotalRowCount%>"></asp:Label>
                                                                        </span>
                                                                        &nbsp;
                                                                    </span>
                                                                </PagerTemplate>
                                                            </asp:TemplatePagerField>
                                                        </Fields>
                                                    </asp:DataPager>
                                                </div>
		                                    </div>  
                                            
                                            <div class="fbtn-container" id="div_button_settings" runat="server">
		                                        <div class="fbtn-inner">
			                                        <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #a91212;" title=" Pilihan ">
                                                        <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                                                        <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                                                    </a>
                                                    <div class="fbtn-dropup" style="z-index: 999999;">
				                                        <asp:LinkButton OnClick="btnRefresh_Click" CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" id="btnRefresh" title=" Refresh " style="background-color: #601B70; color: white;">
                                                            <span class="fbtn-text fbtn-text-left">Refresh Data</span>
                                                            <i class="fa fa-refresh"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton Visible="false" ToolTip=" Tambah Data " runat="server" ID="btnDoAdd" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" style="background-color: #257228;" OnClick="btnDoAdd_Click">
                                                            <span class="fbtn-text fbtn-text-left">Tambah Data</span>
                                                            <i class="fa fa-plus" style="color: white;"></i>
                                                        </asp:LinkButton>
			                                        </div>
		                                        </div>
	                                        </div> 

                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>

                </asp:View>                
            </asp:MultiView>          

        </ContentTemplate>
    </asp:UpdatePanel>  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
</asp:Content>
