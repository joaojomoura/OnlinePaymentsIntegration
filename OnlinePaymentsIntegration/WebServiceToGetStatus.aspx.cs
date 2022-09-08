using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlinePaymentsIntegration
{
    public partial class WebServiceToGetStatus : System.Web.UI.Page
    {
        public string Send { get; set; }
        private SqlConnection sqlcon;
        protected void Page_Load(object sender, EventArgs e) {
            try {
                sqlcon = new SqlConnection(@"Data Source=sql.inovanet.pt,3433;Initial Catalog=pizzarte_testes;User ID=pizzartenet;Password=qjaabsuf6969$;encrypt=true;trustServerCertificate=true");
                sqlcon.Open();
            }
            catch {
                sqlcon.Close();
            }


            var re = Request;
            var body = Request.GetBufferedInputStream();
            StreamReader reader = new StreamReader(body);
            var getBody = reader.ReadToEnd();



            string query = @"SELECT  Payment_Status FROM TRANSACTIONS WITH (NOLOCK) WHERE TransactionId = '"
                   + getBody + "'";
            SqlDataReader sqlDR = null;

            try {
                SqlCommand SqlExecute0 = new SqlCommand(query, sqlcon);
                sqlDR = SqlExecute0.ExecuteReader();
                if (sqlDR.Read()) {

                    if (sqlDR.GetString(sqlDR.GetOrdinal("Payment_Status")).Equals("Success"))
                        Send = "YES";
                    else if (sqlDR.GetString(sqlDR.GetOrdinal("Payment_Status")).Equals("Declined"))
                        Send = "NO";
                    //Send = sqlDR.GetString(sqlDR.GetOrdinal("Payment_Status"));
                }
                sqlDR.Close();
            }
            catch (SqlException ex) {
                if (sqlDR != null) {
                    if (sqlDR.IsClosed == false)
                        sqlDR.Close();
                }
            }
            sqlcon.Close();
        }
    }
}