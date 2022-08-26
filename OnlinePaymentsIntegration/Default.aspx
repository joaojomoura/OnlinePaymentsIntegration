<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OnlinePaymentsIntegration.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <script src="https://spg.qly.site1.sibs.pt/assets/js/widget.js?id=<%=TransactionId %>"></script>
    <form class="paymentSPG" spg-context="<%=FormContext %>" spg-config="<%=FormConfig %>" spg-style="<%=FormStyle %>" spg-signature="<%=Signature %>""></form>
</body>
</html>
