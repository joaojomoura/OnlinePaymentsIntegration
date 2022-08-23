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
        public string CheckoutId { get; set; }
        public string paymentBrand { get; set; }
        protected void Page_Load(object sender, EventArgs e) {
            CheckoutId = Request.QueryString["id"];
            var getBrand = Request.QueryString["brand"];
            paymentBrand = getBrandName(getBrand);
        }

        private string getBrandName(string brandChosen) {
            switch (brandChosen) {
                case "1":
                    return PaymentBrand.SIBS_MULTIBANCO.ToString();
                case "2":
                    return PaymentBrand.MBWAY.ToString();
                default:
                    return PaymentBrand.VISA.ToString() + " " +
                        PaymentBrand.MASTER.ToString() + " " +
                        PaymentBrand.MAESTRO.ToString();
            }
        }
    }
}