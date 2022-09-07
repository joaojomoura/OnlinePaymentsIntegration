<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Status.aspx.cs" Inherits="OnlinePaymentsIntegration.Status" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%--<script type="text/javascript">
        window.onload = function () {
            // Set interval. Currently it is 1 sec.
            setInterval(ShowMessage, 5000);
        }
        function ShowMessage() {

            PageMethods.getStatus(Success, Failure);
        }
            function Success(result) { alert(result); }
            function Failure(error) { alert(error); }
        
    </script>--%>
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
            <%--<asp:ScriptManager ID='ScriptManager1' runat="server" EnablePageMethods="true" />--%>
            <asp:Label ID="StatusPayment" runat="server" Text="Pagamento em espera"></asp:Label>
            <br />
            <br />
            <%=Result %>
            <br />
            <br />
            <br />
            Referencias Multibanco:
            <br />
            Entidade : <%=Entidade %>
            <br />
            Referencia : <%=Referencia %>
            <br />
            Montante : <%=Montante %>
            <br />
            <br />
            <br />
            
           <%-- <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />--%>
        </div>
    </form>
</body>
   <%=getStatus() %>
</html>
