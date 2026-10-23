using DG.Tweening;
using UnityEngine;

namespace JPO
{
    /// <summary>
    /// Base class for all collectable items
    /// </summary>
    public abstract class Item : MonoBehaviour
    {
        [Header("Item Settings")]
        [SerializeField] protected Transform visualT;
        [SerializeField] protected float visualAnimAmplitude = 0.15f;
        [SerializeField] protected float visualAnimDuration = 0.5f;
        [SerializeField] protected float delayBeforeDestroy = 0.5f;
        protected bool _isCollected;
        protected Tween _visualTween;
        protected AudioSource _source;

        protected virtual void Start()
        {
            _source = GetComponent<AudioSource>();
            if (visualT)
            {
                _visualTween = visualT.DOLocalMoveY(visualAnimAmplitude, visualAnimDuration)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (!CanBeCollected(collision)) return;
            _isCollected = true;
            OnCollected(collision);
            Destroy(gameObject, delayBeforeDestroy);
        }

        protected virtual void OnDestroy()
        {
            // Cleanup the tween when the item is destroyed if it is still running
            _visualTween?.Kill();
        }

        /// <summary>
        /// Check if this item can be collected by the colliding object, override this method in child classes
        /// </summary>
        protected virtual bool CanBeCollected(Collider2D collision)
        {
            if (_isCollected) return false;
            if (!collision.CompareTag("Player")) return false;
            return true;
        }

        /// <summary>
        /// Perform action when this item is collected.<br/>
        /// Will hide the visual and play a sound when collected by default
        /// </summary>
        protected virtual void OnCollected(Collider2D collision)
        {
            // Stop anim and hide visual
            _visualTween?.Kill();
            visualT.gameObject.SetActive(false);
            // Play the collect sound
            _source.Play();
        }
    }
}