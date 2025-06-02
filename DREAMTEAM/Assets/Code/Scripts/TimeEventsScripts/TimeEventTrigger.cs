using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTimeEvent : MonoBehaviour
{

    [SerializeField] private float slowSpeed = 0.5f; // 50% speed
    [SerializeField] private float fastSpeed = 1.5f; // 150% speed
    [SerializeField] private float smallSize = 0.50f; // 50% size
    [SerializeField] private float largeSize = 1.50f; // 150% size

    private void OnEventComplete()
    {
        Debug.Log("Time event ended - returned to normal");
        // Reset both speed and size
        TimeEvent.speedModifier = 1f;
        TimeEvent.sizeModifier = 1f;
        TimeEvent.invertControls = false;
    }

    // Call this to trigger a new random speed event
    public void TriggerNewEvent(float duration)
    {
        TimeEvent.Create(OnEventComplete, duration, slowSpeed, fastSpeed, smallSize, largeSize);
    }
}
