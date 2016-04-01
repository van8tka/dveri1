using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using dveri1.Models;

namespace dveri1.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
          
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                if (i > 1 && i == pagingInfo.CurrentPage + 3)
                {                 
                    tag.AddCssClass("td");
                    tag.MergeAttribute("id","numpage");
                    tag.MergeAttribute("href", pageUrl(i-2));
                    tag.InnerHtml = ">>"; 
                }
                else
                if (i < pagingInfo.TotalPages && i == pagingInfo.CurrentPage - 3)
                {                  
                    tag.AddCssClass("td");
                    tag.MergeAttribute("id", "numpage");
                    tag.MergeAttribute("href", pageUrl(i+2));
                    tag.InnerHtml = "<<";                 
                }
                else
                if (i<pagingInfo.CurrentPage+3&&i>pagingInfo.CurrentPage-3)
                {                  
                    tag.AddCssClass("td");
                    tag.MergeAttribute("id", "numpage");
                    tag.MergeAttribute("href", pageUrl(i));
                    tag.InnerHtml = i.ToString();                   
                }
                else
                    tag.MergeAttribute("style", "display:none");
                if (i == pagingInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                }

                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}