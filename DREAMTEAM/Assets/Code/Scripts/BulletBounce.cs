using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/InfiniteBounce")] //create a menu specifictly for powerups for nice organization

public class BulletBounce : PowerUpEffect
{
    [SerializeField] private float powerUpDuration = 25f;  // Duración del efecto

    public override void Apply(GameObject target)
    {
        Weapon weapon = target.GetComponent<Weapon>();

        //  target.GetComponent<Bullet>().bounceTime = 9999f; //powerup for player to bounce the bullet near infinitely in context of a round
        weapon.ActivateinfiniteBounce(powerUpDuration);
    }
}
