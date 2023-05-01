using Unity.Entities;
using UnityEngine;

namespace TMG.LD53
{
    public class PlayerRendererTagAuthoring : MonoBehaviour
    {
        public class PlayerRendererTagBaker : Baker<PlayerRendererTagAuthoring>
        {
            public override void Bake(PlayerRendererTagAuthoring authoring)
            {
                AddComponent(new PlayerRendererTag());
            }
        }
    }
}