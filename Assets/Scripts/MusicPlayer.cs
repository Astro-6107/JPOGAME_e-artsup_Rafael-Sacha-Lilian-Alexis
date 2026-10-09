using UnityEngine;

namespace JPO
{
    public class MusicPlayer : MonoBehaviour
    {
        private static MusicPlayer Instance { get; set; }

        private void Awake()
        {
            if (!Instance) Instance = this;
            else if (Instance != this) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }
}