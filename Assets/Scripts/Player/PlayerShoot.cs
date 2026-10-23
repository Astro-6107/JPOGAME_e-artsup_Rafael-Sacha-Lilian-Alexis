using UnityEngine;
using UnityEngine.UIElements;

namespace JPO
{
    public class PlayerShoot : MonoBehaviour
    {

        [Header("Gun rotation")]
        [SerializeField] private Transform gunPivot;
        [SerializeField] private SpriteRenderer _gunSprite;
        [SerializeField] private float aimSmoothFactor = 5f;

        [Header("Fire")]
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private TrailRenderer trail;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float shotsDelay;
        private PlayerController _controller;
        private Camera _cam;
        private PlayerProjectiles _pProjectiles;
        private bool _isAiming;
        private float _startLocalFirePointY;
        private Timer _fireTimer;
        private Vector2 _aimingDirection;


        private void Awake()

        {
            _pProjectiles = GetComponent<PlayerProjectiles>();
            _cam = Camera.main;
            _controller = GetComponent<PlayerController>();

            _controller.onAimDirectionChanged.AddListener(UpdateAimingDirection);
            _controller.onAttack.AddListener(StartFiring);
            _controller.onStopAttack.AddListener(StopFiring);
            _startLocalFirePointY = firePoint.localPosition.y;
        }

        private void Update()
        {
            var localFirePos = firePoint.localPosition;
            localFirePos.y = _gunSprite.flipY ? -Mathf.Abs(_startLocalFirePointY) : Mathf.Abs(_startLocalFirePointY);
            firePoint.localPosition = localFirePos;
            // Apply a smooth rotation to the weapon
            var angle = Vector2.SignedAngle(Vector2.up, _aimingDirection);
            gunPivot.localRotation = Quaternion.Lerp(gunPivot.localRotation, Quaternion.Euler(0f, 0f, angle), aimSmoothFactor * Time.deltaTime);
            if (angle > -180f && angle < 0f)
            {
                _gunSprite.flipY = true;
            }
            else
            {
                _gunSprite.flipY = false;
            }
            // Update fire timer
            UpdateFireTimer();
        }


        private void OnDestroy()
        {
            _controller.onAttack.RemoveListener(StartFiring);
            _controller.onStopAttack.RemoveListener(StopFiring);
            _controller.onAimDirectionChanged.RemoveListener(UpdateAimingDirection);
        }

        private void UpdateAimingDirection(Vector2 inputValue)
        {
            // Get the direction from the player to the wanted aiming position
            if (_controller.IsUsingKeyboardAndMouse)
            {
                // If the player is using keyboad and mouse, the input value is the mouse screen position
                // So we convert it to world position and calculate the normalized direction
                var pos = _cam.ScreenToWorldPoint(inputValue);
                pos.z = transform.position.z;
                _aimingDirection = (Vector2)(pos - transform.position).normalized;
            }
            else
            {
                // If the player is using a gamepad, only use the stick orientation normalized
                _aimingDirection = inputValue.normalized;
            }
        }

        private void StartFiring()
        {
            _isAiming = true;
            _fireTimer = Timer.CreateFromSeconds(shotsDelay);
            FireProjectile();
        }

        private void StopFiring()
        {
            _isAiming = false;
            _fireTimer = Timer.None;
        }

        private void FireProjectile()
        {
            var projectile = ObjectPooler.Instance.InstantiatePrefab(projectilePrefab, firePoint.position, Quaternion.identity);
            projectile.Initialize(gameObject, gunPivot.up);
        }

        private void UpdateFireTimer()
        {
            if (!_isAiming) return;
            if (_fireTimer.ExpiredAndWasRunning())
            {
                // On reset le timer du pistolet
                if (_isAiming) _fireTimer = Timer.CreateFromSeconds(shotsDelay);
                else _fireTimer = Timer.None;
                FireProjectile();
            }
        }

        public void SetWeaponSprite(Sprite weaponSprite)
        {
            _gunSprite.sprite = weaponSprite;
        }
    }
}