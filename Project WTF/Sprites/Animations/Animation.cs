using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_WTF.Sprites.Animations
{
    public class Animation
    {
        #region Declarations
        private Texture2D texture;
        private int frameWidth;
        private int frameHeight;

        private float frameTimer = 0f;
        private float frameDelay = 0.05f;

        private int currentFrame;
        private int animationID;
        private int frameCount;

        private bool loopAnimation;
        private bool finishedPlaying;

        private string name;
        private string nextAnimation;
        #endregion

        #region Properties
        /// <summary>
        /// Sets / returns the width of each animation frame
        /// </summary>
        public int FrameWidth
        {
            get { return frameWidth; }
            set { frameWidth = value; }
        }

        /// <summary>
        /// Sets / returns the height of each frame
        /// </summary>
        public int FrameHeight
        {
            get { return frameHeight; }
            set { frameHeight = value; }
        }

        /// <summary>
        /// Sets / returns the texture of the animation
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        /// <summary>
        /// Sets / returns the animation name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Sets / returns what the next animation to play is
        /// </summary>
        public string NextAnimation
        {
            get { return nextAnimation; }
            set { nextAnimation = value; }
        }

        /// <summary>
        /// Sets / returns whether or not the animation should loop
        /// </summary>
        public bool LoopAnimation
        {
            get { return loopAnimation; }
            set { loopAnimation = value; }
        }

        /// <summary>
        /// Returns true if the animation has finished
        /// </summary>
        public bool FinishedPlaying
        {
            get { return finishedPlaying; }
        }

        /// <summary>
        /// Sets / returns the frame count
        /// </summary>
        public int FrameCount
        {
            get { return frameCount; }
            set { frameCount = value; }
        }

        /// <summary>
        /// Sets / returns how long each frame should be on the screen
        /// </summary>
        public float FrameLength
        {
            get { return frameDelay; }
            set { frameDelay = value; }
        }

        /// <summary>
        /// Returns the current frame number
        /// </summary>
        public int CurrentFrame
        {
            get { return currentFrame; }
        }

        /// <summary>
        /// Returns the animation ID
        /// </summary>
        public int AnimationID
        {
            get { return animationID; }
        }

        /// <summary>
        /// The rectangle that gets drawn on the screen
        /// </summary>
        public Rectangle FrameRectangle
        {
            get
            {
                return new Rectangle(currentFrame * frameWidth, animationID * frameHeight, frameWidth, frameHeight);
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates an animation with a given texture, dimensions, ID and name.
        /// </summary>
        /// <param name="texture">The animation sheet to use</param>
        /// <param name="frameWidth">The width of each frame of the animation</param>
        /// <param name="frameHeight">The height of each frame of the animation</param>
        /// <param name="frameCount">How many frames of animation there are</param>
        /// <param name="animationID">The ID of the animation (how many animations down the spritesheet it is)</param>
        /// <param name="name">The name of the animation</param>
        public Animation(Texture2D texture, int frameWidth, int frameHeight, int frameCount, int animationID, string name)
        {
            this.texture = texture;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.frameCount = frameCount;
            this.animationID = animationID;
            this.name = name;
        }
        #endregion

        #region Public Methods
        //Sets the current animation to start playing
        public void Play()
        {
            currentFrame = 0;
            finishedPlaying = false;
        }

        //Updates the animation
        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            frameTimer += elapsed;

            if(frameTimer > frameDelay)
            {
                frameTimer -= frameDelay;

                currentFrame++;
                frameTimer = 0f;
                if(currentFrame >= frameCount)
                {
                    if(loopAnimation)
                    {
                        currentFrame = 0;
                    }
                    else
                    {
                        currentFrame = frameCount - 1;
                        finishedPlaying = true;
                    }
                }
            }
        }
        #endregion
    }
}
