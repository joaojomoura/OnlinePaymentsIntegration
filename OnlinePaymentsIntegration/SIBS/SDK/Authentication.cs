using Ninject.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePaymentsIntegration.SIBS.SDK
{
    public class Authentication
    {
        private readonly string userId, password, entityId, bearer, merchantTransactionId;

        //public Authentication(string userId, string password, string entityId){
            
        //    this.userId = userId;
        //    this.entityId = entityId;
        //    this.password = password;
        //}

        
        public Authentication (string entityId, string bearer){
            this.entityId = entityId;
            this.bearer = "Bearer " + bearer;
        }

        public Authentication(string entityId, string bearer, string merchantId) {
            this.entityId = entityId;
            this.bearer = "Bearer " + bearer;
            this.merchantTransactionId = merchantId;
        }
        public Authentication(string entityId) {
            this.entityId = entityId;
        }

        public string getEntityId {
            get { return "entityId=" + entityId; }
        }

        public string getBearer {
            get { return bearer; }
        }

        public string getmerchantId { get { return "&merchantTransactionId=" + merchantTransactionId; } }


    }
}