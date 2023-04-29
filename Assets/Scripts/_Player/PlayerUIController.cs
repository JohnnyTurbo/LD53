using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace TMG.LD53
{
    public class PlayerUIController : MonoBehaviour
    {
        [SerializeField] private Slider _playerHealthSlider;

        private void OnEnable()
        {
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<UpdatePlayerUISystem>().OnUpdateHealth +=
                UpdateHealth;
        }

        private void OnDisable()
        {
            if (World.DefaultGameObjectInjectionWorld == null) return;
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<UpdatePlayerUISystem>().OnUpdateHealth -=
                UpdateHealth;
        }

        private void UpdateHealth(int curHealth, int maxHealth)
        {
            _playerHealthSlider.maxValue = maxHealth;
            _playerHealthSlider.value = curHealth;
        }
    }
}