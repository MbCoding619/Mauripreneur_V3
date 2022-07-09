using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public string PaymentIntentId { get; set; }

        public int Amount { get; set; }

        public string PaymentStatus { get; set; }

        public string ClientSecret { get; set; }

        public string nameOnCard { get; set; }

        public int BidId { get; set; }

        public Bid Bid { get; set; }
    }
}