using Forge.Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Shared.Requests
{
    public class ImageRequest
    {
        public ImageModel Model { get; set; }
        public string ContentBase64 { get; set; }
    }
}
