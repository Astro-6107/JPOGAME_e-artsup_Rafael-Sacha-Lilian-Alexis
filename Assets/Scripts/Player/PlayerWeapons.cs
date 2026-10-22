using UnityEngine;

namespace JPO
{
    public class PlayerWeapons : MonoBehaviour
    {
        public int weapon_index = 0;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void UpgradeWeapon(int index)
        {
            weapon_index = index;

            switch (weapon_index)
            {
                case 1:
                    // Débloquer arme 1
                    print("upgrade prise : Shotgun débloqué !");
                    break;
                case 2:
                    // Débloquer arme 2
                    print("upgrade prise : AK47 débloquée !");
                    break;
                case 3:
                    // Débloquer arme 3
                    print("upgrade prise : Minigun débloqué !");
                    break;
                default:
                    Debug.Log("Index d'arme inconnu : " + weapon_index);
                    break;
            }

        }
    }
}
