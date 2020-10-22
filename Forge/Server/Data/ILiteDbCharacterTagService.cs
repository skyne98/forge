using Forge.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Server.Data
{
    public interface ILiteDbCharacterTagService
    {
        bool Delete(Guid id);
        bool DeleteOne(Guid id);
        bool RestoreOne(Guid id);
        IEnumerable<CharacterTagModel> FindAll();
        CharacterTagModel FindOne(Guid id);
        Guid Insert(CharacterTagModel tag);
        bool Update(CharacterTagModel tag);
    }
}
