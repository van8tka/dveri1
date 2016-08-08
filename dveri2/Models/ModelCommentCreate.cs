using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DALdv1;
using System.ComponentModel.DataAnnotations;

namespace dveri2.Models
{
   
    public class ModelCommentCreate
    {
        public int Id { get; set; }
        public int IdDv { get; set; }
        [Display(Name="Имя*")]
        [Required(ErrorMessage = "Введите имя!")]
        public string Name { get; set; }
        [Display(Name = "Email адрес*")]
        [Required(ErrorMessage ="Введите email адрес!")]
        [RegularExpression(@"^[a-zA-Z0-9.-]{1,30}@[a-zA-Z0-9]{1,20}\.[A-Za-z]{2,6}", ErrorMessage = "Неверный формат email(xxxxxx@xxx.xx).")]
        public string Email { get; set; }
        [Display(Name = "Комментарий")]
        public string Comm { get; set; }
        public string Resp { get; set; }
        [Display(Name = "Заголовок к коментарию")]
        public string Head { get; set; }
        public bool Publish { get; set; }
        public int Stars { get; set; }
        public DateTime Date { get; set; }
    }

    public class ModelCommentMkDv
    {
        public IEnumerable<CommentMkDv> CommentMkDvList { get; set; }
    }
}