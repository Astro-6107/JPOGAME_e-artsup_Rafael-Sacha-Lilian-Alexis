using UnityEngine;

namespace JPO
{
    public class EnemyWaypoint : MonoBehaviour
    {
        public Vector3 wpPosition;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            wpPosition = transform.position;
        }
    }
}
