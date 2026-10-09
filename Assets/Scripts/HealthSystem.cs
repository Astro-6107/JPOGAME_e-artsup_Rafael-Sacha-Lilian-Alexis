using UnityEngine;
using UnityEngine.Events;

namespace JPO
{
    public class HealthSystem : MonoBehaviour, IDamageable
    {
        [SerializeField] private int startHealth = 3;
        public int StartHealth => startHealth;

        [Header("Invulnerability")]
        [SerializeField] private float invulnerabilityTime = 1f;

        public int Health { get; private set; }
        public bool IsAlive => Health > 0;
        public bool IsFullHealth => Health == startHealth;

        [Header("Events"), Space]
        public UnityEvent<int, DamageInfo> onDamageReceived;
        public UnityEvent onInvulnerabilityEnd;
        public UnityEvent<int> onHealthChanged;

        private DeathSystem _death;
        private bool _isInvulnerable;
        private Timer _invulnerabilityTimer;

        private void Start()
        {
            _death = GetComponent<DeathSystem>();
            Health = startHealth;
            onHealthChanged?.Invoke(Health);
        }

        private void Update()
        {
            // Handle the end of the invulnerability timer
            if (_invulnerabilityTimer.ExpiredAndWasRunning())
            {
                // Reset state and invoke the event
                _invulnerabilityTimer = Timer.None;
                _isInvulnerable = false;
                onInvulnerabilityEnd?.Invoke();
            }
        }

        public bool ApplyDamage(DamageInfo damage)
        {
            if (!IsAlive || _isInvulnerable) return false;
            // Update health and invoke event
            Health -= damage.damageAmount;
            onHealthChanged?.Invoke(Health);
            onDamageReceived?.Invoke(Health, damage);
            // Die if health is bellow or at 0
            if (Health <= 0)
            {
                _death.Die(damage.damageOwner);
            }
            // Start the invulnerability timer if the player is not dead
            _invulnerabilityTimer = Timer.CreateFromSeconds(invulnerabilityTime);
            _isInvulnerable = true;
            return true;
        }

        /// <summary>
        /// Add <paramref name="amount"/> health points
        /// </summary>
        public void AddHealth(int amount)
        {
            var oldHealth = Health;
            Health = Mathf.Clamp(Health + amount, 0, startHealth);
            if (oldHealth != Health) onHealthChanged?.Invoke(Health);
        }
    }
}