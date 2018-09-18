using DataAccessLayer.Classes;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class SystemParameterRepository : EditableRepositoryBase<SystemParameter>
    {

        public SystemParameter GetByParameterName(string name)
        {
            return context.SystemParameters.Single(parameter => parameter.ParameterName.Equals(name));
        }

    }
}
