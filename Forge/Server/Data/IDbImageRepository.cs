using Forge.Shared.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Server.Data
{
    public interface IDbImageRepository: IDbIndexedRepository<ImageModel>, IDbDeletableRepository<ImageModel>
    {
        Guid Insert(ImageModel model, string contentBase64);
        bool Update(ImageModel model, string contentBase64 = null);
        Stream GetContent(Guid id);
    }
}
