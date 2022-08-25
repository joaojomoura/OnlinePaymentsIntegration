using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePaymentsIntegration.SIBS.SDK
{
    public class BasicPayment
    {
        private string amount, currency;
        private PaymentType paymentType;
        private Authentication authentication;
        private CustomerInfo customer;

        // Constructor of Basic Payment given only strings
        public BasicPayment(string amount, string currency, string entityId, string bearer, string terminalId, string multibancoEntity, CustomerInfo customer) {
            this.amount = amount;
            this.currency = currency;
            authentication = new Authentication(entityId, bearer, terminalId, multibancoEntity);
            this.customer = customer;
        }
        
        // Constructor of Basic Payment given  merchantId string 
        public BasicPayment(string amount, string currency, string entityId, string bearer, string terminalId, string multibancoEntity, string merchantTransactionId, CustomerInfo customer) {
            this.amount = amount;
            this.currency = currency;
            authentication = new Authentication(entityId, bearer, terminalId ,multibancoEntity, merchantTransactionId);
            this.customer = customer;
        }


        // Return the amount
        public string getAmount { get { return amount; } }

        public string getBearer { get { return authentication.getBearer; } }

        // Return the string for checkout request
        public virtual string dataForPaymentBasic {
            get { return "\"merchant\":{\n" +
                                "\"terminalId\": " + authentication.getTerminalId + "," +
                                "\"channel\": \"web\",\n" +
                                "\"merchantTransactionId\": \"\"\n}," +
                         "\"transaction\": {" +
                                "\n\"transactionTimestamp\": \"Current Date\",\n" +
                                "\"description\": \"My transaction -> Order \"\",\n" +
                                "\"moto\": false,\n" +
                                "\"paymentType\": \"" + PaymentType.PURS.ToString() + "\",\n" +
                                "\"amount\": { \n" +
                                    "\"value\": " + amount + ",\n" +
                                    "\"currency\": \"" + currency.ToUpper() +"\"\n" +
                                    "},\n" +
                                "\"paymentMethod\":[\n" +
                                    "\"" + PaymentBrand.REFERENCE.ToString() + "\",\n" +
                                    "\"" + PaymentBrand.CARD.ToString() + "\",\n" +
                                    "\"" + PaymentBrand.MBWAY.ToString() + "\"\n" +
                                    "]," +
                                "\"paymentReference\": {\n" +
                                    "\"initialDatetime\":\"Current Date\",\n" +
                                    "\"finalDatetime\":\"Expiration  Date\",\n" +
                                    "\"maxAmount\":{\n" +
                                        "\"value\": 500,\n" +
                                        "\"currency\":\"" + currency.ToUpper()+ "\"" +
                                        "},\n" +
                                "\"entity\": \"" + authentication.getmultibancoEntity + "\"\n" +
                                "}\n" +
                           "}\n" +
                      "}\n" +
                       "\"customer\":{\n" +
                            "\"customerName\":\""+ customer.getClientName +"\",\n" +
                            "\"customerEmail\":\""+customer.getClientEmail +"\",\n" +
                            "\"shippingAddress\":{\n" +
                                    "\"street1\":\""+ customer.shippingAddress.street1 +"\",\n" +
                                    "\"street2\":\""+ customer.shippingAddress.street2 +"\",\n" +
                                    "\"postcode\":\""+ customer.shippingAddress.postcode +"\",\n" +
                                    "\"country\":\""+customer.shippingAddress.city +"\"\n" +
                                    "},\n" +
                            "\"billingAddress\":{\n" +
                                    "\"street1\":\"" + customer.shippingAddress.street1 + "\",\n" +
                                    "\"street2\":\"" + customer.shippingAddress.street2 + "\",\n" +
                                    "\"postcode\":\"" + customer.shippingAddress.postcode + "\",\n" +
                                    "\"country\":\"" + customer.shippingAddress.city + "\"\n" +
                                    "}\n" +
                             "}\n" +
                       "}";
            }
        }

        // Return the string for checkout request with merchant Id
        public virtual string dataForPaymentBasicwithMerchantTransactionId {
            get {
                return "\"merchant\":{\n" +
                                "\"terminalId\": " + authentication.getTerminalId + "," +
                                "\"channel\": \"web\",\n" +
                                "\"merchantTransactionId\": \""+ authentication.getmerchantTransactionId +"\"\n" +
                                "}," +
                         "\"transaction\": {" +
                                "\n\"transactionTimestamp\": \"Current Date\",\n" +
                                "\"description\": \"My transaction -> Order \"\",\n" +
                                "\"moto\": false,\n" +
                                "\"paymentType\": \"" + PaymentType.PURS.ToString() + "\",\n" +
                                "\"amount\": { \n" +
                                    "\"value\": " + amount + ",\n" +
                                    "\"currency\": \"" + currency.ToUpper() + "\"\n" +
                                    "},\n" +
                                "\"paymentMethod\":[\n" +
                                    "\"" + PaymentBrand.REFERENCE.ToString() + "\",\n" +
                                    "\"" + PaymentBrand.CARD.ToString() + "\",\n" +
                                    "\"" + PaymentBrand.MBWAY.ToString() + "\"\n" +
                                    "]," +
                                "\"paymentReference\": {\n" +
                                    "\"initialDatetime\":\"Current Date\",\n" +
                                    "\"finalDatetime\":\"Expiration  Date\",\n" +
                                    "\"maxAmount\":{\n" +
                                        "\"value\": 500,\n" +
                                        "\"currency\":\"" + currency.ToUpper() + "\"" +
                                        "},\n" +
                                "\"entity\": \"" + authentication.getmultibancoEntity + "\"\n" +
                                "}\n" +
                           "}\n" +
                      "}\n" +
                       "\"customer\":{\n" +
                            "\"customerName\":\"" + customer.getClientName + "\",\n" +
                            "\"customerEmail\":\"" + customer.getClientEmail + "\",\n" +
                            "\"shippingAddress\":{\n" +
                                    "\"street1\":\"" + customer.shippingAddress.street1 + "\",\n" +
                                    "\"street2\":\"" + customer.shippingAddress.street2 + "\",\n" +
                                    "\"postcode\":\"" + customer.shippingAddress.postcode + "\",\n" +
                                    "\"country\":\"" + customer.shippingAddress.city + "\"\n" +
                                    "},\n" +
                            "\"billingAddress\":{\n" +
                                    "\"street1\":\"" + customer.shippingAddress.street1 + "\",\n" +
                                    "\"street2\":\"" + customer.shippingAddress.street2 + "\",\n" +
                                    "\"postcode\":\"" + customer.shippingAddress.postcode + "\",\n" +
                                    "\"country\":\"" + customer.shippingAddress.city + "\"\n" +
                                    "}\n" +
                             "}\n" +
                       "}";
            }
        }

        public virtual string dataForPaymentBasicforTest {
            get {
                return "\"merchant\":{\n" +
                                "\"terminalId\": " + authentication.getTerminalId + "," +
                                "\"channel\": \"web\",\n" +
                                "\"merchantTransactionId\": \"\"\n}," +
                        "\"transaction\": {" +
                                "\n\"transactionTimestamp\": \"Current Date\",\n" +
                                "\"description\": \"My transaction -> Order \"\",\n" +
                                "\"moto\": false,\n" +
                                "\"paymentType\": \"" + PaymentType.PURS.ToString() + "\",\n" +
                                "\"amount\": { \n" +
                                    "\"value\": " + amount + ",\n" +
                                    "\"currency\": \"" + currency.ToUpper() + "\"\n" +
                                    "},\n" +
                                "\"paymentMethod\":[\n" +
                                    "\"" + PaymentBrand.REFERENCE.ToString() + "\",\n" +
                                    "\"" + PaymentBrand.CARD.ToString() + "\",\n" +
                                    "\"" + PaymentBrand.MBWAY.ToString() + "\"\n" +
                                    "]," +
                                "\"paymentReference\": {\n" +
                                    "\"initialDatetime\":\"Current Date\",\n" +
                                    "\"finalDatetime\":\"Expiration  Date\",\n" +
                                    "\"maxAmount\":{\n" +
                                        "\"value\": 500,\n" +
                                        "\"currency\":\"" + currency.ToUpper() + "\"" +
                                        "},\n" +
                                "\"entity\": \"" + authentication.getmultibancoEntity + "\"\n" +
                                "}\n" +
                           "}\n" +
                        "}\n" +
                       "\"customer\":{\n" +
                            "\"customerName\":\"Client Name\",\n" +
                            "\"customerEmail\":\"client.name@hostname.pt\",\n" +
                            "\"shippingAddress\":{\n" +
                                    "\"street1\":\"Rua123\",\n" +
                                    "\"street2\":\"porta 2\",\n" +
                                    "\"postcode\":\"1200-999\",\n" +
                                    "\"country\":\"PT\"\n" +
                                    "},\n" +
                            "\"billingAddress\":{\n" +
                                     "\"street1\":\"Rua123\",\n" +
                                    "\"street2\":\"porta 2\",\n" +
                                    "\"postcode\":\"1200-999\",\n" +
                                    "\"country\":\"PT\"\n" +
                                    "}\n" +
                             "}\n" +
                       "}";
            }
        }

        public virtual string dataForPaymentBasicwithMerchantTransactionIdforTest {
            get {
                return "\"merchant\":{\n" +
                                "\"terminalId\": " + authentication.getTerminalId + "," +
                                "\"channel\": \"web\",\n" +
                                "\"merchantTransactionId\": \"" + authentication.getmerchantTransactionId + "\"\n" +
                                "}," +
                         "\"transaction\": {" +
                                "\n\"transactionTimestamp\": \"Current Date\",\n" +
                                "\"description\": \"My transaction -> Order \"\",\n" +
                                "\"moto\": false,\n" +
                                "\"paymentType\": \"" + PaymentType.PURS.ToString() + "\",\n" +
                                "\"amount\": { \n" +
                                    "\"value\": " + amount + ",\n" +
                                    "\"currency\": \"" + currency.ToUpper() + "\"\n" +
                                    "},\n" +
                                "\"paymentMethod\":[\n" +
                                    "\"" + PaymentBrand.REFERENCE.ToString() + "\",\n" +
                                    "\"" + PaymentBrand.CARD.ToString() + "\",\n" +
                                    "\"" + PaymentBrand.MBWAY.ToString() + "\"\n" +
                                    "]," +
                                "\"paymentReference\": {\n" +
                                    "\"initialDatetime\":\"Current Date\",\n" +
                                    "\"finalDatetime\":\"Expiration  Date\",\n" +
                                    "\"maxAmount\":{\n" +
                                        "\"value\": 500,\n" +
                                        "\"currency\":\"" + currency.ToUpper() + "\"" +
                                        "},\n" +
                                "\"entity\": \"" + authentication.getmultibancoEntity + "\"\n" +
                                "}\n" +
                           "}\n" +
                      "}\n" +
                      "\"customer\":{\n" +
                            "\"customerName\":\"Client Name\",\n" +
                            "\"customerEmail\":\"client.name@hostname.pt\",\n" +
                            "\"shippingAddress\":{\n" +
                                    "\"street1\":\"Rua123\",\n" +
                                    "\"street2\":\"porta 2\",\n" +
                                    "\"postcode\":\"1200-999\",\n" +
                                    "\"country\":\"PT\"\n" +
                                    "},\n" +
                            "\"billingAddress\":{\n" +
                                     "\"street1\":\"Rua123\",\n" +
                                    "\"street2\":\"porta 2\",\n" +
                                    "\"postcode\":\"1200-999\",\n" +
                                    "\"country\":\"PT\"\n" +
                                    "}\n" +
                             "}\n" +
                       "}";
            }
        }

    }
}