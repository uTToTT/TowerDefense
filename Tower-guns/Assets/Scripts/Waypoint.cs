using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Direction _nextDirection;
    [SerializeField] private bool _isCentralPoint;

    private Enemy _enemy;
    private TypeEnemy _type;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            _enemy = collision.GetComponent<Enemy>();
            _type = _enemy.GetEnemyType();

            if (_isCentralPoint)
            {
                if (_type == TypeEnemy.Heavy || _type == TypeEnemy.King)
                {
                    if (Vector2.Distance(collision.transform.position, transform.position) < 0.2f)
                    {
                        _enemy.SetMoveDirection(_nextDirection);
                    }
                    else
                    {
                        _enemy.MoveTo(transform);
                    }
                }
            }
            else
            {
                if (_type != TypeEnemy.Heavy && _type != TypeEnemy.King)
                {
                    if (Vector2.Distance(collision.transform.position, transform.position) < 0.2f)
                    {
                        _enemy.SetMoveDirection(_nextDirection);
                    }
                    else
                    {
                        _enemy.MoveTo(transform);
                    }
                }
            }
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.GetComponent<Enemy>())
    //    {
    //        _enemy = collision.GetComponent<Enemy>();
    //        _type = _enemy.GetEnemyType();

    //        if (_isCentralPoint)
    //        {
    //            if (_type == TypeEnemy.Heavy || _type == TypeEnemy.King)
    //            {
    //                _enemy.SetMoveDirection(_nextDirection);
    //            }
    //        }
    //        else
    //        {
    //            if (_type != TypeEnemy.Heavy && _type != TypeEnemy.King)
    //            {
    //                _enemy.SetMoveDirection(_nextDirection);
    //            }
    //        }
    //    }
    //}

    public void SetDirection(Direction direction)
    {
        _nextDirection = direction;
    }
}

public enum Direction
{
    None,
    Up,
    Down,
    Left,
    Right
}