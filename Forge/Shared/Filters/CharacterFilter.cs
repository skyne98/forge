﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Shared.Filters
{
    public class CharacterFilter: BaseFilter
    {
        public string Name { get; set; } = String.Empty;
        public List<Guid> Tags { get; set; } = new List<Guid>();

        public override bool Equals(object obj)
        {
            var item = obj as CharacterFilter;

            if (item == null)
            {
                return false;
            }

            return this.Name.Equals(item.Name) && new HashSet<Guid>(this.Tags).SetEquals(item.Tags);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}
