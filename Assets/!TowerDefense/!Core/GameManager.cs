using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MoveManager _moveManager;

    private void Update()
    {
        _moveManager.Tick();
    }
}
