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
        public PagingInfo PagingInfo { get; set; }
        //для сортировки 0-по индексу(пл умолчанию)
        //1-по возрастанию
        //    2-по убыванию
        public int Sort { get; set; }
    }
}