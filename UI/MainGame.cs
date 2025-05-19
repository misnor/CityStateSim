using System;
using DefaultEcs;
using CityStateSim.Gameplay.Services.Interfaces;
using CityStateSim.Gameplay.Simulation.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CityStateSim.UI.Factories.Interfaces;
using CityStateSim.UI.Rendering;
using CityStateSim.UI.Rendering.Interfaces;
using CityStateSim.UI.Camera;
using Microsoft.Xna.Framework.Input;
using CityStateSim.Core.Commands;
using CityStateSim.Gameplay.Commands;

namespace UI
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private readonly World world;
        private TimeSpan accumulator = TimeSpan.Zero;
        private static readonly TimeSpan baseInterval = TimeSpan.FromMilliseconds(100);

        private readonly ILogger<MainGame> logger;
        private readonly ISimulationRunner simulationRunner;
        private readonly ITickSpeedService tickSpeedService;
        private readonly IRenderService renderService;
        private readonly IFontFactory fontFactory;
        private readonly ITextureFactory textureFactory;
        private readonly Camera2D camera;
        private readonly ICommandDispatcher dispatcher;

        public MainGame(
            ILogger<MainGame> logger,
            ISimulationRunner simulationRunner,
            ITickSpeedService tickSpeedService,
            IRenderService renderService,
            IFontFactory fontFactory,
            ITextureFactory textureFactory,
            Camera2D camera,
            ICommandDispatcher dispatcher,
            World world)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            this.logger = logger;
            this.simulationRunner = simulationRunner;
            this.tickSpeedService = tickSpeedService;
            this.renderService = renderService;
            this.fontFactory = fontFactory;
            this.textureFactory = textureFactory;
            this.camera = camera;
            this.dispatcher = dispatcher;
            this.world = world;
        }

        protected override void Initialize()
        {
            logger.LogInformation("MainGame initialized successfully.");

            fontFactory.RegisterFont("DefaultFont", Content.Load<SpriteFont>("DefaultFont"));
            textureFactory.RegisterTexture("whitePixel", Content.Load<Texture2D>("whitePixel"));
            textureFactory.RegisterTexture("tool_axe_single", Content.Load<Texture2D>("tool_axe_single"));
            textureFactory.RegisterTexture("cross_large", Content.Load<Texture2D>("cross_large"));
            textureFactory.RegisterTexture("tool_pickaxe", Content.Load<Texture2D>("tool_pickaxe"));
            textureFactory.RegisterTexture("wood_pile", Content.Load<Texture2D>("wood_pile"));
            textureFactory.RegisterTexture("stone", Content.Load<Texture2D>("stone"));
            textureFactory.RegisterTexture("grass", Content.Load<Texture2D>("grass"));
            textureFactory.RegisterTexture("tree", Content.Load<Texture2D>("tree"));
            textureFactory.RegisterTexture("rock", Content.Load<Texture2D>("rock"));
            textureFactory.RegisterTexture("humanoid1", Content.Load<Texture2D>("humanoid1"));
            textureFactory.RegisterTexture("humanoid2", Content.Load<Texture2D>("humanoid2"));
            textureFactory.RegisterTexture("humanoid3", Content.Load<Texture2D>("humanoid3"));
            textureFactory.RegisterTexture("stockpile_solid", Content.Load<Texture2D>("stockpile_solid"));

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            // Initialize camera with graphics device
            camera.Initialize(GraphicsDevice);
            camera.Zoom = 1f;

            // Ensure mapgeneration occurs.
            simulationRunner.Tick();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            renderService.Initialize(GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            accumulator += gameTime.ElapsedGameTime;

            var interval = TimeSpan.FromTicks(baseInterval.Ticks / tickSpeedService.CurrentMultiplier);

            // handle camera movement with WSAD, fire off MoveCameraCommand
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                this.dispatcher.Dispatch(new MoveCameraCommand(0, -1));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                this.dispatcher.Dispatch(new MoveCameraCommand(0,1));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                this.dispatcher.Dispatch(new MoveCameraCommand(-1, 0));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                this.dispatcher.Dispatch(new MoveCameraCommand(1, 0));
            }


            while (accumulator >= interval)
            {
                simulationRunner.Tick();
                accumulator -= interval;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            renderService.Draw(new SpriteBatch(GraphicsDevice), world);
            base.Draw(gameTime);
        }
    }
}
