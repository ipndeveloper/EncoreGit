   <%@ Page Title="" Language="C#" MasterPageFile="" CodeBehind="ReportsViewer.aspx.cs"
       Inherits="nsRSReportes.ReportViewer.ReportsViewer" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" >
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>  
        <div>
            <rsweb:reportviewer id="rptViewer" runat="server" height="1200px"
                width="100%" ZoomMode="Percent" SizeToReportContent="true">  
            </rsweb:reportviewer>
   
        </div>
    </form>
</body>
</html>
