using Unity.Entities;
using UnityEngine;

namespace TMG.LD53
{
    [RequireComponent(typeof(CollisionDetectorAuthoring))]
    public class DamageOnCollisionAuthoring : MonoBehaviour
    {
        public int DamageValue;

        public class DamageOnCollisionBaker : Baker<DamageOnCollisionAuthoring>
        {
            public override void Bake(DamageOnCollisionAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new DamageOnCollision { Value = authoring.DamageValue });
            }
        }
    }
}