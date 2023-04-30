using Unity.Entities;
using UnityEngine;

namespace TMG.LD53
{
    public class KnockBackOnTriggerAuthoring : MonoBehaviour
    {
        public float KnockBackStrength;

        public class KnockBackOnTriggerBaker : Baker<KnockBackOnTriggerAuthoring>
        {
            public override void Bake(KnockBackOnTriggerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new KnockBackOnTrigger { Strength = authoring.KnockBackStrength });
            }
        }
    }
}