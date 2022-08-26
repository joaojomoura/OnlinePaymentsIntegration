using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePaymentsIntegration.SIBS.SDK
{
    public class BasicPayment
    {
        private string amount, currency;
        private Authentication authentication;
        private CustomerInfo customer;
        private string currentDate = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        private string expirationDate = DateTime.Now.AddDays(1).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");

        // Constructor of Basic Payment given only strings
        public BasicPayment(string amount, string currency, string ClientId, string bearer, string terminalId, string multibancoEntity, CustomerInfo customer) {
            this.amount = amount;
            this.currency = currency;
            authentication = new Authentication(ClientId, bearer, terminalId, multibancoEntity);
            this.customer = customer;
        }
        
        // Constructor of Basic Payment given  merchantId string 
        public BasicPayment(string amount, string currency, string ClientId, string bearer, string terminalId, string multibancoEntity, string merchantTransactionId, CustomerInfo customer) {
            this.amount = amount;
            this.currency = currency;
            authentication = new Authentication(ClientId, bearer, terminalId ,multibancoEntity, merchantTransactionId);
            this.customer = customer;
        }


        // Return the amount
        public string getAmount { get { return amount; } }

        public string getBearer { get { return authentication.getBearer; } }

        public string getClientId { get { return authentication.getxIBMClientId; } }

        // Return the string for checkout request
        public virtual string dataForPaymentBasic {
            get { return  "{\n\"merchant\": {\n" +
                                "\"terminalId\": " + authentication.getTerminalId + ",\n" +
                                "\"channel\": \"web\",\n" +
                                "\"merchantTransactionId\": \"My transaction -> \"\n" +
                                "},\n" +
                         "\"transaction\": {" +
                                "\n\"transactionTimestamp\": \"" + currentDate + "\",\n" +
                                "\"description\": \"My transaction -> \",\n" +
                                "\"moto\": false,\n" +
                                "\"paymentType\": \"" + PaymentType.PURS.ToString() + "\",\n" +
                                "\"amount\": {\n" +
                                    "\"value\": " + amount + ",\n" +
                                    "\"currency\": \"" + currency.ToUpper() + "\"\n" +
                                    "},\n" +
                                "\"paymentMethod\": [\n" +
                                    "\"" + PaymentBrand.CARD.ToString() + "\",\n" +
                                    "\"" + PaymentBrand.MBWAY.ToString() + "\",\n" +
                                    "\"" + PaymentBrand.REFERENCE.ToString() + "\"\n" +
                                    "],\n" +
                                "\"paymentReference\": {\n" +
                                    "\"initialDatetime\": \"" + currentDate + "\",\n" +
                                    "\"finalDatetime\": \"" + expirationDate + "\",\n" +
                                    "\"maxAmount\": {\n" +
                                        "\"value\": 500,\n" +
                                        "\"currency\": \"" + currency.ToUpper() + "\"\n" +
                                        "},\n" +
                                    "\"minAmount\": {\n" +
                                        "\"value\": 5,\n" +
                                        "\"currency\": \"" + currency.ToUpper() + "\"\n" +
                                        "},\n" +
                                "\"entity\": \"" + authentication.getmultibancoEntity + "\"\n" +
                                "}\n" +
                           "},\n" +
                       "\"customer\": {\n" +
                          "\"customerInfo\": {\n" +
                            "\"customerName\": \"" + customer.getClientName + "\",\n" +
                            "\"customerEmail\": \"" + customer.getClientEmail + "\",\n" +
                            "\"shippingAddress\": {\n" +
                                    "\"street1\": \"" + customer.shippingAddress.street1 + "\",\n" +
                                    "\"street2\": \"" + customer.shippingAddress.street2 + "\",\n" +
                                    "\"city\": \"" + customer.shippingAddress.city + "\",\n" +
                                    "\"postcode\": \"" + customer.shippingAddress.postcode + "\",\n" +
                                    "\"country\": \"" + customer.shippingAddress.country + "\"\n" +
                                    "},\n" +
                            "\"billingAddress\": {\n" +
                                    "\"street1\": \"" + customer.shippingAddress.street1 + "\",\n" +
                                    "\"street2\": \"" + customer.shippingAddress.street2 + "\",\n" +
                                    "\"city\": \"" + customer.shippingAddress.city + "\",\n" +
                                    "\"postcode\": \"" + customer.shippingAddress.postcode + "\",\n" +
                                    "\"country\": \"" + customer.shippingAddress.country + "\"\n" +
                                    "}\n" +
                             "}\n" +
                       "}\n}";
            }
        }

        // Return the string for checkout request with merchant Id
        public virtual string dataForPaymentBasicwithMerchantTransactionId {
            get {
                return "{\n\"merchant\": {\n" +
                                "\"terminalId\": " + authentication.getTerminalId + ",\n" +
                                "\"channel\": \"web\",\n" +
                                "\"merchantTransactionId\": \"My transaction -> " + authentication.getmerchantTransactionId + "\"\n" +
                                "},\n" +
                         "\"transaction\": {" +
                                "\n\"transactionTimestamp\": \"" + currentDate + "\",\n" +
                                "\"description\": \"My transaction -> " + authentication.getmerchantTransactionId + "\",\n" +
                                "\"moto\": false,\n" +
                                "\"paymentType\": \"" + PaymentType.PURS.ToString() + "\",\n" +
                                "\"amount\": {\n" +
                                    "\"value\": " + amount + ",\n" +
                                    "\"currency\": \"" + currency.ToUpper() + "\"\n" +
                                    "},\n" +
                                "\"paymentMethod\": [\n" +
                                    "\"" + PaymentBrand.CARD.ToString() + "\",\n" +
                                    "\"" + PaymentBrand.MBWAY.ToString() + "\",\n" +
                                    "\"" + PaymentBrand.REFERENCE.ToString() + "\"\n" +
                                    "],\n" +
                                "\"paymentReference\": {\n" +
                                    "\"initialDatetime\": \"" + currentDate + "\",\n" +
                                    "\"finalDatetime\": \"" + expirationDate + "\",\n" +
                                    "\"maxAmount\": {\n" +
                                        "\"value\": 500,\n" +
                                        "\"currency\": \"" + currency.ToUpper() + "\"\n" +
                                        "},\n" +
                                    "\"minAmount\": {\n" +
                                        "\"value\": 5,\n" +
                                        "\"currency\": \"" + currency.ToUpper() + "\"\n" +
                                        "},\n" +
                                "\"entity\": \"" + authentication.getmultibancoEntity + "\"\n" +
                                "}\n" +
                           "},\n" +
                       "\"customer\": {\n" +
                          "\"customerInfo\": {\n" +
                            "\"customerName\": \"" + customer.getClientName + "\",\n" +
                            "\"customerEmail\": \"" + customer.getClientEmail + "\",\n" +
                            "\"shippingAddress\": {\n" +
                                    "\"street1\": \"" + customer.shippingAddress.street1 + "\",\n" +
                                    "\"street2\": \"" + customer.shippingAddress.street2 + "\",\n" +
                                    "\"city\": \"" + customer.shippingAddress.city + "\",\n" +
                                    "\"postcode\": \"" + customer.shippingAddress.postcode + "\",\n" +
                                    "\"country\": \"" + customer.shippingAddress.country + "\"\n" +
                                    "},\n" +
                            "\"billingAddress\": {\n" +
                                    "\"street1\": \"" + customer.shippingAddress.street1 + "\",\n" +
                                    "\"street2\": \"" + customer.shippingAddress.street2 + "\",\n" +
                                    "\"city\": \"" + customer.shippingAddress.city + "\",\n" +
                                    "\"postcode\": \"" + customer.shippingAddress.postcode + "\",\n" +
                                    "\"country\": \"" + customer.shippingAddress.country + "\"\n" +
                                    "}\n" +
                             "}\n" +
                       "}\n}";
            }
        }

        public virtual string dataForPaymentBasicforTest {
            get {
                return "{\n\"merchant\": {\n" +
                                "\"terminalId\": " + authentication.getTerminalId + ",\n" +
                                "\"channel\": \"web\",\n" +
                                "\"merchantTransactionId\": \"My transaction -> Order \"\n" +
                                "},\n" +
                        "\"transaction\": {" +
                                "\n\"transactionTimestamp\": \""+ currentDate + "\",\n" +
                                "\"description\": \"My transaction -> Order\",\n" +
                                "\"moto\": false,\n" +
                                "\"paymentType\": \"" + PaymentType.PURS.ToString() + "\",\n" +
                                "\"amount\": {\n" +
                                    "\"value\": " + amount + ",\n" +
                                    "\"currency\": \"" + currency.ToUpper() + "\"\n" +
                                    "},\n" +
                                "\"paymentMethod\": [\n" +
                                    "\"" + PaymentBrand.CARD.ToString() + "\",\n" +
                                    "\"" + PaymentBrand.MBWAY.ToString() + "\",\n" +
                                    "\"" + PaymentBrand.REFERENCE.ToString() + "\"\n" +
                                    "],\n" +
                                "\"paymentReference\": {\n" +
                                    "\"initialDatetime\": \"" + currentDate + "\",\n" +
                                    "\"finalDatetime\": \"" + expirationDate + "\",\n" +
                                    "\"maxAmount\": {\n" +
                                        "\"value\": 500,\n" +
                                        "\"currency\": \"" + currency.ToUpper() + "\"\n" +
                                        "},\n" +
                                     "\"minAmount\": {\n" +
                                        "\"value\": 5,\n" +
                                        "\"currency\": \"" + currency.ToUpper() + "\"\n" +
                                        "},\n" +
                                "\"entity\": \"" + authentication.getmultibancoEntity + "\"\n" +
                                "}\n" +
                           "},\n" +
                       "\"customer\": {\n" +
                            "\"customerInfo\": {\n" +
                            "\"customerName\": \"Client Name\",\n" +
                            "\"customerEmail\": \"client.name@hostname.pt\",\n" +
                            "\"shippingAddress\": {\n" +
                                    "\"street1\": \"Rua 123\",\n" +
                                    "\"street2\": \"porta 2\",\n" +
                                    "\"city\": \"Lisboa\",\n" +
                                    "\"postcode\": \"1200-999\",\n" +
                                    "\"country\": \"PT\"\n" +
                                    "},\n" +
                            "\"billingAddress\": {\n" +
                                     "\"street1\": \"Rua 123\",\n" +
                                    "\"street2\": \"porta 2\",\n" +
                                     "\"city\": \"Lisboa\",\n" +
                                    "\"postcode\": \"1200-999\",\n" +
                                    "\"country\": \"PT\"\n" +
                                    "}\n" +
                             "}\n" +
                       "}\n}";
            }
        }

        public virtual string dataForPaymentBasicwithMerchantTransactionIdforTest {
            get {
                return "{\n\"merchant\": {\n" +
                                "\"terminalId\": " + authentication.getTerminalId + ",\n" +
                                "\"channel\": \"web\",\n" +
                                "\"merchantTransactionId\": \"My transaction -> " + authentication.getmerchantTransactionId + "\"\n" +
                                "},\n" +
                         "\"transaction\": {" +
                                "\n\"transactionTimestamp\": \"" + currentDate + "\",\n" +
                                "\"description\": \"My transaction -> " + authentication.getmerchantTransactionId + "\",\n" +
                                "\"moto\": false,\n" +
                                "\"paymentType\": \"" + PaymentType.PURS.ToString() + "\",\n" +
                                "\"amount\": {\n" +
                                    "\"value\": " + amount + ",\n" +
                                    "\"currency\": \"" + currency.ToUpper() + "\"\n" +
                                    "},\n" +
                                "\"paymentMethod\": [\n" +
                                    "\"" + PaymentBrand.CARD.ToString() + "\",\n" +
                                    "\"" + PaymentBrand.MBWAY.ToString() + "\",\n" +
                                    "\"" + PaymentBrand.REFERENCE.ToString() + "\"\n" +
                                    "],\n" +
                                "\"paymentReference\": {\n" +
                                    "\"initialDatetime\": \"" + currentDate + "\",\n" +
                                    "\"finalDatetime\": \"" + expirationDate + "\",\n" +
                                    "\"maxAmount\": {\n" +
                                        "\"value\": 500,\n" +
                                        "\"currency\": \"" + currency.ToUpper() + "\"\n" +
                                        "},\n" +
                                    "\"minAmount\": {\n" +
                                        "\"value\": 5,\n" +
                                        "\"currency\": \"" + currency.ToUpper() + "\"\n" +
                                        "},\n" +
                                "\"entity\": \"" + authentication.getmultibancoEntity + "\"\n" +
                                "}\n" +
                           "},\n" +
                      "\"customer\": {\n" +
                            "\"customerInfo\": {\n" +
                            "\"customerName\": \"Client Name\",\n" +
                            "\"customerEmail\": \"client.name@hostname.pt\",\n" +
                            "\"shippingAddress\": {\n" +
                                    "\"street1\": \"Rua 123\",\n" +
                                    "\"street2\": \"porta 2\",\n" +
                                    "\"city\": \"Lisboa\",\n" +
                                    "\"postcode\": \"1200-999\",\n" +
                                    "\"country\": \"PT\"\n" +
                                    "},\n" +
                            "\"billingAddress\": {\n" +
                                     "\"street1\": \"Rua 123\",\n" +
                                    "\"street2\": \"porta 2\",\n" +
                                     "\"city\": \"Lisboa\",\n" +
                                    "\"postcode\": \"1200-999\",\n" +
                                    "\"country\": \"PT\"\n" +
                                    "}\n" +
                             "}\n" +
                       "}\n}";
            }
        }

 
    }
}