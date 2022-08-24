using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlinePaymentsIntegration.SIBS.SDK;
using System;

namespace OnlinePaymentsIntegration.Tests
{
    [TestClass]
    public class CheckoutRequestTest
    {

        string amount = "92.00", entityId = "8a8294185332bbe601533754724914d9", currency = "EUR",
               paymentType = "DB", merchantId = "merchant", bearer = "OGE4Mjk0MTg1MzMyYmJlNjAxNTMzNzU0NzZjMzE1Mjd8RzV3UDVUekY1aw==",
            multibancoEntity = "25002";


        [TestMethod]
        public void checkIfPaymentMethodWasChosenCorrectly() {
            
            var checkout = new CheckoutRequestCopyAndPay(amount,currency,paymentType,entityId,bearer,PaymentBrand.VISA);
            var checkbrand = checkout.checkPaymentBrand(PaymentBrand.VISA);
            Assert.IsTrue(checkbrand,"Foi escolhido Multibanco");
        }

        [TestMethod]
        public void checkCheckoutRequestMadeSucesseful() {

            var checkout = new CheckoutRequestCopyAndPay(amount, currency, paymentType, entityId, bearer, PaymentBrand.VISA);
            var checkoutReponse = checkout.getCheckoutRequestForTests();
            Assert.IsNotNull(checkoutReponse,"Conexao falhou com o webserver");
        }
        [TestMethod]
        public void checkIfCodeWas000() {
            var checkout = new CheckoutRequestCopyAndPay(amount, currency, paymentType, entityId, bearer, merchantId, PaymentBrand.SIBS_MULTIBANCO);
            var checkoutId = checkout.getCheckoutRequestForTests();
            Assert.IsTrue((checkoutId["result"]["code"]).Equals("000.200.100"));
        }




    }
}
