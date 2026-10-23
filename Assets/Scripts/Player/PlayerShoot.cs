using UnityEngine;

namespace JPO
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private Transform gunPivot;
        [SerializeField] private TrailRenderer trail;
        [SerializeField] private float aimSmoothFactor = 5f;
        private PlayerController _controller;
        private PlayerProjectiles _pProjectiles;
        private Camera _cam;
        private bool _isAiming;
        private Vector2 _lastAimingDirection;

        private void Awake()
        {
            _cam = Camera.main;
            _controller = GetComponent<PlayerController>();

            _controller.onAimDirectionChanged.AddListener(UpdateAimingDirection);
        }

        private void OnDestroy()
        {

            _controller.onAimDirectionChanged.RemoveListener(UpdateAimingDirection);
        }

        /*private void StartAiming()
        {
            gunPivot.gameObject.SetActive(true);
            trail.Clear();
            trail.emitting = true;
            _isAiming = true;
            UpdateAimingDirection(_lastAimingDirection);
        }

        private void StopAiming()
        {
            gunPivot.gameObject.SetActive(false);
            trail.emitting = false;
            _isAiming = false;
        }*/

        private void UpdateAimingDirection(Vector2 inputValue)
        {
            _lastAimingDirection = inputValue;
            // Get the direction from the player to the wanted aiming position
            Vector2 aimDirection;
            if (_controller.IsUsingKeyboardAndMouse)
            {
                // If the player is using keyboad and mouse, the input value is the mouse screen position
                // So we convert it to world position and calculate the normalized direction
                var pos = _cam.ScreenToWorldPoint(inputValue);
                pos.z = transform.position.z;
                aimDirection = (Vector2)(pos - transform.position).normalized;
                FireProjectile(aimDirection);
            }
            else
            {
                // If the player is using a gamepad, only use the stick orientation normalized
                aimDirection = inputValue.normalized;
            }
            // Apply a smooth rotation to the weapon
            var angle = Vector2.SignedAngle(Vector2.up, aimDirection);
            gunPivot.localRotation = Quaternion.Lerp(gunPivot.localRotation, Quaternion.Euler(0f, 0f, angle), aimSmoothFactor * Time.deltaTime);
        }

        void FireProjectile(Vector2 direction)
        {
            print("direction du projectile : ");
            print(direction);
        }
    }
}