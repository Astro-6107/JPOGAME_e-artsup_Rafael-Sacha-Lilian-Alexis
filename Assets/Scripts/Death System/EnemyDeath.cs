using DG.Tweening;
using UnityEngine;

namespace JPO
{
    public class EnemyDeath : DeathSystem
    {
        [SerializeField] private float leaveScreenDuration = 0.5f;
        [SerializeField] private Ease leaveScreenEasing = Ease.InQuad;
        private Collider2D _col;
        private Camera _cam;

        private void Awake()
        {
            _col = GetComponent<Collider2D>();
            _cam = Camera.main;
        }

        public override void Die(GameObject killer)
        {
            if (!killer)
            {
                Debug.LogError("The killer object was not set or is null", this);
                return;
            }
            if (!_cam)
            {
                Debug.LogError("The main camera was not found", this);
                return;
            }
            _col.enabled = false;
            var position = transform.position;
            var exitDirection = (Vector2)(position - killer.transform.position).normalized;
            var camBounds = Utils.GetCameraBounds(_cam);
            var intersection = Utils.GetCameraBoundsIntersection(position, exitDirection, camBounds);
            var exitPosition = intersection + exitDirection * 2f;
            transform.DOMove(exitPosition, leaveScreenDuration)
                .SetEase(leaveScreenEasing)
                .OnComplete(() => Destroy(gameObject));
        }
    }
}