﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wf.BukaSemesterInfo.aspx.cs" Inherits="AI_ERP.Application_Modules.__LOADER.wf_BukaSemesterInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, '\\$&');
            var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, ' '));
        }
    </script>
</head>
<body onload="if (typeof parent.StopProsesBukaSemester === 'function') parent.StopProsesBukaSemester(getParameterByName('m'));">
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
