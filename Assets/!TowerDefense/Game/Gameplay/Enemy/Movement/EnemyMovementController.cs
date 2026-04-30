// not extensible but it's enough for this case
using System;
using UnityEngine;

namespace TToTT.TowerDefense.Enemies
{
    public class EnemyMovementController
    {
        public event Action OnMoveFinished;

        private readonly PathController _path;
        private readonly Transform _transform;

        private bool _finished = false;

        private Vector3 _direction;
        private Vector3 _currTarget;

        public Vector3 Direction => _direction;
        public Vector3 CurrTarget => _currTarget;

        // TODO: remove Transform in pure C# class
        public EnemyMovementController(
            Transform transform,
            PathController path)
        {
            _transform = transform;
            _path = path;
        }

        public void Move(float dt, float speed)
        {
            _path.Advance(_transform.position);

            if (!_path.HasPath)
            {
                if (_finished) return;
                _finished = true;
                OnMoveFinished?.Invoke();
                return;
            }

            _currTarget = _path.Peek();
            _direction = (_currTarget - _transform.position).normalized;

            _transform.MoveTowards(_currTarget, speed, dt);
        }

        public void Reset()
        {
            _finished = false;
        }
    }
}