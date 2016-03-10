using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;
using Domain.Interfaces;

namespace Domain.Implementations
{
   public class EfVhodnyeDvRepository:IVhodnyeDvRepository
    {
        private dbdveri1Entities1 context;
        public EfVhodnyeDvRepository(dbdveri1Entities1 context)
        {
            this.context = context;
        }

        public void CreateVhodnyeDv(int id, string naz, string pr, string strpr, string cvet, string napoln, string yplotnit, int tmet, string furn, string petli, string osn, string ovn, int tdpol, int cena, int skidka, int csskid, string opis, bool publ)
        {
            throw new NotImplementedException();
        }

        public void DeleteVhodnyeDv(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VhodnyeDveri> GetVhodnyeDv()
        {
            throw new NotImplementedException();
        }

        public VhodnyeDveri GetVhodnyeDvById(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveVhodnyeDv(VhodnyeDveri temp)
        {
            throw new NotImplementedException();
        }
    }
}
