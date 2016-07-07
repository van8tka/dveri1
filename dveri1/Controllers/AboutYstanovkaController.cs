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
    public class AboutYstanovkaController : Controller
    {
        DataManager dataManager;
        public AboutYstanovkaController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
      
        //-------------------------------action обработки сведений установкм--------------------------------
        [Authorize]
        [HttpGet]
    public ActionResult AdminYstanovka()
    {
            try
            {
                //возвращаем один текст сведений о установки
                ModelYstanovka model = new ModelYstanovka();
                TableYstanovka ty = dataManager.YstanovkaRepository.GetYstanovka().FirstOrDefault();
                model.YstanovkaInfo = ty != null ? ty.Ystanovka : "информация отсутствует..";
              
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("AboutYstanovkaController/AdminYstanovka-" + er);
                return View("Error");
            }
        }
        [Authorize]
        //для разрешения ввода html кода
        [HttpPost, ValidateInput(false)]
        public ActionResult AdminYstanovka(ModelYstanovka model)
        {
            try
            {
                dataManager.YstanovkaRepository.CreateYstanovka(0, model.YstanovkaInfo);
                TempData["message"] = "Информация о установке обновлена!";
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("AboutYstanovkaController/AdminYstanovka-" + er);
                return View("Error");
            }
        }
        [HttpGet]
        public ActionResult YstanovkaInfo()
        {

            try
            {
                ModelYstanovka model = new ModelYstanovka();
                TableYstanovka ty = dataManager.YstanovkaRepository.GetYstanovka().FirstOrDefault();
                model.YstanovkaInfo = ty != null ? ty.Ystanovka : "информация отсутствует..";
                SeoMain s = dataManager.SeoMainRepository.GetSeoMainByPage("Установка");
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
                        model.SeoHead = "Информация об становке дверей и не только";
                    }
                }
                else
                {
                    model.SeoTitle = "Установка дверей компанией Люксеврострой";
                    model.SeoHead = "Установка металлических и межкомнатных дверей";
                }
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("AboutYstanovkaController/YstanovkaInfo-" + er);
                return View("Error");
            }
        }


    }
}