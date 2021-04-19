using Components;
using Components.Enums;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class UserWeaponSwitchInputSystem : IEcsRunSystem
    {
        private EcsWorld _world = null;
        
        public void Run()
        {
            var wheelAxisValue = Input.GetAxisRaw("Mouse ScrollWheel");

            if (wheelAxisValue != 0f)
            {
                _world.NewEntity().Get<UserWeaponSwitchInputEvent>().WeaponSwitchDirection = wheelAxisValue > 0f
                    ? WeaponSwitchDirection.Previous
                    : WeaponSwitchDirection.Next;
            }
        }
    }
}
