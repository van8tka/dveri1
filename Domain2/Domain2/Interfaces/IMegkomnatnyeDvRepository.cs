using System.Collections.Generic;
using DALdv1;

namespace Domain2.Interfaces
{
    public interface IMegkomnatnyeDvRepository
    {
        IEnumerable<MegkomnatnyeDveri> GetMkDv();
       MegkomnatnyeDveri GetMkDvById(int id);
        void CreateMkDv(int id, string naz, string pr, string strpr, string cvet, string material, string pokryt, string karkas, string typdv, string vnytrnapoln, decimal? cena, int? skidka, decimal? csskid, string opis, bool publ, string dopchar);
        void SaveMkDv(MegkomnatnyeDveri temp);
        void DeleteMkDv(int id);
        IEnumerable<FotoMegkomnDverey> GetFotoMkDv();

        IEnumerable<FotoMegkomnDverey> GetFotoMkDvByID(int id);
        void CreateFotoMkDv(int id, int iddv, string Type, byte[] Image);
        void DeleteFotoMkDv(int idfoto);
        void CreateSeoMkDveri(int id, string title, string keywords, string description);
        IEnumerable<SeoMkDverei> GetSeoMkDv();
    }
}
