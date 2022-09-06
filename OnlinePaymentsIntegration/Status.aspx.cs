using OnlinePaymentsIntegration.SIBS.Exceptions;
using OnlinePaymentsIntegration.SIBS.SDK;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
        private SqlConnection sqlcon;
        protected void Page_Load(object sender, EventArgs e) {
          

           /* checkoutId = Request.QueryString["id"];
            var URL = "https://spg.qly.site1.sibs.pt";
            var clientId = "8b5aa2bc-53ef-4b55-adbb-929aac3ebdda";
            var bearer = "0276b80f950fb446c6addaccd121abfbbb.eyJlIjoiMTk3Njk1NjM3NjExNyIsInJvbGVzIjoiU1BHX01BTkFHRVIiLCJ0b2tlbkFwcERhdGEiOiJ7XCJtY1wiOlwiOTk5OTk5OVwiLFwidGNcIjpcIjU2MzQyXCJ9IiwiaSI6IjE2NjEzMzcxNzYxMTciLCJpcyI6Imh0dHBzOi8vcWx5LnNpdGUxLnNzby5zeXMuc2licy5wdC9hdXRoL3JlYWxtcy9RTFkuTUVSQ0guUE9SVDEiLCJ0eXAiOiJCZWFyZXIiLCJpZCI6IjRGQURXVGdjUWE1NjJlZTQ4ODdkOTA0MTg0YTUyNWQyYjFjYzBlNjAzYiJ9.6e531784385b9211a2dde32bd354bac64bf87e40cd32da95713c29e5e7e89a097e2b5f4044a4f5ee10f13b404f616c77922e775f03e7a89a3ac59bebf07d82";
            responseRequestCopyAndPay = new ResponseRequestCopyAndPay(URL, clientId, bearer, checkoutId);
            var response = responseRequestCopyAndPay.getResponseRequest();
            Result = responseRequestCopyAndPay.getReadAllJson;*/
            //RegisterAsyncTask(new PageAsyncTask(StartTest));
        }

        /*private async Task StartTest() {
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
            else if (response.ContainsKey("paymentReference")) {
                Pagamento = "Pagamento em espera";
                Result = responseRequestCopyAndPay.getReadAllJson;
            }
            else {

                Pagamento = "Pagamento Pending";
                if (await getPaymentStatusUntilSucess()) {
                    
                    Result = responseRequestCopyAndPay.getReadAllJson;
                }
            }
            
        }*/

        protected void Button1_Click(object sender, EventArgs e) {
            try {
                sqlcon = new SqlConnection(@"Data Source=sql.inovanet.pt,3433;Initial Catalog=pizzarte_testes;User ID=pizzartenet;Password=qjaabsuf6969$;encrypt=true;trustServerCertificate=true");
                sqlcon.Open();
            }
            catch {
                sqlcon.Close();
            }
            var URL = "https://spg.qly.site1.sibs.pt";
            var clientId = "8b5aa2bc-53ef-4b55-adbb-929aac3ebdda";
            var bearer = "0276b80f950fb446c6addaccd121abfbbb.eyJlIjoiMTk3Njk1NjM3NjExNyIsInJvbGVzIjoiU1BHX01BTkFHRVIiLCJ0b2tlbkFwcERhdGEiOiJ7XCJtY1wiOlwiOTk5OTk5OVwiLFwidGNcIjpcIjU2MzQyXCJ9IiwiaSI6IjE2NjEzMzcxNzYxMTciLCJpcyI6Imh0dHBzOi8vcWx5LnNpdGUxLnNzby5zeXMuc2licy5wdC9hdXRoL3JlYWxtcy9RTFkuTUVSQ0guUE9SVDEiLCJ0eXAiOiJCZWFyZXIiLCJpZCI6IjRGQURXVGdjUWE1NjJlZTQ4ODdkOTA0MTg0YTUyNWQyYjFjYzBlNjAzYiJ9.6e531784385b9211a2dde32bd354bac64bf87e40cd32da95713c29e5e7e89a097e2b5f4044a4f5ee10f13b404f616c77922e775f03e7a89a3ac59bebf07d82";
            responseRequestCopyAndPay = new ResponseRequestCopyAndPay(URL, clientId, bearer, TransactionDataForForm.transactionID);
            var response = responseRequestCopyAndPay.getResponseRequest();
            Result = responseRequestCopyAndPay.getReadAllJson;
            if (checkIfTransactionIsOnDataBase(response))
                SaveDataFromPost(response);
        }

        /*private async Task<bool> getPaymentStatusUntilSucess() {
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
        }*/

        //private async Task<>

      

        private bool checkIfTransactionIsOnDataBase(Dictionary<string, dynamic> checkoutData) {
            string query = @"SELECT ClientId, TransactionId FROM TRANSACTIONS WITH (NOLOCK) WHERE ClientId = '" + checkoutData["merchant"]["terminalId"] +
                "' AND TransactionId = '" + checkoutData["transactionID"] + "'";
            SqlDataReader sqlDR = null;
            try {
                SqlCommand SqlExecute0 = new SqlCommand(query, sqlcon);
                sqlDR = SqlExecute0.ExecuteReader();
                if (!sqlDR.Read()) {
                    sqlDR.Close();
                    return false;
                }
                sqlDR.Close();
            }
            catch (SqlException ex) {
                if (sqlDR != null) {
                    if (sqlDR.IsClosed == false)
                        sqlDR.Close();
                }
                //File.WriteAllText("caminho", "Select fail \n" + ex.Message);
                return false;
            }
            return true;
        }

        private void SaveDataFromPost(Dictionary<string, dynamic> checkoutData) {

            SqlTransaction transactionlocal = sqlcon.BeginTransaction();
            if (checkoutData.ContainsKey("paymentStatus")) {

                string updateQuery = "UPDATE TRANSACTIONS SET " +
                       "Payment_Type = '" + checkoutData["paymentType"] + "', " +
                       "Payment_Status = '" + checkoutData["paymentStatus"] + "', " +
                       "Payment_Method = '" + checkoutData["paymentMethod"] + "', " +
                       "Execution_Start_Time = '" + checkoutData["execution"]["startTime"] + "', " +
                       "Execution_End_Time = '" + checkoutData["execution"]["endTime"] + "' ";

                if (checkoutData.ContainsKey("token")){
                    updateQuery += ", Token_Type = '" + checkoutData["token"]["tokenType"] + "', " +
                        "Token_Value = '" + checkoutData["token"]["value"] + "' ";
                }
                if (checkoutData.ContainsKey("paymentReference")) {
                    updateQuery += ", MBREF_Entity = '" + checkoutData["paymentReference"]["entity"] + "', " +
                       "MBREF_Payment_Entity = '" +  checkoutData["paymentReference"]["paymentEntity"] + "', " +
                       "MBREF_Status = '" +  checkoutData["paymentReference"]["status"] + "', " +
                       "MBREF_ExpireDate = '" +  checkoutData["paymentReference"]["expireDate"] + "', " +
                       "MBREF_Reference = '" +  checkoutData["paymentReference"]["reference"] + "' ";
                }


                updateQuery += "WHERE (TransactionId = '" + checkoutData["transactionID"] + "' AND ClientId = '" + checkoutData["merchant"]["terminalId"] + "')";
                SqlDataReader sqlDR = null;
                try {
                    SqlCommand SqlExecute0 = new SqlCommand(updateQuery, sqlcon, transactionlocal);
                    //File.WriteAllText(@"D:\Logs\pizzarte_testes_Update3.txt", "Antes do execute \n");
                    sqlDR = SqlExecute0.ExecuteReader();
                    sqlDR.Close();
                    //File.WriteAllText(@"D:\Logs\pizzarte_testes_Update4.txt", "depoiis do execute \n");
                    if ((transactionlocal != null))
                        transactionlocal.Commit();
                    
                }
                catch (SqlException ex) {
                    //File.WriteAllText(@"D:\Logs\pizzarte_testes_Update.txt", "Update fail \n" + ex.Message);
                    if (sqlDR != null) {
                        if (sqlDR.IsClosed == false)
                            sqlDR.Close();
                    }
                    transactionlocal.Rollback();

                }
                catch (Exception ex) {
                    //File.WriteAllText(@"D:\Logs\pizzarte_testes_Update2.txt", "Update fail \n" + ex.Message);
                }
            }
        }

    }
}