using UnityEngine;
using TMPro;

public class EventWarning : MonoBehaviour
{
    [Header ("Setup")]
    [SerializeField] private GameObject warningText;
    [SerializeField] private AudioSource warningSfx;

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
           warningSfx.Play();
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
