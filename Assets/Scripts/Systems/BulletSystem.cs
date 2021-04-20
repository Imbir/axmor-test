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
            if (!_players.Get1(0).IsAlive) return;
            
            HandleShotEvents();
            HandleBulletMovement();
        }

        private void HandleShotEvents()
        {
            for (var i = 0; i < _shootEvents.GetEntitiesCount(); i++)
            {
                var shotEvent = _shootEvents.Get1(i);
                ref var newBullet = ref _world.NewEntity().Get<BulletComponent>();
                newBullet.Transform = _bulletPool.GetObject().transform;
                newBullet.Transform.position = _settings.ProjectileSource.position;
                newBullet.Transform.gameObject.GetComponent<Bullet>().SetSprite(shotEvent.SelectedWeapon);
                newBullet.Damage = _settings.DamageValues[shotEvent.SelectedWeapon];
                newBullet.MovementSpeed = _settings.ProjectileSpeeds[shotEvent.SelectedWeapon];
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
                {
                    DestroyBullet(i);
                    continue;
                }
                    

                for (var j = 0; j < _torpedoes.GetEntitiesCount(); j++)
                {
                    ref var torpedo = ref _torpedoes.Get1(j);
                    
                    if (Vector3.Distance(torpedo.Transform.position, bullet.Transform.position) > 0.5f) continue;
                    
                    torpedo.HitPoints -= bullet.Damage;
                    torpedo.IsAlive = torpedo.HitPoints > 0;
                    DestroyBullet(i);
                    break;
                }
            }
        }

        private void DestroyBullet(int index)
        {
            _bulletPool.Recycle(_bullets.Get1(index).Transform.gameObject.GetComponent<Bullet>());
            _bullets.GetEntity(index).Destroy();
        }
    }
}
