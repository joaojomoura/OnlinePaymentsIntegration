﻿using System;
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
        public static string clientId { get; set; }
        public static string bearer { get; set; }
        public static string clientIdOnSibs { get; set; }
        public static string multibancoEntity { get; set; }
        
    }
}