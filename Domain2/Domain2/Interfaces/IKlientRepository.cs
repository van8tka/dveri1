using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;

namespace Domain2.Interfaces
{
   public interface IKlientRepository
    {
        IEnumerable<Klient> GetKlient();
        void CreateKlient(int iddv, string name, string phone, string adres, string quest, string type);
        void DellClient(int id); 
    }
}
