using Forge.Client.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Client.Models
{
    public class GraphicsContainer: GraphicsDisplayObject
    {
        public IReadOnlyCollection<GraphicsDisplayObject> Children { get => _children.AsReadOnly(); }

        protected List<GraphicsDisplayObject> _children;

        public GraphicsContainer(int id, string target, PixiService pixiService): base(id, target, pixiService)
        {
            _children = new List<GraphicsDisplayObject>();
        }

        public void AddChild(GraphicsDisplayObject graphicsDisplayObject)
        {
            _pixiService.AddDisplayObjectToContainer(_target, graphicsDisplayObject.Id, Id);
            _children.Add(graphicsDisplayObject);
        }

        public void RemoveChild(GraphicsDisplayObject graphicsDisplayObject)
        {
            _pixiService.RemoveDisplayObjectFromContainer(_target, graphicsDisplayObject.Id, Id);
            _children.Add(graphicsDisplayObject);
        }
    }
}
