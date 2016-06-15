using System;
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
using dveri1.DopMethod;

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
        [Authorize]
        public ActionResult Panel(int page = 1)
        {
            try
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
            catch (Exception er)
            {
                ClassLog.Write("Admin/Panel-"+ er);
                return View("Error");
            }
         
        }
        [Authorize]
        //------------------------------создание нового товара--------------------------------------------------------
        [HttpGet]
        public ActionResult CreateVhDv()
        {
            return View();
        }
        //------------------------------------пост метод создание товара с передачей модели параметров и списка файлов(фото)----------------------
        [Authorize]
        [HttpPost]
        public ActionResult CreateVhDv(CreateVhMod model, IEnumerable<HttpPostedFileBase> fileUpload = null)
        {
            try
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
                        Bitmap myBitmap = new Bitmap(img, new Size(420, 720));
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
                      //освобождаем занятый ресурс
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
                    string dompath = Server.MapPath("~/Content/ImageTemp/");
                   DellFilesFromDomain.DellAllFiles(dompath);
                return RedirectToAction("Panel");
            }
            return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("Admin/CreateVhDv-"+ er);
                return View("Error");
            }
        }

        //------------------------------контроллер удаления товара---------------------------------------------
        [Authorize]
        [HttpGet]
        public ActionResult DellVhDv(int id, int page = 1)
        {
            try
            {
                dataManager.VhodnyeDvRepository.DeleteVhodnyeDv(id);
                TempData["message"] = "Товар удален из базы данных!";
                return RedirectToAction("Panel", new { page });
            }
            catch (Exception er)
            {
                ClassLog.Write("Admin/DellVhDv-"+ er);
                return View("Error");
            }
       }
        //------------------------------------------------контроллер главного слайдера--------------------------------------------------------
        [Authorize]
        [HttpGet]
        public ActionResult SliderMain()
        {
            try
            {
               SliderModel model = new SliderModel();
                model.SliderMI = dataManager.SliderRepository.GetSliderMainImg();
                model.CountSlide = model.SliderMI.Count();
               return View(model);
             }
            catch (Exception er)
            {
                ClassLog.Write("Admin/SliderMain-"+ er);
                return View("Error");
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult SliderMain(IEnumerable<HttpPostedFileBase> fileUpload=null)
        {
            try
            {
             SliderModel model = new SliderModel();
            string domainpath = Server.MapPath("~/Content/ImageTemp/");
            int i = 0;
            bool BreakAddFiles = false;
            int countImage = dataManager.SliderRepository.GetSliderMainImg().Count();
            if(countImage >=8)
            {
                TempData["message"] = "Слайдер содержит 8 изображений или более, для добавления другого изображения необходимо удалить старые изображения.";
                return RedirectToAction("SliderMain");
            }
            else
            {
                foreach (var image in fileUpload)
                {
                    if (image != null)
                    {
                        if(countImage==8)
                        {
                            TempData["message"] = "Слайдер содержит 8 изображений, добавлено только " +i+ " изображения";
                            BreakAddFiles = true;
                            break;
                        }
                        countImage++;
                        i++;
                        //получим ID последнего фото                  
                        string path = Path.Combine(domainpath, image.FileName);
                        image.SaveAs(path);
                        //изменим разрешение файла
                        Image img = Image.FromFile(path);
                        Bitmap myBitmap = new Bitmap(img, new Size(1200, 200));
                        Graphics myGraphic = Graphics.FromImage(myBitmap);
                        //теперь нарисуем логотип
                        //новое имя и сохраним
                        string newfilename = "evrostroySlMAin" + i.ToString() + ".jpg";
                        string newfilepath = domainpath + newfilename;
                        myBitmap.Save(newfilepath, ImageFormat.Jpeg);
                        //теперь запишем файл в базу данных
                        FileStream fs = null;
                        fs = new FileStream(newfilepath, FileMode.Open);
                        model.ImgDataSlider = new byte[fs.Length];
                        model.MimeTypeSlider = "image/jpg";
                        fs.Read(model.ImgDataSlider, 0, (int)fs.Length);
                        dataManager.SliderRepository.CreateSliderMainImg(model.MimeTypeSlider, model.ImgDataSlider);
                        fs.Close();
                        //освобождаем занятый ресурс
                        myBitmap.Dispose();
                        myGraphic.Dispose();
                        img.Dispose();
                        myBitmap = null;
                        myGraphic = null;
                        img = null;
                        fs = null;
                    }
                }
                fileUpload = null;
                //удалим файлы из временной папки
                DellFilesFromDomain.DellAllFiles(domainpath);
                if(!BreakAddFiles)
                {
                    TempData["message"] = "Изображения слайдера добавлены!";
                }
                return RedirectToAction("SliderMain");
            }
            }
            catch (Exception er)
            {
                ClassLog.Write("Admin/SliderMain-"+ er);
                return View("Error");
            }
        }
        //-----------------------------------------------контроллер удаления слайда-------------------------------------------------------------------
        [Authorize]
        public ActionResult DellSlide(int id)
        {
            try
            {
                dataManager.SliderRepository.DellSliderMainImg(id);
                TempData["message"] = "Изображение удалено из базы данных!";
                return RedirectToAction("SliderMain");
             }
            catch (Exception er)
            {
                ClassLog.Write("Admin/DellSlide-"+ er);
                return View("Error");
            }
        }
        //------------------------------------------------контроллер боковогослайдера--------------------------------------------------------
        [Authorize]
        [HttpGet]
        public ActionResult SliderLeft()
        {
            try
            {
                SliderModel model = new SliderModel();
                model.SliderLI = dataManager.SliderRepository.GetSliderLeftImg();
                model.CountSlide = model.SliderLI.Count();
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("Admin/SliderLeft-"+ er);
                return View("Error");
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult SliderLeft(IEnumerable<HttpPostedFileBase> fileUpload = null)
        {
            try
            {
            SliderModel model = new SliderModel();
            string domainpath = Server.MapPath("~/Content/ImageTemp/");
            int i = 0;
            bool BreakAddFiles = false;
            int countImage = dataManager.SliderRepository.GetSliderLeftImg().Count();
            if (countImage >= 8)
            {
                TempData["message"] = "Слайдер содержит 8 изображений или более, для добавления другого изображения необходимо удалить старые изображения.";
                return RedirectToAction("SliderLeft");
            }
            else
            {
                foreach (var image in fileUpload)
                {
                    if (image != null)
                    {
                        if (countImage == 8)
                        {
                            TempData["message"] = "Слайдер содержит 8 изображений, добавлено только " + i + " изображения";
                            BreakAddFiles = true;
                            break;
                        }
                        countImage++;
                        i++;
                        //получим ID последнего фото                  
                        string path = Path.Combine(domainpath, image.FileName);
                        image.SaveAs(path);
                        //изменим разрешение файла
                        Image img = Image.FromFile(path);
                        Bitmap myBitmap = new Bitmap(img, new Size(200, 400));
                        Graphics myGraphic = Graphics.FromImage(myBitmap);
                        //новое имя и сохраним
                        string newfilename = "evrostroySlLeft" + i.ToString() + ".jpg";
                        string newfilepath = domainpath + newfilename;
                        myBitmap.Save(newfilepath, ImageFormat.Jpeg);
                        //теперь запишем файл в базу данных
                        FileStream fs = null;
                        fs = new FileStream(newfilepath, FileMode.Open);
                        model.ImgDataSlider = new byte[fs.Length];
                        model.MimeTypeSlider = "image/jpg";
                        fs.Read(model.ImgDataSlider, 0, (int)fs.Length);
                        dataManager.SliderRepository.CreateSliderLeftImg(model.MimeTypeSlider, model.ImgDataSlider);
                        fs.Close();
                        //освобождаем занятый ресурс
                        myBitmap.Dispose();
                        myGraphic.Dispose();
                        img.Dispose();
                        myBitmap = null;
                        myGraphic = null;
                        img = null;
                        fs = null;
                    }
                }
                fileUpload = null;
                //удалим файлы из временной папки
               DellFilesFromDomain.DellAllFiles(domainpath);
                if (!BreakAddFiles)
                {
                    TempData["message"] = "Изображения слайдера добавлены!";
                }
                return RedirectToAction("SliderLeft");
            }
            }
            catch (Exception er)
            {
                ClassLog.Write("Admin/SliderLeft-"+ er);
                return View("Error");
            }
        }
        //-----------------------------------------------контроллер удаления бокового слайда-------------------------------------------------------------------
        [Authorize]
        public ActionResult DellSlideLeft(int id)
        {
            try
            {
                dataManager.SliderRepository.DellSliderLeftImg(id);
                TempData["message"] = "Изображение удалено из базы данных!";
                return RedirectToAction("SliderLeft");
            }
            catch (Exception er)
            {
                ClassLog.Write("Admin/DellSlideLeft-"+ er);
                return View("Error");
            }
        }
        //-------------------------------action обработки сведений доставки---------------------------------
        [Authorize]
        [HttpGet]
       public ActionResult SvedenijaDostavka()
        {
            try
            {
                //возвращаем один текст сведений о доставке
                OplDostModel model = new OplDostModel();
                model.DostInfo = dataManager.OplDostRepository.GetDostavka().FirstOrDefault().Dostavka1;
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("Admin/SvedenijaDostavka-"+ er);
                return View("Error");
            }
        }
        [Authorize]
        //для разрешения ввода html кода
        [HttpPost, ValidateInput(false)]
       public ActionResult SvedenijaDostavka(OplDostModel model)
        {
            try
            {
                dataManager.OplDostRepository.CreateDostavka(0, model.DostInfo);
                TempData["message"] = "Информация о доставке обновлена!";
                return View(model);  
            }
            catch (Exception er)
            {
                ClassLog.Write("Admin/SvedenijaDostavka-"+ er);
                return View("Error");
            }
        }
        //-------------------------------action обработки сведений об оплате---------------------------------
        [Authorize]
        [HttpGet]
        public ActionResult SvedenijaOplata()
        {
            try
            {
                //возвращаем один текст сведений о доставке
                OplDostModel model = new OplDostModel();
                Oplata opl = dataManager.OplDostRepository.GetOplata().FirstOrDefault();
                if (opl != null)
                {
                    model.OplInfo = opl.Oplata1;
                }
                else
                {
                    model.OplInfo = "информация об оплате отсутствует";
                }
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("Admin/SvedenijaOplata-" + er);
                return View("Error");
            }
        }
        //для разрешения ввода html кода
        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult SvedenijaOplata(OplDostModel model)
        {
            try
            {
                dataManager.OplDostRepository.CreateOplata(0, model.OplInfo);
                TempData["message"] = "Информация об оплате обновлена!";
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("Admin/SvedenijaOplata-" + er);
                return View("Error");
            }
        }
        [Authorize]
        public ActionResult UserPage()
        {
            try
            {
                IEnumerable<User> us = dataManager.UserRepository.GetUsers();
                return View(us);
            }         
             catch (Exception er)
            {
                ClassLog.Write("Admin/UserPage-" + er);
                return View("Error");
            }
        }
        [Authorize]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                int z = dataManager.UserRepository.GetUsers().Count();
                if(z!=1)
                {
                    dataManager.UserRepository.DeleteUser(id);
                    TempData["message"] = "Пользователь удален!";
                } 
                else
                {
                    TempData["message"] = "Невозможно удалить последнего пользователя!";
                }            
                return RedirectToAction("UserPage");
            }
            catch (Exception er)
            {
                ClassLog.Write("Admin/DeleteUser-" + er);
                return View("Error");
            }
        }
      
    }

}
