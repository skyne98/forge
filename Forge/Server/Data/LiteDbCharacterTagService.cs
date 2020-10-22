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

        public IEnumerable<CharacterTagModel> FindAll(bool includeDeleted = false)
        {
            return _liteDb.GetCollection<CharacterTagModel>("CharacterTag")
                    .Find(x => x.IsDeleted == false || includeDeleted);
            
        }

        public CharacterTagModel FindOne(Guid id, bool includeDeleted = false)
        {
            
            return _liteDb.GetCollection<CharacterTagModel>("CharacterTag")
                    .Find(x => x.Id == id && (x.IsDeleted == false || includeDeleted))
                    .FirstOrDefault();
        }

        public IEnumerable<CharacterTagModel> FindRange(Guid[] ids, bool includeDeleted = false)
        {
            return _liteDb.GetCollection<CharacterTagModel>("CharacterTag")
                    .Find(x => ((x.IsDeleted == false) || includeDeleted) && ids.Contains(x.Id));
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
            var tag = FindOne(id);
            tag.IsDeleted = true;
            return Update(tag);
        }

        public bool DeleteRange(Guid[] ids)
        {
            var tags = FindRange(ids);
            var result = false;
            foreach(var tag in tags)
            {
                tag.IsDeleted = true;
                result = result || Update(tag);
            }
            return true;
        }

        public bool RestoreOne(Guid id)
        {
            var tag = FindOne(id, true);
            tag.IsDeleted = false;
            return Update(tag);
        }

        public bool RestoreRange(Guid[] ids)
        {
            var tags = FindRange(ids, true);
            var result = false;
            foreach(var tag in tags)
            {
                tag.IsDeleted = false;
                result = result || Update(tag);
            }

            return true;
        }
    }
}
