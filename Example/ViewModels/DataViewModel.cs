﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Example.Models;

namespace Example.ViewModels
{
    public class DataViewModel
    {
        public IEnumerable<URLShort> UrlData { get; set; }
    }
}
