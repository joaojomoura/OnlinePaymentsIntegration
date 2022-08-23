using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OnlinePaymentsIntegration.SIBS.SDK;

namespace OnlinePaymentsIntegration.Tests
{
    [TestClass]
    public class MbwayPaymentTest
    {
        [TestMethod]
        public void ValidTextMbwayPayment() {
            string amount = "10.00", entityId = "8a8294185332bbe601533754724914d9", currency = "EUR",
               paymentType = "DB", merchantId = "merchant", bearer = "OGE4Mjk0MTg1MzMyYmJlNjAxNTMzNzU0NzZjMzE1Mjd8RzV3UDVUekY1aw==";


            //what to expect in live production
            string expectedMbwayPaymentforLive = "entityId=8a8294185332bbe601533754724914d9" +
                "&amount=10.00&currency=EUR&paymentType=DB";
            string expectedMbwayPaymentforLivewithMerchant = "entityId=8a8294185332bbe601533754724914d9" +
                "&amount=10.00&currency=EUR&paymentType=DB&merchantTransactionId=merchant";

            //what to expect in test production
            string expectedMbwayPaymentforTest = "entityId=8a8294185332bbe601533754724914d9" +
                "&amount=10.00&currency=EUR&paymentType=DB&testMode=EXTERNAL&customParameters[SIBS_ENV]=QLY";
            string expectedMbwayPaymentforTestwithMerchant = "entityId=8a8294185332bbe601533754724914d9" +
                "&amount=10.00&currency=EUR&paymentType=DB&merchantTransactionId=merchant" +
                "&testMode=EXTERNAL&customParameters[SIBS_ENV]=QLY";


            // No merchant 
            MbwayPayment mbwayPayment = new MbwayPayment(amount, currency, paymentType, entityId, bearer);
            // with merchant
            MbwayPayment mbwayPaymentwithMerchant = new MbwayPayment(amount, currency, paymentType, entityId, bearer,merchantId);

            Assert.AreEqual(expectedMbwayPaymentforLive, mbwayPayment.dataForPaymentBasic, "MBWAY - Live sem merchant nao sao iguais");
            Assert.AreEqual(expectedMbwayPaymentforLivewithMerchant, mbwayPaymentwithMerchant.dataForPaymentBasicwithMerchantId, "MBWAY - Live com merchant nao sao iguais");
            Assert.AreEqual(expectedMbwayPaymentforTest, mbwayPayment.dataForPaymentBasicforTest, "MBWAY - Test sem merchant nao sao iguais");
            Assert.AreEqual(expectedMbwayPaymentforTestwithMerchant, mbwayPaymentwithMerchant.dataForPaymentBasicwithMerchantIdforTest, "MBWAY - Test com merchant nao sao iguais");



        }
    }
}
