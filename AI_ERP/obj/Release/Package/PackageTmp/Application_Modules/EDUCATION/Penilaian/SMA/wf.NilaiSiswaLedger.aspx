<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="wf.NilaiSiswaLedger.aspx.cs" Inherits="AI_ERP.Application_Modules.EDUCATION.Penilaian.SMA.wf_NilaiSiswaLedger" %>

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
        table { page-break-inside:auto }
        td    { page-break-inside:avoid; page-break-after:auto }
        thead.report-header {
           display: table-header-group;
        }

        tfoot.report-footer {
           display:table-footer-group;
        }

        tabel.report-container {
            page-break-after: always;
        }

        #pageFooter {
            display: table-footer-group;
        }

        #pageFooter:after {
            counter-increment: page;
            content:"Page " counter(page);
            left: 0; 
            top: 100%;
            white-space: nowrap; 
            z-index: 20;
            -moz-border-radius: 5px; 
            -moz-box-shadow: 0px 0px 4px #222;  
            background-image: -moz-linear-gradient(top, #eeeeee, #cccccc);  
        }
    </style>
    <link href="~/Application_CLibs/font-awesome/css/font-awesome.min.css" rel="stylesheet">
    <script type="text/javascript">
        function fnExcelReport() {
            var div = document.getElementById("div_ledger");
            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");

            if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))  //IE
            {
                txtArea1.document.open("txt/html", "replace");
                txtArea1.document.write(div.innerHTML);
                txtArea1.document.close();
                txtArea1.focus();
                sa = txtArea1.document.execCommand("SaveAs", true, "LEDGER NILAI.xls");
            }
            else {
                sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(div.innerHTML));
            }

            return (sa);
        }
    </script>
    <title>LEDGER NILAI</title>
</head>
<body style="font-family: Tahoma; font-size: 10pt;">
    <form id="form1" runat="server">
        <iframe id="txtArea1" style="display:none"></iframe>
        <button style="cursor: pointer; position: fixed; right: 10px; top: 10px; background-color: red; border-style: none; color: white; font-weight: bold; padding-bottom: 8px; padding-top: 8px;" id="btnExport" onclick="fnExcelReport();">&nbsp;&nbsp;DOWNLOAD EXCEL&nbsp;&nbsp;</button>
        <div id="div_ledger">
            <asp:Literal runat="server" ID="ltrHTML"></asp:Literal>
        </div>
    </form>
</body>
</html>
