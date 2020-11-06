using Forge.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace Forge.Client.Models
{
    public class GraphicsDisplayObject
    {
        public int Id { get; private set; }
        public double X { get => _pixiService.GetDisplayObjectMember<double>(_target, Id, new List<string> { "x" }); set => _pixiService.SetDisplayObjectMember(_target, Id, new List<string> { "x" }, value); }
        public double Y { get => _pixiService.GetDisplayObjectMember<double>(_target, Id, new List<string> { "y" }); set => _pixiService.SetDisplayObjectMember(_target, Id, new List<string> { "y" }, value); }
        public double ZIndex { get => _pixiService.GetDisplayObjectMember<double>(_target, Id, new List<string> { "zIndex" }); set => _pixiService.SetDisplayObjectMember(_target, Id, new List<string> { "zIndex" }, value); }

        public double ScaleX { get => _pixiService.GetDisplayObjectMember<double>(_target, Id, new List<string> { "scale", "x" }); set => _pixiService.SetDisplayObjectMember(_target, Id, new List<string> { "scale", "x" }, value); }
        public double ScaleY { get => _pixiService.GetDisplayObjectMember<double>(_target, Id, new List<string> { "scale", "y" }); set => _pixiService.SetDisplayObjectMember(_target, Id, new List<string> { "scale", "y" }, value); }

        /// <summary>
        /// Angle in degrees.
        /// </summary>
        public double Angle { get => _pixiService.GetDisplayObjectMember<double>(_target, Id, new List<string> { "angle" }); set => _pixiService.SetDisplayObjectMember(_target, Id, new List<string> { "angle" }, value); }
        /// <summary>
        /// Angle in radians.
        /// </summary>
        public double Rotation { get => _pixiService.GetDisplayObjectMember<double>(_target, Id, new List<string> { "rotation" }); set => _pixiService.SetDisplayObjectMember(_target, Id, new List<string> { "rotation" }, value); }

        public bool Visible { get => _pixiService.GetDisplayObjectMember<bool>(_target, Id, new List<string> { "visible" }); set => _pixiService.SetDisplayObjectMember(_target, Id, new List<string> { "visible" }, value); }
        public bool Interactive { get => _pixiService.GetDisplayObjectMember<bool>(_target, Id, new List<string> { "interactive" }); set => _pixiService.SetDisplayObjectMember(_target, Id, new List<string> { "interactive" }, value); }

        protected string _target;
        protected PixiService _pixiService;

        public GraphicsDisplayObject(int id, string target, PixiService pixiService)
        {
            Id = id;
            _target = target;
            _pixiService = pixiService;
        }

        private GraphicsDisplayObject() { }

        public (double x, double y) ToLocal(double x, double y)
        {
            var result = _pixiService.ToLocal(_target, Id, x, y);
            return (result[0], result[1]);
        }

        public (double x, double y) ToGlobal(double x, double y)
        {
            var result = _pixiService.ToGlobal(_target, Id, x, y);
            return (result[0], result[1]);
        }
    }
}
