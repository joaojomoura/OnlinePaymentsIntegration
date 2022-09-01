<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OnlinePaymentsIntegration.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    
<body>
    
    <script type="text/javascript">

        window.onload = load();
        window.onunload = unload();
        window.onloadeddata = secondLoad();
        //_ngcontent - puv - c11

        function unload() {
            var t = "ola"
            var t = document.getElementsByClassName('ml-2');
            
        }

        function secondLoad() {
            var t = "ola"
            var t = document.getElementsByClassName('ml-2');
        }

        function load() {

            var t = document.getElementsByClassName('ml-2');
            var ola = "ola"
            var tt = document.getElementById("widget-container");
            debugger;
        }

    </script>
    <script src="https://spg.qly.site1.sibs.pt/assets/js/widget.js?id=<%=TransactionId %>"></script>
    <form class="paymentSPG" spg-context="<%=formContext %>" spg-config="<%=formConfig%>" spg-style="<%=formStyle %>"></form>
</body>
</html>
