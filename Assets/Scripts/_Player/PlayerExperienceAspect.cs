using Unity.Entities;

namespace TMG.LD53
{
    public readonly partial struct PlayerExperienceAspect : IAspect
    {
        private readonly RefRW<CurrentPlayerExperience> _currentPlayerExperience;

        public int CurrentPlayerExperience
        {
            get => _currentPlayerExperience.ValueRO.Value;
            set => _currentPlayerExperience.ValueRW.Value = value;
        }

        private readonly RefRW<CurrentLevelExperience> _currentLevelExperience;

        public int CurrentLevelExperience
        {
            get => _currentLevelExperience.ValueRO.Value;
            set => _currentLevelExperience.ValueRW.Value = value;
        }

        private readonly DynamicBuffer<ExperienceBufferElement> _experienceBuffer;

        private bool ShouldLevelUp => CurrentPlayerExperience >= CurrentLevelExperience;
        
        public bool AccumulateExperience()
        {
            if (_experienceBuffer.IsEmpty) return false;
            
            var curExperience = 0;
            foreach (var experience in _experienceBuffer)
            {
                curExperience += experience.Value;
            }
            _experienceBuffer.Clear();
            CurrentPlayerExperience += curExperience;

            if (!ShouldLevelUp) return false;
            
            CurrentPlayerExperience -= CurrentLevelExperience;
            CurrentLevelExperience *= 2;
            return true;

        }
    }
}