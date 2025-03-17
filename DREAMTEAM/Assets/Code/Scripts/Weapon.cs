using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletSprite;

    private bool isTripleShotActive = false;
    private float tripleShotDuration = 5f;  // Duración del Power-Up

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (isTripleShotActive)
        {
            Debug.Log("TripleShotActive");
            TripleShotFire();
        }
        else
        {
            Instantiate(bulletSprite, firePoint.position, firePoint.rotation);
        }
    }

    void TripleShotFire()
    {
        float spreadAngle = 15f;
        for (int i = 0; i < 3; i++)
        {
            Quaternion bulletRotation = firePoint.rotation * Quaternion.Euler(0f, 0f, spreadAngle * (i - 1));
            Instantiate(bulletSprite, firePoint.position, bulletRotation);
        }
    }

    public void ActivateTripleShot(float duration)
    {
        isTripleShotActive = true;
        StartCoroutine(DisableTripleShotAfterTime(duration));
    }

    private IEnumerator DisableTripleShotAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        isTripleShotActive = false;
    }
}
