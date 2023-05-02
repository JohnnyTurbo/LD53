using Unity.Entities;
using UnityEngine;

namespace TMG.LD53
{
    public class EnemyPrefabsAuthoring : MonoBehaviour
    {
        public GameObject WeakEnemy;
        public GameObject FastEntity;
        public GameObject NormalEntity;
        public GameObject StrongEntity;

        public class EnemyPrefabsBaker : Baker<EnemyPrefabsAuthoring>
        {
            public override void Bake(EnemyPrefabsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new EnemyPrefabs
                {
                    WeakEnemy = GetEntity(authoring.WeakEnemy, TransformUsageFlags.Dynamic),
                    FastEntity = GetEntity(authoring.FastEntity, TransformUsageFlags.Dynamic),
                    NormalEntity = GetEntity(authoring.NormalEntity, TransformUsageFlags.Dynamic),
                    StrongEntity = GetEntity(authoring.StrongEntity, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}