using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace TMG.LD53
{
    public class TestForwardProjectileAuthoring : MonoBehaviour
    {
        public GameObject ProjectilePrefab;
        public float CooldownTime;

        public class TestForwardProjectileBaker : Baker<TestForwardProjectileAuthoring>
        {
            public override void Bake(TestForwardProjectileAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new TestForwardProjectile
                {
                    Prefab = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic),
                    CooldownTime = authoring.CooldownTime,
                    Timer = authoring.CooldownTime
                });
            }
        }
    }
}