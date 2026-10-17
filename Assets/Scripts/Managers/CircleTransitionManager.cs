using DG.Tweening;
using UnityEngine;

namespace JPO
{
    public class CircleTransitionManager : MonoBehaviour
    {
        public static CircleTransitionManager Instance { get; private set; }
        [SerializeField] private RectTransform circle;
        [SerializeField] private RectTransform background;
        private RectTransform _rectT;
        private CanvasGroup _group;
        private Camera _cam;

        private void Awake()
        {
            Instance = this;
            _group = GetComponent<CanvasGroup>();
            _cam = Camera.main;
            _rectT = GetComponent<RectTransform>();
            // Initialize background size
            background.sizeDelta = new Vector2(Screen.width, Screen.height);
            _group.alpha = 0f;
        }

        /// <summary>
        /// Play the circle transition from game view to a black screen, centering the circle on the given world position
        /// </summary>
        public Sequence TransitionToBlack(Vector2 worldPosition, float duration)
        {
            var seq = DOTween.Sequence().SetEase(Ease.Linear);
            seq.AppendCallback(() =>
            {
                // Center circle on position
                var screenPoint = _cam.WorldToScreenPoint(worldPosition);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectT, screenPoint, null, out var localPoint);
                circle.anchoredPosition = localPoint;
                background.anchoredPosition = -circle.anchoredPosition;
                // Set initial circle size
                var size = Screen.width + Screen.width * 0.15f;
                circle.sizeDelta = new Vector2(size, size);
                // Show transition
                _group.alpha = 1f;
            });
            // Animate circle
            seq.Append(DOTween.To(
                () => circle.sizeDelta,
                x => circle.sizeDelta = x,
                Vector2.zero,
                duration
            ));
            return seq;
        }

        /// <summary>
        /// Play the circle transition from black screen to game view, centering the circle on the given world position
        /// </summary>
        public Sequence TransitionToGame(Vector2 worldPosition, float duration)
        {
            var seq = DOTween.Sequence()
                .SetEase(Ease.Linear)
                .OnComplete(() => _group.alpha = 0f);
            seq.AppendCallback(() =>
            {
                // Center circle on position
                var screenPoint = _cam.WorldToScreenPoint(worldPosition);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectT, screenPoint, null, out var localPoint);
                circle.anchoredPosition = localPoint;
                background.anchoredPosition = -circle.anchoredPosition;
                // Set initial circle size
                circle.sizeDelta = Vector2.zero;
                // Show transition and hide screen at the end
                _group.alpha = 1f;
            });
            // Animate circle
            var size = Screen.width + Screen.width * 0.15f;
            seq.Append(DOTween.To(
                () => circle.sizeDelta,
                x => circle.sizeDelta = x,
                new Vector2(size, size),
                duration
            ));
            return seq;
        }
    }
}