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
            string merchantId = "merchant";
            string entityId = "123456789";
            string bearer = "apapaosop1234";
            //what i expect
            string expectedMerchantId = "&merchantTransactionId=merchant";
            string expectedEntityId = "entityId=123456789";
            string expectedBearer = "Bearer apapaosop1234";

            //constructor without merchantID
            Authentication authenticationWithoutMerchant = new Authentication(entityId,bearer);
            //constructor with merchantID
            Authentication authenticationWithMerchant = new Authentication(entityId,bearer,merchantId);

            // Without Merchant
            Assert.AreEqual(expectedBearer,authenticationWithoutMerchant.getBearer,"Sem merchant - Bearer nao sao iguais");
            Assert.AreEqual(expectedEntityId, authenticationWithoutMerchant.getEntityId, "Sem merchant - Entity nao sao iguais");


            // With Merchant
            Assert.AreEqual(expectedEntityId, authenticationWithMerchant.getEntityId, "Com merchant - Entity nao sao iguais");
            Assert.AreEqual(expectedBearer, authenticationWithMerchant.getBearer, "Com merchant - Bearer nao sao iguais");
            Assert.AreEqual(expectedMerchantId, authenticationWithMerchant.getmerchantId, "Com merchant - Merchant nao sao iguais");
        }
    }
}
