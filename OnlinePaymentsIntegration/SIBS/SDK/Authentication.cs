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

        /// <summary>
        /// ClientId = xIBMClientId
        /// Just need to insert ClientId Header and Bearer Autorization Header
        /// </summary>
        /// <param name="xIBMClientId"></param>
        /// <param name="bearer"></param>
        public Authentication(string xIBMClientId, string bearer) {
            this.xIBMClientId = xIBMClientId;
            this.bearer = "Bearer " + bearer;
        }
        /// <summary>
        /// ClientId = xIBMClientId
        /// Just need to insert ClientId Header, Bearer Autorization Header. terminalId and multibancoEntity for JSON body
        /// This Constructor is for transactions that don't need MerchantTransactionID
        /// </summary>
        /// <param name="xIBMClientId"></param>
        /// <param name="bearer"></param>
        /// <param name="terminalId"></param>
        /// <param name="multibancoEntity"></param>
        public Authentication (string xIBMClientId, string bearer, string terminalId, string multibancoEntity){
            this.xIBMClientId = xIBMClientId;
            this.bearer = "Bearer " + bearer;
            this.terminalId = terminalId;
            this.multibancoEntity = multibancoEntity;
        }
        /// <summary>
        /// ClientId = xIBMClientId
        /// Just need to insert ClientId Header, Bearer Autorization Header. terminalId, multibancoEntity and merchantTransactionId for JSON body
        /// </summary>
        /// <param name="xIBMClientId"></param>
        /// <param name="bearer"></param>
        /// <param name="terminalId"></param>
        /// <param name="multibancoEntity"></param>
        /// <param name="merchantTransactionId"></param>
        public Authentication(string xIBMClientId, string bearer, string terminalId, string multibancoEntity, string merchantTransactionId) {
            this.xIBMClientId = xIBMClientId;
            this.bearer = "Bearer " + bearer;
            this.merchantTransactionId = merchantTransactionId;
            this.terminalId = terminalId;
            this.multibancoEntity = multibancoEntity;
        }
      
        /// <summary>
        /// returns ClientId
        /// </summary>
        public string getxIBMClientId {
            get { return xIBMClientId; }
        }
        /// <summary>
        /// return Bearer
        /// </summary>
        public string getBearer {
            get { return bearer; }
        }
        /// <summary>
        /// return merchant Transaction ID
        /// </summary>
        public string getmerchantTransactionId { get { return "Order: " + merchantTransactionId; } }
        /// <summary>
        /// return TerminalId
        /// </summary>
        public string getTerminalId { get { return terminalId; } }
        /// <summary>
        /// return multibanco Entity
        /// </summary>
        public string getmultibancoEntity { get { return multibancoEntity; } }


    }
}