using Unity.Burst;
using Unity.Entities;

namespace TMG.LD53
{
    [UpdateInGroup(typeof(CapabilitySystemGroup), OrderLast = true)]
    public partial struct HandleHitBufferSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new HandleHitBufferJob().Schedule();
        }
    }

    [BurstCompile]
    public partial struct HandleHitBufferJob : IJobEntity
    {
        [BurstCompile]
        private void Execute(ref DynamicBuffer<HitBufferElement> hitBuffer)
        {
            for (var i = 0; i < hitBuffer.Length; i++)
            {
                var hit = hitBuffer[i];
                // if (hit.IsHandled) continue;
                hit.IsHandled = true;
                hitBuffer[i] = hit;
            }
        }
    }
    
}