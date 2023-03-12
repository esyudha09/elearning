<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.DesignSoal.aspx.cs" Inherits="AI_ERP.Application_Modules.CBT.wf_DesignSoal" %>

<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/js/jquery.min.js") %>"></script>
    <script type="text/javascript">

        var currentValue = 0;



        function GoToURL(url) {
            document.location.href = url;
        }

        function GoToURL2(url) {
            let params = `scrollbars=no,resizable=no,status=no,location=no,toolbar=no,menubar=no,width=0,height=0,left=-1000,top=-1000`;

            open(url, 'test', params);
        }

        function btnDoCariSoal(event) {
            if (event.keyCode == 13) {
                document.getElementById("<%= btnDoCariSoal.ClientID %>").click();
                HideModal();
                return true;
            } else {
                return false;
            }
        }
        function HideModal() {
            $('#ui_modal_input_data').modal('hide');
            $('#ui_modal_confirm_hapus').modal('hide');

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
                    window.scrollTo(0, 0);
                    break;
                case "<%= JenisAction.Add %>":
                    $('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.ViewSoal %>":
                    $('#ui_modal_input_data2').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.AddWithMessage %>":
                    HideModal();
                    //$('#ui_modal_input_data').modal({ backdrop: 'static', keyboard: false, show: true });
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
                case "<%= JenisAction.DoShowConfirmHapus %>":
                    $('#ui_modal_confirm_hapus').modal({ backdrop: 'static', keyboard: false, show: true });
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
                    break;
                case "<%= JenisAction.DoShowUpdateSkor %>":
                    $('#ui_modal_update_skor').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                case "<%= JenisAction.DoShowUpdateUrut %>":
                    $('#ui_modal_update_urut').modal({ backdrop: 'static', keyboard: false, show: true });
                    break;
                default:
                    HideModal();
                    if (jenis_act.trim() != "") {
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
            document.getElementById("<%= txtKeyAction.ClientID %>").value = "";

            Sys.Browser.WebKit = {};
            if (navigator.userAgent.indexOf('WebKit/') > -1) {
                Sys.Browser.agent = Sys.Browser.WebKit;
                Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
                Sys.Browser.name = 'WebKit';
            }
        }

        function InitModalFocus() {
           <%-- $('#ui_modal_input_data').on('shown.bs.modal', function () {
                if (document.getElementById("<%= txtSoal.ClientID %>") !== undefined && document.getElementById("<%= txtSoal.ClientID %>") !== null) {
                    document.getElementById("<%= txtJawaban.ClientID %>").focus();
                }
                else {
                    document.getElementById("<%= txtSoal.ClientID %>").focus();
                }
            });--%>
        }

        function TriggerSave() {
            tinyMCE.triggerSave();
        }
        function btnAddSoal() {
            alert("asdf")
        };


        function tes() {
            alert("asdfaf")
        }

    </script>

    <script>
        $(document).ready(function () {
            //$("#txtCariSoal").keyup(function (event) {
            //    alert("asdfa")
            //    if (event.keyCode === 13) {
            //        $("#myButton").click();
            //    }
            //});
        });
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

            <asp:HiddenField runat="server" ID="txtKeyAction" />
            <asp:HiddenField runat="server" ID="txtID" />
            <asp:HiddenField runat="server" ID="id_login" />
            <asp:HiddenField runat="server" ID="txtSoalID" />
            <asp:HiddenField runat="server" ID="txtSkorVal" />
            <asp:HiddenField runat="server" ID="txtUrutVal" />




            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnDoCari" OnClick="btnDoCari_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnDoCariSoal" OnClick="btnDoCariSoal_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <%--<asp:Button runat="server" UseSubmitBehavior="false" ID="btnShowSoalDetail" OnClick="btnShowSOALDetail_Click" Style="position: absolute; left: -1000px; top: -1000px;" />--%>
            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnUpdateSkor" OnClick="btnDoUpdateSkor_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnUpdateUrut" OnClick="btnDoUpdateUrut_Click" Style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button runat="server" UseSubmitBehavior="false" ID="btnShowConfirmDelete" OnClick="btnShowConfirmDelete_Click" Style="position: absolute; left: -1000px; top: -1000px;" />

            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-xs-12">

                    <div class="col-md-8 col-md-offset-2" style="padding: 0px;">
                        <div class="card" style="margin-top: 40px;">
                            <div class="card-main">
                                <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px;">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="background-color: #295BC8; padding: 10px; font-weight: normal; vertical-align: middle; color: white; padding-left: 20px; font-size: 15px;">
                                                <img src="<%= ResolveUrl("~/Application_CLibs/images/svg/document.svg") %>"
                                                    style="margin: 0 auto; height: 25px; width: 25px;" />
                                                &nbsp;
                                                Data Design Soal
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
                                                <asp:ListView ID="lvData" DataSourceID="sql_ds" runat="server">
                                                    <LayoutTemplate>
                                                        <div class="table-responsive" style="margin: 0px; box-shadow: none;">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr style="background-color: #3367d6;">
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle; width: 80px;">No.Urut
                                                                        </th>
                                                                        <th style="text-align: center; background-color: #3367d6; width: 80px; vertical-align: middle;">
                                                                            <i class="fa fa-cog"></i>
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">Soal
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">Jenis
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">Skor
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
                                                            <%-- <td style="text-align: center; padding: 10px; vertical-align: middle; color: #bfbfbf;">
                                                                <%# (int)(this.Session[SessionViewDataName] == null ? 0 : this.Session[SessionViewDataName]) + (Container.DisplayIndex + 1) %>.
                                                            </td>--%>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <a href="javascript:void(0);" style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <i class="fa fa-edit" onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>';<%# txtUrutVal.ClientID%>.value = '<%# Eval("Urut").ToString() %>'; <%= btnUpdateUrut.ClientID %>.click(); "></i>
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Urut").ToString())
                                                                    %>  &nbsp;                                                                                                                                                                                            
                                                                </a>
                                                            </td>
                                                            <td style="padding: 0px; text-align: center; width: 80px; vertical-align: middle;">
                                                                <div style="margin: 0 auto; display: table;">
                                                                    <ul class="nav nav-list margin-no pull-left">
                                                                        <li class="dropdown">
                                                                            <a class="dropdown-toggle text-black waves-attach waves-effect" data-toggle="dropdown" style="color: grey; line-height: 15px; padding: 10px; margin: 0px; min-height: 15px; z-index: 0;" title=" Menu ">
                                                                                <i class="fa fa-cog"></i>
                                                                                <i class="fa fa-caret-down" style="font-size: xx-small;"></i>
                                                                            </a>
                                                                            <ul class="dropdown-menu-list-table">
                                                                                <li style="background-color: white; padding: 10px;">
                                                                                    <%--<label
                                                                                        onclick="<%= txtSoalID.ClientID %>.value = '<%# Eval("Rel_BankSoal").ToString() %>'; <%= btnShowDetail.ClientID %>.click(); "
                                                                                        id="btnDetail" style="color: #114D79; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                        <i class="fa fa-info-circle" title=" Lihat Detail "></i>&nbsp;&nbsp;&nbsp;Lihat Detail</label>--%>
                                                                                </li>
                                                                                <li style="padding: 0px;">
                                                                                    <hr style="margin: 0px; padding: 0px;" />
                                                                                </li>
                                                                                <li style="background-color: white; padding: 10px;">
                                                                                    <label
                                                                                        onclick="<%= txtSoalID.ClientID %>.value = '<%# Eval("Kode").ToString() %>'; <%= btnShowConfirmDelete.ClientID %>.click(); "
                                                                                        id="btnHapus" style="color: #9E0000; cursor: pointer; padding-left: 5px; padding-right: 5px; font-weight: bold;">
                                                                                        <i class="fa fa-times" title=" Hapus Data "></i>&nbsp;&nbsp;&nbsp;Hapus Data</label>
                                                                                </li>
                                                                            </ul>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <%--  <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none; color: #1DA1F2;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Soal").ToString())
                                                                    %>
                                                                </span>
                                                                <br />--%>
                                                                <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                    <%#     (Eval("Soal").ToString().Length > 100) ? 
                                                                                                (AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Soal").ToString().Substring(0,100) + "...")) : 
                                                                                                 AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Soal").ToString())
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Jenis").ToString())
                                                                    %>
                                                                </span>
                                                            </td>
                                                            <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                <a href="javascript:void(0);" style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                    <i class="fa fa-edit" onclick="<%= txtID.ClientID %>.value = '<%# Eval("Kode").ToString() %>';<%# txtSkorVal.ClientID%>.value = '<%# Eval("Skor").ToString() %>'; <%= btnUpdateSkor.ClientID %>.click(); "></i>
                                                                    <%# 
                                                                        AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Skor").ToString())
                                                                    %>  &nbsp;                                                                                                                          
                                                                  
                                                                </a>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                            <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                <thead>
                                                                    <tr style="background-color: #3367d6;">
                                                                        <th style="text-align: center; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle; width: 80px;">#
                                                                        </th>
                                                                        <th style="text-align: center; background-color: #3367d6; width: 80px; vertical-align: middle;">
                                                                            <i class="fa fa-cog"></i>
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">Unit Sekolah, Mata Pelajaran
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">Alias
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">Jenis
                                                                        </th>
                                                                        <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">Keterangan
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="6" style="text-align: center; padding: 10px;">..:: Data Kosong ::..
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
                                                style="background-color: white; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 10; position: fixed; bottom: 28px; right: 25px; width: 320px; border-radius: 25px; padding: 8px; margin: 0px;">

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
                                                        <asp:LinkButton OnClick="btnRefresh_Click" CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" ID="btnRefresh" title=" Refresh " Style="background-color: #601B70; color: white;">
                                                            <span class="fbtn-text fbtn-text-left">Refresh Data</span>
                                                            <i class="fa fa-refresh"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ToolTip=" Tambah Data " runat="server" ID="btnDoAdd" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" Style="background-color: #257228;" OnClick="btnDoAddSoal_Click">
                                                            <span class="fbtn-text fbtn-text-left">Dari Bank Soal</span>
                                                            <i class="fa fa-plus" style="color: white;"></i>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ToolTip=" Tambah Data " runat="server" ID="LinkButton1" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" Style="background-color: #257228;" OnClick="btnDoAddNewSoal_Click">
                                                            <span class="fbtn-text fbtn-text-left">Buat Soal Baru</span>
                                                            <i class="fa fa-plus" style="color: white;"></i>
                                                        </asp:LinkButton>
                                                        <%--  <asp:LinkButton ToolTip=" Kembali " runat="server" ID="btnBack" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" Style="background-color: #257228;" OnClick="btnBackToMenu_Click">
                                                            <span class="fbtn-text fbtn-text-left">Rumah Soal</span>
                                                            <i class="fa fa-arrow-left"></i>
                                                        </asp:LinkButton>--%>
                                                    </div>

                                                </div>
                                            </div>
                                            <div class="content-header ui-content-header"
                                                style="background-color: #00198d; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 33px; right: 50px; width: 830px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                                                <div style="padding-left: 0px;">
                                                    <asp:LinkButton ToolTip=" Kembali " runat="server" ID="LinkButton2"
                                                        OnClick="btnBackToMapel_Click"
                                                        CssClass="btn-trans waves-attach waves-circle waves-effect" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;Mata Pelajaran
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="content-header ui-content-header"
                                                style="background-color: red; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 6; position: fixed; bottom: 33px; right: 50px; width: 680px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                                                <div style="padding-left: 0px;">
                                                    <asp:LinkButton ToolTip=" Kembali " runat="server" ID="LinkButton3"
                                                        OnClick="btnBackToKelas_Click"
                                                        CssClass="btn-trans waves-attach waves-circle waves-effect" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;Kelas
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="content-header ui-content-header"
                                                style="background-color: purple; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 7; position: fixed; bottom: 33px; right: 50px; width: 600px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                                                <div style="padding-left: 0px;">
                                                    <asp:LinkButton ToolTip=" Kembali " runat="server" ID="LinkButton7"
                                                        OnClick="btnBackToStrukturNilai_Click"
                                                        CssClass="btn-trans waves-attach waves-circle waves-effect" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;Struktur Nilai
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="content-header ui-content-header"
                                                style="background-color: green; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 8; position: fixed; bottom: 33px; right: 50px; width: 460px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;">
                                                <div style="padding-left: 0px;">
                                                    <asp:LinkButton ToolTip=" Kembali " runat="server" ID="LinkButton4"
                                                        OnClick="btnBackToFormRumahSoal_Click"
                                                        CssClass="btn-trans waves-attach waves-circle waves-effect" Style="font-weight: bold; color: ghostwhite;">
                                                        &nbsp;&nbsp;
                                                        <i class="fa fa-arrow-left"></i>
                                                        &nbsp;&nbsp;Form Rumah Soal
                                                    </asp:LinkButton>
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
                <div class="modal-dialog modal-lg">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner text-right"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 0; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" onclick="TriggerSave()" data-dismiss="modal"><i class="fa fa-close"></i></a>
                            <div style="width: 100%;">
                                <div class="row">
                                    <div class="col-lg-12">

                                        <div class="" style="padding: 0px;">
                                            <div class="card" style="margin-top: 0px;">
                                                <div class="card-main">
                                                    <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px;">
                                                        <%-- <table style="width: 100%;">

                                                            <tr>
                                                                <td style="background-color: #295BC8; padding: 0px;">
                                                                    <hr style="margin: 0px; border-style: solid; border-width: 1px; border-color: #2555BE;" />
                                                                </td>
                                                            </tr>
                                                        </table>--%>



                                                        <div>
                                                            <div id="div_Header2" class="textsearch" runat="server" style="margin: 0 auto; margin: 9px; padding-left: 10px; padding-right: 10px; width: 50%;">
                                                                <div style="background-color: #5A5A5A; background-color: #F1F3F4; border-radius: 5px; padding: 2px; border-width: 1px; border-style: solid; border-color: #e1e1e1;">
                                                                    <table style="width: 100%;">
                                                                        <tr>
                                                                            <td style="width: 30px; text-align: center; color: grey; background-color: #5A5A5A; background-color: #F1F3F4; padding-top: 0px; padding-bottom: 0px; padding-left: 5px; padding-right: 5px;">
                                                                                <i class="fa fa-search"></i>
                                                                            </td>
                                                                            <td style="padding-left: 0px; background-color: #5A5A5A; background-color: #F1F3F4; padding-top: 3px; padding-bottom: 3px;">
                                                                                <asp:TextBox title=" Cari Data " placeholder="Pencarian Cepat..." ID="txtCariSoal"
                                                                                    onkeypress="btnDoCariSoal(event)" runat="server" Style="color: black; font-weight: bold; background: transparent; padding: 5px; border-style: none; width: 100%; border-radius: 5px; outline: none;"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>

                                                            <asp:MultiView runat="server" ID="MultiView1" ActiveViewIndex="0">
                                                                <asp:View runat="server" ID="View1">
                                                                    <div style="padding: 0px; margin: 0px;">
                                                                        <asp:ListView ID="lvDataBs" DataSourceID="sql_dsbs" runat="server">
                                                                            <LayoutTemplate>

                                                                                <div class="table-responsive" style="margin: 0px; box-shadow: none;">
                                                                                    <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                                        <thead>
                                                                                            <tr style="background-color: #3367d6;">
                                                                                                <th style="text-align: center; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle; width: 80px;">#
                                                                                                </th>

                                                                                                <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">Soal
                                                                                                </th>
                                                                                                <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">Jenis
                                                                                                </th>

                                                                                                <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;"></th>

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
                                                                                    <td style="text-align: center; padding: 10px; vertical-align: middle; color: #bfbfbf;">
                                                                                        <%# (int)(this.Session[SessionViewDataName] == null ? 0 : this.Session[SessionViewDataName]) + (Container.DisplayIndex + 1) %>.
                                                                                    </td>

                                                                                    <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;">
                                                                                        <span style="color: grey; font-weight: bold; text-transform: none; text-decoration: none;">
                                                                                            <%#     (Eval("Soal").ToString().Length > 100) ? 
                                                                                                (AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Soal").ToString().Substring(0,100) + "...")) : 
                                                                                                 AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Soal").ToString())
                                                                                            %>
                                                                                       
                                                                                        </span>
                                                                                    </td>
                                                                                    <td>

                                                                                        <span style="color: grey; font-weight: normal; text-transform: none; text-decoration: none;">
                                                                                            <%#     
                                                                                                 AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Jenis").ToString())
                                                                                            %>
                                                                                       
                                                                                        </span>

                                                                                    </td>

                                                                                    <td style="font-weight: bold; padding: 10px; vertical-align: middle; text-align: right;">
                                                                                        <label
                                                                                            onclick="GoToURL2('<%= ResolveUrl(AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.CBT.SOAL_VIEW.ROUTE) %>?m=<%#  AI_ERP.Application_Libs.Libs.GetQueryString("m")%>&id=<%#  AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kode").ToString())%> ');"
                                                                                            title=" Lihat Data Soal" class="btn btn-brand">
                                                                                            <i class="fa fa-eye"></i>
                                                                                        </label>

                                                                                        <%--<button tooltip=" Lihat Soal " runat="server" id="detailSoal" cssclass="btn btn-orange"
                                                                                        commandargument='<%#AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kode").ToString())%>'
                                                                                        onclick="btnDoViewSoal_Click">

                                                                                        <span class="fbtn-text fbtn-text-left">Lihat Soal</span>
                                                                                        <i class="fa fa-eye"></i>
                                                                                    </button>--%>
                                                                                        <asp:LinkButton ToolTip=" Tambah Soal " runat="server" ID="addSoal" CssClass="btn btn-green  "
                                                                                            CommandArgument='<%#AI_ERP.Application_Libs.Libs.GetHTMLSimpleText(Eval("Kode").ToString())%>'
                                                                                            OnClick="lnkOKInput_Click">
                                                                                      
                                                                                        <span class="fbtn-text fbtn-text-left">Tambah Soal</span>
                                                                                        <i class="fa fa-plus"></i>
                                                                                        </asp:LinkButton>

                                                                                    </td>

                                                                                </tr>
                                                                            </ItemTemplate>
                                                                            <EmptyDataTemplate>
                                                                                <div class="table-responsive" style="margin-top: 0px; margin-bottom: 0px; box-shadow: 0 -1px 0 rgba(0, 0, 0, 0), 0 0 3px rgba(0, 0, 0, 0.18), 0 1px 3px rgba(0, 0, 0, 0.0);">
                                                                                    <table class="table" id="itemPlaceholderContainer" runat="server" style="width: 100%; margin: 0px;">
                                                                                        <thead>
                                                                                            <tr style="background-color: #3367d6;">
                                                                                                <th style="text-align: center; font-weight: bold; color: white; background-color: #3367d6; vertical-align: middle; width: 80px;">#
                                                                                                </th>
                                                                                                <th style="text-align: center; background-color: #3367d6; width: 80px; vertical-align: middle;">
                                                                                                    <i class="fa fa-cog"></i>
                                                                                                </th>
                                                                                                <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">Soal
                                                                                                </th>
                                                                                                <th style="background-color: #3367d6; text-align: left; padding-left: 10px; vertical-align: middle;">Jenis
                                                                                                </th>
                                                                                            </tr>
                                                                                        </thead>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td colspan="6" style="text-align: center; padding: 10px;">..:: Data Kosong ::..
                                                                                                </td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </div>
                                                                            </EmptyDataTemplate>
                                                                        </asp:ListView>
                                                                    </div>
                                                                    <asp:SqlDataSource ID="Sql_dsbs" runat="server"></asp:SqlDataSource>

                                                                    <%--<div class="content-header ui-content-header"
                                                                        style="background-color: white; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 28px; right: 25px; width: 320px; border-radius: 25px; padding: 8px; margin: 0px;">

                                                                        <div style="padding-left: 15px;">
                                                                            <asp:DataPager ID="dpDataBs" runat="server" PageSize="1" PagedControlID="lvDataBs">
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
                                                                    </div>--%>

                                                                    <%--<div class="fbtn-container" id="div1" runat="server">

                                                                    <div class="fbtn-inner">
                                                                        <a class="fbtn fbtn-lg fbtn-brand-accent waves-attach waves-circle waves-light" data-toggle="dropdown" style="background-color: #a91212;" title=" Pilihan ">
                                                                            <span class="fbtn-ori icon"><span class="fa fa-cogs"></span></span>
                                                                            <span class="fbtn-sub icon"><span class="fa fa-cogs"></span></span>
                                                                        </a>
                                                                        <div class="fbtn-dropup" style="z-index: 999999;">
                                                                            <asp:LinkButton OnClick="btnRefresh_Click" CssClass="fbtn fbtn-brand-accent waves-attach waves-circle waves-light" runat="server" ID="LinkButton1" title=" Refresh " Style="background-color: #601B70; color: white;">
                                                            <span class="fbtn-text fbtn-text-left">Refresh Data</span>
                                                            <i class="fa fa-refresh"></i>
                                                                            </asp:LinkButton>
                                                                            <asp:LinkButton ToolTip=" Tambah Data " runat="server" ID="LinkButton2" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" Style="background-color: #257228;" OnClick="btnDoAdd_Click">
                                                            <span class="fbtn-text fbtn-text-left">Tambah Data</span>
                                                            <i class="fa fa-plus" style="color: white;"></i>
                                                                            </asp:LinkButton>
                                                                            <asp:LinkButton ToolTip=" Kembali " runat="server" ID="LinkButton3" CssClass="fbtn fbtn-green waves-attach waves-circle waves-effect" Style="background-color: #257228;" OnClick="btnBackToMenu_Click">
                                                            <span class="fbtn-text fbtn-text-left">Data Mata Pelajaran</span>
                                                            <i class="fa fa-arrow-left"></i>
                                                                            </asp:LinkButton>
                                                                        </div>

                                                                    </div>
                                                                </div>--%>
                                                                </asp:View>
                                                            </asp:MultiView>

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
                                    <%--<asp:LinkButton OnClientClick="TriggerSave()" ValidationGroup="vldInput" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInput" OnClick="lnkOKInput_Click" Text="   OK   "></asp:LinkButton>--%>
                                &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" onclick="TriggerSave()" data-dismiss="modal">Batal</a>
                                    <br />
                                    <br />
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_input_data2" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 0; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" onclick="TriggerSave()" data-dismiss="modal"><i class="fa fa-close"></i></a>
                            <div style="width: 100%;">
                                <div class="row">
                                    <div class="col-lg-12">

                                        <div class="" style="padding: 0px;">
                                            <div class="card" style="margin-top: 0px;">
                                                <div class="card-main">
                                                    <div class="card-inner" style="margin: 0px; padding: 0px; margin-right: -0.5px;">
                                                        <table style="width: 100%;">

                                                            <tr>
                                                                <td style="background-color: #295BC8; padding: 0px;">
                                                                    <hr style="margin: 0px; border-style: solid; border-width: 1px; border-color: #2555BE;" />
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
                        </div>
                        <div class="modal-footer">
                            <p class="text-right">
                                <%--<asp:LinkButton OnClientClick="TriggerSave()" ValidationGroup="vldInput" CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="lnkOKInput" OnClick="lnkOKInput_Click" Text="   OK   "></asp:LinkButton>--%>
                                &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" onclick="TriggerSave()" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_confirm_hapus" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">Konfirmasi
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
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_update_skor" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">Update Skor
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
                                                            <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                <label class="label-input" for="<%= txtSkor.ClientID %>" style="text-transform: none;">Skor :</label>
                                                                <asp:TextBox ValidationGroup="vldInput" CssClass="form-control " runat="server" ID="txtSkor"></asp:TextBox>
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="LinkButton6" OnClick="lnkOKUpdateSkor_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div aria-hidden="true" class="modal fade" id="ui_modal_update_urut" role="dialog" tabindex="-1" style="display: none; padding-right: 9px; z-index: 2000;">
                <div class="modal-dialog modal-xs">
                    <div class="modal-content" style="border: none; border-top-left-radius: 6px; border-top-right-radius: 6px;">
                        <div class="modal-inner"
                            style="margin-left: 0px; margin-right: 0px; margin-bottom: 0px; margin-top: 0px; padding-left: 0px; padding-right: 0px; padding-bottom: 0px; padding-top: 25px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #EDEDED; background-repeat: no-repeat; background-size: auto; background-position: right; background-position-y: -1px;">
                            <div class="media margin-bottom-no margin-top-no" style="padding-left: 20px; padding-right: 20px; color: black; padding-bottom: 20px;">
                                <div class="media-object margin-right-sm pull-left">
                                    <span class="icon icon-lg text-brand-accent" style="color: black;">info_outline</span>
                                </div>
                                <div class="media-inner">
                                    <span style="font-weight: bold;">Update Nomor Urut
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
                                                            <div class="form-group form-group-label" style="margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;">
                                                                <label class="label-input" for="<%= txtUrut.ClientID %>" style="text-transform: none;">Skor :</label>
                                                                <asp:TextBox ValidationGroup="vldInput" CssClass="form-control " runat="server" ID="txtUrut"></asp:TextBox>
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
                                <asp:LinkButton CssClass="btn btn-flat btn-brand-accent waves-attach waves-effect" runat="server" ID="LinkButton5" OnClick="lnkOKUpdateUrut_Click" Text="  OK  "></asp:LinkButton>
                                <a class="btn btn-flat btn-brand-accent waves-attach waves-effect" data-dismiss="modal">Batal</a>
                                <br />
                                <br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnRefresh" />
            <asp:PostBackTrigger ControlID="btnDoCari" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
        //function selectElems() {
        //    var tags = document.querySelectorAll('#04.0716.746');
        //    for (var i = 0; i < tags.length; i++) {
        //        tags.item(i).style.backgroundColor = "red";
        //    }
        //}
        //selectElems();
        RenderDropDownOnTables();
        InitModalFocus();
    </script>
</asp:Content>
