using Forge.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Server.Data
{
    public interface ILiteDbCharacterTagService
    {
        bool DeleteOne(Guid id);
        bool DeleteRange(Guid[] ids);
        bool RestoreOne(Guid id);
        bool RestoreRange(Guid[] ids);
        IEnumerable<CharacterTagModel> FindAll(bool includeDeleted = false);
        CharacterTagModel FindOne(Guid id, bool includeDeleted = false);
        IEnumerable<CharacterTagModel> FindRange(Guid[] ids, bool includeDeleted = false);
        Guid Insert(CharacterTagModel tag);
        bool Update(CharacterTagModel tag);
    }
}
