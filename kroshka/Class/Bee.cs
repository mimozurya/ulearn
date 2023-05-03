using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kroshka
{
    public class Bee
    {
        public Vector2 Pos { get; private set; }
        Vector2 dir;
        public Vector2 Dir
        {
            get
            {
                return dir;
            }
            set
            {
                dir = value;
            }
        }

        Color color;
        public static Texture2D Texture2D { get; set; }

        public Bee(Vector2 Pos, Vector2 Dir)
        {
            this.Pos = Pos;
            this.Dir = Dir;
        }

        public Bee()
        {
            RandomSet();
        }

        public Bee(Vector2 Dir)
        {
            this.Dir = Dir;
            RandomSet();
        }

        public void Update()
        {
            Pos += Dir;
            if (Pos.X < -1200)
                RandomSet();
        }

        public void RandomSet()
        {
            Pos = new Vector2(Asteroid.GetIntRnd(Asteroid.Width, Asteroid.Width + 300), 750);
            Dir = new Vector2(-Asteroid.GetIntRnd(20, 30), 0);
            color = Color.White;
        }

        public void Draw()
        {
            Asteroid.SpriteBatch.Draw(Texture2D, Pos, color);
        }
    }
}
