using Domain2;
using dveri1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DALdv1;

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
        //int sort 
        //    1-по возрастанию,
        //    2-по убыванию,
        //    0-по номеру(по умолчанию)

        public int PageSize = 32;
        //отображение списка и баннера (главная страница)
        [HttpGet]
        public ActionResult Index(int? id, int sort=0)
        {
            int page = id??1;
            ForMainModel model = new ForMainModel();
            string domainpath = Server.MapPath("~/Content/MainSlider");
            //получаем путь 
            var dir = new DirectoryInfo(domainpath);
            //получаем список файлов
            FileInfo[] fileNames = dir.GetFiles("*.*");
            List<string> item = new List<string>();
            //добавляем их в список
            foreach (var file in fileNames)
            {
                item.Add(file.Name);
            }
            model.FileName = item;
            model.CountFile = item.Count();
            model.Sort = sort;
            int TotalItemsProduct = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(x => x.Publicaciya == true).Count();
            //вызлв метода сортировки
            model.ListVhodnDv = SortirDveri(page,sort);
            //model.ListVhodnDv = dataManager.VhodnyeDvRepository.GetVhodnyeDv().OrderBy(x => x.Id).Where(x => x.Publicaciya == true).Skip((page - 1) * PageSize).Take(PageSize);
            model.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                TotalItems = TotalItemsProduct,
                ItemsPerPage = PageSize

            };

            if (Request.IsAjaxRequest())
            {
                 return RedirectToAction("ProductList", new { page,sort });
            }
            return View(model);

        }
        //метод загрузки первого изображения в списке товара
        public FileContentResult GetImage(int id)
        {
            FotoVhodnyhDverey foto = dataManager.VhodnyeDvRepository.GetFotoVhDvByID(id).OrderBy(x=>x.Idfoto).FirstOrDefault();

            if (foto!= null)
            { return File(foto.Imaging, foto.MimeType); }
            else
            {//изображение по умолчанию
                return null;
            }
        }

      
            [HttpGet]
    public ActionResult ProductList(int page = 1, int sort = 0)
    {          
            ForMainModel model = new ForMainModel();
            int TotalItemsProduct = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(x => x.Publicaciya == true).Count();
            model.ListVhodnDv = SortirDveri(page,sort);
            model.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                TotalItems = TotalItemsProduct,
                ItemsPerPage = PageSize
            };        
            return PartialView(model);
    }
      //метод возврата списка отсортированных дверей
      public IEnumerable<VhodnyeDveri> SortirDveri(int page,int s)
        {
            IEnumerable<VhodnyeDveri> temp = null;
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
            return temp;
        }

    }
}