using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.LD53
{
    public partial class SpawnEnemySystem : SystemBase
    {
        private float _nextSpawnTime;
        private const float SPAWN_INTERVAL = 30;
        private EnemyPrefabs _enemyPrefabs;
        private Entity _playerEntity;
        private Random _random;
        
        protected override void OnCreate()
        {
            RequireForUpdate<EnemyPrefabs>();
            RequireForUpdate<PlayerTag>();
            _random = Random.CreateFromIndex(100);
        }

        protected override void OnStartRunning()
        {
            _enemyPrefabs = SystemAPI.GetSingleton<EnemyPrefabs>();
            _playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
            if(GameTimer.Instance.SecondsRemaining >= 298f)
                _nextSpawnTime = 10000000f;
        }

        protected override void OnUpdate()
        {
            var playerPos = SystemAPI.GetComponent<LocalTransform>(_playerEntity).Position;
            if (GameTimer.Instance.SecondsRemaining <= _nextSpawnTime)
            {
                var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
                var ecb = ecbSingleton.CreateCommandBuffer(World.Unmanaged);
                SpawnNextWave(playerPos, ref ecb);
                _nextSpawnTime = GameTimer.Instance.SecondsRemaining - SPAWN_INTERVAL;
            }
        }

        private void SpawnNextWave(float3 playerPos, ref EntityCommandBuffer ecb)
        {
            var nextWave = EnemyWaveController.Instance.GetNextWave();

            for (var i = 0; i < nextWave.WeakCount; i++)
            {
                SpawnEnemy(_enemyPrefabs.WeakEnemy, playerPos, ecb);
            }
            
            for (var i = 0; i < nextWave.FastCount; i++)
            {
                SpawnEnemy(_enemyPrefabs.FastEntity, playerPos, ecb);
            }
            
            for (var i = 0; i < nextWave.MediumCount; i++)
            {
                SpawnEnemy(_enemyPrefabs.NormalEntity, playerPos, ecb);
            }
            
            for (var i = 0; i < nextWave.StrongCount; i++)
            {
                SpawnEnemy(_enemyPrefabs.StrongEntity, playerPos, ecb);
            }
        }

        private void SpawnEnemy(Entity prefab, float3 playerPos, EntityCommandBuffer ecb)
        {
            var angle = _random.NextFloat(2 * math.PI);
            var x = playerPos.x + 25 * math.cos(angle);
            var y = playerPos.z + 25 * math.sin(angle);

            var newEnemy = ecb.Instantiate(prefab);
            var newPos = new float3(x, 0f, y);
            var newTransform = LocalTransform.FromPosition(newPos);
            ecb.SetComponent(newEnemy, newTransform);
        }
    }
}