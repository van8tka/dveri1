using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dveri1.Models
{
    public class ModelFind
    {
        [Required(ErrorMessage = "Введите код товара для поиска!")]
        [RegularExpression(@"^[0123456789]{1,5}", ErrorMessage = "Неверный формат ввода (допускает только цифры)!")]
        public int IndexFind { get; set; }
    }
}