using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DALdv1;
using System.ComponentModel.DataAnnotations;

namespace dveri1.Models
{
    public class ModelExpImp
    {
      
        public string Category { get; set; }
        public bool ID { get; set; }
       
        [Display(Name = "Название")]
        public bool Nazvanie { get; set; }    
        [Display(Name = "Производитель")]
        public bool Proizvoditel { get; set; }
        [Display(Name = "Страна производитель")]
        public bool StranaProizv { get; set; }
        [Display(Name = "Цвет")]
        public bool Cvet { get; set; }
        [Display(Name = "Наполнитель")]
        public bool Napolnitel { get; set; }
        [Display(Name = "Уплотнитель")]
        public bool Yplotnitel { get; set; }
        [Display(Name = "Толщина металла")]     
        public bool TolschinaMetala { get; set; }
        [Display(Name = "Фурнитура")]
        public bool Furnitura { get; set; }
        [Display(Name = "Петли")]
        public bool Petli { get; set; }
        [Display(Name = "Отделка снаружи")]
        public bool OtdSnarugi { get; set; }
        [Display(Name = "Отделка внутри")]
        public bool OtdVnutri { get; set; }
        [Display(Name = "Толщина дверного полотна")]
        public bool TolschinaDvPolotna { get; set; }
        [Display(Name = "Цена")]
        //[Required(ErrorMessage = "Введите цену товара!")]
        //[RegularExpression(@"^[0-9]{1,20}", ErrorMessage = "Неверный формат ввода цены товара!")]
        public bool Cena { get; set; }
        [Display(Name = "Скидка в процентах")]    
        public bool Skidka { get; set; }
        [Display(Name = "Публикация")]
        public bool Publicaciya { get; set; }
        [Display(Name = "Описание")]
        public bool Opisanie { get; set; }
        [Display(Name = "Дополнительные характеристики")]
        public bool DopChar { get; set; }     

    }
}