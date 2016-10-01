using DALdv1;
using Domain2;
using dveri1.DopMethod;
using dveri1.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dveri1.Controllers
{
    public class AdminColorController : Controller
    {
        DataManager dataManager;
        public AdminColorController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        [Authorize]
        [HttpGet]
        public ActionResult ColorAct()
        {
            try
            {
                ModelColors model = new ModelColors();
                model.ColorItems = dataManager.ColorsRepository.GetColors();
                model.CountItemColor = model.ColorItems.Count();
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("AdminColor/ColorAct-" + er);
                return View("Error");
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult ColorAct(IEnumerable<HttpPostedFileBase> fileUpload = null)
        {
            try
            {
                ModelColors model = new ModelColors();
                string domainpath = Server.MapPath("~/Content/ImageTemp/");
                int i = 0;
                bool BreakAddFiles = false;
                int countImage = dataManager.ColorsRepository.GetColors().Count();
                if (countImage >= 12)
                {
                    TempData["message"] = "Превышен лимит количества цветов, для добавления другого изображения необходимо удалить старые изображения.";
                    return RedirectToAction("ColorAct");
                }
                else
                {
                    foreach (var image in fileUpload)
                    {
                        if (image != null)
                        {
                            if (countImage == 12)
                            {
                                TempData["message"] = "Базаданных содержит 12 изображений, добавлено только " + i + " изображения";
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
                            Bitmap myBitmap = new Bitmap(img, new Size(48, 48));
                            Graphics myGraphic = Graphics.FromImage(myBitmap);
                            //теперь нарисуем логотип
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
                            model.ImgData = new byte[fs.Length];
                            model.MimeTypeColor = "image/jpg";
                            model.NameColor = "новый цвет";
                            fs.Read(model.ImgData, 0, (int)fs.Length);
                            dataManager.ColorsRepository.CreateColor(0,model.NameColor,model.MimeTypeColor, model.ImgData);
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
                        TempData["message"] = "Изображения цвета добавлены!";
                    }
                    return RedirectToAction("ColorAct");
                }
            }
            catch (Exception er)
            {
                ClassLog.Write("AdminColor/ColorAct-" + er);
                return View("Error");
            }
        }
        //-----------------------------------------------контроллер удаления слайда-------------------------------------------------------------------
        [Authorize]
        public ActionResult DellColor(int id)
        {
            try
            {
                dataManager.ColorsRepository.DelColor(id);
                TempData["message"] = "Изображение удалено из базы данных!";
                return RedirectToAction("ColorAct");
            }
            catch (Exception er)
            {
                ClassLog.Write("AdminAct/DellColor-" + er);
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
        //контроллер изменения (добавления) названия изображению цвета
        //контроллер добавления изменения и отображения ссылки на изображение слайдера

        [Authorize]
        [HttpPost]
        public ActionResult AddNameColor(int IdColor, string NameColor)
        {
            try
            {
                if (NameColor != null && NameColor != "")
                {
                    TableColor tc = dataManager.ColorsRepository.GetColor(IdColor);
                    dataManager.ColorsRepository.CreateColor(IdColor, NameColor, tc.MimeType, tc.Image);
                    TempData["message"] = "Название цвета изменено!";
                }
                else
                {
                    TempData["message"] = "Заполните поле для добавление названия цвета!";
                }
                return RedirectToAction("ColorAct");
            }
            catch (Exception er)
            {
                ClassLog.Write("AdminColor/AddNameColor-" + er);
                return View("Error");
            }
        }



    }
}