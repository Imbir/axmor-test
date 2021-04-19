using Components;
using Core.MonoBehaviours;
using Core.ObjectPool;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class BulletSystem : IEcsInitSystem, IEcsRunSystem
    {
        private WeaponParameters _settings;
        
        private EcsWorld _world = null;
        private EcsFilter<PlayerComponent> _players = null;
        private EcsFilter<TorpedoComponent> _torpedoes = null;
        private EcsFilter<BulletComponent> _bullets = null;
        private EcsFilter<ShootBulletEvent> _shootEvents = null;
        
        private Camera _camera = null;

        private float _minX;
        private float _maxX;

        [EcsIgnoreInject] private ObjectPool<Bullet> _bulletPool;

        public void Init()
        {
            _bulletPool = new ObjectPool<Bullet>(_settings.ProjectilePrefab);
            
            var verticalBounds = _camera.orthographicSize;
            var horizontalBounds = verticalBounds * Screen.width / Screen.height;
            var cameraPosition = _camera.transform.position;
            _minX = cameraPosition.x - horizontalBounds;
            _maxX = cameraPosition.x + horizontalBounds;
        }
        
        public void Run()
        {
            HandleShotEvents();
            HandleBulletMovement();
        }

        private void HandleShotEvents()
        {
            for (var i = 0; i < _shootEvents.GetEntitiesCount(); i++)
            {
                var shotEvent = _shootEvents.Get1(i);
                var newBullet = _world.NewEntity();
                newBullet.Get<BulletComponent>().Transform = _bulletPool.GetObject().transform;
                newBullet.Get<BulletComponent>().Transform.position = _settings.ProjectileSource.position;
                newBullet.Get<BulletComponent>().Transform.gameObject.GetComponent<Bullet>().SetSprite(shotEvent.SelectedWeapon);
                newBullet.Get<BulletComponent>().Damage = _settings.DamageValues[shotEvent.SelectedWeapon];
                newBullet.Get<BulletComponent>().MovementSpeed = _settings.ProjectileSpeeds[shotEvent.SelectedWeapon];
                _shootEvents.GetEntity(i).Destroy();
            }
        }

        private void HandleBulletMovement()
        {
            for (var i = 0; i < _bullets.GetEntitiesCount(); i++)
            {
                var bullet = _bullets.Get1(i);
                bullet.Transform.position += Vector3.right * (Time.deltaTime * bullet.MovementSpeed);

                if (bullet.Transform.position.x > _maxX || bullet.Transform.position.x < _minX)
                    DestroyBullet(i);
                
                // check torpedo collision, damage
            }
        }

        private void DestroyBullet(int i)
        {
            _bulletPool.Recycle(_bullets.Get1(i).Transform.gameObject.GetComponent<Bullet>());
            _bullets.GetEntity(i).Destroy();
        }
    }
}
