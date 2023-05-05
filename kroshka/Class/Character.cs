using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace kroshka
{
    public class Animation
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

    public class Character : Collision
    {
        Animation animation;
        AnimationCharacter animationCharacter;
        internal AnimationCharacter AnimationCharacter { get { return animationCharacter; } }

        Animation runAnim;
        Animation stayAnim;
        Animation jumpAnim;
        Animation rollAnim;

        public bool isOnGround = true;
        public bool isRunning = false;

        public Vector2 position = new Vector2(0, 1100);
        public Vector2 velocity;

        public Character() : base()
        {
            animationCharacter = new AnimationCharacter();
            runAnim = null;
            stayAnim = null;
            jumpAnim = null;
            rollAnim = null;
            animation = null;
        }
        public void Load(ContentManager content)
        {
            runAnim = new Animation(content.Load<Texture2D>("run"), 400, 0.08f, true);
            stayAnim = new Animation(content.Load<Texture2D>("idle"), 400, 0.2f, true);
            jumpAnim = new Animation(content.Load<Texture2D>("jump"), 400, 0.1f, false);
            rollAnim = new Animation(content.Load<Texture2D>("roll"), 400, 0.15f, true);
            animation = stayAnim;
            animationCharacter.PlayAnimation(stayAnim);
        }
        public void Update()
        {
            position += velocity;

            if (position.Y >= 1100)
            {
                position.Y = 1100;
                isOnGround = true;
            }
            else
            {
                velocity.Y += 0.35f;
                isOnGround = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                isRunning = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W) && isOnGround)
            {
                velocity.Y = -15f;
                animationCharacter.PlayAnimation(jumpAnim);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S) && isOnGround)
            {
                velocity.Y = 0f;
                animationCharacter.PlayAnimation(rollAnim);
            }
            else if (isOnGround)
            {
                if (isRunning)
                    animationCharacter.PlayAnimation(runAnim);
                else
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
