using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Tiles;

namespace LevelUtility
{
    //used for serializing
    [DataContract]
    public class LevelBluePrint
    {
        public string Color;
        public string X;
        public string Y;
        public string TileHeight;
        public string TileWidth;
        public string TileType;

        [DataMember]
        string[] tileContent;
        [DataMember]
        public List<string[]> Storage;

        public void Add(Tile tile)
        {
            tileContent = new string[6]
                {
            Color = tile._Color.PackedValue.ToString(),
            X = tile._Position.X.ToString(),
            Y = tile._Position.Y.ToString(),
            TileHeight = tile._Texture.Height.ToString(),
            TileWidth = tile._Texture.Width.ToString(),
            TileType = tile._Type.ToString()

        };

            Storage.Add(tileContent);

        }

        public void OpenPrint(int index)
        {
            var print = Storage[index];
            Color = print[0];
            X = print[1];
            Y = print[2];
            TileHeight = print[3];
            TileWidth = print[4];
            TileType = print[5];
        }

        public LevelBluePrint()
        {
            Storage = new List<string[]>();
        }


    }
    //use for containing objects
    class Level
    {
        private List<Tiles.Tile> _Tiles;

        public void AddTile(Tile tile)
        {
            Tile newTile = new Tile((Tiles.TileTypes)tile._Type, tile._Texture, tile._Position, tile._Color);
            _Tiles.Add(newTile);
        }

        public void AddTile(List<Tile> tiles)
        {
            _Tiles = tiles;
        }

        public void Delete()
        {
            _Tiles.Clear();
        }

        public void Delete(Tile tile)
        {
            _Tiles.Remove(tile);
        }

        public List<Tile> GetList()
        {
            return _Tiles;
        }

        public LevelBluePrint GenerateBlueprint()
        {
            var blueprint = new LevelBluePrint();
            foreach (var tile in _Tiles)
            {
                blueprint.Add(tile);
            }

            return blueprint;

        }

        public Level()
        {
            _Tiles = new List<Tiles.Tile>();
        }
    }

    //used for generated levels per blueprint
    class LevelGenerator
    {

        public static Level GenerateLevel(LevelBluePrint BluePrint, GraphicsDevice device )
        {
            Utility.TextureGenerator TextureGen = new Utility.TextureGenerator(device);
            var level = new Level();

            for (int i = 0; i < BluePrint.Storage.Count; i++)
            {
                BluePrint.OpenPrint(i);


               var tile = new Tile((Tiles.TileTypes)Convert.ToInt32(BluePrint.TileType), TextureGen.CreateTexture(Convert.ToInt32(BluePrint.TileWidth), 
                                        Convert.ToInt32(BluePrint.TileHeight), new Color(Convert.ToUInt32(BluePrint.Color))),
                                        new Microsoft.Xna.Framework.Vector2((float)Convert.ToInt32(BluePrint.X), (float)Convert.ToInt32(BluePrint.Y)),
                                        new Color(Convert.ToUInt32(BluePrint.Color)));
                
                level.AddTile(tile);
            }

            return level;
        }
    }
}
