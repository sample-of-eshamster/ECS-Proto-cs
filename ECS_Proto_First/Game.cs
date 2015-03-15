using System;
using System.Diagnostics;
using System.Linq;
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

namespace ECS_Proto_First
{
    class Game : IDisposable
    {
        RenderTargetView renderView;
        Texture2D backBuffer;
        Device1 device;
        SwapChain swapChain;
        Factory factory;

        AGraphics graphics;

        public void Run()
        {
            var form = new RenderForm("SharpDX - MiniTri Direct2D - Direct3D 10 Sample");

            // SwapChain description
            var desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription =
                    new ModeDescription(form.ClientSize.Width, form.ClientSize.Height,
                                        new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = form.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            // Create Device and SwapChain
            Device1.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, desc, FeatureLevel.Level_10_0, out device, out swapChain);

            var d2dFactory = new SharpDX.Direct2D1.Factory();

            // Ignore all windows events
            factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAll);

            // New RenderTargetView from the backbuffer
            backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
            renderView = new RenderTargetView(device, backBuffer);

            Surface surface = backBuffer.QueryInterface<Surface>();

            var d2dRenderTarget = new RenderTarget(d2dFactory, surface,
                                                            new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            graphics = new AGraphics(d2dFactory, d2dRenderTarget);

            // Main loop
            RenderLoop.Run(form, () =>
            {
                d2dRenderTarget.BeginDraw();
                d2dRenderTarget.Clear(Color.Black);
                graphics.FillRectangle(128, 128, 128, 128, new Color4(1, 1, 1, (float)(Math.Cos(stopwatch.ElapsedMilliseconds * .002) + 1) / 2f));
                graphics.FillRectangle(384, 128, 128, 128, new Color4(1, 1, 1, (float)(Math.Cos(stopwatch.ElapsedMilliseconds * .002 + Math.PI / 2f) + 1) / 2f));
                d2dRenderTarget.EndDraw();

                swapChain.Present(0, PresentFlags.None);
            });
        }

        public void Dispose()
        {
            renderView.Dispose();
            backBuffer.Dispose();
            device.ClearState();
            device.Flush();
            device.Dispose();
            swapChain.Dispose();
            factory.Dispose();
        }
    }
}
