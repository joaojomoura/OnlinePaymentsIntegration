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
        public string setURL;
        
       

        /// <summary>
        /// Constructor without Merchant Transaction ID
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <param name="ClientId"></param>
        /// <param name="bearer"></param>
        /// <param name="terminalId"></param>
        /// <param name="multibancoEntity"></param>
        /// <param name="customer"></param>
        public CheckoutRequestCopyAndPay(string url,string amount, string currency, string ClientId, string bearer, string terminalId,string multibancoEntity, CustomerInfo customer) {
            basicPayment = new BasicPayment(amount, currency, ClientId, bearer, terminalId, multibancoEntity, customer);
            setURL = url;
        }

        /// <summary>
        /// Constructor with Merchant Transaction ID
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <param name="ClientId"></param>
        /// <param name="bearer"></param>
        /// <param name="terminalId"></param>
        /// <param name="multibancoEntity"></param>
        /// <param name="merchantTransactionId"></param>
        /// <param name="customer"></param>
        public CheckoutRequestCopyAndPay(string url, string amount, string currency, string ClientId, string bearer, string terminalId,string multibancoEntity, string merchantTransactionId, CustomerInfo customer) { 
            basicPayment = new BasicPayment(amount, currency, ClientId, bearer, terminalId, multibancoEntity, merchantTransactionId, customer);    
             merchantIdNeed = true;
            setURL = url;
        }

        /// <summary>
        /// Checkout request 
        /// </summary>
        /// <returns>Dictionary of the checkout request</returns>
        public Dictionary<string, dynamic> getCheckoutRequest() {
            Dictionary<string, dynamic> checkoutData;
            
            string dataToSendWithPost = basicPayment.dataForPaymentBasic;
            if (merchantIdNeed)
                dataToSendWithPost = basicPayment.dataForPaymentBasicwithMerchantTransactionId;
            var url = setURL;
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
        
        /// <summary>
        /// Returns the transaction ID
        /// </summary>
        /// <param name="checkoutData"></param>
        /// <returns>TransactionId</returns>
        public string getTransactionId (Dictionary<string,dynamic> checkoutData) {
            var getResultCode = checkoutData["returnStatus"]["statusCode"];
            if (!getResultCode.Equals("000"))
                throw new OnlinePaymentComunicationException(getResultCode,
                                                    checkoutData["returnStatus"]["statusMsg"]);
            return checkoutData["transactionID"];
        }
        /// <summary>
        /// Returns Transaction Signature
        /// </summary>
        /// <param name="checkoutData"></param>
        /// <returns>TransactionSignature</returns>
        public string getTransactionSignature (Dictionary<string, dynamic> checkoutData) {
            var getResultCode = checkoutData["returnStatus"]["statusCode"];
            if (!getResultCode.Equals("000"))
                throw new OnlinePaymentComunicationException(getResultCode,
                                                    checkoutData["returnStatus"]["statusMsg"]);
            return checkoutData["transactionSignature"];
        }
        /// <summary>
        /// Returns Form Context
        /// </summary>
        /// <param name="checkoutData"></param>
        /// <returns>formContext</returns>
        public string getFormContext(Dictionary<string, dynamic> checkoutData) {
            var getResultCode = checkoutData["returnStatus"]["statusCode"];
            if (!getResultCode.Equals("000"))
                throw new OnlinePaymentComunicationException(getResultCode,
                                                    checkoutData["returnStatus"]["statusMsg"]);
            return checkoutData["formContext"];
        }
        /// <summary>
        /// One way to print the JSON, its buggy
        /// </summary>
        /// <param name="getCheckoutRequest"></param>
        /// <returns></returns>
        public string getCheckoutRequestComplete(Dictionary<string, dynamic> getCheckoutRequest) {
            string response = "";
            foreach (KeyValuePair<string, dynamic> kvp in getCheckoutRequest) {
                response += kvp + "\n";
            }
            return response;
        }
        /// <summary>
        /// Returns the result checkout JSON in a string
        /// </summary>
        public string getReadAllJson { get { return readAllJson; } }

        public bool checkPaymentBrand(PaymentBrand brand) {
            if (brand == PaymentBrand.CARD
                || brand == PaymentBrand.MBWAY)
                return true;
            return false;
        }
    }
}