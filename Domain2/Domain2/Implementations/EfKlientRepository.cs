using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;
using Domain2.Interfaces;

namespace Domain2.Implementations
{
    public class EfKlientRepository : IKlientRepository
    {
        private dbdveri1Entities1 context;
        public EfKlientRepository(dbdveri1Entities1 context)
        {
            this.context = context;
        }
        public void CreateKlient(int iddv, string name, string phone, string adres, string quest, string type)
        {
            Klient k = new Klient
            {
                Id = 0,
                Name = name,
                Phone = phone,
                Adres = adres,
                Question = quest,
                TypDveri = type,
                IdDveri = iddv,
                DateMessage = DateTime.Now              
            };
            context.Klients.Add(k);
            context.SaveChanges();
        }

        public void DellClient(int id)
        {
            context.Klients.Remove(context.Klients.Where(z=>z.Id == id).FirstOrDefault());
            context.SaveChanges();

        }

        public IEnumerable<Klient> GetKlient()
        {
            return context.Klients;
        }
    }
}
