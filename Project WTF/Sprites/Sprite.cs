using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project_WTF.Sprites.Animations;
using Project_WTF.TileEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_WTF.Sprites
{
    public class Sprite
    {
        #region Declarations
        protected Vector2 worldLocation;
        protected Vector2 velocity;
        protected Vector2 origin = Vector2.Zero;
        protected float acceleration;
        protected int frameWidth;
        protected int frameHeight;

        protected Color colour;

        protected bool enabled = false;
        protected bool flipped = false;
        protected bool waitingSelection = false;
        protected bool onGround;
        protected float rotation = 0f;
        protected float scale = 1f;

        protected Rectangle collisionRectangle;
        protected int collideWidth;
        protected int collideHeight;
        protected bool codeBasedBlocks = true;
        protected bool blocking = false;
        public int health = 100;

        protected float drawDepth = 0.85f;
        protected Dictionary<String, Animation> animations = new Dictionary<string, Animation>();
        protected string currentAnimation;
        #endregion

        #region Properties
        /// <summary>
        /// Sets / Returns if the sprite is enabled to be drawn on the screen
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// Sets / Returns the sprites rotation variable
        /// </summary>
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        /// <summary>
        /// Sets / Returns the sprites origin point
        /// </summary>
        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        /// <summary>
        /// Sets / Returns the sprites world location
        /// </summary>
        public Vector2 WorldLocation
        {
            get { return worldLocation; }
            set { worldLocation = value; }
        }

        /// <summary>
        /// Returns the center of the world
        /// </summary>
        public Vector2 WorldCenter
        {
            get
            {
                return new Vector2((int)worldLocation.X + (int)(frameWidth / 2), (int)worldLocation.Y + (int)(frameHeight / 2));
            }
        }

        /// <summary>
        /// Returns the world rectangle
        /// </summary>
        public Rectangle WorldRectangle
        {
            get
            {
                return new Rectangle((int)worldLocation.X, (int)worldLocation.Y, frameWidth, frameHeight);
            }
        }

        /// <summary>
        /// Returns / Sets the sprites hitbox
        /// </summary>
        public Rectangle hitbox
        {
            get
            {
                return new Rectangle((int)worldLocation.X + collisionRectangle.X * (int)scale, (int)WorldRectangle.Y + collisionRectangle.Y * (int)scale, collisionRectangle.Width * (int)scale, collisionRectangle.Height * (int)scale);
            }
            set { collisionRectangle = value; }
        }
        #endregion

        #region Helper Methods
        //Updates the sprites current animation to move to the next frame / animation.
        private void UpdateAnimation(GameTime gameTime)
        {
            if(animations.ContainsKey(currentAnimation))
            {
                if(animations[currentAnimation].FinishedPlaying)
                {
                    PlayAnimation(animations[currentAnimation].NextAnimation);
                }
                else
                {
                    animations[currentAnimation].Update(gameTime);
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Plays the given animation
        /// </summary>
        /// <param name="name">The name of the animation to play</param>
        public void PlayAnimation(string name)
        {
            currentAnimation = name;
            animations[name].Play();
        }

        public virtual void Update(GameTime gameTime)
        {
            //If the sprite is disabled it doesn't get updated
            if (!enabled)
                return;
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateAnimation(gameTime);

            if (!waitingSelection)
            {
                if (velocity.Y >= 2f)
                {
                    onGround = false;
                }
                if (velocity.Y >= 2f)
                {
                    onGround = false;
                }
                Vector2 moveAmount = velocity * elapsed;
                moveAmount = HorizontalCollisionTest(moveAmount);
                moveAmount = VerticalCollisionTest(moveAmount);

                Vector2 newPosition = worldLocation + moveAmount;
                newPosition = new Vector2(MathHelper.Clamp(newPosition.X, 0, Camera.WorldRectangle.Width - frameWidth), MathHelper.Clamp(newPosition.Y, 2 * (-TileMap.TILEHEIGHT), Camera.WorldRectangle.Height - frameHeight));
                worldLocation = newPosition;
            }
        }

        //Draws the sprite to the screen
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //If the sprite is disabled it doesn't get drawn
            if (!enabled)
                return;

            if(animations.ContainsKey(currentAnimation))
            {
                SpriteEffects effect = SpriteEffects.None;
                if(flipped)
                {
                    effect = SpriteEffects.FlipHorizontally;
                }
                spriteBatch.Draw(animations[currentAnimation].Texture, Camera.WorldToScreen(worldLocation), animations[currentAnimation].FrameRectangle, colour, rotation, origin, scale, effect, drawDepth);
            }
        }

        /// <summary>
        /// Damage the player by a specified value (1 quarter if blocking)
        /// </summary>
        /// <param name="value">The amount of damage to do</param>
        public void Damage(int value)
        {
            if (blocking) health -= (value / 4);
            else health -= value;
        }

        public void Knockback(float value, Vector2 attackerPos)
        {
            if(attackerPos.X > worldLocation.X)
            {
                velocity.X -= value;
            }
            else
            {
                velocity.X += value;
            }
            velocity.Y -= value / 5;
        }
        #endregion

        #region Map-Based Collision Detection Methods
        /// <summary>
        /// Checks if the sprite can move without colliding into anything before it gets moved
        /// </summary>
        /// <param name="moveAmount">The value that the sprite is getting moved</param>
        /// <returns></returns>
        private Vector2 HorizontalCollisionTest(Vector2 moveAmount)
        {
            if (moveAmount.X == 0)
                return moveAmount;
            Rectangle afterMoveRect = hitbox;
            afterMoveRect.Offset((int)moveAmount.X, 0);
            Vector2 corner1, corner2;

            if (moveAmount.X < 0)
            {
                corner1 = new Vector2(afterMoveRect.Left, afterMoveRect.Top + 1);
                corner2 = new Vector2(afterMoveRect.Left, afterMoveRect.Bottom - 1);
            }
            else
            {
                corner1 = new Vector2(afterMoveRect.Right, afterMoveRect.Top + 1);
                corner2 = new Vector2(afterMoveRect.Right, afterMoveRect.Bottom - 1);
            }

            Vector2 mapCell1 = TileMap.GetCellByPixel(corner1);
            Vector2 mapCell2 = TileMap.GetCellByPixel(corner2);

            if (!TileMap.CellIsPassable(mapCell1) || !TileMap.CellIsPassable(mapCell2))
            {
                moveAmount.X = 0;
                velocity.X = 0;
            }

            if(codeBasedBlocks)
            {
                if(TileMap.CellCodeValue(mapCell1) == "BLOCK" || TileMap.CellCodeValue(mapCell2) == "BLOCK")
                {
                    moveAmount.X = 0;
                    velocity.X = 0;
                }
            }
            return moveAmount;
        }

        /// <summary>
        /// Checks if the sprite can move without colliding into anything before it gets moved
        /// </summary>
        /// <param name="moveAmount">The value that the sprite is getting moved</param>
        /// <returns></returns>
        private Vector2 VerticalCollisionTest(Vector2 moveAmount)
        {
            if (moveAmount.Y == 0)
                return moveAmount;
            Rectangle afterMoveRect = hitbox;
            afterMoveRect.Offset((int)moveAmount.X, (int)moveAmount.Y);
            Vector2 corner1, corner2;

            if(moveAmount.Y < 0)
            {
                corner1 = new Vector2(afterMoveRect.Left + 1, afterMoveRect.Top);
                corner2 = new Vector2(afterMoveRect.Right - 1, afterMoveRect.Top);
            }
            else
            {
                corner1 = new Vector2(afterMoveRect.Left + 1, afterMoveRect.Bottom);
                corner2 = new Vector2(afterMoveRect.Right - 1, afterMoveRect.Bottom);
            }

            Vector2 mapCell1 = TileMap.GetCellByPixel(corner1);
            Vector2 mapCell2 = TileMap.GetCellByPixel(corner2);

            if (!TileMap.CellIsPassable(mapCell1) || !TileMap.CellIsPassable(mapCell2))
            {
                if (moveAmount.Y > 0)
                    onGround = true;
                moveAmount.Y = 0;
                velocity.Y = 0;
            }
            return moveAmount;
        }
        #endregion
    }
}
