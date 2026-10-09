using UnityEngine;

namespace JPO
{
    /// <summary>
    /// Implement this interface on classes of objects that can take damages
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// Apply Damages to this object, return true if the damage has been applied, false otherwise
        /// </summary>
        /// <param name="damage">All the damage info</param>
        public bool ApplyDamage(DamageInfo damage);
    }
}