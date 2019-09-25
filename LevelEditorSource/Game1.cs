using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

using Tiles;
using LevelUtility;
//using TextureGenerator;
namespace PixelTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Utility.TextureGenerator TextureGen;
        LevelUtility.Level level;

        List<Tile> Boarders;

        Tile cursor;
        Tile[] Tiles;
        int tCount;
        int expansionSpeed = 1;

        bool paused;


        Matrix transform;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //TargetElapsedTime = TimeSpan.FromTicks(333333);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            TextureGen = new Utility.TextureGenerator(GraphicsDevice);
            Boarders = new List<Tile>();

            var ScreenHeight = graphics.GraphicsDevice.Viewport.Height;
            var ScreenWidth = graphics.GraphicsDevice.Viewport.Width;
            Boarders.Add(new Tile(TileTypes.Wall, TextureGen.CreateTexture(ScreenWidth, 1, Color.Brown), new Vector2(0,0), Color.Brown));

            Boarders.Add(new Tile(TileTypes.Wall, TextureGen.CreateTexture(ScreenWidth, 1, Color.Blue), new Vector2(0, ScreenHeight -1), Color.Blue));

            Boarders.Add(new Tile(TileTypes.Wall, TextureGen.CreateTexture(1, ScreenHeight, Color.Pink), new Vector2(0, 0), Color.Pink));

            Boarders.Add(new Tile(TileTypes.Wall, TextureGen.CreateTexture(1, ScreenHeight, Color.Orange), new Vector2(ScreenWidth - 1, 0), Color.Orange));

            Tiles = new Tile[]
            {
                new Tile(TileTypes.Snake, TextureGen.CreateTexture(10, 10, Color.White), new Vector2(-1,-1), Color.Red, false),
                new Tile(TileTypes.Food, TextureGen.CreateTexture(10, 10, Color.White), new Vector2(-1,-1), Color.Chocolate, false),
                new Tile(TileTypes.Wall, TextureGen.CreateTexture(10, 10, Color.White), new Vector2(-1,-1), Color.White, false)

            };

            tCount = Tiles.Length - 1;

            cursor = Tiles[tCount];
            level = new LevelUtility.Level();

            paused = false;

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

            var data = SerializerUtility.DataManagementXML.Load("level0.dat");

            if (data.Count > 0)
            {
                level = LevelGenerator.GenerateLevel(data[0], GraphicsDevice);
            }

            // TODO: use this.Content to load your game content here
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
        /// 
        float x;
        float y;
        double delay = 0;
        protected override void Update(GameTime gameTime)
        {
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            x += -1 * Utility.UserInput.getXAxis();
            y += -1 * Utility.UserInput.getYAxis();

            transform = Matrix.CreateTranslation(x, y, 0);

            var MouseX = Mouse.GetState().X;
            var MouseY = Mouse.GetState().Y;
            
            var mousePosition = new Vector2(MouseX, MouseY);
            cursor.Update(Mouse.GetState().Position.ToVector2());


            Console.WriteLine(paused);


            if (delay < gameTime.TotalGameTime.TotalSeconds)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Back))
                {
                    if (paused)
                    {
                        paused = false;
                    }
                    else
                        paused = true;

                    delay = gameTime.TotalGameTime.TotalSeconds + .25;
                }

                Console.WriteLine(paused);

                if (!paused)
                { 
                    if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                {
                    expansionSpeed = 5;
                }
                    if (Keyboard.GetState().IsKeyUp(Keys.LeftShift))
                {
                    expansionSpeed = 1;
                }
                    if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    var width = cursor._Texture.Width + (1 * expansionSpeed);
                    var height = cursor._Texture.Height;
                    var texture = TextureGen.CreateTexture(width, height, cursor._Color);
                    cursor.Update(texture);
                }
                    if (Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    Console.WriteLine(cursor._Texture.Width);
                    if (cursor._Texture.Width > 5)
                    {
                        var width = cursor._Texture.Width - (1 * expansionSpeed);
                        var height = cursor._Texture.Height;
                        var texture = TextureGen.CreateTexture(width, height, cursor._Color);
                        cursor.Update(texture);
                    }
                }
                    if (Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    var width = cursor._Texture.Width;
                    var height = cursor._Texture.Height + (1 * expansionSpeed);
                    var texture = TextureGen.CreateTexture(width, height, cursor._Color);
                    cursor.Update(texture);
                }
                    if (Keyboard.GetState().IsKeyDown(Keys.O))
                {
                    if (cursor._Texture.Height > 5)
                    {
                        var width = cursor._Texture.Width;
                        var height = cursor._Texture.Height - (1 * expansionSpeed);
                        var texture = TextureGen.CreateTexture(width, height, cursor._Color);
                        cursor.Update(texture);
                    }
                }


                    if (Mouse.GetState().LeftButton == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        level.AddTile(cursor);
                        delay = gameTime.TotalGameTime.TotalSeconds + .25;
                    }

                    if (Mouse.GetState().RightButton == ButtonState.Pressed)
                    {
                        List<Tile> Delete = new List<Tile>();
                        foreach (var block in level.GetList())
                        {
                            if (cursor._Rectangle.Intersects(block._Rectangle))
                            {
                                Delete.Add(block);
                            }
                        }
                        foreach (var block in Delete)
                        {
                            level.Delete(block);
                        }
                        Delete.Clear();

                        delay = gameTime.TotalGameTime.TotalSeconds + .25;

                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.T))
                    {

                        if (tCount <= 0)
                        {
                            tCount = Tiles.Length - 1;
                        }
                        else
                            tCount--;

                        cursor = Tiles[tCount];

                        delay = gameTime.TotalGameTime.TotalSeconds + .25;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.L))
                    {
                        System.Windows.Forms.OpenFileDialog oDialogue = new System.Windows.Forms.OpenFileDialog();

                        oDialogue.Filter = "Data Files (*.dat) | *.dat ";
                        oDialogue.RestoreDirectory = true;

                        if (oDialogue.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            var stream = oDialogue.OpenFile();
                            var data = SerializerUtility.DataManagementXML.Load(stream);

                            if (data.Count > 0)
                            {
                                level = LevelGenerator.GenerateLevel(data[0], GraphicsDevice);
                            }
                        }

                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        System.Windows.Forms.SaveFileDialog sDialogue = new System.Windows.Forms.SaveFileDialog();

                        sDialogue.Filter = "Data Files (*.dat) | *.dat ";
                        sDialogue.RestoreDirectory = true;

                        if (sDialogue.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            var fileStream = sDialogue.OpenFile();
                            SerializerUtility.DataManagementXML.Save(fileStream, level.GenerateBlueprint());
                        }

                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Delete))
                    {
                        level.Delete();
                    }

                }
            }
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, transform);
           // spriteBatch.Begin();

            //cursor.Draw(spriteBatch);
            foreach (var boarder in Boarders)
            {
                spriteBatch.Draw(boarder._Texture, boarder._Position, boarder._Color);
            }

            spriteBatch.Draw(cursor._Texture, cursor._Position, cursor._Color);

            var tiles = level.GetList();

            if (tiles.Count > 0)
            {
                foreach (var tile in tiles)
                {
                    spriteBatch.Draw(tile._Texture, tile._Position, tile._Color);
                }
             
            }


            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
