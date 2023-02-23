<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AutocompleteJurusanPendidikan.ascx.cs" Inherits="AI_ERP.Application_Controls.AutocompleteJurusanPendidikan.AutocompleteJurusanPendidikan" %>
<script type="text/javascript">
    var items = [];
    var items_kode = [];
    var map = {};

    function <%=txtNama.ClientID%>_SHOW_AUTOCOMPLETE(){
        $('[id*=<%=txtNama.ClientID%>]').typeahead({
            hint: true,
            highlight: true,
            minLength: 2,
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("~/APIs/Master/JurusanPendidikan.svc/ShowAutocomplete?kata_kunci=") %>' + request,
                    dataType: "json",
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        items = [];
                        items_kode = [];
                        map = {};
                        $.each(data.d, function (i, item) {
                            var id = item.split('~')[1];
                            var name = item.split('~')[0];
                            map[name] = { id: id, name: name };
                            items.push(name);
                            items_kode.push(id);
                        });
                        if (items.length == 0) {
                            $('[id*=<%=hflKode.ClientID%>]').val("");
                        }
                        response(items);
                        $(".dropdown-menu").css("height", "auto");
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            updater: function (item) {
                $('[id*=<%=hflKode.ClientID%>]').val(map[item].id);
                return item;
            }
        }).on('change', function (evt, data) {
            var ada = false;
            var kode = "";
            for (var i = 0; i < items.length; i++) {
                if (this.value == items[i]) {
                    ada = true;
                    kode = items_kode[i];
                    break;
                }
            }
            if (items.length > 0) {
                if (!ada) {
                    $('[id*=<%=hflKode.ClientID%>]').val("");
                }
                else {
                    $('[id*=<%=hflKode.ClientID%>]').val(kode);
                }
            }
        });
    }

    function <%=txtNama.ClientID%>_SHOW_AUTOCOMPLETE_WITH_EVENT(btn_do){
        $('[id*=<%=txtNama.ClientID%>]').typeahead({
            hint: true,
            highlight: true,
            minLength: 2,
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("~/APIs/Master/JurusanPendidikan.svc/ShowAutocomplete?kata_kunci=") %>' + request,
                    dataType: "json",
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        items = [];
                        items_kode = [];
                        map = {};
                        $.each(data.d, function (i, item) {
                            var id = item.split('~')[1];
                            var name = item.split('~')[0];
                            map[name] = { id: id, name: name };
                            items.push(name);
                            items_kode.push(id);
                        });
                        if (items.length == 0) {
                            $('[id*=<%=hflKode.ClientID%>]').val("");
                        }
                        response(items);
                        $(".dropdown-menu").css("height", "auto");
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            updater: function (item) {
                $('[id*=<%=hflKode.ClientID%>]').val(map[item].id);
                document.getElementById(btn_do).click();
                return item;
            }
        }).on('change', function (evt, data) {
            var ada = false;
            var kode = "";
            for (var i = 0; i < items.length; i++) {
                if (this.value == items[i]) {
                    ada = true;
                    kode = items_kode[i];
                    break;
                }
            }
            if (items.length > 0) {
                if (!ada) {
                    $('[id*=<%=hflKode.ClientID%>]').val("");
                }
                else {
                    $('[id*=<%=hflKode.ClientID%>]').val(kode);
                }
            }
        });
    }
</script>
<asp:TextBox ID="txtNama" runat="server" CssClass="form-control" autocomplete="off" />
<asp:HiddenField ID="hflKode" runat="server" />