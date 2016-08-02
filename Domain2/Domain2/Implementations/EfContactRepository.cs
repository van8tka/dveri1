using Domain2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;
using System.Data.Entity;

namespace Domain2.Implementations
{
    public class EfContactRepository : IContactRepository
    {
       private dbdveri1Entities1 context;
        public EfContactRepository(dbdveri1Entities1 context)
        {
            this.context = context;
        }
        public void CreateYrInfa(int id,string yr)
        {
            if(id==0)
            {
                TableYrInfa y = new TableYrInfa()
                {
                    Id=id,
                    YrInfa = yr
                };
                context.TableYrInfas.Add(y);
                context.SaveChanges();             
            }
            else
            {
                TableYrInfa y = context.TableYrInfas.Where(i => i.Id == id).FirstOrDefault();
                y.YrInfa = yr;
                context.Entry(y).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        public void DellYrInfa(int id)
        {
            TableYrInfa y = context.TableYrInfas.Where(i => i.Id == id).FirstOrDefault();
            context.TableYrInfas.Remove(y);
            context.SaveChanges();
        }
        public TableYrInfa GetYrInfa()
        {
            return context.TableYrInfas.FirstOrDefault();
        }
        public void CreateAdres(int id, string adres)
        {
            if(id==0)
            {
                Adresa a = new Adresa()
                {
                    Id=id,
                    AdresName = adres
                };
                context.Adresas.Add(a);
                context.SaveChanges();
            }
            else
            {
                Adresa a = context.Adresas.Where(i => i.Id == id).FirstOrDefault();
                a.AdresName = adres;
                context.Entry(a).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void CreateContact(int id, string type, string num)
        {
            if (id == 0)
            {
               Contact con = new Contact()
                {
                    Id = id,
                   TypeSv = type,
                   NumberSv = num
                };
                context.Contacts.Add(con);
                context.SaveChanges();
            }
            else
            {
                Contact con = context.Contacts.Where(i => i.Id == id).FirstOrDefault();
                con.NumberSv = num;
                con.TypeSv = type;
                context.Entry(con).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void CreateGrafikWork(int id, string day, string time)
        {
            if (id == 0)
            {
                GrafikWork gr = new GrafikWork()
                {
                    Id = id,
                    NameDay = day,
                    TimeWork = time
                };
                context.GrafikWorks.Add(gr);
                context.SaveChanges();
            }
            else
            {
                GrafikWork gr = context.GrafikWorks.Where(i => i.Id == id).FirstOrDefault();
                gr.NameDay = day;
                gr.TimeWork = time;
                context.Entry(gr).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DellAdres(int id)
        {
            context.Adresas.Remove(context.Adresas.Where(i => i.Id == id).FirstOrDefault());
            context.SaveChanges();
        }

        public void DellContact(int id)
        {
            context.Contacts.Remove(context.Contacts.Where(i => i.Id == id).FirstOrDefault());
            context.SaveChanges();
        }

        public void DellGrafikWork(int id)
        {
            context.GrafikWorks.Remove(context.GrafikWorks.Where(i => i.Id == id).FirstOrDefault());
            context.SaveChanges();
        }

        public IEnumerable<Adresa> GetAdres()
        {
            return context.Adresas;
        }

        public IEnumerable<Contact> GetContacts()
        {
            return context.Contacts;
        }

        public IEnumerable<GrafikWork> GetGrafikWork()
        {
            return context.GrafikWorks;
        }
    }
}
