using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Shared.Filters
{
    public class CharacterTagFilter: BaseFilter
    {
        public string Name { get; set; } = String.Empty;

        public override bool Equals(object obj)
        {
            var item = obj as CharacterTagFilter;

            if (item == null)
            {
                return false;
            }

            return this.Name.Equals(item.Name);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}
