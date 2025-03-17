using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/TripleShot")]
public class TripleShot : PowerUpEffect
{
    [SerializeField] private float powerUpDuration = 25f;  // Duración del efecto

    public override void Apply(GameObject target)
    {
        Weapon weapon = target.GetComponent<Weapon>();

        if (weapon == null)
        {
            Debug.LogWarning("El objeto objetivo no tiene un componente Weapon.");
            return;
        }

        weapon.ActivateTripleShot(powerUpDuration);  // Activa el Power-Up en Weapon
    }
}