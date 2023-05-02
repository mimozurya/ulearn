using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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
        Vector2 cameraPosition;
        Texture2D gameBackground;

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
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            SplashScreen.Background = Content.Load<Texture2D>("background");
            SplashScreen.Font = Content.Load<SpriteFont>("SplashFont");
            Asteroids.Init (spriteBatch, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            Wind.Texture2D = Content.Load<Texture2D>("wind");
            gameBackground = Content.Load<Texture2D>("game_background");
            Vector2 cameraPosition = character.position;
            character.Load(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            switch(Stat)
            {
                case Stat.SplashScreen:
                    SplashScreen.Update();
                    if (keyboardState.IsKeyDown(Keys.Enter))
                    {
                        Stat = Stat.Game;
                    }
                    break;
                case Stat.Game:

                    const float cameraLerpFactor = 0.05f;
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

                    Asteroids.Update();
                    character.Update();

                    if (keyboardState.IsKeyDown(Keys.Escape)) Stat = Stat.SplashScreen;

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
                case Stat.Game:

                    Asteroids.Draw();
                    Vector2 backgroundOffset = new Vector2(-gameBackground.Width / 2f, -gameBackground.Height / 2f);
                    Vector2 backgroundPositions = new Vector2(-970f, 230f);
                    spriteBatch.Draw(gameBackground, backgroundPositions, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    character.Draw(gameTime, spriteBatch);
                    break;

            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}