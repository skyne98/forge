using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Shared.Filters
{
    public class ImageFilter: BaseFilter
    {
        public string Title { get; set; } = String.Empty;

        public override bool Equals(object obj)
        {
            var item = obj as ImageFilter;

            if (item == null)
            {
                return false;
            }

            return this.Title.Equals(item.Title);
        }

        public override int GetHashCode()
        {
            return this.Title.GetHashCode();
        }
    }
}
