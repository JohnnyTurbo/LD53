using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

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
                AddComponent<PlayerMoveInput>(playerEntity);
                AddComponent(playerEntity, new BaseHitPoints { Value = authoring.BaseHitPoints });
                AddComponent(playerEntity, new CurHitPoints { Value = authoring.BaseHitPoints });
            }
        }
    }
}