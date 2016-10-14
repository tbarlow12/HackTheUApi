using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackTheUApi.Models
{
    public class Rental
    {
        public Hardware Hardware { get; set; }
        public Hacker Hacker { get; set; }
        public DateTime CheckOut { get; set; }
        public DateTime CheckIn { get; set; }
    }
}