using OnlinePaymentsIntegration.SIBS.Exceptions;
using OnlinePaymentsIntegration.SIBS.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlinePaymentsIntegration
{
    public partial class Status : System.Web.UI.Page
    {
        public string Result { get; set; }
        public string Pagamento { get; set; }
        private string checkoutId;
        private ResponseRequestCopyAndPay responseRequestCopyAndPay;
        private Dictionary<string, dynamic> getResponse;
        protected void Page_Load(object sender, EventArgs e) {
            /*checkoutId = Request.QueryString["id"];
            var URL = "https://spg.qly.site1.sibs.pt";
            var clientId = "8b5aa2bc-53ef-4b55-adbb-929aac3ebdda";
            var bearer = "0276b80f950fb446c6addaccd121abfbbb.eyJlIjoiMTk3Njk1NjM3NjExNyIsInJvbGVzIjoiU1BHX01BTkFHRVIiLCJ0b2tlbkFwcERhdGEiOiJ7XCJtY1wiOlwiOTk5OTk5OVwiLFwidGNcIjpcIjU2MzQyXCJ9IiwiaSI6IjE2NjEzMzcxNzYxMTciLCJpcyI6Imh0dHBzOi8vcWx5LnNpdGUxLnNzby5zeXMuc2licy5wdC9hdXRoL3JlYWxtcy9RTFkuTUVSQ0guUE9SVDEiLCJ0eXAiOiJCZWFyZXIiLCJpZCI6IjRGQURXVGdjUWE1NjJlZTQ4ODdkOTA0MTg0YTUyNWQyYjFjYzBlNjAzYiJ9.6e531784385b9211a2dde32bd354bac64bf87e40cd32da95713c29e5e7e89a097e2b5f4044a4f5ee10f13b404f616c77922e775f03e7a89a3ac59bebf07d82";
            responseRequestCopyAndPay = new ResponseRequestCopyAndPay(URL,clientId,bearer,checkoutId);
            var response = responseRequestCopyAndPay.getResponseRequest();
            Result = responseRequestCopyAndPay.getReadAllJson;*/
            RegisterAsyncTask(new PageAsyncTask(StartTest));
        }

        private async Task StartTest() {
            checkoutId = Request.QueryString["id"];
            var URL = "https://spg.qly.site1.sibs.pt";
            var clientId = "8b5aa2bc-53ef-4b55-adbb-929aac3ebdda";
            var bearer = "0276b80f950fb446c6addaccd121abfbbb.eyJlIjoiMTk3Njk1NjM3NjExNyIsInJvbGVzIjoiU1BHX01BTkFHRVIiLCJ0b2tlbkFwcERhdGEiOiJ7XCJtY1wiOlwiOTk5OTk5OVwiLFwidGNcIjpcIjU2MzQyXCJ9IiwiaSI6IjE2NjEzMzcxNzYxMTciLCJpcyI6Imh0dHBzOi8vcWx5LnNpdGUxLnNzby5zeXMuc2licy5wdC9hdXRoL3JlYWxtcy9RTFkuTUVSQ0guUE9SVDEiLCJ0eXAiOiJCZWFyZXIiLCJpZCI6IjRGQURXVGdjUWE1NjJlZTQ4ODdkOTA0MTg0YTUyNWQyYjFjYzBlNjAzYiJ9.6e531784385b9211a2dde32bd354bac64bf87e40cd32da95713c29e5e7e89a097e2b5f4044a4f5ee10f13b404f616c77922e775f03e7a89a3ac59bebf07d82";
            responseRequestCopyAndPay = new ResponseRequestCopyAndPay(URL, clientId, bearer, checkoutId);
            var response = responseRequestCopyAndPay.getResponseRequest();
            await Task.Delay(3000);
            if (response["paymentStatus"].Equals("Success")) {
                Pagamento = "Pagamento com sucesso";
                Result = responseRequestCopyAndPay.getReadAllJson;
            }
            else if (response["paymentStatus"].Equals("Declined")) {
                Pagamento = "Pagamento rejeitado";
                Result = responseRequestCopyAndPay.getReadAllJson;
            }
            else {

                Pagamento = "Pagamento Pending";
                if (await getPaymentStatusUntilSucess()) {
                    
                    Result = responseRequestCopyAndPay.getReadAllJson;
                }
            }
            
        }

        protected void Button1_Click(object sender, EventArgs e) {
            var URL = "https://spg.qly.site1.sibs.pt";
            var clientId = "8b5aa2bc-53ef-4b55-adbb-929aac3ebdda";
            var bearer = "0276b80f950fb446c6addaccd121abfbbb.eyJlIjoiMTk3Njk1NjM3NjExNyIsInJvbGVzIjoiU1BHX01BTkFHRVIiLCJ0b2tlbkFwcERhdGEiOiJ7XCJtY1wiOlwiOTk5OTk5OVwiLFwidGNcIjpcIjU2MzQyXCJ9IiwiaSI6IjE2NjEzMzcxNzYxMTciLCJpcyI6Imh0dHBzOi8vcWx5LnNpdGUxLnNzby5zeXMuc2licy5wdC9hdXRoL3JlYWxtcy9RTFkuTUVSQ0guUE9SVDEiLCJ0eXAiOiJCZWFyZXIiLCJpZCI6IjRGQURXVGdjUWE1NjJlZTQ4ODdkOTA0MTg0YTUyNWQyYjFjYzBlNjAzYiJ9.6e531784385b9211a2dde32bd354bac64bf87e40cd32da95713c29e5e7e89a097e2b5f4044a4f5ee10f13b404f616c77922e775f03e7a89a3ac59bebf07d82";
            responseRequestCopyAndPay = new ResponseRequestCopyAndPay(URL, clientId, bearer, checkoutId);
            var response = responseRequestCopyAndPay.getResponseRequest();
            Result = responseRequestCopyAndPay.getReadAllJson;
        }

        private async Task<bool> getPaymentStatusUntilSucess() {
            bool paymentStatusSucess = false;
            var URL = "https://spg.qly.site1.sibs.pt";
            var clientId = "8b5aa2bc-53ef-4b55-adbb-929aac3ebdda";
            var bearer = "0276b80f950fb446c6addaccd121abfbbb.eyJlIjoiMTk3Njk1NjM3NjExNyIsInJvbGVzIjoiU1BHX01BTkFHRVIiLCJ0b2tlbkFwcERhdGEiOiJ7XCJtY1wiOlwiOTk5OTk5OVwiLFwidGNcIjpcIjU2MzQyXCJ9IiwiaSI6IjE2NjEzMzcxNzYxMTciLCJpcyI6Imh0dHBzOi8vcWx5LnNpdGUxLnNzby5zeXMuc2licy5wdC9hdXRoL3JlYWxtcy9RTFkuTUVSQ0guUE9SVDEiLCJ0eXAiOiJCZWFyZXIiLCJpZCI6IjRGQURXVGdjUWE1NjJlZTQ4ODdkOTA0MTg0YTUyNWQyYjFjYzBlNjAzYiJ9.6e531784385b9211a2dde32bd354bac64bf87e40cd32da95713c29e5e7e89a097e2b5f4044a4f5ee10f13b404f616c77922e775f03e7a89a3ac59bebf07d82";
            responseRequestCopyAndPay = new ResponseRequestCopyAndPay(URL, clientId, bearer, checkoutId);
            while (!paymentStatusSucess) {
                var response = responseRequestCopyAndPay.getResponseRequest();
                if (response["paymentStatus"].Equals("Success")) {
                    Pagamento = "Pagamento com sucesso";
                    paymentStatusSucess = true;
                    getResponse = response;
                }
                else if (response["paymentStatus"].Equals("Declined")) {
                    Pagamento = "Pagamento rejeitado";
                    paymentStatusSucess = true;
                    getResponse = response;
                }
                else
                    await Task.Delay(10000);
                //Result = responseRequestCopyAndPay.getReadAllJson;
            }
            return paymentStatusSucess;
        }

        //private async Task<>
    }
}