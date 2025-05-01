using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TankMovement))]
public class TankControlModifier : MonoBehaviour
{
    public enum ModifierType { SpeedUp, SlowDown, InvertControls }

    private TankMovement tankMovement;

    private void Awake() => tankMovement = GetComponent<TankMovement>();

    public void ApplyRandomEffect(float duration)
    {
        ModifierType randomEffect = (ModifierType)Random.Range(0, 3);
        float speedMod = 1f;
        bool invert = false;

        switch (randomEffect)
        {
            case ModifierType.SpeedUp:
                speedMod = Random.Range(1.2f, 2f);
                Debug.Log("Speed Boost Activated!");
                break;

            case ModifierType.SlowDown:
                speedMod = Random.Range(0.3f, 0.8f);
                Debug.Log("Slow Down Effect!");
                break;

            case ModifierType.InvertControls:
                invert = true;
                speedMod = Random.Range(0.8f, 1.2f); // Slight speed variation
                Debug.Log("Controls Inverted!");
                break;
        }

        // Apply the effect immediately
        tankMovement.ModifyControls(invert, tankMovement.GetInitialSpeed() * speedMod, duration);

        // Create timer to log when effect ends
        TimeEvent.Create(
            action: () => Debug.Log($"Effect ended: {randomEffect}"),
            timer: duration,
            speedMod: speedMod
        );
    }
}