using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DALdv1;
using System.ComponentModel.DataAnnotations;

namespace dveri1.Models
{
    public class ModelSeoMain
    {
        public int Id { get; set; }
        [Display(Name = "<title>")]
        public string SeoTitle { get; set; }
        [Display(Name = "<keywords>")]
        public string SeoKey { get; set; }
        [Display(Name = "<h1>")]
        public string SeoHead { get; set; }
        [Display(Name = "<description>")]
        public string SeoDesc { get; set; }
        [Display(Name = "Название страницы (рекомендуется не изменять!)")]
        [Required(ErrorMessage ="Введите название страницы!")]
        public string Page { get; set; }
        public string SeoCat { get; set; }
        public IEnumerable<SeoMain> SeoList { get; set; }
    }
}