using Unity.Entities;
using UnityEngine;

namespace TMG.LD53
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public float BaseMoveSpeed;

        public class BaseMoveSpeedBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var playerEntity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(playerEntity, new BaseMoveSpeed { Value = authoring.BaseMoveSpeed });
                AddComponent<PlayerMoveInput>(playerEntity);
            }
        }
    }
}