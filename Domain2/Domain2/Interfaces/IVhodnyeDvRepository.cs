using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;

namespace Domain2.Interfaces
{
   public interface IVhodnyeDvRepository
    {
        IEnumerable<VhodnyeDveri> GetVhodnyeDv();
        VhodnyeDveri GetVhodnyeDvById(int id);
        void CreateVhodnyeDv(int id, string naz, string pr, string strpr, string cvet, string napoln, string yplotnit, double? tmet, string furn, string petli, string osn, string ovn, double? tdpol, decimal? cena, int? skidka, decimal? csskid, string opis, bool publ, string dopchar);
        void SaveVhodnyeDv(VhodnyeDveri temp);
        void DeleteVhodnyeDv(int id);
        IEnumerable<FotoVhodnyhDverey> GetFotoVhDv();

        IEnumerable<FotoVhodnyhDverey> GetFotoVhDvByID(int id);
        void CreateFotoVhDv(int id, int iddv, string Type, byte[] Image);
        void DeleteFotoVhDv(int idfoto);
        void CreateSeoVhDveri(int id, string title, string keywords, string description);
        IEnumerable<SeoVhodnuhDverei> GetSeoVhDv();
    }
}
