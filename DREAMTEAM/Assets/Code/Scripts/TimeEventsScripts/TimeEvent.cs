using UnityEngine;
using System;
using System.Collections;

public class TimeEvent : MonoBehaviour
{
    public enum EffectType { SpeedUp, SlowDown, InvertControls, Shrink }

    [Header("TimeEvent Settings")]
    [SerializeField] private float shrinkScale = 0.5f;
    [SerializeField] private Vector2 speedUpRange = new Vector2(1.2f, 2f);
    [SerializeField] private Vector2 slowDownRange = new Vector2(0.3f, 0.8f);

    private TankMovement tankMovement;
    private Vector3 originalScale;
    private Coroutine activeEffect;

    public static float speedModifier = 1f;
    private void Awake()
    {
        tankMovement = GetComponent<TankMovement>();
        originalScale = transform.localScale;
    }

    public void TriggerRandomEffect(float duration, Action onComplete = null)
    {
        if (activeEffect != null)
        {
            StopCoroutine(activeEffect);
            ResetEffects();
        }

        var effect = (EffectType)UnityEngine.Random.Range(0, 4);
        activeEffect = StartCoroutine(ApplyEffect(effect, duration, onComplete));
    }

    private IEnumerator ApplyEffect(EffectType effect, float duration, Action onComplete)
    {
        // Apply effect immediately
        switch (effect)
        {
            case EffectType.SpeedUp:
                float speedBoost = UnityEngine.Random.Range(speedUpRange.x, speedUpRange.y);
                tankMovement.ModifyControls(false, tankMovement.GetInitialSpeed() * speedBoost, duration);
                speedModifier = UnityEngine.Random.Range(speedUpRange.x, speedUpRange.y);
                Debug.Log("Speed Boost!");
                break;

            case EffectType.SlowDown:
                float speedReduction = UnityEngine.Random.Range(slowDownRange.x, slowDownRange.y);
                tankMovement.ModifyControls(false, tankMovement.GetInitialSpeed() * speedReduction, duration);
                speedModifier = UnityEngine.Random.Range(slowDownRange.x, slowDownRange.y);
                Debug.Log("Slow Down!");
                break;

            case EffectType.InvertControls:
                tankMovement.ModifyControls(true, tankMovement.GetInitialSpeed(), duration);
                Debug.Log("Controls Inverted!");
                break;

            case EffectType.Shrink:
                transform.localScale = originalScale * shrinkScale;
                Debug.Log("Shrunk!");
                break;
        }

        // Wait for duration
        yield return new WaitForSeconds(duration);

        // Clean up
        ResetEffects();
        onComplete?.Invoke();
        activeEffect = null;
    }

    private void ResetEffects()
    {
        transform.localScale = originalScale;
        tankMovement.ResetToDefaultControls();
        speedModifier = 1f;
    }

    public void CancelEffect()
    {
        if (activeEffect != null)
        {
            StopCoroutine(activeEffect);
            ResetEffects();
            activeEffect = null;
        }
    }
}