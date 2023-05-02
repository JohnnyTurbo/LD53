using System;
using System.Collections;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TMG.LD53
{
    public class GameTimer : MonoBehaviour
    {
        public Action OnGameWin;

        public static GameTimer Instance;
        
        [Range(0,60)] [SerializeField] private int _gameMinutes;
        [Range(0,60)] [SerializeField] private int _gameSeconds;

        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private UpgradeUIController _upgradeUIController;
        
        private float _secondsRemaining;

        public float SecondsRemaining => _secondsRemaining;
        
        private bool _isGamePaused;
        private DeliveryGuyInputActions _inputActions;
        private Coroutine _gameTimeRoutine;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            _inputActions = new DeliveryGuyInputActions();
        }

        private void Start()
        {
            _secondsRemaining = _gameMinutes * 60 + _gameSeconds;
            _gameTimeRoutine = StartCoroutine(GameCountdown());
        }

        private void OnEnable()
        {
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<AccumulateExperienceSystem>().OnLevelUp += PauseGame;
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<OnGameOverSystem>().OnGameOver += OnGameOver;
            _upgradeUIController.OnCloseUpgrade += UnPauseGame;    
            _inputActions.Enable();
            _inputActions.GameplayMap.PauseGame.performed += OnPauseGame;
        }

        private void OnGameOver()
        {
            StopCoroutine(_gameTimeRoutine);
            _timerText.gameObject.SetActive(false);
        }

        private void OnPauseGame(InputAction.CallbackContext obj)
        {
            PauseGame();
        }

        public void UnPauseGame()
        {
            _isGamePaused = false;
        }
        
        private void PauseGame()
        {
            _isGamePaused = true;
        }

        private void OnDisable()
        {
            _inputActions.GameplayMap.PauseGame.performed -= OnPauseGame;
            _upgradeUIController.OnCloseUpgrade -= UnPauseGame;    

            if (World.DefaultGameObjectInjectionWorld == null) return;
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<AccumulateExperienceSystem>().OnLevelUp -= PauseGame;
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<OnGameOverSystem>().OnGameOver -= OnGameOver;
        }
        
        private IEnumerator GameCountdown()
        {
            while (_secondsRemaining > 0f)
            {
                if (!_isGamePaused)
                {
                    var timeRemaining = TimeSpan.FromSeconds(_secondsRemaining + 1);
                    _timerText.text = $"Time to End: {timeRemaining.Minutes}:{timeRemaining.Seconds:00}";
                    _secondsRemaining -= Time.deltaTime;
                }
                yield return null;
            }

            _timerText.gameObject.SetActive(false);
            OnGameWin?.Invoke();
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>().Enabled = false;
        }
    }
}