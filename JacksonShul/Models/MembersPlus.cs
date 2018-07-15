using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JacksonShul.Data;

namespace JacksonShul.Models
{
    public class MembersPlus
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cell { get; set; }
        public string Email { get; set; }
        public IEnumerable<PaymentWithName> Payments { get; set; }
        public IEnumerable<Pledge> Pledges { get; set; }
    }
}