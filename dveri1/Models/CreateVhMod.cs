using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DALdv1;
namespace dveri1.Models
{
    public class CreateVhMod
    {
        public int? ID { get; set; }
        [Required(ErrorMessage ="Введите название товара")]
        [Display(Name = "Название")]
        public string Nazvanie { get; set; }
        [Display(Name = "Производитель")]
        public string Proizvoditel { get; set; }
        [Display(Name = "Страна производитель")]
        public string StranaProizv { get; set; }
        [Display(Name = "Цвет")]
        public string Cvet { get; set; }
        [Display(Name = "Наполнитель")]
        public string Napolnitel { get; set; }
        [Display(Name = "Уплотнитель")]
        public string Yplotnitel { get; set; }
        [Display(Name = "Толщина металла")]
        [RegularExpression(@"^[0-9]{1,20}", ErrorMessage = "Неверный формат ввода толщины металла(допускает только цифры)!")]
        public int? TolschinaMetala { get; set; }
        [Display(Name = "Фурнитура")]
        public string Furnitura { get; set; }
        [Display(Name = "Петли")]
        public string Petli { get; set; }
        [Display(Name = "Отделка снаружи")]
        public string OtdSnarugi { get; set; }
        [Display(Name = "Отделка внутри")]
        public string OtdVnutri { get; set; }
        [Display(Name = "Толщина дверного полотна")]
        [RegularExpression(@"^[0-9]{1,20}", ErrorMessage = "Неверный формат ввода толщины металла(допускает только цифры)!")]
        public int? TolschinaDvPolotna { get; set; }
        [Display(Name = "Цена")]
        [Required(ErrorMessage = "Введите цену товара!")]
        [RegularExpression(@"^[0-9]{1,20}", ErrorMessage = "Неверный формат ввода цены товара!")]
        public int Cena { get; set; }
        [Display(Name = "Скидка в процентах")]
        [RegularExpression(@"^[0-9]{1,20}", ErrorMessage = "Неверный формат ввода скидки товара(допускает только цифры)!")]
        public int? Skidka { get; set; }
        [Display(Name = "Публикация")]
        public bool Publicaciya { get; set; }
        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string Opisanie { get; set; }

        public string ImageMimeType{ get; set; }
        public byte[] ImageData{get;set;}
    }
}