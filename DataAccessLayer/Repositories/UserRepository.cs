using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Classes;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : EditableRepositoryBase<User>
    {

        public User SearchByUsername(string username)
        {
           return context.Users.FirstOrDefault(user => user.Username.Equals(username));
        }

        public User SearchByEmail(string email)
        {
            return context.Users.FirstOrDefault(user => user.Email.Equals(email));
        }
    }
}
