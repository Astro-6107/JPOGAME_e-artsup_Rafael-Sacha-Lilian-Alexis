using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;



#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JPO
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private string gameSceneName = "GameScene1";
        [SerializeField] private CanvasGroup buttonsGroup;
        [SerializeField] private Image blackScreen;
        [SerializeField] private float blackScreenFadeDuration = 1f;

        private void Start()
        {
            // Play enter transition
            blackScreen.SetAlpha(1f);
            blackScreen.gameObject.SetActive(true);
            blackScreen.DOFade(0f, blackScreenFadeDuration)
                // Load the game scene after the transition
                .OnComplete(() => blackScreen.gameObject.SetActive(false));
        }

        #region Buttons
        /// <summary>
        /// Go to the game scene
        /// </summary>
        public void StartGame()
        {
            // Disable navigation
            buttonsGroup.interactable = false;

            // Play transition
            blackScreen.SetAlpha(0f);
            blackScreen.gameObject.SetActive(true);
            blackScreen.DOFade(1f, blackScreenFadeDuration)
                // Load the game scene after the transition
                .OnComplete(() => SceneManager.LoadScene(gameSceneName));
        }

        /// <summary>
        /// Quit the game
        /// </summary>
        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        #endregion
    }
}