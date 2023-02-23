<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Download.aspx.cs" Inherits="AI_ERP.Application_Resources.Download" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Download</title>
    <style type="text/css">
        table, td, th {
            border: 1px solid black;
        }
    </style>
</head>
<body onload="if (typeof parent.ShowProsesDownload === 'function') parent.ShowProsesDownload(false);
              if (typeof parent.ShowProsesLaporanRekapAbsen === 'function') parent.ShowProsesLaporanRekapAbsen(false);
              if (typeof parent.ShowProsesLaporanHistLinkPembelajaran === 'function') parent.ShowProsesLaporanHistLinkPembelajaran(false);
              if (typeof parent.HideModal === 'function') parent.HideModal();">
    <form id="form1" runat="server">
    <div>
        <asp:Literal runat="server" ID="ltrDownload"></asp:Literal>
    </div>
    </form>
</body>
</html>
