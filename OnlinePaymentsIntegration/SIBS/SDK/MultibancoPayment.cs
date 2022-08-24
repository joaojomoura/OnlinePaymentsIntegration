using System;
using OnlinePaymentsIntegration.SIBS.SDK;

namespace OnlinePaymentsIntegration
{
    public class MultibancoPayment : BasicPayment
    {
        public string initialTransactionTime { get; set; }
        public  string endTransactionTime{ get; set; }

        private string multibancoEntity;
        private string country;

        public MultibancoPayment(string amount, string currency, string paymentType, string entityId, string bearer, string multibancoEntity) 
            : base(amount,currency,paymentType,entityId,bearer){
            this.multibancoEntity = multibancoEntity;
            country = "PT";
            initialTransactionTime = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffzzz");
            endTransactionTime = DateTime.Now.AddMinutes(10).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffzzz");
        }

        public MultibancoPayment(string amount, string currency, string paymentType, string entityId, string bearer, string merchantId, string multibancoEntity) : 
            base(amount, currency, paymentType, entityId, bearer, merchantId) {
            this.multibancoEntity = multibancoEntity;
            country = "PT";
            initialTransactionTime = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffzzz");
            endTransactionTime = DateTime.Now.AddMinutes(10).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffzzz");
            
        }

        public override string dataForPaymentBasic => base.dataForPaymentBasic + "&billing.country=" + country
            + "&customParameters[SIBSMULTIBANCO_PtmntEntty]="
            + multibancoEntity + "&customParameters[SIBSMULTIBANCO_RefIntlDtTm]=" + initialTransactionTime +
            "&customParameters[SIBSMULTIBANCO_RefLmtDtTm]=" + endTransactionTime;

        public override string dataForPaymentBasicforTest => base.dataForPaymentBasicforTest + "&billing.country=" + country
            + "&customParameters[SIBSMULTIBANCO_PtmntEntty]="
            + multibancoEntity + "&customParameters[SIBSMULTIBANCO_RefIntlDtTm]=" + initialTransactionTime +
            "&customParameters[SIBSMULTIBANCO_RefLmtDtTm]=" + endTransactionTime;

        public override string dataForPaymentBasicwithMerchantId => base.dataForPaymentBasicwithMerchantId + "&billing.country=" + country
            + "&customParameters[SIBSMULTIBANCO_PtmntEntty]="
            + multibancoEntity + "&customParameters[SIBSMULTIBANCO_RefIntlDtTm]=" + initialTransactionTime +
            "&customParameters[SIBSMULTIBANCO_RefLmtDtTm]=" + endTransactionTime;

        public override string dataForPaymentBasicwithMerchantIdforTest => base.dataForPaymentBasicwithMerchantIdforTest + "&billing.country=" + country
            + "&customParameters[SIBSMULTIBANCO_PtmntEntty]="
            + multibancoEntity + "&customParameters[SIBSMULTIBANCO_RefIntlDtTm]=" + initialTransactionTime +
            "&customParameters[SIBSMULTIBANCO_RefLmtDtTm]=" + endTransactionTime;



    }
}