using Domain2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dveri1.DopMethod;
using dveri1.Models;
using System.Drawing;
using System.IO;
using DALdv1;
using System.Drawing.Imaging;

namespace dveri1.Controllers
{
    public class OurWorksController : Controller
    {
        DataManager datamanager;
        public OurWorksController(DataManager datamanager)
        {
            this.datamanager = datamanager;
        }
        [HttpGet]
        [Authorize]
        public ActionResult AdminOurWorks()
        {
            try
            {
                ModelOurWorks model = new ModelOurWorks()
                {
                    ListOurWorks = datamanager.OurWorksRepository.GetOurWorks().OrderByDescending(x => x.Id)
                };

                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("OurWorksController/AdminOurWorks-" + er);
                return View("Error");
            }
        }
        //создание Наши работы
        [HttpGet]
        [Authorize]
        public ActionResult CreateOurWork(int id = 0)
        {
            try
            {
                ModelCreateOurWorks model = new ModelCreateOurWorks();
                if (id != 0)
                {
                    model.ListFoto = datamanager.OurWorksRepository.GetFotosWorkById(id);
                    model.IdWork = id;
                    model.Descriptwork = datamanager.OurWorksRepository.GetOurWorks().Where(x => x.Id == id).FirstOrDefault().TextOurWorks;
                }
                else
                    model.IdWork = 0;
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("OurWorksController/CreateOurWork-" + er);
                return View("Error");
            }
        }
        //создание Наши работы
        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateOurWork(ModelCreateOurWorks model, IEnumerable<HttpPostedFileBase> fileUpload = null)
        {
            try
            {
                if(model.IdWork!=0)
                model.ListFoto = datamanager.OurWorksRepository.GetFotosWorkById(model.IdWork);
              
                if (!ModelState.IsValid)
                {
                    TempData["message"] = "Заполните описание создаваемой работы!";
                    return View(model);
                }
                else
                {

                    //создаем текст описания нашей работы
                    datamanager.OurWorksRepository.CreateOurWork(model.IdWork, model.Descriptwork);
                    //метод изменения размера изображения и сохраненния в буфере
                    string domainpath = Server.MapPath("~/Content/ImageTemp/");
                    if (!Directory.Exists(domainpath))
                    {
                        Directory.CreateDirectory(domainpath);
                    }
                    //проход по списку загружаемых файлов и если есть, добавление в БД
                    foreach (var image in fileUpload)
                    {
                        int idfot = 0;

                        if (image != null)
                        {
                            string path = Path.Combine(domainpath, image.FileName);
                            image.SaveAs(path);

                            //изменим разрешение файла
                            Image img = Image.FromFile(path);
                            Bitmap myBitmap = new Bitmap(img);
                            Graphics myGraphic = Graphics.FromImage(myBitmap);
                            //изменим качество изображения(это новое от 12.08.2016)
                            Encoder myEncoder = Encoder.Quality;
                            EncoderParameters myEncoderParameters = new EncoderParameters(1);
                            // Save the bitmap as a JPG file with zero quality level compression.
                            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 20L);
                            myEncoderParameters.Param[0] = myEncoderParameter;
                            //вызов метода получения кодировки файла(из msdn)
                            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                            //новое имя и сохраним
                            string newfilename = "evrostroy" + (idfot + 1).ToString() + ".jpg";
                            string newfilepath = domainpath + newfilename;
                            idfot++;
                             myBitmap.Save(newfilepath, jpgEncoder, myEncoderParameters);
                            //теперь запишем файл в базу данных
                            FileStream fs = null;
                            fs = new FileStream(newfilepath, FileMode.Open);
                            byte[] ImageData = new byte[fs.Length];
                            string ImageMimeType = "image/jpg";
                            fs.Read(ImageData, 0, (int)fs.Length);
                            int idow = 0;
                            if (model.IdWork==0)
                            {
                                idow = datamanager.OurWorksRepository.GetOurWorks().OrderBy(x => x.Id).LastOrDefault().Id;
                                TempData["message"] = "Данные о новой работе добавлены!";
                            }
                           else
                            {
                                idow = model.IdWork;
                                TempData["message"] = "Данные о работе изменены!";
                            }
                            datamanager.OurWorksRepository.CreateFotoOurWork(0, idow, ImageMimeType, ImageData);
                            fs.Close();
                            //освобождаем занятый ресурс
                            myBitmap.Dispose();
                            myGraphic.Dispose();
                            img.Dispose();
                            //imgLogo.Dispose();
                            myBitmap = null;
                            myGraphic = null;
                            img = null;
                            //imgLogo = null;
                            fs = null;
                        }
                    }
                    fileUpload = null;
                    //удалим файлы из временной папки
                    string dompath = Server.MapPath("~/Content/ImageTemp/");
                    DellFilesFromDomain.DellAllFiles(dompath);
                  
                    return RedirectToAction("AdminOurWorks");
                }
            }
            catch (Exception er)
            {
                ClassLog.Write("OurWorksController/CreateOurWork-" + er);
                return View("Error");
            }
        }
        [Authorize]
        public ActionResult DellOurWork(int id)
        {
            try
            {
                datamanager.OurWorksRepository.DellOurWork(id);
                TempData["message"] = "Данные удалены из базы данных!";
                return RedirectToAction("AdminOurWorks");
            }
            catch (Exception er)
            {
                ClassLog.Write("OurWorksController/DellOurWork-" + er);
                return View("Error");
            }
        }
        [Authorize]
        public ActionResult DellFotoOurWork(int idf, bool create, int idow)
        {
            try
            {
               
                datamanager.OurWorksRepository.DellFotoOurWork(idf);
                TempData["message"] = "Данные удалены из базы данных!";
                if (create)
                {
                    return RedirectToAction("CreateOurWork",new { id = idow });
                }
                    return RedirectToAction("AdminOurWorks");
            }
            catch (Exception er)
            {
                ClassLog.Write("OurWorksController/DellFotoOurWork-" + er);
                return View("Error");
            }
        }

        //метод получения кодировки файла (из msdn)
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
        int PageSize = 10;
        [HttpGet]
        //метод вывода на главную страничку Наши работы
        public ActionResult OurWorksAll(int page=1)
        {
            try
            {
                ModelOurWorks model = new ModelOurWorks();
                IEnumerable<TableOurWork> two = datamanager.OurWorksRepository.GetOurWorks();
                model.ListOurWorks = two.OrderByDescending(x => x.Id).Skip(PageSize * (page - 1)).Take(PageSize);
                SeoMain seo = datamanager.SeoMainRepository.GetSeoMainByPage("Наши работы");
                model.SeoTitle = seo.Title != null ? seo.Title : "Лучшие работы нашей компании";
                model.SeoKey = seo.Keywords;
                model.SeoHead = seo.Header != null ? seo.Header : "Наши работы";
                model.SeoDesc = seo.Description;
                model.PageInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    TotalItems = two.Count(),
                    ItemsPerPage = PageSize
                };
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("OurWorksController/OurWorksAll-" + er);
                return View("Error");
            }

        }
    }
}