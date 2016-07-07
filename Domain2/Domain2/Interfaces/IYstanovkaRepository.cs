using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;

namespace Domain2.Interfaces
{
   public interface IYstanovkaRepository
    {
        IEnumerable<TableYstanovka> GetYstanovka();
        void CreateYstanovka(int id, string yst);
        void DelYstanovka(int id);
    }
}
