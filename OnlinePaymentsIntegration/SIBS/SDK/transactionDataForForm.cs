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
        public static string clientId { get { return "56342"; } }
        public static string bearer { get { return "0276b80f950fb446c6addaccd121abfbbb.eyJlIjoiMTk3Njk1NjM3NjExNyIsInJvbGVzIjoiU1BHX01BTkFHRVIiLCJ0b2tlbkFwcERhdGEiOiJ7XCJtY1wiOlwiOTk5OTk5OVwiLFwidGNcIjpcIjU2MzQyXCJ9IiwiaSI6IjE2NjEzMzcxNzYxMTciLCJpcyI6Imh0dHBzOi8vcWx5LnNpdGUxLnNzby5zeXMuc2licy5wdC9hdXRoL3JlYWxtcy9RTFkuTUVSQ0guUE9SVDEiLCJ0eXAiOiJCZWFyZXIiLCJpZCI6IjRGQURXVGdjUWE1NjJlZTQ4ODdkOTA0MTg0YTUyNWQyYjFjYzBlNjAzYiJ9.6e531784385b9211a2dde32bd354bac64bf87e40cd32da95713c29e5e7e89a097e2b5f4044a4f5ee10f13b404f616c77922e775f03e7a89a3ac59bebf07d82"; } }
        public static string clientIdOnSibs { get { return "8b5aa2bc-53ef-4b55-adbb-929aac3ebdda"; } }
        public static string multibancoEntity { get { return "24000"; } }
        
    }
}