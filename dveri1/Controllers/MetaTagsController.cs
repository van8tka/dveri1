using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DALdv1;
using Domain2;
using dveri1.DopMethod;
using dveri1.Models;

namespace dveri1.Controllers
{
    public class MetaTagsController : Controller
    {
        private DataManager dataManager;
        public MetaTagsController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        [Authorize]
        [HttpGet]
        public ActionResult SeoGet()
        {
            try
            {
               if (dataManager.SeoMainRepository.GetSeoMainByPage("Главная")==null)
                {
                    dataManager.SeoMainRepository.CreateSeo(0, null, null, null,null, "Главная");
                    dataManager.SeoMainRepository.CreateSeo(0, null, null, null, null, "Входные двери");
                    dataManager.SeoMainRepository.CreateSeo(0, null, null, null, null, "Межкомнатные двери");
                    dataManager.SeoMainRepository.CreateSeo(0, null, null, null, null, "Доставка и оплата");
                    dataManager.SeoMainRepository.CreateSeo(0, null, null, null, null, "Отзывы");
                    dataManager.SeoMainRepository.CreateSeo(0, null, null, null, null, "Контакты");
                }
                ModelSeoMain model = new ModelSeoMain();
                model.SeoList = dataManager.SeoMainRepository.GetSeoMain();
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("MetaTags/SeoGet-" + er);
                return View("Error");
           }
        }
        [Authorize]
        [HttpGet]
        public ActionResult CreateSeo(int id=0)
        {
            try
            {
                ModelSeoMain model = new ModelSeoMain();
                if (id != 0)
                {
                    SeoMain s = dataManager.SeoMainRepository.GetSeoMain().Where(i => i.ID == id).FirstOrDefault();
                    model.Id = id;
                    model.SeoDesc = s.Description;
                    model.SeoKey = s.Keywords;
                    model.SeoTitle = s.Title;
                    model.Page = s.Page;
                    model.SeoHead = s.Header;
                }
                    return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("MetaTags/CreateSeo-" + er);
                return View("Error");
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateSeo(ModelSeoMain model)
        {
            try
            {
                if (dataManager.SeoMainRepository.GetSeoMainByPage(model.Page) == null)
                {
                    if (ModelState.IsValid)
                    {
                    TempData["message"] = "Информация метатэгов добавлена (изменена)";
                    dataManager.SeoMainRepository.CreateSeo(model.Id, model.SeoTitle, model.SeoKey, model.SeoDesc, model.Page,model.SeoHead);                                   
                    return RedirectToAction("SeoGet");
                    }
                }
                else
                {
                    if(model.Id == 0)
                    {
                        ModelState.AddModelError("Page", "Страница с таким именем уже существует!");
                    }
                    else
                    {
                        if (ModelState.IsValid)
                        {
                            TempData["message"] = "Информация метатэгов добавлена (изменена)";
                            dataManager.SeoMainRepository.CreateSeo(model.Id, model.SeoTitle, model.SeoKey, model.SeoDesc, model.Page,model.SeoHead);
                            return RedirectToAction("SeoGet");
                        }
                    }
                }
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("MetaTags/CreateSeo-" + er);
                return View("Error");
            }
        }
        [HttpGet]
        [Authorize]
        public ActionResult DellSeo(int id)
        {
            try
            {
                dataManager.SeoMainRepository.DellSeo(id);
                TempData["message"] = "Информация о странице удалена!";
                return RedirectToAction("SeoGet");
            }
            catch (Exception er)
            {
                ClassLog.Write("MetaTags/DellSeo-" + er);
                return View("Error");
            }
        }
    }
}