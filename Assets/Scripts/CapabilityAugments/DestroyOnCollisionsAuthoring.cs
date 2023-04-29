using Unity.Entities;
using UnityEngine;

namespace TMG.LD53
{
    public class DestroyOnCollisionsAuthoring : MonoBehaviour
    {
        public int CollisionCount;

        public class DestroyOnCollisionsBaker : Baker<DestroyOnCollisionsAuthoring>
        {
            public override void Bake(DestroyOnCollisionsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new DestroyOnCollisions { Value = authoring.CollisionCount });
            }
        }
    }
}