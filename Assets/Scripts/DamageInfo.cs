using UnityEngine;

namespace JPO
{
    [System.Serializable]
    public struct DamageInfo
    {
        public int damageAmount;
        public GameObject damageOwner;
        public Vector2 hitVelocity;
    }
}