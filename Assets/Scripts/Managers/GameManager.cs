using UnityEngine;
using UnityEngine.SceneManagement;

namespace JPO
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Player Death")]
        [SerializeField] private float delayBeforeUIAnim = 2f;
        [SerializeField] private float delayBeforeResetScene = 3f;

        private Timer _uiAnimTimer; // Timer that will play the UI animation when expired
        private Timer _resetSceneTimer; // Timer that will reset the scene when expired
        private bool _gameIsPaused;

        private void Awake()
        {
            if (!Instance) Instance = this;
            else if (Instance != this) Destroy(gameObject);
            // Listen for player death on the event bus
            EventBus.playerDies += HandlePlayerDeath;
        }

        private void Update()
        {
            // Handle end of UI anim timer
            if (_uiAnimTimer.ExpiredAndWasRunning())
            {
                _uiAnimTimer = Timer.None;
                UIManager.Instance.ShowBlackScreen();
            }

            // Handle end of reset scene timer
            if (_resetSceneTimer.ExpiredAndWasRunning())
            {
                _resetSceneTimer = Timer.None;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        private void OnDestroy()
        {
            // Cleanup listener
            EventBus.playerDies -= HandlePlayerDeath;
        }

        /// <summary>
        /// Handle the player death
        /// </summary>
        private void HandlePlayerDeath()
        {
            // Start timers
            _uiAnimTimer = Timer.CreateFromSeconds(delayBeforeUIAnim);
            _resetSceneTimer = Timer.CreateFromSeconds(delayBeforeResetScene);
        }

        /// <summary>
        /// Toggle the game pause state
        /// </summary>
        public void ToggleGamePause()
        {
            // Update game is paused
            _gameIsPaused = !_gameIsPaused;

            // Handle the state change
            if (_gameIsPaused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }

            // Invoke the event
            EventBus.OnGamePauseChanged(_gameIsPaused);
        }

        /// <summary>
        /// Resume the game
        /// </summary>
        public void ResumeGame()
        {
            _gameIsPaused = false;
            Time.timeScale = 1f;
            EventBus.OnGamePauseChanged(_gameIsPaused);
        }
    }
}