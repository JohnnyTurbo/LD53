using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TMG.LD53
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _aboutButton;
        [SerializeField] private Button _closeAboutButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _tmgButton;
        [SerializeField] private Button _lpButton;
        
        [SerializeField] private GameObject _aboutScreen;
        [FormerlySerializedAs("_aboutPanel")] [SerializeField] private GameObject _optionsPanel;
        
        private void OnEnable()
        {
            _playButton.onClick.AddListener(PlayGame);
            EventSystem.current.SetSelectedGameObject(_playButton.gameObject);
            _aboutButton.onClick.AddListener(() => ToggleAboutScreen(true));
            _closeAboutButton.onClick.AddListener(() => ToggleAboutScreen(false));
            _tmgButton.onClick.AddListener(LaunchTurboYT);
            _lpButton.onClick.AddListener(LaunchLogicYT);
            _quitButton.onClick.AddListener(QuitGame);
        }

        private void Start()
        {
            _aboutScreen.SetActive(false);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveAllListeners();
            _aboutButton.onClick.RemoveAllListeners();
            _closeAboutButton.onClick.RemoveAllListeners();
            _quitButton.onClick.RemoveAllListeners();
        }

        private void PlayGame()
        {
            SceneManager.LoadScene(1);
        }

        private void ToggleAboutScreen(bool shouldShow)
        {
            _aboutScreen.SetActive(shouldShow);
            _optionsPanel.SetActive(!shouldShow);
            var selectedObject = shouldShow ? _closeAboutButton.gameObject : _playButton.gameObject;
            EventSystem.current.SetSelectedGameObject(selectedObject);
        }

        private void LaunchTurboYT()
        {
            Application.OpenURL("https://www.youtube.com/@TurboMakesGames");
        }

        private void LaunchLogicYT()
        {
            Application.OpenURL("https://www.youtube.com/@logicprojects");
        }

        private void QuitGame()
        {
            Application.Quit();
        }
    }
}