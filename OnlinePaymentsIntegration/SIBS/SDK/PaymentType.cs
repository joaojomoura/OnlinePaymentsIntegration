using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePaymentsIntegration.SIBS.SDK
{
    /// <summary>
    /// Enum for Payment Types. With the new Digital Payment Gateway of SIBS,
    /// only use the PURS
    /// </summary>
    public enum PaymentType{
        PA,DB,CD,CP,RV,RF,RC,PURS
    }
}