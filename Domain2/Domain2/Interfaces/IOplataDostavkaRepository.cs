using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;

namespace Domain2.Interfaces
{
    public interface IOplataDostavkaRepository
    {
        IEnumerable<Dostavka> GetDostavka();
        IEnumerable<Oplata> GetOplata();
        void CreateDostavka(int id, string d);
        void CreateOplata(int id, string o);
        void DellOplata(int id);
        void DellDostavka(int id);
    }
}
