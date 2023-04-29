using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace TMG.LD53
{
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateBefore(typeof(DestroyEntitySystem))]
    [UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
    [RequireMatchingQueriesForUpdate]
    public partial struct DropMoneyOnDestroySystem : ISystem, ISystemStartStop
    {
        private MoneyPrefabs _moneyPrefabs;
        
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<MoneyPrefabs>();
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            new DropMoneyOnDestroyJob
            {
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                MoneyPrefabs = _moneyPrefabs
            }.ScheduleParallel();
        }
        

        public void OnStartRunning(ref SystemState state)
        {
            _moneyPrefabs = SystemAPI.GetSingleton<MoneyPrefabs>();
        }

        public void OnStopRunning(ref SystemState state)
        {
        }
    }

    [BurstCompile]
    [WithAll(typeof(DestroyEntityTag))]
    public partial struct DropMoneyOnDestroyJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        [ReadOnly] public MoneyPrefabs MoneyPrefabs;

        [BurstCompile]
        private void Execute(in LocalTransform transform, in DropMoneyOnDestroy dropMoneyOnDestroy,
            [ChunkIndexInQuery] int sortKey)
        {
            // TODO: Implement drop rate?
            var moneyPrefab = MoneyPrefabs.GetPrefab(dropMoneyOnDestroy.MoneyType);
            var newMoney = ECB.Instantiate(sortKey, moneyPrefab);
            var newMoneyTransform = LocalTransform.FromPosition(transform.Position);
            ECB.SetComponent(sortKey, newMoney, newMoneyTransform);
        }
    }

}