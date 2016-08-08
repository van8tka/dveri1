using DALdv1;
using Domain2;
using dveri2.Models;
using System.Linq;
using System.Web.Mvc;

namespace dveri2.Controllers
{
    public class ArticlesController : Controller
    {
        private DataManager dataManager;
        public ArticlesController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }



        //получение последних 5 статей на боковую панель layout
        public ActionResult GetFiveArticles()
        {
            ModelArticles model = new ModelArticles();
            model.ArticlesList = dataManager.ArticlesRepository.GetArticlesMk().OrderByDescending(x => x.Date).Take(5);
            return View(model);
        }
        //==============================================получение одной конкретной статьи===================================
        public ActionResult GetArticle(int id)
        {
            ModelArticles model = new ModelArticles();
            model.ArticleOne = dataManager.ArticlesRepository.GetArticleMk(id);
            TableSeoArticlesMk ta = dataManager.ArticlesRepository.GetSeoArticleMk(id);
            if (ta.Title != null)
            {
                model.SeoTitle = ta.Title;
                model.SeoKey = ta.Keywords;
                model.SeoDesc = ta.Description;
            }
            else
            {
                model.SeoTitle = model.ArticleOne.Name;
            }
            return View(model);
        }
        //                       ============================получение свсего списка статей===================================
        public int PageSize = 50;
        public ActionResult GetArticles(int page = 1)
        {
            ModelArticles model = new ModelArticles();
            SeoMain seo = dataManager.SeoMainRepository.GetSeoMainByPage("Статьи");
            model.SeoTitle = seo.Title != null ? seo.Title : "Лучшие статьи про двери, установку дверей и правильный подбор";
            model.SeoKey = seo.Keywords;
            model.SeoHead = seo.Header != null ? seo.Header : "Самая полезная информация про установку, монтаж, выбор межкомнатных и входных дверей.";
            model.SeoDesc = seo.Description;
            model.ArticlesList = dataManager.ArticlesRepository.GetArticlesMk().OrderByDescending(z => z.Date).Skip(PageSize * (page - 1)).Take(PageSize);

            model.pagingInfo = new PagingInfo()
            {
                CurrentPage = page,
                TotalItems = dataManager.ArticlesRepository.GetArticles().Count(),
                ItemsPerPage = PageSize
            };

            return View(model);
        }

    }
}