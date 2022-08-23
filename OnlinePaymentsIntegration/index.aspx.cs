using OnlinePaymentsIntegration.SIBS.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlinePaymentsIntegration
{
    public partial class index : System.Web.UI.Page
    {
        private CheckoutRequestCopyAndPay checkoutRequestCopyAndPay;
        private string checkoutText;

        protected void Page_Load(object sender, EventArgs e) {
            
        }

        protected void ShowJsonButton_Click(object sender, EventArgs e) {
            checkoutRequestCopyAndPay = new CheckoutRequestCopyAndPay(AmountTextBox.Text,"EUR","PA", "8ac7a4c87e899ee3017e8c789a9f088b", "OGE4Mjk0MTc2MTUxNjEwODAxNjE2YzI0M2RkNjNlNzF8NnA1Y2Z0eXBzUQ==");
            checkoutRequestCopyAndPay.setURLTest = "https://test.oppwa.com/v1/checkouts";
            var checkoutRequest = checkoutRequestCopyAndPay.getCheckoutRequestForTests();
            var checkoutID = checkoutRequestCopyAndPay.getCheckoutId(checkoutRequest);
            checkoutText = checkoutRequestCopyAndPay.getCheckoutRequestComplete(checkoutRequest);
            CheckoutResult.Text = checkoutText;
            ViewState["id"] = checkoutID;
        }

        protected void PaymentButton_Click(object sender, EventArgs e) {
            Response.Redirect("~/Default.aspx?id=" + ViewState["id"].ToString() + "&brand=" + paymentTypeChosen.Value.ToString());
        }
    }
}