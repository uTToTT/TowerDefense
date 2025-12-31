using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _buttonChangeLevelLeft;
    [SerializeField] private Button _buttonChangeLevelRight;
    [Space]
    [SerializeField] private GameObject[] _levelPanels;

    private int _currLevelPanel;

    private void Start()
    {
        _currLevelPanel = PlayerPrefs.GetInt("_currLevelPanel");
        //Debug.Log("Start: " + _currLevelPanel);

        DisableAllPanels();
        EnablePanel(_currLevelPanel);
    }

    private void FixedUpdate()
    {
        if (_currLevelPanel == 0)
        {
            _buttonChangeLevelLeft.interactable = false;
        }
        else
        {
            _buttonChangeLevelLeft.interactable = true;

        }

        if (_currLevelPanel == _levelPanels.Length - 1)
        {
            _buttonChangeLevelRight.interactable = false;
        }
        else
        {
            _buttonChangeLevelRight.interactable = true;
        }
    }

    public void SwipeLeft()
    {
        DisableAllPanels();

        if (_currLevelPanel != 0)
        {
            _currLevelPanel--;
        }

        Debug.Log("Swipe left: " + _currLevelPanel);
        PlayerPrefs.SetInt("_currLevelPanel", _currLevelPanel);
        EnablePanel(_currLevelPanel);
    }

    public void SwipeRight()
    {
        DisableAllPanels();

        if (_currLevelPanel != _levelPanels.Length - 1)
        {
            _currLevelPanel++;
        }
        Debug.Log("Swipe right: " + _currLevelPanel);
        PlayerPrefs.SetInt("_currLevelPanel", _currLevelPanel);
        EnablePanel(_currLevelPanel);
    }

    public void LoadLink(string link)
    {
        Application.OpenURL(link);
    }

    private void DisableAllPanels()
    {
        foreach (var item in _levelPanels)
        {
            item.SetActive(false);
        }
    }

    public void ResetSwipePrefs()
    {
        PlayerPrefs.SetInt("_currLevelPanel", 0);
    }

    private void EnablePanel(int index)
    {
        _levelPanels[index].gameObject.SetActive(true);
    }

    public void SceneLoad(int levelNum)
    {
        SceneManager.LoadScene("Level" + levelNum);
        Time.timeScale = 1;
    }

    public void GameobjectSetActiveTrue(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void GameobjectSetActiveFalse(GameObject obj)
    {
        obj.SetActive(false);
    }
 }
