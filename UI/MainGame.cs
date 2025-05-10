using System;
using Gameplay.Services.Interfaces;
using Gameplay.Simulation.Interfaces;
using Infrastructure.Factories.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UI
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private TimeSpan accumulator = TimeSpan.Zero;
        private static readonly TimeSpan baseInterval = TimeSpan.FromMilliseconds(100);

        private readonly ILogger<MainGame> logger;
        private readonly ISimulationRunner simulationRunner;
        private readonly ITickSpeedService tickSpeedService;

        public MainGame(
            ILogger<MainGame> logger, 
            ISimulationRunner simulationRunner, 
            ITickSpeedService tickSpeedService)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.logger = logger;
            this.simulationRunner = simulationRunner;
            this.tickSpeedService = tickSpeedService;
        }

        protected override void Initialize()
        {
            logger.LogInformation("MainGame initialized successfully.");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            { 
                Exit();
            }

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

            base.Draw(gameTime);
        }
    }
}
