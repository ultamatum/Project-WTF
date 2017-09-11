using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_WTF.TileEngine
{
    [Serializable]
    public class MapSquare
    {
        #region Declarations
        public int[] layerTiles = new int[3];           //The amount of layers on each tile
        public string codeValue = "";                   //A value to dictate special traits of each tile
        public bool passable = true;                    //Dictates whether or not the tile can be passed through
        #endregion

        #region Constructor
        /// <summary>
        /// Create a tile on the map with specific values
        /// </summary>
        /// <param name="background">The textureID of the background layer of the tile</param>
        /// <param name="interactive">The textureID of the interactive layer of the tile</param>
        /// <param name="foreground">The textureID of the foreground layer of the tile</param>
        /// <param name="code">The codevalue of the tile</param>
        /// <param name="passable">Is the tile passable</param>
        public MapSquare(int background, int interactive, int foreground, string code, bool passable)
        {
            layerTiles[0] = background;
            layerTiles[1] = interactive;
            layerTiles[2] = foreground;
            codeValue = code;
            this.passable = passable;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Toggles the passable value of a block
        /// </summary>
        public void TogglePassable()
        {
            passable = !passable;
        }
        #endregion
    }
}
