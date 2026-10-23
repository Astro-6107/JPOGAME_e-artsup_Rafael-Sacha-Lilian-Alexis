using UnityEngine;
using UnityEngine.Events;

namespace JPO
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float knockBackForce = 10f;
        [SerializeField] private float hittedDisableInputDuration = 0.3f;

        [Header("Events"), Space]
        public UnityEvent onStartMoving;
        public UnityEvent onStopMoving;

        public float MoveSpeed => moveSpeed;
        public FacingDirection FacingDirection { get; private set; }
        public bool IsMoving { get; private set; }
        private PlayerController _controller;
        private HealthSystem _healthSystem;
        private Rigidbody2D _rb;
        private Vector2 _movementDirection;

        private void Awake()
        {
            _controller = GetComponent<PlayerController>();
            _healthSystem = GetComponent<HealthSystem>();
            _rb = GetComponent<Rigidbody2D>();

            FacingDirection = FacingDirection.Right;

            _controller.onMove.AddListener(OnMove);
            _healthSystem.onDamageReceived.AddListener(OnDamageReceived);
        }

        private void FixedUpdate()
        {
            // Update state
            var wasMoving = IsMoving;
            IsMoving = _movementDirection.sqrMagnitude > 0f;
            if (!wasMoving && IsMoving) onStartMoving?.Invoke();
            else if (wasMoving && !IsMoving) onStopMoving?.Invoke();

            // Update movement
            if (_controller.InputIsActive)
            {
                var nextPosition = _rb.position + _movementDirection * (moveSpeed * Time.fixedDeltaTime);
                _rb.MovePosition(nextPosition);
            }
        }

        private void OnDestroy()
        {
            if (!_controller) return;
            _controller.onMove.RemoveListener(OnMove);
            _healthSystem.onDamageReceived.RemoveListener(OnDamageReceived);
        }

        private void OnDamageReceived(int health, DamageInfo info)
        {
            if (health > 0)
            {
                // Disable player inputs for a short duration
                _controller.DisableInputForDuration(hittedDisableInputDuration);
            }
            else
            {
                // Definitively disable inputs
                _controller.DisableInput();
            }

            // Knock back
            _rb.linearVelocity = Vector2.zero;
            _rb.AddForce(info.hitVelocity.normalized * knockBackForce, ForceMode2D.Impulse);
        }

        private void OnMove(Vector2 moveInput)
        {
            // Update the movement direction to move player in the fixed update
            _movementDirection = moveInput.normalized;

            // Update the facing direction value
            FacingDirection = _movementDirection.x switch
            {
                < 0f => FacingDirection.Left,
                > 0f => FacingDirection.Right,
                _ => FacingDirection
            };
        }
    }
}
