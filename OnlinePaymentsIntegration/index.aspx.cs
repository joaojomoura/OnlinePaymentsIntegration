using OnlinePaymentsIntegration.SaveToBD;
using OnlinePaymentsIntegration.SIBS.SDK;
using System;
using System.Collections.Generic;
using System.IO;
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
        private string url = "https://spg.qly.site1.sibs.pt/api/v1/payments";
        protected void Page_Load(object sender, EventArgs e) {
            CustomerInfo customer = new CustomerInfo("software", "joao.moura@inovasis.pt");
            customer.shippingAddress.street1 = "rua";
            customer.shippingAddress.street2 = "rua";
            customer.shippingAddress.city = "aveiro";
            customer.shippingAddress.postcode = "3440";
            customer.shippingAddress.country = "PT";

            customer.billingAddress.street1 = "rua";
            customer.billingAddress.street2 = "rua";
            customer.billingAddress.city = "aveiro";
            customer.billingAddress.postcode = "3440";
            customer.billingAddress.country = "PT";
            this.customer = customer;


        }

        protected void ShowJsonButton_Click(object sender, EventArgs e) {
            checkoutRequestCopyAndPay = new CheckoutRequestCopyAndPay(url,AmountTextBox.Text, "EUR", TransactionDataForForm.clientIdOnSibs,
               TransactionDataForForm.bearer,TransactionDataForForm.clientId, TransactionDataForForm.multibancoEntity,"20220826",customer);
            
            var checkoutRequest = checkoutRequestCopyAndPay.getCheckoutRequest();
            var transactionID = checkoutRequestCopyAndPay.getTransactionId(checkoutRequest);
            var signature = checkoutRequestCopyAndPay.getTransactionSignature(checkoutRequest);
            var formContext = checkoutRequestCopyAndPay.getFormContext(checkoutRequest);
            checkoutText = checkoutRequestCopyAndPay.getReadAllJson;
            CheckoutResult.Text = checkoutText;
            TransactionDataForForm.transactionID = transactionID;
            TransactionDataForForm.transactionSignature = signature;
            TransactionDataForForm.formContext = formContext;
            TransactionDataForForm.amount = AmountTextBox.Text;

            //Saves to table Transaction
            SaveForInitialTransaction saveToTransaction = new SaveForInitialTransaction(1, "20220826","coiso",1);
            saveToTransaction.saveToTransactionsBD(DateTime.Now.ToString(),"Pao Pao Pao Pao");
            saveToTransaction.saveToTempBDTest();

        }

        protected void PaymentButton_Click(object sender, EventArgs e) {
            Response.Redirect("https://localhost:44314/Default.aspx");
        }
    }
}