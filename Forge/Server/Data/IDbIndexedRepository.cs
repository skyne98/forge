using Forge.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Server.Data
{
    public interface IDbIndexedRepository<TModel> where TModel: IModelIndexed
    {
        IEnumerable<TModel> FindAll(bool includeDeleted = false);
        TModel FindOne(Guid id, bool includeDeleted = false);
        IEnumerable<TModel> FindRange(Guid[] ids, bool includeDeleted = false);
        Guid Insert(TModel model);
        bool Update(TModel model);
    }
}
