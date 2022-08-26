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
        CustomerInfo customer;
        protected void Page_Load(object sender, EventArgs e) {
            CustomerInfo customer = new CustomerInfo("Joao", "Moura");
            customer.shippingAddress.street1 = "Rua 123";
            customer.shippingAddress.street2 = "porta 2";
            customer.shippingAddress.city = "Aveiro";
            customer.shippingAddress.postcode = "3800-208";
            customer.shippingAddress.country = "PT";

            customer.billingAddress.street1 = "Rua 123";
            customer.billingAddress.street2 = "porta 2";
            customer.billingAddress.city = "Aveiro";
            customer.billingAddress.postcode = "3800-208";
            customer.billingAddress.country = "PT";
            this.customer = customer;
        }

        protected void ShowJsonButton_Click(object sender, EventArgs e) {
            checkoutRequestCopyAndPay = new CheckoutRequestCopyAndPay(AmountTextBox.Text, "EUR", "8b5aa2bc-53ef-4b55-adbb-929aac3ebdda",
                "0276b80f950fb446c6addaccd121abfbbb.eyJlIjoiMTk3Njk1NjM3NjExNyIsInJvbGVzIjoiU1BHX01BTkFHRVIiLCJ0b2tlbkFwcERhdGEiOiJ7XCJtY1wiOlwiOTk5OTk5OVwiLFwidGNcIjpcIjU2MzQyXCJ9IiwiaSI6IjE2NjEzMzcxNzYxMTciLCJpcyI6Imh0dHBzOi8vcWx5LnNpdGUxLnNzby5zeXMuc2licy5wdC9hdXRoL3JlYWxtcy9RTFkuTUVSQ0guUE9SVDEiLCJ0eXAiOiJCZWFyZXIiLCJpZCI6IjRGQURXVGdjUWE1NjJlZTQ4ODdkOTA0MTg0YTUyNWQyYjFjYzBlNjAzYiJ9.6e531784385b9211a2dde32bd354bac64bf87e40cd32da95713c29e5e7e89a097e2b5f4044a4f5ee10f13b404f616c77922e775f03e7a89a3ac59bebf07d82",
                "56342", "24000","20220826",customer);
            
            var checkoutRequest = checkoutRequestCopyAndPay.getCheckoutRequestForTests();
            var transactionID = checkoutRequestCopyAndPay.getTransactionId(checkoutRequest);
            var signature = checkoutRequestCopyAndPay.getTransactionSignature(checkoutRequest);
            var formContext = checkoutRequestCopyAndPay.getFormContext(checkoutRequest);
            checkoutText = checkoutRequestCopyAndPay.getReadAllJson;
            CheckoutResult.Text = checkoutText;
            ViewState["id"] = transactionID;
            ViewState["siganture"] = signature;
            ViewState["context"] = formContext;
            ViewState["amount"] = AmountTextBox.Text;

        }

        protected void PaymentButton_Click(object sender, EventArgs e) {
            Response.Redirect("~/Default.aspx?id=" + ViewState["id"].ToString() + "&signature=" + ViewState["siganture"].ToString() + "&context=" + ViewState["context"].ToString() + 
                "&amount=" + ViewState["amount"].ToString());
        }
    }
}