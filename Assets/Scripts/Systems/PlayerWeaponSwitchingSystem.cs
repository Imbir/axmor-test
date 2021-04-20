using Components;
using Components.Enums;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class PlayerWeaponSwitchingSystem : IEcsInitSystem, IEcsRunSystem
    {
        private WeaponParameters _shootSystemParameters;
        
        private EcsWorld _world = null;
        private EcsFilter<PlayerComponent> _players = null;
        private EcsFilter<UserWeaponSwitchInputEvent> _inputEvents = null;

        [EcsIgnoreInject] private int _availableWeapons;

        public void Init()
        {
            _availableWeapons = Mathf.Min(_shootSystemParameters.DamageValues.Length,
                _shootSystemParameters.ShotPauseValues.Length);
        }
        
        public void Run()
        {
            if (!_players.Get1(0).IsAlive) return;
            
            for (var i = 0; i < _inputEvents.GetEntitiesCount(); i++)
            {
                var switchDirection = _inputEvents.Get1(i).WeaponSwitchDirection;

                for (var j = 0; j < _players.GetEntitiesCount(); j++)
                {
                    ref var player = ref _players.Get1(j);
                    switch (switchDirection)
                    {
                        case WeaponSwitchDirection.Next:
                            player.SelectedWeapon += 1;
                            if (player.SelectedWeapon >= _availableWeapons) player.SelectedWeapon = 0;
                            break;
                        case WeaponSwitchDirection.Previous:
                            player.SelectedWeapon -= 1;
                            if (player.SelectedWeapon < 0) player.SelectedWeapon = _availableWeapons - 1;
                            break;
                    }
                }
                
                _inputEvents.GetEntity(i).Destroy();
            }
        }
    }
}
