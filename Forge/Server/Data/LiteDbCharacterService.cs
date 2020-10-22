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
            if(includeDeleted)
            {
                return _liteDb.GetCollection<CharacterModel>("Character")
                    .Include(x => x.Tags)
                    .FindAll();
            }
            else
            {
                return _liteDb.GetCollection<CharacterModel>("Character")
                    .Include(x => x.Tags)
                    .Find(x => x.IsDeleted == false);
            }
            
        }

        public CharacterModel FindOne(Guid id, bool includeDeleted = false)
        {
            if(includeDeleted)
            {
                return _liteDb.GetCollection<CharacterModel>("Character")
                    .Include(x => x.Tags)
                    .Find(x => x.Id == id)
                    .FirstOrDefault();
            }
            else
            {
                return _liteDb.GetCollection<CharacterModel>("Character")
                    .Include(x => x.Tags)
                    .Find(x => x.Id == id && x.IsDeleted == false)
                    .FirstOrDefault();
            }
            
        }

        public List<CharacterModel> FindRange(Guid[] ids, bool includeDeleted = false)
        {
            List<CharacterModel> characters = new List<CharacterModel>();
            foreach(var id in ids)
            {
                characters.Add(FindOne(id, includeDeleted));
            }
            return characters;
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
            if(characters.Contains(null))
            {
                return false;
            }
            else
            {
                foreach(var character in characters)
                {
                    character.IsDeleted = true;
                    Update(character);
                }
            }
            return true;
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
            if(characters.Contains(null))
            {
                return false;
            }
            else
            {
                foreach(var character in characters)
                {
                    character.IsDeleted = false;
                    Update(character);
                }
            }
            return true;
        }
    }
}
