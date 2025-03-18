using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //private GameObject bullet;
    public Transform firePoint;
    public GameObject bulletSprite;

    private bool isTripleShotActive = false;
    private bool isInfiniteBounceActive = false;
    private bool isBulletSpeedBoostActive = false;



    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (isInfiniteBounceActive)
        {
            Debug.Log("InfiniteBounceActive");
            InfiniteBounce();
        }
        else if (isTripleShotActive)
        {
            Debug.Log("TripleShotActive");
            TripleShotFire();
        }
        else if(isBulletSpeedBoostActive)
        {
            Debug.Log("BulletSpeedBoostActive");
            BulletSpeedBoost();
        }
        else
        {
            Instantiate(bulletSprite, firePoint.position, firePoint.rotation);
        }
    }

    void InfiniteBounce()
    {
        GameObject bulletInstance = Instantiate(bulletSprite, firePoint.position, firePoint.rotation);

        // Obtener el componente Bullet de la instancia y modificar bounceTime
        Bullet bulletScript = bulletInstance.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.bounceTime = 9999f; //powerup for player to bounce the bullet near infinitely in context of a round
        }
        
        //Instantiate(bulletSprite, firePoint.position, firePoint.rotation);

    } //power up of infinite bounce

    void TripleShotFire()
    {
        float spreadAngle = 15f;
        for (int i = 0; i < 3; i++)
        {
            Quaternion bulletRotation = firePoint.rotation * Quaternion.Euler(0f, 0f, spreadAngle * (i - 1));
            Instantiate(bulletSprite, firePoint.position, bulletRotation);
        }
    }   // power up of triple shot

    void BulletSpeedBoost()
    {
        GameObject bulletInstance = Instantiate(bulletSprite, firePoint.position, firePoint.rotation);

        // Obtener el componente Bullet de la instancia y modificar bullet speed
        Bullet bulletScript = bulletInstance.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.bulletSpeed = 50f; //powerup for player to speed up bullet
        }
    } // power up of bullet speed boost

    public void ActivateTripleShot(float duration)
    {
        isTripleShotActive = true;
        //StartCoroutine(DisableTripleShotAfterTime(duration));
    }
    public void ActivateinfiniteBounce(float duration)
    {
        isInfiniteBounceActive = true;
        //StartCoroutine(DisableTripleShotAfterTime(duration));
    }
    public void ActivateBulletSpeedBoost(float duration)
    {
        isBulletSpeedBoostActive = true;
        //StartCoroutine(DisableTripleShotAfterTime(duration));
    }

    //private IEnumerator DisableTripleShotAfterTime(float duration)
    //{
    //    yield return new WaitForSeconds(duration);
    //    isTripleShotActive = false;
    //}

}
