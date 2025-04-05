using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/TripleShot")]
public class TripleShot : PowerUpEffect
{
    //[SerializeField] private float powerUpDuration = 25f;  // Duraci�n del efecto
    [SerializeField] private GameObject bulletPrefab;
    private Transform firePoint;
    private Transform firePointL;
    private Transform firePointR;
    //private bool isTripleShotActive = false;

    public override void Apply(GameObject target)
    {
        Weapon weapon = target.GetComponent<Weapon>();


        if (weapon != null)
        {
            firePoint = weapon.firePoint;  // Obtiene el punto de disparo del arma
            firePointL = weapon.firePointL;
            firePointR = weapon.firePointR;
            weapon.SetPowerUp(this);  // Asigna este Power-Up al arma
        }

        //StartCoroutine(ActivateTripleShot(powerUpDuration));
    }

    public void TripleShotFire()
    {
        var firePos = firePoint;

        //if (firePoint == null) return;
        Debug.Log("TripleShot");
        //float spreadAngle = 45f;
        for (int i = 0; i <= 2; i++)
        {
            if (i == 0)
            {
                firePos = firePointL;
            }
            else if (i == 1)
            {
                firePos = firePoint;
            }
            else if (i == 2)
            {
                firePos = firePointR;
            }

            Bullet b = Instantiate(bulletPrefab, firePos.position, firePos.rotation).GetComponent<Bullet>();
            b.SetVelocity();
            //Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
    //private IEnumerator ActivateTripleShot(float duration)
    //{
    //    isTripleShotActive = true;
    //    yield return new WaitForSeconds(duration);
        
    //    isTripleShotActive = false;
    //}

}