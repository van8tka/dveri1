using System.Collections.Generic;
using System.Linq;
using DALdv1;
using Domain2.Interfaces;

namespace Domain2.Implementations
{
    public class EfOurWorksRepository:IOurWorksRepository
    {
        dbdveri1Entities1 context;
        public EfOurWorksRepository(dbdveri1Entities1 context)
        {
            this.context = context;
        }

        public void CreateFotoOurWork(int idfoto, int idow, string type, byte[] img)
        {
            if(idfoto==0)
            {
                TableFotoOurWork tf = new TableFotoOurWork()
                {
                    IdFoto = 0,
                    Id = idow,
                    MymeType = type,
                    Image = img
                };
                context.TableFotoOurWorks.Add(tf);
                context.SaveChanges();
            }
        }

        public void CreateOurWork(int id, string art)
        {
            if(id==0)
            {
                TableOurWork to = new TableOurWork() {
                    Id=0,
                    TextOurWorks = art
                };
                context.TableOurWorks.Add(to);
            }
            else
            {
                TableOurWork to = context.TableOurWorks.Where(x => x.Id == id).FirstOrDefault();
                to.TextOurWorks = art;
                context.Entry(to).State = System.Data.Entity.EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DellFotoOurWork(int idfoto)
        {
            TableFotoOurWork t = context.TableFotoOurWorks.Where(x => x.IdFoto == idfoto).FirstOrDefault();
            context.TableFotoOurWorks.Remove(t);
            context.SaveChanges();
        }

        public void DellOurWork(int id)
        {
            TableOurWork t = context.TableOurWorks.Where(x => x.Id == id).FirstOrDefault();
            context.TableOurWorks.Remove(t);
            context.SaveChanges();
        }

        public IEnumerable<TableFotoOurWork> GetFotosWorkById(int id)
        {
            return context.TableFotoOurWorks.Where(i => i.Id == id);
        }

        public IEnumerable<TableOurWork> GetOurWorks()
        {
            return context.TableOurWorks;
        }
    }
}
