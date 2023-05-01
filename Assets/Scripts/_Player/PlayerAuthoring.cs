using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TMG.LD53
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public float BaseMoveSpeed;
        public int BaseHitPoints;
        
        public class BaseMoveSpeedBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var playerEntity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<PlayerTag>(playerEntity);
                AddComponent(playerEntity, new BaseMoveSpeed { Value = authoring.BaseMoveSpeed });
                AddComponent(playerEntity, new PlayerMoveInput
                {
                    Value = float2.zero,
                    LastHorizontal = 1f
                });
                AddComponent(playerEntity, new BaseHitPoints { Value = authoring.BaseHitPoints });
                AddComponent(playerEntity, new CurHitPoints { Value = authoring.BaseHitPoints });
                AddBuffer<DamageBufferElement>(playerEntity);
                AddBuffer<ExperienceBufferElement>(playerEntity);
                AddComponent<CurrentPlayerExperience>(playerEntity);
                AddComponent(playerEntity, new CurrentLevelExperience { Value = 3 });
                AddComponent<PlayerRenderer>(playerEntity);
            }
        }
    }
}