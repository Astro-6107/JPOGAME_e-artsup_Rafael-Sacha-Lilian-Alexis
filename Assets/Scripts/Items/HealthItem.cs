using UnityEngine;

namespace JPO
{
    public class HealthItem : Item
    {
        [Header("Health Item Settings")]
        [SerializeField] private int healthAmount = 1;
        private HealthSystem _collidingHealth;

        protected override bool CanBeCollected(Collider2D collision)
        {
            // Check if the item can be collected with the conditions from the base class
            if (!base.CanBeCollected(collision)) return false;
            // Check if the player colliding has the HealthSystem component on it and store it in the _collidingHealth variable
            return collision.TryGetComponent(out _collidingHealth);
        }

        protected override void OnCollected(Collider2D collision)
        {
            // Perform the actions of the base class on this item (hide visual, play sound)
            base.OnCollected(collision);
            // Check if we successfuly get the health component during the last check
            if (!_collidingHealth) return;
            // Add health to colliding player
            _collidingHealth.AddHealth(healthAmount);
        }
    }
}