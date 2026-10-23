using UnityEngine;

namespace JPO
{
    public class PlayerWeapons : MonoBehaviour
    {
        public enum WeaponType
        {
            Uzi,
            Ak47,
            Minigun
        }

        [System.Serializable]
        public struct Weapon
        {
            public WeaponType type;
            public Sprite sprite;
            public Vector2 localFirePoint;
            // Stats
            public float fireRate;
        }

        [SerializeField] private SpriteRenderer weaponSprite;
        [SerializeField] private Transform firePoint;
        [SerializeField] private Weapon currentWeapon;

        public void UpgradeWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            // Update weapon sprite
            weaponSprite.sprite = currentWeapon.sprite;
            firePoint.localPosition = weapon.localFirePoint;
            
            //TODO: Update stats, local fire point etc...
        }
    }
}
