using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "PowerUps/BulletSpeedBoost")]
public class bulletSpeedBoost : PowerUpEffect
{
    private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeedBoosted = 5f;
    public override void Apply(GameObject target) //apply power up
    {
        Weapon weapon = target.GetComponent<Weapon>();

        firePoint = weapon.firePoint;

        weapon.SetPowerUp(this);
    }

    public void BulletSpeedBoost()
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Obtener el componente Bullet de la instancia y modificar bullet speed
        Bullet bulletScript = bulletInstance.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.bulletSpeed = bulletSpeedBoosted; //powerup for player to speed up bullet
        }
    }
}