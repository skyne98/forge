using Forge.Shared.Data;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Server.Data
{
    public class LiteDbUserService: ILiteDbUserService
    {
        private LiteDatabase _liteDb;

        public LiteDbUserService(ILiteDbContext liteDbContext)
        {
            _liteDb = liteDbContext.Database;
        }

        public IEnumerable<UserModel> FindAll()
        {
            return _liteDb.GetCollection<UserModel>("User")
                .FindAll();
        }

        public UserModel FindOne(Guid id)
        {
            return _liteDb.GetCollection<UserModel>("User")
                .Find(x => x.Id == id).FirstOrDefault();
        }

        public Guid Insert(UserModel user)
        {
            return _liteDb.GetCollection<UserModel>("User")
                .Insert(user);
        }

        public bool Update(UserModel user)
        {
            return _liteDb.GetCollection<UserModel>("User")
                .Update(user);
        }

        public bool Delete(Guid id)
        {
            return _liteDb.GetCollection<UserModel>("User")
                .Delete(id);
        }
    }
}
