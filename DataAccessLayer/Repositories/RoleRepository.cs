using System.Linq;
using DataAccessLayer.Classes;

namespace DataAccessLayer.Repositories
{
    public class RoleRepository : ReadOnlyRepositoryBase<Role>
    {

        public Role GetByType(string type)
        {
            return context.Roles.FirstOrDefault(role => role.Type.Equals(type));
        }

    }
}
