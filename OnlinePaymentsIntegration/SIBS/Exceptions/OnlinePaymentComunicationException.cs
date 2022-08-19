using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePaymentsIntegration.SIBS.Exceptions
{
    [Serializable]
    public class OnlinePaymentComunicationException : Exception
    {
        public OnlinePaymentComunicationException() {

        }
        public OnlinePaymentComunicationException(string errorCode, string errorDescription)
            : base(String.Format("Tente novamente mais tarde:\nCodigo{0}\nDescricao:{1}", errorCode,errorDescription)) {
            
        }
    }
}