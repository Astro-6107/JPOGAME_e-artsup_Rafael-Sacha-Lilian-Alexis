using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace JPO
{
    /// <summary>
    /// Manager handling object pooling, must be present in all gameplay scenes
    /// </summary>
    public class ObjectPooler : MonoBehaviour
    {
        public static ObjectPooler Instance { get; private set; }
        // Link a prefab to its pool
        private readonly Dictionary<GameObject, ObjectPool<GameObject>> _prefabPools = new();
        // Link an instance to its pool
        private readonly Dictionary<GameObject, ObjectPool<GameObject>> _instancesPools = new();

        private void Awake()
        {
            if (!Instance) Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        /// <summary>
        /// Instantiate or get a prefab instance
        /// </summary>
        public GameObject InstantiatePrefab(GameObject prefab, Vector3? position = null, Quaternion? rotation = null, Transform parent = null)
        {
            // If the pool of this prefab doesn't exist, create it
            if (!_prefabPools.TryGetValue(prefab, out var pool))
            {
                pool = new ObjectPool<GameObject>(
                    createFunc: () =>
                    {
                        var obj = Instantiate(prefab, transform);
                        _instancesPools.Add(obj, pool);
                        return obj;
                    },
                    actionOnGet: (obj) => obj.SetActive(true),
                    actionOnRelease: (obj) =>
                    {
                        obj.SetActive(false);
                        obj.transform.SetParent(transform);
                        obj.transform.localPosition = Vector3.zero;
                    },
                    actionOnDestroy: (obj) =>
                    {
#if UNITY_EDITOR
                        // Avoid bug in the editor when leaving play mode
                        if (!Application.isPlaying) return;
#endif
                        _instancesPools.Remove(obj);
                        Destroy(obj);
                    },
                    collectionCheck: true, // Avoid to release an already released instance
                    defaultCapacity: 10,
                    maxSize: 500
                );
                _prefabPools.Add(prefab, pool);
            }

            // Get the game object, update its position and return it
            var go = pool.Get();
            var goT = go.transform;
            if (position.HasValue) goT.position = position.Value;
            if (rotation.HasValue) goT.rotation = rotation.Value;
            goT.SetParent(parent, true);
            return go;
        }

        /// <summary>
        /// Instantiate or get a prefab instance, and get its component of type T
        /// </summary>
        public T InstantiatePrefab<T>(T prefab, Vector3? position = null, Quaternion? rotation = null, Transform parent = null) where T : Component
        {
            return InstantiatePrefab(prefab.gameObject, position, rotation, parent).GetComponent<T>();
        }

        /// <summary>
        /// Called at the end of the normal life cycle of a pooled object (instead of manually destroying it or disabling it, call this)
        /// </summary>
        public void ReleaseObject(GameObject instance)
        {
            if (_instancesPools.TryGetValue(instance, out var pool))
            {
                pool.Release(instance);
            }
            else
            {
                // If the object has not been pooled
                Destroy(instance);
            }
        }
    }
}