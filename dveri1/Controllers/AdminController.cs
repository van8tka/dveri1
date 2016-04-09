﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DALdv1;
using Domain2;
using dveri1.Models;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace dveri1.Controllers
{
    public class AdminController : Controller
    {
        DataManager dataManager;
        public AdminController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        int PageSize = 32;
        // GET: Admin
        public ActionResult Panel(int page = 1)
        {
            ForMainModel model = new ForMainModel();
            int TotalItemsProduct = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Count();
            model.ListVhodnDv = dataManager.VhodnyeDvRepository.GetVhodnyeDv().OrderBy(x => x.Id).Skip((page - 1) * PageSize).Take(PageSize);
            model.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                TotalItems = TotalItemsProduct,
                ItemsPerPage = PageSize

            };
           
            return View(model);
        }
        

        [HttpGet]
        public ActionResult CreateVhDv()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateVhDv(CreateVhMod model, IEnumerable<HttpPostedFileBase> fileUpload = null)
        {
            int? CenaSoSkidkoy=null;
            if (ModelState.IsValid)
            {       
                              
                TempData["message"]="Новый товар добавлен в базу данных!";
                if(model.Skidka!=null)
                {
                    if(model.Skidka!= 0)
                    {
                        CenaSoSkidkoy = model.Cena - model.Cena * model.Skidka / 100;
                    }
                }
                dataManager.VhodnyeDvRepository.CreateVhodnyeDv(1, model.Nazvanie, model.Proizvoditel, model.StranaProizv, model.Cvet, model.Napolnitel,
                    model.Yplotnitel, model.TolschinaMetala, model.Furnitura, model.Petli, model.OtdSnarugi,model.OtdVnutri,model.TolschinaDvPolotna,
                    model.Cena,model.Skidka,CenaSoSkidkoy,model.Opisanie,model.Publicaciya);
                int iddver = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Last().Id;
                //метод изменения размера изображения и сохраненния в буфере
                string domainpath = Server.MapPath("~/Content/ImageTemp/");
                if(!Directory.Exists(domainpath))
                {
                    Directory.CreateDirectory(domainpath);
                }

              
                //проход по списку загружаемых файлов и если есть, добавление в БД
                foreach (var image in fileUpload)
                {
                   
                    if (image != null)
                    {
                        //получим ID последнего фото
                        int idfot;
                        FotoVhodnyhDverey ft = dataManager.VhodnyeDvRepository.GetFotoVhDv().LastOrDefault();
                        if (ft == null)
                            idfot = 0;
                        else
                            idfot = ft.Idfoto;

                        string path = Path.Combine(domainpath, image.FileName);
                        image.SaveAs(path);
                        
                        //изменим разрешение файла
                        Image img = Image.FromFile(path);
                        Bitmap myBitmap = new Bitmap(img, new Size(350, 600));
                        Graphics myGraphic = Graphics.FromImage(myBitmap);
                        //теперь нарисуем логотип
                        Image imgLogo = Image.FromFile(Server.MapPath("~/Content/logoinAllImage.png"));
                        //в метод DRAW передали изобр для наложения координата X и Y и размер изобра W и H
                         myGraphic.DrawImage(imgLogo,0, 0, 220,270);
                        //новое имя и сохраним
                        string newfilename = "evrostroy" + (idfot + 1).ToString() + ".jpg";
                        string newfilepath = domainpath + newfilename;
                        myBitmap.Save(newfilepath, ImageFormat.Jpeg);
                       //теперь запишем файл в базу данных
                        FileStream fs = null;
                        fs = new FileStream(newfilepath, FileMode.Open);
                        model.ImageData = new byte[fs.Length];
                        model.ImageMimeType = "image/jpg";
                        fs.Read(model.ImageData, 0, (int)fs.Length);
                        dataManager.VhodnyeDvRepository.CreateFotoVhDv(1, iddver, model.ImageMimeType, model.ImageData);
                        fs.Close();
                      
                        myBitmap.Dispose();
                        myGraphic.Dispose();
                        img.Dispose();
                        imgLogo.Dispose();
                        myBitmap = null;
                        myGraphic = null;
                        img = null;
                        imgLogo = null;
                       fs = null;
                       }
                }
                 fileUpload = null;
                //удалим файлы из временной папки
                DellAllFiles();
                return RedirectToAction("Panel");
            }
            return View(model);
        }
        //метод удаления файлов из каталога
        private void DellAllFiles()
        {
            string domainpath = Server.MapPath("~/Content/ImageTemp/");
            var dir = new DirectoryInfo(domainpath);
            FileInfo[] fileNames = dir.GetFiles("*.*");
            foreach (var item in fileNames)
            {
                   item.Delete();
            }
        }
        [HttpGet]
        public ActionResult DellVhDv(int id, int page = 1)
        {
            try
            {
                dataManager.VhodnyeDvRepository.DeleteVhodnyeDv(id);
                TempData["message"] = "Товар удален из базы данных!";
                return RedirectToAction("Panel", new { page });
            }
            catch
            {
                return RedirectToAction("Exception");
            }
          
        }

        public ActionResult Exception()
        {
            return View();
        }

    }
}
