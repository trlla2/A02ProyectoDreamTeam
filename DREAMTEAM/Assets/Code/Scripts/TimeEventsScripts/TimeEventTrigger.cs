using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTimeEvent : MonoBehaviour
{
    [Header("Effect Settings")]
    [SerializeField] private float minDuration = 10f;
    [SerializeField] private float maxDuration = 20f;
    private TankControlModifier tankControlModifier;

        [SerializeField] private float slowSpeed = 0.5f; // 50% speed
        [SerializeField] private float fastSpeed = 1.5f; // 150% speed

        void Start()
        {
            // Create event with random speed modification
            //TimeEvent.Create(OnEventComplete, eventDuration, slowSpeed, fastSpeed);
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

        // Call this to trigger a new random speed event
        public void TriggerNewEvent(float duration)
        {
            TimeEvent.Create(OnEventComplete, duration, slowSpeed, fastSpeed);
        }
    }
}
