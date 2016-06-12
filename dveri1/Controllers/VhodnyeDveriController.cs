using Domain2;
using dveri1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DALdv1;
using dveri1.DopMethod;

namespace dveri1.Controllers
{
    public class VhodnyeDveriController : Controller
    {

        private DataManager dataManager;

        public VhodnyeDveriController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        //сортировка
        //    int sort 
        //    1-по возрастанию,
        //    2-по убыванию,
        //    0-по номеру(по умолчанию)
      
        public int PageSize = 32;
        //отображение списка и баннера (главная страница)
        [HttpGet]
        public ActionResult VhodnyeDveriIndex(int? id, int sort=0, string brand="весьтовар")
        {
            try
            {
                int page = id ?? 1;
                ForMainModel model = new ForMainModel();
                model.SliderImg = dataManager.SliderRepository.GetSliderMainImg();
                model.CountFile = model.SliderImg.Count();
                model.SliderLeftImg = dataManager.SliderRepository.GetSliderLeftImg();
                model.Sort = sort;
                int TotalItemsProduct;
                if (brand == "весьтовар")
                {
                    TotalItemsProduct = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(x => x.Publicaciya == true).Count();
                }
                else
                {
                    TotalItemsProduct = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(x => x.Publicaciya == true && x.Proizvoditel == brand).Count();
                }
                //вызлв метода сортировки

                model.ListVhodnDv = SortirDveri(page, sort, brand);

                model.CurrentBrand = brand;


                //model.ListVhodnDv = dataManager.VhodnyeDvRepository.GetVhodnyeDv().OrderBy(x => x.Id).Where(x => x.Publicaciya == true).Skip((page - 1) * PageSize).Take(PageSize);
                model.PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    TotalItems = TotalItemsProduct,
                    ItemsPerPage = PageSize
                };
                model.Brand = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Select(z => z.Proizvoditel).Distinct().OrderBy(z => z);
                if (Request.IsAjaxRequest())
                {
                     return RedirectToAction("ProductList", new { page, sort, brand });
                }
              
                return View(model);
                
            }
            catch (Exception er)
            {
                ClassLog.Write("VhodnyeDveri/VhodnyeDveriIndex-", er);
                return View("Error");
            }
        }
        //   метод загрузки первого изображения в списке товара
        public FileContentResult GetImage(int id)
        {
            try
            {
                FotoVhodnyhDverey foto = dataManager.VhodnyeDvRepository.GetFotoVhDvByID(id).OrderBy(x=>x.Idfoto).FirstOrDefault();
                if (foto!= null)
                { return File(foto.Imaging, foto.MimeType); }
                else
                {//   изображение по умолчанию
                    return null;
                }
            }
            catch (Exception er)
            {
                ClassLog.Write("VhodnyeDveri/GetImage-", er);
                return null;
            }
        }
      
