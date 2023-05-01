using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.LD53
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(CapabilitySystemGroup))]
    [UpdateBefore(typeof(ApplyDamageSystem))]
    public partial class DisplayDamageIconSystem : SystemBase
    {
        public Action<float, float3> OnDamage;

        protected override void OnUpdate()
        {
            foreach (var (damageBuffer, transform) in SystemAPI.Query<DynamicBuffer<DamageBufferElement>, LocalTransform>().WithNone<PlayerTag>())
            {
                foreach (var damage in damageBuffer)
                {
                    OnDamage?.Invoke(damage.Value, transform.Position);
                }
            }
        }
    }
}