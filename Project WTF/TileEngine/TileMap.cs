using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Project_WTF.TileEngine
{
    public static class TileMap
    {
        #region Declarations
        public const int TILEWIDTH = 64;                 //Width of each tile on the map.
        public const int TILEHEIGHT = 64;                //Height of each tile on the map.
        public const int MAPWIDTH = 100;                 //Amount of tiles that go across the length of the map.
        public const int MAPHEIGHT = 17;                 //Amount of tiles that go across the height of the map.
        public const int MAPLAYERS = 3;                  //The amount of layers on each tile.
        private const int SKYTILE = 2;                   //The tile ID of the sky tile
        private const int BLANKTILE = 36;

        static private MapSquare[,] mapCells = new MapSquare[MAPWIDTH, MAPHEIGHT];      //Array of map tiles that make up the map

        public static bool EditorMode = false;           //Whether or not edit mode is active currently.

        public static SpriteFont spriteFont;             //The font to use (only in editor mode)
        static private Texture2D tileSheet;              //The tilesheet of all the tiles.
        #endregion

        #region Initialization
        /// <summary>
        /// Fills in all of the blocks on the map to their default value
        /// </summary>
        /// <param name="tileTexture">The texture file to be used</param>
        static public void Initialize(Texture2D tileTexture)
        {
            tileSheet = tileTexture;
            
            for (int x = 0; x < MAPWIDTH; x++)
            {
                for(int y = 0; y < MAPHEIGHT; y++)
                {
                    for (int z = 0; z < MAPLAYERS; z++)
                    {
                        mapCells[x, y] = new MapSquare(SKYTILE, BLANKTILE, BLANKTILE, "", true);
                    }
                }
            }
        }
        #endregion

        #region Tile and Tile Sheet Handling
        /// <summary>
        /// Returns how many tiles are in a row on the tile sheet.
        /// </summary>
        public static int TilesPerRow
        {
            get { return tileSheet.Width / TILEWIDTH; }
        }

        /// <summary>
        /// The rectangle that has to be drawn to the screen
        /// </summary>
        /// <param name="tileIndex">The tileID that is being checked</param>
        /// <returns></returns>
        public static Rectangle TileSourceRectangle(int tileIndex)
        {
            return new Rectangle((tileIndex % TilesPerRow) * TILEWIDTH, (tileIndex / TilesPerRow) * TILEHEIGHT, TILEWIDTH, TILEHEIGHT);
        }
        #endregion

        #region Information about Map Cells
        /// <summary>
        /// Gets the tile under the specific X coordinate in the world
        /// </summary>
        /// <param name="pixelX">The X location of the tile</param>
        /// <returns></returns>
        static public int GetCellByPixelX(int pixelX)
        {
            return pixelX / TILEWIDTH;
        }

        /// <summary>
        /// Gets the tile under the specific Y coordinate in the world
        /// </summary>
        /// <param name="pixelX">The Y location of the tile</param>
        /// <returns></returns>
        static public int GetCellByPixelY(int pixelY)
        {
            return pixelY / TILEHEIGHT;
        }

        /// <summary>
        /// Gets the tile under the coordinates given on the tilesheet
        /// </summary>
        /// <param name="pixelLocation">The vector of the location to check on the tilesheet</param>
        /// <returns></returns>
        static public Vector2 GetCellByPixel(Vector2 pixelLocation)
        {
            return new Vector2(GetCellByPixelX((int)pixelLocation.X), GetCellByPixelY((int)pixelLocation.Y));
        }

        /// <summary>
        /// Gets the center of the requested cell
        /// </summary>
        /// <param name="cellX">The X location of the tile on the tilesheet</param>
        /// <param name="cellY">The Y location of the tile on the tilesheet</param>
        /// <returns></returns>
        static public Vector2 GetCellCenter(int cellX, int cellY)
        {
            return new Vector2((cellX * TILEWIDTH) + (TILEWIDTH / 2), (cellY * TILEHEIGHT) + (TILEHEIGHT / 2));
        }

        /// <summary>
        /// Gets the center of the requested tile
        /// </summary>
        /// <param name="cell">The vector position of the tile on the tilesheet</param>
        /// <returns></returns>
        static public Vector2 GetCellCenter(Vector2 cell)
        {
            return GetCellCenter((int)cell.X, (int)cell.Y);
        }

        /// <summary>
        /// Returns the rectangle of a tile.
        /// </summary>
        /// <param name="cellX">The X position of the tile on the tilesheet</param>
        /// <param name="cellY">The Y position of the tile on the world</param>
        /// <returns></returns>
        static public Rectangle CellWorldRectangle(int cellX, int cellY)
        {
            return new Rectangle(cellX * TILEHEIGHT, cellY * TILEHEIGHT, TILEWIDTH, TILEHEIGHT);
        }

        /// <summary>
        /// Returns the rectangle of a tile in the world
        /// </summary>
        /// <param name="cell">The position of the tile in the world</param>
        /// <returns></returns>
        static public Rectangle CellWorldRectangle(Vector2 cell)
        {
            return CellWorldRectangle((int)cell.X, (int)cell.Y);
        }

        /// <summary>
        /// Returns the rectangle of a tile on the screen
        /// </summary>
        /// <param name="cellX">The world X coordinate of the tile</param>
        /// <param name="cellY">The world Y coordinate of the tile</param>
        /// <returns></returns>
        static public Rectangle CellScreenRectangle(int cellX, int cellY)
        {
            return Camera.WorldToScreen(CellWorldRectangle(cellX, cellY));
        }

        /// <summary>
        /// Returns the rectangle of a tile on the screen
        /// </summary>
        /// <param name="cell"The world position of the tile></param>
        /// <returns></returns>
        static public Rectangle CellSreenRectangle(Vector2 cell)
        {
            return CellScreenRectangle((int)cell.X, (int)cell.Y);
        }

        /// <summary>
        /// Returns if the tile given is passable
        /// </summary>
        /// <param name="cellX">The X position of the tile in the world </param>
        /// <param name="cellY">The Y position of the tile in the world </param>
        /// <returns></returns>
        static public bool CellIsPassable(int cellX, int cellY)
        {
            MapSquare square = GetMapSquareAtCell(cellX, cellY);

            if (square == null)
                return false;
            else
                return square.passable;
        }

        /// <summary>
        /// Returns if the tile given is passable
        /// </summary>
        /// <param name="cell">The word position of the tile</param>
        /// <returns></returns>
        static public bool CellIsPassable(Vector2 cell)
        {
            return CellIsPassable((int)cell.X, (int)cell.Y);
        }

        /// <summary>
        /// Returns if the tile under a certain pixel is passable
        /// </summary>
        /// <param name="pixelLocation">The world position of the pixel</param>
        /// <returns></returns>
        static public bool CellIsPassableByPixel(Vector2 pixelLocation)
        {
            return CellIsPassable(GetCellByPixelX((int)pixelLocation.X), GetCellByPixelY((int)pixelLocation.Y));
        }

        /// <summary>
        /// Returns the cell code value of a specific cell
        /// </summary>
        /// <param name="cellX">The X position of the cell in the world</param>
        /// <param name="cellY">The Y position of the cell in the world</param>
        /// <returns></returns>
        static public string CellCodeValue(int cellX, int cellY)
        {
            MapSquare square = GetMapSquareAtCell(cellX, cellY);

            if (square == null)
                return "";
            else
                return square.codeValue;
        }

        /// <summary>
        /// Returns the cell code value of a specific cell
        /// </summary>
        /// <param name="cell">The world position of the cell</param>
        /// <returns></returns>
        static public string CellCodeValue(Vector2 cell)
        {
            return CellCodeValue((int)cell.X, (int)cell.Y);
        }
        #endregion

        #region Information about MapSquare objects
        /// <summary>
        /// Returns the MapSquare object of the tile
        /// </summary>
        /// <param name="tileX">The X coordinate of the tile in the world</param>
        /// <param name="tileY">The Y coordinate of the tile in the world</param>
        /// <returns></returns>
        static public MapSquare GetMapSquareAtCell(int tileX, int tileY)
        {
            if ((tileX >= 0) && (tileX < MAPWIDTH) && (tileY >= 0) && (tileY < MAPHEIGHT))
            {
                return mapCells[tileX, tileY];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Sets the MapSquare object at a specific tile
        /// </summary>
        /// <param name="tileX">The X position of the tile in the world</param>
        /// <param name="tileY">The Y position of the tile in the world</param>
        /// <param name="tile">The MapSquare object to place</param>
        static public void SetMapSquareAtCell(int tileX, int tileY, MapSquare tile)
        {
            if ((tileX >= 0) && (tileX < MAPWIDTH) && (tileY >= 0) && (tileY < MAPHEIGHT))
            {
                mapCells[tileX, tileY] = tile;
            }
        }

        /// <summary>
        /// Sets the texture of a tile.
        /// </summary>
        /// <param name="tileX">The X position of the tile in the world</param>
        /// <param name="tileY">The Y position of the tile in the world</param>
        /// <param name="layer">What layer the texture has to be placed at</param>
        /// <param name="tileIndex">The TileID of the texture that has to be placed</param>
        static public void SetTileAtCell(int tileX, int tileY, int layer, int tileIndex)
        {
            if ((tileX >= 0) && (tileX < MAPWIDTH) && (tileY >= 0) && (tileY < MAPHEIGHT))
            {
                mapCells[tileX, tileY].layerTiles[layer] = tileIndex;
            }
        }

        /// <summary>
        /// Returns the MapSquare object at the specified pixel
        /// </summary>
        /// <param name="pixelX">The X coordinate of the pixel to return</param>
        /// <param name="pixelY">The Y coordinate of the pixel to return</param>
        /// <returns></returns>
        static public MapSquare GetMapSquareAtPixel(int pixelX, int pixelY)
        {
            return GetMapSquareAtCell(GetCellByPixelX(pixelX), GetCellByPixelY(pixelY));
        }

        /// <summary>
        /// Returns the MapSquare object at the specified location
        /// </summary>
        /// <param name="pixelLocation">The pixel location to check</param>
        /// <returns></returns>
        static public MapSquare GetMapSquareAtPixel(Vector2 pixelLocation)
        {
            return GetMapSquareAtPixel((int)pixelLocation.X, (int)pixelLocation.Y);
        }
        #endregion

        #region Loading and Saving Maps
        /// <summary>
        /// Saves the map to a specified location
        /// </summary>
        /// <param name="fileStream">The location to save the map</param>
        public static void SaveMap(FileStream fileStream)
        {
            BinaryFormatter formatter = new BinaryFormatter();          //Sets the formatter to be a binary formatter
            formatter.Serialize(fileStream, mapCells);                  //Serializes the map in order for it to be easily loaded later
            fileStream.Close();                                         //Closes the filestream
        }

        /// <summary>
        /// Loads the map from a specified location
        /// </summary>
        /// <param name="fileStream">The location to load the map</param>
        public static void LoadMap(FileStream fileStream)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();                  //Sets the formatter to be a binary formatter
                mapCells = (MapSquare[,])formatter.Deserialize(fileStream);         //Deserialises the map and sets the default map cells to the new values
                fileStream.Close();                                                 //Closes the filestream
            }
            catch
            {
                ClearMap();                                                         //If this fails it sets the map to its default values
            }
        }

        /// <summary>
        /// Sets the entire map to its default values.
        /// </summary>
        public static void ClearMap()
        {
            for (int x = 0; x < MAPWIDTH; x++)
            {
                for (int y = 0; y < MAPHEIGHT; y++)
                {
                    for (int z = 0; z < MAPLAYERS; z++)
                    {
                        mapCells[x, y] = new MapSquare(SKYTILE, BLANKTILE, BLANKTILE, "", true);
                    }
                }
            }
        }
        #endregion

        #region Drawing
        /// <summary>
        /// Draws all the tiles to the screen
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to use to draw the files</param>
        static public void Draw(SpriteBatch spriteBatch)
        {
            int startX = GetCellByPixelX((int)Camera.Position.X);                                       //The starting cell in the X axis that is on the screen so it knows where to start drawing
            int endX = GetCellByPixelX((int)Camera.Position.X + Camera.ViewPortWidth);                  //The ending cell in the X axis that is on the screen so it knows where to stop drawing
            int startY = GetCellByPixelY((int)Camera.Position.Y);                                       //The starting cell in the Y axis that is on the screen so it knows where to start drawing
            int endY = GetCellByPixelY((int)Camera.Position.Y + Camera.ViewPortHeight);                 //The ending cell in the Y axis that is on the screen so it knows where to stop drawing

            //Loops through all of the tiles that are on the screen and draws them
            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    for (int z = 0; z < MAPLAYERS; z++)
                    {
                        if ((x >= 0) && (y >= 0) &&
                            (x < MAPWIDTH) && (y < MAPHEIGHT))
                        {
                            spriteBatch.Draw(tileSheet, CellScreenRectangle(x, y), TileSourceRectangle(mapCells[x, y].layerTiles[z]), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 1f - ((float)z * 0.1f));
                        }
                    }

                    //If the editor is open it draws all of the edit mode items
                    if (EditorMode)
                    {
                        DrawEditModeItems(spriteBatch, x, y);
                    }
                }
            }
        }

        /// <summary>
        /// Draws all of the edit mode items
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to use to draw the items</param>
        /// <param name="x">The X coordinate of the item</param>
        /// <param name="y">The Y cordinate of the item</param>
        public static void DrawEditModeItems(SpriteBatch spriteBatch, int x, int y)
        {
            if ((x < 0) || (x >= MAPWIDTH) || (y < 0) || (y >= MAPHEIGHT))
                return;

            if (!CellIsPassable(x, y))
            {
                spriteBatch.Draw(tileSheet, CellScreenRectangle(x, y), TileSourceRectangle(31), new Color(255, 0, 0, 80), 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
            }

            if (mapCells[x, y].codeValue != "")
            {
                Rectangle screenRect = CellScreenRectangle(x, y);

                spriteBatch.DrawString(spriteFont, mapCells[x, y].codeValue, new Vector2(screenRect.X, screenRect.Y), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            }
        }
        #endregion
    }
}
