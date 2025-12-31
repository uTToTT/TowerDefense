using UnityEngine;

public class Screnshots : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            int i = 0;

            while (PlayerPrefs.HasKey("Screenshots" + i))
            {
                i++;
            }

            ScreenCapture.CaptureScreenshot("Screenshot_" + i + ".png");
            PlayerPrefs.SetInt("Screenshots" + i, i);
        }
    }
}
