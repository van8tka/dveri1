using Domain2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DALdv1;
using dveri1.Models;
namespace dveri1.Controllers
{
    public class ContactController : Controller
    {
        private DataManager dataManager;
        public ContactController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        // GET: Contact
        public ActionResult AdminContact()
        {
            ContactModel model = new ContactModel();
            model.AdresList = dataManager.ContactRepository.GetAdres();
            model.ContactList = dataManager.ContactRepository.GetContacts();
            model.GrafikWorkList = dataManager.ContactRepository.GetGrafikWork();
            return View(model);
        }
        //==========================создание(добавление) контактных данных========================================
        [HttpGet]
        public ActionResult CreateGrafik()
        {
            ViewBag.CreateContact = "Добавление графика работы";
            return View();
        }
        [HttpPost]
        public ActionResult CreateGrafik(CreateGrafikModel model)
        {
            if(ModelState.IsValid)
            {
                dataManager.ContactRepository.CreateGrafikWork(0, model.Day, model.Time);
                TempData["message"] = "Информация о периоде работы добавлена!";
                return RedirectToAction("AdminContact");
            }
            return View();
        }
        [HttpGet]
        public ActionResult CreateContact()
        {
            ViewBag.CreateContact = "Добавление контактных данных";
            return View();
        }
        [HttpPost]
        public ActionResult CreateContact(CreateContactModel model)
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
        [HttpGet]
        public ActionResult CreateAdres()
        {
            ViewBag.CreateContact = "Добавление адресных данных";
            return View();
        }
        [HttpPost]
        public ActionResult CreateAdres(CreateAdresModel model)
        {
            if (ModelState.IsValid)
            {
                dataManager.ContactRepository.CreateAdres(0, model.AdresName);
                TempData["message"] = "Адресные данные добавлены!";
                return RedirectToAction("AdminContact");
            }
            return View();
        }
        //============================изменение контактных данных======================================
        [HttpGet]
        public ActionResult ChangeContact(int id)
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
        [HttpPost]
        public ActionResult ChangeContact(CreateContactModel model)
        {
            if (ModelState.IsValid)
            {
                dataManager.ContactRepository.CreateContact((int)model.IDContact, model.TypContact, model.NameContact);
                TempData["message"] = "Контактная информация изменена!";
                return RedirectToAction("AdminContact");
            }
            return View();
        }

        [HttpGet]
        public ActionResult ChangeGrafik(int id)
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
        [HttpPost]
        public ActionResult ChangeGrafik(CreateGrafikModel model)
        {
            if (ModelState.IsValid)
            {
                dataManager.ContactRepository.CreateGrafikWork((int)model.IDGrafik, model.Day, model.Time);
                TempData["message"] = "График работы изменен!";
                return RedirectToAction("AdminContact");
            }
            return View();
        }

        [HttpGet]
        public ActionResult ChangeAdres(int id)
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
        [HttpPost]
        public ActionResult ChangeAdres(CreateAdresModel model)
        {
            if (ModelState.IsValid)
            {
                dataManager.ContactRepository.CreateAdres((int)model.IDAdres, model.AdresName);
                TempData["message"] = "Адрес изменен!";
                return RedirectToAction("AdminContact");
            }
            return View();
        }

        //==========================удаление контактных данных========================================
        public ActionResult DellAdres(int id)
        {
            dataManager.ContactRepository.DellAdres(id);
            TempData["message"] = "Адрес удален!";
            return RedirectToAction("AdminContact");
        }
        public ActionResult DellContact(int id)
        {
            dataManager.ContactRepository.DellContact(id);
            TempData["message"] = "Контактная информация удалена!";
            return RedirectToAction("AdminContact");
        }
        public ActionResult DellGrafik(int id)
        {
            dataManager.ContactRepository.DellGrafikWork(id);
            TempData["message"] = "Информация из графика работы удалена!";
            return RedirectToAction("AdminContact");
        }
    }
}