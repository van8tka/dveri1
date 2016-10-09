using System.Linq;
using DALdv1;
using Domain2.Interfaces;
using System.Data.Entity;

namespace Domain2.Implementations
{
    public class EfAboutUsRepository : IAboutUsRepository
    {
        dbdveri1Entities1 context;
        public EfAboutUsRepository(dbdveri1Entities1 context)
        {
            this.context = context;
        }
        public void CreateAboutUs(int id, string text)
        {
            if(id==0)
            {
                TableAboutU ta = new TableAboutU()
                {
                    Id = id,
                    TextAboutUs = text
                };

                TableAboutU existta = context.TableAboutUs.FirstOrDefault();
                if (existta != null)
                {
                    context.TableAboutUs.Remove(existta);
                }
                context.TableAboutUs.Add(ta);
            }
            context.SaveChanges();
        }

        public TableAboutU GetAboutUs()
        {
            return context.TableAboutUs.FirstOrDefault();
        }
    }
}
