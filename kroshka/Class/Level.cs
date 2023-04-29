using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;
using System.Drawing;
using System.Reflection.Metadata;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace kroshka
{
    class Level
    {
        private IGraphicsDeviceProvider graphicsDeviceProvider;
        Texture2D mapTexture;
        Color[] mapData;
        int mapWidth;
        int mapHeight;

        public void Load(ContentManager content)
        {
            mapTexture = content.Load<Texture2D>("design");
            //mapTexture = content.Load<Texture2D>("level");
            mapData = new Color[mapTexture.Width * mapTexture.Height];
            mapTexture.GetData(mapData);
            mapWidth = mapTexture.Width;
            mapHeight = mapTexture.Height;
        }

        public Level(IGraphicsDeviceProvider graphicsDeviceProvider, Texture2D mapTexture, Color[] mapData, int mapWidth, int mapHeight) : this(graphicsDeviceProvider)
        {
            this.mapTexture = mapTexture;
            this.mapData = mapData;
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
        }

        public Level(IGraphicsDeviceProvider graphicsDeviceProvider) 
        {
            this.graphicsDeviceProvider = graphicsDeviceProvider;
            // this.graphicsDeviceProvider.GraphicsDeviceManager.GraphicsProfile = GraphicsProfile.HiDef;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mapTexture, Vector2.Zero, Color.White);
        }

        public bool IsColliding(Character character)
        {
            Rectangle characterBounds = new Rectangle((int)character.position.X, (int)character.position.Y, character.AnimationCharacter.Animation.FrameWidth, character.AnimationCharacter.Animation.FrameHeight);

            return IsColliding(characterBounds);
        }

        private bool IsColliding(Rectangle bounds)
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
                    if (pixelColor.A > 0 && pixelColor.R == 255 && pixelColor.G == 0 && pixelColor.B == 0)
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

        //public bool IsColliding(Rectangle bounds)
        //{
        //    int left = (int)Math.Floor((float)bounds.Left / TileWidth);
        //    int right = (int)Math.Ceiling(((float)bounds.Right / TileWidth)) - 1;
        //    int top = (int)Math.Floor((float)bounds.Top / TileHeight);
        //    int bottom = (int)Math.Ceiling(((float)bounds.Bottom / TileHeight)) - 1;

        //    for (int y = top; y <= bottom; y++)
        //    {
        //        for (int x = left; x <= right; x++)
        //        {
        //            Color pixelColor = GetPixel(x, y);
        //            if (pixelColor.A > 0 && pixelColor.R == 255 && pixelColor.G == 0 && pixelColor.B == 0)
        //            {
        //                Rectangle tileBounds = new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight);
        //                if (bounds.Intersects(tileBounds))
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //    }

        //    return false;
        //}

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

    public interface IGraphicsDeviceProvider
    {
        GraphicsDeviceManager GraphicsDeviceManager { get; }
    }

    public class KroshkaGame : Game1, IGraphicsDeviceProvider
    {
        private GraphicsDeviceManager graphics;

        public GraphicsDeviceManager GraphicsDeviceManager => graphics;

        public KroshkaGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
    }
}
