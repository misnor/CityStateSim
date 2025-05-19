using CityStateSim.Core.Commands;
using CityStateSim.Core.Components;
using CityStateSim.Gameplay.Commands;
using DefaultEcs;

namespace CityStateSim.Gameplay.Handlers;
public class CreatePickupJobCommandHandler : ICommandHandler<CreatePickupJobCommand>
{
    private readonly World world;
    public CreatePickupJobCommandHandler(World world)
    {
        this.world = world;
    }

    public void Handle(CreatePickupJobCommand command)
    {
        // spawn a one‐off job entity at the resource tile
        var job = world.CreateEntity();
        job.Set(new PositionComponent(command.X, command.Y));
        job.Set(new JobComponent(command.X, command.Y, JobType.PickupResource) { Status = JobStatus.Pending });

    }
}
