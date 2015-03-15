using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D10;
using SharpDX.DXGI;
using SharpDX.Windows;

using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Device1 = SharpDX.Direct3D10.Device1;
using DriverType = SharpDX.Direct3D10.DriverType;
using Factory = SharpDX.DXGI.Factory;
using FeatureLevel = SharpDX.Direct3D10.FeatureLevel;
using D2DFactory = SharpDX.Direct2D1.Factory;

namespace ECS_Proto_First
{
    class AGraphics
    {
        D2DFactory d2dFactory;
        RenderTarget renderTarget;

        SolidColorBrush solidBrush;

        public AGraphics(D2DFactory factory, RenderTarget target)
        {
            this.d2dFactory = factory;
            this.renderTarget = target;

            this.solidBrush = new SolidColorBrush(this.renderTarget, Color.White);
        }

        public void FillRectangle(float x, float y, float width, float height, Color4 color)
        {
            this.solidBrush.Color = color;

            this.renderTarget.FillRectangle(new Rectangle() { Left = (int)x, Top = (int)y, Width = (int)width, Height = (int)height }, solidBrush);
        }
    }
}
