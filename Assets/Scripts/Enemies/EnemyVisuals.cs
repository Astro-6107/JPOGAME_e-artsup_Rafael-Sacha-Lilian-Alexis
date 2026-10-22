using UnityEngine;

namespace JPO
{
    public class EnemyVisuals : MonoBehaviour
    {

        [SerializeField] private SpriteRenderer enemySprite;

        [Header("Movement Animation")]
        [SerializeField] private float movementRotationAmplitude = 10f;
        [SerializeField] private float movementScaleAmplitude;
        [SerializeField] private float movementFrequency = 20f;

        private EnemyMovement _movement;
        private float _movementTimer;


        private void Awake()
        {
            _movement = GetComponent<EnemyMovement>();
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created


        // Update is called once per frame
        void Update()
        {
            AnimateMovement();
        }

        void AnimateMovement()
        {
            if (_movement.IsMoving)
            {
                _movementTimer += Time.deltaTime * movementFrequency;

                // Animate rotation
                var angle = Mathf.Sin(_movementTimer) * movementRotationAmplitude;
                enemySprite.transform.localRotation = Quaternion.Euler(0f, 0f, angle);

                // Animate scale
                var verticalModifier = Mathf.Sin(_movementTimer) * movementScaleAmplitude;
                var newScale = 1f + verticalModifier;
                enemySprite.transform.localScale = new Vector3(1f, newScale, 1f);

                // Handle move particles
                /*if (!moveParticles.isPlaying) moveParticles.Play();
                var shape = moveParticles.shape;
                var arc = shape.arc;
                shape.rotation = new Vector3(0f, 0f, _movement.FacingDirection == FacingDirection.Right
                    ? 180f - arc * 0.5f
                    : -arc * 0.5f
                );*/
            }
            else
            {
                _movementTimer = 0f;
                enemySprite.transform.localRotation = Quaternion.identity;
                enemySprite.transform.localScale = Vector3.one;
                //if (moveParticles.isPlaying) moveParticles.Stop();
            }


        }
    }
}
