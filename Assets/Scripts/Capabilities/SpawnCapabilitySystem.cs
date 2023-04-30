using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.LD53
{
    [UpdateInGroup(typeof(CapabilitySystemGroup), OrderFirst = true)]
    public partial struct SpawnCapabilitySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
            state.RequireForUpdate<CapabilityPrefabs>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var capabilityPrefabs = SystemAPI.GetSingleton<CapabilityPrefabs>();
            var deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (tag, entity) in SystemAPI.Query<PerformCapabilityTag>().WithEntityAccess())
            {
                if (SystemAPI.HasComponent<CheeseWhipProperties>(entity))
                {
                    var props = SystemAPI.GetComponent<CheeseWhipProperties>(entity);
                    props.Timer -= deltaTime;
                    SystemAPI.SetComponent(entity, props);
                    if (props.Timer > 0f) continue;
                    
                    var newCap = state.EntityManager.Instantiate(capabilityPrefabs.CheeseWhipEntity);
                    var player = SystemAPI.GetSingletonEntity<PlayerTag>();
                    var playerPos = SystemAPI.GetComponent<LocalTransform>(player).Position;
                    var playerFacingDirection = SystemAPI.GetComponent<PlayerMoveInput>(player).LastHorizontal;
                    var mod = playerFacingDirection < 0 ? 1 : 0;
                    var rot = quaternion.Euler(0f, math.PI * ((props.NumberSpawned + mod) % 2), 0f);
                    var whipPos = LocalTransform.FromPositionRotation(playerPos, rot);
                    state.EntityManager.SetComponentData(newCap, whipPos);

                    props.NumberSpawned++;
                    if (props.NumberSpawned < props.NumberToSpawn)
                    {
                        props.Timer = props.TimeBetweenSpawns;
                        SystemAPI.SetComponent(entity, props);
                        continue;
                    }

                    props.Timer = 0f;
                    props.NumberSpawned = 0;
                    SystemAPI.SetComponent(entity, props);
                    state.EntityManager.SetComponentEnabled<PerformCapabilityTag>(entity, false);
                }
                
            }
        }
    }
}