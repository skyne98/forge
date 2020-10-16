using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Forge.Server.Data
{
    public class LiteDbOptions
    {
        public string DatabaseLocation { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database.db");
    }
}
