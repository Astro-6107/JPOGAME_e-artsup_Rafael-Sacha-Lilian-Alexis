using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace JPO
{
    public class InteriorDoor : MonoBehaviour
    {
        [SerializeField] private Transform playerSnapPoint;
        [SerializeField] private Transform exitTeleportPoint;
        [SerializeField] private float transitionDuration;
        [SerializeField] private float delayBeforeTeleport;
        [SerializeField] private float delayAfterTeleport;

        [Header("Camera")]
        [SerializeField] private CinemachineCamera cam;
        [SerializeField] private CinemachineConfiner2D cameraConfiner;
        [SerializeField] private Collider2D destinationConfinerCollider;
        [SerializeField] private float destinationOrthoSize;

        private Sequence _teleportSeq;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            _teleportSeq = DOTween.Sequence();
            // Disable player inputs and collider
            var controller = collision.GetComponent<PlayerController>();
            controller.DisableInput();
            collision.enabled = false;

            var movement = collision.GetComponent<PlayerMovement>();
            var speed = movement.MoveSpeed > 0f ? movement.MoveSpeed : 1f;
            var distance = Vector3.Distance(collision.transform.position, playerSnapPoint.position);
            var snapDuration = distance / speed;
            // Snap player to door position
            _teleportSeq.Append(collision.transform.DOMove(playerSnapPoint.position, snapDuration));
            _teleportSeq.AppendInterval(delayBeforeTeleport);
            // Play circle UI transition to black
            _teleportSeq.Append(CircleTransitionManager.Instance.TransitionToBlack(playerSnapPoint.position, transitionDuration));
            _teleportSeq.AppendCallback(() =>
            {
                collision.transform.position = exitTeleportPoint.position;
                cam.Lens.OrthographicSize = destinationOrthoSize;
                cameraConfiner.BoundingShape2D = destinationConfinerCollider;
            });
            _teleportSeq.AppendInterval(delayAfterTeleport);
            // Play circle UI transition to game view
            _teleportSeq.Append(CircleTransitionManager.Instance.TransitionToGame(exitTeleportPoint.position, transitionDuration));
            _teleportSeq.AppendCallback(() =>
            {
                controller.EnableInput();
                collision.enabled = true;
            });
            _teleportSeq.Play();
        }

        private void OnDestroy()
        {
            _teleportSeq?.Kill();
        }
    }
}