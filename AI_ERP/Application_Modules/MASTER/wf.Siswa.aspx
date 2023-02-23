<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="wf.Siswa.aspx.cs" Inherits="AI_ERP.Application_Modules.MASTER.wf_Siswa" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function HideModal() {
            $('#mdlTampilanData').modal('hide');

            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();

            document.body.style.paddingRight = "0px";
        }

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
                case "<%= JenisAction.DoShowTampilkanPilihan %>":
                    $('#mdlTampilanData').modal({ backdrop: 'static', keyboard: false, show: true });
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
            document.getElementById("<%= txtKeyAction.ClientID %>").value = "";            
        }

        function ShowKelasByUnit(unit) {
            var txt_arr = document.getElementById("<%= txtParseKelasUnit.ClientID %>");
            var cbo_kelas = document.getElementById("<%= cboKelasBiodataSiswa.ClientID %>");
            if(txt_arr != null && txt_arr != undefined && cbo_kelas != null && cbo_kelas != undefined){
                if (unit.trim() != "") {
                    var arr_kelas = txt_arr.value.split(";");
                    if (arr_kelas.length > 0) {
                        if (cbo_kelas.options.length > 0) {
                            for (var i = cbo_kelas.options.length - 1; i >= 0; i--) {
                                cbo_kelas.options[i] = null;
                            }
                        }
                        var option = document.createElement("option");
                        option.value = "";
                        option.text = "(Semua)";
                        cbo_kelas.add(option);
                        for (var i = 0; i < arr_kelas.length; i++) {
                            var kk_unit = unit + '->';
                            if (arr_kelas[i].indexOf(kk_unit) >= 0) {
                                var s_kelas = arr_kelas[i].replace(kk_unit, "");                                
                                var arr_item_kelas = s_kelas.split("|");
                                if (arr_item_kelas.length === 2) {
                                    option = document.createElement("option");
                                    option.value = arr_item_kelas[0];
                                    option.text = arr_item_kelas[1];
                                    cbo_kelas.add(option);
                                }
                            }
                        }
                    }
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="upMain">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="txtID" />
            <asp:HiddenField runat="server" ID="txtKeyAction" />
            <asp:HiddenField runat="server" ID="txtParseKelasUnit" />

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
                                                        <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/student.svg") %>" 
                                                            style="margin: 0 auto; height: 25px; width: 25px;" />
                                                        &nbsp;
                                                        Data Siswa
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
                                                                            Identitas Siswa
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
                                                                                <sup style="font-size: x-small; color: grey; float: left; font-weight: normal;">
                                                                                    <%# (int)(this.Session[SessionViewDataName] == null ? 0 : this.Session[SessionViewDataName]) + (Container.DisplayIndex + 1) %>.
                                                                                </sup>
                                                                                <img
                                                                                    src="<%# 
                                                                                            ResolveUrl(AI_ERP.Application_Libs.Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/" + Eval("NIS").ToString() + ".jpg"))
                                                                                         %>"
                                                                                    style="height: 60px; width: 60px; border-radius: 100%; margin-bottom: 10px;" />
                                                                                <%# 
                                                                                    AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("NISSekolah").ToString().ToUpper()).Trim() != ""
                                                                                    ? "<br />" : ""
                                                                                %>
                                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                                    <%# 
                                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("NISSekolah").ToString().ToUpper())
                                                                                    %>
                                                                                </span>
                                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                                    <%# 
                                                                                        (
                                                                                            AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("NISSekolah").ToString().ToUpper()).Trim() != ""
                                                                                            ? " / "
                                                                                            : "<br />"
                                                                                        ) +
                                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("NIS").ToString().ToUpper())
                                                                                    %>
                                                                                </span>                                                                                
                                                                            </a>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Nama").ToString().ToUpper())
                                                                    %>
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Panggilan").ToString().ToUpper()).Trim() != ""
                                                                        ? " <span style='font-weight: normal;'>/ " + AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Panggilan").ToString().ToUpper()) + "</span>"
                                                                        : ""
                                                                    %>   
                                                                    <label 
                                                                        onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetail.ClientID %>.click();"
                                                                        title=" Lihat Data Detail "
                                                                        style="float: right; padding: 0px; padding-left: 15px; padding-right: 15px; cursor: pointer; color: cadetblue; border-width: 1px; border-style: solid; border-color: cadetblue; border-radius: 10px; font-size: 9pt;">
                                                                        Detail
                                                                    </label>
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
                                                                <sup class="badge" style="color: white; font-weight: bold; font-size: x-small; text-transform: none; text-decoration: none; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kelas").ToString())
                                                                    %>
                                                                </sup>
                                                                <div class="row" style="width: 100%; padding-left: 10px; color: grey; <%# Eval("Alamat").ToString().Trim() == "" ? " display: none; ": "" %>">
                                                                    <table style="margin: 0px;">
                                                                        <tr>
                                                                            <td style="width: 25px; padding: 0px; font-weight: normal; padding-left: 5px;">
                                                                                <%#
                                                                                    (
                                                                                        Eval("Alamat").ToString().Trim() != "" 
                                                                                        ? "<i class=\"fa fa-home\" style=\"color: #bfbfbf;\"></i>"
                                                                                        : ""
                                                                                    )
                                                                                %>
                                                                            </td>
                                                                            <td style="padding: 0px; font-weight: normal; padding-right: 5px;">
                                                                                <%#
                                                                                    (
                                                                                        Eval("Alamat").ToString().Trim() != "" 
                                                                                        ? 
                                                                                            (AI_ERP.Application_Libs.Libs.GetQ().Trim() != ""  ? AI_ERP.Application_Libs.Libs.GetHTMLHighLightSearch(Eval("Alamat").ToString(), AI_ERP.Application_Libs.Libs.GetQ().Trim(), true) : Eval("Alamat"))
                                                                                        : ""
                                                                                    )
                                                                                %>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div class="row" style="width: 100%; padding-left: 10px; color: grey; <%# Eval("TempatLahir").ToString().Trim() == "" ? " display: none; ": "" %>">
                                                                    <table style="margin: 0px;">
                                                                        <tr>
                                                                            <td style="width: 25px; padding: 0px; font-weight: normal; padding-left: 5px;">
                                                                                <%#
                                                                                    (
                                                                                        Eval("TempatLahir").ToString().Trim() != "" 
                                                                                        ? "<i class=\"fa fa-calendar\" style=\"color: #bfbfbf;\"></i>"
                                                                                        : ""
                                                                                    )
                                                                                %>
                                                                            </td>
                                                                            <td style="padding: 0px; font-weight: normal; padding-right: 5px;">
                                                                                <%#
                                                                                    (
                                                                                        Eval("TempatLahir").ToString().Trim() != ""
                                                                                        ?
                                                                                            AI_ERP.Application_Libs.Libs.GetPerbaikiEjaanNama(
                                                                                                (AI_ERP.Application_Libs.Libs.GetQ().Trim() != ""  ? AI_ERP.Application_Libs.Libs.GetHTMLHighLightSearch(Eval("TempatLahir").ToString(), AI_ERP.Application_Libs.Libs.GetQ().Trim(), true) : Eval("TempatLahir").ToString())
                                                                                            ) +
                                                                                            (
                                                                                                Eval("TanggalLahir") != DBNull.Value
                                                                                                ? ", " +
                                                                                                  AI_ERP.Application_Libs.Libs.GetTanggalIndonesiaFromDate(Convert.ToDateTime(Eval("TanggalLahir")), false)
                                                                                                : ""
                                                                                            ) 
                                                                                        : ""
                                                                                    )
                                                                                %>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div class="row" style="width: 100%; padding-left: 10px; color: grey; <%# Eval("TelpRumah").ToString().Trim() == "" && Eval("HP").ToString().Trim() == "" ? " display: none; ": "" %>">
                                                                    <table style="margin: 0px;">
                                                                        <tr>
                                                                            <td style="width: 25px; padding: 0px; font-weight: normal; padding-left: 5px;">
                                                                                <%#
                                                                                    (
                                                                                        Eval("TelpRumah").ToString().Trim().Length >= 6 || Eval("HP").ToString().Trim().Length >= 6 
                                                                                        ? "<i class=\"fa fa-phone\" style=\"color: #bfbfbf;\"></i>"
                                                                                        : ""
                                                                                    )
                                                                                %>
                                                                            </td>
                                                                            <td style="padding: 0px; font-weight: normal; padding-right: 5px;">
                                                                                <%#
                                                                                    (
                                                                                        Eval("TelpRumah").ToString().Trim().Length >= 6 
                                                                                        ? 
                                                                                            (AI_ERP.Application_Libs.Libs.GetQ().Trim() != ""  ? AI_ERP.Application_Libs.Libs.GetHTMLHighLightSearch(Eval("TelpRumah").ToString().Replace(",", "&nbsp;"), AI_ERP.Application_Libs.Libs.GetQ().Trim().ToString().Replace(",", "&nbsp;"), true) : Eval("TelpRumah").ToString().Replace(",", "&nbsp;")) +
                                                                                            "&nbsp;&nbsp;&nbsp;"
                                                                                        : ""
                                                                                    ) +
                                                                                    (
                                                                                        Eval("HP").ToString().Trim().Length >= 6 
                                                                                        ? 
                                                                                            (AI_ERP.Application_Libs.Libs.GetQ().Trim() != ""  ? AI_ERP.Application_Libs.Libs.GetHTMLHighLightSearch(Eval("HP").ToString().ToString().Replace(",", "&nbsp;"), AI_ERP.Application_Libs.Libs.GetQ().Trim().ToString().Replace(",", "&nbsp;"), true) : Eval("HP").ToString().Replace(",", "&nbsp;"))
                                                                                        : ""
                                                                                    )
                                                                                %>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div class="row" style="width: 100%; padding-left: 10px; color: grey; <%# Eval("Email").ToString().Trim() == "" ? " display: none; ": "" %>">
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
                                                                <div class="row" style="width: 100%; padding-left: 10px; color: grey; <%# Eval("NamaAyah").ToString().Trim() == "" ? " display: none; ": "" %>">
                                                                    <table style="margin: 0px;">
                                                                        <tr>
                                                                            <td style="width: 25px; padding: 0px; font-weight: normal; padding-left: 5px;">
                                                                                <%#
                                                                                    (
                                                                                        Eval("NamaAyah").ToString().Trim() != ""
                                                                                        ? "<i class=\"fa fa-male\" style=\"color: #bfbfbf; font-size: small;\"></i>"
                                                                                        : ""
                                                                                    )
                                                                                %>
                                                                            </td>
                                                                            <td style="padding: 0px; font-weight: normal; padding-right: 5px;">
                                                                                <%#
                                                                                    "<span style=\"font-weight: normal;\">" + Eval("NamaAyah") + "</span>"
                                                                                %>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div class="row" style="width: 100%; padding-left: 10px; color: grey; <%# Eval("NamaIbu").ToString().Trim() == "" ? " display: none; ": "" %>">
                                                                    <table style="margin: 0px;">
                                                                        <tr>
                                                                            <td style="width: 25px; padding: 0px; font-weight: normal; padding-left: 5px;">
                                                                                <%#
                                                                                    (
                                                                                        Eval("NamaIbu").ToString().Trim() != ""
                                                                                        ? "<i class=\"fa fa-female\" style=\"color: #bfbfbf; font-size: small;\"></i>"
                                                                                        : ""
                                                                                    )
                                                                                %>
                                                                            </td>
                                                                            <td style="padding: 0px; font-weight: normal; padding-right: 5px;">
                                                                                <%#
                                                                                    "<span style=\"font-weight: normal;\">" + Eval("NamaIbu") + "</span>"
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
                                                        position: fixed; bottom: 28px; right: 25px; width: 320px; border-radius: 25px;
                                                        padding: 8px; margin: 0px;">
                	
                                                <div style="padding-left: 15px;">
				                                    <asp:DataPager ID="dpData" runat="server" PageSize="100" PagedControlID="lvData">
                                                        <Fields>
                                                            <asp:NextPreviousPagerField ButtonType="Link" ButtonCssClass="btn-trans" ShowFirstPageButton="True" FirstPageText='&nbsp;<i class="fa fa-backward"></i>&nbsp;' ShowPreviousPageButton="True" PreviousPageText='&nbsp;<i class="fa fa-arrow-left"></i>&nbsp;' ShowNextPageButton="false" />
                                                            <asp:TemplatePagerField>
                                                                <PagerTemplate>
                                                                    <label style="color: grey; font-weight: normal; padding: 5px; border-style: solid; border-color: #F1F1F1; border-width: 1px; padding-left: 10px; padding-right: 10px; border-radius: 5px;">
                                                                        Hal.
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
                                                                        <span class="badge">
                                                                            <asp:Label ID="ttlRcrd" runat="server" Text="<%#Container.TotalRowCount%>"></asp:Label>
                                                                        </span>
                                                                    </span>
                                                                </PagerTemplate>
                                                            </asp:TemplatePagerField>
                                                        </Fields>
                                                    </asp:DataPager>
                                                </div>
		                                    </div>    

                                            <div class="fbtn-container" id="div_button_settings" runat="server">
                                                <div class="fbtn-inner">
                                                    <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #329CC3;" title=" Pilihan ">
                                                        <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                                                        <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                                                    </a>
                                                    <div class="fbtn-dropup" style="z-index: 999999;">
                                                        <asp:LinkButton ToolTip=" Tampilkan Data " runat="server" ID="btnTampilkanData" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" Style="background-color: #005b72;" OnClick="btnTampilkanData_Click">
                                                            <span class="fbtn-text fbtn-text-left">Tampilkan Data</span>
                                                            <i class="fa fa-eye" style="color: white;"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" ID="btnRefresh" title=" Refresh " Style="background-color: #601B70; color: white;" OnClick="btnRefresh_Click" >
                                                            <span class="fbtn-text fbtn-text-left">Refresh Data</span>
                                                            <i class="fa fa-refresh"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton Visible="false" ToolTip=" Tambah Data " runat="server" ID="btnDoAdd" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" Style="background-color: cadetblue;" OnClick="btnDoAdd_Click">
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
            
            <div aria-hidden="true" class="modal fade" id="mdlTampilanData" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
			        <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
				        <div class="modal-inner" 
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; 
                            padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; 
                            border-top-left-radius: 5px;
                            border-top-right-radius: 5px;
                            background-color: #EDEDED;
                            background-repeat: no-repeat;
                            background-size: auto;
                            background-position: right; 
                            background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
							    <div class="media-object margin-right-sm pull-left">
								    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
							    </div>
							    <div class="media-inner">
								    <span style="font-weight: bold;">
                                        Tampilan Data
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            
                                            <div style="padding-left: 30px; padding-right: 30px; padding-top: 0px;">
                                                <div class="row" runat="server" id="div_input_filter_unit">
                                                    <div class="col-xs-12">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= cboTampilanDataUnit.ClientID %>" style="text-transform: none;">Unit</label>
                                                            <asp:DropDownList runat="server" ID="cboTampilanDataUnit" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-xs-12">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= cboTampilanDataTahunAjaran.ClientID %>" style="text-transform: none;">Tahun Ajaran</label>
                                                            <asp:DropDownList runat="server" ID="cboTampilanDataTahunAjaran" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-xs-12">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= cboTampilanDataSemester.ClientID %>" style="text-transform: none;">Semester</label>
                                                            <asp:DropDownList runat="server" ID="cboTampilanDataSemester" CssClass="form-control">
                                                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-xs-12">
                                                        <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                            <label class="label-input" for="<%= cboKelasBiodataSiswa.ClientID %>" style="text-transform: none;">Kelas</label>
                                                            <asp:DropDownList runat="server" ID="cboKelasBiodataSiswa" CssClass="form-control">
                                                            </asp:DropDownList>
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
                                <asp:LinkButton OnClick="lnkOKShowBiodataSiswa_Click" ValidationGroup="vldInputBiodataSiswa" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKShowBiodataSiswa" Text="   Tampilkan   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
            </div>        

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkOKShowBiodataSiswa" />
            <asp:PostBackTrigger ControlID="btnRefresh" />
            <asp:PostBackTrigger ControlID="btnDoCari" />            
        </Triggers>
    </asp:UpdatePanel>  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
</asp:Content>
