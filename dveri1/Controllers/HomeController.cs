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


        public int PageSize = 32;

        [HttpGet]
        public ActionResult Index(int? id)
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

            int TotalItemsProduct = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(x => x.Publicaciya == true).Count();
            model.ListVhodnDv = dataManager.VhodnyeDvRepository.GetVhodnyeDv().OrderBy(x => x.Id).Where(x => x.Publicaciya == true).Skip((page - 1) * PageSize).Take(PageSize);
            model.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                TotalItems = TotalItemsProduct,
                ItemsPerPage = PageSize

            };

            if (Request.IsAjaxRequest())
            {
                 return RedirectToAction("ProductList", new { page });
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
    public ActionResult ProductList(int page = 1)
    {          
            ForMainModel model = new ForMainModel();
            int TotalItemsProduct = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(x => x.Publicaciya == true).Count();
            model.ListVhodnDv = dataManager.VhodnyeDvRepository.GetVhodnyeDv().OrderBy(x => x.Id).Where(x => x.Publicaciya == true).Skip((page - 1) * PageSize).Take(PageSize);
          
            model.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                TotalItems = TotalItemsProduct,
                ItemsPerPage = PageSize
            };        
            return PartialView(model);
    }
       

    }
}