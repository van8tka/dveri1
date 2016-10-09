using DALdv1;

namespace Domain2.Interfaces
{
   public interface IAboutUsRepository
    {
        TableAboutU GetAboutUs();
        void CreateAboutUs(int id, string text);
    }
}
