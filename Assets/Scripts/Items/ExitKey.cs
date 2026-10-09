using DG.Tweening;
using UnityEngine;

namespace JPO
{
    public class ExitKey : MonoBehaviour
    {
        [SerializeField] private Transform keySprite;
        [SerializeField] private float delayBeforeDestroy = 0.5f;
        private Tween _keyTween;
        private bool _isCollected;
        private AudioSource _source;

        private void Start()
        {
            _source = GetComponent<AudioSource>();
            _keyTween = keySprite.DOLocalMoveY(0.15f, 0.5f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            if (_isCollected) return;
            _isCollected = true;
            EventBus.OnKeyCollected();
            _keyTween?.Kill();
            keySprite.gameObject.SetActive(false);
            _source.Play();
            Destroy(gameObject, delayBeforeDestroy);
        }
    }
}