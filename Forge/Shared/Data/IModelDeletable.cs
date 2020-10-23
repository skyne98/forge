using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Shared.Data
{
    public interface IModelDeletable
    {
        bool Deleted { get; set; }
    }
}
