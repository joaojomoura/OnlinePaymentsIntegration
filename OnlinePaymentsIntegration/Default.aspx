<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OnlinePaymentsIntegration.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <script src="https://test.oppwa.com/v1/paymentWidgets.js?checkoutId=<%=CheckoutId %>"></script>
    <form action="http://localhost:50893/Status.aspx" class="paymentWidgets" data-brands="<%=paymentBrand %>""></form>
</body>
</html>
