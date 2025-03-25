using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletSprite;

    private PowerUpEffect currentPowerUp; // Referencia al Power-Up actual

    private bool isInfiniteBounceActive = false;
    private bool isBulletSpeedBoostActive = false;

    private Tank_Behaviour tb;

    private void Start()
    {
        tb = GetComponent<Tank_Behaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && tb.GetPlayer() == 1)
        {
            Shoot();
        }
        if (Input.GetButtonDown("Fire2") && tb.GetPlayer() == 2)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (currentPowerUp is TripleShot tripleShotPowerUp)
        {
            tripleShotPowerUp.TripleShotFire();
        }
        else if (isBulletSpeedBoostActive)
        {
            Debug.Log("BulletSpeedBoostActive");
            BulletSpeedBoost();
        }
        else
        {

            GameObject bulletInstance = Instantiate(bulletSprite, firePoint.position, firePoint.rotation);
            Bullet bulletScript = bulletInstance.GetComponent<Bullet>();

            if (isInfiniteBounceActive)
            {
                bulletScript.bounceTime = 9999f;  
            }
            else
            {
                bulletScript.bounceTime = 3f;
            }
        }
        
    }

    void BulletSpeedBoost()
    {
        GameObject bulletInstance = Instantiate(bulletSprite, firePoint.position, firePoint.rotation);

        // Obtener el componente Bullet de la instancia y modificar bullet speed
        Bullet bulletScript = bulletInstance.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.bulletSpeed = 50f; //powerup for player to speed up bullet
        }
    }

    public void SetPowerUp(PowerUpEffect powerUp)
    {
        currentPowerUp = powerUp;

        if (currentPowerUp is InfiniteBounce)
        {
            isInfiniteBounceActive = true;
            StartCoroutine(DisableAfterTime(25f)); 
        }
    }

    public void ActivateBulletSpeedBoost(float duration)
    {
        isBulletSpeedBoostActive = true;
        //StartCoroutine(DisableTripleShotAfterTime(duration));
    }
    private IEnumerator DisableAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (isBulletSpeedBoostActive)
        {
            isBulletSpeedBoostActive = false;
        }
        else if (isInfiniteBounceActive)
        {
            isInfiniteBounceActive = false;
        }
    }
}
