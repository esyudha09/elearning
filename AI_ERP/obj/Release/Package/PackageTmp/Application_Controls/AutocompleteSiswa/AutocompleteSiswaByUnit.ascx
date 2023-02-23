<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AutocompleteSiswaByUnit.ascx.cs" Inherits="AI_ERP.Application_Controls.AutocompleteSiswa.AutocompleteSiswaByUnit" %>
<script type="text/javascript">
    function KodeUnitSiswa(){
        return document.getElementById("<%= txtKodeUnit.ClientID %>");
    }
    function TahunAjaran(){
        return document.getElementById("<%= txtTahunAjaran.ClientID %>");
    }
    function Semester(){
        return document.getElementById("<%= txtSemester.ClientID %>");
    }
    function <%=txtNama.ClientID%>_SHOW_AUTOCOMPLETE(){
        $('[id*=<%=txtNama.ClientID%>]').typeahead({
            hint: true,
            highlight: true,
            minLength: 2,
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("~/APIs/Master/Siswa.svc/ShowAutocompleteByUnitByTahunAjaranBySemester?kata_kunci=") %>' + request +
                         (KodeUnitSiswa().value.trim() !== "" ? "&rel_unit=" + KodeUnitSiswa().value.trim() : "") +
                         (TahunAjaran().value.trim() !== "" ? "&tahun_ajaran=" + TahunAjaran().value.trim() : "") +
                         (Semester().value.trim() !== "" ? "&semester=" + Semester().value.trim() : ""),
                    dataType: "json",
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        items = [];
                        map = {};
                        $.each(data.d, function (i, item) {
                            var id = item.split('~')[1];
                            var name = item.split('~')[0];
                            map[name] = { id: id, name: name };
                            items.push(name);
                        });
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
        });
    }
    function <%=txtNama.ClientID%>_SHOW_AUTOCOMPLETE_WITH_EVENT(btn_do){
        $('[id*=<%=txtNama.ClientID%>]').typeahead({
            hint: true,
            highlight: true,
            minLength: 2,
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("~/APIs/Master/Siswa.svc/ShowAutocompleteByUnitByTahunAjaranBySemester?kata_kunci=") %>' + request +
                         (KodeUnitSiswa().value.trim() !== "" ? "&rel_unit=" + KodeUnitSiswa().value.trim() : "") +
                         (TahunAjaran().value.trim() !== "" ? "&tahun_ajaran=" + TahunAjaran().value.trim() : "") +
                         (Semester().value.trim() !== "" ? "&semester=" + Semester().value.trim() : ""),
                    dataType: "json",
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        items = [];
                        map = {};
                        $.each(data.d, function (i, item) {
                            var id = item.split('~')[1];
                            var name = item.split('~')[0];
                            map[name] = { id: id, name: name };
                            items.push(name);
                        });
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
        });
    }
</script>
<asp:TextBox ID="txtNama" runat="server" CssClass="form-control" autocomplete="off" />
<asp:HiddenField ID="hflKode" runat="server" />
<asp:HiddenField ID="txtKodeUnit" runat="server" />
<asp:HiddenField ID="txtTahunAjaran" runat="server" />
<asp:HiddenField ID="txtSemester" runat="server" />