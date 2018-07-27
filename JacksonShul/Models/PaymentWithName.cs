using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JacksonShul.Models
{
    public class PaymentWithName
    { 
       public DateTime Date { get; set; }
       public decimal Amount { get; set; } 
       public string Name { get; set; }
    }
}