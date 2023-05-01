using Unity.Collections;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace TMG.LD53
{
    public class GameOverUIController : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverScreen;
        [SerializeField] private GameObject _winScreen;
        [SerializeField] private GameObject _pauseScreen;
        [SerializeField] private SubScene _townScene;
        [SerializeField] private GameObject _expSlider;
        
        private EntityQuery _spawnedQuery;

        private DeliveryGuyInputActions _inputActions;

        private void Awake()
        {
            _inputActions = new DeliveryGuyInputActions();
        }

        private void OnEnable()
        {
            _inputActions.Enable();
            _inputActions.GameplayMap.PauseGame.performed += ShowPauseScreen;

            GameTimer.Instance.OnGameWin += ShowWinScreen;
            
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<OnGameOverSystem>().OnGameOver +=
                ShowGameOverScreen;
            
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<OnWinSystem>().OnWin +=
                ShowWinScreen;
        }

        private void OnDisable()
        {
            _inputActions.Disable();
            _inputActions.GameplayMap.PauseGame.performed -= ShowPauseScreen;

            GameTimer.Instance.OnGameWin -= ShowWinScreen;
            
            if (World.DefaultGameObjectInjectionWorld == null) return;
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<OnGameOverSystem>().OnGameOver -=
                ShowGameOverScreen;
            
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<OnWinSystem>().OnWin -=
                ShowWinScreen;
        }
        
        private void ShowPauseScreen(InputAction.CallbackContext obj)
        {
            _pauseScreen.SetActive(true);
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>().Enabled = false;
        }


        private void Start()
        {
            var sqBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<PerformCapabilityTag>()
                .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState);
            _spawnedQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(sqBuilder);
        }

        public void OnButtonHidePauseScreen()
        {
            _pauseScreen.SetActive(false);
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>().Enabled = true;
        }
        
        private void ShowGameOverScreen()
        {
            _gameOverScreen.SetActive(true);
            _expSlider.SetActive(false);
        }

        private void ShowWinScreen()
        {
            _winScreen.SetActive(true);
        }

        public void OnButtonPlayAgain()
        {
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>().Enabled = true;
            World.DefaultGameObjectInjectionWorld.EntityManager.DestroyEntity(_spawnedQuery);
            SceneSystem.UnloadScene(World.DefaultGameObjectInjectionWorld.Unmanaged, _townScene.SceneGUID);
            SceneManager.LoadScene(1);
        }

        public void OnButtonMainMenu()
        {
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>().Enabled = true;
            World.DefaultGameObjectInjectionWorld.EntityManager.DestroyEntity(_spawnedQuery);
            SceneSystem.UnloadScene(World.DefaultGameObjectInjectionWorld.Unmanaged, _townScene.SceneGUID);
            SceneManager.LoadScene(0);
        }
    }
}