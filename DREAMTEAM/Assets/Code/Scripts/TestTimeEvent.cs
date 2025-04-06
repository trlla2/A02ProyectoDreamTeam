using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTimeEvent : MonoBehaviour
{
    [SerializeField] private float slowSpeed = 0.5f; // 50% speed
    [SerializeField] private float fastSpeed = 1.5f; // 150% speed


    private void OnEventComplete()
    {
        Debug.Log("Speed event ended - returned to normal speed");
    }

    // Call this to trigger a new random speed event
    public void TriggerNewEvent(float duration)
    {
            Debug.Log("Time Event triggered");
            TimeEvent.Create(OnEventComplete, duration, slowSpeed, fastSpeed);
    }
}
