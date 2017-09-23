using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project_WTF.TileEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_WTF.GameStates
{
    class GamePlay
    {
        #region Declarations
        private Player player = new Player;

        private SpriteFont font;
        #endregion

        #region Constructor
        public GamePlay()
        {

        }
        #endregion

        #region Public Methods
        public void Init(ContentManager cm, int levelID)
        {
            TileMap.Initialize(cm.Load<Texture2D>("Tileset"));

            font = cm.Load<SpriteFont>("Pixel Font");

            //World Initialization
            Camera.WorldRectangle = new Rectangle(0, 0, TileMap.MAPWIDTH * TileMap.TILEWIDTH, TileMap.MAPHEIGHT * TileMap.TILEHEIGHT);
            Camera.ViewPortWidth = 1920;
            Camera.ViewPortHeight = 1080;

            LevelManager.Initialize(cm);
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch sp)
        {
            sp.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
            TileMap.Draw(sp);

            player.Draw();

            sp.End();
        }
        #endregion
    }
}
