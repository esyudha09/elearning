<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wf.ListAbsensiSiswa.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Elearning.wf_ListAbsensiSiswa" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Application_Templates/material-master/css/project.min.css" rel="stylesheet">
    <link href="~/Application_CLibs/font-awesome/css/font-awesome.min.css" rel="stylesheet">
    <style type="text/css">
        body {
            font-family: 'Raleway', Arial, sans-serif;
            font-size: small;
            padding-left: 0px;
            margin-left: 0px;
            margin-top: 0px;
        }
        td {
            padding: 5px;
        }

        table thead th:first-child {
            position: sticky;
            left: 0;
            top: 0;
            z-index: 2;
        }

        table tbody th {
            position: sticky;
            left: 0;
            background: white;
            color: black;
            z-index: 1;
        }

        table, th, td {
            border: 0.1px dotted #bfbfbf; padding: 5px;
        }
    </style>
</head>
<body onload="if (typeof parent.ShowProses === 'function') parent.ShowProses(false, null);">
    <form id="form1" runat="server">
        <div>

        </div>
        <div>
            <asp:Literal runat="server" ID="ltrListAbsensi"></asp:Literal>
        </div>
    </form>
</body>
</html>
