using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OnlinePaymentsIntegration.SIBS.SDK
{
    public class AfterPaymentUtils
    {
        private SqlConnection sqlcon;
        private string transactionId = TransactionDataForForm.transactionID;
        private string merchantTransactionId;
        private string clientId = TransactionDataForForm.clientId;


        /// <summary>
        /// Method to do a first check on BD for the transaction Payment Status. Ideal for a Page Load
        /// </summary>
        /// <returns>
        /// 0 - Success
        /// -1 - Pending
        /// -2 - Declined
        /// -3 - Sql error
        /// </returns>
        //public int firstTimeCheckBDIfTransactionWasMadeSuccessfully() {

        //    try {
        //        sqlcon = new SqlConnection("Data Source=sql.inovanet.pt,3433;Initial Catalog=pizzarte_testes;User ID=pizzartenet;Password=qjaabsuf6969$;encrypt=true;trustServerCertificate=true");
        //        sqlcon.Open();
        //    }
        //    catch {
        //        sqlcon.Close();
        //    }

        //    string queryOnPageLoad = "SELECT Payment_Status, Processed_Transaction FROM TRANSACTIONS" +
        //        " WITH (NOLOCK) WHERE ClientId = '" + clientId + "' AND " +
        //        "TransactionId = '" + transactionId + "' AND " +
        //        "Merchant_TransactionID = '" + merchantTransactionId + "' AND " +
        //        "DoNotRefresh = 0";
        //    SqlDataReader sqlDR = null;
        //    string paymentStatus = "";
        //    bool processed_Transaction = false;
        //    try {
        //        // verifica se existe na BD a transacao
        //        sqlDR = Data.Consulta(sqlcon, queryOnPageLoad, false, null);
        //        if (sqlDR.Read()) {
        //            paymentStatus = sqlDR.GetString(sqlDR.GetOrdinal("Payment_Status")).ToUpper();
        //            processed_Transaction = sqlDR.GetBoolean(sqlDR.GetOrdinal("Processed_Transaction"));
        //        }
        //        sqlDR.Close();
        //        SqlTransaction transactionlocal = null;
        //        transactionlocal = sqlcon.BeginTransaction();
        //        string updateBD = "";
        //        // Declined
        //        if (paymentStatus.Equals(PaymentStatus.DECLINED.ToString())) {

        //        }
        //    }
        //    catch {
        //        if (sqlDR != null) {
        //            if (sqlDR.IsClosed == false)
        //                sqlDR.Close();
        //        }
        //    }
        //    return 0;
        //}
        
    }
}