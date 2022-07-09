using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class PaymentBidDTO
    {
        public int BidId { get; set; }

        public string nameOnCard { get; set; }

        public int BidAmount { get; set; }
    }
}