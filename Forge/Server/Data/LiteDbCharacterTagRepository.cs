using Forge.Shared.Data;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Server.Data
{
    public class LiteDbCharacterTagRepository : IDbCharacterTagRepository
    {
        private LiteDatabase _liteDb;

        public LiteDbCharacterTagRepository(ILiteDbContext liteDbContext)
        {
            _liteDb = liteDbContext.Database;
        }

        public IEnumerable<CharacterTagModel> FindAll(bool includeDeleted = false)
        {
            return _liteDb.GetCollection<CharacterTagModel>("CharacterTag")
                    .Find(x => x.Deleted == false || includeDeleted);

        }

        public CharacterTagModel FindOne(Guid id, bool includeDeleted = false)
        {

            return _liteDb.GetCollection<CharacterTagModel>("CharacterTag")
                    .Find(x => x.Id == id && (x.Deleted == false || includeDeleted))
                    .FirstOrDefault();
        }

        public IEnumerable<CharacterTagModel> FindRange(Guid[] ids, bool includeDeleted = false)
        {
            return _liteDb.GetCollection<CharacterTagModel>("CharacterTag")
                    .Find(x => ((x.Deleted == false) || includeDeleted) && ids.Contains(x.Id));
        }

        public Guid Insert(CharacterTagModel tag)
        {
            return _liteDb.GetCollection<CharacterTagModel>("CharacterTag")
                .Insert(tag);
        }

        public bool Update(CharacterTagModel tag)
        {
            return _liteDb.GetCollection<CharacterTagModel>("CharacterTag")
                .Update(tag);
        }
    }
}
