using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JPO
{
    /// <summary>
    /// Base class for objects performing damages to other object, using onTriggerEnter2D method
    /// </summary>
    public abstract class DamageArea : MonoBehaviour
    {
        [Header("Damage Area Settings")]
        [SerializeField] protected DamageInfo damageInfo;
        [SerializeField] protected List<GameObject> objectsToIgnore = new();
        [SerializeField] protected List<string> tagsToIgnore = new();

        [Header("Events"), Space]
        public UnityEvent<IDamageable> onDamageApplied;


        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            if (!CanDamageObject(col.gameObject)) return;
            // Try to get the IDamageable interface
            if (!col.gameObject.TryGetComponent(out IDamageable damageable)) return;
            // Apply damages
            if (damageable.ApplyDamage(damageInfo))
            {
                HandleDamageApplied(damageable);
                onDamageApplied?.Invoke(damageable);
            }
        }

        /// <summary>
        /// Add an object to ignore
        /// </summary>
        protected void AddObjectToIgnore(GameObject obj)
        {
            if (objectsToIgnore.Contains(obj)) return;
            objectsToIgnore.Add(obj);
        }

        /// <summary>
        /// Check if we can damage the object
        /// </summary>
        protected bool CanDamageObject(GameObject obj)
        {
            // Check if the object exists before performing any other check
            if (!obj) return false;
            // Check if we are not ignoring this object
            if (objectsToIgnore.Contains(obj)) return false;
            //// Check if we are not ignoring the tag
            //if (tagsToIgnore.Contains(obj.tag)) return false;
            // Else we can damage the object
            return true;
        }

        /// <summary>
        /// Perform some action after having applied damage to the other object, override this in child classes
        /// </summary>
        protected virtual void HandleDamageApplied(IDamageable damagedObject) { }
    }
}