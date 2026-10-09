using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace JPO
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Input Events"), Space]
        public UnityEvent<Vector2> onMove;
        public UnityEvent onAttack;
        public UnityEvent onStopAttack;

        private PlayerInput _pInput;
        private Timer _disabledInputTimer;
        public bool InputIsActive => _pInput.inputIsActive;

        private void Awake()
        {
            _pInput = GetComponent<PlayerInput>();
            SetupInputListeners();
        }

        private void Update()
        {
            // Handles end of disabled input timer
            if (_disabledInputTimer.ExpiredAndWasRunning())
            {
                _disabledInputTimer = Timer.None;
                _pInput.ActivateInput();
            }
        }

        private void OnDestroy()
        {
            RemoveInputListeners();
        }

        /// <summary>
        /// Disable the player inputs for a duration
        /// </summary>
        public void DisableInputForDuration(float duration)
        {
            _disabledInputTimer = Timer.CreateFromSeconds(duration);
            _pInput.DeactivateInput();
        }

        /// <summary>
        /// Disable player inputs
        /// </summary>
        public void DisableInput()
        {
            _pInput.DeactivateInput();
        }

        /// <summary>
        /// Setup listeners for player input
        /// </summary>
        private void SetupInputListeners()
        {
            if (!_pInput) return;
            _pInput.actions.FindAction("Player/Move").performed += OnMovePerformed;
            _pInput.actions.FindAction("Player/Move").canceled += OnMoveCanceled;
            _pInput.actions.FindAction("Player/Attack").performed += OnAttackPerformed;
            _pInput.actions.FindAction("Player/Attack").canceled += OnAttackCanceled;
            _pInput.actions.FindAction("Player/Pause").performed += OnPausePerformed;
        }

        /// <summary>
        /// Remove listeners for player input
        /// </summary>
        private void RemoveInputListeners()
        {
            if (!_pInput) return;
            _pInput.actions.FindAction("Player/Move").performed -= OnMovePerformed;
            _pInput.actions.FindAction("Player/Move").canceled -= OnMoveCanceled;
            _pInput.actions.FindAction("Player/Attack").performed -= OnAttackPerformed;
            _pInput.actions.FindAction("Player/Attack").canceled -= OnAttackCanceled;
            _pInput.actions.FindAction("Player/Pause").performed -= OnPausePerformed;
        }

        /// <summary>
        /// When the move input action is performed, get the movement value
        /// </summary>
        private void OnMovePerformed(InputAction.CallbackContext ctx)
        {
            onMove?.Invoke(ctx.ReadValue<Vector2>());
        }

        /// <summary>
        /// When the move input action is canceled, reset the movement value
        /// </summary>
        private void OnMoveCanceled(InputAction.CallbackContext ctx)
        {
            onMove?.Invoke(Vector2.zero);
        }

        /// <summary>
        /// When the attack input action is performed
        /// </summary>
        private void OnAttackPerformed(InputAction.CallbackContext ctx)
        {
            onAttack?.Invoke();
        }

        /// <summary>
        /// When the attack input action is performed
        /// </summary>
        private void OnAttackCanceled(InputAction.CallbackContext ctx)
        {
            onStopAttack?.Invoke();
        }

        private void OnPausePerformed(InputAction.CallbackContext ctx)
        {
            GameManager.Instance.ToggleGamePause();
        }
    }
}