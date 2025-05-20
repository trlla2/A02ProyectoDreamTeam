using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTimeEvent : MonoBehaviour
{
    [Header("Effect Settings")]
    [SerializeField] private float minDuration = 10f;
    [SerializeField] private float maxDuration = 20f;

    private TimeEvent timeEvent;

    private void Awake()
    {

        timeEvent = GetComponent<TimeEvent>();

        if (timeEvent == null)
        {
            Debug.LogError("TimeEvent component missing!", this);
        }
    }

    public void TriggerRandomEffect()
    {

        Debug.Log("trigger timeEvent");

        if (timeEvent == null) return;

        float duration = Random.Range(minDuration, maxDuration);
        Debug.Log($"Triggering random tank effect for {duration} seconds");

        timeEvent.TriggerRandomEffect(duration, OnEffectComplete);
    }

    private void OnEffectComplete()
    {
        Debug.Log("Tank effect ended - controls returned to normal");
    }
}
