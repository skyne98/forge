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
            if(includeDeleted)
            {
                return _liteDb.GetCollection<CharacterTagModel>("CharacterTag")
                    .FindAll();
            }
            else
            {
                return _liteDb.GetCollection<CharacterTagModel>("CharacterTag")
                    .Find(x => x.IsDeleted == false);
            }
            
        }

        public CharacterTagModel FindOne(Guid id, bool includeDeleted = false)
        {
            
            if(includeDeleted)
            {
                return _liteDb.GetCollection<CharacterTagModel>("CharacterTag")
                    .Find(x => x.Id == id).FirstOrDefault();
            }
            else
            {
                return _liteDb.GetCollection<CharacterTagModel>("CharacterTag")
                    .Find(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
            }
        }

        public List<CharacterTagModel> FindRange(Guid[] ids, bool includeDeleted = false)
        {
            List<CharacterTagModel> tags = new List<CharacterTagModel>();
            foreach(var id in ids)
            {
                tags.Add(FindOne(id, includeDeleted));
            }
            return tags;
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
            if(tags.Contains(null))
            {
                return false;
            }
            else
            {
                foreach(var tag in tags)
                {
                    tag.IsDeleted = true;
                    Update(tag);
                }
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
            if(tags.Contains(null))
            {
                return false;
            }
            else
            {
                foreach(var tag in tags)
                {
                    tag.IsDeleted = false;
                    Update(tag);
                }
            }
            return true;
        }
    }
}
