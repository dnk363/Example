using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Models
{
    public class News
    {
        public int Id { get; set; }
        public string NewsHeading { get; set; }
        public string NewsText { get; set; }
        public DateTime NewsDate { get; set; }
    }
}
