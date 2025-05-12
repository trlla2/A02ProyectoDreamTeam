using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Shield : MonoBehaviour
{
    private Weapon weapon;

    private void Awake()
    {
        weapon = GetComponentInParent<Weapon>();
    }

    /* private void OnCollisionEnter2D(Collision2D collision)
     {
         {
             if (collision.gameObject.CompareTag("Bullet") && weapon != null)
             {
                 Destroy(collision.gameObject);
                 weapon.ShieldHit();
             }
         }
     }
    */
}
