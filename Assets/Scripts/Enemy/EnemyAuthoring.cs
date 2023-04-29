using Unity.Entities;
using UnityEngine;

namespace TMG.LD53
{
    public class EnemyAuthoring : MonoBehaviour
    {
        public float BaseMoveSpeed;
        public int BaseHitPoints;
        public MoneyType MoneyType;
        public float DropRate;

        public class BaseMoveSpeedBaker : Baker<EnemyAuthoring>
        {
            public override void Bake(EnemyAuthoring authoring)
            {
                var enemyEntity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<EnemyTag>(enemyEntity);
                AddComponent(enemyEntity, new BaseMoveSpeed { Value = authoring.BaseMoveSpeed });
                AddComponent(enemyEntity, new BaseHitPoints { Value = authoring.BaseHitPoints });
                AddComponent(enemyEntity, new CurHitPoints { Value = authoring.BaseHitPoints });
                AddComponent(enemyEntity, new DropMoneyOnDestroy
                {
                    MoneyType = authoring.MoneyType, 
                    DropRate = authoring.DropRate
                });
            }
        }
    }
}