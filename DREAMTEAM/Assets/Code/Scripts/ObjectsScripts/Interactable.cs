using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent OnHit;
    public void BulletHit()
    {
        OnHit.Invoke();
    }
}
