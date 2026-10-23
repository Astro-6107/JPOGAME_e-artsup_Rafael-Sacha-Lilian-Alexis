using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace JPO
{
    public class PlayerProjectiles : MonoBehaviour
    {
        [Header("Projectile Settings")]
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private Vector2 firePositionOffset;
        [SerializeField] private int projectilePerWave = 1;
        [SerializeField] private float waveCooldownDuration = 2f;
        [SerializeField] private float delayBetweenProjectile = 1f;

        [Header("Player Detection")]
        [SerializeField] private float playerDetectionRadius = 5f;
        [SerializeField] private LayerMask playerLayer;

        [Header("Damage Feedback")]
        [SerializeField] private SpriteRenderer playerSprite;
        [SerializeField] private float damageBlinkFadeDuration = 0.1f;
        [SerializeField] private float damageBlinkInterval = 0.2f;


        [Header("Item Drop")]
        [SerializeField] private HealthItem healthItemPrefab;
        [SerializeField, Range(0f, 1f)] private float itemDropChance = 0.5f;

        private bool _hasDetectedPlayer;
        private bool _waveIsActive; // If the enemy is currently firing
        private int _waveFiredProjectiles; // Number of projectiles fired during this wave
        private Timer _fireTimer; // Timer between each projectile
        private Timer _waveCooldownTimer; // Timer used to wait for the next fire wave
        private HealthSystem _healthSystem;
        private Vector2 _lastPlayerPosition;
        private Tween _fireFeedbackTween;
        private AudioSource _source;

        private void Awake()
        {
            _healthSystem = GetComponent<HealthSystem>();
            _source = GetComponent<AudioSource>();
            /*_healthSystem.onDamageReceived.AddListener(OnDamageTaken);*/

        }

        private void Update()
        {
            if (!_healthSystem.IsAlive) return;
            // Look in the player direction
            if (_hasDetectedPlayer)
            {
                playerSprite.flipX = _lastPlayerPosition.x < transform.position.x;
            }

            // Start the fire wave
            if (_hasDetectedPlayer && !_waveIsActive && !_waveCooldownTimer.IsRunning)
            {
                _waveIsActive = true;
                _waveFiredProjectiles = 0;
                var dirToPlayer = (_lastPlayerPosition - (Vector2)transform.position).normalized;
               // FireProjectile(dirToPlayer);
            }

            // Fire a new projectile at the end of the timer
            if (_fireTimer.ExpiredAndWasRunning())
            {
                var dirToPlayer = (_lastPlayerPosition - (Vector2)transform.position).normalized;
                //FireProjectile(dirToPlayer);
            }

            // Handle end of wave cooldown timer
            if (_waveCooldownTimer.ExpiredAndWasRunning())
            {
                _waveCooldownTimer = Timer.None;
            }
        }

        private void FixedUpdate()
        {
            if (!_healthSystem.IsAlive) return;
            // Detect the player
            var playerCol = Physics2D.OverlapCircle(transform.position, playerDetectionRadius, playerLayer);
            _hasDetectedPlayer = playerCol != null;
            if (_hasDetectedPlayer) _lastPlayerPosition = playerCol.transform.position;
        }

        private void OnDestroy()
        {
            _fireFeedbackTween?.Kill();

        }

        /// <summary>
        /// Instantiate and setup a projectile
        /// </summary>
        /*public void FireProjectile(Vector2 direction)
        {
            var pos = transform.TransformPoint(firePositionOffset);
            var proj = ObjectPooler.Instance.InstantiatePrefab(projectilePrefab, pos, Quaternion.identity);
            proj.Initialize(gameObject, direction);
            _waveFiredProjectiles++;

            // Play animation
            _fireFeedbackTween?.Kill();
            _fireFeedbackTween = playerSprite.transform.DOPunchScale(new Vector3(1f, -1f) * 0.5f, 0.3f)
                .OnUpdate(() =>
                {
                    // Limit the sprite scale
                    var scale = playerSprite.transform.localScale;
                    scale.x = Mathf.Clamp(scale.x, 1f, 1.5f);
                    scale.y = Mathf.Clamp(scale.y, 0.5f, 1f);
                    playerSprite.transform.localScale = scale;
                })
                // Reset the sprite scale
                .OnComplete(() => playerSprite.transform.localScale = Vector3.one);

            // Start the timer for the next projectile or stop the wave
            if (_waveFiredProjectiles < projectilePerWave)
            {
                _fireTimer = Timer.CreateFromSeconds(delayBetweenProjectile);
            }
            else
            {
                _waveIsActive = false;
                _fireTimer = Timer.None;
                _waveCooldownTimer = Timer.CreateFromSeconds(waveCooldownDuration);
            }
        }*/

        /// <summary>
        /// When the player takes a damage, play hit feedback
        /// </summary>
   
        private void OnDrawGizmosSelected()
        {
            // Set the circles color
            Gizmos.color = Color.red;
            // Draw the projectile spawn point
            Gizmos.DrawWireSphere(transform.TransformPoint(firePositionOffset), 0.1f);
            // Draw the player detection area
            Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
        }
    }
}