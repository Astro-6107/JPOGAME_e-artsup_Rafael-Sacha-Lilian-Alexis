using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UIElements;


namespace JPO
{
    public class EnemyMovement : MonoBehaviour
    {
        [Header("Déplacements")]
        [SerializeField] private float wpDistanceThreshold = 0.1f;
        [SerializeField] private int speed = 1;


        [Header("Chemin")]
        [SerializeField] private EnemyWaypoint[] enemyPath;
        int currentWaypoint = 0;
        private EnemyWaypoint wp;
        private EnemyWaypoint target;

        [Header("Events")]
        [Space]
        public UnityEvent onStartMoving;
        public UnityEvent onStopMoving;
        public bool IsMoving { get; private set; }

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

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
            
        void FixedUpdate()
        {
            //La fonction Update appelle sans arrêt la fonction TrackPath, cela permet de ne pas surcharger la fonction Update.
            TrackPath();  
        }


        // La fonction TrackPath permet de gérer les déplacements de l'ennemi selon un chemin prédéfini avec des Waypoints.
        void TrackPath()
        {
            bool wasMoving = IsMoving;
            IsMoving = target != null;


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

            if (!wasMoving && IsMoving)
            {
                onStartMoving?.Invoke();
                print("enemi en mouvement");
            }
                

            else if (wasMoving && !IsMoving)
            {
                onStopMoving?.Invoke();
                print("enemi stoppé");
            }
                
        }  
    }
}