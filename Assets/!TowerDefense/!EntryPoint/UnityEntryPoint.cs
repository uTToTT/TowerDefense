using UnityEngine;

public class UnityEntryPoint : MonoBehaviour
{
    private GameBootstrap _bootstrap;

    private void Awake()
    {
        _bootstrap = new GameBootstrap();

        _bootstrap.Initialize();

        StartGame();
    }

    private void StartGame()
    {

    }
}
