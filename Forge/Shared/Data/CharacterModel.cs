using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Shared.Data
{
    public class CharacterModel: IModelIndexed, IModelDeletable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Deleted { get; set; }

        [BsonRef("CharacterTag")]
        public List<CharacterTagModel> Tags { get; set; }
    }
}
