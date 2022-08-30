using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace OnlinePaymentsIntegration.SIBS.SDK
{
    public class ResponseRequestCopyAndPay
    {

        private Authentication authentication;
        private string clientId, bearer, readAllJson;
        private string transactionId;
        public string getReadAllJson { get { return readAllJson; } }
        private string requestGetURL;

        // Constructor given the entity, bearer and checkoutId
        public ResponseRequestCopyAndPay(string redirectURL, string clientId, string bearer, string transactionId) {
            authentication = new Authentication(clientId, bearer);
            this.clientId = authentication.getxIBMClientId;
            this.bearer = authentication.getBearer;
            this.transactionId = transactionId;
            this.requestGetURL = urlBaseToCompletePayment(redirectURL);
        }

        public string urlBaseToCompletePayment(string baseUrl) {
            return baseUrl + "/api/v1/payments/" + transactionId + "/status";
        }

        // get the response payment data
        public Dictionary<string, dynamic> getResponseRequest() {
            Dictionary<string, dynamic> responseData;

            string url = requestGetURL;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            request.Headers["Authorization"] = bearer;
            request.Headers["X-IBM-Client-Id"] = clientId;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                readAllJson = reader.ReadToEnd();
                var s = new JavaScriptSerializer();
                responseData = s.Deserialize<Dictionary<string, dynamic>>(readAllJson);
                reader.Close();
                dataStream.Close();
            }
            return responseData;
        }

        


        public string getResponseRequestCompleteText(Dictionary<string, dynamic> getResponse) {
            string response = "";
            foreach (KeyValuePair<string, dynamic> kvp in getResponse) {
                response += kvp;
            }
            return response;
        }




    }
}