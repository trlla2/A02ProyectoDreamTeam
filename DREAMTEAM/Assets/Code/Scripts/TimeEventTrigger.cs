using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTimeEvent : MonoBehaviour
{
    [Header("Effect Settings")]
    [SerializeField] private float minDuration = 10f;
    [SerializeField] private float maxDuration = 20f;
    [SerializeField] private TankControlModifier tankControlModifier;

    private void Awake()
    {
        // Auto-get reference if not set in inspector
        if (tankControlModifier == null)
        {
            tankControlModifier = GetComponent<TankControlModifier>();
        }
    }

    private void OnEffectComplete()
    {
        Debug.Log("Tank effect ended - controls returned to normal");
    }

    // Call this to trigger a new random tank effect
    public void TriggerRandomEffect()
    {
        float randomDuration = Random.Range(minDuration, maxDuration);
        Debug.Log($"Triggering random tank effect for {randomDuration} seconds");

        if (tankControlModifier != null)
        {
            tankControlModifier.ApplyRandomEffect(randomDuration);

            TimeEvent.Create(
                OnEffectComplete,
                randomDuration
            );
        }
        else
        {
            Debug.LogWarning("No TankControlModifier found");
        }
    }
}
