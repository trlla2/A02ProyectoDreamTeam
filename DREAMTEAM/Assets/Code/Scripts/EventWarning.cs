using UnityEngine;
using TMPro;

public class EventWarning : MonoBehaviour
{

    [SerializeField] private GameObject warningText; 

    private void Start()
    {
        GameManager.Instance.OnTimeEventWarning += SetWarningText; // Subscribe Event

        warningText.SetActive(false);
    }

    private void SetWarningText(bool timeEventWarning)
    {
        if (timeEventWarning)
        {
           warningText.SetActive(true);
        }
        else { 
           warningText.SetActive(false); 
        }

    }

    private void OnDestroy()
    {
        GameManager.Instance.OnTimeEventWarning -= SetWarningText; // UnSubscribe Event
    }
}
