using Forge.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Server.Data
{
    public interface ILiteDbCharacterService
    {
        bool DeleteOne(Guid id);
        bool DeleteRange(Guid[] ids);
        bool RestoreOne(Guid id);
        bool RestoreRange(Guid[] ids);
        IEnumerable<CharacterModel> FindAll(bool includeDeleted = false);
        CharacterModel FindOne(Guid id, bool includeDeleted = false);
        List<CharacterModel> FindRange(Guid[] ids, bool includeDeleted = false);
        Guid Insert(CharacterModel user);
        bool Update(CharacterModel user);
    }
}
