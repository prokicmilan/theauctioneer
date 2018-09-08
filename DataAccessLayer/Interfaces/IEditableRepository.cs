using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    interface IEditableRepository<T>
    {
        void Save(T entity);

        void Remove(T entity);
    }
}
