<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImageUploader.aspx.cs" Inherits="AI_ERP.Application_Controls.Res.ImageUploader" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="../../CLibs/css/style.css" />
</head>
<body style="padding: 0px; margin: 0px; background-color: transparent;">
    <form id="form1" runat="server" style="padding: 0px; margin: 0px;">
        <div style="text-align: center; vertical-align: middle; padding: 0px; margin: 0px; height: 250px; width: 250px;">
            <asp:FileUpload ID="FileUpload1" runat="server" style="display: none; cursor: pointer;" />
            <asp:Button runat="server" ID="btnhapusimage" style="display: none; cursor: pointer;" OnClick="btnhapusimage_Click" />
            <asp:Button runat="server" ID="btnuploadimage" style="display: none; cursor: pointer;" onClick="btnuploadimage_Click" />
            <asp:CheckBox ID="chkishapusfoto" runat="server" style="display: none;" />
            <div id="btnuploadfoto" onclick="
                <%=
                    "if(parent.document.getElementById('" + Request.QueryString["inputkeyid"] + "') != null) " +
                    "{ " +
                    "     if(parent.document.getElementById('" + Request.QueryString["inputkeyid"] + "').value == '') " +
                    "     { " +
                    "         alert('Sebelum Upload Image,\\nInput &quot;" + Request.QueryString["inputkeyname"] + "&quot; harus diisi terlebih dahulu.'); " +
                    "         parent.document.getElementById('" + Request.QueryString["inputkeyid"] + "').focus(); " +
                    "         parent.document.getElementById('" + FileUpload1.ClientID + "').value = ''; " +
                    "         return false; " +
                    "     } " +
                    "} "
                %>
                <%= FileUpload1.ClientID %>.click();" 
                style="font-weight: bold; width: 70%; float: none; color: White; font-size: 10pt; background-color: #009494; padding: 5px; border-radius: 10px; margin: auto; margin-bottom: 5px; cursor: pointer;">UPLOAD IMAGE</div>
            <div onclick="if(!confirm('Anda yakin akan menghapus image?')) { return false; } else { document.getElementById('<%= btnhapusimage.ClientID %>').click(); }" id="btnhapusfoto" <%= "style=\"font-weight: bold; width: 70%; float: none; color: White; font-size: 10pt; background-color: Red; padding: 5px;  margin: auto; border-radius: 10px; auto; margin-bottom: 5px; cursor: pointer;" + (Request.QueryString["noimage"] != null ? " display: none; " : "") + "\"" %>>HAPUS IMAGE</div>
            <div id="btnbatal" style="font-weight: bold; width: 70%; float: none; color: White; font-size: 10pt; background-color: Orange; padding: 5px;  margin: auto; border-radius: 10px; cursor: pointer;" onclick="parent.document.getElementById('<% if(Request.QueryString["cid"] != null) { Response.Write(Request.QueryString["cid"].ToString()); } %>').style.display = 'inherit';">&laquo;&nbsp;BATAL</div>                    
            <asp:HiddenField runat="server" ID="txtkeyvalue" />
        </div>
    </form>
</body>
</html>