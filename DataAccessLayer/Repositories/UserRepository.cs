using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : EditableRepositoryBase<User>
    {

        public User SearchByUsername(string username)
        {
           return _context.User.FirstOrDefault(user => user.Username.Equals(username));
        }

        public User SearchByEmail(string email)
        {
            return _context.User.FirstOrDefault(user => user.Email.Equals(email));
        }
    }
}
