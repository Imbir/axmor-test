using System;
using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class PlayerShootSystem : IEcsRunSystem
    {
        private WeaponParameters _settings;
        
        private EcsWorld _world = null;
        private EcsFilter<PlayerComponent> _players = null;
        private EcsFilter<UserShootInputEvent> _inputEvents = null;

        private float _sinceLastShot;

        public void Run()
        {
            if (!_players.Get1(0).IsAlive) return;
            
            for (var i = 0; i < _inputEvents.GetEntitiesCount(); i++)
            {
                var isShooting = _inputEvents.Get1(i).IsShooting;

                for (var j = 0; j < _players.GetEntitiesCount(); j++)
                {
                    ref var player = ref _players.Get1(j);
                    
                    if (isShooting) Shoot(player);
                }
                
                _inputEvents.GetEntity(i).Destroy();
            }
        }

        private void Shoot(PlayerComponent player)
        {
            _sinceLastShot += Time.deltaTime;

            if (!(_sinceLastShot >= _settings.ShotPauseValues[player.SelectedWeapon])) return;
            
            _world.NewEntity().Get<ShootBulletEvent>().SelectedWeapon = player.SelectedWeapon;
            _sinceLastShot = 0f;
        }
    }
}
