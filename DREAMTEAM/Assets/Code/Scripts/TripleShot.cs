using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/TripleShot")]
public class TripleShot : PowerUpEffect
{
    public Transform firePoint;
    public GameObject bulletSprite;

    [SerializeField] float spreadAngle = 15f;
    public override void Apply(GameObject target)
    {
        target.GetComponent<Bullet>();
        for (int i = 0; i < 3; i++)
        {
            target = Instantiate(bulletSprite, firePoint.position, firePoint.rotation);
            target.transform.SetParent(firePoint);
            target.transform.Rotate(0f, spreadAngle * (i - 1), 0f);

        }
    }
}