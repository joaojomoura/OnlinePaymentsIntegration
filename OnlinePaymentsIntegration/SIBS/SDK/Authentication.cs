using Ninject.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePaymentsIntegration.SIBS.SDK
{
    public class Authentication
    {
        private readonly string xIBMClientId,bearer, merchantTransactionId, terminalId, multibancoEntity;

     
        public Authentication(string xIBMClientId, string bearer) {
            this.xIBMClientId = xIBMClientId;
            this.bearer = bearer;
        }
        
        public Authentication (string xIBMClientId, string bearer, string terminalId, string multibancoEntity){
            this.xIBMClientId = xIBMClientId;
            this.bearer = "Bearer " + bearer;
            this.terminalId = terminalId;
            this.multibancoEntity = multibancoEntity;
        }

        public Authentication(string xIBMClientId, string bearer, string terminalId, string multibancoEntity, string merchantTransactionId) {
            this.xIBMClientId = xIBMClientId;
            this.bearer = "Bearer " + bearer;
            this.merchantTransactionId = merchantTransactionId;
            this.terminalId = terminalId;
            this.multibancoEntity = multibancoEntity;
        }
      

        public string getxIBMClientId {
            get { return xIBMClientId; }
        }

        public string getBearer {
            get { return bearer; }
        }

        public string getmerchantTransactionId { get { return "Order: " + merchantTransactionId; } }

        public string getTerminalId { get { return terminalId; } }

        public string getmultibancoEntity { get { return multibancoEntity; } }


    }
}