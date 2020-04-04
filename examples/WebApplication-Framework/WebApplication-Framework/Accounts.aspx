<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Accounts.aspx.cs" Inherits="WebApplication_Framework.Accounts" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Accounts</h2>
            <asp:Label ID="lblColour" runat="server" Text="Not Set"></asp:Label>
            <ul>
                <li><asp:HyperLink ID="HyperLink1" NavigateUrl="~/Default.aspx" runat="server">Default</asp:HyperLink></li>
                <li><asp:HyperLink ID="HyperLink2" NavigateUrl="~/Accounts.aspx" runat="server">Accounts</asp:HyperLink></li>
            </ul>
        </div>
    </form>
</body>
</html>
