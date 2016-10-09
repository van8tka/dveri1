using Domain2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DALdv1;
using dveri1.Models;
using dveri1.DopMethod;

namespace dveri1.Controllers
{
    public class ContactController : Controller
    {
        private DataManager dataManager;
        public ContactController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
     
      [Authorize]
        public ActionResult AdminContact()
        {
            try
            {
            ContactModel model = new ContactModel();
            model.AdresList = dataManager.ContactRepository.GetAdres();
            model.ContactList = dataManager.ContactRepository.GetContacts();
            model.GrafikWorkList = dataManager.ContactRepository.GetGrafikWork();
            model.YrInformationProp = dataManager.ContactRepository.GetYrInfa();
            model.WorkEmails = dataManager.ContactRepository.GetWorkingEmails().FirstOrDefault();
            return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("Contact/AdminContact-", er);
                return View("Error");
            }
        }
        [Authorize]
        //==========================создание(добавление) контактных данных========================================
        [HttpGet]
        public ActionResult CreateGrafik()
        {
            ViewBag.CreateContact = "Добавление графика работы";
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateGrafik(CreateGrafikModel model)
        {
            try
            {
            if(ModelState.IsValid)
            {
                dataManager.ContactRepository.CreateGrafikWork(0, model.Day, model.Time);
                TempData["message"] = "Информация о периоде работы добавлена!";
                return RedirectToAction("AdminContact");
            }
            return View();
            }
            catch (Exception er)
            {
                ClassLog.Write("Contact/CreateGrafik-", er);
                return View("Error");
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult CreateContact()
        {
            ViewBag.CreateContact = "Добавление контактных данных";
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateContact(CreateContactModel model)
        {
            try
            {
            model.IDContact = 0;
            if (ModelState.IsValid)
            {
                dataManager.ContactRepository.CreateContact(0, model.TypContact, model.NameContact);
                TempData["message"] = "Контактная информация добавлена!";
                return RedirectToAction("AdminContact");
            }
            return View();
            }
            catch (Exception er)
            {
                ClassLog.Write("Contact/CreateContact-", er);
                return View("Error");
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult CreateAdres()
        {
            ViewBag.CreateContact = "Добавление адресных данных";
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateAdres(CreateAdresModel model)
        {
            try
            {
            if (ModelState.IsValid)
            {
                dataManager.ContactRepository.CreateAdres(0, model.AdresName);
                TempData["message"] = "Адресные данные добавлены!";
                return RedirectToAction("AdminContact");
            }
            return View();
            }
            catch (Exception er)
            {
                ClassLog.Write("Contact/CreateAdres-", er);
                return View("Error");
            }
        }


        [Authorize]
        [HttpGet]
        public ActionResult CreateYrInfa()
        {
            ViewBag.CreateContact = "Добавление юридической информации";
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateYrInfa(CreateAdresModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TableYrInfa yr = dataManager.ContactRepository.GetYrInfa();
                    dataManager.ContactRepository.CreateYrInfa(0, model.AdresName);
                    if (yr != null)
                        dataManager.ContactRepository.DellYrInfa(yr.Id);
                    TempData["message"] = "Юридическая информация добавлена!";
                    return RedirectToAction("AdminContact");
                }
                return View();
            }
            catch (Exception er)
            {
                ClassLog.Write("Contact/CreateYrInfa-", er);
                return View("Error");
            }
        }
        //----------создание рабочей почты-------------------------
        [Authorize]
        [HttpGet]
        public ActionResult CreateWorkEmail(int id=0)
        {
            CreateEmailModel model = new CreateEmailModel();
            model.ID = id;
            if (id==0)
            {
                ViewBag.CreateContact = "Добавление рабочей почты";
            }         
            else
            {
                ViewBag.CreateContact = "Изменение рабочей почты";
                model.EmailVal = dataManager.ContactRepository.GetWorkingEmails().FirstOrDefault().Email;
            }
                        
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateWorkEmail(CreateEmailModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(model.ID==0)
                    {
                        TableWorkingEmail em = dataManager.ContactRepository.GetWorkingEmails().FirstOrDefault();
                        dataManager.ContactRepository.CreateWorkingEmail(0, model.EmailVal);
                        if (em != null)
                            dataManager.ContactRepository.DellWorkingEmail(em.ID);
                        TempData["message"] = "Рабочая почта добавлена!";
                    }
                   else
                    {
                        dataManager.ContactRepository.CreateWorkingEmail(model.ID, model.EmailVal);
                        TempData["message"] = "Рабочая почта изменена!";
                    }
                    return RedirectToAction("AdminContact");
                }
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("Contact/CreateWorkingEmail-", er);
                return View("Error");
            }
        }



        //============================изменение контактных данных======================================
        [Authorize]
        [HttpGet]
        public ActionResult ChangeContact(int id)
        {
            try
            {
            ViewBag.CreateContact = "Изменение контактных данных";
            Contact con = dataManager.ContactRepository.GetContacts().Where(z => z.Id == id).FirstOrDefault();
            CreateContactModel model = new CreateContactModel()
            {
                IDContact = con.Id,
                TypContact = con.TypeSv,
                NameContact = con.NumberSv
            };
            return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("Contact/ChangeContact-", er);
                return View("Error");
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult ChangeContact(CreateContactModel model)
        {
            try
            {
            if (ModelState.IsValid)
            {
                dataManager.ContactRepository.CreateContact((int)model.IDContact, model.TypContact, model.NameContact);
                TempData["message"] = "Контактная информация изменена!";
                return RedirectToAction("AdminContact");
            }
            return View();
            }
            catch (Exception er)
            {
                ClassLog.Write("Contact/ChangeContact-", er);
                return View("Error");
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult ChangeGrafik(int id)
        {
            try
            {
            ViewBag.CreateContact = "Изменение графика работы";
            GrafikWork con = dataManager.ContactRepository.GetGrafikWork().Where(z => z.Id == id).FirstOrDefault();
            CreateGrafikModel model = new CreateGrafikModel()
            {
                IDGrafik = con.Id,
                Day = con.NameDay,
                Time = con.TimeWork
            };
            return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("Contact/ChangeGrafik-", er);
                return View("Error");
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult ChangeGrafik(CreateGrafikModel model)
        {
            try
            {
            if (ModelState.IsValid)
            {
                dataManager.ContactRepository.CreateGrafikWork((int)model.IDGrafik, model.Day, model.Time);
                TempData["message"] = "График работы изменен!";
                return RedirectToAction("AdminContact");
            }
            return View();
            }
            catch (Exception er)
            {
                ClassLog.Write("Contact/ChangeGrafik-", er);
                return View("Error");
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult ChangeAdres(int id)
        {
            try
            {
            ViewBag.CreateContact = "Изменение адреса местоположения офиса";
            Adresa con = dataManager.ContactRepository.GetAdres().Where(z => z.Id == id).FirstOrDefault();
            CreateAdresModel model = new CreateAdresModel()
            {
                IDAdres = con.Id,
                AdresName = con.AdresName
            };
            return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("Contact/ChangeAdres-", er);
                return View("Error");
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult ChangeAdres(CreateAdresModel model)
        {
            try
            {
            if (ModelState.IsValid)
            {
                dataManager.ContactRepository.CreateAdres((int)model.IDAdres, model.AdresName);
                TempData["message"] = "Адрес изменен!";
                return RedirectToAction("AdminContact");
            }
            return View();
            }
            catch (Exception er)
            {
                ClassLog.Write("Contact/ChangeAdres-", er);
                return View("Error");
            }
        }



        [Authorize]
        [HttpGet]
        public ActionResult ChangeYrInfa(int id)
        {
            try
            {
                ViewBag.CreateContact = "Изменение юридичской информации";
                TableYrInfa yi = dataManager.ContactRepository.GetYrInfa();
                //используем модель для адресов
                CreateAdresModel model = new CreateAdresModel()
                {
                    IDAdres = yi.Id,
                    AdresName = yi.YrInfa
                };
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("Contact/ChangeYrnfa-", er);
                return View("Error");
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult ChangeYrInfa(CreateAdresModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dataManager.ContactRepository.CreateYrInfa((int)model.IDAdres, model.AdresName);
                    TempData["message"] = "Юридическая информация изменена!";
                    return RedirectToAction("AdminContact");
                }
                return View();
            }
            catch (Exception er)
            {
                ClassLog.Write("Contact/ChangeYrInfa-", er);
                return View("Error");
            }
        }


        //==========================удаление контактных данных========================================
        [Authorize]
        public ActionResult DellAdres(int id)
        {
            try
            {
            dataManager.ContactRepository.DellAdres(id);
            TempData["message"] = "Адрес удален!";
            return RedirectToAction("AdminContact");
            }
            catch (Exception er)
            {
                ClassLog.Write("Contact/DellAdres-", er);
                return View("Error");
            }
        }
        [Authorize]
        public ActionResult DellContact(int id)
        {
            try
            {
            dataManager.ContactRepository.DellContact(id);
            TempData["message"] = "Контактная информация удалена!";
            return RedirectToAction("AdminContact");
            }
            catch (Exception er)
            {
                ClassLog.Write("Contact/DellContact-", er);
                return View("Error");
            }
        }
        [Authorize]
        public ActionResult DellGrafik(int id)
        {
            try
            {
            dataManager.ContactRepository.DellGrafikWork(id);
            TempData["message"] = "Информация из графика работы удалена!";
            return RedirectToAction("AdminContact");
            }
            catch (Exception er)
            {
                ClassLog.Write("Contact/DellGrafik-",er);
                return View("Error");
            }
        }
        [Authorize]
        public ActionResult DellYrInfa(int id)
        {
            try
            {
                dataManager.ContactRepository.DellYrInfa(id);
                TempData["message"] = "Юридическая информация удалена!";
                return RedirectToAction("AdminContact");
            }
            catch (Exception er)
            {
                ClassLog.Write("Contact/DellAdres-", er);
                return View("Error");
            }
        }

        [Authorize]
        public ActionResult DellWorkEmail(int id)
        {
            try
            {
                IEnumerable<TableWorkingEmail> e = dataManager.ContactRepository.GetWorkingEmails();
                if(e.Count()==1)
                {
                    TempData["message"] = "Осталась одна рабочая почта, удаление невозможно!";
                    return RedirectToAction("AdminContact");
                }
                else
                {
                    dataManager.ContactRepository.DellWorkingEmail(id);
                    TempData["message"] = "Рабочая почта удалена!";
                    return RedirectToAction("AdminContact");
                }             
            }
            catch (Exception er)
            {
                ClassLog.Write("Contact/DellWorkEmail-", er);
                return View("Error");
            }
        }
    }
}