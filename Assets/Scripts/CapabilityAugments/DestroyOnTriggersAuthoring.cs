using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace TMG.LD53
{
    [RequireComponent(typeof(TriggerDetectorAuthoring))]
    public class DestroyOnTriggersAuthoring : MonoBehaviour
    {
        [FormerlySerializedAs("CollisionCount")] public int HitCount;

        public class DestroyOnTriggersBaker : Baker<DestroyOnTriggersAuthoring>
        {
            public override void Bake(DestroyOnTriggersAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new DestroyOnTriggers { Value = authoring.HitCount });
            }
        }
    }
}