using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePaymentsIntegration.SIBS.SDK
{
    public class CustomerInfo
    {
        private string clientName, clientEmail;
        
        public Address shippingAddress = new Address(), billingAddress = new Address();
        /// <summary>
        /// Constructor for the customer info. 
        /// 
        /// Use shippingAddress and billingAddress to change 
        /// the values of each address
        /// </summary>
        /// <param name="clientName"></param>
        /// <param name="clientEmail"></param>
        public CustomerInfo(string clientName, string clientEmail) {
            this.clientName = clientName;
            this.clientEmail = clientEmail;
            
        }

        public string getClientName { get { return clientName; } }
        public string getClientEmail { get { return clientEmail; } }

        
        
    }

    public class Address
    {
       
        public string street1 { get; set; }
        public string street2 { get; set; }
        public string city { get; set; }
        public string postcode { get; set; }
        public string country { get; set; }
        

    }
}