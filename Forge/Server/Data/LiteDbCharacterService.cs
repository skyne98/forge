using Forge.Shared.Data;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Server.Data
{
    public class LiteDbCharacterService: ILiteDbCharacterService
    {
        private LiteDatabase _liteDb;

        public LiteDbCharacterService(ILiteDbContext liteDbContext)
        {
            _liteDb = liteDbContext.Database;
        }

        public IEnumerable<CharacterModel> FindAll(bool includeDeleted = false)
        {
            return _liteDb.GetCollection<CharacterModel>("Character")
                    .Include(x => x.Tags)
                    .Find(x => (x.IsDeleted == false) || includeDeleted);
        }

        public CharacterModel FindOne(Guid id, bool includeDeleted = false)
        {
            return _liteDb.GetCollection<CharacterModel>("Character")
                    .Include(x => x.Tags)
                    .Find(x => x.Id == id && ((x.IsDeleted == false) || includeDeleted))
                    .FirstOrDefault();
        }

        public IEnumerable<CharacterModel> FindRange(Guid[] ids, bool includeDeleted = false)
        {
            return _liteDb.GetCollection<CharacterModel>("Character")
                    .Include(x => x.Tags)
                    .Find(x => ((x.IsDeleted == false) || includeDeleted) && ids.Contains(x.Id));
        }

        public Guid Insert(CharacterModel character)
        {
            return _liteDb.GetCollection<CharacterModel>("Character")
                .Insert(character);
        }

        public bool Update(CharacterModel character)
        {
            return _liteDb.GetCollection<CharacterModel>("Character")
                .Update(character);
        }
        
        public bool DeleteOne(Guid id)
        {
            var character = FindOne(id);
            character.IsDeleted = true;
            return Update(character);
        }

        public bool DeleteRange(Guid[] ids)
        {
            var characters = FindRange(ids);
            var result = false;
            foreach(var character in characters)
            {
                character.IsDeleted = true;
                result = result || Update(character);
            }
            return result;
        }

        public bool RestoreOne(Guid id)
        {
            var character = FindOne(id, true);
            character.IsDeleted = false;
            return Update(character);
        }

        public bool RestoreRange(Guid[] ids)
        {
            var characters = FindRange(ids, true);
            var result = false;
            foreach(var character in characters)
            {
                character.IsDeleted = false;
                result = result || Update(character);
            }
            return true;
        }
    }
}
