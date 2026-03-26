using UnityEngine;

public class Like : MonoBehaviour // legacy
{
    public void OnClick()
    {
        Application.OpenURL("market://details?id=" + Application.identifier);
    }
}
