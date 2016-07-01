using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DALdv1;
using dveri1.Models;
using dveri1.DopMethod;
using Domain2;
using dveri1.DopMethod;


namespace dveri1.Controllers
{
    public class CommentController : Controller
    {
        private DataManager dataManager;
        public CommentController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        // GET: Comment
        public int PageSize = 100;
        public ActionResult GetAllComment(int page = 1)
        {
            try
            {
                ModelComment model = new ModelComment();
                int TotalComment;
                //обработка данных для метатэгов страницы
                SeoMain s = dataManager.SeoMainRepository.GetSeoMainByPage("Отзывы");
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
                        model.SeoHead = "Отзывы и комментарии о работе компании Люксеврострой и предлагаемой продукции";
                    }
                }
                else
                {
                    model.SeoTitle = "Люксеврострой - отзывы о работе компании, отзывы о входных и межкомнатных дверях";
                    model.SeoHead = "Отзывы и комментарии о работе компании Люксеврострой и предлагаемой продукции";
                }
                model.CommentCompList = dataManager.CommentRepository.GetCommetCompany().Where(x=>x.Public==true).OrderByDescending(x=>x.Date).Skip((page-1)*PageSize).Take(PageSize);
                TotalComment = dataManager.CommentRepository.GetCommetCompany().Where(x => x.Public == true).Count();
                model.PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    TotalItems = TotalComment,
                    ItemsPerPage = PageSize
                };
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("/Comment/GetAllComment" + er);
                return View("Error");
            }
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
                ClassLog.Write("/Comment/GetAllComment" + er);
                return View("Error");
            }
        }
        [HttpPost]
        public ActionResult CreateComment(ModelCommentCreate model)
        {
            try
            {
               if(ModelState.IsValid)
                {
                    if (model.Stars == 0)
                    {
                        model.Stars = 5;
                    }
                    //если id двери 0 то отзыв о работе компании
                    if (model.IdDv==0)
                    {//если заголовка нет то он пишется в зависимости от выбранного количества звезд
                        if (model.Head == null || model.Head == "")
                        {
                            model.Head = AutoHead(model.Stars);
                        }
                        dataManager.CommentRepository.CreateCommentCompany(0, model.Name, model.Email, model.Comm, null, model.Head, false, model.Stars, DateTime.Now);
                        TempData["messageklient"] = "Ваш комментарий будет добавлен после проверки модератором!";
                        //отправим сообщение админу на емаил
                        string body ="Количество звезд: "+model.Stars+"\r\n"+"Заголовок: "+ model.Head+"\r\n"+"Коментарий: "+ model.Comm;
                        string them = "Отзыв о компаниии от клиента: " + model.Name + ", e-mail " + model.Email;
                        //получаем адрес администратора указанный в контаках с пизнаком e-mail
                        string adres = dataManager.ContactRepository.GetContacts().Where(x => x.TypeSv == "e-mail").FirstOrDefault().NumberSv;
                        SendMsg.Message(body, them, adres);
                        return RedirectToAction("GetAllComment");                                     
                    }
                    else
                    {
                        if (model.Head == null || model.Head == "")
                        {
                            model.Head = AutoHead(model.Stars);
                        }
                        dataManager.CommentRepository.CreateCommentVhDv(0, model.IdDv, model.Name, model.Email, model.Comm, model.Resp, model.Head, false, model.Stars, DateTime.Now);
                        TempData["messageklient"] = "Ваш комментарий будет добавлен после проверки модератором!";
                        //отправим сообщение админу на емаил
                        //получим инфу о дверях
                        string namedv = dataManager.VhodnyeDvRepository.GetVhodnyeDvById(model.IdDv).Nazvanie;
                        string body = "Отзыв о вх. дверях N: "+model.IdDv+" название: "+namedv+ "\r\n" + "Количество звезд: " + model.Stars + "\r\n" + "Заголовок: " + model.Head + "\r\n" + "Коментарий: " + model.Comm;
                        string them = "Отзыв о входных дверях, от клиента: " + model.Name + ", e-mail " + model.Email;
                        //получаем адрес администратора указанный в контаках с пизнаком e-mail
                        string adres = dataManager.ContactRepository.GetContacts().Where(x => x.TypeSv == "e-mail").FirstOrDefault().NumberSv;
                        SendMsg.Message(body, them, adres);
                        return RedirectToAction("TovarPage","VhodnyeDveri",new {id=model.IdDv});
                    }
                   
                }              
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("/Comment/GetAllComment" + er);
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
        //вывод всех коментов компании
        [Authorize]
        [HttpGet]
        public ActionResult AdminCommentCompany()
        {
            ModelComment model = new ModelComment();
            model.CommentCompList = dataManager.CommentRepository.GetCommetCompany().OrderByDescending(x=>x.Date);
            return View(model);
        }
        //вывод всех коментов о входных дверях
        [Authorize]
        [HttpGet]
        public ActionResult AdminCommentVhDv()
        {
            ModelCommentVhDv model = new ModelCommentVhDv();
            model.CommentVhDvList = dataManager.CommentRepository.GetCommentVhDv().OrderByDescending(x => x.Date); 
            return View(model);
        }
        //удаление коментов о входных дверях
        [Authorize]
        [HttpGet]
        public ActionResult AdminDellCommentVhDv(int id)
        {
            try
            {
                dataManager.CommentRepository.DellComVhDv(id);
                return RedirectToAction("AdminCommentVhDv");
            }
            catch (Exception er)
            {
                ClassLog.Write("/Comment/AdminDellVhDv" + er);
                return View("Error");
            }
        }
        //удаление коментов о компании 
        [Authorize]
        [HttpGet]
        public ActionResult AdminDellCommentCompany(int id)
        {
            try
            {
                dataManager.CommentRepository.DellComCop(id);
                return RedirectToAction("AdminCommentCompany");
            }
            catch (Exception er)
            {
                ClassLog.Write("/Comment/AdminDellCommentCompany" + er);
                return View("Error");
            }
        }
        //изменение коментов о компании 
        [Authorize]
        [HttpGet]
        public ActionResult AdminEditCommentCompany(int id)
        {
            try
            {
                ModelCommentCreate model = new ModelCommentCreate();
                CommentCompany cc = dataManager.CommentRepository.GetCommetCompany().Where(x => x.ID == id).FirstOrDefault();
                model.Id = cc.ID;
                model.Name = cc.Name;
                model.Publish = cc.Public;
                model.Resp = cc.Response;
                model.Stars = cc.Stars;
                model.Email = cc.E_mail;
                model.Comm = cc.Comment;
                model.Date = cc.Date;
                model.Head = cc.Heading;
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("/Comment/AdminEditCommentCompany" + er);
                return View("Error");
            }
        }
        //изменение коментов о компании 
        [Authorize]
        [HttpPost]
        public ActionResult AdminEditCommentCompany(ModelCommentCreate model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    if(model.IdDv == 0)
                    {
                        dataManager.CommentRepository.CreateCommentCompany(model.Id, model.Name, model.Email, model.Comm, model.Resp, model.Head, model.Publish, model.Stars, model.Date);
                        TempData["message"] = "Комментарий изменен!";
                        return RedirectToAction("AdminCommentCompany");
                    }
                }              
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("/Comment/AdminEditCommentCompany(post)" + er);
                return View("Error");
            }
        }
        //изменение коментов о входных дверях
        [Authorize]
        [HttpGet]
        public ActionResult AdminEditCommentVhDv(int id)
        {
            try
            {
                ModelCommentCreate model = new ModelCommentCreate();
                CommentVhDveri cc = dataManager.CommentRepository.GetCommentVhDv().Where(x => x.ID == id).FirstOrDefault();
                model.Id = cc.ID;
                model.IdDv = cc.IDdv;
                model.Name = cc.Name;
                model.Publish = cc.Public;
                model.Resp = cc.Response;
                model.Stars = cc.Stars;
                model.Email = cc.E_mail;
                model.Comm = cc.Comment;
                model.Date = cc.Date;
                model.Head = cc.Heading;
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("/Comment/AdminEditCommentVhDv" + er);
                return View("Error");
            }
        }
        //изменение коментов о входных дверях
        [Authorize]
        [HttpPost]
        public ActionResult AdminEditCommentVhDv(ModelCommentCreate model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.IdDv != 0)
                    {
                        dataManager.CommentRepository.CreateCommentVhDv(model.Id,model.IdDv, model.Name, model.Email, model.Comm, model.Resp, model.Head, model.Publish, model.Stars, model.Date);
                        TempData["message"] = "Комментарий изменен!";
                        return RedirectToAction("AdminCommentVhDv");
                    }
                }
                return View(model);
            }
            catch (Exception er)
            {
                ClassLog.Write("/Comment/AdminEditCommentVhDv(post)" + er);
                return View("Error");
            }
        }
       
    }
}