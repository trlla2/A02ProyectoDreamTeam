using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletSprite;

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
            shoot();
        }
        if (Input.GetButtonDown("Fire2") && tb.GetPlayer() == 2)
        {
            shoot();
        }
    }

    void shoot()
    {
        Instantiate(bulletSprite, firePoint.position, firePoint.rotation);
    }
}
