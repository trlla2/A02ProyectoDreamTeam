using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/InfiniteBounce")] //create a menu specifictly for powerups for nice organization

public class BulletBounce : PowerUpEffect
{
    public override void Apply(GameObject target)
    {
        target.GetComponent<Bullet>().bounceTime = 9999f; //powerup for player to bounce the bullet near infinitely in context of a round
    }
}
