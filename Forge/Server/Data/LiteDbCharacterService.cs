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

        public IEnumerable<CharacterModel> FindAll()
        {
            return _liteDb.GetCollection<CharacterModel>("Character")
                .FindAll();
        }

        public CharacterModel FindOne(Guid id)
        {
            return _liteDb.GetCollection<CharacterModel>("Character")
                .Find(x => x.Id == id).FirstOrDefault();
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

        public bool Delete(Guid id)
        {
            return _liteDb.GetCollection<CharacterModel>("Character")
                .Delete(id);
        }
    }
}
