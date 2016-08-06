using System.Collections.Generic;
using DALdv1;

namespace dveri1.Models
{
    public class ModelAdMenu
    {
        public Dictionary<string, List<MegkomnatnyeDveri>> DictionaryMkDvProizvoditel { get; set; }
        public Dictionary<string, List<MegkomnatnyeDveri>> DictionaryMkDvMaterial { get; set; }
    }
}