using System;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace TMG.LD53
{
    public delegate void UpgradeWeapon();
    
    public class UpgradeUIController : MonoBehaviour
    {
        public Action OnCloseUpgrade;
        
        [SerializeField] private GameObject _upgradeMenu;
        [SerializeField] private UpgradeElement[] _upgradeElements;
        [SerializeField] private GameObject[] _upgradeButtons;
        
        private void OnEnable()
        {
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<AccumulateExperienceSystem>().OnLevelUp += OnLevelUp;
        }

        private void OnDisable()
        {
            if (World.DefaultGameObjectInjectionWorld == null) return;
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<AccumulateExperienceSystem>().OnLevelUp -= OnLevelUp;
        }

        private void OnLevelUp()
        {
            _upgradeMenu.SetActive(true);
            
            // TODO: randomly pick upgrades

            for (var i = 0; i < _upgradeButtons.Length; i++)
            {
                SetUpgradeButton(_upgradeButtons[i], _upgradeElements[i]);
            }
        }

        private void SetUpgradeButton(GameObject buttonGO, UpgradeElement upgradeElement)
        {
            var button = buttonGO.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => CloseUpgrade(upgradeElement.InvokeUpgrade));
            
            var titleText = buttonGO.transform.Find("TitleText").GetComponent<TextMeshProUGUI>();
            titleText.text = $"{upgradeElement.Name}: Level {upgradeElement.Level}";

            var descriptionText = buttonGO.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
            descriptionText.text = upgradeElement.Descriptions[0];

            var iconImage = buttonGO.transform.Find("Icon").GetComponent<Image>();
            iconImage.sprite = upgradeElement.Icon;
        }

        private void CloseUpgrade(UpgradeWeapon upgradeWeapon)
        {
            upgradeWeapon?.Invoke();
            _upgradeMenu.SetActive(false);
            OnCloseUpgrade?.Invoke();
        }
    }
}