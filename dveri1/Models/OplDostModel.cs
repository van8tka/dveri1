using System;
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
        public Dostavka DostavkaProp { get; set; }
        public Oplata OplataProp { get; set; }
    }
}