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
        IEnumerable<CharacterTagModel> FindAll();
        CharacterTagModel FindOne(Guid id);
        Guid Insert(CharacterTagModel tag);
        bool Update(CharacterTagModel tag);
    }
}
