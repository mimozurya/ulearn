using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace kroshka
{
    public class Asteroid
    {
        public static int Width, Height;
        public Vector2 position;
        public Vector2 velocity;
        public static Random rnd = new Random();
        static public SpriteBatch SpriteBatch { get; set; }
        //static Wind[] winds;
        static List<Bee> bees = new List<Bee>();
        static List<Cockroach> cockroaches = new List<Cockroach> ();

        static public int GetIntRnd (int min, int max)
        {
            return rnd.Next(min, max);
        }
        static public void Init (SpriteBatch SpriteBatch, int Width, int Height)
        {
            Asteroid.Width = Width;
            Asteroid.Height = Height;
            Asteroid.SpriteBatch = SpriteBatch;
            //winds = new Wind[50];
            //for (int i = 0; i < winds.Length; i++)
            //    winds[i] = new Wind(new Vector2(-rnd.Next(1, 10), 0));
            for (int i = 0; i < 1; i++)
                bees.Add(new Bee());
            for (int i = 0; i < 1; i++)
                cockroaches.Add(new Cockroach());
        }

        static public void Draw()
        {
            //foreach (Wind wind in winds)
            //    wind.Draw();
            foreach (Bee bee in bees)
                bee.Draw();
            foreach (Cockroach cockroach in cockroaches)
                cockroach.Draw();
        }

        static public void Update()
        {
            //foreach (Wind wind in winds)
            //    wind.Update();
            foreach (Bee bee in bees)
                bee.Update();
            foreach (Cockroach cockroach in cockroaches)
                cockroach.Update();
        }
    }
}
