using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Shared.Data
{
    public class ImageModel : IModelIndexed, IModelDeletable
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        /// <summary>
        /// Type of the file (aka image/png)
        /// </summary>
        public string Type { get; set; }
        public string Filename { get; set; }
        public DateTime Uploaded { get; set; }
        public DateTime Updated { get; set; }
        public bool Deleted { get; set; }
    }
}
