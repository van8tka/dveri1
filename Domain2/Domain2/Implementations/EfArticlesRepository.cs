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
                    Date = DateTime.Now,
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
            if(id==0)
            {
                TableSeoArticle ta = new TableSeoArticle()
                {
                    ID = context.TableArticles.OrderByDescending(z => z.ID).First().ID,
                    Title = name,
                    Keywords = null,
                    Description = null
                };
                context.TableSeoArticles.Add(ta);
                context.SaveChanges();
            }
        }

        public void CreateSeoArticle(int id, string title, string key, string desc)
        {
           
             TableSeoArticle ta = context.TableSeoArticles.Where(i => i.ID == id).FirstOrDefault();
                    ta.Title = title;
                    ta.Keywords = key;
                    ta.Description = desc;
              context.Entry(ta).State = System.Data.Entity.EntityState.Modified;
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

        public TableSeoArticle GetSeoArticle(int id)
        {
            return context.TableSeoArticles.Where(x => x.ID == id).FirstOrDefault();
        }
    }
}
