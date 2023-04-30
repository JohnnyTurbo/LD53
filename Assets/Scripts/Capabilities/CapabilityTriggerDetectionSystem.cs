using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

namespace TMG.LD53
{
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateAfter(typeof(PhysicsSimulationGroup))]
    [UpdateBefore(typeof(ExportPhysicsWorld))]
    public partial struct CapabilityCollisionDetectionSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SimulationSingleton>();
            state.RequireForUpdate<PhysicsWorldSingleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var simSingleton = SystemAPI.GetSingleton<SimulationSingleton>();
            var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld;
            
            state.Dependency = new CapabilityCollisionDetectionJob
            {
                PhysicsWorld = physicsWorld,
                CapabilityLookup = SystemAPI.GetComponentLookup<CapabilityTag>(true),
                CapabilityHitLookup = SystemAPI.GetBufferLookup<HitBufferElement>(false),
                TransformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true)
            }.Schedule(simSingleton, state.Dependency);
        }
    }

    public struct CapabilityCollisionDetectionJob : ITriggerEventsJob
    {
        [ReadOnly] public PhysicsWorld PhysicsWorld;
        [ReadOnly] public ComponentLookup<CapabilityTag> CapabilityLookup;
        [ReadOnly] public ComponentLookup<LocalTransform> TransformLookup;
        
        public BufferLookup<HitBufferElement> CapabilityHitLookup;

        public void Execute(TriggerEvent triggerEvent)
        {
            var entityA = triggerEvent.EntityA;
            var entityB = triggerEvent.EntityB;
            
            var isEntityACapability = CapabilityLookup.HasComponent(entityA);
            var isEntityBCapability = CapabilityLookup.HasComponent(entityB);

            if(!isEntityACapability && !isEntityBCapability){return;}
            
            var capabilityEntity = isEntityACapability ? entityA : entityB;
            var hitEntity = isEntityACapability ? entityB : entityA;
            
            if(!CapabilityHitLookup.HasBuffer(capabilityEntity)) return;

            var hitBuffer = CapabilityHitLookup[capabilityEntity];

            foreach (var hit in hitBuffer)
            {
                if(hit.HitEntity.Equals(hitEntity)) return;
            }

            var capabilityEntityPos = TransformLookup[capabilityEntity].Position;
            var hitEntityPos = TransformLookup[hitEntity].Position;

            var hitDirection = math.normalizesafe(hitEntityPos - capabilityEntityPos);

            hitBuffer.Add(new HitBufferElement
            {
                HitEntity = hitEntity,
                HitDirection = hitDirection,
                IsHandled = false
            });
        }
    }
}
