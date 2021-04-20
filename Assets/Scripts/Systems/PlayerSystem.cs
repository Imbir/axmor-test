using Components;
using Core.DataClasses;
using Leopotam.Ecs;
using Leopotam.Ecs.UnityIntegration;
using UI;
using UnityEngine;

namespace Systems
{
    public class PlayerSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private PlayerParameters _settings;
        private WeaponParameters _weaponParameters;

        private EcsWorld _world = null;
        private EcsSystems _systems;
        private EcsFilter<PlayerComponent> _players = null;
        private EcsFilter<BulletComponent> _bullets = null;
        private EcsFilter<TorpedoComponent> _torpedoes = null;
        private EcsFilter<TorpedoDestroyedEvent> _torpedoesKilled = null;

        [EcsIgnoreInject] private ScoreView _scoreView;
        [EcsIgnoreInject] private BestScoreView _bestScoreView;
        [EcsIgnoreInject] private WeaponNameView _weaponNameView;
        [EcsIgnoreInject] private DefeatWindowView _defeatWindow;
        
        private int _score = 0;
        private int _bestScore = 0;
        
        public void Init()
        {
            foreach (var playerTaggedObject in GameObject.FindGameObjectsWithTag(_settings.PlayerTag))
            {
                var player = _world.NewEntity();
                
                player.Get<PlayerComponent>().Transform = playerTaggedObject.transform;
                player.Get<PlayerComponent>().MovementSpeed = _settings.PlayerMovementSpeed;
                player.Get<PlayerComponent>().IsAlive = true;
            }

            _scoreView = ViewHolder.GetView<ScoreView>();
            _bestScoreView = ViewHolder.GetView<BestScoreView>();
            _weaponNameView = ViewHolder.GetView<WeaponNameView>();
            _defeatWindow = ViewHolder.GetView<DefeatWindowView>();
            
            ViewHolder.GetView<RestartButtonView>().ClickEvent.AddListener(Restart);
            
            _bestScore = PlayerPrefs.GetInt("best", 0);
        }

        public void Run()
        {
            if (_defeatWindow.IsActive) return;
            
            if (!HasAlivePlayers())
            {
                _defeatWindow.Show(_score, _bestScore);
                if (_score > _bestScore) _bestScore = _score;
                _score = 0;
            }

            for (var i = 0; i < _torpedoesKilled.GetEntitiesCount(); i++)
            {
                _score++;
                _torpedoesKilled.GetEntity(i).Destroy();
            }

            _scoreView.Value = _score;
            _bestScoreView.Value = _bestScore;
            _weaponNameView.Value = _weaponParameters.Names[_players.Get1(0).SelectedWeapon];
        }

        public void Destroy()
        {
            PlayerPrefs.SetInt("best", _bestScore);
            PlayerPrefs.Save();
        }

        private bool HasAlivePlayers()
        {
            var iterator = _players.GetEnumerator();
            while (iterator.MoveNext())
                if (_players.Get1(iterator.Current).IsAlive) return true;

            return false;
        }

        private void Restart()
        {
            for (var i = 0; i < _bullets.GetEntitiesCount(); i++)
                _bullets.GetEntity(i).Destroy();
            
            for (var i = 0; i < _torpedoes.GetEntitiesCount(); i++)
                _torpedoes.GetEntity(i).Destroy();
            
            for (var i = 0; i < _players.GetEntitiesCount(); i++)
            {
                ref var player = ref _players.Get1(i);
                player.Transform.position = Vector3.zero;
                player.IsAlive = true;
                player.RoundStart = Time.time;
            }
            
            _defeatWindow.Hide();
        }
    }
}