using Unity.Entities;

namespace TMG.LD53
{
    public struct EnemyPrefabs : IComponentData
    {
        public Entity WeakEnemy;
        public Entity FastEntity;
        public Entity NormalEntity;
        public Entity StrongEntity;
    }
}