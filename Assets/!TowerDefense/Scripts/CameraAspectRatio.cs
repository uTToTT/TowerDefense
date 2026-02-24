using UnityEngine;

public class CameraAspectRatio : MonoBehaviour
{
    public float targetAspectWidth = 9f;
    public float targetAspectHeight = 16f;
    private float pixelsToUnits = 0.46f;

    void Start()
    {
        // Рассчитываем желаемое соотношение сторон камеры
        float targetAspect = targetAspectWidth / targetAspectHeight;

        // Получаем текущее соотношение сторон экрана
        float currentAspect = (float)Screen.width / Screen.height;

        // Вычисляем масштаб, который нужно применить к камере
        float scaleHeight = currentAspect / targetAspect;

        UnityEngine.Camera camera = GetComponent<UnityEngine.Camera>();

        // Если текущее соотношение сторон экрана шире желаемого, устанавливаем размер камеры по ширине
        if (scaleHeight < 1.0f)
        {
            camera.orthographicSize = targetAspectHeight / (pixelsToUnits * 2.0f * scaleHeight);
        }
        else // В противном случае устанавливаем размер камеры по высоте
        {
            camera.orthographicSize = targetAspectHeight / (pixelsToUnits * 2.0f);
        }
    }
}
