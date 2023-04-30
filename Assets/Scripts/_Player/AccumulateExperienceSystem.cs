using Unity.Burst;
using Unity.Entities;

namespace TMG.LD53
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
    public partial struct AccumulateExperienceSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var experienceAspect in SystemAPI.Query<PlayerExperienceAspect>())
            {
                experienceAspect.AccumulateExperience();
            }
        }
        
    }
}