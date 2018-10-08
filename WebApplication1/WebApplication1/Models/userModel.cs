using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class userModel
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string group { get; set; }
        public string state { get; set; }
        public string department { get; set; }
        public string sex { get; set; }
    }
}