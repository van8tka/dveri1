using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DALdv1;
using dveri1.Models;
using dveri1.DopMethod;
using Domain2;


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
        public ActionResult GetAllComment()
        {
            ModelComment model = new ModelComment();
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
            return View(model);
      }
    }
}