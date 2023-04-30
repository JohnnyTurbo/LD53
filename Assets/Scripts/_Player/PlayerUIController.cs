using System.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace TMG.LD53
{
    public class PlayerUIController : MonoBehaviour
    {
        [SerializeField] private Slider _playerHealthSlider;
        [SerializeField] private Slider _playerExperienceSlider;
        [SerializeField] private Image _playerExperienceFillImage;
        [SerializeField] private UpgradeUIController _upgradeUIController;
    
        private bool _isUpgrading;
        
        private void OnEnable()
        {
            _upgradeUIController.OnCloseUpgrade += CloseUpgrade;
            
            var updatePlayerUISystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<UpdatePlayerUISystem>();
            updatePlayerUISystem.OnUpdateHealth += UpdateHealth;
            updatePlayerUISystem.OnUpdateExperience += UpdateExperience;

            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<AccumulateExperienceSystem>().OnLevelUp += OnLevelUp;
        }

        private void CloseUpgrade()
        {
            _isUpgrading = false;
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>().Enabled = true;
        }

        private void UpdateExperience(int playerExperience, int levelExperience)
        {
            _playerExperienceSlider.maxValue = levelExperience;
            _playerExperienceSlider.value = playerExperience;
        }

        private void OnDisable()
        {
            _upgradeUIController.OnCloseUpgrade -= CloseUpgrade;
            
            if (World.DefaultGameObjectInjectionWorld == null) return;
            var updatePlayerUISystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<UpdatePlayerUISystem>();
            updatePlayerUISystem.OnUpdateHealth -= UpdateHealth;
            updatePlayerUISystem.OnUpdateExperience -= UpdateExperience;
            
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<AccumulateExperienceSystem>().OnLevelUp -= OnLevelUp;
        }

        private void UpdateHealth(int curHealth, int maxHealth)
        {
            _playerHealthSlider.maxValue = maxHealth;
            _playerHealthSlider.value = curHealth;
        }

        private void OnLevelUp()
        {
            _playerExperienceSlider.value = _playerExperienceSlider.maxValue;
            _isUpgrading = true;
            StartCoroutine(ExperienceCelebration());
        }

        private IEnumerator ExperienceCelebration()
        {
            var elapsedTime = 0f;
            var cycleDuration = 0.2f;
            Color[] colors = { Color.red, Color.white, Color.blue };
            var curIndex = 0;
            
            while (_isUpgrading)
            {
                while (elapsedTime < cycleDuration)
                {
                    elapsedTime += Time.deltaTime;
                    var t = elapsedTime / cycleDuration;
                    var nextIndex = (curIndex + 1) % colors.Length;
                    _playerExperienceFillImage.color = Color.Lerp(colors[curIndex], colors[nextIndex], t);
                    yield return null;
                }

                elapsedTime = 0f;
                curIndex = (curIndex + 1) % colors.Length;
            }
            _playerExperienceFillImage.color = Color.yellow;
        }
    }
}