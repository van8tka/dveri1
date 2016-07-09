using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DALdv1;
using dveri1.Models;
using dveri1.Infrastructure;
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
        //получение всего списка статей
        public ActionResult AdminGetArticles()
        {
            ModelArticles model = new ModelArticles();
            model.ArticlesList = dataManager.ArticlesRepository.GetArticles();
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

    }
}