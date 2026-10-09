using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JPO
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private Image heartUiPrefab;
        [SerializeField] private RectTransform heartsContainer;
        [SerializeField, Range(0f, 1f)] private float disabledHeartAlpha = 0.25f;
        private HealthSystem _playerHealth;
        private readonly List<Image> _hearts = new();

        private void Start()
        {
            // Try to find the player object with its tag
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (!playerObj)
            {
                Debug.LogError("The player object has not been found");
                return;
            }
            if (!playerObj.TryGetComponent(out _playerHealth))
            {
                Debug.LogError("The player object has not the Health System script on it");
                return;
            }

            SpawnHearts(_playerHealth.StartHealth);
            _playerHealth.onHealthChanged.AddListener(UpdateHearts);
        }

        private void OnDestroy()
        {
            if (!_playerHealth) return;
            _playerHealth.onHealthChanged.RemoveListener(UpdateHearts);
        }

        /// <summary>
        /// Spawn the hearts images in the UI
        /// </summary>
        private void SpawnHearts(int heartCount)
        {
            for (var i = 0; i < heartCount; i++)
            {
                _hearts.Add(Instantiate(heartUiPrefab, heartsContainer));
            }
        }

        /// <summary>
        /// Update the health UI
        /// </summary>
        private void UpdateHearts(int heatlh)
        {
            for (var i = 0; i < _hearts.Count; i++)
            {
                // Change the heart alpha based on if it's enabled or not
                var heart = _hearts[i];
                heart.SetAlpha(heatlh >= i + 1 ? 1f : disabledHeartAlpha);
            }
        }
    }
}