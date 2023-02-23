<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wf.NilaiSiswaLTSPrint.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP.wf_NilaiSiswaLTSPrint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
        .pagebreak { page-break-before: always; }
        tfoot td {
            border-bottom-width: 0px ;
            border-top: 2px solid #333333 ;
            padding-top: 0px ;
        }
    </style>
    <link href="~/Application_CLibs/font-awesome/css/font-awesome.min.css" rel="stylesheet">
    <title>CETAK LTS</title>
</head>
<body style="font-family: Tahoma; margin-left: 10px; margin-right: 0px;">
    <form id="form1" runat="server">
    <div>
        <asp:Literal runat="server" ID="ltrHTML"></asp:Literal>
    </div>
    </form>
</body>
</html>
