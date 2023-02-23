<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="wf.Guru.DataSiswa.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Elearning.wf_Guru_DataSiswa" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function HideModal(){
            $('#ui_modal_input_data').modal('hide');
            $('#ui_modal_catatan_siswa').modal('hide');            
            
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();    
            
            document.body.style.paddingRight = "0px";
        }

        function InitPicker() {
            $('#<%= txtTanggalCatatan.ClientID %>').pickdate({ cancel: '', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });
        }

        function EndRequestHandler() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        }

        function EndRequest() {
            var jenis_act = document.getElementById("<%= txtKeyAction.ClientID %>").value;

            switch (jenis_act) {
                case "<%= JenisAction.AddCatatanSiswa %>":
                    ReInitTinyMCE();   
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoUpdate %>":
                    HideModal();
                    if (jenis_act.trim() != ""){
                        $('body').snackbar({
                            alive: 2000,
                            content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                            show: function () {
                                snackbarText++;
                            }
                        });
                    }
                    break;
                default:
                    HideModal();
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
            
            ResizeIFrame();
            InitPicker();
            RenderDropDownOnTables();
            InitModalFocus();
            document.getElementById("<%= txtKeyAction.ClientID %>").value = "";            
            
            Sys.Browser.WebKit = {};
            if (navigator.userAgent.indexOf('WebKit/') > -1) {
                Sys.Browser.agent = Sys.Browser.WebKit;
                Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
                Sys.Browser.name = 'WebKit';
            }
        }

        function ReInitTinyMCE(){
            LoadTinyMCECatatan();            
        }

        function InitModalFocus(){
            $('#ui_modal_input_data').on('shown.bs.modal', function () {
                <%= cboKategoriCatatan.ClientID %>.focus();
            });
        }

        function TriggerSave(){
            tinyMCE.triggerSave();
        }
                
        function InitModalShow(){
            $('#ui_modal_catatan_siswa').on('shown.bs.modal', function () {                    
                var frm = document.getElementById("frm_catatan_siswa");            
                if(frm !== null && frm !== undefined){
                    frm.style.height = frm.contentWindow.document.body.scrollHeight + 'px';
                }
            });               
        }

        function GetDateTime(){
            var currentdate = new Date(); 
            var datetime = 
                  currentdate.getDate() +
                + (currentdate.getMonth()+1)  +
                + currentdate.getFullYear() +
                + currentdate.getHours() +  
                + currentdate.getMinutes() +
                + currentdate.getSeconds();

            return datetime;
        }

        function ShowCatatanSiswa(siswa){
            var frm = document.getElementById("frm_catatan_siswa");
            if(frm !== null && frm !== undefined){
                frm.src = "<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_DATASISWACATATAN.ROUTE) %>?" +
                          "s=" + siswa + "&" +                          
                          "t=" + GetDateTime();    
                $('#ui_modal_catatan_siswa').modal({ backdrop: 'static', keyboard: false, show: true });
            }
        }

        function ResizeIFrame(){
            setInterval(
                function(){
                    var frm = document.getElementById("frm_catatan_siswa");
                    if(frm !== null && frm !== undefined){
                        if(frm.style.height !== frm.contentWindow.document.body.scrollHeight + 'px'){
                            frm.style.height = frm.contentWindow.document.body.scrollHeight + 'px';
                        }
                    }
                }, 300
            );
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="upMain">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="txtID" />
            <asp:HiddenField runat="server" ID="txtKeyAction" />
            <asp:HiddenField runat="server" ID="txtCatatanVal" />
            <asp:HiddenField runat="server" ID="txtIDCatatan" />

            <asp:MultiView runat="server" ID="mvMain" ActiveViewIndex="0">
                <asp:View runat="server" ID="vList">

                    <div class="row" style="margin-left: 0px; margin-right: 0px;">
                        <div class="col-xs-12">

                            <div class="col-md-6 col-md-offset-3" style="padding: 0px;">
                                <div class="card" style="margin-top: 0px; box-shadow: none; border-style: solid; border-width: 1px; border-color: #dddddd; box-shadow: none;">
                                    <div class="card-main">
                                        <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: 0px; padding: 0px;">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="background-color: #295BC8; padding: 10px; font-weight: normal; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;
                                                               <asp:Literal runat="server" ID="ltrBGHeader"></asp:Literal>">
                                                        <asp:Literal runat="server" ID="ltrCaption"></asp:Literal>
                                                    </td>
                                                </tr>
                                            </table>

                                            <div style="padding: 0px; margin: 0px;">
                                                <asp:ListView ID="lvData" DataSourceID="sql_ds" runat="server" OnSorting="lvData_Sorting" OnPagePropertiesChanging="lvData_PagePropertiesChanging">
                                                    <LayoutTemplate>
                                                        <div class="table-responsive" style="margin: 0px; box-shadow: none;">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <tbody>
                                                                    <tr id="itemPlaceholder" runat="server"></tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr <%--class="<%# (Container.DisplayIndex % 2 == 0 ? "standardrow" : "oddrow") %>"--%>>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center;">
                                                                <div style="margin: 0 auto; display: table;">
                                                                    <ul class="nav nav-list margin-no pull-left">
                                                                        <li class="dropdown">
                                                                            <a class="dropdown-toggle text-black waves-attach waves-effect" data-toggle="dropdown" style="cursor: default; color: grey; line-height: 15px; padding: 2px; margin: 0px; min-height: 15px; z-index: 0;">
                                                                                <sup style="font-size: x-small; color: grey; float: left; font-weight: normal; margin-top: 10px; margin-left: -15px;">
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
                                                                <sup title=" Kelas Perwalian " class="badge" style="color: white; font-weight: bold; font-size: x-small; text-transform: none; text-decoration: none; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;">
                                                                    <%# 
                                                                        AI_ERP.Application_Modules.EDUCATION.Elearning.wf_Guru_DataSiswa.GetKelasPerwalian(Eval("Kode").ToString())
                                                                    %>
                                                                </sup>
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
                                                        <tr>
                                                            <td colspan="2" style="padding: 0px;">
                                                                <hr style="margin: 0px; border-color: #E6ECF0;" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr style="background-color: #3367d6;">
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle; min-width: 100px;">&nbsp;&nbsp;&nbsp;
                                                                            Identitas Siswa
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="text-align: center; padding: 10px; color: grey;">..:: Data Kosong ::..
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </div>
                                            <asp:SqlDataSource ID="sql_ds" runat="server"></asp:SqlDataSource>


                                            <div runat="server" id="div_catatan">

                                                <div class="content-header ui-content-header" 
                                                    style="background-color: #005b72;
                                                            box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
                                                            background-image: none; 
                                                            color: white;
                                                            display: block;
                                                            z-index: 6;
                                                            position: fixed; bottom: 32px; right: 25px; width: 245px; border-radius: 25px;
                                                            padding: 8px; margin: 0px;">
                	
                                                    <div style="padding-left: 15px; background-color: #005b72;">
				                                        <asp:HyperLink runat="server" ToolTip=" Catatan Saya " ID="lnkCatatanSaya" Style="color: white; font-weight: bold;">
                                                            <i class="fa fa-edit" style="color: white;"></i>
                                                            &nbsp;
                                                            Edit Catatan Saya
                                                        </asp:HyperLink>
                                                    </div>
		                                        </div>    

                                                <div class="fbtn-container" id="div_button_settings" runat="server">
                                                    <div class="fbtn-inner">
                                                        <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #329CC3;" title=" Pilihan ">
                                                            <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                                                            <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                                                        </a>
                                                        <div class="fbtn-dropup" style="z-index: 999999;">
                                                            <asp:LinkButton OnClick="btnRefresh_Click" CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" ID="btnRefresh" title=" Refresh " Style="background-color: #601B70; color: white;">
                                                                <span class="fbtn-text fbtn-text-left">Refresh Data</span>
                                                                <i class="fa fa-refresh"></i>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ToolTip=" Tambah Data " runat="server" ID="btnDoAdd" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" Style="background-color: cadetblue;" OnClick="btnDoAdd_Click">
                                                                <span class="fbtn-text fbtn-text-left">Tambah Data</span>
                                                                <i class="fa fa-plus" style="color: white;"></i>
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="content-header ui-content-header"
                                                    style="background-color: whitesmoke; border-style: solid; border-width: 1px; border-color: #dddddd; box-shadow: none; background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 30px; right: 0px; width: 300px; border-radius: 5px; padding: 12px; padding-right: 0px; padding-top: 0px; padding-left: 0px; padding-bottom: 0px; margin: 0px; margin-bottom: 20px;">

                                                    <div style="width: 100%; background-color: white; padding: 10px; border-bottom-color: #d3d3d3; border-bottom-style: solid; border-bottom-width: 1px;">
                                                        <span class="icon icon-lg text-brand-accent" style="color: grey;">info_outline</span>
                                                        &nbsp;
                                                        <span style="font-weight: bold; color: grey;">Catatan Khusus Siswa</span>
                                                    </div>

                                                    <div style="max-height: 300px; overflow-y: scroll; background-color: white;">

                                                        <div style="margin-bottom: 30px; background-color: white;">

                                                            <asp:ListView ID="lvCatatan" DataSourceID="sql_ds_catatan" runat="server">
                                                                <LayoutTemplate>
                                                                    <div class="table-responsive" style="margin: 0px; box-shadow: none;">
                                                                        <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                            <tbody>
                                                                                <tr id="itemPlaceholder" runat="server"></tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </div>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                            <div class="row" style="width: 100%; color: grey; padding-right: 0px; margin-left: 0px; margin-right: 0px;">
                                                                                <table style="margin: 0px; width: 100%;">
                                                                                    <tr>
                                                                                        <td style="padding-right: 5px; padding-bottom: 5px; padding-top: 5px; padding-left: 5px;">
                                                                                            <span style="font-weight: bold; color: black;">
                                                                                                <%#
                                                                                                    AI_ERP.Application_Libs.Libs.GetPersingkatNama(
                                                                                                        Eval("NamaSiswa").ToString().Trim()
                                                                                                    )                                                                                            
                                                                                                %>
                                                                                            </span>
                                                                                            <sup class="badge" style="font-size: x-small; background-color: red;">
                                                                                                <%#
                                                                                                Eval("JumlahCatatan").ToString().Trim()
                                                                                                %>
                                                                                            </sup>
                                                                                            <br />
                                                                                            <span style="font-weight: normal; color: grey; font-size: small;">
                                                                                                <%# 
                                                                                                    "<span style='font-weight: bold;'>" +
                                                                                                        Eval("UserID").ToString() +
                                                                                                    "</span>&nbsp;"
                                                                                                %>
                                                                                                <%#
                                                                                                    "@" +
                                                                                                    AI_ERP.Application_Libs.Libs.GetTanggalIndonesiaFromDate(
                                                                                                        Convert.ToDateTime(Eval("Tanggal")), false
                                                                                                    )
                                                                                                %>                                                                                        
                                                                                            </span>
                                                                                            <%#
                                                                                                "<span style=\"float: right;\">" +
                                                                                                    (
                                                                                                        Eval("Rel_Kategori").ToString().Trim().ToUpper() == AI_ERP.Application_Libs.KategoriCatatanSiswa.Pelanggaran.Kode.Trim().ToUpper()
                                                                                                        ? "<i title=\" Pelanggaran \" class=\"fa fa-exclamation-triangle\" style=\"color: orange;\"></i>"
                                                                                                        : (
                                                                                                            Eval("Rel_Kategori").ToString().Trim().ToUpper() == AI_ERP.Application_Libs.KategoriCatatanSiswa.Prestasi.Kode.Trim().ToUpper()
                                                                                                            ? "<i title=\" Prestasi \" class=\"fa fa-check-circle\" style=\"color: green;\"></i>"
                                                                                                            : "<i class=\"fa fa-info-circle\" style=\"color: #bfbfbf;\"></i>"
                                                                                                          )
                                                                                                    ) +
                                                                                                "</span>"
                                                                                            %>
                                                                                            <div style="font-weight: normal; font-size: small;">
                                                                                                <%#
                                                                                                "<div style=\"margin-top: 5px; padding-left: 10px; padding-right: 10px; background-color: white; font-weight: normal; width: 100%; border-style: solid; border-color: #c0dfd7; border-width: 1px; border-radius: 5px; background-color: #F1F9F7;\">" +
                                                                                                    (
                                                                                                        Eval("Catatan").ToString().Trim().Length > 100
                                                                                                        ? Eval("Catatan").ToString().Trim().Substring(0, 100) +
                                                                                                          (
                                                                                                                Eval("Catatan").ToString().Trim().Length > 100
                                                                                                                ? "..."
                                                                                                                : ""
                                                                                                          )
                                                                                                        : Eval("Catatan").ToString().Trim()
                                                                                                    ) +
                                                                                                "</div>"
                                                                                                %>
                                                                                                <label onclick="ShowCatatanSiswa('<%# Eval("Rel_Siswa").ToString() %>');" style="cursor: pointer; font-size: small; color: darkslateblue; float: right; margin-top: 5px;">
                                                                                                    <i class="fa fa-eye"></i>
                                                                                                    &nbsp;
                                                                                                    Lihat Detil
                                                                                                </label>
                                                                                            </div>
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
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td style="text-align: center; padding: 10px; color: grey;">..:: Data Kosong ::..
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </div>
                                                                </EmptyDataTemplate>
                                                            </asp:ListView>
                                                            <asp:SqlDataSource ID="sql_ds_catatan" runat="server"></asp:SqlDataSource>

                                                        </div>

                                                    </div>

                                                </div>

                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div aria-hidden="true" class="modal fade" id="ui_modal_input_data" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                            <div class="modal-dialog modal-xs">
                                <div class="modal-content">
                                    <div class="modal-inner"
                                        style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                                        <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: white; padding-bottom: 20px;">
                                            <div class="media-object margin-right-sm pull-left">
                                                <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                            </div>
                                            <div class="media-inner">
                                                <span style="font-weight: bold; color: black;">
                                                    Catatan Khusus Siswa
                                                </span>
                                            </div>
                                        </div>
                                        <div style="width: 100%;">
                                            <div class="row">
                                                <div class="col-lg-12">

                                                    <div style="width: 100%; background-color: white; padding-top: 15px;">
                                                        <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                            <div class="col-xs-12">
                                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                    <label class="label-input" for="<%= txtTanggalCatatan.ClientID %>" style="text-transform: none;">Kategori</label>
                                                                    <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldKategoriCatatan"
                                                                        ControlToValidate="cboKategoriCatatan" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                                        Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                    <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboKategoriCatatan" CssClass="form-control"></asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                            <div class="col-xs-12">
                                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                    <label class="label-input" for="<%= cboSiswa.ClientID %>" style="text-transform: none;">Siswa</label>
                                                                    <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldSiswa"
                                                                        ControlToValidate="cboSiswa" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                                        Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                    <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboSiswa" CssClass="form-control"></asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                            <div class="col-xs-12">
                                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                    <label class="label-input" for="<%= txtTanggalCatatan.ClientID %>" style="text-transform: none;">Tanggal</label>
                                                                    <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldTanggal"
                                                                        ControlToValidate="txtTanggalCatatan" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                                        Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                    <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtTanggalCatatan" CssClass="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                            <div class="col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                                                                <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 0px;">
                                                                    <label class="label-input" for="<%= txtCatatan.ClientID %>" style="color: black; text-transform: none; margin-bottom: 6px;">
                                                                        Catatan
                                                                    </label>
                                                                    <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldCatatan"
                                                                        ControlToValidate="txtCatatan" Display="Dynamic" Style="float: right; font-weight: bold;"
                                                                        Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                    <asp:TextBox ValidationGroup="vldInput" CssClass="mcetiny_catatan" runat="server" ID="txtCatatan" Style="height: 60px;"></asp:TextBox>
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
                                            <asp:LinkButton OnClientClick="TriggerSave()" ValidationGroup="vldInput" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInput" OnClick="lnkOKInput_Click" Text="SIMPAN DATA"></asp:LinkButton>
                                            <a onclick="TriggerSave()" class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                        </p>
                                    </div>
                                </div>
                            </div>
                        </div>
                </asp:View>
            </asp:MultiView>

            <div aria-hidden="true" class="modal fade" id="ui_modal_catatan_siswa" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

                <label title=" Tutup " data-dismiss="modal" style="padding: 10px; padding-top: 6px; padding-bottom: 6px; background-color: white; cursor: pointer; position: fixed; right: 35px; top: 20px; z-index: 999999; border-radius: 100%;">
                    <i class="fa fa-times" style="color: black; font-size: large; font-weight: normal;"></i>
                </label>

                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">Catatan Khusus Siswa
                                    </span>
                                </div>
                            </div>
                            <div style="width: 100%;">
                                <div class="row">
                                    <div class="col-lg-12">

                                        <div style="width: 100%; background-color: white; padding-top: 15px;">
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12">
                                                    <div class="row" style="margin-bottom: 5px; padding-bottom: 5px;">
                                                        <div class="col-xs-12">

                                                            <iframe src="" id="frm_catatan_siswa" frameborder="0" scrolling="no" style="width: 100%;"></iframe>

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
                            <p class="text-center">
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Tutup</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
        function LoadTinyMCECatatan() {
            tfm_path = 'Application_CLibs/fileman';
            tinymce.remove();
            tinymce.init({
                selector: ".mcetiny_catatan",
                theme: "modern",
                plugins: [
                    "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                    "searchreplace wordcount visualblocks visualchars code fullscreen",
                    "insertdatetime media nonbreaking save table contextmenu directionality",
                    "emoticons template paste textcolor tinyfilemanager.net"
                ],
                toolbar1: "bold italic underline",
                image_advtab: true,
                templates: [
                    { title: 'Test template 1', content: 'Test 1' },
                    { title: 'Test template 2', content: 'Test 2' }
                ],
                resize: "vertical",
                statusbar: false,
                menubar: false,
                height: 150,
                convert_urls: false,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtCatatanVal.ClientID %>').value = ed.getContent();
                    });

                    ed.on('init', function() 
                    {
                        ed.getBody().style.fontSize = '14px';
                    });
                }                
            });
            }
    </script>

    <script type="text/javascript">
        InitModalShow();
        RenderDropDownOnTables();
        InitModalFocus();
        ResizeIFrame();
    </script>
</asp:Content>
