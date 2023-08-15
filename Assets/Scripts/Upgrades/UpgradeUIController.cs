using System;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
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
            EventSystem.current.SetSelectedGameObject(_upgradeButtons[0]);
        }

        private void SetUpgradeButton(GameObject buttonGO, UpgradeElement upgradeElement)
        {
            var button = buttonGO.GetComponent<Button>();
            button.onClick.RemoveAllListeners();

            if (upgradeElement.Level >= upgradeElement.Descriptions.Length)
            {
                button.onClick.AddListener(NoUpgradeForYou);
                var titleText1 = buttonGO.transform.Find("TitleText").GetComponent<TextMeshProUGUI>();
                titleText1.text = $"{upgradeElement.Name}: Max Level";
                
                var descriptionText1 = buttonGO.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
                descriptionText1.text =
                    "This item is at max level. Clicking this will close the upgrade window and not upgrade anything else!";

                var iconImage1 = buttonGO.transform.Find("Icon").GetComponent<Image>();
                iconImage1.sprite = upgradeElement.Icon;
                
                return;
            }
            
            button.onClick.AddListener(() => CloseUpgrade(upgradeElement.InvokeUpgrade));
            
            var titleText = buttonGO.transform.Find("TitleText").GetComponent<TextMeshProUGUI>();
            titleText.text = $"{upgradeElement.Name}: Level {upgradeElement.Level}";

            var descriptionText = buttonGO.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
            descriptionText.text = upgradeElement.Descriptions[upgradeElement.Level];

            var iconImage = buttonGO.transform.Find("Icon").GetComponent<Image>();
            iconImage.sprite = upgradeElement.Icon;
        }

        private void CloseUpgrade(UpgradeWeapon upgradeWeapon)
        {
            upgradeWeapon?.Invoke();
            _upgradeMenu.SetActive(false);
            OnCloseUpgrade?.Invoke();
        }

        private void NoUpgradeForYou()
        {
            _upgradeMenu.SetActive(false);
            OnCloseUpgrade?.Invoke();
        }
    }
}