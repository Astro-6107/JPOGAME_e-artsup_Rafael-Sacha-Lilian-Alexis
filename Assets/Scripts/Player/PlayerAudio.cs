using DG.Tweening;
using UnityEngine;

namespace JPO
{
    public class PlayerAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource oneShotAudioSource;
        [SerializeField] private AudioSource footstepSource;
        [SerializeField] private float footstepInterval = 0.2f;
        [SerializeField] private AudioClip hittedAudio;
        [SerializeField] private AudioClip deathAudio;
        [SerializeField] private float deathDelay = 0.3f;

        private PlayerMovement _movement;
        private HealthSystem _health;
        private Timer _footstepTimer;

        private void Start()
        {
            _movement = GetComponent<PlayerMovement>();
            _health = GetComponent<HealthSystem>();

            _movement.onStartMoving.AddListener(StartFootstepAudio);
            _movement.onStopMoving.AddListener(StopFootstepAudio);
            _health.onDamageReceived.AddListener(PlayHittedAudio);
        }

        private void Update()
        {
            if (_footstepTimer.ExpiredAndWasRunning())
            {
                // Restart the timer and play the audio
                _footstepTimer = Timer.CreateFromSeconds(footstepInterval);
                footstepSource.Play();
            }
        }

        private void OnDestroy()
        {
            _movement.onStartMoving.RemoveListener(StartFootstepAudio);
            _movement.onStopMoving.RemoveListener(StopFootstepAudio);
            _health.onDamageReceived.RemoveListener(PlayHittedAudio);
        }

        /// <summary>
        /// Start the footstep timer
        /// </summary>
        private void StartFootstepAudio()
        {
            footstepSource.Play();
            _footstepTimer = Timer.CreateFromSeconds(footstepInterval);
        }

        /// <summary>
        /// Stop the footstep timer
        /// </summary>
        private void StopFootstepAudio()
        {
            _footstepTimer = Timer.None;
        }

        private void PlayHittedAudio(int health, DamageInfo info)
        {
            oneShotAudioSource.PlayOneShot(hittedAudio, 0.5f);
            if (health <= 0)
            {
                DOVirtual.DelayedCall(deathDelay, () => oneShotAudioSource.PlayOneShot(deathAudio, 0.25f));
            }
        }
    }
}