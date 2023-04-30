using Unity.Entities;
using UnityEngine;

namespace TMG.LD53
{
    public class ExperiencePointValueAuthoring : MonoBehaviour
    {
        public int ExperienceValue;

        public class ExperiencePointValueBaker : Baker<ExperiencePointValueAuthoring>
        {
            public override void Bake(ExperiencePointValueAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ExperiencePointValue { Value = authoring.ExperienceValue });
            }
        }
    }
}