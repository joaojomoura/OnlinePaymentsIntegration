using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlinePaymentsIntegration.SIBS.SDK;
using System;
using System.Collections.Generic;

namespace OnlinePaymentsIntegration.Tests
{
    [TestClass]
    public class CheckoutRequestTest
    {

        string amount = "5.5", clientId = "8b5aa2bc-53ef-4b55-adbb-929aac3ebdda", currency = "EUR",multibancoEntity = "24000",
               terminalId = "56342", merchantTransactionId = "20220826", bearer = "0276b80f950fb446c6addaccd121abfbbb.eyJlIjoiMTk3Njk1NjM3NjExNyIsInJvbGVzIjoiU1BHX01BTkFHRVIiLCJ0b2tlbkFwcERhdGEiOiJ7XCJtY1wiOlwiOTk5OTk5OVwiLFwidGNcIjpcIjU2MzQyXCJ9IiwiaSI6IjE2NjEzMzcxNzYxMTciLCJpcyI6Imh0dHBzOi8vcWx5LnNpdGUxLnNzby5zeXMuc2licy5wdC9hdXRoL3JlYWxtcy9RTFkuTUVSQ0guUE9SVDEiLCJ0eXAiOiJCZWFyZXIiLCJpZCI6IjRGQURXVGdjUWE1NjJlZTQ4ODdkOTA0MTg0YTUyNWQyYjFjYzBlNjAzYiJ9.6e531784385b9211a2dde32bd354bac64bf87e40cd32da95713c29e5e7e89a097e2b5f4044a4f5ee10f13b404f616c77922e775f03e7a89a3ac59bebf07d82";
        string customerName = "Client Name", customerEmail = "client.name@hostname.pt", street1 = "Rua 123",
                street2 = "porta 2", city = "Lisboa", postcode = "1200-999", country = "PT";
        Dictionary<string, dynamic> checkoutRequest;
        CheckoutRequestCopyAndPay checkout;

        private CustomerInfo initCustomer() {
            CustomerInfo customer = new CustomerInfo(customerName, customerEmail);
            customer.shippingAddress.street1 = street1;
            customer.shippingAddress.street2 = street2;
            customer.shippingAddress.city = city;
            customer.shippingAddress.postcode = postcode;
            customer.shippingAddress.country = country;

            customer.billingAddress.street1 = street1;
            customer.billingAddress.street2 = street2;
            customer.billingAddress.city = city;
            customer.billingAddress.postcode = postcode;
            customer.billingAddress.country = country;
            return customer;
        }
        [TestInitialize]
        public void TestInitialize() {
            var customer = initCustomer();
            checkout = new CheckoutRequestCopyAndPay(amount, currency, clientId, bearer, terminalId,multibancoEntity, merchantTransactionId, customer);
            //checkoutRequest = checkout.getCheckoutRequestForTests();
        }

        [TestMethod]
        public void checkCheckoutRequestTextCorrect() {
            Assert.IsTrue(Convert.ToDouble((checkoutRequest["amount"]["value"])) == 5.5, "Valor nao esta igual");
        }

        [TestMethod]
        public void checkCheckoutRequestMadeSucesseful() {
            
            Assert.IsNotNull(checkoutRequest, "Conexao falhou com o webserver");
        }
        [TestMethod]
        public void checkIfCodeWas000() {
            Assert.IsTrue((checkoutRequest["returnStatus"]["statusCode"]).Equals("000"), checkoutRequest["returnStatus"]["statusMsg"]);
        }

        [TestMethod]
        public void checkIfTransactionIdExtractedSuccesfull() {
            Assert.IsNotNull(checkout.getTransactionId(checkoutRequest), " Transaction ID - Conexao falhou com o webserver");
        }


        [TestMethod]
        public void checkIfTransactionSignatureExtractedSuccesfull() {
            Assert.IsNotNull(checkout.getTransactionSignature(checkoutRequest), "Transaction Signature - Conexao falhou com o webserver");
        }

        [TestMethod]
        public void checkIfFormContextExtractedSuccesfull() {
            Assert.IsNotNull(checkout.getFormContext(checkoutRequest), "Form Context - Conexao falhou com o webserver");
        }

    }
}
