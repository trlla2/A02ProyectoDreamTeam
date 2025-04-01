using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/BulletSpeedBoost")]
public class bulletSpeedBoost : PowerUpEffect
{
    //[SerializeField] private float powerUpDuration = 25f;  // Duración del efecto
    //private bool isBulletSpeedBoostActive = false;
    private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    public override void Apply(GameObject target) //apply power up
    {
        Weapon weapon = target.GetComponent<Weapon>();

        firePoint = weapon.firePoint;

        weapon.SetPowerUp(this);
        //StartCoroutine(ActivateBulletSpeedBoost(powerUpDuration));
    }

    public void BulletSpeedBoost()
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Obtener el componente Bullet de la instancia y modificar bullet speed
        Bullet bulletScript = bulletInstance.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.bulletSpeed = 50f; //powerup for player to speed up bullet
        }
    }

    //private IEnumerator ActivateBulletSpeedBoost(float duration)
    //{
    //    isBulletSpeedBoostActive = true;
    //    yield return new WaitForSeconds(duration);
    //    isBulletSpeedBoostActive = false;
    //}
}