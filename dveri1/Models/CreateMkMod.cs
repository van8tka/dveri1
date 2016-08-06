using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DALdv1;
using System.ComponentModel.DataAnnotations;

namespace dveri1.Models
{
    public class CreateMkMod
    {
        public int? ID { get; set; }
        [Required(ErrorMessage = "Введите название товара")]
        [Display(Name = "Название")]
        public string Nazvanie { get; set; }
        [Required(ErrorMessage = "Введите название фирмы производителя товара")]
        [Display(Name = "Производитель")]
        public string Proizvoditel { get; set; }
        [Display(Name = "Страна производитель")]
        public string StranaProizv { get; set; }
        [Display(Name = "Цвет")]
        public string Cvet { get; set; }
        [Required(ErrorMessage = "Введите название материала товара")]
        [Display(Name = "Материал")]
        public string Material { get; set; }
        [Display(Name = "Покрытие")]
        public string Pokrytie { get; set; }
        [Display(Name = "Каркас")]
        public string Karkas { get; set; }
        [Display(Name = "Тип двери")]
        public string TypeDv { get; set; }
        [Display(Name = "Внутреннее наполнение")]
        public string VnNapoln { get; set; }
        [Display(Name = "Цена")]
        [RegularExpression(@"^[0123456789,]{1,20}", ErrorMessage = "Неверный формат ввода цены товара!")]
        public decimal? Cena { get; set; }
        [Display(Name = "Скидка в процентах (указать число без символа %)")]
        [RegularExpression(@"^[0123456789]{1,20}", ErrorMessage = "Неверный формат ввода скидки товара(допускает только цифры)!")]
        public int? Skidka { get; set; }
        [Display(Name = "Публикация")]
        public bool Publicaciya { get; set; }
        [Display(Name = "Описание")]
        public string Opisanie { get; set; }
        [Display(Name = "Дополнительные характеристики")]
        public string DopChar { get; set; }
        //для сео оптимизации
        [Display(Name = "<title>")]
        public string TitleMkDv { get; set; }
        [Display(Name = "<keywords>")]
        public string KeywordsMkDv { get; set; }
        [Display(Name = "<description>")]
        public string DescriptionMkDv { get; set; }



        public IEnumerable<FotoMegkomnDverey> FotoMkDvList { get; set; }
        public string ImageMimeType { get; set; }
        public byte[] ImageData { get; set; }
    }
}