            [HttpGet]
    public ActionResult ProductList(int page = 1, int sort = 0,string brand= "весьтовар")
    {
            try
            {
            ForMainModel model = new ForMainModel();
            model.CurrentBrand = brand;
            int TotalItemsProduct;
            if (brand== "весьтовар")
            {
                TotalItemsProduct = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(x => x.Publicaciya == true).Count();
            }
            else
            {
                TotalItemsProduct = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(x => x.Publicaciya == true && x.Proizvoditel == brand).Count();
            }
          
            model.ListVhodnDv = SortirDveri(page,sort,brand);
            model.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                TotalItems = TotalItemsProduct,
                ItemsPerPage = PageSize
            };        
            return PartialView(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("VhodnyeDveri/ProductList-", er);
                return View("Error");
            }
        }
      //   метод возврата списка отсортированных дверей
      public IEnumerable<VhodnyeDveri> SortirDveri(int page,int s, string br)
        {
            try
            {
            IEnumerable<VhodnyeDveri> temp = null;
            if(br!= "весьтовар")
            {
                switch (s)
                {
                    case 0:
                        {
                            temp = dataManager.VhodnyeDvRepository.GetVhodnyeDv().OrderBy(x => x.Id).Where(x => x.Publicaciya == true && x.Proizvoditel == br).Skip((page - 1) * PageSize).Take(PageSize);
                            break;
                        }
                    case 1:
                        {
                            temp = dataManager.VhodnyeDvRepository.GetVhodnyeDv().OrderBy(x => x.Cena).Where(x => x.Publicaciya == true && x.Proizvoditel == br).Skip((page - 1) * PageSize).Take(PageSize);
                            break;
                        }
                    case 2:
                        {
                            temp = dataManager.VhodnyeDvRepository.GetVhodnyeDv().OrderByDescending(x => x.Cena).Where(x => x.Publicaciya == true && x.Proizvoditel == br).Skip((page - 1) * PageSize).Take(PageSize);
                            break;
                        }
                    default:
                        break;
                }
            }
            else
            {
                switch (s)
                {
                    case 0:
                        {
                            temp = dataManager.VhodnyeDvRepository.GetVhodnyeDv().OrderBy(x => x.Id).Where(x => x.Publicaciya == true).Skip((page - 1) * PageSize).Take(PageSize);
                            break;
                        }
                    case 1:
                        {
                            temp = dataManager.VhodnyeDvRepository.GetVhodnyeDv().OrderBy(x => x.Cena).Where(x => x.Publicaciya == true).Skip((page - 1) * PageSize).Take(PageSize);
                            break;
                        }
                    case 2:
                        {
                            temp = dataManager.VhodnyeDvRepository.GetVhodnyeDv().OrderByDescending(x => x.Cena).Where(x => x.Publicaciya == true).Skip((page - 1) * PageSize).Take(PageSize);
                            break;
                        }
                    default:
                        break;
                }
            }       
            return temp;
            }
            catch (Exception er)
            {
                ClassLog.Write("VhodnyeDveri/SortirDveri-", er);
                return null;
            }
        }
        //вызов сведений о контактах в layout
 public ActionResult ContactOnPanel()
        {
            try
            {
            ViewBag.Count = 0;
            ViewBag.Count1 = 0;
            ContactModel model = new ContactModel();
            model.ContactList = dataManager.ContactRepository.GetContacts();
            model.GrafikWorkList = dataManager.ContactRepository.GetGrafikWork();
            return PartialView(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("VhodnyeDveri/ContactOnPanel-", er);
                return View("Error");
            }
        }
        //==========================================карточка товара==============================================================================
        [HttpGet]
        public ActionResult TovarPage(int id)
        {
            KartochkaTovaraModel model = new KartochkaTovaraModel();
            model.Tovar = dataManager.VhodnyeDvRepository.GetVhodnyeDvById(id);
            model.FotoTovara = dataManager.VhodnyeDvRepository.GetFotoVhDvByID(id);
            return View(model);
        }

//акшен для возврата частичного представления с данными кликнутого фото в карточке товара
        public ActionResult ImgTov(int id)
        {
            try
            {
                FotoVhodnyhDverey foto = dataManager.VhodnyeDvRepository.GetFotoVhDv().Where(x => x.Idfoto == id).FirstOrDefault();
                return View(foto);
            }
            catch (Exception er)
            {
                ClassLog.Write("VhodnyeDveri/ImgTov-", er);
                return View("Error");
            }
        }
        //хлебные крошки
        public ActionResult BreadCrumbs(ForMainModel mainmod = null,KartochkaTovaraModel kmod = null, string namepart = null)
        {
            try
            {
                ModelBreadCrumbs mod = new ModelBreadCrumbs();
                if (namepart != null)
                {
                    mod.NamePartSite = namepart;
                }
                if (mainmod.Brand != null&& mainmod.Brand.First()!="весьтовар")
                {
                    mod.NameCategory = mainmod.Brand.First();
                }
                else
                {
                    if (kmod.Tovar != null)
                    {
                        mod.NameCategory = kmod.Tovar.Proizvoditel;
                        mod.NameProduct = kmod.Tovar.Nazvanie;
                    }
                }
                return View(mod);
            }
            catch (Exception er)
            {
                ClassLog.Write("VhodnyeDveri/BreadCrumbs-", er);
                return View("Error");
            }
        }

        //===============================купить двери===================================================
        [HttpGet]
        public ActionResult BuyDveri(int id, string type)
        {
            try
            {
                ModelBuyDveri model = new ModelBuyDveri();
                model.IdDveri = id;
                model.Type = type;
                if (type == "входные двери")
                {
                    model.DvName = dataManager.VhodnyeDvRepository.GetVhodnyeDvById(id).Nazvanie;
                }
               return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("VhodnyeDveri/BuyDveri-", er);
                return View("Error");
            }

        }
        [HttpPost]
        public ActionResult BuyDveri(ModelBuyDveri model)
        {
            try
            {
                 if(ModelState.IsValid)
                {//вызов метода отправки сообщегия админу
                   if( SendMessageAboutBuyDoor(model))                  
                    {
                             TempData["messageklient"] = "Ваше сообщение отправлено, менеджер перезвонит Вам в течении 15 минут!";     
                    }
                    else
                    {   
                            TempData["messageklient"] = "При отправке сообщения возникла ошибка, сообщите пожалуйста нашему менеджеру по указанному выше номеру телефона!";                          
                    }
                    return RedirectToAction("TovarPage", new {id=model.IdDveri});
                }
                else
                {
                    return View(model);
                }                
            }
            catch (Exception er)
            {
               ClassLog.Write("VhodnyeDveri/BuyDveri-", er);
               return View("Error");
           }
      }
       //================метод отправки сообщения админу от клиента========================
       protected bool SendMessageAboutBuyDoor(ModelBuyDveri model)
        {
            //запись клиента в бд
            dataManager.KlientRepository.CreateKlient(model.IdDveri, model.KlientName, model.KlientPhone, model.KlientAdres, model.KlientQuestion, model.Type);
            //создание и отправка сообщение админу
            string body;
            string them = "Cообщение от клиента: " + model.KlientName + ", тел. " + model.KlientPhone;
            string adres = dataManager.ContactRepository.GetContacts().Where(x => x.TypeSv == "e-mail").FirstOrDefault().NumberSv;

            VhodnyeDveri vhd = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(x => x.Id == model.IdDveri).FirstOrDefault();

            body = "Текст сообщения: " + model.KlientQuestion + "\r\n" + "тип товара: " + model.Type + "\r\n" + "код товара: " + model.IdDveri + "\r\n" + "наименование товара: " + vhd.Nazvanie + "\r\n" + "фирма производитель: " + vhd.Proizvoditel +
            "\r\n" + "Цена: " + vhd.Cena + "\r\n" + "Скидка: " + vhd.Skidka + "\r\n" + "Цена со скидкой: " + vhd.CenaSoSkidcoy;
            if (SendMsg.Message(body, them, adres))
            { return true; }
            else
            {return false; }
        }


        //===========================для модального окна покупки=====================
        //[HttpGet]
        public ActionResult BuyDveriModal(int id, string type)
        {
            try
            {
                ModelBuyDveri model = new ModelBuyDveri();
                model.IdDveri = id;
                model.Type = type;
                if (type == "входные двери")
                {
                    model.DvName = dataManager.VhodnyeDvRepository.GetVhodnyeDvById(id).Nazvanie;
                }
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("VhodnyeDveri/BuyDveriModal-", er);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult BuyDveriModal(ModelBuyDveri model)
        {
            try
            {
                if (ModelState.IsValid)
                {//вызов метода отправки сообщения админу
                    if(SendMessageAboutBuyDoor(model))
                    {
                        return Json(model.KlientName + "! Вы сделали заказ на покупку товара: " + model.DvName + ". Наш менеджер перезвонит Вам в течении 15 минут! Спасибо что выбрали наш магазин!", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("При отправке сообщения возникла ошибка, сообщите пожалуйста нашему менеджеру по указанному выше номеру телефона!", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception er)
            {
                ClassLog.Write("VhodnyeDveri/BuyDveriModel-", er);
                return View("Error");
            }
        }
    }
}