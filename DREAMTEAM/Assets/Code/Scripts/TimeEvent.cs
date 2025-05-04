using UnityEngine;
using System;
using Unity.VisualScripting;

public class TimeEvent
{
    public static float speedModifier = 1f; // Global speed modifier
    private Action action;
    private float timer;
    private GameObject timerObject;
    private bool isDestroyed;

    private class TimerComponent : MonoBehaviour
    {
        public Action onUpdate;
        public Action onDestroy;

        private void Update() => onUpdate?.Invoke();
        private void OnDestroy() => onDestroy?.Invoke();
    }

    public static TimeEvent Create(Action action, float timer, float speedMod = 1f)
    {
        GameObject go = new GameObject("TimeEvent", typeof(TimerComponent));
        TimeEvent timeEvent = new TimeEvent(action, timer, go, speedMod);
        go.GetComponent<TimerComponent>().onUpdate = timeEvent.Update;
        go.GetComponent<TimerComponent>().onDestroy = timeEvent.DestroyEvent;
        return timeEvent;
    }

    private TimeEvent(Action action, float timer, GameObject timerObject, float speedMod = 1f)
    {
        this.action = action;
        this.timer = timer;
        this.timerObject = timerObject;
        isDestroyed = false;
        TimeEvent.speedModifier = speedMod;
    }

    private void Update()
    {
        if (!isDestroyed)
        {
            timer -= Time.deltaTime * speedModifier;
            if (timer < 0) DestroyEvent();
        }
    }

    private void DestroyEvent()
    {
        action?.Invoke();
        TimeEvent.speedModifier = 1f;
        isDestroyed = true;
        UnityEngine.Object.Destroy(timerObject);
    }
}