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
        private string entityId, bearer, readAllJson;
        private string checkoutId;
        public string getReadAllJson { get { return readAllJson; } }
        public string setBaseURLLive { get; set; }
        public string setBaseURLTest { get; set; }
        
        // Constructor given the entity, bearer and checkoutId
        public ResponseRequestCopyAndPay(string entityId,string bearer, string checkoutId) {
            authentication = new Authentication(entityId,bearer);
            this.entityId = authentication.getEntityId;
            this.bearer = authentication.getBearer;
            this.checkoutId = checkoutId;
        }

        public string urlBaseToCompletePayment(string baseUrl) {
            return baseUrl + "/v1/checkouts/" + checkoutId + "/payment?";
        }

        // get the response payment data for live production
        public Dictionary <string, dynamic> getResponseRequest () {
            Dictionary<string, dynamic> responseData;
            
            string url = urlBaseToCompletePayment(setBaseURLLive) + entityId;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            request.Headers["Authorization"] = bearer;
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

        // get the response payment data for tests
        public Dictionary<string, dynamic> getResponseRequestForTest() {
            Dictionary<string, dynamic> responseData;

            string url = urlBaseToCompletePayment(setBaseURLTest) + entityId;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            request.Headers["Authorization"] = bearer;
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
            foreach(KeyValuePair<string,dynamic> kvp in getResponse) {
                response += kvp;
            }
            return response;
        }

        


    }
}