using System.IO;
using UnityEngine;


namespace JPO
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private EnemyWaypoint[] enemyPath;
        [SerializeField] private float wpDistanceThreshold = 0.1f;

        [SerializeField] private SpriteRenderer enemySprite;

        private Transform target;
        [SerializeField]private float speed = 1.0f;
        private EnemyWaypoint wp;

        void Start()
        {
            if (enemyPath.Length > 0)
                print(enemyPath.Length);
            else
                print("path is empty");

            target = enemyPath[0].transform;


            if (target == null)
            {
                print("aucune target n'est assignée");

            }

            else
            {
                print(Vector3.Distance(target.position, target.position));
            }
        }
            

        void Update()
        {
            TrackPath();  
        }

        void TrackPath()
        {

            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            }
            /*wpPosition = wp.transform.position;
            enemySprite.flipX = wpPosition.x < transform.position.x;
            transform.position = directionToWp * wp.transform.position;*/

        }  

    }

}