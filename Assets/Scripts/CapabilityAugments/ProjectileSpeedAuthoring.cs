using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace TMG.LD53
{
    public class ProjectileSpeedAuthoring : MonoBehaviour
    {
        public float ProjectileSpeed;

        public class ProjectileSpeedBaker : Baker<ProjectileSpeedAuthoring>
        {
            public override void Bake(ProjectileSpeedAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ProjectileSpeed { Value = authoring.ProjectileSpeed });
            }
        }
    }
}