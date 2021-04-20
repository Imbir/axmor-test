using System;
using Components;
using Core.DataClasses;
using Leopotam.Ecs;
using UnityEditor;
using UnityEngine;

namespace Systems
{
    public class PlayerMovementSystem : IEcsInitSystem, IEcsRunSystem
    {
        private PlayerParameters _settings;

        private EcsWorld _world = null;
        private EcsFilter<PlayerComponent> _players = null;
        private EcsFilter<UserMovementInputEvent> _inputEvents = null;
        private Camera _camera = null;

        private float _minX = 0;
        private float _maxX = 0;
        private float _minY = 0;
        private float _maxY = 0;

        public void Init()
        {
            var verticalBounds = _camera.orthographicSize;
            var horizontalBounds = verticalBounds * Screen.width / Screen.height;
            var cameraPosition = _camera.transform.position;

            _minX = cameraPosition.x - horizontalBounds;
            _maxX = cameraPosition.x + horizontalBounds;
            _minY = cameraPosition.y - verticalBounds;
            _maxY = cameraPosition.y + verticalBounds;
        }

        public void Run()
        {
            if (!_players.Get1(0).IsAlive) return;
            
            for (var i = 0; i < _inputEvents.GetEntitiesCount(); i++)
            {
                var direction = _inputEvents.Get1(i).MoveDirection;

                for (var j = 0; j < _players.GetEntitiesCount(); j++)
                {
                    var player = _players.Get1(j);
                    
                    var nextPos = player.Transform.position + direction * (Time.deltaTime * player.MovementSpeed);
                    nextPos.x = Mathf.Clamp(nextPos.x, _minX, _maxX);
                    nextPos.y = Mathf.Clamp(nextPos.y, _minY, _maxY);
                    
                    player.Transform.position = nextPos;
                }
                
                _inputEvents.GetEntity(i).Destroy();
            }
        }
    }
}
