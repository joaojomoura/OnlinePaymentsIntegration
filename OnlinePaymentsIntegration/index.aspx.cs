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
            var getPaymentBrand = PaymentBrand.MAESTRO;
            var getPaymentType = PaymentType.DB;
            var isMultibanco = true;

            // it depends on payment method chosen
            switch (paymentTypeChosen.Value.ToString()) {
                case "1":
                    getPaymentBrand = PaymentBrand.SIBS_MULTIBANCO;
                    getPaymentType = PaymentType.PA;
                    break;
                case "2":
                    getPaymentBrand = PaymentBrand.MBWAY;
                    getPaymentType = PaymentType.DB;
                    isMultibanco = false;
                    break;
                default:
                    getPaymentBrand = PaymentBrand.VISA;
                    getPaymentType = PaymentType.DB;
                    isMultibanco = false;
                    break;
            }


            checkoutRequestCopyAndPay = new CheckoutRequestCopyAndPay(AmountTextBox.Text,"EUR",getPaymentType.ToString(), "8ac7a4c87e899ee3017e8c789a9f088b", "OGE4Mjk0MTc2MTUxNjEwODAxNjE2YzI0M2RkNjNlNzF8NnA1Y2Z0eXBzUQ==",getPaymentBrand);
            
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