using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace kroshka
{
    public class Cockroach : Collision
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

        public Cockroach(Vector2 Pos, Vector2 Dir) : base(Texture2D)
        {
            this.Pos = Pos;
            this.Dir = Dir;
        }

        public Cockroach() : base(Texture2D)
        {
            RandomSet();
        }

        public Cockroach(Vector2 Dir) : base(Texture2D)
        {
            this.Dir = Dir;
            RandomSet();
        }

        public void Load(ContentManager content)
        {
            Texture2D = content.Load<Texture2D>("cockroach_game");
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
