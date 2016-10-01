using System.Collections.Generic;
using DALdv1;

namespace dveri1.Models
{
    public class ModelColors
    {
        public IEnumerable<TableColor> ColorItems { get; set; }
        public string MimeTypeColor { get; set; }
        public byte[] ImgData { get; set; }
        public string NameColor { get; set; }
        public int CountItemColor { get; set; }
    }
}