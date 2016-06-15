using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using dveri1.Models;

namespace dveri1.Controllers
{
       
        public class AccountController : Controller
        {
        [AllowAnonymous]
        public ActionResult Login()
            {
                return View();
            }
        [AllowAnonymous]
        [HttpPost]
            public ActionResult Login(ModelUser model, string returnUrl)
            {
                if (ModelState.IsValid)
                {
                    if (Membership.ValidateUser(model.NameUser, model.PasswordUser))
                    {
                        FormsAuthentication.SetAuthCookie(model.NameUser, false);
                        return RedirectToAction("Panel", "Admin");                    
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неправильный пароль или логин");
                    }
                }
                return View(model);
            }
            public ActionResult LogOff()
            {
                FormsAuthentication.SignOut();
               return RedirectToAction("VhodnyeDveriIndex", "VhodnyeDveri");
            }
        [Authorize]
            public ActionResult Register()
            {
                return View();
            }
        [Authorize]
        [HttpPost]
            public ActionResult Register(ModelUser model)
            {
                if (ModelState.IsValid)
                {
                    MembershipUser membershipUser = ((Providers.CustomMembershipProvider)Membership.Provider).CreateUser(model.NameUser, model.PasswordUser);

                    if (membershipUser != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.NameUser, false);
                        TempData["message"] = "Новый пользователь добавлен в систему!";
                        return RedirectToAction("Panel", "Admin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ошибка при регистрации");
                    }
                }
                return View(model);
            }
        }
    }