using System;
using Unity.Entities;

namespace TMG.LD53
{
    public partial class UpdatePlayerUISystem : SystemBase
    {
        public Action<int, int> OnUpdateHealth;
        public Action<int, int> OnUpdateExperience;
        
        protected override void OnUpdate()
        {
            foreach (var (curHitPoints, baseHitPoints) in SystemAPI.Query<CurHitPoints, BaseHitPoints>().WithAll<PlayerTag>())
            {
                OnUpdateHealth?.Invoke(curHitPoints.Value, baseHitPoints.Value);
            }

            foreach (var experienceAspect in SystemAPI.Query<PlayerExperienceAspect>())
            {
                OnUpdateExperience?.Invoke(experienceAspect.CurrentPlayerExperience,
                    experienceAspect.CurrentLevelExperience);
            }
        }
    }
}