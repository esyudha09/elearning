<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="wf.Guru.Praota.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Elearning.wf_Guru_Praota" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .mce-primary button, .mce-primary button i {
            background-color: darkblue;
            color: #fff;
            text-shadow: 1px 1px none;
        }
    </style>
    <script type="text/javascript">
        function HideModal(){
            $('#ui_modal_input_data').modal('hide');
            $('#ui_modal_confirm_hapus').modal('hide');
            $('#ui_modal_input_data_det').modal('hide');            

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
                case "<%= JenisAction.Add %>":
                    HideModal();
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.AddDet %>":
                    ReInitTinyMCE();                       
                    $('#ui_modal_input_data_det').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.AddWithMessage %>":
                    HideModal();
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoShowData %>":
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowDataDet %>":
                    ReInitTinyMCE();                      
                    $('#ui_modal_input_data_det').modal({ backdrop: 'static', keyboard: false, show: true });
                    UbahViewFile();
                    break;
                case "<%= JenisAction.DoShowConfirmHapus %>":
                    $('#ui_modal_confirm_hapus').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmHapusDet %>":
                    $('#ui_modal_confirm_hapus_det').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowConfirmPublish %>":
                    $('#<%= ui_modal_confirm_publish.ClientID %>').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.Update %>":
                    HideModal();
                    break;
                case "<%= JenisAction.Delete %>":
                    HideModal();
                    break;
                case "<%= JenisAction.DoAdd %>":  
                    HideModal();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoUpdate %>":
                    HideModal();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah diupdate',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    break;
                case "<%= JenisAction.DoDelete %>":
                    HideModal();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah dihapus',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    DeleteDirectory();
                    break;
                case "<%= JenisAction.DoDeleteDet %>":
                    HideModal();
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah dihapus',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    DeleteDirectoryDet();
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
            InitModalFocus();
            InitEditableTextbox();
            document.getElementById("<%= txtKeyAction.ClientID %>").value = "";            
            
            Sys.Browser.WebKit = {};
            if (navigator.userAgent.indexOf('WebKit/') > -1) {
                Sys.Browser.agent = Sys.Browser.WebKit;
                Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
                Sys.Browser.name = 'WebKit';
            }
        }

        function InitModalFocus(){
            $('#ui_modal_input_data').on('shown.bs.modal', function () {
                ShowUploader();
                <%= cboTahunAjaran.ClientID %>.focus();                
            });
            $('#ui_modal_input_data_det').on('shown.bs.modal', function () {
                ShowUploader();
                <%= txtMateriPokok.ClientID %>.focus();                
            });            
        }

        function ShowUploader(){
            var fra = document.getElementById("fraUploaderMateriPembelajaran");
            if(fra !== null && fra !== undefined) {                
                fra.src = "<%= ResolveUrl("~/Application_Resources/Uploader.aspx?jenis=" + AI_ERP.Application_Libs.Libs.JENIS_UPLOAD.MATERI_PEMBELAJARAN + "&id=") %>" + 
                           <%= txtID.ClientID %>.value + 
                          "&id2=" + 
                          (
                            <%= txtIDDet.ClientID %>.value.trim() !== "" 
                            ? <%= txtIDDet.ClientID %>.value
                            : <%= txtIDDetNew.ClientID %>.value
                          );
            }
        }

        function TriggerSave(){
            tinyMCE.triggerSave();
        }

        function ParseInput(){
            var cbo_mapel = document.getElementById("<%= cboMataPelajaran.ClientID %>");
            var cbo_kelas = document.getElementById("<%= cboKelas.ClientID %>");
            var cbo_guru = document.getElementById("<%= cboGuru.ClientID %>");

            var txt_mapel = document.getElementById("<%= txtMapel.ClientID %>");
            var txt_kelas = document.getElementById("<%= txtKelas.ClientID %>");
            var txt_guru = document.getElementById("<%= txtGuru.ClientID %>");

            if(
                cbo_mapel != null && cbo_mapel != undefined &&
                cbo_kelas != null && cbo_kelas != undefined &&
                cbo_guru != null && cbo_guru != undefined &&
                txt_mapel != null && txt_mapel != undefined &&
                txt_kelas != null && txt_kelas != undefined &&
                txt_guru != null && txt_guru != undefined
            ){
                txt_mapel.value = cbo_mapel.value;
                txt_kelas.value = cbo_kelas.value;
                txt_guru.value = cbo_guru.value;
            }
        }

        function ReInitTinyMCE(){
            LoadTinyMCEMateriPokokDeskripsi();            
        }

        var intervalDelete = null;
        function DeleteBatal(){
            var fra = document.getElementById("fraUploaderMateriPembelajaran");
            if(fra !== null && fra !== undefined) {
                var innerDoc = fra.contentDocument || fra.contentWindow.document;
                if(innerDoc != null && innerDoc != undefined){
                    ShowProsesBatal(true);

                    var id = 1;
                    var arr = innerDoc.getElementsByName("arr_delete[]");   
                    for (var i = arr.length - 1; i >= 0; i--) {
                        var btn = arr[i];
                        $.ajax({
                            url: btn.lang,
                            dataType: "json",
                            type: "GET",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) { },
                            error: function (response) { },
                            failure: function (response) { }
                        });
                        btn.click();
                        id++;
                    }

                    intervalDelete = setInterval(function(){
                        var arr = innerDoc.getElementsByName("arr_delete[]");                           
                        if(arr.length == 0){
                            ShowProsesBatal(false);
                            <%= btnBindDataDet.ClientID %>.click();
                            HideModal();
                            clearInterval(intervalDelete);                            
                        }
                    }, 1000);
                }
            } 
            else {
                HideModal();
            }
        }

        function DeleteDirectory(){
            var s_url = <%= txtURLDelete.ClientID %>.value ;
            $.ajax({
                url: s_url,
                dataType: "json",
                type: "GET",
                contentType: "application/json; charset=utf-8",
                success: function (data) { <%= btnDoHapus.ClientID %>.click(); },
                error: function (response) { <%= btnDoHapus.ClientID %>.click(); },
                failure: function (response) { }
            });            
        }

        function DeleteDirectoryDet(){
            var s_url = <%= txtURLDelete.ClientID %>.value ;
            $.ajax({
                url: s_url,
                dataType: "json",
                type: "GET",
                contentType: "application/json; charset=utf-8",
                success: function (data) { <%= btnBindDataDet.ClientID %>.click(); },
                error: function (response) { <%= btnBindDataDet.ClientID %>.click(); },
                failure: function (response) { }
            });            
        }

        function ShowProsesBatal(show){
            lbl_delete_batal_upload.style.display = (show ? "" : " none ");
            btn_batal_upload.style.display = (!show ? "" : " none ");
        }

        function DoDeleteFileClick(btn){
            btn.click();
        }

        function UbahViewFile(){
            var div_embed_video = document.getElementById("<%= div_embed_video.ClientID %>");
            var div_upload_file = document.getElementById("<%= div_upload_file.ClientID %>");
            var cbo_jenis_file = document.getElementById("<%= cboJenisFile.ClientID %>");
            if(
                div_embed_video !== null && div_embed_video !== undefined &&
                div_upload_file !== null && div_upload_file !== undefined &&
                cbo_jenis_file !== null && cbo_jenis_file !== undefined
            ){
                if(cbo_jenis_file.value === "0"){
                    div_embed_video.style.display = "none";
                    div_upload_file.style.display = "";
                }
                else if(cbo_jenis_file.value === "1"){
                    div_embed_video.style.display = "";
                    div_upload_file.style.display = "none";
                }
            }
        }

        function InitEditableTextbox(){
            $(document).on('focusin', function(e) {
                if ($(e.target).closest(".mce-window").length) {
                    e.stopImmediatePropagation();
                }
            });
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

            <asp:HiddenField runat="server" ID="txtParseListMataPelajaran" />
            <asp:HiddenField runat="server" ID="txtParseListKelas" />
            <asp:HiddenField runat="server" ID="txtParseListGuru" />
            <asp:HiddenField runat="server" ID="txtMapel" />
            <asp:HiddenField runat="server" ID="txtKelas" />
            <asp:HiddenField runat="server" ID="txtGuru" />
            <asp:HiddenField runat="server" ID="txtKeyAction" />
            <asp:HiddenField runat="server" ID="txtMateriPokokDeskripsiVal" />
            <asp:HiddenField runat="server" ID="txtID" />
            <asp:HiddenField runat="server" ID="txtIDDet" />
            <asp:HiddenField runat="server" ID="txtIDDetNew" />
            <asp:HiddenField runat="server" ID="txtURLDelete" />

            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnShowMateriPembelajaran" OnClick="btnShowMateriPembelajaran_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnDoCari" OnClick="btnDoCari_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnShowDetail" OnClick="btnShowDetail_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnShowDetailDet" OnClick="btnShowDetailDet_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnShowConfirmDelete" OnClick="btnShowConfirmDelete_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnShowConfirmDeleteDet" OnClick="btnShowConfirmDeleteDet_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnShowConfirmPublish" OnClick="btnShowConfirmPublish_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnBindDataDet" OnClick="btnBindDataDet_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnBindData" OnClick="btnBindData_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnDoHapus" OnClick="btnDoHapus_Click" style="position: absolute; left: -1000px; top: -1000px;" />

            <iframe src="<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.ALL_UNIT.DELETE_FILE.ROUTE) %>" id="fraDelete" style="position: absolute; left: -1000px; width: -1000px;"></iframe>

            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-12">

                    <div class="col-md-8 col-md-offset-2" style="padding: 0px;">
                        <div class="card" style="margin-top: 40px;">
				            <div class="card-main">
					            <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px; ">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="background-color: #295BC8; padding: 10px; font-weight: normal; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                                <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/school-material-0.svg") %>" 
                                                    style="margin: 0 auto; height: 25px; width: 25px;" />
                                                &nbsp;
                                                Materi Pembelajaran
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="background-color: #295BC8; padding: 0px;">
                                                <hr style="margin: 0px; border-style: solid; border-width: 1px; border-color: #2555BE;" />
                                            </td>
                                        </tr>
                                    </table>

						            <asp:MultiView runat="server" ID="mvMain" ActiveViewIndex="0">
                                        <asp:View runat="server" ID="vList">

                                            <div style="padding: 0px; margin: 0px;">
                                                <asp:ListView ID="lvData" DataSourceID="sql_ds" runat="server" OnSorting="lvData_Sorting" OnPagePropertiesChanging="lvData_PagePropertiesChanging">
                                                    <LayoutTemplate>
                                                        <div class="table-responsive" style="margin: 0px; box-shadow: none;">
							                                <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
								                                <thead>
								                                    <tr style="background-color: #3367d6;">
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle; width: 80px;">
                                                                            #
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            <asp:LinkButton ID="H_TahunAjaran" runat="server" CommandName="Sort" CommandArgument="TahunAjaran" style="color: white; font-weight: bold;">
                                                                                Tahun Ajaran
                                                                            </asp:LinkButton>
                                                                            <asp:Literal runat="server" ID="imgh_tahunajaran" Visible="false"></asp:Literal>
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            <asp:LinkButton ID="H_Kelas" runat="server" CommandName="Sort" CommandArgument="Kelas" style="color: white; font-weight: bold;">
                                                                                Kelas
                                                                            </asp:LinkButton>
                                                                            <asp:Literal runat="server" ID="imgh_kelas" Visible="false"></asp:Literal>
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            <asp:LinkButton ID="H_Jenis" runat="server" CommandName="Sort" CommandArgument="Guru" style="color: white; font-weight: bold;">
                                                                                Guru
                                                                            </asp:LinkButton>
                                                                            <asp:Literal runat="server" ID="imgh_guru" Visible="false"></asp:Literal>
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            <asp:LinkButton ID="H_Mapel" runat="server" CommandName="Sort" CommandArgument="Mapel" style="color: white; font-weight: bold;">
                                                                                Mata Pelajaran
                                                                            </asp:LinkButton>
                                                                            <asp:Literal runat="server" ID="imgh_mapel" Visible="false"></asp:Literal>
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
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
                                                            <td style="text-align: center; padding: 10px; vertical-align: middle;">
                                                                <%# (int)(this.Session[SessionViewDataName] == null ? 0 : this.Session[SessionViewDataName]) + (Container.DisplayIndex + 1) %>.
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <div style="margin: 0 auto; display: table; margin-left: 0px;">
                                                                    <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                        <%# 
                                                                            AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("TahunAjaran").ToString())
                                                                        %>
                                                                    </span>
                                                                    <ul class="nav nav-list margin-no pull-left" style="<%= AI_ERP.Application_Libs.Libs.URL_IDENTIFIER_ORTU.IsAdaIDUrlIdOrtu() ? " display: none; " : "" %>">
										                                <li class="dropdown">
                                                                            <a class="dropdown-toggle text-black waves-attach waves-effect" data-toggle="dropdown" style="color: grey; line-height: 15px; padding: 0px; margin: 0px; min-height: 15px; z-index: 0; margin-right: 15px;" title=" Menu ">
                                                                                <i class="fa fa-cog"></i>
                                                                                <i class="fa fa-caret-down" style="font-size: xx-small;"></i>
                                                                            </a>
											                                <ul class="dropdown-menu-list-table">
												                                <li style="background-color: white; padding: 10px;">
													                                <label 
                                                                                        onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetail.ClientID %>.click(); " 
                                                                                        id="btnDetail" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-info-circle" title=" Lihat Detail "></i>&nbsp;&nbsp;&nbsp;Lihat Detail</label>
												                                </li>
                                                                                <li style="padding: 0px;<%# (Eval("IsPublished") == DBNull.Value ? "" : (Convert.ToBoolean(Eval("IsPublished"))) ? "display: none;" : "") %>">
                                                                                    <hr style="margin: 0px; padding: 0px;" />
                                                                                </li>
												                                <li style="background-color: white; padding: 10px;<%# (Eval("IsPublished") == DBNull.Value ? "" : (Convert.ToBoolean(Eval("IsPublished"))) ? "display: none;" : "") %>">
													                                <label
                                                                                        onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmDelete.ClientID %>.click(); " 
                                                                                        id="btnHapus" style="color: #9E0000; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-times" title=" Hapus Data "></i>&nbsp;&nbsp;&nbsp;Hapus Data</label>
												                                </li>
                                                                                <li style="padding: 0px;">
                                                                                    <hr style="margin: 0px; padding: 0px;" />
                                                                                </li>
												                                <li style="background-color: white; padding: 10px;">
													                                <label
                                                                                        onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmPublish.ClientID %>.click(); " 
                                                                                        id="btnPublish" style="color: <%# (Eval("IsPublished") == DBNull.Value ? "green" : (Convert.ToBoolean(Eval("IsPublished"))) ? "darkorange" : "green") %>; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                            <i class="fa fa-check-circle"></i>&nbsp;&nbsp;&nbsp;<%# (Eval("IsPublished") == DBNull.Value ? "Publish" : (Convert.ToBoolean(Eval("IsPublished"))) ? "UnPublish" : "Publish") %>
													                                </label>
												                                </li>
											                                </ul>
										                                </li>
									                                </ul>
                                                                </div>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kelas").ToString())
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Guru").ToString())
                                                                    %>
                                                                </span>
                                                            </td>                                                                                                                        
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Mapel").ToString())
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="text-align: right;">
                                                                <%# 
                                                                    (
                                                                        (
                                                                            Eval("IsPublished") == DBNull.Value
                                                                            ? false
                                                                            : Convert.ToBoolean(Eval("IsPublished"))
                                                                        )
                                                                        ? "<i class='fa fa-check-circle' style='color: green; font-weight: bold; display:none;'></i>"
                                                                        : ""
                                                                    )
                                                                %>
                                                                <label id="img_<%# Eval("Kode").ToString().Replace("-", "_") %>" style="display: none; font-size: small; color: grey; font-weight: bold;">
                                                                    <img src="../../../../Application_CLibs/images/giphy.gif" style="height: 16px; width: 20px;" />
                                                                    &nbsp;&nbsp;Proses...
                                                                </label>
                                                                <label id="lbl_<%# Eval("Kode").ToString().Replace("-", "_") %>" onclick="this.style.display = 'none'; img_<%# Eval("Kode").ToString().Replace("-", "_") %>.style.display = ''; setTimeout(function() { <%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>';<%= btnShowMateriPembelajaran.ClientID %>.click(); }, 1000);" class="badge" style="cursor: pointer; font-weight: normal; font-size: x-small;">
                                                                    &nbsp;
                                                                    <i class="fa fa-edit"></i>
                                                                    &nbsp;
                                                                    Materi Pembelajaran
                                                                    &nbsp;
                                                                </label>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
								                                    <tr style="background-color: #3367d6;">
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle; width: 80px;">
                                                                            #
									                                    </th>
                                                                        <th style="text-align: center; background-color: #3367d6; width: 80px; vertical-align: middle;">
                                                                            <i class="fa fa-cog"></i>
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Tahun Ajaran
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Kelas
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Guru
									                                    </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">
                                                                            Mata Pelajaran
									                                    </th>
								                                    </tr>
							                                    </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="6" style="text-align: center; padding: 10px;">
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
			                                        <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #a91212;" title=" Pilihan ">
                                                        <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                                                        <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                                                    </a>
                                                    <div class="fbtn-dropup" style="z-index: 999999;">
				                                        <asp:LinkButton OnClick="btnRefresh_Click" CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" id="btnRefresh" title=" Refresh " style="background-color: #601B70; color: white;">
                                                            <span class="fbtn-text fbtn-text-left">Refresh Data</span>
                                                            <i class="fa fa-refresh"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ToolTip=" Tambah Data " runat="server" ID="btnDoAdd" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" style="background-color: #257228;" OnClick="btnDoAdd_Click">
                                                            <span class="fbtn-text fbtn-text-left">Tambah Data</span>
                                                            <i class="fa fa-plus" style="color: white;"></i>
                                                        </asp:LinkButton>
			                                        </div>
		                                        </div>
	                                        </div>

                                        </asp:View>

                                        <asp:View ID="vDetail" runat="server">

                                            <div>

                                                <div style="padding: 0px; margin: 0px;">
                                                    <div style="padding: 20px; padding-bottom: 25px;">
                                                        <asp:Literal runat="server" ID="ltrJudulMapel"></asp:Literal>
                                                    </div>
                                                    <asp:ListView ID="lvDataDet" DataSourceID="sql_ds_det" runat="server" OnSorting="lvData_Sorting" OnPagePropertiesChanging="lvData_PagePropertiesChanging">
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
                                                            <tr class="<%# (Container.DisplayIndex % 2 == 0 ? "standardrow" : "oddrow") %>">
                                                                <td style="text-align: center; padding: 10px; vertical-align: top; color: #bfbfbf; font-weight: normal; width: 50px;">
                                                                    <%# (int)(this.Session[SessionViewDataName] == null ? 0 : this.Session[SessionViewDataName]) + (Container.DisplayIndex + 1) %>.
                                                                </td>
                                                                <td style="padding: 0px; text-align: center; width: 80px; vertical-align: top;">                                                                    
                                                                    <div style="margin: 0 auto; display: table; margin-top: 5px;<%= AI_ERP.Application_Libs.Libs.URL_IDENTIFIER_ORTU.IsAdaIDUrlIdOrtu() ? " display: none; " : "" %>">
                                                                        <ul class="nav nav-list margin-no pull-left">
										                                    <li class="dropdown">
                                                                                <a class="dropdown-toggle text-black waves-attach waves-effect" data-toggle="dropdown" style="color: grey; line-height: 15px; padding: 10px; margin: 0px; min-height: 15px; z-index: 0;" title=" Menu ">
                                                                                    <i class="fa fa-cog"></i>
                                                                                    <i class="fa fa-caret-down" style="font-size: xx-small;"></i>
                                                                                </a>
											                                    <ul class="dropdown-menu-list-table">
												                                    <li style="background-color: white; padding: 10px;">
													                                    <label 
                                                                                            onclick="<%= txtIDDet.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowDetailDet.ClientID %>.click(); " 
                                                                                            id="btnDetail" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-info-circle" title=" Lihat Detail "></i>&nbsp;&nbsp;&nbsp;Lihat Detail</label>
												                                    </li>
                                                                                    <li style="padding: 0px;">
                                                                                        <hr style="margin: 0px; padding: 0px;" />
                                                                                    </li>
												                                    <li style="background-color: white; padding: 10px;">
													                                    <label
                                                                                            onclick="<%= txtIDDet.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmDeleteDet.ClientID %>.click(); " 
                                                                                            id="btnHapus" style="color: #9E0000; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;"><i class="fa fa-times" title=" Hapus Data "></i>&nbsp;&nbsp;&nbsp;Hapus Data</label>
												                                    </li>
											                                    </ul>
										                                    </li>
									                                    </ul>
                                                                    </div>
                                                                </td>
                                                                <td style="font-weight: normal; padding: 10px; vertical-align: middle; text-align: left;">
                                                                    <div>
                                                                        <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                            <%# 
                                                                                AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("TahunAjaran").ToString())
                                                                            %>
                                                                        </span>
                                                                        <sup title=" Semester " style="font-weight: normal; text-transform: none; text-decoration: none; font-weight: bold;">
                                                                            <%# 
                                                                                AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Semester").ToString())
                                                                            %>
                                                                        </sup>
                                                                    </div>
                                                                    <div>
                                                                        <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                            <%# 
                                                                                AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("MateriPokok").ToString())
                                                                            %>
                                                                        </span>
                                                                    </div>
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("DeskripsiMateriPokok").ToString()) + "<br />"
                                                                    %>
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLListUploadedFiles(this.Page, AI_ERP.Application_Libs.Libs.GetFolderElearningMateriPembelajaran(Eval("Rel_Praota").ToString(), Eval("Kode").ToString()), AI_ERP.Application_Libs.Libs.JENIS_UPLOAD.MATERI_PEMBELAJARAN, Eval("Rel_Praota").ToString(), Eval("Kode").ToString(), false)
                                                                    %>
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLEmbed(Eval("URLEmbed").ToString())
                                                                    %>
                                                                </td>                                                                
                                                            </tr>
                                                        </ItemTemplate>
                                                        <EmptyDataTemplate>
                                                            <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                                <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td colspan="3" style="text-align: center; padding: 10px;">
                                                                                ..:: Data Kosong ::..
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </EmptyDataTemplate>
                                                    </asp:ListView>
                                                </div>
                                                <asp:SqlDataSource ID="sql_ds_det" runat="server"></asp:SqlDataSource>

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
				                                        <asp:DataPager ID="DataPager1" runat="server" PageSize="100" PagedControlID="lvData">
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

                                                <div class="fbtn-container" id="div1" runat="server">
		                                            <div class="fbtn-inner">
			                                            <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #a91212;" title=" Pilihan ">
                                                            <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                                                            <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                                                        </a>
                                                        <div class="fbtn-dropup" style="z-index: 999999;">
				                                            <asp:LinkButton ToolTip=" Tambah Data " runat="server" ID="btnDoAddDet" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" style="background-color: #257228;" OnClick="btnDoAddDet_Click">
                                                                <span class="fbtn-text fbtn-text-left">Tambah Data</span>
                                                                <i class="fa fa-plus" style="color: white;"></i>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton OnClick="btnDoBack_Click" CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" id="btnDoBack" title=" Refresh " style="background-color: #601B70; color: white;">
                                                                <span class="fbtn-text fbtn-text-left">Kembali</span>
                                                                <i class="fa fa-arrow-left"></i>
                                                            </asp:LinkButton>
			                                            </div>
		                                            </div>
	                                            </div>

											</div>

                                        </asp:View>
                                    </asp:MultiView>

					            </div>
				            </div>
			            </div>
                    </div>

                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_data" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Isi Data
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">

                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboTahunAjaran.ClientID %>" style="text-transform: none;">Tahun Ajaran</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldTahunAjaran"
                                                                ControlToValidate="cboTahunAjaran" Display="Dynamic" style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList runat="server" ID="cboTahunAjaran" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboKelas.ClientID %>" style="text-transform: none;">Kelas</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldKelas"
                                                                ControlToValidate="cboKelas" Display="Dynamic" style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList runat="server" ID="cboKelas" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboMataPelajaran.ClientID %>" style="text-transform: none;">Mata Pelajaran</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldMataPelajaran"
                                                                ControlToValidate="cboMataPelajaran" Display="Dynamic" style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList runat="server" ID="cboMataPelajaran" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 30px; margin-right: 30px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboGuru.ClientID %>" style="text-transform: none;">Guru</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldGuru"
                                                                ControlToValidate="cboGuru" Display="Dynamic" style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList runat="server" ID="cboGuru" CssClass="form-control"></asp:DropDownList>
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
                                <asp:LinkButton OnClientClick="TriggerSave(); ParseInput();" ValidationGroup="vldInput" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInput" OnClick="lnkOKInput_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" onclick="TriggerSave();" data-dismiss="modal">Batal</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_data_det" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">

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
                                        Isi Data
								    </span>
							    </div>
						    </div>
                            <div style="width: 100%;">
							    <div class="row">
                                    <div class="col-lg-12">
                                        
                                        <div style="width: 100%; background-color: white; padding-top: 15px;">

                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboSemesterDet.ClientID %>" style="text-transform: none;">Semester</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputDet" runat="server" ID="vldSemesterDet"
                                                                ControlToValidate="cboSemesterDet" Display="Dynamic" style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:DropDownList runat="server" ID="cboSemesterDet" CssClass="form-control">
                                                            <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                            <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtMateriPokok.ClientID %>" style="text-transform: none;">Judul Materi</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputDet" runat="server" ID="vldMateriPokok"
                                                                ControlToValidate="txtMateriPokok" Display="Dynamic" style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" ID="txtMateriPokok" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="div_deskripsi_materi_edit" class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 0px;">
                                                        <label class="label-input" for="<%= txtMateriPokokDeskripsi.ClientID %>" style="color: slategrey; text-transform: none; margin-bottom: 6px;">
                                                            Deskripsi Materi
                                                        </label>
                                                        <asp:TextBox TextMode="MultiLine" CssClass="mcetiny_materi_pokok_deskripsi" runat="server" ID="txtMateriPokokDeskripsi" style="resize: none;"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" visible="false" id="div_deskripsi_materi_view" class="row" style="margin-left: 15px; margin-right: 15px; padding: 0px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 0px;">
                                                        <label class="label-input" style="color: slategrey; text-transform: none; margin-bottom: 6px;">
                                                            Deskripsi Materi
                                                        </label>
                                                        <div style="border-style: solid; border-color: rgba(0,0,0,.2); border-width: 1px; padding: 10px;">
                                                            <asp:Literal runat="server" ID="ltrDeskripsiMateriView"></asp:Literal>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtAlokasiWaktu.ClientID %>" style="text-transform: none;">Alokasi Waktu (Jam Pelajaran)</label>
                                                        <asp:RequiredFieldValidator ValidationGroup="vldInputDet" runat="server" ID="vldAlokasiWaktu"
                                                                ControlToValidate="txtAlokasiWaktu" Display="Dynamic" style="float: right; font-weight: bold;"
                                                                Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:TextBox runat="server" ID="txtAlokasiWaktu" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= cboJenisFile.ClientID %>" style="text-transform: none;">Jenis File</label>
                                                        <asp:DropDownList runat="server" ID="cboJenisFile" CssClass="form-control">
                                                            <asp:ListItem Value="0" Text="Upload File"></asp:ListItem>
                                                            <asp:ListItem Value="1" Text="Embed Video"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="div_embed_video" class="row" style="display: none; margin-left: 15px; margin-right: 15px;">
                                                <div class="col-xs-12" style="padding-left:  0px; padding-right: 0px;">
                                                    <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                        <label class="label-input" for="<%= txtURLEmbed.ClientID %>" style="text-transform: none;">URL Embed Video</label>
                                                        <asp:TextBox runat="server" ID="txtURLEmbed" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div runat="server" id="div_upload_file" class="row" style="margin-left: 15px; margin-right: 15px;">
                                                <div style="padding: 10px; border-style: solid; border-width: 1px; border-color: #b1e1ff; border-radius: 5px; background-color: #ddf2ff;">
                                                    <span style="font-weight: normal; color: #1DA1F2;">
                                                        <span style="font-weight: bold;">
                                                            Upload materi pembelajaran untuk orang tua
                                                        </span>
                                                        <br />
                                                        Format File : 
                                                        <br />
                                                        PDF, DOC, DOCX, PPT, PPTX, XLS, XLSX
                                                        <br />
                                                        JPG, JPEG, PNG, BMP
							                        </span>

                                                    <div class="row">
				                                        <div class="col-md-12">
					                                        
                                                            <div style="margin-top: 10px; width: 100%; overflow: hidden; padding: 0px;">
                                                                <div runat="server" id="div_upload">
                                                                    <iframe id="fraUploaderMateriPembelajaran" style="width: 100%; height: 67px; overflow: hidden; border-style: solid; border-width: 0px; border-color: #E5E5E5;" frameborder="0" scrolling="no"></iframe>
                                                                </div>
                                                                <div id="divUpload" style="width: 100%; position: absolute; left: -10000px; top: -10000px;">
                                                                    <asp:FileUpload runat="server" ID="flUpload"/>
                                                                </div>                                                                
                                                                <div id="divUploadedFiles" runat="server" visible="false">
                                                                    <asp:Literal runat="server" ID="ltrUploadedFiles"></asp:Literal>
                                                                </div>
                                                            </div>

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
                                <asp:LinkButton ValidationGroup="vldInputDet" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInputDet" OnClick="lnkOKInputDet_Click" Text="   OK   "></asp:LinkButton>
                                &nbsp;&nbsp;
                                <label id="lbl_delete_batal_upload" style="display: none; font-size: small; color: grey; font-weight: bold;">
                                    &nbsp;
                                    <img src="../../../../Application_CLibs/images/giphy.gif" style="height: 16px; width: 20px;" />
                                    &nbsp;
                                </label>
                                <a id="btn_batal_upload" onclick="DeleteBatal();" class="btn btn-flat btn-brand-accent waves-attach waves-effect">Batal</a>                                    
                                <br /><br />
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_confirm_hapus" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Konfirmasi
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
                                                            <asp:Literal runat="server" ID="ltrMsgConfirmHapus"></asp:Literal>
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapus" OnClick="lnkOKHapus_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_confirm_hapus_det" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
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
                                        Konfirmasi
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
                                                            Anda yakin akan menghapus data yang dipilih?
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKHapusDet" OnClick="lnkOKHapusDet_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>      
                                <br /><br />                              
					        </p>
				        </div>
			        </div>
		        </div>
	        </div>

            <div aria-hidden="true" class="modal fade" runat="server" id="ui_modal_confirm_publish" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 1500;">
				<div class="modal-dialog modal-xs">
					<div class="modal-content">
						<div class="modal-inner">
							<div class="media margin-bottom margin-top">
								<div class="media-object margin-right-sm pull-left">
									<span class="icon icon-lg text-brand-accent">info_outline</span>
								</div>
								<div class="media-inner">
									<span style="font-weight: bold;">
										KONFIRMASI
									</span>
								</div>
							</div>
                            <asp:Literal runat="server" ID="ltrMsgConfirmPublish"></asp:Literal>
						</div>
						<div class="modal-footer">
							<p class="text-right">
								<asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect"  runat="server" ID="btnOKPublish" OnClick="btnOKPublish_Click">
									OK
								</asp:LinkButton>
								<a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
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
        function LoadTinyMCEMateriPokokDeskripsi() {
            tfm_path = "../Application_CLibs/fileman";
            tinymce.remove();
            tinymce.init({
                selector: ".mcetiny_materi_pokok_deskripsi",
                theme: "modern",
                plugins: [
                    "advlist autolink lists link image charmap print preview hr anchor pagebreak",
                    "searchreplace wordcount visualblocks visualchars code fullscreen",
                    "insertdatetime media nonbreaking save table contextmenu directionality",
                    "emoticons template paste textcolor"
                ],
                toolbar1: "bold italic underline link",
                image_advtab: true,
                templates: [
                    { title: 'Test template 1', content: 'Test 1' },
                    { title: 'Test template 2', content: 'Test 2' }
                ],
                resize: "vertical",
                statusbar: false,
                menubar: false,
                height: 200,
                contextmenu: "cut copy paste selectall",
                setup: function (ed) {
                    ed.on('change', function (e) {
                        document.getElementById('<%= txtMateriPokokDeskripsiVal.ClientID %>').value = ed.getContent();
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
        LoadTinyMCEMateriPokokDeskripsi();
		RenderDropDownOnTables();
		InitModalFocus();
		InitEditableTextbox();
	</script>
</asp:Content>
