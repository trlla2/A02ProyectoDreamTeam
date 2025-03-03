using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [Header("Screen Shake Main Parameters")]
    [SerializeField]
    private float cameraShakeForce = 1;
    [SerializeField]
    private float cameraLerp = 12f;

    [Header("Debug")]
    [SerializeField]
    private float durationScreenShake = 3;
    [SerializeField]
    private float magnitudeScreenShake = 10;

    private Vector3 initialPos;

    private void Start()
    {
        // Get initial position

        initialPos = transform.position;

        // Debug
        StartCoroutine(CameraShake(durationScreenShake, magnitudeScreenShake));
    }
    public IEnumerator CameraShake(float duration, float magnitude)
    {
        float timer = 0;

        while (timer < magnitude)
        {
            //Random position
            float x = Random.Range(-cameraShakeForce, cameraShakeForce) * magnitude;
            float y = Random.Range(-cameraShakeForce, cameraShakeForce) * magnitude;

            transform.localPosition += new Vector3(x, y, 0); // ad random position to current position

            timer += Time.deltaTime; // add timer

            transform.position = Vector3.Lerp(transform.position, initialPos, cameraLerp * Time.deltaTime); // move camera to center
            yield return null;
        }

        while(transform.position != initialPos)
        {
            transform.position = Vector3.Lerp(transform.position, initialPos, cameraLerp * Time.deltaTime);// move camera to center

            yield return null;
        }
    }
}
