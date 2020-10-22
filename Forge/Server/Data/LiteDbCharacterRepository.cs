using Forge.Shared.Data;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Server.Data
{
    public class LiteDbCharacterRepository: IDbCharacterRepository
    {
        private LiteDatabase _liteDb;

        public LiteDbCharacterRepository(ILiteDbContext liteDbContext)
        {
            _liteDb = liteDbContext.Database;
        }

        public IEnumerable<CharacterModel> FindAll(bool includeDeleted = false)
        {
            return _liteDb.GetCollection<CharacterModel>("Character")
                    .Include(x => x.Tags)
                    .Find(x => (x.Deleted == false) || includeDeleted);
        }

        public CharacterModel FindOne(Guid id, bool includeDeleted = false)
        {
            return _liteDb.GetCollection<CharacterModel>("Character")
                    .Include(x => x.Tags)
                    .Find(x => x.Id == id && ((x.Deleted == false) || includeDeleted))
                    .FirstOrDefault();
        }

        public IEnumerable<CharacterModel> FindRange(Guid[] ids, bool includeDeleted = false)
        {
            return _liteDb.GetCollection<CharacterModel>("Character")
                    .Include(x => x.Tags)
                    .Find(x => ((x.Deleted == false) || includeDeleted) && ids.Contains(x.Id));
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
    }
}
