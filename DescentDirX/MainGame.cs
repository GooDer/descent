using DescentDirX.Helpers;
using DescentDirX.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using DescentDirX.UI;
using DescentDirX.BusEvents.General;
using NGuava;

namespace DescentDirX
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static EventBus EVENT_BUS = new EventBus();

        private GameScene gameScene;

        private MouseState mouseCurrentState;
        private MouseState mouseLastState;

        private float ratioX = 1;
        private float ratioY = 1;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;            
        }

        private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            //graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferMultiSampling = true;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            GameCamera.Instance.ScreenViewport = graphics.GraphicsDevice.Viewport;
            ImageProvider.Graphics = graphics;

            Mouse.WindowHandle = this.Window.Handle;
            mouseCurrentState = Mouse.GetState();
            mouseLastState = Mouse.GetState();

            if (graphics.IsFullScreen)
            {
                ratioX = (graphics.PreferredBackBufferWidth / (float)graphics.GraphicsDevice.DisplayMode.Width);
                ratioY = (graphics.PreferredBackBufferHeight / (float)graphics.GraphicsDevice.DisplayMode.Height);
            }

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

            // Load all needed images and store them for later use
            foreach (ImageListEnum img in Enum.GetValues(typeof(ImageListEnum)))
            {
                var dir = img.ToString().Split('_')[0].ToLower();
                var name = img.ToString().Substring(img.ToString().IndexOf('_') + 1);
                var path = @"images\" + dir + @"\" + name.Replace('_', '-').ToLower();
                ImageProvider.Instance.RegisterImage(img, Content.Load<Texture2D>(path));
            }

            // load font resource
            FontFactory.Font = Content.Load<SpriteFont>(@"fonts\font-big");

            // initialize custom mouse cursor
            this.IsMouseVisible = true;
            CursorHelper.LoadCustomCursor(@"Content\images\cursors\SUnormal.cur", Window.Handle);

            // initialize game screen after all resources are loaded
            gameScene = new GameScene();
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

            base.Update(gameTime);

            mouseLastState = mouseCurrentState;
            mouseCurrentState = Mouse.GetState();

            var mouseX = (int)Math.Ceiling(Mouse.GetState().X * ratioX);
            var mouseY = (int)Math.Ceiling(Mouse.GetState().Y * ratioY);

            if (mouseLastState.LeftButton == ButtonState.Pressed && mouseCurrentState.LeftButton == ButtonState.Released)
            {
                EVENT_BUS.Post(new ClickMessage(this, new Vector2(mouseX, mouseY)));
            }

            gameScene.Update(mouseX, mouseY);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(gameScene.GetBackgroundColor());

            spriteBatch.Begin();

            gameScene.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
