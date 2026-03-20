using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private float _delayWinPanel;
    [SerializeField] private float _delayGiveStar;
    [SerializeField] private GameObject _panelPause;
    [SerializeField] private GameObject _panelLose;
    [SerializeField] private GameObject _panelWin;
    [Space]
    [SerializeField] private Image[] _stars;
    [Space]
    [SerializeField] private Player _healthControl;
    [Space]

    private float _currGameSpeed;

    private void Start()
    {
        _panelPause.SetActive(false);
        _panelLose.SetActive(false);
        _panelWin.SetActive(false);

        foreach (var star in _stars)
        {
            star.gameObject.SetActive(false);
        }
    }

    private void StopTime()
    {
        _currGameSpeed = Time.timeScale;
        Time.timeScale = 0;
    }

    private void StartTime()
    {
        Time.timeScale = _currGameSpeed;
    }

    private void Lose()
    {
        _panelLose.SetActive(true);
        StopTime();
    }

    private void Win()
    {
        Time.timeScale = 1;
        StartCoroutine(ShowPanelWin());
    }

    IEnumerator ShowPanelWin()
    {
        yield return new WaitForSeconds(_delayWinPanel);
        _panelWin.SetActive(true);

        int countStar = 0;

        if (_healthControl.CurrHP == 20)
        {
            countStar = 3;
        }
        if (_healthControl.CurrHP <= 19)
        {
            countStar = 2;
        }
        if (_healthControl.CurrHP <= 9)
        {
            countStar = 1;
        }

        EventBus.GetStar?.Invoke(countStar, SceneManager.GetActiveScene().buildIndex);

        for (int i = 0; i < countStar; i++)
        {
            _stars[i].gameObject.SetActive(true);

            yield return new WaitForSeconds(_delayGiveStar);
        }

        StopTime();
    }

    public void CloseMenu()
    {
        _panelPause.SetActive(false);
        StartTime();
    }

    public void OpenMenu()
    {
        _panelPause.SetActive(true);
        StopTime();
    }

    public void RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        Time.timeScale = 1;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadNextLevel()
    {
        Debug.Log("Load next level");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
            Time.timeScale = 1;
        }
        else
        {
            Debug.Log("Coming soon...");
        }
    }

    public void GameobjectSetActiveTrue(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void GameobjectSetActiveFalse(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void DisablePanelLose()
    {
        _panelLose.SetActive(false);
    }

    private void OnEnable()
    {
        EventBus.onPanelLoseDisable += DisablePanelLose;
        EventBus.onNextSceneLoad += LoadNextLevel;
        EventBus.GameOver += Lose;
        EventBus.GameWin += Win;
    }

    private void OnDisable()
    {
        EventBus.onPanelLoseDisable -= DisablePanelLose;
        EventBus.onNextSceneLoad -= LoadNextLevel;
        EventBus.GameOver -= Lose;
        EventBus.GameWin -= Win;
    }
}
