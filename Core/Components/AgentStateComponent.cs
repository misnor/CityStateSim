
using CityStateSim.Core.Enums;

namespace CityStateSim.Core.Components;
public struct AgentStateComponent
{
    private AgentState idle;

    public AgentStateComponent(AgentState idle) : this()
    {
        this.idle = idle;
    }

    public AgentState State { get; set; }
}
