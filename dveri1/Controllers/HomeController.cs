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
    public class HomeController : Controller
    {

        private DataManager dataManager;

        public HomeController(DataManager dataManager)
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
        public ActionResult Index(int? id, int sort=0, string brand="весьтовар")
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
                ClassLog.Write("Home/Index-", er);
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
                ClassLog.Write("Home/GetImage-", er);
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
                ClassLog.Write("Home/ProductList-", er);
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
                ClassLog.Write("Home/SortirDveri-", er);
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
                ClassLog.Write("Home/ContactOnPanel-", er);
                return View("Error");
            }
        }
    }
}