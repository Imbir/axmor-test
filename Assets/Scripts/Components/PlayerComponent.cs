using UnityEngine;

namespace Components
{
    public struct PlayerComponent
    {
        public float MovementSpeed;
        public Transform Transform;
        public int SelectedWeapon;
        public bool IsAlive;
        public float RoundStart;
    }
}
