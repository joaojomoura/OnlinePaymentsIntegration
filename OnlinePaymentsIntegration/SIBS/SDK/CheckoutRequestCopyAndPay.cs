using OnlinePaymentsIntegration.SIBS.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace OnlinePaymentsIntegration.SIBS.SDK
{
    public class CheckoutRequestCopyAndPay
    {
        private string readAllJson = string.Empty;
        
        private BasicPayment basicPayment;
        private bool merchantIdNeed = false;
        public string setURLLive = "https://test.oppwa.com/v1/checkouts"; 
        public string setURLTest = "https://spg.qly.site1.sibs.pt/api/v1/payments";

        // Construtor card without merchantId
        public CheckoutRequestCopyAndPay(string amount, string currency, string ClientId, string bearer, string terminalId,string multibancoEntity, CustomerInfo customer) {
            basicPayment = new BasicPayment(amount, currency, ClientId, bearer, terminalId, multibancoEntity, customer);
        }
        // Constructor card with merchantId
        public CheckoutRequestCopyAndPay(string amount, string currency, string ClientId, string bearer, string terminalId,string multibancoEntity, string merchantTransactionId, CustomerInfo customer) { 
            basicPayment = new BasicPayment(amount, currency, ClientId, bearer, terminalId, multibancoEntity, merchantTransactionId, customer);    
             merchantIdNeed = true;
        }

        // Checkout request for live production
        public Dictionary<string, dynamic> getCheckoutRequest() {
            Dictionary<string, dynamic> checkoutData;
            
            string dataToSendWithPost = basicPayment.dataForPaymentBasic;
            if (merchantIdNeed)
                dataToSendWithPost = basicPayment.dataForPaymentBasicwithMerchantTransactionId;
            var url = setURLLive;
            byte[] buffer = Encoding.ASCII.GetBytes(dataToSendWithPost);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.Headers["Authorization"] = basicPayment.getBearer;
            request.Headers["X-IBM-Client-Id"] = basicPayment.getClientId;
            request.ContentType = "application/json";
            Stream PostData = request.GetRequestStream();
            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                readAllJson = reader.ReadToEnd();
                var s = new JavaScriptSerializer();
                checkoutData = s.Deserialize<Dictionary<string, dynamic>>(readAllJson);
                reader.Close();
                dataStream.Close();
            }
            return checkoutData;   
        }
        //Checkout request for test production
        public Dictionary<string, dynamic> getCheckoutRequestForTests() {
            Dictionary<string, dynamic> checkoutData;
            
            string dataToSendWithPost = basicPayment.dataForPaymentBasicforTest;
            if (merchantIdNeed)
                dataToSendWithPost = basicPayment.dataForPaymentBasicwithMerchantTransactionIdforTest;
            var url = setURLTest;
            byte[] buffer = Encoding.ASCII.GetBytes(dataToSendWithPost);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.Headers["Authorization"] = basicPayment.getBearer;
            request.Headers["X-IBM-Client-Id"] = basicPayment.getClientId;
            request.ContentType = "application/json";
            //request.ContentType = "application/x-www-form-urlencoded";
            Stream PostData = request.GetRequestStream();
            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                readAllJson = reader.ReadToEnd();
                var s = new JavaScriptSerializer();
                checkoutData = s.Deserialize<Dictionary<string, dynamic>>(readAllJson);
                reader.Close();
                dataStream.Close();
            }
            return checkoutData;
        }

        public string getTransactionId (Dictionary<string,dynamic> checkoutData) {
            var getResultCode = checkoutData["returnStatus"]["statusCode"];
            if (!getResultCode.Equals("000"))
                throw new OnlinePaymentComunicationException(getResultCode,
                                                    checkoutData["returnStatus"]["statusMsg"]);
            return checkoutData["transactionID"];
        }

        public string getTransactionSignature (Dictionary<string, dynamic> checkoutData) {
            var getResultCode = checkoutData["returnStatus"]["statusCode"];
            if (!getResultCode.Equals("000"))
                throw new OnlinePaymentComunicationException(getResultCode,
                                                    checkoutData["returnStatus"]["statusMsg"]);
            return checkoutData["transactionSignature"];
        }

        public string getFormContext(Dictionary<string, dynamic> checkoutData) {
            var getResultCode = checkoutData["returnStatus"]["statusCode"];
            if (!getResultCode.Equals("000"))
                throw new OnlinePaymentComunicationException(getResultCode,
                                                    checkoutData["returnStatus"]["statusMsg"]);
            return checkoutData["formContext"];
        }

        public string getCheckoutRequestComplete(Dictionary<string, dynamic> getCheckoutRequest) {
            string response = "";
            foreach (KeyValuePair<string, dynamic> kvp in getCheckoutRequest) {
                response += kvp + "\n";
            }
            return response;
        }

        public string getReadAllJson { get { return readAllJson; } }

        public bool checkPaymentBrand(PaymentBrand brand) {
            if (brand == PaymentBrand.CARD
                || brand == PaymentBrand.MBWAY)
                return true;
            return false;
        }
    }
}