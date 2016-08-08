using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DALdv1;
using System.ComponentModel.DataAnnotations;

namespace dveri2.Models
{
    public class ContactModel
    {
        public TableYrInfa YrInformationProp { get; set; }
        public IEnumerable<Adresa> AdresList { get; set; }
        public IEnumerable<Contact> ContactList { get; set; }
        public IEnumerable<GrafikWork> GrafikWorkList { get; set; }
        public string SeoKey { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDesc { get; set; }
        public string SeoHead { get; set; }
    }
   
}