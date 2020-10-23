using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Shared.Data
{
    public class UserModel: IModelIndexed, IModelDeletable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Deleted { get; set; }
    }
}
