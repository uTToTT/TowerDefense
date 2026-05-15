using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

// TODO: migrate to pure C# class
namespace TToTT.TowerDefense.Enemies
{
    public class Enemy : MonoBehaviour,
        IPoolable, IEntityLifecycle, IBuffable, ITickable
    {
        public event Action<Enemy> OnDeath;
        public event Action<float> OnReachedFinish;

        [SerializeField] private EnemyType _type;
        [Expandable]
        [SerializeField] private EnemyConfig _config;

        private float _currHP;

        private PathLane _lane;

        private BuffController _buffController;
        private PathController _pathController;
        private EnemyMovementController _movement;
        private ParticlesGenerator _particlesGenerator;
        private AudioService _audioService;

        private List<Vector3> _points;

        #region Characteristic API

        public float CurrHP => _currHP;
        public float MaxHp => BuffController.Calculate(Characteristics.HP, _config.HP);
        public float Speed => BuffController.Calculate(Characteristics.SPEED, _config.Speed);
        public float Damage => BuffController.Calculate(Characteristics.DAMAGE, _config.Damage);
        public float Armor => BuffController.Calculate(Characteristics.ARMOR, _config.Armor);
        public float MoneyDrop => BuffController.Calculate(Characteristics.MONEY_DROP, _config.DropMoney);

        #endregion

        public bool IsActive { get; set; }
        public bool IsAlive { get; private set; }

        public float RemainingDistance => _pathController.RemainingDistance;

        public BuffController BuffController => _buffController;
        public EnemyType EnemyType => _type;

        #region Init

        public void Init(ParticlesGenerator particlesGenerator, AudioService audioService)
        {
            _particlesGenerator = particlesGenerator;
            _audioService = audioService;
        }

        private void Awake()
        {
            _buffController = new BuffController();
            _pathController = new PathController();
            _movement = new EnemyMovementController(transform, _pathController);
            _movement.OnMoveFinished += FinishReached;
        }

        public void ResetData()
        {
            _buffController.Reset();
            _pathController.Clear();

            _currHP = _config.HP;
        }

        public void SetLane(PathLane lane) => _lane = lane;

        public void BuildRoute(List<Vector3> points)
        {
            _points = PathController.OffsetPath(points, _lane);
            _pathController.SetPath(_points, transform.position);
            _movement.Reset();
        }

        #endregion

        #region Game loop

        public void Tick(float dt)
        {
            if (!IsActive || !IsAlive) return;

            _buffController.Update(dt);
            _movement.Move(dt, Speed);
            Rotate();
        }

        #endregion

        private void Rotate() => transform.LookAt2D(_movement.CurrTarget);

        private void OnDrawGizmos()
        {
            if (_points == null || _points.Count == 0)
                return;

            Gizmos.color = Color.red;

            for (int i = 0; i < _points.Count; i++)
            {
                Gizmos.DrawSphere(_points[i], 0.1f);

                if (i < _points.Count - 1)
                {
                    Gizmos.DrawLine(_points[i], _points[i + 1]);
                }
            }
        }

        public void TakeDamage(float damage, float armorPiercing, Vector2 hitDirection = default)
        {
            var tmpArmor = Armor;
            tmpArmor = Mathf.Max(0, tmpArmor - armorPiercing);
            damage *= 1 - tmpArmor;

            _currHP = Mathf.Max(_currHP - damage, 0);

            if (_currHP <= 0)
            {
                DeathByPlayer();
                return;
            }

            var request = hitDirection != default
                ? ParticleRequest.WithDirection(ParticlesType.Blood, transform.position, hitDirection)
                : ParticleRequest.At(ParticlesType.Blood, transform.position);

            _particlesGenerator.Play(request);
            _audioService.Play(_config.HitSound);
        }

        private void FinishReached()
        {
            OnReachedFinish?.Invoke(Damage);
            Death();
        }

        #region Lifecycle

        public void OnPreload() { }

        public void OnActivated()
        {
            ResetData();
            IsAlive = true;
        }

        private void DeathByPlayer()
        {
            if (!IsAlive) return;
            IsAlive = false;

            var request = ParticleRequest.At(ParticlesType.EnemyDeath, transform.position);
            _particlesGenerator.Play(request);
            _audioService.Play(_config.DeathSound);
            OnDeath?.Invoke(this);
        }

        private void Death()
        {
            if (!IsAlive) return;
            IsAlive = false;

            OnDeath?.Invoke(this);
        }

        public void OnDeactivated() { }

        public void OnReturned()
        {
            IsAlive = false;
        }

        public void Dispose()
        {
            _buffController.Dispose();
            _pathController.Dispose();
        }

        public void OnDestroyed() { }

        #endregion
    }
}