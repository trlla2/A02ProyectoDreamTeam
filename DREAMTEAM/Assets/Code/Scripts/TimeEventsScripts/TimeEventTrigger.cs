using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTimeEvent : MonoBehaviour
{

        [SerializeField] private float eventDuration = 3f;
        [SerializeField] private float slowSpeed = 0.5f; // 50% speed
        [SerializeField] private float fastSpeed = 1.5f; // 150% speed

        void Start()
        {
            // Create event with random speed modification
            TimeEvent.Create(OnEventComplete, eventDuration, slowSpeed, fastSpeed);
        }

        private void OnEventComplete()
        {
            Debug.Log("Speed event ended - returned to normal speed");
        }

        // Call this to trigger a new random speed event
        public void TriggerNewEvent()
        {
            TimeEvent.Create(OnEventComplete, eventDuration, slowSpeed, fastSpeed);
        }
}
