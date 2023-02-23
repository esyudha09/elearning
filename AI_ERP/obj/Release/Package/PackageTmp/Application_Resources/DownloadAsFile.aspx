<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DownloadAsFile.aspx.cs" Inherits="AI_ERP.Application_Resources.DownloadAsFile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body onload="if (typeof parent.ShowProsesDownload === 'function') parent.ShowProsesDownload(false);">
    <form id="form1" runat="server">
    <div id="div_content" style="width: 100%;">
        <asp:Literal runat="server" ID="ltrDownload"></asp:Literal>
    </div>
    </form>
</body>
</html>
