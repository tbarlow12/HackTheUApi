using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackTheUApi.Models
{
    public class Hardware
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Owner { get; set; }
    }
}