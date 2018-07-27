using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JacksonShul.Data;

namespace JacksonShul.Models
{
    public class HomePageViewModel
    {
        public IEnumerable<Message> Messages { get; set; }
        public string Notify { get; set; }
        public List<string> Notifies { get; set; }
    }
}