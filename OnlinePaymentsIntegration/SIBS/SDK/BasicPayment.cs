using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePaymentsIntegration.SIBS.SDK
{
    public class BasicPayment
    {
        private string amount, currency;
        private PaymentType paymentType;
        private Authentication authentication;

        // Constructor of Basic Payment given only strings
        public BasicPayment(string amount, string currency, string paymentType, string entityId, string bearer) {
            this.amount = amount;
            this.currency = currency;
            this.paymentType = (PaymentType)Enum.Parse(typeof(PaymentType),paymentType);
            authentication = new Authentication(entityId, bearer);
        }
        
        // Constructor of Basic Payment given  merchantId string 
        public BasicPayment(string amount, string currency, string paymentType, string entityId, string bearer, string merchantId) {
            this.amount = amount;
            this.currency = currency;
            this.paymentType = (PaymentType)Enum.Parse(typeof(PaymentType), paymentType);
            authentication = new Authentication(entityId, bearer,merchantId);

        }


        // Return the amount
        public string getAmount { get { return amount; } }

        public string getBearer { get { return authentication.getBearer; } }

        // Return the string for checkout request
        public virtual string dataForPaymentBasic {
            get { return authentication.getEntityId + 
                    "&amount=" + amount +
                    "&currency=" + currency +
                    "&paymentType=" + paymentType.ToString(); }
        }

        // Return the string for checkout request with merchant Id
        public virtual string dataForPaymentBasicwithMerchantId {
            get {
                return authentication.getEntityId +
                  "&amount=" + amount +
                  "&currency=" + currency +
                  "&paymentType=" + paymentType.ToString() +
                  authentication.getmerchantId;
            }
        }

        public virtual string dataForPaymentBasicforTest {
            get {
                return authentication.getEntityId +
                  "&amount=" + amount +
                  "&currency=" + currency +
                  "&paymentType=" + paymentType.ToString() +
                  "&testMode=" + SibsEnvironmentMode.EXTERNAL.ToString()+
                  "&customParameters[SIBS_ENV]=QLY";
            }
        }

        public virtual string dataForPaymentBasicwithMerchantIdforTest {
            get {
                return authentication.getEntityId +
                  "&amount=" + amount +
                  "&currency=" + currency +
                  "&paymentType=" + paymentType.ToString() +
                  authentication.getmerchantId +
                  "&testMode=" + SibsEnvironmentMode.EXTERNAL.ToString() +
                  "&customParameters[SIBS_ENV]=QLY";
            }
        }

    }
}