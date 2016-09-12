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
    public class AdminMkDvController : Controller
    {
        DataManager dataManager;
        public AdminMkDvController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        int PageSize = 250;
        public ActionResult Index()
        {
            return View();
        }
        
        //глобал конст
        private const string cMegomnAllList = "Межкомнатные двери - весь список";
        private const string cProizvMegkomn = "Производитель межкомнатных дверей";
        private const string cMaterialMegkomn = "Материал межкомнатных дверей";
       


        //для добавления списка меню в админку по категориям товара(производителя) и по материалу
        public ActionResult MenuAdminMkDv()
        {
            try
            {//выберем и сгрупируем данные по производителю для составления меню
                ModelAdMenu model = new ModelAdMenu();
                IEnumerable<MegkomnatnyeDveri> vhd = dataManager.MegkomDvRepository.GetMkDv().Where(z => z.Proizvoditel != null);
                IEnumerable<MegkomnatnyeDveri> mk = dataManager.MegkomDvRepository.GetMkDv().Where(z=>z.Material!= null);
                model.DictionaryMkDvProizvoditel = vhd.OrderBy(z => z.Proizvoditel).GroupBy(z => z.Proizvoditel).ToDictionary(z => z.Key, z => z.ToList());
                model.DictionaryMkDvMaterial = mk.OrderBy(z => z.Material).GroupBy(z => z.Material).ToDictionary(z => z.Key, z => z.ToList());
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("AdminMkDv/MenuAdminMkDv-" + er);
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult PanelMkDv(int page = 1, string brand = null,string material = null, int index = 0)
        {
            try
            {
                int TotalItemsProduct = 0;
                ForMainModel model = new ForMainModel();
                //индекс для поиска товара по индексу
                if (index == 0)
                {
                    if (brand == null && material == null)
                    {
                        ViewBag.NameMat = null;
                        ViewBag.NameProductList = cMegomnAllList;
                        TotalItemsProduct = dataManager.MegkomDvRepository.GetMkDv().Count();
                        model.ListMkDv = dataManager.MegkomDvRepository.GetMkDv().OrderBy(x => x.Id).Skip((page - 1) * PageSize).Take(PageSize);
                    }
                    else
                    {
                        if (brand !=null && material==null)
                        {
                            ViewBag.NameMat = null;
                            ViewBag.NameProductList = brand;
                            TotalItemsProduct = dataManager.MegkomDvRepository.GetMkDv().Where(x => x.Proizvoditel == brand).Count();
                            model.ListMkDv = dataManager.MegkomDvRepository.GetMkDv().Where(x => x.Proizvoditel == brand).OrderBy(x => x.Id).Skip((page - 1) * PageSize).Take(PageSize);
                        }
                        if (brand == null && material != null)
                        {
                            ViewBag.NameMat = material;
                            ViewBag.NameProductList = material;
                            TotalItemsProduct = dataManager.MegkomDvRepository.GetMkDv().Where(x => x.Material == material).Count();
                            model.ListMkDv = dataManager.MegkomDvRepository.GetMkDv().Where(x => x.Material == material).OrderBy(x => x.Id).Skip((page - 1) * PageSize).Take(PageSize);
                        }
                    }
                }
                else
                {
                    model.ListMkDv = dataManager.MegkomDvRepository.GetMkDv().Where(x => x.Id == index);
                    if (model.ListMkDv.Count() != 0)
                    {
                        ViewBag.NameProductList = model.ListMkDv.ElementAt(0).Proizvoditel;
                        ViewBag.NameMat = null;
                        TotalItemsProduct = 1;
                    }
                    else
                    {
                        TempData["message"] = "Товар с кодом " + index + " ненайден!";
                        return RedirectToAction("PanelMkDv");
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
                ClassLog.Write("AdminMkDv/PanelMkDv-" + er);
                return View("Error");
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
                    return RedirectToAction("PanelMkDv", new { index = model.IndexFind });
                }
                TempData["message"] = "Неверный формат ввода кода товара, допускаются только цифры.";
                return RedirectToAction("PanelMkDv");
            }
            catch (Exception er)
            {
                ClassLog.Write("AdminMkDv/FindById-" + er);
                return View("Error");
            }
        }


        [Authorize]
        //------------------------------создание нового товара--------------------------------------------------------
        [HttpGet]
        public ActionResult CreateMkDv(int id = 0)
        {
            if (id != 0)
            {
                CreateMkMod model = new CreateMkMod();
                MegkomnatnyeDveri v = dataManager.MegkomDvRepository.GetMkDvById(id);
                model.Cena = v.Cena;
                model.Cvet = v.Cvet;
                model.Material = v.Material;
                model.ID = id;
                model.VnNapoln = v.VnytrenneeNapolnenie;
                model.Nazvanie = v.Nazvanie;
                model.Opisanie = v.Opisanie;
                model.Karkas = v.Karkas;
                model.Pokrytie = v.Pokrytie;
                model.TypeDv = v.TypDveri;
                model.Proizvoditel = v.Proizvoditel;
                model.Publicaciya = v.Publicaciya;
                model.Skidka = v.Skidka;
                model.StranaProizv = v.Strana;
                model.DopChar = v.DopCharacteristics;


                model.FotoMkDvList = dataManager.MegkomDvRepository.GetFotoMkDv().Where(z=>z.Id==id);
                if (dataManager.MegkomDvRepository.GetSeoMkDv().Where(z => z.Id == id).FirstOrDefault() != null)
                {
                    model.TitleMkDv = v.SeoMkDverei.TitleDveri;
                    model.KeywordsMkDv = v.SeoMkDverei.KeywordsDveri;
                    model.DescriptionMkDv = v.SeoMkDverei.DescriptionDveri;
                }
                return View(model);
            }
            return View();

        }
      
        //------------------------------------пост метод создание товара с передачей модели параметров и списка файлов(фото)----------------------
        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateMkDv(CreateMkMod model, IEnumerable<HttpPostedFileBase> fileUpload = null)
        {
            try
            {
               
                if (ModelState.IsValid)
                {
                    decimal? CenaSoSkidkoy = null;
                    //если ввели отрицательное значение цены
                    if (model.Cena < 0)
                    {
                        string[] g = model.Cena.Value.ToString().Split('-');
                        model.Cena = decimal.Parse(g[1]);
                    }
                    TempData["message"] = "Новый товар добавлен в базу данных!";
                    if (model.Skidka != null)
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
                    dataManager.MegkomDvRepository.CreateMkDv(0, model.Nazvanie, model.Proizvoditel, model.StranaProizv, model.Cvet,model.Material,
                       model.Pokrytie,model.Karkas,model.TypeDv,model.VnNapoln, model.Cena, model.Skidka, CenaSoSkidkoy, model.Opisanie, model.Publicaciya, model.DopChar);
                    int iddver = dataManager.MegkomDvRepository.GetMkDv().Last().Id;
                    //метод создания элементов сео единицы товара
                    dataManager.MegkomDvRepository.CreateSeoMkDveri(iddver, model.TitleMkDv, model.KeywordsMkDv, model.DescriptionMkDv);
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
                            FotoMegkomnDverey ft = dataManager.MegkomDvRepository.GetFotoMkDv().LastOrDefault();
                            if (ft == null)
                                idfot = 0;
                            else
                                idfot = ft.IdFoto;

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
                            //изменим качество изображения(это новое от 12.08.2016)
                            Encoder myEncoder = Encoder.Quality;
                            EncoderParameters myEncoderParameters = new EncoderParameters(1);
                            // Save the bitmap as a JPG file with zero quality level compression.
                            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 40L);
                            myEncoderParameters.Param[0] = myEncoderParameter;
                            //вызов метода получения кодировки файла(из msdn)
                            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);


                            myBitmap.Save(newfilepath, jpgEncoder, myEncoderParameters);
                            //теперь запишем файл в базу данных
                            FileStream fs = null;
                            fs = new FileStream(newfilepath, FileMode.Open);
                            model.ImageData = new byte[fs.Length];
                            model.ImageMimeType = "image/jpg";
                            fs.Read(model.ImageData, 0, (int)fs.Length);
                            dataManager.MegkomDvRepository.CreateFotoMkDv(1, iddver, model.ImageMimeType, model.ImageData);
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
                    SeoMain s = dataManager.SeoMainRepository.GetSeoMain().Where(x => x.Page == model.Proizvoditel && x.Category == cProizvMegkomn).FirstOrDefault();
                    if (s == null)
                    {
                        string category = cProizvMegkomn;
                        dataManager.SeoMainRepository.CreateSeo(0, "Купить межкомнатные двери фирмы " + model.Proizvoditel, null, null, model.Proizvoditel, null, category);
                    }
                  
                    //для продвижения по названию материала межкомн дверей
                    SeoMain sm = dataManager.SeoMainRepository.GetSeoMainByPage(model.Material);
                    if (sm == null)
                    {
                        string category = cMaterialMegkomn;
                        dataManager.SeoMainRepository.CreateSeo(0, "Купить межкомнатные двери из " + model.Material, null, null, model.Material, null, category);
                    }
                    return RedirectToAction("PanelMkDv");
                }
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("AdminMkDv/CreateMkDv-" + er);
                return View("Error");
            }
        }

        //-----------------------------------контроллер изменения товара---------------------------------------------
        [Authorize]
        [HttpGet]
        public ActionResult EditMkDv(int id)
        {
            try
            {

              
                    CreateMkMod model = new CreateMkMod();
                    MegkomnatnyeDveri v = dataManager.MegkomDvRepository.GetMkDvById(id);
                    model.Cena = v.Cena;
                    model.Cvet = v.Cvet;
                    model.Material = v.Material;
                    model.ID = id;
                    model.VnNapoln = v.VnytrenneeNapolnenie;
                    model.Nazvanie = v.Nazvanie;
                    model.Opisanie = v.Opisanie;
                    model.Karkas = v.Karkas;
                    model.Pokrytie = v.Pokrytie;
                    model.TypeDv = v.TypDveri;
                    model.Proizvoditel = v.Proizvoditel;
                    model.Publicaciya = v.Publicaciya;
                    model.Skidka = v.Skidka;
                    model.StranaProizv = v.Strana;
                    model.DopChar = v.DopCharacteristics;
                    model.FotoMkDvList = dataManager.MegkomDvRepository.GetFotoMkDv().Where(z => z.Id == id);
                    if (dataManager.MegkomDvRepository.GetSeoMkDv().Where(z => z.Id == id).FirstOrDefault() != null)
                    {
                        model.TitleMkDv = v.SeoMkDverei.TitleDveri;
                        model.KeywordsMkDv = v.SeoMkDverei.KeywordsDveri;
                        model.DescriptionMkDv = v.SeoMkDverei.DescriptionDveri;
                    }
                    return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("AdminMkDv/EditMkDv-" + er);
                return View("Error");
            }
        }
        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult EditMkDv(CreateMkMod model, IEnumerable<HttpPostedFileBase> fileUpload = null)
        {
            try
            {
                decimal? CenaSoSkidkoy = null;
                if (ModelState.IsValid)
                {
                    //если ввели отрицательное значение цены
                    if (model.Cena < 0)
                    {
                        string[] g = model.Cena.Value.ToString().Split('-');
                        model.Cena = decimal.Parse(g[1]);
                    }
                    TempData["message"] = "Товар изменен в базе данных!";
                    if (model.Skidka != null)
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
                    dataManager.MegkomDvRepository.CreateMkDv((int)model.ID, model.Nazvanie, model.Proizvoditel, model.StranaProizv, model.Cvet, model.Material,
                    model.Pokrytie, model.Karkas, model.TypeDv, model.VnNapoln, model.Cena, model.Skidka, CenaSoSkidkoy, model.Opisanie, model.Publicaciya, model.DopChar);


                    //метод создания элементов сео единицы товара
                    dataManager.MegkomDvRepository.CreateSeoMkDveri((int)model.ID, model.TitleMkDv, model.KeywordsMkDv, model.DescriptionMkDv);
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
                            FotoMegkomnDverey ft = dataManager.MegkomDvRepository.GetFotoMkDv().LastOrDefault();
                            if (ft == null)
                                idfot = 0;
                            else
                                idfot = ft.IdFoto;

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
                            //изменим качество изображения(это новое от 12.08.2016)
                            Encoder myEncoder = Encoder.Quality;
                            EncoderParameters myEncoderParameters = new EncoderParameters(1);
                            // Save the bitmap as a JPG file with zero quality level compression.
                            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 40L);
                            myEncoderParameters.Param[0] = myEncoderParameter;
                            //вызов метода получения кодировки файла(из msdn)
                            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);


                            myBitmap.Save(newfilepath, jpgEncoder, myEncoderParameters);
                            //теперь запишем файл в базу данных
                            FileStream fs = null;
                            fs = new FileStream(newfilepath, FileMode.Open);
                            model.ImageData = new byte[fs.Length];
                            model.ImageMimeType = "image/jpg";
                            fs.Read(model.ImageData, 0, (int)fs.Length);
                            dataManager.MegkomDvRepository.CreateFotoMkDv(1, (int)model.ID, model.ImageMimeType, model.ImageData);
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
                    model.FotoMkDvList = dataManager.MegkomDvRepository.GetFotoMkDv().Where(z => z.Id == (int)model.ID);
                    //удалим файлы из временной папки
                    string dompath = Server.MapPath("~/Content/ImageTemp/");
                    DellFilesFromDomain.DellAllFiles(dompath);
                    return View(model);
                }
                model.FotoMkDvList = dataManager.MegkomDvRepository.GetFotoMkDv().Where(z => z.Id == (int)model.ID);
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("AdminMkDv/EditMkDv-" + er);
                return View("Error");
            }
        }
        //удаление фотографии товара
        [Authorize]
        public ActionResult DellFotoMkDv(int idf, int idt)
        {
            try
            {
                dataManager.MegkomDvRepository.DeleteFotoMkDv(idf);
                return RedirectToAction("EditMkDv", new { id = idt });
            }
            catch (Exception er)
            {
                ClassLog.Write("AdminMkDv/DellFotoMkDv-" + er);
                return View("Error");
            }
        }

        //------------------------------контроллер удаления товара---------------------------------------------
        [Authorize]
        [HttpGet]
        public ActionResult DellMkDv(int id, int page = 1)
        {
            try
            {
                //проверим на наличие еще таких товаров, 
                MegkomnatnyeDveri vh = dataManager.MegkomDvRepository.GetMkDvById(id);
                IEnumerable<MegkomnatnyeDveri> List = dataManager.MegkomDvRepository.GetMkDv().Where(x => x.Proizvoditel == vh.Proizvoditel);
                if (List.Count() == 1)//если последний товар удаляем,то
                {//удалим и страницу продвижения его 
                    SeoMain s = dataManager.SeoMainRepository.GetSeoMain().Where(x => x.Page == vh.Proizvoditel && x.Category == cProizvMegkomn).FirstOrDefault();
                    if (s != null)
                          dataManager.SeoMainRepository.DellSeo(s.ID);
               }
                //получим список по материалам из которых изготовлены двери
                IEnumerable<MegkomnatnyeDveri> List2 = dataManager.MegkomDvRepository.GetMkDv().Where(x => x.Material == vh.Material);
                //если это последняя дверь то удалим и сео по этой странице
                if (List.Count()==1)
                {
                    SeoMain s = dataManager.SeoMainRepository.GetSeoMainByPage(vh.Material);
                    dataManager.SeoMainRepository.DellSeo(s.ID);
                }
             
                //теперь удалим 
                dataManager.MegkomDvRepository.DeleteMkDv(id);
                TempData["message"] = "Товар удален из базы данных!";
                return RedirectToAction("PanelMkDv", new { page });
            }
            catch (Exception er)
            {
                ClassLog.Write("AdminMkDv/DellMkDv-" + er);
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
                if (list == "межкомнатныедвери")//обработка публикации списка товаров межкомнатных двери
                {
                    MegkomnatnyeDveri vh = dataManager.MegkomDvRepository.GetMkDvById(id);
                    //изменили в зависимости от состояния
                    if (vh.Publicaciya)
                    {
                        vh.Publicaciya = false;
                    }
                    else
                    {
                        vh.Publicaciya = true;
                    }//перезаписали
                    dataManager.MegkomDvRepository.CreateMkDv(vh.Id,vh.Nazvanie,vh.Proizvoditel,vh.Strana,vh.Cvet,vh.Material,vh.Pokrytie,vh.Karkas,
                        vh.TypDveri,vh.VnytrenneeNapolnenie,vh.Cena,vh.Skidka,vh.CenaSoSkidkoy,vh.Opisanie,vh.Publicaciya,vh.DopCharacteristics);

                }
                //обработка публикации коммента межкомнатных дверей
                if (list == "комментариймежкомнатныхдверей")
                {
                   CommentMkDv cvhd = dataManager.CommentRepository.GetCommentMkDv().Where(x => x.ID == id).FirstOrDefault();
                    if (cvhd.Public)
                    {
                        cvhd.Public = false;
                    }
                    else
                    {
                        cvhd.Public = true;
                    }
                    dataManager.CommentRepository.CreateCommentMkDv(cvhd.ID, cvhd.IDdv, cvhd.Name, cvhd.E_mail, cvhd.Comment, cvhd.Response, cvhd.Heading, cvhd.Public, cvhd.Stars, cvhd.Date);
                }
            }
            catch (Exception er)
            {
                ClassLog.Write("AdminMkDv/ChangePublic-" + er);
            }
        }
        //--------------------------------------изменение цены------------------------------------------
        public ActionResult ChangeCena(int id, string newprice = "0")
        {//получим длину строки
            try
            {
                int count = newprice.Length;
                decimal res;
                //если строка не пустая
                MegkomnatnyeDveri vh = dataManager.MegkomDvRepository.GetMkDvById(id);
                if (count != 0)
                {//получим элемент по ID               
                    if (vh != null)
                    {//проверим на то что это цифровая строка

                        if (newprice == "Цена не установлена")
                        {
                            vh.Cena = 0;
                        }
                        else
                        {
                            string[] g = newprice.Split('р');
                            if (Decimal.TryParse(g[0], out res))
                            {//проверим на отрицательное значение
                                if (res < 0)
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
                if (vh.Cena != 0 && (vh.Skidka != 0 && vh.Skidka != null))
                {
                    vh.CenaSoSkidkoy = vh.Cena - vh.Cena * vh.Skidka / 100;
                }
                else
                {
                    vh.CenaSoSkidkoy = null;
                    vh.Skidka = null;
                }
                //изменяем данные товара
                dataManager.MegkomDvRepository.CreateMkDv(vh.Id, vh.Nazvanie, vh.Proizvoditel, vh.Strana, vh.Cvet, vh.Material, vh.Pokrytie, vh.Karkas,
                        vh.TypDveri, vh.VnytrenneeNapolnenie, vh.Cena, vh.Skidka, vh.CenaSoSkidkoy, vh.Opisanie, vh.Publicaciya, vh.DopCharacteristics);
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
//возвращаем данные в формате JSon
                return Json(price, JsonRequestBehavior.AllowGet);
            }
            catch (Exception er)
            {
                ClassLog.Write("AdminMkDv/ChangeCena-" + er);
                return View("Error");
            }
        }



        //------------------------------------------------контроллер главного слайдера--------------------------------------------------------
        [Authorize]
        [HttpGet]
        public ActionResult SliderMainMk()
        {
            try
            {
                SliderModel model = new SliderModel();
                model.SliderMImk = dataManager.SliderRepository.GetSliderMainImgMk();
                model.CountSlide = model.SliderMImk.Count();
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("AdminMk/SliderMainMk-" + er);
                return View("Error");
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult SliderMainMk(IEnumerable<HttpPostedFileBase> fileUpload = null)
        {
            try
            {
                SliderModel model = new SliderModel();
                string domainpath = Server.MapPath("~/Content/ImageTemp/");
                int i = 0;
                bool BreakAddFiles = false;
                int countImage = dataManager.SliderRepository.GetSliderMainImgMk().Count();
                if (countImage >= 8)//максимальное число слайдов = 8
                {
                    TempData["message"] = "Слайдер содержит 8 изображений, для добавления другого изображения необходимо удалить старые изображения.";
                    return RedirectToAction("SliderMainMk");
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
                            Bitmap myBitmap = new Bitmap(img, new Size(800, 220));
                            Graphics myGraphic = Graphics.FromImage(myBitmap);
                            //новое имя и сохраним
                            string newfilename = "evrostroySlMAin" + i.ToString() + ".jpg";
                            string newfilepath = domainpath + newfilename;
                            //изменим качество изображения(это новое от 12.08.2016)
                            Encoder myEncoder = Encoder.Quality;
                            EncoderParameters myEncoderParameters = new EncoderParameters(1);
                            // Save the bitmap as a JPG file with zero quality level compression.
                            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 40L);
                            myEncoderParameters.Param[0] = myEncoderParameter;
                            //вызов метода получения кодировки файла(из msdn)
                            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);


                            myBitmap.Save(newfilepath, jpgEncoder, myEncoderParameters);
                            //теперь запишем файл в базу данных
                            FileStream fs = null;
                            fs = new FileStream(newfilepath, FileMode.Open);
                            model.ImgDataSlider = new byte[fs.Length];
                            model.MimeTypeSlider = "image/jpg";
                            fs.Read(model.ImgDataSlider, 0, (int)fs.Length);
                            dataManager.SliderRepository.CreateSliderMainImgMk(model.MimeTypeSlider, model.ImgDataSlider);
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
                    return RedirectToAction("SliderMainMk");
                }
            }
            catch (Exception er)
            {
                ClassLog.Write("AdminMkDv/SliderMainMk-" + er);
                return View("Error");
            }
        }
        //-----------------------------------------------контроллер удаления слайда-------------------------------------------------------------------
        [Authorize]
        public ActionResult DellSlideMk(int id)
        {
            try
            {
                dataManager.SliderRepository.DellSliderMainImgMk(id);
                TempData["message"] = "Изображение удалено из базы данных!";
                return RedirectToAction("SliderMainMk");
            }
            catch (Exception er)
            {
                ClassLog.Write("AdminMkDv/DellSlideMk-" + er);
                return View("Error");
            }
        }
        //------------------------------------------------контроллер боковогослайдера--------------------------------------------------------
        [Authorize]
        [HttpGet]
        public ActionResult SliderLeftMk()
        {
            try
            {
                SliderModel model = new SliderModel();
                model.SliderLImk = dataManager.SliderRepository.GetSliderLeftImgMk();
                model.CountSlide = model.SliderLImk.Count();
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("AdminMkDv/SliderLeftMk-" + er);
                return View("Error");
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult SliderLeftMk(IEnumerable<HttpPostedFileBase> fileUpload = null)
        {
            try
            {
                SliderModel model = new SliderModel();
                string domainpath = Server.MapPath("~/Content/ImageTemp/");
                int i = 0;
                bool BreakAddFiles = false;
                int countImage = dataManager.SliderRepository.GetSliderLeftImgMk().Count();
                if (countImage >= 8)
                {
                    TempData["message"] = "Слайдер содержит 8 изображений или более, для добавления другого изображения необходимо удалить старые изображения.";
                    return RedirectToAction("SliderLeftMk");
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
                            //изменим качество изображения(это новое от 12.08.2016)
                            Encoder myEncoder = Encoder.Quality;
                            EncoderParameters myEncoderParameters = new EncoderParameters(1);
                            // Save the bitmap as a JPG file with zero quality level compression.
                            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 40L);
                            myEncoderParameters.Param[0] = myEncoderParameter;
                            //вызов метода получения кодировки файла(из msdn)
                            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);


                            myBitmap.Save(newfilepath, jpgEncoder, myEncoderParameters);
                            //теперь запишем файл в базу данных
                            FileStream fs = null;
                            fs = new FileStream(newfilepath, FileMode.Open);
                            model.ImgDataSlider = new byte[fs.Length];
                            model.MimeTypeSlider = "image/jpg";
                            fs.Read(model.ImgDataSlider, 0, (int)fs.Length);
                            dataManager.SliderRepository.CreateSliderLeftImgMk(model.MimeTypeSlider, model.ImgDataSlider);
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
                    return RedirectToAction("SliderLeftMk");
                }
            }
            catch (Exception er)
            {
                ClassLog.Write("AdminMkDv/SliderLeftMk-" + er);
                return View("Error");
            }
        }

        //========================================метод получения кодировки файла (из msdn)========================
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        //-----------------------------------------------контроллер удаления бокового слайда-------------------------------------------------------------------
        [Authorize]
        public ActionResult DellSlideLeftMk(int id)
        {
            try
            {
                dataManager.SliderRepository.DellSliderLeftImgMk(id);
                TempData["message"] = "Изображение удалено из базы данных!";
                return RedirectToAction("SliderLeftMk");
            }
            catch (Exception er)
            {
                ClassLog.Write("AdminMkDv/DellSlideLeftMk-" + er);
                return View("Error");
            }
        }
    }
}