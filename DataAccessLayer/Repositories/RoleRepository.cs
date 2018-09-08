using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class RoleRepository : ReadOnlyRepositoryBase<Role>
    {

        public Role GetByType(string type)
        {
            return _context.Role.FirstOrDefault(role => role.Type.Equals(type));
        }

    }
}
