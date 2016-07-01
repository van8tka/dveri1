using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DALdv1;
using System.ComponentModel.DataAnnotations;

namespace dveri1.Models
{
    public class ModelComment
    {
        public string SeoKey { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDesc { get; set; }
        public string SeoHead { get; set; }
        public IEnumerable<CommentCompany> CommentCompList { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
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
    public class ModelCommentVhDv{
        public IEnumerable<CommentVhDveri> CommentVhDvList { get; set; }
    }
}