using UnityEngine;

public class Like : MonoBehaviour
{
    public void OnClick()
    {
        Application.OpenURL("market://details?id=" + Application.identifier);
    }
}
