using OnlinePaymentsIntegration.SIBS.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace OnlinePaymentsIntegration
{
    public partial class Default : System.Web.UI.Page
    {
        public string TransactionId { get; set; }
        public string formContext { get; set; }
        public string formConfig { get; set; }
        public string formStyle { get; set; }
        public string Signature { get; set; }
        

        private string amount;

        private string urlToRedirect = HttpUtility.HtmlEncode("https://localhost:44314/Status.aspx");
        
        protected void Page_Load(object sender, EventArgs e) {
            TransactionId = TransactionDataForForm.transactionID;
            formContext = TransactionDataForForm.formContext;
            Signature = TransactionDataForForm.transactionSignature;
            amount = TransactionDataForForm.amount;
            formconfigText();
            formstyleText();
        }

        private string getBrandName(string brandChosen) {
            switch (brandChosen) {
                case "1":
                    return PaymentBrand.REFERENCE.ToString();
                case "2":
                    return PaymentBrand.MBWAY.ToString();
                default:
                    return PaymentBrand.CARD.ToString();
            }
        }

        private void formconfigText() {
            formConfig = HttpUtility.HtmlEncode("{\"paymentMethodList\": [],\n" +
                "\"amount\": { \"value\": " + amount +", \"currency\": \"EUR\"},\n" +
                "\"language\": \"pt\",\n" +
                "\"redirectUrl\": \"" + urlToRedirect+ "\",\n" +
                "\"customerData\": null }");
        }

        private void formstyleText() {
            formStyle = HttpUtility.HtmlEncode(@"{
   ""transaction"":{
      ""layout"":""default"",
      ""theme"":""default"",
      ""color"":{
                ""primary"":"""",
         ""secondary"":"""",
         ""border"":"""",
         ""surface"":"""",
         ""header"":{
                    ""text"":"""",
            ""background"":""""
         },
         ""body"":{
                    ""text"":"""",
            ""background"":""""
         }
            },
      ""font"":""""
   }
    }");
        }
    }
}