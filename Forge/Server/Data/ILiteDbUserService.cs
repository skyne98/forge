using Forge.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Server.Data
{
    public interface ILiteDbUserService
    {
        bool Delete(Guid id);
        IEnumerable<UserModel> FindAll();
        UserModel FindOne(Guid id);
        Guid Insert(UserModel user);
        bool Update(UserModel user);
    }
}
