using UnityEngine;
using System;
using Unity.VisualScripting;
public class TimeEvent
{
    public static float speedModifier = 1f; //velocidad normal

    public static TimeEvent Create(Action action, float timer, float minSpeedMod = 0.5f, float maxSpeedMod = 1.5f)
    {

        int randomChoice = UnityEngine.Random.Range(0, 2);
        float speedMod = randomChoice == 0 ? minSpeedMod : maxSpeedMod;

        GameObject gameObject = new GameObject("TimeEvent", typeof(GetMonoBehaviour));
        TimeEvent timeEvent = new TimeEvent(action, timer, gameObject, speedMod);
        gameObject.GetComponent<GetMonoBehaviour>().onUpdate = timeEvent.Update;
        gameObject.GetComponent<GetMonoBehaviour>().onDestroy = timeEvent.DestroyEvent;

        Debug.Log(randomChoice == 0 ? "Slowing down game!" : "Speeding up game!");
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

    public TimeEvent(Action action, float timer, GameObject gameObject, float speedMod = 1f)
    {
        this.action = action;
        this.timer = timer;
        this.gameObject = gameObject;
        isDestroyed = false;
        TimeEvent.speedModifier = speedMod;
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
        TimeEvent.speedModifier = 1f;
        isDestroyed = true;
        UnityEngine.Object.Destroy(gameObject);
    }


}