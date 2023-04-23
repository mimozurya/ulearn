using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace kroshka
{
    class Animation
    {
        Texture2D texture;
        float frameTime;
        public int FrameCount;
        bool isLooping;
        public int FrameWidth;
        public Texture2D Texture { get { return texture; } }
        public int FrameHeight { get { return texture.Height; } }
        public float FrameTime { get { return frameTime; } }
        public bool IsLooping { get { return isLooping; } }

        public Animation(Texture2D newTexture, int newFrameWidth, float newFrameTime, bool newIsLooping)
        {
            texture = newTexture;
            FrameWidth = newFrameWidth;
            frameTime = newFrameTime;
            isLooping = newIsLooping;
            FrameCount = texture.Width / newFrameWidth;
        }
    }

    struct AnimationCharacter
    {
        Animation animation;
        int frameIndex;
        private float timer;
        public Animation Animation { get { return animation; } }
        public int FrameIndex
        {
            get { return frameIndex; }
            set { frameIndex = value; }
        }
        public Vector2 Origin
        {
            get { return new Vector2(animation.FrameWidth / 2, animation.FrameHeight / 2); }
        }
        public void PlayAnimation(Animation newAnimation)
        {
            if (animation == newAnimation)
                return;
            animation = newAnimation;
            frameIndex = 0;
            timer = 0;
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (timer >= animation.FrameTime)
            {
                timer -= animation.FrameTime;
                if (animation.IsLooping)
                    frameIndex = (frameIndex + 1) % animation.FrameCount;
                else
                    frameIndex = Math.Min(frameIndex + 1, animation.FrameCount - 1);
            }
            Rectangle rectangle = new Rectangle(frameIndex * Animation.FrameWidth, 0, Animation.FrameWidth, animation.FrameHeight);
            spriteBatch.Draw(Animation.Texture, position, rectangle, Color.White, 0f, Origin, 1f, spriteEffects, 0f);
        }

    }

    class Character
    {
        AnimationCharacter animationCharacter;

        Animation runAnim;
        Animation stayAnim;
        Animation jumpAnim;
        Animation rollAnim;

        bool isOnGround = true;

        Vector2 position = new Vector2(50, 450);
        Vector2 velocity;

        public Character() { animationCharacter = new AnimationCharacter(); }
        public void Load(ContentManager content)
        {
            runAnim = new Animation(content.Load<Texture2D>("run"), 400, 0.08f, true);
            stayAnim = new Animation(content.Load<Texture2D>("idle"), 400, 0.2f, true);
            jumpAnim = new Animation(content.Load<Texture2D>("jump"), 400, 0.1f, false);
            rollAnim = new Animation(content.Load<Texture2D>("roll"), 400, 0.1f, false);
        }
        public void Update()
        {
            position += velocity;

            if (position.Y >= 450)
            {
                position.Y = 450;
                isOnGround = true;
            }
            else
            {
                velocity.Y += 0.5f;
                isOnGround = false;
            }

            if (velocity.X == 0f && isOnGround)
                animationCharacter.PlayAnimation(stayAnim);
            if (velocity.X != 0 && animationCharacter.Animation != runAnim)
            {
                animationCharacter.PlayAnimation(runAnim);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && isOnGround)
            {
                velocity.Y = -15f;
                isOnGround = false;
                animationCharacter.PlayAnimation(jumpAnim);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) && isOnGround)
            {
                velocity.Y = 0f;
                isOnGround = false;
                animationCharacter.PlayAnimation(rollAnim);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && isOnGround)
            {
                velocity.Y = -15f;
                isOnGround = false;
                animationCharacter.PlayAnimation(jumpAnim);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                velocity.X = 5f;
                if (isOnGround)
                    animationCharacter.PlayAnimation(runAnim);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                velocity.X = -5f;
                if (isOnGround)
                    animationCharacter.PlayAnimation(runAnim);
            }
            else
            {
                velocity.X = 0f;
                if (isOnGround && velocity.X == 0f)
                    animationCharacter.PlayAnimation(stayAnim);
            }

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects flip = SpriteEffects.None;
            if (velocity.X >= 0)
                flip = SpriteEffects.None;
            else if (velocity.X < 0)
                flip = SpriteEffects.FlipHorizontally;

            animationCharacter.Draw(gameTime, spriteBatch, position, flip);
        }
    }
}
