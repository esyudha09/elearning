<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploaderSingle.aspx.cs" Inherits="AI_ERP.Application_Resources.UploaderSingle" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title></title>
        <%--<link rel="stylesheet" href="http://netdna.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css">--%>
        <link rel="stylesheet" href="<%= ResolveUrl("~/Application_Templates/bootstrap/css/bootstrap.css") %>" type="text/css" />
        <link rel="stylesheet" href="<%= ResolveUrl("~/Application_Templates/front/css/font-awesome.min.css") %>" type="text/css" />    
        <link rel="stylesheet" href="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/css/style.css") %>" type="text/css" />            

        <link rel="stylesheet" href="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/css/jquery.fileupload-ui.css") %>" type="text/css" />
        <link rel="stylesheet" href="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/css/jquery.fileupload.css") %>" type="text/css" />

        <link href="https://fonts.googleapis.com/css?family=Raleway:200,400,700,800" rel="stylesheet" type="text/css" />
		<link href="http://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />

        <noscript>
            <link href="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/css/jquery.fileupload-noscript.css") %>" rel="stylesheet" type="text/css" />
        </noscript>
        <noscript>
            <link href="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/css/jquery.fileupload-ui-noscript.css") %>" rel="stylesheet" type="text/css" />
        </noscript>
        <script type="text/javascript">
            function ResizeFrame() {
                if (window.parent.fraUploaderFilePendukung != null && window.parent.fraUploaderFilePendukung != undefined) {
                    if (window.parent.fraUploaderFilePendukung.contentWindow.document.body != null && window.parent.fraUploaderFilePendukung.contentWindow.document.body != undefined) {
                        window.parent.fraUploaderFilePendukung.style.height = (window.parent.fraUploaderFilePendukung.contentWindow.document.body.scrollHeight - 18) + 'px';
                    }
                }
            }
            function ResizeFrameDel() {
                if (window.parent.fraUploaderFilePendukung != null && window.parent.fraUploaderFilePendukung != undefined) {
                    if (window.parent.fraUploaderFilePendukung.contentWindow.document.body != null && window.parent.fraUploaderFilePendukung.contentWindow.document.body != undefined) {
                        window.parent.fraUploaderFilePendukung.style.height = (window.parent.fraUploaderFilePendukung.contentWindow.document.body.scrollHeight - 69) + 'px';
                    }
                }
            }
            function HideButtonSave() {
                
            }
            function ShowButtonSave() {
                
            }
            function ShowImage() {
                if (window.parent.fraShowFoto != null && window.parent.fraShowFoto != undefined) {
                    if (window.parent.fraShowFoto.contentWindow.document.body != null && window.parent.fraShowFoto.contentWindow.document.body != undefined) {                        
                        window.parent.fraShowFoto.src = window.parent.fraShowFoto.src;
                    }
                }
            }
            setInterval(
                    function () {
                        var arr_doc = document.getElementsByName("proses_upload[]");
                        if (arr_doc.length > 0) {
                            HideButtonSave();
                        }
                        else {
                            ShowButtonSave();
                        }
                        ResizeFrame();
                    },
                    200
                );
            setInterval(
                    function () {
                        ResizeFrame();
                    },
                    10
                );
        </script>
    </head>
    <body style="background-color: #ddf2ff; padding: 0px; padding-left: 0px; padding-top: 0px; margin: 0px; font-family: 'Raleway', Arial, sans-serif">
        <div>
            <form id="fileupload" 
                action="<%= ResolveUrl("~/Application_Resources/UploadHandler_2_0.aspx?jenis=" + AI_ERP.Application_Libs.Libs.GetQueryString("jenis") + "&id=" + AI_ERP.Application_Libs.Libs.GetQueryString("id") + "&id2=" + AI_ERP.Application_Libs.Libs.GetQueryString("id2")) %>" 
                method="POST" enctype="multipart/form-data">
                <iframe id="fraDelete" style="position: absolute; left: -1000px; top: -1000px; height: 100px; width: 100px;"></iframe>
                <!-- Redirect browsers with JavaScript disabled to the origin page -->
                <noscript>
                    <input type="hidden" name="redirect" value="https://blueimp.github.io/jQuery-File-Upload/">
                </noscript>
                <!-- The fileupload-buttonbar contains buttons to add/delete files and start/cancel the upload -->
                <div class="row fileupload-buttonbar" style="padding-left: 15px;">
                    <div class="col-lg-7" style="margin-top: 5px; margin-bottom: 15px; padding-left: 0px; padding-right: 0px;">
                        <!-- The fileinput-button span is used to style the file input field as button -->
                        <span class="btn btn-success fileinput-button" style="width: 100%; border-radius: 0px; margin: 0px; background-color: #1DA1F2; font-size: small; border-style: none;">
                            <i class="fa fa-file-o"></i>
                            &nbsp;
                            <span style="font-family: 'Raleway', Arial, sans-serif;">Pilih file yang akan di-upload...</span>
                            <input type="file" name="files[]" multiple="multiple">
                        </span>
                        <button type="submit" class="btn btn-primary start" id="btnDoUpload" style="position: absolute; left: -1000px; top: -1000px; border-radius: 0px; margin: 0px;">
                            <i class="glyphicon glyphicon-upload"></i>
                            <span>Upload</span>
                        </button>
                        <button type="reset" class="btn btn-warning cancel" style="border-radius: 0px; margin: 0px; background-color: #C78528; display: none;">
                            <i class="glyphicon glyphicon-ban-circle"></i>
                            <span>Batal upload</span>
                        </button>
                        <!-- The global file processing state -->
                        <span class="fileupload-process"></span>
                    </div>
                    <!-- The global progress state -->
                    <div class="col-lg-5 fileupload-progress" style="width: 100%; padding-right: 0px; display: none;">
                        <!-- The global progress bar -->
                        <div class="progress progress-striped active" role="progressbar" aria-valuemin="0" aria-valuemax="100" style="margin-bottom: 0px; margin-top: 10px;">
                            <div class="progress-bar progress-bar-success" style="width: 0%;"></div>
                        </div>
                        <!-- The extended global progress state -->
                        <div class="progress-extended">&nbsp;</div>
                    </div>
                </div>
                <!-- The table listing the files available for upload/download -->
                <div style="overflow-y: auto; width: 100%; border-top-style: solid; border-top-width: 1px; border-top-color: #E5E5E5;">
                    <table role="presentation" class="table table-striped" style="width: 100%;">
                        <tbody class="files"></tbody>
                    </table>
                </div>
            </form>
        </div>

        <!-- The template to display files available for upload -->
        <script id="template-upload" type="text/x-tmpl">
            {% HideButtonSave(); %}
            {% for (var i=0, file; file=o.files[i]; i++) { %}
                <tr class="template-upload{{if error}} ui-state-error{{/if}}">
                    <td style="vertical-align: middle; display: none;">
                        <span class="preview"></span>
                    </td>
                    <td class="col-xs-8" style="vertical-align: middle;">
                        <p class="name" style="margin: 0">{%=file.name%}</p>
                        <strong class="error text-danger"></strong>
                    </td>
                    <td class="col-xs-2" style="vertical-align: middle;">
                        <span name="proses_upload[]" class="size" style="margin: 0">Processing...</span>
                    </td>
                    <td class="col-xs-2" style="vertical-align: middle; padding-top: 20px;">
                        <div class="progress progress-striped active" role="progressbar" aria-valuemin="0" aria-valuemax="100" aria-valuenow="0"><div class="progress-bar progress-bar-success" style="width: 0%;"></div></div>
                    </td>
                </tr>
                {% break; %}
            {% } %}
        </script>
        
        <!-- The template to display files available for download -->
        <script id="template-download" type="text/x-tmpl">
            {% for (var i=0, file; file=o.files[i]; i++) { %}
                <tr class="template-download fade" <%= AI_ERP.Application_Libs.Libs.GetQueryString("jenis") == "foto" ? "style= \"display: none;\" " : "style= \"background-color: white;\" " %>
                    <td style="vertical-align: middle;">
                        <span class="preview">
                            {% if (file.thumbnailUrl) { %}
                                <a href="{%=file.url%}" title="{%=file.name%}" download="{%=file.name%}" data-gallery><img src="{%=file.thumbnailUrl%}"></a>
                            {% } %}
                        </span>
                    </td>
                    <td style="vertical-align: middle;">
                        <p class="name" style="margin: 0">
                            {% if (file.url) { %}
                                {%=file.name%}
                            {% } else { %}
                                <span>{%=file.name%}</span>
                            {% } %}
                        </p>
                        {% if (file.error) { %}
                            <div><span class="label label-danger">Error</span> {%=file.error%}</div>
                        {% } %}
                    </td>
                    <td style="vertical-align: middle;">
                        <span class="size">{%=o.formatFileSize(file.size)%}</span>
                    </td>
                    <td style="text-align: right; vertical-align: middle;">
                        <span class="size"></span>
                        {% if (file.deleteUrl) { %}
                            <button title=" Hapus File " name="arr_delete[]" class="btn btn-danger delete" style="border-style: none; background: transparent; color: #B93221; outline: none;" lang='<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.ALL_UNIT.DELETE_FILE.ROUTE + "?jenis=" + AI_ERP.Application_Libs.Libs.GetQueryString("jenis")) %>&id=<%= AI_ERP.Application_Libs.Libs.GetQueryString("id") %>&id2=<%= AI_ERP.Application_Libs.Libs.GetQueryString("id2") %>&file={%=file.name.replace(/'/g,'[kutip_satu]').replace(/&/g,'[dan]')%}' onclick="document.getElementById('fraDelete').src = '<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.LOADER.ALL_UNIT.DELETE_FILE.ROUTE + "?jenis=" + AI_ERP.Application_Libs.Libs.GetQueryString("jenis")) %>&id=<%= AI_ERP.Application_Libs.Libs.GetQueryString("id") %>&id2=<%= AI_ERP.Application_Libs.Libs.GetQueryString("id2") %>&file={%=file.name.replace(/'/g,'[kutip_satu]').replace(/&/g,'[dan]')%}';">
                                <i class="fa fa-times"></i>
                            </button>
                            <input type="checkbox" name="delete" value="1" class="toggle" style="display: none">
                        {% } else { %}
                            <button class="btn btn-warning cancel" style="display: none">
                                <i class="glyphicon glyphicon-ban-circle"></i>
                                <span>Cancel</span>
                            </button>
                        {% } %}
                    </td>
                </tr>
                {% ShowImage(); %}
                {% break; %}
            {% } %}            
        </script>

        <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/js/vendor/jquery.ui.widget.js") %>"></script>
        <script type="text/javascript" src="http://blueimp.github.io/JavaScript-Templates/js/tmpl.min.js"></script>
        <script type="text/javascript" src="http://blueimp.github.io/JavaScript-Load-Image/js/load-image.all.min.js"></script>
        <script type="text/javascript" src="http://blueimp.github.io/JavaScript-Canvas-to-Blob/js/canvas-to-blob.min.js"></script>
        <script type="text/javascript" src="http://netdna.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
        <script type="text/javascript" src="http://blueimp.github.io/Gallery/js/jquery.blueimp-gallery.min.js"></script>--%>

        <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/js/jquery.min.js") %>"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/js/vendor/jquery.ui.widget.js") %>"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/js/tmpl.min.js") %>"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/js/load-image.all.min.js") %>"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/js/canvas-to-blob.min.js") %>"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/js/bootstrap.min.js") %>"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/js/jquery.blueimp-gallery.min.js") %>"></script>

        <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/js/vendor/jquery.ui.widget.js") %>"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/js/jquery.iframe-transport.js") %>"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/js/jquery.fileupload.js") %>"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/js/jquery.fileupload-process.js") %>"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/js/jquery.fileupload-image.js") %>"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/js/jquery.fileupload-audio.js") %>"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/js/jquery.fileupload-video.js") %>"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/js/jquery.fileupload-validate.js") %>"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/js/jquery.fileupload-ui.js") %>"></script>
        <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/uploader-jquery/js/main.js") %>"></script>
        <!-- The XDomainRequest Transport is included for cross-domain file deletion for IE 8 and IE 9 -->
        <!--[if (gte IE 8)&(lt IE 10)]>
    <script src="js/cors/jquery.xdr-transport.js"></script>
    <![endif]-->
    </body>
</html>