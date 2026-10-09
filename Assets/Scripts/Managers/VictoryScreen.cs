using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JPO
{
    public class VictoryScreen : MonoBehaviour
    {
        [Header("Black Screen")]
        [SerializeField] private Image blackScreen;
        [SerializeField] private float fadeDuration = 1f;

        [Header("Leave Screen")]
        [SerializeField] private float delayBeforeAllowLeaving = 5f;
        [SerializeField] private TMP_Text continueText;
        [SerializeField] private float textFadeDuration = 0.5f;
        [SerializeField] private string menuSceneName = "MenuScene";
        private Timer _timer;
        private Tween _textTween;
        private bool _canLeaveScreen;

        private void Start()
        {
            // Fade animation
            blackScreen.SetAlpha(1f);
            blackScreen.gameObject.SetActive(true);
            blackScreen.DOFade(0f, fadeDuration).SetEase(Ease.Linear)
                .OnComplete(() => blackScreen.gameObject.SetActive(false));

            // Hide continue text
            continueText.gameObject.SetActive(false);

            _timer = Timer.CreateFromSeconds(delayBeforeAllowLeaving);
            _canLeaveScreen = false;
        }

        private void Update()
        {
            if (_timer.ExpiredAndWasRunning() && !_canLeaveScreen)
            {
                _timer = Timer.None;
                _canLeaveScreen = true;

                // Show text
                continueText.gameObject.SetActive(true);
                _textTween = continueText.DOFade(0f, textFadeDuration)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutQuad);
            }

            if (_canLeaveScreen)
            {
                // Check if any key is pressed to leave this screen
                var keyboardKeyPressed = Keyboard.current.anyKey.wasPressedThisFrame;
                var gp = Gamepad.current;
                var gamepadButtonPressed = gp != null && (gp.buttonNorth.wasPressedThisFrame ||
                    gp.buttonEast.wasPressedThisFrame ||
                    gp.buttonSouth.wasPressedThisFrame ||
                    gp.buttonWest.wasPressedThisFrame ||
                    gp.startButton.wasPressedThisFrame);
                if (keyboardKeyPressed || gamepadButtonPressed)
                {
                    SceneManager.LoadScene(menuSceneName);
                }
            }
        }

        private void OnDestroy()
        {
            _textTween?.Kill();
        }
    }
}