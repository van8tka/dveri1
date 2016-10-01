using System.Collections.Generic;
using DALdv1;

namespace Domain2.Interfaces
{
    public interface IColorsRepository
    {
        IEnumerable<TableColor> GetColors();
        TableColor GetColor(int idcolor);
        void CreateColor(int idcolor, string namecolor, string type, byte[] image);
        void DelColor(int idcolor);
    }
}
