using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/InfiniteBounce")] //create a menu specifictly for powerups for nice organization

public class InfiniteBounce : PowerUpEffect
{
    [SerializeField] private float powerUpDuration = 25f;  // Duración del efecto
    //public GameObject bulletSprite;
    //private Transform firePoint;
    private bool isInfiniteBounceActive = false;
    public override void Apply(GameObject target)
    {
        Weapon weapon = target.GetComponent<Weapon>();
        if (weapon != null)
        {
            weapon.SetPowerUp(this);
            weapon.StartCoroutine(ApplyInfiniteBounce(weapon, powerUpDuration));
        }
    }
    public void SetInfiniteBounce(bool state)
    {
        isInfiniteBounceActive = state;
    }
    private IEnumerator ApplyInfiniteBounce(Weapon weapon, float duration)
    {
        //weapon.SetInfiniteBounce(true);
        SetInfiniteBounce(true);
        yield return new WaitForSeconds(duration);
        //weapon.SetInfiniteBounce(false);
        SetInfiniteBounce(false);
    }

   
}
