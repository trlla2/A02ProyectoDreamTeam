using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTimeEvent : MonoBehaviour
{
    private void Start()
    {
        TimeEvent.Create(TestTimeAction, 3f);
    }

    private void TestTimeAction()
    {
        Debug.Log("Test");
    }
}
