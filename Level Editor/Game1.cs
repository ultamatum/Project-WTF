using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project_WTF.TileEngine;
using System.Windows.Forms;

namespace Level_Editor
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Declarations
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        IntPtr drawSurface;
        System.Windows.Forms.Form parentForm;
        System.Windows.Forms.PictureBox pictureBox;
        System.Windows.Forms.Control gameForm;

        public int DrawLayer = 0;
        public int DrawTile = 0;

        public bool EditingCode = false;
        public bool leftButtonClicked = false;
        public bool rightButtonClicked = false;
        public string CurrentCodeValue = "";
        public string HoverCodeValue = "";

        public static Texture2D tileMap;

        private MouseState lastMouseState = Mouse.GetState();

        System.Windows.Forms.VScrollBar vscroll;
        System.Windows.Forms.HScrollBar hscroll;
        #endregion
        
        public Game1(IntPtr drawSurface, System.Windows.Forms.Form parentForm, System.Windows.Forms.PictureBox surfacePictureBox)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.drawSurface = drawSurface;
            this.parentForm = parentForm;
            this.pictureBox = surfacePictureBox;

            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);

            Mouse.WindowHandle = drawSurface;

            gameForm = System.Windows.Forms.Control.FromHandle(this.Window.Handle);
            gameForm.VisibleChanged += new EventHandler(gameForm_VisibleChanged);
            pictureBox.SizeChanged += new EventHandler(pictureBox_SizeChanged);

            vscroll = (System.Windows.Forms.VScrollBar)parentForm.Controls["vScrollBar1"];
            hscroll = (System.Windows.Forms.HScrollBar)parentForm.Controls["hScrollBar1"];
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Camera.ViewPortWidth = pictureBox.Width;
            Camera.ViewPortHeight = pictureBox.Height;
            Camera.WorldRectangle = new Rectangle(0, 0, TileMap.TILEWIDTH * TileMap.MAPWIDTH, TileMap.TILEHEIGHT * TileMap.MAPHEIGHT);

            tileMap = Content.Load<Texture2D>("Tileset");

            TileMap.Initialize(Content.Load<Texture2D>("Tileset"));
            TileMap.spriteFont = Content.Load<SpriteFont>("Arial");

            pictureBox_SizeChanged(null, null);
        }
        
        protected override void UnloadContent(){}
        
        protected override void Update(GameTime gameTime)
        {
            Camera.Position = new Vector2(hscroll.Value, vscroll.Value);

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Vector2 mouseLoc = Camera.ScreenToWorld(new Vector2(Control.MousePosition.X, Control.MousePosition.Y - 50));

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            spriteBatch.DrawString(TileMap.spriteFont, "X: " + mouseLoc.X + mouseLoc.Y, mouseLoc, Color.Beige);

            TileMap.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void PlaceBlock(int xCoord, int yCoord, MouseButtons button)
        {
            Vector2 worldCoords = Camera.ScreenToWorld(new Vector2(xCoord, yCoord));

            if (worldCoords.X <= 0)
                worldCoords.X = 0;
            else if (worldCoords.Y <= 0)
                worldCoords.Y = 0;
            else if (worldCoords.Y >= (TileMap.MAPHEIGHT * TileMap.TILEHEIGHT))
                worldCoords.Y = (TileMap.MAPHEIGHT * TileMap.TILEHEIGHT) - 1;
            else if (worldCoords.X >= (TileMap.MAPWIDTH * TileMap.TILEWIDTH))
                worldCoords.X = (TileMap.MAPHEIGHT * TileMap.TILEHEIGHT) - 1;

            if (button == MouseButtons.Left)
            {
                TileMap.SetTileAtCell(TileMap.GetCellByPixelX((int)worldCoords.X), TileMap.GetCellByPixelY((int)worldCoords.Y), DrawLayer, DrawTile);
            }

            if (button == MouseButtons.Right && !lastMouseState.RightButton.Equals(MouseButtons.Right))
            {
                if (EditingCode)
                {
                    TileMap.GetMapSquareAtCell(TileMap.GetCellByPixelX((int)worldCoords.X), TileMap.GetCellByPixelY((int)worldCoords.Y)).codeValue = CurrentCodeValue;
                }
                else
                {
                    TileMap.GetMapSquareAtCell(TileMap.GetCellByPixelX((int)worldCoords.X), TileMap.GetCellByPixelY((int)worldCoords.Y)).TogglePassable();
                }
            }

            lastMouseState = Mouse.GetState();

            HoverCodeValue = TileMap.GetMapSquareAtCell(TileMap.GetCellByPixelX((int)worldCoords.X), TileMap.GetCellByPixelY((int)worldCoords.Y)).codeValue;
        }

        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = drawSurface;
        }

        void gameForm_VisibleChanged(object sender, EventArgs e)
        {
            if (gameForm.Visible == true)
                gameForm.Visible = false;
        }

        void pictureBox_SizeChanged(object sender, EventArgs e)
        {
            if (parentForm.WindowState != System.Windows.Forms.FormWindowState.Minimized)
            {
                graphics.PreferredBackBufferWidth = pictureBox.Width;
                graphics.PreferredBackBufferHeight = pictureBox.Height;
                Camera.ViewPortWidth = pictureBox.Width;
                Camera.ViewPortHeight = pictureBox.Height;
                graphics.ApplyChanges();
            }
        }
    }
}
