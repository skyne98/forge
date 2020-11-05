using Forge.Client.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Client.Models
{
    public class GraphicsEngine
    {
        /// <summary>
        /// Fraction of 60fps. If 30fps, then 2. If 120fps, then 0.5.
        /// </summary>
        public double Delta { get; set; }
        /// <summary>
        /// Ellapsed time in seconds.
        /// </summary>
        public double Ellapsed { get; set; }
        public IReadOnlyCollection<GraphicsDisplayObject> DisplayObjects { get => _children.AsReadOnly(); }
        public Size ScreenSize { get { var size = _pixiService.GetScreenSize(_target); return new Size((int)size[0], (int)size[1]); } }

        private string _target;
        private PixiService _pixiService;
        private List<GraphicsDisplayObject> _children;

        public GraphicsEngine(string target, PixiService pixiService)
        {
            _target = target;
            _pixiService = pixiService;
            _children = new List<GraphicsDisplayObject>();
        }

        public GraphicsContainer CreateContainer()
        {
            var id = _pixiService.Container(_target);
            var result = new GraphicsContainer(id, _target, _pixiService);

            _children.Add(result);

            return result;
        }

        public GraphicsSprite CreateSprite(string uri)
        {
            var id = _pixiService.Sprite(_target, uri);
            var result = new GraphicsSprite(id, _target, _pixiService);

            _children.Add(result);

            return result;
        }

        public void AddToStage(GraphicsDisplayObject displayObject)
        {
            _pixiService.AddDisplayObjectToStage(_target, displayObject.Id);
        }

        public void RemoveFromStage(GraphicsDisplayObject displayObject)
        {
            _pixiService.RemoveDisplayObjectFromStage(_target, displayObject.Id);
        }
    }
}
