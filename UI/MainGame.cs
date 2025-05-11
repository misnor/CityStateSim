using System;
using DefaultEcs;
using Gameplay.Services.Interfaces;
using Gameplay.Simulation.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UI.Factories.Interfaces;
using UI.Rendering;
using UI.Rendering.Interfaces;

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

        public MainGame(
            ILogger<MainGame> logger,
            ISimulationRunner simulationRunner,
            ITickSpeedService tickSpeedService,
            IRenderService renderService,
            IFontFactory fontFactory,
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
            this.world = world;
        }

        protected override void Initialize()
        {
            logger.LogInformation("MainGame initialized successfully.");

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            // Ensure mapgeneration occurs.
            simulationRunner.Tick();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            renderService.Initialize(GraphicsDevice, Content);
            fontFactory.RegisterFont("DefaultFont", Content.Load<SpriteFont>("DefaultFont"));
        }

        protected override void Update(GameTime gameTime)
        {
            accumulator += gameTime.ElapsedGameTime;

            var interval = TimeSpan.FromTicks(baseInterval.Ticks / tickSpeedService.CurrentMultiplier);

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
