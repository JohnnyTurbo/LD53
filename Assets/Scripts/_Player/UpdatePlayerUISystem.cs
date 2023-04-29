using System;
using Unity.Entities;

namespace TMG.LD53
{
    public partial class UpdatePlayerUISystem : SystemBase
    {
        public Action<int, int> OnUpdateHealth;
        
        protected override void OnUpdate()
        {
            foreach (var (curHitPoints, baseHitPoints) in SystemAPI.Query<CurHitPoints, BaseHitPoints>().WithAll<PlayerTag>())
            {
                OnUpdateHealth?.Invoke(curHitPoints.Value, baseHitPoints.Value);
            }            
        }
    }
}