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

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Verificar si hay alg�n power-up activo y aplicarlo
        if (currentPowerUp is TripleShot tripleShotPowerUp)
        {
            tripleShotPowerUp.TripleShotFire();  // Llama a Fire() de TripleShot
        }
        else
        {

            GameObject bulletInstance = Instantiate(bulletSprite, firePoint.position, firePoint.rotation);
            Bullet bulletScript = bulletInstance.GetComponent<Bullet>();

            if (isInfiniteBounceActive)
            {
                bulletScript.bounceTime = 9999f;  // Aplica rebote infinito a la bala
            }
            else
            {
                bulletScript.bounceTime = 3f; // Valor predeterminado para rebote normal
            }
        }
        
    }

    public void SetPowerUp(PowerUpEffect powerUp)
    {
        currentPowerUp = powerUp;

        // Verificar si el power-up es de rebote infinito y activar el estado correspondiente
        if (currentPowerUp is InfiniteBounce)
        {
            isInfiniteBounceActive = true; // Activar rebote infinito
            StartCoroutine(DisableInfiniteBounceAfterTime(25f)); // Establecer el tiempo de duraci�n del power-up
        }
    }
    private IEnumerator DisableInfiniteBounceAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        isInfiniteBounceActive = false; // Desactivar rebote infinito despu�s del tiempo
    }
}
