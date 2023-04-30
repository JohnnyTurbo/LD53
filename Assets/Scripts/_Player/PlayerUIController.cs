using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace TMG.LD53
{
    public class PlayerUIController : MonoBehaviour
    {
        [SerializeField] private Slider _playerHealthSlider;
        [SerializeField] private Slider _playerExperienceSlider;
        
        private void OnEnable()
        {
            var updatePlayerUISystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<UpdatePlayerUISystem>();
            updatePlayerUISystem.OnUpdateHealth += UpdateHealth;
            updatePlayerUISystem.OnUpdateExperience += UpdateExperience;
        }

        private void UpdateExperience(int playerExperience, int levelExperience)
        {
            _playerExperienceSlider.maxValue = levelExperience;
            _playerExperienceSlider.value = playerExperience;
        }

        private void OnDisable()
        {
            if (World.DefaultGameObjectInjectionWorld == null) return;
            var updatePlayerUISystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<UpdatePlayerUISystem>();
            updatePlayerUISystem.OnUpdateHealth -= UpdateHealth;
            updatePlayerUISystem.OnUpdateExperience -= UpdateExperience;
        }

        private void UpdateHealth(int curHealth, int maxHealth)
        {
            _playerHealthSlider.maxValue = maxHealth;
            _playerHealthSlider.value = curHealth;
        }
    }
}