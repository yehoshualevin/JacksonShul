using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JacksonShul.Data;

namespace JacksonShul.Models
{
    public class ExpensePlus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Cost { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalDonations { get; set; }
    }
}