using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronBalls.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IronBalls.Extensions
{
    public static class DrawPrimitivesHelper
    {
        public static Texture2D CreateCircle(int radius, byte thickness)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            var texture = new Texture2D(DisplayService.Instance.GraphicsDevice, outerRadius, outerRadius);

            var data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.FromNonPremultiplied(0, 0, 0, 0);

            // Work out the minimum step necessary using trigonometry + sine approximation.

            for (int i = 0; i < thickness; i++)
            {
                var drawRadius = radius - i;
                double angleStep = 1f / drawRadius;
                for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
                {
                    var x = (int)Math.Round(radius + drawRadius * Math.Cos(angle));
                    var y = (int)Math.Round(radius + drawRadius * Math.Sin(angle));
                    data[y * outerRadius + x + 1] = Color.White;
                }
            }

            texture.SetData(data);
            return texture;
        }

        public static Texture2D CreateRectangleFill(int width, int height)
        {

            var texture = new Texture2D(DisplayService.Instance.GraphicsDevice, width, height);

            var data = new Color[width * height];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.White;

            texture.SetData(data);
            return texture;
        }

        public static Texture2D CreateRectangle(int width, int height, byte thickness)
        {

            var texture = new Texture2D(DisplayService.Instance.GraphicsDevice, width, height);
            if (width < thickness)
                width = thickness;

            if (height < thickness)
                height = thickness;

            var data = new Color[width * height];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.FromNonPremultiplied(0, 0, 0, 0);

            for (int i = 0; i < thickness; i++ )
            {
                for (int j = 0; j < width; j++)
                {
                    data[j + i * width] = Color.White;
                    data[width * height - (j + i * width) - 1] = Color.White;
                }
            }

            for (int i = 0; i < thickness; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    data[j * width + i] = Color.White;
                    data[width * height - (j * width + i) - 1] = Color.White;
                }
            }

            texture.SetData(data);
            return texture;
        }
    }
}
