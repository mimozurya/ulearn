using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection.Metadata;

namespace kroshka
{
    class Level
    {
        Texture2D mapTexture;
        Color[] mapData;
        int mapWidth;
        int mapHeight;

        public void Load(ContentManager content)
        {
            mapTexture = content.Load<Texture2D>("level");
            mapData = new Color[mapTexture.Width * mapTexture.Height];
            mapTexture.GetData(mapData);
            mapWidth = mapTexture.Width;
            mapHeight = mapTexture.Height;
        }
        public Level() { }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mapTexture, Vector2.Zero, Color.White);
        }

        public bool IsColliding(Rectangle bounds)
        {
            int left = (int)Math.Floor((float)bounds.Left / TileWidth);
            int right = (int)Math.Ceiling(((float)bounds.Right / TileWidth)) - 1;
            int top = (int)Math.Floor((float)bounds.Top / TileHeight);
            int bottom = (int)Math.Ceiling(((float)bounds.Bottom / TileHeight)) - 1;

            for (int y = top; y <= bottom; y++)
            {
                for (int x = left; x <= right; x++)
                {
                    Color pixelColor = GetPixel(x, y);
                    if (pixelColor.A > 0)
                    {
                        Rectangle tileBounds = new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight);
                        if (bounds.Intersects(tileBounds))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private Color GetPixel(int x, int y)
        {
            if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight)
            {
                return Color.Transparent;
            }

            return mapData[x + y * mapWidth];
        }

        public const int TileWidth = 32;
        public const int TileHeight = 32;
    }
}
