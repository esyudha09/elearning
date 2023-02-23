<%@ Page Language="C#" EnableSessionState="ReadOnly" Async="true" AutoEventWireup="true" CodeBehind="wf.NilaiRaporPrint.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP.wf_NilaiRaporPrint" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/Application_CLibs/font-awesome/css/font-awesome.min.css" rel="stylesheet">
    <title>CETAK RAPOR</title>
    <style type="text/css">
        .underline{
            text-decoration: underline;
        }
    </style>
</head>
<body style="font-family:Times New Roman;" 
    onload="if (typeof parent.HideModal === 'function') parent.HideModal(); 
            if (typeof parent.SelesaiProses === 'function') parent.SelesaiProses(); 
            if (window.opener !== null) { if (typeof window.opener.ShowProgress === 'function') { window.opener.ShowProgress(false); } }
            ">
    <form id="form1" runat="server">
    <div>
        <CR:CrystalReportViewer ID="CRV1" runat="server" AutoDataBind="true" />
    </div>
    </form>
</body>
</html>
