using Forge.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Server.Data
{
    public interface IDbDeletableRepository<TModel>: IDbIndexedRepository<TModel> where TModel: IModelIndexed, IModelDeletable
    {
        bool DeleteOne(Guid id)
        {
            var model = FindOne(id);
            model.Deleted = true;
            return Update(model);
        }

        bool DeleteRange(Guid[] ids)
        {
            var models = FindRange(ids);
            var result = false;
            foreach (var model in models)
            {
                model.Deleted = true;
                result = result || Update(model);
            }
            return result;
        }

        bool RestoreOne(Guid id)
        {
            var model = FindOne(id, true);
            model.Deleted = false;
            return Update(model);
        }

        bool RestoreRange(Guid[] ids)
        {
            var models = FindRange(ids, true);
            var result = false;
            foreach (var model in models)
            {
                model.Deleted = false;
                result = result || Update(model);
            }
            return true;
        }
    }
}
