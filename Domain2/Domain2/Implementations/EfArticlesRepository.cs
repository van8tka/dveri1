using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;
using Domain2.Interfaces;

namespace Domain2.Implementations
{
    public class EfArticlesRepository : IArticlesRepository
    {
        dbdveri1Entities1 context;
        public EfArticlesRepository(dbdveri1Entities1 context)
        {
            this.context = context;
        }
        public void CreateArticle(int id, string name, string descrip, string art)
        {
            if (id == 0)
            {
                TableArticle ta = new TableArticle()
                {
                    ID = id,
                    Name = name,
                    Headings = descrip,
                    Articles = art,
                    Date = DateTime.Now
                };
                context.TableArticles.Add(ta);
            }
            else
            {
                TableArticle ta = context.TableArticles.Where(i => i.ID == id).FirstOrDefault();
                if(ta!=null)
                {
                    ta.Name = name;
                    ta.Headings = descrip;
                    ta.Articles = art;
                    ta.Date = DateTime.Now;
                    context.Entry(ta).State = System.Data.Entity.EntityState.Modified;
                }
            }
            context.SaveChanges();     

        }

        public void DelArticle(int id)
        {
            TableArticle ta = context.TableArticles.Where(i => i.ID == id).FirstOrDefault();
            if(ta!=null)
            {
                context.TableArticles.Remove(ta);
                context.SaveChanges();
            }
            
        }

        public TableArticle GetArticle(int id)
        {
            return context.TableArticles.Where(z => z.ID == id).FirstOrDefault();
        }

        public IEnumerable<TableArticle> GetArticles()
        {
            return context.TableArticles;
        }
    }
}
