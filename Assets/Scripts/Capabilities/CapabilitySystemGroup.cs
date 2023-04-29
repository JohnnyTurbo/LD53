using Unity.Entities;
using Unity.Transforms;

namespace TMG.LD53
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial class CapabilitySystemGroup : ComponentSystemGroup {}
}