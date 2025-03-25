using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEvent 
{

    public static TimeEvent Create(Action action, float timer)
    {
        GameObject gameObject = new GameObject("TimeEvent", typeof(GetMonoBehaviour));

        TimeEvent timeEvent = new TimeEvent(action, timer, gameObject);

       
        gameObject.GetComponent<GetMonoBehaviour>().onUpdate = timeEvent.Update;

        return timeEvent;
    }



    private class GetMonoBehaviour : MonoBehaviour
    {
        public Action onUpdate;
        private void Update()
        {
            if (onUpdate != null) onUpdate();
        }
    }

    private Action action;
    private float timer;
    private GameObject gameObject;
    private bool isDestroyed;
    public TimeEvent(Action action, float timer, GameObject gameObject)
    {
        this.action = action;
        this.timer = timer;
        this.gameObject = gameObject;
        isDestroyed = false;
    }
    
    public void Update()
    {
        if (!isDestroyed)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                action();
                DestroyEvent();
            }
        }
    }

    private void DestroyEvent()
    {
        isDestroyed = true;
        UnityEngine.Object.Destroy(gameObject);
    }

}
