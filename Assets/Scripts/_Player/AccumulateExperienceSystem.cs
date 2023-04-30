using System;
using Unity.Burst;
using Unity.Entities;

namespace TMG.LD53
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
    public partial class AccumulateExperienceSystem : SystemBase
    {
        public Action OnLevelUp;
        
        [BurstCompile]
        protected override void OnUpdate()
        {
            foreach (var experienceAspect in SystemAPI.Query<PlayerExperienceAspect>())
            {
                if (!experienceAspect.AccumulateExperience()) continue;

                OnLevelUp?.Invoke();
                
                var simulationSystemGroup = World.GetExistingSystemManaged<SimulationSystemGroup>();
                simulationSystemGroup.Enabled = false;
            }
        }
    }
}