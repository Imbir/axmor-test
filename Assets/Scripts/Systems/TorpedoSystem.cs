using Components;
using Core.DataClasses;
using Core.MonoBehaviours;
using Core.ObjectPool;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class TorpedoSystem : IEcsInitSystem, IEcsRunSystem
    {
        private ThreatParameters _settings = null;
        
        private EcsWorld _world = null;
        private EcsFilter<PlayerComponent> _players = null;
        private EcsFilter<TorpedoComponent> _torpedoes = null;
        private Camera _camera = null;
        private Torpedo _torpedoPrefab = null;

        [EcsIgnoreInject] private ObjectPool<Torpedo> _torpedoPool;

        private float _maxX = 0;
        private float _minY = 0;
        private float _maxY = 0;
        
        private float _sinceLastSpawned = 0f;

        public void Init()
        {
            _torpedoPool = new ObjectPool<Torpedo>(_torpedoPrefab);
            
            var verticalBounds = _camera.orthographicSize;
            var horizontalBounds = verticalBounds * Screen.width / Screen.height;
            var cameraPosition = _camera.transform.position;

            _maxX = cameraPosition.x + horizontalBounds;
            _minY = cameraPosition.y - verticalBounds;
            _maxY = cameraPosition.y + verticalBounds;
        }

        public void Run()
        {
            if (!_players.Get1(0).IsAlive) return;
            
            HandleSpawning();

            for (var i = 0; i < _torpedoes.GetEntitiesCount(); i++)
            {
                ref var torpedo = ref _torpedoes.Get1(i);
                HandleTorpedoMovement(ref torpedo);
                HandlePlayerCollisions(ref torpedo);

                if (!torpedo.IsAlive) DestroyTorpedo(i);
            }
        }

        private void HandleSpawning()
        {
            _sinceLastSpawned += Time.deltaTime;

            var fullIncreaseCycles =
                Mathf.CeilToInt((Time.time - _players.Get1(0).RoundStart) / _settings.IncreaseFrequency);

            var spawnFreq = _settings.BaseSpawnFrequency
                * Mathf.Pow(_settings.SpawnFrequencyMultiplier, fullIncreaseCycles);

            if (!(_sinceLastSpawned > spawnFreq)) return;
            ref var newTorpedo = ref _world.NewEntity().Get<TorpedoComponent>();
            newTorpedo.Transform = _torpedoPool.GetObject().transform;
            newTorpedo.Transform.position = new Vector3(_maxX + 3f, Random.Range(_minY, _maxY));
            newTorpedo.MovementSpeed = _settings.BaseMovementSpeed
                                       * Mathf.Pow(_settings.MovementSpeedMultiplier, fullIncreaseCycles);
            newTorpedo.MaxHitPoints = Random.Range(1, 3);
            newTorpedo.HitPoints = newTorpedo.MaxHitPoints;
            newTorpedo.IsAlive = true;
                
            _sinceLastSpawned = 0f;
        }

        private void HandleTorpedoMovement(ref TorpedoComponent torpedo)
        {
            if (!torpedo.IsAlive) return;
            
            var playerPosition = _players.Get1(0).Transform.position;
            if (torpedo.Transform.position.x > _maxX + 2f)
            {
                torpedo.Transform.position += Vector3.left * (Time.deltaTime * torpedo.MovementSpeed);
            }
            else
            {
                var angle = Mathf.Atan2(
                                Mathf.Abs(playerPosition.y - torpedo.Transform.position.y), 
                                Mathf.Abs(playerPosition.x - torpedo.Transform.position.x)
                                ) * Mathf.Rad2Deg;
                if (playerPosition.y >= torpedo.Transform.position.y) angle = -angle;
                
                torpedo.Transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                torpedo.Transform.position = Vector3.MoveTowards(
                    torpedo.Transform.position,
                    playerPosition,
                    Time.deltaTime * torpedo.MovementSpeed);
            }
            
        }

        private void HandlePlayerCollisions(ref TorpedoComponent torpedo)
        {
            if (!torpedo.IsAlive) return;
            
            for (var i = 0; i < _players.GetEntitiesCount(); i++)
            {
                ref var player = ref _players.Get1(i);
                var torpedoPosition = torpedo.Transform.position;
                
                if (Vector3.Distance(player.Transform.position, torpedoPosition) > 0.75f) continue;

                player.IsAlive = false;
                torpedo.IsAlive = false;
            }
        }

        private void DestroyTorpedo(int index)
        {
            if (_torpedoes.Get1(index).HitPoints <= 0) _world.NewEntity().Get<TorpedoDestroyedEvent>();
            _torpedoPool.Recycle(_torpedoes.Get1(index).Transform.gameObject.GetComponent<Torpedo>());
            _torpedoes.GetEntity(index).Destroy();
        }
    }
}
