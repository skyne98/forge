using Forge.Shared.Data;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Server.Data
{
    public class LiteDbCharacterTagService: ILiteDbCharacterTagService
    {
        private LiteDatabase _liteDb;

        public LiteDbCharacterTagService(ILiteDbContext liteDbContext)
        {
            _liteDb = liteDbContext.Database;
        }

        public IEnumerable<CharacterTagModel> FindAll()
        {
            return _liteDb.GetCollection<CharacterTagModel>("CharacterTag")
                .FindAll();
        }

        public CharacterTagModel FindOne(Guid id)
        {
            return _liteDb.GetCollection<CharacterTagModel>("CharacterTag")
                .Find(x => x.Id == id).FirstOrDefault();
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

        public bool DeleteOne(Guid id)
        {
            CharacterTagModel tag = _liteDb.GetCollection<CharacterTagModel>("CharacterTag").Find(x => x.Id == id).FirstOrDefault();
            tag.IsDeleted = true;
            return _liteDb.GetCollection<CharacterTagModel>("CharacterTag").Update(tag);
        }

        public bool RestoreOne(Guid id)
        {
            CharacterTagModel tag = _liteDb.GetCollection<CharacterTagModel>("CharacterTag").Find(x => x.Id == id).FirstOrDefault();
            tag.IsDeleted = false;
            return _liteDb.GetCollection<CharacterTagModel>("CharacterTag").Update(tag);
        }

        public bool Delete(Guid id)
        {
            return _liteDb.GetCollection<CharacterModel>("CharacterTag")
                .Delete(id);
        }
    }
}
