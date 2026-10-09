using UnityEngine;

namespace JPO
{
    public class FPSLimiter : MonoBehaviour
    {
        [SerializeField] private int targetFrameRate = 60;

        private void Start()
        {
            Application.targetFrameRate = targetFrameRate;
            QualitySettings.vSyncCount = 0;
        }
    }
}