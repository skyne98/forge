using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Shared.Filters
{
    public class ImageFilter: BaseFilter
    {
        public string Title { get; set; } = String.Empty;
        public string Body { get; set; } = String.Empty;

        public override bool Equals(object obj)
        {
            var item = obj as ImageFilter;

            if (item == null)
            {
                return false;
            }

            return Title.Equals(item.Title) && Body.Equals(item.Body);
        }

        public override int GetHashCode()
        {
            return (Title + Body).GetHashCode();
        }
    }
}
