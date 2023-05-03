using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kroshka.Class
{
    public class Wind
    {
        Vector2 Pos;
        Vector2 Dir;
        Color color;

        public static Texture2D Texture2D { get; set; }

        public Wind(Vector2 Pos, Vector2 Dir)
        {
            this.Pos = Pos;
            this.Dir = Dir;
        }

        public Wind(Vector2 Dir)
        {
            this.Dir = Dir;
            RandomSet();
        }

        public void Update()
        {
            Pos += Dir;
            if (Pos.X < 0)
                RandomSet();
        }

        public void RandomSet()
        {
            Pos = new Vector2(Asteroid.GetIntRnd(Asteroid.Width, Asteroid.Width + 300), Asteroid.GetIntRnd(0, Asteroid.Height));
            color = Color.White;
        }

        public void Draw()
        {
            Asteroid.SpriteBatch.Draw(Texture2D, Pos, color);
        }
    }
}
