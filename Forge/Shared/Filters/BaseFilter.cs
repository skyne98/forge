using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Shared.Filters
{
    public class BaseFilter
    {
        public int? Take { get; set; }
        public int? Skip { get; set; }
        public string Sorting { get; set; }
        public SortingDirection SortingDirection { get; set; }
    }

    public enum SortingDirection
    {
        Ascending,
        Descending
    }
}
