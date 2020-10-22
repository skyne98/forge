using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Shared.Data
{
    public class CharacterModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [BsonRef("CharacterTag")]
        public List<CharacterTagModel> Tags { get; set; }

        public bool IsDeleted { get; set; }
    }
}
