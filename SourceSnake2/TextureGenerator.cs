using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Utility
{

    class TextureGenerator
    {
        GraphicsDevice _GraphicsDevice;

        private struct _Pixel
        {
            public Color[] pixel;

            public _Pixel(int Width, int Height, Color color)
            {
                pixel = new Color[Width * Height];

                for (int i = 0; i < pixel.Length; i++)
                {
                    pixel[i] = Color.White;
                }
            }
        }

        public Texture2D CreateTexture(int width, int height, Color color)
        {
            var newTexture = new Texture2D(_GraphicsDevice, width, height);
            var pixel = new _Pixel(width, height, color);

            newTexture.SetData<Color>(pixel.pixel);

            return newTexture;
        }

        public TextureGenerator(GraphicsDevice graphicsDevice)
        {
            _GraphicsDevice = graphicsDevice;
        }

    }
}
