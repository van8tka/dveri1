﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DALdv1;
using System.Web.Mvc;

namespace dveri1.Models
{
    public class OplDostModel
    {
        //[AllowHtml]
        public string DostInfo { get; set; }
        public string OplInfo { get; set; }
        public string SeoKey { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDesc { get; set; }
        public string SeoHead { get; set; }

    }
}