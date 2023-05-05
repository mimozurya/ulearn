using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using SamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;

namespace kroshka
{
    enum Stat
    {
        SplashScreen,
        Game,
        Final,
        Pause
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        Stat Stat = Stat.Game;
        Character character;
        Bee bee;
        Cockroach cockroach;
        Vector2 cameraPosition;
        Texture2D gameBackground;
        Texture2D diedBackground;
        private List<Collision> _collisions;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.IsFullScreen = true;
            character = new Character();
            bee = new Bee();
            cockroach = new Cockroach();
            _collisions = new List<Collision>();
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            SplashScreen.Background = Content.Load<Texture2D>("background");
            SplashScreen.Font = Content.Load<SpriteFont>("SplashFont");
            gameBackground = Content.Load<Texture2D>("game_background");
            diedBackground = Content.Load<Texture2D>("died_background");
            Vector2 cameraPosition = character.position;
            Asteroid.Init (spriteBatch, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            //Wind.Texture2D = Content.Load<Texture2D>("wind");
            bee.Load(Content);
            cockroach.Load(Content);
            character.Load(Content);

            Collision characterCollision = new Collision(Content.Load<Texture2D>("cockroach"));
            _collisions.Add(characterCollision);

            Collision beeCollision = new Collision(Content.Load<Texture2D>("bee"));
            _collisions.Add(beeCollision);

            Collision cockroachCollision = new Collision(Content.Load<Texture2D>("cockroach"));
            _collisions.Add(cockroachCollision);

        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            switch(Stat)
            {
                case Stat.SplashScreen:
                    SplashScreen.Update();
                    if (keyboardState.IsKeyDown(Keys.Space))
                    {
                        Stat = Stat.Game;
                    }
                    break;
                case Stat.Final:
                    if (keyboardState.IsKeyDown(Keys.Space))
                    {
                        Stat = Stat.Game;
                    }
                    break;
                case Stat.Game:

                    const float cameraLimitX = 100f;
                    const float cameraLimitY = 50f;
                    Vector2 targetCameraPosition = new Vector2(
                        character.position.X - graphics.GraphicsDevice.Viewport.Width / 2f,
                        character.position.Y - graphics.GraphicsDevice.Viewport.Height / 2f
                    );
                    Vector2 clampedTargetCameraPosition = new Vector2(
                        MathHelper.Clamp(targetCameraPosition.X, cameraLimitX, cameraLimitX - graphics.GraphicsDevice.Viewport.Width),
                        MathHelper.Clamp(targetCameraPosition.Y, cameraLimitY, cameraLimitY - graphics.GraphicsDevice.Viewport.Height)
                    );
                    cameraPosition = new Vector2(
                        character.position.X - graphics.GraphicsDevice.Viewport.Width / 2f,
                        character.position.Y - graphics.GraphicsDevice.Viewport.Height / 2f
                    );

                    character.Update();
                    Asteroid.Update();
                    foreach (var collision in _collisions)
                        collision.Update(gameTime, _collisions);
                    if (keyboardState.IsKeyDown(Keys.F))
                        Stat = Stat.Final;

                    break;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            SplashScreen.Update();
            character.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Matrix.CreateTranslation(-cameraPosition.X, -cameraPosition.Y, 0f));

            switch (Stat)
            {
                case Stat.SplashScreen:
                    SplashScreen.Draw(spriteBatch);
                    break;
                case Stat.Final:
                    Vector2 diedBackgroundOffset = new Vector2(-diedBackground.Width / 2f, -diedBackground.Height / 2f);
                    Vector2 diedBackgroundPositions = new Vector2(-970f, 550f);
                    spriteBatch.Draw(diedBackground, diedBackgroundPositions, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    break;
                case Stat.Game:
                    Vector2 backgroundOffset = new Vector2(-gameBackground.Width / 2f, -gameBackground.Height / 2f);
                    Vector2 backgroundPositions = new Vector2(-970f, 230f);
                    spriteBatch.Draw(gameBackground, backgroundPositions, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    character.Draw(gameTime, spriteBatch);
                    Asteroid.Draw();
                    foreach (var collision in _collisions)
                        collision.Draw(spriteBatch);
                    break;

            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}