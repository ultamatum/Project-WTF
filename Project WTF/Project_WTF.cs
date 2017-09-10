using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project_WTF.GameStates;

namespace Project_WTF
{
    public class Project_WTF : Game
    {
        #region Declarations
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int selectedLevel = 3;

        enum gamestate { menu, gamePlay, levelselect};
        static gamestate currentGamestate = gamestate.menu;

        static bool changedState = true;

        Menu menu = new Menu();
        GamePlay game = new GamePlay();
        LevelSelect lvlSelect = new LevelSelect();
        #endregion

        #region Constructor
        public Project_WTF()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = Constants.SCREENWIDTH;
            graphics.PreferredBackBufferHeight = Constants.SCREENHEIGHT;
            graphics.IsFullScreen = Constants.FULLSCREEN;
        }
        #endregion 

        protected override void Initialize()
        {
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        
        protected override void UnloadContent() { }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

        public static void SetGameState(int stateID)
        {
            changedState = true;

            switch(stateID)
            {
                case 0:
                    currentGamestate = gamestate.menu;
                    break;
                case 1:
                    currentGamestate = gamestate.gamePlay;
                    break;
                case 2:
                    currentGamestate = gamestate.levelselect;
                    break;
            }
        }
    }
}
