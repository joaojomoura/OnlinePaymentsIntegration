using Ninject.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePaymentsIntegration.SIBS.SDK
{
    public class Authentication
    {
        private readonly string xIBMClient,bearer, merchantTransactionId, terminalId, multibancoEntity;

        //public Authentication(string userId, string password, string xIBMClient){
            
        //    this.userId = userId;
        //    this.xIBMClient = xIBMClient;
        //    this.password = password;
        //}

        
        public Authentication (string xIBMClient, string bearer, string terminalId, string multibancoEntity){
            this.xIBMClient = xIBMClient;
            this.bearer = "Bearer " + bearer;
            this.terminalId = terminalId;
            this.multibancoEntity = multibancoEntity;
        }

        public Authentication(string xIBMClient, string bearer, string terminalId, string multibancoEntity, string merchantId) {
            this.xIBMClient = xIBMClient;
            this.bearer = "Bearer " + bearer;
            this.merchantTransactionId = merchantId;
            this.terminalId = terminalId;
            this.multibancoEntity = multibancoEntity;
        }
        public Authentication(string xIBMClient) {
            this.xIBMClient = xIBMClient;
        }

        public string getxIBMClient {
            get { return xIBMClient; }
        }

        public string getBearer {
            get { return bearer; }
        }

        public string getmerchantTransactionId { get { return "Order: " + merchantTransactionId; } }

        public string getTerminalId { get { return terminalId; } }

        public string getmultibancoEntity { get { return multibancoEntity; } }


    }
}