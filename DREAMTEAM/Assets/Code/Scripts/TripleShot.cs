using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/TripleShot")]
public class TripleShot : PowerUpEffect
{
    [SerializeField] private float powerUpDuration = 25f;  // Duración del efecto
    [SerializeField] private GameObject bulletPrefab;
    private Transform firePoint;
    private bool isTripleShotActive = false;



    public override void Apply(GameObject target)
    {
        Weapon weapon = target.GetComponent<Weapon>();

        if (weapon != null)
        {
            firePoint = weapon.firePoint;  // Obtiene el punto de disparo del arma
            weapon.SetPowerUp(this);  // Asigna este Power-Up al arma
        }

        weapon.StartCoroutine(ActivateTripleShot(powerUpDuration));
    }

    public void TripleShotFire()
    {
        //if (firePoint == null) return;
        Debug.Log("TripleShot");
        float spreadAngle = 15f;
        for (int i = 0; i < 3; i++)
        {
            Quaternion bulletRotation = firePoint.rotation * Quaternion.Euler(0f, 0f, spreadAngle * (i - 1));
            Instantiate(bulletPrefab, firePoint.position, bulletRotation);
            //Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
    public void SetTripleShot(bool state)
    {
        isTripleShotActive = state;
    }
    private IEnumerator ActivateTripleShot(float duration)
    {
        //weapon.SetTripleShot(true);
        SetTripleShot(true);
        yield return new WaitForSeconds(duration);
        //weapon.SetTripleShot(false);
        SetTripleShot(false);
    }

    

    //public void ActivateTripleShot(float duration)
    //{
    //    isTripleShotActive = true;
    //    //StartCoroutine(DisableTripleShotAfterTime(duration));
    //}

}