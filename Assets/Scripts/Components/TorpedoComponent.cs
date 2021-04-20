using UnityEngine;

namespace Components
{
    public struct TorpedoComponent
    {
        public Transform Transform;
        public float MovementSpeed;
        public int MaxHitPoints;
        public int HitPoints;
        public bool IsAlive;
    }
}
