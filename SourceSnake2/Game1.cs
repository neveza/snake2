using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

using LevelUtility;
using Utility;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TextureGenerator textureGen;
        Random random;

        Foods.Food food;

        Snake snake;
        Snake oldSnake;
        bool Collision = true;

        Level level;
        string[] levelAddress;
        int currentLevel = 0;
        int nextLevel = 0;

        bool paused = true;


        //int foodToEat;

        void CheckCollision()
        {
            var headRect = snake.segmentsDictionary["Head"].rect;
            var headPos = snake.segmentsDictionary["Head"].Position;
            if (Collision)
            {
                foreach (var key in snake.segmentsDictionary.Keys)
                {
                    if (key != "Head")
                    {
                        if (snake.segmentsDictionary[key].Position == headPos)
                        {
                            snake.isDead = true;

                        }
                    }
                }

                var wallList = level.GetList();

                foreach (var wall in wallList)
                {
                    if (headRect.Intersects(wall._Rectangle))
                    {
                        snake.isDead = true;
                    }
                    //if (food.foodblock.getRectangle().Intersects( wall._Rectangle))
                    //{
                    //    food.createFood();
                    //}
                }

                
                foreach (var aFood in level.GetFoods())
                {
                    if (headRect.Intersects(aFood.foodblock.rect))
                    {
                        aFood.isEaten = true;
                        snake.SnakeAteCounter++;
                        snake.addSegment();
                        food = aFood;
                    }
                }

                if (food != null)
                {
                    level.Delete(food);
                    food = null;
                }

            }



        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 50);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            random = new Random();
            textureGen = new TextureGenerator(GraphicsDevice);
            snake = null;
            oldSnake = null;
            food = null;

            var directory = System.IO.Directory.GetCurrentDirectory();
            directory = directory + @"\Levels";

            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            levelAddress = System.IO.Directory.GetFiles(directory);

            var ScreenHeight = graphics.GraphicsDevice.Viewport.Height;
            var ScreenWidth = graphics.GraphicsDevice.Viewport.Width;

            Collision = true;

            base.Initialize();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadLevel();
            oldSnake = new Snake(snake);

        }

        void LoadLevel(int levelBlock)
        {

            var data = SerializerUtility.DataManagementXML.Load(levelAddress[levelBlock]);

            if (data.Count > 0)
            {
                level = LevelGenerator.GenerateLevel(data[0], GraphicsDevice);
            }

            List<Tiles.Tile> ToDelete = new List<Tiles.Tile>();
            foreach (var tile in level.GetList())
            {
                if (tile._Type == (int)Tiles.TileTypes.Food)
                {
                    level.AddTile(new Foods.Food(tile));
                    ToDelete.Add(tile);
                }
                else if (tile._Type == (int)Tiles.TileTypes.Snake)
                {
                    if (snake == null)
                    {
                        snake = new Snake(tile);
                       
                    }
                    else
                        snake.UpdateHeadPosition(tile._Position);

                    ToDelete.Add(tile);
                }
            }

            foreach (var item in ToDelete)
            {
                level.Delete(item);
            }
        }

        void LoadLevel()
        {
            if (nextLevel == levelAddress.Length)
            {
                nextLevel = 0;
            }

            var data = SerializerUtility.DataManagementXML.Load(levelAddress[nextLevel]);

            nextLevel++;
            currentLevel = nextLevel - 1;

            if (data.Count > 0)
            {
                level = LevelGenerator.GenerateLevel(data[0], GraphicsDevice);
            }

            List<Tiles.Tile> ToDelete = new List<Tiles.Tile>();
            foreach (var tile in level.GetList())
            {
                if (tile._Type == (int)Tiles.TileTypes.Food)
                {
                    level.AddTile(new Foods.Food(tile));
                    ToDelete.Add(tile);
                }
                else if (tile._Type == (int)Tiles.TileTypes.Snake)
                {
                    if (snake == null)
                    {
                        snake = new Snake(tile);
                    }
                    else
                        snake.UpdateHeadPosition(tile._Position);
                    ToDelete.Add(tile);
                }


            }

            foreach (var item in ToDelete)
            {
                level.Delete(item);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here

            if (UserInput.getXAxis() != 0 || UserInput.getYAxis() != 0)
            {
                paused = false;
            }

            if (!paused)
            {
                snake.Movement();

                CheckCollision();

                if (level.getFoodCount() <= 0)
                {
                    snake.Movement(Vector2.Zero);
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        oldSnake = new Snake(snake);
                        paused = true;
                        LoadLevel();
                    }

                }

                //if (Keyboard.GetState().IsKeyDown(Keys.S))
                //{
                //    loadLevel();
                //}

                if (snake.isDead)
                {
                    snake.Movement(new Vector2(0, 0));
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        LoadLevel(currentLevel);
                        snake = new Snake(oldSnake);
                        snake.UpdateHeadPosition(oldSnake.getPositionHead());

                        snake.isDead = false;
                        paused = true;
                    }
                }
            }

            

            base.Update(gameTime);
        }
        //int x = 0;
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            var SnakeDrawList = snake.DrawOrder();
            var WallDrawList = level.GetList();

            //spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Matrix.CreateScale(8f));
            //spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, transform);
            spriteBatch.Begin();

            foreach (var wall in WallDrawList)
            {
                spriteBatch.Draw(wall._Texture, wall._Rectangle, wall._Color);
            }

            foreach (var food in level.GetFoods())
            {
                spriteBatch.Draw(food.foodblock.Texture, food.foodblock.rect, food.foodblock.Color);
            }

            if (snake.isDead != true)
            {
                foreach (var segment in SnakeDrawList)
                {
                    spriteBatch.Draw(segment.Texture, segment.rect, segment.Color);
                }
            }
            
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
