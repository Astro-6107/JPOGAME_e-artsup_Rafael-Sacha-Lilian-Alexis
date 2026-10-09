using UnityEngine;

namespace JPO
{
    /// <summary>
    /// Base class of the death system
    /// </summary>
    public abstract class DeathSystem : MonoBehaviour
    {
        /// <summary>
        /// Implement this method to make the object die
        /// </summary>
        public abstract void Die(GameObject killer);
    }
}