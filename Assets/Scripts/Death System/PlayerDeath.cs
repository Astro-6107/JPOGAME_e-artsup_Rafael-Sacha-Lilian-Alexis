using DG.Tweening;
using UnityEngine;

namespace JPO
{
    public class PlayerDeath : DeathSystem
    {
        [SerializeField] private float stopMovementDelay = 0.3f;
        [SerializeField] private float delayBeforeDeathAnim = 1.2f;
        [SerializeField] private SpriteRenderer playerSprite;
        [SerializeField] private float scaleDuration = 0.5f;
        [SerializeField] private Ease scaleEasing = Ease.InBack;

        private Rigidbody2D _rb;
        private Timer _stopMovementTimer; // Timer that will stop the player movement when expired
        private Timer _deathAnimTimer; // Timer that will start the death animation when expired

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            // Handle end of stop movement timer
            if (_stopMovementTimer.ExpiredAndWasRunning())
            {
                // Cleanup timer and stop movement
                _stopMovementTimer = Timer.None;
                _rb.linearVelocity = Vector2.zero;
            }

            // Handle end of death anim timer
            if (_deathAnimTimer.ExpiredAndWasRunning())
            {
                // Cleanup timer and play animation
                _deathAnimTimer = Timer.None;
                playerSprite.transform.DOScaleY(0f, scaleDuration)
                    .SetEase(scaleEasing)
                    .OnComplete(() => Destroy(gameObject));
            }
        }

        public override void Die(GameObject killer)
        {
            // Start timers and invoke the player death event in the event bus
            _stopMovementTimer = Timer.CreateFromSeconds(stopMovementDelay);
            _deathAnimTimer = Timer.CreateFromSeconds(delayBeforeDeathAnim);
            EventBus.OnPlayerDies();
        }
    }
}