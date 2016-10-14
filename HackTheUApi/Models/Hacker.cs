using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackTheUApi.Models
{
    public class Hacker
    {
        public int Id { get; set; }
        public string First { get; set; }
        public int Last { get; set; }
        public string Email { get; set; }
        public bool CheckedIn { get; set; }
        public bool Present { get; set; }

    }
}