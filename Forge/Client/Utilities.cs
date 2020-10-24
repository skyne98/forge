using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Client
{
    public class Utilities
    {
        public static System.Guid EnsureStartsWithLetter(System.Guid guid)
        {
            var bytes = guid.ToByteArray();

            if ((bytes[3] & 0xf0) < 0xa0)
            {
                bytes[3] |= 0xc0;
                return new System.Guid(bytes);
            }
            return guid;
        }
    }
}
