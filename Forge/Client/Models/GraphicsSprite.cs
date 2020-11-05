using Forge.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Client.Models
{
    public class GraphicsSprite: GraphicsContainer
    {
        public double Width { get => _pixiService.GetSpriteMember<double>(_target, Id, new List<string> { "width" }); set => _pixiService.SetSpriteMember(_target, Id, new List<string> { "width" }, value); }
        public double Height { get => _pixiService.GetSpriteMember<double>(_target, Id, new List<string> { "height" }); set => _pixiService.SetSpriteMember(_target, Id, new List<string> { "height" }, value); }

        public double AnchorX { get => _pixiService.GetSpriteMember<double>(_target, Id, new List<string> { "anchor", "x" }); set => _pixiService.SetSpriteMember(_target, Id, new List<string> { "anchor", "x" }, value); }
        public double AnchorY { get => _pixiService.GetSpriteMember<double>(_target, Id, new List<string> { "anchor", "y" }); set => _pixiService.SetSpriteMember(_target, Id, new List<string> { "anchor", "y" }, value); }

        public GraphicsSprite(int id, string target, PixiService pixiService) : base(id, target, pixiService)
        {

        }

        public void SetAnchor(double anchor)
        {
            AnchorX = anchor;
            AnchorY = anchor;
        }
    }
}
