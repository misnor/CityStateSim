using System;
using DefaultEcs;
using Gameplay.Services.Interfaces;
using Gameplay.Simulation.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public MainGame(
            ILogger<MainGame> logger,
            ISimulationRunner simulationRunner,
            ITickSpeedService tickSpeedService,
            IRenderService renderService,
            World world)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.logger = logger;
            this.simulationRunner = simulationRunner;
            this.tickSpeedService = tickSpeedService;
            this.renderService = renderService;
            this.world = world;
        }

        protected override void Initialize()
        {
            logger.LogInformation("MainGame initialized successfully.");
            
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
