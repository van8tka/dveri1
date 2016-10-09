using System;
using System.Linq;
using System.Web.Mvc;
using DALdv1;
using dveri1.Models;
using Domain2;
using dveri1.DopMethod;

namespace dveri1.Controllers
{
    public class AboutUsController : Controller
    {
        DataManager dataManager;
        public AboutUsController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        [Authorize]
        [HttpGet]
        public ActionResult AdminAboutUs()
        {
            try
            {
                //возвращаем один текст сведений о установки
                ModelYstanovka model = new ModelYstanovka();
                TableAboutU ta = dataManager.AboutUsRepository.GetAboutUs();
                model.YstanovkaInfo = ta != null ? ta.TextAboutUs : "информация отсутствует..";

                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("AboutUsController/AdminAboutUs-" + er);
                return View("Error");
            }
        }

        [Authorize]
        //для разрешения ввода html кода
        [HttpPost, ValidateInput(false)]
        public ActionResult AdminAboutUs(ModelYstanovka model)
        {
            try
            {
                dataManager.AboutUsRepository.CreateAboutUs(0, model.YstanovkaInfo);
                TempData["message"] = "Информация о компании обновлена!";
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("AboutUsController/AdminAboutUs-" + er);
                return View("Error");
            }
        }
        [HttpGet]
        public ActionResult AboutUsInfo()
        {
            try
            {
                ModelYstanovka model = new ModelYstanovka();
                TableAboutU ty = dataManager.AboutUsRepository.GetAboutUs();
                model.YstanovkaInfo = ty != null ? ty.TextAboutUs : "информация отсутствует..";
                SeoMain s = dataManager.SeoMainRepository.GetSeoMainByPage("О нас");
                if (s != null)
                {
                    model.SeoTitle = s.Title;
                    model.SeoKey = s.Keywords;
                    model.SeoDesc = s.Description;
                    if (s.Header != null)
                    {
                        model.SeoHead = s.Header;
                    }
                    else
                    {
                        model.SeoHead = "Информация о компании";
                    }
                }
                else
                {
                    model.SeoTitle = "Информация о компании";
                    model.SeoHead = "Информация о компании";
                }
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("AboutUsController/AboutUsInfo-" + er);
                return View("Error");
            }
        }


    }
}