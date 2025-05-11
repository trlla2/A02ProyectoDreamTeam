using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "PowerUps/BulletSpeedBoost")]
public class bulletSpeedBoost : PowerUpEffect
{
    private GameObject firePoint;
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
        if (firePoint.GetComponent<FirePointActive>().CanShoot)
        {
            GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);

            // Obtener el componente Bullet de la instancia y modificar bullet speed
            Bullet bulletScript = bulletInstance.GetComponent<Bullet>();

            if (bulletScript != null)
            {
                bulletScript.bulletSpeed = bulletSpeedBoosted; //powerup for player to speed up bullet
            }
        }
    }
}