using Domain2;
using dveri2.Models;
using dveri2.DopMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dveri2.Controllers
{
    public class CommentController : Controller
    {
        private DataManager dataManager;
        public CommentController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        //добавление комментария покупателем
        [HttpGet]
        public ActionResult CreateComment(int iddv = 0)
        {
            try
            {
                ModelCommentCreate model = new ModelCommentCreate();
                model.IdDv = iddv;
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("/Comment/CreateComment" + er);
                return View("Error");
            }
        }
        [HttpPost]
        public ActionResult CreateComment(ModelCommentCreate model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Stars == 0)
                    {
                        model.Stars = 5;
                    }
                  
                        if (model.Head == null || model.Head == "")
                        {
                            model.Head = AutoHead(model.Stars);
                        }
                        dataManager.CommentRepository.CreateCommentMkDv(0, model.IdDv, model.Name, model.Email, model.Comm, model.Resp, model.Head, false, model.Stars, DateTime.Now);
                        TempData["messageklient"] = "Ваш комментарий будет добавлен после проверки модератором!";
                        //отправим сообщение админу на емаил
                        //получим инфу о дверях
                        string namedv = dataManager.MegkomDvRepository.GetMkDvById(model.IdDv).Nazvanie;
                        string body = "Отзыв о межкомнатных дверях N: " + model.IdDv + " название: " + namedv + "\r\n" + "Количество звезд: " + model.Stars + "\r\n" + "Заголовок: " + model.Head + "\r\n" + "Коментарий: " + model.Comm;
                        string them = "Отзыв о межкомнатных дверях, от клиента: " + model.Name + ", e-mail " + model.Email;
                        //получаем адрес администратора указанный в контаках с пизнаком e-mail
                        string adres = dataManager.ContactRepository.GetContacts().Where(x => x.TypeSv == "e-mail").FirstOrDefault().NumberSv;
                        SendMsg.Message(body, them, adres);
                        return RedirectToAction("TovarPage", "MegkomnatnyeDveri", new { id = model.IdDv });
                  
                }
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("/Comment/CreateComment" + er);
                return View("Error");
            }
        }
        //метод авто подставновки заголовка если пользователь проигнорировал, в зависимости от кол-ва звезд
        private string AutoHead(int stars)
        {
            string Head;
            switch (stars)
            {
                case 1:
                    {
                        Head = "Ужасно!";
                        break;
                    }
                case 2:
                    {
                        Head = "Плохо!";
                        break;
                    }
                case 3:
                    {
                        Head = "Нормально!";
                        break;
                    }
                case 4:
                    {
                        Head = "Хорошо!";
                        break;
                    }
                case 5:
                    {
                        Head = "Отлично!";
                        break;
                    }
                default:
                    {
                        Head = "Но все таки, я доволен работой магазина!";
                        break;
                    }
            }
            return Head;
        }
    }
}