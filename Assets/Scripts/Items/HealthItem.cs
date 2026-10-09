using DG.Tweening;
using UnityEngine;

namespace JPO
{
    public class HealthItem : MonoBehaviour
    {
        [SerializeField] private int healthAmount = 1;
        [SerializeField] private Transform sprite;
        [SerializeField] private float delayBeforeDestroy = 0.5f;

        private AudioSource _source;
        private Tween _tween;
        private bool _isCollected;

        private void Start()
        {
            _source = GetComponent<AudioSource>();
            _tween = sprite.DOLocalMoveY(0.15f, 0.5f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Check if the item has already been collected, if the player is colliding and try to get its health
            if (_isCollected) return;
            if (!collision.CompareTag("Player")) return;
            if (!collision.TryGetComponent(out HealthSystem playerHealth)) return;
            _isCollected = true;
            _tween?.Kill();
            sprite.gameObject.SetActive(false);
            playerHealth.AddHealth(healthAmount);
            _source.Play();
            Destroy(gameObject, delayBeforeDestroy);
        }
    }
}