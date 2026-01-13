using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    public Vector3 currentShakeOffset = Vector3.zero;

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            currentShakeOffset = new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        currentShakeOffset = Vector3.zero;
    }
}
