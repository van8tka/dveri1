using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALdv1;
using Domain2.Interfaces;

namespace Domain2.Implementations
{
    public class EfUserRepository : IUserRepository
    {
        private dbdveri1Entities1 context;
        public EfUserRepository(dbdveri1Entities1 context)
        {
            this.context = context;
        }
        public void CreateUser(int id, string name, string password)
        {
            User user = new User
            {
                ID = id,
                Name = name,
                Password = password
            };
            context.Users.Add(user);
            context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            context.Users.Remove(context.Users.Where(i => i.ID == id).FirstOrDefault());
            context.SaveChanges();
        }

        public IEnumerable<User> GetUsers()
        {
            return context.Users;
        }
    }
}
