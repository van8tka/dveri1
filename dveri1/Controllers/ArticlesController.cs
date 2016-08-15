using System;
using System.Linq;
using System.Web.Mvc;
using DALdv1;
using dveri1.Models;
using Domain2;
using dveri1.DopMethod;


namespace dveri1.Controllers
{
    public class ArticlesController : Controller
    {
        DataManager dataManager;
        public ArticlesController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        [Authorize]
        //получение всего списка статей вх дв
        public ActionResult AdminGetArticles()
        {
            ModelArticles model = new ModelArticles();
            model.ArticlesList = dataManager.ArticlesRepository.GetArticles().OrderByDescending(x=>x.Date);
            return View(model);
        }

        //создание статьи
        [Authorize]
        [HttpGet]
        public ActionResult CreateArticle(int id = 0)
        {
            ModelArticleCreate model = new ModelArticleCreate();
            if(id==0)
            {
                model.ArticleID = 0;
            }
            else
            {
              TableArticle ta = dataManager.ArticlesRepository.GetArticle(id);
                if(ta!=null)
                {
                    model.ArticleID = id;
                    model.ArticleDescript = ta.Headings;
                    model.ArticleName = ta.Name;
                    model.ArticleText = ta.Articles;
                }
            }
            return View(model);
        }
        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateArticle(ModelArticleCreate model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    dataManager.ArticlesRepository.CreateArticle(model.ArticleID, model.ArticleName, model.ArticleDescript, model.ArticleText);
                    if(model.ArticleID==0)
                    TempData["message"] = "Статья добавлена в базуданных!";
                    else
                        TempData["message"] = "Статья изменена!";
                    return RedirectToAction("AdminGetArticles");
                }
                return View(model);
            }        
              catch (Exception er)
            {
                ClassLog.Write("CreateArticle/Articles-" + er);
                return View("Error");
            }
        }
        [Authorize]
        //удаление статьи
        public ActionResult DellArticle(int id)
        {
            try
            {
                dataManager.ArticlesRepository.DelArticle(id);
                TempData["message"] = "Статья удалена!";
                return RedirectToAction("AdminGetArticles");
            }
            catch (Exception er)
            {
                ClassLog.Write("DelArticle/Articles-" + er);
                return View("Error");
            }
        }

        [Authorize]
        [HttpGet]
       //создание сео для статей
       public ActionResult CreateSeoArticle(int id)
        {
            ModelSeoArticle model = new ModelSeoArticle();
            model.IDart = id;
            TableSeoArticle ta = dataManager.ArticlesRepository.GetSeoArticle(id);
            model.Title = ta.Title;
            model.Key = ta.Keywords;
            model.Desc = ta.Description;
            return View(model);
        }
        [Authorize]
        [HttpPost]
        //создание сео для статей
        public ActionResult CreateSeoArticle(ModelSeoArticle model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    dataManager.ArticlesRepository.CreateSeoArticle(model.IDart, model.Title, model.Key, model.Desc);
                    TempData["message"] = "Значения метатегов изменены(добавлены)!";
                    return RedirectToAction("AdminGetArticles");
                }
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("CreateSeoArticle/Articles-" + er);
                return View("Error");
            }
        }

        //получение последних 5 статей на боковую панель layout
        public ActionResult GetFiveArticles()
        {
            ModelArticles model = new ModelArticles();
             model.ArticlesList = dataManager.ArticlesRepository.GetArticles().OrderByDescending(x => x.Date).Take(5);
            return View(model);
        } 
        //==============================================получение одной конкретной статьи===================================
        public ActionResult GetArticle(string tov, int id)
        {
            ModelArticles model = new ModelArticles();
            model.ArticleOne = dataManager.ArticlesRepository.GetArticle(id);
            TableSeoArticle ta = dataManager.ArticlesRepository.GetSeoArticle(id);
            if(ta.Title!=null)
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
        public ActionResult GetArticles(int page=1)
        {
            ModelArticles model = new ModelArticles();
            SeoMain seo = dataManager.SeoMainRepository.GetSeoMainByPage("Статьи");
            model.SeoTitle = seo.Title != null ? seo.Title : "Лучшие статьи про двери, установку дверей и правильный подбор";
            model.SeoKey = seo.Keywords;
            model.SeoHead = seo.Header != null ? seo.Header : "Самая полезная информация про установку, монтаж, выбор межкомнатных и входных дверей.";
            model.SeoDesc = seo.Description; 
            model.ArticlesList = dataManager.ArticlesRepository.GetArticles().OrderByDescending(z=>z.Date).Skip(PageSize*(page-1)).Take(PageSize);

            model.pagingInfo = new PagingInfo()
            {
                CurrentPage = page,
                TotalItems = dataManager.ArticlesRepository.GetArticles().Count(),
                ItemsPerPage = PageSize
            };
                  
            return View(model);
        }

        //=======================================о МК ДВ=======================
        //получение всего списка статей вх дв
        [Authorize]
        public ActionResult AdminGetArticlesMk()
        {
            ModelArticles model = new ModelArticles();
            model.ArticlesListMk = dataManager.ArticlesRepository.GetArticlesMk().OrderByDescending(x => x.Date);
            return View(model);
        }

        //создание статьи
        [Authorize]
        [HttpGet]
        public ActionResult CreateArticleMk(int id = 0)
        {
            ModelArticleCreate model = new ModelArticleCreate();
            if (id == 0)
            {
                model.ArticleID = 0;
            }
            else
            {
                TableArticlesMk ta = dataManager.ArticlesRepository.GetArticleMk(id);
                if (ta != null)
                {
                    model.ArticleID = id;
                    model.ArticleDescript = ta.Headings;
                    model.ArticleName = ta.Name;
                    model.ArticleText = ta.Articles;
                }
            }
            return View(model);
        }
        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateArticleMk(ModelArticleCreate model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dataManager.ArticlesRepository.CreateArticleMk(model.ArticleID, model.ArticleName, model.ArticleDescript, model.ArticleText);
                    if (model.ArticleID == 0)
                        TempData["message"] = "Статья добавлена в базуданных!";
                    else
                        TempData["message"] = "Статья изменена!";
                    return RedirectToAction("AdminGetArticlesMk");
                }
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("CreateArticleMk/Articles-" + er);
                return View("Error");
            }
        }
        [Authorize]
        //удаление статьи
        public ActionResult DellArticleMk(int id)
        {
            try
            {
                dataManager.ArticlesRepository.DelArticleMk(id);
                TempData["message"] = "Статья удалена!";
                return RedirectToAction("AdminGetArticlesMk");
            }
            catch (Exception er)
            {
                ClassLog.Write("DelArticleMk/Articles-" + er);
                return View("Error");
            }
        }

        [Authorize]
        [HttpGet]
        //создание сео для статей
        public ActionResult CreateSeoArticleMk(int id)
        {
            ModelSeoArticle model = new ModelSeoArticle();
            model.IDart = id;
            TableSeoArticlesMk ta = dataManager.ArticlesRepository.GetSeoArticleMk(id);
            model.Title = ta.Title;
            model.Key = ta.Keywords;
            model.Desc = ta.Description;
            return View(model);
        }
        [Authorize]
        [HttpPost]
        //создание сео для статей
        public ActionResult CreateSeoArticleMk(ModelSeoArticle model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dataManager.ArticlesRepository.CreateSeoArticleMk(model.IDart, model.Title, model.Key, model.Desc);
                    TempData["message"] = "Значения метатегов изменены(добавлены)!";
                    return RedirectToAction("AdminGetArticlesMk");
                }
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("CreateSeoArticleMk/Articles-" + er);
                return View("Error");
            }
        }


    }
}