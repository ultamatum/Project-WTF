using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_WTF.TileEngine
{
    class LevelManager
    {
        #region Declarations
        private static ContentManager Content;
        private static int currentLevel;
        private static Vector2 respawnLocation;
        #endregion

        #region Properties
        /// <summary>
        /// Returns the current level ID
        /// </summary>
        public static int CurrentLevel
        {
            get { return CurrentLevel; }
        }

        /// <summary>
        /// Returns or sets the current level's respawn location
        /// </summary>
        public static Vector2 RespawnLocation
        {
            get { return RespawnLocation; }
            set { respawnLocation = value; }
        }
        #endregion

        #region Initialization
        public static void Initialize(ContentManager content)
        {
            Content = content;
        }
        #endregion

        #region Public Methods
        public static void LoadLevel(int levelNumber, ContentManager cm)
        {
            TileMap.LoadMap(new FileStream(cm.RootDirectory + @"\Maps\MAP" + levelNumber.ToString().PadLeft(3, '0') + ".MAP", FileMode.Open));

            currentLevel = levelNumber;
        }
#endregion

    }
}
