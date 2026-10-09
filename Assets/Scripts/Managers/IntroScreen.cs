using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JPO
{
    public class IntroScreen : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private string menuSceneName = "MenuScene";
        [SerializeField] private float delayBefore = 1f;
        [SerializeField] private float fadeInDuration = 1f;
        [SerializeField] private float visibleDuration = 1f;
        [SerializeField] private float fadeOutDuration = 1f;
        private Sequence _introSeq;

        private void Start()
        {
            // Hide cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            image.SetAlpha(0f);

            _introSeq = DOTween.Sequence().SetEase(Ease.InOutQuad);
            _introSeq.AppendInterval(delayBefore);
            _introSeq.Append(image.DOFade(1f, fadeInDuration));
            _introSeq.AppendInterval(visibleDuration);
            _introSeq.Append(image.DOFade(0f, fadeOutDuration));
            _introSeq.OnComplete(() => SceneManager.LoadScene(menuSceneName));
            _introSeq.Play();
        }

        private void OnDestroy()
        {
            _introSeq?.Kill();
        }
    }
}