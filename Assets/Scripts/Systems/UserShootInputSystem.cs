using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class UserShootInputSystem : IEcsRunSystem
    {
        private EcsWorld _world = null;
        
        public void Run()
        {
            _world.NewEntity().Get<UserShootInputEvent>().IsShooting = Input.GetMouseButton(0);
        }
    }
}
