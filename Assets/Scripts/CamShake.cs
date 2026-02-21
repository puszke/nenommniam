using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    public static CamShake Instance { get; private set; }

    private Vector3 originalPos;
    private Coroutine shakeRoutine;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        originalPos = transform.localPosition;
    }

    /// <summary>
    /// Wywo³aj trzêsienie kamery
    /// </summary>
    public void Shake(float duration, float strength)
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(ShakeCoroutine(duration, strength));
    }

    private IEnumerator ShakeCoroutine(float duration, float strength)
    {
        float timer = 0f;

        while (timer < duration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * strength;
            transform.localPosition = originalPos + randomOffset;

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        shakeRoutine = null;
    }
}
