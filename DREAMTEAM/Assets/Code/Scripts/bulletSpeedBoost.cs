using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/BulletSpeedBoost")]
public class bulletSpeedBoost : PowerUpEffect
{
    [SerializeField] private float powerUpDuration = 25f;  // Duración del efecto

    public override void Apply(GameObject target) //apply power up
    {
        Weapon weapon = target.GetComponent<Weapon>();

        weapon.ActivateBulletSpeedBoost(powerUpDuration); 
    }
}
