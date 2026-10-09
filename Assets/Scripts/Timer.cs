using UnityEngine;

namespace JPO
{
    public struct Timer
    {
        /// <summary>
        /// Create an empty timer, useful to reset an already existing timer
        /// </summary>
        public static Timer None => default;

        private float _startTime;
        private float _duration;

        /// <summary>
        /// Tells if this timer is currently running (WARNING: does not check for expiration)
        /// </summary>
        public bool IsRunning => _startTime >= 0f && _duration > 0f;

        /// <summary>
        /// Create a new timer that will run for <paramref name="duration"/> seconds
        /// </summary>
        public static Timer CreateFromSeconds(float duration)
        {
            return new Timer
            {
                _startTime = Time.time,
                _duration = duration
            };
        }

        /// <summary>
        /// Check if the timer is running and expired
        /// </summary>
        public bool ExpiredAndWasRunning()
        {
            if (!IsRunning) return false;
            return Time.time >= _startTime + _duration;
        }

        /// <summary>
        /// Get the remaining time of the timer
        /// </summary>
        public float GetRemainingTime()
        {
            return _startTime + _duration - Time.time;
        }

        /// <summary>
        /// Get the elapsed time since the start of the timer
        /// </summary>
        public float GetElapsedTime()
        {
            return Time.time - _startTime;
        }
    }
}