<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Status.aspx.cs" Inherits="OnlinePaymentsIntegration.Status" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Resultado do CopyanPay</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%--Async="true"--%>
            <br />
            <br />
            <br />
            <%=Pagamento %>
            <br />
            <br />
            <br />
            <%=Result %>
            <br />
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
        </div>
    </form>
</body>
</html>
