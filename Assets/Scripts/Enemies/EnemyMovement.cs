using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;


namespace JPO
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private EnemyWaypoint[] enemyPath;

        int currentWaypoint = 0;

        bool isMovingForward = true;
        [SerializeField] private float wpDistanceThreshold = 0.1f;

        private EnemyWaypoint target;
        [SerializeField]private int speed = 1;
        private EnemyWaypoint wp;

        

        void Start()
        {
            if (enemyPath.Length <= 0)
                print("path is empty");

            // on initialise le premier waypoint ainsi que la cible
            currentWaypoint = 0;
            target = enemyPath[currentWaypoint];

            if (target == null)
            {
                print("aucune target n'est assignée");
            }

        }
            
        void Update()
        {
            //La fonction Update appelle sans arrêt la fonction TrackPath, cela permet de ne pas surcharger la fonction Update.
            TrackPath();  
        }


        // La fonction TrackPath permet de gérer les déplacements de l'ennemi selon un chemin prédéfini avec des Waypoints.
        void TrackPath()
        {
            if (target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, target.transform.position) <= wpDistanceThreshold)
                {
                    // On passe au waypoint suivant
                    currentWaypoint += 1;
                }
                if (currentWaypoint >= enemyPath.Length)
                {
                    // Quand on atteint le waypoint max, on revient au début 
                    currentWaypoint = 0;
                }

                target = enemyPath[currentWaypoint];
            }
        }  

    }

}