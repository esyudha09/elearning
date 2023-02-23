<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wf.FileRaporView.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Penilaian.ALL.wf_FileRaporView" %>
<!DOCTYPE html>

<html style="background-color: #ffffff;">
<head runat="server">
    <meta charset="UTF-8">
    <meta content="IE=edge" http-equiv="X-UA-Compatible">
    <meta content="initial-scale=1.0, maximum-scale=1.0, user-scalable=no, width=device-width" name="viewport">
    <title>Al-Izhar Pondok Labu</title>

    <!-- css -->
    <link href="~/Application_Templates/material-master/css/base.min-20200113h.css" rel="stylesheet">
    <link href="~/Application_Templates/material-master/css/project.min.css" rel="stylesheet">
    <link href="~/Application_CLibs/font-awesome/css/font-awesome.min.css" rel="stylesheet">
    <link href="~/Application_CLibs/uploadercss/component.css" rel="stylesheet">
    <link href="~/Application_CLibs/main.css" rel="stylesheet">

    <style type="text/css">
        .tooltip {
            display: inline-block;
            position: relative;
            text-align: left;
        }

            .tooltip h3 {
                margin: 12px 0;
            }

            .tooltip .bottom {
                /*min-width:200px;*/
                /*max-width:400px;*/
                top: 40px;
                left: 50%;
                transform: translate(-50%, 0);
                padding: 20px;
                color: #666666;
                background-color: #EEEEEE;
                font-weight: normal;
                font-size: 13px;
                border-radius: 8px;
                position: absolute;
                z-index: 99999999;
                box-sizing: border-box;
                box-shadow: 0 1px 8px rgba(0,0,0,0.5);
                display: none;
            }

            .tooltip:hover .bottom {
                display: block;
            }

            .tooltip .bottom img {
                width: 400px;
            }

            .tooltip .bottom i {
                position: absolute;
                bottom: 100%;
                left: 50%;
                margin-left: -12px;
                width: 24px;
                height: 12px;
                overflow: hidden;
            }

                .tooltip .bottom i::after {
                    content: '';
                    position: absolute;
                    width: 12px;
                    height: 12px;
                    left: 50%;
                    transform: translate(-50%,50%) rotate(45deg);
                    background-color: #EEEEEE;
                    box-shadow: 0 1px 8px rgba(0,0,0,0.5);
                }

            .tooltip .top {
                top: -5px;
                left: 50%;
                transform: translate(-50%,-100%);
                padding: 4px 10px 4px 10px;
                color: #ffffff;
                background-color: black;
                font-weight: normal;
                font-size: 14px;
                border-radius: 4px;
                position: absolute;
                z-index: 99999999;
                box-sizing: border-box;
                box-shadow: 0 1px 8px rgba(0,0,0,0.5);
                /*display:none;*/
                visibility: hidden;
                opacity: 0;
                transition: opacity 0.8s;
            }

            .tooltip:hover .top {
                visibility: visible;
                opacity: 1;
            }

            .tooltip .top i {
                position: absolute;
                top: 100%;
                left: 50%;
                margin-left: -15px;
                width: 30px;
                height: 15px;
                overflow: hidden;
            }

            .tooltip .text-content {
                padding: 10px 20px;
            }

            .tooltip .top i::after {
                content: '';
                position: absolute;
                width: 15px;
                height: 15px;
                left: 50%;
                transform: translate(-50%,-50%) rotate(45deg);
                background-color: black;
                box-shadow: 0 1px 8px rgba(0,0,0,0.5);
            }

            .tooltip .right {
                min-width: 200px;
                max-width: 400px;
                top: 50%;
                left: 100%;
                margin-left: 20px;
                transform: translate(0, -50%);
                padding: 0;
                color: #EEEEEE;
                background-color: #444444;
                font-weight: normal;
                font-size: 13px;
                border-radius: 8px;
                position: absolute;
                z-index: 99999999;
                box-sizing: border-box;
                box-shadow: 0 1px 8px rgba(0,0,0,0.5);
                visibility: hidden;
                opacity: 0;
                transition: opacity 0.8s;
            }

            .tooltip:hover .right {
                visibility: visible;
                opacity: 1;
            }

            .tooltip .right img {
                width: 400px;
                border-radius: 8px 8px 0 0;
            }

            .tooltip .text-content {
                padding: 10px 20px;
                width: 400px;
            }

            .tooltip .right i {
                position: absolute;
                top: 50%;
                right: 100%;
                margin-top: -12px;
                width: 12px;
                height: 24px;
                overflow: hidden;
            }

                .tooltip .right i::after {
                    content: '';
                    position: absolute;
                    width: 12px;
                    height: 12px;
                    left: 0;
                    top: 50%;
                    transform: translate(50%,-50%) rotate(-45deg);
                    background-color: #444444;
                    box-shadow: 0 1px 8px rgba(0,0,0,0.5);
                }

        /**
         * Tooltip Styles
         */

        /* Add this attribute to the element that needs a tooltip */
        [data-tooltip] {
            position: relative;
            z-index: 99;
            cursor: pointer;
        }

            /* Hide the tooltip content by default */
            [data-tooltip]:before,
            [data-tooltip]:after {
                visibility: hidden;
                -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=0)";
                filter: progid: DXImageTransform.Microsoft.Alpha(Opacity=0);
                opacity: 0;
                pointer-events: none;
            }

            /* Position tooltip above the element */
            [data-tooltip]:before {
                position: absolute;
                bottom: 150%;
                left: 50%;
                margin-bottom: 5px;
                margin-left: -80px;
                padding: 7px;
                width: 160px;
                -webkit-border-radius: 3px;
                -moz-border-radius: 3px;
                border-radius: 3px;
                background-color: #000;
                background-color: hsla(0, 0%, 20%, 0.9);
                color: #fff;
                content: attr(data-tooltip);
                text-align: center;
                font-size: 14px;
                line-height: 1.2;
            }

            /* Triangle hack to make tooltip look like a speech bubble */
            [data-tooltip]:after {
                position: absolute;
                bottom: 150%;
                left: 50%;
                margin-left: -5px;
                width: 0;
                border-top: 5px solid #000;
                border-top: 5px solid hsla(0, 0%, 20%, 0.9);
                border-right: 5px solid transparent;
                border-left: 5px solid transparent;
                content: " ";
                font-size: 0;
                line-height: 0;
            }

            /* Show tooltip content on hover */
            [data-tooltip]:hover:before,
            [data-tooltip]:hover:after {
                visibility: visible;
                -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=100)";
                filter: progid: DXImageTransform.Microsoft.Alpha(Opacity=100);
                opacity: 1;
            }
    </style>
    <style type="text/css">
        .checkbox-circle, .radiobtn-circle {
            border: 2px solid rgba(130, 130, 130, 0.54);
            -webkit-transition: border-color 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            transition: border-color 0.3s cubic-bezier(0.4, 0, 0.2, 1);
        }
    </style>
    <style type="text/css">
        .mce-toolbar-grp {
            background-color: #F6F6F6 !important;
            background-image: none !important;
        }

        .mce-btn {
            background-color: #F6F6F6 !important;
        }

        .mce-tinymce.mce-container.mce-panel {
            border: 1.5px solid #E0E0E0 !important;
        }

        .btn-trans {
            background: transparent;
            border-style: none;
            font-weight: bold;
            color: grey;
        }

        .btn-nav {
            background: transparent;
            border-style: none;
            font-weight: bold;
            color: mediumvioletred;
        }

        .typeahead {
            z-index: 1051;
        }

        .label-input {
            /*color: #6C774C;*/
            color: slategrey;
            font-weight: normal;
            text-transform: uppercase;
        }

        @media screen and (max-width: 600px) {
            .textsearch {
                display: none;
            }
        }

        @media screen and (min-width: 601px) {
            .textsearch {
                display: table;
            }
        }

        .menu-backdrop.in {
            opacity: 0;
        }

        .tile-collapse.active {
            margin-bottom: 0px;
        }

        .tile.active {
            /* margin-top: 24px; */
            margin-bottom: 0px;
        }
    </style>
    <style type="text/css">
        .input-box {
            border-style: solid;
            border-width: 1px;
            border-color: #DBDBDB;
            padding: 6px;
            outline: none;
            width: 100%;
            margin-top: 5px;
            font-size: small;
        }

        .text-input {
            display: inline-block;
            padding: 6px;
            padding-left: 15px;
            padding-right: 15px;
            margin: 0;
            outline: 0;
            background-color: #F5F8FA;
            border: 1px solid #E6ECF0;
            border-radius: 30px;
            font-size: small;
        }

        .selectedrow {
            background-color: #bfede8;
        }
    </style>

    <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/tinymce/tinymce.min.js") %>"></script>
    <script type="text/javascript">
        function imageExists(image_url) {

            var http = new XMLHttpRequest();

            http.open('HEAD', image_url, false);
            http.send();

            return http.status != 404;

        }

        String.prototype.replaceAll = function (search, replacement) {
            var target = this;
            return target.split(search).join(replacement);
        };
    </script>
