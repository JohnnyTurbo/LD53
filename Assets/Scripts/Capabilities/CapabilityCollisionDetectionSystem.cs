using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

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
            var capabilityLookup = SystemAPI.GetComponentLookup<CapabilityTag>(true);
            var capabilityHitLookup = SystemAPI.GetBufferLookup<HitBufferElement>(false);
            
            state.Dependency = new CapabilityCollisionDetectionJob
            {
                PhysicsWorld = physicsWorld,
                CapabilityLookup = capabilityLookup,
                CapabilityHitLookup = capabilityHitLookup
            }.Schedule(simSingleton, state.Dependency);
        }
    }

    public struct CapabilityCollisionDetectionJob : ICollisionEventsJob
    {
        [ReadOnly] public PhysicsWorld PhysicsWorld;
        [ReadOnly] public ComponentLookup<CapabilityTag> CapabilityLookup;
        public BufferLookup<HitBufferElement> CapabilityHitLookup;

        public void Execute(CollisionEvent collisionEvent)
        {
            var entityA = collisionEvent.EntityA;
            var entityB = collisionEvent.EntityB;
            
            var isEntityACapability = CapabilityLookup.HasComponent(entityA);
            var isEntityBCapability = CapabilityLookup.HasComponent(entityB);

            if(!isEntityACapability && !isEntityBCapability){return;}
            
            var capabilityEntity = isEntityACapability ? entityA : entityB;
            var otherEntity = isEntityACapability ? entityB : entityA;
            
            if(!CapabilityHitLookup.HasBuffer(capabilityEntity)) return;

            var hitBuffer = CapabilityHitLookup[capabilityEntity];

            foreach (var hit in hitBuffer)
            {
                if(hit.HitEntity.Equals(otherEntity)) return;
            }
            
            hitBuffer.Add(new HitBufferElement
            {
                HitEntity = otherEntity,
                HitPosition = collisionEvent.CalculateDetails(ref PhysicsWorld).AverageContactPointPosition,
                IsHandled = false
            });
        }
    }
}