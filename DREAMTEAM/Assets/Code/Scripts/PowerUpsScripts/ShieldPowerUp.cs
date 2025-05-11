using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/ShieldPowerUp")]
public class ShieldPowerUp : PowerUpEffect
{
    public override void Apply(GameObject target)
    {
        Weapon weapon = target.GetComponent<Weapon>();
        if (weapon != null)
        {
            weapon.ActivateShield();  
        }
    }
}
