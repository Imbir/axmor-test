using Systems;
using Core.DataClasses;
using Leopotam.Ecs;
using UnityEngine;

namespace Core
{
    public class GameCore : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems _systems;

        [SerializeField] private Camera _mainCamera;
        [SerializeField] private PlayerParameters _playerParameters;
        [SerializeField] private WeaponParameters _shootSystemParameters;

        private void Start()
        {
            _world = new EcsWorld();

            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
            
            _systems = new EcsSystems(_world)
                .Add(new UserMovementInputSystem())
                .Add(new PlayerMovementSystem())
                .Add(new UserWeaponSwitchInputSystem())
                .Add(new PlayerWeaponSwitchingSystem())
                .Add(new UserShootInputSystem())
                .Add(new PlayerShootSystem())
                .Add(new BulletSystem())
                .Inject(_mainCamera)
                .Inject(_playerParameters)
                .Inject(_shootSystemParameters);
            
            _systems.Init();

            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
        }
        
        private void Update()
        {
            _systems.Run();
        }

        private void OnDisable()
        {
            _systems.Destroy();
            _world.Destroy();
        }
    }
}
