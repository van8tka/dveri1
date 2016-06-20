using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DALdv1;
using System.ComponentModel.DataAnnotations;

namespace dveri1.Models
{
    public class ContactModel
    {
        public IEnumerable<Adresa> AdresList { get; set; }
        public IEnumerable<Contact> ContactList { get; set; }
        public IEnumerable<GrafikWork> GrafikWorkList { get; set; }
        public string SeoKey { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDesc { get; set; }
        public string SeoHead { get; set; }
    }
    public class CreateContactModel
    {
        public int? IDContact { get; set; }
        [Required(ErrorMessage = "Введите данные, тип контакта н.р. МТС, Life, Skype, e-mail и т.д.")]
        public string TypContact { get; set; }
        [Required(ErrorMessage = "Введите данные, номер контакта или адрес контакта.")]
        public string NameContact { get; set; }     
    }
    public class CreateGrafikModel
    {
        public int? IDGrafik { get; set; }
        [Required(ErrorMessage = "Введите день, или период дней.")]
        public string Day { get; set; }
        [Required(ErrorMessage = "Введите период времени работы")]
        public string Time { get; set; }
    }
    public class CreateAdresModel
    {
        public int? IDAdres { get; set; }
        [Required(ErrorMessage = "Введите данные адрес.")]
        public string AdresName { get; set; }
    }
}