using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;

namespace Domain2.Interfaces
{
    public interface IOurWorksRepository
    {
        IEnumerable<TableOurWork> GetOurWorks();
        IEnumerable<TableFotoOurWork> GetFotosWorkById(int id);
        void DellOurWork(int id);
        void DellFotoOurWork(int idfoto);
        void CreateOurWork(int id, string art);
        void CreateFotoOurWork(int idfoto, int idow, string type, byte[] img);
    }
}
