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
        static gamestate currentGamestate = gamestate.gamePlay;

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

            switch(currentGamestate)
            {
                case gamestate.menu:
                    if(changedState)
                    {
                        menu.Init();
                        changedState = false;
                    }
                    menu.Update();
                    break;
                case gamestate.gamePlay:
                    if (changedState)
                    {
                        game.Init(Content, 0);
                        changedState = false;
                    }
                    game.Update();
                    break;
                case gamestate.levelselect:
                    if (changedState)
                    {
                        lvlSelect.Init();
                        changedState = false;
                    }
                    lvlSelect.Update();
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if(changedState)
            {
                return;
            }

            switch(currentGamestate)
            {
                case gamestate.menu:
                    menu.Draw();
                    break;
                case gamestate.gamePlay:
                    game.Draw(spriteBatch);
                    break;
                case gamestate.levelselect:
                    lvlSelect.Draw();
                    break;
            }

            base.Draw(gameTime);
        }
        
        /// <summary>
        /// Change the gamestate to the given stateID
        /// </summary>
        /// <param name="stateID">0 = Menu, 1 = Game Play, 2 = Level Select</param>
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
