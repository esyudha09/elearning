<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="wf.ReportViewer.aspx.cs" Inherits="AI_ERP.Application_Modules.wf_ReportViewer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="<%= ResolveUrl("~/Application_CLibs/js/jquery.min.js") %>"></script>	
    <script type="text/javascript">
        $(document).ready(function () {
            setTimeout(updateProgress, 100);
        });

        function updateProgress() {
            $("#percentage").text("");
            $.ajax({
                type: "POST",
                url: "<%= ResolveUrl("~/Application_Modules/__LOADER/wf.ProgressMonitor.aspx") %>/GetProgress",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (msg) {
                    $("#percentage").text(msg.d);
                    setTimeout(updateProgress, 100);
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <button onclick="updateProgress(); return false;">Proses</button>
        <br />
        Proses: <asp:Label ID="percentage" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
