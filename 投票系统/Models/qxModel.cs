using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 投票系统.Models
{
    public class qxModel
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int addReview { get; set; }
        public int Review { get; set; }
        public int Administrator { get; set; }
        public int Technical { get; set; }
        public int TJ { get; set; }
        public int ZG { get; set; }
    }
}