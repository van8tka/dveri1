using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;
using Domain2.Interfaces;

namespace Domain2.Implementations
{
    public class EfSeoMainRepository : ISeoMainRepository
    {
        private dbdveri1Entities1 contex;
        public EfSeoMainRepository(dbdveri1Entities1 contex)
        {
            this.contex = contex;
        }
        public void CreateSeo(int id, string tit, string key, string des, string page, string head, string categ)
        {
            if(id==0)
            {
                SeoMain s = new SeoMain
                {
                    ID = id,
                    Title = tit,
                    Keywords = key,
                    Description = des,
                    Page = page,
                    Header = head,
                    Category = categ
                };
                contex.SeoMains.Add(s);
            }
            else
            {
                SeoMain s = contex.SeoMains.Where(x => x.ID == id).FirstOrDefault();
                s.Title = tit;
                s.Keywords = key;
                s.Description = des;
                s.Page = page;
                s.Header = head;
                s.Category = categ;
                contex.Entry(s).State = System.Data.Entity.EntityState.Modified;
            }
            contex.SaveChanges();
        }

        public void DellSeo(int id)
        {
            SeoMain s = contex.SeoMains.Where(x => x.ID == id).FirstOrDefault();
            contex.SeoMains.Remove(s);
            contex.SaveChanges();
        }

        public IEnumerable<SeoMain> GetSeoMain()
        {
            return contex.SeoMains;
        }

        public SeoMain GetSeoMainByPage(string p)
        {
            return contex.SeoMains.Where(x => x.Page == p).FirstOrDefault();
        }
    }
}
