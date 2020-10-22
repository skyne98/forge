using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Shared.Data
{
    public class CharacterTagModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                CharacterTagModel t = (CharacterTagModel)obj;
                return Id == t.Id;
            }
        }
    }
}
