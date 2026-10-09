using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace JPO
{
    public class PlayerVisual : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer playerSprite;
        [SerializeField] private DamageArea sword;
        [SerializeField] private ParticleSystem moveParticles;

        [Header("Movement Animation")]
        [SerializeField] private float movementRotationAmplitude = 10f;
        [SerializeField] private float movementScaleAmplitude;
        [SerializeField] private float movementFrequency = 20f;

        [Header("Damage Feedback")]
        [SerializeField] private float damageBlinkFadeDuration = 0.1f;
        [SerializeField] private float damageBlinkInterval = 0.2f;
        [SerializeField] private ParticleSystem damageParticles;

        [Header("Camera Shake")]
        [SerializeField] private CinemachineImpulseSource applyDamageCameraShake;
        [SerializeField] private CinemachineImpulseSource takeDamageCameraShake;

        private PlayerMovement _movement;
        private HealthSystem _healthSystem;
        private float _movementTimer; // Used to update the sprite angle with a sine wave
        private Sequence _blinkSequence;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            _healthSystem = GetComponent<HealthSystem>();
            _healthSystem.onDamageReceived.AddListener(OnDamageTaken);
            _healthSystem.onInvulnerabilityEnd.AddListener(OnInvulnerabilityEnd);
            sword.onDamageApplied.AddListener(OnDamageApplied);
        }

        private void Update()
        {
            if (!_healthSystem.IsAlive) return;
            // Update sprite flip x
            playerSprite.flipX = _movement.FacingDirection == FacingDirection.Left;

            if (_movement.IsMoving)
            {
                _movementTimer += Time.deltaTime * movementFrequency;

                // Animate rotation
                var angle = Mathf.Sin(_movementTimer) * movementRotationAmplitude;
                playerSprite.transform.localRotation = Quaternion.Euler(0f, 0f, angle);

                // Animate scale
                var verticalModifier = Mathf.Sin(_movementTimer) * movementScaleAmplitude;
                var newScale = 1f + verticalModifier;
                playerSprite.transform.localScale = new Vector3(1f, newScale, 1f);

                // Handle move particles
                if (!moveParticles.isPlaying) moveParticles.Play();
                var shape = moveParticles.shape;
                var arc = shape.arc;
                shape.rotation = new Vector3(0f, 0f, _movement.FacingDirection == FacingDirection.Right
                    ? 180f - arc * 0.5f
                    : -arc * 0.5f
                );
            }
            else
            {
                _movementTimer = 0f;
                playerSprite.transform.localRotation = Quaternion.identity;
                playerSprite.transform.localScale = Vector3.one;
                if (moveParticles.isPlaying) moveParticles.Stop();
            }
        }

        private void OnDestroy()
        {
            _blinkSequence?.Kill();
            if (!_healthSystem) return;
            _healthSystem.onDamageReceived.RemoveListener(OnDamageTaken);
            _healthSystem.onInvulnerabilityEnd.RemoveListener(OnInvulnerabilityEnd);
            sword.onDamageApplied.RemoveListener(OnDamageApplied);
        }

        /// <summary>
        /// When the player takes a damage, play hit feedback
        /// </summary>
        private void OnDamageTaken(int health, DamageInfo _)
        {
            // Setup and play the blink feedback with a DOTween sequence
            _blinkSequence?.Kill();
            _blinkSequence = DOTween.Sequence().SetLoops(-1);
            _blinkSequence.Append(playerSprite.DOFade(0f, damageBlinkFadeDuration));
            _blinkSequence.AppendInterval(damageBlinkInterval);
            _blinkSequence.Append(playerSprite.DOFade(1f, damageBlinkFadeDuration));
            _blinkSequence.AppendInterval(damageBlinkInterval);
            _blinkSequence.Play();
            // Play camera shake
            takeDamageCameraShake.GenerateImpulse();
            // Play particles
            damageParticles.Play();
        }

        private void OnDamageApplied(IDamageable _)
        {
            applyDamageCameraShake.GenerateImpulse();
        }

        /// <summary>
        /// Stop the blink feedback at the end of invulnerability
        /// </summary>
        private void OnInvulnerabilityEnd()
        {
            _blinkSequence?.Kill();
            // Reset sprite alpha to 1
            playerSprite.SetAlpha(1f);
            // Cleanup sequence
            _blinkSequence = null;
        }
    }
}