using CityStateSim.Core.Commands;
using CityStateSim.Core.Components;
using CityStateSim.Gameplay.Commands;
using DefaultEcs;

namespace CityStateSim.Gameplay.Handlers;
public class CreateDeliverJobCommandHandler : ICommandHandler<CreateDeliverJobCommand>
{
    private World world;

    public CreateDeliverJobCommandHandler(World world)
    {
        this.world = world;
    }

    public void Handle(CreateDeliverJobCommand command)
    {
        // spawn the deliver job at the stockpile tile
        var job = world.CreateEntity();
        job.Set(new PositionComponent(command.TargetX, command.TargetY));
        job.Set(new JobComponent(command.TargetX, command.TargetY, JobType.DeliverResource) { Status = JobStatus.Pending });
    }
}
