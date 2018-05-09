using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class SnakeSegment
    {
        public Vector2 Position;
        public Texture2D Texture;
        public Color Color;
        public Rectangle rect;
        
        public void UpdatePosition()
        {
            rect = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public SnakeSegment(Texture2D texture, Vector2 position, Color color)
        {
            Position = position;
            Texture = texture;
            Color = color; 

            rect = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            
        }
    }
    class Snake
    {
        Texture2D _Texture;
        Texture2D _EmptyTexture;

        private SnakeSegment segment;
        public Dictionary<string, SnakeSegment> segmentsDictionary;
        private int _segmentCount;

        private Vector2 _direction;

        public bool isDead = false;
        public int SnakeAteCounter = 0;

        public Vector2 getPositionHead()
        {
            return segmentsDictionary["Head"].Position;
        }

        public void Movement()
        {
            if (UserInput.getYAxis() == 1)
            {
                _direction = new Vector2(0, 1);
            }
            if (UserInput.getYAxis() == -1)
            {
                _direction = new Vector2(0, -1);
            }
            if (UserInput.getXAxis() == 1)
            {
                _direction = new Vector2(1, 0);
            }
            if (UserInput.getXAxis() == -1)
            {
                _direction = new Vector2(-1, 0);
            }
            updateBodyPosition();
        }

        public void Movement(Vector2 direction)
        {
            _direction = direction;

        }


        SnakeSegment createSegment(Texture2D Texture, Vector2 vector2D, Color color )
        {
            return new SnakeSegment(Texture, vector2D, color);
        }

        public void addSegment()
        {
            string nameKey = "body" + _segmentCount;

            //var blankSegment = createSegment(_EmptyTexture, new Vector2(-5, -5), Color.Transparent);
            //segmentsDictionary.Add(nameKey, blankSegment);
            //_segmentCount++;

            nameKey = "body" + _segmentCount;
            segment = createSegment(_Texture, new Vector2(-5,-5), Color.White);
            segmentsDictionary.Add(nameKey, segment);
            _segmentCount++;
        }

        public List<SnakeSegment> DrawOrder()
        {
            var order = new List<SnakeSegment>();
            foreach (var segment in segmentsDictionary.Keys)
            {
                order.Add(segmentsDictionary[segment]);
            }
            return order;
        }
        

        public void UpdateHeadPosition(Vector2 Position)
        {
            segmentsDictionary["Head"].Position = Position;
            segmentsDictionary["Head"].UpdatePosition();
            updateBodyPosition();
        }

        void updateBodyPosition()
        {
            Vector2 PreviousPosition = segmentsDictionary["Head"].Position;
            
            segmentsDictionary["Head"].Position += (_direction) * 10f;

            segmentsDictionary["Head"].UpdatePosition();
            
            for (int i = 1; i < segmentsDictionary.Keys.Count; i++)
            {
                var key = "body" + i;
                var NewPosition = PreviousPosition;

                if (isDead)
                {
                    segmentsDictionary[key].Position = new Vector2(-5, -5);
                    segmentsDictionary[key].UpdatePosition();
                }
                else
                    PreviousPosition = segmentsDictionary[key].Position;

                    segmentsDictionary[key].Position =  NewPosition;

                    segmentsDictionary[key].UpdatePosition();
                    //PreviousPosition = segmentsDictionary[key].Position;
            }

        }

        //figure out a better way to copy objects
        public Snake(Snake copy)
        {
            _Texture = copy._Texture;
            _EmptyTexture = copy._EmptyTexture;

            segmentsDictionary = new Dictionary<string, SnakeSegment>();

            foreach (var key in copy.segmentsDictionary.Keys)
            {
                segmentsDictionary.Add(key, copy.segmentsDictionary[key]);
            }

            _segmentCount = segmentsDictionary.Keys.Count;

        }

        public Snake(Tiles.Tile Tile)
        {

            _Texture = Tile._Texture;
            //_EmptyTexture = TextureGen.CreateTexture(Width, Height, Color.Transparent);


            segmentsDictionary = new Dictionary<string, SnakeSegment>();

            segmentsDictionary.Add("Head", createSegment(Tile._Texture, Tile._Position, Tile._Color));

            _segmentCount = segmentsDictionary.Keys.Count;
        }

        public Snake(Utility.TextureGenerator TextureGen, int Width, int Height)
        {

            _Texture = TextureGen.CreateTexture(Width, Height, Color.White);
            _EmptyTexture = TextureGen.CreateTexture(Width, Height, Color.Transparent);

            
            segmentsDictionary = new Dictionary<string, SnakeSegment>();

            segmentsDictionary.Add("Head", createSegment(_Texture, new Vector2(5,5), Color.Red));

            _segmentCount = segmentsDictionary.Keys.Count;
        }
    }
}