</head>
<body class="page-brand"
    style="padding-right: 0px; background-color: #ffffff;">
    <div id="preloader" style="background: rgba(0, 0, 0, 0.7); position: fixed; left: 0px; top: 0px; bottom: 0px; right: 0px; z-index: 9999999999">
        <div class="loder-box" style="background-color: transparent; color: #ffffff;">
            <div class="progress-circular progress-circular-white">
                <div class="progress-circular-wrapper" style="margin: 0 auto; display: table;">
                    <div class="progress-circular-inner">
                        <div class="progress-circular-left">
                            <div class="progress-circular-spinner"></div>
                        </div>
                        <div class="progress-circular-gap"></div>
                        <div class="progress-circular-right">
                            <div class="progress-circular-spinner"></div>
                        </div>
                    </div>
                </div>
            </div>
            <div style="margin: 0 auto; display: table; padding-top: 30px; font-weight: bold;">
                Sedang Proses...
            </div>
        </div>
    </div>

    <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="upMain">
            <ContentTemplate>
                <div style="margin: 0 auto; display: table;">
                    <asp:Image runat="server" ID="imgLogoAtas" ImageUrl="~/Application_Templates/material-master/images/logotrans.png" style="width: 40px; height: 60px; margin-top: 20px; " />
                </div>
                <div style="margin: 0 auto; display: table; border-style: solid; border-width: 1px; border-color: #e3e3e3; border-radius: 10px; width: 98%; margin-top: 0px; max-width: 1000px;">
                    <asp:MultiView runat="server" ID="mvMain" ActiveViewIndex="1">
                        <asp:View runat="server" ID="vNone">
                            <div style="padding: 15px; color:grey;">
                                <h3 style="margin: 0 auto; display: table">
                                    <i class="fa fa-exclamation-triangle" style="color: darkorange;"></i>
                                </h3>
                                <h5 style="margin: 0 auto; display: table">
                                    Data Tidak Ditemukan
                                </h5>
                            </div>
                        </asp:View>
                        <asp:View runat="server" ID="vListRapor">
                            <table style="margin: 0px; width: 100%;">
                                <tr>
                                    <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: center; width: 100px;">
                                        <div style="margin: 0 auto; display: table;">
                                            <img
                                                src="<%= 
                                                    ResolveUrl(AI_ERP.Application_Libs.Libs.GetImageFotoURL("~/Application_Resources/Files/Foto/Siswa/A2345.jpg"))
                                                    %>"
                                            style="height: 60px; width: 60px; border-radius: 100%; margin-bottom: 0px;" />
                                        </div>
                                    </td>
                                    <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; padding-left: 0px;">
                                        <span style="color: black; font-weight: bold; text-transform: none; text-decoration: none;">
                                            <asp:Literal runat="server" ID="ltrNamaSiswa"></asp:Literal>
                                        </span>
                                        <div class="row">
                                            <div class="col-xs-12" style="font-weight: normal;">
                                                Kelas 
                                                <span style="font-weight: bold;">
                                                    <asp:Literal runat="server" ID="ltrKelasSiswa"></asp:Literal>
                                                </span>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="font-weight: normal; color: grey; padding-top: 0px; padding-bottom: 0px;">                                    
                                        <hr style="margin: 0px;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="font-weight: normal; color: grey;">
                                        Download Rapor Tahun Pelajaran <span style="font-weight: bold;"><asp:Literal runat="server" ID="ltrTahunAjaran"></asp:Literal></span> Semester <span style="font-weight: bold;"><asp:Literal runat="server" ID="ltrSemester"></asp:Literal></span> :
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="font-weight: normal; color: grey; padding-top: 0px; padding-bottom: 0px;">                                    
                                        <hr style="margin: 0px;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="font-weight: normal; color: grey; padding-top: 0px;">
                                        <asp:MultiView runat="server" ID="mvFileRapor" ActiveViewIndex="0">
                                            <asp:View runat="server" ID="vNoFileRapor">
                                                <div style="padding: 15px; color:grey;">
                                                    <h3 style="margin: 0 auto; display: table">
                                                        <i class="fa fa-exclamation-triangle" style="color: darkorange;"></i>
                                                    </h3>
                                                    <h5 style="margin: 0 auto; display: table">
                                                        Rapor mulai bisa diunduh pada tanggal
                                                        <span style="font-weight: bold;">
                                                            <asp:Literal runat="server" ID="ltrTanggalDownloadRapor"></asp:Literal>                                                            
                                                        </span>
                                                    </h5>
                                                </div>
                                            </asp:View>
                                            <asp:View runat="server" ID="vFileRapor">
                                                <asp:Literal runat="server" ID="ltrFileRapor"></asp:Literal>
                                            </asp:View>
                                        </asp:MultiView>                                        
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                    </asp:MultiView>
                </div>
            </ContentTemplate>            
        </asp:UpdatePanel>
    </form>

    <!-- js -->
    <%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/2.2.0/jquery.min.js"></script>--%>
    <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/js/jquery.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/js/main.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/ChartJS/Chart.bundle.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/uploadercss/custom-file-input.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Application_Templates/material-master/js/base.ok.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Application_Templates/material-master/js/project.min.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/js/bootstrap3-typeahead.min.js") %>"></script>
</body>
</html>
