using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JacksonShul.Data;
using JacksonShul.Models;

namespace JacksonShul.Models
{
    public class PaymentsAndPledges
    {
        public IEnumerable<PaymentWithName> Payments { get; set; }
        public IEnumerable<PledgeWithName> Pledges { get; set; }
        public Member Member { get; set; }
    }
}