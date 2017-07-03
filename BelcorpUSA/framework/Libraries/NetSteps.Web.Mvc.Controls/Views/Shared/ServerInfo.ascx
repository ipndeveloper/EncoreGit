<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Net" %>
<%
    var ipString = string.Join(" ",
        Dns.GetHostAddresses(Dns.GetHostName())
            .Where(x => x.ToString().Contains('.'))
            .Select(x => x.GetAddressBytes()[3])
            .OrderBy(x => x)
            .Select(x => string.Format(".{0}", x))
        );
%>
<meta id="SERVER_IS_UP" content="Server IP(s): <%: ipString %>" />
