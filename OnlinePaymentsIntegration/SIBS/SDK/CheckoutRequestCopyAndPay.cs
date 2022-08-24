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
        private string multibancoEntity = "25002";
        private BasicPayment basicPayment;
        private bool merchantIdNeed = false;
        public string setURLLive = "https://test.oppwa.com/v1/checkouts"; 
        public string setURLTest = "https://test.oppwa.com/v1/checkouts";

        // Construtor card without merchantId
        public CheckoutRequestCopyAndPay(string amount, string currency, string paymentType, string entityId, string bearer,PaymentBrand brand) {
            if(checkPaymentBrand(brand))
                basicPayment = new BasicPayment(amount, currency, paymentType, entityId, bearer);
            else
                basicPayment = new MultibancoPayment(amount, currency, paymentType, entityId, bearer, multibancoEntity);

        }
        // Constructor card with merchantId
        public CheckoutRequestCopyAndPay(string amount, string currency, string paymentType, string entityId, string bearer, string merchantId, PaymentBrand brand) {
            if(checkPaymentBrand(brand))
                basicPayment = new BasicPayment(amount, currency, paymentType, entityId, bearer, merchantId);
            else
                basicPayment = new MultibancoPayment(amount, currency, paymentType, entityId, bearer, merchantId, multibancoEntity);
            merchantIdNeed = true;
        }

        // Checkout request for live production
        public Dictionary<string, dynamic> getCheckoutRequest() {
            Dictionary<string, dynamic> checkoutData;
            
            string dataToSendWithPost = basicPayment.dataForPaymentBasic;
            if (merchantIdNeed)
                dataToSendWithPost = basicPayment.dataForPaymentBasicwithMerchantId;
            var url = setURLLive;
            byte[] buffer = Encoding.ASCII.GetBytes(dataToSendWithPost);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.Headers["Authorization"] = basicPayment.getBearer;
            request.ContentType = "application/x-www-form-urlencoded";
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
                dataToSendWithPost = basicPayment.dataForPaymentBasicwithMerchantIdforTest;
            var url = setURLTest;
            byte[] buffer = Encoding.ASCII.GetBytes(dataToSendWithPost);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.Headers["Authorization"] = basicPayment.getBearer;
            request.ContentType = "application/x-www-form-urlencoded";
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

        public string getCheckoutId (Dictionary<string,dynamic> checkoutData) {
            var getResultCode = checkoutData["result"]["code"];
            if (!getResultCode.Equals("000.200.100"))
                throw new OnlinePaymentComunicationException(getResultCode,
                                                    checkoutData["result"]["description"]);
            return checkoutData["id"];
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
            if (brand == PaymentBrand.VISA || brand == PaymentBrand.MASTER || brand == PaymentBrand.MAESTRO
                || brand == PaymentBrand.MBWAY)
                return true;
            return false;
        }
    }
}