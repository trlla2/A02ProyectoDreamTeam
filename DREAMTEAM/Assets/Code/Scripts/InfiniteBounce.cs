using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/InfiniteBounce")] //create a menu specifictly for powerups for nice organization

public class InfiniteBounce : PowerUpEffect
{
    //[SerializeField] private float powerUpDuration = 25f;  // Duración del efecto
    //public GameObject bulletSprite;
    //private Transform firePoint;
    //private bool isInfiniteBounceActive = false;
    public override void Apply(GameObject target)
    {
        Weapon weapon = target.GetComponent<Weapon>();

            weapon.SetPowerUp(this);
            //StartCoroutine(ActivateInfiniteBounce(powerUpDuration));

    }
   
    //private IEnumerator ActivateInfiniteBounce(float duration)
    //{
    //    isInfiniteBounceActive = true;
    //    yield return new WaitForSeconds(duration);

    //    isInfiniteBounceActive = false;
    //}

   
}
