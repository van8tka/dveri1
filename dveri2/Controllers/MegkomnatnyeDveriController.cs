using DALdv1;
using Domain2;
using dveri2.DopMethod;
using dveri2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dveri2.Controllers
{
    public class MegkomnatnyeDveriController : Controller
    {

        private DataManager dataManager;
        public MegkomnatnyeDveriController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        //сортировка
        //    int sort 
        //    1-по возрастанию,
        //    2-по убыванию,
        //    0-по номеру(по умолчанию)

        public int PageSize = 32;
        const decimal cF1 = 50;
        const decimal cF2 = 120;
        const decimal cS2 = 200;
        const decimal cTh2 = 300;
        const decimal cFo2 = 400;
        const string cena1 = "50-120р.";
        const string cena2 = "120-200р.";
        const string cena3 = "200-300р.";
        const string cena4 = "300-400р.";
        const string cena5 = "больше 400р.";

        //отображение списка и баннера (главная страница)
        [HttpGet]

        public ActionResult DveriMegkomnatnye(int page = 1, int sort = 0, List<string> firma = null, List<string> country = null, List<string> color = null, List<string> typdv = null, List<string> material = null, List < string> cena = null)
        {
            ForMainModel model = new ForMainModel();
            model.SliderImgMk = dataManager.SliderRepository.GetSliderMainImgMk();
            model.CountFile = model.SliderImgMk.Count();
            GetIemsForFilter(model);
            //отбираем  двери по фильтру

            //выбор чекнутых фирм производителей 
            //метод фильтровки данных

            IEnumerable<MegkomnatnyeDveri> vhd = GetFilter(page, model, firma, country, color, typdv,material , cena);


            //заполняем модель товаров
            switch (sort)
            {
                case 0:
                    {
                        model.ListMkDv = vhd.Where(x => x.Publicaciya == true).OrderBy(x => x.Id).Skip((page - 1) * PageSize).Take(PageSize);
                        break;
                    }
                case 1:
                    {
                        model.ListMkDv = vhd.Where(x => x.Publicaciya == true).OrderBy(x => x.Cena).Skip((page - 1) * PageSize).Take(PageSize);
                        break;
                    }
                case 2:
                    {
                        model.ListMkDv = vhd.Where(x => x.Publicaciya == true).OrderByDescending(x => x.Cena).Skip((page - 1) * PageSize).Take(PageSize);
                        break;
                    }
                default: break;
            }

            model.Sort = sort;
            //сео
            SeoMain s = dataManager.SeoMainRepository.GetSeoMainByPage("Межкомнатные двери");
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
                    model.SeoHead = "Межкомнатные двери от лучших производителей! Вы здесь найдете межкомнатные двери, которые удовлетворят Вашим требованиям!";
                }
            }
            else
            {
                model.SeoTitle = "Купить межкомнатные двери в Минске с бесплатной доставкой";
                model.SeoHead = "Межкомнатные двери от лучших производителей! Вы здесь найдете межкомнатные двери, которые удовлетворят Вашим требованиям!";
            }


            return View(model);
        }


        private IEnumerable<MegkomnatnyeDveri> GetFilter(int page, ForMainModel model, List<string> firma, List<string> country, List<string> color, List<string> typdv,List<string> material, List<string> cena)
        {
            IEnumerable<MegkomnatnyeDveri> vhd = dataManager.MegkomDvRepository.GetMkDv();
            if (firma != null || TempData["firma"] != null)
            {
                if (firma == null)
                {
                    firma = (List<string>)TempData["firma"];
                }
                IEnumerable<MegkomnatnyeDveri> temp1 = null;
                IEnumerable<MegkomnatnyeDveri> temp2 = null;
                bool a = true;//переменная для приравнивания temp1 и temp2
                foreach (string i in firma)//проход по списку checkнутых элементов списка меню
                {

                    temp1 = vhd.Where(x => x.Proizvoditel == i.Replace("/", " "));//отбираем с каждоко элемента
                    if (a)
                    {
                        temp2 = temp1;
                        a = false;
                    }
                    else
                    {
                        temp2 = temp2.Union<MegkomnatnyeDveri>(temp1);//объединяем интерфейсы
                    }

                }
                vhd = temp2;//присваиваем выходному интерфейсу   
                model.CurrentBrand = firma;
                TempData["firma"] = firma;

            }
            else
            {
                model.CurrentBrand = null;
                TempData["firma"] = null;
            }
            //выбор чекнутых стран
            if (country != null || TempData["country"] != null)
            {
                if (country == null)
                    country = (List<string>)TempData["country"];
                IEnumerable<MegkomnatnyeDveri> temp1 = null;
                IEnumerable<MegkomnatnyeDveri> temp2 = null;
                bool a = true;//переменная для приравнивания temp1 и temp2
                foreach (string i in country)//проход по списку checkнутых элементов списка меню
                {
                    temp1 = vhd.Where(x => x.Strana == i.Replace("/", " "));//отбираем с каждоко элемента
                    if (a)
                    {
                        temp2 = temp1;
                        a = false;
                    }
                    else
                    {
                        temp2 = temp2.Union<MegkomnatnyeDveri>(temp1);//объединяем интерфейсы
                    }
                }
                vhd = temp2;//присваиваем выходному интерфейсу   
                model.CurrentCountry = country;
                TempData["country"] = country;
            }
            else
            {
                model.CurrentCountry = null;
                TempData["country"] = null;
            }

            //проверка чекнутого цвета
            if (color != null || TempData["color"] != null)
            {
                if (color == null)
                    color = (List<string>)TempData["color"];
                IEnumerable<MegkomnatnyeDveri> temp1 = null;
                IEnumerable<MegkomnatnyeDveri> temp2 = null;
                bool a = true;//переменная для приравнивания temp1 и temp2
                foreach (string i in color)//проход по списку checkнутых элементов списка меню
                {
                    temp1 = vhd.Where(x => x.IdColor == int.Parse(i));//отбираем с каждоко элемента
                    if (a)
                    {
                        temp2 = temp1;
                        a = false;
                    }
                    else
                    {
                        if (temp1.Count() != 0)
                            temp2 = temp2.Union<MegkomnatnyeDveri>(temp1);//объединяем интерфейсы
                    }
                }
                vhd = temp2;//присваиваем выходному интерфейсу   
                model.CurrentColor = color;
                TempData["color"] = color;
            }
            else
            {
                model.CurrentColor = null;
                TempData["color"] = null;
            }

            //проверка чекнутого материала
            if (material != null || TempData["material"] != null)
            {
                if (material == null)
                    material = (List<string>)TempData["material"];
                IEnumerable<MegkomnatnyeDveri> temp1 = null;
                IEnumerable<MegkomnatnyeDveri> temp2 = null;
                bool a = true;//переменная для приравнивания temp1 и temp2
                foreach (string i in material)//проход по списку checkнутых элементов списка меню
                {

                    temp1 = vhd.Where(x => x.Material == i.Replace("/", " "));//отбираем с каждоко элемента
                    if (a)
                    {
                        temp2 = temp1;
                        a = false;
                    }
                    else
                    {
                        temp2 = temp2.Union<MegkomnatnyeDveri>(temp1);//объединяем интерфейсы
                    }
                }
                vhd = temp2;//присваиваем выходному интерфейсу   
                model.CurrentMaterial = material;
                TempData["material"] = material;
            }
            else
            {
                model.CurrentMaterial = null;
                TempData["material"] = null;
            }

            //проверка чекнутого типа двери
            if (typdv != null || TempData["typdv"] != null)
            {
                if (typdv == null)
                    typdv = (List<string>)TempData["typdv"];
                IEnumerable<MegkomnatnyeDveri> temp1 = null;
                IEnumerable<MegkomnatnyeDveri> temp2 = null;
                bool a = true;//переменная для приравнивания temp1 и temp2
                foreach (string i in typdv)//проход по списку checkнутых элементов списка меню
                {

                    temp1 = vhd.Where(x => x.TypDveri == i.Replace("/", " "));//отбираем с каждоко элемента
                    if (a)
                    {
                        temp2 = temp1;
                        a = false;
                    }
                    else
                    {
                        temp2 = temp2.Union<MegkomnatnyeDveri>(temp1);//объединяем интерфейсы
                    }
                }
                vhd = temp2;//присваиваем выходному интерфейсу   
                model.CurrentTypDv = typdv;
                TempData["typdv"] = typdv;
            }
            else
            {
                model.CurrentTypDv = null;
                TempData["typdv"] = null;
            }

            //проверяем цену товара
            if (cena != null || TempData["cena"] != null)
            {
                if (cena == null)
                    cena = (List<string>)TempData["cena"];

                IEnumerable<MegkomnatnyeDveri> temp1 = null;
                IEnumerable<MegkomnatnyeDveri> temp2 = null;
                bool a = true;//переменная для приравнивания temp1 и temp2
                foreach (string i in cena)//проход по списку checkнутых элементов списка меню
                {

                    //TempData.Add(i, "cena");
                    switch (i.Replace("/", " "))
                    {
                        case cena1:
                            {
                                temp1 = vhd.Where(x => x.Cena > cF1 && x.Cena <= cF2);//отбираем с каждоко элемента
                                break;
                            }
                        case cena2:
                            {
                                temp1 = vhd.Where(x => x.Cena > cF2 && x.Cena <= cS2);//отбираем с каждоко элемента
                                break;
                            }
                        case cena3:
                            {
                                temp1 = vhd.Where(x => x.Cena > cS2 && x.Cena <= cTh2);//отбираем с каждоко элемента
                                break;
                            }
                        case cena4:
                            {
                                temp1 = vhd.Where(x => x.Cena > cTh2 && x.Cena <= cFo2);//отбираем с каждоко элемента
                                break;
                            }
                        case cena5:
                            {
                                temp1 = vhd.Where(x => x.Cena > cFo2);//отбираем с каждоко элемента
                                break;
                            }
                    }

                    if (a)
                    {
                        temp2 = temp1;
                        a = false;
                    }
                    else
                    {
                        temp2 = temp2.Union<MegkomnatnyeDveri>(temp1);//объединяем интерфейсы
                    }
                }
                vhd = temp2;//присваиваем выходному интерфейсу   
                model.CurrentCost = cena;
                TempData["cena"] = cena;
            }
            else
            {
                model.CurrentCost = null;
                TempData["cena"] = null;
            }

            //для страницы
            model.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                TotalItems = vhd.Where(x => x.Publicaciya == true).Count(),
                ItemsPerPage = PageSize
            };

            return vhd;
        }

        private void GetIemsForFilter(ForMainModel model)
        {
            model.Brand = dataManager.MegkomDvRepository.GetMkDv().Select(z => z.Proizvoditel).Distinct().OrderBy(z => z);
            model.Country = dataManager.MegkomDvRepository.GetMkDv().Select(z => z.Strana).Distinct().OrderBy(z => z);
            model.Color = dataManager.ColorsRepository.GetColors();
            model.TypDv = dataManager.MegkomDvRepository.GetMkDv().Select(z => z.TypDveri).Distinct().OrderBy(z => z);
            model.Cost = new List<string> { cena1, cena2, cena3, cena4, cena5 };
            model.Material = dataManager.MegkomDvRepository.GetMkDv().Select(z => z.Material).Distinct().OrderBy(z => z);
        }

        [HttpPost]
        public ActionResult DveriMegkomnatnye(List<string> firma = null, List<string> country = null, List<string> color = null, List<string> typdv = null, List<string> material = null, List<string> cena = null)
        {
            TempData["firma"] = firma;
            TempData["country"] = country;
            TempData["color"] = color;
            TempData["typdv"] = typdv;
            TempData["cena"] = cena;
            TempData["material"] = material;

            ForMainModel model = new ForMainModel();
            model.SliderImgMk = dataManager.SliderRepository.GetSliderMainImgMk();
            model.CountFile = model.SliderImgMk.Count();
            GetIemsForFilter(model);
            //отбираем  двери по фильтру
            //метод фильтровки данных
            IEnumerable<MegkomnatnyeDveri> vhd = GetFilter(1, model, firma, country, color, typdv,material, cena);

            //заполняем модель товаров
            model.ListMkDv = vhd.Where(x => x.Publicaciya == true).OrderBy(x => x.Id).Skip((1 - 1) * PageSize).Take(PageSize);
            //сео
            SeoMain s = dataManager.SeoMainRepository.GetSeoMainByPage("Межкомнатные двери");
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
                    model.SeoHead = "Межкомнатные двери от лучших производителей! Вы здесь найдете межкомнатные двери, которые удовлетворят Вашим требованиям!";
                }
            }
            else
            {
                model.SeoTitle = "Купить межкомнатные двери в Минске с бесплатной доставкой";
                model.SeoHead = "Межкомнатные двери от лучших производителей! Вы здесь найдете межкомнатные двери, которые удовлетворят Вашим требованиям!";
            }
           
            return View(model);
        }









        //public ActionResult MegkomnatnyeDveriIndex(string brand = "весьтовар", int? id = 0, int sort = 0, bool flagMaterial = false)
        //{
        //    try
        //    {
        //        int page = id ?? 1;
        //        ForMainModel model = new ForMainModel();
        //        model.SliderImgMk = dataManager.SliderRepository.GetSliderMainImgMk();
        //        model.CountFile = model.SliderImgMk.Count();

        //        model.Sort = sort;
        //        int TotalItemsProduct;
        //        if (brand == "весьтовар")
        //        {
        //            TotalItemsProduct = dataManager.MegkomDvRepository.GetMkDv().Where(x => x.Publicaciya == true).Count();
        //            SeoMain s = dataManager.SeoMainRepository.GetSeoMainByPage("Межкомнатные двери");
        //            if (s != null)
        //            {
        //                model.SeoTitle = s.Title;
        //                model.SeoKey = s.Keywords;
        //                model.SeoDesc = s.Description;
        //                if (s.Header != null)
        //                {
        //                    model.SeoHead = s.Header;
        //                }
        //                else
        //                {
        //                    model.SeoHead = "Межкомнатные двери от лучших производителей! Вы здесь найдете лучшие двери на Ваш вкус, как дешевые двери так и качественные двери, межкомнатные двери из различных материалов!";
        //                }
        //            }
        //            else
        //            {
        //                model.SeoTitle = "Купить межкомнатные двери в Минске с бесплатной доставкой";
        //                model.SeoHead = "Межкомнатные двери от лучших производителей! Вы здесь найдете лучшие двери на Ваш вкус, как дешевые двери так и качественные двери, межкомнатные двери из различных материалов!";
        //            }
        //        }
        //        else
        //        {
        //            if(!flagMaterial)
        //            {
        //                TotalItemsProduct = dataManager.MegkomDvRepository.GetMkDv().Where(x => x.Publicaciya == true && x.Proizvoditel == brand).Count();
        //                SeoMain s = dataManager.SeoMainRepository.GetSeoMain().Where(x=>x.Page == brand && x.Category == "Производитель межкомнатных дверей").FirstOrDefault();
        //                if (s != null)
        //                {
        //                    model.SeoTitle = s.Title;
        //                    model.SeoKey = s.Keywords;
        //                    model.SeoDesc = s.Description;
        //                    if (s.Header != null)
        //                    {
        //                        model.SeoHead = s.Header;
        //                    }
        //                    else
        //                    {
        //                        model.SeoHead = "Межкомнатные двери от производителя " + brand + "! Вы здесь найдете межкомнатные двери на Ваш вкус, как дешевые двери так и качественные двери!";
        //                    }
        //                }
        //                else
        //                {
        //                    model.SeoTitle = "Купить межкомнатные двери фирмы " + brand;
        //                    model.SeoHead = "Межкомнатные двери от производителя " + brand + "! Вы здесь найдете межкомнатные двери на Ваш вкус, как дешевые двери так и качественные двери!";
        //                }
        //            }
        //            else//если выбраны мл двери по материалу
        //            {
        //                TotalItemsProduct = dataManager.MegkomDvRepository.GetMkDv().Where(x => x.Publicaciya == true && x.Material == brand).Count();
        //                SeoMain s = dataManager.SeoMainRepository.GetSeoMain().Where(x => x.Page == brand && x.Category == "Материал межкомнатных дверей").FirstOrDefault();
        //                if (s != null)
        //                {
        //                    model.SeoTitle = s.Title;
        //                    model.SeoKey = s.Keywords;
        //                    model.SeoDesc = s.Description;
        //                    if (s.Header != null)
        //                    {
        //                        model.SeoHead = s.Header;
        //                    }
        //                    else
        //                    {
        //                        model.SeoHead = "Межкомнатные двери из " + brand + "! Вы здесь найдете межкомнатные двери на Ваш вкус, как дешевые двери так и качественные двери!";
        //                    }
        //                }
        //                else
        //                {
        //                    model.SeoTitle = "Купить межкомнатные двери из " + brand;
        //                    model.SeoHead = "Межкомнатные двери из " + brand + "! Вы здесь найдете межкомнатные двери на Ваш вкус, как дешевые двери так и качественные двери!";
        //                }
        //            }

        //        }
        //        //вызов метода сортировки

        //        model.ListMkDv = SortirDveri(page, sort, brand, flagMaterial);

        //        model.CurrentBrand = brand;
        //        model.FlagMaterial = flagMaterial;

        //        model.PagingInfo = new PagingInfo
        //        {
        //            CurrentPage = page>0? page :1,
        //            TotalItems = TotalItemsProduct,
        //            ItemsPerPage = PageSize
        //        };
        //        //для заполнения меню каталога
        //        model.Brand = dataManager.MegkomDvRepository.GetMkDv().Select(z => z.Proizvoditel).Distinct().OrderBy(z => z);
        //        model.Material = dataManager.MegkomDvRepository.GetMkDv().Select(z => z.Material).Distinct().OrderBy(z => z);
        //        if (Request.IsAjaxRequest())
        //        {
        //            return RedirectToAction("ProductList", new { page, sort, brand, flagMaterial });
        //        }

        //        return View(model);

        //    }
        //    catch (Exception er)
        //    {
        //        ClassLog.Write("MegkomnatnyeDveri/MegkomnatnyeDveriIndex-" + er);
        //        return View("Error");
        //    }
        //}

        //   метод возврата списка отсортированных дверей
        //public IEnumerable<MegkomnatnyeDveri> SortirDveri(int page, int s, string br,bool flagMaterial)
        //{
        //    try
        //    {
        //        IEnumerable<MegkomnatnyeDveri> temp = null;
        //        if (br != "весьтовар")
        //        {
        //            switch (s)
        //            {
        //                case 0:
        //                    {
        //                        if(!flagMaterial)
        //                            temp = dataManager.MegkomDvRepository.GetMkDv().OrderBy(x => x.Id).Where(x => x.Publicaciya == true && x.Proizvoditel == br).Skip((page - 1) * PageSize).Take(PageSize);
        //                        else
        //                            temp = dataManager.MegkomDvRepository.GetMkDv().OrderBy(x => x.Id).Where(x => x.Publicaciya == true && x.Material == br).Skip((page - 1) * PageSize).Take(PageSize);
        //                        break;
        //                    }
        //                case 1:
        //                    {
        //                        if(!flagMaterial)
        //                            temp = dataManager.MegkomDvRepository.GetMkDv().OrderBy(x => x.Cena).Where(x => x.Publicaciya == true && x.Proizvoditel == br).Skip((page - 1) * PageSize).Take(PageSize);
        //                        else
        //                            temp = dataManager.MegkomDvRepository.GetMkDv().OrderBy(x => x.Cena).Where(x => x.Publicaciya == true && x.Material == br).Skip((page - 1) * PageSize).Take(PageSize);
        //                        break;
        //                    }
        //                case 2:
        //                    {
        //                        if(!flagMaterial)
        //                            temp = dataManager.MegkomDvRepository.GetMkDv().OrderByDescending(x => x.Cena).Where(x => x.Publicaciya == true && x.Proizvoditel == br).Skip((page - 1) * PageSize).Take(PageSize);
        //                        else
        //                            temp = dataManager.MegkomDvRepository.GetMkDv().OrderByDescending(x => x.Cena).Where(x => x.Publicaciya == true && x.Material == br).Skip((page - 1) * PageSize).Take(PageSize);
        //                        break;
        //                    }
        //                default:
        //                    break;
        //            }
        //        }
        //        else
        //        {
        //            switch (s)
        //            {
        //                case 0:
        //                    {
        //                        temp = dataManager.MegkomDvRepository.GetMkDv().OrderBy(x => x.Id).Where(x => x.Publicaciya == true).Skip((page - 1) * PageSize).Take(PageSize);
        //                        break;
        //                    }
        //                case 1:
        //                    {
        //                        temp = dataManager.MegkomDvRepository.GetMkDv().OrderBy(x => x.Cena).Where(x => x.Publicaciya == true).Skip((page - 1) * PageSize).Take(PageSize);
        //                        break;
        //                    }
        //                case 2:
        //                    {
        //                        temp = dataManager.MegkomDvRepository.GetMkDv().OrderByDescending(x => x.Cena).Where(x => x.Publicaciya == true).Skip((page - 1) * PageSize).Take(PageSize);
        //                        break;
        //                    }
        //                default:
        //                    break;
        //            }
        //        }
        //        return temp;
        //    }
        //    catch (Exception er)
        //    {
        //        ClassLog.Write("VhodnyeDveri/SortirDveri-" + er);
        //        return null;
        //    }
        //}

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
                ClassLog.Write("MegkomnatnyeDveri/ContactOnPanel-" + er);
                return View("Error");
            }
        }
        //вызов контактов в футере
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
                ClassLog.Write("MegkomnatnyeDveri/ContactOnFooter-" + er);
                return View("Error");
            }
        }
        //для получения юридической информации на layout
        public ActionResult YrInf()
        {
            try
            {
                ModelYrinfa model = new ModelYrinfa();
                model.TextYrinfa = dataManager.ContactRepository.GetYrInfa().YrInfa;
                return PartialView(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("MegkomnatnyeDveri/YrInf-", er);
                return View("Error");
            }
        }

        //---------------------------------------------------------------хлебные крошки----------------------------------------------
        public ActionResult BreadCrumbs(ForMainModel mainmod = null, KartochkaTovaraModel kmod = null, string namepart = null, string nameart = null)
        {
            try
            {
                ModelBreadCrumbs mod = new ModelBreadCrumbs();

                if (mainmod.Brand != null)
                    mod.FlagMaterial = mainmod.FlagMaterial;
                else
                    mod.FlagMaterial = kmod.FlagMaterial;

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
                        if (!kmod.FlagMaterial)
                            mod.NameCategory = kmod.Tovar.Proizvoditel;
                        else
                            mod.NameCategory = kmod.Tovar.Material;
                        mod.NameProduct = kmod.Tovar.Nazvanie;
                    }
                }

                return View(mod);
            }
            catch (Exception er)
            {
                ClassLog.Write("MegkomnatnyeDveri/BreadCrumbs-" + er);
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
                    if (type == "межкомнатные двери")
                    {
                        model.DvName = dataManager.MegkomDvRepository.GetMkDvById(id).Nazvanie;
                    }
                }

                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("MegkomnatnyeDveri/BuyDveriModal-" + er);
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
                        return RedirectToAction("MegkomnatnyeDveriIndex");
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
                ClassLog.Write("MegkomnatnyeDveri/FastBuyDveri-" + er);
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
                ClassLog.Write("MegkomnatnyeDveri/FastBuyDveriModal-" + er);
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
                ClassLog.Write("MegkomnatnyeDveri/FastBuyDveriModel-" + er);
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
            string adres = dataManager.ContactRepository.GetWorkingEmails().FirstOrDefault().Email;
            //проверяем если нет такой двери
            if (model.IdDveri != 9999)
            {
                MegkomnatnyeDveri vhd = dataManager.MegkomDvRepository.GetMkDv().Where(x => x.Id == model.IdDveri).FirstOrDefault();
                string cena, cenasoskidkoy;
                if (vhd.Cena == null)
                {
                    cena = "Не установлена";
                }
                else
                {
                    cena = vhd.Cena.Value.ToString("C");
                }
                if (vhd.CenaSoSkidkoy == null)
                {
                    cenasoskidkoy = "Скидки нет";
                }
                else
                {
                    cenasoskidkoy = vhd.CenaSoSkidkoy.Value.ToString("C");
                }

                body = "Текст сообщения: " + model.KlientQuestion + "\r\n" + "тип товара: " + model.Type + "\r\n" + "код товара: " + model.IdDveri + "\r\n" + "наименование товара: " + vhd.Nazvanie + "\r\n" + "фирма производитель: " + vhd.Proizvoditel +
                "\r\n" + "Цена: " + cena + "\r\n" + "Скидка: " + vhd.Skidka + "\r\n" + "Цена со скидкой: " + cenasoskidkoy;
            }
            else
            {
                body = "Свяжитесь с клиентом по тел. или email: " + model.KlientPhone +"\r\n" + model.KlientQuestion ;
            }
            if (SendMsg.Message(body, them, adres))
            { return true; }
            else
            { return false; }
        }
     //=================================           продукт лист - список товаров                   =======================
        //[HttpGet]
        //public ActionResult ProductList(int page = 1, int sort = 0, string brand = "весьтовар",bool flagMaterial = false)
        //{
        //    try
        //    {
        //        ForMainModel model = new ForMainModel();
        //        model.CurrentBrand = brand;
        //        model.FlagMaterial = flagMaterial;
        //        int TotalItemsProduct;
        //        if (brand == "весьтовар")
        //        {
        //            TotalItemsProduct = dataManager.MegkomDvRepository.GetMkDv().Where(x => x.Publicaciya == true).Count();
        //        }
        //        else
        //        {
        //            if(!flagMaterial)
        //                TotalItemsProduct = dataManager.MegkomDvRepository.GetMkDv().Where(x => x.Publicaciya == true && x.Proizvoditel == brand).Count();
        //            else
        //                TotalItemsProduct = dataManager.MegkomDvRepository.GetMkDv().Where(x => x.Publicaciya == true && x.Material == brand).Count();
        //        }

        //        model.ListMkDv = SortirDveri(page, sort, brand,flagMaterial);
        //        model.PagingInfo = new PagingInfo
        //        {
        //            CurrentPage = page,
        //            TotalItems = TotalItemsProduct,
        //            ItemsPerPage = PageSize
        //        };
        //        return PartialView(model);
        //    }
        //    catch (Exception er)
        //    {
        //        ClassLog.Write("MegkomnatnyeDveri/ProductList-" + er);
        //        return View("Error");
        //    }
        //}

        //   метод загрузки первого изображения в списке товара
        public FileContentResult GetImage(int id)
        {
            try
            {
                FotoMegkomnDverey foto = dataManager.MegkomDvRepository.GetFotoMkDvByID(id).OrderBy(x => x.IdFoto).FirstOrDefault();
                if (foto != null)
                { return File(foto.Imaging, foto.MimeType); }
                else
                {//   изображение по умолчанию
                    return null;
                }
            }
            catch (Exception er)
            {
                ClassLog.Write("MegkomnatnyeDveri/GetImage-" + er);
                return null;
            }
        }

        //==========================================карточка товара==============================================================================
        [HttpGet]
        public ActionResult TovarPage(string tov, int id, bool flag = false)
        {
            try
            {
                KartochkaTovaraModel model = new KartochkaTovaraModel();
              
                    model.FlagMaterial = flag;
              
                model.Tovar = dataManager.MegkomDvRepository.GetMkDvById(id);
                model.FotoTovara = dataManager.MegkomDvRepository.GetFotoMkDvByID(id);
                //отбираем коментарии к товару по ID с указанной публикацией, сортироуем по дате (сначала последние(свежие), и отбираем 1-ые 100 комментов)
                model.CommentMkDvList = dataManager.CommentRepository.GetCommentMkDv().Where(x => x.IDdv == id && x.Public == true).OrderByDescending(z => z.Date).Take(100);
                OplDostModel md = new OplDostModel();
                md = InfaDostOplata();
                model.InfoDostavka = md.DostInfo;
                model.InfoOplata = md.OplInfo;
                SeoMkDverei se = dataManager.MegkomDvRepository.GetSeoMkDv().Where(x => x.Id == id).FirstOrDefault();
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
                ClassLog.Write("MegkomnatnyeDveri/ TovarPage-" + er);
                return View("Error");
            }
        }

        //акшен для возврата частичного представления с данными кликнутого фото в карточке товара
        [HttpGet]
        public ActionResult ImgTov(int id)
        {
            try
            {
                FotoMegkomnDverey foto = dataManager.MegkomDvRepository.GetFotoMkDv().Where(x => x.IdFoto == id).FirstOrDefault();
                return View(foto);
            }
            catch (Exception er)
            {
                ClassLog.Write("MegkomnatnyeDveri/ImgTov-" + er);
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

        //для загрузки похожих товаров на странице карточка товара КАРУСЕЛЬ
        public ActionResult SimilarGoods()
        {
            try
            {
                KaruselTovara model = new KaruselTovara();
                //получим рандомное число
                int maxgoods = dataManager.MegkomDvRepository.GetMkDv().Count();
                Random rand = new Random();
                int co = 25;
                int randomize;
                randomize = rand.Next(1, maxgoods - co);
              
                //пропустим рандомное число товаров и выберем следующие 24;
                model.ListMkDv = dataManager.MegkomDvRepository.GetMkDv().Skip(randomize).Take(24);
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("MegkomnatnyeDveri/SimilarGoods-", er);
                return View("Error");
               
            }
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
                if (type == "межкомнатные двери")
                {
                    if(id!=9999)
                        model.DvName = dataManager.MegkomDvRepository.GetMkDvById(id).Nazvanie;
                }
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("MegkomnatnyeDveri/BuyDveriModal-" + er);
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
                        if (model.IdDveri != 9999)
                        {
                            return Json(model.KlientName + "! Вы сделали заказ на покупку товара: " + model.DvName + ". Наш менеджер свяжется с Вами в течении 15 минут! Спасибо, что выбрали наш магазин!", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(model.KlientName + "! Cпасибо, что оставили свое сообщение. Мы ответим Вам в течении 15 минут.");
                        }
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
                ClassLog.Write("MegkomnatnyeDveri/BuyDveriModel-" + er);
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
                if (type == "межкомнатные двери")
                {
                    if(id!=9999)
                    model.DvName = dataManager.MegkomDvRepository.GetMkDvById(id).Nazvanie;
                }
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("MegkomnatnyeDveri/BuyDveri-" + er);
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
                    if(model.IdDveri!=9999)
                    {
                        return RedirectToAction("TovarPage", new { id = model.IdDveri });
                    }
                    else
                    {
                        return RedirectToAction("MegkomnatnyeDveriIndex");
                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception er)
            {
                ClassLog.Write("MegkomnatnyeDveri/BuyDveri-" + er);
                return View("Error");
            }
        }



        //============================написать нам====================================================
        [HttpGet]
        public ActionResult WriteToUs()
        {
            try
            {
                ModelBuyDveri model = new ModelBuyDveri();
                model.Type = "Простое сообщение от клиента!";
                model.IdDveri = 9999;
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("MegkomnatnyeDveri/WriteToUs-" + er);
                return View("Error");
            }

        }
        [HttpPost]
        public ActionResult WriteToUs(ModelBuyDveri model)
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
                    return RedirectToAction("MegkomnatnyeDveriIndex");
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception er)
            {
                ClassLog.Write("MegkomnatnyeDveri/WriteToUs-" + er);
                return View("Error");
            }
        }


        //===========================для модального окна написать нам =====================
        [HttpGet]
        public ActionResult WriteToUsModal()
        {
            try
            {
                ModelBuyDveri model = new ModelBuyDveri();
                model.IdDveri = 9999;
                model.Type = "Простое сообщение от клиента!";
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("MegkomnatnyeDveri/WriteToUsModal-" + er);
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult WriteToUsModal(ModelBuyDveri model)
        {
            try
            {
                if (ModelState.IsValid)
                {//вызов метода отправки сообщения админу
                    if (SendMessageAboutBuyDoor(model))
                    {
                        return Json(model.KlientName + "! Спасибо, что оставили свое сообщение, мы обязательно ответим Вам!", JsonRequestBehavior.AllowGet);
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
                ClassLog.Write("MegkomnatnyeDveri/WriteToUsModal-" + er);
                return View("Error");
            }
        }



    }
}
