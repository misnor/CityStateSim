using System;
using CityStateSim.Infrastructure.Application;
using Microsoft.Xna.Framework;

namespace CityStateSim.UI.Services;
internal class GameControlAdapter : IGameControl
{
    private readonly Game game;

    public GameControlAdapter(Game game) => this.game = game;
    
    public void Exit()
    {
        this.game.Exit();
    }
}
