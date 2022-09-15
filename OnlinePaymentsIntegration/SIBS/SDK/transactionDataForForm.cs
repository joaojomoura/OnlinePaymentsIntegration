using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePaymentsIntegration.SIBS.SDK
{
    public static class TransactionDataForForm
    {
        public static string transactionID { get; set; }
        public static string transactionSignature { get; set; }
        public static string formContext { get; set; }
        public static string amount { get; set; }
        public static string clientId { get { return ""; } } //insert terminalId here
        public static string bearer { get { return ""; } } //insert bearer here
        public static string clientIdOnSibs { get { return ""; } } // insert client id from sibs here
        public static string multibancoEntity { get { return ""; } } // insert multibanco entity here
        
    }
}