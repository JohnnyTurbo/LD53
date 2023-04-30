using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

namespace TMG.LD53
{
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateAfter(typeof(PhysicsSimulationGroup))]
    [UpdateBefore(typeof(ExportPhysicsWorld))]
    public partial struct ItemTriggerDetectionSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<SimulationSingleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var simSingleton = SystemAPI.GetSingleton<SimulationSingleton>();
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.Dependency = new ItemTriggerDetectionJob
            {
                ItemLookup = SystemAPI.GetComponentLookup<ItemTag>(),
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged)
            }.Schedule(simSingleton, state.Dependency);
        }
    }
    
    public struct ItemTriggerDetectionJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<ItemTag> ItemLookup;
        public EntityCommandBuffer ECB;
        
        public void Execute(TriggerEvent triggerEvent)
        {
            var entityA = triggerEvent.EntityA;
            var entityB = triggerEvent.EntityB;

            var isEntityAItem = ItemLookup.HasComponent(entityA);
            var isEntityBItem = ItemLookup.HasComponent(entityB);

            if (!isEntityAItem && !isEntityBItem) return;

            var itemEntity = isEntityAItem ? entityA : entityB;
            var playerEntity = isEntityAItem ? entityB : entityA;
            
            // Debug.Log($"Player {playerEntity.ToString()} found Item: {itemEntity.ToString()}");
            ECB.AddComponent(itemEntity, new ItemEntityTarget { Value = playerEntity });
        }
    }
}