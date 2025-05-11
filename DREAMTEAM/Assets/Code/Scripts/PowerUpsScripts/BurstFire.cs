using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PowerUps/BurstFirePowerUp")]
public class BurstFirePowerUp : PowerUpEffect
{
    [Header("Burst Fire Settings")]
    [SerializeField] private int bulletsPerBurst = 3;
    [SerializeField] private float delayBetweenShots = 0.1f;

    public int BulletsPerBurst => bulletsPerBurst;
    public float DelayBetweenShots => delayBetweenShots;

    public override void Apply(GameObject target)
    {
        Weapon weapon = target.GetComponent<Weapon>();
        if (weapon != null)
        {
            weapon.SetPowerUp(this);
        }
    }
}