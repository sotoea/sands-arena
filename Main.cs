using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace sands_arena
{
    public class Main : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        Boolean odd = false;
        Boolean turn = false;

        Particle[,] particles;

        int scale = 5;
        int w = 200;
        int h = 150;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {



            graphics.PreferredBackBufferWidth = w * scale;// GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = h * scale;// GraphicsDevice.DisplayMode.Height;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            var rand = new Random();
            particles = new Particle[w, h];
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    particles[i, j] = new Particle(rand.Next(6), false);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            MouseState state = Mouse.GetState();
            if (state.LeftButton == ButtonState.Pressed)
            {
                particles[state.X / scale, state.Y / scale].type = 4;
            }
            if (state.RightButton == ButtonState.Pressed)
            {
                particles[state.X / scale, state.Y / scale].type = 5;
            }
            if (state.MiddleButton == ButtonState.Pressed)
            {
                particles[state.X / scale, state.Y / scale].type = 3;
            }

            int rand = new Random().Next(2);

            Particle[,] tmp = particles;

            // TODO: Add your update logic here
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h - 1; j++)
                {

                    odd = !odd;
                    if (particles[i, j].turn == turn)
                    {
                        // Plant
                        if(particles[i,j].type == 3)
                        {
                            if (i > 0 &&                    particles[i - 1, j].type == 5)      particles[i - 1, j].type = 3;
                            if (i > 0 && j > 0 &&           particles[i - 1, j - 1].type == 5)  particles[i - 1, j - 1].type = 3;
                            if (i > 0 && j < h - 1 &&       particles[i - 1, j + 1].type == 5)  particles[i - 1, j + 1].type = 3;
                            if (j > 0 &&                    particles[i, j - 1].type == 5)      particles[i, j - 1].type = 3;
                            if (j < h - 1 &&                particles[i, j + 1].type == 5)      particles[i, j + 1].type = 3;
                            if (i < w - 1 &&                particles[i + 1, j].type == 5)      particles[i + 1, j].type = 3;
                            if (i < w - 1 && j > 0 &&       particles[i + 1, j - 1].type == 5)  particles[i + 1, j - 1].type = 3;
                            if (i < w - 1 && j < h - 1 &&   particles[i + 1, j + 1].type == 5)  particles[i + 1, j + 1].type = 3;
                        }
                        // Sand or Water with nothing underneath
                        else if ((particles[i, j].type == 4 || particles[i, j].type == 5) && particles[i, j + 1].type < 4)
                        {
                            tmp[i, j + 1].type = particles[i, j].type;
                            tmp[i, j + 1].turn = !tmp[i, j + 1].turn;
                            tmp[i, j].type = 0;
                        }

                        // Sand with water underneath
                        else if (particles[i, j].type == 4 && particles[i, j + 1].type == 5)
                        {
                            tmp[i, j].type = 5;
                            tmp[i, j + 1].type = 4;
                            tmp[i, j + 1].turn = !tmp[i, j + 1].turn;
                            tmp[i, j].turn = !tmp[i, j].turn;
                        }

                        // Sand with sand underneath
                        else if (particles[i, j].type == 4 && particles[i, j + 1].type == 4)
                        {
                            // left down - nothing
                            if (odd && i > 0 && particles[i - 1, j + 1].type < 4)
                            {
                                tmp[i - 1, j + 1].type = 4;
                                tmp[i - 1, j + 1].turn = !tmp[i - 1, j + 1].turn;
                                tmp[i, j].type = 0;

                            }
                            // right down - nothing
                            else if (!odd && i < w - 1 && particles[i + 1, j + 1].type < 4)
                            {
                                tmp[i + 1, j + 1].type = 4;
                                tmp[i + 1, j + 1].turn = !tmp[i + 1, j + 1].turn;
                                tmp[i, j].type = 0;
                            }
                            // left down - water
                            else if (odd && i > 0 && particles[i - 1, j + 1].type == 5)
                            {
                                tmp[i - 1, j + 1].type = 4;
                                tmp[i - 1, j + 1].turn = !tmp[i - 1, j + 1].turn;
                                tmp[i, j].type = 5;

                            }
                            // right down - water
                            else if (!odd && i < w - 1 && particles[i + 1, j + 1].type == 5)
                            {
                                tmp[i + 1, j + 1].type = 4;
                                tmp[i + 1, j + 1].turn = !tmp[i + 1, j + 1].turn;
                                tmp[i, j].type = 5;
                            }
                        }

                        // Water with water or (sand??) underneath
                        else if (particles[i, j].type == 5 && (particles[i, j + 1].type == 5 || particles[i, j + 1].type == 4))
                        {
                            // left down - nothing
                            if (odd && i > 0 && particles[i - 1, j + 1].type < 4)
                            {
                                tmp[i - 1, j + 1].type = 5;
                                tmp[i - 1, j + 1].turn = !tmp[i - 1, j + 1].turn;
                                tmp[i, j].type = 0;

                            }
                            // right down - nothing
                            else if (!odd && i < w - 1 && particles[i + 1, j + 1].type < 4)
                            {
                                tmp[i + 1, j + 1].type = 5;
                                tmp[i + 1, j + 1].turn = !tmp[i + 1, j + 1].turn;
                                tmp[i, j].type = 0;
                            }
                            // left - nothing
                            else if (odd && i > 0 && particles[i - 1, j].type < 4)
                            {
                                tmp[i - 1, j].type = 5;
                                tmp[i - 1, j].turn = !tmp[i - 1, j].turn;
                                tmp[i, j].type = 0;
                            }
                            // right - nothing
                            else if (!odd && i < w - 1 && particles[i + 1, j].type < 4)
                            {
                                tmp[i + 1, j].type = 5;
                                tmp[i + 1, j].turn = !tmp[i + 1, j].turn;
                                tmp[i, j].type = 0;
                            }
                        }
                    }
                }
            }
            odd = !odd;
            particles = tmp;
            turn = !turn;
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h - 1; j++)
                {
                    particles[i, j].turn = turn;
                }
            }
            //System.Threading.Thread.Sleep(20);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {


            GraphicsDevice.Clear(Color.Black);
            Texture2D pixel;
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[1] { Color.White });

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    Color c = Color.White;
                    if (particles[i, j].type == 0) c = Color.Black;
                    else if (particles[i, j].type == 1) c = Color.Black;
                    else if (particles[i, j].type == 2) c = Color.Black;
                    else if (particles[i, j].type == 3) c = Color.Green;
                    else if (particles[i, j].type == 4) c = Color.White;
                    else if (particles[i, j].type == 5) c = Color.Blue;

                    if (particles[i, j].type > 2)
                        spriteBatch.Draw(pixel, new Vector2(i * scale, j * scale), new Rectangle(0, 0, scale, scale), c);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
