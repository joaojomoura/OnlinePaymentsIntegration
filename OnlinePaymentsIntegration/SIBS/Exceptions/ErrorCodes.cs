using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace OnlinePaymentsIntegration.SIBS.Exceptions
{
    public class ErrorCodes
    {
        
        

        public ErrorCodes() {
            
        }

        public string getErrorCodeDescription(string errorCode) {

            if (Regex.IsMatch(errorCode, @"^(000\.000\.|000\.100\.1|000\.[36])"))
                return "SUCCESSFUL TRANSACTION";
            else if (Regex.IsMatch(errorCode, @"^(000\.400\.0[^3]|000\.400\.[0-1]{2}0)"))
                return "SUCESSFUL PROCESSED TRANSACTION FOR REVIEW";
            else if (Regex.IsMatch(errorCode, @"^(000\.200)|^(800\.400\.5|100\.400\.500)"))
                return "PENDING TRANSACTION";
            else if (Regex.IsMatch(errorCode, @"^(000\.400\.[1][0-9][1-9]|000\.400\.2)"))
                return "REJECTED TRANSACTION RISK CHECK";
            else if (Regex.IsMatch(errorCode, @"^(800\.[17]00|800\.800\.[123])"))
                return "REJECTED TRANSACTION EXTERNAL BANK";
            else if (Regex.IsMatch(errorCode, @"^(900\.[1234]00|000\.400\.030)"))
                return "REJECTED COMMUNICATION";
            else if (Regex.IsMatch(errorCode, @"^(800\.[56]|999\.|600\.1|800\.800\.[84])"))
                return "REJECTED SYSTEM ERROR";
            else if (Regex.IsMatch(errorCode, @"^(100\.39[765])"))
                return "REJECTED ASYNC FLOW";
            else if (Regex.IsMatch(errorCode, @"^(300\.100\.100)"))
                return "SOFT DECLINE";
            else if (Regex.IsMatch(errorCode, @"^(100\.400\.[0-3]|100\.38|100\.370\.100|100\.370\.11)"))
                return "REJECTED EXTERNAL RISK";
            else if (Regex.IsMatch(errorCode, @"^(800\.400\.1)"))
                return "REJECTED RISK ADDRESS VALIDATION";
            else if (Regex.IsMatch(errorCode, @"^(800\.400\.2|100\.380\.4|100\.390)"))
                return "REJECTED RISK 3DSECURE";
            else if (Regex.IsMatch(errorCode, @"^(100\.100\.701|800\.[32])"))
                return "REJECTED RISK BLACKLIST";
            else if (Regex.IsMatch(errorCode, @"^(800\.1[123456]0)"))
                return "REJECTED RISK VALIDATION";
            else if (Regex.IsMatch(errorCode, @"^(600\.[23]|500\.[12]|800\.121)"))
                return "REJECTED INVALID CONFIGURATION";
            else if (Regex.IsMatch(errorCode, @"^(100\.[13]50)"))
                return "REJECTED INVALID REGISTRATION";
            else if (Regex.IsMatch(errorCode, @"^(100\.250|100\.360)"))
                return "REJECTED INVALID JOB";
            else if (Regex.IsMatch(errorCode, @"^(700\.[1345][05]0)"))
                return "REJECTED_INVALID_REFERENCE";
            else if (Regex.IsMatch(errorCode, @"^(200\.[123]|100\.[53][07]|800\.900|100\.[69]00\.500)"))
                return "REJECTED INVALID FORMAT";
            else if (Regex.IsMatch(errorCode, @"^(100\.800)"))
                return "REJECTED INVALID ADDRESS";
            else if (Regex.IsMatch(errorCode, @"^(100\.[97]00)"))
                return "REJECTED INVALID CONTACT";
            else if (Regex.IsMatch(errorCode, @"^(100\.100|100.2[01])"))
                return "REJECTED INVALID ACCOUNT";
            else if (Regex.IsMatch(errorCode, @"^(100\.55)"))
                return "REJECTED INVALID AMOUNT";
            else if (Regex.IsMatch(errorCode, @"^(100\.380\.[23]|100\.380\.101)"))
                return "REJECTED RISK MANAGEMENT";
            else if (Regex.IsMatch(errorCode, @"^(000\.100\.2)"))
                return "CHARGEBACK RELATED";

            return null;
        }
    }
}