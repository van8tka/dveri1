using DALdv1;
using System.Collections.Generic;

namespace dveri1.Models
{
    public class KartochkaTovaraModel
    {
        public VhodnyeDveri Tovar { get; set; }
        public IEnumerable<FotoVhodnyhDverey> FotoTovara { get; set; }
        public string InfoDostavka { get; set; }
        public string InfoOplata { get; set; }
    }
}