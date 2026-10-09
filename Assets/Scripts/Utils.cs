using System;
using UnityEngine;

namespace JPO
{
    public static class Utils
    {
        /// <summary>
        /// Get the bounds of the camera in world space
        /// </summary>
        public static Bounds GetCameraBounds(Camera cam)
        {
            if (!cam) throw new NullReferenceException("The camera is null");
            var size = cam.orthographicSize;
            var ratio = cam.aspect;
            return new Bounds(cam.transform.position, new Vector2(size * ratio * 2, size * 2));
        }

        /// <summary>
        /// Calculate the intersection point between the moving direction of an object and the edges of the camera
        /// </summary>
        public static Vector2 GetCameraBoundsIntersection(Vector2 position, Vector2 direction, Bounds cameraBounds)
        {
            if (direction == Vector2.zero)
                return position;

            direction.Normalize();

            var tx = float.MaxValue;
            var ty = float.MaxValue;

            // Impact to the right
            if (direction.x > 0)
                tx = (cameraBounds.max.x - position.x) / direction.x;
            // Impact to the left
            else if (direction.x < 0)
                tx = (cameraBounds.min.x - position.x) / direction.x;

            // Impact up
            if (direction.y > 0)
                ty = (cameraBounds.max.y - position.y) / direction.y;
            // Impact down
            else if (direction.y < 0)
                ty = (cameraBounds.min.y - position.y) / direction.y;

            // The first edge crossed is the one with the lowest distance t
            var tMin = Mathf.Min(tx, ty);

            return position + direction * tMin;
        }
    }
}