using Unity.Entities;
using UnityEngine;

namespace TMG.LD53
{
    public class EnemyAuthoring : MonoBehaviour
    {
        public float BaseMoveSpeed;

        public class BaseMoveSpeedBaker : Baker<EnemyAuthoring>
        {
            public override void Bake(EnemyAuthoring authoring)
            {
                var enemyEntity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<EnemyTag>(enemyEntity);
                AddComponent(enemyEntity, new BaseMoveSpeed { Value = authoring.BaseMoveSpeed });
            }
        }
    }
}