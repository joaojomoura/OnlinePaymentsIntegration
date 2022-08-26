using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OnlinePaymentsIntegration.SIBS.SDK;

namespace OnlinePaymentsIntegration.Tests
{
    [TestClass]
    public class AuthenticationTest
    {
        [TestMethod]
        public void ValidAuthentication() {
            //examples for test
            string merchantTransactionId = "merchant";
            string clientId = "123456789";
            string bearer = "apapaosop1234";
            string multibancoEntity = "1234";
            string terminalId = "22222";
            //what i expect
            string expectedMerchantTransactionId = "Order: merchant";
            string expectedClientId = "123456789";
            string expectedBearer = "Bearer apapaosop1234";
            string expectedMultibancoEntity = "1234";
            string expectedTerminalId = "22222";

            //constructor without merchantID
            Authentication authenticationWithoutMerchant = new Authentication(clientId,bearer,terminalId,multibancoEntity);
            //constructor with merchantID
            Authentication authenticationWithMerchant = new Authentication(clientId,bearer,terminalId,multibancoEntity, merchantTransactionId);

            // Without Merchant
            Assert.AreEqual(expectedBearer,authenticationWithoutMerchant.getBearer,"Sem merchant - Bearer nao sao iguais");
            Assert.AreEqual(expectedClientId, authenticationWithoutMerchant.getxIBMClientId, "Sem merchant - ClientID nao sao iguais");
            Assert.AreEqual(expectedTerminalId, authenticationWithoutMerchant.getTerminalId, "Sem merchant - terminalID nao sao iguais");
            Assert.AreEqual(expectedMultibancoEntity, authenticationWithoutMerchant.getmultibancoEntity, "Sem merchant - multibancoEntity nao sao iguais");



            // With Merchant
            Assert.AreEqual(expectedBearer, authenticationWithMerchant.getBearer, "Com merchant - Bearer nao sao iguais");
            Assert.AreEqual(expectedClientId, authenticationWithMerchant.getxIBMClientId, "Com merchant - ClientID nao sao iguais");
            Assert.AreEqual(expectedTerminalId, authenticationWithMerchant.getTerminalId, "Com merchant - terminalID nao sao iguais");
            Assert.AreEqual(expectedMultibancoEntity, authenticationWithMerchant.getmultibancoEntity, "Com merchant - multibancoEntity nao sao iguais");
            Assert.AreEqual(expectedMerchantTransactionId, authenticationWithMerchant.getmerchantTransactionId, "Com merchant - merchantTransactionID nao sao iguais");
        }
    }
}
