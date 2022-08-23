<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="OnlinePaymentsIntegration.index" %>

<%@ Register assembly="DevExpress.Web.v21.1, Version=21.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
     <form id="form1" runat="server">
        <div style="width: 1315px">
            <br />
            Valor:<dx:ASPxTextBox ID="AmountTextBox" runat="server" Width="170px">
            </dx:ASPxTextBox>
            <br />
            <dx:ASPxButton ID="ShowJsonButton" runat="server" OnClick="ShowJsonButton_Click" Text="Mostrar JSON">
            </dx:ASPxButton>
            <br />
            <dx:ASPxRadioButtonList ID="paymentTypeChosen" runat="server" DataSourceID="XmlDataSource1" ValueField="ID" TextField="Name">
                <Border BorderColor="White" />
            </dx:ASPxRadioButtonList>
            <asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/SIBS/XML/PaymentMethods.xml" XPath="//PaymentMethod"></asp:XmlDataSource>
            <dx:ASPxButton ID="PaymentButton" runat="server" OnClick="PaymentButton_Click" Text="Pagar">
            </dx:ASPxButton>
            <br />
            Resultado do JSON:<br />
            <dx:ASPxLabel ID="CheckoutResult" runat="server" Text="CheckoutResult">
            </dx:ASPxLabel>
            <br />
            <br />
        </div>
    </form>
</body>
</html>
