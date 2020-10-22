using Forge.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Server.Data
{
    public interface ILiteDbCharacterService
    {
        bool Delete(Guid id);
        IEnumerable<CharacterModel> FindAll();
        CharacterModel FindOne(Guid id);
        Guid Insert(CharacterModel user);
        bool Update(CharacterModel user);
    }
}
