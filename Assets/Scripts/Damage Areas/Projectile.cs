using DG.Tweening;
using UnityEngine;

namespace JPO
{
    public class Projectile : DamageArea
    {
        [Header("Projectile Settings")]
        [SerializeField] private SpriteRenderer projectileSprite;
        [SerializeField] private float speed = 4f;
        [SerializeField] private float lifeSpan = 5f;

        [Header("Animation Settings")]
        [SerializeField] private Vector2 punchScale = new Vector2(0.5f, 0.5f);
        [SerializeField] private float punchDuration = 0.5f;
        [SerializeField] private int punchVibrato = 1;

        private Collider2D _col;
        private Rigidbody2D _rb;
        private Vector2 _direction;
        private Timer _lifeSpanTimer;
        private Tween _tween;

        private void Awake()
        {
            _col = GetComponent<Collider2D>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            _col.enabled = false;
            _lifeSpanTimer = Timer.CreateFromSeconds(lifeSpan);
            // Setup and start the animation
            projectileSprite.transform.localScale = Vector3.one;
            _tween = projectileSprite.transform.DOPunchScale(punchScale, punchDuration, punchVibrato).SetLoops(-1);
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }

        private void Update()
        {
            // At the end of the lifespan timer, cleanup the timer and disable this object
            if (_lifeSpanTimer.ExpiredAndWasRunning())
            {
                _lifeSpanTimer = Timer.None;
                ObjectPooler.Instance.ReleaseObject(gameObject);
            }
        }

        private void FixedUpdate()
        {
            // Update projectile position based on its direction and speed
            var nextPos = _rb.position + _direction * (speed * Time.fixedDeltaTime);
            _rb.MovePosition(nextPos);
        }

        /// <summary>
        /// Initialize the projectile
        /// </summary>
        public void Initialize(GameObject owner, Vector2 direction)
        {
            AddObjectToIgnore(owner);
            // Update movement direction and enable collisions
            _direction = direction;
            _col.enabled = true;
            // Update damage info owner and velocity
            damageInfo.damageOwner = owner;
            damageInfo.hitVelocity = direction * (speed * Time.fixedDeltaTime);
        }

        protected override void HandleDamageApplied(IDamageable damageable)
        {
            // Disable the object from the object pooler
            ObjectPooler.Instance.ReleaseObject(gameObject);
        }
    }
}