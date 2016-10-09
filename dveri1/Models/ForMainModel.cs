using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DALdv1;

namespace dveri1.Models
{
    public class ForMainModel
    {
        public List<string> FileName { get; set; }
        public int CountFile { get; set; }
        public IEnumerable<VhodnyeDveri> ListVhodnDv { get; set; }
        public IEnumerable<MegkomnatnyeDveri> ListMkDv { get; set; }
        public PagingInfo PagingInfo { get; set; }
        //для сортировки 0-по индексу(пл умолчанию)
        //1-по возрастанию
        //    2-по убыванию
        //public int Sort { get; set; }
        //для выбора товара по брэнду
     public IEnumerable<string> Brand { get; set; }
        //public IEnumerable<string> Material { get; set; }
     public IEnumerable<TableColor> Color  {get;set;}
     public IEnumerable<string>Country  {get;set;}
     public IEnumerable<string> Napolnitel {get;set;}
     public IEnumerable<string> Yplotnitel {get;set;}
     public IEnumerable<string> OtdNaryg {get;set;}
     public IEnumerable<string> OtdVnytri {get;set;}
     public List<string> Cost { get; set; }
   
     public int Sort { get; set; }
     //чекнутые элементы
        public List<string> CurrentBrand { get; set; }
        public List<string> CurrentCountry { get; set; }
        public List<string> CurrentNapolnitel { get; set; }
     
        public List<string> CurrentColor { get; set; }
        public List<string> CurrentCost { get; set; }

        //public string CurrentMaterial { get; set; }
        public IEnumerable<SliderMainImg> SliderImg { get; set; }
      
        public string SeoKey { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDesc { get; set; }
        public string SeoHead { get; set; }

    }
    public class KaruselTovara
    {
        public IEnumerable<VhodnyeDveri> ListVhodnDv { get; set; }    
    }
}