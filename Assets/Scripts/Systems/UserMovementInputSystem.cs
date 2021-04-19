using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class UserMovementInputSystem : IEcsRunSystem
    {
        private EcsWorld _world = null;
        
        public void Run()
        {
            var direction = Vector3.zero;
            
            if (Input.GetKey(KeyCode.W)) direction += Vector3.up;
            if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
            if (Input.GetKey(KeyCode.S)) direction += Vector3.down;
            if (Input.GetKey(KeyCode.D)) direction += Vector3.right;

            var inputEvent = _world.NewEntity();
            inputEvent.Get<UserMovementInputEvent>().MoveDirection = direction;
        }
    }
}
