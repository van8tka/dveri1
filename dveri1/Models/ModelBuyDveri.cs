using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dveri1.Models
{
    public class ModelBuyDveri
    {
        public int IdDveri { get; set; }
        [Required(ErrorMessage = "Введите имя!")]
        [RegularExpression(@"^[а-яА-Я ]{1,50}", ErrorMessage = "Неверный формат имени!")]
        public string KlientName { get; set; }
        [Required(ErrorMessage = "Введите номер телефона!")]
        [RegularExpression(@"^[0-9-+)( ]{1,20}", ErrorMessage = "Неверный формат ввода номера телефона!")]
        public string KlientPhone { get; set; }
        public string KlientAdres { get; set; }
        public string KlientQuestion { get; set; }
        public string Type { get; set; }
        public string DvName { get; set; }
    }
}