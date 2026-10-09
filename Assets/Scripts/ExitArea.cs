using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JPO
{
    public class ExitArea : MonoBehaviour
    {
        [SerializeField] private string nextSceneName;

        [Header("Ladder Sprites")]
        [SerializeField] private SpriteRenderer ladderSprite;
        [SerializeField] private Sprite lockedSprite;
        [SerializeField] private Sprite unlockedSprite;
        [SerializeField] private GameObject padlockSprite;

        [Header("Arrow Animation")]
        [SerializeField] private Transform arrow;

        [Header("Player Exit Animation")]
        [SerializeField] private Vector2 animExitPosition;
        [SerializeField] private float delayBeforeClimbingLadder = 0.5f;
        [SerializeField] private float delayBeforeBlackScreen = 0.5f;

        private Sequence _playerExitSeq;
        private AudioSource _source;
        private Collider2D _col;
        private bool _isLocked;
        private float _arrowStartY;
        private Tween _arrowTween;

        private void Start()
        {
            _col = GetComponent<Collider2D>();
            _source = GetComponent<AudioSource>();
            _isLocked = true;
            _col.isTrigger = false;
            ladderSprite.sprite = lockedSprite;
            padlockSprite.SetActive(true);
            arrow.gameObject.SetActive(false);
            _arrowStartY = arrow.localPosition.y;
            EventBus.keyCollected += UnlockExit;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!CanExit(collision.gameObject)) return;
            var playerGo = collision.gameObject;
            var playerT = playerGo.transform;
            var playerSprite = playerGo.GetComponentInChildren<SpriteRenderer>();
            // Disable inputs
            var controller = playerGo.GetComponent<PlayerController>();
            var movement = playerGo.GetComponent<PlayerMovement>();
            controller.DisableInput();
            // Disable collider
            collision.enabled = false;
            // Animate the player exit
            var distance = Vector3.Distance(transform.position, playerT.position);
            var speed = movement.MoveSpeed > 0f ? movement.MoveSpeed : 1f;
            var duration = distance / speed;
            _playerExitSeq = DOTween.Sequence();
            _playerExitSeq.Append(playerT.DOMove(transform.position, duration));
            _playerExitSeq.AppendCallback(HideArrow);
            _playerExitSeq.AppendInterval(delayBeforeClimbingLadder);
            _playerExitSeq.AppendCallback(_source.Play);
            _playerExitSeq.Append(playerT.DOMove(transform.TransformPoint(animExitPosition), 0.5f));
            _playerExitSeq.Join(playerSprite.DOFade(0f, 0.5f));
            _playerExitSeq.AppendInterval(delayBeforeBlackScreen);
            _playerExitSeq.AppendCallback(UIManager.Instance.ShowBlackScreen);
            _playerExitSeq.AppendInterval(UIManager.Instance.ScreenFadeDuration);
            _playerExitSeq.OnComplete(() =>
            {
                // Go to next scene
                SceneManager.LoadScene(nextSceneName);
            });
            _playerExitSeq.Play();
        }

        private void OnDestroy()
        {
            _arrowTween?.Kill();
            EventBus.keyCollected -= UnlockExit;
        }

        private bool CanExit(GameObject enteringObject)
        {
            if (!enteringObject.CompareTag("Player")) return false;
            return !_isLocked;
        }

        /// <summary>
        /// Unlock the exit for the player
        /// </summary>
        private void UnlockExit()
        {
            _isLocked = false;
            _col.isTrigger = true;
            ladderSprite.sprite = unlockedSprite;
            padlockSprite.SetActive(false);
            arrow.gameObject.SetActive(true);
            _arrowTween = arrow.DOLocalMoveY(_arrowStartY + 0.2f, 0.5f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void HideArrow()
        {
            _arrowTween?.Kill();
            arrow.gameObject.SetActive(false);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.TransformPoint(animExitPosition), 0.1f);
        }
    }
}