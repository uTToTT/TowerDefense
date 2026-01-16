using UnityEngine;

public class HelpController : MonoBehaviour
{
    [SerializeField] private GameObject[] _helpObjectsFirst;
    [SerializeField] private GameObject[] _helpObjectsSecond;

    private void DeleteHelpObjects()
    {
        foreach (var item in _helpObjectsFirst)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in _helpObjectsSecond)
        {
            Destroy(item.gameObject);
        }

        Destroy(this.gameObject);
    }

    private void GoToSecondHelp()
    {
        foreach (var item in _helpObjectsFirst)
        {
            item.gameObject.SetActive(false);
        }

        foreach (var item in _helpObjectsSecond)
        {
            item.gameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {
        EventBus.FirstTowerWasBuilt += DeleteHelpObjects;
        EventBus.onCellSelected += GoToSecondHelp;
    }

    private void OnDisable()
    {
        EventBus.FirstTowerWasBuilt -= DeleteHelpObjects;
        EventBus.onCellSelected -= GoToSecondHelp;
    }
}
