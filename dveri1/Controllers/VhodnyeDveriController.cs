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
        public ActionResult VhodnyeDveriIndex(int? id, int sort = 0, string brand = "весьтовар")
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
                    SeoMain s = dataManager.SeoMainRepository.GetSeoMainByPage("Входные двери");
                    if (s != null)
                    {
                        model.SeoTitle = s.Title;
                        model.SeoKey = s.Keywords;
                        model.SeoDesc = s.Description;
                        if (s.Header != null)
                        {
                            model.SeoHead = s.Header;
                        }
                        else
                        {
                            model.SeoHead = "Входные двери от лучших производителей! Вы здесь найдете как металлические двери так и стальные, как дешевые двери так и качественные двери!";
                        }
                    }
                    else
                    {
                        model.SeoTitle = "Купить входные металлические двери в Минске с бесплатной доставкой";
                        model.SeoHead = "Входные двери от лучших производителей! Вы здесь найдете как металлические двери так и стальные, как дешевые двери так и качественные двери!";
                    }
                }
                else
                {
                    TotalItemsProduct = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(x => x.Publicaciya == true && x.Proizvoditel == brand).Count();
                    SeoMain s = dataManager.SeoMainRepository.GetSeoMainByPage(brand);
                    if (s != null)
                    {
                        model.SeoTitle = s.Title;
                        model.SeoKey = s.Keywords;
                        model.SeoDesc = s.Description;
                        if (s.Header != null)
                        {
                            model.SeoHead = s.Header;
                        }
                        else
                        {
                            model.SeoHead = "Входные двери от производителя " + brand + "! Вы здесь найдете как металлические двери так и стальные, как дешевые двери так и качественные двери!";
                        }
                    }
                    else
                    {
                        model.SeoTitle = "Купить входные двери фирмы " + brand;
                        model.SeoHead = "Входные двери от производителя " + brand + "! Вы здесь найдете как металлические двери так и стальные, как дешевые двери так и качественные двери!";
                    }
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
                ClassLog.Write("VhodnyeDveri/VhodnyeDveriIndex-" + er);
                return View("Error");
            }
        }
        //   метод загрузки первого изображения в списке товара
        public FileContentResult GetImage(int id)
        {
            try
            {
                FotoVhodnyhDverey foto = dataManager.VhodnyeDvRepository.GetFotoVhDvByID(id).OrderBy(x => x.Idfoto).FirstOrDefault();
                if (foto != null)
                { return File(foto.Imaging, foto.MimeType); }
                else
                {//   изображение по умолчанию
                    return null;
                }
            }
            catch (Exception er)
            {
                ClassLog.Write("VhodnyeDveri/GetImage-" + er);
                return null;
            }
        }

        [HttpGet]
        public ActionResult ProductList(int page = 1, int sort = 0, string brand = "весьтовар")
        {
            try
            {
                ForMainModel model = new ForMainModel();
                model.CurrentBrand = brand;
                int TotalItemsProduct;
                if (brand == "весьтовар")
                {
                    TotalItemsProduct = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(x => x.Publicaciya == true).Count();
                }
                else
                {
                    TotalItemsProduct = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(x => x.Publicaciya == true && x.Proizvoditel == brand).Count();
                }

                model.ListVhodnDv = SortirDveri(page, sort, brand);
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
                ClassLog.Write("VhodnyeDveri/ProductList-" + er);
                return View("Error");
            }
        }
        //   метод возврата списка отсортированных дверей
        public IEnumerable<VhodnyeDveri> SortirDveri(int page, int s, string br)
        {
            try
            {
                IEnumerable<VhodnyeDveri> temp = null;
                if (br != "весьтовар")
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
                ClassLog.Write("VhodnyeDveri/SortirDveri-" + er);
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
                ClassLog.Write("VhodnyeDveri/ContactOnPanel-" + er);
                return View("Error");
            }
        }
        public ActionResult ContactOnFoter(bool contact = true)
        {
            try
            {
                ViewBag.Count = 0;
                ViewBag.Count1 = 0;
                if (contact)
                {
                    ViewBag.Contact = true;
                }
                else
                    ViewBag.Contact = false;
                ContactModel model = new ContactModel();
                model.ContactList = dataManager.ContactRepository.GetContacts();
                return PartialView(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("VhodnyeDveri/ContactOnFooter-" + er);
                return View("Error");
            }
        }



        //==========================================карточка товара==============================================================================
        [HttpGet]
        public ActionResult TovarPage(int id)
        {
            try
            {
                KartochkaTovaraModel model = new KartochkaTovaraModel();
                model.Tovar = dataManager.VhodnyeDvRepository.GetVhodnyeDvById(id);
                model.FotoTovara = dataManager.VhodnyeDvRepository.GetFotoVhDvByID(id);
                //отбираем коментарии к товару по ID с указанной публикацией, сортироуем по дате (сначала последние(свежие), и отбираем 1-ые 100 комментов)
                model.CommentVhDvList = dataManager.CommentRepository.GetCommentVhDv().Where(x => x.IDdv == id && x.Public == true).OrderByDescending(z => z.Date).Take(100);
                OplDostModel md = new OplDostModel();
                md = InfaDostOplata();
                model.InfoDostavka = md.DostInfo;
                model.InfoOplata = md.OplInfo;
                SeoVhodnuhDverei se = dataManager.VhodnyeDvRepository.GetSeoVhDv().Where(x => x.Id == id).FirstOrDefault();
                if (se != null)
                {
                    if (se.TitleDveri != null && se.TitleDveri != "")
                    {
                        model.TitleD = se.TitleDveri;
                    }
                    else
                    {
                        model.TitleD = "купить " + model.Tovar.Nazvanie;
                    }
                    model.KeyD = se.KeywordsDveri;
                    model.DescrD = se.DescriptionDveri;
                }
                else
                {
                    model.TitleD = "купить " + model.Tovar.Nazvanie;
                }

                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("VhodnyeDveri/ TovarPage-" + er);
                return View("Error");
            }
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
                ClassLog.Write("VhodnyeDveri/ImgTov-" + er);
                return View("Error");
            }
        }
        //---------------------------------------------------------------хлебные крошки----------------------------------------------
        public ActionResult BreadCrumbs(ForMainModel mainmod = null, KartochkaTovaraModel kmod = null, string namepart = null, string nameart = null)
        {
            try
            {
                ModelBreadCrumbs mod = new ModelBreadCrumbs();
                if (namepart != null)
                {
                    mod.NamePartSite = namepart;
                    if (nameart != null)
                    {
                        mod.NameProduct = nameart;
                    }
                }
                if (mainmod.Brand != null && mainmod.Brand.First() != "весьтовар")
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
                ClassLog.Write("VhodnyeDveri/BreadCrumbs-" + er);
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
                ClassLog.Write("VhodnyeDveri/BuyDveri-" + er);
                return View("Error");
            }

        }
        [HttpPost]
        public ActionResult BuyDveri(ModelBuyDveri model)
        {
            try
            {
                if (ModelState.IsValid)
                {//вызов метода отправки сообщегия админу
                    if (SendMessageAboutBuyDoor(model))
                    {
                        TempData["messageklient"] = "Ваше сообщение отправлено, наш менеджер свяжется с Вам в течении 15 минут!";
                    }
                    else
                    {
                        TempData["messageklient"] = "При отправке сообщения возникла ошибка, сообщите пожалуйста нашему менеджеру по указанному выше номеру телефона!";
                    }
                    return RedirectToAction("TovarPage", new { id = model.IdDveri });
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception er)
            {
                ClassLog.Write("VhodnyeDveri/BuyDveri-" + er);
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
            string them = "Cообщение от клиента: " + model.KlientName + ", тел. или email: " + model.KlientPhone;
            string adres = dataManager.ContactRepository.GetContacts().Where(x => x.TypeSv == "e-mail").FirstOrDefault().NumberSv;
            //проверяем если нет такой двери
            if (model.IdDveri != 9999)
            {
                VhodnyeDveri vhd = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Where(x => x.Id == model.IdDveri).FirstOrDefault();
                string cena, cenasoskidkoy;
                if (vhd.Cena == null)
                {
                    cena = "Не установлена";
                }
                else
                {
                    cena = vhd.Cena.Value.ToString("C");
                }
                if (vhd.CenaSoSkidcoy == null)
                {
                    cenasoskidkoy = "Скидки нет";
                }
                else
                {
                    cenasoskidkoy = vhd.CenaSoSkidcoy.Value.ToString("C");
                }

                body = "Текст сообщения: " + model.KlientQuestion + "\r\n" + "тип товара: " + model.Type + "\r\n" + "код товара: " + model.IdDveri + "\r\n" + "наименование товара: " + vhd.Nazvanie + "\r\n" + "фирма производитель: " + vhd.Proizvoditel +
                "\r\n" + "Цена: " + cena + "\r\n" + "Скидка: " + vhd.Skidka + "\r\n" + "Цена со скидкой: " + cenasoskidkoy;
            }
            else
            {
                body = "Свяжитесь с клиентом по тел. или e-email: " + model.KlientPhone;
            }
            if (SendMsg.Message(body, them, adres))
            { return true; }
            else
            { return false; }
        }


        //===========================для модального окна покупки=====================
        [HttpGet]
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
                ClassLog.Write("VhodnyeDveri/BuyDveriModal-" + er);
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
                    if (SendMessageAboutBuyDoor(model))
                    {
                        return Json(model.KlientName + "! Вы сделали заказ на покупку товара: " + model.DvName + ". Наш менеджер свяжется с Вами в течении 15 минут! Спасибо, что выбрали наш магазин!", JsonRequestBehavior.AllowGet);
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
                ClassLog.Write("VhodnyeDveri/BuyDveriModel-" + er);
                return View("Error");
            }
        }
        //быстрая покупка, только для ввода номера телефона или просто перезвоните мне
        [HttpGet]
        public ActionResult FastBuyDveri(int id, string type)
        {
            try
            {
                ModelBuyDveri model = new ModelBuyDveri();
                model.IdDveri = id;
                model.Type = type;
                //9999 взято как исключающее ID товара, т.е. такого нет и не будет
                if (id == 9999)
                {
                    model.KlientName = "Перезвоните мне";
                }
                else
                {
                    model.KlientName = "Быстрая покупка";
                    if (type == "входные двери")
                    {
                        model.DvName = dataManager.VhodnyeDvRepository.GetVhodnyeDvById(id).Nazvanie;
                    }
                }

                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("VhodnyeDveri/BuyDveriModal-" + er);
                return View("Error");
            }
        }
        [HttpPost]
        public ActionResult FastBuyDveri(ModelBuyDveri model)
        {
            try
            {
                if (ModelState.IsValid)
                {//вызов метода отправки сообщегия админу
                    if (SendMessageAboutBuyDoor(model))
                    {
                        TempData["messageklient"] = "Ваше сообщение отправлено, наш менеджер свяжется с Вами в течении 15 минут!";
                    }
                    else
                    {
                        TempData["messageklient"] = "При отправке сообщения возникла ошибка, сообщите пожалуйста нашему менеджеру по указанному выше номеру телефона!";
                    }
                    if (model.IdDveri == 9999)
                    {
                        return RedirectToAction("VhodnyeDveriIndex");
                    }
                    else
                    {
                        return RedirectToAction("TovarPage", new { id = model.IdDveri });
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception er)
            {
                ClassLog.Write("VhodnyeDveri/FastBuyDveri-" + er);
                return View("Error");
            }
        }
        //для модального окна быстрой покупки
        //===========================для модального окна покупки=====================
        [HttpGet]
        public ActionResult FastBuyDveriModal(int id, string type)
        {
            try
            {
                ModelBuyDveri model = new ModelBuyDveri();
                model.IdDveri = id;
                model.Type = type;
                if (id == 9999)
                {
                    model.KlientName = "Перезвоните мне";
                }
                else
                {
                    model.KlientName = "Быстрая покупка";
                    if (type == "входные двери")
                    {
                        model.DvName = dataManager.VhodnyeDvRepository.GetVhodnyeDvById(id).Nazvanie;
                    }
                }

                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("VhodnyeDveri/FastBuyDveriModal-" + er);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult FastBuyDveriModal(ModelBuyDveri model)
        {
            try
            {
                if (ModelState.IsValid)
                {//вызов метода отправки сообщения админу
                    if (SendMessageAboutBuyDoor(model))
                    {
                        return Json("Наш менеджер свяжется с Вами в течении 15 минут! Спасибо, что выбрали наш магазин!", JsonRequestBehavior.AllowGet);
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
                ClassLog.Write("VhodnyeDveri/FastBuyDveriModel-" + er);
                return View("Error");
            }
        }
        //оплата и доставка
        public ActionResult OplataDostavka()
        {
            try
            {
                OplDostModel model = new OplDostModel();
                model = InfaDostOplata();
                SeoMain s = dataManager.SeoMainRepository.GetSeoMainByPage("Доставка и оплата");
                if (s != null)
                {
                    model.SeoTitle = s.Title;
                    model.SeoKey = s.Keywords;
                    model.SeoDesc = s.Description;
                    if (s.Header != null)
                    {
                        model.SeoHead = s.Header;
                    }
                    else
                    {
                        model.SeoHead = "Информация о доставке товаров и об оплате";
                    }
                }
                else
                {
                    model.SeoTitle = "Оплата и доставка входных дверей в Минске компанией Люксеврострой";
                    model.SeoHead = "Информация о доставке товаров и об оплате";
                }
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("VhodnyeDveri/FastBuyDveriModel-" + er);
                return View("Error");
            }
        }
        //метод определения наличия информации о доставке и оплате
        private OplDostModel InfaDostOplata()
        {
            OplDostModel model = new OplDostModel();
            Oplata op = dataManager.OplDostRepository.GetOplata().FirstOrDefault();
            if (op != null)
            {
                model.OplInfo = op.Oplata1;
            }
            else
            {
                model.OplInfo = "нет информации об оплате";
            }
            Dostavka dos = dataManager.OplDostRepository.GetDostavka().FirstOrDefault();
            if (dos != null)
            {
                model.DostInfo = dos.Dostavka1;
            }
            else
            {
                model.DostInfo = "нет информации о доставке";
            }
            return model;
        }
        //для загрузки похожих товаров на странице карточка товара
        public ActionResult SimilarGoods()
        {
            KaruselTovara model = new KaruselTovara();
            //получим рандомное число
            int maxgoods = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Count();
            Random rand = new Random();
            int randomize = rand.Next(1, maxgoods-25);

            //пропустим рандомное число товаров и выберем следующие 24;
            model.ListVhodnDv = dataManager.VhodnyeDvRepository.GetVhodnyeDv().Skip(randomize).Take(24);
            return View(model);
        }

    }
}