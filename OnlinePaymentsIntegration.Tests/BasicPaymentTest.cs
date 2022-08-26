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
            string amount = "5.5", terminaltId = "52221", currency = "EUR",
                paymentType = "PURS", merchantTransactionId = "6lvbdn5i5w", bearer = "123", clientId = "123",
                multibancoEntity = "24000";
            string customerName = "Client Name", customerEmail = "client.name@hostname.pt", street1 = "Rua 123",
                street2 = "porta 2", city = "Lisboa", postcode = "1200-999", country = "PT";
            CustomerInfo customer = new CustomerInfo(customerName,customerEmail);
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



            //what to expect in test production
            string expectedBasicPaymentforTest = @"{
""merchant"": {
""terminalId"": 52221,
""channel"": ""web"",
""merchantTransactionId"": ""My transaction -> ""
},
""transaction"": {
""transactionTimestamp"": ""2022-08-26T08:39:30.480Z"",
""description"": ""My transaction -> "",
""moto"": false,
""paymentType"": ""PURS"",
""amount"": {
""value"": 5.5,
""currency"": ""EUR""
},
""paymentMethod"": [
""CARD"",
""MBWAY"",
""REFERENCE""
],
""paymentReference"": {
""initialDatetime"": ""2022-08-26T08:39:30.480Z"",
""finalDatetime"": ""2022-08-28T08:39:30.480Z"",
""maxAmount"": {
""value"": 500,
""currency"": ""EUR""
},
""minAmount"": {
""value"": 5,
""currency"": ""EUR""
},
""entity"": ""24000""
}
}
""customer"": {
""customerInfo"": {
""customerName"": ""Client Name"",
""customerEmail"": ""client.name@hostname.pt"",
""shippingAddress"": {
""street1"": ""Rua 123"",
""street2"": ""porta 2"",
""city"": ""Lisboa"",
""postcode"": ""1200-999"",
""country"": ""PT""
},
""billingAddress"": {
""street1"": ""Rua 123"",
""street2"": ""porta 2"",
""city"": ""Lisboa"",
""postcode"": ""1200-999"",
""country"": ""PT""
}
}
}
}";
            string expectedBasicPaymentforTestwithMerchant = @"{
""merchant"": {
""terminalId"": 52221,
""channel"": ""web"",
""merchantTransactionId"": ""My transaction -> Order: 6lvbdn5i5w""
},
""transaction"": {
""transactionTimestamp"": ""2022-08-26T08:39:30.480Z"",
""description"": ""My transaction -> Order: 6lvbdn5i5w"",
""moto"": false,
""paymentType"": ""PURS"",
""amount"": {
""value"": 5.5,
""currency"": ""EUR""
},
""paymentMethod"": [
""CARD"",
""MBWAY"",
""REFERENCE""
],
""paymentReference"": {
""initialDatetime"": ""2022-08-26T08:39:30.480Z"",
""finalDatetime"": ""2022-08-28T08:39:30.480Z"",
""maxAmount"": {
""value"": 500,
""currency"": ""EUR""
},
""minAmount"": {
""value"": 5,
""currency"": ""EUR""
},
""entity"": ""24000""
}
}
""customer"": {
""customerInfo"": {
""customerName"": ""Client Name"",
""customerEmail"": ""client.name@hostname.pt"",
""shippingAddress"": {
""street1"": ""Rua 123"",
""street2"": ""porta 2"",
""city"": ""Lisboa"",
""postcode"": ""1200-999"",
""country"": ""PT""
},
""billingAddress"": {
""street1"": ""Rua 123"",
""street2"": ""porta 2"",
""city"": ""Lisboa"",
""postcode"": ""1200-999"",
""country"": ""PT""
}
}
}
}";

            //Constructor without merchant
            BasicPayment basicPayment = new BasicPayment(amount,currency,clientId,bearer,terminaltId, multibancoEntity,customer);
            //Constructor with merchant
            BasicPayment basicPaymentwithMerchant = new BasicPayment(amount, currency, clientId, bearer, terminaltId, multibancoEntity,merchantTransactionId, customer);

            Assert.AreEqual(amount,basicPayment.getAmount,"Amount diferente");
            Assert.AreEqual("Bearer " + bearer, basicPayment.getBearer, "bearer diferente");
            Assert.AreEqual(clientId, basicPayment.getClientId, "clientId diferente");
            



        }
    }
}
