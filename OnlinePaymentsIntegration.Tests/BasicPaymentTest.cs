using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OnlinePaymentsIntegration.SIBS.SDK;

namespace OnlinePaymentsIntegration.Tests
{
    [TestClass]
    public class BasicPaymentTest
    {
        [TestMethod]
        public void ValidTestforPayment() {
            //examples to test
            string amount = "92.00", entityId = "8a8294185332bbe601533754724914d9", currency = "EUR",
                paymentType = "DB", merchantId = "merchant", bearer = "OGE4Mjk0MTg1MzMyYmJlNjAxNTMzNzU0NzZjMzE1Mjd8RzV3UDVUekY1aw==";

            //what to expect in live production
            string expectedBasicPaymentforLive = "entityId=8a8294185332bbe601533754724914d9" +
                "&amount=92.00&currency=EUR&paymentType=DB";
            string expectedBasicPaymentforLivewithMerchant = "entityId=8a8294185332bbe601533754724914d9" +
                "&amount=92.00&currency=EUR&paymentType=DB&merchantTransactionId=merchant";

            //what to expect in test production
            string expectedBasicPaymentforTest = "entityId=8a8294185332bbe601533754724914d9" +
                "&amount=92.00&currency=EUR&paymentType=DB&testMode=EXTERNAL&customParameters[SIBS_ENV]=QLY";
            string expectedBasicPaymentforTestwithMerchant = "entityId=8a8294185332bbe601533754724914d9" +
                "&amount=92.00&currency=EUR&paymentType=DB&merchantTransactionId=merchant" +
                "&testMode=EXTERNAL&customParameters[SIBS_ENV]=QLY";

            //Constructor without merchant
            BasicPayment basicPayment = new BasicPayment(amount,currency,paymentType,entityId,bearer);
            //Constructor with merchant
            BasicPayment basicPaymentwithMerchant = new BasicPayment(amount, currency, paymentType, entityId, bearer,merchantId);

            Assert.AreEqual(expectedBasicPaymentforLive, basicPayment.dataForPaymentBasic,"Live sem merchant nao sao iguais");
            Assert.AreEqual(expectedBasicPaymentforLivewithMerchant, basicPaymentwithMerchant.dataForPaymentBasicwithMerchantId, "Live com merchant nao sao iguais");
            Assert.AreEqual(expectedBasicPaymentforTest, basicPayment.dataForPaymentBasicforTest, "Test sem merchant nao sao iguais");
            Assert.AreEqual(expectedBasicPaymentforTestwithMerchant, basicPaymentwithMerchant.dataForPaymentBasicwithMerchantIdforTest, "Test com merchant nao sao iguais");
        }
    }
}
