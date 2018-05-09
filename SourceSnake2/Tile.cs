using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tiles
{
    public enum TileTypes
    {
        Wall,
        Food,
        Snake
    }

    public class Tile
    {
        public int _Type;
        public Vector2 _Position;
        public Texture2D _Texture;
        public Color _Color;
        public Rectangle _Rectangle;

        public bool haveCollision;

        public void Update(Vector2 Position)
        {
            _Position = Position;
            _Rectangle = new Rectangle((int)Position.X, (int)Position.Y, _Texture.Width, _Texture.Height);
        }

        public void Update(Texture2D Texture)
        {
            _Texture = Texture;
        }

        public Tile(TileTypes Type, Texture2D Texture, Vector2 Position, Color Color, bool Collision = true)
        {
            _Type = (int)Type;
            _Position = Position;
            _Texture = Texture;
            _Color = Color;

            _Rectangle = new Rectangle((int)Position.X, (int)Position.Y, _Texture.Width, _Texture.Height);

            haveCollision = Collision;

        }
    }
}
