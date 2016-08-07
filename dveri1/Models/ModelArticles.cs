using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DALdv1;
using System.ComponentModel.DataAnnotations;

namespace dveri1.Models
{
    public class ModelArticles
    {
        public string SeoKey { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDesc { get; set; }
        public string SeoHead { get; set; }
        public TableArticle ArticleOne { get; set; }
        public IEnumerable<TableArticle> ArticlesList { get; set; }
        public IEnumerable<TableArticlesMk> ArticlesListMk { get; set; }
        public PagingInfo pagingInfo { get; set; }
    }
    public class ModelArticleCreate
    {
        [Required(ErrorMessage = "Введите текст статьи!")]
        public string ArticleText { get; set; }
        [Required(ErrorMessage = "Введите название статьи!")]
        [StringLength(549, MinimumLength = 1, ErrorMessage = "Максимальная длина названия 550 символов")]
        public string ArticleName { get; set; }
        [Required(ErrorMessage = "Введите краткое описание статьи!")]
        [StringLength(1549, MinimumLength = 1, ErrorMessage = "Максимальная длина описания 1550 символов")]
        public string ArticleDescript { get; set; }
        public int ArticleID { get; set; }
    }
    public class ModelSeoArticle
    {
        public int IDart { get; set; }
        public string Key { get; set; }
        public string Desc { get; set; }
        [Required(ErrorMessage ="Введите значение title!")]
        public string Title { get; set; }
    }
}