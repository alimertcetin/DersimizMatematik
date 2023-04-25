using UnityEngine;

namespace XIV.Core.Utils
{
    public static class TextureUtils
    {
        public static Texture2D Resize(Texture2D source, int textureWidth, int textureHeight)
        {
            var currentWidth = source.width;
            var currentHeight = source.height;

            float scaleX = (float)textureWidth / currentWidth;
            float scaleY = (float)textureHeight / currentHeight;
            Rect scaledRect = new Rect(0f, 0f, currentWidth * scaleX, currentHeight * scaleY);

            Texture2D newTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
            Scale.Bilinear(source, newTexture, scaledRect);
            return newTexture;
        }
    
        public static Texture2D ResizeNonAlloc(Texture2D source, int textureWidth, int textureHeight, Color[] pixels)
        {
            var currentWidth = source.width;
            var currentHeight = source.height;

            float scaleX = (float)textureWidth / currentWidth;
            float scaleY = (float)textureHeight / currentHeight;
            Rect scaledRect = new Rect(0f, 0f, currentWidth * scaleX, currentHeight * scaleY);

            Texture2D newTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
            Scale.BilinearNonAlloc(source, newTexture, scaledRect, pixels);
            return newTexture;
        }
    
        public static Texture2D CreateTexture(Sprite sprite, Texture2D spriteTexture)
        {
            Rect spriteRect = sprite.textureRect;
            Texture2D newTexture = new Texture2D((int)spriteRect.width, (int)spriteRect.height, TextureFormat.RGBA32, false);
            Scale.Bilinear(spriteTexture, newTexture, spriteRect);
            return newTexture;
        }
    
        public static Texture2D CreateTexture(Sprite sprite)
        {
            var spriteTexture = sprite.texture.isReadable ? sprite.texture : Copy(sprite.texture);
            return CreateTexture(sprite, spriteTexture);
        }
    
        public static Texture2D CreateTexture(Sprite sprite, Texture2D spriteTexture, int textureWidth, int textureHeight)
        {
            Rect sourceRect = sprite.textureRect;
            sourceRect.x /= sprite.texture.width;
            sourceRect.width /= sprite.texture.width;
            sourceRect.y /= sprite.texture.height;
            sourceRect.height /= sprite.texture.height;

            Texture2D newTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
            Scale.Bilinear(spriteTexture, newTexture, sourceRect);
            return newTexture;
        }
    
        public static Texture2D CreateTexture(Sprite sprite, int textureWidth, int textureHeight)
        {
            var spriteTexture = sprite.texture.isReadable ? sprite.texture : Copy(sprite.texture);
            return CreateTexture(sprite, spriteTexture, textureWidth, textureHeight);
        }
    
        public static Texture2D CreateTextureNonAlloc(Sprite sprite, Texture2D spriteTexture, int textureWidth, int textureHeight, Color[] pixels)
        {
            Rect sourceRect = sprite.textureRect;
            sourceRect.x /= sprite.texture.width;
            sourceRect.width /= sprite.texture.width;
            sourceRect.y /= sprite.texture.height;
            sourceRect.height /= sprite.texture.height;

            Texture2D newTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
            Scale.BilinearNonAlloc(spriteTexture, newTexture, sourceRect, pixels);
            return newTexture;
        }
    
        public static Texture2D CreateTextureNonAlloc(Sprite sprite, int textureWidth, int textureHeight, Color[] pixels)
        {
            var spriteTexture = sprite.texture.isReadable ? sprite.texture : Copy(sprite.texture);
            return CreateTextureNonAlloc(sprite, spriteTexture, textureWidth, textureHeight, pixels);
        }
    
        public static Texture2D Copy(Texture2D sourceTexture)
        {
            Texture2D copyTexture = new Texture2D(sourceTexture.width, sourceTexture.height, sourceTexture.format, false);
            Graphics.CopyTexture(sourceTexture, copyTexture);
            return copyTexture;
        }
    
        public static Texture2D Copy(Texture2D sourceTexture, Texture2D destinationTexture)
        {
            Graphics.CopyTexture(sourceTexture, destinationTexture);
            return destinationTexture;
        }
    
        public static class Scale
        {
            public static void Bilinear(Texture2D source, Texture2D dest)
            {
                var destWidth = dest.width;
                var destheight = dest.height;

                for (int y = 0; y < destheight; y++)
                {
                    float v = (float)y / (destheight - 1);
                    for (int x = 0; x < destWidth; x++)
                    {
                        float u = (float)x / (destWidth - 1);
                        dest.SetPixel(x, y, source.GetPixelBilinear(u, v));
                    }
                }

                dest.Apply();
            }

            public static void Bilinear(Texture2D source, Texture2D dest, Rect sourceRect)
            {
                var destWidth = dest.width;
                var destheight = dest.height;
                var sourceRectWidth = sourceRect.width;
                var sourceRectHeight = sourceRect.height;

                for (int y = 0; y < destheight; y++)
                {
                    float v = ((float)y / (destheight - 1)) * sourceRectHeight + sourceRect.y;
                    for (int x = 0; x < destWidth; x++)
                    {
                        float u = ((float)x / (destWidth - 1)) * sourceRectWidth + sourceRect.x;
                        dest.SetPixel(x, y, source.GetPixelBilinear(u, v));
                    }
                }

                dest.Apply();
            }

            public static void BilinearNonAlloc(Texture2D source, Texture2D dest, Color[] pixels)
            {
                var destWidth = dest.width;
                var destheight = dest.height;
                Debug.Assert(pixels.Length == destheight * destWidth, "Array lenght must be equal to destination texture width * height");

                for (int y = 0; y < destheight; y++)
                {
                    float v = (float)y / (destheight - 1);
                    for (int x = 0; x < destWidth; x++)
                    {
                        float u = (float)x / (destWidth - 1);
                        pixels[y * destWidth + x] = source.GetPixelBilinear(u, v);
                    }
                }

                dest.SetPixels(pixels);
                dest.Apply();
            }

            public static void BilinearNonAlloc(Texture2D source, Texture2D dest, Rect sourceRect, Color[] pixels)
            {
                var destWidth = dest.width;
                var destheight = dest.height;
                var sourceRectWidth = sourceRect.width;
                var sourceRectHeight = sourceRect.height;
                Debug.Assert(pixels.Length == destheight * destWidth, "Array lenght must be equal to destination texture width * height");

                for (int y = 0; y < destheight; y++)
                {
                    float v = ((float)y / (destheight - 1)) * sourceRectHeight + sourceRect.y;
                    for (int x = 0; x < destWidth; x++)
                    {
                        float u = ((float)x / (destWidth - 1)) * sourceRectWidth + sourceRect.x;
                        pixels[y * destWidth + x] = source.GetPixelBilinear(u, v);
                    }
                }

                // https://docs.unity3d.com/2021.3/Documentation/ScriptReference/Texture2D.SetPixels.html
                dest.SetPixels(pixels);
                dest.Apply();
            }

            public static Color GetPixelBilinear(Texture2D source, float x, float y)
            {
                var xInt = (int)x;
                var yInt = (int)y;
                var a = Color.Lerp(source.GetPixel(xInt, yInt), source.GetPixel(xInt + 1, yInt), x - xInt);
                var b = Color.Lerp(source.GetPixel(xInt, yInt + 1), source.GetPixel(xInt + 1, yInt + 1), x - xInt);

                return Color.Lerp(a, b, y - yInt);
            }
        }
    }
}