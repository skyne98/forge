using Forge.Shared.Data;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Server.Data
{
    public class LiteDbUserService: IDbUserRepository
    {
        private LiteDatabase _liteDb;

        public LiteDbUserService(ILiteDbContext liteDbContext)
        {
            _liteDb = liteDbContext.Database;
        }

        public IEnumerable<UserModel> FindAll(bool includeDeleted = false)
        {
            return _liteDb.GetCollection<UserModel>("User")
                    .Find(x => x.Deleted == false || includeDeleted);

        }

        public UserModel FindOne(Guid id, bool includeDeleted = false)
        {

            return _liteDb.GetCollection<UserModel>("User")
                    .Find(x => x.Id == id && (x.Deleted == false || includeDeleted))
                    .FirstOrDefault();
        }

        public IEnumerable<UserModel> FindRange(Guid[] ids, bool includeDeleted = false)
        {
            return _liteDb.GetCollection<UserModel>("User")
                    .Find(x => ((x.Deleted == false) || includeDeleted) && ids.Contains(x.Id));
        }

        public Guid Insert(UserModel tag)
        {
            return _liteDb.GetCollection<UserModel>("User")
                .Insert(tag);
        }

        public bool Update(UserModel tag)
        {
            return _liteDb.GetCollection<UserModel>("User")
                .Update(tag);
        }
    }
}
