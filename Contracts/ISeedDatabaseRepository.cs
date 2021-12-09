using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ISeedDatabaseRepository
    {
        Task<bool> seedRoles();
    }
}
