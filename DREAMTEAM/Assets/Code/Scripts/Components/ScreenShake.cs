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
    [SerializeField]
    private float durationScreenShake = 0.2f;
    [SerializeField]
    private float magnitudeScreenShake = 0.3f;

    private Vector3 initialPos;

    private bool isShaking = false;

    private void Start()
    {
        // Get initial position

        initialPos = transform.position;
    }

    public void Shake()
    {
        if (!isShaking) 
            StartCoroutine(CameraShake()); // Start the coroutine
    }
    private IEnumerator CameraShake()
    {
        isShaking = true;

        float timer = 0;

        while (timer < durationScreenShake)
        {
           
            //Random position
            float x = Random.Range(-cameraShakeForce, cameraShakeForce) * magnitudeScreenShake;
            float y = Random.Range(-cameraShakeForce, cameraShakeForce) * magnitudeScreenShake;

            transform.localPosition += new Vector3(x, y, 0); // ad random position to current position

            timer += Time.deltaTime; // add timer

            transform.position = Vector3.Lerp(transform.position, initialPos, cameraLerp * Time.unscaledDeltaTime); // move camera to center
            
            yield return null;
        }

        while(transform.position != initialPos)
        {
            
            transform.position = Vector3.Lerp(transform.position, initialPos, cameraLerp * Time.unscaledDeltaTime);// move camera to center
            
            yield return null;

        }

        isShaking = false;

    }
}
