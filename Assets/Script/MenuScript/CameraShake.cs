using System.Collections; // Bu ifade gerekiyor
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.2f;
    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.localPosition;
    }

    public void TriggerShake()
    {
        // Sallanma efektini başlat
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
