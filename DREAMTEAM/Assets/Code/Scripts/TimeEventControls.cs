using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TankMovement))]
public class TimeEventControls : MonoBehaviour
{
    private TankMovement tankMovement;

    private void Awake()
    {
        tankMovement = GetComponent<TankMovement>();
    }

    public void ApplyControlModifier(float duration, bool invert = false, float speedMultiplier = 1f)
    {
        if (tankMovement == null)
        {
            Debug.LogWarning("TankMovement no encontrado en " + gameObject.name);
            return;
        }

        float newSpeed = tankMovement.GetInitialSpeed() * speedMultiplier;
        tankMovement.ModifyControls(invert, newSpeed, duration);
    }
}
