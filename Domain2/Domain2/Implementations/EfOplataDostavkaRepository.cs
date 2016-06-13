using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;
using Domain2.Interfaces;

namespace Domain2.Implementations
{
    public class EfOplataDostavkaRepository : IOplataDostavkaRepository
    {
       private dbdveri1Entities1 context;
        public EfOplataDostavkaRepository(dbdveri1Entities1 context)
        {
            this.context = context;
        }
        public void CreateDostavka(int id, string d)
        {
            Dostavka dost = new Dostavka
            {
                Id = id,
                Dostavka1 = d
            };
          Dostavka dosta = context.Dostavkas.FirstOrDefault();
            if(dosta!=null)
            {
                DellDostavka(dosta.Id);
            }             
            context.Dostavkas.Add(dost);       
            context.SaveChanges();
        }

        public void CreateOplata(int id, string o)
        {
            Oplata opl = new Oplata
            {
                Id = id,
                Oplata1 = o
            };
           Oplata opla = context.Oplatas.FirstOrDefault();
            if(opla!=null)
            {
                DellOplata(opla.Id);
            }          
            context.Oplatas.Add(opl);
            context.SaveChanges();
        }

        public void DellDostavka(int id)
        {
            context.Dostavkas.Remove(context.Dostavkas.Where(x => x.Id == id).FirstOrDefault());
            context.SaveChanges();
        }

        public void DellOplata(int id)
        {
            context.Oplatas.Remove(context.Oplatas.Where(x => x.Id == id).FirstOrDefault());
            context.SaveChanges();
        }

        public IEnumerable<Dostavka> GetDostavka()
        {
            return context.Dostavkas;
        }

        public IEnumerable<Oplata> GetOplata()
        {
            return context.Oplatas;
        }
    }
}
