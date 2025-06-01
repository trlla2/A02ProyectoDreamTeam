/*
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TankMovement))]
public class TankControlModifier : MonoBehaviour
{
    public enum ModifierType { SpeedUp, SlowDown, InvertControls, Shrink }

    [Header("Shrink Settings")]
    [SerializeField] private float shrinkScale = 0.5f;

    private TankMovement tankMovement;
    private Vector3 originalScale;
    private Coroutine currentEffectRoutine;
    private Collider2D tankCollider;

    private void Awake()
    {
        tankMovement = GetComponent<TankMovement>();
        originalScale = transform.localScale;
        tankCollider = GetComponent<Collider2D>();
    }

    public void ApplyRandomEffect(float duration)
    {
        if (currentEffectRoutine != null)
        {
            StopCoroutine(currentEffectRoutine);
            ResetToDefault();
        }

        ModifierType randomEffect = (ModifierType)Random.Range(0, 4);
        float speedMod = 1f;
        bool invert = false;
        bool shrink = false;

        switch (randomEffect)
        {
            case ModifierType.SpeedUp:
                speedMod = Random.Range(1.2f, 2f);
                Debug.Log("Speed Boost Activated!");
                break;

            case ModifierType.SlowDown:
                speedMod = Random.Range(0.3f, 0.8f);
                Debug.Log("Slow Down Effect!");
                break;

            case ModifierType.InvertControls:
                invert = true;
                speedMod = 1f; // No speed change
                Debug.Log("Controls Inverted!");
                break;

            case ModifierType.Shrink:
                shrink = true;
                speedMod = 1f; // No speed change
                Debug.Log("Shrink Effect!");
                break;
        }

        currentEffectRoutine = StartCoroutine(ApplyEffectRoutine(speedMod, invert, shrink, duration));
    }

    private IEnumerator ApplyEffectRoutine(float speedMod, bool invert, bool shrink, float duration)
    {
        // Apply visual effect
        if (shrink)
        {
            transform.localScale = originalScale * shrinkScale;
            if (tankCollider != null)
                tankCollider.transform.localScale = originalScale * shrinkScale;
        }

        // Only modify controls if not shrinking
        if (!shrink)
        {
            float newSpeed = tankMovement.GetInitialSpeed() * speedMod;
            tankMovement.ModifyControls(invert, newSpeed, duration);
        }

        yield return new WaitForSeconds(duration);

        ResetToDefault();
        Debug.Log("Effect ended");
        currentEffectRoutine = null;
    }

    private void ResetToDefault()
    {
        transform.localScale = originalScale;
        tankMovement.ResetToDefaultControls();
    }

    public void CancelCurrentEffect()
    {
        if (currentEffectRoutine != null)
        {
            StopCoroutine(currentEffectRoutine);
            ResetToDefault();
            currentEffectRoutine = null;
        }
    }
}
*/