using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace dveri1.Models
{
    public class ModelUser
    {
        [Required(ErrorMessage = "Введите имя пользователя!")]
        [Display(Name = "Имя пользователя")]
        public string NameUser{ get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Введите пароль!")]
        [Display(Name = "Пароль")]
        public string PasswordUser { get; set; }
    }
}