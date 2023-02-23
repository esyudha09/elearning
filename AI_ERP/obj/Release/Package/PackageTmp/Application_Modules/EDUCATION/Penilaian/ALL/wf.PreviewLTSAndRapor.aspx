<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.PreviewLTSAndRapor.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Penilaian.ALL.wf_PreviewLTSAndRapor" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function HideModal(){
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

            RenderDropDownOnTables();
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

        function TriggerSave(){
            tinyMCE.triggerSave();
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="upMain">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="txtID" />
            <asp:HiddenField runat="server" ID="txtKeyAction" />
            <asp:HiddenField runat="server" ID="txtCatatanVal" />
            <asp:HiddenField runat="server" ID="txtIDCatatan" />
            <asp:HiddenField runat="server" ID="txtURLRapor" />
            <asp:HiddenField runat="server" ID="txtURLLTS" />
            <asp:HiddenField runat="server" ID="txtURLNilai" />

            <div class="fbtn-container" id="div_button_settings" runat="server" style="z-index: 999999;">
		        <div class="fbtn-inner">
			        <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #329CC3;" title=" Pengaturan ">
                        <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                        <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                    </a>
                    <div class="fbtn-dropup" style="z-index: 999999;">
                        <asp:LinkButton CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" runat="server" ID="lnkOKNilaiAkademik" OnClick="lnkOKNilaiAkademik_Click" style="background-color: #424242;">
                            <span class="fbtn-text fbtn-text-left">Nilai Akademik</span>
                            <i class="fa fa-list" style="color: white;"></i>
                        </asp:LinkButton>
                    </div>
		        </div>
	        </div>

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
                                                                <span style="color: black; font-weight: bold; text-transform: none; text-decoration: none;">
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
                                                                <br /><br />
                                                                <label 
                                                                    style="font-weight: bold; cursor: pointer; color: #47098a; background-color: #f6f6f6; padding: 5px; padding-left: 15px; padding-right: 15px; border-radius: 5px; border-bottom-style: solid; border-bottom-color: #47098a;"
                                                                    onclick="alert('Preview LTS hanya dapat dilakukan oleh PU & TU'); return false; window.open(<%= txtURLLTS.ClientID %>.value.replaceAll('@sw', '<%# Eval("Kode").ToString() %>'), '_blank');"
                                                                    >
                                                                    <i class="fa fa-file-text-o"></i>&nbsp;
                                                                    Lihat Nilai LTS</label>
                                                                &nbsp;
                                                                <label 
                                                                    style="font-weight: bold; cursor: pointer; color: green; background-color: #f6f6f6; padding: 5px; padding-left: 15px; padding-right: 15px; border-radius: 5px; border-bottom-style: solid; border-bottom-color: green;"
                                                                    onclick="window.open(<%= txtURLRapor.ClientID %>.value.replaceAll('@sw', '<%# Eval("Kode").ToString() %>'), '_blank');"
                                                                    >
                                                                    <i class="fa fa-file-text-o"></i>&nbsp;
                                                                    Lihat Nilai Rapor</label>
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

                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkOKNilaiAkademik"  />
        </Triggers>
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
        RenderDropDownOnTables();
    </script>
</asp:Content>

