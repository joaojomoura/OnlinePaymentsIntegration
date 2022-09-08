using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OnlinePaymentsIntegration.RedirectedFunctions
{
    public class FunctionsAfterPayment
    {
        
        public bool checkIfTransactionIsOnDataBase(Dictionary<string, dynamic> checkoutData, SqlConnection sqlcon) {
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

        public void SaveDataFromGet(Dictionary<string, dynamic> checkoutData, SqlConnection sqlcon) {

            SqlTransaction transactionlocal = sqlcon.BeginTransaction();
            if (checkoutData.ContainsKey("paymentStatus")) {

                string updateQuery = "UPDATE TRANSACTIONS SET " +
                       "Payment_Type = '" + checkoutData["paymentType"] + "', " +
                       "Payment_Status = '" + checkoutData["paymentStatus"] + "', " +
                       "Payment_Method = '" + checkoutData["paymentMethod"] + "', " +
                       "Execution_Start_Time = '" + checkoutData["execution"]["startTime"] + "', " +
                       "Execution_End_Time = '" + checkoutData["execution"]["endTime"] + "' ";

                if (checkoutData.ContainsKey("token")) {
                    updateQuery += ", Token_Type = '" + checkoutData["token"]["tokenType"] + "', " +
                        "Token_Value = '" + checkoutData["token"]["value"] + "' ";
                }
                if (checkoutData.ContainsKey("paymentReference")) {
                    updateQuery += ", MBREF_Entity = '" + checkoutData["paymentReference"]["entity"] + "', " +
                       "MBREF_Payment_Entity = '" + checkoutData["paymentReference"]["paymentEntity"] + "', " +
                       "MBREF_Status = '" + checkoutData["paymentReference"]["status"] + "', " +
                       "MBREF_ExpireDate = '" + checkoutData["paymentReference"]["expireDate"] + "', " +
                       "MBREF_Reference = '" + checkoutData["paymentReference"]["reference"] + "' ";
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