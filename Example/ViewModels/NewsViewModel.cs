using Example.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.ViewModels
{
    public class NewsViewModel
    {
        public IEnumerable<News> NewsData { get; set; }
    }
}
