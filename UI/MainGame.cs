using System;
using Core.EventBus.Interfaces;
using DefaultEcs;
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

        private readonly ILogger<MainGame> logger;
        private readonly IEcsEventBus ecsEventBus;
        private readonly IAppEventBus appBus;
        private World world;

        public MainGame(ILogger<MainGame> logger, IEcsEventBus ecsEventBus, IAppEventBus appBus, World world)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.logger = logger;
            this.ecsEventBus = ecsEventBus;
            this.appBus = appBus;
            this.world = world;
        }

        protected override void Initialize()
        {
            ecsEventBus.Subscribe<string>(msg =>
                // this never comes back
                logger.LogInformation("ECS bus received: {Message}", msg)
            );

            appBus.Subscribe<int>(value =>
                logger.LogInformation("App bus got int: {Value}", value)
            );

            ecsEventBus.Publish("Hello from ECS bus!");
            appBus.Publish(123);

            logger.LogInformation("MainGame initialized successfully.");
            base.Initialize();
        }

        private void On(in string message)
        {
            logger.LogInformation("Received message: {Message}", message);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
