using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kroshka
{
    public class Cockroach
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

        public Cockroach(Vector2 Pos, Vector2 Dir)
        {
            this.Pos = Pos;
            this.Dir = Dir;
        }

        public Cockroach()
        {
            RandomSet();
        }

        public Cockroach(Vector2 Dir)
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
            Pos = new Vector2(Asteroid.GetIntRnd(Asteroid.Width, Asteroid.Width + 300), 1000);
            Dir = new Vector2(-Asteroid.GetIntRnd(12, 18), 0);
            color = Color.White;
        }

        public void Draw()
        {
            Asteroid.SpriteBatch.Draw(Texture2D, Pos, color);
        }
    }
}
