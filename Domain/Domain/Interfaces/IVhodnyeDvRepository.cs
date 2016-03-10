using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;

namespace Domain.Interfaces
{
   public interface IVhodnyeDvRepository
    {
        IEnumerable<VhodnyeDveri> GetVhodnyeDv();
        VhodnyeDveri GetVhodnyeDvById(int id);
        void CreateVhodnyeDv(int id, string naz, string pr, string strpr, string cvet, string napoln, string yplotnit, int tmet, string furn, string petli, string osn, string ovn, int tdpol, int cena, int skidka, int csskid, string opis, bool publ);
        void SaveVhodnyeDv(VhodnyeDveri temp);
        void DeleteVhodnyeDv(int id);

    }
}
