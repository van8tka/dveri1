using DALdv1;
using System.Collections.Generic;

namespace dveri2.Models
{
    public class KartochkaTovaraModel
    {
        public MegkomnatnyeDveri Tovar { get; set; }
        public IEnumerable<FotoMegkomnDverey> FotoTovara { get; set; }
        public IEnumerable<CommentMkDv> CommentMkDvList { get; set; }
        public bool FlagMaterial { get; set; }
        public string InfoDostavka { get; set; }
        public string InfoOplata { get; set; }
        public string TitleD { get; set; }
        public string KeyD { get; set; }
        public string DescrD { get; set; }
    }
}