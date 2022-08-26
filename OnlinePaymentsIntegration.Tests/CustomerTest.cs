using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OnlinePaymentsIntegration.SIBS.SDK;

namespace OnlinePaymentsIntegration.Tests
{
    [TestClass]
    public class CustomerTest
    {
        [TestMethod]
        public void ValidCustomerTest() {
            // expected
            string expectedCustomerName = "Client Name", expectedCustomerEmail = "client.name@hostname.pt", expectedStreet1 = "Rua 123",
               expectedStreet2 = "porta 2", expectedCity = "Lisboa", expectedPostcode = "1200-999", expectedCountry = "PT";
            string expectedClientEmail = "client.name@hostname.pt", expectedClientName = "Client Name";
            ///////////


            CustomerInfo customer = new CustomerInfo("Client Name", "client.name@hostname.pt");
            customer.shippingAddress.street1 = "Rua 123";
            customer.shippingAddress.street2 = "porta 2";
            customer.shippingAddress.city = "Lisboa";
            customer.shippingAddress.postcode = "1200-999";
            customer.shippingAddress.country = "PT";

            customer.billingAddress.street1 = "Rua 123";
            customer.billingAddress.street2 = "porta 2";
            customer.billingAddress.city = "Lisboa";
            customer.billingAddress.postcode = "1200-999";
            customer.billingAddress.country = "PT";

            Assert.AreEqual(expectedCity,customer.shippingAddress.city,"a cidade em shipping nao e igual");
            Assert.AreEqual(expectedCity, customer.billingAddress.city, "a cidade em billing nao e igual");
            Assert.AreEqual(expectedClientName, customer.getClientName, "a cidade em shipping nao e igual");



        }
    }
}
