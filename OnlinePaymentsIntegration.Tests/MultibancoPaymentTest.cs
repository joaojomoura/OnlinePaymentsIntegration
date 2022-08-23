using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OnlinePaymentsIntegration.SIBS.SDK;

namespace OnlinePaymentsIntegration.Tests
{
    [TestClass]
    public class MultibancoPaymentTest
    {
        [TestMethod]
        public void ValidTextforMultibanco() {
            string amount = "10.00", entityId = "8a8294185332bbe601533754724914d9", currency = "EUR",
               paymentType = "PA", merchantId = "geracaoMB1", bearer = "OGE4Mjk0MTg1MzMyYmJlNjAxNTMzNzU0NzZjMzE1Mjd8RzV3UDVUekY1aw==",
               multibancoEntity = "25002";

            // no merchant
            MultibancoPayment multibancoPayment = new MultibancoPayment(amount, currency, paymentType, entityId, bearer,multibancoEntity);

            string initialTransactionTime = multibancoPayment.initialTransactionTime;
            string endTransactionTime = multibancoPayment.endTransactionTime;

            string expectedMultibancoPaymentforLive = "entityId=8a8294185332bbe601533754724914d9" +
                "&amount=10.00&currency=EUR&paymentType=PA&billing.country=PT&customParameters[SIBSMULTIBANCO_PtmntEntty]=25002" +
                "&customParameters[SIBSMULTIBANCO_RefIntlDtTm]=" + initialTransactionTime + "&customParameters[SIBSMULTIBANCO_RefLmtDtTm]=" + endTransactionTime;

            string expectedMultibancoPaymentforTest = "entityId=8a8294185332bbe601533754724914d9" +
                "&amount=10.00&currency=EUR&paymentType=PA&testMode=EXTERNAL&customParameters[SIBS_ENV]=QLY&billing.country=PT&customParameters[SIBSMULTIBANCO_PtmntEntty]=25002" +
                "&customParameters[SIBSMULTIBANCO_RefIntlDtTm]=" + initialTransactionTime + "&customParameters[SIBSMULTIBANCO_RefLmtDtTm]=" + endTransactionTime;

            // with merchant
            MultibancoPayment multibancoPaymentwithMerchant = new MultibancoPayment(amount, currency, paymentType, entityId, bearer, merchantId, multibancoEntity);
            string initialTransactionTimeMerch = multibancoPaymentwithMerchant.initialTransactionTime;
            string endTransactionTimeMerch = multibancoPaymentwithMerchant.endTransactionTime;

            string expectedMultibancoPaymentforLiveMerchant = "entityId=8a8294185332bbe601533754724914d9" +
                "&amount=10.00&currency=EUR&paymentType=PA&merchantTransactionId=geracaoMB1&billing.country=PT&customParameters[SIBSMULTIBANCO_PtmntEntty]=25002" +
                "&customParameters[SIBSMULTIBANCO_RefIntlDtTm]=" + initialTransactionTimeMerch + "&customParameters[SIBSMULTIBANCO_RefLmtDtTm]=" + endTransactionTimeMerch;

            string expectedMultibancoPaymentforTestMerchant = "entityId=8a8294185332bbe601533754724914d9" +
                "&amount=10.00&currency=EUR&paymentType=PA&merchantTransactionId=geracaoMB1" +
                "&testMode=EXTERNAL&customParameters[SIBS_ENV]=QLY&billing.country=PT&customParameters[SIBSMULTIBANCO_PtmntEntty]=25002" +
                "&customParameters[SIBSMULTIBANCO_RefIntlDtTm]=" + initialTransactionTimeMerch + "&customParameters[SIBSMULTIBANCO_RefLmtDtTm]=" + endTransactionTimeMerch;

            Assert.AreEqual(expectedMultibancoPaymentforLive,multibancoPayment.dataForPaymentBasic,"Multibanco - Live sem merchant nao sao iguais");
            Assert.AreEqual(expectedMultibancoPaymentforTest, multibancoPayment.dataForPaymentBasicforTest, "Multibanco - Test sem merchant nao sao iguais");
            Assert.AreEqual(expectedMultibancoPaymentforLiveMerchant, multibancoPaymentwithMerchant.dataForPaymentBasicwithMerchantId, "Multibanco - Live com merchant nao sao iguais");
            Assert.AreEqual(expectedMultibancoPaymentforTestMerchant, multibancoPaymentwithMerchant.dataForPaymentBasicwithMerchantIdforTest, "Multibanco - Test com merchant nao sao iguais");

        }
    }
}
