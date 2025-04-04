using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [Header("Setup")]
    public Transform firePoint;
    public Transform firePointL;
    public Transform firePointR;
    public GameObject bulletSprite;

    [Header("Events")]
    public UnityEvent OnShoot;
    public UnityEvent OnSetPowerUp;
    

    private PowerUpEffect currentPowerUp; // Referencia al Power-Up actual

    private bool isInfiniteBounceActive = false;
    private bool isBulletSpeedBoostActive = false;
    private bool isTripleShotActive = false;
    private float powerTime = 5f;

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
        if (isTripleShotActive && currentPowerUp is TripleShot tripleShotPowerUp)
        {
            tripleShotPowerUp.TripleShotFire();
        }
        else if (isBulletSpeedBoostActive && currentPowerUp is bulletSpeedBoost bulletSpeedBoost)
        {
            Debug.Log("BulletSpeedBoostActive");
            bulletSpeedBoost.BulletSpeedBoost();
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
        
        OnShoot.Invoke(); // invoke event
    }

    public void SetPowerUp(PowerUpEffect powerUp)
    {
        currentPowerUp = powerUp;

        if (currentPowerUp is InfiniteBounce)
        {
            Debug.Log("Active InfiniteBounce");
            isInfiniteBounceActive = true;
        }
        else if (currentPowerUp is TripleShot) 
        {
            Debug.Log("Active TripleShot");
            isTripleShotActive = true;
            

        }
        else if (currentPowerUp is bulletSpeedBoost)
        {
            Debug.Log("Active InfiniteBounce");
            isBulletSpeedBoostActive = true;
        }

        StartCoroutine(DisableAfterTime(powerTime));
        Debug.Log("Disabled PowerUp");

        OnSetPowerUp.Invoke();// invoke event
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
        else if (isTripleShotActive)
        {
            isTripleShotActive = false;
        }
    }
}
