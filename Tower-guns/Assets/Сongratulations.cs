using UnityEngine;

public class Сongratulations : MonoBehaviour
{
    [SerializeField] private GameObject _greetings;

    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("StarCount") == 150)
        {
            _greetings.SetActive(true);
            PlayerPrefs.SetInt("PlatinumStar1", 1);
        }
        else
        {
            _greetings.SetActive(false);
        }
    }
}
