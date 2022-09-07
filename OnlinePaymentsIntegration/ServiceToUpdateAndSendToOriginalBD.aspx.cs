using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OnlinePaymentsIntegration.SaveToRealBD;

namespace OnlinePaymentsIntegration
{
    public partial class ServiceToUpdateAndSendToOriginalBD : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) {
            SaveToRealBDMesa save = new SaveToRealBDMesa();
            save.checkTimeOfPendingTransaction();
            save.forEachTransactionNotProcessed();
              
        }
    }
}