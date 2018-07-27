using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JacksonShul.Models
{
    public class MonthlyPaymentWithName
    {
        public decimal Amount { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
    }
}