using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Domain2.Interfaces;
using DALdv1;


namespace Domain2.Implementations
{
    public class EfCommentRepository : ICommentRepository
    {
        dbdveri1Entities1 context;
        public EfCommentRepository(dbdveri1Entities1 context)
        {
            this.context = context;
        }
        public void CreateCommentCompany(int id, string name, string email, string comment, string response, string header, bool publ, int stars, DateTime date)
        {
            if (id == 0)
            {
                CommentCompany c = new CommentCompany
                {
                   ID = id,
                   Name = name,
                   E_mail = email,
                   Comment = comment,
                   Response = response,
                   Heading = header,
                   Public = publ,
                   Date = date,
                   Stars = stars
                };
                context.CommentCompanies.Add(c);
            }
            else
            {
                CommentCompany c = context.CommentCompanies.Where(x => x.ID == id).FirstOrDefault();
                c.ID = id;
                c.Name = name;
                c.E_mail = email;
                c.Comment = comment;
                c.Response = response;
                c.Heading = header;
                c.Public = publ;
                c.Date = date;
                c.Stars = stars;
                context.Entry(c).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void CreateCommentVhDv(int id, int iddv, string name, string email, string comment, string response, string header, bool publ, int stars, DateTime date)
        {
           if (id == 0)
            {
                CommentVhDveri c = new CommentVhDveri
                {
                    ID = id,
                    IDdv = iddv,
                    Name = name,
                    E_mail = email,
                    Comment = comment,
                    Response = response,
                    Heading = header,
                    Public = publ,
                    Date = date,
                    Stars = stars
                };
                context.CommentVhDveris.Add(c);
            }
            else
            {
                CommentVhDveri c = context.CommentVhDveris.Where(x => x.ID == id).FirstOrDefault();
                c.ID = id;
                c.IDdv = iddv;
                c.Name = name;
                c.E_mail = email;
                c.Comment = comment;
                c.Response = response;
                c.Heading = header;
                c.Public = publ;
                c.Date = date;
                c.Stars = stars;
                context.Entry(c).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DellComCop(int id)
        {
            context.CommentCompanies.Remove(context.CommentCompanies.Where(i => i.ID == id).FirstOrDefault());
            context.SaveChanges();
        }

        public void DellComVhDv(int id)
        {
            context.CommentVhDveris.Remove(context.CommentVhDveris.Where(i => i.ID == id).FirstOrDefault());
            context.SaveChanges();
        }

        public IEnumerable<CommentVhDveri> GetCommentVhDv()
        {
            return context.CommentVhDveris;
        }

        public IEnumerable<CommentCompany> GetCommetCompany()
        {
            return context.CommentCompanies;
        }
    }
}
