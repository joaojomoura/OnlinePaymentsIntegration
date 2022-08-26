using OnlinePaymentsIntegration.SIBS.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlinePaymentsIntegration
{
    public partial class Default : System.Web.UI.Page
    {
        public string TransactionId { get; set; }
        public string FormContext { get; set; }
        public string FormConfig { get; set; }
        public string FormStyle { get; set; }
        public string Signature { get; set; }
        

        private string amount;

        private string urlToRedirect = "http://localhost:50893/Status.aspx";

        protected void Page_Load(object sender, EventArgs e) {
            TransactionId = Request.QueryString["id"];
            FormContext = Request.QueryString["context"];
            Signature = Request.QueryString["signature"];
            amount = Request.QueryString["amount"];
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
            FormConfig = "{\"paymentMethodList\": [],\n" +
                "\"amount\": { \"value\": " + amount +", \"currency: \"EUR\"\"},\n" +
                "\"language\": \"pt\",\n" +
                "\"redirectUrl\": \"" + urlToRedirect + "\",\n" +
                "\"customerData\": null }";
        }

        private void formstyleText() {
            FormStyle = @"{ ""transaction"": {
                            {
                                ""layout"": 'default',
                            ""theme"": 'default',
                            ""color"": {
                                 ""primary"": """",
                            ""secondary"": """",
                            ""border"": """",
                            ""surface"": """",
                            ""header"": {
                                ""text"": """",
                            ""background"": """"
                            },
                            ""body"": {
                                ""text"": """",
                            ""background"": """"
                            }
                              },
                            ""font"": """"
                            } ";
        }
    }
}