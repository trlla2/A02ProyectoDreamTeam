using UnityEngine;
using System;
using Unity.VisualScripting;

public class TimeEvent
{
    public static float speedModifier = 1f; // normal speed
    public static float sizeModifier = 1f; // normal size

    public static TimeEvent Create(Action action, float timer, float minSpeedMod = 0.5f, float maxSpeedMod = 1.5f, float minSizeMod = 0.75f, float maxSizeMod = 1.25f)
    {
        // Randomly choose between speed or size event
        int eventType = UnityEngine.Random.Range(0, 2); // 0 = speed, 1 = size

        float speedMod = 1f;
        float sizeMod = 1f;
        string eventMessage = "";

        switch (eventType)
        {
            case 0: // Speed event
                int speedChoice = UnityEngine.Random.Range(0, 2);
                speedMod = speedChoice == 0 ? minSpeedMod : maxSpeedMod;
                eventMessage = speedChoice == 0 ? "Slowing down game!" : "Speeding up game!";
                break;

            case 1: // Size event
                int sizeChoice = UnityEngine.Random.Range(0, 2);
                sizeMod = sizeChoice == 0 ? minSizeMod : maxSizeMod;
                eventMessage = sizeChoice == 0 ? "Shrinking tanks!" : "Enlarging tanks!";
                break;
        }

        GameObject gameObject = new GameObject("TimeEvent", typeof(GetMonoBehaviour));
        TimeEvent timeEvent = new TimeEvent(action, timer, gameObject, speedMod, sizeMod);
        gameObject.GetComponent<GetMonoBehaviour>().onUpdate = timeEvent.Update;
        gameObject.GetComponent<GetMonoBehaviour>().onDestroy = timeEvent.DestroyEvent;

        Debug.Log(eventMessage);
        return timeEvent;
    }

    private class GetMonoBehaviour : MonoBehaviour
    {
        public Action onUpdate;
        public Action onDestroy;
        private void Update()
        {
            if (onUpdate != null) onUpdate();
        }

        private void OnDestroy()
        {
            if (onDestroy != null) onDestroy();
        }
    }

    private Action action;
    private float timer;
    private GameObject gameObject;
    private bool isDestroyed;

    public TimeEvent(Action action, float timer, GameObject gameObject, float speedMod = 1f, float sizeMod = 1f)
    {
        this.action = action;
        this.timer = timer;
        this.gameObject = gameObject;
        isDestroyed = false;
        TimeEvent.speedModifier = speedMod;
        TimeEvent.sizeModifier = sizeMod;
    }

    public void Update()
    {
        if (!isDestroyed)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                DestroyEvent();
            }
        }
    }

    private void DestroyEvent()
    {
        Debug.Log("Invoke action");
        action?.Invoke();
        isDestroyed = true;
        UnityEngine.Object.Destroy(gameObject);
    }
}