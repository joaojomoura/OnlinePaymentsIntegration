using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OnlinePaymentsIntegration.SaveToRealBD
{
    public class getReferencias
    {
        private SqlConnection sqlcon;

        public string getRef(string transactionId) {
            try {
                sqlcon = new SqlConnection(@"Data Source=sql.inovanet.pt,3433;Initial Catalog=pizzarte_testes;User ID=pizzartenet;Password=qjaabsuf6969$;encrypt=true;trustServerCertificate=true");
                sqlcon.Open();
            }
            catch {
                sqlcon.Close();
            }

            string query = @"SELECT  MBREF_Reference FROM TRANSACTIONS WITH (NOLOCK) WHERE TransactionId = '"
                   + transactionId + "'";
            SqlDataReader sqlDR = null;
            string referencia = string.Empty;
            try {
                SqlCommand SqlExecute0 = new SqlCommand(query, sqlcon);
                sqlDR = SqlExecute0.ExecuteReader();
                if (sqlDR.Read()) {
                    referencia = sqlDR.GetString(sqlDR.GetOrdinal("MBREF_Reference"));
                }
                sqlDR.Close();
            }
            catch (SqlException ex) {
                if (sqlDR != null) {
                    if (sqlDR.IsClosed == false)
                        sqlDR.Close();
                }
                sqlcon.Close();
            }
            sqlcon.Close();
            return referencia;
        }
    }
}