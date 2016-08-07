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
        //global
        private const string cProizVhonyh = "Производитель входных дверей";
        int PageSize = 250;
       

        //для добавления списка меню в админку по категориям товара(производителя)
        public ActionResult MenuAdmin()
        {
            try
            {//выберем и сгрупируем данные по производителю для составления меню
                IEnumerable<VhodnyeDveri> vhd = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(z => z.Proizvoditel != null);
                var model = vhd.OrderBy(z => z.Proizvoditel).GroupBy(z => z.Proizvoditel).ToDictionary(z => z.Key, z => z.ToList());
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("Admin/MenuAdmin-" + er);
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult Panel(int page = 1, string brand = null, int index = 0)
        {
            try
            {
                int TotalItemsProduct = 0;
                ForMainModel model = new ForMainModel();
                //индекс для поиска товара по индексу
                if(index==0)
                {
                    if (brand == null || brand == "Входные двери - весь список")
                    {
                        ViewBag.NameProductList = "Входные двери - весь список";
                        TotalItemsProduct = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Count();
                        model.ListVhodnDv = dataManager.VhodnyeDvRepository.GetVhodnyeDv().OrderBy(x => x.Id).Skip((page - 1) * PageSize).Take(PageSize);
                    }
                    else
                    {
                        ViewBag.NameProductList = brand;
                        TotalItemsProduct = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(x => x.Proizvoditel == brand).Count();
                        model.ListVhodnDv = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(x => x.Proizvoditel == brand).OrderBy(x => x.Id).Skip((page - 1) * PageSize).Take(PageSize);
                    }
                }
                else
                {
                    model.ListVhodnDv = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(x => x.Id == index);
                    if(model.ListVhodnDv.Count()!=0)
                    {
                        ViewBag.NameProductList = model.ListVhodnDv.ElementAt(0).Proizvoditel;
                        TotalItemsProduct = 1;
                    }
                    else
                    {
                        TempData["message"] = "Товар с кодом " + index + " ненайден!";
                        return RedirectToAction("Panel");
                    }
                   
                }

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
        public ActionResult CreateVhDv(int id=0)
        {
            if(id!=0)
            {
                CreateVhMod model = new CreateVhMod();
                VhodnyeDveri v = dataManager.VhodnyeDvRepository.GetVhodnyeDvById(id);
                model.Cena = v.Cena;
                model.Cvet = v.Cvet;
                model.Furnitura = v.Furnitura;
                model.ID = id;
                model.Napolnitel = v.Napolnitel;
                model.Nazvanie = v.Nazvanie;
                model.Opisanie = v.Opisanie;
                model.OtdSnarugi = v.OtdelkaSnarugi;
                model.OtdVnutri = v.OtdelkaVnutri;
                model.Petli = v.Petli;
                model.Proizvoditel = v.Proizvoditel;
                model.Publicaciya = v.Publicaciya;
                model.Skidka = v.Skidka;
                model.StranaProizv = v.Strana;
                model.TolschinaDvPolotna = v.TolschinaDvPolotna;
                model.TolschinaMetala = v.TolschinaMetalla;
                model.Yplotnitel = v.Yplotnitel;
                model.DopChar = v.DopCharacteristics;
                model.FotoVhDvList = dataManager.VhodnyeDvRepository.GetFotoVhDvByID(id);
                if (dataManager.VhodnyeDvRepository.GetSeoVhDv().Where(z => z.Id == id).FirstOrDefault() != null)
                {
                    model.TitleVhDv = v.SeoVhodnuhDverei.TitleDveri;
                    model.KeywordsVhDv = v.SeoVhodnuhDverei.KeywordsDveri;
                    model.DescriptionVhDv = v.SeoVhodnuhDverei.DescriptionDveri;
                }
                return View(model);
            }
            return View();
           
        }
        //------------------------------------пост метод создание товара с передачей модели параметров и списка файлов(фото)----------------------
        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateVhDv(CreateVhMod model, IEnumerable<HttpPostedFileBase> fileUpload = null)
        {
            try
            {        
            decimal? CenaSoSkidkoy=null;
            if (ModelState.IsValid)
            {
                    //если ввели отрицательное значение цены
                    if (model.Cena < 0)
                    {
                        string[] g = model.Cena.Value.ToString().Split('-');
                        model.Cena = decimal.Parse(g[1]);
                    }
                    TempData["message"]="Новый товар добавлен в базу данных!";
                if(model.Skidka!=null)
                {
                        if (model.Skidka != 0 && (model.Cena != 0 && model.Cena != null))
                        {
                        CenaSoSkidkoy = model.Cena - model.Cena * model.Skidka / 100;
                    }
                        else
                        {
                            model.Skidka = null;
                        }
                    }
                dataManager.VhodnyeDvRepository.CreateVhodnyeDv(0, model.Nazvanie, model.Proizvoditel, model.StranaProizv, model.Cvet, model.Napolnitel,
                    model.Yplotnitel, model.TolschinaMetala, model.Furnitura, model.Petli, model.OtdSnarugi,model.OtdVnutri,model.TolschinaDvPolotna,
                    model.Cena,model.Skidka,CenaSoSkidkoy,model.Opisanie,model.Publicaciya,model.DopChar);
                int iddver = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Last().Id;
                    //метод создания элементов сео единицы товара
                    dataManager.VhodnyeDvRepository.CreateSeoVhDveri(iddver, model.TitleVhDv, model.KeywordsVhDv, model.DescriptionVhDv);
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
                    //проверим есть ли сео теги для данной категории товара согласно фирмы производителя
                    //для продвижения по фирме производителю
                    SeoMain s = dataManager.SeoMainRepository.GetSeoMain().Where(x=>x.Page==model.Proizvoditel && x.Category==cProizVhonyh).FirstOrDefault();
                    if(s==null)
                    {
                        string category = cProizVhonyh;
                        dataManager.SeoMainRepository.CreateSeo(0, "Купить входные двери фирмы "+model.Proizvoditel,null, null, model.Proizvoditel,null, category);
                    }
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
        //-----------------------------------контроллер изменения товара---------------------------------------------
        [Authorize]
        [HttpGet]
        public ActionResult EditVhDv(int id)
        {
            try
            {
                CreateVhMod model = new CreateVhMod();
                VhodnyeDveri v = dataManager.VhodnyeDvRepository.GetVhodnyeDvById(id);
                model.Cena = v.Cena;
                model.Cvet = v.Cvet;            
                model.Furnitura = v.Furnitura;
                model.ID = id;            
                model.Napolnitel = v.Napolnitel;
                model.Nazvanie = v.Nazvanie;
                model.Opisanie = v.Opisanie;
                model.OtdSnarugi = v.OtdelkaSnarugi;
                model.OtdVnutri = v.OtdelkaVnutri;
                model.Petli = v.Petli;
                model.Proizvoditel = v.Proizvoditel;
                model.Publicaciya = v.Publicaciya;
                model.Skidka = v.Skidka;
                model.StranaProizv = v.Strana;              
                model.TolschinaDvPolotna = v.TolschinaDvPolotna;
                model.TolschinaMetala = v.TolschinaMetalla;
                model.Yplotnitel = v.Yplotnitel;
                model.DopChar = v.DopCharacteristics;
                model.FotoVhDvList = dataManager.VhodnyeDvRepository.GetFotoVhDvByID(id);
                if(dataManager.VhodnyeDvRepository.GetSeoVhDv().Where(z=>z.Id==id).FirstOrDefault()!=null)
                {
                    model.TitleVhDv = v.SeoVhodnuhDverei.TitleDveri;
                    model.KeywordsVhDv = v.SeoVhodnuhDverei.KeywordsDveri;
                    model.DescriptionVhDv = v.SeoVhodnuhDverei.DescriptionDveri;
                }              
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("Admin/EditVhDv-" + er);
                return View("Error");
            }
        }
        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult EditVhDv(CreateVhMod model, IEnumerable<HttpPostedFileBase> fileUpload = null)
        {
            try
            {
                decimal? CenaSoSkidkoy = null;
                    if (ModelState.IsValid)
                    {
                    //если ввели отрицательное значение цены
                        if(model.Cena<0)
                        {
                            string[] g = model.Cena.Value.ToString().Split('-');
                            model.Cena = decimal.Parse(g[1]);
                        }
                        TempData["message"] = "Товар изменен в базе данных!";
                        if (model.Skidka != null)
                        {
                            if (model.Skidka != 0&&(model.Cena!=0&&model.Cena!=null))
                            {
                                CenaSoSkidkoy = model.Cena - model.Cena * model.Skidka / 100;
                            }
                        else
                        {
                            model.Skidka = null;
                        }
                        }
                         dataManager.VhodnyeDvRepository.CreateVhodnyeDv((int)model.ID, model.Nazvanie, model.Proizvoditel, model.StranaProizv, model.Cvet, model.Napolnitel,
                            model.Yplotnitel, model.TolschinaMetala, model.Furnitura, model.Petli, model.OtdSnarugi, model.OtdVnutri, model.TolschinaDvPolotna,
                            model.Cena, model.Skidka, CenaSoSkidkoy, model.Opisanie, model.Publicaciya, model.DopChar);
                       
                        //метод создания элементов сео единицы товара
                        dataManager.VhodnyeDvRepository.CreateSeoVhDveri((int)model.ID, model.TitleVhDv, model.KeywordsVhDv, model.DescriptionVhDv);
                        //метод изменения размера изображения и сохраненния в буфере
                        string domainpath = Server.MapPath("~/Content/ImageTemp/");
                        if (!Directory.Exists(domainpath))
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
                                myGraphic.DrawImage(imgLogo, 0, 0, 220, 270);
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
                                dataManager.VhodnyeDvRepository.CreateFotoVhDv(1, (int)model.ID, model.ImageMimeType, model.ImageData);
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
                        model.FotoVhDvList = dataManager.VhodnyeDvRepository.GetFotoVhDvByID((int)model.ID);
                        //удалим файлы из временной папки
                        string dompath = Server.MapPath("~/Content/ImageTemp/");
                        DellFilesFromDomain.DellAllFiles(dompath);
                        return View(model);
                    }
                model.FotoVhDvList = dataManager.VhodnyeDvRepository.GetFotoVhDvByID((int)model.ID);
                return View(model);
                }
             catch (Exception er)
            {
                ClassLog.Write("Admin/EditVhDv-" + er);
                return View("Error");
            }
        }

        //удаление фотографии товара
        [Authorize]
        public ActionResult DellFotoVhDv(int idf, int idt)
        {
            try
            {
                dataManager.VhodnyeDvRepository.DeleteFotoVhDv(idf);
                return RedirectToAction("EditVhDv", new { id = idt });
            }
            catch (Exception er)
            {
                ClassLog.Write("Admin/DellVhDv-" + er);
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
                //проверим на наличие еще таких товаров, 
                VhodnyeDveri vh = dataManager.VhodnyeDvRepository.GetVhodnyeDvById(id);
                IEnumerable<VhodnyeDveri> List = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(x => x.Proizvoditel == vh.Proizvoditel);
                if(List.Count()==1)//если последний товар удаляем,то
                {//удалим и страницу продвижения его 
                       SeoMain s = dataManager.SeoMainRepository.GetSeoMain().Where(x=>x.Page==vh.Proizvoditel && x.Category == cProizVhonyh).FirstOrDefault();
                    if(s!=null)
                        dataManager.SeoMainRepository.DellSeo(s.ID);
                       
                }
                //теперь удалим      
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
                Dostavka dos = dataManager.OplDostRepository.GetDostavka().FirstOrDefault();
                //если нет сущности то инфо отсутст иначе присваиваем знасчение поля Dostavka1
                model.DostInfo = dos!=null ? dos.Dostavka1 : "информация отсутствует..";                       
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
        //экшен для вывода страницы пользователей
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
        //экшен удаления пользователя
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
        //функция вызванная ajax запросом на изменение публикации
       [Authorize]
       [HttpGet]
       public void ChangePublic(int id, string list)
        {
            try
            {//получили по id элемент
                if(list =="входныедвери")//обработка публикации списка товаров входных дверей
                {
                    VhodnyeDveri vh = dataManager.VhodnyeDvRepository.GetVhodnyeDvById(id);
                    //изменили в зависимости от состояния
                    if (vh.Publicaciya)
                    {
                        vh.Publicaciya = false;
                    }
                    else
                    {
                        vh.Publicaciya = true;
                    }//перезаписали
                    dataManager.VhodnyeDvRepository.CreateVhodnyeDv(vh.Id, vh.Nazvanie, vh.Proizvoditel, vh.Strana, vh.Cvet, vh.Napolnitel,
                                 vh.Yplotnitel, vh.TolschinaMetalla, vh.Furnitura, vh.Petli, vh.OtdelkaSnarugi, vh.OtdelkaVnutri, vh.TolschinaDvPolotna,
                                  vh.Cena, vh.Skidka, vh.CenaSoSkidcoy, vh.Opisanie, vh.Publicaciya, vh.DopCharacteristics);
                }
                //обработка публикации коммента входных дверей
                if (list == "комментарийвходныхдверей")
                {
                    CommentVhDveri cvhd = dataManager.CommentRepository.GetCommentVhDv().Where(x => x.ID == id).FirstOrDefault();
                    if (cvhd.Public)
                    {
                        cvhd.Public = false;
                    }
                    else
                    {
                        cvhd.Public = true;
                    }
                    dataManager.CommentRepository.CreateCommentVhDv(cvhd.ID, cvhd.IDdv, cvhd.Name, cvhd.E_mail, cvhd.Comment, cvhd.Response, cvhd.Heading, cvhd.Public, cvhd.Stars, cvhd.Date);
                }
                //обработка публикации компании
               if(list =="комментарийкомпании")
                {
                    CommentCompany cvhd = dataManager.CommentRepository.GetCommetCompany().Where(x => x.ID == id).FirstOrDefault();
                    if (cvhd.Public)
                    {
                        cvhd.Public = false;
                    }
                    else
                    {
                        cvhd.Public = true;
                    }
                    dataManager.CommentRepository.CreateCommentCompany(cvhd.ID, cvhd.Name, cvhd.E_mail, cvhd.Comment, cvhd.Response, cvhd.Heading, cvhd.Public, cvhd.Stars, cvhd.Date);
                  //потом добавить о межкомнатных дверях..
                }            
            }
            catch (Exception er)
            {
                ClassLog.Write("Admin/ChangePublic-" + er);               
            }
        }
       //------------поиск товара по номеру ID
        [Authorize]
        [HttpGet]
        public ActionResult FindById()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult FindById(ModelFind model)
        {
            try
            {
                 if (ModelState.IsValid)
                {
                    return RedirectToAction("Panel", new { index = model.IndexFind});
                }
                TempData["message"] = "Неверный формат ввода кода товара, допускаются только цифры.";
                return RedirectToAction("Panel");
            }
           catch (Exception er)
            {
                ClassLog.Write("Admin/FindById-" + er);
                return View("Error");
            }
        }
     
        [Authorize]
         //--------------------------------------изменение цены------------------------------------------
        public ActionResult ChangeCena(int id, string newprice="0")
        {//получим длину строки
            try
            {
            int count = newprice.Length;
            decimal res;
            //если строка не пустая
            VhodnyeDveri vh = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(x => x.Id == id).FirstOrDefault();
            if (count!=0)
            {//получим элемент по ID               
                if (vh != null)
                {//проверим на то что это цифровая строка
                  
                     if(newprice == "Цена не установлена")
                        {
                            vh.Cena = 0;
                        }
                      else
                        {
                            string[] g = newprice.Split('р');
                            if(Decimal.TryParse(g[0],out res))
                            {//проверим на отрицательное значение
                                if(res<0)
                                {
                                    string[] k = g[0].Split('-');
                                    vh.Cena = decimal.Parse(k[1]);
                                }
                                else
                                {
                                    vh.Cena = res;
                                }
                               
                            }
                        }
                  }
            }
            else
            {
                vh.Cena = 0;
            }
          if(vh.Cena!=0 && (vh.Skidka!=0&&vh.Skidka!=null))
          {
                vh.CenaSoSkidcoy = vh.Cena - vh.Cena * vh.Skidka /100;
          }
          else
                {
                    vh.CenaSoSkidcoy = null;
                    vh.Skidka = null;
                }
          //изменяем данные товара
            dataManager.VhodnyeDvRepository.CreateVhodnyeDv(id, vh.Nazvanie, vh.Proizvoditel, vh.Strana, vh.Cvet, vh.Napolnitel, vh.Yplotnitel, vh.TolschinaMetalla, vh.Furnitura, vh.Petli, vh.OtdelkaSnarugi, vh.OtdelkaVnutri, vh.TolschinaDvPolotna, vh.Cena, vh.Skidka, vh.CenaSoSkidcoy, vh.Opisanie, vh.Publicaciya, vh.DopCharacteristics);
//создадим массив для передачи на страницу с ценами и скидкой
                string[] price = new string[2];
                if (vh.Cena != null)
                {
                    price[0] = vh.Cena.Value.ToString("c");
                }
                else
                {
                    price[0] = "Цена не установлена";
                }
                price[1] = vh.Skidka == null ? "" : vh.Skidka.ToString();
                        
                return Json(price,JsonRequestBehavior.AllowGet);
            }
            catch (Exception er)
            {
                ClassLog.Write("Admin/ChangeCena-" + er);
                return View("Error");
            }
        }
    }

}
