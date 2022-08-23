using OnlinePaymentsIntegration.SIBS.Exceptions;
using OnlinePaymentsIntegration.SIBS.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlinePaymentsIntegration
{
    public partial class Status : System.Web.UI.Page
    {
        public string Result { get; set; }
        private ResponseRequestCopyAndPay responseRequestCopyAndPay;
        protected void Page_Load(object sender, EventArgs e) {
            var checkoutId = Request.QueryString["id"];
            responseRequestCopyAndPay = new ResponseRequestCopyAndPay("8ac7a4c87e899ee3017e8c789a9f088b", "OGE4Mjk0MTc2MTUxNjEwODAxNjE2YzI0M2RkNjNlNzF8NnA1Y2Z0eXBzUQ==",checkoutId);
            responseRequestCopyAndPay.setBaseURLTest = "https://test.oppwa.com";
            var responseData = responseRequestCopyAndPay.getResponseRequestForTest();
            var errorInitialText = new ErrorCodes();
            Result = errorInitialText.getErrorCodeDescription(responseData["result"]["code"]);
            Result += "\n" + responseRequestCopyAndPay.getReadAllJson;
        }
    }
}