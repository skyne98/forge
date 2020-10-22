using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Shared.Filters
{
    public class CharacterFilter
    {
        public string Name { get; set; } = String.Empty;
        public List<Guid> Tags { get; set; } = new List<Guid>();
    }
}
