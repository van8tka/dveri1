using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DALdv1;
using System.ComponentModel.DataAnnotations;

namespace dveri1.Models
{
    public class ModelOurWorks
    {
        public IEnumerable<TableOurWork> ListOurWorks { get; set; }
        public PagingInfo PageInfo { get; set; }
        public string SeoKey { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDesc { get; set; }
        public string SeoHead { get; set; }
    }
    public class ModelCreateOurWorks
    {
        public int IdWork { get; set; }
        [Required(ErrorMessage="Заполните описание работы!")]
        public string Descriptwork { get; set; }
        public IEnumerable<TableFotoOurWork> ListFoto { get; set; }

    }
}