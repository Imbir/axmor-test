using System;
using UnityEngine;

namespace Core.DataClasses
{
    [Serializable]
    public class ThreatParameters
    {
        [Tooltip("Pause in seconds between torpedo generation")]
        public float BaseSpawnFrequency;
        public float BaseMovementSpeed;
        [Tooltip("Values are multiplies each N seconds")]
        public float IncreaseFrequency;
        [Tooltip("Should be between 0 and 1")]
        public float SpawnFrequencyMultiplier;
        [Tooltip("Should be more than 1")]
        public float MovementSpeedMultiplier;

    }
}