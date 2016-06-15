using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;

namespace Domain2.Interfaces
{
   public interface IUserRepository
    {
        void CreateUser(int id, string name, string password);
        void DeleteUser(int id);
        IEnumerable<User> GetUsers();
    }
}
