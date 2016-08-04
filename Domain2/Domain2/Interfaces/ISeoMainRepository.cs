using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;

namespace Domain2.Interfaces
{
   public interface ISeoMainRepository
    {
        void DellSeo(int id);
        void CreateSeo(int id, string tit, string key, string des, string page, string head,string categ);
        IEnumerable<SeoMain> GetSeoMain();
        SeoMain GetSeoMainByPage(string p);
    }
}
