using UnityEngine;

namespace JPO
{
    public class WeaponsUpgrade : Item
    {

        [Header("Weapon Item Settings")]
        [SerializeField] private int weaponIndex = 0;

        protected PlayerWeapons collindingUpgrade;
        
        protected override bool CanBeCollected(Collider2D collision)
        {
            // Check if the item can be collected with the conditions from the base class
            if (!base.CanBeCollected(collision)) return false;
            // Check if the player colliding has the HealthSystem component on it and store it in the _collidingHealth variable
            return collision.TryGetComponent(out collindingUpgrade);
        }

        protected override void OnCollected(Collider2D collision)
        {
            base.OnCollected(collision);
            if (!collindingUpgrade) return;
            collindingUpgrade.UpgradeWeapon(weaponIndex);
        }
    }
}
